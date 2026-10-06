using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Implementations;
using DesignABear.Services.Services.Interfaces;
using Moq;
using Xunit;

namespace DesignABear.Tests
{
    public class RegisterServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IAuthService> _mockAuth;
        private readonly Mock<IEmailService> _mockEmail;
        private readonly RegisterService _registerService;

        public RegisterServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockAuth = new Mock<IAuthService>();
            _mockEmail = new Mock<IEmailService>();
            _registerService = new RegisterService(_mockUow.Object, _mockAuth.Object, _mockEmail.Object);
        }

        [Fact]
        public async Task QuickSignUpAsync_ValidRequest_CreatesActiveUserAndReturnsToken()
        {
            // Arrange
            var request = new QuickRegisterRequest
            {
                Email = "quick@test.com",
                Password = "Password123",
                FullName = "Quick Tester",
                RoleId = 4, // Craftsman
                WorkplaceId = Guid.NewGuid()
            };

            _mockUow.Setup(u => u.Users.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);
            _mockAuth.Setup(a => a.HashPassword(request.Password)).Returns("hashed_pwd");
            _mockAuth.Setup(a => a.GenerateJwtToken(It.IsAny<User>())).Returns("jwt_token");
            _mockUow.Setup(u => u.Users.GetWithRolesAsync(It.IsAny<Guid>())).ReturnsAsync(new User { Email = request.Email });

            // Act
            var (success, message, token) = await _registerService.QuickSignUpAsync(request);

            // Assert
            Assert.True(success);
            Assert.Equal("jwt_token", token);
            _mockUow.Verify(u => u.Users.AddAsync(It.Is<User>(u => 
                u.Status == UserStatus.Active && 
                u.RoleId == 4 && 
                u.WorkplaceId == request.WorkplaceId)), Times.Once);
            _mockUow.Verify(u => u.SaveChangesAsync(), Times.Once);
            
            // Verify Email was NOT sent
            _mockEmail.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task QuickSignUpAsync_AdminRole_ReturnsError()
        {
            // Arrange
            var request = new QuickRegisterRequest
            {
                Email = "admin@test.com",
                Password = "Password123",
                FullName = "Admin Attempt",
                RoleId = 1 // Admin
            };

            _mockUow.Setup(u => u.Users.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);

            // Act
            var (success, message, token) = await _registerService.QuickSignUpAsync(request);

            // Assert
            Assert.False(success);
            Assert.Contains("Cannot create Admin", message);
            _mockUow.Verify(u => u.Users.AddAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
