using System;

namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ExportManualLockUnlockLigneDeCommandeParameters
    {
        public int CommandeLigneId { get; set; }
        public int AuteurModificationId { get; set; }
        public int AuteurModificationSocieteId { get; set; }
        public string AuteurModificationGroupeCode { get; set; }
        public DateTime DateVerouillage { get; set; }
        public bool IsLocked { get; set; }
        public int ActionCommandeLigneId { get; set; }
        public string JobId { get; set; }
    }
}
