using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.WebAPI.IntegrationTests.Controllers
{
    [TestClass]
    public class PersonsControllerTests
    {
        [TestMethod]
        public void GetAll_SendRequest_ShouldReturnOk()
        {
            Mock<IPersonsData> personsDataMock = null;
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var personsData = services
                            .SingleOrDefault(s => s.ServiceType == typeof(IPersonsData));
                        services.Remove(personsData);
                        personsDataMock = new Mock<IPersonsData>();
                        personsDataMock
                            .Setup(_ => _.GetAll())
                            .Returns(
                                Enumerable.Range(1, 3).Select(i => new Person
                                {
                                    Id = i,
                                    LastName = "Ivanov",
                                    FirstName = "Ivan",
                                    Patronymic = "Ivanovich",
                                })
                            );
                        services.AddTransient(_ => personsDataMock.Object);
                    });
                });
            HttpClient httpClient = webHost.CreateClient();

            var response = httpClient.GetAsync("api/persons").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            personsDataMock.Verify(_ => _.GetAll());
            personsDataMock.Verify();
        }



    }
}
