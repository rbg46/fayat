namespace Fred.Framework.Export.Order.Models
{
    public class CommandeLigneExportModel
    {
        public string Libelle { get; set; }

        public string Quantite { get; set; }

        public string UniteCode { get; set; }

        public string UniteLibelle { get; set; }

        public string PUHT { get; set; }

        public string MontantHT { get; set; }

        public decimal MontantHTValue { get; set; }

        /// <summary>
        /// Le numéro d'avenant ou null s'il s'agit d'une ligne de commande.
        /// </summary>
        public int? AvenantNumero { get; set; }

        public bool AvenantIsDiminution { get; set; }
    }
}
