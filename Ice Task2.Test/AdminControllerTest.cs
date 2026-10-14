using Race_Events2.Controllers;
using Race_Events2.Model;
using Race_Events2.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Ice_Task2.Tests
{
    public class AdminControllerTest
    {
        [Fact]
        public async Task GetAllEvents_ReturnsOk()
        {
            var db = DbContextFactory.Create($"AdminDb_{Guid.NewGuid()}");
            var controller = new AdminController(db);

            var result = await controller.GetAllEvents();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetEvent_WhenExists_ReturnsOk()
        {
            var db = DbContextFactory.Create($"AdminDb_{Guid.NewGuid()}");
            var controller = new AdminController(db);

            var result = await controller.GetEvent(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetEvent_WhenMissing_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"AdminDb_{Guid.NewGuid()}");
            var controller = new AdminController(db);

            var result = await controller.GetEvent(999);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateEvent_WithValidData_ReturnsCreated()
        {
            var db = DbContextFactory.Create($"AdminDb_{Guid.NewGuid()}");
            var controller = new AdminController(db);

            var result = await controller.CreateEvent(new EventCreateRequest
            {
                Name = "New Event",
                Description = "Desc",
                Date = DateTime.Now.AddDays(3),
                Location = "Joburg"
            });

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task CreateEvent_WithNull_ReturnsBadRequest()
        {
            var db = DbContextFactory.Create($"AdminDb_{Guid.NewGuid()}");
            var controller = new AdminController(db);

            var result = await controller.CreateEvent(null!);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateEvent_WhenExists_ReturnsOk()
        {
            var db = DbContextFactory.Create($"AdminDb_{Guid.NewGuid()}");
            var controller = new AdminController(db);

            var result = await controller.UpdateEvent(1, new Event
            {
                Name = "Updated",
                Description = "Updated",
                Date = DateTime.Now.AddDays(5),
                Location = "Updated"
            });

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateEvent_WhenMissing_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"AdminDb_{Guid.NewGuid()}");
            var controller = new AdminController(db);

            var result = await controller.UpdateEvent(999, new Event
            {
                Name = "X",
                Description = "X",
                Date = DateTime.Now,
                Location = "X"
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteEvent_WhenExists_ReturnsNoContent()
        {
            var db = DbContextFactory.Create($"AdminDb_{Guid.NewGuid()}");
            var controller = new AdminController(db);

            var result = await controller.DeleteEvent(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEvent_WhenMissing_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"AdminDb_{Guid.NewGuid()}");
            var controller = new AdminController(db);

            var result = await controller.DeleteEvent(999);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
