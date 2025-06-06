using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Util;
using Project.Application.Interfaces;

namespace Project.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AutenticateController : Controller
    {
        private readonly IAutenticateService _autenticateService;

        public AutenticateController(IAutenticateService autenticateService)
        {
            _autenticateService = autenticateService;
        }
        /// <summary>
        /// Endpoint utilizado para gerar token de autenticação nesta Api
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost(Name = "Autenticate")]
        public async Task<ActionResult> Autenticate(string email, string password)
        {
            var response = await _autenticateService.Autenticate(email, password);
            if (response.IsFailure)
            {
                return NotFound(new ApiResponse(response.Error.Code, response.Error.Message));
            }

            return Ok(new ApiOkResponse(response));
        }

    }
}
