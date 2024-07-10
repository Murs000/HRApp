namespace HRApp.DataAccess
{
    public interface IRepository<T> : IDisposable
    {
        public List<T> Get();

        public T Get(int id);

        public int Insert(T entity);

        public int Update(T entity);

        public bool Delete(int id);

        public int Save();
    }
}