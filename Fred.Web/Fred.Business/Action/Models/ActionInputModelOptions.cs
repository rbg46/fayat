namespace Fred.Business.Action.Models
{
    public class ActionInputModelOptions
    {
        public bool ConcatMessage { get; set; } = true;

        public string ConcatSeparator { get; set; } = " - ";

        public static ActionInputModelOptions Default => new ActionInputModelOptions();

        public static ActionInputModelOptions NoConcat => new ActionInputModelOptions()
        {
            ConcatMessage = false
        };

        public static ActionInputModelOptions ConcatLine => new ActionInputModelOptions()
        {
            ConcatMessage = true,
            ConcatSeparator = "\n"
        };
    }
}
