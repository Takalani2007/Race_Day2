using Race_Events2.Controllers;
using Race_Events2.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Race_Events2.Tests
{
    public class EventControllerTest
    {
        [Fact]
        public async Task GetAll_ReturnsOkWithSeededEvents()
        {
            var db = DbContextFactory.Create($"EventDb_{Guid.NewGuid()}");
            var controller = new EventController(db);

            var result = await controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task GetById_WhenExists_ReturnsOk()
        {
            var db = DbContextFactory.Create($"EventDb_{Guid.NewGuid()}");
            var controller = new EventController(db);

            var result = await controller.GetById(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_WhenMissing_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"EventDb_{Guid.NewGuid()}");
            var controller = new EventController(db);

            var result = await controller.GetById(999);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetUpcoming_ReturnsOk()
        {
            var db = DbContextFactory.Create($"EventDb_{Guid.NewGuid()}");
            var controller = new EventController(db);

            var result = await controller.GetUpcoming();
            Assert.IsType<OkObjectResult>(result);
        }
    }
}