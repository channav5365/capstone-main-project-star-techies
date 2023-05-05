using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Core.ExcelPackage;
using POGOMVC.DataLayer;
using POGOMVC.Models;
using System.Collections.Generic;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace POGOMVC.Views.FileUpload
{
    public class DataFileUploadCopyController : Controller
    {
        private readonly GasStationProjectDbContext _context;

        public DataFileUploadController(GasStationProjectDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            try
            {
                string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";

                var _extension = System.IO.Path.GetExtension(fileName);

                var _fileName = file.FileName;
                if (_extension.Contains("csv"))
                {
                    using (FileStream fileStream = System.IO.File.Create(fileName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    var data = GetDataList(_fileName);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }

        private List<FileUploadModel> GetDataList(string fileName)
        {
            List<FileUploadModel> data = new List<FileUploadModel>();
            var _fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(_fileName, FileMode.Open, FileAccess.Read))
            {
                var _extension = System.IO.Path.GetExtension(_fileName);

                if (_extension.Contains("csv"))
                {
                    using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                    {
                        while (reader.Read())
                        {
                            data.Add(new FileUploadModel
                            {
                                C1 = Convert.ToInt32(reader.GetValue(0)),
                                C2 = Convert.ToInt32(reader.GetValue(1)),
                                C3 = Convert.ToDateTime(reader[2].ToString()),
                                C4 = Convert.ToInt32(reader.GetValue(3))
                            });
                        }
                    }
                    _context.t_FileUploadModels.AddRangeAsync(data);
                    _context.SaveChanges();
                    return data;
                }
            }
            return null;
        }


        public IActionResult DownloadCSV()
        {
            var data = _context.t_FileUploadModels.ToList();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("C1,C2,C3,C4");
            foreach (var row in data)
            {
                stringBuilder.AppendLine($"{row.C1},{row.C2},{row.C3},{row.C1}");
            }

            return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", "data.csv");
        }
        public IActionResult DownloadExcel()
        {
            var data = _context.t_FileUploadModels.ToList();
            string contentType = "application/vnd.opendxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "data.xlsx";
            using (var wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("DataItems");
                ws.Cell(1, 1).Value = "C1";
                ws.Cell(1, 2).Value = "C2";
                ws.Cell(1, 3).Value = "C3";
                ws.Cell(1, 4).Value = "C4";
                for (int i = 1; i < data.Count; i++)
                {
                    ws.Cell(i + 1, 1).Value = data[i - 1].C1;
                    ws.Cell(i + 1, 2).Value = data[i - 1].C2;
                    var _val = data[i - 1].C3;
                    if (_val != null && _val.HasValue)
                    {
                        ws.Cell(i + 1, 3).Value = _val.Value;
                    }
                    ws.Cell(i + 1, 4).Value = data[i - 1].C4;
                }
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
    }
}
