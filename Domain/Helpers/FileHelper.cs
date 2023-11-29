namespace Domain.Helpers;

public static class FileHelper
{
    private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
    
    public static async Task<string> ReadToEndAsync(string path)
    {
        using StreamReader reader = new(path);
        string bodyContent = await reader.ReadToEndAsync();

        return bodyContent;
    }
}