using HRApp.Models;

namespace HRApp.DataAccess
{
    public class OrderRepository(HRAppDb context) : IOrderRepository
    {
        public List<Order> Get() => context.Orders.ToList();
        public Order Get(int id) => context.Orders.Find(new object[] { id })  ?? new();
        public int Insert(Order entity)
        {
            context.Orders.Add(entity);
            return Save();
        }
        public int Update(Order entity)
        {
            var entityFromDb = context.Orders.Find(new object[]{entity.Id});
            if (entityFromDb == null) return -1;

            entityFromDb.Date = entity.Date;
            entityFromDb.EmployeeId = entity.EmployeeId;
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