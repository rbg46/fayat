namespace Fred.Entities.Affectation
{
    /// <summary>
    /// Astreinte view entity
    /// </summary>
    public class AstreinteViewEnt
    {
        /// <summary>
        /// Obtient ou definit le jour de la semaine
        /// </summary>
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Obtient ou definit is astreinte attribut
        /// </summary>
        public bool IsAstreinte { get; set; }

        /// <summary>
        /// Obtient ou definit l'astreinte identiifer
        /// </summary>
        public int AstreinteId { get; set; }

        /// <summary>
        /// Obtient ou definit si l'astreinte a éte modifier ou non
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// Obtient ou définit si le rapport ligne est verouillé
        /// </summary>
        public bool IsRapportLigneVerouille { get; set; }
    }
}
