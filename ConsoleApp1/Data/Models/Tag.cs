using FilmCatalog.Data.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FilmCatalog.Data.Models
{
    public class Tag : ITag
    {
        public Tag()
        {

        }
        public Tag(string name, User user)
        {
            this.Name = name;
            this.User = user;
        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<MovieTag> MovieTags { get; set; }

        public User User { get; set; }
    }
}
