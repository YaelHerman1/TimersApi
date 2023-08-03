using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Timers.Contract;
using Timers.Controllers;
using Timers.Models;
using Timers.Persistence;
using Timers.Services;
using Xunit;

namespace Timers.Tests
{
    public class TimerControllerTests
    {
        private readonly TimerController _controller;
        private readonly Mock<TimerRepository> _mockRepository;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;

        public TimerControllerTests()
        {
            _mockRepository = new Mock<TimerRepository>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _controller = new TimerController(_mockRepository.Object, _mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task SetTimer_ValidInput_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var timerInput = new TimerInputModel
            {
                Hours = 1,
                Minutes = 30,
                Seconds = 0,
                WebUrl = "http://example.com/webhook"
            };
            int timerId = 1;
            _mockRepository.Setup(repo => repo.AddTimerAsync(It.IsAny<TimerModel>()))
                           .ReturnsAsync(timerId);

            // Act
            var result = await _controller.SetTimer(timerInput);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(timerId, createdAtRouteResult.Value);
        }

        [Fact]
        public async Task GetTimerStatus_ExistingTimerId_ReturnsOkResultWithSecondsLeft()
        {
            // Arrange
            int timerId = 1;
            var timer = new TimerModel
            {
                Id = timerId,
                ExpiryTime = DateTime.Now.AddSeconds(60) // Timer will expire in 60 seconds
            };
            _mockRepository.Setup(repo => repo.GetTimerByIdAsync(timerId))
                           .ReturnsAsync(timer);

            // Act
            var result = await _controller.GetTimerStatus(timerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Dictionary<string, int>>(okResult.Value);
            Assert.True(value.ContainsKey("secondsLeft"));
            Assert.Equal(60, value["secondsLeft"]);
        }

        [Fact]
        public async Task GetTimerStatus_NonExistingTimerId_ReturnsNotFoundResult()
        {
            // Arrange
            int timerId = 1;
            _mockRepository.Setup(repo => repo.GetTimerByIdAsync(timerId))
                           .ReturnsAsync((TimerModel)null);

            // Act
            var result = await _controller.GetTimerStatus(timerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

    public class TimerRepositoryTests
    {
        private readonly TimerRepository _repository;
        private readonly Mock<TimerDbContext> _mockDbContext;

        public TimerRepositoryTests()
        {
            _mockDbContext = new Mock<TimerDbContext>();
            _repository = new TimerRepository(_mockDbContext.Object);
        }

        //[Fact]
        //public async Task AddTimerAsync_ValidInput_ReturnsTimerId()
        //{
        //    // Arrange
        //    var timer = new TimerModel
        //    {
        //        Hours = 1,
        //        Minutes = 30,
        //        Seconds = 0,
        //        WebUrl = "http://example.com/webhook",
        //        ExpiryTime = DateTime.Now.AddHours(1).AddMinutes(30) // Timer will expire in 1 hour and 30 minutes
        //    };
        //    int expectedId = 1;
        //    _mockDbContext.Setup(db => db.Timers.AddAsync(It.IsAny<TimerModel>(), default))
        //                  .Callback((TimerModel t, CancellationToken token) =>
        //                  {
        //                      t.Id = expectedId;
        //                  })
        //                  .Returns(Task.FromResult<TimerModel>(null));

        //    // Act
        //    int result = await _repository.AddTimerAsync(timer);

        //    // Assert
        //    Assert.Equal(expectedId, result);
        //}

        [Fact]
        public async Task GetTimerByIdAsync_ExistingTimerId_ReturnsTimerModel()
        {
            // Arrange
            int timerId = 1;
            var timer = new TimerModel
            {
                Id = timerId,
                ExpiryTime = DateTime.Now.AddSeconds(60) // Timer will expire in 60 seconds
            };
            _mockDbContext.Setup(db => db.Timers.FindAsync(timerId))
                          .ReturnsAsync(timer);

            // Act
            var result = await _repository.GetTimerByIdAsync(timerId);

            // Assert
            Assert.Equal(timer, result);
        }

        [Fact]
        public async Task GetTimerByIdAsync_NonExistingTimerId_ReturnsNull()
        {
            // Arrange
            int timerId = 1;
            _mockDbContext.Setup(db => db.Timers.FindAsync(timerId))
                          .ReturnsAsync((TimerModel)null);

            // Act
            var result = await _repository.GetTimerByIdAsync(timerId);

            // Assert
            Assert.Null(result);
        }
    }
}


