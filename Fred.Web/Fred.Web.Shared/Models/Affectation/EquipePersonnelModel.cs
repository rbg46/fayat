using Fred.Web.Models.Personnel;

namespace Fred.Web.Models.Affectation
{
    /// <summary>
    /// Equipe Personnel model class
    /// </summary>
    public class EquipePersonnelModel
    {
        /// <summary>
        /// Obtient ou definit l'identifiant unique de la relation Equipe Personnel
        /// </summary>
        public int EquipePersonnelId { get; set; }

        /// <summary>
        /// Obtient ou definit l'entité equipe
        /// </summary>
        public EquipeModel Equipe { get; set; }

        /// <summary>
        /// Obtient ou definit l'entité personnel
        /// </summary>
        public PersonnelModel Personnel { get; set; }
    }
}
