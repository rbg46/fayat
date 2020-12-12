using Fred.Entities.Organisation.Tree;

namespace Fred.Business.Tests.Integration.Organisation
{
    internal class ArbreInfo
    {
        public OrganisationBase ciParent { get; set; }
        public string ciArbreCode { get; set; }
        public OrganisationBase etabParent { get; set; }
        public string etabParentOrganisationId { get; set; }
        public OrganisationBase societeParent { get; set; }
        public string societeParentOrganisationId { get; set; }
        public string ciArbreSocieteCode { get; set; }
        public string ciArbreCodeEtablissementComptable { get; set; }
        public OrganisationBase ciArbre { get; set; }
    }


}
