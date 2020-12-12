using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Business.RepriseDonnees.Rapport.ContextProviders;
using Fred.Business.RepriseDonnees.Rapport.Selector;
using Fred.Business.Utilisateur;
using Fred.Entities.CI;
using Fred.Entities.Rapport;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Rapport.Mapper
{
    public class RapportDataMapper : IRapportDataMapper
    {
        private readonly IUtilisateurManager utilisateurManager;

        public RapportDataMapper(IUtilisateurManager utilisateurManager)
        {
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// returne la liste des rapports à créer 
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="repriseExcelRapports">les rapports sous la forme excel</param>
        /// <returns>Liste des rapports à créer</returns>
        public RapportTransformResult Transform(ContextForImportRapport context, List<RepriseExcelRapport> repriseExcelRapports)
        {
            var result = new RapportTransformResult();
            var commonFieldSelector = new CommonFieldSelector();

            var rapports = repriseExcelRapports.GroupBy(x => new { x.CodeCi, x.DateRapport }).Select(x => x.First()).ToList();
            foreach (var rapport in rapports)
            {
                var ci = commonFieldSelector.GetCiOfDatabase(context.OrganisationTree, context.GroupeId, rapport.CodeSocieteCi, rapport.CodeCi, context.CisUsedInExcel);

                var newRapport = new RapportEnt
                {
                    RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key,
                    DateChantier = commonFieldSelector.GetDate(rapport.DateRapport),
                    HoraireDebutM = DateTime.UtcNow.Date.AddHours(8),
                    HoraireFinM = DateTime.UtcNow.Date.AddHours(12),
                    HoraireDebutS = DateTime.UtcNow.Date.AddHours(14),
                    HoraireFinS = DateTime.UtcNow.Date.AddHours(18),
                    AuteurCreationId = context.FredIeUser.UtilisateurId,
                    DateCreation = DateTime.UtcNow,
                    TypeRapport = TypeRapport.Journee.ToIntValue(),
                    Meteo = null,
                    Evenements = null,
                    AuteurModificationId = null,
                    AuteurSuppressionId = null,
                    AuteurVerrouId = null,
                    DateModification = null,
                    DateSuppression = null,
                    DateVerrou = null,
                    ValideurCDCId = null,
                    ValideurCDTId = null,
                    ValideurDRCId = null,
                    DateValidationCDC = null,
                    DateValidationCDT = null,
                    DateValidationDRC = null,
                    IsGenerated = false
                };
                if (ci != null)
                {
                    newRapport.CiId = ci.CiId;
                }
                result.Rapports.Add(newRapport);
                result.RapportLignes.AddRange(TransformToRapportligneList(context, repriseExcelRapports, newRapport, ci));
            }
            return result;
        }

        /// <summary>
        /// returne la liste des rapports lignes à créer 
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="repriseExcelRapports">les rapports sous la forme excel</param>
        /// <param name="createdRapport">le rapport créé</param>
        /// <param name="ci">ci associé au rapport crée</param>
        /// <returns>Liste des rapports lignes à créer</returns>
        private List<RapportLigneEnt> TransformToRapportligneList(ContextForImportRapport context, List<RepriseExcelRapport> repriseExcelRapports, RapportEnt createdRapport, CIEnt ci)
        {
            var commonSelector = new CommonFieldSelector();
            PersonnelSelectorService personnelSelectorService = new PersonnelSelectorService();
            CodeDeplacementSelectorService codeDeplacementSelectorService = new CodeDeplacementSelectorService();
            CodeZoneDeplacSelectorService codeZoneDeplacSelectorService = new CodeZoneDeplacSelectorService();

            // prendre seulement les rapports lignes de rapport créé
            var repriseExcelLignesRapport = repriseExcelRapports.Where(x => x.CodeCi == ci.Code && commonSelector.GetDate(x.DateRapport) == createdRapport.DateChantier).ToList();
            var rapportLignes = new List<RapportLigneEnt>();
            foreach (var repriseExcelRapport in repriseExcelLignesRapport)
            {
                var newRapportLigne = new RapportLigneEnt
                {
                    Rapport = createdRapport,
                    CiId = createdRapport.CiId,
                    PersonnelId = personnelSelectorService.GetPersonnel(repriseExcelRapport.CodeSocieteCi, repriseExcelRapport.MatriculePersonnel, context).PersonnelId,
                    MaterielId = null,
                    CodeAbsenceId = null,
                    CodeMajorationId = null,
                    CodeDeplacementId = codeDeplacementSelectorService.GetCodeDeplacement(repriseExcelRapport.CodeSocieteCi, repriseExcelRapport.CodeDeplacement, context)?.CodeDeplacementId,
                    CodeZoneDeplacementId = codeZoneDeplacSelectorService.GetCodeZoneDeplacement(repriseExcelRapport.CodeSocieteCi, repriseExcelRapport.CodeZoneDeplacement, context)?.CodeZoneDeplacementId,
                    AuteurCreationId = context.FredIeUser.UtilisateurId,
                    AuteurModificationId = null,
                    AuteurSuppressionId = null,
                    PrenomNomTemporaire = null,
                    HeureNormale = commonSelector.GetDouble(repriseExcelRapport.HeuresTotal),
                    HeureMajoration = default(int),
                    HeureAbsence = default(int),
                    NumSemaineIntemperieAbsence = null,
                    DeplacementIV = commonSelector.GetBoolean(repriseExcelRapport.IVD),
                    MaterielMarche = default(int),
                    MaterielArret = default(int),
                    MaterielPanne = default(int),
                    MaterielIntemperie = default(int),
                    DatePointage = createdRapport.DateChantier,
                    DateCreation = DateTime.UtcNow,
                    DateModification = null,
                    DateSuppression = null,
                    MaterielNomTemporaire = null,
                    IsGenerated = false,
                    LotPointageId = null,
                    AvecChauffeur = false,
                    ReceptionInterimaire = false,
                    RapportLigneStatutId = null,
                    HeuresTotalAstreintes = default(int),
                    CodeZoneDeplacementSaisiManuellement = false,
                    ValideurId = null,
                    DateValidation = null
                };
                var newRapportLigneTache = new RapportLigneTacheEnt
                {
                    RapportLigne = newRapportLigne,
                    TacheId = commonSelector.GetTache(createdRapport.CiId, repriseExcelRapport.CodeTache, context.TachesUsedInExcel).TacheId,
                    HeureTache = commonSelector.GetDouble(repriseExcelRapport.HeuresTotal)
                };
                newRapportLigne.ListRapportLigneTaches.Add(newRapportLigneTache);
                rapportLignes.Add(newRapportLigne);
            }
            return rapportLignes;
        }
    }
}

