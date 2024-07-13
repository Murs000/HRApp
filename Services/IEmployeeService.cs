using HRApp.Models;

namespace HRApp.Services
{
    public interface IEmployeeService : IService<Employee>
    {
        public List<Employee> Get(string? name, string? surname, string? fatherName, bool? sex, int? maxAge, int? minAge);
        public List<Employee> Search(string? term);
        public byte[] Exel();
        public byte[] Doc(int id);
    }
}