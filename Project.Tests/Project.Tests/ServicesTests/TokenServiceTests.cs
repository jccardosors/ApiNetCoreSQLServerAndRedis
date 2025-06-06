using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Project.Application.Interfaces;
using Project.Application.Servicos;
using Project.Application.Utils;
using System.Security.Claims;

namespace Project.Tests.ServicesTests
{
    public class TokenServiceTests
    {
        private static ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "MyClaimValue") }, "Basic"));

        [Fact]
        public async Task GenerateToken_Success()
        {
            //Assert           
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogsService = new Mock<ILogsService>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var email = "teste@teste.com.br";

            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Key")]).Returns("Teste API 2025 Teste API 2025 Teste API 2025");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Issuer")]).Returns("TesteAPIIssuer");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Audience")]).Returns("TesteAPIAudience");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:TokenDuration")]).Returns("5");

            //Act
            var service = new TokenService(mockConfiguration.Object, mockLogsService.Object, mockHttpContextAccessor.Object);
            var response = await Task.FromResult(service.GenerateToken(email));

            //Assert
            var customResult = Assert.IsType<string>(response);

            Assert.NotNull(response);
            Assert.NotNull(customResult);
        }

        [Fact]
        public async Task GetEmailbyTokenClaims_Success()
        {
            //Assert           
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogsService = new Mock<ILogsService>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            mockHttpContextAccessor.Setup(h => h.HttpContext.User).Returns(user);

            //Act
            var service = new TokenService(mockConfiguration.Object, mockLogsService.Object, mockHttpContextAccessor.Object);
            var response = await service.GetEmailbyTokenClaims();

            //Assert
            var customResult = Assert.IsType<CustomResult<string>>(response);

            Assert.NotNull(response);
            Assert.NotNull(customResult);
            Assert.True(response.IsSuccess);
            Assert.Equal("MyClaimValue", response.Value);
        }

        [Fact]
        public async Task GetEmailbyTokenClaims_Failure()
        {
            //Assert           
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogsService = new Mock<ILogsService>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            mockHttpContextAccessor.Setup(h => h.HttpContext.User).Throws(new Exception("Falha ao tentar recuperar email do usuário logado"));

            //Act
            var service = new TokenService(mockConfiguration.Object, mockLogsService.Object, mockHttpContextAccessor.Object);
            var response = await service.GetEmailbyTokenClaims();

            //Assert
            var customResult = Assert.IsType<CustomResult<string>>(response);
            var result = Assert.IsType<CustomError>(customResult.Error);

            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsFailure);
            Assert.False(customResult.IsSuccess);
            Assert.Equal("Falha ao tentar recuperar email do usuário logado", result.Message);
            Assert.Equal(((int)System.Net.HttpStatusCode.InternalServerError).ToString(), result.Code);
        }
    }
}
