using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Dal.Context;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;
using WebApplication1.Services.Data;

namespace WebApplication1.WebAPI.IntegrationTests.Controllers
{
    [TestClass]
    public class PersonsControllerTests
    {
        [TestMethod]
        public void GetAll_SendRequest_ReplaceService_ShouldReturnOk()
        {
            const int expectedId = 1;
            const int expectedCount = 3;
            Mock<IPersonsData> personsDataMock = null;
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var descriptor = services
                            .SingleOrDefault(s => s.ServiceType == typeof(IPersonsData));
                        services.Remove(descriptor);
                        personsDataMock = new Mock<IPersonsData>();
                        personsDataMock
                            .Setup(_ => _.GetAll())
                            .Returns(
                                Enumerable.Range(1, expectedCount).Select(i => new Person
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

            var response = httpClient.GetAsync("api/persons").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var persons = response.Content.ReadFromJsonAsync(typeof(IEnumerable<Person>)).Result;
            Assert.IsInstanceOfType(persons, typeof(IEnumerable<Person>));
            var personsList = (IEnumerable<Person>) persons;
            Assert.AreEqual(expectedId, personsList.FirstOrDefault().Id);
            Assert.AreEqual(expectedCount, personsList.Count());
            personsDataMock.Verify(_ => _.GetAll());
            personsDataMock.Verify();
        }

        [TestMethod]
        public void GetAll_SendRequest_ReplaceDatabaseContext_ShouldReturnOk()
        {
            const int expectedId = 1;
            const int expectedCount = 3;
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var descriptor = services
                            .SingleOrDefault(_ => _.ServiceType == typeof(DbContextOptions<Application1Context>));
                        services.Remove(descriptor);
                        services.AddDbContext<Application1Context>(options =>
                        {
                            options.UseInMemoryDatabase(nameof(Application1Context));
                        });
                        var descriptorInitializer = services
                            .SingleOrDefault(_ => _.ServiceType == typeof(WebStoreDBInitializer));
                        services.Remove(descriptorInitializer);
                        services.AddTransient(_ => Mock.Of<WebStoreDBInitializer>());
                    });
                });
            var testContext = webHost
                .Services.CreateScope().ServiceProvider.GetService<Application1Context>();
            testContext!.Persons.AddRange(
                Enumerable.Range(1, expectedCount).Select(i => new Person
                {
                    LastName = $"Ivanov_{i}",
                    FirstName = $"Ivan_{i}",
                    Patronymic = $"Ivanovich_{i}",
                }));
            testContext!.SaveChanges();
            HttpClient httpClient = webHost.CreateClient();

            var response = httpClient.GetAsync("api/persons").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var persons = response.Content.ReadFromJsonAsync(typeof(IEnumerable<Person>)).Result;
            Assert.IsInstanceOfType(persons, typeof(IEnumerable<Person>));
            var personsList = (IEnumerable<Person>) persons;
            Assert.AreEqual(expectedId, personsList.FirstOrDefault().Id);
            Assert.AreEqual(expectedCount, personsList.Count());
        }

        [TestMethod]
        public void GetById_SendRequest_ReplaceDatabaseContext_ShouldReturnOk()
        {
            const int expectedId = 1;
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var descriptor = services
                            .SingleOrDefault(_ => _.ServiceType == typeof(DbContextOptions<Application1Context>));
                        services.Remove(descriptor);
                        services.AddDbContext<Application1Context>(options =>
                        {
                            options.UseInMemoryDatabase(nameof(Application1Context));
                        });
                        var descriptorInitializer = services
                            .SingleOrDefault(_ => _.ServiceType == typeof(WebStoreDBInitializer));
                        services.Remove(descriptorInitializer);
                        services.AddTransient(_ => Mock.Of<WebStoreDBInitializer>());
                    });
                });
            var testContext = webHost
                .Services.CreateScope().ServiceProvider.GetService<Application1Context>();
            testContext!.Persons.AddRange(
                Enumerable.Range(1, 3).Select(i => new Person
                {
                    LastName = $"Ivanov_{i}",
                    FirstName = $"Ivan_{i}",
                    Patronymic = $"Ivanovich_{i}",
                }));
            testContext!.SaveChanges();
            HttpClient httpClient = webHost.CreateClient();

            var response = httpClient.GetAsync($"api/persons/{expectedId}").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var person = response.Content.ReadFromJsonAsync(typeof(Person)).Result;
            Assert.IsInstanceOfType(person, typeof(Person));
            var personOne = (Person) person;
            Assert.AreEqual(expectedId, personOne.Id);
        }
    }
}
