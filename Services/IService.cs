using DocumentFormat.OpenXml.Math;

namespace HRApp.Services
{
    public interface IService<T>
    {
        public List<T> Get();
        public T Get(int id);
        public int Insert(T model);
        public int Update(T model);
        public bool Delete(int id);
    }
}