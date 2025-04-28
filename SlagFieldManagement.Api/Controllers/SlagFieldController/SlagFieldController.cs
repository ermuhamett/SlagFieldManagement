using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlagFieldManagement.Application.Commands.EmptyBucket;
using SlagFieldManagement.Application.Commands.Invalid;
using SlagFieldManagement.Application.Commands.OutOfUse;
using SlagFieldManagement.Application.Commands.PlaceBucket;
using SlagFieldManagement.Application.Commands.RemoveBucket;
using SlagFieldManagement.Application.Commands.WentInUse;
using SlagFieldManagement.Application.DTO;
using SlagFieldManagement.Application.Queries.GetSlagFieldState;
using SlagFieldManagement.Application.Queries.GetSlagFieldStateSnapshot;

namespace SlagFieldManagement.Api.Controllers.SlagFieldController
{
    [Route("api/slagfield")]
    [ApiController]
    public class SlagFieldController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SlagFieldController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // Запросы (Queries)

        /// <summary>
        /// Получить текущее состояние всех мест на шлаковом поле
        /// </summary>
        [HttpGet("state")]
        public async Task<IActionResult> GetCurrentState()
        {
            var query = new GetSlagFieldStateQuery();
            var result = await _mediator.Send(query);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Получить состояние шлакового поля на заданный момент времени
        /// </summary>
        [HttpGet("state/snapshot")]
        public async Task<ActionResult<List<SlagFieldStateResponse>>> GetStateSnapshot([FromQuery] DateTime timestamp)
        {
            var query = new GetSlagFieldSnapshotQuery(timestamp);
            var result = await _mediator.Send(query);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        // Команда: Установить ковш
        [HttpPost("places/{placeId}/place-bucket")]
        public async Task<IActionResult> PlaceBucket(Guid placeId, [FromBody] PlaceBucketCommand request)
        {
            var command = new PlaceBucketCommand(
                placeId,
                request.BucketId,
                request.MaterialId,
                request.SlagWeight,
                request.StartDate
            );
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        // Команда: Опустошить ковш
        [HttpPost("places/{placeId}/empty-bucket")]
        public async Task<IActionResult> EmptyBucket(Guid placeId, [FromBody] EmptyBucketCommand request)
        {
            var command = new EmptyBucketCommand(
                placeId,
                request.EndDate
            );
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }

        // Команда: Убрать ковш
        [HttpPost("places/{placeId}/remove-bucket")]
        public async Task<IActionResult> RemoveBucket(Guid placeId)
        {
            var command = new RemoveBucketCommand(placeId);
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }

        // Команда: Очистить/Ошибка (Invalid)
        [HttpPost("places/{placeId}/invalid")]
        public async Task<IActionResult> MarkInvalid(Guid placeId, [FromBody] InvalidCommand request)
        {
            var command = new InvalidCommand(
                placeId,
                request.Description
            );
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }
        
        /// <summary>
        /// Отметить место как используемое
        /// </summary>
        [HttpPost("places/{placeId}/went-in-use")]
        public async Task<IActionResult> WentInUse(Guid placeId)
        {
            var command = new WentInUseCommand(placeId);
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }

        /// <summary>
        /// Отметить место как неиспользуемое
        /// </summary>
        [HttpPost("places/{placeId}/out-of-use")]
        public async Task<IActionResult> OutOfUse(Guid placeId)
        {
            var command = new OutOfUseCommand(placeId);
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }
    }
}
