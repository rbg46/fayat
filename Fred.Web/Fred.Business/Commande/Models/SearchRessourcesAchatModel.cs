using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Fred.Business.Commande.Models
{
    /// <summary>
    /// Modele de recherche de ressources du referentiel etendu pour les fonctionnalites du perimetre Achat
    /// </summary>
    public class SearchRessourcesAchatModel
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public SearchRessourcesAchatModel()
        {
            CiId = 0;
            SocieteId = 0;
            Page = 1;
            PageSize = 20;
            RessourcesRecommandeesOnly = 0;
        }

        /// <summary>
        /// SocieteId
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Ci ID
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Page
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Recherche en texte
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Recherche
        /// </summary>
        public string Recherche { get; set; }

        /// <summary>
        /// Recherche par mots clés
        /// </summary>
        public string Recherche2 { get; set; }

        /// <summary>
        /// Seulement les ressources recommendees
        /// </summary>
        public int RessourcesRecommandeesOnly { get; set; }

        /// <summary>
        /// Id ressource
        /// </summary>
        public int? RessourceIdNatureFilter { get; set; }

        /// <summary>
        /// Si on prend en compte le tag Achat
        /// </summary>
        public bool AchatsEnable { get; set; }

        /// <summary>
        /// Identifiant de la ressource
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Identifiant du type de la ressource
        /// </summary>
        public int? RessourceTypeId { get; set; }

    }
}
