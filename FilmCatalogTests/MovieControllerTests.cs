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
    public class MovieControllerTests
    {
        [TestCase]
        public void Test_Add_Movie_In_Database()
        {
            var data = new List<Movie>
            {
                new Movie("It","First Actor,Second Actor","J.R.","horror","no description"){Id=1 },
                new Movie("It2","First Actor,Second Actor","J.R.","horror","no description"){Id=2}
            }.AsQueryable();
            //ako ne setna id je e 0

            //Mockvam si dbSet-a:
            var mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //mokvam si context-a
            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Movies).Returns(mockSet.Object);

            //vzimam si realniq movie Controller,tr mi realniq movie contr.,no tr da imam falshiv context za da ne se dobavq v bazata.A moknatiq context zavisi ot nqkakuv DBSET.Ne polzvam ralniq DBSET za da ne pipam realnite obekti.Iztestvam logikata na prilojenieto(contr),a ne na dbset ||na contexta.Falshivite neja podavam na realniq obekt za da iztestvam samo logikata na realniq obekt
            var service = new MovieController(mockContext.Object);
            data.ToList().ForEach(m => service.AddMovie(m));

            var movie = service.GetMovie(1);

            Assert.AreEqual(1, movie.Id);//assertvam dali id e ravno na id,koeto sum podala
        }
        [TestCase]
        public void CreateMovie_saves_a_movie_context()
        {

            var mockSet = new Mock<DbSet<Movie>>();

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(m => m.Movies).Returns(mockSet.Object);//zadavam povedenie na movies-kogato izvika mockContext.Movies iskam da mi vurne  mockSet t.e DbSet ot <Movie>>();

            var service = new MovieController(mockContext.Object);
            service.AddMovie(new Movie("Test1", "Actor1", "none", "drama", "none"));

            //Kogato rabotim s CRUD operacii i s mokvane na obekti nqmame Assert imame Verify,
            //Kogato testvam zaqvka ili durpam inf ne izp Verify a assert
            mockSet.Verify(m => m.Add(It.IsAny<Movie>()), Times.Once());//times.Once-vednuj se izp dobavqne
            //mockSet.Verify(m => m.Remove(It.IsAny<Movie>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        [TestCase]//je testva get all movie methoda
        public void GetAllMovies_Should_Return_All_Movie_In_Database()
        {
            var data = this.CreateMovieMethod().AsQueryable();

            //ako ne setna id je e 0

            //Mockvam si dbSet-a:
            var mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //mokvam si context-a
            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Movies).Returns(mockSet.Object);

            var service = new MovieController(mockContext.Object);

            foreach (var m in data)
            {
                service.AddMovie(m);
            }//data.ToList().ForEach(m => service.AddMovie(m));

            var movie = service.GetAllMovies();
            int expected = 4;
            var result = movie.Count();
            //assertvame broqt
            Assert.AreEqual(expected, result);
        }
        [TestCase]
        public void GetAllMovies_Should_Return_CorrectName()
        {
            var data = this.CreateMovieMethod().AsQueryable();
            //ako ne setna id je e 0

            //Mockvam si dbSet-a:
            var mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //mokvam si context-a
            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Movies).Returns(mockSet.Object);

            var service = new MovieController(mockContext.Object);
            data.ToList().ForEach(m => service.AddMovie(m));

            var movie = service.GetAllMovies();
            Assert.AreEqual("Harry Potter", movie[2].Name);
        }
        [TestCase]//testva getId
        public void GetMovieById_Should_Return_Correct_Genre()
        {
            var data = this.CreateMovieMethod().AsQueryable();
            //ako ne setna id je e 0

            //Mockvam si dbSet-a:
            var mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //mokvam si context-a
            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Movies).Returns(mockSet.Object);

            var service = new MovieController(mockContext.Object);
            data.ToList().ForEach(m => service.AddMovie(m));

            var result = service.GetMovie(2);

            Assert.AreEqual("horror", result.Genre);
        }
        public List<Movie> CreateMovieMethod()
        {
            List<Movie> movies = new List<Movie>
            {
                new Movie("It","First Actor,Second Actor","J.R.","horror","no description"){Id=1 },
                new Movie("It2","First Actor,Second Actor","J.R.","horror","no description"){Id=2},
                new Movie("Harry Potter","Daniel Radglif,Emma Watson","J.R.","fantasy","magical world"){Id=3},
                new Movie("Harry Potter 2","Daniel Radglif,Emma Watson","J.R.","fantasy","magical world"){Id=4}
            }.ToList();
            return movies;
        }
    }
}
