using System;
using System.Collections.Generic;
using System.Text;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.Log;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Models.Personnel;
using Newtonsoft.Json;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Logger
{
    public class PersonnelImportExportLogger : BaseFredIELogger
    {
        public PersonnelImportExportLogger(string fluxDirection, string codeFlux)
            : base(fluxDirection, codeFlux)
        {
        }

        internal void LogAnaelModels(List<PersonnelAnaelModel> personnelAnaelModels)
        {
            foreach (var personnelAnaelModel in personnelAnaelModels)
            {
                Log("[LOG-INFO-PERSONNEL-ANAEL]", $" ANAEL : {personnelAnaelModel}.");
            }
        }

        internal void LogFredEnts(List<PersonnelEnt> personnelEnts)
        {
            if (personnelEnts == null || personnelEnts.Count == 0)
            {
                return;
            }

            foreach (var personnelEnt in personnelEnts)
            {
                Log("[LOG-INFO-PERSONNEL-FRED]", $" FRED : " +
                                         $" PersonnelId = ({personnelEnt.PersonnelId}) - " +
                                         $" CodeSociete = ({personnelEnt.Societe?.CodeSocieteComptable}) - " +
                                         $" Matricule = ({personnelEnt.Matricule}) - " +
                                         $" Nom = ({personnelEnt.Nom}) - " +
                                         $" Prenom = ({personnelEnt.Prenom}) - " +
                                         $" CategoriePerso = ({personnelEnt.CategoriePerso}) - " +
                                         $" Statut = ({personnelEnt.Statut}) - " +
                                         $" DateEntree = ({personnelEnt.DateEntree}) - " +
                                         $" DateSortie = ({personnelEnt.DateSortie}) - " +
                                         $" Adresse = ({personnelEnt.Adresse}) - " +
                                         $" Adresse2 = ({personnelEnt.Adresse2}) - " +
                                         $" Adresse3 = ({personnelEnt.Adresse3}) - " +
                                         $" Ville = ({personnelEnt.Ville}) - " +
                                         $" CodePostal = ({personnelEnt.CodePostal}) - " +
                                         $" PaysId = ({personnelEnt.PaysId}) - ");
            }
        }

        internal void LogSapModel(PersonnelStormModel personnelStormModel)
        {
            Log("[LOG-INFO-PERSONNEL-SAP]", $" SAP : {personnelStormModel}.");
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

        public void ErrorWhenSendToSap(Exception e, string url, PersonnelStormModel personnelStormModel)
        {
            Error("[ERR-DATA-SEND-TO-SAP]", $" Url :{url}.");
            Error("[ERR-DATA-SEND-TO-SAP]", $" Personnel envoyé a SAP :{JsonConvert.SerializeObject(personnelStormModel, Formatting.Indented)}.");
            Error("[ERR-DATA-SEND-TO-SAP]", $" Exception :{e?.Message + Environment.NewLine + e?.InnerException?.Message }.");
        }

        public void ErrorWhenSendToSapNoConfig(SocieteEnt societe)
        {
            Error("[ERR-DATA-SEND-TO-SAP]", $"Export 'PERSONNEL' vers SAP: Il n'y a pas de configuration correspondant à cette société {societe.Code} ({societe.CodeSocieteComptable}) ({societe.SocieteId}).");
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
