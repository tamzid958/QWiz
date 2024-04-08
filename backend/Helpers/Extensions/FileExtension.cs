using System.Text.RegularExpressions;
using Microsoft.Net.Http.Headers;

namespace QWiz.Helpers.Extensions;

public class FileExtension(IWebHostEnvironment environment)
{
    private static string MakeValidFileName(string name)
    {
        var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
        var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)|\s+", invalidChars);

        return $"{Guid.NewGuid()}___{Regex.Replace(name, invalidRegStr, "_")}".ToLower();
    }

    public async Task<string> FileUploadAsync(IFormFile file)
    {
        if (file.Length <= 0) throw new FileLoadException("file load failed");
        var fileName =
            MakeValidFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString());
        var fullPath = Path.Combine(environment.WebRootPath, fileName);
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        await stream.DisposeAsync();
        return Path.Combine(fileName);
    }

    public void FileDelete(string path)
    {
        if (!File.Exists(Path.Combine(environment.WebRootPath, path)))
            throw new FileNotFoundException("file not found");
        File.Delete(Path.Combine(environment.WebRootPath, path));
    }
}