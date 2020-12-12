using System.Collections.Generic;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context.Common
{
    public class CommonAnaelSystemContextProvider : ICommonAnaelSystemContextProvider
    {
        private readonly ISocieteManager societeManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IPaysManager paysManager;
        private readonly ITypeSocieteManager typeSocieteManager;

        public CommonAnaelSystemContextProvider(
            ISocieteManager societeManager,
            IPersonnelManager personnelManager,
            IPaysManager paysManager,
            ITypeSocieteManager typeSocieteManager)
        {
            this.societeManager = societeManager;
            this.personnelManager = personnelManager;
            this.paysManager = paysManager;
            this.typeSocieteManager = typeSocieteManager;
        }

        /// <summary>
        /// Recupere tous les responsables necessaire a l'import
        /// </summary>
        /// <param name="societesContexts">societesContexts</param>
        /// <returns>liste de responsables</returns>
        public List<PersonnelEnt> GetResponsables(List<ImportCiSocieteContext> societesContexts)
        {

            var allFredCis = societesContexts.SelectMany(x => x.FredCis);

            var allResponsableChantierIds = allFredCis.Where(x => x.ResponsableChantierId != null).Select(x => x.ResponsableChantierId.Value).ToList();

            var allRResponsableAdministratifIds = allFredCis.Where(x => x.ResponsableAdministratifId != null).Select(x => x.ResponsableAdministratifId.Value).ToList();

            var allResponsables = new List<int>();

            allResponsables.AddRange(allResponsableChantierIds);

            allResponsables.AddRange(allRResponsableAdministratifIds);

            allResponsables = allResponsables.Distinct().ToList();

            return personnelManager.GetPersonnelsByIds(allResponsables);
        }

        /// <summary>
        /// Recupere toutes les societs des responsables necessaire a l'import
        /// </summary>
        /// <param name="allResponsables">allResponsables</param>
        /// <returns>Liste de societes</returns>
        public List<SocieteEnt> GetSocietesOfResponsables(List<PersonnelEnt> allResponsables)
        {
            var result = new List<SocieteEnt>();
            var allSocieteIdsOfResponsables = allResponsables.Select(y => y.SocieteId).Where(x => x.HasValue).Select(x => x.Value).Distinct().ToList();

            if (allSocieteIdsOfResponsables.Any())
            {
                result = societeManager.GetAllSocietesByIds(allSocieteIdsOfResponsables);
            }

            return result;
        }

        /// <summary>
        /// Recupere toutes les Pays (pays des ci) necessaire a l'import
        /// </summary>
        /// <param name="societesContexts">societesContexts</param>
        /// <returns>Liste de pays</returns>
        public List<PaysEnt> GetPaysOfCis(List<ImportCiSocieteContext> societesContexts)
        {
            var result = new List<PaysEnt>();

            var allPaysIdsOfCis = societesContexts.SelectMany(x => x.FredCis.Select(y => y.PaysId)).Where(x => x.HasValue).Select(x => x.Value).Distinct().ToList();

            if (allPaysIdsOfCis.Any())
            {
                result = paysManager.GetPaysByIds(allPaysIdsOfCis);
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
