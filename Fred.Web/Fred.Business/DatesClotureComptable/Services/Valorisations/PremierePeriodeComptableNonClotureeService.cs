using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.DatesClotureComptable;

namespace Fred.Business.DatesClotureComptable.Services.Valorisations
{
    public class PremierePeriodeComptableNonClotureeService : IPremierePeriodeComptableNonClotureeService
    {

        private readonly IDatesClotureComptableRepository datesClotureComptableRepository;
        private readonly ICIRepository ciRepository;

        public PremierePeriodeComptableNonClotureeService(ICIRepository ciRepository, IDatesClotureComptableRepository datesClotureComptableRepository)
        {
            this.datesClotureComptableRepository = datesClotureComptableRepository;
            this.ciRepository = ciRepository;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////// RECUPERTAION-CREATION DE TOUTES LES PREMIERES PERIODES COMPTABLES NON CLOTUREES/////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public List<CiDernierePeriodeComptableNonCloturee> GetPremierePeriodeComptableNonCloturees(List<int> ciIds, DateTime periode)
        {
            List<CiDateOuvertureDateFermeture> ciDateOuvertureDateFermetures = ciRepository.GetDateOuvertureFermeturesCis(ciIds);
            List<DatesClotureComptableEnt> lastDatesCompatbles = datesClotureComptableRepository.GetLastDateClotures(ciIds);

            List<CiDernierePeriodeComptableNonCloturee> result = new List<CiDernierePeriodeComptableNonCloturee>();

            foreach (int ciId in ciIds)
            {
                var ciDernierePeriodeComptableCloturee = CreateDernierePeriodeComptableClotureeForCi(ciId, ciDateOuvertureDateFermetures, lastDatesCompatbles, periode);
                result.Add(ciDernierePeriodeComptableCloturee);
            }

            return result;
        }

        private CiDernierePeriodeComptableNonCloturee CreateDernierePeriodeComptableClotureeForCi(int ciId, List<CiDateOuvertureDateFermeture> ciDateOuvertureDateFermetures, List<DatesClotureComptableEnt> lastDatesCompatbles, DateTime periode)
        {
            CiDernierePeriodeComptableNonCloturee ciDernierePeriodeComptableCloturee = new CiDernierePeriodeComptableNonCloturee();
            ciDernierePeriodeComptableCloturee.CiId = ciId;

            var dateComptableOfCi = lastDatesCompatbles.FirstOrDefault(x => x.CiId == ciId);

            if (dateComptableOfCi != null)
            {
                DateTime dernierePeriodeComptableCloturee = new DateTime(dateComptableOfCi.Annee, dateComptableOfCi.Mois, 1);
                ciDernierePeriodeComptableCloturee.DernierePeriodeComptableNonCloturee = dernierePeriodeComptableCloturee.AddMonths(1);
            }
            else
            {
                DateTime? dateOuvertureCi = ciDateOuvertureDateFermetures.FirstOrDefault(x => x.CiId == ciId)?.DateOuverture;
                if (dateOuvertureCi.HasValue)
                {
                    ciDernierePeriodeComptableCloturee.DernierePeriodeComptableNonCloturee = dateOuvertureCi.Value;
                }
                else
                {
                    ciDernierePeriodeComptableCloturee.DernierePeriodeComptableNonCloturee = periode;
                }
            }
            return ciDernierePeriodeComptableCloturee;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////// DETERMINATION DE LA PREMIERE PERIODE COMPTABLE NON CLOTUREE POUR UN CI /////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public DateTime GetPremierePeriodeComptableNonClotureeByCiId(List<CiDernierePeriodeComptableNonCloturee> premierePeriodeComptableNonCloturees, int ciId)
        {
            return premierePeriodeComptableNonCloturees.First(x => x.CiId == ciId).DernierePeriodeComptableNonCloturee;
        }
    }

}
