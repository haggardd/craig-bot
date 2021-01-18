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
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Wager> Wagers { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databasePath = Path.Combine(AppContext.BaseDirectory, "Database");

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
            
            // Bank Account
            modelBuilder.Entity<BankAccount>().ToTable("BankAccount");
            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                // TODO: Might be able to use a default value setting for this, worth looking into
                entity.Property(e => e.Balance).IsRequired();
            });
            
            // Bet
            modelBuilder.Entity<Bet>().ToTable("Bet");
            modelBuilder.Entity<Bet>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.ForOdds).IsRequired();
                entity.Property(e => e.AgainstOdds).IsRequired();
                // TODO: This might need a default value setting
                entity.Property(e => e.HasEnded).IsRequired();
            });
            
            // Wager
            modelBuilder.Entity<Wager>().ToTable("Wager");
            modelBuilder.Entity<Wager>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.BetId).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Stake).IsRequired();
                entity.Property(e => e.InFavour).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}