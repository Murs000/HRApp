using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using Hangfire.PostgreSql.Properties;
using HRApp.DataAccess;
using HRApp.Models;
using Microsoft.VisualBasic;
using NPOI.XWPF.UserModel;

namespace HRApp.Services
{
    public class EmployeeService(IRepositoryUnitOfWork repository) : IEmployeeService
    {
        public List<Employee> Get()
        {
            return repository.EmployeeRepository.Get();
        }
        public Employee Get(int id)
        {
            return repository.EmployeeRepository.Get(id);
        }
        public List<Employee> Get(string? name, string? surname, string? fatherName, bool? sex, int? maxAge, int? minAge)
        {
            return repository.EmployeeRepository.Get(name, surname, fatherName, sex, maxAge, minAge);
        }
        public int Insert(Employee model)
        {
            return repository.EmployeeRepository.Insert(model);
        }
        public int Update(Employee model)
        {
            return repository.EmployeeRepository.Update(model);
        }
        public bool Delete(int id)
        {
            return repository.EmployeeRepository.Delete(id);
        }
        public List<Employee> Search(string? term)
        {
            return repository.EmployeeRepository.Search(term);
        }
        public byte[] Exel()
        {
            List<Employee> employees = repository.EmployeeRepository.Get();

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

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }
        public byte[] Doc(int id)
        {
            Employee employee = repository.EmployeeRepository.Get(id);

            Order order = new()
            {
                EmployeeId = employee.Id,
                Date = DateTime.UtcNow
            };
            repository.OrderRepository.Insert(order);
            
            string filePath = "../HRApp/Files/document_hrclubaz_72.docx";

            Dictionary<string,string> wordsToReplace2 =   
                new Dictionary<string, string>(){
                                {"3359fff6", employee.Name},
                                {"a744e9b4", employee.Surname},
                                {"308b848f", employee.FatherName},
                                {"2870c526", order.Id.ToString()},
                                {"ded9e9a6", order.Date.ToShortDateString()} };
            if(employee.Sex)
            {
                wordsToReplace2.Add("78daf1b2", "oglu");
            }
            else
            {
                wordsToReplace2.Add("78daf1b2", "qizi");
            }
            
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                XWPFDocument doc = new XWPFDocument(stream);

                foreach(string key in wordsToReplace2.Keys)
                {
                    ReplaceWordInDoc(doc,key,wordsToReplace2[key]);
                }

                using(MemoryStream ms = new MemoryStream())
                {
                    doc.Write(ms);
                    return ms.ToArray();
                }
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
    }
}