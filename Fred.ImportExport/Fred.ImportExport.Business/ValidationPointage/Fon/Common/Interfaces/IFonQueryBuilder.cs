using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.Common
{
    /// <summary>
    /// Builder de requête sql ANAEL
    /// </summary>
    public interface IFonQueryBuilder
    {
        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400
        /// </summary>
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="periode">Période choisie</param> 
        /// <param name="rapportLignePrime">Ligne rapport prime</param>
        /// <param name="rapportLignes">Liste des pointages FRED à contrôler</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400</returns>
        IEnumerable<QueryInfo> BuildAnaelInsertsQueries(ValidationPointageContextData globalData, DateTime periode, List<RapportLignePrimeEnt> rapportLignePrime, ICollection<RapportLigneEnt> rapportLignes);
    }
}
