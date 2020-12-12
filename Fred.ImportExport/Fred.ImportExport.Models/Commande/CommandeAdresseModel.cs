namespace Fred.ImportExport.Models.Commande
{
    public class CommandeAdresseModel
    {
        public string FournisseurCode { get; set; }
        public string FournisseurAdresse { get; set; }
        public string FournisseurCPostal { get; set; }
        public string FournisseurVille { get; set; }
        public string FournisseurPaysCode { get; set; }

        public string AgenceCode { get; set; }
        public string AgenceAdresse { get; set; }
        public string AgenceCPostal { get; set; }
        public string AgenceVille { get; set; }
        public string AgencePaysCode { get; set; }
    }
}
