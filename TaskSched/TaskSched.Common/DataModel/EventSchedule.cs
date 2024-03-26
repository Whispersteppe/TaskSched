namespace TaskSched.Common.DataModel
{
    public class EventSchedule
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CRONData { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
