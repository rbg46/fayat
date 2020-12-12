using Fred.Entities;

namespace Fred.Business.SeuilValidation.Models
{
    /// <summary>
    /// Model pour faire une recherche de personnels eyant les personnels qui ont la permission (sur un des roles associés a l'utilisateur)
    /// et qui ont aussi un seuil de validation minimum de commande dans l'arbre
    /// </summary>
    public class PersonnelWithPermissionAndSeuilValidationRequest
    {

        /// <summary>
        /// L'id de la permission dont l'utilisateur doit avoir par l'intermediaire de ses role/fonctionnalite/Permission
        /// </summary>
        public int PermissionId { get; set; }
        /// <summary>
        /// Le Ci sur lequel on fait la recherche de seuil
        /// </summary>
        public int CiId { get; set; }
        /// <summary>
        /// La devise concernée
        /// </summary>
        public int DeviseId { get; set; }
        /// <summary>
        /// Le seuil minimun a partir du quel on considere que l'utilisateur peut faire une validation de commande 
        /// </summary>
        public decimal SeuilMinimum { get; set; }
        /// <summary>
        /// La page pour la pagination
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// la taille de la pge
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Le texte que l'on recherche pour faire notre recherche de personnel
        /// </summary>
        public string Recherche { get; set; }
        /// <summary>
        /// Permet de savoir si on inclut SEULEMENT les personnel qui ont le seuil necessaire, 
        /// sahchant que la reponse contient l'information s'il a ou non le seuil necessaire
        /// </summary>
        public bool AuthorizedOnly { get; set; }

        /// <summary>
        /// Mode requeté
        /// </summary>
        public FonctionnaliteTypeMode FonctionnaliteTypeMode { get; internal set; }
    }
}
