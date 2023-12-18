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
            Users users = UserMockData.UserValidated;
            UserCredential credential = UserMockData.UserCredential;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByNicknameOrEmailAsync(request.User)).ReturnsAsync(users);
            userRepostory.Setup(x => x.GetUserCredentialByIdAsync(users.UserId)).ReturnsAsync(credential);

            Mock<ISecutiryService> securityService = new();
            securityService.Setup(x => x.ValidatePassword(request.Password, credential.HashedPassword)).Returns(true);

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            Users res = await userService.ValidateUserAsync(request);

            //Assert
            Assert.Equal(res, users);
        }

        [Fact]
        public async Task ValidateUserAsyncShouldApiExeption401Credenciales()
        {
            //Arrange
            LoginRequest request = UserMockData.LoginRequest;
            Users users = UserMockData.UserValidated;
            UserCredential credential = UserMockData.UserCredential;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByNicknameOrEmailAsync(request.User)).ReturnsAsync(users);
            userRepostory.Setup(x => x.GetUserCredentialByIdAsync(users.UserId)).ReturnsAsync(credential);

            Mock<ISecutiryService> securityService = new();
            securityService.Setup(x => x.ValidatePassword(request.Password, credential.HashedPassword)).Returns(false);

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async() => await userService.ValidateUserAsync(request));
        }

        [Fact]
        public async Task ValidateUserAsyncShouldApiExeption401Inactive()
        {
            //Arrange
            LoginRequest request = UserMockData.LoginRequest;
            Users users = UserMockData.UserValidated;
            UserCredential credential = UserMockData.InactiveUserCredential;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByNicknameOrEmailAsync(request.User)).ReturnsAsync(users);
            userRepostory.Setup(x => x.GetUserCredentialByIdAsync(users.UserId)).ReturnsAsync(credential);

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.ValidateUserAsync(request));
        }

        [Fact]
        public async Task ValidateUserAsyncShouldApiExeption401EmailNotVerified()
        {
            //Arrange
            LoginRequest request = UserMockData.LoginRequest;
            Users users = UserMockData.Users;
            UserCredential credential = UserMockData.UserCredential;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByNicknameOrEmailAsync(request.User)).ReturnsAsync(users);
            userRepostory.Setup(x => x.GetUserCredentialByIdAsync(users.UserId)).ReturnsAsync(credential);

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

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

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

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

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.ValidateUserAsync(request));
        }

        [Fact]
        public async Task SignupUserAsyncShouldEmailMessage()
        {
            //Arrange
            SignupRequest request = UserMockData.SignupRequest;
            Users users = UserMockData.Users;

            Mock<IUserRepostory> userRepostory = new();
            #pragma warning disable CS8600
            userRepostory.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync((Users)null);
            userRepostory.Setup(x => x.GetUserByNicknameAsync(request.Nickname)).ReturnsAsync((Users)null);
            userRepostory.Setup(x => x.GetUserByEmailToken(It.IsAny<string>())).ReturnsAsync((Users)null);
            #pragma warning restore CS8600
            userRepostory.Setup(x => x.SaveNewUserAsync(It.IsAny<Users>())).ReturnsAsync(users);

            Mock<ISecutiryService> securityService = new();
            securityService.Setup(x => x.EncryptPassword(request.Password)).Returns(UserMockData.HashedPassword);
            securityService.Setup(x => x.RandomToken()).Returns("1234");

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            EmailMessage res = await userService.SignupUserAsync(request);

            Assert.Equal(res, UserMockData.SignupEmail);
        }

        [Fact]
        public async Task SignupUserAsyncShouldExption409Email()
        {
            //Arrange
            SignupRequest request = UserMockData.SignupRequest;
            Users users = UserMockData.Users;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync(users);

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

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

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.SignupUserAsync(request));
        }

        [Fact]
        public async Task SignupUserAsyncShouldExption409Birthdate()
        {
            //Arrange
            SignupRequest request = UserMockData.SignupRequest;
            request.Birthdate = DateTime.Now;

            Mock<IUserRepostory> userRepostory = new();
            #pragma warning disable CS8600
            userRepostory.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync((Users)null);
            userRepostory.Setup(x => x.GetUserByNicknameAsync(request.Nickname)).ReturnsAsync((Users)null);
            #pragma warning restore CS8600

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.SignupUserAsync(request));
        }

        [Fact]
        public async Task SignupUserAsyncShouldExption409Password()
        {
            //Arrange
            SignupRequest request = UserMockData.SignupRequestBadPassword;

            Mock<IUserRepostory> userRepostory = new();
            #pragma warning disable CS8600
            userRepostory.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync((Users)null);
            userRepostory.Setup(x => x.GetUserByNicknameAsync(request.Nickname)).ReturnsAsync((Users)null);
            #pragma warning restore CS8600

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.SignupUserAsync(request));
        }

        [Fact]
        public async Task SignupUserAsyncShouldExption500SaveUser()
        {
            //Arrange
            SignupRequest request = UserMockData.SignupRequest;
            request.Birthdate = DateTime.Now;

            Mock<IUserRepostory> userRepostory = new();
            #pragma warning disable CS8600
            userRepostory.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync((Users)null);
            userRepostory.Setup(x => x.GetUserByNicknameAsync(request.Nickname)).ReturnsAsync((Users)null);
            #pragma warning restore CS8600
            userRepostory.Setup(x => x.SaveNewUserAsync(It.IsAny<Users>())).ThrowsAsync(ApiExceptionMockData.ApiException500);

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.SignupUserAsync(request));
        }

        [Fact]
        public async Task SignupUserAsyncShouldExption500SaveCredential()
        {
            //Arrange
            SignupRequest request = UserMockData.SignupRequest;
            request.Birthdate = DateTime.Now;
            Users users = UserMockData.Users;
            string hashedPassword = UserMockData.HashedPassword;

            Mock<IUserRepostory> userRepostory = new();
            #pragma warning disable CS8600
            userRepostory.Setup(x => x.GetUserByEmailAsync(request.Email)).ReturnsAsync((Users)null);
            userRepostory.Setup(x => x.GetUserByNicknameAsync(request.Nickname)).ReturnsAsync((Users)null);
            #pragma warning restore CS8600
            userRepostory.Setup(x => x.SaveNewUserAsync(It.IsAny<Users>())).ReturnsAsync(users);
            userRepostory.Setup(x => x.SaveNewCredentialAsync(It.IsAny<UserCredential>())).ThrowsAsync(ApiExceptionMockData.ApiException500);

            Mock<ISecutiryService> securityService = new();
            securityService.Setup(x => x.EncryptPassword(request.Password)).Returns(hashedPassword);

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.SignupUserAsync(request));
        }

        [Fact]
        public async Task UpdatePasswordAsyncShouldVoid()
        {
            //Arrange
            UpdatePasswordRequest request = UserMockData.UpdatePasswordRequest;
            UserCredential credential = UserMockData.UserCredentialRecoveryPassword;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserCredentialByResetToken(request.Token)).ReturnsAsync(credential);

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            await userService.UpdatePasswordAsync(request);
        }

        [Fact]
        public async Task UpdatePasswordAsyncShouldApiException404()
        {
            //Arrange
            UpdatePasswordRequest request = UserMockData.UpdatePasswordRequest;
            UserCredential credential = UserMockData.UserCredentialRecoveryPassword;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserCredentialByResetToken(request.Token)).ThrowsAsync(ApiExceptionMockData.ApiException404);

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.UpdatePasswordAsync(request));
        }

        [Fact]
        public async Task UpdatePasswordAsyncShouldApiException401()
        {
            //Arrange
            UpdatePasswordRequest request = UserMockData.UpdatePasswordRequest;
            UserCredential credential = UserMockData.UserCredentialRecoveryPasswordBadTokenExpiration;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserCredentialByResetToken(request.Token)).ReturnsAsync(credential);

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.UpdatePasswordAsync(request));
        }

        [Fact]
        public async Task UpdatePasswordAsyncShouldApiException409()
        {
            //Arrange
            UpdatePasswordRequest request = UserMockData.UpdatePasswordRequestBadPassword;
            UserCredential credential = UserMockData.UserCredentialRecoveryPassword;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserCredentialByResetToken(request.Token)).ReturnsAsync(credential);

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.UpdatePasswordAsync(request));
        }

        [Fact]
        public async Task UpdatePasswordAsyncShouldApiException500()
        {
            //Arrange
            UpdatePasswordRequest request = UserMockData.UpdatePasswordRequest;
            UserCredential credential = UserMockData.UserCredentialRecoveryPassword;

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserCredentialByResetToken(request.Token)).ReturnsAsync(credential);
            userRepostory.Setup(x => x.UpdateCredentialsAsync(credential)).ThrowsAsync(ApiExceptionMockData.ApiException500);

            Mock<ISecutiryService> securityService = new();

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.UpdatePasswordAsync(request));
        }

        [Fact]
        public async Task EmailRecoverPasswordAsyncShouldEmailMessage()
        {
            //Arrange
            Users user = UserMockData.Users;
            UserCredential credential = UserMockData.UserCredential;
            EmailMessage emailMessage = UserMockData.RecoveryEmail;
            string email = "email_random@gmail.com";

            Mock<IUserRepostory> userRepostory = new();
            userRepostory.Setup(x => x.GetUserByEmailAsync(email)).ReturnsAsync(user);
            userRepostory.Setup(x => x.GetUserCredentialByIdAsync(user.UserId)).ReturnsAsync(credential);

            Mock<ISecutiryService> securityService = new();
            securityService.Setup(x => x.RandomToken()).Returns("1234");

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            EmailMessage res = await userService.EmailRecoverPasswordAsync(email);

            //Assert
            Assert.Equal(res, emailMessage);
        }

        [Fact]
        public async Task EmailRecoverPasswordAsyncShouldApiExceptio404()
        {
            //Arrange
            Users user = UserMockData.Users;
            UserCredential credential = UserMockData.UserCredential;
            EmailMessage emailMessage = UserMockData.RecoveryEmail;
            string email = "email_random@gmail.com";

            Mock<IUserRepostory> userRepostory = new();
            #pragma warning disable CS8600
            userRepostory.Setup(x => x.GetUserByEmailAsync(email)).ReturnsAsync((Users)null);
            #pragma warning restore CS8600

            Mock<ISecutiryService> securityService = new();
            securityService.Setup(x => x.RandomToken()).Returns("1234");

            //Act
            UserService userService = new(userRepostory.Object, securityService.Object);

            //Assert
            await Assert.ThrowsAsync<ApiException>(async () => await userService.EmailRecoverPasswordAsync(email));
        }
    }
}
