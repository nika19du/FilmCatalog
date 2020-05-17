using FilmCatalog.Controller.Validation;
using FilmCatalog.Data;
using FilmCatalog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmCatalog.Controller
{
    public class GenreController
    {
        private FilmCatalogContext context;
        private string result;
        private Validate validate;
        public GenreController()
        {
            context = new FilmCatalogContext();
            validate = new Validate();
        }
        public GenreController(FilmCatalogContext context)
        {
            this.context = context;
            this.validate = new Validate(context);
        }

        public List<Genre> GetAllGenres()
        {
            return context.Genres.ToList();
        }

        public Genre GetGenre(int id)
        {
            return context.Genres.FirstOrDefault(x => x.Id == id);
        }

        public void AddGenre(Genre genre)
        {
            context.Genres.Add(genre);
            context.SaveChanges();
        }

        public void UpdateGenre(Genre genre)
        {
            Genre item = context.Genres.FirstOrDefault(x => x.Id == genre.Id);
            if (item != null)
            {
                context.Entry(item).CurrentValues.SetValues(genre);
                context.SaveChanges();
            }
        }

        public void DeleteGenres(int id)
        {
            Genre genre = context.Genres.FirstOrDefault(x => x.Id == id);
            context.Genres.Remove(genre);
            context.SaveChanges();
        }

        public string GenreRead()
        {
            string genreName = "";
            int validator = 0;
            List<string> genreToParse = new List<string>();
            int genreCount = 0;

            while (true)
            {
                Console.WriteLine("\nEnter genre:");
                genreName = Console.ReadLine().ToLower().Trim();
                if (genreCount == 0 && genreName.Equals("end"))
                {
                    result = "Please enter atleast one genre.";
                    Console.WriteLine(result);
                }
                if (genreName.Equals("end") == false)
                {
                    validator = validate.ValidateGenreName(genreName);
                    if (validator == 1)
                    {
                        result = "Successfully added genre!\n";
                        genreCount++;
                        genreToParse.Add(genreName);
                    }
                    else if (validator == -1)
                    {
                        result = "Genre doesn't exist!";
                    }
                    Console.WriteLine(result);
                }
                else if (genreName.Equals("end"))
                {
                    break;
                }
            }
            result = "Finished adding genres!\n";
            Console.WriteLine(result);
            var genres = GenreEdit(genreToParse);
            return genres;
        }

        public string GenreEdit(List<string> genreToParse)
        {
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (var genre in genreToParse)
            {
                counter++;
                if (counter == genreToParse.Count)
                {
                    sb.Append(genre);
                    continue;
                }
                sb.Append(genre + ",");
            }
            return sb.ToString();
        }
        public string GenresToPlainText(string genres)
        {
            List<string> allgenres = genres.Split(',').ToList();
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var genre in allgenres)
            {
                sb.Append(i + " " + genre);
                i++;
            }
            return sb.ToString();
        }
    }
}
