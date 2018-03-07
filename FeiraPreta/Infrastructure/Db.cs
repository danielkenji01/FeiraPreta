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

        public DbSet<Tag> Tag { get; set; }

        public DbSet<Publication_Tag> Publication_Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventScore>().ToTable("EventScore");
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Publication>().ToTable("Publication");
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<Publication_Tag>().ToTable("Publication_Tag");

            modelBuilder.Entity<Person>().HasMany(p => p.Publications).WithOne(p => p.Person).HasForeignKey(p => p.PersonId);
            modelBuilder.Entity<Person>().Property(p => p.CreatedDate).ValueGeneratedOnAdd();
            modelBuilder.Entity<Person>().Property(p => p.UpdatedDate).ValueGeneratedOnAddOrUpdate();
            
            modelBuilder.Entity<Publication>().Property(p => p.CreatedDate).ValueGeneratedOnAdd();
            modelBuilder.Entity<Publication>().Property(p => p.UpdatedDate).ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Publication_Tag>().HasKey(p => new { p.PublicationId, p.TagId }).ForSqlServerIsClustered(true);
            modelBuilder.Entity<Publication_Tag>().HasOne(p => p.Publication).WithMany(p => p.Publication_Tags).HasForeignKey(p => p.PublicationId);
            modelBuilder.Entity<Publication_Tag>().HasOne(p => p.Tag).WithMany(p => p.Publication_Tags).HasForeignKey(p => p.TagId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
