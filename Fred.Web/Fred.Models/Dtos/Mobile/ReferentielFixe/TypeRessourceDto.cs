namespace Fred.Web.Dtos.Mobile.ReferentielFixe
{
    /// <summary>
    /// Dto type ressource
    /// </summary>
    public class TypeRessourceDto
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un Groupe.
        /// </summary>
        public int TypeRessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une OrgaGroupe.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'un Groupe.
        /// </summary>
        public string Libelle { get; set; }
    }
}