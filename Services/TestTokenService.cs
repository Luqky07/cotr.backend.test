using cotr.backend.Model;
using cotr.backend.Service.Token;
using Microsoft.Extensions.Configuration;
using Moq;

namespace cotr.backend.test.Services
{
    public class TestTokenService
    {
        private readonly IEnumerable<KeyValuePair<string, string>> inMemorySettings;

        //Precarga de la configuración de appsettings
        public TestTokenService()
        {
            inMemorySettings = new Dictionary<string, string> {
                {"JwtConfiguration:TokenApi:AccessKey", "RZWyLgooaVwxYcT95nNpVNmdRBqQLgr24ag2tfmL"},
                {"JwtConfiguration:TokenApi:RefreshKey", "8Rx7926sws2PM2KLEDuewRYdpZ4DuP3A86BCSzH2"},
                {"JwtConfiguration:TokenApi:Issuer", "https://codesofthering.com"},
                {"JwtConfiguration:TokenApi:Audience", "https://codesofthering.com/cotr.api"},
                {"JwtConfiguration:TokenApi:DurationInMinutes", "1"}
            };
        }

        [Fact]
        public void GetToken_ReturnsAccessToken()
        {
            //Arrange
            #pragma warning disable CS8620
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            #pragma warning restore CS8620

            //Act
            TokenService tokenService = new(configuration);

            string accessToken = tokenService.GetToken(true);

            //Assert
            Assert.NotNull(accessToken);
        }

        [Fact]
        public void GetToken_ReturnsRefreshToken()
        {
            //Arrange
            #pragma warning disable CS8620
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            #pragma warning restore CS8620

            //Act
            TokenService tokenService = new(configuration);

            string accessToken = tokenService.GetToken(false);

            //Assert
            Assert.NotNull(accessToken);
        }

        [Fact]
        public void GetToken_ThrowsApiException()
        {
            //Arrange
            IConfiguration configuration = new ConfigurationBuilder()
                .Build();

            //Act
            TokenService tokenService = new(configuration);

            //Assert
            Assert.Throws<ApiException>(() => tokenService.GetToken(true));
        }
    }
}
