using Microsoft.EntityFrameworkCore;
using WMOffice.Models;
using System.IO;
using System;

namespace WMOffice.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public string DbPath { get; }

        public AppDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "wmoffice.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasDefaultValue("Active");

            modelBuilder.Entity<Inspection>()
                .HasOne(i => i.Project)
                .WithMany()
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Photo>()
                .HasOne(p => p.Inspection)
                .WithMany(i => i.Photos)
                .HasForeignKey(p => p.InspectionId)
                .OnDelete(DeleteBehavior.Cascade);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}
