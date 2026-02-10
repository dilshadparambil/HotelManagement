namespace Hotel.Management.Application.DTOs
{
    public class AddRoomTypeDTO
    {
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
    }

    public class UpdateRoomTypeDTO
    {
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
    }

    public class RoomTypeResponseDTO
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
    }

}
