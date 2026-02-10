namespace Hotel.Management.Application.DTOs
{
    public class AddCustomerDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdProofNumber { get; set; }
    }

    public class UpdateCustomerDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string IdProofNumber { get; set; }
    }

    public class CustomerResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdProofNumber { get; set; }
    }

}
