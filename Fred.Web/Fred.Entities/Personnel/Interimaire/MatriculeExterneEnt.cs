namespace Fred.Entities.Personnel.Interimaire
{
    /// <summary>
    ///   Représente un contrat intérimaire.
    /// </summary>
    public class MatriculeExterneEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Matricule Externe.
        /// </summary>
        public int MatriculeExterneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant d'un personnel.
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de matricule externe
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        ///   Obtient ou définit la source du matricule  
        /// </summary>
        public string Source { get; set; }
    }
}