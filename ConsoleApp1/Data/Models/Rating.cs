using FilmCatalog.Data.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data.Models
{
    public class Rating : IRating
    {
        public Rating()
        {

        }
        public Rating(double score, Movie movie,User user)
        {
            this.Score = score;
            this.Movie = movie;
            this.User = user;
        }

        public int Id { get; set; }

        public double Score { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public virtual User User { get; set; }
    }
}
