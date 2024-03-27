using System.Dynamic;
using System.Globalization;
using TaskSched.Common.DataModel;
using Folder = TaskSched.Common.DataModel.Folder;

namespace TaskSched.Common.Interfaces
{
    /// <summary>
    /// repository interface for folders
    /// </summary>
    public interface IFolderStore
    {
        /// <summary>
        /// Get a single folder
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        Task<ExpandedResult<Folder?>> Get(Guid folderId);

        /// <summary>
        /// Get all folders using the parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ExpandedResult<List<Folder>>> GetAll(FolderRetrievalParameters parameters);

        /// <summary>
        /// update a single folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        Task<ExpandedResult> Update(Folder folder);

        /// <summary>
        /// remove a folder
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        Task<ExpandedResult> Delete(Guid folderId);

        /// <summary>
        /// create a folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        Task<ExpandedResult<Guid>> Create(Folder folder);

        /// <summary>
        /// move a folder to a new parent
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="newParentFolderId"></param>
        /// <returns></returns>
        Task<ExpandedResult> MoveFolder(Guid folderId, Guid? newParentFolderId);

    }
}
