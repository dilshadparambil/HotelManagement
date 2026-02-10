
using MediatR;
using Hotel.Management.Application.Queries;
using Hotel.Management.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Entities;

namespace HotelMangement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] AddBookingDTO dto)
        {
            var command = new CreateBookingCommand
            {
                CustomerId=dto.CustomerId,
                RoomId = dto.RoomId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate =dto.CheckOutDate,
                Status = dto.Status
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDTO dto)
        {
            var command = new UpdateBookingCommand
            {
                Id = id,
                Status = dto.Status,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                RoomId = dto.RoomId,
                CustomerId = dto.CustomerId
            };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound($"Booking with ID {id} not found");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var command = new DeleteBookingCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound($"Booking with ID {id} not found");
            return Ok("Deleted successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var query = new GetBookingByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound($"Booking with ID {id} not found");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var query = new GetAllBookingsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }



}