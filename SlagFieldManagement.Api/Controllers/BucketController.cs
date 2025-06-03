using MediatR;
using Microsoft.AspNetCore.Mvc;
using SlagFieldManagement.Application.Commands.CreateBucket;
using SlagFieldManagement.Application.Commands.DeleteBucket;
using SlagFieldManagement.Application.Queries.GetAllBuckets;

namespace SlagFieldManagement.Api.Controllers
{
    [Route("api/buckets")]
    [ApiController]
    public class BucketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BucketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBucket([FromBody] CreateBucketCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBuckets()
        {
            var buckets = await _mediator.Send(new GetAllBucketsQuery());
            return Ok(buckets);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBucket(Guid id)
        {
            await _mediator.Send(new DeleteBucketCommand(id));
            return NoContent();
        }
    }
}
