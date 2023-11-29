using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebUI.Models;

namespace WebUI.WebHelper;

public static class ValidationHelper<T>
{
    public static ValidationModel IsValid(T t)
    {
        // Get all property of T
        var props = t?.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (props == null) return new ValidationModel(true, "Model is valid.");
        foreach (var prop in props)
        {
            var attr = prop.GetCustomAttributes<RequiredAttribute>();
            if (attr.ToList().Count <= 0) continue;
            var propName = prop.Name;
            var propValue = prop.GetValue(t);
            if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
            {
                return new ValidationModel(false, $"{propName} is required.");
            }
        }

        return new ValidationModel(true, "Model is valid.");
    }
}