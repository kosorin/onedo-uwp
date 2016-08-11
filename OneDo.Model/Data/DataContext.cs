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
            var folder = modelBuilder.Entity<Folder>();

            todo.HasOne(x => x.Parent);
            todo.HasOne(x => x.Folder).WithMany(x => x.Todos);
            folder.HasMany(x => x.Todos).WithOne(x => x.Folder);
        }
    }
}
