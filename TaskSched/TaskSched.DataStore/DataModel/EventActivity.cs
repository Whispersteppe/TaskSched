namespace TaskSched.DataStore.DataModel
{
    public class EventActivity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<EventActivityField> Fields { get; set; }

        public Guid EventId { get; set; }
        public Guid ActivityId { get; set; }
        public Event EventItem { get; set; }

        public Activity ActivityItem { get; set; }
    }
}
