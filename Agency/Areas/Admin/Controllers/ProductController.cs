using Agency.Areas.Admin.ViewModels;
using Agency.DAL;
using Agency.Models;
using Agency.Utilities.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agency.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Product> products = await _context.Products.Include(c => c.Category).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            CreateProductVM create = new CreateProductVM
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM create)
        {
            if (!ModelState.IsValid)
            {
                create.Categories = await _context.Categories.ToListAsync();
                return View(create);
            }
            bool categoryResult = await _context.Categories.AnyAsync(x => x.Id == create.CategoryId);
            if (!categoryResult)
            {
                create.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("CategoryId", "This id has no category");
                return View(create);
            }
            if (!create.Photo.IsValid())
            {
                create.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("Photo", "File type not valid");
                return View(create);
            }
            if (!create.Photo.LimitSize())
            {
                create.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("Photo", "File size not be grater than 10MB");
                return View(create);
            }
            Product product = new Product
            {
                Name = create.Name,
                Price = create.Price,
                Description = create.Description,
                Img = await create.Photo.CrateFileAsync(_env.WebRootPath, "assets", "img"),
                CategoryId = create.CategoryId
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Product product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();
            UpdateProductVM update = new UpdateProductVM
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Img = product.Img,
                CategoryId = product.CategoryId,
                Categories = await _context.Categories.ToListAsync()
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProductVM update)
        {
            if (!ModelState.IsValid)
            {
                update.Categories = await _context.Categories.ToListAsync();
                return View(update);
            }
            Product existed = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
            if (existed == null) return NotFound();
            bool categoryResult = await _context.Categories.AnyAsync(x => x.Id == update.CategoryId);
            if (!categoryResult)
            {
                update.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("CategoryId", "This id has no category");
                return View(update);
            }
            if (update.Photo is not null)
            {
                if (!update.Photo.IsValid())
                {
                    update.Categories = await _context.Categories.ToListAsync();
                    ModelState.AddModelError("Photo", "File type not valid");
                    return View(update);
                }
                if (!update.Photo.LimitSize())
                {
                    update.Categories = await _context.Categories.ToListAsync();
                    ModelState.AddModelError("Photo", "File size not be grater than 10MB");
                    return View(update);
                }
                existed.Img.DeleteFileAsync(_env.WebRootPath, "assets", "img");
                existed.Img = await update.Photo.CrateFileAsync(_env.WebRootPath, "assets", "img");
            }
            existed.Name = update.Name;
            existed.Description = update.Description;
            existed.Price = update.Price;
            existed.CategoryId = (int)update.CategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Product product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();
            product.Img.DeleteFileAsync(_env.WebRootPath, "assets", "img");
            return RedirectToAction(nameof(Index));
        }


    }
}
