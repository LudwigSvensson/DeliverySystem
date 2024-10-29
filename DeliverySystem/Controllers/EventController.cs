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
                var currentEmployee = await _userManager.GetUserAsync(User);
                if (currentEmployee == null)
                {
                    return Unauthorized();
                }
                model.ResponsibleEmployeeid = currentEmployee.Id;

                _context.Events.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Driver", new { id = model.DriverID });
            }

            var driver = _context.Drivers.FirstOrDefault(d => d.DriverID == model.DriverID);
            ViewBag.DriverName = driver?.DriverName;
            return View(model);
        }
        public async Task<IActionResult> Notifications()
        {
            var lastTwelveHours = DateTime.Now.AddHours(-12);

            var recentEvents = await _context.Events
                .Where(e => e.NoteDate >= lastTwelveHours)
                .Include(e => e.Driver) 
                .Include(e => e.ResponsibleEmployee) 
                .ToListAsync();

            return View(recentEvents);
        }
        public async Task<int> GetRecentEventCount()
        {
            var twelveHoursAgo = DateTime.Now.AddHours(-12);
            return await _context.Events
                .Where(e => e.NoteDate >= twelveHoursAgo)
                .CountAsync();
        }
    }
}
