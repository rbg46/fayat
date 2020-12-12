using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.Budget.Avancement;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Common;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Business.Referential.Tache
{
    public class TacheManager : Manager<TacheEnt, ITacheRepository>, ITacheManager
    {
        private readonly IUtilisateurManager usrManager;
        private readonly ITacheSearchHelper taskSearchHelper;

        public TacheManager(
            IUnitOfWork uow,
            ITacheRepository tacheRepo,
            ITacheValidator tacheValidator,
            IUtilisateurManager userManager,
            ITacheSearchHelper taskSearchHelper)
            : base(uow, tacheRepo, tacheValidator)
        {
            usrManager = userManager;
            this.taskSearchHelper = taskSearchHelper;
        }

        public bool IsDeletable(TacheEnt tache)
        {
            return Repository.IsDeletable(tache);
        }

        public IEnumerable<TacheEnt> GetAllTacheList()
        {
            return Repository.GetAllTacheList() ?? new TacheEnt[] { };
        }

        public IEnumerable<TacheEnt> GetAllTacheListSync()
        {
            return Repository.GetAllTacheListSync() ?? new TacheEnt[] { };
        }

        public IEnumerable<TacheEnt> GetTacheList()
        {
            return Repository.GetTacheList() ?? new TacheEnt[] { };
        }

        public IEnumerable<TacheEnt> GetTacheListByCiId(int ciId, bool includeTacheEcart = false)
        {
            return Repository.GetTacheListByCiId(ciId, includeTacheEcart) ?? new TacheEnt[] { };
        }

        public TacheEnt GetTacheLitigeByCiId(int ciId)
        {
            return Repository.GetTacheLitigeByCiId(ciId);
        }

        public IEnumerable<TacheEnt> GetTacheLevel1ByCiId(int ciId, bool includeTacheEcart = false)
        {
            return Repository.GetTacheLevel1ByCiId(ciId, includeTacheEcart) ?? new TacheEnt[] { };
        }

        public IEnumerable<TacheEnt> GetTacheListByCiIdAndParentId(int ciId, int parentId)
        {
            return Repository.GetTacheListByCiIdAndParentId(ciId, parentId);
        }

        public IEnumerable<TacheEnt> GetTacheListByCiIdAndNiveau(int ciId, int niveau)
        {
            return Repository.GetTacheListByCiIdAndNiveau(ciId, niveau);
        }

        public IEnumerable<TacheEnt> GetActiveTacheListByCiIdAndNiveau(int ciId, int niveau)
        {
            return Repository.GetActiveTacheListByCiIdAndNiveau(ciId, niveau);
        }

        public async Task<IEnumerable<TacheEnt>> GetActiveTacheListByCiIdAndNiveauAsync(int ciId, int niveau)
        {
            return await Repository.GetActiveTacheListByCiIdAndNiveauAsync(ciId, niveau);
        }

        /// <summary>
        ///   Ajout la tâche par défaut pour un CI
        /// </summary>
        /// <param name="ciIds">Identifiant du CI</param>
        public void AddTachesSysteme(IEnumerable<int> ciIds)
        {
            List<TacheEnt> taches = new List<TacheEnt>();
            var date = DateTime.UtcNow;

            foreach (var ciId in ciIds.ToList())
            {
                // NPI : transaction
                var tache1 = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheDefautNiveau1, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheDefaut, ParentId = null, Niveau = 1, TacheType = (int)TacheType.Defaut, DateCreation = date };
                taches.Add(tache1);
                var tache2 = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheDefautNiveau2, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheDefaut, Parent = tache1, Niveau = 2, TacheType = (int)TacheType.Defaut, DateCreation = date };
                taches.Add(tache2);
                var tache3 = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheDefautNiveau3, TacheParDefaut = true, Libelle = Constantes.TacheSysteme.LibelleTacheDefaut, Parent = tache2, Niveau = 3, TacheType = (int)TacheType.Defaut, DateCreation = date };
                taches.Add(tache3);
                var ecart1 = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartNiveau1, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartNiveau1Et2, ParentId = null, Niveau = 1, TacheType = (int)TacheType.EcartNiveau1, DateCreation = date };
                taches.Add(ecart1);
                var ecart2 = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartNiveau2, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartNiveau1Et2, Parent = ecart1, Niveau = 2, TacheType = (int)TacheType.EcartNiveau2, DateCreation = date };
                taches.Add(ecart2);
                var ecartMOEncadrement = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartMOEncadrement, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartMOEncadrement, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartMOEncadrement, DateCreation = date };
                taches.Add(ecartMOEncadrement);
                var ecartMOProduction = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartMOProduction, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartMOProduction, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartMOProduction, DateCreation = date };
                taches.Add(ecartMOProduction);
                var ecartMateriel = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartMateriel, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartMateriel, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartMateriel, DateCreation = date };
                taches.Add(ecartMateriel);
                var ecartMaterielImmobilise = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartMaterielImmobilise, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartMaterielImmobilise, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartMaterielImmobilise, DateCreation = date };
                taches.Add(ecartMaterielImmobilise);
                var ecartAchat = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartAchat, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartAchat, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartAchat, DateCreation = date };
                taches.Add(ecartAchat);
                var ecartAutreFrais = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartAutreFrais, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartAutreFrais, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartAutreFrais, DateCreation = date };
                taches.Add(ecartAutreFrais);
                var ecartInterim = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartInterim, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartInterim, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartInterim, DateCreation = date };
                taches.Add(ecartInterim);
                var ecartMaterielExterne = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartMaterielExterne, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartMaterielExterne, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartMaterielExterne, DateCreation = date };
                taches.Add(ecartMaterielExterne);
                var ecartRecette = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartRecette, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartRecette, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartRecette, DateCreation = date };
                taches.Add(ecartRecette);
                var ecartFraisGeneraux = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartFraisGeneraux, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartFraisGeneraux, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartFraisGeneraux, DateCreation = date };
                taches.Add(ecartFraisGeneraux);
                var ecartAutresDepensesHorsDebours = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheEcartAutresDepensesHorsDebours, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheEcartAutresDepensesHorsDebours, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.EcartAutresDepensesHorsDebours, DateCreation = date };
                taches.Add(ecartAutresDepensesHorsDebours);
                var litige = new TacheEnt { CiId = ciId, Code = Constantes.TacheSysteme.CodeTacheLitige, TacheParDefaut = false, Libelle = Constantes.TacheSysteme.LibelleTacheLitige, Parent = ecart2, Niveau = 3, TacheType = (int)TacheType.Litige, DateCreation = date };
                taches.Add(litige);
            }

            Repository.BulkInsert(taches);
        }

        public bool CheckDefaultTache(int ciId)
        {
            return Repository.CheckDefaultTache(ciId);
        }

        public TacheEnt GetTacheById(int tacheId)
        {
            return Repository.GetTacheById(tacheId);
        }

        public TacheEnt GetParentTacheById(int tacheId)
        {
            return Repository.GetParentTacheById(tacheId);
        }

        public int AddTache(TacheEnt tacheEnt)
        {
            if (!IsCodeTacheExist(tacheEnt.Code, tacheEnt.CiId))
            {
                tacheEnt.DateCreation = DateTime.UtcNow;

                if (!tacheEnt.AuteurCreationId.HasValue)
                {
                    tacheEnt.AuteurCreationId = usrManager.GetContextUtilisateurId();
                }

                try
                {
                    // YDY : transaction
                    if (tacheEnt.TacheParDefaut)
                    {
                        // Cette tâche est devenue celle par défaut
                        var currentTacheParDefaut = Repository.GetTacheParDefaut(tacheEnt.CiId);
                        if (currentTacheParDefaut != null)
                        {
                            currentTacheParDefaut.TacheParDefaut = false;
                            Repository.UpdateTache(currentTacheParDefaut);
                        }
                    }

                    Repository.AddTache(tacheEnt);
                    Save();
                }
                catch (FredRepositoryException ex)
                {
                    throw new FredBusinessException(ex.Message, ex);
                }

                return tacheEnt.TacheId;
            }
            else
            {
                throw new FredBusinessConflictException(string.Format(BusinessResources.CodeDejaExistant, BusinessResources.Tache));
            }
        }

        public int AddTache4(TacheEnt tacheEnt, int budgetId, List<TacheEnt> existingTaches)
        {
            if (!IsCodeTache4Exist(tacheEnt, existingTaches))
            {
                tacheEnt.DateCreation = DateTime.UtcNow;
                tacheEnt.AuteurCreationId = usrManager.GetContextUtilisateurId();

                try
                {
                    // YDY : transaction
                    if (tacheEnt.TacheParDefaut)
                    {
                        // Cette tâche est devenue celle par défaut
                        var currentTacheParDefaut = GetTachesByDefault(tacheEnt.CiId, existingTaches);
                        if (currentTacheParDefaut != null)
                        {
                            currentTacheParDefaut.TacheParDefaut = false;
                            Repository.UpdateTache(currentTacheParDefaut);
                        }
                    }

                    Repository.AddTache(tacheEnt);
                }
                catch (FredRepositoryException ex)
                {
                    throw new FredBusinessException(ex.Message, ex);
                }

                return tacheEnt.TacheId;
            }
            else
            {
                return GetTacheIdByCodeAndCiId(tacheEnt.Code, tacheEnt.CiId, existingTaches);
            }
        }

        private bool IsCodeTache4Exist(TacheEnt tache, List<TacheEnt> existingTaches)
        {
            return existingTaches.Count(t => t.CiId.Equals(tache.CiId) && t.BudgetId.HasValue && t.BudgetId == tache.BudgetId && t.Code.ToLower().Equals(tache.Code.ToLower())) > 0;
        }

        private TacheEnt GetTachesByDefault(int ciId, List<TacheEnt> existingTaches)
        {
            return existingTaches.FirstOrDefault(t => t.CiId == ciId && t.TacheParDefaut && t.Niveau == 3);
        }

        private int GetTacheIdByCodeAndCiId(string code, int ciId, List<TacheEnt> existingTaches)
        {
            TacheEnt tache = existingTaches.FirstOrDefault(t => t.CiId == ciId && t.Code == code);

            return tache?.TacheId ?? 0;
        }

        public void UpdateTache(TacheEnt updatedTask)
        {
            var currentTask = GetTacheById(updatedTask.TacheId);

            if (updatedTask.Code == currentTask.Code || !IsCodeTacheExist(updatedTask.Code, updatedTask.CiId))
            {
                updatedTask.DateModification = DateTime.UtcNow;
                updatedTask.AuteurModificationId = usrManager.GetContextUtilisateurId();
                BusinessValidation(updatedTask);

                // YDY : transaction
                if (updatedTask.TacheParDefaut && !currentTask.TacheParDefaut)
                {
                    // Cette tâche est devenue celle par défaut
                    var currentTacheParDefaut = Repository.GetTacheParDefaut(currentTask.CiId);
                    if (currentTacheParDefaut != null)
                    {
                        currentTacheParDefaut.TacheParDefaut = false;
                        Repository.UpdateTache(currentTacheParDefaut);
                    }
                }

                Repository.UpdateTache(updatedTask);
                Save();
            }
            else
            {
                throw new FredBusinessException(string.Format(BusinessResources.CodeDejaExistant, BusinessResources.Tache));
            }
        }

        public void UpdateChildrenActiveStatus(TacheEnt tacheEnt)
        {
            foreach (var tache in tacheEnt.TachesEnfants.ToList())
            {
                if (tache.Niveau <= 4)
                {
                    tache.Active = tacheEnt.Active;
                    UpdateTache(tache);
                }

                if (tache.TachesEnfants?.Count > 0)
                {
                    UpdateChildrenActiveStatus(tache);
                }
            }
        }

        public void DeleteTacheById(int id)
        {
            var tache = Repository.FindById(id);
            if (IsDeletable(tache))
            {
                Repository.DeleteTacheById(tache.TacheId);
                Save();
            }
            else
            {
                throw new FredBusinessMessageResponseException(TacheResources.Tache_SuppressionImpossible);
            }
        }

        public TacheEnt GetTacheParDefaut(int ciId)
        {
            return Repository.GetTacheParDefaut(ciId);
        }

        public bool IsCodeTacheExist(string code, int ciId)
        {
            return Repository.IsCodeTacheExist(code, ciId);
        }

        public IEnumerable<TacheEnt> SearchTachesByCiAndNiveau(string text, int ciId, int niveau)
        {
            if (string.IsNullOrEmpty(text))
            {
                return GetTacheListByCiIdAndNiveau(ciId, niveau);
            }

            return Repository.SearchTachesByCiAndNiveau(text, ciId, niveau);
        }

        public IEnumerable<TacheEnt> SearchLight(string text, int page, int pageSize, int? ciId, bool activeOnly = true, int niveauTache = 3, bool isTechnicalTask = false)
        {
            return Repository.SearchLight(text, page, pageSize, ciId, activeOnly, niveauTache, isTechnicalTask);
        }

        public async Task<IEnumerable<TacheEnt>> SearchLightAsync(int ciId, int page, int pageSize, string recherche, bool activeOnly, int niveauTache = 3)
        {
            return await Repository.SearchLightAsync(ciId, page, pageSize, recherche, activeOnly, niveauTache);
        }

        /// <summary>
        ///   Génère une proposition de nouveau code pour une tâche enfant en fonction de la tâche parent
        /// </summary>
        /// <param name="parentTask">La tâche parente</param>
        /// <returns>Retourne un nouveau code</returns>
        public string GetNextTaskCode(TacheEnt parentTask)
        {
            var childTaches4 = Repository.GetParentTache(parentTask);
            childTaches4 = childTaches4.Where(t => !taskSearchHelper.IsTacheEcart(t)).ToList();
            return GetNextTaskCode(parentTask, childTaches4);
        }

        public List<string> GetNextTaskCodes(TacheEnt parentTask, int count)
        {
            var childTaches4 = Repository.GetParentTache(parentTask);
            childTaches4 = childTaches4.Where(t => !taskSearchHelper.IsTacheEcart(t)).ToList();

            var ret = new List<string>();
            for (var i = 0; i < count; i++)
            {
                var code = GetNextTaskCode(parentTask, childTaches4);
                ret.Add(code);
                childTaches4.Add(new TacheEnt() { Code = code });
            }
            return ret;
        }

        private string GetNextTaskCode(TacheEnt parentTask, List<TacheEnt> childTaches4)
        {
            int increment = 5, firstCode = 1;
            string nextCode = string.Empty;

            if (childTaches4.Count > 0)
            {
                int maxCode = 0, index = 0, tmp = 0;
                string currentCode = string.Empty;

                foreach (var t in childTaches4)
                {
                    index = t.Code.IndexOf(parentTask.Code);
                    currentCode = index < 0 ? t.Code : t.Code.Remove(index, parentTask.Code.Length);
                    if (int.TryParse(currentCode, out tmp) && ((tmp % increment) == 0 || tmp == 1) && tmp > maxCode)
                    {
                        maxCode = tmp;
                    }
                }

                switch (maxCode)
                {
                    case 0:
                        maxCode = firstCode;
                        break;
                    case 1:
                        maxCode = increment;
                        break;
                    default:
                        maxCode += increment;
                        break;
                }

                nextCode = parentTask.Code + maxCode.ToString("D2");
            }
            else
            {
                nextCode = parentTask.Code + firstCode.ToString("D2");
            }

            return nextCode;
        }

        public IEnumerable<TacheEnt> GetSyncTaches(DateTime lastModification = default(DateTime))
        {
            //On récupère tous les CIs de l'utilisateur courant.
            //var cis = ciMgr.GetSyncCIs()
            var cis = new List<CIEnt>();

            IEnumerable<int> ciIds = cis.Select(r => r.CiId);
            return Repository.GetTaches(ciIds, lastModification);
        }

        public IEnumerable<TacheEnt> GetAllT1ByCiId(int ciId, DateTime? date, bool includeTacheEcart = false)
        {
            return Repository.GetAllT1ByCiId(ciId, date, includeTacheEcart).OrderBy(t => t.Code);
        }

        public TacheEnt GetTache(string code, int ciId, bool asNoTracking = false)
        {
            try
            {
                return Repository.GetTache(code, ciId, asNoTracking);
            }
            catch (Exception exception)
            {
                throw new FredBusinessException(exception.Message, exception);
            }
        }

        public void AddOrUpdateTache(TacheEnt tache)
        {
            try
            {
                if (tache.TacheId.Equals(0))
                {
                    Repository.AddTache(tache);
                }
                else
                {
                    Repository.UpdateTache(tache);
                }

                Save();
            }
            catch (Exception exception)
            {
                throw new FredBusinessException(exception.Message, exception);
            }
        }

        public void CopyPlanTache(int ciIdSource, int ciIdDestination)
        {
            try
            {
                if (ciIdDestination != ciIdSource)
                {
                    IEnumerable<TacheEnt> tachesSource = Repository.GetAllT1ByCiId(ciIdSource, null).Where(x => x.Code != Constantes.TacheSysteme.CodeTacheDefautNiveau1).ToList();
                    IEnumerable<TacheEnt> tachesCible = Repository.GetTacheListByCiId(ciIdDestination, true).ToList();
                    Copy(tachesSource, null, tachesCible, ciIdDestination, false);
                    Save();
                }
            }
            catch (Exception exception)
            {
                throw new FredBusinessException(exception.Message, exception);
            }
        }

        private void Copy(IEnumerable<TacheEnt> tachesSource, TacheEnt tacheCibleParent, IEnumerable<TacheEnt> tachesDestination, int ciIdDestination, bool isPartialCopy)
        {
            if (tachesSource == null)
            {
                return;
            }
            foreach (var tacheSource in tachesSource)
            {
                // Regarde si le code de la tâche source existe dans le CI cible
                var tacheCible = tachesDestination.FirstOrDefault(t => t.Code == tacheSource.Code);
                if (tacheCible != null && !isPartialCopy)
                {
                    // Le code de la tâche source existe dans le CI cible
                    ProcessExistingTache(tacheSource, tacheCible, ciIdDestination);
                }
                else
                {
                    // Le code de la tâche source n'existe pas dans le CI cible
                    // Dans ce cas on crée une copie de la tache source dans le CI cible
                    ProcessNewTache(tacheSource, tacheCibleParent, tachesDestination, ciIdDestination, isPartialCopy);
                }
            }
        }

        private void ProcessExistingTache(TacheEnt tacheSource, TacheEnt tacheCible, int ciIdDestination)
        {
            // Le niveau de la tâche source doit être le même que celui de la tâche cible, sinon on ne fait rien
            // De plus on ne traite pas les tâches de niveau 4
            if (tacheCible.Niveau == tacheSource.Niveau && tacheSource.Niveau < 3)
            {
                // Les tâches sont de même niveau
                // Dans ce cas on considère que la tâche cible correspond à la tâche source
                Copy(tacheSource.TachesEnfants, tacheCible, tacheCible.TachesEnfants, ciIdDestination, false);
            }
        }

        private void ProcessNewTache(TacheEnt tacheSource, TacheEnt tacheCibleParent, IEnumerable<TacheEnt> tachesDestination, int ciIdDestination, bool isPartialCopy)
        {
            var nextTacheCibleParent = CloneTache(tacheSource, tacheCibleParent, ciIdDestination, isPartialCopy);

            while (tachesDestination != null && isPartialCopy && tachesDestination.Select(m => m.Code).Contains(nextTacheCibleParent.Code))
            {
                nextTacheCibleParent = CloneTache(nextTacheCibleParent, tacheCibleParent, ciIdDestination, true);
            }

            if (tacheCibleParent == null)
            {
                // Il s'agit d'une tâche de niveau 1 qui n'a donc pas de parent, on l'insert
                Repository.Insert(nextTacheCibleParent);
            }
            else
            {
                // Il s'agit d'une tâche de niveau 2 qui a donc un parent, on l'ajoute juste à son parent
                if (tacheCibleParent.TachesEnfants == null)
                {
                    tacheCibleParent.TachesEnfants = new List<TacheEnt>();
                }
                tacheCibleParent.TachesEnfants.Add(nextTacheCibleParent);
            }

            if (tacheSource.Niveau < 3)
            {
                var newTachesDestination = tachesDestination.Where(x => x.TachesEnfants != null).SelectMany(x => x.TachesEnfants).ToList();
                Copy(tacheSource.TachesEnfants, nextTacheCibleParent, newTachesDestination, ciIdDestination, isPartialCopy);
            }
        }

        public void CopyTachePartially(int ciIdDestination, int ciIdSource, List<int> listIdTache)
        {
            foreach (int idTache in listIdTache)
            {
                IEnumerable<TacheEnt> tachesSource = null;
                try
                {

                    IEnumerable<TacheEnt> tachesDestination = Repository.GetTacheLevel1ByCiId(ciIdDestination).ToList();
                    tachesSource = Repository.GetTAssocitedToT1(new List<int>() { idTache }).ToList();

                    if (ciIdDestination != ciIdSource)
                    {
                        Copy(tachesSource, null, tachesDestination, ciIdDestination, false);
                    }
                    else
                    {
                        Copy(tachesSource, null, tachesDestination, ciIdDestination, true);
                    }

                    Save();
                }
                catch (Exception exception)
                {
                    if (exception.Message.Contains("DbUpdateException"))
                    {
                        //taille du code en bdd = 10
                        //taille du préfixe ajouté à la tache "X-" = 2
                        var duplicateNumber = (10 - tachesSource.FirstOrDefault().Code.Length) / 2;
                        var message = duplicateNumber == 0
                            ? string.Format(BusinessResources.ErrorDuplicateTache, tachesSource.FirstOrDefault().Code)
                            : string.Format(BusinessResources.ErrorDuplicateTacheNTimes, tachesSource.FirstOrDefault().Code, duplicateNumber);
                        throw new FredBusinessMessageResponseException(message);
                    }
                    throw new FredBusinessException(exception.Message, exception);
                }
            }

        }

        private TacheEnt CloneTache(TacheEnt tacheSource, TacheEnt tacheParent, int ciIdCible, bool isPartialEqual)
        {
            string code = isPartialEqual ? "X-" + tacheSource.Code : tacheSource.Code;

            return new TacheEnt
            {
                TacheId = 0,
                Code = code,
                Libelle = tacheSource.Libelle,
                TacheParDefaut = false,
                DateCreation = DateTime.UtcNow,
                Niveau = tacheSource.Niveau,
                Active = tacheSource.Active,
                QuantiteBase = tacheSource.QuantiteBase,
                PrixTotalQB = tacheSource.PrixTotalQB,
                PrixUnitaireQB = tacheSource.PrixUnitaireQB,
                TotalHeureMO = tacheSource.TotalHeureMO,
                HeureMOUnite = tacheSource.HeureMOUnite,
                QuantiteARealise = tacheSource.QuantiteARealise,
                NbrRessourcesToParam = tacheSource.NbrRessourcesToParam,
                TacheType = tacheSource.TacheType,
                CiId = ciIdCible,
                ParentId = tacheParent?.TacheId
            };
        }

        public List<TTache> GetTaches<TTache>(int ciId, bool includeT4, Expression<Func<TacheEnt, TTache>> selector)
        {
            return Repository.Get()
                .Where(t => t.CiId == ciId && !t.DateSuppression.HasValue && (includeT4 || t.Niveau < 4))
                .Select(selector)
                .ToList();
        }

        public List<TTache> GetTaches<TTache>(int ciId, int niveau, bool onlyActive, bool includeTacheEcart, Expression<Func<TacheEnt, TTache>> selector)
        {
            return Repository.GetTaches(ciId, niveau, onlyActive, includeTacheEcart, selector);
        }

        public List<TacheEnt> Get(List<CiIdAndTacheCodeModel> listCiIdAndTacheCode)
        {
            return Repository.Get(listCiIdAndTacheCode);
        }

        public List<TacheEnt> GetListByCi(int ciId)
        {
            return Repository.GetListByCi(ciId);
        }
    }
}
