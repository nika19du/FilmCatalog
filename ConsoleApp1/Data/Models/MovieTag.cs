using FilmCatalog.Data.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data.Models
{
    public class MovieTag : IMovieTag
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
