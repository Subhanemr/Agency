namespace Agency.Utilities.Extentions
{
    public static class FileValidator
    {
        public static bool IsValid(this IFormFile file, string image = "image/")
        {
            if (file.ContentType.Contains(image)) return true;
            return false;
        }
        public static bool LimitSize(this IFormFile file, int limit = 10)
        {
            if (file.Length <= limit * 1024 * 1024) return true;
            return false;
        }
        public static async Task<string> CrateFileAsync(this IFormFile file, string root, params string[] folders)
        {
            string originalName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string guidName = GetGuid(originalName);
            string fileFormat = GetFormat(originalName);
            string finalFileName = guidName + fileFormat;
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, finalFileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return finalFileName;
        }
        public static async void DeleteFileAsync(this string fileName, string root, params string[] folders)
        {
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, fileName);

            if(File.Exists(path)) File.Delete(path);
        }
        public static string GetGuid(string fullFileName)
        {
            int score = fullFileName.LastIndexOf("_");
            if (score < 0)
            {
                string guidName = fullFileName.Substring(0, score);
                return guidName;
            }
            return fullFileName;
        }
        public static string GetFormat(string fullFileName)
        {
            int score = fullFileName.LastIndexOf(".");
            if (score < 0)
            {
                string fileType = fullFileName.Substring(score);
                return fileType;
            }
            return fullFileName;
        }
    }
}
