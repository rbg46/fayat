using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Referential;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Interface des gestionnaires des devises
    /// </summary>
    public interface IDeviseManager : IManager<DeviseEnt>
    {
        /// <summary>
        ///   Retourne la liste des devise.
        /// </summary>
        /// <returns>Renvoie la liste des devises.</returns>
        IEnumerable<DeviseEnt> GetList();

        /// <summary>
        ///   Retourne la liste des devises modifiés ou créer depuiis une date si celle-ci est passé en paramètre.
        ///   sinon retourner la liste complètes
        /// </summary>
        /// <param name="sinceDate">date de prise en compte des modifications</param>
        /// <returns>Liste des devises.</returns>
        IQueryable<DeviseEnt> GetAll(DateTime? sinceDate = null);

        /// <summary>
        ///   Obtient la devise par identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la devise.</param>
        /// <returns>La devise</returns>
        DeviseEnt GetById(int id);

        /// <summary>
        ///   Retourne l'identifiant de la devise portant le code devise indiqué.
        /// </summary>
        /// <param name="code">Code de la devise à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon null.</returns>
        int? GetDeviseIdByCode(string code);

        /// <summary>
        ///   Ajout une nouvelle devise.
        /// </summary>
        /// <param name="item"> Devise à ajouter.</param>
        /// <returns> L'identifiant de la devise ajouté.</returns>
        int Add(DeviseEnt item);

        /// <summary>
        ///   Vérifie si le code ISO est disponible
        /// </summary>
        /// <param name="codeIso"> Code Iso Devise à vérifier</param>
        /// <param name="deviseId"> Code de la devise pour éventuellement exclusion</param>
        /// <returns> True si le Code Iso est disponible, sinon False pour spécifier que le code Iso est non disponible.</returns>
        bool CheckUnicityCodeIso(string codeIso, int? deviseId);

        /// <summary>
        ///   Sauvegarde les modifications d'une devise.
        /// </summary>
        /// <param name="item">Devise à modifier</param>
        void Update(DeviseEnt item);

        /// <summary>
        ///   Supprime une devise.
        /// </summary>
        /// <param name="item">La devise à supprimer.</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        bool DeleteById(DeviseEnt item);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer une devise
        /// </summary>
        /// <param name="item">Devise à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(DeviseEnt item);

        /// <summary>
        ///   Permet de récupérer la liste des devises en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les devises</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré des devises</returns>
        IEnumerable<DeviseEnt> SearchDeviseWithFilters(string text, SearchDeviseEnt filters);

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié à une devise
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'une devise</returns>
        SearchDeviseEnt GetDefaultFilter();

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de devise.
        /// </summary>
        /// <returns>Nouvelle instance de devise intialisée</returns>
        DeviseEnt GetNewDevise();

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="ciId">L'identifiant du CI (optionnel pour le filtrage)</param>
        /// <param name="societeId">L'identiant de la société</param>
        /// <param name="organisationId">Identifiant organisation</param>
        /// <param name="text">Le texte recherché</param>
        /// <param name="page">La page courante</param>
        /// <param name="pageSize">La taille de la page</param>
        /// <returns>Une liste d' items de référentiel</returns>
        IEnumerable<DeviseEnt> SearchLight(int? ciId, int? societeId, int? organisationId, string text, int page, int pageSize);

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);

        /// <summary>
        /// Permet de récupérer la devise par son code ISO.
        /// </summary>
        /// <param name="isoCode">Le code ISO.</param>
        /// <returns>Une devise.</returns>
        DeviseEnt GetDevise(string isoCode);

        /// <summary>
        /// Retourne la liste de devise pour une liste de code de devise
        /// </summary>
        /// <param name="codeDevises">Liste de code de devise</param>
        /// <returns>Liste de devise</returns>
        IReadOnlyList<DeviseEnt> GetDevises(List<string> codeDevises);
    }
}
