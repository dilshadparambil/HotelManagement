using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetAllEmployeesQuery : IRequest<List<EmployeeResponseDTO>>
    {
    }
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, List<EmployeeResponseDTO>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetAllEmployeesQueryHandler(IEmployeeRepository EmployeeRepository)
        {
            _employeeRepository = EmployeeRepository;
        }

        public async Task<List<EmployeeResponseDTO>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var Employees = await _employeeRepository.GetAllEmployeesAsync();
            var dtoList = Employees.Select(e => new EmployeeResponseDTO
            {
                Id = e.Id,
                HotelName = e.HotelClass?.Name ?? "Not Assigned",
                HotelClassId = e.HotelClassId,
                FullName = e.FullName,
                Role = e.Role,
                Email = e.Email
            })
            .ToList();
            return dtoList;
        }
    }
}