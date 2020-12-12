using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.RepriseDonnees.Rapport
{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public class RepriseRapportRepository : IRepriseRapportRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>       
        public RepriseRapportRepository(IUnitOfWork unitOfWork, FredDbContext context)
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
        /// <param name="societeIds">societeId list</param>
        /// <param name="codeDeplacementCodes">codes des codeDeplacements rechercher</param>
        /// <returns>les codes deplacements</returns>
        public List<CodeDeplacementEnt> GetCodeDeplacementListByCodes(List<int> societeIds, List<string> codeDeplacementCodes)
        {
            return context.CodesDeplacement
                    .Where(x => !string.IsNullOrEmpty(x.Code) && codeDeplacementCodes.Contains(x.Code) && societeIds.Contains(x.SocieteId))
                    .AsNoTracking()
                    .ToList();
        }

        /// <summary>
        /// Recupere la liste des code zone deplacement par societeId et dont le Code est contenu dans la liste 'codes'
        /// </summary>
        /// <param name="societeIds">societeId list</param>
        /// <param name="codeZoneDeplacementCodes">codes des codeZoneDeplacements rechercher</param>
        /// <returns>les code zone deplacements</returns>
        public List<CodeZoneDeplacementEnt> GetCodeZoneDeplacementListByCodes(List<int> societeIds, List<string> codeZoneDeplacementCodes)
        {
            return context.CodeZoneDeplacement
                    .Where(x => !string.IsNullOrEmpty(x.Code) && codeZoneDeplacementCodes.Contains(x.Code) && societeIds.Contains(x.SocieteId))
                    .AsNoTracking()
                    .ToList();
        }
        /// <summary>
        ///  Recupere les personnels dont le matricule est contenu dans la liste matricules et pour plusieurs societes
        ///  ATTENTION !!!! : il se peut que 2 personnels aient le meme matricule pour 2 societes differentes.
        /// </summary>
        /// <param name="societeIds">liste des societes dans lequel les personnels existent</param>
        /// <param name="matricules">liste des matricules recherchés</param>
        /// <returns>les personnels pour plusieurs societes</returns>
        public List<PersonnelEnt> GetPersonnelListBySocieteIdsAndMatricules(List<int> societeIds, List<string> matricules)
        {
            return context.Personnels
                    .Where(x => x.SocieteId.HasValue && societeIds.Contains(x.SocieteId.Value) && matricules.Contains(x.Matricule))
                    .AsNoTracking()
                    .ToList();
        }

        /// <summary>
        ///  Recupere statuts de rapport
        /// </summary>
        /// <returns>statuts de rapport</returns>
        public List<RapportStatutEnt> GetRapportStatutList()
        {
            return context.RapportStatuts
                    .AsNoTracking()
                    .ToList();
        }
    }
}
