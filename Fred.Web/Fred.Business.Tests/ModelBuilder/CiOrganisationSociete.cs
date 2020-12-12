using System;
using Fred.Business.ExplorateurDepense;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Societe;



namespace Fred.Business.Tests.ModelBuilder
{
    internal static class CiOrganisationSociete
    {
        public static OrganisationEnt GetFakeOrganisation()
        {
            return new OrganisationEnt
            {
                OrganisationId = 1
            };
        }


        public static SocieteEnt GetFakeSociete()
        {
            var fakeOrganisation = GetFakeOrganisation();
            return new SocieteEnt
            {
                SocieteId = 1,
                Organisation = fakeOrganisation
            };
        }

        public static CIEnt GetFakeCi()
        {
            var fakeSociete = GetFakeSociete();
            return new CIEnt
            {
                Societe = fakeSociete,
                SocieteId = fakeSociete.SocieteId,
                CiId = 1
            };

        }


        public static UniteEnt GetFakeUnite()
        {
            return new UniteEnt
            {
                Code = "F",
                Libelle = "Fake",
                UniteId = 1
            };
        }

        public static SearchExplorateurDepense GetFakeExplorateurDepensesFiltre()
        {
            return new SearchExplorateurDepense
            {
                PeriodeDebut = DateTime.UtcNow,
                PeriodeFin = DateTime.UtcNow,
                AxeAnalytique = 0,
                AxePrincipal = new string[] { "T1", "T2", "T3" },
                AxeSecondaire = new string[] { "Chapitre", "SousChapitre", "Ressource" }
            };
        }
    }

}
