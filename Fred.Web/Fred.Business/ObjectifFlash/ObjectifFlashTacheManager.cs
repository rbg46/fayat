using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ObjectifFlash;

namespace Fred.Business.ObjectifFlash
{
    /// <summary>
    ///   Gestionnaire des ObjectifFlashs
    /// </summary>
    public class ObjectifFlashTacheManager : Manager<ObjectifFlashTacheEnt, IObjectifFlashTacheRepository>, IObjectifFlashTacheManager
    {
        public ObjectifFlashTacheManager(IUnitOfWork uow, IObjectifFlashTacheRepository objectifFlashLigneRepo)
          : base(uow, objectifFlashLigneRepo)
        {
        }

        #region ObjectifFlashs

        /// <summary>
        ///   Supprime les ressources associées à un Objectif flash.
        /// </summary>
        /// <param name="ofId">L'identifiant du Objectif flash.</param>
        public void DeleteObjectifFlashRessourcesTaches(int ofId)
        {
            this.Repository.DeleteRessourceTaches(ofId);
        }

        /// <summary>
        ///   Supprime les ressources associées à un Objectif flash.
        /// </summary>
        /// <param name="objectifFlash">L'identifiant du Objectif flash.</param>
        public void DeleteObjectifFlashRessourcesTaches(ObjectifFlashEnt objectifFlash)
        {
            DeleteObjectifFlashRessourcesTaches(objectifFlash.ObjectifFlashId);
        }

        #endregion

        #region RessourcesTaches ObjectifFlash


        /// <inheritdoc />
        public ObjectifFlashTacheEnt GetObjectifFlashRessourceTacheById(int objectifFlashRessourceTacheId)
        {
            return Repository.Query()
                                    .Include(c => c.ObjectifFlash)
                                    //.Include(c => c.Ressource)
                                    .Include(c => c.Tache)
                                    .Include(c => c.Unite)
                                    .Filter(c => c.ObjectifFlashId.Equals(objectifFlashRessourceTacheId))
                                    .Get()
                                    .FirstOrDefault();
        }

        /// <inheritdoc />
        public IEnumerable<ObjectifFlashTacheEnt> GetObjectifFlashRessourceTacheList(int objectifFlashId)
        {
            return Repository.Query()
                                    .Include(c => c.Unite)
                                    //.Include(c => c.Ressource)
                                    .Include(c => c.Tache)
                                    //REWORK Data Budget : 
                                    //.Include(c => c.Tache.Unite)
                                    .Include(c => c.ObjectifFlash)
                                    //.Include(c => c.Reporting)
                                    .Include(c => c.Tache.RapportLigneTaches)
                                    .Filter(c => c.ObjectifFlashId == objectifFlashId && !c.ObjectifFlash.DateSuppression.HasValue)
                                    .Get()
                                    .OrderBy(c => c.TacheId).AsEnumerable();
        }

        #endregion

        #region tache rapport realisation

        /// <summary>
        /// Récupère la liste des tache d'objectifs flashs filtrés en de la date, du CI et des Id taches fournis
        /// </summary>
        /// <param name="date">date d'objectif flash</param>
        /// <param name="ciId">id du CI</param>
        /// <param name="rapportId">id de rapport</param>
        /// <param name="tacheIds">liste des tache ids</param>
        /// <returns>liste de taches d'objectifs flashs</returns>
        public IEnumerable<ObjectifFlashEnt> GetObjectifFlashListByDateCiIdAndTacheIds(DateTime date, int ciId, int rapportId, List<int> tacheIds)
        {
            // récupération des réalisations attendues pour le rapport 
            // taches des objectif flashs actifs associés au CI dont la plage de date correspond au rapport
            var allTaches = this.Repository.Query()
                .Include(x => x.ObjectifFlash)
                .Include(x => x.Tache)
                .Include(x => x.Unite)
                .Get()
                .Where(x => tacheIds.Contains(x.TacheId)
                    && x.ObjectifFlash.CiId == ciId
                    && x.ObjectifFlash.DateDebut <= date.Date
                    && x.ObjectifFlash.IsActif
                    && !x.ObjectifFlash.DateCloture.HasValue
                    && !x.ObjectifFlash.DateSuppression.HasValue)
                .ToList();

            // récupération des réalisations existantes pour le rapport
            var tacheRealisations = this.Repository.Query()
                .Include(x => x.ObjectifFlash)
                .Include(x => x.TacheRealisations)
                .Get()
                .SelectMany(x => x.TacheRealisations)
                .Where(x => x.RapportId == rapportId).ToList();

            foreach (var tache in allTaches)
            {
                var relatedTacheRealisations = tacheRealisations.Where(x => x.ObjectifFlashTacheId == tache.ObjectifFlashTacheId).ToList();
                if (relatedTacheRealisations.Any())
                {
                    tache.TacheRealisations = relatedTacheRealisations;
                }
                else
                {
                    tache.TacheRealisations = new List<ObjectifFlashTacheRapportRealiseEnt>
                    {
                        new ObjectifFlashTacheRapportRealiseEnt
                        {
                            ObjectifFlashTacheId = tache.ObjectifFlashTacheId,
                            RapportId = rapportId,
                            DateRealise = date.Date

                        }
                    };
                }
            }
            return allTaches.Select(x => x.ObjectifFlash).Distinct();
        }

        /// <summary>
        /// Met à jour la liste des tache d'objectif flash réalisées pour un rapport
        /// </summary>
        /// <param name="rapportId">ID de rapport</param>
        /// <param name="tacheRealises">liste des taches d'objectif flash réalisées du rapport</param>
        /// <returns>Liste des ObjectifFlashTacheRapportRealise</returns>
        public List<ObjectifFlashTacheRapportRealiseEnt> UpdateObjectifFlashTacheRapportRealise(int rapportId, List<ObjectifFlashTacheRapportRealiseEnt> tacheRealises)
        {
            return this.Repository.UpdateObjectifFlashTacheRapportRealise(rapportId, tacheRealises);
        }
        #endregion tache rapport realisation
    }
}
