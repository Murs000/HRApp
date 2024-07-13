using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using HRApp.DataAccess;
using HRApp.Models;

namespace HRApp.Services
{
    public class EmailService(IRepositoryUnitOfWork repository) : IEmailService
    {

        public void SendEmail(List<Employee> employees)
        {
            var fromAddress = new MailAddress("m.mastali7@gmail.com", "Company");
            var toAddress = new MailAddress("m.mastali7@gmail.com");

            var smtp = Configure(fromAddress);

            string body;
            if (employees.Count == 0)
            {
                body = "Today no orders were written";
            }
            else
            {
                body = "Today Orders Written to:\n";
                foreach (var employee in employees)
                {
                    body += employee.Name + "\n";
                }
            }

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "Daily Report",
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private SmtpClient Configure(MailAddress fromAddress)
        {
            string fromPassword = "odjm vapt mgod dzgu";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
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
            var orders = repository.OrderRepository.Get().Where(o => o.Date.Day == DateTime.Now.Day).ToList();
            List<Employee> employees = new List<Employee>();
            if (orders.Count != 0)
            {
                foreach (var order in orders)
                {
                    employees.Add(repository.EmployeeRepository.Get(order.EmployeeId));
                }
            }

            SendEmail(employees);
        }
    }
}