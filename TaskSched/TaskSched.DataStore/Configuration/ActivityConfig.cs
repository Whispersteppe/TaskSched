using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Activity = TaskSched.DataStore.DataModel.Activity;

namespace TaskSched.DataStore.Configuration
{
    internal class ActivityConfig : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToTable("Activity");
            builder.HasKey(t => t.Id);

            builder
                .HasMany(x => x.DefaultFields)
                .WithOne(x => x.ActivityItem)
                .HasForeignKey(x => x.ActivityId);

            builder
                .HasMany(x => x.TaskActions)
                .WithOne(x => x.ActivityItem)
                .HasForeignKey(x=>x.ActivityId);

        }
    }
}
