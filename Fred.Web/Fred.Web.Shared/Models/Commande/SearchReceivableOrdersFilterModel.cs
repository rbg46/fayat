using System;
using System.Collections.Generic;
using Fred.Web.Models;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential.Light;
using Fred.Web.Models.ReferentielFixe.Light;
using Fred.Web.Models.Search;
using Fred.Web.Shared.Models.Referential;

namespace Fred.Web.Shared.Models.Commande
{
    public class SearchReceivableOrdersFilterModel : ISearchValueModel
    {
        /// <summary>
        ///   Obtient ou définit la valeur recherchée
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        ///   Identifiant Fournisseur recherché
        /// </summary>
        public int? FournisseurId { get; set; }

        public FournisseurLightModel Fournisseur { get; set; } // necessaire pour les favoris

        /// <summary>
        ///   Identifiant de l'agence recherchée
        /// </summary>
        public int? AgenceId { get; set; }

        public AgenceLightModel Agence { get; set; } // necessaire pour les favoris

        /// <summary>
        ///   Identifiant CI recherché
        /// </summary>
        public int? CiId { get; set; }

        public CILightModel CI { get; set; } // necessaire pour les favoris

        /// <summary>
        ///     Identifiant de la ressource
        /// </summary>
        public int? RessourceId { get; set; }

        public RessourceLightModel Ressource { get; set; } // necessaire pour les favoris

        /// <summary>
        ///     Identifiant de la tache
        /// </summary>
        public int? TacheId { get; set; }

        public TacheLightModel Tache { get; set; } // necessaire pour les favoris

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Date min
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Date max
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Identifiant du création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        public PersonnelLightModel AuteurCreation { get; set; } // necessaire pour les favoris

        /// <summary>
        ///     Liste des codes type commande
        /// </summary>
        public List<string> TypeCodes { get; set; } = new List<string>();

        /// <summary>
        ///   Obtient ou définit le booléen commande abonnement
        /// </summary>
        public bool IsAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande contient un matériel externe à pointer ou non 
        /// </summary>
        public bool IsMaterielAPointer { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est energie
        /// </summary>
        public bool IsEnergie { get; set; }

        /// <summary>
        ///     Obtient ou définit si on filtre par commande abonnement
        /// </summary>
        public bool IsSoldee { get; set; }

        /// <summary>
        ///   si on selectionne les commandes avec au moins une ligne vérouillée
        /// </summary>
        public bool OnlyCommandeWithAtLeastOneCommandeLigneLocked { get; set; }

    }
}
