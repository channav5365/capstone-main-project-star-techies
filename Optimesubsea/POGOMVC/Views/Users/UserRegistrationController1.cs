//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using POGOMVC.DataLayer;
//using POGOMVC.Models;

//namespace POGOMVC.Views.Users
//{
//    public class UserRegistrationController1 : Controller
//    {
//        private readonly GasStationProjectDbContext _context;

//        public UserRegistrationController1(GasStationProjectDbContext context)
//        {
//            _context = context;
//        }

//        // GET: UserRegistration
//        public async Task<IActionResult> Index()
//        {
//              return View(await _context.t_UserRegistration.ToListAsync());
//        }

//        // GET: UserRegistration/Details/5
//        public async Task<IActionResult> Details(long? id)
//        {
//            if (id == null || _context.t_UserRegistration == null)
//            {
//                return NotFound();
//            }

//            var userRegistrationModel = await _context.t_UserRegistration
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (userRegistrationModel == null)
//            {
//                return NotFound();
//            }

//            return View(userRegistrationModel);
//        }

//        // GET: UserRegistration/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: UserRegistration/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,UserId,UserName,Passcode,EmailId,Narration,IsActive,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy")] UserRegistrationModel userRegistrationModel)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(userRegistrationModel);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(userRegistrationModel);
//        }

//        // GET: UserRegistration/Edit/5
//        public async Task<IActionResult> Edit(long? id)
//        {
//            if (id == null || _context.t_UserRegistration == null)
//            {
//                return NotFound();
//            }

//            var userRegistrationModel = await _context.t_UserRegistration.FindAsync(id);
//            if (userRegistrationModel == null)
//            {
//                return NotFound();
//            }
//            return View(userRegistrationModel);
//        }

//        // POST: UserRegistration/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,UserName,Passcode,EmailId,Narration,IsActive,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy")] UserRegistrationModel userRegistrationModel)
//        {
//            if (id != userRegistrationModel.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(userRegistrationModel);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!UserRegistrationModelExists(userRegistrationModel.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(userRegistrationModel);
//        }

//        // GET: UserRegistration/Delete/5
//        public async Task<IActionResult> Delete(long? id)
//        {
//            if (id == null || _context.t_UserRegistration == null)
//            {
//                return NotFound();
//            }

//            var userRegistrationModel = await _context.t_UserRegistration
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (userRegistrationModel == null)
//            {
//                return NotFound();
//            }

//            return View(userRegistrationModel);
//        }

//        // POST: UserRegistration/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(long id)
//        {
//            if (_context.t_UserRegistration == null)
//            {
//                return Problem("Entity set 'GasStationProjectDbContext.t_UserRegistration'  is null.");
//            }
//            var userRegistrationModel = await _context.t_UserRegistration.FindAsync(id);
//            if (userRegistrationModel != null)
//            {
//                _context.t_UserRegistration.Remove(userRegistrationModel);
//            }
            
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }
//        // GET: Users/Create
//        public IActionResult Login()
//        {
//            ViewBag.ShowAppHeader = false;
//            return View();
//        }
//        //POST: Users/Login
//        [HttpPost, ActionName("Login")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login([Bind("UserName,Passcode")] LoginModel loginModel)
//        {
//            if (!string.IsNullOrWhiteSpace(loginModel.UserName) && !string.IsNullOrWhiteSpace(loginModel.Passcode))
//            {
//                var _user = _context.t_UserRegistration
//                    .FirstOrDefault(a => a.UserName.ToLower() == loginModel.UserName.ToLower() && a.Passcode == loginModel.Passcode);
//                if (_user != null)
//                {
//                    TempData["ShowAppHeader"] = "Y";
//                    TempData["UserName"] = $"Welcome, { _user.UserName}";
//                    ViewBag.ShowAppHeader = true;
//                    return RedirectToAction("Index", "Projects");
//                }
//            }
            
//            //if (loginModel.UserName == "admin" && loginModel.Passcode == "admin")
//            //{
//            //    //return RedirectToPage("./Projects/Index");
//            //}
//            TempData["ShowAppHeader"] = "N";
//            TempData["UserName"] = "";
//            ViewBag.ShowAppHeader = false;
//            return View();
//            //return RedirectToPage("/Users/Login");
//            //return View(loginModel);
//        }

//        private bool UserRegistrationModelExists(long id)
//        {
//          return _context.t_UserRegistration.Any(e => e.Id == id);
//        }
//    }
//}
