using HRApp.Models;

namespace HRApp.Utilits
{
    public interface IEmailSender
    {
        public void SendEmail(List<Employee> employees);
        public void SendDailyEmail();
    }
}