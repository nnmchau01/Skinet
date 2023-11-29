using System.Text;

namespace Domain.Helpers;

public static class EmailHelper
{
    public static async Task<string> EmailTemplateBuilder(string pathEmailHeader, string pathEmailBody,
        params object[] parameters)
    {
        StringBuilder sb = new();

        string header = await FileHelper.ReadToEndAsync(pathEmailHeader);
        string body = await FileHelper.ReadToEndAsync(pathEmailBody);
        string newBody = string.Format(body, parameters);
        const string head = "<!DOCTYPE html>\n"
                            + "<html lang = \"en\">";
        const string end = "</html>";

        sb.Append(head);
        sb.Append(header);
        sb.Append(newBody);
        sb.Append(end);

        return sb.ToString();
    }
    
    public static async Task<string> EmailTemplateBuilderWithoutCss(string path, params object[] parameters)
    {
        StringBuilder sb = new();
        
        string content = await FileHelper.ReadToEndAsync(path);
        string newContent = string.Format(content, parameters);
        
        sb.Append(newContent);
        
        return sb.ToString();
    }
}