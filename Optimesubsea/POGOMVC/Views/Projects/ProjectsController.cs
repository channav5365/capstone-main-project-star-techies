using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using NuGet.Protocol;
using OfficeOpenXml.Core.ExcelPackage;
using POGOMVC.DataLayer;
using POGOMVC.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace POGOMVC.Views.Projects
{
    public class ProjectsController : Controller
    {
        private readonly GasStationProjectDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProjectsController(GasStationProjectDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            if (_contextAccessor == null || _contextAccessor.HttpContext == null || _contextAccessor.HttpContext.Session.GetInt32("UserPK") == null)
            {
                return RedirectToAction("Login", "UserRegistration");
            }
            //if (!BaseSessionModel.IsValidUser)
            //{

            //}
            //
            //Need to add the logic for loading only project which are assign.
            //
            List<ProjectModel> _projectList = new List<ProjectModel>();
            if (BaseSessionModel.UserRole == "Admin")
            {

                _projectList = _context.t_ProjectTable.ToList();
            }
            else
            {
                var _projectId = _context.t_UserHasProjects.Where(a => a.UserRegistrationId == BaseSessionModel.UserPK).Select(b => b.ProjectId).ToList();
                _projectList = _context.t_ProjectTable.Where(a => _projectId.Contains(a.Id)
                || a.SuperUserId == _contextAccessor.HttpContext.Session.GetInt32("UserPK")).ToList();
                foreach (var item in _projectList)
                {
                    if (item.SuperUserId == _contextAccessor.HttpContext.Session.GetInt32("UserPK"))
                    {
                        item.AssignedRoleToProject = "Super User";
                    }
                    else
                    {
                        item.AssignedRoleToProject = "End User";
                    }
                }
            }

            return View(_projectList);
        }

        private void GetAssinedRole(Action<ProjectModel> obj)
        {
            throw new NotImplementedException();
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (_contextAccessor == null || _contextAccessor.HttpContext == null || _contextAccessor.HttpContext.Session.GetInt32("UserPK") == null)
            {
                return RedirectToAction("Login", "UserRegistration");
            }


            if (id == null || _context.t_ProjectTable == null)
            {
                return NotFound();
            }

            var projectModel = await _context.t_ProjectTable
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projectModel == null)
            {
                return NotFound();
            }

            //if (projectModel.SuperUserId == _contextAccessor.HttpContext.Session.GetInt32("UserPK"))
            //{
                var _superUserName = (from pt in _context.t_ProjectTable
                                      join usReg in _context.t_UserRegistration
                                     on pt.SuperUserId equals usReg.Id
                                      where pt.Id == id
                                      select usReg.FirstName + " " + usReg.LastName).FirstOrDefault();
                projectModel.SelectedSuperUserName = _superUserName ?? "";

                var _endUserNamelst = (from up in _context.t_UserHasProjects
                                      join usReg in _context.t_UserRegistration
                                     on up.UserRegistrationId equals usReg.Id
                                      where up.ProjectId == id
                                      select usReg.FirstName + " " + usReg.LastName).ToList();

                var _endUserNames = string.Join(" , ", _endUserNamelst);

                projectModel.SelectedEndUserNames = _endUserNames;

                var _uploadDataEndUserName = (from pt in _context.t_ProjectTable
                                      join usReg in _context.t_UserRegistration
                                     on pt.UploadDataEndUserId equals usReg.Id
                                      where pt.Id == id
                                      select usReg.FirstName + " " + usReg.LastName).FirstOrDefault();
                projectModel.SelectedEndUserFileUploadName = _uploadDataEndUserName ?? "";

                //projectModel.AssignedRoleToProject = "Super User";
            //}
            //else
            //{
            //    projectModel.AssignedRoleToProject = "End User";
            //}
            return View(projectModel);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            if (!BaseSessionModel.IsValidUser)
            {
                return RedirectToAction("Login", "UserRegistration");
            }
            ProjectModel projectModel = new ProjectModel();

            int? adminId = _context.m_Roles.FirstOrDefault(a => a.RoleName == "Admin")?.Id;
            var lstUsers = _context.t_UserRegistration.Where(x => x.UserRoleIdId != adminId);
            var lstSuperUser = new SelectList(lstUsers, "Id", "UserName");
            var EndUsers = _context.t_UserRegistration.Where(x => x.UserRoleIdId != adminId && _context.t_ProjectTable.Any(y => y.SuperUserId != x.Id));

            var lstEndUsers = new MultiSelectList(EndUsers, "Id", "UserName", projectModel.EndUserIds);


            var selectedEndUser = _context.t_UserRegistration.Where(a => _context.t_UserHasProjects.Any(b => b.UserRegistrationId == a.Id));
            var lstSelectedEndUser = new SelectList(selectedEndUser, "Id", "UserName");

            List<SelectListItem> v2 = lstEndUsers.ToList();
            projectModel.ErrorMessage = string.Empty;
            projectModel.EndUsers = v2;
            projectModel.SuperUsers = lstSuperUser.ToList();
            projectModel.SelectedEndUser = lstSelectedEndUser.ToList();
            
            return View(projectModel);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,ProjectName,ProjectDescription,ProjectType,MyProperty,IsActive,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy")] ProjectModel projectModel)
        public async Task<IActionResult> Create(ProjectModel projectModel)
        {
            if (!BaseSessionModel.IsValidUser)
            {
                return RedirectToAction("Login", "UserRegistration");
            }

            projectModel.IsActive = true;
            projectModel.UpdatedBy = 1;
            projectModel.CreatedBy = 1;
            projectModel.CreatedOn = DateTime.Now;
            projectModel.UpdatedOn = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (projectModel.UploadDataEndUserId <= 0 )
                {
                    projectModel.UploadDataEndUserId = null;
                }
                _context.Add(projectModel);
                await _context.SaveChangesAsync();
                
                var l1 = projectModel?.EndUserIds?.ToList();

                if (projectModel != null && l1 != null)
                {
                    int[] l2 = l1.ToArray();
                    var lstUserHasProject = _context.t_UserHasProjects
                        .Where(a => a.ProjectId == projectModel.Id).ToList();

                    var lstRemoveUser = _context.t_UserHasProjects
                        .Where(a => a.ProjectId == projectModel.Id
                        && a.UserRegistrationId != null
                        && !l1.Contains((int)a.UserRegistrationId)).ToList();

                    //Need to delete
                    if (lstRemoveUser != null && lstRemoveUser.Count > 0)
                    {
                        _context.t_UserHasProjects.RemoveRange(lstRemoveUser);
                        _context.SaveChanges();
                    }

                    //var lstAddUsers = (from d1 in l2
                    //                   from d2 in lstUserHasProject 
                    //                   where d1 != d2.UserRegistrationId && d2.ProjectId == projectModel.Id
                    //                   select d1).ToList();


                    var lstAddUsers = l2.Where(a => !lstUserHasProject.Any(b => b.UserRegistrationId == a)).ToList();

                    List<UserHasProjectsModel> userHasProjects = new List<UserHasProjectsModel>();
                    if (lstAddUsers != null && lstAddUsers.Count > 0)
                    {

                        foreach (var item in lstAddUsers)
                        {
                            int _i = Convert.ToInt32(item);

                            UserHasProjectsModel userHasProject = new UserHasProjectsModel()
                            {
                                ProjectId = projectModel.Id,
                                UserRegistrationId = _i,
                                CreatedBy = 1,
                                CreatedOn = DateTime.Now,
                                IsActive = true,
                                UpdatedBy = 1,
                                UpdatedOn = DateTime.Now
                            };
                            userHasProjects.Add(userHasProject);
                        }
                       

                        if (userHasProjects.Count > 0)
                        {
                            _context.t_UserHasProjects.AddRange(userHasProjects);
                            _context.SaveChanges();
                        }
                    }
                   

                }                

                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!BaseSessionModel.IsValidUser)
            {
                return RedirectToAction("Login", "UserRegistration");
            }

            if (id == null || _context.t_ProjectTable == null)
            {
                return NotFound();
            }

            var projectModel = await _context.t_ProjectTable.FindAsync(id);
            if (projectModel == null)
            {
                return NotFound();
            }
            //Users
            projectModel.EndUserIds = (from d1 in _context.t_UserRegistration
                                       from d2 in _context.t_UserHasProjects
                                       where d1.Id == d2.UserRegistrationId && d2.ProjectId == id
                                       select d1.Id).ToArray();

            int? adminId = _context.m_Roles.FirstOrDefault(a => a.RoleName == "Admin")?.Id;
            var lstUsers = _context.t_UserRegistration.Where(x => x.UserRoleIdId != adminId);
            var lstSuperUser = new SelectList(lstUsers, "Id", "UserName");
            var EndUsers = _context.t_UserRegistration.Where(x => x.UserRoleIdId != adminId
            && _context.t_ProjectTable.Any(y => y.SuperUserId != x.Id && y.Id == id));

            var lstEndUsers = new MultiSelectList(EndUsers, "Id", "UserName", projectModel.EndUserIds);


            var selectedEndUser = _context.t_UserRegistration.Where(a => _context.t_UserHasProjects.Any(b => b.UserRegistrationId == a.Id));
            var lstSelectedEndUser = new SelectList(selectedEndUser, "Id", "UserName");

            List<SelectListItem> v2 = lstEndUsers.ToList();
            projectModel.ErrorMessage = string.Empty;
            projectModel.EndUsers = v2;
            projectModel.SuperUsers = lstSuperUser.ToList();
            projectModel.SelectedEndUser = lstSelectedEndUser.ToList();
            return View(projectModel);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectModel projectModel)
        {
            if (id != projectModel.Id)
            {
                return NotFound();
            }
            projectModel.IsActive = true;
            projectModel.UpdatedBy = 1;
            projectModel.UpdatedOn = DateTime.Now;
            //ModelValidatorProviders
            if (ModelState.IsValid)
            {
                try
                {
                    if (projectModel.UploadDataEndUserId == 0)
                    {
                        projectModel.UploadDataEndUserId = null;
                    }
                    //Needs to get all UserIds which are assigned to this Project Id
                    //If the list don't contains in the selected List then we need to remove
                    //If New User Is their then we need to add
                    var l1 = projectModel?.EndUserIds?.ToList();

                    if (projectModel != null && l1 != null)
                    {
                        int[] l2 = l1.ToArray();
                        var lstUserHasProject = _context.t_UserHasProjects
                            .Where(a => a.ProjectId == projectModel.Id).ToList();

                        var lstRemoveUser = _context.t_UserHasProjects
                            .Where(a => a.ProjectId == projectModel.Id
                            && a.UserRegistrationId != null
                            && !l1.Contains((int)a.UserRegistrationId)).ToList();

                        //Need to delete
                        if (lstRemoveUser != null && lstRemoveUser.Count > 0)
                        {
                            _context.t_UserHasProjects.RemoveRange(lstRemoveUser);
                            _context.SaveChanges();
                        }

                        //var lstAddUsers = (from d1 in l2
                        //                   from d2 in lstUserHasProject 
                        //                   where d1 != d2.UserRegistrationId && d2.ProjectId == projectModel.Id
                        //                   select d1).ToList();


                        var lstAddUsers = l2.Where(a => !lstUserHasProject.Any(b => b.UserRegistrationId == a)).ToList();

                        List<UserHasProjectsModel> userHasProjects = new List<UserHasProjectsModel>();
                        if (lstAddUsers != null && lstAddUsers.Count > 0)
                        {

                            foreach (var item in lstAddUsers)
                            {
                                int _i = Convert.ToInt32(item);

                                UserHasProjectsModel userHasProject = new UserHasProjectsModel()
                                {
                                    ProjectId = projectModel.Id,
                                    UserRegistrationId = _i,
                                    CreatedBy = 1,
                                    CreatedOn = DateTime.Now,
                                    IsActive = true,
                                    UpdatedBy = 1,
                                    UpdatedOn = DateTime.Now
                                };
                                userHasProjects.Add(userHasProject);
                            }
                            _context.Update(projectModel);
                            await _context.SaveChangesAsync();

                            if (userHasProjects.Count > 0)
                            {
                                _context.t_UserHasProjects.AddRange(userHasProjects);
                                _context.SaveChanges();
                            }
                        }
                        _context.Update(projectModel);
                        await _context.SaveChangesAsync();

                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectModelExists(projectModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.t_ProjectTable == null)
            {
                return NotFound();
            }

            var projectModel = await _context.t_ProjectTable
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.t_ProjectTable == null)
            {
                return Problem("Entity set 'GasStationProjectDbContext.t_ProjectTable'  is null.");
            }
            var projectModel = await _context.t_ProjectTable.FindAsync(id);
            if (projectModel != null)
            {
                _context.t_ProjectTable.Remove(projectModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult GetEndUsersList(int projectId, int id)
        {
            int[]? endUserIds = (from d1 in _context.t_UserRegistration
                                 from d2 in _context.t_UserHasProjects
                                 where d1.Id == d2.UserRegistrationId && d2.ProjectId == projectId && d1.Id != id
                                 select d1.Id).ToArray();
            int? adminId = _context.m_Roles.FirstOrDefault(a => a.RoleName == "Admin")?.Id;

            var lstUsers = _context.t_UserRegistration.Where(x => x.UserRoleIdId != adminId && x.Id != id);
            var lstQuery = new MultiSelectList(lstUsers, "Id", "UserName", endUserIds);
            List<SelectListItem> v2 = lstQuery.ToList();

            //return Json(new MultiSelectList(v2, "Id", "UserName"));
            return Json(lstUsers);
        }
        [HttpGet]
        public IActionResult GetSelectedEndUsersList(int projectId, int[] id)
        {
            //int[]? endUserIds = (from d1 in _context.t_UserRegistration
            //                     from d2 in _context.t_UserHasProjects
            //                     where d1.Id == d2.UserRegistrationId && d2.ProjectId == projectId && d1.Id != id
            //                     select d1.Id).ToArray();
            int? adminId = _context.m_Roles.FirstOrDefault(a => a.RoleName == "Admin")?.Id;

            var lstUsers = _context.t_UserRegistration.Where(x => x.UserRoleIdId != adminId && id.Contains(x.Id));
            var lstQuery = new SelectList(lstUsers, "Id", "UserName");

            List<SelectListItem> v2 = lstQuery.ToList();

            //return Json(new MultiSelectList(v2, "Id", "UserName"));
            return Json(v2);
        }
        [HttpGet]
        public async Task<IActionResult> ViewGraph(int id)
        {
            if (_contextAccessor == null || _contextAccessor.HttpContext == null || _contextAccessor.HttpContext.Session.GetInt32("UserPK") == null)
            {
                return RedirectToAction("Login", "UserRegistration");
            }

            bool isUploadAvailable = false;

            //---IsUploadAvailable
            ViewGraphModel viewGraphModel = new ViewGraphModel();
            viewGraphModel.Id = id;
            var _project = _context.t_ProjectTable.Where(a => a.Id == id).FirstOrDefault();
            if (_project != null)
            {
                viewGraphModel.ProjectName = _project.ProjectName;
                isUploadAvailable = BaseSessionModel.UserPK == _project.UploadDataEndUserId;
            }
            if (BaseSessionModel.UserRole == "Admin" || isUploadAvailable)
            {
                viewGraphModel.IsUploadAvailable = true;

            }
            var filePath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + id + "\\" + "Data.csv";
            var lst = GetGraphColumnData(filePath);
            Models.BarChart barChart = new Models.BarChart();
            if (lst != null && lst.Count > 0)
            {
                //var lstDistinct = lst.Select(a => Convert.ToDateTime(a.TimeStampValue.ToString("MM/dd/yyyy hh:mm:ss tt"))).Distinct().ToArray();
                //var lstDistinct = lst.Select(a => Convert.ToDateTime(a.TimeStampValue.ToString("MM/dd/yyyy hh:mm"))).Distinct().ToArray();
                var lstDistinct = lst.Select(a => Convert.ToDateTime(a.TimeStampValue.ToString("MM/dd/yyyy hh:mm"))).ToArray();
                //var lstDistinct = lst.Select(a => a.TimeStampValue.ToString("MM/dd/yyyy hh:mm:ss tt")).Distinct().ToArray();

                barChart.XAxis = lstDistinct;
                barChart.Data = lst;
                barChart.Key2 = lst.Where(a => a.Key == 2).ToList();
                barChart.Key3 = lst.Where(a => a.Key == 3).ToList();
                barChart.Key4 = lst.Where(a => a.Key == 4).ToList();
                barChart.Key5 = lst.Where(a => a.Key == 5).ToList();
                barChart.Key6 = lst.Where(a => a.Key == 6).ToList();
                viewGraphModel.IsDownloadAvailable = true;
            }
            viewGraphModel.BarChart = barChart;

            return View(viewGraphModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment, int id)
        {
            //string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            //Need to create the folder based on ProjectId

            var getFileType = System.IO.Path.GetExtension(file.FileName);

            if (getFileType == ".zip")
            {
                //UnZip
                var stream = file.OpenReadStream();
                var archive = new ZipArchive(stream);
                ZipArchiveEntry innerFile = archive.GetEntry("Data.csv");
                if (innerFile != null)
                {
                    string _folderPath = $"{hostingEnvironment.WebRootPath}\\files\\{id}";

                    if (!Directory.Exists(_folderPath))
                    {
                        Directory.CreateDirectory(_folderPath);
                    }
                    var filePath = Path.Combine(hostingEnvironment.WebRootPath, @"files\" + id, "Data.csv");
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    Thread.Sleep(2000);
                    innerFile.ExtractToFile(filePath);

                    ViewGraphModel viewGraphModel = new ViewGraphModel();
                    viewGraphModel.Id = id;

                    //var lst = GetGraphColumnData(filePath);
                    //var lstDistinct = lst.Select(a => Convert.ToDateTime(a.TimeStampValue.ToString("MM/dd/yyyy hh:mm:ss tt"))).Distinct().ToArray();
                    ////var lstDistinct = lst.Select(a => Convert.ToDateTime(a.TimeStampValue.ToString("MM/dd/yyyy hh:mm"))).Distinct().ToArray();
                    ////var lstDistinct = lst.Select(a => a.TimeStampValue.ToString("MM/dd/yyyy hh:mm:ss tt")).Distinct().ToArray();
                    //POGOMVC.Models.BarChart barChart = new POGOMVC.Models.BarChart();
                    //barChart.XAxis = lstDistinct;
                    //barChart.Data = lst;
                    //barChart.Key2 = lst.Where(a => a.Key == 2).ToList();
                    //barChart.Key3 = lst.Where(a => a.Key == 3).ToList();
                    //barChart.Key4 = lst.Where(a => a.Key == 4).ToList();
                    //barChart.Key5 = lst.Where(a => a.Key == 5).ToList();
                    //barChart.Key6 = lst.Where(a => a.Key == 6).ToList();

                    //viewGraphModel.BarChart = barChart;
                    //return View(viewGraphModel);
                    return RedirectToAction("ViewGraph", new { id = id });

                }
            }
            //string _folderPath = $"{hostingEnvironment.WebRootPath}\\files\\{"1"}";

            //if (!Directory.Exists(_folderPath))
            //{
            //    Directory.CreateDirectory(_folderPath);
            //}
            //string fileName = $"{_folderPath}\\{file.FileName}";
            //using (FileStream fileStream = System.IO.File.Create(fileName))
            //{
            //    ExcelPackage s = new ExcelPackage(fileStream);
            //    var v1 = s.Workbook.Worksheets[0].View;

            //    file.CopyTo(fileStream);
            //    fileStream.Flush();

            //}
            //var data = this.GetDataList(file.FileName);
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

        public async Task<IActionResult> DownloadZipFile(int id)
        {
            var filePathZip = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + id + "\\" + "Data.csv";

            string sourceDirectory = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + id + "\\";

            if (Directory.Exists(sourceDirectory))
            {
                string archive = $"{sourceDirectory}Data.zip";
                if (System.IO.File.Exists(archive))
                {
                    System.IO.File.Delete(archive);
                }
                using (var archive1 = ZipFile.Open(archive, ZipArchiveMode.Create))
                {
                    archive1.CreateEntryFromFile(filePathZip, Path.GetFileName(filePathZip));

                }
                using (var net = new System.Net.WebClient())
                {
                    var data = net.DownloadData(archive);
                    var content = new System.IO.MemoryStream(data);

                    return File(content.ToArray(), "application/zip");
                }
            }

            return await ViewGraph(id);

        }

        private bool ProjectModelExists(int id)
        {
            return _context.t_ProjectTable.Any(e => e.Id == id);
        }
        public List<GraphColumns> GetGraphColumnData(string filePath)
        {
            //string fileName = "Data.csv";
            //var filePath = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;

            if (System.IO.File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(System.IO.File.OpenRead(filePath)))
                {
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
            }
            else
            {
                Console.WriteLine("File doesn't exist");
                return null;
            }
        }
    }
}
