using HRApp.Models;

namespace HRApp.DataAccess
{
public class EmployeeRepository(HRAppDb context) : IEmployeeRepository
{
        public List<Employee> Get() => context.Employees.ToList();

        public Employee Get(int id) => context.Employees.Find(new object[] { id })  ?? new();

        public int Insert(Employee entity) 
        {
            context.Employees.Add(entity);
            return Save();
        }
        public int Update(Employee entity)
        {
            var entityFromDb = context.Employees.Find(new object[]{entity.Id});
            if (entityFromDb == null) return -1;

            entityFromDb.Name = entity.Name;
            entityFromDb.Surname = entity.Surname;
            entityFromDb.BirthDate = entity.BirthDate;
            return Save();
        }

        public bool Delete(int id)
        {
            var entityFromDb = context.Employees.Find(new object[]{id});
            if (entityFromDb == null) 
                return false;
            context.Employees.Remove(entityFromDb);
            Save();
            return true;
        }

        public int Save() => context.SaveChanges();

        protected bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            
            if (disposing)
            {
                context.Dispose();
            }
            
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
