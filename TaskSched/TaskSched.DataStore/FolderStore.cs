using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model=TaskSched.Common.DataModel;
using Db = TaskSched.DataStore.DataModel;
using TaskSched.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskSched.Common.DataModel;
using TaskSched.DataStore.DataModel;

namespace TaskSched.DataStore
{
    public class FolderStore : IFolderStore
    {
        TaskSchedDbContextFactory _contextFactory;
        IDataStoreMapper _mapper;
        ILogger _logger;

        public FolderStore(TaskSchedDbContextFactory contextFactory, IDataStoreMapper mapper, ILogger<FolderStore> logger) 
        { 
            _contextFactory = contextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Model.ExpandedResult<Guid>> Create(Model.Folder folder)
        {
            Db.Folder item = _mapper.Map<Db.Folder>(folder);

            item.Id = Guid.Empty;

            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {
                _dbContext.Folders.Add(item);

                await _dbContext.SaveChangesAsync();

                Model.ExpandedResult<Guid> rslt = new Model.ExpandedResult<Guid>()
                {
                    Result = item.Id
                };

                rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Folder created" });

                _logger.LogInformation($"Created {folder.Name}, ID={item.Id}");
                return rslt;
            }
        }



        public async Task<Model.ExpandedResult> Delete(Guid folderId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entity = await _dbContext.Folders.FirstOrDefaultAsync(x => x.Id == folderId);

                Model.ExpandedResult rslt = new Model.ExpandedResult();


                if (entity != null)
                {
                    _dbContext.Folders.Remove(entity);

                    await _dbContext.SaveChangesAsync();
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Folder deleted" });
                    _logger.LogInformation($"Deleted {entity.Name}, ID={entity.Id}");

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Warning, Message = "Folder not found.  no deletion occurred" });
                    _logger.LogWarning($"Folder not found.  no deletion occurred. ID={folderId}");
                }

                return rslt;
            }
        }

        public async Task<Model.ExpandedResult<Model.Folder?>> Get(Guid folderId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var entity = await _dbContext
                .Folders
                .FirstOrDefaultAsync(x => x.Id == folderId)
                ;

                Model.ExpandedResult<Model.Folder?> rslt = new Model.ExpandedResult<Model.Folder?>();

                if (entity != null)
                {

                    rslt.Result = _mapper.Map<Model.Folder>(entity);  
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Folder retrieved" });

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Warning, Message = "Folder not found." });
                    _logger.LogWarning($"Folder not found.  ID={folderId}");
                }

                return rslt;
            }
        }


        public async Task<Model.ExpandedResult<List<Model.Folder>>> GetAll(FolderRetrievalParameters parameters)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var query = _dbContext
                .Folders.AsQueryable()
                ;

                //  this seems to get all the folders already built into a tree.  cool
                var folders = await query.ToListAsync();


                if (parameters.AddChildEvents == true)
                {
                    //  add the unassigned folder
                    folders.Add(new Db.Folder()
                    {
                        Id = Guid.Empty,
                        Name = "Unassigned Events",
                        ParentFolderId = null,
                        Events = new List<Db.Event>(),
                    });

                    //  this drops all the events into the folders directly.  also cool.
                    var events = await _dbContext
                        .Events
                        .Include(x => x.Schedules)
                        .Include(x => x.Activities).ThenInclude(x=>x.Fields)
                        .ToListAsync();
                    ;


                    foreach (var eventItem in events)
                    {
                        Db.Folder? associatedFolder;
                        if (eventItem.FolderId != null)
                        {
                            associatedFolder = folders.FirstOrDefault(x => x.Id == eventItem.FolderId);
                            if (associatedFolder == null)
                            {
                                associatedFolder = folders.FirstOrDefault(x => x.Id == Guid.Empty);
                            }
                        }
                        else
                        {
                            associatedFolder = folders.FirstOrDefault(x => x.Id == Guid.Empty);
                        }

                        if (associatedFolder != null)
                        {
                            //  lets not add them again.  it gets clumsy
                            if (associatedFolder.Events.Any(x=>x.Id == eventItem.Id) == false)
                            {
                                associatedFolder.Events.Add(eventItem);
                            }
                        }
                    }
                }

                if (parameters.AsTree == true)
                {
                    //  build out the tree

                    var folderTree = new List<Db.Folder>();

                    foreach (var folder in folders)
                    {
                        if (folder.ParentFolderId == null)
                        {
                            folderTree.Add(folder);
                        }
                        else
                        {
                            var parentFolder = folders.FirstOrDefault(x => x.Id == folder.ParentFolderId);
                            if (parentFolder != null)
                            {
                                if (parentFolder.ChildFolders == null)
                                {
                                    parentFolder.ChildFolders = new List<Db.Folder>();
                                }
                                //  again, don't duplicate things
                                if (parentFolder.ChildFolders.Any(x => x.Id == folder.Id) == false)
                                {
                                    parentFolder.ChildFolders.Add(folder);
                                }
                            }
                        }
                    }

                    folders = folderTree;
                }


                Model.ExpandedResult<List<Model.Folder>> rslt = new Model.ExpandedResult<List<Model.Folder>>();

                if (folders != null)
                {

                    rslt.Result = _mapper.Map<List<Model.Folder>>(folders);
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Folders retrieved" });

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Warning, Message = "Folders not found." });
                    _logger.LogWarning($"Folders not found.");
                }

                return rslt;
            }
        }

        public async Task<Model.ExpandedResult> Update(Model.Folder folder)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var dbEntity = await _dbContext
                .Folders
                .FirstOrDefaultAsync(x => x.Id == folder.Id)
                ;

                Model.ExpandedResult rslt = new Model.ExpandedResult();


                if (dbEntity != null)
                {

                    //  we're not doing a mapping since we've only got one field, and i don't want to screw up child events and folders
                    //_dbContext.Entry(dbEntity).CurrentValues.SetValues(folder); 
                    dbEntity.Name = folder.Name;
                    dbEntity.DefaultSchedule = folder.DefaultSchedule;

                    _dbContext.Update(dbEntity);

                    await _dbContext.SaveChangesAsync();

                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.OK, Message = "Folder updated" });
                    _logger.LogInformation($"Folder {folder.Name} updated ID={folder.Id}");

                }
                else
                {
                    rslt.Messages.Add(new Model.ResultMessage() { Severity = Model.ResultMessageSeverity.Error, Message = "Folder not found.  no update occurred" });
                    _logger.LogWarning($"Folder not found.  no update occurred. ID={folder.Id}");
                }

                return rslt;
            }

        }


        public async Task<ExpandedResult> MoveFolder(Guid folderId, Guid? newParentFolderId)
        {
            using (TaskSchedDbContext _dbContext = _contextFactory.GetConnection())
            {

                var folder = _dbContext.Folders.FirstOrDefault(x => x.Id == folderId);

                if (folder != null)
                {
                    folder.ParentFolderId = newParentFolderId;
                    _dbContext.Folders.Update(folder);
                    await _dbContext.SaveChangesAsync();

                    _logger.LogInformation($"Folder {folderId} moved to {newParentFolderId}");
                    return new ExpandedResult() { Messages = new List<Model.ResultMessage>() { new ResultMessage() { Message = "Folder moved", Severity = ResultMessageSeverity.OK } } };

                }
                else
                {
                    _logger.LogWarning($"Folder not found.  no move occurred. ID={folderId}");
                    return new ExpandedResult() { Messages = new List<Model.ResultMessage>() { new ResultMessage() { Message = "Folder was not found", Severity = ResultMessageSeverity.Error } } };
                }
            }

        }





    }
}
