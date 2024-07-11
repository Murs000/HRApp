using System.Drawing.Text;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Packaging;
using Hangfire;
using HRApp.DataAccess;
using HRApp.Models;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;

namespace HRApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController(IEmployeeRepository repository, IOrderRepository orderRepository) : ControllerBase
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
        [HttpGet("Export")]
        public FileResult ExportEmployeesToExcel()
        {
            List<Employee> employees = repository.Get();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("List Of Employees");

                var headerRow = new List<string> { "Name", "Surname", "Birth Date" };
                for (int col = 0; col < headerRow.Count; col++)
                {
                    worksheet.Cell(1, col + 1).Value = headerRow[col];
                }

                for (int i = 0; i < employees.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = employees[i].Name;
                    worksheet.Cell(i + 2, 2).Value = employees[i].Surname;
                    worksheet.Cell(i + 2, 3).Value = employees[i].BirthDate.Date.ToShortDateString();
                }

                var stream = new MemoryStream();
                
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
                
            }
        }
        [HttpGet("Document/{id}")]
        public FileResult ExportEmployeeDoc(int id)
        {
            Employee employee = repository.Get(id);

            Order order = new()
            {
                EmployeeId = employee.Id,
                Date = DateTime.UtcNow
            };
            orderRepository.Insert(order);
            
            string filePath = "/Users/mursal/Projects/HRApp/Files/document_hrclubaz_72.docx";

            
            List<string> wordsToReplace = ["OrderId", "OrderDate", "EName", "ESName", "EFName", "ESex"];
            List<string> replacementWords = [order.Id.ToString(), order.Date.ToShortDateString(),employee.Name, employee.Surname, employee.FatherName];

            if(employee.Sex)
            {
                replacementWords.Add("oglu");
            }
            else
            {
                replacementWords.Add("qizi");
            }
            
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                XWPFDocument doc = new XWPFDocument(stream);

                for(int i = 0; i < wordsToReplace.Count; i++)
                {
                    ReplaceWordInDoc(doc,wordsToReplace[i],replacementWords[i]);
                }

                MemoryStream memoryStream = new MemoryStream();
                
                doc.Write(memoryStream);
                    memoryStream.Position = 0;

                return File(memoryStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Employee.docx");
            }
        }
        private XWPFDocument ReplaceWordInDoc(XWPFDocument doc,string wordToReplace,string replacementWord)
        {
            foreach (var para in doc.Paragraphs)
            {
                string text = para.ParagraphText;
                if (text.Contains(wordToReplace))
                {
                    para.ReplaceText(wordToReplace, replacementWord);
                }
            }
            return doc;
        }
        [HttpPost]
        public IActionResult Email()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Ok"), Cron.Daily);

            return Ok();
        }
    }
}