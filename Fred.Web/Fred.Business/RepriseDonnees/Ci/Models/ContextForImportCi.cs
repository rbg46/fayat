using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;

namespace Fred.Business.RepriseDonnees.Ci.Models
{
    /// <summary>
    /// Contient les données necessaire pour faire l'import des ci
    /// </summary>
    public class ContextForImportCi
    {
        /// <summary>
        /// Societes du groupe selectionné  dans l'interface fred ie
        /// </summary>
        public List<OrganisationBase> SocietesOfGroupe { get; set; }
        /// <summary>
        /// l'arbre des Organisation de fayat
        /// </summary>
        public OrganisationTree OrganisationTree { get; set; }
        /// <summary>
        /// Leas pays utilisé pour l'import des cis
        /// </summary>
        public List<PaysEnt> PaysUsedInExcel { get; set; }
        /// <summary>
        /// les ci utiliser pour l'import des cis
        /// </summary>
        public List<CIEnt> CisUsedInExcel { get; set; }
        /// <summary>
        /// les personnels utilisé pour l'import des cis
        /// </summary>
        public List<PersonnelEnt> PersonnelsUsedInExcel { get; set; }
    }
}
