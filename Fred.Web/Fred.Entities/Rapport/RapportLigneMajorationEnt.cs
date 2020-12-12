using Fred.Entities.Referential;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente ou défini une majoration associées à une ligne de rapport
    /// </summary>
    public class RapportLigneMajorationEnt : PointageMajorationBase
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne majoration du rapport
        /// </summary>
        public override int PointageMajorationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne majoration du rapport
        /// </summary>
        public int RapportLigneMajorationId
        {
            get { return PointageMajorationId; }

            set { PointageMajorationId = value; }
        }

        /// <summary>
        ///   Obtient ou définit l'id de pointage de la ligne de rapport de rattachement
        /// </summary>
        public override int PointageId
        {
            get { return RapportLigneId; }

            set { RapportLigneId = value; }
        }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport de rattachement
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport
        /// </summary>
        public RapportLigneEnt RapportLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de l'entité CodeMajoration
        /// </summary>
        public override int CodeMajorationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité CodeMajoration
        /// </summary>
        public override CodeMajorationEnt CodeMajoration { get; set; }

        /// <summary>
        ///   Obtient ou définit le l'heure majorée
        /// </summary>
        public override double HeureMajoration { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public override bool IsCreated => RapportLigneMajorationId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en modification
        /// </summary>
        public override bool IsDeleted { get; set; } = false;

        /// <summary>
        ///   Vide une propriété
        /// </summary>
        public void CleanLinkedProperties()
        {
            RapportLigne = null;
            CodeMajoration = null;
        }

        /// <summary>
        /// Duplicate a majoration
        /// </summary>
        /// <returns>RapportLigneMajorationEnt</returns>
        public RapportLigneMajorationEnt Duplicate()
        {
            return new RapportLigneMajorationEnt
            {
                PointageMajorationId = 0,
                PointageId = 0,
                CodeMajoration = CodeMajoration,
                CodeMajorationId = CodeMajorationId,
                HeureMajoration = HeureMajoration,
                RapportLigne = null,
                RapportLigneId = 0
            };
        }
    }
}
