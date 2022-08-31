using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using Models;
using Tesseract;

public static class Helper
{
    static Regex regexLettersNumbers = new Regex("\\W");
    static Regex regexWords = new Regex("[^а-яА-Я ]");
    static Regex nonIntRegex = new Regex("\\D");
    static Regex decimalRegex = new Regex("\\D*(?<first>\\d+)(^,)*(?<sign>.|,?)\\D*(?<last>\\d*)\\D*");

    public static string CleanString(this string value)
    {
        var result = value.Trim();
        if (regexWords.IsMatch(value))
        {
            return regexWords.Replace(result, "");
        }
        return result.Trim();
    }

    public static string CleanIntString(this string value)
    {
        if (nonIntRegex.IsMatch(value))
        {
            return nonIntRegex.Replace(value, "");
        }
        return value;
    }

    public static string CleanDecimalString(this string value)
    {
        if (decimalRegex.IsMatch(value))
        {
            var groups = decimalRegex.Match(value).Groups;
            var delimeter = groups["sign"].Length > 0 ? "," : string.Empty;
            return $"{groups["first"]}{delimeter}{groups["last"]}";
        }
        return string.Empty;
    }
    public static T GetEnumValueByDisplayName<T>(this string rowData)
    where T : struct
    {
        var attributeName = rowData.ToLower();
        if (regexLettersNumbers.IsMatch(attributeName))
        {
            attributeName = regexLettersNumbers.Replace(attributeName, "");
        }
        var fInfos = typeof(T).GetFields();

        foreach (var fInfo in fInfos)
        {
            var attributes = (DisplayAttribute[])fInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                if (attributeName == attributes[0].Name)
                {
                    Enum.TryParse<T>(fInfo.Name, out T value);
                    return (T)value;
                }
            }
        }
        return default(T);
    }

    public static Rect GetRectFromCoordinates(this Coordinate c) => Rect.FromCoords(c.X1, c.Y1, c.X2, c.Y2);
}