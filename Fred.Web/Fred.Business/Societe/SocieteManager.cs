using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.Groupe;
using Fred.Business.Organisation;
using Fred.Business.Organisation.Tree;
using Fred.Business.Params;
using Fred.Business.RoleFonctionnalite;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Groupe;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.RoleFonctionnalite;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Security;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.Societe
{
    public abstract class SocieteManager : Manager<SocieteEnt, ISocieteRepository>, ISocieteManager
    {
        private readonly ISocieteDeviseRepository societeDeviseRepo;
        private readonly IUniteSocieteRepository uniteSocieteRepo;
        private readonly ISecurityManager securityManager;
        private readonly IOrganisationManager orgaManager;
        private readonly IUtilisateurManager userManager;
        private readonly IParamsManager paramsManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly ITypeSocieteManager typeSocieteManager;
        private readonly IGroupeManager groupeManager;
        private readonly ISocieteRepository societeRepository;
        private readonly IRoleFonctionnaliteManager roleFonctionnaliteManager;
        private readonly ICIRepository ciRepository;
        private readonly IOrganisationRepository organisationRepository;

        public SocieteManager(
            IUnitOfWork uow,
            ISocieteRepository societeRepository,
            ISocieteValidator validator,
            ISecurityManager securityManager,
            IUtilisateurManager userManager,
            IOrganisationManager orgaManager,
            IParamsManager paramsManager,
            IOrganisationTreeService organisationTreeService,
            ITypeSocieteManager typeSocieteManager,
            IGroupeManager groupeManager,
            IRoleFonctionnaliteManager roleFonctionnaliteManager,
            ISocieteDeviseRepository societeDeviseRepo,
            IUniteSocieteRepository uniteSocieteRepo,
            ICIRepository ciRepository,
            IOrganisationRepository organisationRepository)
          : base(uow, societeRepository, validator)
        {
            this.securityManager = securityManager;
            this.userManager = userManager;
            this.orgaManager = orgaManager;
            this.paramsManager = paramsManager;
            this.organisationTreeService = organisationTreeService;
            this.typeSocieteManager = typeSocieteManager;
            this.groupeManager = groupeManager;
            this.societeRepository = societeRepository;
            this.roleFonctionnaliteManager = roleFonctionnaliteManager;
            this.societeDeviseRepo = societeDeviseRepo;
            this.uniteSocieteRepo = uniteSocieteRepo;
            this.ciRepository = ciRepository;
            this.organisationRepository = organisationRepository;
        }

        /// <inheritdoc />
        public override void CheckAccessToEntity(SocieteEnt entity)
        {
            int userId = securityManager.GetUtilisateurId();

            Repository.CheckAccessToEntity(entity, userId);
        }

        /// <inheritdoc />
        public SocieteEnt GetNewSociete()
        {
            return new SocieteEnt { Active = true, Externe = true, IsInterimaire = false };
        }

        /// <inheritdoc />
        public IEnumerable<SocieteEnt> GetSocieteListAll()
        {
            return Repository.GetSocieteListAll();
        }

        /// <inheritdoc />
        public IEnumerable<SocieteEnt> GetSocieteListAllSync()
        {
            return Repository.GetSocieteListAll();
        }

        /// <inheritdoc />
        public IEnumerable<SocieteEnt> GetSocieteList()
        {
            return Repository.GetSocieteList();
        }

        /// <inheritdoc />
        public SocieteEnt GetSocieteById(int societeId, bool includeGroupe = false)
        {
            return Repository.GetSocieteById(societeId, includeGroupe);
        }

        /// <inheritdoc />
        public SocieteEnt GetSocieteById(int societeId, List<Expression<Func<SocieteEnt, object>>> includes, bool asNoTracking = false)
        {
            var societes = Repository.Search(new List<Expression<Func<SocieteEnt, bool>>> { x => x.SocieteId == societeId }, null, includes, null, null, asNoTracking);

            return societes.FirstOrDefault();
        }

        /// <inheritdoc />
        public SocieteEnt GetSocieteByIdWithParameters(int societeId)
        {
            SocieteEnt societe = GetSocieteById(societeId, new List<Expression<Func<SocieteEnt, object>>>
            {
                x => x.Fournisseur,
                x => x.TypeSociete,
                x => x.Classification,
                x => x.Organisation.ParamValues.Select(y => y.ParamKey)
            },
            true);

            if (societe.Organisation != null)
            {
                InitSocieteFromParamValues(societe);
                societe.Organisation.ParamValues = null;
            }

            return societe;
        }

        /// <inheritdoc />
        public SocieteEnt GetSocieteWithOrganisation(int societeId)
        {
            return Repository.GetSocieteWithOrganisation(societeId);
        }

        /// <inheritdoc />
        public SocieteEnt GetDefaultSocieteInterim()
        {
            UtilisateurEnt currentUser = userManager.GetContextUtilisateur();

            return Repository.GetSocieteList().FirstOrDefault(s => s.IsInterimaire && s.GroupeId == currentUser.Personnel.Societe.GroupeId);
        }

        /// <inheritdoc />
        public SocieteEnt GetSocieteInterim(int groupeId)
        {
            return Repository.GetSocieteList().FirstOrDefault(s => s.IsInterimaire && s.GroupeId == groupeId);
        }

        /// <inheritdoc />
        public SocieteExistResult GetSocieteExistsWithSameCodeOrLibelle(int idCourant, string codeSociete, string libelle)
        {
            var result = new SocieteExistResult();
            var lowerCode = codeSociete.ToLower();
            var lowerlibelle = libelle.ToLower();
            var societe = Repository.GetSocieteWithSameCodeOrLibelle(idCourant, codeSociete, libelle);
            if (societe != null)
            {
                result.CodeIdentique = societe.Code.ToLower() == lowerCode;
                result.LibelleIdentique = societe.Libelle.ToLower() == lowerlibelle;
            }

            return result;
        }

        /// <inheritdoc />
        public int? GetSocieteIdByCodeSocieteComptable(string code)
        {
            return Repository.GetSocieteIdByCodeSocieteComptable(code);
        }

        /// <inheritdoc />
        public SocieteEnt GetSocieteByCodeSocieteComptable(string code)
        {
            return Repository.GetSocieteByCodeSocieteComptable(code);
        }

        /// <inheritdoc />
        public SocieteEnt GetSocieteByCodeSocietePaye(string code)
        {
            return Repository.GetSocieteByCodeSocietePaye(code);
        }

        /// <summary>
        /// Retourne la liste des indentifiant des société pour un identifiant de groupe
        /// </summary>
        /// <param name="groupeId">identifiant de groupe</param>
        /// <returns> la liste des indentifiant des société</returns>
        public IReadOnlyList<int> GetSocieteIdsByGroupeId(int groupeId)
        {
            return Repository.GetSocieteIdsByGroupeId(groupeId).ToList();
        }

        /// <inheritdoc />
        public async Task<SocieteEnt> AddSocieteAsync(SocieteEnt societeEnt)
        {
            UtilisateurEnt currentUser = await userManager.GetContextUtilisateurAsync().ConfigureAwait(false);
            var organisationTree = organisationTreeService.GetOrganisationTree();

            var group = organisationTree.GetGroupe(currentUser.Personnel.Societe.GroupeId);
            if (group != null)
            {
                OrganisationEnt societeOrga = orgaManager.GenerateOrganisation(Constantes.OrganisationType.CodeSociete, group.OrganisationId);
                if (societeOrga != null)
                {
                    societeEnt.Organisation = societeOrga;
                }
            }
            else
            {
                throw new FredBusinessException("Impossible d'ajouter cette société.");
            }

            BusinessValidation(societeEnt);
            societeEnt.SocieteDevises.ForEach(societeDevise =>
            {
                societeDevise.Devise = null;
            });

            Repository.AddSociete(societeEnt);
            await SaveAsync();

            if (societeEnt.Organisation != null)
            {
                SaveParamValues(societeEnt);
            }

            await AddCiAbsenceIfNotExistAsync(societeEnt, currentUser).ConfigureAwait(false);
            await SaveAsync();

            return societeEnt;
        }

        /// <inheritdoc />
        public async Task<SocieteEnt> UpdateSocieteAsync(SocieteEnt societeEnt)
        {
            if (societeEnt.Organisation != null)
            {
                SaveParamValues(societeEnt);
                societeEnt.Organisation.ParamValues = null;

                var organisationTree = organisationTreeService.GetOrganisationTree();

                var groupe = organisationTree.GetGroupe(societeEnt.GroupeId);

                orgaManager.UpdateOrganisation(societeEnt.Organisation, groupe.OrganisationId);
            }

            BusinessValidation(societeEnt);
            societeEnt.SocieteDevises?.ForEach(societeDevise =>
            {
                societeDevise.Devise = null;
            });

            Repository.UpdateSociete(societeEnt);

            await AddCiAbsenceIfNotExistAsync(societeEnt).ConfigureAwait(false);
            await SaveAsync();

            return societeEnt;
        }

        private async Task AddCiAbsenceIfNotExistAsync(SocieteEnt societe, UtilisateurEnt currentUser = null)
        {
            if (currentUser == null)
            {
                currentUser = await userManager.GetContextUtilisateurAsync().ConfigureAwait(false);
            }

            // RG_8769_07A : Add CI Absence pour les sociétés FES nouvellements créées
            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFES)
            {
                if (ciRepository.GetCIByCodeAndSocieteId("ABSENCES", societe.SocieteId) == null)
                {
                    // Création de l'organisation liée au nouveau CI
                    var newOrganisation = new OrganisationEnt
                    {
                        TypeOrganisationId = OrganisationType.Ci.ToIntValue(),
                        PereId = societe.OrganisationId
                    };

                    // Obligé de passer directement par le repository que ICiManager pour éviter le stackoverflow
                    // ICiManager injecte ISocieteManager ce qui engendre une injection cyclique si ICiManager est injecté ici.
                    ciRepository.AddCI(new Entities.CI.CIEnt
                    {
                        Code = "ABSENCES",
                        Libelle = "Gestion des absences",
                        Description = "Gestion des absences",
                        CodeExterne = "CIABS",
                        SocieteId = societe.SocieteId,
                        Organisation = newOrganisation,
                        EtablissementComptableId = null,
                        CITypeId = ciRepository.GetCITypes().First(t => t.Code.Equals("S", StringComparison.OrdinalIgnoreCase)).CITypeId,
                        DateOuverture = new DateTime(1990, 1, 1),
                        IsAbsence = true
                    });
                }
            }
        }

        public void DeleteSocieteById(int societeId)
        {
            if (!Repository.IsDeletable(societeId))
            {
                throw new FredBusinessException(BusinessResources.SuppressionImpossibleGenerique);
            }

            Repository.DeleteSocieteById(societeId);
        }

        /// <inheritdoc />
        public int? GetSocieteIdByCodeSocietePaye(string codeSociete)
        {
            return Repository.GetSocieteIdByCodeSocietePaye(codeSociete);
        }

        /// <inheritdoc />
        public IEnumerable<SocieteEnt> GetListSocieteToImportFacture()
        {
            return Repository.GetListSocieteToImportFacture();
        }

        /// <inheritdoc />
        public IEnumerable<SocieteEnt> SearchSocieteAllWithFilters(SearchSocieteEnt filters)
        {
            UtilisateurEnt currentUser = userManager.GetContextUtilisateur();

            return Repository.SearchSocieteAllWithFilters(filters.GetPredicateWhere()).Where(x => x.GroupeId == currentUser.Personnel.Societe.GroupeId);
        }

        /// <inheritdoc />
        public SearchSocieteEnt GetDefaultFilter()
        {
            return new SearchSocieteEnt { Code = true, Libelle = true };
        }

        /// <inheritdoc />
        public IEnumerable<SocieteEnt> SearchLight(string text, int page, int pageSize, List<string> typeSocieteCodes, bool? isExterne = null)
        {
            UtilisateurEnt currentUser = userManager.GetContextUtilisateur();

            return Repository.Query()
                             .Filter(ci => string.IsNullOrEmpty(text) || ci.Code.Contains(text.Trim()) || ci.Libelle.Contains(text.Trim()))
                             .Filter(ci => ci.Active)
                             .Filter(s => s.GroupeId == currentUser.Personnel.Societe.GroupeId)
                             .Filter(s => !s.IsInterimaire)
                             .Filter(x => typeSocieteCodes.Count == 0 || (x.TypeSocieteId.HasValue && typeSocieteCodes.Any(y => y == x.TypeSociete.Code)))
                             .Filter(x => !isExterne.HasValue || (isExterne.Value && x.Externe) || (!isExterne.Value && !x.Externe))
                             .OrderBy(o => o.OrderBy(ci => ci.Code))
                             .Include(x => x.TypeSociete)
                             .GetPage(page, pageSize);
        }

        public IEnumerable<SocieteEnt> SearchLightForRoles(string text, int page, int pageSize, List<string> typeSocieteCodes)
        {
            UtilisateurEnt currentUser = userManager.GetContextUtilisateur();
            bool superAdmin = currentUser.SuperAdmin;
            int groupeId = currentUser.Personnel.Societe.GroupeId;

            return Repository.Query()
                             .Include(g => g.Groupe)
                             .Filter(ci => string.IsNullOrEmpty(text) || ci.Code.Contains(text.Trim()) || ci.Libelle.Contains(text.Trim()))
                             .Filter(ci => ci.Active)
                             .Filter(x => superAdmin || x.GroupeId == groupeId)
                             .Filter(s => !s.IsInterimaire)
                             .Filter(x => !x.TypeSocieteId.HasValue || typeSocieteCodes.Count == 0 || typeSocieteCodes.Any(y => y == x.TypeSociete.Code))
                             .OrderBy(o => o.OrderBy(ci => ci.Code))
                             .GetPage(page, pageSize);
        }

        /// <inheritdoc />
        public SocieteEnt GetSocieteParentByOrgaId(int organisationId)
        {
            return Repository.GetSocieteParentByOrgaId(organisationId);
        }

        /// <inheritdoc />
        public IEnumerable<DeviseEnt> GetListDeviseBySocieteId(int societeId)
        {
            return Repository.GetListDeviseBySocieteId(societeId);
        }

        /// <inheritdoc />
        public void DeleteDeviseBySocieteDevise(SocieteDeviseEnt societeDevise)
        {
            try
            {
                societeDeviseRepo.Delete(societeDevise);
                Save();
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public void DeleteDeviseBySocieteId(int idSociete)
        {
            societeDeviseRepo.DeleteByIdSociete(idSociete);
            var query = Repository.Query().Filter(x => x.SocieteId == idSociete).Get();
            foreach (var societeDevise in query)
            {
                Repository.Delete(societeDevise);
            }

            Save();
        }

        /// <inheritdoc />
        public DeviseEnt GetListSocieteDeviseRef(SocieteEnt societe)
        {
            var lstDevise = societeDeviseRepo.GetListDeviseRefBySociete(societe.SocieteId).ToList();

            return lstDevise.FirstOrDefault();
        }

        /// <inheritdoc />
        public IEnumerable<SocieteDeviseEnt> GetListSocieteDevise(SocieteEnt societe)
        {
            return societeDeviseRepo.Query()
                       .Include(x => x.Devise)
                       .Filter(x => x.SocieteId.Equals(societe.SocieteId))
                       .Get();
        }

        /// <inheritdoc />
        public IEnumerable<DeviseEnt> GetListSocieteDeviseSec(int societeId)
        {
            return societeDeviseRepo.GetListDeviseSecBySociete(societeId).ToList();
        }

        /// <inheritdoc />
        public int? GetOrganisationIdBySocieteId(int? id)
        {
            return Repository.GetOrganisationIdBySocieteId(id.Value);
        }

        /// <inheritdoc />
        public IEnumerable<SocieteDeviseEnt> GetSocieteDeviseList(int societeId)
        {
            return societeDeviseRepo.GetSocieteDeviseList(societeId);
        }

        /// <inheritdoc />
        public IEnumerable<SocieteDeviseEnt> ManageSocieteDeviseList(int societeId, IEnumerable<SocieteDeviseEnt> societeDeviseList)
        {
            // TODO: Business validation (vérifier si il y au moins une et une seule devise de référence)
            List<int> existingSocieteDeviseList = GetSocieteDeviseList(societeId).Select(x => x.SocieteDeviseId).ToList();
            List<int> societeDeviseIdList = societeDeviseList.Select(x => x.SocieteDeviseId).ToList();

            // Suppresion des relations SocieteDevise
            if (existingSocieteDeviseList.Count > 0)
            {
                foreach (int societeDeviseId in existingSocieteDeviseList)
                {
                    if (!societeDeviseIdList.Contains(societeDeviseId))
                    {
                        societeDeviseRepo.DeleteById(societeDeviseId);
                        Save();
                    }
                }
            }

            societeDeviseRepo.AddOrUpdate(societeDeviseList);
            Save();

            return societeDeviseList;
        }

        public bool IsSocieteInGroupe(int societeId, string codeGroupe)
        {
            var societe = GetSocieteById(societeId, true);

            return societe?.Groupe?.Code.Trim() == codeGroupe;
        }

        /// <inheritdoc/>
        public SocieteEnt GetSocieteByOrganisationId(int organisationId)
        {
            return Repository.GetSocieteParentByOrgaId(organisationId);
        }

        /// <inheritdoc/>
        public SocieteEnt GetSocieteByOrganisationIdEx(int organisationId, bool withIncludeTypeSociete)
        {
            return Repository.GetSocieteParentByOrgaIdEx(organisationId, withIncludeTypeSociete);
        }

        /// <inheritdoc/>
        public IEnumerable<SocieteEnt> GetSocieteList(int utilisateurId)
        {
            int typeOrgaSocieteId = orgaManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeSociete);
            IEnumerable<int> orgaIdList = orgaManager.GetOrganisationsAvailable(types: new List<int> { typeOrgaSocieteId }, utilisateurId: utilisateurId).Select(x => x.OrganisationId);
            var societes = new List<SocieteEnt>();

            foreach (var orgaId in orgaIdList.ToList())
            {
                societes.Add(GetSocieteByOrganisationId(orgaId));
            }

            return societes;
        }

        public SocieteEnt GetSocieteByCodeAndGroupeId(string code, int groupeId)
        {
            return Repository.GetSocieteByCodeAndGroupeId(code, groupeId);
        }

        public bool IsSocieteIdInGroupe(int societeId, string codeGroupe)
        {
            return Repository.IsSocieteIdInGroupe(societeId, codeGroupe);
        }

        public async Task<Dictionary<string, int>> GetCompanyIdsByPayrollCompanyCodesAsync()
        {
            return await societeRepository.GetCompanyIdsByPayrollCompanyCodesAsync();
        }

        #region UniteSociete

        /// <inheritdoc />
        public IEnumerable<UniteSocieteEnt> ManageSocieteUniteList(int societeId, IEnumerable<UniteSocieteEnt> uniteSocieteList)
        {
            List<int> existingUniteSocieteList = GetListSocieteUnite(societeId).Select(x => x.UniteSocieteId).ToList();
            List<int> uniteSocieteIdList = uniteSocieteList.Select(x => x.UniteSocieteId).ToList();

            // Suppresion des relations SocieteDevise
            foreach (int uniteSocieteId in existingUniteSocieteList)
            {
                if (!uniteSocieteIdList.Contains(uniteSocieteId))
                {
                    try
                    {
                        uniteSocieteRepo.DeleteSocieteUnite(uniteSocieteId);
                        Save();
                    }
                    catch (FredRepositoryException)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        throw new FredRepositoryException(e.Message, e);
                    }
                    uniteSocieteList.ToList().Remove(uniteSocieteList.FirstOrDefault(us => us.UniteSocieteId == uniteSocieteId));
                }
            }

            foreach (var societeUnite in uniteSocieteList)
            {
                try
                {
                    societeUnite.Societe = null;
                    societeUnite.Unite = null;
                    if (societeUnite.UniteSocieteId.Equals(0))
                    {
                        uniteSocieteRepo.AddSocieteUnite(societeUnite);
                    }
                    else
                    {
                        uniteSocieteRepo.UpdateSocieteUnite(societeUnite);
                    }

                    Save();
                }
                catch (FredRepositoryException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new FredRepositoryException(e.Message, e);
                }
            }
            return GetListSocieteUnite(societeId);
        }

        /// <inheritdoc/>
        public void AddSocieteUnite(int societeId, int uniteId)
        {
            try
            {
                var uniteSocieteEnt = new UniteSocieteEnt()
                {
                    SocieteId = societeId,
                    UniteId = uniteId,
                    Type = 1
                };

                uniteSocieteRepo.AddSocieteUnite(uniteSocieteEnt);
                Save();
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <inheritdoc/>
        public void DeleteSocieteUnite(int uniteSocieteId)
        {
            uniteSocieteRepo.DeleteSocieteUnite(uniteSocieteId);
            Save();
        }

        /// <inheritdoc/>
        public List<UniteSocieteEnt> GetListSocieteUnite(int societeId)
        {
            return uniteSocieteRepo.GetListSocieteUnite(societeId);
        }

        /// <inheritdoc/>
        public IEnumerable<UniteEnt> SearchLightUniteBySocieteId(string recherche, int societeId, int page, int pageSize, string type)
        {
            var query = uniteSocieteRepo
                            .Query()
                            .Include(us => us.Unite)
                            .Filter(us => us.SocieteId == societeId)
                            .Filter(us => string.IsNullOrEmpty(recherche)
#pragma warning disable RCS1155 // Use StringComparison when comparing strings.
                                   || us.Unite.Code.ToLower().Contains(recherche.ToLower())
                                         || us.Unite.Libelle.ToLower().Contains(recherche.ToLower()));
#pragma warning restore RCS1155 // Use StringComparison when comparing strings.

            if (!string.IsNullOrEmpty(type))
            {
                if (type == "HA")
                {
                    query.Filter(us => us.Type == (int)TypeUnite.HA);
                }
                if (type == "Valo")
                {
                    query.Filter(us => us.Type == (int)TypeUnite.Valo);
                }
            }

            return query.OrderBy(list => list.OrderBy(us => us.Unite.Libelle)).GetPage(page, pageSize).Select(us => us.Unite).ToList();
        }

        #endregion

        public List<SocieteEnt> GetAllSocietesByIds(List<int> societeIds)
        {
            return Repository.Query()
                 .Filter(x => societeIds.Contains(x.SocieteId))
                 .Get()
                 .AsNoTracking()
                 .ToList();
        }

        public List<int?> GetTypeSocieteId(bool isSEP)
        {
            List<int?> typesSocietesId = new List<int?>();
            List<int?> typesSocietesIdAll = new List<int?>();

            typesSocietesIdAll.Add(EnumSocieteType.ISSEP.ToIntValue());
            typesSocietesIdAll.Add(EnumSocieteType.IsPAR.ToIntValue());
            typesSocietesIdAll.Add(EnumSocieteType.IsINT.ToIntValue());

            if (isSEP)
            {
                typesSocietesId.Add(EnumSocieteType.ISSEP.ToIntValue());
            }

            return typesSocietesId.Any() ? typesSocietesId : typesSocietesIdAll;
        }

        public List<SocieteEnt> Search(List<Expression<Func<SocieteEnt, bool>>> filters,
                                          Func<IQueryable<SocieteEnt>, IOrderedQueryable<SocieteEnt>> orderBy = null,
                                          List<Expression<Func<SocieteEnt, object>>> includeProperties = null,
                                          int? page = null,
                                          int? pageSize = null,
                                          bool asNoTracking = false)
        {
            return Repository.Search(filters, orderBy, includeProperties, page, pageSize, asNoTracking).ToList();
        }

        private void SaveParamValues(SocieteEnt societeEnt)
        {
            var typeSocieteCode = string.Empty;
            if (societeEnt.TypeSociete != null)
            {
                typeSocieteCode = societeEnt.TypeSociete.Code;
            }
            else if (societeEnt.TypeSocieteId.HasValue)
            {
                var typeSociete = typeSocieteManager.FindById(societeEnt.TypeSocieteId.Value);
                typeSocieteCode = typeSociete?.Code;
            }

            // pas de sauvegarde des parametres pour les sociétés interimaires et partenaires
            if (!societeEnt.IsInterimaire
                && !string.IsNullOrEmpty(typeSocieteCode)
                && typeSocieteCode != Constantes.TypeSociete.Partenaire)
            {
                var paramValues = new Dictionary<string, string>
                {
                    { ParamsKeys.BudgetAvancementEcart,  societeEnt.IsBudgetAvancementEcart? "1" : "0" },
                    { ParamsKeys.BudgetTypeAvancementDynamique,  societeEnt.IsBudgetTypeAvancementDynamique? "1" : "0" },
                    { ParamsKeys.BudgetSaisieRecette,  societeEnt.IsBudgetSaisieRecette? "1" : "0" }
                };
                paramsManager.AddOrUpdateOrganisationParamValues(societeEnt.Organisation.OrganisationId, paramValues);
            }
        }

        private void InitSocieteFromParamValues(SocieteEnt societeEnt)
        {
            // pas de sauvegarde des parametres pour les sociétés interimaires et partenaires
            if (!societeEnt.IsInterimaire && societeEnt.TypeSociete != null && societeEnt.TypeSociete.Code != Constantes.TypeSociete.Partenaire)
            {
                societeEnt.IsBudgetAvancementEcart = societeEnt.Organisation?.ParamValues?.SingleOrDefault(x => x.ParamKey.Libelle == ParamsKeys.BudgetAvancementEcart)?.Valeur == "1";
                societeEnt.IsBudgetTypeAvancementDynamique = societeEnt.Organisation?.ParamValues?.SingleOrDefault(x => x.ParamKey.Libelle == ParamsKeys.BudgetTypeAvancementDynamique)?.Valeur == "1";
                societeEnt.IsBudgetSaisieRecette = societeEnt.Organisation?.ParamValues?.SingleOrDefault(x => x.ParamKey.Libelle == ParamsKeys.BudgetSaisieRecette)?.Valeur == "1";
            }
        }

        public IEnumerable<GroupeEnt> GetSocietesGroupesByUserHabibilitation()
        {
            int utilisateurId = userManager.GetContextUtilisateurId();

            IEnumerable<SocieteEnt> societes = GetSocieteList(utilisateurId).Select(IncludeGroupe);

            return societes.GroupBy(s => s.Groupe).Select(g => g.Key);
        }

        private SocieteEnt IncludeGroupe(SocieteEnt societe)
        {
            societe.Groupe = groupeManager.FindById(societe.GroupeId);

            return societe;
        }

        /// <summary>
        /// Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        public SocieteEnt GetSocieteByCodeSocieteComptables(string code)
        {
            return Repository.GetSocieteByCodeSocieteComptables(code);
        }

        public async Task<int?> GetSocieteImageLogoIdByCodeAsync(int organisationId)
        {
            return await societeRepository.GetSocieteImageLogoIdByOrganisationIdAsync(organisationId);
        }

        public List<SocieteEnt> GetSocieteBySirenAndGroupeCode(string siren, string groupeCode)
        {
            return Repository.GetSocieteBySirenAndGroupeCode(siren, groupeCode);
        }

        /// <summary>
        /// Obtenir la liste des sociétés par organisation
        /// </summary>
        /// <param name="organisationIds">List des organisations ids</param>
        /// <returns>Liste des sociétés</returns>
        public List<SocieteEnt> GetSocieteByOrganisationIds(List<int> organisationIds)
        {
            return Repository.GetSocieteListByOrganisationIds(organisationIds).ToList();
        }

        /// <summary>
        /// Get societes list for remontee vrac fes Async
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="recherche">Recherche</param>
        /// <returns>List des societes</returns>
        public async Task<IEnumerable<SocieteEnt>> GetSocietesListForRemonteeVracFesAsync(int page, int pageSize, string recherche)
        {
            int utilisateurId = Managers.Utilisateur.GetContextUtilisateurId();
            IEnumerable<SocieteEnt> listSocietes = new List<SocieteEnt>();
            List<RoleFonctionnaliteEnt> roleFonctionnalites = await roleFonctionnaliteManager.GetRoleFonctionnaliteByUserIdAsync(utilisateurId, Constantes.FonctionnaliteLibelle.ValidationLotsPointage).ConfigureAwait(false);
            if (roleFonctionnalites.Any())
            {
                IEnumerable<OrganisationEnt> organisationList = roleFonctionnalites.Where(x => x.Role.AffectationSeuilUtilisateurs.Any()).SelectMany(x => x.Role.AffectationSeuilUtilisateurs)
                                                                                   .Where(x => x.UtilisateurId == utilisateurId).Select(x => x.Organisation);
                if (organisationList.Any())
                {
                    listSocietes = organisationList.Where(x => x.TypeOrganisation.Code.Equals(Constantes.OrganisationType.CodeSociete)).Select(x => x.Societe).Distinct();
                }
            }

            if (string.IsNullOrEmpty(recherche))
            {
                return listSocietes;
            }

            return listSocietes.Where((o) => FilterText(recherche, o)).OrderBy(x => x.Code).Skip((page - 1) * pageSize).Take(pageSize);
        }

        private bool FilterText(string text, SocieteEnt o)
        {
            if (string.IsNullOrEmpty(text) || (o.Libelle != null && ComparatorHelper.ComplexContains(o.Libelle, text)) ||
               (o.Code != null && ComparatorHelper.ComplexContains(o.Code, text)))
            {
                return true;
            }

            return false;
        }
    }
}
