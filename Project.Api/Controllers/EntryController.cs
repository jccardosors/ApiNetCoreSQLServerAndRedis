using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Util;
using Project.Application.Caching;
using Project.Application.Interfaces;
using Project.Application.ViewModels;

namespace Project.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EntryController : Controller
    {
        private IEntryService _entryService;
        private readonly ITokenService _tokenService;

        public EntryController(IEntryService entryService, ITokenService tokenService)
        {
            _entryService = entryService;
            _tokenService = tokenService;           
        }

        /// <summary>
        /// Endpoint utilizado para retornar TODOS os lançamentos
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Get")]
        public async Task<ActionResult> Get()
        {
            var resultEmail = await _tokenService.GetEmailbyTokenClaims();

            var response = await _entryService.GetAll(resultEmail.Value);
            if (response.IsFailure)
            {
                return NotFound(new ApiResponse(response.Error.Code, response.Error.Message));
            }

            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Endpoint utilizado para retornar APENAS UM lançamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetItem")]
        public async Task<ActionResult> GetItem(int id)
        {
            var resultEmail = await _tokenService.GetEmailbyTokenClaims();

            var response = await _entryService.GetItem(resultEmail.Value, id);
            if (response.IsFailure)
            {
                return NotFound(new ApiResponse(response.Error.Code, response.Error.Message));
            }

            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Endpoint utilizado para adicionar um NOVO lançamento
        /// </summary>
        /// <param name="entryRequest"></param>
        /// <returns></returns>
        [HttpPost(Name = "Add")]
        public async Task<ActionResult> Add(EntryVM entryRequest)
        {
            var resultEmail = await _tokenService.GetEmailbyTokenClaims();

            var response = await _entryService.AddEntry(resultEmail.Value, entryRequest);
            if (response.IsFailure)
            {
                return NotFound(new ApiResponse(response.Error.Code, response.Error.Message));
            }

            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Endpoint utilizado para atualizar um lançamento EXISTENTE
        /// </summary>
        /// <param name="entryRequest"></param>
        /// <returns></returns>
        [HttpPut(Name = "Update")]
        public async Task<ActionResult> Update(EntryVM entryRequest)
        {
            var resultEmail = await _tokenService.GetEmailbyTokenClaims();

            var response = await _entryService.UpdateEntry(resultEmail.Value, entryRequest);
            if (response.IsFailure)
            {
                return NotFound(new ApiResponse(response.Error.Code, response.Error.Message));
            }

            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Endpoint utilizado para EXCLUIR um lançamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var resultEmail = await _tokenService.GetEmailbyTokenClaims();

            var response = await _entryService.DeleteEntry(resultEmail.Value, id);
            if (response.IsFailure)
            {
                return NotFound(new ApiResponse(response.Error.Code, response.Error.Message));
            }

            return Ok(new ApiOkResponse(response));
        }
    }
}
