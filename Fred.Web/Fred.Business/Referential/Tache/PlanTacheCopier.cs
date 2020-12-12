using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using CommonServiceLocator;

namespace Fred.Business.Referential.Tache
{
    /// <summary>
    /// Permet de copier un plan de tâche d'un CI à un autre.
    /// </summary>
    public class PlanTacheCopier : ManagersAccess
    {
        private readonly IUnitOfWork uow;
        private readonly ITacheRepository tacheRepository;
        private readonly ITacheManager tacheManager;
        private List<TacheEnt> tachesCible;

        /// <summary>
        /// Constructeur.
        /// </summary>
        public PlanTacheCopier(IUnitOfWork unitOfWork, ITacheRepository tacheRepository, ITacheManager tacheManager)
        {
            uow = unitOfWork;
            this.tacheRepository = tacheRepository;
            this.tacheManager = tacheManager;
        }

        /// <summary>
        /// Copie.
        /// </summary>
        public void Copy(int ciIdSource, int ciIdCible)
        {
            var tachesSource = tacheManager.GetAllT1ByCiId(ciIdSource, null).ToList();
            tachesCible = tacheManager.GetTacheListByCiId(ciIdCible, true).ToList();
            Copy(ciIdCible, tachesSource, null);
            uow.Save();
        }

        /// <summary>
        /// Copie les tâches sources dans la tâche cible parente.
        /// </summary>
        /// <param name="tachesSource">Les tâches sources concernées.</param>
        /// <param name="tacheCibleParent">La tâche cible parente concernée, peut-être null.</param>
        private void Copy(int ciIdCible, IEnumerable<TacheEnt> tachesSource, TacheEnt tacheCibleParent)
        {
            if (tachesSource == null)
            {
                return;
            }

            foreach (var tacheSource in tachesSource)
            {
                // Regarde si le code de la tâche source existe dans le CI cible
                var tacheCible = tachesCible.FirstOrDefault(t => t.Code == tacheSource.Code);
                if (tacheCible != null)
                {
                    // Le code de la tâche source existe dans le CI cible
                    ProcessExistingTache(ciIdCible, tacheSource, tacheCible);
                }
                else
                {
                    // Le code de la tâche source n'existe pas dans le CI cible
                    // Dans ce cas on crée une copie de la tache source dans le CI cible
                    ProcessNewTache(ciIdCible, tacheSource, tacheCibleParent);
                }
            }
        }

        /// <summary>
        /// Traite une tâche dont le code existe dans le CI source et dans le CI cible.
        /// </summary>
        /// <param name="tacheSource">La tâche source.</param>
        /// <param name="tacheCible">La tâche cible correspondante.</param>
        private void ProcessExistingTache(int ciIdCible, TacheEnt tacheSource, TacheEnt tacheCible)
        {
            // Le niveau de la tâche source doit être le même que celui de la tâche cible, sinon on ne fait rien
            // De plus on ne traite pas les tâches de niveau 4
            if (tacheCible.Niveau == tacheSource.Niveau && tacheSource.Niveau < 3)
            {
                // Les tâches sont de même niveau
                // Dans ce cas on considère que la tâche cible correspond à la tâche source
                Copy(ciIdCible, tacheSource.TachesEnfants, tacheCible);
            }
        }

        /// <summary>
        /// Traite une tâche dont le code n'existe pas dans le CI cible.
        /// </summary>
        /// <param name="tacheSource">La tâche source.</param>
        /// <param name="tacheCibleParent">La tâche cible parente.</param>
        private void ProcessNewTache(int ciIdCible, TacheEnt tacheSource, TacheEnt tacheCibleParent)
        {
            var nextTacheCibleParent = CloneTache(ciIdCible, tacheSource, tacheCibleParent);

            if (tacheCibleParent == null)
            {
                // Il s'agit d'une tâche de niveau 1 qui n'a donc pas de parent, on l'insert
                tacheRepository.Insert(nextTacheCibleParent);
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
                Copy(ciIdCible, tacheSource.TachesEnfants, nextTacheCibleParent);
            }
        }

        /// <summary>
        /// Clone une tâche.
        /// </summary>
        /// <param name="tacheSource">La tâche à cloner.</param>
        /// <param name="tacheParent">La tâche parente de la tâche clonée.</param>
        /// <returns>La tâche clonée.</returns>
        private TacheEnt CloneTache(int ciIdCible, TacheEnt tacheSource, TacheEnt tacheParent)
        {
            return new TacheEnt
            {
                TacheId = 0,
                Code = tacheSource.Code,
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
    }
}
