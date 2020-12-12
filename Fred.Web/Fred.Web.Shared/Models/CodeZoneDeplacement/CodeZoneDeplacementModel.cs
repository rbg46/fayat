using System;
using Fred.Web.Models.Referential;


namespace Fred.Web.Models.CodeZoneDeplacement
{
    /// <summary>
    /// Représente un code zone deplacement
    /// </summary>
    public class CodeZoneDeplacementModel : IReferentialModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un Code Zone Deplacement.
        /// </summary>
        public int CodeZoneDeplacementId { get; set; }

        /// <summary>
        /// Obtient ou définit le code du Code Zone Deplacement
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la date de creation du code zone deplacement 
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de l'auteur de la création
        /// </summary>
        public int? AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification du code zone deplacement 
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de l'auteur de la modification
        /// </summary>
        public int? AuteurModification { get; set; }

        ///// <summary>
        ///// Obtient ou définit la date de suppression du code zone deplacement 
        ///// </summary>
        ////public DateTime? DateSuppression { get; set; }

        ///// <summary>
        ///// Obtient ou définit l'id de l'auteur de la suppression
        ///// </summary>
        ////public int? AuteurSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de la société à laquel le code zone est rattaché
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si Actif
        /// </summary>
        public bool IsActif { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si ETAM est actif ou non
        /// </summary>
        public bool? IsETAM { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si Cadre est actif ou non
        /// </summary>
        public bool? IsCadre { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si ETAM est actif ou non
        /// </summary>
        public bool? IsOuvrier { get; set; }

        /// <summary>
        /// Obtient ou définit le kilométrage minimum.
        /// </summary>
        public int KmMini { get; set; }

        /// <summary>
        /// Obtient ou définit le kilométrage maximum.
        /// </summary>
        public int KmMaxi { get; set; }

        /// <summary>
        /// Id code deplacement
        /// </summary>
        public string IdRef => this.CodeZoneDeplacementId.ToString();

        /// <summary>
        /// Libelle code deplacement
        /// </summary>
        public string LibelleRef => this.Libelle;

        /// <summary>
        /// COde deplacement
        /// </summary>
        public string CodeRef => this.Code;
    }
}
