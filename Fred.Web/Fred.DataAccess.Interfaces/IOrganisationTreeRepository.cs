using Fred.Entities.Organisation.Tree;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repository qui permet de recuperer les organisation ainsi que les affections
    /// </summary>
    public interface IOrganisationTreeRepository : IMultipleRepository
    {
        /// <summary>
        ///   Renvoi la liste des organisations d'un Utilisateur
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="types">Types d'organisation</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="organisationIdPere">Identifiant de l'organisation parente</param>
        /// <returns>Une liste d'organisations</returns>
        System.Collections.Generic.IEnumerable<Entities.Organisation.OrganisationLightEnt> GetOrganisationsAvailable(
                    string text = null,
                    System.Collections.Generic.List<int> types = null,
                    int? utilisateurId = null,
                    int? organisationIdPere = null);

        /// <summary>
        /// Retourne l'arbre des organisations de Fayat
        /// Construit l"abre des organisations de fred avec les organisations et les affectations de tous les utilisateurs
        /// IL NE FAUT PAS UTILISER CETTE FONCTIONS DANS UNE BOUCLE FOR OU FOREACH
        /// IL NE FAUT PAS RENVOYER L'OBJECT RETOURNER AU FRONT SAUF SI C'EST SUR UNE PAGE SECURISÉE => SUPER ADMIN SEULEMENT
        /// </summary>
        /// <returns>l'arbre des organisations de Fayat</returns>
        OrganisationTree GetOrganisationTree();
    }
}
