namespace Fred.Web.Models
{
    /// <summary>
    /// Représente un fournisseur
    /// </summary>
    public class FournisseurLightModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un fournisseur.
        /// </summary>
        public int FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un fournisseur.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'un fournisseur.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;
    }
}