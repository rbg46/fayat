using System;
using System.Collections.Generic;

namespace Fred.Entities.EcritureComptable
{
    /// <summary>
    /// Ecriture Comptable DTO pour Fayat TP
    /// </summary>
    public class EcritureComptableFtpDto
    {
        /// <summary>
        /// Date Comptable
        /// </summary>
        public DateTime DateComptable { get; set; }

        /// <summary>
        /// Libelle de l'écriture comptable
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Montant de l'écriture
        /// </summary>
        public decimal Montant { get; set; }

        /// <summary>
        /// Montant de l'écriture dans la devise locale
        /// </summary>
        public decimal MontantDevise { get; set; }

        /// <summary>
        /// Code de la devise
        /// </summary>
        public string CodeDevise { get; set; }

        /// <summary>
        /// Code du CI
        /// </summary>
        public string CodeCi { get; set; }

        /// <summary>
        /// Quantité
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        /// Code de la nature analytique
        /// </summary>
        public string NatureAnalytique { get; set; }

        /// <summary>
        /// Code de la ressource
        /// </summary>
        public string RessourceCode { get; set; }

        /// <summary>
        /// Identifiant de la ligne de rapport
        /// </summary>
        public int? RapportLigneId { get; set; }

        /// <summary>
        /// Identifiant de la famille d'OD
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// Code de la famille d'OD
        /// </summary>
        public string FamilleOperationDiversesCode { get; set; }

        /// <summary>
        /// Nuémro de pièce
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Code du personnel
        /// </summary>
        public string PersonnelCode { get; set; }

        /// <summary>
        /// Code de la société propriétaire du matereil
        /// </summary>
        public string SocieteMaterielCode { get; set; }

        /// <summary>
        /// Code du Materiel
        /// </summary>
        public string CodeMateriel { get; set; }

        /// <summary>
        /// Document d'origine si différent achat ou pointage
        /// </summary>
        public string CodeRef { get; set; }

        /// <summary>
        /// Numéro de la commande
        /// </summary>
        public string NumeroCommande { get; set; }

        /// <summary>
        /// Code de la société
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Code du groupe
        /// </summary>
        public string GroupeCode { get; set; }

        /// <summary>
        /// Identifiant du CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Identifiant de la ressource
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// identifiant de la devise
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
        /// Identifiant de l'écriture Comptable
        /// </summary>
        public int EcritureComptableId { get; set; }

        /// <summary>
        /// Identifiant de la nature
        /// </summary>
        public int NatureId { get; set; }

        /// <summary>
        /// Liste des messages d'erreurs
        /// </summary>
        public List<string> ErrorMessages { get; set; }

        /// <summary>
        /// Numéro de la facture SAP
        /// </summary>
        public string NumFactureSAP { get; set; }

        /// <summary>
        /// Indique si le pointage est de type personnel
        /// </summary>
        public bool IsPointagePersonnel { get; set; }

        /// <summary>
        /// Indique si le pointage est de type materiel
        /// </summary>
        public bool IsPointageMateriel { get; set; }
    }
}
