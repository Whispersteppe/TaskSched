using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskSched.DataStore.DataModel;

namespace TaskSched.DataStore.Configuration
{
    internal class EventActivityFieldConfig : IEntityTypeConfiguration<EventActivityField>
    {
        public void Configure(EntityTypeBuilder<EventActivityField> builder)
        {
            builder.ToTable("EventActivityField");
            builder.HasKey(t => t.Id);


        }
    }
}
