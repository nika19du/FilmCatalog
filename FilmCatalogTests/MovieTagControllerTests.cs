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
    public class MovieTagControllerTests
    {
        [TestCase]
        public void AddMovieTag()
        {
            var mockSet = new Mock<DbSet<MovieTag>>();

            var movieTag = new MovieTag();
            var mockContext = new Mock<FilmCatalogContext>();

            mockContext.Setup(m => m.MovieTags).Returns(mockSet.Object);

            var service = new MovieTagController(mockContext.Object);
            service.AddMovieTag(movieTag);

            mockSet.Verify(m => m.Add(It.IsAny<MovieTag>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        [TestCase]
        public void GetAllMovieTags_Gives_All_MoviesTags()
        {
            var data = this.CreateMovieTagMethod().AsQueryable();

            var mockSet = new Mock<DbSet<MovieTag>>();
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(m => m.MovieTags).Returns(mockSet.Object);

            var service = new MovieTagController(mockContext.Object);
            var movieTags = service.GetAllMovieTags();

            //Assertva count
            Assert.AreEqual(3, movieTags.Count());
        }
        [TestCase]
        public void Get_First_MovieId()
        {
            var data = this.CreateMovieTagMethod().AsQueryable();

            var mockSet = new Mock<DbSet<MovieTag>>();
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(m => m.MovieTags).Returns(mockSet.Object);

            var service = new MovieTagController(mockContext.Object);

            foreach (var movieTag in data)
            {
                service.AddMovieTag(movieTag);
            }

            var movieTags = service.GetAllMovieTags();

            //Assertva count
            Assert.AreEqual(1, movieTags[0].MovieId);
        }
        [TestCase]
        public void Get_First_TagId()
        {
            var data = this.CreateMovieTagMethod().AsQueryable();

            var mockSet = new Mock<DbSet<MovieTag>>();
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<MovieTag>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(m => m.MovieTags).Returns(mockSet.Object);

            var service = new MovieTagController(mockContext.Object);
            var movieTags = service.GetAllMovieTags();

            //Assertva count
            Assert.AreEqual(1, movieTags[0].TagId);
        }
        public List<MovieTag> CreateMovieTagMethod()
        {
            List<MovieTag> movieTags = new List<MovieTag>
            {
                new MovieTag{MovieId=1,TagId=1},
                new MovieTag { MovieId = 2, TagId = 2 },
                new MovieTag { MovieId = 3, TagId = 3 },
            };
            return movieTags;
        }
    }
}
