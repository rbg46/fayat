using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Entities.Referential;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common
{
    public class CommonAnaelSystemContextProvider : ICommonAnaelSystemContextProvider
    {
        private readonly IPaysManager paysManager;
        private readonly ITypeSocieteManager typeSocieteManager;

        public CommonAnaelSystemContextProvider(IPaysManager paysManager, ITypeSocieteManager typeSocieteManager)
        {
            this.paysManager = paysManager;
            this.typeSocieteManager = typeSocieteManager;
        }

        /// <summary>
        /// Recupere toutes les Pays (pays des ci) necessaire a l'import
        /// </summary>
        /// <param name="societesContexts">societesContexts</param>
        /// <returns>Liste de pays</returns>
        public List<PaysEnt> GetPaysOfFournisseurs(List<ImportFournisseurSocieteContext> societesContexts)
        {
            var result = new List<PaysEnt>();

            var allPaysIdsOfFournisseurs = societesContexts.SelectMany(x => x.FredFournisseurs.Select(y => y.PaysId)).Where(x => x.HasValue).Select(x => x.Value).Distinct().ToList();

            if (allPaysIdsOfFournisseurs.Any())
            {
                result = paysManager.GetPaysByIds(allPaysIdsOfFournisseurs);
            }

            return result;
        }

        /// <summary>
        ///  Recupere la liste des types de societe, permettra de savoir si la societe est une sep et donc de savoir si on doit envoyé les ci de la societe a sap
        /// </summary>
        /// <returns>la liste des types de societe</returns>
        public List<TypeSocieteEnt> GetTypeSocietes()
        {
            return typeSocieteManager.GetAll();
        }
    }
}
