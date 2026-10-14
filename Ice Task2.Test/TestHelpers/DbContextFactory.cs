using Ice_Task2.Data;
using Microsoft.EntityFrameworkCore;

namespace Ice_Task2.Tests.TestHelpers
{
    public static class DbContextFactory
    {
        public static AppDbContext Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var db = new AppDbContext(options);
            db.Database.EnsureCreated();
            return db;
        }
    }
}