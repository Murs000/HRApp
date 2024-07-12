using HRApp.Utilits;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController(IEmailSender emailSender) : ControllerBase
    {
        [HttpPost]
        public IActionResult SendDailyEmail()
        {
            emailSender.SendDailyEmail();

            return Ok("Daily email sent.");
        }
    }
}