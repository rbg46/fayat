using System;

namespace Fred.Web.Models
{
    /// <summary>
    ///   Représente une répcetion pour l'export vers Excel
    /// </summary>  
    public class ReceptionExportModel
    {
        public string NumeroBC { get; set; }
        public string FournisseurLibelle { get; set; }
        public string CICode { get; set; }
        public string CommandeLigneLibelle { get; set; }
        public string CommandeDate { get; set; }
        public string CommandeMontantHT { get; set; }
        public string CommandeMontantHTReceptionne { get; set; }
        public string Date { get; set; }
        public string Periode { get; set; }
        public string Libelle { get; set; }
        public string RessourceCodeLibelle { get; set; }
        public string NatureCodeLibelle { get; set; }
        public string TacheCodeLibelle { get; set; }
        public string UniteCode { get; set; }
        public string Quantite { get; set; }
        public string PUHT { get; set; }
        public string MontantHT { get; set; }
        public string NumeroBL { get; set; }
        public string DateVisaReception { get; set; }
        public string Commentaire { get; set; }
        public string DateAuteurCreation { get; set; }
        public string DateAuteurModification { get; set; }
        public string MontantHTFacture { get; set; }
        public string SoldeFar { get; set; }
        public string DatesFactures { get; set; }
        public string DatesRapprochements { get; set; }
        public string MontantHTFactures { get; set; }
        public string CommandeLigneQuantite { get; set; }
        public string DeviseCode { get; set; }
        public DateTime? DateTransfertFAR { get; set; }
    }
}
