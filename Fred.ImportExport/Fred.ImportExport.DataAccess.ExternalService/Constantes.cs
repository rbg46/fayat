namespace Fred.ImportExport.DataAccess.ExternalService
{
    /// <summary>
    /// Classe qui regroupe les constantes utilisées dans le projet
    /// </summary>
    public static class Constantes
    {
        /// <summary>
        /// Code du succés renvoyé par TIBCO
        /// </summary>
        public const string TibcoRetourSuccessCode = "0";

        /// <summary>
        /// Code d'erreur envoyé par TIBCO
        /// </summary>
        public const string TibcoRetourErrorCode = "-1";

        /// <summary>
        /// Tibco export moyen key
        /// </summary>
        public const string TibcoExportMoyenKey = "Tibco:ExportPointageMoyen";

        /// <summary>
        /// Tibco import GetListeContratATraiter
        /// </summary>
        public const string TibcoImportGetListeContratATraiter = "Tibco:ImportGetListeContratATraiter";

        /// <summary>
        /// Tibco import GetDetailContrat
        /// </summary>
        public const string TibcoImportGetDetailContrat = "Tibco:ImportGetDetailContrat";

        /// <summary>
        /// Tibco import GetDetailContrat
        /// </summary>
        public const string TibcoImportSalarieFes = "Tibco:ImportSalarieFes";

        /// <summary>
        /// Dans le cas d'un probléme de configuration c'est 
        /// la clé générée par la Web.config (Qu'il faudra remplaçer par une url par défaut)
        /// </summary>
        public const string TibcoUrlDefaultValue = "__TibcoExportPointageMoyen__";

        public const string AM = "AM";
        public const string PM = "PM";
        public const string DAY = "DAY";
        public const string Hourly = "hourly";
        public const int RapportVerrouilleId = 5;
        public const int RapportValide2 = 3;
        public const string Annulle = "ANNULE";
        public const string Valide = "VALIDE";
        public const string ABSENCE = "ABSENCE";
        public const string ENCOURS = "ENCOURS";
        public const string Pixid = "PIXID";
        public const string GroupeRazelBec = "GRZB";
        public const string Contrat = "CTR";
        public const string EURO = "EUR";
        public const string Success = "200";
        public const string Fred = "Fred";
    }
}
