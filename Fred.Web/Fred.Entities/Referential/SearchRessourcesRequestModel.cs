namespace Fred.Entities.Referential
{
    public class SearchRessourcesRequestModel
    {
        public int RessourcesRecommandeesOnly { get; set; }
        public string Recherche2 { get; set; }
        public string Recherche { get; set; }
        public int? RessourceIdNatureFilter { get; set; }
        public int SocieteId { get; set; }
        public int CiId { get; set; }
        public int EtablissementOrganisationId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
