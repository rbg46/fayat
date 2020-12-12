using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.RapportPrime;

namespace Fred.Business.RapportPrime
{
    /// <summary>
    /// Interface RapportPrimeLignePrimeManager
    /// </summary>
    public interface IRapportPrimeLignePrimeManager : IManager<RapportPrimeLignePrimeEnt>
    {
        /// <summary>
        /// Indique si une ligne de prime peux être envoyé vers Anael
        /// </summary>
        /// <param name="rapportPrimeLigne">Ligne de prime du rapport de prime</param>
        /// <returns>True si la ligne peux être envoyée</returns>
        bool CanBeSendToAnael(RapportPrimeLigneEnt rapportPrimeLigne);

        /// <summary>
        /// Permet la mise à jour de certains champs de l'entité <see cref="RapportPrimeLignePrimeEnt"/>
        /// </summary>
        /// <param name="rapportPrimeLignePrimeEnt">Entité  <see cref="RapportPrimeLignePrimeEnt"/></param>
        /// <param name="fieldsToUpdate">List des champs à mettre à jour</param>
        void QuickUpdate(RapportPrimeLignePrimeEnt rapportPrimeLignePrimeEnt, List<Expression<Func<RapportPrimeLignePrimeEnt, object>>> fieldsToUpdate);

        /// <summary>
        /// Renvoie la liste des lignes de rapports de ligne de primes donné sur une periode
        /// </summary>
        /// <param name="periode">Periode choisie</param>
        /// <param name="isSendToAnael">Indique si l'on doit retourner les lignes déjà envoyé a ANAEL</param>
        /// <returns>Liste de RapportPrimeLignePrimeEnt</returns>
        List<RapportPrimeLignePrimeEnt> GetRapportPrimeLignePrime(DateTime periode, bool isSendToAnael);
    }
}
