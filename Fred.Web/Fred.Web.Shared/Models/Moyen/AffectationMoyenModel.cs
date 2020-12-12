using Fred.Entities;
using Fred.Entities.Moyen;
using Fred.Web.Models.CI;
using Fred.Web.Models.Personnel;
using System;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Représente une affectation d'un moyen
    /// </summary>
    public class AffectationMoyenModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant d'une affectation d'un moyen
        /// </summary>
        public int AffectationMoyenId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du moyen
        /// </summary>
        public int MoyenId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du moyen
        /// </summary>
        public int? MaterielLocationId { get; set; }

        /// <summary>
        /// Materiel Location model
        /// </summary>
        public MaterielLocationModel MaterielLocation { get; set; }

        /// <summary>
        /// Obtient ou définit le moyen
        /// </summary>
        public MoyenModel Moyen { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du CI
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// Obtient ou définit le CI
        /// </summary>
        public CIModel Ci { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public PersonnelModel Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du conducteur
        /// </summary>
        public int? ConducteurId { get; set; }

        /// <summary>
        /// Obtient ou définit le conducteur
        /// </summary>
        public PersonnelModel Conducteur { get; set; }

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
        /// Obtient ou définit le type affectation
        /// </summary>
        public AffectationMoyenTypeModel TypeAffectation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du site d'affectation
        /// </summary>
        public int? SiteId { get; set; }

        /// <summary>
        /// Obtient ou définit le modèle du site
        /// </summary>
        public SiteModel Site { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit information sur l'affectation
        /// </summary>
        public string AffectedTo
        {
            get
            {
                if (TypeAffectation == null)
                {
                    return null;
                }

                if (TypeAffectation.Code.Equals(AffectationMoyenTypeCode.Personnel.ToString()))
                {
                    return Personnel?.Nom + " " + Personnel?.Prenom;
                }
                else if (TypeAffectation.Code.Equals(AffectationMoyenTypeCode.CI.ToString()))
                {
                    return Ci?.Libelle;
                }
                else
                {
                    return TypeAffectation.Libelle;
                }
            }
        }

        /// <summary>
        /// A utiliser en front (AngularJs) pour la séléction 
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Code du moyen
        /// </summary>
        public string MoyenCode
        {
            get
            {
                return Moyen?.Code;
            }
        }

        /// <summary>
        /// Libelle de l'affectation du moyen
        /// </summary>
        public string Libelle => MaterielLocation?.Libelle ?? Moyen?.Libelle;

        /// <summary>
        /// Immatriculation de l'affectation du moyen
        /// </summary>
        public string Immatriculation => MaterielLocation?.Immatriculation ?? Moyen?.Immatriculation;
    }
}
