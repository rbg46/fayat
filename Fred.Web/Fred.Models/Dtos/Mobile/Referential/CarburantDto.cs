namespace Fred.Web.Dtos.Mobile.Referential
{
    /// <summary>
    /// Dto d'un carburant
    /// </summary>
    public class CarburantDto
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un Groupe.
        /// </summary>
        public int CarburantId { get; set; }

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