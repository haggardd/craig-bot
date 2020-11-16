using System;
using System.IO;
using CraigBot.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CraigBot.Infrastructure.Database
{
    public class CraigBotDbContext : DbContext
    {
        public DbSet<FortuneCookie> FortuneCookies { get; set; }
        public DbSet<EightBallResponse> EightBallResponses { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databasePath = Path.Combine(AppContext.BaseDirectory, "Data");

            if (!Directory.Exists(databasePath))
            {
                Directory.CreateDirectory(databasePath);
            }

            var databaseDirectory = Path.Combine(databasePath, "CraigDb.db");
            optionsBuilder.UseSqlite($"Filename={databaseDirectory}");
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FortuneCookie>().ToTable("FortuneCookies", "test");
            modelBuilder.Entity<FortuneCookie>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fortune).IsRequired();
            });
            
            modelBuilder.Entity<EightBallResponse>().ToTable("EightBallResponses", "test");
            modelBuilder.Entity<EightBallResponse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Response).IsRequired();
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}