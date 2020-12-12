using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Fred.Entities.CI;
using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;

namespace Fred.Entities.RepartitionEcart
{
    /// <summary>
    /// Represent une repartion d'ecart 
    /// </summary>
    [DebuggerDisplay("Famille= {Libelle} RepartitionEcartId = {RepartitionEcartId} CiId = {CiId}  ValorisationInitiale = {ValorisationInitiale} ValorisationRectifiee = {ValorisationRectifiee} MontantCapitalise = {MontantCapitalise} Ecart = {Ecart} ")]
    public class RepartitionEcartEnt
    {
        private DateTime? dateCloture;
        private DateTime? dateComptable;

        /// <summary>
        /// ctor
        /// </summary>
        public RepartitionEcartEnt()
        {
            OperationDiverses = new List<OperationDiverseEnt>();
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une Repartition d'écart.
        /// </summary>
        public int RepartitionEcartId { get; set; }

        /// <summary>
        /// Represente l'index de la ligne lors de l'affichage de la synthese.
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        ///   Obtient ou définit l'affaire d'une OD.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'affaire d'une OD.
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit la DateCloture.
        /// </summary>
        public DateTime? DateCloture
        {
            get
            {
                return (dateCloture.HasValue) ? DateTime.SpecifyKind(dateCloture.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCloture = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la DateCloture.
        /// </summary>
        public DateTime? DateComptable
        {
            get
            {
                return (dateComptable.HasValue) ? DateTime.SpecifyKind(dateComptable.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateComptable = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit le libelle d'une Repartition d'écart.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant de la Valorisation initiale.
        /// </summary>
        public decimal ValorisationInitiale { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant de la Valorisation Rectifiee.
        /// </summary>
        public decimal ValorisationRectifiee { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant Capitalise.
        /// </summary>
        public decimal MontantCapitalise { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant de la Valorisation initiale.
        /// </summary>
        public decimal Ecart { get; set; }

        /// <summary>
        /// OperationDiverses
        /// </summary>
        public IEnumerable<OperationDiverseEnt> OperationDiverses { get; set; }

        /// <summary>
        /// code chapitres
        /// </summary>
        public IEnumerable<string> ChapitreCodes { get; set; }


        /// <summary>
        /// Famille OD
        /// </summary>
        public FamilleOperationDiverseEnt FamilleOperationDiverse { get; set; }

        /// <summary>
        /// Ecriture Comptables 
        /// /// </summary>
        public List<EcritureComptableEnt> EcritureComptables { get; set; }
    }
}