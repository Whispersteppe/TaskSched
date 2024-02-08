using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskSched.DataStore.DataModel;

namespace TaskSched.DataStore.Configuration
{
    internal class EventActivityConfig : IEntityTypeConfiguration<EventActivity>
    {
        public void Configure(EntityTypeBuilder<EventActivity> builder)
        {
            builder.ToTable("EventActivity");
            builder.HasKey(t => t.Id);

            builder
                .HasMany(t => t.Fields)
                .WithOne(x => x.EventActivityItem)
                .HasForeignKey(x => x.EventActivityId);

        }
    }
}
