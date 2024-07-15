namespace HRApp.DataAccess
{
    public interface IRepositoryUnitOfWork
    {
        public IEmployeeRepository EmployeeRepository {get;}
        public IOrderRepository OrderRepository {get;}
        public IFileRepository FileRepository {get;}
    }
}