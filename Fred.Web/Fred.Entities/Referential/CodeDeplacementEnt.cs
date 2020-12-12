using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Rapport;
using Fred.Entities.Societe;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente un code déplacement
    /// </summary>
    [DebuggerDisplay("{Code}")]
    public class CodeDeplacementEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un code déplacement.
        /// </summary>
        public int CodeDeplacementId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un code déplacement.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un code déplacement.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        /// <summary>
        ///   Obtient ou définit le kilométrage minimum d'un code déplacement.
        /// </summary>
        public int KmMini { get; set; }

        /// <summary>
        ///   Obtient ou définit le kilométrage maximum d'un code déplacement.
        /// </summary>
        public int KmMaxi { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si IGD est le type d'un code déplacement.
        /// </summary>
        public bool IGD { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le code déplacement est soumis à indemnité forfaitaire ou non.
        /// </summary>
        public bool IndemniteForfaitaire { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si un code déplacement est actif ou non
        /// </summary>
        public bool Actif { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la société
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si ETAM est actif ou non
        /// </summary>
        public bool? IsETAM { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si Cadre est actif ou non
        /// </summary>
        public bool? IsCadre { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si ouvrier est actif ou non
        /// </summary>
        public bool? IsOuvrier { get; set; }

        /// <summary>
        /// Child IndemniteDeplacements where [FRED_INDEMNITE_DEPLACEMENT].[CodeDeplacementId] point to this entity (fk_indemniteCodeDeplacement)
        /// </summary>
        public virtual ICollection<IndemniteDeplacementEnt> IndemniteDeplacements { get; set; }

        /// <summary>
        /// Child PointageAnticipes where [FRED_POINTAGE_ANTICIPE].[CodeDeplacementId] point to this entity (FK_POINTAGE_ANTICIPE_CODE_DEPLACEMENT)
        /// </summary>
        public virtual ICollection<PointageAnticipeEnt> PointageAnticipes { get; set; }

        /// <summary>
        /// Child RapportLignes where [FRED_RAPPORT_LIGNE].[CodeDeplacementId] point to this entity (FK_RAPPORT_LIGNE_CODE_DEPLACEMENT)
        /// </summary>
        public virtual ICollection<RapportLigneEnt> RapportLignes { get; set; }

        /// <summary>
        /// Parent Societe pointed by [FRED_CODE_DEPLACEMENT].([SocieteId]) (FK_FRED_CODE_DEPLACEMENT_SOCIETE)
        /// </summary>
        public virtual SocieteEnt Societe { get; set; }
    }
}
