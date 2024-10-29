using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliverySystem.Data;
using DriverInfo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DeliverySystem.Controllers
{
    [Authorize(Roles ="Admin, Employee")]
    public class DriverController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Employee> _userManager;

        public DriverController(AppDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var currentEmployee = await _userManager.GetUserAsync(User);
            if (currentEmployee == null) return Unauthorized();

            IQueryable<Driver> drivers;

            // Om användaren är Admin, visa alla förare
            if (User.IsInRole("Admin"))
            {
                drivers = _context.Drivers.Include(d => d.ResponsibleEmployee);
            }
            else
            {
                // Om användaren är en Employee, visa endast förare skapade av denna Employee
                drivers = _context.Drivers
                    .Where(d => d.ResponsibleEmployeeId == currentEmployee.Id)
                    .Include(d => d.ResponsibleEmployee);
            }

            return View(await drivers.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.Events)
                .FirstOrDefaultAsync(m => m.DriverID == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }


        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.Employees = new SelectList(await _userManager.Users.ToListAsync(), "Id", "Name");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Driver driver)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin") && !string.IsNullOrEmpty(driver.ResponsibleEmployeeId))
                {

                }
                else
                {
                    var currentEmployee = await _userManager.GetUserAsync(User);
                    if (currentEmployee == null) return Unauthorized();

                    driver.ResponsibleEmployeeId = currentEmployee.Id;
                }

                _context.Drivers.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DriverID,DriverName,CarReg")] Driver driver)
        {
            if (id != driver.DriverID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.DriverID))
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
            return View(driver);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(m => m.DriverID == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.DriverID == id);
        }
    }
}
