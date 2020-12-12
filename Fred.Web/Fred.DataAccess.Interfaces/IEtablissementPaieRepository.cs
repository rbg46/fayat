
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données pour les établissements de paie.
    /// </summary>
    public interface IEtablissementPaieRepository : IRepository<EtablissementPaieEnt>
    {
        /// <summary>
        ///   Retourne la liste des établissements de paie.
        /// </summary>
        /// <returns>La liste des établissements de paie.</returns>
        IEnumerable<EtablissementPaieEnt> GetEtablissementPaieList();

        /// <summary>
        ///   Méthode GET de récupération de tous les établissements de paie éligibles à être une agence de rattachement
        /// </summary>
        /// <param name="currentEtabPaieId">ID de l'établissement de paie à exclure de la recherche</param>
        /// <returns>Retourne une nouvelle instance d'établissement de paie intialisée</returns>
        IEnumerable<EtablissementPaieEnt> AgencesDeRattachement(int currentEtabPaieId);

        /// <summary>
        ///   Retourne l'établissement de paie portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="etablissementPaieId">Identifiant de l'établissement de paie à retrouver.</param>
        /// <returns>L'établissement de paie retrouvé, sinon nulle.</returns>
        EtablissementPaieEnt GetEtablissementPaieById(int etablissementPaieId);

        /// <summary>
        ///   Retourne l'établissement de paie portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="idCourant">Identifiant de l'établissement de paie à retrouver.</param>
        /// <param name="code">code de l'établissement de paie à retrouver.</param>
        /// <param name="libelle">Libelle de l'établissement de paie à retrouver.</param>
        /// <returns> booleen exist</returns>
        bool IsEtablissementPaieExistsByCodeLibelle(int idCourant, string code, string libelle);

        /// <summary>
        ///   Ajoute un nouvel établissement de paie.
        /// </summary>
        /// <param name="etablissementPaie">Etablissement de paie à ajouter</param>
        /// <returns>L'identifiant de l'établissement de paie ajouté</returns>
        int AddEtablissementPaie(EtablissementPaieEnt etablissementPaie);

        /// <summary>
        ///   Sauvegarde les modifications d'un établissement de paie.
        /// </summary>
        /// <param name="etablissementPaie">Etablissement de paie à modifier</param>
        void UpdateEtablissementPaie(EtablissementPaieEnt etablissementPaie);

        /// <summary>
        ///   Supprime un établissement de paie.
        /// </summary>
        /// <param name="etablissementPaieId">L'identifiant du établissement de paie à supprimer</param>
        void DeleteEtablissementPaieById(int etablissementPaieId);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un établissement paie
        /// </summary>
        /// <param name="etablissementPaieEnt">L'établissement paie à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(EtablissementPaieEnt etablissementPaieEnt);

        /// <summary>
        ///   Ligne de recherche
        /// </summary>
        /// <param name="text">Le text recherché</param>
        /// <returns>Renvoie une liste</returns>
        IEnumerable<EtablissementPaieEnt> SearchLight(string text);

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="predicate">Prédicat de recherche de l'etablissement paie</param>
        /// <returns>Retourne la liste filtrée de tous les etablissements paie</returns>
        IEnumerable<EtablissementPaieEnt> SearchEtablissementPaieAllWithFilters(int societeId, Expression<Func<EtablissementPaieEnt, bool>> predicate);

        /// <summary>
        ///   Permet de connaître l'existence d'un code déplacement depuis son code.
        /// </summary>
        /// <param name="idCourant">Identifiant du code déplacement courant</param>
        /// <param name="code">Code de deplacement à vérifier</param>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon.</returns>
        bool IsCodeEtablissementPaieExistsByCode(int idCourant, string code, int societeId);

        /// <summary>
        /// Get etablissement paie by list des societes ids
        /// </summary>
        /// <param name="societesIdList">List des societes id</param>
        /// <returns>List des etabs Paie</returns>
        Task<List<EtablissementPaieEnt>> GetEtabPaieBySocieteIdList(List<int> societesIdList);
    }
}