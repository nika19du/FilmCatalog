using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data.Models.Contracts
{
    public interface IMovieTag
    {//The specified type 'FilmCatalog.Data.Models.Contracts.IMovieTag'must be a non-interface reference type to be used as an entity type .
        int MovieId { get; set; }

        Movie Movie { get; set; }

        int TagId { get; set; }

        Tag Tag { get; set; }
    }
}
