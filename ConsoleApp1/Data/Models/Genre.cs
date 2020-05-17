using FilmCatalog.Data.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data.Models
{
    public class Genre : IGenre
    {
        public Genre()
        {

        }

        public Genre(string name)
        {
            this.Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
