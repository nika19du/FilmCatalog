using FilmCatalog.Data.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FilmCatalog.Data.Models
{
    public class User : IUser
    {
        public User()
        {
            this.MoviesList = new List<Movie>();
            this.Ratings = new List<Rating>();
            this.Tags = new List<Tag>();
        }

        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string email { get; set; }

        public virtual ICollection<Movie> MoviesList { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
