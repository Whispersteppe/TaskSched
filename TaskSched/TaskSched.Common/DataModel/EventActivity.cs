namespace TaskSched.Common.DataModel
{
    public class EventActivity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid ActivityId { get; set; }

        public List<EventActivityField>? Fields { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
