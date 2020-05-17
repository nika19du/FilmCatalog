using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data.Models.Contracts
{
    public interface IGenre
    {
        int Id { get; set; }

        string Name { get; set; }
    }
}
