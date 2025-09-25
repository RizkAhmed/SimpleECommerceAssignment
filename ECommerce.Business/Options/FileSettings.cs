namespace ECommerce.Business.Options
{
    public class FileSettings
    {
        public static string SectionName { get; set; } = "FileSettings";

        public long MaxFileSize { get; set; }
        public string[] AllowedImageExtensions { get; set; }
        public string[] AllowedFileExtensions { get; set; }
        public string ImageFolderPath { get; set; }
        public string FileFolderPath { get; set; }
    }

}
