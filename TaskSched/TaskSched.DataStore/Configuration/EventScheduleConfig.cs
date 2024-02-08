using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskSched.DataStore.DataModel;

namespace TaskSched.DataStore.Configuration
{
    internal class EventScheduleConfig : IEntityTypeConfiguration<EventSchedule>
    {
        public void Configure(EntityTypeBuilder<EventSchedule> builder)
        {
            builder.ToTable("EventSchedule");
            builder.HasKey(t => t.Id);


        }
    }
}
