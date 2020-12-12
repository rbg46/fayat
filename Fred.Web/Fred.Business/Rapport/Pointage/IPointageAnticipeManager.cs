using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    ///   Interface des gestionnaires des pointages
    /// </summary>
    public interface IPointageAnticipeManager : IManager<PointageAnticipeEnt>
    {
        /// <summary>
        ///   Ajoute une prime au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="pointage">Le pointage</param>
        /// <param name="prime">La prime</param>
        /// <returns>Un rapport</returns>
        PointageBase AddPrimeToPointage(PointageBase pointage, PrimeEnt prime);

        /// <summary>
        ///   Créer une ligne de rapport vide
        /// </summary>
        /// <param name="pointage">La ligne du rapport</param>
        /// <param name="prime">La prime</param>
        /// <returns>Une ligne de rapport</returns>
        PointagePrimeBase GetNewPointagePrime(PointageBase pointage, PrimeEnt prime);

        /// <summary>
        ///   Duplique une liste de lignes de rapport
        /// </summary>
        /// <param name="listPointage">La liste de lignes de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de rapport</returns>
        IEnumerable<PointageBase> DuplicateListPointage(IEnumerable<PointageBase> listPointage, bool emptyValues = false);

        /// <summary>
        ///   Duplique une liste de lignes de rapport
        /// </summary>
        /// <param name="listPointage">La liste de lignes de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de rapport</returns>
        IEnumerable<PointageAnticipeEnt> DuplicateListPointageAnticipe(IEnumerable<PointageAnticipeEnt> listPointage, bool emptyValues = false);

        /// <summary>
        ///   Duplique une de lignes de rapport
        /// </summary>
        /// <param name="pointage">Une ligne de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de rapport</returns>
        PointageBase DuplicatePointage(PointageBase pointage, bool emptyValues = false);

        /// <summary>
        ///   Duplique une ligne de rapport
        /// </summary>
        /// <param name="pointageAnticipe">La ligne de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de rapport</returns>
        PointageAnticipeEnt DuplicatePointageAnticipe(PointageAnticipeEnt pointageAnticipe, bool emptyValues = false);

        /// <summary>
        ///   Duplique une ligne de prime de rapport
        /// </summary>
        /// <param name="pointagePrime">La ligne de prime de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de prime de rapport</returns>
        PointagePrimeBase DuplicatePointagePrime(PointagePrimeBase pointagePrime, bool emptyValues = false);

        /// <summary>
        ///   Duplique une ligne de rapport
        /// </summary>
        /// <param name="pointagePrime">La ligne de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de rapport</returns>
        PointageAnticipePrimeEnt DuplicatePointagePrimeAnticipe(PointageAnticipePrimeEnt pointagePrime, bool emptyValues = false);

        /// <summary>
        ///   Ajoute une ligne de rapport
        /// </summary>
        /// <param name="pointage">La base de pointage</param>
        void AddPointage(PointageBase pointage);

        /// <summary>
        ///   Met à jour une ligne de rapport
        /// </summary>
        /// <param name="pointage">pointage anticipé ou réel à mettre à jour</param>
        void UpdatePointage(PointageBase pointage);

        /// <summary>
        ///   Supprime une ligne de rapport
        /// </summary>
        /// <param name="pointage">La ligne de rapport à supprimer</param>
        void DeletePointage(PointageBase pointage);

        /// <summary>
        ///   Ajoute une ligne de prime à un pointage réel ou anticipe
        /// </summary>
        /// <param name="pointagePrime">Ligne de prime ajouter au pointage</param>
        void AddPointagePrime(PointagePrimeBase pointagePrime);

        /// <summary>
        ///   Met à jour une ligne de prime
        /// </summary>
        /// <param name="pointagePrime">Ligne de prime à mettre à jour dans le pointage réel ou anticipé</param>
        void UpdatePointagePrime(PointagePrimeBase pointagePrime);

        /// <summary>
        ///   Supprime une prime d'un pointage
        /// </summary>
        /// <param name="pointagePrime">Ligne de prime à supprimer dans le pointage réel ou anticipé</param>
        void DeletePointagePrime(PointagePrimeBase pointagePrime);

        /// <summary>
        ///   Retourne une liste de pointages anticipés en fonction du personnel et d'une date
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="date">La date du pointage</param>
        /// <returns>Une liste de pointages anticipés</returns>
        IEnumerable<PointageAnticipeEnt> GetListPointagesAnticipesByPersonnelIdAndDatePointage(int personnelId, DateTime date);
    }
}
