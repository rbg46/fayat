using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Journal;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Fred.Entities.EcritureComptable
{
    /// <summary>
    /// Représente un module.
    /// </summary>
    [DebuggerDisplay("EcritureComptableId = {EcritureComptableId} NumeroPiece = {NumeroPiece} CiId = {CiId}  Montant = {Montant} ")]
    public class EcritureComptableEnt
    {
        private DateTime dateCreation;
        private DateTime? dateComptable;

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une écriture comptable.
        /// </summary>
        public int EcritureComptableId { get; set; }

        /// <summary>
        /// Obtient ou définit la date du Import
        /// </summary>
        public DateTime DateCreation
        {
            get { return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc); }
            set { dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        /// Obtient ou définit la date de création de la écriture comptable.
        /// </summary>
        public DateTime? DateComptable
        {
            get { return (dateComptable.HasValue) ? DateTime.SpecifyKind(dateComptable.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateComptable = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit le libellé d'une écriture comptable.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le	Numéro de pièce .
        /// </summary>
        public string NumeroPiece { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du journal comptable
        /// </summary>
        public int? JournalId { get; set; }

        /// <summary>
        /// Obtient ou définit le journal comptable
        /// </summary>
        public JournalEnt Journal { get; set; }

        /// <summary>
        /// Obtient ou définit l'affaire d'une écriture comptable.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'affaire d'une écriture comptable.
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        /// Obtient le montant HT de la écriture comptable
        /// </summary>
        public decimal Montant { get; set; }

        /// <summary>
        /// Obtient la quantité de la écriture comptable
        /// </summary>
        public decimal Quantite { get; set; }

        public int DeviseId { get; set; }

        /// <summary>
        /// Obtient ou définit la devise associée à une écriture comptable.
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la commande liée à l'écriture comptable
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit la commande liée à l'écriture comptable
        /// </summary>
        public CommandeEnt Commande { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la famille d'OD liée à l'écriture comptable
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// Obtient ou définit la famille d'OD liée à l'écriture comptable
        /// </summary>
        public FamilleOperationDiverseEnt FamilleOperationDiverse { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des lignes d'une commande
        /// </summary>
        public ICollection<EcritureComptableCumulEnt> EcritureComptableCumul { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du code nature liée à l'écriture comptable
        /// </summary>
        public int? NatureId { get; set; }

        /// <summary>
        /// Obtient ou définit la nature liée à l'écriture comptable
        /// </summary>
        public NatureEnt Nature { get; set; }

        /// <summary>
        /// Obtient ou définit le code de référence d'une écriture comptable. Actuellement cette donnée n'est pas exploité au sein de FRED 
        /// </summary>
        [Column("CodeRef")]
        public string CodeRef { get; set; }

        /// <summary>
        /// Indique le nombre d'OD attaché à une écriture comptable
        /// </summary>
        public int NombreOD { get; set; }

        /// <summary>
        /// Indique le montant total des OD rattaché à l'écriture comptable
        /// </summary>
        public decimal MontantTotalOD { get; set; }

        /// <summary>
        /// Numéro de facture SAP
        /// </summary>
        public string NumeroFactureSAP { get; set; }
    }
}
