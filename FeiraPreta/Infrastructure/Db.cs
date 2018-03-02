using FeiraPreta.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Infrastructure
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base (options)
        {
        }

        public DbSet<EventScore> EventScore { get; set; }

        public DbSet<Person> Person { get; set; }

        public DbSet<Publication> Publication { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventScore>().ToTable("EventScore");
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Publication>().ToTable("Publication");

            modelBuilder.Entity<Person>().HasMany(p => p.Publications).WithOne(p => p.Person).HasForeignKey(p => p.PersonId);
            modelBuilder.Entity<Person>().Property(p => p.CreatedDate).ValueGeneratedOnAdd();
            modelBuilder.Entity<Person>().Property(p => p.UpdatedDate).ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Publication>().Property(p => p.CreatedDate).ValueGeneratedOnAdd();
            modelBuilder.Entity<Publication>().Property(p => p.UpdatedDate).ValueGeneratedOnAddOrUpdate();

            base.OnModelCreating(modelBuilder);
        }
    }
}
