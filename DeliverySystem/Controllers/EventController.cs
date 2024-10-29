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
    [Authorize(Roles = "Employee, Admin")] // Både Employee och Admin har tillgång
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Employee> _userManager;

        public EventController(AppDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Event/Create
        public IActionResult Create(int driverId)
        {
            var driver = _context.Drivers.FirstOrDefault(d => d.DriverID == driverId);
            if (driver == null) return NotFound();

            var model = new Event { DriverID = driverId, NoteDate = DateTime.Now };
            ViewBag.DriverName = driver.DriverName;
            return View(model);
        }

        // POST: Event/Create
        [HttpPost]
        public async Task<IActionResult> Create(Event model)
        {
            if (ModelState.IsValid)
            {
                // Hämta inloggad användare
                var currentEmployee = await _userManager.GetUserAsync(User);
                if (currentEmployee == null)
                {
                    return Unauthorized();
                }

                // Tilldela ResponsibleEmployee och DriverID
                model.ResponsibleEmployee = currentEmployee.Name;
                model.DriverID = model.DriverID; // Detta fält är redan satt via GET-metoden

                // Spara händelsen
                _context.Events.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Driver", new { id = model.DriverID });
            }

            var driver = _context.Drivers.FirstOrDefault(d => d.DriverID == model.DriverID);
            ViewBag.DriverName = driver?.DriverName;
            return View(model);
        }
    }
}
