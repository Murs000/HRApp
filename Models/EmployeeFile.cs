using System.ComponentModel.DataAnnotations;

namespace HRApp.Models
{
    public class EmployeeFile
    {
        [Key]
        public int Id {get; set;}
        public string FilePath {get; set;} = string.Empty;
        public int EmployeeId {get; set;}
        public Employee Employee {get; set;} = new();
    }
}