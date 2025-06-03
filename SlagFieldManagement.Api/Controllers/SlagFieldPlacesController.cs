using MediatR;
using Microsoft.AspNetCore.Mvc;
using SlagFieldManagement.Application.Commands.CreatePlaceCommand;
using SlagFieldManagement.Application.Queries.GetAllPlaces;

namespace SlagFieldManagement.Api.Controllers
{
    [Route("api/slagfieldplaces")]
    [ApiController]
    public class SlagFieldPlacesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SlagFieldPlacesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlace([FromBody] CreateSlagFieldPlaceCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlaces()
        {
            var places = await _mediator.Send(new GetAllPlacesQuery());
            return Ok(places);
        }
    }
}
