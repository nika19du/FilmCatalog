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
    public class TagController
    {
        private FilmCatalogContext context;
        private Validate validate;
        private string result;
        public TagController()
        {
            context = new FilmCatalogContext();
            validate = new Validate();
        }

        public TagController(FilmCatalogContext context)
        {
            this.context = context;
            this.validate = new Validate(context);
        }

        public List<Tag> GetAllTags()
        {
            return context.Tags
                .Include(t => t.MovieTags)
                .ThenInclude(x => x.Movie)
                .ThenInclude(x => x.MovieTags).ToList();
        }

        public Tag GetTag(int id)
        {
            return context.Tags.FirstOrDefault(x => x.Id == id);
        }

        public void AddTag(Tag tag)
        {
            context.Tags.Add(tag);
            context.SaveChanges();
        }

        public void UpdateTag(Tag tag)
        {
            Tag item = context.Tags.FirstOrDefault(x => x.Id == tag.Id);
            if (item != null)
            {
                context.Entry(item).CurrentValues.SetValues(tag);
                context.SaveChanges();
                Console.WriteLine("Successfully update tag!");
            }
        }

        public void DeleteTag(int id)
        {
            var MovieTags = context.MovieTags.Where(x => x.TagId == id);
            context.MovieTags.RemoveRange(MovieTags);

            Tag tag = context.Tags.FirstOrDefault(x => x.Id == id);
            context.Tags.Remove(tag);
            context.SaveChanges();
        }

        public void Delete()
        {
            int validator = 0;
            Tag tag = new Tag();
            string result = null;
            string tagName = null;

            while (validate.ValidateTagName(tagName) != 0)
            {
                Console.Write("\nName: ");
                tagName = Console.ReadLine().Trim().ToLower();
                validator = validate.ValidateTagName(tagName);

                if (validator == 0)
                {
                    result = "Successfully deleted tag!";
                    break;
                }
                else if (validator == -1)
                {
                    result = "Name cannot be empty!";
                }
                else if (validator == 1)
                {
                    result = "Such tag doesn't exist!";
                }
                Console.WriteLine(result);
            }
            tag.Name = tagName;
            context.Remove(context.Tags.First(x => x.Name == tag.Name));
            context.SaveChanges();
            Console.WriteLine(result);
        }

        public string TagRead()
        {
            string tagName = null;
            int validator = 0;

            do
            {
                Console.WriteLine("\nEnter tag name: ");
                tagName = Console.ReadLine().ToLower().Trim();
                validator = validate.ValidateTagName(tagName);
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
                    result = "Successfully added tag name!";
                }
                Console.WriteLine(result);
            } while (validator != 1);
            return tagName;
        }
    }
}
