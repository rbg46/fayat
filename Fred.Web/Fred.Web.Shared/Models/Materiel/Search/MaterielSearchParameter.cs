namespace Fred.Web.Shared.Models.Materiel.Search
{
    public class MaterielSearchParameter
    {
        private const int DefaultPageSize = 20;

        public int SocieteId { get; set; }
        public string SearchText { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public MaterielSearchParameter()
        {
            PageSize = DefaultPageSize;
        }
    }
}