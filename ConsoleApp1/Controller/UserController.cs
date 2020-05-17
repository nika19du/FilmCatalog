using FilmCatalog.Data;
using FilmCatalog.Data.Models;
using FilmCatalog.Controller.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FilmCatalog.Data.Models.Contracts;
using System.Windows.Forms;

namespace FilmCatalog.Controller
{
    public class UserController
    {
        private FilmCatalogContext context;
        //private User currentUser = new User();
        private Validate validate;
        private string result;
        public UserController()
        {
            context = new FilmCatalogContext();
            this.validate = new Validate();
        }
        public UserController(FilmCatalogContext context)
        {
            this.context = context;
            this.validate = new Validate(context);
        }

        public bool AddUsers(string userName, string password, string email)
        {
            User currentUser = new User();
            currentUser.UserName = userName;
            currentUser.Password = password;
            currentUser.email = email;
            //bool isInDb = false;

            var user = context.Users.FirstOrDefault(x => x.UserName == currentUser.UserName);

            if (user != null)
            {
                currentUser.MoviesList = new List<Movie>();
                currentUser.Ratings = new List<Rating>();
                currentUser.Tags = new List<Tag>();
                context.Users.Add(currentUser);
                context.SaveChanges();
                return true;//!!!!!!!!
            }
            else return false;

            //foreach (var user1 in context.Users)
            //{
            //    try
            //    {
            //        if (user1.UserName == currentUser.UserName)
            //        {
            //            isInDb = true;
            //        }
            //    }
            //    catch
            //    {

            //    }
            //}
            //if (isInDb == false)
            //{
            //    currentUser.MoviesList = new List<Movie>();
            //    currentUser.Ratings = new List<Rating>();
            //    currentUser.Tags = new List<Tag>();
            //    context.Users.Add(currentUser);
            //    context.SaveChanges();
            //    return true;//!!!!!!!!
            //}
            //else return false;
        }

        public bool LogIn(string[] input)
        {
            User currentUser = new User();
            User user = null;
            user = this.GetUser(input[0]);
            if (user != null)
            {
                if (user.Password == input[1])
                {
                    currentUser = user;
                    return true;
                }
                else return false;
            }
            return false;
        }

        public void DeleteUser(string userName)
        {
            User user = context.Users.Include(x => x.MoviesList).Include(x => x.Ratings).FirstOrDefault(x => x.UserName == userName);
            //Tag tag = context.Tags.Include(x => x.User).FirstOrDefault(x => x.User == user);
            //context.Tags.Remove()
            context.RemoveRange(context.Tags.Where(x => x.User.Id == user.Id));
            context.Users.Remove(user);
            context.SaveChanges();
        }
        public User GetUser(string userName)
        {
            User user = context.Users.Include(x => x.MoviesList).Include(x => x.Ratings).Include(x => x.Tags).FirstOrDefault(x => x.UserName == userName);
            return user;
        }

        public User GetUserById(int id)
        {
            return context.Users.Include(x => x.MoviesList).FirstOrDefault(x => x.Id == id);
        }

        public List<User> GetAllUsers()
        {
            return context.Users.Include(x => x.Tags).Include(x => x.Ratings).Include(x => x.MoviesList).ToList();
        }
        public void UpdateUserPassword(string userName, string nextPassword)
        {
            User userToUpdate = context.Users.FirstOrDefault(x => x.UserName == userName);
            if (userToUpdate != null)
            {
                Console.WriteLine("Password updated!");
                userToUpdate.Password = nextPassword;
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("User name doesn't exist!");
            }
        }

        public void UpdateUserName(string userName, string nextName)
        {
            User userToUpdate = context.Users.FirstOrDefault(x => x.UserName == userName);
            if (userToUpdate != null)
            {
                Console.WriteLine("Name updated!");
                userToUpdate.UserName = nextName;
                context.SaveChanges();
            }
            else Console.WriteLine("User name doesn't exist!");
        }

        public void UpdateUserEmail(string userEmail, string nextEmail)
        {
            User userToUpdate = context.Users.FirstOrDefault(x => x.email == userEmail);
            if (userToUpdate != null)
            {
                Console.WriteLine("Email updated!");
                userToUpdate.email = nextEmail;
                context.SaveChanges();
            }
            else Console.WriteLine("Invalid email!");
        }

        public void AddMovieList(Movie movie, User user)
        {
            user.MoviesList.Add(movie);
            context.SaveChanges();
        }

        public void AddRating(Rating rating, User user)
        {
            user.Ratings.Add(rating);
            context.SaveChanges();
        }

        public void AddTag(Tag tag, User user)
        {
            user.Tags.Add(tag);
            context.SaveChanges();
        }

        public string UserRead(string username)
        {
           // string username = null;
            int validator = 0;

            do
            {
                //Console.WriteLine("Enter user name:");
                //username = Console.ReadLine().Trim();
                validator = validate.ValidateUserName(username.Trim());

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
                    result = "Successfully added user name!";
                }
               // MessageBox.Show(result);
               // Console.WriteLine(result);

            } while (validator != 1);

            return username;
        }

        public string PasswordRead(string password)
        {
           // string password = null;
            int validator = 0;

            do
            {
                //Console.WriteLine("Enter password: ");
                //password = Console.ReadLine().Trim();
                validator = validate.ValidatePassword(password.Trim());
                if (validator == -1)
                {
                    result = "Password cannot be empty!";
                }
                else if (validator == 0)
                {
                    result = "Password length need to be greater than 6 symbols.";
                }
                else if (validator == 1)
                {
                    result = "Successfylly added password!";
                }
               // MessageBox.Show(result);
              //  Console.WriteLine(result);
            } while (validator != 1);

            return password;
        }

        public string EmailRead(string email)
        {
            //string email = null;
            while (true)
            {
                //Console.WriteLine("Enter email: ");
                //email = Console.ReadLine();
                if (validate.IsEmailValid(email) == false)
                {
                    result = "Invalid email!!!";
                    //Console.WriteLine(result);
                    MessageBox.Show(result);
                }
                else
                {
                    result = "Successfully added email address!";
                    break;
                }
            }
            //Console.WriteLine(result);
            //MessageBox.Show(result);
            return email;
        }
    }
}
