using Fred.Entities.Referential;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente ou défini les primes associées à un pointage anticipé
    /// </summary>
    public class PointageAnticipePrimeEnt : PointagePrimeBase
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne prime du rapport
        /// </summary>
        public override int PointagePrimeId
        {
            get { return PointageAnticipePrimeId; }

            set { PointageAnticipePrimeId = value; }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne prime du rapport
        /// </summary>
        public int PointageAnticipePrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport de rattachement
        /// </summary>
        public override int PointageId
        {
            get { return PointageAnticipeId; }

            set { PointageAnticipeId = value; }
        }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport de rattachement
        /// </summary>
        public int PointageAnticipeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport
        /// </summary>
        public PointageAnticipeEnt PointageAnticipe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la prime
        /// </summary>
        public override int PrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Prime
        /// </summary>
        public override PrimeEnt Prime { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la prime est checkée
        /// </summary>
        public override bool IsChecked { get; set; }

        /// <summary>
        ///   Obtient ou définit l'heure de la prime uniquement si TypeHoraire est en heure
        /// </summary>
        public override double? HeurePrime { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public override bool IsCreated => PointageAnticipePrimeId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en modification
        /// </summary>
        public override bool IsDeleted { get; set; } = false;
    }
}