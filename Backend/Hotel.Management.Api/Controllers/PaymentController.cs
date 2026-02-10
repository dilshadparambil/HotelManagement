
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
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] AddPaymentDTO dto)
        {
            var command = new CreatePaymentCommand
            {
                BookingId = dto.BookingId,
                Amount = dto.Amount,
                Method = (PaymentMethod)dto.Method,
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentDTO dto)
        {
            var command = new UpdatePaymentCommand
            {
                Id = id,
                Status = dto.Status
            };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound($"Payment with ID {id} not found");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var command = new DeletePaymentCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound($"Payment with ID {id} not found");
            return Ok("Deleted successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var query = new GetPaymentByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound($"Payment with ID {id} not found");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var query = new GetAllPaymentsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }



}