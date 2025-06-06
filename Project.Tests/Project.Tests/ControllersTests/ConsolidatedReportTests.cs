using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.Api.Controllers;
using Project.Api.Util;
using Project.Application.Interfaces;
using Project.Application.Utils;
using Project.Application.ViewModels;

namespace Project.Tests.ControllersTests
{
    public class ConsolidatedReportTests
    {
        [Fact]
        public async Task GenerateReport_Success()
        {
            //Assert
            var mockAutenticate = new Mock<IConsolidatedReportService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";
            var parameters = new ConsolidatedReportVM
            {
                CreditAndDebit = true,
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now,
                OnlyCredit = false,
            };

            mockAutenticate.Setup(p => p.GenerateReport(email, parameters)).ReturnsAsync(GenerateReportMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new ConsolidatedReportController(mockToken.Object, mockAutenticate.Object);
            var response = await controller.GenerateReport(parameters);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiOkResponse>(okObjectResult.Value);
            var result = Assert.IsType<CustomResult<ConsolidatedReportResultVM>>(apiOkResponse.Result);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.TotalValue > 0);
            Assert.True(result.Value.Items.Any());
        }

        [Fact]
        public async Task GenerateReport_Failure()
        {
            //Assert
            var mockAutenticate = new Mock<IConsolidatedReportService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";
            var parameters = new ConsolidatedReportVM
            {
                CreditAndDebit = true,
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now,
                OnlyCredit = false,
            };

            mockAutenticate.Setup(p => p.GenerateReport(email, parameters)).ReturnsAsync(GenerateReportFailureMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new ConsolidatedReportController(mockToken.Object, mockAutenticate.Object);
            var response = await controller.GenerateReport(parameters);

            //Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.InternalServerError, apiOkResponse.StatusCode);
            Assert.Equal("Falha ao tentar gera relatório de lançamentos", apiOkResponse.Message);
        }

        #region Mocks Methods

        private CustomResult<string> GetEmailByTokenMock()
        {
            return CustomResult<string>.Success("teste@teste.com.br");
        }

        private CustomResult<ConsolidatedReportResultVM> GenerateReportMock()
        {
            var result = new ConsolidatedReportResultVM
            {
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now,
                TotalValue = 10M,
                Items = new List<ConsolidatedReportResultItemVM> {
                    new ConsolidatedReportResultItemVM
                {
                     DateEntry = DateTime.Now,
                     Description = string.Empty,
                     IsCredit = false,
                     Value = 10M
                }
                }
            };

            return CustomResult<ConsolidatedReportResultVM>.Success(result);
        }

        private CustomResult<ConsolidatedReportResultVM> GenerateReportFailureMock()
        {
            return CustomResult<ConsolidatedReportResultVM>.Failure(CustomError.ExceptionError("Falha ao tentar gera relatório de lançamentos"));
        }

        #endregion
    }
}
