using FilmCatalog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmCatalog.Controller.Validation
{
    public class Validate
    {
        private FilmCatalogContext context;
        public Validate()
        {
            context = new FilmCatalogContext();
        }
        public Validate(FilmCatalogContext context)
        {
            this.context = context;
        }

        public int ValidateMovieName(string movieName)
        {
            if (string.IsNullOrEmpty(movieName))
            {
                return -1;
            }
            try
            {
                context.Movies.First(x => x.Name == movieName);
                return 0;
            }
            catch
            {
                return 1;
            }
        }
        public int ValidateUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return -1;
            }
            try
            {
                context.Users.First(x => x.UserName == userName);
                return 0;
            }
            catch
            {
                return 1;
            }
        }
        public int ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return -1;
            }
            else if (password.Length < 6)
            {
                return 0;
            }
            return 1;
        }
        public bool IsEmailValid(string email)
        {
            //if (string.IsNullOrWhiteSpace(email)) return false;
            //// MUST CONTAIN ONE AND ONLY ONE @
            //var atCount = email.Count(x => x == '@');
            //if (atCount != 1) return false;
            //if (email.Contains(".")) return false;

            try
            {//The code uses System.Net.Mail Namespace to check email address and returns True/False.
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int ValidateGenreName(string genreName)
        {
            var check = context.Genres.ToList();
            try
            {
                check.First(x => x.Name.ToLower() == genreName.ToLower());
            }
            catch
            {
                return -1;
            }
            return 1;
        }

        public int ValidateTagName(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return -1;
            }
            try
            {
                context.Tags.First(x => x.Name == tagName);
                return 0;
            }
            catch
            {
                return 1;
            }
        }
    }
}
