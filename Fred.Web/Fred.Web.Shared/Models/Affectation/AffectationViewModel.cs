using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Affectation
{
    /// <summary>
    /// Affectation view model
    /// </summary>
    public class AffectationViewModel
    {
        /// <summary>
        ///  Obtient ou definit l'affectation identifiers
        /// </summary>
        public int AffectationId { get; set; }

        /// <summary>
        /// Obtient ou definit personnel identifier
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou definit le code du societe
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Obtient ou definit personnel nom 
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou definit personnel prenom
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Obtient ou definit statut du personnel
        /// </summary>
        public string Statut { get; set; }

        /// <summary>
        /// Obtient ou definit matricule du personnel
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Obtient ou definit si le personnel est dans l'equipe favorite
        /// </summary>
        public bool IsInFavoriteTeam { get; set; }

        /// <summary>
        /// Obtient ou definit si le personnel est delegue
        /// </summary>
        public bool IsDelegate { get; set; }

        /// <summary>
        /// Obtient ou definit si le personnel a eté supprimer d'une affaire
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// Obtient ou definit la liste des astreintes
        /// </summary>
        public IEnumerable<AstreinteViewModel> Astreintes { get; set; }

        /// <summary>
        /// Obtient ou définit si le CI est celui par defaut
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
