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
    public class GenreControllerTests
    {
        [TestCase]
        public void Add_Genre_In_Database()
        {
            var mockSet = new Mock<DbSet<Genre>>();

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(m => m.Genres).Returns(mockSet.Object);

            var service = new GenreController(mockContext.Object);
            service.AddGenre(new Genre());

            mockSet.Verify(m => m.Add(It.IsAny<Genre>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        [TestCase]
        public void Get_All_Genres_Should_Return_Correct_Count()
        {
            var data = new List<Genre>()
            {
                new Genre("drama"){Id=1 },
                new Genre("horror") {Id=2 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Genre>>();
            mockSet.As<IQueryable<Genre>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Genre>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Genre>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Genre>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //mokvam si context-a
            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Genres).Returns(mockSet.Object);

            var service = new GenreController(mockContext.Object);
            data.ToList().ForEach(g => service.AddGenre(g));

            var movie = service.GetAllGenres();
            int expected = 2;
            var result = movie.Count();
            //assertvame broqt
            Assert.AreEqual(expected, result);
        }

        [TestCase]
        public void GetAllGenres_Should_Return_Correct_GenreName()
        {
            var data = new List<Genre>()
            {
                new Genre("drama"){Id=1 },
                new Genre("horror") {Id=2 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Genre>>();
            mockSet.As<IQueryable<Genre>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Genre>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Genre>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Genre>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //mokvam si context-a
            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Genres).Returns(mockSet.Object);

            var service = new GenreController(mockContext.Object);
            data.ToList().ForEach(g => service.AddGenre(g));

            var movie = service.GetAllGenres();

            Assert.AreEqual("horror", movie[1].Name);
        }

        [TestCase]
        public void GetGenreById_Should_Return_Correct_Genre()
        {
            var data = new List<Genre>()
            {
                new Genre("drama"){Id=1 },
                new Genre("horror") {Id=2 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Genre>>();
            mockSet.As<IQueryable<Genre>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Genre>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Genre>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Genre>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //mokvam si context-a
            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Genres).Returns(mockSet.Object);

            var service = new GenreController(mockContext.Object);
            data.ToList().ForEach(g => service.AddGenre(g));

            var movie = service.GetGenre(1);

            Assert.AreEqual("drama", movie.Name);
        }
    }
}
