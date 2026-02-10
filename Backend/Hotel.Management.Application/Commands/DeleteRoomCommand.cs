using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class DeleteRoomCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, bool>
    {
        private readonly IRoomRepository _roomRepository;

        public DeleteRoomCommandHandler(IRoomRepository RoomRepository)
        {
            _roomRepository = RoomRepository;
        }

        public async Task<bool> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoomByIdAsync(request.Id);
            if (room == null)
                return false;

            await _roomRepository.DeleteRoomAsync(room);
            return true;
        }
    }
}