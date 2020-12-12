using Fred.Web.Models.Groupe;

namespace Fred.Web.Shared.Models.Societe.Classification
{
    /// <summary>
    /// Model de classification des sociétés
    /// </summary>
    public class SocieteClassificationModel 
    {
        /// <summary>
        /// Obtient ou définit l'Identifiant de la classification
        /// </summary>
        public int SocieteClassificationId { get; set; }

        /// <summary>
        /// Obtient ou définit le Code de la classification
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit Libelle de la classification
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le Flag du statut de la classification
        /// </summary>
        public bool Statut { get; set; }

        /// <summary>
        /// Obtient ou définit L'identication du groupe
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit la concatenation du code et du libelle 
        /// </summary>
        public string CodeLibelle { get; set; }

        /// <summary>
        /// Obtient l'identifiant du référentiel Classification
        /// </summary>
        public string IdRef => this.SocieteClassificationId.ToString();

        /// <summary>
        /// Obtient le libelle du référentiel Classification
        /// </summary>
        public string LibelleRef => this.Libelle;

        /// <summary>
        /// Obtient le code du référentiel Classification
        /// </summary>
        public string CodeRef => this.Code;
    }
}
