using cotr.backend.Model;
using cotr.backend.Model.Request;
using cotr.backend.Model.Tables;
using cotr.backend.Repository.User;
using cotr.backend.Service.Encrypt;
using cotr.backend.Service.User;
using cotr.backend.test.Moq;
using Moq;

namespace cotr.backend.test.Services
{
    public class TestUserService
    {
        [Fact]
        public async Task ValidateUserAsyncShouldReturnUserData()
        {
            //Arrange
            LoginRequest request = UserMockData.LoginRequest;
            Users users = UserMockData.Users;
            UserCredential credential = UserMockData.UserCredential;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByNicknameOrEmailAsync(request.User)).ReturnsAsync(users);
            userRepostory.Setup(x => x.GetUserCredentialByIdAsync(users.UserId)).ReturnsAsync(credential);

            Mock<IEncryptService> encryptService = new();
            encryptService.Setup(x => x.ValidatePassword(request.Password, credential.HashedPassword)).Returns(true);

            //Act
            UserService userService = new(userRepostory.Object, encryptService.Object);

            Users res = await userService.ValidateUserAsync(request);

            //Assert
            Assert.Equal(res, users);
        }

        [Fact]
        public async Task ValidateUserAsyncShouldApiExeption401Credenciales()
        {
            //Arrange
            LoginRequest request = UserMockData.LoginRequest;
            Users users = UserMockData.Users;
            UserCredential credential = UserMockData.UserCredential;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByNicknameOrEmailAsync(request.User)).ReturnsAsync(users);
            userRepostory.Setup(x => x.GetUserCredentialByIdAsync(users.UserId)).ReturnsAsync(credential);

            Mock<IEncryptService> encryptService = new();
            encryptService.Setup(x => x.ValidatePassword(request.Password, credential.HashedPassword)).Returns(false);

            //Act
            UserService userService = new(userRepostory.Object, encryptService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async() => await userService.ValidateUserAsync(request));
        }

        [Fact]
        public async Task ValidateUserAsyncShouldApiExeption401Inactive()
        {
            //Arrange
            LoginRequest request = UserMockData.LoginRequest;
            Users users = UserMockData.Users;
            UserCredential credential = UserMockData.InactiveUserCredential;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByNicknameOrEmailAsync(request.User)).ReturnsAsync(users);
            userRepostory.Setup(x => x.GetUserCredentialByIdAsync(users.UserId)).ReturnsAsync(credential);

            Mock<IEncryptService> encryptService = new();

            //Act
            UserService userService = new(userRepostory.Object, encryptService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.ValidateUserAsync(request));
        }

        [Fact]
        public async Task ValidateUserAsyncShouldApiExeption404User()
        {
            //Arrange
            LoginRequest request = UserMockData.LoginRequest;
            Users users = UserMockData.Users;
            UserCredential credential = UserMockData.InactiveUserCredential;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByNicknameOrEmailAsync(request.User)).ThrowsAsync(ApiExceptionMockData.ApiException404);

            Mock<IEncryptService> encryptService = new();

            //Act
            UserService userService = new(userRepostory.Object, encryptService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.ValidateUserAsync(request));
        }

        [Fact]
        public async Task ValidateUserAsyncShouldApiExeption404Credential()
        {
            //Arrange
            LoginRequest request = UserMockData.LoginRequest;
            Users users = UserMockData.Users;
            UserCredential credential = UserMockData.InactiveUserCredential;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByNicknameOrEmailAsync(request.User)).ReturnsAsync(users);
            userRepostory.Setup(x => x.GetUserCredentialByIdAsync(users.UserId)).ThrowsAsync(ApiExceptionMockData.ApiException404);

            Mock<IEncryptService> encryptService = new();

            //Act
            UserService userService = new(userRepostory.Object, encryptService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.ValidateUserAsync(request));
        }

        [Fact]
        public async Task SignupUserAsyncShouldVoid()
        {
            //Arrange
            SignupRequest request = UserMockData.SignupRequest;
            Users users = UserMockData.Users;

            Mock<IUserRepostory> userRepostory = new();
            #pragma warning disable CS8600
            userRepostory.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync((Users)null);
            userRepostory.Setup(x => x.GetUserByNicknameAsync(request.Nickname)).ReturnsAsync((Users)null);
            #pragma warning restore CS8600
            userRepostory.Setup(x => x.SaveNewUserAsync(It.IsAny<Users>())).ReturnsAsync(users);

            Mock<IEncryptService> encryptService = new();
            encryptService.Setup(x => x.GenerateSalt()).Returns(UserMockData.Salt);
            encryptService.Setup(x => x.EncryptPassword(request.Password, UserMockData.Salt)).Returns(UserMockData.HashedPassword);

            //Act
            UserService userService = new(userRepostory.Object, encryptService.Object);

            await userService.SignupUserAsync(request);
        }

        [Fact]
        public async Task SignupUserAsyncShouldExption409Email()
        {
            //Arrange
            SignupRequest request = UserMockData.SignupRequest;
            Users users = UserMockData.Users;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync(users);

            Mock<IEncryptService> encryptService = new();

            //Act
            UserService userService = new(userRepostory.Object, encryptService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.SignupUserAsync(request));
        }

        [Fact]
        public async Task SignupUserAsyncShouldExption409NickName()
        {
            //Arrange
            SignupRequest request = UserMockData.SignupRequest;
            Users users = UserMockData.Users;

            Mock<IUserRepostory> userRepostory = new();
            #pragma warning disable CS8600
            userRepostory.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync((Users)null);
            #pragma warning restore CS8600
            userRepostory.Setup(x => x.GetUserByNicknameAsync(request.Nickname)).ReturnsAsync(users);

            Mock<IEncryptService> encryptService = new();

            //Act
            UserService userService = new(userRepostory.Object, encryptService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.SignupUserAsync(request));
        }
    }
}
