using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.Web.Shared.Models.ValidationPointage;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.Common
{
    /// <summary>
    /// Builder qui construit les requetes du controle vrac
    /// </summary>
    public interface IFesQueryBuilder
    {
        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400
        /// </summary>
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="periode">Période choisie</param>       
        /// <param name="rapportLignes">Liste des pointages FRED à contrôler</param>
        /// <param name="listPrimeParameters">Liste de modèle contenant les paramètres des requêtes d'insertion des primes</param>
        /// <param name="listPointageParameters">Liste de modèle contenant les paramètres des requêtes d'insertion des pointages</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400</returns>
        IEnumerable<QueryInfo> BuildAnaelInsertsQueries(ValidationPointageContextData globalData, DateTime periode, ICollection<RapportLigneEnt> rapportLignes, out List<InsertQueryPrimeParametersModel> listPrimeParameters, out List<InsertQueryPointageParametersModel> listPointageParameters);
    }
}
