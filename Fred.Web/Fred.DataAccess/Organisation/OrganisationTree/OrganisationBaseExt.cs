using Fred.Entities;
using Fred.Entities.Organisation.Tree;
using Fred.Framework.Extensions;

namespace Fred.DataAccess.OrganisationTree
{

    /// <summary>
    /// Methode d'extension pour l'entite OrganisationBase
    /// </summary>
    public static class OrganisationBaseExt
    {
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est une holding
        /// </summary>
        /// <param name="organisation">OrganisationBase dont on cherche sont type</param>
        /// <returns>Vraie si l'organisationBase est une holding</returns>
        public static bool IsHolding(this OrganisationBase organisation)
        {
            return organisation.TypeOrganisationId == OrganisationType.Holding.ToIntValue();
        }

        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un pole
        /// </summary>
        /// <param name="organisation">OrganisationBase dont on cherche sont type</param>
        /// <returns>Vraie si l'organisationBase est un pole</returns>
        public static bool IsPole(this OrganisationBase organisation)
        {
            return organisation.TypeOrganisationId == OrganisationType.Pole.ToIntValue();
        }

        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un groupe
        /// </summary>
        /// <param name="organisation">OrganisationBase dont on cherche sont type</param>
        /// <returns>Vraie si l'organisationBase est un groupe</returns>
        public static bool IsGroupe(this OrganisationBase organisation)
        {
            return organisation.TypeOrganisationId == OrganisationType.Groupe.ToIntValue();
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est une societe
        /// </summary>
        /// <param name="organisation">OrganisationBase dont on cherche sont type</param>
        /// <returns>Vraie si l'organisationBase est une societe</returns>
        public static bool IsSociete(this OrganisationBase organisation)
        {
            return organisation.TypeOrganisationId == OrganisationType.Societe.ToIntValue();
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un PUO
        /// </summary>
        /// <param name="organisation">OrganisationBase dont on cherche sont type</param>
        /// <returns>Vraie si l'organisationBase est une PUO</returns>
        public static bool IsPuo(this OrganisationBase organisation)
        {
            return organisation.TypeOrganisationId == OrganisationType.Puo.ToIntValue();
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un UO 
        /// </summary>
        /// <param name="organisation">OrganisationBase dont on cherche sont type</param>
        /// <returns>Vraie si l'organisationBase est un Uo</returns>
        public static bool IsUo(this OrganisationBase organisation)
        {
            return organisation.TypeOrganisationId == OrganisationType.Uo.ToIntValue();
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est une Etablissement
        /// </summary>
        /// <param name="organisation">OrganisationBase dont on cherche sont type</param>
        /// <returns>Vraie si l'organisationBase est un Etablissement</returns>
        public static bool IsEtablissement(this OrganisationBase organisation)
        {
            return organisation.TypeOrganisationId == OrganisationType.Etablissement.ToIntValue();
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un CI
        /// </summary>
        /// <param name="organisation">OrganisationBase dont on cherche sont type</param>
        /// <returns>Vraie si l'organisationBase est un CI</returns>
        public static bool IsCi(this OrganisationBase organisation)
        {
            return organisation.TypeOrganisationId == OrganisationType.Ci.ToIntValue();
        }
        /// <summary>
        /// Permet de savoir si l'OrganisationBase est un sous CI
        /// </summary>
        /// <param name="organisation">OrganisationBase dont on cherche sont type</param>
        /// <returns>Vraie si l'organisationBase est un sous CIsous CI</returns>
        public static bool IsSousCi(this OrganisationBase organisation)
        {
            return organisation.TypeOrganisationId == OrganisationType.SousCi.ToIntValue();
        }
    }

}
