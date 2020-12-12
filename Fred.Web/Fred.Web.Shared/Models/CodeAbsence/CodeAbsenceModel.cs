using Fred.Web.Models.Holding;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;

namespace Fred.Web.Models.CodeAbsence
{
    /// <summary>
    /// Représente une société
    /// </summary>
    public class CodeAbsenceModel : IReferentialModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un Code d'absence.
        /// </summary>
        public int CodeAbsenceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la société 
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la société
        /// </summary>
        public SocieteModel Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la Holding
        /// </summary>
        public int? HoldingId { get; set; }

        /// <summary>
        ///   Obtient ou définit la Holding
        /// </summary>
        public HoldingModel Holding { get; set; }

        /// <summary>
        /// Obtient ou définit le code du CodeAbsence
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si Intemperie
        /// </summary>
        public bool Intemperie { get; set; }

        /// <summary>
        /// Obtient ou définit le TauxDecote
        /// </summary>
        public double TauxDecote { get; set; }

        /// <summary>
        /// Obtient ou définit le NBHeuresDefautETAM
        /// </summary>
        public double NBHeuresDefautETAM { get; set; }

        /// <summary>
        /// Obtient ou définit le NBHeuresMinETAM
        /// </summary>
        public double NBHeuresMinETAM { get; set; }

        /// <summary>
        /// Obtient ou définit le NBHeuresMaxETAM
        /// </summary>
        public double NBHeuresMaxETAM { get; set; }

        /// <summary>
        /// Obtient ou définit le NBHeuresDefautCO
        /// </summary>
        public double NBHeuresDefautCO { get; set; }

        /// <summary>
        /// Obtient ou définit le NBHeuresMinCO
        /// </summary>
        public double NBHeuresMinCO { get; set; }

        /// <summary>
        /// Obtient ou définit le NBHeuresMaxCO
        /// </summary>
        public double NBHeuresMaxCO { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si Actif
        /// </summary>
        public bool Actif { get; set; }

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
        /// Obtient l'identifiant du référentiel CodeAbsence
        /// </summary>
        public string IdRef => this.CodeAbsenceId.ToString();

        /// <summary>
        /// Obtient le libelle du référentiel CodeAbsence
        /// </summary>
        public string LibelleRef => this.Libelle;

        /// <summary>
        /// Obtient le code du référentiel CodeAbsence
        /// </summary>
        public string CodeRef => this.Code;

        /// <summary>
        ///  Obtient ou définit la valeur du groupe identifier
        /// </summary>
        public int? GroupeId { get; set; }

        /// <summary>
        ///  Obtient ou définit la valeur d'absence parent
        /// </summary>
        public int? CodeAbsenceParentId { get; set; }
    }
}
