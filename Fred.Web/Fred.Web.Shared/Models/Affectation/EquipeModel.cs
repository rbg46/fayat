using Fred.Web.Models.Personnel;

namespace Fred.Web.Models.Affectation
{
    /// <summary>
    /// Equipe class model
    /// </summary>
    public class EquipeModel
    {
        /// <summary>
        /// Obtient ou definit l'identifiant unique d'une equipe
        /// </summary>
        public int EquipeId { get; set; }

        /// <summary>
        /// Obtient le proprietaire de l'equipe
        /// </summary>
        public PersonnelModel Proprietaire { get; set; }
    }
}
