using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ObjectifFlash;
using Fred.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;


namespace Fred.DataAccess.ObjectifFlash
{
    /// <summary>
    ///   Représente un référentiel de données pour les Objectifs flash.
    /// </summary>
    public class ObjectifFlashTacheRepository : FredRepository<ObjectifFlashTacheEnt>, IObjectifFlashTacheRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ObjectifFlashRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public ObjectifFlashTacheRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Met à jour la liste des tache d'objectif flash réalisées pour un rapport
        /// </summary>
        /// <param name="rapportId">ID de rapport</param>
        /// <param name="tacheRealises">liste des taches d'objectif flash réalisées du rapport</param>
        /// <returns>Liste des ObjectifFlashTacheRapportRealises</returns>
        public List<ObjectifFlashTacheRapportRealiseEnt> UpdateObjectifFlashTacheRapportRealise(int rapportId, List<ObjectifFlashTacheRapportRealiseEnt> tacheRealises)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                var tacheRealiseesInDB = Context.ObjectifFlashTacheRapportRealise.Where(x => x.RapportId == rapportId).ToList();

                var tacheRealiseesToAdd = tacheRealises.Where(x => x.ObjectifFlashTacheRapportRealiseId == 0);
                var tacheRealiseesToDelete = tacheRealiseesInDB.Where(x => !tacheRealises.Any(y => x.ObjectifFlashTacheRapportRealiseId == y.ObjectifFlashTacheRapportRealiseId));
                var tacheRealiseesToUpdate = tacheRealiseesInDB.Where(x => tacheRealises.Any(y => x.ObjectifFlashTacheRapportRealiseId == y.ObjectifFlashTacheRapportRealiseId) && x.ObjectifFlashTacheRapportRealiseId != 0);

                // Suppression des taches realisées n'existant plus
                Context.ObjectifFlashTacheRapportRealise.RemoveRange(tacheRealiseesToDelete);

                // Ajout des nouvelles taches realisées
                Context.ObjectifFlashTacheRapportRealise.AddRange(tacheRealiseesToAdd);

                // update des taches realisées existantes
                foreach (var tacheRealiseeToUpdate in tacheRealiseesToUpdate)
                {
                    var tacheRealisee = tacheRealises.Single(x => x.ObjectifFlashTacheRapportRealiseId == tacheRealiseeToUpdate.ObjectifFlashTacheRapportRealiseId);
                    Context.Entry(tacheRealiseeToUpdate).CurrentValues.SetValues(tacheRealisee);
                }

                Context.SaveChanges();
                dbContextTransaction.Commit();
            }

            return tacheRealises;
        }

        #region Ressources Taches

        /// <inheritdoc />
        public void DeleteRessourceTaches(int objectifFlashId)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    // On récupére la liste courante.
                    var oldRessourceTaches = Context.ObjectifFlashTache.Where(r => r.ObjectifFlashId == objectifFlashId).ToList();

                    //Suppression des ressources tâches de l'objectif Flash     
                    Context.ObjectifFlashTache.RemoveRange(oldRessourceTaches);
                    Context.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }
        #endregion
    }
}
