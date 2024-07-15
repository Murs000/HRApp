using HRApp.Models;

namespace HRApp.DataAccess
{
    public class FileRepository(HRAppDb context) : IFileRepository
    {
        public List<EmployeeFile> Get() => context.Files.ToList();
        public EmployeeFile Get(int id) => context.Files.Find(new object[] { id })  ?? new EmployeeFile(){Id = -1};
        public int Insert(EmployeeFile entity)
        {
            context.Files.Add(entity);
            return Save();
        }
        public int Update(EmployeeFile entity)
        {
            var entityFromDb = context.Files.Find(new object[]{entity.Id});
            if (entityFromDb == null) return -1;

            entityFromDb.FilePath = entity.FilePath;
            entityFromDb.EmployeeId = entity.EmployeeId;
            return Save();
        }
        public bool Delete(int id)
        {
            var entityFromDb = context.Files.Find(new object[]{id});
            if (entityFromDb == null) 
                return false;
            context.Files.Remove(entityFromDb);
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