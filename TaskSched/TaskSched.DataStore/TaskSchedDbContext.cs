using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TaskSched.DataStore.DataModel;

namespace TaskSched.DataStore
{
    public class TaskSchedDbContextFactory
    {

        TaskSchedDbContextConfiguration _configuration;
        public TaskSchedDbContextFactory(TaskSchedDbContextConfiguration configuration) 
        { 
            _configuration = configuration;
        }

        public TaskSchedDbContext GetConnection()
        {
            TaskSchedDbContext dbContext = new TaskSchedDbContext(_configuration);

            return dbContext;
        }
    }

    public class TaskSchedDbContext : DbContext
    {

        public DbSet<Event> Events { get; set; }
        public DbSet<EventActivity> EventActivities { get; set; }
        public DbSet<EventSchedule> EventSchedules { get; set; }
        public DbSet<EventActivityField> EventActivityFields { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityField> ActivityFields { get; set; }
        public DbSet<Calendar> Calendars { get; set; }

        TaskSchedDbContextConfiguration _config;

        internal TaskSchedDbContext(TaskSchedDbContextConfiguration configuration) 
        {
            _config = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (optionsBuilder.IsConfigured == false)
            {

                SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();
                builder.DataSource = _config.DataSource;

                optionsBuilder.UseSqlite(builder.ConnectionString);

            }


            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public async Task EnsureCreated()
        {
            await Database.EnsureCreatedAsync();
        }
    }
}
