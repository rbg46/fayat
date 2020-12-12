using System.Collections.Generic;
using Fred.Entities;
using Fred.Entities.Organisation.Tree;
using Fred.Framework.Extensions;

namespace Fred.Common.Tests.Data.Organisation.Tree.Mock
{
    public static class OrganisationTreeMocks
    {
        // holding
        public const int ORGANISATION_ID_HOLDING_RZB = 1;

        // groupes  - type 3
        public const int ORGANISATION_ID_GROUPE_RZB = 301;
        public const int ORGANISATION_ID_GROUPE_FES = 302;

        public const int GROUPE_ID_RZB = 351;
        public const int GROUPE_ID_FES = 352;

        public const string CODE_GROUPE_RZB = "GRZB";
        public const string CODE_GROUPE_FES = "GFES";

        // societes - type 4
        public const int ORGANISATION_ID_SOCIETE_RZB = 401;
        public const int ORGANISATION_ID_SOCIETE_BIANCO = 402;
        public const int ORGANISATION_ID_SOCIETE_SATELEC = 403;

        public const int SOCIETE_ID_RZB = 451;
        public const int SOCIETE_ID_BIANCO = 452;
        public const int SOCIETE_ID_SATELEC = 453;

        public const string CODE_SOCIETE_RZB = "RB";
        public const string CODE_SOCIETE_BIANCO = "BIAN";
        public const string CODE_SOCIETE_SATELEC = "STATELEC";


        // etablissement  - type 7
        public const int ORGANISATION_ID_ETABLISSEMENT_BIANCO_RZB = 701;

        public const int ETABLISSEMENT_ID_BIANCO_RZB = 751;

        public const string CODE_ETABLISSEMENT_BIANCO_RZB = "00";




        // CI
        public const int ORGANISATION_ID_CI_411100_SOCIETE_RZB = 801;
        public const int ORGANISATION_ID_CI_411100_SOCIETE_BIANCO = 802;
        public const int ORGANISATION_ID_CI_411100_SOCIETE_SATELEC = 803;

        public const int CI_ID_411100_SOCIETE_RZB = 851;
        public const int CI_ID_411100_SOCIETE_BIANCO = 852;
        public const int CI_ID_411100_SOCIETE_SATELEC = 853;

        public const string CODE_CI_411100_SOCIETE_RZB = CODE_411100;
        public const string CODE_CI_411100_SOCIETE_BIANCO = CODE_411100;
        public const string CODE_411100 = "411100";

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////    HOLDING
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public static OrganisationBase GetHoldingFayat()
        {
            return new OrganisationBase
            {
                Code = "FSA",
                Id = 1,
                Libelle = "FAYAT SA",
                OrganisationId = ORGANISATION_ID_HOLDING_RZB,
                PereId = null,
                TypeOrganisationId = OrganisationType.Holding.ToIntValue()
            };
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////    GROUPES
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static OrganisationBase GetGroupeRzb()
        {
            return new OrganisationBase
            {
                Code = CODE_GROUPE_RZB,
                Id = GROUPE_ID_RZB,
                Libelle = "GROUPE RAZEL-BEC",
                OrganisationId = ORGANISATION_ID_GROUPE_RZB,
                PereId = 1,
                TypeOrganisationId = OrganisationType.Groupe.ToIntValue()
            }; ;
        }

        public static OrganisationBase GetGroupeFES()
        {
            return new OrganisationBase
            {
                Code = CODE_GROUPE_FES,
                Id = GROUPE_ID_FES,
                Libelle = "GROUPE FAYAT ENERGIE SERVICE",
                OrganisationId = ORGANISATION_ID_GROUPE_FES,
                PereId = 1,
                TypeOrganisationId = OrganisationType.Groupe.ToIntValue()
            };
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////    SOCIETES
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static OrganisationBase GetSocieteRzb()
        {
            return new OrganisationBase
            {
                Code = CODE_SOCIETE_RZB,
                Id = SOCIETE_ID_RZB,
                Libelle = "RAZEL-BEC",
                OrganisationId = ORGANISATION_ID_SOCIETE_RZB,
                PereId = ORGANISATION_ID_GROUPE_RZB,
                TypeOrganisationId = OrganisationType.Societe.ToIntValue()
            };
        }

        public static OrganisationBase GetSocieteBianco()
        {
            return new OrganisationBase
            {
                Code = CODE_SOCIETE_BIANCO,
                Id = SOCIETE_ID_BIANCO,
                Libelle = "BIANCO",
                OrganisationId = ORGANISATION_ID_SOCIETE_BIANCO,
                PereId = ORGANISATION_ID_GROUPE_RZB,
                TypeOrganisationId = OrganisationType.Societe.ToIntValue()
            };

        }

        public static OrganisationBase GetSocieteSatelec()
        {
            return new OrganisationBase
            {
                Code = CODE_SOCIETE_SATELEC,
                Id = SOCIETE_ID_SATELEC,
                Libelle = "SATELEC",
                OrganisationId = ORGANISATION_ID_SOCIETE_SATELEC,
                PereId = ORGANISATION_ID_GROUPE_FES,
                TypeOrganisationId = OrganisationType.Societe.ToIntValue()
            };

        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////    ETABLISSEMENT
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static OrganisationBase GetEtablissementBiancoRzb()
        {
            return new OrganisationBase
            {
                Code = CODE_ETABLISSEMENT_BIANCO_RZB,
                Id = ETABLISSEMENT_ID_BIANCO_RZB,
                Libelle = "Etablissement RAZEL-BEC",
                OrganisationId = ORGANISATION_ID_ETABLISSEMENT_BIANCO_RZB,
                PereId = ORGANISATION_ID_SOCIETE_BIANCO,
                TypeOrganisationId = OrganisationType.Etablissement.ToIntValue()
            };
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////    CIS
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public static OrganisationBase GetCi_411100_SocieteRzb()
        {
            return new OrganisationBase
            {
                Code = CODE_411100,
                Id = CI_ID_411100_SOCIETE_RZB,
                Libelle = "CI1 DE RAZEL-BEC Code" + CODE_411100,
                OrganisationId = ORGANISATION_ID_CI_411100_SOCIETE_RZB,
                PereId = ORGANISATION_ID_SOCIETE_RZB,
                TypeOrganisationId = OrganisationType.Ci.ToIntValue()
            };

        }
        public static OrganisationBase GetCi_411100_SocieteBianco()
        {
            return new OrganisationBase
            {
                Code = CODE_411100,
                Id = CI_ID_411100_SOCIETE_BIANCO,
                Libelle = "CI2 De bianco Code" + CODE_411100,
                OrganisationId = ORGANISATION_ID_CI_411100_SOCIETE_BIANCO,
                PereId = ORGANISATION_ID_SOCIETE_BIANCO,
                TypeOrganisationId = OrganisationType.Ci.ToIntValue()
            };
        }

        public static OrganisationBase GetCi_411100_SocieteSatelec()
        {
            return new OrganisationBase
            {
                Code = CODE_411100,
                Id = CI_ID_411100_SOCIETE_SATELEC,
                Libelle = "CI De Satelec Code " + CODE_411100,
                OrganisationId = ORGANISATION_ID_CI_411100_SOCIETE_SATELEC,
                PereId = ORGANISATION_ID_SOCIETE_SATELEC,
                TypeOrganisationId = OrganisationType.Ci.ToIntValue()
            };
        }


        public static OrganisationTree GetOrganisationTree()
        {

            var oranisations = new List<OrganisationBase>
                {
                    GetHoldingFayat(),
                    GetGroupeRzb(),
                    GetSocieteRzb(),
                    GetSocieteBianco(),
                    GetCi_411100_SocieteRzb()
                };

            var orgaTree = new OrganisationTree(oranisations);

            return orgaTree;
        }




    }
}
