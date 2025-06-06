using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.Api.Controllers;
using Project.Api.Util;
using Project.Application.Interfaces;
using Project.Application.Utils;
using Project.Application.ViewModels;

namespace Project.Tests.ControllersTests
{

    public class EntryTests
    {
        [Fact]
        public async Task GetAll_Success()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";

            mockEntry.Setup(p => p.GetAll(email)).ReturnsAsync(GetAllMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.Get();

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiOkResponse>(okObjectResult.Value);
            var result = Assert.IsType<CustomResult<IEnumerable<EntryVM>>>(apiOkResponse.Result);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count() >= 0);
        }

        [Fact]
        public async Task GetAll_Failure()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";

            mockEntry.Setup(p => p.GetAll(email)).ReturnsAsync(GetAllFailureMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.Get();

            //Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.InternalServerError, apiOkResponse.StatusCode);
            Assert.Equal("Falha na busca de lançamentos", apiOkResponse.Message);
        }

        [Fact]
        public async Task GetItem_Success()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";
            var id = 3;

            mockEntry.Setup(p => p.GetItem(email, id)).ReturnsAsync(GetItemMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.GetItem(id);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiOkResponse>(okObjectResult.Value);
            var result = Assert.IsType<CustomResult<EntryVM>>(apiOkResponse.Result);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Id == 3);
        }

        [Fact]
        public async Task GetItem_Failure()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br"; var id = 3;

            mockEntry.Setup(p => p.GetItem(email, id)).ReturnsAsync(GetItemFailureMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.GetItem(id);

            //Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.InternalServerError, apiOkResponse.StatusCode);
            Assert.Equal("Falha ao buscar lançamento", apiOkResponse.Message);
        }

        [Fact]
        public async Task Add_Success()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";
            var entryVM = new EntryVM
            {
                Id = 0,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = false,
                ValueEntry = 198.98M
            };

            mockEntry.Setup(p => p.AddEntry(email, entryVM)).ReturnsAsync(AddItemMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.Add(entryVM);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiOkResponse>(okObjectResult.Value);
            var result = Assert.IsType<CustomResult<EntryVM>>(apiOkResponse.Result);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Id == 10);
        }

        [Fact]
        public async Task Add_Failure()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";
            var entryVM = new EntryVM
            {
                Id = 0,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = false,
                ValueEntry = 198.98M
            };

            mockEntry.Setup(p => p.AddEntry(email, entryVM)).ReturnsAsync(AddItemFailureMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.Add(entryVM);

            //Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.InternalServerError, apiOkResponse.StatusCode);
            Assert.Equal("Falha ao tentar incluir novo lançamento", apiOkResponse.Message);
        }

        [Fact]
        public async Task Update_Success()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";
            var entryVM = new EntryVM
            {
                Id = 10,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = false,
                ValueEntry = 198.98M
            };

            mockEntry.Setup(p => p.UpdateEntry(email, entryVM)).ReturnsAsync(UpdateItemMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.Update(entryVM);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiOkResponse>(okObjectResult.Value);
            var result = Assert.IsType<CustomResult<EntryVM>>(apiOkResponse.Result);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Id == 10);
            Assert.True(result.Value.IsCredit);
            Assert.True(result.Value.ValueEntry == 200.00M);
        }

        [Fact]
        public async Task Update_Failure()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";
            var entryVM = new EntryVM
            {
                Id = 10,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = false,
                ValueEntry = 198.98M
            };

            mockEntry.Setup(p => p.UpdateEntry(email, entryVM)).ReturnsAsync(UpdateItemFailureMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.Update(entryVM);

            //Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.InternalServerError, apiOkResponse.StatusCode);
            Assert.Equal("Falha ao tentar atualizar lançamento", apiOkResponse.Message);
        }

        [Fact]
        public async Task Delete_Success()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";
            var id = 3;

            mockEntry.Setup(p => p.DeleteEntry(email, id)).ReturnsAsync(DeleteItemMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.Delete(id);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiOkResponse>(okObjectResult.Value);
            var result = Assert.IsType<CustomResult<int>>(apiOkResponse.Result);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, okObjectResult.StatusCode);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value > 0);
        }

        [Fact]
        public async Task Delete_Failure()
        {
            //Assert
            var mockEntry = new Mock<IEntryService>();
            var mockToken = new Mock<ITokenService>();
            var email = "teste@teste.com.br";
            var id = 3;

            mockEntry.Setup(p => p.DeleteEntry(email, id)).ReturnsAsync(DeleteItemFailureMock());
            mockToken.Setup(p => p.GetEmailbyTokenClaims()).ReturnsAsync(GetEmailByTokenMock());

            //Act
            var controller = new EntryController(mockEntry.Object, mockToken.Object);
            var response = await controller.Delete(id);

            //Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(response);
            var apiOkResponse = Assert.IsType<ApiResponse>(okObjectResult.Value);

            Assert.NotNull(response);
            Assert.NotNull(apiOkResponse);
            Assert.Equal((int)System.Net.HttpStatusCode.InternalServerError, apiOkResponse.StatusCode);
            Assert.Equal("Falha ao tentar excluir lançamento", apiOkResponse.Message);
        }

        #region Mocks Methods

        private CustomResult<string> GetEmailByTokenMock()
        {
            return CustomResult<string>.Success("teste@teste.com.br");
        }

        private CustomResult<IEnumerable<EntryVM>> GetAllMock()
        {
            var result = new List<EntryVM>();

            result.Add(new EntryVM
            {
                Id = 1,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = true,
                ValueEntry = 145.89M
            });

            return CustomResult<IEnumerable<EntryVM>>.Success(result);
        }

        private CustomResult<IEnumerable<EntryVM>> GetAllFailureMock()
        {
            return CustomResult<IEnumerable<EntryVM>>.Failure(CustomError.ExceptionError("Falha na busca de lançamentos"));
        }

        private CustomResult<EntryVM> GetItemMock()
        {
            var result = new EntryVM
            {
                Id = 3,
                DateEntry = DateTime.Now,
                Description = "Description 3",
                IsCredit = true,
                ValueEntry = 145.89M
            };

            return CustomResult<EntryVM>.Success(result);
        }

        private CustomResult<EntryVM> GetItemFailureMock()
        {
            return CustomResult<EntryVM>.Failure(CustomError.ExceptionError("Falha ao buscar lançamento"));
        }

        private CustomResult<EntryVM> AddItemMock()
        {
            var result = new EntryVM
            {
                Id = 10,
                DateEntry = DateTime.Now,
                Description = "Description 1",
                IsCredit = false,
                ValueEntry = 198.98M
            };

            return CustomResult<EntryVM>.Success(result);
        }

        private CustomResult<EntryVM> AddItemFailureMock()
        {
            return CustomResult<EntryVM>.Failure(CustomError.ExceptionError("Falha ao tentar incluir novo lançamento"));
        }

        private CustomResult<EntryVM> UpdateItemMock()
        {
            var result = new EntryVM
            {
                Id = 10,
                DateEntry = DateTime.Now,
                Description = "Description 1111111",
                IsCredit = true,
                ValueEntry = 200.00M
            };

            return CustomResult<EntryVM>.Success(result);
        }

        private CustomResult<EntryVM> UpdateItemFailureMock()
        {
            return CustomResult<EntryVM>.Failure(CustomError.ExceptionError("Falha ao tentar atualizar lançamento"));
        }

        private CustomResult<int> DeleteItemMock()
        {
            return CustomResult<int>.Success(1);
        }

        private CustomResult<int> DeleteItemFailureMock()
        {
            return CustomResult<int>.Failure(CustomError.ExceptionError("Falha ao tentar excluir lançamento"));
        }


        #endregion
    }
}
