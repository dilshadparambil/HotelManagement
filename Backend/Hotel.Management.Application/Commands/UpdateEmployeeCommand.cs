using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class UpdateEmployeeCommand : IRequest<EmployeeResponseDTO>
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? HotelClassId { get; set; }
    }
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeResponseDTO>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHotelRepository _hotelRepository;

        public UpdateEmployeeCommandHandler(IEmployeeRepository EmployeeRepository, IHotelRepository HotelRepository)
        {
            _employeeRepository = EmployeeRepository;
            _hotelRepository = HotelRepository;
        }

        public async Task<EmployeeResponseDTO> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(request.Id);
            if (employee == null)
                return null;

            employee.Role = request.Role ?? employee.Role;
            employee.FullName = request.FullName ?? employee.FullName;
            employee.Email = request.Email ?? employee.Email;

            if (request.HotelClassId.HasValue)
            {
                employee.HotelClassId = request.HotelClassId.Value;
            }

            await _employeeRepository.UpdateEmployeeAsync(employee);
            
            var updatedEmployee = await _employeeRepository.GetEmployeeByIdAsync(employee.Id);

            string hotelName = updatedEmployee.HotelClass?.Name ?? "Not Assigned";

            return new EmployeeResponseDTO
            {
                Id = updatedEmployee.Id,
                HotelName = hotelName,
                FullName = updatedEmployee.FullName,
                Role = updatedEmployee.Role,
                Email = updatedEmployee.Email
            };
        }
    }
}