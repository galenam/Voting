using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public static class Helper
{
    static Regex regexLettersNumbers = new Regex("\\W");
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
}