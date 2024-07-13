using HRApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController(IEmailService emailSender) : ControllerBase
    {
        [HttpPost]
        public IActionResult SendDailyEmail()
        {
            emailSender.SendDailyEmail();

            return Ok("Daily email sent.");
        }
    }
}