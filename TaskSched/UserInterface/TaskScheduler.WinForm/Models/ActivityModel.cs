using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class ActivityModel : ITreeItem
    {
        Activity _activity;

        public ActivityModel(Activity activity)
        {
            _activity = activity;
        }
        public string Name
        {
            get
            {
                return _activity.Name;
            }
        }
        public List<ITreeItem>? Children
        {
            get
            {
                return null;
            }

        }

        public void Revert()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }

    //public class ProcessModel : ITreeItem
    //{
    //    Proce

    //}
}
