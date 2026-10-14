using Race_Events2.Controllers;
using Race_Events2.Model;
using Race_Events2.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Race_Events2.Tests
{
    public class WeatherControllerTest
    {
        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            var db = DbContextFactory.Create($"WDb_{Guid.NewGuid()}");
            var controller = new WeatherController(db);

            var result = await controller.GetAll();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_WhenExists_ReturnsOk()
        {
            var db = DbContextFactory.Create($"WDb_{Guid.NewGuid()}");
            var controller = new WeatherController(db);

            var result = await controller.GetById(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Create_WithValidEvent_ReturnsCreated()
        {
            var db = DbContextFactory.Create($"WDb_{Guid.NewGuid()}");
            var controller = new WeatherController(db);

            var result = await controller.Create(new Weather
            {
                EventId = 1,
                Condition = "Sunny",
                Temperature = 25
            });

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task Create_WithUnknownEvent_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"WDb_{Guid.NewGuid()}");
            var controller = new WeatherController(db);

            var result = await controller.Create(new Weather
            {
                EventId = 999,
                Condition = "Sunny",
                Temperature = 25
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Update_WhenExists_ReturnsOk()
        {
            var db = DbContextFactory.Create($"WDb_{Guid.NewGuid()}");
            var controller = new WeatherController(db);

            var result = await controller.Update(1, new Weather
            {
                Condition = "Rainy",
                Temperature = 15
            });

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_WhenExists_ReturnsNoContent()
        {
            var db = DbContextFactory.Create($"WDb_{Guid.NewGuid()}");
            var controller = new WeatherController(db);

            var result = await controller.Delete(1);
            Assert.IsType<NoContentResult>(result);
        }
    }
}