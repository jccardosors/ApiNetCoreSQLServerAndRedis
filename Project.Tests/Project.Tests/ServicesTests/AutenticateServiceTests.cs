using AutoMapper;
using Moq;
using Project.Application;
using Project.Application.Interfaces;
using Project.Application.Servicos;
using Project.Application.Utils;
using Project.Application.ViewModels;
using Project.Domain;
using Project.Domain.Entities;

namespace Project.Tests.ServicesTests
{
    public class AutenticateServiceTests
    {
        [Fact]
        public async Task Autenticate_Success()
        {
            //Assert           
            var mockUserRepository = new Mock<IUserRepository>();
            var mockTokenRepository = new Mock<ITokenService>();
            var mockLogsService = new Mock<ILogsService>();
            var mockControlUserAccessRepository = new Mock<IControlUserAccessRepository>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";
            var senha = "senha123";

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockUserRepository.Setup(p => p.GetItemByEmail(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(GetItemByEmailMock());
            mockControlUserAccessRepository.Setup(p => p.GetUserBlocked(It.IsAny<string>())).ReturnsAsync(true);
            mockTokenRepository.Setup(p => p.GenerateToken(It.IsAny<string>())).Returns(GenerateTokenMock());

            //Act
            var service = new AutenticateService(mockMapper, mockUserRepository.Object, mockTokenRepository.Object, mockLogsService.Object, mockControlUserAccessRepository.Object);
            var response = await service.Autenticate(email, senha);

            //Assert
            var customResult = Assert.IsType<CustomResult<AutenticateVM>>(response);
            var result = Assert.IsType<AutenticateVM>(customResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsSuccess);
            Assert.Equal("teste@teste.com.br", result.Email);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task Autenticate_Failure()
        {
            //Assert           
            var mockUserRepository = new Mock<IUserRepository>();
            var mockTokenRepository = new Mock<ITokenService>();
            var mockLogsService = new Mock<ILogsService>();
            var mockControlUserAccessRepository = new Mock<IControlUserAccessRepository>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";
            var senha = "senha123";

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockUserRepository.Setup(p => p.GetItemByEmail(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Falha ao tentar autenticar na Api"));
            mockControlUserAccessRepository.Setup(p => p.GetUserBlocked(It.IsAny<string>())).ReturnsAsync(true);
            mockTokenRepository.Setup(p => p.GenerateToken(It.IsAny<string>())).Returns(GenerateTokenMock());

            //Act
            var service = new AutenticateService(mockMapper, mockUserRepository.Object, mockTokenRepository.Object, mockLogsService.Object, mockControlUserAccessRepository.Object);
            var response = await service.Autenticate(email, senha);

            //Assert
            var customResult = Assert.IsType<CustomResult<AutenticateVM>>(response);
            var result = Assert.IsType<CustomError>(customResult.Error);

            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsFailure);
            Assert.False(customResult.IsSuccess);
            Assert.Equal("Falha ao tentar autenticar na Api", result.Message);
            Assert.Equal(((int)System.Net.HttpStatusCode.InternalServerError).ToString(), result.Code);
        }


        #region Mock Methods

        private CustomResult<LogsVM> AddLogSuccessMock()
        {
            var result = new LogsVM
            {
                Data = DateTime.Now,
                Description = "Test",
                Id = 1,
                IdUser = 1,
            };

            return CustomResult<LogsVM>.Success(result);
        }

        private User GetItemByEmailMock()
        {
            return new User
            {
                Email = "teste@teste.com.br",
                Id = 1,
                Password = "password",
            };
        }


        private string GenerateTokenMock()
        {
            return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGVzdGVAdGVzdGUuY29tLmJyIiwiZXhwIjoxNzQ1NTQxNjQ4LCJpc3MiOiJUZXN0ZUFQSUlzc3VlciIsImF1ZCI6IlRlc3RlQVBJQXVkaWVuY2UifQ.ptFzsktmrdVcU7Ij0Oet8a4K9Yq-5hYm2Up0h03lrb0";
        }

        #endregion
    }
}
