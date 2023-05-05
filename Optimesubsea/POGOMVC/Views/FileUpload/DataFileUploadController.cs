//using ClosedXML.Excel;
//using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using POGOMVC.DataLayer;
using POGOMVC.Models;
using System.Collections.Generic;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.IO;
using System;
using System.Linq;

namespace POGOMVC.Views.FileUpload
{
    public class DataFileUploadController : Controller
    {
        private readonly GasStationProjectDbContext _context;

        public DataFileUploadController(GasStationProjectDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string fileName = "Data.csv";
            var lst = GraphValues.GetGraphColumnData();
            var lstDistinct = lst.Select(a => Convert.ToDateTime(a.TimeStampValue.ToString("MM/dd/yyyy hh:mm:ss tt"))).Distinct().ToArray();
            //var lstDistinct = lst.Select(a => Convert.ToDateTime(a.TimeStampValue.ToString("MM/dd/yyyy hh:mm"))).Distinct().ToArray();
            //var lstDistinct = lst.Select(a => a.TimeStampValue.ToString("MM/dd/yyyy hh:mm:ss tt")).Distinct().ToArray();
            BarChart barChart = new BarChart();
            barChart.XAxis = lstDistinct;
            barChart.Data = lst;
            barChart.Key2 = lst.Where(a => a.Key == 2).ToList();
            barChart.Key3 = lst.Where(a => a.Key == 3).ToList();
            barChart.Key4 = lst.Where(a => a.Key == 4).ToList();
            barChart.Key5 = lst.Where(a => a.Key == 5).ToList();
            barChart.Key6 = lst.Where(a => a.Key == 6).ToList();
            ////File file = new File();
            //var file = File()
            //var _fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            //List<GraphValues> values = File. .ReadLines(_fileName)
            //    .Skip(1)
            //    .Select(v => GraphValues.FromCsv(v))
            //    .ToList();
            return View(barChart);
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
                    //var data = GetDataList(_fileName);
                }
            }
            catch (Exception ex)
            {

                throw;
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

    }
   
    public class GraphValues
    {
        //int Key;
        //decimal FloatValue;
        //DateTime TimeStampValue;
        //decimal IgnoreColumn;

        //public static GraphValues FromCsv(string csvLine)
        //{
        //    string[] values = csvLine.Split(',');
        //    //string[] values = csvLine.Split(splitWith);
        //    GraphValues graphValues = new GraphValues();
        //    graphValues.Key = Convert.ToInt32(values[0]);
        //    graphValues.FloatValue = Convert.ToDecimal(values[1]);
        //    graphValues.TimeStampValue = Convert.ToDateTime(values[2]);
        //    graphValues.IgnoreColumn = Convert.ToDecimal(values[3]);
        //    return graphValues;
        //}

        public static List<GraphColumns> GetGraphColumnData()
        {
            string fileName = "Data.csv";
            var filePath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            StreamReader reader;
            if (File.Exists(filePath))
            {
                reader = new StreamReader(File.OpenRead(filePath));
                List<GraphColumns> lstGraphColumns = new List<GraphColumns>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        string[] values = line.Split(',');
                        //foreach (var item in values)
                        {
                            GraphColumns graphColumns = new GraphColumns
                            {
                                Key = Convert.ToInt32(values[0]),
                                FloatValue = Convert.ToDecimal(values[1]),
                                TimeStampValue = Convert.ToDateTime(values[2]),
                                IgnoreColumn = Convert.ToDecimal(values[3])
                            };
                            lstGraphColumns.Add(graphColumns);
                        }
                    }
                }
                return lstGraphColumns;
            }
            else
            {
                Console.WriteLine("File doesn't exist");
                return null;
            }
        }
    }
}
