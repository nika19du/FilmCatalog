using FilmCatalog.Controller.Validation;
using FilmCatalog.Data;
using FilmCatalog.Data.Models;
using FilmCatalog.Data.Models.Contracts;
using FilmCatalog.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmCatalog.Controller
{
    public class RatingController
    {
        private FilmCatalogContext context;
        private string result;
        //private MovieController movieController;
        private Validate validation;
        private UserController userController;
        private static int rating4e;
        // private Display display;
        public RatingController()
        {
            context = new FilmCatalogContext();
            //movieController = new MovieController();
            validation = new Validate();
            userController = new UserController();
        }
        public RatingController(FilmCatalogContext context)
        {
            this.context = context;
            //movieController = new MovieController(context);//davami stackoverflow za tva bex nego
            validation = new Validate(context);
            userController = new UserController(context);
        }

        public object AddRating()
        {//it was a void, but i try recoursian cuz when i have invalid movie name or username let me go back again.if recursion wasnt sucsess i would try with methods.
            string movieName = null;
            int validator = 0;
            int rating = 0;
            string userName = null;

            do
            {
                Console.Write("\nEnter Movie name: ");
                movieName = Console.ReadLine().Trim();
                validator = validation.ValidateMovieName(movieName);
                if (validator == 0)
                {
                    result = "Successfully found movie!";
                    // break;
                }
                else if (validator == -1)
                {
                    result = "Name cannot be empty!";
                }
                else if (validator == 1)
                {
                    result = "Such movie doesn't exist!";
                    Console.WriteLine(result);
                    return this.AddRating();
                }
                Console.WriteLine(result);
            } while (validation.ValidateMovieName(movieName) != 0);

            if (string.IsNullOrEmpty(Form2.loginUser) == false)
            {
                userName = Form2.loginUser;
            }
            if (string.IsNullOrEmpty(RegisterForm.user) == false)
            {
                userName = RegisterForm.user;
            }

            Movie movie = context.Movies.First(x => x.Name == movieName);
            User user = context.Users.First(x => x.UserName == userName);

            var answer = Setting(movie, user);

            if (answer == "y") //TO DO YES-A ??!!
            {
                var r = context.Ratings.FirstOrDefault(x => x.MovieId == movie.Id); //1 user може да е добавил мн рейтинг към мн филми,избираме не първият филм от базата,а този който искаме.

                foreach (var film in user.MoviesList)
                {
                    if (r.MovieId == film.Id)
                    {
                        if (r != null)
                        {
                            ManipulationRating(rating, movieName, validator);
                            r.Score = rating4e;
                            context.Ratings.Update(r);
                            context.SaveChanges();
                            return "Successfully updated rating in database.";
                        }
                    }
                }
            }
            if (answer == "yep")//if movieID=userId but this movie is added by other user
            {
                var a = userController.GetUser(user.UserName);
                var u = context.Ratings.FirstOrDefault(x => x.User.Id == a.Id);

                foreach (var rt in u.User.Ratings)
                {
                    if (rt.MovieId == a.Id)
                    {
                        ManipulationRating(rating, movieName, validator);
                        rt.Score = rating4e;
                        context.Ratings.Update(u);
                        context.SaveChanges();
                        return "Successfully updated rating in database.";
                    }
                }
            }
            if (answer == "yey")
            {
                var a = userController.GetUser(user.UserName);
                var m = context.Ratings.FirstOrDefault(x => x.MovieId == movie.Id);

                foreach (var r in a.Ratings)
                {
                    if (m.MovieId == r.MovieId)
                    {
                        ManipulationRating(rating, movieName, validator);
                        r.Score = rating4e;
                        context.Ratings.Update(r);
                        context.SaveChanges();
                        return "Successfully updated rating in database.";
                    }
                }
            }
            if (answer == "n") return "The rating has not changed. It's still the same.";

            if (answer == "r not exist")
            {
                ManipulationRating(rating, movieName, validator);

                Rating ratingToAdd = new Rating
                {
                    Score = rating4e,
                    Movie = movie,
                    User = user
                };
                context.Ratings.Add(ratingToAdd);
                userController.AddRating(ratingToAdd, user);
                context.SaveChanges();
                return "Successfully added rating to database.";
            }
            else return "";
        }

        private void ManipulationRating(int rating, string movieName, int validator)
        {
            do
            {
                try
                {
                    Console.Write("\nEnter rating between 0 and 5: ");
                    string inputRating = Console.ReadLine().Trim();
                    rating = int.Parse(inputRating);
                    if (rating < 0 || rating > 5)
                    {
                        result = "Rating must be between 0 and 5!";
                        validator = -1;
                    }
                    else
                    {
                        result = "Rating is valid.";
                        validator = 1;
                        rating4e = rating;
                    }
                }
                catch
                {
                    result = "Rating must be an integer!";
                    validator = 0;
                }
                Console.WriteLine(result);
            } while (validator != 1);
        }

        private string Setting(Movie movie, User user)
        {
            var a = userController.GetUser(user.UserName);
            var u = context.Ratings.FirstOrDefault(x => x.User.Id == a.Id);
            if (u == null) { return "r not exist"; }//v sluchaq nqmam user v reitinga ???

            var m = context.Ratings.FirstOrDefault(x => x.MovieId == movie.Id);
            if (m == null) { return "r not exist"; }//pr proverka, zajoto moje v ratings clasa da nqma dadeniq film i kato go lipsva go dobavqme bez da produljava nadolu 
            string answer;

            if (a.Ratings.FirstOrDefault(x => x.MovieId == m.MovieId) == null)
            {
                return "r not exist";
            }

            foreach (var r in a.Ratings)//tova go izp kogato imam user koito updateva reiting kum chujd film
            {
                if (m.MovieId == r.MovieId)
                {
                    Console.WriteLine("You have been added rating in this movie.\nDo you want to update old rating? Yes or No?");
                    answer = Console.ReadLine();
                    if (answer == "Yes" || answer == "yes")
                    {
                        return "yey";
                    }
                    else { return "n"; }
                }
            }


            if (u != null)
            {
                foreach (var film in a.MoviesList)
                {
                    if (m.MovieId == film.Id)//m.MovieId
                    {
                        Console.WriteLine("You have been added rating in this movie.\nDo you want to update old rating? Yes or No?");
                        answer = Console.ReadLine();
                        if (answer == "Yes" || answer == "yes")
                        {
                            return "y";
                        }
                        else { return "n"; }
                    }
                    else continue;
                }
                if (u.User.Id == u.MovieId)
                {
                    Console.WriteLine("You have been added rating in this movie.\nDo you want to update old rating? Yes or No?");
                    answer = Console.ReadLine();
                    if (answer == "Yes" || answer == "yes")
                    {
                        return "yep";
                    }
                    else { return "n"; }
                }
                else
                { return ""; }
            }
            else { return "is null"; }
        }

        public double CalculateRating(IMovie movie)
        {
            double rating = 0;
            List<Rating> ratings = new List<Rating>();

            foreach (var item in context.Ratings.Where(x => x.Movie.Name == movie.Name))
            {
                ratings.Add(item);
            }
            rating = ratings.Sum(x => x.Score);
            rating = rating / ratings.Count();
            return rating;
        }
    }
}
