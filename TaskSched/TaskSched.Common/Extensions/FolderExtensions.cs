using TaskSched.Common.DataModel;

namespace TaskSched.Common.Extensions
{
    public static class FolderExtensions
    {
        public static void SetForCreate(this Folder folder)
        {
            folder.Id = Guid.NewGuid();
            folder.ParentFolderId = null;
        }
    }




}
