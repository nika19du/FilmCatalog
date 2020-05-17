using Microsoft.EntityFrameworkCore;
using Moq;
using FilmCatalog.Controller;
using FilmCatalog.Data;
using FilmCatalog.Data.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieCatalogTests
{
    public class RatingControllerTests
    {
        [TestCase]
        public void Test_CalculateRating()
        {
            var user = new User { UserName = "pepo", Password = "pepo123", email = "pepo@abv.bg", Id = 1 };
            var movie = new Movie("Hachiko", "First actor", "L.M.", "drama", "none");
            var data = new List<Rating>
            {
                new Rating(5,movie,user){Id=1}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Rating>>();
            mockSet.As<IQueryable<Rating>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Rating>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Rating>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Rating>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Ratings).Returns(mockSet.Object);

            var service = new RatingController(mockContext.Object);

            var calc = service.CalculateRating(movie);

            Assert.AreEqual(5, calc);
        }
    }
}
