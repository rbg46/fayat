using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Referential;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Interface des gestionnaires des pays
    /// </summary>
    public interface IPaysManager : IManager<PaysEnt>
    {
        /// <summary>
        ///   Retourne la liste des pays.
        /// </summary>
        /// <returns>Renvoie la liste des pays.</returns>
        IEnumerable<PaysEnt> GetList();

        Task<IEnumerable<PaysEnt>> GetListAsync();

        /// <summary>
        ///   Retourne le pays dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="countryId">Identifiant du pays à retrouver.</param>
        /// <returns>Le pays retrouvé, sinon null.</returns>
        PaysEnt GetById(int countryId);

        /// <summary>
        ///   Ajout un nouveau pays.
        /// </summary>
        /// <param name="countryEnt"> Pays à ajouter.</param>
        /// <returns> L'identifiant du pays ajouté.</returns>
        int Add(PaysEnt countryEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'un pays.
        /// </summary>
        /// <param name="pays">Pays à modifier</param>
        void Update(PaysEnt pays);

        /// <summary>
        ///   Supprime un pays.
        /// </summary>
        /// <param name="id">L'identifiant du pays à supprimer.</param>
        void DeleteById(int id);

        /// <summary>
        ///   Recherche de pays dans le référentiel
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Une liste de personnel</returns>
        IEnumerable<PaysEnt> SearchLight(string text, int page, int pageSize);

        /// <summary>
        ///   Obtient le pays selon son code
        /// </summary>
        /// <param name="code">Code du pays</param>
        /// <returns>Pays retrouvé</returns>
        PaysEnt GetByCode(string code);

        /// <summary>
        ///   Obtient le pays selon son code
        /// </summary>
        /// <param name="libelle">Libellé du pays</param>
        /// <returns>Pays retrouvé</returns>
        PaysEnt GetByLibelle(string libelle);

        /// <summary>
        ///   Retourne la liste des pays recherchés.
        /// </summary>
        /// <param name="paysIds">Liste des pays recherchés</param>
        /// <returns>Renvoie la liste des pays.</returns>
        List<PaysEnt> GetPaysByIds(List<int> paysIds);
    }
}
