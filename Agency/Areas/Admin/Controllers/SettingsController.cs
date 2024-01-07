using Agency.Areas.Admin.ViewModels;
using Agency.DAL;
using Agency.Models;
using Agency.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agency.Areas.Admin.Controllers
{
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page)
        {
            if (page < 0) return BadRequest();
            double count = await _context.Settings.CountAsync();
            ICollection<Settings> settings = await _context.Settings.Skip(page * 4).Take(4)
                .ToListAsync();
            

            PaginationVM<Settings> paginationVM = new PaginationVM<Settings>
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 4),
                Items = settings
            };
            if (paginationVM.TotalPage < page) return NotFound();

            return View(paginationVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSettingsVM create)
        {
            if (!ModelState.IsValid) return View(create);

            bool result = await _context.Settings.AnyAsync(c => c.Key.ToLower().Trim() == create.Key.ToLower().Trim());

            if (result)
            {
                ModelState.AddModelError("Key", "A Key with this name already exists");
                return View(create);
            }

            Settings settings = new Settings { Key = create.Key, Value = create.Value };

            await _context.Settings.AddAsync(settings);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest(); 

            Settings settings = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (settings == null) return NotFound();
            UpdateSettingsVM update = new UpdateSettingsVM { Key = settings.Key, Value = settings.Value };

            return View(update);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingsVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Settings settings = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (settings == null) return NotFound();

            bool result = await _context.Settings.AnyAsync(c => c.Key.ToLower().Trim() == update.Key.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Key", "A Key with this name already exists");
                return View(update);
            }

            settings.Key = update.Key;
            settings.Value = update.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Settings settings = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (settings == null) return NotFound();
            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
