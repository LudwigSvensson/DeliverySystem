using DriverInfo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeliverySystem.Controllers
{
    [Authorize(Roles = "Admin")] // Endast Admin-rollen har tillgång
    public class EmployeeController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeController(UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            return View(employees);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Employee model, string password)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee { UserName = model.Email, Email = model.Email, Name = model.Name };
                var result = await _userManager.CreateAsync(employee, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(employee, "Employee");
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Edit(string id)
        {
            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee model)
        {
            var employee = await _userManager.FindByIdAsync(model.Id);
            if (employee == null) return NotFound();

            employee.UserName = model.Email;
            employee.Email = model.Email;
            employee.Name = model.Name;

            var result = await _userManager.UpdateAsync(employee);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null) return NotFound();

            var result = await _userManager.DeleteAsync(employee);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something went wrong while deleting this employee.");
            return View(employee);
        }
    }
}

