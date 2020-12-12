
using System;
using System.Collections.Generic;
using Fred.Entities.ValidationPointage;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Interface IRemonteeVracRepository
    /// </summary>
    public interface IRemonteeVracRepository : IRepository<RemonteeVracEnt>
    {
        /// <summary>
        ///   Récupère une RemonteeVrac en fonction de son Identifiant
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la Remontée Vrac</param>
        /// <returns>Remontée Vrac</returns>
        RemonteeVracEnt Get(int remonteeVracId);

        /// <summary>
        ///   Récupère la dernière opération de remontée vrac
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période choisie</param>
        /// <returns>Dernière Remontée Vrac</returns>
        RemonteeVracEnt GetLatest(int utilisateurId, DateTime periode);

        /// <summary>
        ///   Récupère toutes les remontées vrac
        /// </summary>
        /// <returns>Liste des remontées vrac</returns>
        IEnumerable<RemonteeVracEnt> GetAll();

        /// <summary>
        ///   Récupère la liste des remontées vrac par période
        /// </summary>
        /// <param name="periode">période choisie</param>
        /// <returns>Liste des lots de pointage filtré</returns>
        IEnumerable<RemonteeVracEnt> GetList(DateTime periode);

        /// <summary>
        ///   Récupère la liste des remontées vrac par auteur création
        /// </summary>
        /// <param name="auteurCreationId">Identifiant de l'auteur de création</param>
        /// <returns>Liste des lots de pointage filtré</returns>
        IEnumerable<RemonteeVracEnt> GetList(int auteurCreationId);

        /// <summary>
        ///   Ajoute un nouveau Remontée Vrac
        /// </summary>
        /// <param name="remonteeVrac">Remontée Vrac a ajouter</param>    
        /// <returns>Remontée Vrac ajoutée</returns>
        RemonteeVracEnt AddRemonteeVrac(RemonteeVracEnt remonteeVrac);

        /// <summary>
        ///   Mise à jour de la Remontée Vrac
        /// </summary>
        /// <param name="remonteeVrac">Remontée Vrac à mettre à jour</param>    
        /// <returns>Remontée Vrac mise à jour</returns>
        RemonteeVracEnt UpdateRemonteeVrac(RemonteeVracEnt remonteeVrac);

        /// <summary>
        ///   Supprime un Remontée Vrac par son identifiant
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la Remontée Vrac</param>
        void DeleteRemonteeVrac(int remonteeVracId);
    }
}
