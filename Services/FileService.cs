
using HRApp.DataAccess;
using HRApp.Models;

namespace HRApp.Services
{
    public class FileService(IRepositoryUnitOfWork repository) : IFileService
    {
        public List<EmployeeFile> Get()
        {
            return repository.FileRepository.Get();
        }
        public EmployeeFile Get(int id)
        {
            return repository.FileRepository.Get(id);
        }
        public int Insert(EmployeeFile model)
        {
            model.Employee = repository.EmployeeRepository.Get(model.EmployeeId);
            return repository.FileRepository.Insert(model);
        }
        public int Update(EmployeeFile model)
        {
            return repository.FileRepository.Update(model);
        }
        public bool Delete(int id)
        {
            return repository.FileRepository.Delete(id);
        }
        public byte[] GetFile(EmployeeFile model)
        {
            return File.ReadAllBytes(model.FilePath);
        }
        public string SaveFile(IFormFile file, int employeeId)
        {
            var uploadPath = $"../HRApp/Files/{employeeId}";
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, file.FileName);

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                        file.CopyTo(stream);
                }
            }
            return filePath;
        }
        public bool DeleteFile(int id)
        {
            var file = Get(id);
            if(file.Id == -1)
                return false;
            
            File.Delete(file.FilePath);
            return true;
        }
    }
}