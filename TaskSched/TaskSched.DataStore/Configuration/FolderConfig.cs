using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskSched.DataStore.DataModel;

namespace TaskSched.DataStore.Configuration
{
    internal class FolderConfig : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.ToTable("Folder");
            builder.HasKey(t => t.Id);

            builder
                .HasMany(x => x.Events)
                .WithOne(x => x.Folder)
                .HasForeignKey(x => x.FolderId);

            builder
                .HasMany(x => x.ChildFolders)
                .WithOne(x => x.ParentFolder)
                .HasForeignKey(x => x.ParentFolderId);
        }
    }
}
