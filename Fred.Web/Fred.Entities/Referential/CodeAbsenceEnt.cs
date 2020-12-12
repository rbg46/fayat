using Fred.Entities.Groupe;
using Fred.Entities.Holding;
using Fred.Entities.Rapport;
using Fred.Entities.Societe;
using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Represente une code absence
    /// </summary>
    public class CodeAbsenceEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Code d'absence.
        /// </summary>
        public int CodeAbsenceId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la société
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la société
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la Holding
        /// </summary>
        public int? HoldingId { get; set; }

        /// <summary>
        ///   Obtient ou définit la Holding
        /// </summary>
        public HoldingEnt Holding { get; set; }

        /// <summary>
        ///   Obtient ou définit le code du CodeAbsence
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si Intemperie
        /// </summary>
        public bool Intemperie { get; set; }

        /// <summary>
        ///   Obtient ou définit le TauxDecote
        /// </summary>
        public double TauxDecote { get; set; }

        /// <summary>
        ///   Obtient ou définit le NBHeuresDefautETAM
        /// </summary>
        public double NBHeuresDefautETAM { get; set; }

        /// <summary>
        ///   Obtient ou définit le NBHeuresMinETAM
        /// </summary>
        public double NBHeuresMinETAM { get; set; }

        /// <summary>
        ///   Obtient ou définit le NBHeuresMaxETAM
        /// </summary>
        public double NBHeuresMaxETAM { get; set; }

        /// <summary>
        ///   Obtient ou définit le NBHeuresDefautCO
        /// </summary>
        public double NBHeuresDefautCO { get; set; }

        /// <summary>
        ///   Obtient ou définit le NBHeuresMinCO
        /// </summary>
        public double NBHeuresMinCO { get; set; }

        /// <summary>
        ///   Obtient ou définit le NBHeuresMaxCO
        /// </summary>
        public double NBHeuresMaxCO { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si Actif
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
        ///    Obtient ou définit une valeur indiquant si ouvrier est actif ou non
        /// </summary>
        public bool? IsOuvrier { get; set; }

        /// <summary>
        ///  Obtient ou définit la valeur du groupe identifier
        /// </summary>
        public int? GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit la valeur le groupe
        /// </summary>
        public GroupeEnt Groupe { get; set; }

        /// <summary>
        ///  Obtient ou définit la valeur d'absence parent
        /// </summary>
        public int? CodeAbsenceParentId { get; set; }

        /// <summary>
        ///  Obtient ou définit la valeur du code absence parent
        /// </summary>
        public CodeAbsenceEnt CodeAbsneceParent { get; set; }

        /// <summary>
        /// Niveau du code d'absences
        /// </summary>
        public CodeAbsenceNiveau? Niveau { get; set; }

        /// <summary>
        /// Child PointageAnticipes where [FRED_POINTAGE_ANTICIPE].[CodeAbsenceId] point to this entity (FK_POINTAGE_ANTICIPE_CODE_ABSENCE)
        /// </summary>
        public virtual ICollection<PointageAnticipeEnt> PointageAnticipes { get; set; }

        /// <summary>
        /// Child RapportLignes where [FRED_RAPPORT_LIGNE].[CodeAbsenceId] point to this entity (FK_RAPPORT_LIGNE_CODE_ABSENCE)
        /// </summary>
        public virtual ICollection<RapportLigneEnt> RapportLignes { get; set; }
    }
}