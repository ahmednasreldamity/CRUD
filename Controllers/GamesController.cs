namespace GameZone.Controllers
{

    [Authorize(Roles = "Admin")]
    public class GamesController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly ICategoriesService _CategoriesService;
		private readonly IDevicesService _DevicesService;
		private readonly IGamesService _gamesService;

		public GamesController(ApplicationDbContext context, ICategoriesService CategoriesService, 
            IDevicesService DevicesService, IGamesService gamesService)
        {
            _context = context;
			_CategoriesService = CategoriesService;
			_DevicesService = DevicesService;
            _gamesService = gamesService;
		}

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_gamesService.GetAll());
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);
            if (game is null)
                return NotFound();
            return View(game);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel moodel = new()
            {
                Categories = _CategoriesService.GetSelectList(),

				Devices = _DevicesService.GetSelectList(),
			};

            return View(moodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid) // server side validation
            {
                model.Categories = _CategoriesService.GetSelectList();

                model.Devices = _DevicesService.GetSelectList();

				return View(model);
            }

            await _gamesService.Create(model);
            return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
            var game = _gamesService.GetById(id);
            if (game is null)
                return NotFound();

            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _CategoriesService.GetSelectList(),
                Devices = _DevicesService.GetSelectList(),
                currentCover = game.Cover,
            };

            return View(viewModel);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditGameFormViewModel model)
		{
			if (!ModelState.IsValid) // server side validation
			{
				model.Categories = _CategoriesService.GetSelectList();

				model.Devices = _DevicesService.GetSelectList();

				return View(model);
			}

			var game = await _gamesService.Edit(model);

            if (game is null)
                return BadRequest();

			return RedirectToAction(nameof(Index));
		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
            var isDeleted = _gamesService.Delete(id);

            if (isDeleted) return Ok();

            return BadRequest();
		}
	}
}
