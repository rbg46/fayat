namespace Fred.Web.Shared.Models.Materiel.Search
{
    public class MaterielSearchModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string EtablissementComptable { get; set; }
        public bool IsActif { get; set; }
        public bool IsStorm { get; set; }
    }
}
