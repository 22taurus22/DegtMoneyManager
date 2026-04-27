using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DegtMoney.Models;
using System.Data.Entity;


namespace DegtMoney.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=MoneyManagerDb") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<SavingGoal> SavingGoals { get; set; }
        public DbSet<BudgetLimit> BudgetLimits { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>();
               modelBuilder.Entity<Category>()
                .HasRequired(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Transaction>()
                .HasRequired(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Transaction>()
                .HasRequired(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SavingGoal>()
                .HasRequired(s => s.User)
                .WithMany(u => u.SavingGoals)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BudgetLimit>()
                .HasRequired(b => b.User)
                .WithMany(u => u.BudgetLimits)
                .HasForeignKey(b => b.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BudgetLimit>()
                .HasRequired(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasRequired(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
