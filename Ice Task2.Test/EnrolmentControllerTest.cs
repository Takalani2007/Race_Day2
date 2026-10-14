using Ice_Task2.Controllers;
using Ice_Task2.Model;
using Ice_Task2.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ice_Task2.Tests
{
    public class EnrolmentsControllerTest
    {
        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            var db = DbContextFactory.Create($"EnrDb_{Guid.NewGuid()}");
            var controller = new EnrolmentsController(db);

            var result = await controller.GetAll();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Enrol_WithValidData_ReturnsCreated()
        {
            var db = DbContextFactory.Create($"EnrDb_{Guid.NewGuid()}");
            var controller = new EnrolmentsController(db);

            // UserId = 2 (John), EventId = 1 (seeded)
            var result = await controller.Enrol(new Enrolment
            {
                UserId = 2,
                EventId = 1
            });

            Assert.IsType<CreatedAtActionResult>(result);
            Assert.Single(db.Enrolments);
        }

        [Fact]
        public async Task Enrol_WithDuplicate_ReturnsConflict()
        {
            var db = DbContextFactory.Create($"EnrDb_{Guid.NewGuid()}");
            db.Enrolments.Add(new Enrolment { UserId = 2, EventId = 1 });
            await db.SaveChangesAsync();

            var controller = new EnrolmentsController(db);
            var result = await controller.Enrol(new Enrolment
            {
                UserId = 2,
                EventId = 1
            });

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task Enrol_WithUnknownEvent_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"EnrDb_{Guid.NewGuid()}");
            var controller = new EnrolmentsController(db);

            var result = await controller.Enrol(new Enrolment
            {
                UserId = 2,
                EventId = 999
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Enrol_WithUnknownUser_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"EnrDb_{Guid.NewGuid()}");
            var controller = new EnrolmentsController(db);

            var result = await controller.Enrol(new Enrolment
            {
                UserId = 999,
                EventId = 1
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Cancel_WhenExists_ReturnsNoContent()
        {
            var db = DbContextFactory.Create($"EnrDb_{Guid.NewGuid()}");
            db.Enrolments.Add(new Enrolment { EnrolmentId = 1, UserId = 2, EventId = 1 });
            await db.SaveChangesAsync();

            var controller = new EnrolmentsController(db);
            var result = await controller.Cancel(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(db.Enrolments);
        }

        [Fact]
        public async Task Cancel_WhenMissing_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"EnrDb_{Guid.NewGuid()}");
            var controller = new EnrolmentsController(db);

            var result = await controller.Cancel(999);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}