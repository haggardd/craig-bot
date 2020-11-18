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
        public DbSet<Bank> Banks { get; set; }
        
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
            // Fortune Cookie
            modelBuilder.Entity<FortuneCookie>().ToTable("FortuneCookie");
            modelBuilder.Entity<FortuneCookie>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fortune).IsRequired();
            });
            
            // Eight-Ball Response
            modelBuilder.Entity<EightBallResponse>().ToTable("EightBallResponse");
            modelBuilder.Entity<EightBallResponse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Response).IsRequired();
            });
            
            // Bank
            modelBuilder.Entity<Bank>().ToTable("Bank");
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Balance).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}