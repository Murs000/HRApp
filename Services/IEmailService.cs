using HRApp.Models;

namespace HRApp.Services
{
    public interface IEmailService
    {
        public void SendEmail(List<Employee> employees);
        public void SendDailyEmail();
    }
}