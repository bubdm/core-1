using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApplication1.WebAPI.IntegrationTests.Controllers
{
    [TestClass]
    public class ValuesControllerTests
    {
        [TestMethod]
        public void Get_SendRequest_ShouldReturnOk()
        {
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => {});
            HttpClient httpClient = webHost.CreateClient();

            var response = httpClient.GetAsync("api/values").Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
