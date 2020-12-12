namespace Fred.Web.Shared.Models
{
    public class TypeSocieteModel
    {
        /// <summary>
        ///   Identifiant unique de l'entité
        /// </summary>        
        public int TypeSocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code
        /// </summary>        
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé
        /// </summary>        
        public string Libelle { get; set; }
    }
}
