using System;
using System.Collections.Generic;
using Fred.Entities.Depense;


namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Service qui Fournie les receptions pour l'affichage sur la page 'Tableaux des receptions'
    /// </summary>
    public interface ISearchReceptionsService : IService
    {
        /// <summary>
        ///   Récupération des réceptions en fonction du filtre
        /// </summary>
        /// <param name="filter">Filtre des réceptions</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page (nombre de résultat par page)</param>
        /// <returns>Objet résultat</returns>
        TableauReceptionResult SearchReceptionsWithTotals(SearchDepenseEnt filter, int? page, int? pageSize);

        /// <summary>
        ///  Récupération des réceptions en fonction d'une liste d'id
        /// </summary>
        /// <param name="dateFrom">date depart du filtre</param>
        /// <param name="dateTo">date de fin du filtre</param>
        /// <param name="receptionsIds">receptionsIds</param>
        /// <returns>Une liste de receptions</returns>
        List<DepenseAchatEnt> SearchReceptionsByIds(DateTime? dateFrom, DateTime? dateTo, List<int> receptionsIds);

    }
}
