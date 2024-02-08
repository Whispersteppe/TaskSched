namespace TaskSched.Common.DataModel
{
    public class ActivityField
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsReadOnly { get; set; } //  read only makes it fixed for the activity, such as an executable path.


    }
}
