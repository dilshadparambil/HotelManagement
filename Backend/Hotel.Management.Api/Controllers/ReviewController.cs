
using MediatR;
using Hotel.Management.Application.Queries;
using Hotel.Management.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using Hotel.Management.Application.DTOs;

namespace HotelMangement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDTO dto)
        {
            var command = new CreateReviewCommand
            {
                HotelClassId = dto.HotelClassId,
                CustomerId = dto.CustomerId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDTO dto)
        {
            var command = new UpdateReviewCommand
            {
                Id = id,
                Rating = dto.Rating,
                Comment = dto.Comment
            };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound($"Review with ID {id} not found");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var command = new DeleteReviewCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound($"Review with ID {id} not found");
            return Ok("Deleted successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var query = new GetReviewByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound($"Review with ID {id} not found");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var query = new GetAllReviewsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }



}