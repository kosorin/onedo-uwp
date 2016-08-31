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


        public DbSet<Note> Notes { get; set; }

        public DbSet<Folder> Folders { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={FileName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var note = modelBuilder.Entity<Note>();

            var folder = modelBuilder.Entity<Folder>();
            folder.Property(x => x.Name).IsRequired();
            folder.Property(x => x.Color).IsRequired();
        }

        public async Task Clear()
        {
            Notes.RemoveRange(await Notes.ToListAsync());
            Folders.RemoveRange(await Folders.ToListAsync());
        }
    }
}
