using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using HRApp.DataAccess;
using HRApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController(IEmployeeRepository repository) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Employee>> Get() => repository.Get();
        [HttpGet("{id}")]
        public ActionResult<Employee> GetById(int id) 
        {
            Employee employee = repository.Get(id);
            if(employee.Id != 0)
            {
                return employee;
            }
            else
            {
                return NotFound();
            }
        } 
        [HttpPost]
        public IActionResult Insert([FromBody]Employee entity)
        {
            repository.Insert(entity);
            
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        } 
        [HttpPut]
        public IActionResult Update([FromBody]Employee entity)
        {
            int resultId = repository.Update(entity);
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
            if(repository.Delete(id))
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }
        [HttpGet("Exel")]
        public FileResult Exel()
        {
            List<Employee> employees= repository.Get();
            
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("List Of Employees");
            worksheet.Cell(1,1).Value = "Name";
            worksheet.Cell(1,2).Value = "Surname";
            worksheet.Cell(1,3).Value = "Birth Date";
            for(int i = 0; i < employees.Count; i++)
            {
                worksheet.Cell(i+2,1).Value = employees[i].Name;
                worksheet.Cell(i+2,2).Value = employees[i].Surname;
                worksheet.Cell(i+2,3).Value = employees[i].BirthDate.Date;
            }
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
        }
    }
}