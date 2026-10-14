using Race_Events2.Controllers;
using Race_Events2.Data;
using Race_Events2.Model;
using Race_Events2.Services;
using Race_Events2.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Race_Events2.Tests
{
    public class AuthControllerTest
    {
        private static AuthController CreateController(out AppDbContext db)
        {
            db = DbContextFactory.Create($"AuthDb_{Guid.NewGuid()}");
            var service = new UserService(db);
            return new AuthController(service);
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsOk()
        {
            var controller = CreateController(out var db);

            var user = new User
            {
                FullName = "Test User",
                Email = "new@mail.com",
                Password = "pass123"
            };

            var result = await controller.Register(user);

            Assert.IsType<OkObjectResult>(result);
            Assert.True(db.Users.Any(u => u.Email == "new@mail.com"));
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ReturnsConflict()
        {
            var controller = CreateController(out _);

            // "admin@mail.com" is already seeded
            var user = new User
            {
                FullName = "Duplicate",
                Email = "admin@mail.com",
                Password = "pass123"
            };

            var result = await controller.Register(user);
            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task Register_WithNullUser_ReturnsBadRequest()
        {
            var controller = CreateController(out _);
            var result = await controller.Register(null!);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOk()
        {
            var controller = CreateController(out _);

            var result = await controller.Login(new LoginRequest
            {
                Email = "admin@mail.com",
                Password = "admin123"
            });

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithWrongPassword_ReturnsUnauthorized()
        {
            var controller = CreateController(out _);

            var result = await controller.Login(new LoginRequest
            {
                Email = "admin@mail.com",
                Password = "wrong"
            });

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}