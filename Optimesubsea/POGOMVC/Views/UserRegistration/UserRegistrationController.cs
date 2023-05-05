using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POGOMVC.DataLayer;
using POGOMVC.Models;

namespace POGOMVC.Views.UserRegistration
{
    public class UserRegistrationController : Controller
    {
        private readonly GasStationProjectDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserRegistrationController(GasStationProjectDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: UserRegistration
        public async Task<IActionResult> Index()
        {
            return View(await _context.t_UserRegistration.ToListAsync());
        }

        // GET: UserRegistration/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.t_UserRegistration == null)
            {
                return NotFound();
            }

            var userRegistrationModel = await _context.t_UserRegistration
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRegistrationModel == null)
            {
                return NotFound();
            }

            return View(userRegistrationModel);
        }

        // GET: UserRegistration/Create
        public IActionResult Create()
        {
            //Roles
            UserRegistrationModel userRegistrationModel = new UserRegistrationModel();

            var lstQuery = new SelectList(_context.m_PasscodeRecoveryQuestionnaire, "Id", "QuestionName");

            var v2 = (List<SelectListItem>)lstQuery.ToList();
            userRegistrationModel.ErrorMessage = string.Empty;
            userRegistrationModel.Questionnaire1 = v2;
            return View(userRegistrationModel);
        }

        // POST: UserRegistration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRegistrationModel userRegistrationModel)
        //public async Task<IActionResult> Create([Bind("Id,UserId,UserName,Passcode,EmailId,Narration,PasscodeRecoveryAnswer1,PasscodeRecoveryQuestionnaireId1Id")] UserRegistrationModel userRegistrationModel)
        {
            var _isExistUser = _context.t_UserRegistration.Any(a => a.UserName.ToLower().Trim() == userRegistrationModel.UserName.ToLower());
            if (_isExistUser)
            {
                var lstQuery = new SelectList(_context.m_PasscodeRecoveryQuestionnaire, "Id", "QuestionName");

                var v2 = (List<SelectListItem>)lstQuery.ToList();
                userRegistrationModel.ErrorMessage = "User Name is already Exist";
                userRegistrationModel.Questionnaire1 = v2;
                return View(userRegistrationModel);
            }
            userRegistrationModel.IsActive = true;
            userRegistrationModel.UpdatedBy = 1;
            userRegistrationModel.CreatedBy = 1;
            userRegistrationModel.CreatedOn = DateTime.Now;
            userRegistrationModel.UpdatedOn = DateTime.Now;
            userRegistrationModel.UserRoleIdId = _context.m_Roles.FirstOrDefault(a => a.RoleName == "End User")?.Id;
            userRegistrationModel.Passcode = EncryptDecrypt.Encrypt(userRegistrationModel.Passcode, BaseSessionModel.SecrateKey);

            if (ModelState.IsValid)
            {
                _context.Add(userRegistrationModel);
                await _context.SaveChangesAsync();
                if (_contextAccessor.HttpContext != null && !string.IsNullOrEmpty(_contextAccessor.HttpContext.Session.GetString("RoleName")) && _contextAccessor.HttpContext.Session.GetString("RoleName") == "Admin")
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Login));
            }
            userRegistrationModel.ErrorMessage = "Please fill required details..";
            return View(userRegistrationModel);
        }

        // GET: UserRegistration/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.t_UserRegistration == null)
            {
                return NotFound();
            }

            var userRegistrationModel = await _context.t_UserRegistration.FindAsync(id);
            if (userRegistrationModel == null)
            {
                return NotFound();
            }

            var lstRole = new SelectList(_context.m_Roles, "Id", "RoleName");
            var v1 = (List<SelectListItem>)lstRole.ToList();
            userRegistrationModel.Roles = v1;

            var lstProjects = _context.t_ProjectTable.Select(a => new SelectListItem { Text = a.ProjectName, Value = a.Id.ToString() }).ToList();
            //var lstProjects = new SelectList(_context.t_ProjectTable, "Id", "ProjectName");
            //var v2 = (List<SelectListItem>)lstProjects.ToList();
            userRegistrationModel.Projects = lstProjects;

            return View(userRegistrationModel);
        }

        // POST: UserRegistration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserRegistrationModel userRegistrationModel)
        //public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,UserName,Passcode,EmailId,Narration,PasscodeRecoveryAnswer1")] UserRegistrationModel userRegistrationModel)
        {
            userRegistrationModel.IsActive = true;
            userRegistrationModel.UpdatedBy = 1;
            userRegistrationModel.CreatedBy = 1;
            userRegistrationModel.CreatedOn = DateTime.Now;
            userRegistrationModel.UpdatedOn = DateTime.Now;
            if (id != userRegistrationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (userRegistrationModel != null)
                    {
                        _context.Update(userRegistrationModel);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRegistrationModelExists(userRegistrationModel.Id))
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
            return View(userRegistrationModel);
        }

        // GET: UserRegistration/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.t_UserRegistration == null)
            {
                return NotFound();
            }

            var userRegistrationModel = await _context.t_UserRegistration
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRegistrationModel == null)
            {
                return NotFound();
            }

            return View(userRegistrationModel);
        }

        // POST: UserRegistration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.t_UserRegistration == null)
            {
                return Problem("Entity set 'GasStationProjectDbContext.t_UserRegistration'  is null.");
            }
            var userRegistrationModel = await _context.t_UserRegistration.FindAsync(id);
            if (userRegistrationModel != null)
            {
                _context.t_UserRegistration.Remove(userRegistrationModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Users/Create
        public IActionResult Login()
        {
            //HttpContext.Session.Clear();
            if (_contextAccessor.HttpContext != null)
            {
                _contextAccessor.HttpContext.Session.Clear();
            }

            ViewBag.ShowAppHeader = false;
            BaseSessionModel.UserRole = string.Empty;
            BaseSessionModel.IsValidUser = false;
            BaseSessionModel.UserName = string.Empty;
            return View();
        }
        //POST: Users/Login
        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,Passcode")] LoginModel loginModel)
        {
            BaseSessionModel.IsValidUser = false;
            if (!string.IsNullOrWhiteSpace(loginModel.UserName) && !string.IsNullOrWhiteSpace(loginModel.Passcode))
            {
                var encryptPascode = EncryptDecrypt.Encrypt(loginModel.Passcode, BaseSessionModel.SecrateKey);
                var _user = _context.t_UserRegistration
                    .FirstOrDefault(a => a.UserName.ToLower() == loginModel.UserName.ToLower() && a.Passcode == encryptPascode);
                if (_user != null)
                {
                    var role = _context.m_Roles.FirstOrDefault(a => a.Id == _user.UserRoleIdId);
                    if (_contextAccessor != null && _contextAccessor.HttpContext != null)
                    {
                        if (role != null)
                        {
                            BaseSessionModel.UserPK = _user.Id;
                            BaseSessionModel.UserRole = role.RoleName;
                            BaseSessionModel.IsValidUser = true;
                            _contextAccessor.HttpContext.Session.SetString("RoleName", role.RoleName);
                        }
                        _contextAccessor.HttpContext.Session.SetInt32("validUser", 1);
                        _contextAccessor.HttpContext.Session.SetInt32("UserPK", Convert.ToInt32(_user.Id));
                        _contextAccessor.HttpContext.Session.SetString("UserName", $"Welcome, {_user.LastName}");

                    }

                    BaseSessionModel.UserName = _user.LastName;


                    return RedirectToAction("Index", "Projects");
                }
            }

            if (_contextAccessor.HttpContext != null)
            {
                _contextAccessor.HttpContext.Session.Clear();
            }

            return View();
        }

        private bool UserRegistrationModelExists(int id)
        {
            return _context.t_UserRegistration.Any(e => e.Id == id);
        }
    }
}
