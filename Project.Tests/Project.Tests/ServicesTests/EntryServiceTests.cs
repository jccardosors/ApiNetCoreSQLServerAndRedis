using AutoMapper;
using Moq;
using Project.Application;
using Project.Application.Caching;
using Project.Application.Interfaces;
using Project.Application.Servicos;
using Project.Application.Utils;
using Project.Application.ViewModels;
using Project.Domain;
using Project.Domain.Entities;

namespace Project.Tests.ServicesTests
{
    public class EntryServiceTests
    {
        [Fact]
        public async Task Add_Success()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";
            var entryVM = new EntryVM
            {
                Id = 0,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = false,
                ValueEntry = 198.98M
            };

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.Add(It.IsAny<Entry>())).ReturnsAsync(AddEntrySuccessMock());

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.AddEntry(email, entryVM);

            //Assert
            var customResult = Assert.IsType<CustomResult<EntryVM>>(response);
            var result = Assert.IsType<EntryVM>(customResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsSuccess);
            Assert.Equal("Despesas com luz", result.Description);
            Assert.Equal(1, result.Id);
            Assert.Equal(127.48M, result.ValueEntry);
            Assert.False(result.IsCredit);
        }

        [Fact]
        public async Task Add_Failure()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";
            var entryVM = new EntryVM
            {
                Id = 0,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = false,
                ValueEntry = 198.98M
            };

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.Add(It.IsAny<Entry>())).Throws(new Exception("Falha ao tentar incluir lançamento"));

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.AddEntry(email, entryVM);

            //Assert
            var customResult = Assert.IsType<CustomResult<EntryVM>>(response);
            var result = Assert.IsType<CustomError>(customResult.Error);

            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsFailure);
            Assert.False(customResult.IsSuccess);
            Assert.Equal("Falha ao tentar incluir lançamento", result.Message);
            Assert.Equal(((int)System.Net.HttpStatusCode.InternalServerError).ToString(), result.Code);
        }

        [Fact]
        public async Task Update_Success()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";
            var entryVM = new EntryVM
            {
                Id = 0,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = false,
                ValueEntry = 198.98M
            };

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.Update(It.IsAny<Entry>())).ReturnsAsync(UpdateEntrySuccessMock());

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.UpdateEntry(email, entryVM);

            //Assert
            var customResult = Assert.IsType<CustomResult<EntryVM>>(response);
            var result = Assert.IsType<EntryVM>(customResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsSuccess);
            Assert.Equal("Despesas com água", result.Description);
            Assert.Equal(1, result.Id);
            Assert.Equal(104.75M, result.ValueEntry);
            Assert.False(result.IsCredit);
        }

        [Fact]
        public async Task Update_Failure()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";
            var entryVM = new EntryVM
            {
                Id = 0,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = false,
                ValueEntry = 198.98M
            };

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.Update(It.IsAny<Entry>())).Throws(new Exception("Falha ao tentar atualizar lançamento"));

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.UpdateEntry(email, entryVM);

            //Assert
            var customResult = Assert.IsType<CustomResult<EntryVM>>(response);
            var result = Assert.IsType<CustomError>(customResult.Error);

            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsFailure);
            Assert.False(customResult.IsSuccess);
            Assert.Equal("Falha ao tentar atualizar lançamento", result.Message);
            Assert.Equal(((int)System.Net.HttpStatusCode.InternalServerError).ToString(), result.Code);
        }

        [Fact]
        public async Task GetItem_Success()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.GetItem(It.IsAny<int>())).ReturnsAsync(GetItemSuccessMock());
            //mockCachRedis.Setup(p => p.GetAsync(It.IsAny<string>())).ReturnsAsync(Task.FromResult("MockedValue"),TimeSpan.FromSeconds(3600));

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.GetItem(email, It.IsAny<int>());

            //Assert
            var result = Assert.IsType<CustomResult<EntryVM>>(response);

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Id == 1);
            Assert.Equal("Despesas com água", result.Value.Description);
            Assert.False(result.Value.IsCredit);
            Assert.Equal(104.75M, result.Value.ValueEntry);
        }

        [Fact]
        public async Task GetItem_Failure()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.GetItem(It.IsAny<int>())).Throws(new Exception("Falha ao tentar consultar lançamento"));

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.GetItem(email, It.IsAny<int>());

            //Assert
            var customResult = Assert.IsType<CustomResult<EntryVM>>(response);
            var result = Assert.IsType<CustomError>(customResult.Error);

            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsFailure);
            Assert.False(customResult.IsSuccess);
            Assert.Equal("Falha ao tentar consultar lançamento", result.Message);
            Assert.Equal(((int)System.Net.HttpStatusCode.InternalServerError).ToString(), result.Code);
        }

        [Fact]
        public async Task GetAll_Success()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.GetAll()).ReturnsAsync(GetAllSuccessMock());

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.GetAll(email);

            //Assert
            var result = Assert.IsType<CustomResult<IEnumerable<EntryVM>>>(response);

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count() >= 0);
        }

        [Fact]
        public async Task GetAll_Failure()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.GetAll()).Throws(new Exception("Falha ao tentar listar lançamentos"));

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.GetAll(email);

            //Assert
            var customResult = Assert.IsType<CustomResult<IEnumerable<EntryVM>>>(response);
            var result = Assert.IsType<CustomError>(customResult.Error);

            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsFailure);
            Assert.False(customResult.IsSuccess);
            Assert.Equal("Falha ao tentar listar lançamentos", result.Message);
            Assert.Equal(((int)System.Net.HttpStatusCode.InternalServerError).ToString(), result.Code);
        }

        [Fact]
        public async Task Delete_Success()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.Delete(It.IsAny<int>())).ReturnsAsync(1);

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.DeleteEntry(email, It.IsAny<int>());

            //Assert
            var result = Assert.IsType<CustomResult<int>>(response);

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value == 1);
        }

        [Fact]
        public async Task Delete_Failure()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var mockCachRedis = new Mock<ICachingService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.Delete(It.IsAny<int>())).Throws(new Exception("Falha ao tentar excluir lançamento"));

            //Act
            var service = new EntryService(mockMapper, mockEntryRepository.Object, mockLogsService.Object, mockCachRedis.Object);
            var response = await service.DeleteEntry(email, It.IsAny<int>());

            //Assert
            var customResult = Assert.IsType<CustomResult<int>>(response);
            var result = Assert.IsType<CustomError>(customResult.Error);

            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsFailure);
            Assert.False(customResult.IsSuccess);
            Assert.Equal("Falha ao tentar excluir lançamento", result.Message);
            Assert.Equal(((int)System.Net.HttpStatusCode.InternalServerError).ToString(), result.Code);
        }

        #region Mocks Method

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

        private Entry AddEntrySuccessMock()
        {
            var result = new Entry
            {
                DateEntry = DateTime.Now,
                Description = "Despesas com luz",
                Id = 1,
                IsCredit = false,
                ValueEntry = 127.48M
            };

            return result;
        }

        private Entry UpdateEntrySuccessMock()
        {
            var result = new Entry
            {
                DateEntry = DateTime.Now,
                Description = "Despesas com água",
                Id = 1,
                IsCredit = false,
                ValueEntry = 104.75M
            };

            return result;
        }

        private Entry GetItemSuccessMock()
        {
            var result = new Entry
            {
                DateEntry = DateTime.Now,
                Description = "Despesas com água",
                Id = 1,
                IsCredit = false,
                ValueEntry = 104.75M
            };

            return result;
        }

        private IEnumerable<Entry> GetAllSuccessMock()
        {
            var result = new List<Entry>();

            result.Add(new Entry
            {
                Id = 1,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = true,
                ValueEntry = 145.89M
            });

            return result;
        }



        #endregion
    }
}
