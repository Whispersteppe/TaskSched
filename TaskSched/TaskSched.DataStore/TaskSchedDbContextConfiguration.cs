namespace TaskSched.DataStore
{
    public class TaskSchedDbContextConfiguration
    {
        public string DataSource { get; set; }

        public override string ToString()
        {
            return "Database Configuration";
        }
    }
}
