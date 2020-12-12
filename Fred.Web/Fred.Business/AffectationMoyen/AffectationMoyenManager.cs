using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Fred.Business.Affectation;
using Fred.Business.Moyen;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.AffectationMoyen
{
    public class AffectationMoyenManager : Manager<AffectationMoyenEnt, IAffectationMoyenRepository>, IAffectationMoyenManager
    {
        private readonly IAffectationMoyenTypeRepository affectationMoyenTypeRepository;
        private readonly IMapper mapper;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IAffectationManager affectationManager;
        private readonly IMaterielLocationManager materielLocationManger;
        private readonly IPointageManager pointageManager;
        private readonly IRepository<RapportLigneEnt> rapportLigneRepository;

        public AffectationMoyenManager(
            IUnitOfWork uow,
            IAffectationMoyenRepository affectationMoyenRepository,
            IMapper mapper,
            IUtilisateurManager utilisateurManager,
            IPersonnelManager personnelManager,
            IAffectationManager affectationManager,
            IMaterielLocationManager materielLocationManger,
            IPointageManager pointageManager,
            IRepository<RapportLigneEnt> rapportLigneRepository,
            IAffectationMoyenTypeRepository affectationMoyenTypeRepository)
            : base(uow, affectationMoyenRepository)
        {
            this.mapper = mapper;
            this.utilisateurManager = utilisateurManager;
            this.personnelManager = personnelManager;
            this.affectationManager = affectationManager;
            this.materielLocationManger = materielLocationManger;
            this.pointageManager = pointageManager;
            this.rapportLigneRepository = rapportLigneRepository;
            this.affectationMoyenTypeRepository = affectationMoyenTypeRepository;
        }

        /// <summary>
        /// Permet de récupérer la liste des affectation des moyens en fonction des critères de recherche.
        /// </summary>
        /// <param name="searchFilters">Filtres de recherche</param>
        /// <param name="page">Page actuelle</param>
        /// <param name="pageSize">Taille de page</param>
        /// <returns>Retourne la liste filtré des affectations des moyens</returns>
        public IEnumerable<AffectationMoyenEnt> SearchWithFilters(SearchAffectationMoyenEnt searchFilters, int page, int pageSize)
        {
            try
            {
                AffectationMoyenRolesFiltersEnt affectationMoyenRolesFilters = GetAffectationMoyenRolesFilter();
                return Repository.SearchWithFilters(searchFilters, affectationMoyenRolesFilters, page, pageSize).ToList();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public IEnumerable<AffectationMoyenEnt> GetAffectationMoyens()
        {
            try
            {
                return Repository.GetAffectationMoyens();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);

            }
        }

        /// <inheritdoc />
        public AffectationMoyenEnt AddAffectationMoyen(AffectationMoyenEnt affectationMoyen)
        {
            try
            {
                return Repository.AddAffectationMoyen(affectationMoyen);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de supprimer une affection apres la suppression de location 
        /// </summary>
        /// <param name="idMaterielLocation"> L'id de materiel en location a supprimer</param>
        public void DeleteAffectationMoyen(int idMaterielLocation)
        {
            try
            {
                Repository.DeleteAffectationMoyen(idMaterielLocation);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Get Affectation moyen famille by type model
        /// </summary>
        /// <returns>Affectation moyen famille by type model</returns>
        public IEnumerable<AffectationMoyenFamilleByTypeModel> GetAffectationMoyenFamilleByType()
        {
            try
            {
                var result = new List<AffectationMoyenFamilleByTypeModel>();
                var moyenTypeList = affectationMoyenTypeRepository.GetAffectationMoyenType();
                if (moyenTypeList.IsNullOrEmpty())
                {
                    return new List<AffectationMoyenFamilleByTypeModel>();
                }

                foreach (var m in moyenTypeList)
                {
                    var model = new AffectationMoyenTypeModel
                    {

                        AffectationMoyenTypeId = m.AffectationMoyenTypeId,
                        Libelle = m.Libelle,
                    };

                    var element = result.FirstOrDefault(v => v.AffectationMoyenFamilleCode == m.AffectationMoyenFamille?.Code);
                    if (element != null)
                    {
                        element.AffecationTypeList.Add(model);
                    }
                    else
                    {
                        result.Add(new AffectationMoyenFamilleByTypeModel
                        {
                            AffectationMoyenFamilleCode = m.AffectationMoyenFamille?.Code,
                            AffecationTypeList = new List<AffectationMoyenTypeModel> { model }
                        });
                    }
                }

                return result;

            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne la liste des affectations éligible au pointage matériel
        /// </summary>
        /// <param name="datesPredicate">Predicat à utiliser pour les dates des afféctations</param>
        /// <param name="typePredicate">Predicate pour les types d'affectation</param>
        /// <returns>La liste des affectations dans l'intervalle des dates défini par le Predicate </returns>
        public IEnumerable<AffectationMoyenEnt> GetPointageMoyenAffectations(
            Expression<Func<AffectationMoyenEnt, bool>> datesPredicate,
            Expression<Func<AffectationMoyenEnt, bool>> typePredicate)
        {
            try
            {
                return Repository.GetPointageMoyenAffectations(datesPredicate, typePredicate);

            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Récupére la liste des codes ci pour les restitutions et les maintenances
        /// </summary>
        /// <param name="codeList">La liste des codes</param>
        /// <returns>List des codes</returns>
        public IEnumerable<string> GetRestitutionAndMaintenanceCiCodes(IEnumerable<string> codeList)
        {
            return affectationMoyenTypeRepository.GetRestitutionAndMaintenanceCiCodes(codeList);
        }

        /// <summary>
        /// Récupérer le filtre des roles pour les affectations des moyens
        /// </summary>
        /// <returns>Le filtre des affectation des moyens en fonction des roles de l'utilisateur connecté</returns>
        private AffectationMoyenRolesFiltersEnt GetAffectationMoyenRolesFilter()
        {
            AffectationMoyenRolesFiltersEnt affectationMoyenRolesFilters = new AffectationMoyenRolesFiltersEnt();
            int userId = utilisateurManager.GetContextUtilisateurId();
            affectationMoyenRolesFilters.UtilisateurId = userId;

            if (utilisateurManager.IsSuperAdmin(userId))
            {
                affectationMoyenRolesFilters.IsSuperAdmin = true;
            }
            else if (utilisateurManager.HasPermissionToSeeAllAffectationMoyens())
            {
                affectationMoyenRolesFilters.HasPermissionToSeeAllAffectationMoyens = true;
            }
            else
            {
                if (utilisateurManager.HasPermissionToSeeManagerPersonnelAffectationMoyens())
                {
                    affectationMoyenRolesFilters.HasPermissionToSeeManagerPersonnelsAffectationMoyens = true;
                    affectationMoyenRolesFilters.ManagerPersonnelsList = personnelManager.GetManagedEmployeeIdList(userId);
                }

                if (utilisateurManager.HasPermissionToSeeResponsableCiAffectationMoyens())
                {
                    affectationMoyenRolesFilters.HasPermissionToSeeResponsableCiAffectationMoyens = true;
                    affectationMoyenRolesFilters.ResponsableCiList = utilisateurManager.GetCiListOfResponsable().Select(c => c.CiId).ToList();
                    affectationMoyenRolesFilters.ResponsableCiPersonnelList = affectationManager.GetPersonnelsAffectationListByCiList(affectationMoyenRolesFilters.ResponsableCiList).ToList();
                }

                if (utilisateurManager.HasPermissionToSeeDelegueCiAffectationMoyens())
                {
                    affectationMoyenRolesFilters.HasPermissionToSeeDelegueCiAffectationMoyens = true;
                    affectationMoyenRolesFilters.DelegueCiList = utilisateurManager.GetCiListForDelegue().Select(c => c.CiId).ToList();
                }
            }

            return affectationMoyenRolesFilters;
        }

        /// <summary>
        /// Validate affectation moyen
        /// Apres on recupere une list si il y a une restitution de type retour au loueur pour desactiver la location 
        /// </summary>
        /// <param name="model">Update affectation moyen model</param>
        public void ValidateAffectationMoyen(ValidateAffectationMoyenModel model)
        {
            if (model == null)
            {
                throw new FredBusinessException("Validate affectation moyen model cannot be null");
            }

            if (model.AffectationMoyenModelList.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<AffectationMoyenModel> listToAdd = model.AffectationMoyenModelList.Where(x => x.AffectationMoyenId == 0);
            if (!listToAdd.IsNullOrEmpty())
            {
                AddOrUpdateRangeAffectationList(mapper.Map<IEnumerable<AffectationMoyenEnt>>(listToAdd), isAdd: true);
            }

            IEnumerable<AffectationMoyenModel> listToUpdate = model.AffectationMoyenModelList.Where(x => x.AffectationMoyenId > 0);
            listToUpdate.Select(x => x?.MaterielLocation).ForEach((x) =>
            {
                if (x != null)
                {
                    x.AuteurCreation = null;
                    x.AuteurModification = null;
                }
            });
            if (!listToUpdate.IsNullOrEmpty())
            {
                AddOrUpdateRangeAffectationList(mapper.Map<IEnumerable<AffectationMoyenEnt>>(listToUpdate), isAdd: false);
                List<int?> listMaterielLocationId = listToUpdate.Where(x => x.MaterielLocationId != null && x.AffectationMoyenTypeId == AffectationMoyenTypeCode.RetourLoueur.ToIntValue()).Select(x => x.MaterielLocationId).ToList();
                if (listMaterielLocationId.IsNullOrEmpty())
                {
                    return;
                }

                foreach (int? materielLocationId in listMaterielLocationId)
                {
                    materielLocationManger.DeleteMaterielLocation(materielLocationId.Value);
                }
            }
        }

        /// <summary>
        /// Return affectation moyen by materielLocationId
        /// </summary>
        /// <param name="materielLocationId">Materiel location Id</param>
        /// <returns>Retourne une enumerable des affectations moyens associes a un materiel en location</returns>
        public IEnumerable<AffectationMoyenEnt> GetAllAffectationByMaterielLocationId(int materielLocationId)
        {
            try
            {
                return Repository.GetAllAffectationByMaterielLocationId(materielLocationId);
            }
            catch (FredException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Add or update range affectation list
        /// </summary>
        /// <param name="affectationList">Affectation list</param>
        /// <param name="isAdd">Is an add opration</param>
        public void AddOrUpdateRangeAffectationList(IEnumerable<AffectationMoyenEnt> affectationList, bool isAdd)
        {
            try
            {
                if (isAdd)
                {
                    this.Repository.AddAffectationMoyenList(affectationList);
                }
                else
                {
                    List<AffectationMoyenEnt> existantAffectationMoyenList = this.Repository.GetListAffectationMoyenByIds(affectationList.Select(x => x.AffectationMoyenId));
                    List<AffectationMoyenEnt> affectationMoyenListToDeletePointage = existantAffectationMoyenList.Where(x => affectationList
                                                                                    .Any(y => y.AffectationMoyenId == x.AffectationMoyenId)).ToList();
                    ManageUpdatedAffectationMoyenLis(affectationMoyenListToDeletePointage);
                    UpdateAffectationMoyenList(affectationList);
                }


                Save();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Supprimer les rapports lignes lors de modification de l'affectation moyen
        /// </summary>
        /// <param name="rapportLignes">List des rapports lignes</param>
        private void DeletePointageWhenAffectationMoyenUpdated(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            if (!rapportLignes.IsNullOrEmpty())
            {
                foreach (RapportLigneEnt ligne in rapportLignes)
                {
                    ligne.HeuresMachine = 0;
                    ligne.MaterielId = null;
                    ligne.AffectationMoyenId = null;
                    if (!ligne.PersonnelId.HasValue)
                    {
                        pointageManager.DeletePointage(ligne, false);
                    }
                    else
                    {
                        rapportLigneRepository.Update(ligne);
                    }

                }
            }
        }

        /// <summary>
        /// Manage Affectation moyen
        /// </summary>
        /// <param name="affectationMoyenList">Liste des affectations moyens</param>
        private void ManageUpdatedAffectationMoyenLis(IEnumerable<AffectationMoyenEnt> affectationMoyenList)
        {
            if (!affectationMoyenList.IsNullOrEmpty())
            {
                IEnumerable<RapportLigneEnt> rapportLignes = pointageManager.GetPointageByAffectaionMoyenIds(affectationMoyenList.Select(x => x.AffectationMoyenId));
                if (!rapportLignes.IsNullOrEmpty())
                {
                    foreach (AffectationMoyenEnt affectationMoyen in affectationMoyenList)
                    {
                        IEnumerable<RapportLigneEnt> rapportLigneToUpdate = rapportLignes.Where(x => x.AffectationMoyenId.HasValue && x.AffectationMoyenId.Value == affectationMoyen.AffectationMoyenId
                                                                            && x.DatePointage >= affectationMoyen.DateDebut.Date && (!affectationMoyen.DateFin.HasValue || x.DatePointage <= affectationMoyen.DateFin.Value.Date))
                                                                            .ToList();
                        DeletePointageWhenAffectationMoyenUpdated(rapportLigneToUpdate);
                    }
                }
            }
        }

        /// <summary>
        /// Supprimer les anciens rapports lignes lors de la réaffectation
        /// </summary>
        /// <param name="rapportLigneToUpdate">Rapport ligne à supprimer</param>
        public void DeleteRapportLigneWithPointage(IEnumerable<RapportLigneEnt> rapportLigneToUpdate)
        {
            DeletePointageWhenAffectationMoyenUpdated(rapportLigneToUpdate);
        }

        /// <summary>
        /// Update une liste des affectations moyens
        /// </summary>
        /// <param name="affectationMoyenList">Liste des affectations</param>
        private void UpdateAffectationMoyenList(IEnumerable<AffectationMoyenEnt> affectationMoyenList)
        {
            foreach (AffectationMoyenEnt affectationMoyen in affectationMoyenList)
            {
                this.Repository.Update(affectationMoyen);
            }
        }
    }
}
