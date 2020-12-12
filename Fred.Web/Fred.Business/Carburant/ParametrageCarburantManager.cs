using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Organisation;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Carburant;

namespace Fred.Business.Carburant
{
    /// <summary>
    ///   Gestionnaire du personnel
    /// </summary>
    public class ParametrageCarburantManager : Manager<CarburantOrganisationDeviseEnt, ICarburantOrganisationDeviseRepository>, IParametrageCarburantManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly IDatesClotureComptableManager dateClotureComptableManager;
        private readonly IOrganisationManager organisationManager;

        public ParametrageCarburantManager(
            IUnitOfWork uow,
            ICarburantOrganisationDeviseRepository carburantOrganisationDeviseRepository,
            IParametrageCarburantValidator validator,
            IUtilisateurManager userManager,
            IDatesClotureComptableManager dateClotureComptableManager,
            IOrganisationManager organisationManager)
          : base(uow, carburantOrganisationDeviseRepository, validator)
        {
            this.userManager = userManager;
            this.dateClotureComptableManager = dateClotureComptableManager;
            this.organisationManager = organisationManager;
        }

        /// <inheritdoc />
        public CarburantOrganisationDeviseEnt AddParametrageCarburant(CarburantOrganisationDeviseEnt paramCarburant)
        {
            paramCarburant.DateCreation = DateTime.Now;
            paramCarburant.AuteurCreationId = this.userManager.GetContextUtilisateurId();
            paramCarburant.Periode = paramCarburant.Periode.ToLocalTime();

            BusinessValidation(paramCarburant);

            Repository.AddParametrageCarburant(paramCarburant);
            Save();

            return paramCarburant;
        }

        /// <inheritdoc />
        public void DeleteParametrageCarburant(CarburantOrganisationDeviseEnt paramCarburant)
        {
            paramCarburant.DateSuppression = DateTime.Now;
            paramCarburant.AuteurSuppressionId = this.userManager.GetContextUtilisateurId();

            Repository.UpdateParametrageCarburant(paramCarburant);
            Save();
        }

        /// <inheritdoc />
        public IEnumerable<CarburantOrganisationDeviseEnt> ManageParametrageCarburant(IEnumerable<CarburantOrganisationDeviseEnt> parametrageCarburantList)
        {
            if (parametrageCarburantList != null && parametrageCarburantList.Any())
            {
                foreach (CarburantOrganisationDeviseEnt paramCarburant in parametrageCarburantList.ToList())
                {
                    if (paramCarburant.DateSuppression.HasValue)
                    {
                        DeleteParametrageCarburant(paramCarburant);
                    }
                    else
                    {
                        if (paramCarburant.CarburantOrganisationDeviseId.Equals(0))
                        {
                            AddParametrageCarburant(paramCarburant);
                        }
                        else
                        {
                            UpdateParametrageCarburant(paramCarburant);
                        }
                    }
                }
            }

            return parametrageCarburantList;
        }

        /// <inheritdoc />
        public IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantListByCarburantId(int carburantId)
        {
            return Repository.GetParametrageCarburantListByCarburantId(carburantId);
        }

        /// <inheritdoc />
        public IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantListByOrganisationId(int organisationId)
        {
            return Repository.GetParametrageCarburantListByOrganisationId(organisationId);
        }

        /// <inheritdoc />
        public IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantList(int organisationId, int deviseId)
        {
            return Repository.GetParametrageCarburantList(organisationId, deviseId);
        }

        /// <inheritdoc />
        public IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantList(int carburantId, int organisationId, int deviseId)
        {
            return Repository.GetParametrageCarburantList(carburantId, organisationId, deviseId);
        }

        /// <inheritdoc />
        public CarburantOrganisationDeviseEnt UpdateParametrageCarburant(CarburantOrganisationDeviseEnt paramCarburant)
        {
            paramCarburant.DateModification = DateTime.Now;
            paramCarburant.AuteurModificationId = this.userManager.GetContextUtilisateurId();
            paramCarburant.Periode = paramCarburant.Periode.ToLocalTime();

            BusinessValidation(paramCarburant);

            Repository.UpdateParametrageCarburant(paramCarburant);
            Save();

            return paramCarburant;
        }

        /// <inheritdoc />
        public IEnumerable<CarburantEnt> GetParametrageCarburantListAsCarburantList(int organisationId, int deviseId, DateTime? periode = default(DateTime?))
        {
            List<CarburantEnt> carburants = Repository.GetParametrageCarburantListAsCarburantList(organisationId, deviseId, periode).ToList();

            // Si l'organisation correspond à un CI, on va remplir le champ "Cloturee" de l'entité CarburantOrganisationDeviseEnt (indique que la période est clôturée)
            // à partir des données issues de DateClotureComptable
            var ci = this.organisationManager.GetOrganisationById(organisationId);

            if (ci != null && ci.CI != null)
            {
                carburants.ForEach(x =>
                {
                    x.ParametrageCarburants.ToList().ForEach(cod => cod.Cloture = this.dateClotureComptableManager.IsPeriodClosed(ci.CI.CiId, cod.Periode.Year, cod.Periode.Month));
                });
            }

            return carburants;
        }

        /// <inheritdoc />
        public CarburantOrganisationDeviseEnt GetParametrageCarburant(int carburantId, int organisationId, int deviseId, DateTime? periode = default(DateTime?))
        {
            return Repository.GetParametrageCarburant(carburantId, organisationId, deviseId, periode);
        }
    }
}