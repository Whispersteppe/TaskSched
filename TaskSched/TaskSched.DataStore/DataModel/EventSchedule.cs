namespace TaskSched.DataStore.DataModel
{
    public class EventSchedule
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CRONData { get; set; }

        public Guid EventId { get; set; }

        public Event EventItem { get; set; }

    }
}
