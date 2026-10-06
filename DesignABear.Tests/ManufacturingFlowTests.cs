using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.Services.Implementations;
using DesignABear.Services.Services.Interfaces;
using Moq;
using System.Text.Json;
using Xunit;

namespace DesignABear.Tests
{
    public class ManufacturingFlowTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly ProductionJobService _jobService;

        public ManufacturingFlowTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockEmailService = new Mock<IEmailService>();
            _jobService = new ProductionJobService(_mockUow.Object, _mockMapper.Object, _mockEmailService.Object);
        }

        [Fact]
        public async Task SyncOrderStatus_WhenAllJobsFinished_SetsReadyForPickup()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderItemId1 = Guid.NewGuid();
            var orderItemId2 = Guid.NewGuid();
            var jobId1 = Guid.NewGuid();
            var jobId2 = Guid.NewGuid();

            var order = new Order { OrderId = orderId, Status = "PROCESSING" };
            var item1 = new OrderItem { OrderItemId = orderItemId1, OrderId = orderId };
            var item2 = new OrderItem { OrderItemId = orderItemId2, OrderId = orderId };
            
            var job1 = new ProductionJob { JobId = jobId1, OrderItemId = orderItemId1, Status = ProductionStatus.Finished };
            var job2 = new ProductionJob { JobId = jobId2, OrderItemId = orderItemId2, Status = ProductionStatus.InQueue };

            item1.ProductionJobs = new List<ProductionJob> { job1 };
            item2.ProductionJobs = new List<ProductionJob> { job2 };
            order.OrderItems = new List<OrderItem> { item1, item2 };

            _mockUow.Setup(u => u.OrderItems.GetByIdAsync(orderItemId2)).ReturnsAsync(item2);
            _mockUow.Setup(u => u.Orders.GetWithDetailsAsync(orderId)).ReturnsAsync(order);
            _mockUow.Setup(u => u.ProductionJobs.GetByIdAsync(jobId2)).ReturnsAsync(job2);
            _mockUow.Setup(u => u.Users.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User());

            // Act: Finish the 2nd job
            await _jobService.SubmitInspectionAsync(jobId2, Guid.NewGuid(), 100, "Perfect", true);

            // Assert
            Assert.Equal(ProductionStatus.Finished, job2.Status);
            Assert.Equal("READY_FOR_PICKUP", order.Status);
            _mockUow.Verify(u => u.Orders.Update(order), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Commission_CreditCorrectAmountToUsers()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var craftsmanId = Guid.NewGuid();
            var qcId = Guid.NewGuid();
            var baseCost = 100000m;

            var job = new ProductionJob 
            { 
                JobId = jobId, 
                TechnicianId = craftsmanId,
                TotalAssemblyCost = baseCost,
                CraftsmanCommission = baseCost * 0.7m,
                QCCommission = baseCost * 0.3m,
                OrderItemId = Guid.NewGuid()
            };

            var craftsman = new User { UserId = craftsmanId, TotalJobsCompleted = 10, TotalEarned = 500000 };
            var qcInspector = new User { UserId = qcId, TotalJobsCompleted = 5, TotalEarned = 200000 };

            var item = new OrderItem { OrderItemId = job.OrderItemId, OrderId = Guid.NewGuid() };
            var order = new Order { OrderId = item.OrderId, OrderItems = new List<OrderItem> { item } };
            item.ProductionJobs = new List<ProductionJob> { job };

            _mockUow.Setup(u => u.ProductionJobs.GetByIdAsync(jobId)).ReturnsAsync(job);
            _mockUow.Setup(u => u.Users.GetByIdAsync(craftsmanId)).ReturnsAsync(craftsman);
            _mockUow.Setup(u => u.Users.GetByIdAsync(qcId)).ReturnsAsync(qcInspector);
            _mockUow.Setup(u => u.OrderItems.GetByIdAsync(job.OrderItemId)).ReturnsAsync(item);
            _mockUow.Setup(u => u.Orders.GetWithDetailsAsync(item.OrderId)).ReturnsAsync(order);

            // Act
            await _jobService.SubmitInspectionAsync(jobId, qcId, 90, "Good", true);

            // Assert
            Assert.Equal(11, craftsman.TotalJobsCompleted);
            Assert.Equal(570000, craftsman.TotalEarned);
            Assert.Equal(6, qcInspector.TotalJobsCompleted);
            Assert.Equal(230000, qcInspector.TotalEarned);
        }

        [Fact]
        public async Task ReworkCycle_ProgressesAutomaticallyAfterUpdate()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var job = new ProductionJob 
            { 
                JobId = jobId, 
                Status = ProductionStatus.ReworkRequired, 
                QCStatus = ProductionStatus.ReworkRequired,
                HasSmartChip = false,
                EspStatus = ProductionStatus.Completed 
            };
            
            _mockUow.Setup(u => u.ProductionJobs.GetByIdAsync(jobId)).ReturnsAsync(job);

            // Act: Handover again after fix
            await _jobService.HandoverPartAsync(jobId, "SHELL", new List<string> { "fix.jpg" }, "Fixed stitching", Guid.NewGuid());

            // Assert
            Assert.Equal(ProductionStatus.AwaitingQC, job.Status);
            var evidence = JsonSerializer.Deserialize<List<JsonElement>>(job.EvidenceImages!);
            Assert.Contains(evidence, e => e.GetProperty("tag").GetString() == "FINAL_PRODUCT");
        }
    }
}
