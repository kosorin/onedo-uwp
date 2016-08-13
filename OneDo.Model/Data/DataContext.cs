using Microsoft.EntityFrameworkCore;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Model.Data
{
    public class DataContext : DbContext
    {
        private const string FileName = "OneDo.Data.db";


        public DbSet<Todo> Todos { get; set; }

        public DbSet<Folder> Folders { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={FileName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var todo = modelBuilder.Entity<Todo>();
            todo.HasOne(x => x.Folder).WithMany(x => x.Todos);

            var folder = modelBuilder.Entity<Folder>();
            folder.Property(x => x.Name).IsRequired();
            folder.Property(x => x.Color).IsRequired().HasMaxLength(7);
            folder.HasMany(x => x.Todos).WithOne(x => x.Folder);
        }
    }
}
