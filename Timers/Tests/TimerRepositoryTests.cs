//using System;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using TimerAPI.Models;
//using Timers.Models;
//using Xunit;
//namespace Timers.Tests
//{
//    public class TimerRepositoryTests
//    {
//        [Fact]
//        public async Task AddTimerAsync_ReturnsValidId()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<TimerDbContext>()
//                          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                          .Options;

//            using (var dbContext = new TimerDbContext(options))
//            {
//                var repository = new TimerRepository(dbContext);

//                // Act
//                var timer = new TimerModel { Hours = 0, Minutes = 1, Seconds = 30, WebUrl = "http://example.com", ExpiryTime = DateTime.Now.AddSeconds(90) };
//                var timerId = await repository.AddTimerAsync(timer);

//                // Assert
//                Assert.True(timerId > 0); // Verify that a valid ID is returned
//            }
//        }

//        [Fact]
//        public async Task GetTimerByIdAsync_ReturnsValidTimer()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<TimerDbContext>()
//                         .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use the in-memory database
//                         .Options;

//            using (var dbContext = new TimerDbContext(options))
//            {
//                var repository = new TimerRepository(dbContext);

//                // Add a dummy timer to the database for testing
//                var timer = new TimerModel { Hours = 0, Minutes = 1, Seconds = 30, WebUrl = "http://example.com", ExpiryTime = DateTime.Now.AddSeconds(90) };
//                dbContext.Timers.Add(timer);
//                await dbContext.SaveChangesAsync();

//                // Act
//                var retrievedTimer = await repository.GetTimerByIdAsync(timer.Id);

//                // Assert
//                Assert.NotNull(retrievedTimer);
//                Assert.Equal(timer.Id, retrievedTimer.Id);
//            }
//        }

//        [Fact]
//        public async Task GetTimerByIdAsync_ReturnsNullForInvalidId()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<TimerDbContext>()
//                          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                          .Options;

//            using (var dbContext = new TimerDbContext(options))
//            {
//                var repository = new TimerRepository(dbContext);

//                // Act
//                var retrievedTimer = await repository.GetTimerByIdAsync(999);

//                // Assert
//                Assert.Null(retrievedTimer);
//            }
//        }
//    }
//}


