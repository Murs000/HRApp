using HRApp.Models;

namespace HRApp.Services
{
    public interface IFileService : IService<EmployeeFile>
    {
        public byte[] GetFile(EmployeeFile model);
        public string SaveFile(IFormFile file, int employeeId);
        public bool DeleteFile(int id);
    }
}