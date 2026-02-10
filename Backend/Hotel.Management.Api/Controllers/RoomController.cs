
using MediatR;
using Hotel.Management.Application.Queries;
using Hotel.Management.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Infrastructure.Implementations;

namespace HotelMangement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public RoomsController(IMediator mediator, IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _mediator = mediator;
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] AddRoomDTO dto)
        {
            var command = new CreateRoomCommand
            {
                RoomNumber = dto.RoomNumber,
                HotelClassId = dto.HotelClassId,
                RoomTypeId = dto.RoomTypeId,
                PricePerNight = dto.PricePerNight,
                Status = dto.Status
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDTO dto)
        {
            var command = new UpdateRoomCommand
            {
                Id = id,
                RoomNumber = dto.RoomNumber,
                PricePerNight = dto.PricePerNight,
                Status = dto.Status,
                HotelClassId = dto.HotelClassId,
                RoomTypeId = dto.RoomTypeId
            };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound($"Room with ID {id} not found");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var command = new DeleteRoomCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound($"Room with ID {id} not found");
            return Ok("Deleted successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var query = new GetRoomByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound($"Room with ID {id} not found");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var query = new GetAllRoomsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("search-by-hotel/{hotelId}")]
        public async Task<IActionResult> SearchRoomsByHotel(
        int hotelId,
        [FromQuery] string checkIn,
        [FromQuery] string checkOut)
        {
            
            var allRooms = await _roomRepository.GetAllRoomsAsync();

            
            var roomsForHotel = allRooms
                .Where(r => r.HotelClassId == hotelId && r.Status != RoomStatus.UnderMaintenance)
                .ToList();

            List<Room> availableRoomsEntities;

            
            DateTime checkInDate = DateTime.MinValue;
            DateTime checkOutDate = DateTime.MinValue;
            bool datesValid = DateTime.TryParse(checkIn, out checkInDate) &&
                              DateTime.TryParse(checkOut, out checkOutDate);

            if (!datesValid)
            {
            
                availableRoomsEntities = roomsForHotel;
            }
            else
            {
            
                var allBookings = await _bookingRepository.GetAllBookingsAsync();

                var conflictingRoomIds = allBookings
                    .Where(b =>
                        b.CheckInDate < checkOutDate &&
                        b.CheckOutDate > checkInDate
                    )
                    .Select(b => b.RoomId)
                    .Distinct()
                    .ToHashSet();

                availableRoomsEntities = roomsForHotel
                    .Where(r => !conflictingRoomIds.Contains(r.Id))
                    .ToList();
            }

            
            
            var dtos = availableRoomsEntities.Select(r => new RoomResponseDTO
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                PricePerNight = r.PricePerNight,
                Status = r.Status,
                HotelClassId = (int)r.HotelClassId,
                HotelName = r.HotelClass?.Name,

            
                RoomTypeId = (int)r.RoomTypeId,
                RoomType = r.RoomType?.TypeName, 
                Capacity = r.RoomType?.Capacity ?? 0,
                Description = r.RoomType?.Description
            }).ToList();

            return Ok(dtos);
        }
    }
}
