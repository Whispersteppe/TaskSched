namespace TaskSched.DataStore.DataModel
{
    public class EventActivityField
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Guid EventActivityId { get; set; }
        public Guid  ActivityFieldId { get; set; }

        public EventActivity EventActivityItem { get; set; }
        public ActivityField ActivityFieldItem { get; set; }
    }
}
