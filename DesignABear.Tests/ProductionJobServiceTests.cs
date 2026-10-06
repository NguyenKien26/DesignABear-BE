using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.Services.Implementations;
using DesignABear.Services.Services.Interfaces;
using Moq;
using Xunit;

namespace DesignABear.Tests
{
    public class ProductionJobServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly ProductionJobService _jobService;

        public ProductionJobServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockEmailService = new Mock<IEmailService>();
            _jobService = new ProductionJobService(_mockUow.Object, _mockMapper.Object, _mockEmailService.Object);
        }

        [Fact]
        public async Task AssignTechnicianAsync_AssignsSingleTechnicianToAllParts()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var technicianId = Guid.NewGuid();
            var job = new ProductionJob { JobId = jobId };

            _mockUow.Setup(u => u.ProductionJobs.GetByIdAsync(jobId))
                .ReturnsAsync(job);

            // Act
            await _jobService.AssignTechnicianAsync(jobId, technicianId);

            // Assert
            Assert.Equal(technicianId, job.TechnicianId);
            Assert.Equal(technicianId, job.EspTechnicianId);
            Assert.Equal(technicianId, job.ShellTechnicianId);
            Assert.Equal(ProductionStatus.Assigned, job.Status);
            _mockUow.Verify(u => u.ProductionJobs.Update(job), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SubmitInspectionAsync_WhenApproved_SetsFinishedAndTriggersShipment()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var qcUserId = Guid.NewGuid();
            var orderItemId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            
            var job = new ProductionJob { JobId = jobId, OrderItemId = orderItemId };
            var orderItem = new OrderItem { OrderItemId = orderItemId, OrderId = orderId };
            var user = new User { UserId = userId, Email = "test@example.com", FullName = "Test User" };
            var order = new Order { OrderId = orderId, User = user };

            _mockUow.Setup(u => u.ProductionJobs.GetByIdAsync(jobId)).ReturnsAsync(job);
            _mockUow.Setup(u => u.OrderItems.GetByIdAsync(orderItemId)).ReturnsAsync(orderItem);
            _mockUow.Setup(u => u.Orders.GetWithDetailsAsync(orderId)).ReturnsAsync(order);

            // Act
            await _jobService.SubmitInspectionAsync(jobId, qcUserId, 95, "Looks great!", true);

            // Assert
            Assert.Equal(ProductionStatus.Finished, job.Status);
            Assert.Equal(ProductionStatus.Completed, job.QCStatus);
            Assert.Equal(95, job.QualityScore);
            
            // Verify Email was called
            _mockEmailService.Verify(e => e.SendEmailAsync(
                It.Is<string>(to => to == "test@example.com"),
                It.IsAny<string>(),
                It.Is<string>(body => body.Contains("GHTK-DEMO")), 
                true), Times.Once);

            _mockUow.Verify(u => u.ProductionJobs.Update(job), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SubmitInspectionAsync_WhenRejected_SetsReworkAndResetsParts()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var qcUserId = Guid.NewGuid();
            var job = new ProductionJob { JobId = jobId };

            _mockUow.Setup(u => u.ProductionJobs.GetByIdAsync(jobId)).ReturnsAsync(job);

            // Act
            await _jobService.SubmitInspectionAsync(jobId, qcUserId, 40, "Bad stitching", false);

            // Assert
            Assert.Equal(ProductionStatus.ReworkRequired, job.Status);
            Assert.Equal(ProductionStatus.ReworkRequired, job.QCStatus);
            Assert.Equal(ProductionStatus.InProgress, job.EspStatus);
            Assert.Equal(ProductionStatus.InProgress, job.ShellStatus);
            Assert.Equal("Bad stitching", job.RejectionReason);

            _mockUow.Verify(u => u.ProductionJobs.Update(job), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
