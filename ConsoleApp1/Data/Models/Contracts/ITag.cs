using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data.Models.Contracts
{
    public interface ITag
    {
        int Id { get; set; }

        string Name { get; set; }

        ICollection<MovieTag> MovieTags { get; set; }

        User User { get; set; }
    }
}
