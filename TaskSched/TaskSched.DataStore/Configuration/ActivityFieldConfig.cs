using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskSched.DataStore.DataModel;

namespace TaskSched.DataStore.Configuration
{
    internal class ActivityFieldConfig : IEntityTypeConfiguration<ActivityField>
    {
        public void Configure(EntityTypeBuilder<ActivityField> builder)
        {
            builder.ToTable("ActivityField");
            builder.HasKey(t => t.Id);

            builder
                .HasMany(x => x.EventActivityFields)
                .WithOne(x => x.ActivityFieldItem)
                .HasForeignKey(x => x.ActivityFieldId);

        }
    }
}
