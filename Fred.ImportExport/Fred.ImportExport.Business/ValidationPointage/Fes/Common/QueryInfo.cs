namespace Fred.ImportExport.Business.ValidationPointage.Fes.Common
{
    public class QueryInfo
    {
        public bool IsComment
        {
            get
            {
                return !string.IsNullOrEmpty(this.Comment);
            }
        }

        public string Comment { get; set; }

        public string Query { get; set; }

        public bool IsPointage { get; set; }

        public override string ToString()
        {
            return IsComment ? Comment : Query;
        }
    }
}
