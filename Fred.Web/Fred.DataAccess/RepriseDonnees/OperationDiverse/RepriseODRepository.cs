using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.RepriseDonnees.OperationDiverse
{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public class RepriseODRepository : IRepriseODRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>       
        public RepriseODRepository(IUnitOfWork unitOfWork, FredDbContext context)
        {
            this.UnitOfWork = unitOfWork;
            this.context = context;
        }

        /// <summary>
        /// UnitOfWork
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Creation des rapports, rapportLignes
        /// </summary>
        /// <param name="rapports">Les rapports</param>
        /// <param name="rapportLignes">Les rapport lignes</param>
        public void SaveEntities(List<RapportEnt> rapports, List<RapportLigneEnt> rapportLignes)
        {
            if (!rapports.Any() && !rapportLignes.Any())
            {
                return;
            }
            using (var dbContextTransaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    // disable detection of changes
                    this.context.ChangeTracker.AutoDetectChangesEnabled = false;

                    if (rapports.Any())
                    {
                        this.context.Rapports.AddRange(rapports);
                    }
                    if (rapportLignes.Any())
                    {
                        this.context.RapportLignes.AddRange(rapportLignes);
                    }

                    this.context.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
                finally
                {
                    // re-enable detection of changes
                    this.context.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }
        }

        /// <summary>
        /// Recherche dans la base les taches a partir de Requette :
        /// Si la requette n'a pas de code alors : 
        /// - rechercher la tâche par défaut du CI identifié
        /// Si la valeur "Code" de la requette est non vide :
        /// - rechercher cette valeur parmi les tâches de niveau 3 associées au CI identifié
        /// </summary>
        /// <param name="requests">Liste de requettes </param>
        /// <returns>Liste de reponse : si la "Tache" de la reponse est vide, c'est qu'aucune tache n'a été trouvée.</returns>
        public List<GetT3ByCodesOrDefaultResponse> GetT3ByCodesOrDefault(List<GetT3ByCodesOrDefaultRequest> requests)
        {
            var result = new List<GetT3ByCodesOrDefaultResponse>();

            var defaultTacheRequests = requests.Where(x => string.IsNullOrEmpty(x.Code)).ToList();
            if (defaultTacheRequests.Any())
            {
                var defaultTacheRequestsCiIds = defaultTacheRequests.Select(x => x.CiId).ToList();
                var defaultDatabaseResult = context.Taches
                    .Where(t => t.TacheParDefaut && defaultTacheRequestsCiIds.Contains(t.CiId) && t.Niveau == 3)
                    .AsNoTracking()
                    .ToList();

                foreach (var defaultTacheRequest in defaultTacheRequests)
                {
                    var foundTache = defaultDatabaseResult.FirstOrDefault(x => x.CiId == defaultTacheRequest.CiId);

                    result.Add(new GetT3ByCodesOrDefaultResponse()
                    {
                        CiId = defaultTacheRequest.CiId,
                        Code = foundTache?.Code,
                        Tache = foundTache
                    });
                }
            }
            var t3sRequests = requests.Where(x => !string.IsNullOrEmpty(x.Code)).ToList();
            if (t3sRequests.Any())
            {
                var t3sRequestsCiIds = t3sRequests.Select(x => x.CiId).ToList();
                var t3sRequestsCodes = t3sRequests.Select(x => x.Code.ToLower()).ToList();
                var t3sRequestsDatabaseResult = context.Taches
                    .Where(t => t3sRequestsCodes.Contains(t.Code.ToLower()) && t3sRequestsCiIds.Contains(t.CiId) && t.Niveau == 3)
                    .AsNoTracking()
                    .ToList();

                foreach (var t3sRequest in t3sRequests)
                {

                    var foundTache = t3sRequestsDatabaseResult.FirstOrDefault(x => x.CiId == t3sRequest.CiId && x.Code.ToLower() == t3sRequest.Code.ToLower());

                    result.Add(new GetT3ByCodesOrDefaultResponse()
                    {
                        CiId = t3sRequest.CiId,
                        Code = foundTache?.Code,
                        Tache = foundTache
                    });
                }
            }
            return result;
        }
        /// <summary>
        /// Recupere la liste des code deplacement par societeId et dont le Code est contenu dans la liste 'codes'
        /// </summary>
        /// <param name="uniteCodes">codes des codeDeplacements rechercher</param>
        /// <returns>les codes deplacements</returns>
        public List<UniteEnt> GetUniteListByCodes(List<string> uniteCodes)
        {
            return context.Unites
                    .Where(x => !string.IsNullOrEmpty(x.Code) && uniteCodes.Contains(x.Code))
                    .AsNoTracking()
                    .ToList();
        }
        /// <summary>
        /// Recupere la liste des code deplacement par societeId et dont le Code est contenu dans la liste 'codes'
        /// </summary>
        /// <param name="uniteCode">codes des codeDeplacements rechercher</param>
        /// <returns>les codes deplacements</returns>
        public UniteEnt GetDefaultUniteCode(string uniteCode)
        {
            return context.Unites
                    .Where(x => !string.IsNullOrEmpty(x.Code) && x.Code == uniteCode)
                    .AsNoTracking()
                    .FirstOrDefault();
        }

        /// <summary>
        /// Recupere la liste des code deplacement par societeId et dont le Code est contenu dans la liste 'codes'
        /// </summary>
        /// <param name="deviseCode">code devise rechercher</param>
        /// <returns>les codes deplacements</returns>
        public DeviseEnt GetDefaultDeviseCode(string deviseCode)
        {
            return context.Devise
                    .Where(x => !string.IsNullOrEmpty(x.IsoCode) && x.IsoCode == deviseCode)
                    .AsNoTracking()
                    .FirstOrDefault();
        }

        /// <summary>
        /// Recupere la liste des code zone deplacement par societeId et dont le Code est contenu dans la liste 'codes'
        /// </summary>
        /// <param name="deviseCodes">codes des devises à rechercher</param>
        /// <returns>les devises</returns>
        public List<DeviseEnt> GetDeviseListByCodes(List<string> deviseCodes)
        {
            return context.Devise
                    .Where(x => !string.IsNullOrEmpty(x.IsoCode) && deviseCodes.Contains(x.IsoCode))
                    .AsNoTracking()
                    .ToList();
        }
        /// <summary>
        ///  Recupere les famille Oparation diverses dont le code est contenu dans la liste des codes famille
        /// </summary>
        /// <param name="societesIds">societeId list</param>
        /// <param name="codesFamille">liste des codes famille recherchés</param>
        /// <returns>les FamilleOperationDiverseEnt</returns>
        public List<FamilleOperationDiverseEnt> GetFamilleODListByCodes(List<int> societesIds, List<string> codesFamille)
        {
            return context.FamilleOperationDiverse
                    .Where(x => !string.IsNullOrEmpty(x.Code) && codesFamille.Contains(x.Code) && societesIds.Contains(x.SocieteId))
                    .AsNoTracking()
                    .ToList();
        }

        /// <summary>
        ///  Recupere les famille Oparation diverses dont le code est contenu dans la liste des codes famille
        /// </summary>
        /// <param name="groupeId">GroupeId</param>
        /// <param name="codesRessource">liste des codes famille recherchés</param>
        /// <returns>les RessourceEnt</returns>
        public List<RessourceEnt> GetRessourceListByCodes(int groupeId, List<string> codesRessource)
        {
            return context.Ressources
                    .Include(x => x.SousChapitre.Chapitre)
                    .Where(x => !string.IsNullOrEmpty(x.Code) && codesRessource.Contains(x.Code)
                            && x.SousChapitre.Chapitre.GroupeId.Equals(groupeId))
                    .AsNoTracking()
                    .ToList();
        }
    }
}
