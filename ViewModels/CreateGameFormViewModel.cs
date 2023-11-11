namespace GameZone.ViewModels
{
	public class CreateGameFormViewModel: GameFormViewModel
	{
		[AllowedExtensions(FileSettings.AllowedExtensions)]
		[MaxFileSize(FileSettings.MaxFileInBytes)]
		public IFormFile Cover { get; set; } = default!;
	}
}
