using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Fred.Business.CI;
using Fred.Business.Referential;
using Fred.Business.Referential.Tache;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.CI.ImportTacheEtl.Result;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using Fred.ImportExport.Models.Tache;

namespace Fred.ImportExport.Business.CI.ImportTacheEtl.Transform
{
    /// <summary>
    /// Processus etl : Transformation du resultat.
    /// </summary>
    public class ImportTacheTransform : IEtlTransform<TacheModel, TacheEnt>
    {
        #region Properties 

        private readonly EtlExecutionLogger etlExecutionLogger;

        #endregion Properties 

        #region Managers 

        private readonly Lazy<ICIManager> ciManager = new Lazy<ICIManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<ICIManager>();
        });

        private readonly Lazy<ISocieteManager> societeManager = new Lazy<ISocieteManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<ISocieteManager>();
        });

        private readonly Lazy<ITacheManager> tacheManager = new Lazy<ITacheManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<ITacheManager>();
        });

        private readonly Lazy<IUtilisateurManager> utilisateurManager = new Lazy<IUtilisateurManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IUtilisateurManager>();
        });

        private readonly Lazy<IEtablissementComptableManager> etablissementComptableManager = new Lazy<IEtablissementComptableManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IEtablissementComptableManager>();
        });

        #endregion Managers

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="etlExecutionLogger">etlExecutionLogger</param>  
        public ImportTacheTransform(EtlExecutionLogger etlExecutionLogger)
        {
            this.etlExecutionLogger = etlExecutionLogger;
        }

        public void Execute(IEtlInput<TacheModel> input, ref IEtlResult<TacheEnt> result)
        {
            if (result == null)
            {
                result = new ImportTacheResult();
            }

            List<CIEnt> cis = ciManager.Value.GetCIList().ToList();
            List<SocieteEnt> societes = societeManager.Value.GetSocieteListAll().ToList();
            List<EtablissementComptableEnt> etablissementComptables = etablissementComptableManager.Value.GetEtablissementComptableList().ToList();

            foreach (var item in input.Items)
            {
                etlExecutionLogger.LogAndSerialize($"log", item);
                result.Items.Add(CreateTacheEnt(item, cis, societes, etablissementComptables));
            }
        }

        private TacheEnt CreateTacheEnt(TacheModel tacheModel, List<CIEnt> cis, List<SocieteEnt> societes, List<EtablissementComptableEnt> etablissementComptables)
        {
            // On récupère les ForeignKeys
            SocieteEnt societe = GetSociete(tacheModel, societes);
            EtablissementComptableEnt etablissementComptable = GetEtablissementComptable(tacheModel, etablissementComptables, societe);
            CIEnt ci = GetCi(tacheModel, societe.SocieteId, etablissementComptable.EtablissementComptableId, cis);


            TacheEnt tache = GetTache(tacheModel, ci.CiId);
            TacheEnt tacheParent = GetTacheParent(tacheModel, ci.CiId);

            tache.Code = tacheModel.Code;
            tache.Libelle = tacheModel.Libelle;
            tache.TacheParDefaut = false;
            tache.Niveau = 3;
            tache.Active = true;
            tache.TacheType = 0;
            tache.ParentId = tacheParent?.TacheId;
            tache.CiId = ci.CiId;

            return tache;
        }

        private TacheEnt GetTacheParent(TacheModel tacheModel, int ciId)
        {
            TacheEnt tache = tacheManager.Value.GetTacheListByCiIdAndNiveau(ciId, 2).FirstOrDefault(x => x.Code == "0000");

            if (tache == null)
            {
                throw new FredBusinessException($"[TACHE] Import Tache {tacheModel.Code} : Aucun tache par défaut pour l'identifiant CI ({ciId}) et le niveau (2)");
            }

            return tache;
        }

        private TacheEnt GetTache(TacheModel tacheModel, int ciId)
        {
            UtilisateurEnt utilisateur = GetUtilisateurFredIE(tacheModel);

            TacheEnt tache = tacheManager.Value.GetTache(tacheModel.Code, ciId);
            if (tache == null)
            {
                tache = new TacheEnt();
                tache.DateCreation = DateTime.UtcNow;
                tache.AuteurCreationId = utilisateur.UtilisateurId;
            }
            else
            {
                tache.DateModification = DateTime.UtcNow;
                tache.AuteurModificationId = utilisateur.UtilisateurId;
            }

            return tache;
        }

        private UtilisateurEnt GetUtilisateurFredIE(TacheModel tacheModel)
        {
            UtilisateurEnt utilisateur = utilisateurManager.Value.GetByLogin("fred_ie");

            if (utilisateur == null)
            {
                throw new FredBusinessException($"[TACHE] Import Tache {tacheModel.Code} : Aucun utilisateur pour le code (fred_ie)");
            }

            return utilisateur;
        }

        private CIEnt GetCi(TacheModel tacheModel, int societeId, int etablissementComptableId, List<CIEnt> cis)
        {
            CIEnt ci = cis.FirstOrDefault(x => x.Code == tacheModel.CodeCi
                                            && x.SocieteId == societeId
                                            && x.EtablissementComptableId == etablissementComptableId);

            if (ci == null)
            {
                throw new FredBusinessException($"[TACHE] Import Tache {tacheModel.Code} : Aucun CI pour le code ({tacheModel.CodeCi}) et l'ID de société ({societeId})");
            }

            return ci;
        }

        private SocieteEnt GetSociete(TacheModel tacheModel, List<SocieteEnt> societes)
        {
            SocieteEnt societe = societes.FirstOrDefault(x => x.CodeSocieteComptable == tacheModel.CodeSociete && x.Active);

            if (societe == null)
            {
                throw new FredBusinessException($"[TACHE] Import Tache {tacheModel.Code} : Aucune société pour le CodeSocieteComptable ({tacheModel.CodeSociete})");
            }

            return societe;
        }

        private EtablissementComptableEnt GetEtablissementComptable(TacheModel tacheModel, List<EtablissementComptableEnt> etablissementComptables, SocieteEnt societe)
        {
            EtablissementComptableEnt etablissementComptable = etablissementComptables.FirstOrDefault(x => x.Code == tacheModel.CodeEtablissement && x.SocieteId == societe.SocieteId);
            if (etablissementComptable == null)
            {
                throw new FredBusinessException($"[MOYEN] Import Tache {tacheModel.Code} : Aucun etablissementComptable ({tacheModel.CodeEtablissement}) pour la société ({tacheModel.CodeSociete})");
            }

            return etablissementComptable;
        }
    }
}
