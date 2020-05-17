using FilmCatalog.Data.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FilmCatalog.Data.Models
{
    public class Movie : IMovie
    {
        public Movie()
        {

        }
        public Movie(string name, string actors, string director, string genre, string description)
        {
            this.Name = name;
            this.Actors = actors;
            this.Director = director;
            this.Genre = genre;
            this.Description = description;
        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Actors { get; set; }

        public string Director { get; set; }

        public string Genre { get; set; }

        public string Description { get; set; }

        public ICollection<MovieTag> MovieTags { get; set; }

        public int UserId { get; set; }
    }
}
