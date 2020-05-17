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

                //Console.Write("\nEnter User name: ");
                //userName = Console.ReadLine();
           //     userName = display.ReturnName();
                //validator = validation.ValidateUserName(userName);
                //if (validator == 0)
                //{
                //    result = "Successfully found user!";
                //    // break;
                //}
                //else if (validator == -1)
                //{
                //    result = "Name cannot be empty!";
                //}
                //else if (validator == 1)
                //{
                //    result = "Such user doesn't exist!";
                //    return this.AddRating();
                //}
                //Console.WriteLine(result);
            } while (validation.ValidateMovieName(movieName) != 0);//&& validation.ValidateUserName(userName) != 0);

            if (string.IsNullOrEmpty(Form2.loginUser)==false)
            {
                userName = Form2.loginUser;
            }
            if (string.IsNullOrEmpty(RegisterForm.user)==false)
            {
                userName = RegisterForm.user;
            }

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
                    }
                }
                catch
                {
                    result = "Rating must be an integer!";
                    validator = 0;
                }
                Console.WriteLine(result);
            } while (validator != 1);

            Movie movie = context.Movies.First(x => x.Name == movieName);
            User user = context.Users.First(x => x.UserName == userName);
            Rating ratingToAdd = new Rating
            {
                Score = rating,
                Movie = movie,
                User = user
            };
            context.Ratings.Add(ratingToAdd);
            userController.AddRating(ratingToAdd, user);
            context.SaveChanges();
            // Console.WriteLine("Rating added to database.");
            return "Successfully added rating to database.";
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
