using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeResponseDTO>
    {
        public int Id { get; set; }
    }
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery,EmployeeResponseDTO>
    {
        private readonly IEmployeeRepository _employeeRepository;


        public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeResponseDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await  _employeeRepository.GetEmployeeByIdAsync(request.Id);
            if (employee == null)
            {
                return null;
            }
            return new EmployeeResponseDTO
            {
                Id = employee.Id,
                HotelName = employee.HotelClass?.Name,
                HotelClassId = employee.HotelClassId,
                FullName = employee.FullName,
                Role = employee.Role,
                Email = employee.Email
            };
        }
    }
}