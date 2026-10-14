using Ice_Task2.Controllers;
using Ice_Task2.Model;
using Ice_Task2.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Ice_Task2.Tests
{
    public class FeedbackControllerTest
    {
        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            var db = DbContextFactory.Create($"FbDb_{Guid.NewGuid()}");
            var controller = new FeedbackController(db);

            var result = await controller.GetAll();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Submit_WithValidData_ReturnsCreated()
        {
            var db = DbContextFactory.Create($"FbDb_{Guid.NewGuid()}");
            var controller = new FeedbackController(db);

            var result = await controller.Submit(1, new FeedbackRequest
            {
                UserId = 2,
                Comment = "Great event!",
                Rating = 5
            });

            Assert.IsType<CreatedAtActionResult>(result);
            Assert.Single(db.Feedbacks);
        }

        [Fact]
        public async Task Submit_WithUnknownEvent_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"FbDb_{Guid.NewGuid()}");
            var controller = new FeedbackController(db);

            var result = await controller.Submit(999, new FeedbackRequest
            {
                UserId = 2,
                Comment = "Nice",
                Rating = 4
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Submit_WithUnknownUser_ReturnsNotFound()
        {
            var db = DbContextFactory.Create($"FbDb_{Guid.NewGuid()}");
            var controller = new FeedbackController(db);

            var result = await controller.Submit(1, new FeedbackRequest
            {
                UserId = 999,
                Comment = "Nice",
                Rating = 4
            });

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Submit_WithInvalidRating_ReturnsUnprocessableEntity()
        {
            var db = DbContextFactory.Create($"FbDb_{Guid.NewGuid()}");
            var controller = new FeedbackController(db);

            var result = await controller.Submit(1, new FeedbackRequest
            {
                UserId = 2,
                Comment = "Bad",
                Rating = 10
            });

            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [Fact]
        public async Task Delete_WhenExists_ReturnsNoContent()
        {
            var db = DbContextFactory.Create($"FbDb_{Guid.NewGuid()}");
            db.Feedbacks.Add(new Feedback
            {
                FeedbackId = 1,
                EventId = 1,
                UserId = 2,
                Comment = "Test",
                Rating = 4
            });
            await db.SaveChangesAsync();

            var controller = new FeedbackController(db);
            var result = await controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}