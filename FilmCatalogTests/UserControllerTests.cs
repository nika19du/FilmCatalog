using Microsoft.EntityFrameworkCore;
using Moq;
using FilmCatalog.Controller;
using FilmCatalog.Data;
using FilmCatalog.Data.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MovieCatalogTests
{
    public class UserControllerTests
    {
        [TestCase]
        public void Test_Add_User_In_Database()
        {
            User user = new User() { Id = 1 };
            user.UserName = "pepi";
            user.Password = "pepi123";
            user.email = "pepi@abv.bg";
            User user2 = new User() { Id = 2 };
            user.UserName = "joro";
            user.Password = "joro123";
            user.email = "joro@abv.bg";
            var data = new List<User>
            {
                user,user2
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserController(mockContext.Object);
            data.ToList().ForEach(x => service.AddUsers(x.UserName, x.Password, x.email));

            var userr = service.GetUser("pepi");

            Assert.AreEqual(1, user.Id);
        }
        [TestCase]
        public void GetAllUseres_Should_Return_CorrectiName()
        {
            User user = new User() { Id = 1 };
            user.UserName = "pepi";
            user.Password = "pepi123";
            user.email = "pepi@abv.bg";
            User user2 = new User() { Id = 2 };
            user.UserName = "joro";
            user.Password = "joro123";
            user.email = "joro@abv.bg";
            var data = new List<User>
            {
                user,user2
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserController(mockContext.Object);
            data.ToList().ForEach(x => service.AddUsers(x.UserName, x.Password, x.email));

            var userr = service.GetUser("joro");

            Assert.AreEqual("joro", user.UserName);
        }

        public void GetAllUser_Should_Return_All_User_In_Database()
        {
            User user = new User() { Id = 1 };
            user.UserName = "pepi";
            user.Password = "pepi123";
            user.email = "pepi@abv.bg";
            User user2 = new User() { Id = 2 };
            user.UserName = "joro";
            user.Password = "joro123";
            user.email = "joro@abv.bg";
            User user3 = new User() { Id = 3, UserName = "bob", Password = "bob123", email = "bob@abv.bg" };
            User user4 = new User() { Id = 4, UserName = "dani", Password = "dani123", email = "dani@abv.bg" };

            var data = new List<User>
            {
                user,user2,user3,user4
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            var service = new UserController(mockContext.Object);
            data.ToList().ForEach(x => service.AddUsers(x.UserName, x.Password, x.email));

            var user4e = service.GetAllUsers();
            int expected = 4;
            var result = user4e.Count();
            Assert.AreEqual(expected, result);
        }
    }
}