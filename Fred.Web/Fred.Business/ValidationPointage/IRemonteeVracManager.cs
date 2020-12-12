using Fred.Entities.ValidationPointage;
using System;
using System.Collections.Generic;

namespace Fred.Business.ValidationPointage
{
  /// <summary>
  ///   Interface Gestionnaire des RemonteeVracEnt
  ///   Une remontée Vrac est une opération exécuté par AS400
  /// </summary>
  public interface IRemonteeVracManager : IManager<RemonteeVracEnt>
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
    ///   Récupère la dernière opération de remontée vrac de l'utilisateur connecté
    /// </summary>    
    /// <param name="periode">Période choisie</param>
    /// <returns>Dernière Remontée Vrac</returns>
    RemonteeVracEnt GetLatest(DateTime periode);

    /// <summary>
    ///   Récupère toutes les remontées vrac
    /// </summary>
    /// <returns>Liste des remontées vrac</returns>
    IEnumerable<RemonteeVracEnt> GetAll();

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
    ///   Mise à jour du statut de la Remontée Vrac
    /// </summary>
    /// <param name="remonteeVrac">Remontée Vrac à mettre à jour</param>   
    /// <param name="status">Identifiant du statut</param>
    /// <returns>Remontée Vrac mise à jour</returns>
    RemonteeVracEnt UpdateRemonteeVrac(RemonteeVracEnt remonteeVrac, int status);

    /// <summary>
    ///   Supprime un Remontée Vrac par son identifiant
    /// </summary>
    /// <param name="remonteeVracId">Identifiant de la Remontée Vrac</param>
    void DeleteremonteeVrac(int remonteeVracId);

    #region RemonteeVracErreurEnt 

    /// <summary>
    ///   Ajoute une erreur de remontée vrac
    /// </summary>
    /// <param name="rvErreur">RemonteeVracErreur à ajouter</param>
    /// <returns>RemonteeVracErreur ajoutée</returns>
    RemonteeVracErreurEnt AddRemonteeVracErreur(RemonteeVracErreurEnt rvErreur);

    /// <summary>
    ///   Récupère le nombre d'erreur de remontée vrac en fonction d'une RemonteeVracId
    /// </summary>
    /// <param name="remonteeVracId">Identifiant de la remontée vrac</param>
    /// <param name="searchText">Texte a recherché</param>
    /// <returns>Nombre d'erreur pour une remontée vrac</returns>
    int CountRemonteeVracErreur(int remonteeVracId, string searchText);

    /// <summary>
    ///   Récupère la liste des personnels avec leurs erreurs
    /// </summary>
    /// <param name="remonteeVracId">Identifiant de la remontée vrac</param>
    /// <param name="searchText">Texte recherché</param>
    /// <param name="page">Numéro de la page</param>
    /// <param name="pageSize">Taille de la page</param>
    /// <returns>Liste des personnels avec erreurs</returns>
    IEnumerable<PersonnelErreur<RemonteeVracErreurEnt>> GetPersonnelErreurList(int remonteeVracId, string searchText, int page, int pageSize);

    /// <summary>
    ///   Récupère la liste des personnels avec leurs erreurs
    /// </summary>
    /// <param name="remonteeVracId">Identifiant de la remontée vrac</param>        
    /// <returns>Liste des personnels avec erreurs</returns>
    IEnumerable<PersonnelErreur<RemonteeVracErreurEnt>> GetPersonnelErreurList(int remonteeVracId);

    #endregion
  }
}