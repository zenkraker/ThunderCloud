using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using ThunderServer.API.Services.Interfaces;

namespace ThunderServer.IntegrationTests
{
    public class ThunderServerFactory : WebApplicationFactory<IFileManager>
    {
        public ThunderServerFactory(IWebHostBuilder webHostBuilder) {

        //webHostBuilder.ConfigureTestContainer<>.Add
        //USE THIS VIDOE TO start integrating testContaine
        //https://www.youtube.com/watch?v=8IRNC7qZBmk

        }


        [Fact]
        public void Test1()
        {

        }
    }
}