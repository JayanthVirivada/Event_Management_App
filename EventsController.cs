using EventManagement.Data;
using EventManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Controllers
{
    /// <summary>
    /// Handles all event management operations: Create, Read, Update, Delete.
    /// [Authorize] ensures only logged-in users can access any of these actions.
    /// </summary>
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ---------------------------------------------------------------
        // READ — Display all events for the logged-in user
        // GET: /Events
        // ---------------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            // Get only the events that belong to the current user
            string userId = _userManager.GetUserId(User)!;
            var events = (await _context.Events
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Date)
                .ToListAsync())
                .OrderBy(e => e.Date)
                .ThenBy(e => e.Time)
                .ToList();

            return View(events);
        }

        // ---------------------------------------------------------------
        // DETAILS — View a single event
        // GET: /Events/Details/5
        // ---------------------------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            string userId = _userManager.GetUserId(User)!;
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (@event == null) return NotFound();

            return View(@event);
        }

        // ---------------------------------------------------------------
        // CREATE (GET) — Show the empty create form
        // GET: /Events/Create
        // ---------------------------------------------------------------
        public IActionResult Create()
        {
            // Pre-fill date with today for convenience
            var model = new Event { Date = DateTime.Today };
            return View(model);
        }

        // ---------------------------------------------------------------
        // CREATE (POST) — Save the new event to the database
        // POST: /Events/Create
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Date,Time,Location")] Event @event)
        {
            if (ModelState.IsValid)
            {
                // Attach the current user as owner before saving
                @event.UserId = _userManager.GetUserId(User);
                _context.Add(@event);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Event \"{@event.Title}\" was created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // If validation failed, return the form with error messages
            return View(@event);
        }

        // ---------------------------------------------------------------
        // EDIT (GET) — Show the edit form pre-filled with existing data
        // GET: /Events/Edit/5
        // ---------------------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            string userId = _userManager.GetUserId(User)!;
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (@event == null) return NotFound();

            return View(@event);
        }

        // ---------------------------------------------------------------
        // EDIT (POST) — Save the updated event to the database
        // POST: /Events/Edit/5
        // ---------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Date,Time,Location")] Event @event)
        {
            if (id != @event.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Re-attach the user ID (it's not in the form)
                    @event.UserId = _userManager.GetUserId(User);
                    _context.Update(@event);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Event \"{@event.Title}\" was updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(@event);
        }

        // ---------------------------------------------------------------
        // DELETE (GET) — Show the delete confirmation page
        // GET: /Events/Delete/5
        // ---------------------------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            string userId = _userManager.GetUserId(User)!;
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (@event == null) return NotFound();

            return View(@event);
        }

        // ---------------------------------------------------------------
        // DELETE (POST) — Remove the event from the database
        // POST: /Events/Delete/5
        // ---------------------------------------------------------------
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = _userManager.GetUserId(User)!;
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (@event != null)
            {
                string title = @event.Title;
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Event \"{title}\" was deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ---------------------------------------------------------------
        // Helper — check if an event with this ID exists in the DB
        // ---------------------------------------------------------------
        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}