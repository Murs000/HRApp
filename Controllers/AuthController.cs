using DocumentFormat.OpenXml.EMMA;
using HRApp.Models;
using HRApp.Utilits;
using Microsoft.AspNetCore.Mvc;

namespace HRApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController() : ControllerBase
    {
        [HttpGet]
        public IActionResult Login(string name, string surname, string fatherName)
        {
            var models = XmlDeserializer.Deserialize<XmlModel>(@"http://www.hms.gov.az/frq-content/nov_snk_v1/DOMESTIC.xml");
            foreach(var model in models)
            {
                var credentials = model.NAME_ORIGINAL_SCRIPT.Split();
                if(credentials[0] == surname && credentials[1] == name && credentials[2] == fatherName)
                {
                    return BadRequest("You cant autorize");
                }
            }
            return Ok();
        }
    }
}