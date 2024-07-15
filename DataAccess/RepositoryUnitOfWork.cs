namespace HRApp.DataAccess
{
    public class RepositoryUnitOfWork(HRAppDb context) : IRepositoryUnitOfWork
    {
        public IEmployeeRepository EmployeeRepository => new EmployeeRepository(context);
        public IOrderRepository OrderRepository => new OrderRepository(context);
        public IFileRepository FileRepository => new FileRepository(context);
    }
}