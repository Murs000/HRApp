using DocumentFormat.OpenXml.Office2010.Excel;
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
        public List<Employee> Get(string? name, string? surname, string? fatherName, bool? sex, int? maxAge, int? minAge)
        {
            var employees = context.Employees.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                employees = employees.Where(e => e.Name.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(surname))
            {
                employees = employees.Where(e => e.Surname.Contains(surname));
            }
            if (!string.IsNullOrWhiteSpace(fatherName))
            {
                employees = employees.Where(e => e.FatherName.Contains(fatherName));
            }
            if (sex.HasValue)
            {
                employees = employees.Where(e => e.Sex == sex);
            }
            if (maxAge.HasValue)
            {
                var maxBirthDate = DateTime.Now.AddYears(-maxAge.Value);
                employees = employees.Where(e => e.BirthDate <= maxBirthDate);
            }
            if (minAge.HasValue)
            {
                var minBirthDate = DateTime.Now.AddYears(-minAge.Value);
                employees = employees.Where(e => e.BirthDate >= minBirthDate);
            }
            return employees.ToList();
        }
        public int Update(Employee entity)
        {
            var entityFromDb = context.Employees.Find(new object[]{entity.Id});
            if (entityFromDb == null) return -1;

            entityFromDb.Name = entity.Name;
            entityFromDb.Surname = entity.Surname;
            entityFromDb.FatherName = entity.FatherName;
            entityFromDb.BirthDate = entity.BirthDate;
            entityFromDb.Sex = entity.Sex;
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
        public List<Employee> Search(string? term)
        {
            if(string.IsNullOrWhiteSpace(term))
                return context.Employees.ToList();
            term = term.ToLower();
            return context.Employees.Where(o => o.Name.ToLower().Contains(term) || o.Surname.ToLower().Contains(term) || o.FatherName.ToLower().Contains(term)).ToList();
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
