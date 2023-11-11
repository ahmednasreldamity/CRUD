namespace GameZone.Controllers
{

    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(RoleViewModel dataRrole)
        {
            if (ModelState.IsValid == true)
            {
                IdentityRole roleModel = new()
                {
                    Name = dataRrole.RoleName
                };

                var result = await _roleManager.CreateAsync(roleModel);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
				}
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(dataRrole);
        }
    }
}
