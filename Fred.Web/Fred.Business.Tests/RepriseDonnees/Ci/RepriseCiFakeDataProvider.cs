using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.Tests.RepriseDonnees
{
    public static class RepriseCiFakeDataProvider
    {
        public static RepriseExcelCi GetExcelCiRzb()
        {
            return new RepriseExcelCi()
            {
                NumeroDeLigne = "1",
                CodeSociete = OrganisationTreeMocks.CODE_SOCIETE_RZB,
                CodeCi = OrganisationTreeMocks.CODE_CI_411100_SOCIETE_RZB,
                Adresse = "place de la mairie",
                Adresse2 = "",
                Adresse3 = "",
                Ville = "Toulouse",
                CodePostal = "31000",
                CodePays = "FR",
                EnteteLivraison = "Chantier de renovation",
                AdresseLivraison = "place de la mairie de toulouse",
                CodePostalLivraison = "31001",
                VilleLivraison = "Toulouse",
                CodePaysLivraison = "FR",
                AdresseFacturation = "centre facturation place de la mairie",
                CodePostalFacturation = "31002",
                VilleFacturation = "Toulouse",
                CodePaysFacturation = "FR",
                MatriculeResponsableChantier = "001",
                MatriculeResponsableAdministratif = "002",
                ZoneModifiable = "n",
                DateOuverture = "31/12/2018"
            };
        }

        public static RepriseExcelCi GetExcelCiBianco()
        {
            return new RepriseExcelCi()
            {
                NumeroDeLigne = "2",
                CodeSociete = OrganisationTreeMocks.CODE_SOCIETE_BIANCO,
                CodeCi = "411100",
                Adresse = "place de la mairie",
                Adresse2 = "",
                Adresse3 = "",
                Ville = "Toulouse",
                CodePostal = "31000",
                CodePays = "FR",
                EnteteLivraison = "Chantier de renovation",
                AdresseLivraison = "place de la mairie de toulouse",
                CodePostalLivraison = "31001",
                VilleLivraison = "Toulouse",
                CodePaysLivraison = "FR",
                AdresseFacturation = "centre facturation place de la mairie",
                CodePostalFacturation = "31002",
                VilleFacturation = "Toulouse",
                CodePaysFacturation = "FR",
                MatriculeResponsableChantier = "001",
                MatriculeResponsableAdministratif = "002",
                ZoneModifiable = "n",
                DateOuverture = "31/12/2018"
            };
        }

        public static List<RepriseExcelCi> CreateExcelRows()
        {
            return new List<RepriseExcelCi>() {

                GetExcelCiRzb()
            };
        }

        public static List<RepriseExcelCi> CreateExcelRowsWith2CiWithSameCode()
        {
            return new List<RepriseExcelCi>() {

               GetExcelCiRzb(),
               GetExcelCiBianco()
            };
        }

        public static List<RepriseExcelCi> CreateEmptyExcelRows()
        {
            return new List<RepriseExcelCi>() {

                new RepriseExcelCi()
                {
                    NumeroDeLigne = "",
                    CodeSociete = "RB",
                    CodeCi = "411100",
                    Adresse = "",
                    Adresse2 = "",
                    Adresse3 = "",
                    Ville = "",
                    CodePostal = "",
                    CodePays = "",
                    EnteteLivraison = "",
                    AdresseLivraison = "",
                    CodePostalLivraison = "",
                    VilleLivraison = "",
                    CodePaysLivraison = "",
                    AdresseFacturation = "",
                    CodePostalFacturation = "",
                    VilleFacturation = "",
                    CodePaysFacturation = "",
                    MatriculeResponsableChantier = "",
                    MatriculeResponsableAdministratif = "",
                    ZoneModifiable = "",
                    DateOuverture = "31/12/2018"
                }
            };
        }



        private static List<Entities.Referential.PaysEnt> CreatePays()
        {
            return new List<Entities.Referential.PaysEnt>
                {
                    new Entities.Referential.PaysEnt
                    {
                        Code = "FR",
                        Libelle = "FRANCE",
                        PaysId = 1,
                        Personnels = null
                    },
                    new Entities.Referential.PaysEnt
                    {
                        Code = "MQ",
                        Libelle = "MARTINIQUE",
                        PaysId = 4,
                        Personnels = null
                    }
                };
        }

        private static List<Entities.Personnel.PersonnelEnt> CreatePersonnels()
        {
            return new List<Entities.Personnel.PersonnelEnt>
            {
                new Entities.Personnel.PersonnelEnt
                {
                    PersonnelId = 1191,
                    Matricule = "001",
                    Nom = "LAUNAY",
                    SocieteId = OrganisationTreeMocks.SOCIETE_ID_RZB,

                },
                 new Entities.Personnel.PersonnelEnt
                {
                    PersonnelId = 119,
                    Matricule = "002",
                    Nom = "JOURDES",
                    SocieteId = OrganisationTreeMocks.SOCIETE_ID_RZB,

                }
            };
        }

        private static List<CIEnt> CreateFirstCi()
        {
            return new List<CIEnt>()
                                                    {
                                                       new CIEnt
                                                        {
                                                            Adresse = "Adresse (1ère ligne)",
                                                            Adresse2 = "Adresse (2ème ligne)",
                                                            Adresse3 = "Adresse (3ème ligne)",
                                                            AdresseFacturation = "",
                                                            AdresseLivraison = "",
                                                            Affectations = null,
                                                            CiId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB,
                                                            Code = "411100",
                                                            CodeExterne = null,
                                                            CodePostal = "01234567890123456789",
                                                            CodePostalFacturation = "",
                                                            CodePostalLivraison = "",
                                                            EnteteLivraison = "",


                                                            Pays = null,
                                                            PaysFacturation = null,
                                                            PaysFacturationId = null,
                                                            PaysId = 4,
                                                            PaysLivraison = null,
                                                            PaysLivraisonId = null,

                                                            ResponsableAdministratif = null,
                                                            ResponsableAdministratifId = null,
                                                            ResponsableChantier = "RICHARD LAUNAY",
                                                            Societe = null,
                                                            SocieteId = 1,

                                                            Ville = "Ville",
                                                            VilleFacturation = "",
                                                            VilleLivraison = "",
                                                            ZoneModifiable = false,

                                                        }
                                                    };
        }

        private static CIEnt CreateCiForBiancoWithSameCodeAsCiRzb()
        {
            return new CIEnt
            {
                Adresse = "Adresse (1ère ligne)",
                Adresse2 = "Adresse (2ème ligne)",
                Adresse3 = "Adresse (3ème ligne)",
                AdresseFacturation = "",
                AdresseLivraison = "",
                Affectations = null,
                CiId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_BIANCO,
                Code = "411100",
                CodeExterne = null,
                CodePostal = "01234567890123456789",
                CodePostalFacturation = "",
                CodePostalLivraison = "",
                EnteteLivraison = "",


                Pays = null,
                PaysFacturation = null,
                PaysFacturationId = null,
                PaysId = 4,
                PaysLivraison = null,
                PaysLivraisonId = null,

                ResponsableAdministratif = null,
                ResponsableAdministratifId = null,
                ResponsableChantier = "RICHARD LAUNAY",
                Societe = null,
                SocieteId = 1,

                Ville = "Ville",
                VilleFacturation = "",
                VilleLivraison = "",
                ZoneModifiable = false,

            };


        }

        public static ContextForImportCi CreateContext()
        {
            List<CIEnt> cisUsedInExcelWithSameCodes = CreateFirstCi();

            List<Entities.Referential.PaysEnt> paysUsedInExcelWithSameCodes = CreatePays();

            List<Entities.Personnel.PersonnelEnt> personnelsUsedInExcelWithSameMatricules = CreatePersonnels();

            var SocietesOfGroupe = new List<OrganisationBase>
            {
                 OrganisationTreeMocks.GetSocieteRzb(),
                 OrganisationTreeMocks.GetSocieteBianco()
            };

            var oranisations = new List<OrganisationBase>
                {
                    OrganisationTreeMocks.GetHoldingFayat(),
                    OrganisationTreeMocks.GetGroupeRzb(),
                    OrganisationTreeMocks.GetSocieteRzb(),
                    OrganisationTreeMocks.GetSocieteBianco(),
                    OrganisationTreeMocks.GetCi_411100_SocieteRzb()
                };

            var orgaTree = new Entities.Organisation.Tree.OrganisationTree(oranisations);

            ContextForImportCi context = new ContextForImportCi()
            {
                CisUsedInExcel = cisUsedInExcelWithSameCodes,
                PaysUsedInExcel = paysUsedInExcelWithSameCodes,
                PersonnelsUsedInExcel = personnelsUsedInExcelWithSameMatricules,
                SocietesOfGroupe = SocietesOfGroupe,
                OrganisationTree = orgaTree

            };
            return context;
        }


        public static ContextForImportCi CreateComplexeContext()
        {
            List<CIEnt> cisUsedInExcelWithSameCodes = CreateFirstCi();

            cisUsedInExcelWithSameCodes.Add(CreateCiForBiancoWithSameCodeAsCiRzb());// diff with normal context

            List<Entities.Referential.PaysEnt> paysUsedInExcelWithSameCodes = CreatePays();

            List<Entities.Personnel.PersonnelEnt> personnelsUsedInExcelWithSameMatricules = CreatePersonnels();

            var SocietesOfGroupe = new List<OrganisationBase>
            {
                  OrganisationTreeMocks.GetSocieteRzb(),
                 OrganisationTreeMocks.GetSocieteBianco()
            };

            var oranisations = new List<OrganisationBase>
                {
                    OrganisationTreeMocks.GetHoldingFayat(),
                    OrganisationTreeMocks.GetGroupeRzb(),
                    OrganisationTreeMocks.GetSocieteRzb(),
                    OrganisationTreeMocks.GetSocieteBianco(),
                    OrganisationTreeMocks.GetCi_411100_SocieteRzb(),
                   OrganisationTreeMocks.GetCi_411100_SocieteBianco() // diff with normal context
                };

            var orgaTree = new OrganisationTree(oranisations);

            ContextForImportCi context = new ContextForImportCi()
            {
                CisUsedInExcel = cisUsedInExcelWithSameCodes,
                PaysUsedInExcel = paysUsedInExcelWithSameCodes,
                PersonnelsUsedInExcel = personnelsUsedInExcelWithSameMatricules,
                SocietesOfGroupe = SocietesOfGroupe,
                OrganisationTree = orgaTree

            };
            return context;
        }
    }
}
