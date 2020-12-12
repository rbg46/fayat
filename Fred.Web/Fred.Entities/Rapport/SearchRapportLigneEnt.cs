using System;
using System.Linq.Expressions;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente une recherche sur les pointages
    /// </summary>
    public class SearchRapportLigneEnt
    {
        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la date minimale pour laquelle récupérer les pointages
        /// </summary>
        public DateTime DatePointageMin { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la date maximale pour laquelle récupérer les pointages
        /// </summary>
        public DateTime DatePointageMax { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le personnel pour lequel récupérer les pointages
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la récupération des pointages réels
        /// </summary>
        public bool IsReel { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la récupération des pointages prévisionnels
        /// </summary>
        public bool IsPrevisionnel { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'ordre de tri sur les dates
        /// </summary>
        public bool? DatePointageAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'ordre de tri sur le type de pointages
        /// </summary>
        public bool? ReelFirst { get; set; }

        /// <summary>
        ///   Retourne l'expression de la recherche. A utiliser dans un IQueryable.
        /// </summary>
        /// <returns>Retourne l'expression</returns>
        public Expression<Func<RapportLigneEnt, bool>> GetExpressionWhere()
        {
            return p => p.DatePointage >= DatePointageMin
                        && p.DatePointage < DatePointageMax
                        &&
                        (p.PersonnelId.HasValue && p.PersonnelId.Value == PersonnelId
                         || !PersonnelId.HasValue);
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche
        /// </summary>
        /// <returns>Retourne la condition de recherche</returns>
        public Func<PointageAnticipeEnt, bool> GetPredicateWherePointageAnticipe()
        {
            return p => p.DatePointage >= DatePointageMin
                        && p.DatePointage < DatePointageMax
                        &&
                        (p.PersonnelId.HasValue && p.PersonnelId.Value == PersonnelId
                         || !PersonnelId.HasValue);
        }

        /// <summary>
        ///   Permet de récupérer le prédicat d'ordonnacement ascendant
        /// </summary>
        /// <returns>prédicat d'ordonnacement ascendant</returns>
        public Func<RapportLigneEnt, string> GetPredicateOrderByAsc()
        {
            Func<RapportLigneEnt, string> orderByAsc;
            orderByAsc = p => DatePointageAsc.HasValue && DatePointageAsc.Value ? p.DatePointage.ToString("dd/MM/yyyy") : DateTime.MinValue.ToString("dd/MM/yyyy");
            return orderByAsc;
        }

        /// <summary>
        ///   Permet de récupérer le prédicat d'ordonnacement descendant
        /// </summary>
        /// <returns>prédicat d'ordonnacement descendant</returns>
        public Func<RapportLigneEnt, string> GetPredicateOrderByDesc()
        {
            Func<RapportLigneEnt, string> orderByDesc;
            orderByDesc = p => DatePointageAsc.HasValue && !DatePointageAsc.Value ? p.DatePointage.ToString("dd/MM/yyyy") : DateTime.MinValue.ToString("dd/MM/yyyy");
            return orderByDesc;
        }
    }
}
