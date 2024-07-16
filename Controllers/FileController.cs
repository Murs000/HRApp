using System.Drawing.Text;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using Hangfire;
using HRApp.DataAccess;
using HRApp.Models;
using HRApp.Services;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Dml;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;

namespace HRApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController(IFileService service) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<EmployeeFile>> Get() => service.Get();
        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            var file = service.Get(id);
            if(file.Id != -1)
            {
                return File(service.GetFile(file) , "application/octet-stream");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Insert(IFormFileCollection files, int employeeId)
        {
            foreach(IFormFile file in files )
            {
                var path = service.SaveFile(file,employeeId);
                service.Insert(new EmployeeFile
                {
                    EmployeeId = employeeId,
                    FilePath = path
                });
            }
            
            return Ok();
        } 
        [HttpPut]
        public IActionResult Update(IFormFileCollection files, [FromQuery]int[] fileIds, int employeeId)
        {
            foreach(int id in fileIds)
            {
                if(service.Delete(id))
                {
                    service.DeleteFile(id);
                }
            }
            foreach(IFormFile file in files )
            {
                var path = service.SaveFile(file,employeeId);
                service.Insert(new EmployeeFile
                {
                    EmployeeId = employeeId,
                    FilePath = path
                });
            }

            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if(service.Delete(id))
                if(service.Delete(id))
                    return Ok();
            
            return NotFound();
        }
    }
}