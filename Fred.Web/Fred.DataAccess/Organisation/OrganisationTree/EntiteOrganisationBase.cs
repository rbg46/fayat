namespace Fred.DataAccess.OrganisationTree
{
    /// <summary>
    /// Reprenste une entite de base de type organisation
    /// </summary>
    internal class EntiteOrganisationBase
    {

        /// <summary>
        /// OrganisationId
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Id de l'entite = ciID, GoupeId, societeId ...
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; internal set; }

        /// <summary>
        /// Libelle
        /// </summary>
        public string Libelle { get; internal set; }
    }
}
