using DeliverySystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliverySystem.Views.Shared.Components.NotificationIcon
{
    public class NotificationIconViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public NotificationIconViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var twelveHoursAgo = DateTime.Now.AddHours(-12);
            var recentEventCount = await _context.Events
                .Where(e => e.NoteDate >= twelveHoursAgo)
                .CountAsync();

            return View(recentEventCount);
        }

    }
}
