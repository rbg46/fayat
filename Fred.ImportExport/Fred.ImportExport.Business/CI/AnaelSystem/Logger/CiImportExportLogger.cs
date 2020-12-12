using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Business.Log;
using Fred.ImportExport.Models.Ci;
using Newtonsoft.Json;

namespace Fred.ImportExport.Business.CI.AnaelSystem
{
    public class CiImportExportLogger : BaseFredIELogger
    {
        public CiImportExportLogger(string fluxDirection, string codeFlux)
            : base(fluxDirection, codeFlux)
        {
        }

        internal void ParsageDuFichierSucess()
        {
            Log("[LOG-PARSE]", "Parsage du fichier excel réussi.");
        }

        internal void LogInfoContext(ImportCiContext<ImportCisByExcelInputs> context)
        {
            var societeCodes = context.SocietesNeeded.Select(x => x.Code).Aggregate((a, c) => a + " " + c);
            Log("[LOG-SOCIETE-INFO]", $"Societes détectée :{societeCodes}.");
        }

        internal void LogAnaelModels(List<CiAnaelModel> ciAnaelModels)
        {
            foreach (var ciAnaelModel in ciAnaelModels)
            {
                Log("[LOG-INFO-CI-ANAEL]", $" ANAEL : {ciAnaelModel}.");
            }
        }

        internal void LogFredEnts(List<CIEnt> cIEnts)
        {
            foreach (var ciEnt in cIEnts)
            {
                Log("[LOG-INFO-CI-FRED]", $" FRED : " +
                                         $" CiId = ({ciEnt.CiId}) - " +
                                         $" Code = ({ciEnt.Code}) - " +
                                         $" DateOuverture = ({ciEnt.DateOuverture}) - " +
                                         $" DateFermeture = ({ciEnt.DateFermeture}) - " +
                                         $" EtablissementComptableId = ({ciEnt.EtablissementComptableId}) - " +
                                         $" Libelle = ({ciEnt.Libelle}) - " +
                                         $" ChantierFRED = ({ciEnt.ChantierFRED}) - " +
                                         $" CodeSociete = ({ciEnt.Libelle}) - " +
                                         $" ResponsableChantierId = ({ciEnt.ResponsableChantierId}) - " +
                                         $" ResponsableAdministratifId = ({ciEnt.ResponsableAdministratifId}) - " +
                                         $" Adresse = ({ciEnt.Adresse}) - " +
                                         $" Adresse2 = ({ciEnt.Adresse2}) - " +
                                         $" Adresse3 = ({ciEnt.Adresse3}) - " +
                                         $" Ville = ({ciEnt.Ville}) - " +
                                         $" CodePostal = ({ciEnt.CodePostal}) - " +
                                         $" PaysId = ({ciEnt.PaysId}) - ");
            }
        }

        internal void LogSapModel(CiStormModel ciStormModel)
        {
            Log("[LOG-INFO-CI-SAP]", $" SAP : {ciStormModel}.");
        }

        internal void LogEtablissementComptablesOfSociete(SocieteEnt societe, List<EtablissementComptableEnt> etablissementComptables)
        {
            var messageEtablissements = new StringBuilder();

            messageEtablissements.AppendLine($"La societe {societe.Code} - {societe.CodeSocieteComptable} a les etablissements comptables suivant: ");

            foreach (var etablissementComptable in etablissementComptables)
            {
                messageEtablissements.AppendLine($"  (EtablissementComptableId={etablissementComptable.EtablissementComptableId} Code={etablissementComptable.Code})  ");
            }

            Log("[LOG-INFO-ETABLISSEMENT-COMPTABLES]", messageEtablissements.ToString());
        }

        internal void WarnValidationErrors(ImportResult rulesResult)
        {
            foreach (var error in rulesResult.ErrorMessages)
            {
                Warn("[WARN-RULES-ERROR]", $"{error}.");
            }
        }

        public void NoEtablissementComptable(string codeEtablissementComptableAnael)
        {
            Warn("[WARN-INSERT-DATA-TO-FRED]", $"Il n'y a pas d'etablissement comptable correspondant au code {codeEtablissementComptableAnael}.");
        }

        internal void WarnNoEtablissementComptableIfNecessary(EtablissementComptableEnt etablissementComptable, CiAnaelModel ciAnaelModel)
        {
            if (etablissementComptable == null && ciAnaelModel != null && ciAnaelModel.CodeEtablissement != null && !ciAnaelModel.CodeEtablissement.Trim().IsNullOrEmpty())
            {
                Warn("[WARN-INSERT-DATA-TO-FRED]", $"Il n'y a pas d'etablissement comptable correspondant au code {ciAnaelModel.CodeEtablissement}.");
            }
        }

        public void ErrorWhenSendToSap(Exception e, string url, CiStormModel ciStormModel)
        {
            Error("[ERR-DATA-SEND-TO-SAP]", $" Url :{url}.");
            Error("[ERR-DATA-SEND-TO-SAP]", $" CI envoyé a SAP :{JsonConvert.SerializeObject(ciStormModel, Formatting.Indented)}.");
            Error("[ERR-DATA-SEND-TO-SAP]", $" Exception :{e?.Message + Environment.NewLine + e?.InnerException?.Message }.");
        }



        public void ErrorWhenSendToSapNoConfig(SocieteEnt societe)
        {
            Error("[ERR-DATA-SEND-TO-SAP]", $"Export 'CI' vers SAP: Il n'y a pas de configuration correspondant à cette société {societe.Code} ({societe.CodeSocieteComptable}) ({societe.SocieteId}).");
        }

        internal void LogEtablissementComptableWillBeDefault(SocieteEnt societe)
        {
            Log("[LOG-INFO-SOCIETE]", $"La societe {societe.Code} - {societe.CodeSocieteComptable} utilisera l'etablissement par default.");
        }

        internal void WarnEtablissementComptableCantBeOverrided(SocieteEnt societe)
        {
            Warn("[WARN-ETABLISSEMENT-MISSING]", $"La societe {societe.Code} - {societe.CodeSocieteComptable} n'a pas l'etablissement comptable par defaut, tous les cis de cette societe ne seront pas importés.");
        }

        internal void WarnNoSocieteWillBeImportedInFred()
        {
            Warn("[WARN-SOCIETE-NOT-IMPORTED]", $"Aucune société ne sera importées dans fred.");
        }

        internal void WarnNoSocieteWillBeExportedInSap()
        {
            Warn("[WARN-SOCIETE-NOT-EXPORTED]", $"Aucune société ne sera exportées vers SAP.");
        }
    }
}
