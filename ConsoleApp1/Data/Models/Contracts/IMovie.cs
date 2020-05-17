using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data.Models.Contracts
{
    public interface IMovie
    {
        int Id { get; set; }

        string Name { get; set; }

        string Actors { get; set; }

        string Director { get; set; }

        string Genre { get; set; }

        string Description { get; set; }

        ICollection<MovieTag> MovieTags { get; set; }

        int UserId { get; set; }
    }
}
