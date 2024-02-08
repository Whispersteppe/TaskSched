using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TaskSched.DataStore.DataModel;

namespace TaskSched.DataStore.Configuration
{
    internal class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Event");
            builder.HasKey(t => t.Id);

            builder
                .HasMany(x => x.Activities)
                .WithOne(x => x.EventItem)
                .HasForeignKey(x => x.EventId);

            builder
                .HasMany(x => x.Schedules)
                .WithOne(x => x.EventItem)
                .HasForeignKey(x => x.EventId);
        }
    }
}
