using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class DeleteRoomTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DeleteRoomTypeCommandHandler : IRequestHandler<DeleteRoomTypeCommand, bool>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public DeleteRoomTypeCommandHandler(IRoomTypeRepository RoomTypeRepository)
        {
            _roomTypeRepository = RoomTypeRepository;
        }

        public async Task<bool> Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var roomType = await _roomTypeRepository.GetRoomTypeByIdAsync(request.Id);
            if (roomType == null)
                return false;

            await _roomTypeRepository.DeleteRoomTypeAsync(roomType);
            return true;
        }
    }
}