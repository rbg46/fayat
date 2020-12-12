using System;

namespace Fred.Web.Shared.Models.Commande
{
    public class ExportManualLockUnlockLigneDeCommandeToSapModel
    {
        public int UtilisateurId { get; set; }
        public int UtilisateurSocieteId { get; set; }
        public string UtilisateurGroupeCode { get; set; }
        public int CommandeLigneId { get; set; }
        public DateTime Date { get; set; }
    }
}
