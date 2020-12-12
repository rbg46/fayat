using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Gestionnaire des pays.
    /// </summary>
    public class PaysManager : Manager<PaysEnt, IPaysRepository>, IPaysManager
    {
        public PaysManager(IUnitOfWork uow, IPaysRepository paysRepository)
          : base(uow, paysRepository)
        { }

        /// <summary>
        ///   Retourne la liste des pays.
        /// </summary>
        /// <returns>Liste des pays.</returns>
        public IEnumerable<PaysEnt> GetList()
        {
            return Repository.GetList();
        }

        public async Task<IEnumerable<PaysEnt>> GetListAsync()
        {
            return await Repository.GetListAsync().ConfigureAwait(false);
        }

        /// <summary>
        ///   Retourne le pays dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="countryId">Identifiant du pays à retrouver.</param>
        /// <returns>Le pays retrouvé, sinon null.</returns>
        public PaysEnt GetById(int countryId)
        {
            return Repository.FindById(countryId);
        }

        /// <summary>
        ///   Ajout un nouveau pays
        /// </summary>
        /// <param name="countryEnt">Pays à ajouter</param>
        /// <returns>L'identifiant du pays ajouté</returns>
        public int Add(PaysEnt countryEnt)
        {
            Repository.Insert(countryEnt);
            Save();
            return countryEnt.PaysId;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un pays
        /// </summary>
        /// <param name="pays">Pays à modifier</param>
        public void Update(PaysEnt pays)
        {
            Repository.Update(pays);
            Save();
        }

        /// <summary>
        ///   Supprime un pays
        /// </summary>
        /// <param name="id">L'identifiant du pays à supprimer</param>
        public void DeleteById(int id)
        {
            Repository.DeleteById(id);
        }

        /// <summary>
        ///   Recherche de pays dans le référentiel
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Une liste de pays</returns>
        public IEnumerable<PaysEnt> SearchLight(string text, int page, int pageSize)
        {
            return Repository
                            .Query()
                            .Filter(p => string.IsNullOrEmpty(text)
#pragma warning disable RCS1155 // Use StringComparison when comparing strings.
                              || p.Code.ToLower().Contains(text.ToLower())
                                    || p.Libelle.ToLower().Contains(text.ToLower()))
#pragma warning restore RCS1155 // Use StringComparison when comparing strings.
                      .OrderBy(list => list.OrderBy(pe => pe.Libelle))
                            .GetPage(page, pageSize);
        }

        /// <inheritdoc/>
        public PaysEnt GetByCode(string code)
        {
            return this.Repository.Query().Get().FirstOrDefault(x => string.Compare(x.Code, code, true) == 0);
        }

        /// <inheritdoc/>
        public PaysEnt GetByLibelle(string libelle)
        {
#pragma warning disable RCS1155 // Use StringComparison when comparing strings.
            return this.Repository.Query().Get().AsNoTracking().FirstOrDefault(x => x.Libelle.ToLower() == libelle.ToLower());
#pragma warning restore RCS1155 // Use StringComparison when comparing strings.
        }


        /// <summary>
        ///   Retourne la liste des pays recherchés.
        /// </summary>
        /// <param name="paysIds">Liste des pays recherchés</param>
        /// <returns>Renvoie la liste des pays.</returns>
        public List<PaysEnt> GetPaysByIds(List<int> paysIds)
        {
            return this.Repository.Query().Filter(x => paysIds.Contains(x.PaysId)).Get().AsNoTracking().ToList();
        }


    }
}
