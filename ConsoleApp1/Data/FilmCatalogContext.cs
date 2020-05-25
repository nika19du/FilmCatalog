using FilmCatalog.Data.Models;
using FilmCatalog.Data.Models.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data
{
    public class FilmCatalogContext: DbContext
    {
        public FilmCatalogContext()
        {

        }
        public FilmCatalogContext(DbContextOptions options) : base(options)
        {

        }

        //имплементация на класовете като таблици в бд, virtual e,заради nunit тестовете
        public virtual DbSet<Movie> Movies { get; set; } 

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<MovieTag> MovieTags { get; set; }

        public virtual DbSet<Genre> Genres { get; set; }

        public virtual DbSet<Rating> Ratings { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)//ако не е свързано с бд, то if го пр:
            {
                optionsBuilder.UseSqlServer(Connection.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {//правя връзките м-у класовете
            // Many - to - Many Relationships:
            modelBuilder.Entity<MovieTag>().HasKey(x => new
            { x.MovieId, x.TagId });

            //конфигурация на one-to-many relationship: 
            modelBuilder.Entity<MovieTag>().HasOne(x => x.Movie).WithMany(m => m.MovieTags).HasForeignKey(x => x.MovieId);

            modelBuilder.Entity<MovieTag>().HasOne(x => x.Tag).WithMany(k => k.MovieTags).HasForeignKey(x => x.TagId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
