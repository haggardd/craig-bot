using System;
using System.IO;
using CraigBot.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CraigBot.Infrastructure.Database
{
    public class CraigBotDbContext : DbContext
    {
        public DbSet<Fortune> Fortunes { get; set; }
        
        public DbSet<AskResponse> AskResponses { get; set; }
        
        public DbSet<BankAccount> BankAccounts { get; set; }
        
        public DbSet<Bet> Bets { get; set; }
        
        public DbSet<Wager> Wagers { get; set; }
        
        public DbSet<Stock> Stocks { get; set; }
        
        public DbSet<Investment> Investments { get; set; }
        
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
            modelBuilder.Entity<Fortune>().ToTable("Fortune");
            modelBuilder.Entity<Fortune>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Text).IsRequired();
            });
            
            // Ask Response
            modelBuilder.Entity<AskResponse>().ToTable("AskResponse");
            modelBuilder.Entity<AskResponse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Text).IsRequired();
            });
            
            // Bank Account
            modelBuilder.Entity<BankAccount>().ToTable("BankAccount");
            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Username).IsRequired();
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
                entity.Property(e => e.HasEnded).IsRequired().HasDefaultValue(false);
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
            
            // Stock
            modelBuilder.Entity<Stock>().ToTable("Stock");
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Ticker).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.High).IsRequired();
                entity.Property(e => e.Low).IsRequired();
                entity.Property(e => e.PreviousPrice).IsRequired();
                entity.Property(e => e.LastUpdate).IsRequired();
            });
            
            // Investment
            modelBuilder.Entity<Investment>().ToTable("Investment");
            modelBuilder.Entity<Investment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.StockId).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.BuyPrice).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}