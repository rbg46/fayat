using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.CI;
using Fred.Business.EcritureComptable;
using Fred.Business.FeatureFlipping;
using Fred.Business.Journal;
using Fred.Business.Referential.Nature;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EcritureComptable;
using Fred.Entities.Journal;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Models;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Journal;
using Fred.Web.Shared.Models.Nature;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Business.OperationDiverse
{
    /// <summary>
    /// Manager des familles d'OD
    /// </summary>
    public class FamilleOperationDiverseManager : Manager<FamilleOperationDiverseEnt, IFamilleOperationDiverseRepository>, IFamilleOperationDiverseManager
    {
        private readonly ICIManager ciManager;
        private readonly IJournalManager journalManager;
        private readonly INatureManager natureManager;
        private readonly IEcritureComptableManager ecritureComptableManager;
        private readonly IMapper mapper;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IFeatureFlippingManager featureFlippingManager;

        public FamilleOperationDiverseManager(
            IUnitOfWork uow,
            IFamilleOperationDiverseRepository familleOperationDiverseRepository,
            ICIManager ciManager,
            IJournalManager journalManager,
            INatureManager natureManager,
            IEcritureComptableManager ecritureComptableManager,
            IMapper mapper,
            IUtilisateurManager utilisateurManager,
            IFeatureFlippingManager featureFlippingManager)
          : base(uow, familleOperationDiverseRepository)
        {
            this.ciManager = ciManager;
            this.journalManager = journalManager;
            this.natureManager = natureManager;
            this.ecritureComptableManager = ecritureComptableManager;
            this.mapper = mapper;
            this.utilisateurManager = utilisateurManager;
            this.featureFlippingManager = featureFlippingManager;
        }

        /// <summary>
        /// Récupère la liste des familles d'OD pour une liste de sociétés
        /// </summary>
        /// <param name="societeIds">Identifiants des sociétés à laquelle les familles sont rattachées</param>
        /// <returns>Liste des familles d'OD de la société</returns>
        /// <remarks>A des fins d'optimisation, la liste est stockée en cache</remarks>
        public IEnumerable<FamilleOperationDiverseEnt> GetFamiliesBySociety(List<int> societeIds)
        {
            return Repository.GetFamilyBySociety(societeIds).ToList();
        }

        /// <summary>
        /// Récupère la liste des familles d'OD pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des familles d'OD de la société</returns>
        /// <remarks>A des fins d'optimisation, la liste est stockée en cache</remarks>
        public IEnumerable<TypeOdFilterExplorateurDepense> GetTypeOdFilter(int societeId)
        {
            List<FamilleOperationDiverseEnt> familiesBySociety = Repository.GetFamilyBySociety(societeId).ToList();
            List<TypeOdFilterExplorateurDepense> listTypeOdFilter = new List<TypeOdFilterExplorateurDepense>();

            foreach (FamilleOperationDiverseEnt family in familiesBySociety)
            {
                TypeOdFilterExplorateurDepense typeOdFilter = new TypeOdFilterExplorateurDepense
                {
                    Code = family.Code,
                    LibelleCourt = family.LibelleCourt
                };

                listTypeOdFilter.Add(typeOdFilter);
            }

            return listTypeOdFilter;
        }

        /// <summary>
        /// Récupère la liste des familles d'OD pour une société
        /// </summary>
        /// <param name="societeIds">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des familles d'OD de la société</returns>
        /// <remarks>A des fins d'optimisation, la liste est stockée en cache</remarks>
        public IEnumerable<FamilleOperationDiverseEnt> GetFamiliesBySociety(int societeId)
        {
            return Repository.GetFamilyBySociety(societeId).ToList();
        }

        public IEnumerable<FamilleOperationDiverseModel> GetFamiliesOdOrdered(int societeId)
        {
            List<FamilleOperationDiverseEnt> familleOperationDiverseEnt = Repository.GetFamilyBySociety(societeId).ToList();
            IEnumerable<FamilleOperationDiverseModel> familleOperationDiverseModelList = mapper.Map<IEnumerable<FamilleOperationDiverseModel>>(familleOperationDiverseEnt).ToList();

            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000))
            {
                familleOperationDiverseModelList.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.LibelleCourt))
                    {
                        f.LibelleCourt = f.Libelle.Substring(0, 20);
                    }
                    f.LibelleCourt = f.LibelleCourt.ToLower();
                });
            }

            familleOperationDiverseModelList = familleOperationDiverseModelList.OrderBy(o => o.Order).ThenBy(x => x.DateCreation);
            return familleOperationDiverseModelList;
        }

        /// <summary>
        /// Récupère la liste des familles d'OD pour la société d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI dont la société est rattachée aux familles d'OD</param>
        /// <returns>Liste des familles d'OD de la société du CI</returns>
        /// <remarks>A des fins d'optimisation, la liste est stockée en cache</remarks>
        public IEnumerable<FamilleOperationDiverseEnt> GetFamiliesByCI(int ciId)
        {
            return GetFamiliesBySociety(ciManager.GetSocieteByCIId(ciId).SocieteId);
        }

        /// <summary>
        /// Lance le contrôle paramétrage pour les journaux
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Un modèle contenant le résultat du contrôle paramétrage</returns>
        public IReadOnlyList<ControleParametrageFamilleOperationDiverseModel> LaunchControleParametrageForJournal(int societeId)
        {
            List<ControleParametrageFamilleOperationDiverseModel> controleParametrageFamilleOdModel = new List<ControleParametrageFamilleOperationDiverseModel>();

            controleParametrageFamilleOdModel.AddRange(GetJournauxWithoutFamille(societeId));
            controleParametrageFamilleOdModel.AddRange(GetFamiliesWithoutJournal(societeId));

            return controleParametrageFamilleOdModel;
        }

        /// <summary>
        /// Récupère la liste des familles qui ne possèdent pas de journal
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des familles qui ne possèdent pas de journal</returns>
        private List<ControleParametrageFamilleOperationDiverseModel> GetFamiliesWithoutJournal(int societeId)
        {
            List<ControleParametrageFamilleOperationDiverseModel> controleParametrageFamilleOdModel = new List<ControleParametrageFamilleOperationDiverseModel>();

            /*** Identifier les familles sans journal ***/
            IEnumerable<FamilleOperationDiverseEnt> familyList = Repository.GetFamilyBySociety(societeId);
            List<JournalFamilleODModel> journalList = journalManager.GetJournauxActifs(societeId);
            List<FamilleOperationDiverseEnt> familyODWithoutOrder = familyList.Where(q => !journalList.Select(x => x.ParentFamilyODWithoutOrder).Contains(q.FamilleOperationDiverseId)).ToList();
            List<FamilleOperationDiverseEnt> familyODWithOrder = familyList.Where(q => !journalList.Select(x => x.ParentFamilyODWithOrder).Contains(q.FamilleOperationDiverseId)).ToList();
            IEnumerable<FamilleOperationDiverseEnt> familiesWithoutJournal = familyODWithoutOrder.Where(q => familyODWithOrder.Select(x => x.Code).Contains(q.Code));

            familiesWithoutJournal.ForEach(x => controleParametrageFamilleOdModel.Add(new ControleParametrageFamilleOperationDiverseModel()
            {
                TypeFamilleOperationDiverse = "Famille OD",
                Code = x.Code,
                Libelle = x.Libelle,
                Erreur = "Aucun journal associé"
            }));

            return controleParametrageFamilleOdModel;
        }

        /// <summary>
        /// Récupère la liste des journaux qui ne possèdent pas de famille
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des journaux qui ne possèdent pas de famille</returns>
        private List<ControleParametrageFamilleOperationDiverseModel> GetJournauxWithoutFamille(int societeId)
        {
            List<ControleParametrageFamilleOperationDiverseModel> controleParametrageFamilleOdModel = new List<ControleParametrageFamilleOperationDiverseModel>();
            /*** Identifier les journaux sans famille ***/
            IEnumerable<JournalEnt> journauxWithoutFamille = journalManager.GetListJournauxWithoutFamille(societeId).Distinct();

            journauxWithoutFamille.ForEach(x => controleParametrageFamilleOdModel.Add(new ControleParametrageFamilleOperationDiverseModel()
            {
                TypeFamilleOperationDiverse = "Journal",
                Code = x.Code,
                Libelle = x.Libelle,
                Erreur = "Aucune famille associée"
            }));

            return controleParametrageFamilleOdModel;
        }

        /// <summary>
        /// Sauvegarde les modifications d'une famille OD
        /// </summary>
        /// <param name="familleOperationDiverseModel">Famille OD à modifier</param>
        public async Task UpdateFamilleOperationDiverseAsync(FamilleOperationDiverseModel familleOperationDiverseModel)
        {
            if (familleOperationDiverseModel != null)
            {
                if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000))
                {
                    ThrowsIfShortLabelHasWrongFormat(familleOperationDiverseModel.LibelleCourt);
                }

                FamilleOperationDiverseModel oldFamilleOperationDiverse = GetFamille(familleOperationDiverseModel.FamilleOperationDiverseId);

                UpdateFamilleOperationDiverseDatas(familleOperationDiverseModel);

                if (familleOperationDiverseModel.IsAccrued)
                {
                    IReadOnlyList<EcritureComptableEnt> allEcritureComptables = await ecritureComptableManager.GetByFamilleOdIdsAsync(new List<int> { familleOperationDiverseModel.FamilleOperationDiverseId }).ConfigureAwait(false);

                    IReadOnlyList<EcritureComptableEnt> ecritureComptablesWithModifiedLibelle = ReplaceLibelleEcritureComptable(allEcritureComptables, oldFamilleOperationDiverse.Libelle, familleOperationDiverseModel.Libelle);

                    ecritureComptableManager.UpdateLibelleEcritureComptable(ecritureComptablesWithModifiedLibelle);
                }
            }
        }

        private void ThrowsIfShortLabelHasWrongFormat(string shortLabel)
        {
            if (string.IsNullOrEmpty(shortLabel))
            {
                throw new ArgumentException(FeatureFamilleOperationDiverse.FamilleOperationDiverse_LibelleCourtVide);
            }

            if (shortLabel.Length > 20)
            {
                throw new ArgumentException(FeatureFamilleOperationDiverse.FamilleOperationDiverse_LibelleCourtTropLong);
            }
        }

        private IReadOnlyList<EcritureComptableEnt> ReplaceLibelleEcritureComptable(IReadOnlyList<EcritureComptableEnt> ecritureComptablesList, string oldLibelleFamille, string newLibelleFamille)
        {
            ecritureComptablesList.ForEach(ecriture => { ecriture.Libelle = ecriture.Libelle.Replace(oldLibelleFamille, newLibelleFamille); });

            return ecritureComptablesList;
        }

        private void UpdateFamilleOperationDiverseDatas(FamilleOperationDiverseModel familleOperationDiverseModel)
        {
            FamilleOperationDiverseEnt familleOperationDiverseEnt = Repository.FindById(familleOperationDiverseModel.FamilleOperationDiverseId);

            if (familleOperationDiverseEnt != null)
            {
                familleOperationDiverseEnt.Libelle = familleOperationDiverseModel.Libelle;
                familleOperationDiverseEnt.LibelleCourt = familleOperationDiverseModel.LibelleCourt;
                familleOperationDiverseEnt.DateModification = DateTime.UtcNow;
                familleOperationDiverseEnt.MustHaveOrder = familleOperationDiverseModel.MustHaveOrder;
                familleOperationDiverseEnt.Order = familleOperationDiverseModel.Order;
                familleOperationDiverseEnt.AuteurModificationId = utilisateurManager.GetContextUtilisateurId();

                List<Expression<Func<FamilleOperationDiverseEnt, object>>> fieldsToUpdate = new List<Expression<Func<FamilleOperationDiverseEnt, object>>>
                {
                    x =>x.Libelle,
                    x =>x.LibelleCourt,
                    x =>x.DateModification,
                    x =>x.MustHaveOrder,
                    x =>x.Order,
                    x =>x.AuteurModificationId
                };

                Repository.Update(familleOperationDiverseEnt, fieldsToUpdate);

                Save();
            }
        }

        /// <summary>
        /// Récupére un famille d'opération diverse pour un id
        /// </summary>
        /// <param name="familleOperationDiverseId">identifiant de la famille</param>
        /// <returns><see cref="FamilleOperationDiverseModel" /></returns>
        public FamilleOperationDiverseModel GetFamille(int familleOperationDiverseId)
        {
            FamilleOperationDiverseModel model = new FamilleOperationDiverseModel();
            FamilleOperationDiverseEnt ent = Repository.FindById(familleOperationDiverseId);
            model.Code = ent.Code;
            model.FamilleOperationDiverseId = ent.FamilleOperationDiverseId;
            model.Libelle = ent.Libelle;
            model.LibelleCourt = ent.LibelleCourt;
            model.SocieteCode = ent.Societe?.Code;
            model.IsAccrued = ent.IsAccrued;
            return model;
        }

        public int GetFamilyTaskId(int familleOperationDiverseId)
        {
            return Repository.GetFamilyTaskId(familleOperationDiverseId);
        }

        /// <summary>
        /// Retourne la liste des familles d'opérations diverse pour une liste de code de famille
        /// </summary>
        /// <param name="familleIds">Identifiants des mfamilles</param>
        /// <returns>Liste de <see cref="FamilleOperationDiverseModel"/></returns>
        public IReadOnlyList<FamilleOperationDiverseModel> GetFamilles(List<int> familleIds)
        {
            List<FamilleOperationDiverseModel> models = new List<FamilleOperationDiverseModel>();

            foreach (FamilleOperationDiverseEnt famille in Repository.GetByIds(familleIds))
            {
                models.Add(new FamilleOperationDiverseModel
                {
                    Code = famille.Code,
                    FamilleOperationDiverseId = famille.FamilleOperationDiverseId,
                    Libelle = famille.Libelle,
                    LibelleCourt = famille.LibelleCourt,
                    SocieteCode = famille.Societe?.Code,
                    IsAccrued = famille.IsAccrued
                });
            }
            return models;
        }

        /// <summary>
        /// Lance le contrôle paramétrage pour les natures
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Un modèle contenant le résultat du contrôle paramétrage</returns>
        public IReadOnlyList<ControleParametrageFamilleOperationDiverseModel> LaunchControleParametrageForNature(int societeId)
        {
            List<ControleParametrageFamilleOperationDiverseModel> controleParametrageFamilleOdModel = new List<ControleParametrageFamilleOperationDiverseModel>();

            controleParametrageFamilleOdModel.AddRange(GetNaturesWithoutFamille(societeId));
            controleParametrageFamilleOdModel.AddRange(GetFamiliesWithoutNature(societeId));

            return controleParametrageFamilleOdModel;
        }

        private List<ControleParametrageFamilleOperationDiverseModel> GetFamiliesWithoutNature(int societeId)
        {
            /*** Identifier les familles sans nature ***/
            List<ControleParametrageFamilleOperationDiverseModel> controleParametrageFamilleOdModel = new List<ControleParametrageFamilleOperationDiverseModel>();
            IEnumerable<FamilleOperationDiverseEnt> familyList = Repository.GetFamilyBySociety(societeId);
            IEnumerable<NatureFamilleOdModel> natureList = natureManager.GetNatureActiveFamilleOds(societeId);
            List<FamilleOperationDiverseEnt> familyODWithoutOrder = familyList.Where(q => !natureList.Select(x => x.ParentFamilyODWithoutOrder).Contains(q.FamilleOperationDiverseId)).ToList();
            List<FamilleOperationDiverseEnt> familyODWithOrder = familyList.Where(q => !natureList.Select(x => x.ParentFamilyODWithOrder).Contains(q.FamilleOperationDiverseId)).ToList();
            IEnumerable<FamilleOperationDiverseEnt> familiesWithoutNature = familyODWithoutOrder.Where(q => familyODWithOrder.Select(x => x.Code).Contains(q.Code));

            familiesWithoutNature.ForEach(x => controleParametrageFamilleOdModel.Add(new ControleParametrageFamilleOperationDiverseModel()
            {
                TypeFamilleOperationDiverse = "Famille OD",
                Code = x.Code,
                Libelle = x.Libelle,
                Erreur = "Aucune nature associée"
            }));

            return controleParametrageFamilleOdModel;
        }

        private List<ControleParametrageFamilleOperationDiverseModel> GetNaturesWithoutFamille(int societeId)
        {
            /*** Identifier les natures sans famille ***/
            List<ControleParametrageFamilleOperationDiverseModel> controleParametrageFamilleOdModel = new List<ControleParametrageFamilleOperationDiverseModel>();
            IEnumerable<NatureEnt> naturesWithoutFamille = natureManager.GetListNaturesWithoutFamille(societeId).Distinct();

            naturesWithoutFamille.ForEach(x => controleParametrageFamilleOdModel.Add(new ControleParametrageFamilleOperationDiverseModel()
            {
                TypeFamilleOperationDiverse = "Nature",
                Code = x.Code,
                Libelle = x.Libelle,
                Erreur = "Aucune famille associée"
            }));

            return controleParametrageFamilleOdModel;
        }

        public async Task<IReadOnlyList<FamilleOperationDiverseNatureJournalModel>> SetParametrageNaturesJournaux(FamilleOperationDiverseModel fod)
        {
            if ((fod?.AssociatedNatures?.Any() ?? false) && (fod?.AssociatedJournaux?.Any() ?? false))
            {
                var duplicateParametrages = GetDuplicateParametrageFamilleOperationDiverse(fod);
                if (!duplicateParametrages?.Any() ?? true)
                {
                    natureManager.UpdateNatures(fod?.AssociatedNatures);
                    journalManager.UpdateJournaux(fod?.AssociatedJournaux);
                    return null;
                }
                return duplicateParametrages;
            }
            return null;
        }

        public IReadOnlyList<FamilleOperationDiverseNatureJournalModel> GetAllParametrageFamilleOperationDiverseNaturesJournaux(int societeId)
        {
            List<FamilleOperationDiverseNatureJournalModel> familleOperationDiverseNatureJournalModelList = new List<FamilleOperationDiverseNatureJournalModel>();
            var listParametrage = Repository.GetAllParametrageFamilleOperationDiverseNaturesJournaux(societeId);

            listParametrage.ForEach(parametrage => familleOperationDiverseNatureJournalModelList.Add(new FamilleOperationDiverseNatureJournalModel
            {
                FamilleOperationDiverse = parametrage.FamilleOperationDiverse,
                Nature = parametrage.Nature,
                Journal = parametrage.Journal
            }));

            return familleOperationDiverseNatureJournalModelList;
        }

        /// <summary>
        /// Lance le contrôle paramétrage pour les natures
        /// </summary>
        /// <param name="fod">famille operation diverse</param>
        /// <returns>Une liste de doublons si existe</returns>
        public IReadOnlyList<FamilleOperationDiverseNatureJournalModel> GetDuplicateParametrageFamilleOperationDiverse(FamilleOperationDiverseModel fod)
        {
            List<FamilleOperationDiverseNatureJournalModel> listDuplicate = new List<FamilleOperationDiverseNatureJournalModel>();
            IReadOnlyList<FamilleOperationDiverseNatureJournalModel> existingParametrages = GetAllParametrageFamilleOperationDiverseNaturesJournaux(fod.SocieteId);
            existingParametrages = existingParametrages.Where(p => p.FamilleOperationDiverse.FamilleOperationDiverseId != fod.FamilleOperationDiverseId && p.FamilleOperationDiverse.MustHaveOrder == fod.MustHaveOrder).ToList();

            fod?.AssociatedNatures?.ForEach(nature => {
                fod.AssociatedJournaux?.ForEach(journal =>
                {
                    var parametrage = existingParametrages.FirstOrDefault(el => el.Journal.Code == journal.Code && el.Nature.Code == nature.Code);
                    if (parametrage != null)
                    {
                        listDuplicate.Add(new FamilleOperationDiverseNatureJournalModel
                        {
                            FamilleOperationDiverse = parametrage.FamilleOperationDiverse,
                            Nature = parametrage.Nature,
                            Journal = parametrage.Journal
                        });
                    }
                });
            });

            return listDuplicate;
        }
    }
}
