using TaskSched.Common.DataModel;

namespace TaskScheduler.WinForm.Models
{
    public class EventModel : ITreeItem
    {
        Event _eventItem;

        public EventModel(Event eventItem) 
        {
            _eventItem = eventItem;
        }
        public string Name
        {
            get
            {
                return _eventItem.Name;
            }
        }

        public DateTime LastExecutionDate
        { 
            get
            {
                return _eventItem.LastExecution;
            }
        }

        public DateTime NextExecutionDate
        {
            get
            {
                return _eventItem.NextExecution;
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
