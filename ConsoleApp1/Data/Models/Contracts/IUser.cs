using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Data.Models.Contracts
{
    public interface IUser
    {
        int Id { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        string email { get; set; }

        ICollection<Movie> MoviesList { get; set; }

        ICollection<Rating> Ratings { get; set; }

        ICollection<Tag> Tags { get; set; }
    }
}
