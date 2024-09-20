using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ModelViews;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<EmployeeResponseModel>> GetAll()
        {
            // Lấy tất cả nhân viên từ repository
            var employees = await _unitOfWork.GetRepository<Employees>().GetAllAsync();

            // Chuyển đổi từ Employees sang EmployeeResponseModel
            var employeeResponseModels = employees.Select(employee => new EmployeeResponseModel
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNPhoneNumber,
                DateOfBirth = employee.DayofBirth,
                Address = employee.Address,
                Salary = employee.Salary,
            }).ToList();

            return employeeResponseModels;
        }

        public async Task<EmployeeResponseModel?> GetById(object id)
        {
            // Lấy nhân viên theo ID từ repository
            var employee = await _unitOfWork.GetRepository<Employees>().GetByIdAsync(id);

            // Chuyển đổi từ Employees sang EmployeeResponseModel nếu nhân viên tồn tại
            if (employee == null)
                return null;

            return new EmployeeResponseModel
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNPhoneNumber,
                DateOfBirth = employee.DayofBirth,
                Address = employee.Address,
                Salary = employee.Salary,
            };
        }

        public async Task Add(EmployeeResponseModel employee)
        {
            // Chuyển đổi từ EmployeeResponseModel sang Employees
            var newEmployee = new Employees
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                PhoneNPhoneNumber = employee.PhoneNumber,
                DayofBirth = employee.DateOfBirth,
                Address = employee.Address,
                Salary = employee.Salary,
            };

            var repository = _unitOfWork.GetRepository<Employees>();
            await repository.InsertAsync(newEmployee);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(EmployeeResponseModel employee)
        {
            // Chuyển đổi từ EmployeeResponseModel sang Employees
            var existingEmployee = new Employees
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                PhoneNPhoneNumber = employee.PhoneNumber,
                DayofBirth = employee.DateOfBirth,
                Address = employee.Address,
                Salary = employee.Salary,
            };

            var repository = _unitOfWork.GetRepository<Employees>();
            repository.Update(existingEmployee);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(object id)
        {
            var employeeId = (Guid)id;
            var repository = _unitOfWork.GetRepository<Employees>();
            var employee = await repository.GetByIdAsync(employeeId);

            if (employee != null)
            {
                repository.Delete(employee);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
