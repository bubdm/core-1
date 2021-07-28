using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;
using WebApplication1.WebAPI.Clients.Persons;

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
                        var delDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IProductData));
                        services.Remove(delDescriptor);
                        services.AddTransient(_ => Mock.Of<IProductData>());
                    });
                });
            var httpClient = webHost.CreateClient();

            var response = httpClient.GetAsync("persons").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            personsDataMock.Verify(_ => _.GetAll());
            personsDataMock.Verify();
        }
        
        [TestMethod]
        public void Index_SendRequest_HttpDriver_ShouldReturnView()
        {
            const int expectedCount = 3;
            var webAPIPersonsControllerDriver = new WebAPI_HttpDriver_PersonsController_GetAll();
            Mock<IPersonsData> personsDataMock = null;
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IPersonsData));
                        services.Remove(descriptor);
                        var mockMessageHandler = new Mock<HttpMessageHandler>();
                        mockMessageHandler.Protected()
                            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                            .ReturnsAsync(() =>
                            {
                                return webAPIPersonsControllerDriver.GetAll(expectedCount);
                            });
                        var client = new PersonsClient(new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localost/") });

                        services.AddTransient<IPersonsData>(_ => client);
                        var delDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IProductData));
                        services.Remove(delDescriptor);
                        services.AddTransient(_ => Mock.Of<IProductData>());
                    });
                });
            var httpClient = webHost.CreateClient();

            var response = httpClient.GetAsync("persons").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(webAPIPersonsControllerDriver.BeCalled);
        }

        /// <summary> Драйвер для контроллера веб апи </summary>
        public class WebAPI_HttpDriver_PersonsController_GetAll
        {
            public bool BeCalled { get; set; } = false;
            public HttpResponseMessage GetAll(int expectedCount = 3)
            {
                WebApplicationFactory<WebAPI.Startup> webHost = new WebApplicationFactory<WebAPI.Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            var descriptor = services
                                .SingleOrDefault(s => s.ServiceType == typeof(IPersonsData));
                            services.Remove(descriptor);
                            var personsDataMock = new Mock<IPersonsData>();
                            personsDataMock
                                .Setup(_ => _.GetAll())
                                .Returns(
                                    () =>
                                    {
                                        BeCalled = true;
                                        return Enumerable.Range(1, expectedCount).Select(i => new Person
                                        {
                                            Id = i,
                                            LastName = "Ivanov",
                                            FirstName = "Ivan",
                                            Patronymic = "Ivanovich",
                                        });
                                    }
                                );
                            services.AddTransient(_ => personsDataMock.Object);
                        });
                    });
                var httpClient = webHost.CreateClient();

                var response = httpClient.GetAsync("api/persons").Result;

                return response;
            }
        }
    }
}
