using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour les associations entre devises et sociétés.
  /// </summary>
  public interface ISocieteDeviseRepository : IRepository<SocieteDeviseEnt>
  {
    /// <summary>
    ///   Retourne la liste de toutes les sociétés
    /// </summary>
    /// <param name="idSociete"> Identifiant de la société </param>
    /// <returns> Liste de toutes les sociétés/Devises </returns>
    IEnumerable<DeviseEnt> GetDeviseListBySociete(int idSociete);

    /// <summary>
    ///   Retourne la liste de devise de reference de la société passée en paramètre
    /// </summary>
    /// <param name="idSociete"> Identifiant de la société </param>
    /// <returns> Liste des sociétés/Devises de ref </returns>
    IQueryable<DeviseEnt> GetListDeviseRefBySociete(int idSociete);

    /// <summary>
    ///   Retourne la liste de devise secondaire de la société passée en paramètre
    /// </summary>
    /// <param name="idSociete"> Identifiant de la société </param>
    /// <returns> Liste des sociétés/Devises secondaire </returns>
    IQueryable<DeviseEnt> GetListDeviseSecBySociete(int idSociete);

    /// <summary>
    ///   Ajout une nouvelle association entre société et devise
    /// </summary>
    /// <param name="societeDeviseEnt"> Association société devise à ajouter</param>
    /// <returns> L'identifiant de la société ajoutée</returns>
    int Add(SocieteDeviseEnt societeDeviseEnt);

    /// <summary>
    ///   Suppression des association devise societe par id societe
    /// </summary>
    /// <param name="idSociete">Id de la societe</param>
    void DeleteByIdSociete(int idSociete);

    /// <summary>
    ///   Log une erreur se déclenchant dans le manager
    /// </summary>
    /// <param name="exception"> Exception pourtant le message d'erreur </param>
    void LogManagerException(Exception exception);

    /// <summary>
    ///   Récupère la liste des association SocieteDevise d'une société
    /// </summary>
    /// <param name="societeId">Identifiant de la société</param>
    /// <returns>Liste des associations SocieteDevise</returns>
    IEnumerable<SocieteDeviseEnt> GetSocieteDeviseList(int societeId);

    /// <summary>
    ///   Suppression des association devise societe
    /// </summary>
    /// <param name="societeDeviseId">Id de la societeDevise</param>
    void DeleteById(int societeDeviseId);

    /// <summary>
    ///   Mise à jour des Devises d'une Société
    /// </summary>
    /// <param name="societeDeviseList">liste des relation Société Devise</param>    
    /// <returns>Liste des SocieteDevise mise à jour</returns>
    IEnumerable<SocieteDeviseEnt> AddOrUpdate(IEnumerable<SocieteDeviseEnt> societeDeviseList);
  }
}