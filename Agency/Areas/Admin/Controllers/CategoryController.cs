using Agency.Areas.Admin.ViewModels;
using Agency.DAL;
using Agency.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agency.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Category> categories = await _context.Categories.Include(x => x.Products).ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM create)
        {
            if(!ModelState.IsValid) return View(create);
            bool result = await _context.Categories.AnyAsync(x => x.Name.ToLower().Trim() == create.Name.ToLower().Trim());
            if(result)
            {
                ModelState.AddModelError("Name", "Name is exists");
                return View(create);
            }
            Category category = new Category { Name = create.Name };
            
            await _context.Categories.AddAsync(category);   
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            UpdateCategoryVM update = new UpdateCategoryVM { Name = category.Name };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateCategoryVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();

            bool result = await _context.Categories.AnyAsync(c => c.Name.ToLower().Trim() == update.Name.ToLower().Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError("Name", "A Category is available");
                return View(update);
            }

            existed.Name = update.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Categories.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
