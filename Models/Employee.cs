using System.ComponentModel.DataAnnotations;

namespace HRApp.Models
{
    public class Employee
    {
        [Key]
        public int Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public string Surname {get; set;} = string.Empty;
        public string FatherName {get; set;} = string.Empty;
        public bool Sex {get; set;}
        public DateTime BirthDate {get; set;} 
    }
}