using System.Drawing.Text;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Packaging;
using Hangfire;
using HRApp.DataAccess;
using HRApp.Models;
using HRApp.Services;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;

namespace HRApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController(IEmployeeService service) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Employee>> Get([FromQuery] string? name,[FromQuery] string? surname,[FromQuery] string? fatherName,[FromQuery] bool? sex,
                                                    [FromQuery] int? maxAge,[FromQuery] int? minAge,[FromQuery] string? term)
        {
            if (!string.IsNullOrWhiteSpace(term))
            {
                var searchResults = service.Search(term);
                return Ok(searchResults);
            }

            var products = service.Get(name, surname, fatherName, sex, maxAge, minAge);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public ActionResult<Employee> GetById(int id) 
        {
            Employee employee = service.Get(id);
            if(employee.Id != 0)
            {
                return Ok(employee);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Insert([FromBody]Employee entity)
        {
            service.Insert(entity);
            
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        } 
        [HttpPut]
        public IActionResult Update([FromBody]Employee entity)
        {
            int resultId = service.Update(entity);
            if( resultId == -1)
            {
                return NotFound();
            }
            else
            {
                return Ok(entity);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if(service.Delete(id))
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }
        [HttpGet("Export")]
        public FileResult ExportEmployeesToExcel()
        {
            byte[] exelBytes = service.Exel();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Employees.xlsx";

            return File(exelBytes, contentType, fileName);
        }
        [HttpGet("Document/{id}")]
        public FileResult ExportEmployeeDoc(int id)
        {
            byte[] docBytes = service.Doc(id);
            string contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            string fileName = $"{service.Get(id).Name}.docx";

            return File(docBytes, contentType, fileName);
        }
    }
}