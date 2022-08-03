using System.Text.RegularExpressions;

namespace Services;

public static class Helper
{

    private static Regex digits = new Regex("^\\d+$");
    private static string intervalTemplate = "^(\\d+)-(\\d+)$";
    public static IEnumerable<int> GetPages(this string range)
    {
        var result = new List<int>();
        if (digits.IsMatch(range))
        {
            if (int.TryParse(range, out int value))
            {
                result.Add(value);
                return result;
            }
        }
        var matches = Regex.Matches(range, intervalTemplate);
        if (matches.Count != 1 && matches[0].Groups.Count != 3)
        {
            return result;
        }
        if (int.TryParse(matches[0].Groups[1].Value, out int start) && int.TryParse(matches[0].Groups[2].Value, out int finish)
            && start < finish)
        {
            result.AddRange(Enumerable.Range(start - 1, finish - start + 1));
        }
        return result;
    }
}