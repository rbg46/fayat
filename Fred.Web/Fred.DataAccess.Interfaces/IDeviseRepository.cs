
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les devises.
    /// </summary>
    public interface IDeviseRepository : IRepository<DeviseEnt>
    {
        /// <summary>
        ///   Retourne luen nouvelle instance de devise.
        /// </summary>
        /// <returns>Une nouvelle instance de devise.</returns>
        DeviseEnt New();

        /// <summary>
        ///   Retourne la liste des devises.
        /// </summary>
        /// <returns>La liste des devises.</returns>
        IEnumerable<DeviseEnt> GetList();

        /// <summary>
        ///   Retourne la liste complète des devises.
        /// </summary>
        /// <returns>Liste des devises.</returns>
        IQueryable<DeviseEnt> GetAll();

        /// <summary>
        ///   Retourne la devise portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la devise à retrouver.</param>
        /// <returns>Devise retrouvé, sinon null.</returns>
        DeviseEnt GetByCode(string code);

        /// <summary>
        ///   Retourne la devise par id.
        /// </summary>
        /// <param name="id">Id de la devise à retrouver.</param>
        /// <returns>Devise retrouvé, sinon null.</returns>
        DeviseEnt GetById(int id);

        /// <summary>
        ///   Retourne l'identifiant de la devise portant le code devise indiqué.
        /// </summary>
        /// <param name="code">Code de la devise à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon null.</returns>
        int? GetDeviseIdByCode(string code);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer une devise
        /// </summary>
        /// <param name="item">Devise à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(DeviseEnt item);

        /// <summary>
        ///   Permet de récupérer la liste des devises en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les devises</param>
        /// <returns>Retourne la liste filtré de devises</returns>
        IEnumerable<DeviseEnt> SearchDeviseWithFilters(Expression<Func<DeviseEnt, bool>> predicate);


        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);
    }
}
