using System.Collections.Generic;
using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.Tests.RepriseDonnees
{
    public static class CommandeFakeDataProvider
    {
        public static RepriseExcelCommande GetRepriseExcelCommande_RB_123456_SIGN0026_1()
        {
            return new RepriseExcelCommande
            {
                CodeCi = "123456",
                CodeDevise = "EUR",
                CodeFournisseur = "SIGN0026",
                CodeRessource = "ENCA-35",
                CodeSociete = "RB",
                CodeTache = "",
                DateCommande = "31/03/2019",
                DateReception = "31/03/2019",
                DesignationLigneCommande = "Divers ENCA-35…",
                Far = "=(P3-Q3)*N3",
                LibelleEnteteCommande = "Solde commandes FRN SIGN0026…",
                NumeroCommandeExterne = "RB-123456-SIGN0026",
                NumeroDeLigne = "1",
                PuHt = "1",
                QuantiteCommandee = "1000",
                QuantiteFactureeRapprochee = "",
                QuantiteReceptionnee = "450",
                TypeCommande = "Fourniture",
                Unite = "FRT"
            };
        }

        public static RepriseExcelCommande GetRepriseExcelCommande_RB_123456_SIGN0026_2()
        {
            return new RepriseExcelCommande
            {
                CodeCi = "123456",
                CodeDevise = "EUR",
                CodeFournisseur = "SIGN0026",
                CodeRessource = "STPE-26",
                CodeSociete = "RB",
                CodeTache = "",
                DateCommande = "31/03/2019",
                DateReception = "31/03/2019",
                DesignationLigneCommande = "Divers STPE-26…",
                Far = "=(P4-Q4)*N4",
                LibelleEnteteCommande = "Solde commandes FRN SIGN0026…",
                NumeroCommandeExterne = "RB-123456-SIGN0026",
                NumeroDeLigne = "2",
                PuHt = "1",
                QuantiteCommandee = "2000",
                QuantiteFactureeRapprochee = "",
                QuantiteReceptionnee = "",
                TypeCommande = "Fourniture",
                Unite = "FRT"
            };
        }

        public static RepriseExcelCommande GetRepriseExcelCommande_RB_123456_SIGN0026_3_INCOHERENTE()
        {
            return new RepriseExcelCommande
            {
                CodeCi = "1234567",//incoherent
                CodeDevise = "EUR",
                CodeFournisseur = "SIGN0026",
                CodeRessource = "STPE-26",
                CodeSociete = "RB",
                CodeTache = "",
                DateCommande = "31/03/2019",
                DateReception = "31/03/2019",
                DesignationLigneCommande = "Divers STPE-26…",
                Far = "=(P4-Q4)*N4",
                LibelleEnteteCommande = "Solde commandes FRN SIGN0026…",
                NumeroCommandeExterne = "RB-123456-SIGN0026",
                NumeroDeLigne = "4",
                PuHt = "1",
                QuantiteCommandee = "2000",
                QuantiteFactureeRapprochee = "",
                QuantiteReceptionnee = "",
                TypeCommande = "Fourniture",
                Unite = "FRT"
            };
        }

        public static RepriseExcelCommande GetRepriseExcelCommande_RB_123456_SIGN0026_4_REQUIRED_FIELD_ERROR()
        {
            return new RepriseExcelCommande
            {
                CodeCi = "",//required
                CodeDevise = "",//required
                CodeFournisseur = "",//required
                CodeRessource = "",//required
                CodeSociete = "",//required
                CodeTache = "",
                DateCommande = null,//required
                DateReception = "",//required
                DesignationLigneCommande = "",//required
                Far = "=(P4-Q4)*N4",
                LibelleEnteteCommande = "",//required
                NumeroCommandeExterne = "",//required
                NumeroDeLigne = "5",
                PuHt = "",//required
                QuantiteCommandee = "",//required
                QuantiteFactureeRapprochee = "",
                QuantiteReceptionnee = "",
                TypeCommande = "Fourniture",
                Unite = null//required
            };
        }

        public static RepriseExcelCommande GetRepriseExcelCommande_MOU_654321_CFR00001_1()
        {
            return new RepriseExcelCommande
            {
                CodeCi = "654321",
                CodeDevise = "EUR",
                CodeFournisseur = "CFR00001",
                CodeRessource = "PCR-02",
                CodeSociete = "MOU",
                CodeTache = "",
                DateCommande = "31/03/2019",
                DateReception = "31/03/2019",
                DesignationLigneCommande = "Divers PCR-02…",
                Far = "=(P5-Q5)*N5",
                LibelleEnteteCommande = "Solde commandes FRN CFR00001…",
                NumeroCommandeExterne = "MOU-654321-CFR00001",
                NumeroDeLigne = "3",
                PuHt = "1",
                QuantiteCommandee = "3000",
                QuantiteFactureeRapprochee = "1400",
                QuantiteReceptionnee = "2100",
                TypeCommande = "Prestation",
                Unite = "FRT"
            };
        }

        public static List<RepriseExcelCommande> CreateRepriseExcelCommandeExcelRows()
        {
            return new List<RepriseExcelCommande>() {

                GetRepriseExcelCommande_RB_123456_SIGN0026_1(),
                GetRepriseExcelCommande_RB_123456_SIGN0026_2(),
                GetRepriseExcelCommande_MOU_654321_CFR00001_1()
            };
        }


        public static Entities.Organisation.Tree.OrganisationTree OrganisationTree()
        {


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

            return orgaTree;
        }


    }
}
