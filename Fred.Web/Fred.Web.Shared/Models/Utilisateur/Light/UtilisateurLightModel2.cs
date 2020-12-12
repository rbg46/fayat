using Fred.Web.Shared.Models;

namespace Fred.Web.Models
{
    public class UtilisateurLightModel2
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un utilisateur
        /// </summary>
        public int UtilisateurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du utilisateur ayant valider la commande
        /// </summary>
        public PersonnelLightModel2 Personnel { get; set; } = null;

        /// <summary>
        /// Obtient ou définit l'identifiant personnel de l'utilisateur
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Obtient une concaténation du nom et du prénom du membre du personnel
        /// </summary>    
        public string NomPrenom => this.Personnel != null ? this.Personnel.NomPrenom : string.Empty;

        /// <summary>
        /// Obtient une concaténation du prénom et du nom du membre du personnel
        /// </summary>    
        public string PrenomNom => this.Personnel != null ? this.Personnel.PrenomNom : string.Empty;

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le personnel peut saisir des commandes manuelles
        /// </summary>
        public bool CommandeManuelleAllowed { get; set; }
    }
}

