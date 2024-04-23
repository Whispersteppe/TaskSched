using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Common.DataModel
{
    public class Folder
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "New Folder";
        public string? DefaultSchedule { get; set; }

        public Guid? ParentFolderId { get; set; }
        public List<Event>? Events { get; set; } = new List<Event>();
        public List<Folder>? ChildFolders { get; set; } = new List<Folder>();

    }


}
