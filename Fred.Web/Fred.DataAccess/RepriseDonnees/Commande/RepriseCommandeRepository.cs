using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.DataAccess.RepriseDonnees.Commande
{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public class RepriseCommandeRepository : IRepriseCommandeRepository
    {

        private readonly FredDbContext fredDbContext;



        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>       
        public RepriseCommandeRepository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            this.fredDbContext = (UnitOfWork as UnitOfWork).Context;
            //// this.fredDbContext.Database.Log = s => Debug.WriteLine(s) 
        }

        /// <summary>
        /// UnitOfWork
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }



        /// <summary>
        /// Recuere les commandes eyant le numero ou le NumeroCommandeExterne contenu dans la liste.
        /// </summary>
        /// <param name="numeroAndNumeroCommandeExternes">Liste desnumero de commandes contenu </param>
        /// <returns>Les commandes</returns>
        public List<CommandeEnt> GetCommandes(List<string> numeroAndNumeroCommandeExternes)
        {
            return this.fredDbContext.Commandes
                .Where(x => numeroAndNumeroCommandeExternes.Contains(x.Numero) || numeroAndNumeroCommandeExternes.Contains(x.NumeroCommandeExterne))
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Recupere les type de commandes
        /// </summary>
        /// <returns>les type de commandes</returns>
        public List<CommandeTypeEnt> GetCommandesTypes()
        {
            return this.fredDbContext.CommandeTypes
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Recupere la liste des fournissuer par groupe et dont le Code estcontenu dans la liste 'codes'
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="codes">codes des fournisseurs rechercher</param>
        /// <returns>les de fournisseurs</returns>
        public List<FournisseurEnt> GetFournisseurByGroupeAndCodes(int groupeId, List<string> codes)
        {
            return this.fredDbContext.Fournisseurs
                .Where(x => x.GroupeId == groupeId && codes.Contains(x.Code))
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Creation des commandes, commandesLignes et receptions(DepenseAchatEnt)
        /// </summary>
        /// <param name="commandes">Les commandes</param>
        /// <param name="commandeLignes">Les Commandes lignes</param>
        /// <param name="receptions">Les receptions</param>
        /// <param name="entitiesCreatedcallback">Action qui sera executée apres l'insertion des entitées</param>
        public void SaveEntities(List<CommandeEnt> commandes,
            List<CommandeLigneEnt> commandeLignes,
            List<DepenseAchatEnt> receptions,
            Action<List<CommandeEnt>> entitiesCreatedcallback)
        {
            if (!commandes.Any() && !commandeLignes.Any() && !receptions.Any())
            {
                return;
            }
            using (var dbContextTransaction = this.fredDbContext.Database.BeginTransaction())
            {
                try
                {
                    // disable detection of changes
                    this.fredDbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    if (commandes.Any())
                    {
                        this.fredDbContext.Commandes.AddRange(commandes);
                    }
                    if (commandeLignes.Any())
                    {
                        this.fredDbContext.CommandeLigne.AddRange(commandeLignes);
                    }
                    if (receptions.Any())
                    {
                        this.fredDbContext.DepenseAchats.AddRange(receptions);
                    }

                    this.fredDbContext.SaveChanges();

                    if (entitiesCreatedcallback != null)
                    {
                        entitiesCreatedcallback(commandes);
                    }

                    foreach (var commande in commandes)
                    {
                        this.fredDbContext.Entry(commande).Property(x => x.Numero).IsModified = true;
                    }

                    this.fredDbContext.SaveChanges();

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
                    this.fredDbContext.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }


        }

        /// <summary>
        /// Retourne la listes de devises par codes
        /// </summary>
        /// <param name="codesDevises">codesDevises</param>
        /// <returns>liste de devises</returns>
        public List<DeviseEnt> GetDeviseByCodes(List<string> codesDevises)
        {
            return this.fredDbContext.Devise
                .Where(x => codesDevises.Contains(x.IsoCode))
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retourne une liste d'unité en fonction d'une liste de codes
        /// </summary>
        /// <param name="codesUnites">Liste de codes</param>
        /// <returns>Liste d'unités</returns>
        public List<UniteEnt> GetUnitesByCodes(List<string> codesUnites)
        {
            return this.fredDbContext.Unites
                .Where(x => codesUnites.Contains(x.Code.ToLower()))
                .AsNoTracking()
                .ToList();
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
                var defaultDatabaseResult = fredDbContext.Taches
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
                var t3sRequestsDatabaseResult = fredDbContext.Taches
                    .Where(t => t3sRequestsCodes.Contains(t.Code.ToLower()) && t3sRequestsCiIds.Contains(t.CiId) && t.Niveau == 3)
                    .AsNoTracking()
                    .ToList();

                foreach (var t3sRequest in t3sRequests)
                {
                    var foundTache = t3sRequestsDatabaseResult.FirstOrDefault(x => x.CiId == t3sRequest.CiId && string.Equals(x.Code, t3sRequest.Code, StringComparison.OrdinalIgnoreCase));

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
        /// Retourne le DepenseTypeEnt de type reception
        /// </summary>
        /// <returns>DepenseTypeEnt de type reception</returns>
        public DepenseTypeEnt GetDepenseTypeReception()
        {
            var codeDepenseTypeReception = DepenseType.Reception.ToIntValue();
            return this.fredDbContext.DepenseTypes
                .AsNoTracking()
                .First(x => x.Code == codeDepenseTypeReception);
        }

    }
}
