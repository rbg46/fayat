using System.Collections.Generic;

using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.Unite
{
    /// <summary>
    ///  Gestionnaire des Unites.
    /// </summary>
    public class UniteManager : Manager<UniteEnt, IUniteRepository>, IUniteManager
    {
        public UniteManager(IUnitOfWork uow, IUniteRepository uniteRepository)
          : base(uow, uniteRepository)
        { }

        /// <summary>
        /// Retourne l'unité en fonction de son code
        /// </summary>
        /// <param name="codeUnite">Code de l'unité</param>
        /// <returns>Unité</returns>
        public UniteEnt GetUnite(string codeUnite)
        {
            return Repository.Get().AsNoTracking().FirstOrDefault(u => u.Code == codeUnite);
        }

        /// <summary>
        ///   Méthode de recherche des unités
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>    
        /// <returns>Une liste des unités</returns>
        public IEnumerable<UniteEnt> SearchLight(string text, int page, int pageSize)
        {
            return Repository.Query()
                       .Filter(c => string.IsNullOrEmpty(text) || c.Code.ToLower().Contains(text.ToLower()) || c.Libelle.ToLower().Contains(text.ToLower()))
                       .OrderBy(ls => ls.OrderBy(c => c.Code))
                       .GetPage(page, pageSize);
        }

        /// <summary>
        ///   Retourne une liste d'identifiant unique d'unité correspondant au code passé en paramètre
        /// </summary>
        /// <param name="listCode">Liste code unité</param>   
        /// <returns>Une liste des identifiants unique d'unité</returns>
        public List<int> GetUniteIdsByListCode(List<string> listCode)
        {
            return Repository.Query()
                       .Filter(c => listCode.Contains(c.Code))
                       .Get()
                       .Select(c => c.UniteId)
                       .ToList();
        }

        /// <summary>
        /// Retourne une liste d'unite en fonction d'identifiant
        /// </summary>
        /// <param name="uniteIds">Identifiant des Unités</param>
        /// <returns>Liste de <see cref="UniteEnt" /></returns>
        public IReadOnlyList<UniteEnt> GetUnites(List<int> uniteIds)
        {
            return Repository.Get(uniteIds).ToList();
        }

        /// <summary>
        /// Retourne une liste d'identifiant unique d'unité correspondant au code passé en paramètre
        /// </summary>
        /// <param name="uniteCodes">Liste des codes des unités</param>
        /// <returns>Liste de <see cref="UniteEnt"/></returns>
        public IReadOnlyList<UniteEnt> GetUnites(List<string> uniteCodes)
        {
            return Repository.Get(uniteCodes).ToList();
        }
    }
}

