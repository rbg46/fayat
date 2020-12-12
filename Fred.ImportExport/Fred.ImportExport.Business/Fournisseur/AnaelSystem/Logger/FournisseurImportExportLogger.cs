using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Business.Log;
using Fred.ImportExport.Models;
using Fred.ImportExport.Models.Ci;
using Newtonsoft.Json;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem
{
    public class FournisseurImportExportLogger : BaseFredIELogger
    {
        public FournisseurImportExportLogger(string fluxDirection, string codeFlux)
            : base(fluxDirection, codeFlux)
        {
        }

        internal void ParsageDuFichierSucess()
        {
            Log("[LOG-PARSE]", "Parsage du fichier excel réussi.");
        }

        internal void LogInfoContext(ImportFournisseurContext<ImportFournisseursByExcelInputs> context)
        {
            var societeCodes = context.SocietesNeeded.Select(x => x.Code).Aggregate((a, c) => a + " " + c);
            Log("[LOG-SOCIETE-INFO]", $"Societes détectée :{societeCodes}.");
        }

        internal void LogAnaelModels(List<FournisseurAnaelModel> ciAnaelModels)
        {
            foreach (var ciAnaelModel in ciAnaelModels)
            {
                Log("[LOG-INFO-FOURNISSEUR-ANAEL]", $" ANAEL : {ciAnaelModel}.");
            }
        }

        internal void LogFredEnts(List<FournisseurEnt> fournisseurEnts)
        {
            foreach (var fournisseurEnt in fournisseurEnts)
            {
                Log("[LOG-INFO-FOURNISSEUR-FRED]", $" FRED : " +
                                         $" FournisseurId = ({fournisseurEnt.FournisseurId}) - " +
                                         $" Code = ({fournisseurEnt.Code}) - " +
                                         $" DateOuverture = ({fournisseurEnt.DateOuverture}) - " +
                                         $" DateCloture = ({fournisseurEnt.DateCloture}) - " +
                                         $" Libelle = ({fournisseurEnt.Libelle}) - " +
                                         $" CodeSociete = ({fournisseurEnt.Libelle}) - " +
                                         $" Adresse = ({fournisseurEnt.Adresse}) - " +
                                         $" Ville = ({fournisseurEnt.Ville}) - " +
                                         $" CodePostal = ({fournisseurEnt.CodePostal}) - " +
                                         $" PaysId = ({fournisseurEnt.PaysId}) - ");
            }
        }

        internal void LogSapModel(FournisseurStormModel ciStormModel)
        {
            Log("[LOG-INFO-FOURNISSEUR-SAP]", $" SAP : {ciStormModel}.");
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

        public void ErrorWhenSendToSap(Exception e, string url, FournisseurStormModel fournisseurStormModel)
        {
            Error("[ERR-DATA-SEND-TO-SAP]", $" Url :{url}.");
            Error("[ERR-DATA-SEND-TO-SAP]", $" Fournisseur envoyé a SAP :{JsonConvert.SerializeObject(fournisseurStormModel, Formatting.Indented)}.");
            Error("[ERR-DATA-SEND-TO-SAP]", $" Exception :{e?.Message + Environment.NewLine + e?.InnerException?.Message }.");
        }



        public void ErrorWhenSendToSapNoConfig(SocieteEnt societe)
        {
            Error("[ERR-DATA-SEND-TO-SAP]", $"Export 'Fournisseur' vers SAP: Il n'y a pas de configuration correspondant à cette société {societe.Code} ({societe.CodeSocieteComptable}) ({societe.SocieteId}).");
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
