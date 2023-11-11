namespace GameZone.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel dataUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    UserName = dataUser.UserName,
                    PasswordHash = dataUser.Password,
                    Address = dataUser.Address,
                };

                var result = await _userManager.CreateAsync(user, dataUser.Password);

                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, "Student");

                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("index", "home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                }
            }

            return View(dataUser);
        }


        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInUserViewModel dataUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(dataUser.UserName);

                if (user != null) // user name exist
                {
                    bool check = await _userManager.CheckPasswordAsync(user, dataUser.Password);

                    if (check) // valid password
                    {
                        await _signInManager.SignInAsync(user, dataUser.RememberMe);
                        return RedirectToAction("index", "home");
                    }
                    else // Invalid password
                    {
                        ModelState.AddModelError("Password", "Password Is Wrong!");
                    }
                }
                else // user name not exist
                {
                    ModelState.AddModelError("UserName", "User Name Is Wrong!");
                }
            }

            return View(dataUser);
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(RegisterUserViewModel dataUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    UserName = dataUser.UserName,
                    Address = dataUser.Address,
                    PasswordHash = dataUser.Password,
                };

                var result = await _userManager.CreateAsync(user, dataUser.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");

                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("index", "games");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                }
            }

            return View(dataUser);
        }
    }
}
