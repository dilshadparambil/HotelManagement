
using MediatR;
using Hotel.Management.Application.Queries;
using Hotel.Management.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using Hotel.Management.Application.DTOs;

namespace HotelMangement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddRoomType([FromBody] AddRoomTypeDTO dto)
        {
            var command = new CreateRoomTypeCommand
            {
                TypeName = dto.TypeName,
                Description = dto.Description,
                Capacity = dto.Capacity
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomType(int id, [FromBody] UpdateRoomTypeDTO dto)
        {
            var command = new UpdateRoomTypeCommand
            {
                Id = id,
                TypeName = dto.TypeName,
                Description = dto.Description,
                Capacity = dto.Capacity
            };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound($"RoomType with ID {id} not found");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            var command = new DeleteRoomTypeCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound($"RoomType with ID {id} not found");
            return Ok("Deleted successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomTypeById(int id)
        {
            var query = new GetRoomTypeByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound($"RoomType with ID {id} not found");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoomTypes()
        {
            var query = new GetAllRoomTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }



}