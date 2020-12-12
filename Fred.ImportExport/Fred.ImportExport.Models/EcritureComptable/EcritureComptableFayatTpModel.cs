using System;
using System.Collections.Generic;

namespace Fred.ImportExport.Models.EcritureComptable
{
    public class EcritureComptableFayatTpModel
    {
        public DateTime? DateCreation { get; set; }

        public DateTime? DateComptable { get; set; }
        public string GroupeCode { get; set; }

        public string SocieteCode { get; set; }
        public int SocieteId { get; set; }

        public string Libelle { get; set; }

        public string NatureAnalytique { get; set; }

        public decimal? MontantDeviseInterne { get; set; }

        public string DeviseInterne { get; set; }

        public decimal? MontantDeviseTransaction { get; set; }

        public string DeviseTransaction { get; set; }

        public string CiCode { get; set; }

        public string NumeroPiece { get; set; }

        public string NumeroCommande { get; set; }

        public string Ressource { get; set; }

        public decimal? Quantite { get; set; }

        public string Unite { get; set; }

        public string RapportLigneId { get; set; }

        public string Personne { get; set; }

        public string MaterielSocieteCode { get; set; }

        public string MaterielCode { get; set; }

        /// <summary>
        /// Document de référence de l'écriture comptable
        /// </summary>
        public string CodeRef { get; set; }
        
        public string CodeNature { get; set; }

        /// <summary>
        /// Identifiant de la famille d'OD avec commande
        /// </summary>
        public int ParentFamilyODWithOrder { get; set; }

        /// <summary>
        /// Identifiant de la famille d'OD sans commande
        /// </summary>
        public int ParentFamilyODWithoutOrder { get; set; }

        /// <summary>
        /// Identifiant de la famille d'operation diverse
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// Code de la famille d'OD
        /// </summary>
        public string CodeFamille { get; set; }

        /// <summary>
        /// Code de la famille d'OD avec commande 
        /// </summary>
        public string CodeFamilleWithOrder { get; set; }

        /// <summary>
        /// Code de la famille d'OD sans commande 
        /// </summary>
        public string CodeFamilleWithoutOrder { get; set; }

        public List<string> Errors { get; set; }

        public string NumFactureSAP { get; set; }
    }
}
