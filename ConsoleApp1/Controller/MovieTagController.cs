using FilmCatalog.Data;
using FilmCatalog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmCatalog.Controller
{
    public class MovieTagController
    {
        private FilmCatalogContext context;

        public MovieTagController()
        {
            context = new FilmCatalogContext();
        }
        public MovieTagController(FilmCatalogContext context)
        {
            this.context = context;
        }

        public void AddMovieTag(MovieTag movieTag)
        {
            context.MovieTags.Add(movieTag);
            context.SaveChanges();
        }

        public void DeleteMovieTag(int MovieId, int TagId)
        {
            var movieTag = context.MovieTags.Where(x => x.MovieId == MovieId && x.TagId == TagId);
            context.MovieTags.RemoveRange(movieTag);
            context.SaveChanges();
        }

        public List<MovieTag> GetAllMovieTags()
        {
            return context.MovieTags.ToList();
        }
    }
}
