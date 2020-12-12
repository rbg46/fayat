namespace Fred.ImportExport.Business.Anael
{
    public static class StringExtensions
    {
        public static string FormatUsername(this string username) => username.PadRight(10, 'X');
    }
}
