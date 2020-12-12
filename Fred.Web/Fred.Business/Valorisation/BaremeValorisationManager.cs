using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Entities.Valorisation;
using Fred.Framework.Exceptions;

namespace Fred.Business.Valorisation
{
    public class BaremeValorisationManager : Manager<ValorisationEnt, IValorisationRepository>, IBaremeValorisationManager
    {
        private readonly Semaphore semaphore = new Semaphore(1, 1);


        private readonly IPointageRepository pointageRepository;
        private readonly IValorisationRepository valorisationRepository;
        private readonly IValorisationManager valorisationManager;

        public BaremeValorisationManager(IUnitOfWork uow,
            IValorisationRepository valorisationRepository,
            IPointageRepository pointageRepository,
            IValorisationManager valorisationManager)
            : base(uow, valorisationRepository)
        {

            this.pointageRepository = pointageRepository;
            this.valorisationRepository = valorisationRepository;
            this.valorisationManager = valorisationManager;
        }

        public void NewValorisationJobBareme(int objectId, Action<int, DateTime, DateTime> procedureToExecute, DateTime startPeriode, DateTime baremePeriode)
        {
            StartTask(() => procedureToExecute(objectId, startPeriode, baremePeriode));
        }

        private void StartTask(System.Action procedureToExecute)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    semaphore.WaitOne();
                    procedureToExecute();
                }
                catch (Exception e)
                {
                    throw new FredBusinessException(e.Message, e);
                }
                finally
                {
                    semaphore.Release();

                }
            });
        }

        public void UpdateValorisationFromBaremeCI(int ciId, DateTime startPeriode, DateTime baremePeriode)
        {
            // Suppresion des lignes de valorisations existantes pour la période
            DeleteValorisationByCiIdFromPeriode(ciId, startPeriode);

            // Insertion des lignes en fonction des pointages de la période
            RefreshValorisationFromBaremeCI(ciId, startPeriode, "Bareme", baremePeriode);
        }

        private void RefreshValorisationFromBaremeCI(int ciId, DateTime startPeriode, string source, DateTime baremePeriode)
        {
            List<RapportLigneEnt> pointages = pointageRepository.GetListPointagesReelsByCiIdFromPeriode(ciId, startPeriode).Where(o =>
           !(o.PersonnelId.HasValue && o.Personnel.IsInterimaire)
           && !(o.MaterielId.HasValue && o.Materiel.MaterielLocation)).ToList();
            valorisationManager.CreateValorisation(pointages, source, baremePeriode);
            Save();
        }

        /// <summary>
        /// Supprime les valorisations en fonction des paramètres
        /// </summary>
        /// <param name="ciId">identifiant du Ci</param>
        /// <param name="periode">Période</param>
        private void DeleteValorisationByCiIdFromPeriode(int ciId, DateTime periode)
        {
            IReadOnlyList<ValorisationEnt> valorisations = valorisationRepository.GetValorisationWithoutInterimaireOrMaterielExterneFromCiIdAfterPeriodeWithQuantityGreaterThanZeroAndWithoutLockPeriod(ciId, periode);
            valorisationRepository.DeleteValorisations(valorisations.ToList());
        }
    }
}