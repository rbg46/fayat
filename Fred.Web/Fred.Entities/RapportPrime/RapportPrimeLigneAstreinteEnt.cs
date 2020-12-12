using Fred.Entities.Affectation;

namespace Fred.Entities.RapportPrime
{
    /// <summary>
    ///   Représente ou défini une astreinte d'une ligne d'un Rapport Prime
    /// </summary>
    public class RapportPrimeLigneAstreinteEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'association Astreinte + ligne du Rapport Prime
        /// </summary>
        public int RapportPrimeLigneAstreinteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne du Rapport Prime
        /// </summary>
        public int RapportPrimeLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne du Rapport Prime
        /// </summary>
        public RapportPrimeLigneEnt RapportPrimeLigne { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'astreinte
        /// </summary>
        public int AstreinteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité Astreinte
        /// </summary>
        public AstreinteEnt Astreinte { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public bool IsCreated => RapportPrimeLigneAstreinteId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est à supprimer
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        ///   Vide certaines propriété
        /// </summary>
        public void CleanLinkedProperties()
        {
            RapportPrimeLigne = null;
        }
    }
}
