using Fred.Entities.Groupe;

namespace Fred.Entities
{
    /// <summary>
    ///   Représente un parametre.
    /// </summary>
    public class ParametreEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un parametre.
        /// </summary>
        public int ParametreId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un parametre
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un parametre
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la valeur d'un parametre
        /// </summary>
        public string Valeur { get; set; }

        /// <summary>
        ///   Obtient ou définit le GroupeId d'un parametre
        /// </summary>
        public int? GroupeId { get; set; }

        /// <summary>
        /// Le groupe 
        /// </summary>  
        public virtual GroupeEnt Groupe { get; set; }
    }
}