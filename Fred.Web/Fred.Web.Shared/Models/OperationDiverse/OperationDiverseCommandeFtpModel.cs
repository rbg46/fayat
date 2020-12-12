using System;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    /// <summary>
    /// Modèle utilisé lors la génération des OD automatique pour les écritures comptables venant de FAYAT  TP
    /// </summary>
    public class OperationDiverseCommandeFtpModel
    {
        /// <summary>
        /// Date comtpable de l'écriture comptable
        /// </summary>
        public DateTime DateComptable { get; set; }

        /// <summary>
        /// LIbelle de l'écriture comptable
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Montant de l'écriture comptable
        /// </summary>
        public decimal Montant { get; set; }

        /// <summary>
        /// Montant de l'OD pour la famille ACH
        /// Correspond à la somme des écritures comptable - somme des receptions - somme des OD associé à une commande
        /// </summary>
        public decimal MontantODCommande { get; set; }

        /// <summary>
        /// Montant l'écriture comptable en devise locale
        /// </summary>
        public decimal MontantDevise { get; set; }

        /// <summary>
        /// Code de l'écriture comptable
        /// </summary>
        public string CodeDevise { get; set; }

        /// <summary>
        /// Code du CI
        /// </summary>
        public string CodeCi { get; set; }

        /// <summary>
        /// Quantité de l'écriture comptable
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        /// Code nature de l'écriture comptable
        /// </summary>
        public string SapCodeNature { get; set; }

        /// <summary>
        /// Code de la ressource de l'écriture comptable
        /// </summary>
        public string RessourceCode { get; set; }

        /// <summary>
        /// Identifiant de la ligne de rapport (pointage) de l'écriture comptable
        /// </summary>
        public int? RapportLigneId { get; set; }

        /// <summary>
        /// Identifiant de la famille d'opéaration diverse de l'écriture comptable
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// Code de la famille d'operation diverse de l'écriture comptable
        /// </summary>
        public string FamilleOperationDiverseCode { get; set; }

        /// <summary>
        /// Identifiant de l'écriture comptables
        /// </summary>
        public int EcritureComptableId { get; set; }

        /// <summary>
        /// Numéro de la pièce comptable
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Code du personnel qui à fait l'écriture comptable
        /// </summary>
        public string PersonnelCode { get; set; }

        /// <summary>
        /// Code de la société du materiel
        /// </summary>
        public string SocieteMaterielCode { get; set; }

        /// <summary>
        /// Code du materiel
        /// </summary>
        public string CodeMateriel { get; set; }

        /// <summary>
        /// Code Ref
        /// </summary>
        public string CodeRef { get; set; }

        /// <summary>
        /// Numéro de commande
        /// </summary>
        public string NumeroCommande { get; set; }

        /// <summary>
        /// Code de la société
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Identifiant de la commande
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Quantité Commandé
        /// </summary>
        public decimal QuantiteCommande { get; set; }

        /// <summary>
        /// Montant de la commande
        /// </summary>
        public decimal MontantCommande { get; set; }

        /// <summary>
        /// Identifiant du Ci
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Identifiant de la ressource
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// /Identifiant de la devise
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        /// Identifiant de la tache
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        /// Code de l'unité
        /// </summary>
        public string Unite { get; set; }

        /// <summary>
        /// Identifiant de la nature
        /// </summary>
        public int NatureId { get; set; }
    }
}
