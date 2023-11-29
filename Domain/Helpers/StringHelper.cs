using PasswordGenerator;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Domain.Helpers;

public static class StringHelper
{
    #region readonly variable

    private static readonly string[] VietnameseSigns =
    {
        "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
    };

    private static readonly string SpecialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
    private static readonly string UpperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly string LowerCaseCharacters = "abcdefghijklmnopqrstuvwxyz";
    private static readonly string NumberCharacters = "0123456789";

    #endregion readonly variable

    public static string PasswordGenerate()
    {
        var pwd = new Password()
            .IncludeLowercase()
            .IncludeUppercase()
            .IncludeSpecial()
            .IncludeNumeric()
            .LengthRequired(10);

        return pwd.Next();
    }

    public static bool ContainsAnyCase(string str, char character)
    {
        return str.IndexOf(character, StringComparison.CurrentCultureIgnoreCase) != -1;
    }

    public static string ToCurrencyMoney(this string str, decimal money)
    {
        if (ContainsAnyCase(str, '$'))
        {
            return "$" + money.FormatMoney().Split(".")[0];
        }
        else
        {
            return money.FormatMoney().Split(".")[0] + "đ";
        }
    }
    
    public static string ToCurrencyMoneyVN(this double money)
    {
        return money.FormatMoney().Split(".")[0] + "đ";
    }

    public static string ToDatetimeString(this DateTime date)
    {
        return date.ToString("MM/dd/yyyy hh:mm tt");
    }

    public static string ToDatetimeStringNullable(this DateTime? date)
    {
        var text = date?.ToString("MM/dd/yyyy hh:mm tt");

        return string.IsNullOrEmpty(text) ? "" : text;
    }

    public static string VietnameseToNormalString(this string str)
    {
        for (int i = 1; i < VietnameseSigns.Length; i++)
        {
            for (int j = 0; j < VietnameseSigns[i].Length; j++)
            {
                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
        }

        return str;
    }

    public static string RandomString()
    {
        char[] stringChars = new char[3];
        Random random = new Random();

        for (int i = 0; i < 1; i++)
        {
            stringChars[i] = NumberCharacters[random.Next(NumberCharacters.Length)];
        }

        for (int i = 1; i < 2; i++)
        {
            stringChars[i] = LowerCaseCharacters[random.Next(LowerCaseCharacters.Length)];
        }

        for (int i = 2; i < 3; i++)
        {
            stringChars[i] = UpperCaseCharacters[random.Next(UpperCaseCharacters.Length)];
        }

        return new string(stringChars);
    }
    
    public static string UniqueUrl(this string title)
    {
        string guid = Guid.NewGuid().ToString().Split("-")[1];
        string url = ToStringCode(title);
        return $"{url}-{guid}{RandomString()}";
    }
    
    public static string RemoveAllNumber(this string input)
    {
        return Regex.Replace(input, @"[\d-]", string.Empty);
        ;
    }

    public static string ToStringCode(this string str)
    {
        return VietnameseToNormalString(str).ToLower().Replace(' ', '-');
    }

    public static string FormatMoney(this decimal total)
    {
        return $"{total:C}".Replace("$", string.Empty).Split(".").First();
    }
    
    public static string FormatMoney(this double total)
    {
        return $"{total:C}".Replace("$", string.Empty).Split(".").First();
    }

    public static decimal CalDiscountPrice(this decimal current, decimal discount)
    {
        var discountPrice = current * (discount / (decimal)100.0);
        var priceAfterMinus = current - discountPrice;
        return priceAfterMinus;
    }
}