using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskSched.DataStore.DataModel;

namespace TaskSched.DataStore.Configuration
{
    internal class CalendarConfig : IEntityTypeConfiguration<Calendar>
    {
        public void Configure(EntityTypeBuilder<Calendar> builder)
        {
            builder.ToTable("Calendar");
            builder.HasKey(t => t.Id);

            builder
                .HasMany(x => x.Events)
                .WithOne(x => x.Calendar)
                .HasForeignKey(x => x.CalendarId);

            builder
                .HasMany(x => x.ChildCalendars)
                .WithOne(x => x.ParentCalendar)
                .HasForeignKey(x => x.ParentCalendarId);
        }
    }
}
