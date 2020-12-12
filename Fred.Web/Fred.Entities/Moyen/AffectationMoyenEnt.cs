using System;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente une affectation d'un moyen
    /// </summary>
    public class AffectationMoyenEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant d'une affectation d'un moyen
        /// </summary>
        public int AffectationMoyenId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du moyen
        /// </summary>
        public int MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité d'un moyen
        /// </summary>
        public MaterielEnt Materiel { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du CI
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité CI
        /// </summary>
        public CIEnt Ci { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du conducteur
        /// </summary>
        public int? ConducteurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité conducteur
        /// </summary>
        public PersonnelEnt Conducteur { get; set; }

        /// <summary>
        /// Obtient ou définit la date de début d'affectation
        /// </summary>
        public DateTime DateDebut { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fin d'affectation
        /// </summary>
        public DateTime? DateFin { get; set; }

        /// <summary>
        /// Obtient ou définit si l'affectation est active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Obtient ou définit le type d'affectation
        /// </summary>
        public int AffectationMoyenTypeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité type affectation
        /// </summary>
        public AffectationMoyenTypeEnt TypeAffectation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du site d'affectation
        /// </summary>
        public int? SiteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité site
        /// </summary>
        public SiteEnt Site { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant matériel location
        /// </summary>
        public int? MaterielLocationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité materiel location
        /// </summary>
        public MaterielLocationEnt MaterielLocation { get; set; }
    }
}
