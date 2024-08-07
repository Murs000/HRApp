using System.ComponentModel.DataAnnotations;
using Hangfire.PostgreSql.Properties;

namespace HRApp.Models
{
    public class Order
    {
        [Key]
        public int Id {get; set;}
        public int EmployeeId {get; set;}
        public Employee Employee {get; set;} = new();
        public DateTime Date {get; set;} 
    }
}