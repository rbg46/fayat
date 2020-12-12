using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.Common.Interfaces
{
     public interface IFtpQueryExecutor
    {
        /// <summary>
        ///   Déversement des lignes de pointages et des primes dans l'AS400
        /// </summary>
        /// <typeparam name="T">ControlePointageEnt ou RemonteeVracEnt</typeparam>    
        /// <param name="globalData">Données globales</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="fredPointages">Liste des pointages FRED à contrôler</param>
        /// <param name="rvgPointagesAndPrimes">Les pointages et les primes de RVG. Peut être null.</param>
        /// <param name="instance">Représente soit ControlePointageEnt ou RemonteeVracEnt</param>       
        void InsertPointageAndPrime<T>(ValidationPointageContextData globalData, DateTime periode, IEnumerable<RapportLigneEnt> fredPointages, RvgPointagesAndPrimes rvgPointagesAndPrimes, T instance) where T : class;
    }
}
