using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace QASite.Data
{
    public class QuestionsDataContext : DbContext
    {
         private readonly string _connectionString;

        public QuestionsDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionsTags> QuestionsTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //by default Entity Framework sets all foreign key relationship delete rules
            //to be Cascade delete. This code changes it to be Restrict which is more in line
            //of what we're used to in that it will fail deleting a parent, if there are still
            //any children
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //set up composite primary key
            modelBuilder.Entity<QuestionsTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

        }
    }
}
