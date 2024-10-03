using Application.LogViewer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LogViewerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LogViewerController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] LogViewerQuery query) =>
            Ok(await _mediator.Send(query));
    }
}
