namespace GameZone.ViewModels
{
    public class EditGameFormViewModel: GameFormViewModel
    {
        public int Id { get; set; }

        public string? currentCover { get; set; }

        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [MaxFileSize(FileSettings.MaxFileInBytes)]
        public IFormFile? Cover { get; set; } = default!;
    }
}
