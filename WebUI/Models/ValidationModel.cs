namespace WebUI.Models;

public class ValidationModel
{
    public ValidationModel(bool isValid, string message)
    {
        this.IsValid = isValid;
        this.Message = message;
    }

    public bool IsValid { get; set; }
    public string Message { get; set; }
}