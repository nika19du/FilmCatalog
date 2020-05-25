using FilmCatalog.Controller.Validation;
using FilmCatalog.Data;
using FilmCatalog.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmCatalog.Controller
{
    public class MovieController
    {
        private FilmCatalogContext context;
        private string result;
        private GenreController genreController;
        private RatingController ratingController;
        private Validate validate;
        public MovieController()
        {
            context = new FilmCatalogContext();
            ratingController = new RatingController();
            validate = new Validate();
        }

        public MovieController(FilmCatalogContext context)
        {
            this.context = context;
            this.genreController = new GenreController(context);
            this.ratingController = new RatingController(context);
            this.validate = new Validate(context);
        }

        public List<Movie> GetAllMovies()
        {
            return context.Movies.Include(x => x.MovieTags)
               .ThenInclude(x => x.Tag).ToList();
        }

        public Movie GetMovie(int id)
        {
            return context.Movies.Include(x => x.MovieTags)
                .ThenInclude(x => x.Tag).FirstOrDefault(x => x.Id == id);
        }

        public List<Movie> GetMovieByName(string name)
        {
            return context.Movies.Include(x => x.MovieTags)
                .ThenInclude(x => x.Tag)
                .Where(x => x.Name == name).ToList();
        }
        public Movie GetMovieByNameReturnMovie(string name)
        {
            //var movie = context.Movies.Include(x => x.MovieTags)
            //    .ThenInclude(x => x.Tag)
            //    .Where(x => x.Name == name).ToList();
            var m = context.Movies.FirstOrDefault(x => x.Name == name);
            return m;
        }
        public void AddMovie(Movie movie)
        {
            context.Movies.Add(movie);
            context.SaveChanges();
        }

        public void UpdateMovieName(Movie movie)
        {
            string newMovieName = MovieRead();
            movie.Name = newMovieName;
            context.Movies.Update(movie);
            context.SaveChanges();
        }
        public string MovieRead()
        {
            string movieName = null;
            int validator = 0;

            do
            {
                Console.WriteLine("\nEnter movie name: ");
                movieName = Console.ReadLine().ToLower().Trim();
                if (validate.ValidateMovieName(movieName) != -1)
                {
                    movieName = movieName.First().ToString().ToUpper() + movieName.Substring(1);
                }
                validator = validate.ValidateMovieName(movieName);
                if (validator == 0)
                {
                    result = "Name already exist!";
                }
                else if (validator == -1)
                {
                    result = "Name cannot be empty";
                }
                else if (validator == 1)
                {
                    result = "Successfully added movie name!";
                }
                Console.WriteLine(result);
            } while (validator != 1);
            return movieName;
        }
        public void UpdateCommand()
        {
            string movieName = null;
            string[] options = { "genre", "name", "description" };
            string choice = null;
            int validator = 0;
            do
            {
                Console.Write("\nName: ");
                movieName = Console.ReadLine().Trim().ToLower();
                if (validate.ValidateMovieName(movieName) != -1)
                {
                    movieName.Split(movieName.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    movieName = movieName.First().ToString().ToUpper() + movieName.Substring(1);
                }
                validator = validate.ValidateMovieName(movieName);
                if (validator == 0)
                {
                    result = "Successfully found movie!";
                }
                else if (validator == -1)
                {
                    result = "Name cannot be empty!";
                }
                else if (validator == 1)
                {
                    result = "Such movie doesn't exist!";
                }
                Console.WriteLine(result);
            } while (validator != 0);

            Movie movie = context.Movies.First(x => x.Name == movieName);
            Console.WriteLine(this.MovieOutput(movie));
            Console.WriteLine("\nWhat do you want to update?");

            while (options.Contains(choice) == false)
            {
                choice = Console.ReadLine().ToLower().Trim();
                if (options.Contains(choice))
                {
                    break;
                }
                Console.WriteLine("Invalid input");
            }
            switch (choice)
            {
                case "name":
                    this.UpdateMovieName(movie);
                    break;
                case "genre":
                    this.UpdateGenres(movie);
                    break;
                case "description":
                    UpdateDescription(movie);
                    break;
            }
        }

        private void UpdateDescription(Movie movie)
        {//updateva descriptiona na movieto kato maha stariqt description i slaga nov na negovoto mqsto
            string desctiption = DescriptionRead();
            movie.Description = desctiption;
            context.Movies.Update(movie);
            context.SaveChanges();
        }

        public string DescriptionRead()
        {
            int paragraphCount = 1;
            string descriptionName = null;
            int validator = 0;
            StringBuilder sb = new StringBuilder();
            do
            {
                Console.Write("\nEnter description: ");
                string paragraph = Console.ReadLine();
                validator = ValidateDescription(paragraph, paragraphCount);
                if (validator == 1)
                {
                    result = "Successfully added description!";
                }
                else if (validator == -1 || validator == 0)
                {
                    result = "Please enter a valid paragraph.";
                    Console.WriteLine(result);
                    continue;
                }
                sb.Append(paragraph + "\n");
                paragraphCount++;
            } while (validator != 1);
            descriptionName = sb.ToString();
            descriptionName = descriptionName.Remove(descriptionName.Length - 2);
            Console.WriteLine(result);
            return descriptionName;
        }

        private int ValidateDescription(string description, int paragraphCount)
        {
            if (string.IsNullOrEmpty(description))
            {
                return -1;
            }
            else if (description.Equals("#") && paragraphCount == 1)
            {
                return 0;
            }
            else if (description.EndsWith("#"))
            {
                return 1;
            }
            return 2;
        }

        private void UpdateGenres(Movie movie)
        {
            Console.WriteLine("Do you want to add a new genre or to remove an existing one.");
            string command = Console.ReadLine().Trim().ToLower();
            string genres = movie.Genre;
            switch (command)
            {
                case "add":
                    UpdateGenresAdd(movie);
                    break;
                case "remove":
                    UpdateGenreRemove(movie);
                    break;
            }
        }

        private void UpdateGenreRemove(Movie movie)
        {
            var allGenres = movie.Genre.Split(',').ToArray();
            StringBuilder sb = new StringBuilder();
            int index = 0;

            if (allGenres.Length == 1)
            {
                result = "You cannot delete genres because there is only one!";
                Console.WriteLine(result);
                UpdateGenres(movie);
            }
            while (true)
            {
                try
                {
                    Console.WriteLine("Which genres do you want to edit? (enter a number): ");
                    string inputIndex = Console.ReadLine().Trim();
                    index = int.Parse(inputIndex) - 1;// -1 cuz we have array wich start from zero
                    if (index < allGenres.Count() && index >= 0)
                    {
                        break;
                    }
                }
                catch
                { }
                Console.WriteLine("Invalid input!");
            }
            for (int i = 0; i < allGenres.Length; i++)
            {
                if (i == index)
                {//прескача жанрът който искаме да махнем.
                    continue;
                }
                sb.Append(allGenres[i] + ",");
            }//sb=>(drama,)=> sb.length-1 maha ,
            movie.Genre = sb.ToString().Remove(sb.Length - 1);
            context.Movies.Update(movie);
            context.SaveChanges();
            Console.WriteLine("Genre was removed");
        }

        private void UpdateGenresAdd(Movie movie)
        {
            // Console.WriteLine("Enter genre name:");
            string genres = genreController.GenreRead();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(movie.Genre + "," + genres);
            movie.Genre = sb.ToString();
            context.Movies.Update(movie);
            context.SaveChanges();
        }

        public string MovieOutput(Movie movie)
        {
            string genres = null;
            string description = null;
            StringBuilder sb = new StringBuilder();
            genres = movie.Genre;
            description = movie.Description;

            double rating = ratingController.CalculateRating(movie);

            sb.Append("\nName: " + movie.Name + $"  Rating: {rating:f2}" + "\n" + "\nGenres:");
            sb.Append(genres + "\n");
            sb.Append("Movie info:\n" + movie.Description);
            return sb.ToString();
        }

        public void Delete()
        {
            int validator = 0;
            Movie movie = new Movie();
            string result = null;
            string movieName = null;

            while (validate.ValidateMovieName(movieName) != 0)
            {
                Console.Write("\nName: ");
                movieName = Console.ReadLine().Trim().ToLower();
                if (validate.ValidateMovieName(movieName) != -1)
                {
                    movieName = movieName.First().ToString().ToUpper() + movieName.Substring(1);
                }
                validator = validate.ValidateMovieName(movieName);
                if (validator == 0)
                {
                    result = "Successfully deleted movie!";
                    break;
                }
                else if (validator == -1)
                {
                    result = "Name cannot be empty!";
                }
                else if (validator == 1)
                {
                    result = "Such movie doesn't exist!";
                }
                Console.WriteLine(result);
            }
            movie.Name = movieName;
            context.RemoveRange(context.Ratings.First(x => x.Movie.Name == movie.Name));

            context.Remove(context.Movies.First(x => x.Name == movie.Name));
            context.SaveChanges();
            Console.WriteLine(result);
        }

        public string ActorsRead()
        {
            string actorName = "";
            List<string> actors = new List<string>();

            while (true)
            {
                actorName = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(actorName))
                {
                    result = "Actors names cannot be empty!";
                }
                else
                {
                    result = "Successfully added actors!\n";
                    break;
                }
            }
            result = "Finished adding actors\n";
            Console.WriteLine(result);
            return actorName;
        }
        public string DirectorRead()
        {
            string directorName = null;

            while (true)
            {
                directorName = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(directorName))
                {
                    result = "Director cannot be empty";
                }
                else
                {
                    result = "Successfully added director!\n";
                    break;
                }
                Console.WriteLine(result);
            }
            return directorName;
        }
    }
}
