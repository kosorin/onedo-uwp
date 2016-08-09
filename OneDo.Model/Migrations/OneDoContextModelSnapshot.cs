using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OneDo.Model.Data;

namespace OneDo.Model.Migrations
{
    [DbContext(typeof(OneDoContext))]
    partial class OneDoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("OneDo.Model.Data.Objects.Folder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("OneDo.Model.Data.Objects.Todo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Completed");

                    b.Property<DateTime?>("Date");

                    b.Property<DateTime?>("Deleted");

                    b.Property<bool>("Flag");

                    b.Property<Guid?>("FolderId");

                    b.Property<string>("Note");

                    b.Property<Guid?>("ParentId");

                    b.Property<TimeSpan?>("Reminder");

                    b.Property<string>("Title");

                    b.Property<DateTime?>("Updated");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.HasIndex("ParentId");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("OneDo.Model.Data.Objects.Todo", b =>
                {
                    b.HasOne("OneDo.Model.Data.Objects.Folder", "Folder")
                        .WithMany("Todos")
                        .HasForeignKey("FolderId");

                    b.HasOne("OneDo.Model.Data.Objects.Todo", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });
        }
    }
}
