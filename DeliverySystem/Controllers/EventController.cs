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
using System.Diagnostics.Metrics;

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
            if (User.IsInRole("Admin"))
            {
                var lastTwentyFourHours = DateTime.Now.AddHours(-24);

                var recentEvents24 = await _context.Events
                    .Where(e => e.NoteDate >= lastTwentyFourHours)
                    .OrderByDescending(e => e.NoteDate)
                    .Include(e => e.Driver)
                    .Include(e => e.ResponsibleEmployee)
                    .ToListAsync();

                return View(recentEvents24);
            }
            var currentEmployee = await _userManager.GetUserAsync(User);
            var twelveHoursAgo = DateTime.Now.AddHours(-12);

            var recentEvents = await _context.Events
                .Where(e => e.NoteDate >= twelveHoursAgo && e.Driver.ResponsibleEmployeeId == currentEmployee.Id)
                .Include(e => e.Driver)
                .Include(e => e.ResponsibleEmployee)
                .OrderByDescending(e => e.NoteDate)
                .ToListAsync();

            return View(recentEvents);
        }
        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event model)
        {
            if (id != model.EventID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Driver", new { id = model.DriverID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Events.Any(e => e.EventID == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Driver)
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem != null)
            {
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Driver", new { id = eventItem.DriverID });
        }
            public async Task<int> GetRecentEventCount()
        {
            
            if (User.IsInRole("Admin"))
            {
                var twentyFourHoursAgo = DateTime.Now.AddHours(-24);
                return await _context.Events
                    .Where(e => e.NoteDate >= twentyFourHoursAgo)
                    .CountAsync();
            } 
            var twelveHoursAgo = DateTime.Now.AddHours(-12);
            return await _context.Events
                .Where(e => e.NoteDate >= twelveHoursAgo)
                .CountAsync();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOverview(DateTime? startDate, DateTime? endDate, string driverName, string employeeName)
        {
            var query = _context.Events
                .Include(e => e.Driver)
                .Include(e => e.ResponsibleEmployee)
                .AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(e => e.NoteDate >= startDate && e.NoteDate <= endDate);
            }

            if (!string.IsNullOrEmpty(driverName))
            {
                query = query.Where(e => e.Driver.DriverName.Contains(driverName));
            }

            if (!string.IsNullOrEmpty(employeeName))
            {
                query = query.Where(e => e.ResponsibleEmployee.Name.Contains(employeeName));
            }
            var filteredEvents = await query.OrderByDescending(e => e.NoteDate).ToListAsync();

            return View(filteredEvents);
        }
    }
}
