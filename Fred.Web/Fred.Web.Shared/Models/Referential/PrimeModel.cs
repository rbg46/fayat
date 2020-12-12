using Fred.Entities;
using Fred.Entities.Referential;
using Fred.Web.Models.CI;
using Fred.Web.Models.Societe;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Referential
{
    /// <summary>
    /// Représente une prime
    /// </summary>
    public class PrimeModel : IReferentialModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une prime.
        /// </summary>
        public int PrimeId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une prime.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une prime.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une prime est de type horaire (1), journalière (0) ou mensuelle (2)
        /// </summary>
        public ListePrimeType PrimeType { get; set; }

        /// <summary>
        /// Target personnel statut
        /// </summary>
        public TargetPersonnel TargetPersonnel { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre d'heures max dans le cas où la prime serait de type horaire.
        /// </summary>
        public double? NombreHeuresMax { get; set; }

        /// <summary>
        /// Obtient ou définit le Seuil de la prime mensuelle
        /// </summary>
        public double? SeuilMensuel { get; set; }

        /// <summary>
        /// Obtient ou définit la prime est active ou non.
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
        /// Obtient ou définit si la prime est une prime partenaire ou non.
        /// </summary>
        public bool PrimePartenaire { get; set; }

        /// <summary>
        /// Obtient ou définit si la prime est une prime publique ou non.
        /// </summary>
        public bool Publique { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la société associée.
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des CIs associés.
        /// </summary>
        public ICollection<CIPrimeModel> CIPrimes { get; set; }

        /// <summary>
        /// Obtient ou définit si la prime est associée à un CI ou non.
        /// </summary>
        public bool IsLinkedToCI { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du référentiel prime
        /// </summary>
        public string IdRef => Convert.ToString(this.PrimeId);

        /// <summary>
        /// Obtient ou définit le libelle du référentiel prime
        /// </summary>
        public string LibelleRef
        {
            get
            {
                switch (PrimeType)
                {
                    case ListePrimeType.PrimeTypeHoraire:
                        return Shared.App_LocalResources.FeaturePrime.Prime_PrimeModel_Horaire;
                    case ListePrimeType.PrimeTypeJournaliere:
                        return Shared.App_LocalResources.FeaturePrime.Prime_PrimeModel_Journaliere;
                    default:
                        // ListePrimeType.PrimeTypeMensuelle
                        return Shared.App_LocalResources.FeaturePrime.Prime_PrimeModel_Mensuelle;
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le code du référentiel prime 
        /// </summary>
        public string CodeRef => this.Code + " - " + this.Libelle;

        /// <summary>
        /// Obtient ou définit le code du référentiel prime + code Societe
        /// </summary>
        public string CodeSocietePrime
        {
            get
            {
                if (this.Societe!=null)
                {
                    return this.Societe.Code + " -" + this.CodeRef;

                }
                else
                {
                    return this.CodeRef;
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le Model Societe
        /// </summary>
        public SocieteModel Societe { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID du groupe associée.
        /// </summary>
        public int? GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit si le prime est multi pointage par jour
        /// </summary>
        public bool? MultiPerDay { get; set; }

        /// <summary>
        /// Obtient ou définit si le prime est un prime astreinte
        /// </summary>
        public bool? IsPrimeAstreinte { get; set; }
    }
}
