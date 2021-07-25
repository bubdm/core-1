using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Tests.Controllers
{
    [TestClass]
    public class PersonsControllerTests
    {
        [TestMethod]
        public void GetAll_Invoke_ReturnView()
        {
            const int expectedId = 1;
            var personsDataMock = new Mock<IPersonsData>();
            personsDataMock.Setup(d => d.GetAll()).Returns(() =>
            {
                return new[]
                {
                    new Person
                    {
                        Id = expectedId,
                    },
                    new Person(),
                    new Person(),
                };
            });
            var controller = new PersonsController(personsDataMock.Object);

            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(IEnumerable<Person>));
            var persons = (IEnumerable<Person>) viewResult.Model;
            Assert.AreEqual(expectedId, persons.FirstOrDefault().Id);
        }
    }
}
