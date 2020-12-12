using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities.CI;
using Fred.Entities;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.Business.Tests.Integration.Organisation.Extensions
{
    /// <summary>
    /// Toutes les extensions liées à l'affichage des entités
    /// </summary>
    public static class AffichageEntiteExtension
    {

        /// <summary>
        /// Extension du ToString sur CIEnt
        /// </summary>
        /// <param name="ciEnt">Object CiEnt</param>
        /// <returns>Affichage formaté pour le test</returns>
        public static string ToStringTest(this CIEnt ciEnt)
        {
            return $"CiId = {ciEnt.CiId} Code = {ciEnt.Code} Libelle = {ciEnt.Libelle} EtablissementComptableId = {ciEnt.EtablissementComptableId} SocieteId = {ciEnt.SocieteId}";
        }

        /// <summary>
        /// Extension du ToString sur EtablissementComptableEnt
        /// </summary>
        /// <param name="etabEnt">Object EtablissementComptableEnt</param>
        /// <returns>Affichage formaté pour le test</returns>
        public static string ToStringTest(this EtablissementComptableEnt etabEnt)
        {
            return $"EtablissementComptableId = {etabEnt.EtablissementComptableId} Code = {etabEnt.Code} Libelle = {etabEnt.Libelle} SocieteId = {etabEnt.SocieteId}";
        }

        /// <summary>
        /// Extension du ToString sur EtablissementComptableEnt
        /// </summary>
        /// <param name="socEnt">Object EtablissementComptableEnt</param>
        /// <returns>Affichage formaté pour le test</returns>
        public static string ToStringTest(this SocieteEnt socEnt)
        {
            return $"SocieteId = {socEnt.SocieteId} Code = {socEnt.Code} Libelle = {socEnt.Libelle} GroupeId = {socEnt.GroupeId}";
        }


        /// <summary>
        /// Retourne le type de l'organisation
        /// </summary>
        /// <param name="orgBase">L'organisation concernée</param>
        /// <returns>Le type de l'organisation en string</returns>
        public static string ToFriendlyString(this OrganisationType orgType)
        {
            switch (orgType)
            {
                case OrganisationType.Holding:
                    return "Holding";
                case OrganisationType.Pole:
                    return "Pôle";
                case OrganisationType.Groupe:
                    return "Groupe";
                case OrganisationType.Societe:
                    return "Société";
                case OrganisationType.Puo:
                    return "Périmètre UO";
                case OrganisationType.Uo:
                    return "Unité Opérationnelle";
                case OrganisationType.Etablissement:
                    return "Établissement";
                case OrganisationType.Ci:
                    return "Centre d'imputation";
                case OrganisationType.SousCi:
                    return "Sous centre d'imputation";
                default:
                    return "Type Inconnu";
            }
        }

        /// <summary>
        /// Retourne le type de l'organisation
        /// </summary>
        /// <param name="orgBase">L'organisation concernée</param>
        /// <returns>Le type de l'organisation en string</returns>
        public static string ToFriendlyId(this OrganisationType orgType)
        {
            switch (orgType)
            {
                case OrganisationType.Holding:
                    return "HoldingId";
                case OrganisationType.Pole:
                    return "PoleId";
                case OrganisationType.Groupe:
                    return "GroupeId";
                case OrganisationType.Societe:
                    return "SocieteId";
                case OrganisationType.Etablissement:
                    return "EtablissementComptableId";
                case OrganisationType.Ci:
                    return "CiId";
                case OrganisationType.SousCi:
                    return "CiId";
                default:
                    return null;
            }
        }
    }
}
