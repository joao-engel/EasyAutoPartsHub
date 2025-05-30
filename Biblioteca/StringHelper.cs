public static class StringHelper
{
    public static string FormataComoParam(this string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : $"%{input.Replace(" ", "%")}%";
    }

    public static string FormataTelComoParam(this string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : $"%{input.Replace(" ", "%")
                                                                    .Replace("-", "%")
                                                                    .Replace("(", "%")
                                                                    .Replace(")", "%")}%";
    }
}