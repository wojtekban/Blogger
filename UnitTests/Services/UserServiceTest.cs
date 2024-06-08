using Infrastructure.Identity;
using Xunit;

namespace UnitTests.UnitTests.Services
{
    public class UserServiceTest
    {
        [Fact]
        public void Is_User_Email_Confirmed_When_Email_Confirmed_Is_True_Returns_True()
        {
            // Arrange
            ApplicationUser user = new()
            {
                UserName = "wojtek",
                EmailConfirmed = true
            };

            UserService service = new();
            // Act

            bool isEmailConfirmed = service.IsUserEmailConfirmed(user);

            // Assert
            Assert.True(isEmailConfirmed);
        }

        [Fact]
        public void Is_User_Email_Confirmed_When_Email_Confirmed_Is_False_Returns_False()
        {
            // Arrange
            ApplicationUser user = new()
            {
                UserName = "wojtek",
                EmailConfirmed = false
            };

            UserService service = new();
            // Act

            bool isEmailConfirmed = service.IsUserEmailConfirmed(user);

            // Assert
            Assert.False(isEmailConfirmed);
        }
    }
}