using System;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    public class PreFillingOperationDiverseModel
    {
        /// <summary>
        /// Obtient ou définit l'affaire d'une OD.
        /// </summary>  
        public int CiId { get; set; }

        /// <summary>
        /// Libelle de l'OD
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// RessourceID
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la tâche d'une OD.
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        /// Entité de la tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///  Obtient ou définit l'unité.
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///  Obtient ou définit l'unité.
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité d'une OD.
        /// </summary>    
        public decimal Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit le prix unitaire d'une OD.
        /// </summary>   
        public decimal PUHT { get; set; }

        /// <summary>
        /// Commentaire de l'OD
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Famille d'opeation diverse
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// Identifiant de l'écriture comptable
        /// </summary>
        public int? EcritureComptableId { get; set; }

        /// <summary>
        /// Obtient ou définit l'appartenance à un abonnement
        /// </summary>
        public bool EstUnAbonnement { get; set; }
    }
}
