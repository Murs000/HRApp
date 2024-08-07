using HRApp.Models;

namespace HRApp.DataAccess
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        public List<Employee> Get(string? name, string? surname, string? fatherName, bool? sex, int? maxAge, int? minAge);
        public List<Employee> Search(string? term);
    }
}