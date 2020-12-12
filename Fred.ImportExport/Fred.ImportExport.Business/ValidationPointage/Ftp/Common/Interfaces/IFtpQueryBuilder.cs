using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.Common.Interfaces
{
    public interface IFtpQueryBuilder
    {
        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes FRED et RVG dans l'AS400
        /// </summary>
        /// <param name="globalData">onnées globales</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="fredPointages">Liste des pointages FRED à contrôler</param>
        /// <param name="rvgPointagesAndPrimes">Les pointages et les primes de RVG. Peut être null.</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes FRED et RVG dans l'AS400</returns>
        (IEnumerable<string> pointageQueries, IEnumerable<string> primeQueries) GetPointageAndPrimeQueries(ValidationPointageContextData globalData, DateTime periode, IEnumerable<RapportLigneEnt> fredPointages, RvgPointagesAndPrimes rvgPointagesAndPrimes);

        /// <summary>
        /// Récupère les pointages et les primes de RVG.
        /// </summary>
        /// <param name="periode">La période concernée.</param>
        /// <param name="filtre">Le filtre à appliquer.</param>
        /// <returns>Les pointages et les primes de RVG.</returns>
        RvgPointagesAndPrimes GetRvgPointagesAndPrimes(DateTime periode, PointageFiltre filtre);
    }
}
