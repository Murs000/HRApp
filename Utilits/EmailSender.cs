using System.Linq;
using System.Net;
using System.Net.Mail;
using DocumentFormat.OpenXml.Drawing.Charts;
using HRApp.DataAccess;
using HRApp.Models;

namespace HRApp.Utilits
{
    public class EmailSender(IEmployeeRepository employeeRepository, IOrderRepository orderRepository) : IEmailSender
    {
        public void SendEmail(List<Employee> employees)
        {
            var fromAddress = new MailAddress("mailaddressforlab@yahoo.com", "Company");
            var toAddress = new MailAddress("m.mastali7@gmail.com");

            var smtp = Configure(fromAddress);

            string body;
            if(employees.Count == 0)
            {
                body = "Today is no orders written";
            }
            else
            {
                body = "Today Orders Write to:\n";
                foreach(var employee in employees)
                {
                    body = body + employee.Name + "\n";
                }
            }

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "Daily Report",
                Body = body
            })
            {
                try
                {
                    smtp.Send(message);
                }
                catch (SmtpException ex)
{
    // Handle or log the exception
    Console.WriteLine($"SMTP Exception: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
}
            }
    
        }
        private SmtpClient Configure(MailAddress fromAddress)
        {
            string fromPassword = "AA1111111111aa!";
        
            var smtp = new SmtpClient
            {
                // TODO: Yahoo app pasword
                Host = "smtp.mail.yahoo.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };                          

            return smtp;
        }
        public void SendDailyEmail()
        {
            var orders = orderRepository.Get().Where(o => o.Date.Day == DateTime.Now.Day).ToList();
            List<Employee> employees = [];
            if(orders.Count != 0)
            {
                foreach(var order in orders)
                {
                    employees.Add(employeeRepository.Get(order.EmployeeId));
                }
            }
            
            SendEmail(employees);
        }
    }
}