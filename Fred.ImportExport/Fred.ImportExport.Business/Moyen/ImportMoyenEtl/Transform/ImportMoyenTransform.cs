using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Fred.Business.Moyen;
using Fred.Business.Moyen.Commun;
using Fred.Business.Referential;
using Fred.Business.ReferentielFixe;
using Fred.Business.Site;
using Fred.Business.Societe;
using Fred.Entities.Moyen;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.CI.ImportMoyenEtl.Result;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using Fred.ImportExport.Models.Moyen;

namespace Fred.ImportExport.Business.CI.ImportMoyenEtl.Transform
{
    /// <summary>
    /// Processus etl : Transformation du resultat.
    /// </summary>
    public class ImportMoyenTransform : IEtlTransform<MoyenModel, MaterielEnt>
    {
        #region Properties 

        private readonly EtlExecutionLogger etlExecutionLogger;

        #endregion Properties 

        #region Managers 

        private readonly Lazy<IEtablissementComptableManager> etablissementComptableManager = new Lazy<IEtablissementComptableManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IEtablissementComptableManager>();
        });

        private readonly Lazy<ISocieteManager> societeManager = new Lazy<ISocieteManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<ISocieteManager>();
        });

        private readonly Lazy<IReferentielFixeManager> referentielFixeManager = new Lazy<IReferentielFixeManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IReferentielFixeManager>();
        });

        private readonly Lazy<IMoyenManager> moyenManager = new Lazy<IMoyenManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IMoyenManager>();
        });

        private readonly Lazy<ISiteManager> siteManager = new Lazy<ISiteManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<ISiteManager>();
        });

        #endregion Managers

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="etlExecutionLogger">etlExecutionLogger</param>  
        public ImportMoyenTransform(EtlExecutionLogger etlExecutionLogger)
        {
            this.etlExecutionLogger = etlExecutionLogger;
        }

        public void Execute(IEtlInput<MoyenModel> input, ref IEtlResult<MaterielEnt> result)
        {
            if (result == null)
            {
                result = new ImportMoyenResult();
            }

            List<EtablissementComptableEnt> etablissementComptables = etablissementComptableManager.Value.GetEtablissementComptableList().ToList();
            List<SocieteEnt> societes = societeManager.Value.GetSocieteListAll().ToList();
            List<SiteEnt> sites = siteManager.Value.GetSites().ToList();
            List<RessourceEnt> ressources = referentielFixeManager.Value.GetRessourceList().ToList();

            foreach (var item in input.Items)
            {
                etlExecutionLogger.LogAndSerialize($"log", item);
                result.Items.Add(CreateMoyenEnt(item, societes, etablissementComptables, sites, ressources));
            }
        }

        private MaterielEnt CreateMoyenEnt(MoyenModel moyenModel, List<SocieteEnt> societes, List<EtablissementComptableEnt> etablissementComptables, List<SiteEnt> sites, List<RessourceEnt> ressources)
        {
            // On récupère les ForeignKeys
            SocieteEnt societe = GetSociete(moyenModel, societes);
            EtablissementComptableEnt etablissementComptable = GetEtablissementComptable(moyenModel, etablissementComptables, societe);
            SiteEnt site = GetSite(moyenModel, sites);
            RessourceEnt ressource = GetRessource(moyenModel, ressources);

            MaterielEnt materiel;

            if (societe.Groupe.Code == Constantes.GFES)
            {
                materiel = GetMoyen(moyenModel.Code, societe.SocieteId, etablissementComptable?.EtablissementComptableId) ?? new MaterielEnt();
            }
            else
            {
                materiel = GetMoyen(moyenModel.Code, societe.SocieteId) ?? new MaterielEnt();
            }

            materiel.Code = moyenModel.Code;
            materiel.Libelle = moyenModel.Libelle;
            materiel.Commentaire = moyenModel.Commentaire;
            materiel.Actif = moyenModel.IsActif;
            materiel.Immatriculation = moyenModel.Immatriculation;
            materiel.DateCreation = moyenModel.DateCreation;
            materiel.DateModification = moyenModel.DateModification;
            materiel.IsLocation = moyenModel.IsLocation;
            materiel.IsImported = moyenModel.IsImported;
            materiel.Fabriquant = moyenModel.Fabriquant;
            materiel.DateMiseEnService = moyenModel.DateMiseEnService;
            materiel.SocieteId = societe.SocieteId;
            materiel.EtablissementComptableId = etablissementComptable?.EtablissementComptableId;
            materiel.SiteAppartenanceId = site?.SiteId;
            materiel.RessourceId = ressource.RessourceId;

            return materiel;
        }

        /// <summary>
        /// Récupération du moyen en utilisant le code du moyen et l'id de la société
        /// </summary>
        /// <param name="moyenCode">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <returns>Le moyen qui corresponds au code et à la société id fournis</returns>
        private MaterielEnt GetMoyen(string moyenCode, int societeId)
        {
            return moyenManager.Value.GetMoyen(moyenCode, societeId);
        }

        /// <summary>
        /// Récupération du moyen en utilisant le code du moyen et l'id de la société
        /// </summary>
        /// <param name="moyenCode">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <param name="etablissementComptableId">Id de l'établiessement comptable</param>
        /// <returns>Le moyen qui corresponds au code et à la société id fournis</returns>
        private MaterielEnt GetMoyen(string moyenCode, int societeId, int? etablissementComptableId)
        {
            return moyenManager.Value.GetMoyen(moyenCode, societeId, etablissementComptableId);
        }

        private SocieteEnt GetSociete(MoyenModel moyenModel, List<SocieteEnt> societes)
        {
            SocieteEnt societe = societes.FirstOrDefault(x => x.CodeSocieteComptable == moyenModel.CodeSociete && x.Active);

            if (societe == null)
            {
                throw new FredBusinessException($"[MOYEN] Import Moyen {moyenModel.Code} : Aucune société pour le CodeSocieteComptable ({moyenModel.CodeSociete})");
            }

            return societe;
        }

        private EtablissementComptableEnt GetEtablissementComptable(MoyenModel moyenModel, List<EtablissementComptableEnt> etablissementComptables, SocieteEnt societe)
        {
            EtablissementComptableEnt etablissementComptable = etablissementComptables.FirstOrDefault(x => x.Code == moyenModel.CodeEtablissement && x.SocieteId == societe.SocieteId);
            if (etablissementComptable == null)
            {
                throw new FredBusinessException($"[MOYEN] Import Moyen {moyenModel.Code} : Aucun etablissementComptable ({moyenModel.CodeEtablissement}) pour la société ({moyenModel.CodeSociete})");
            }

            return etablissementComptable;
        }

        private RessourceEnt GetRessource(MoyenModel moyenModel, List<RessourceEnt> ressources)
        {
            RessourceEnt ressource = ressources.FirstOrDefault(x => x.Code == moyenModel.CodeRessource);

            if (ressource == null)
            {
                throw new FredBusinessException($"[MOYEN] Import Moyen {moyenModel.Code} : Aucune ressource pour le code ({moyenModel.CodeRessource})");
            }

            return ressource;
        }

        /// <summary>
        /// Récupére le site en utilisant le code d'appartenance
        /// </summary>
        /// <param name="moyenModel">Moyen model</param>
        /// <param name="sites">Liste des sites</param>
        /// <returns>Le site avec le code renvoyé</returns>
        private SiteEnt GetSite(MoyenModel moyenModel, List<SiteEnt> sites)
        {
            return sites.FirstOrDefault(x => x.Code == moyenModel.CodeSiteAppartenance);
        }
    }
}
