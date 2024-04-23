using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.DataStore.DataModel
{
    public class Folder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? DefaultSchedule { get; set; }

        public Guid? ParentFolderId { get; set; }
        public Folder ParentFolder { get; set; }
        public List<Event> Events { get; set; }
        public List<Folder> ChildFolders { get; set; }

    }


}
