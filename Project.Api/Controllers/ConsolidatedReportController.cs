using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Util;
using Project.Application.Interfaces;
using Project.Application.ViewModels;

namespace Project.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConsolidatedReportController : Controller
    {
        private IConsolidatedReportService _consolidatedReportService;
        private readonly ITokenService _tokenService;

        public ConsolidatedReportController(ITokenService tokenService, IConsolidatedReportService consolidatedReportService)
        {
            _consolidatedReportService = consolidatedReportService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Endpoint utilizado para gerar dados para o relatório consolidado de lançamentos
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost(Name = "GenerateReport")]
        public async Task<ActionResult> GenerateReport(ConsolidatedReportVM parameters)
        {
            var resultEmail = await _tokenService.GetEmailbyTokenClaims();

            var response = await _consolidatedReportService.GenerateReport(resultEmail.Value, parameters);
            if (response.IsFailure)
            {
                return NotFound(new ApiResponse(response.Error.Code, response.Error.Message));
            }

            return Ok(new ApiOkResponse(response));
        }
    }
}
