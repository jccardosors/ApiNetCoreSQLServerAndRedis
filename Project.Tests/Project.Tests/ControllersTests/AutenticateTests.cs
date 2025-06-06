using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.Api.Controllers;
using Project.Api.Util;
using Project.Application.Interfaces;
using Project.Application.Utils;
using Project.Application.ViewModels;

namespace Project.Tests.ControllersTests
{
    public class AutenticateTests
    {
        [Fact]
        public async Task Autenticate_Success()
        {
            //Assert
            var mockAutenticate = new Mock<IAutenticateService>();
            var email = "teste@teste.com.br";
            var senha = "senha123";

            mockAutenticate.Setup(p => p.Autenticate(email, senha)).ReturnsAsync(GetAutenticateMock());

            //Act
            var controller = new AutenticateController(mockAutenticate.Object);
            var response = await controller.Autenticate(email, senha);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiOkResponse>(okObjectResult.Value);
            var result = Assert.IsType<CustomResult<AutenticateVM>>(apiOkResponse.Result);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.True(result.IsSuccess);
            Assert.Equal("teste@teste.com.br", result.Value.Email);
            Assert.NotEmpty(result.Value.Token);
        }

        [Fact]
        public async Task Autenticate_Failure()
        {
            //Assert
            var mockAutenticate = new Mock<IAutenticateService>();
            var email = "teste@teste.com.br";
            var senha = "senha123";

            mockAutenticate.Setup(p => p.Autenticate(email, senha)).ReturnsAsync(GetAutenticateFailureMock());

            //Act
            var controller = new AutenticateController(mockAutenticate.Object);
            var response = await controller.Autenticate(email, senha);

            //Assert
            //Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.InternalServerError, apiOkResponse.StatusCode);
            Assert.Equal("Falha ao tentar gerar token de autenticação", apiOkResponse.Message);
        }


        #region Mock Methods


        private CustomResult<AutenticateVM> GetAutenticateMock()
        {
            var result = new AutenticateVM
            {
                UserId = 1,
                Email = "teste@teste.com.br",
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGVzdGVAdGVzdGUuY29tLmJyIiwiZXhwIjoxNzQ1NTQxNjQ4LCJpc3MiOiJUZXN0ZUFQSUlzc3VlciIsImF1ZCI6IlRlc3RlQVBJQXVkaWVuY2UifQ.ptFzsktmrdVcU7Ij0Oet8a4K9Yq-5hYm2Up0h03lrb0"
            };

            return CustomResult<AutenticateVM>.Success(result);
        }


        private CustomResult<AutenticateVM> GetAutenticateFailureMock()
        {
            return CustomResult<AutenticateVM>.Failure(CustomError.ExceptionError("Falha ao tentar gerar token de autenticação"));
        }

        #endregion

    }
}
