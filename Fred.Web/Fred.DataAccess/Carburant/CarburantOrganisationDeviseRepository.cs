using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Carburant;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Carburant
{
    /// <summary>
    ///   Référentiel de données pour les Paramétrage Carburant.
    /// </summary>
    public class CarburantOrganisationDeviseRepository : FredRepository<CarburantOrganisationDeviseEnt>, ICarburantOrganisationDeviseRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CarburantOrganisationDeviseRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public CarburantOrganisationDeviseRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc />
        public CarburantOrganisationDeviseEnt GetParametrageCarburant(int carburantId, int organisationId, int deviseId, DateTime? periode = default(DateTime?))
        {
            var query = Query()
                          .Include(x => x.Carburant.Unite)
                          .Include(x => x.Organisation)
                          .Include(x => x.Devise)
                          .Filter(x => x.CarburantId.Equals(carburantId) &&
                                       x.OrganisationId.Equals(organisationId) &&
                                       x.DeviseId.Equals(deviseId) &&
                                       !x.DateSuppression.HasValue);

            if (periode != null)
            {
                query = query.Filter(x => x.Periode.Month.Equals(periode.Value.Month) && x.Periode.Year.Equals(periode.Value.Year));
            }

            return query.Get().AsNoTracking().FirstOrDefault();
        }


        /// <inheritdoc />
        public CarburantOrganisationDeviseEnt AddParametrageCarburant(CarburantOrganisationDeviseEnt paramCarburant)
        {
            DetachDependencies(paramCarburant);
            Insert(paramCarburant);

            return paramCarburant;
        }

        /// <inheritdoc />
        public IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantListByCarburantId(int carburantId)
        {
            var query = Query()
                          .Include(x => x.Carburant.Unite)
                          .Include(x => x.Organisation)
                          .Include(x => x.Devise)
                          .Filter(x => x.CarburantId.Equals(carburantId) &&
                                      !x.DateSuppression.HasValue);

            return query.Get().AsNoTracking();
        }

        /// <inheritdoc />
        public IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantListByOrganisationId(int organisationId)
        {
            var query = Query()
                          .Include(x => x.Carburant.Unite)
                          .Include(x => x.Organisation)
                          .Include(x => x.Devise)
                          .Filter(x => x.OrganisationId.Equals(organisationId) &&
                                      !x.DateSuppression.HasValue);

            return query.Get().AsNoTracking();
        }

        /// <inheritdoc />
        public IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantList(int organisationId, int deviseId)
        {
            var query = Query()
                          .Include(x => x.Carburant.Unite)
                          .Include(x => x.Organisation)
                          .Include(x => x.Devise)
                          .Filter(x => x.OrganisationId.Equals(organisationId) &&
                                       x.DeviseId.Equals(deviseId) &&
                                       !x.DateSuppression.HasValue);

            return query.Get().AsNoTracking();
        }

        /// <inheritdoc />
        public IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantList(int carburantId, int organisationId, int deviseId)
        {
            var query = Query()
                          .Include(x => x.Carburant.Unite)
                          .Include(x => x.Organisation)
                          .Include(x => x.Devise)
                          .Filter(x => x.CarburantId.Equals(carburantId) &&
                                       x.OrganisationId.Equals(organisationId) &&
                                       x.DeviseId.Equals(deviseId) &&
                                       !x.DateSuppression.HasValue);

            return query.Get().AsNoTracking();
        }

        /// <inheritdoc />
        public CarburantOrganisationDeviseEnt UpdateParametrageCarburant(CarburantOrganisationDeviseEnt paramCarburant)
        {
            DetachDependencies(paramCarburant);
            Update(paramCarburant);

            return paramCarburant;
        }

        /// <inheritdoc />
        public IEnumerable<CarburantEnt> GetParametrageCarburantListAsCarburantList(int organisationId, int deviseId, DateTime? periode = default(DateTime?))
        {
            var carburants = Context.Carburants
                              .Include(x => x.ParametrageCarburants.Select(d => d.Devise))
                              .Include(x => x.Unite)
                              .AsNoTracking()
                              .ToList();
            if (periode != null)
            {
                carburants.ForEach(x => x.ParametrageCarburants = x.ParametrageCarburants.Where(c => c.OrganisationId.Equals(organisationId) &&
                                                                                                     c.DeviseId.Equals(deviseId) &&
                                                                                                     !c.DateSuppression.HasValue &&
                                                                                                     c.Periode.Month.Equals(periode.Value.Month) &&
                                                                                                     c.Periode.Year.Equals(periode.Value.Year)).ToList());
            }
            else
            {
                carburants.ForEach(x => x.ParametrageCarburants = x.ParametrageCarburants.Where(c => c.OrganisationId.Equals(organisationId) && c.DeviseId.Equals(deviseId) && !c.DateSuppression.HasValue).ToList());
            }

            return carburants;
        }

        /// <summary>
        ///   Mise à Null des objets imbriqués
        /// </summary>
        /// <param name="paramCarburant">CarburantOrganisationDeviseEnt</param>
        private void DetachDependencies(CarburantOrganisationDeviseEnt paramCarburant)
        {
            paramCarburant.Devise = null;
            paramCarburant.Organisation = null;
            paramCarburant.Carburant = null;
        }
    }

}