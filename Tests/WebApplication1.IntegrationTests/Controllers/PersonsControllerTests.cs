using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.IntegrationTests.Controllers
{
    [TestClass]
    public class PersonsControllerTests
    {
        [TestMethod]
        public void Index_SendRequest_ReplaceService_ShouldReturnView()
        {
            Mock<IPersonsData> personsDataMock = null;
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IPersonsData));
                        services.Remove(descriptor);
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
            var httpClient = webHost.CreateClient();

            var response = httpClient.GetAsync("persons").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            personsDataMock.Verify(_ => _.GetAll());
            personsDataMock.Verify();
        }
    }
}
