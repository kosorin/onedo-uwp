using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OneDo.Model.Data;

namespace OneDo.Model.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("OneDo.Model.Data.Entities.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color")
                        .IsRequired();

                    b.Property<int?>("Left");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentId");

                    b.Property<int?>("Right");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("OneDo.Model.Data.Entities.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Completed");

                    b.Property<DateTime?>("Date");

                    b.Property<bool>("Flag");

                    b.Property<int?>("FolderId");

                    b.Property<string>("Note");

                    b.Property<TimeSpan?>("Reminder");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("OneDo.Model.Data.Entities.Folder", b =>
                {
                    b.HasOne("OneDo.Model.Data.Entities.Folder", "Parent")
                        .WithMany("Subfolders")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("OneDo.Model.Data.Entities.Todo", b =>
                {
                    b.HasOne("OneDo.Model.Data.Entities.Folder", "Folder")
                        .WithMany("Todos")
                        .HasForeignKey("FolderId");
                });
        }
    }
}
