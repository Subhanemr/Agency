using Agency.Areas.Admin.ViewModels;
using Agency.DAL;
using Agency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View(register);
            AppUser user = new AppUser
            {
                UserName = register.UserName,
                Email = register.Email,
                Name = register.Name,
                Surname = register.Surname,
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, "Username, Email or Password not correct");
                }
                return View(result);
            }

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        [AutoValidateAntiforgeryToken]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInVM logIn)
        {
            if (!ModelState.IsValid) return View(logIn);
            AppUser user = await _userManager.FindByNameAsync(logIn.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(logIn.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username, Email or Password not correct");
                    return View(logIn);
                }

            }
            var result = await _signInManager.PasswordSignInAsync(user, logIn.Password, false, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Loked Outed");
                return View(logIn);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username, Email or Password not correct");
                return View(result);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
