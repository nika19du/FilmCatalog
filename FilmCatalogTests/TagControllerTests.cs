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
    public class TagControllerTests
    {
        [TestCase]
        public void Test_Add_Tag_In_Database()
        {
            var data = this.CreateTag();

            var mockSet = new Mock<DbSet<Tag>>();

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(m => m.Tags).Returns(mockSet.Object);

            var service = new TagController(mockContext.Object);

            service.AddTag(data[0]);

            mockSet.Verify(m => m.Add(It.IsAny<Tag>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

        }
        [TestCase]
        public void Get_All_Tags()
        {
            var data = this.CreateTag().AsQueryable();

            var mockSet = new Mock<DbSet<Tag>>();
            mockSet.As<IQueryable<Tag>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Tags).Returns(mockSet.Object);

            var service = new TagController(mockContext.Object);

            data.ToList().ForEach(t => service.AddTag(t));

            var tags = service.GetAllTags();

            var expected = 4;
            var result = tags.Count();

            Assert.AreEqual(expected, result);
        }
        public void Get_All_Tags_Should_Return_Correct_FirstName()
        {
            var data = this.CreateTag().AsQueryable();

            var mockSet = new Mock<DbSet<Tag>>();
            mockSet.As<IQueryable<Tag>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Tags).Returns(mockSet.Object);

            var service = new TagController(mockContext.Object);
            var tags = service.GetAllTags();

            Assert.AreEqual("top", tags[0].Name);
        }
        [TestCase]
        public void GetTagById_Should_Return_Correct_Name()
        {
            var data = this.CreateTag().AsQueryable();

            var mockSet = new Mock<DbSet<Tag>>();
            mockSet.As<IQueryable<Tag>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Tag>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FilmCatalogContext>();
            mockContext.Setup(c => c.Tags).Returns(mockSet.Object);

            var service = new TagController(mockContext.Object);
            var tags = service.GetTag(1);

            var expected = "top";
            var result = tags.Name;

            Assert.AreEqual(expected, result);
        }
        public List<Tag> CreateTag()
        {
            var user = new User() { UserName = "ann", Password = "ann123", email = "ann@abv.bg", Id = 1 };
            var user2 = new User() { UserName = "sisi", Password = "sisi123", email = "sisi@abv.bg", Id = 1 };
            var user3 = new User() { UserName = "sonq", Password = "sonq123", email = "sonq@abv.bg", Id = 1 };
            var user4 = new User() { UserName = "mimi", Password = "mimi123", email = "mimi@abv.bg", Id = 1 };

            var tags = new List<Tag>
            {
                new Tag("top",user){Id=1 },
                new Tag("filmi.com",user2){Id=2 },
                new Tag("filmisub.com",user3){Id=3 },
                new Tag("2020",user4){Id=4}
            };
            return tags;
        }
    }
}
