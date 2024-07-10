using HRApp.Models;

namespace HRApp.DataAccess
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        public List<Employee> Get();

        public Employee Get(int id);

        public int Insert(Employee entity);

        public int Update(Employee entity);

        public bool Delete(int id);

        public int Save();
    }
}