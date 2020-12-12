using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Commande;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Filtre search commande énergie
    /// </summary>
    public class SearchCommandeEnergieModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SearchCommandeEnergieModel()
        {
            Predicats = new List<Expression<Func<CommandeEnt, bool>>>();
        }

        /// <summary>
        /// Obtient ou définit le numéro de la page
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Obtient ou définit la taille de la page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Obtient ou définit la Période
        /// </summary>
        public DateTime Periode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du CI
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du fournisseur
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit une liste d'identifiant de statut de commande
        /// </summary>
        public List<int> StatutCommandeIds { get; set; }

        /// <summary>
        /// Obtient ou définit une liste d'identifiants de type d'énergie
        /// </summary>
        public List<int> TypeEnergieIds { get; set; }

        /// <summary>
        /// Liste des CIs visible à l'utilisateur connecter
        /// </summary>
        public List<int> ListCis { get; set; }


        /// <summary>
        /// Obtient la liste de prédicats
        /// </summary>
        public List<Expression<Func<CommandeEnt, bool>>> Predicats { get; set; }

        /// <summary>
        /// Construs la liste de prédicats de recherche
        /// </summary>
        /// <returns>liste de prédicats de recherche</returns>
        public List<Expression<Func<CommandeEnt, bool>>> GetDefaultPredicats()
        {

            List<Expression<Func<CommandeEnt, bool>>> predicats = new List<Expression<Func<CommandeEnt, bool>>>
            {
                x => Periode.Month == x.Date.Month && Periode.Year == x.Date.Year,
                x => !CiId.HasValue || x.CiId == CiId.Value, 
                x => ListCis.Contains(x.CiId.Value),
                x => !FournisseurId.HasValue || x.FournisseurId == FournisseurId.Value,
                x => x.TypeEnergieId.HasValue && x.IsEnergie,
                x => !x.DateSuppression.HasValue
            };

            Predicats.Clear();
            Predicats.AddRange(predicats);

            return predicats;
        }
    }
}
