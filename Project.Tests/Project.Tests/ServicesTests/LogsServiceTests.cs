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
    public class LogsServiceTests
    {
        [Fact]
        public async Task Add_Success()
        {
            //Assert           
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogsRepository = new Mock<ILogsRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";

            mockUserRepository.Setup(p => p.GetItemByEmail(email)).ReturnsAsync(GetItemByEmailSuccessMock());
            mockLogsRepository.Setup(p => p.Add(It.IsAny<Logs>())).ReturnsAsync(AddSuccessMock());

            //Act
            var service = new LogsService(mockMapper, mockLogsRepository.Object, mockUserRepository.Object);
            var response = await service.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            var customResult = Assert.IsType<CustomResult<LogsVM>>(response);
            var result = Assert.IsType<LogsVM>(customResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsSuccess);
            Assert.Equal("Log teste", result.Description);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task Add_Failure()
        {
            //Assert           
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogsRepository = new Mock<ILogsRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";

            mockUserRepository.Setup(p => p.GetItemByEmail(email)).Throws(new Exception("Falha ao tentar incluir log"));
            mockLogsRepository.Setup(p => p.Add(It.IsAny<Logs>())).ReturnsAsync(AddSuccessMock());

            //Act
            var service = new LogsService(mockMapper, mockLogsRepository.Object, mockUserRepository.Object);
            var response = await service.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            var customResult = Assert.IsType<CustomResult<LogsVM>>(response);
            var result = Assert.IsType<CustomError>(customResult.Error);

            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsFailure);
            Assert.False(customResult.IsSuccess);
            Assert.Equal("Falha ao tentar incluir log", result.Message);
            Assert.Equal(((int)System.Net.HttpStatusCode.InternalServerError).ToString(), result.Code);
        }

        #region Mock Methods

        private User GetItemByEmailSuccessMock()
        {
            return new User
            {
                Email = "teste@teste.com.br",
                Id = 1,
                Password = "password"
            };
        }

        private Logs AddSuccessMock()
        {
            return new Logs
            {
                Data = DateTime.Now,
                Description = "Log teste",
                Id = 1,
                IdUser = 1
            };
        }

        #endregion
    }
}
