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
    public class ConsolidatedReportServiceTests
    {
        [Fact]
        public async Task Autenticate_Success()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";
            var parameters = new ConsolidatedReportVM
            {
                CreditAndDebit = true,
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now,
                OnlyCredit = false,
            };

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.GetAllByPeriod(parameters.InitialDate, parameters.FinalDate)).ReturnsAsync(GetAllByPeriodSuccessMock());

            //Act
            var service = new ConsolidatedReportService(mockMapper, mockEntryRepository.Object, mockLogsService.Object);
            var response = await service.GenerateReport(email, parameters);

            //Assert
            var customResult = Assert.IsType<CustomResult<ConsolidatedReportResultVM>>(response);
            var result = Assert.IsType<ConsolidatedReportResultVM>(customResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsSuccess);
            Assert.Equal(1000.99M, result.TotalValue);
            Assert.True(result.Items.Any());
        }

        [Fact]
        public async Task Autenticate_Failure()
        {
            //Assert           
            var mockEntryRepository = new Mock<IEntryRepository>();
            var mockLogsService = new Mock<ILogsService>();
            var autoMappings = new ConfigurationMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMappings));
            Mapper mockMapper = new Mapper(configuration);
            var email = "teste@teste.com.br";
            var parameters = new ConsolidatedReportVM
            {
                CreditAndDebit = true,
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now,
                OnlyCredit = false,
            };

            mockLogsService.Setup(p => p.Add(email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(AddLogSuccessMock());
            mockEntryRepository.Setup(p => p.GetAllByPeriod(parameters.InitialDate, parameters.FinalDate)).Throws(new Exception("Falha ao tentar gerar relatório de lançamentos"));

            //Act
            var service = new ConsolidatedReportService(mockMapper, mockEntryRepository.Object, mockLogsService.Object);
            var response = await service.GenerateReport(email, parameters);

            //Assert
            var customResult = Assert.IsType<CustomResult<ConsolidatedReportResultVM>>(response);
            var result = Assert.IsType<CustomError>(customResult.Error);

            Assert.NotNull(customResult);
            Assert.NotNull(result);
            Assert.True(customResult.IsFailure);
            Assert.False(customResult.IsSuccess);
            Assert.Equal("Falha ao tentar gerar relatório de lançamentos", result.Message);
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

        public IEnumerable<Entry> GetAllByPeriodSuccessMock()
        {
            var result = new List<Entry>();
            result.Add(new Entry
            {
                DateEntry = DateTime.Now,
                Description = "Description 1",
                Id = 1,
                IsCredit = true,
                ValueEntry = 1000.99M
            });

            return result;
        }
        #endregion

    }
}
