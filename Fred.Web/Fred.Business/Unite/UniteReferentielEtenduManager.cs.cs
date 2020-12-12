using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;

namespace Fred.Business.Unite
{
    /// <summary>
    ///  Gestionnaire des Unites.
    /// </summary>
    public class UniteReferentielEtenduManager : Manager<UniteReferentielEtenduEnt, IUniteReferentielEtenduRepository>, IUniteReferentielEtenduManager
    {
        public UniteReferentielEtenduManager(IUnitOfWork uow, IUniteReferentielEtenduRepository uniteReferentielEtenduRepository)
            : base(uow, uniteReferentielEtenduRepository)
        {
        }

        /// <summary>
        /// Retourne la liste des unités en fonction d'une société et d'une ressource 
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <returns>La liste des unités</returns>
        public IEnumerable<UniteEnt> GetListUniteByRessourceId(int societeId, int ressourceId)
        {
            return this.Repository.Query().Include(ur => ur.Unite).Include(ur => ur.ReferentielEtendu)
                 .Filter(u => u.ReferentielEtendu.SocieteId == societeId && u.ReferentielEtendu.RessourceId == ressourceId)
                 .OrderBy(ls => ls.OrderBy(c => c.Unite.Code)).Get()
                 .Select(ur => ur.Unite);
        }

        /// <summary>
        ///   Méthode de recherche des unités
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <returns>Une liste des unités</returns>
        public IEnumerable<UniteEnt> SearchLight(string text, int page, int pageSize, int societeId, int ressourceId)
        {
            return this.Repository.Query().Include(ur => ur.Unite).Include(ur => ur.ReferentielEtendu)
                       .Filter(u => u.ReferentielEtendu.SocieteId == societeId && u.ReferentielEtendu.RessourceId == ressourceId && (string.IsNullOrEmpty(text) || u.Unite.Code.ToLower().Contains(text.ToLower()) || u.Unite.Libelle.ToLower().Contains(text.ToLower())))
                       .OrderBy(ls => ls.OrderBy(c => c.Unite.Code))
                       .GetPage(page, pageSize)
                       .Select(ur => ur.Unite);
        }
    }
}
