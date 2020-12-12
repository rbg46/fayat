﻿using System;
using Fred.ImportExport.Business.Log;
using Fred.ImportExport.Business.ValidationPointage.Factory;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.Logs
{
    /// <summary>
    /// Logger pour la validation pointage pour le groupe FONDATION
    /// </summary>
    public class ValidationPointageFonLogger : BaseFredIELogger, IValidationPointageFonLogger
    {
        public ValidationPointageFonLogger()
          : base("IMPORT-EXPORT", "VALIDATION_POINTAGE_FON")
        {
        }

        public string ErrorNoConfigForGroupe(string groupeCode)
        {
            return Error("[ERR-CONTVRAC-FON-001]", TypeLog.BadConfig, $"Il n' y a pas de configuration pour le groupe {groupeCode}.");
        }

        public string ErrorUserNotFound(int utilisateurId)
        {
            return Error("[ERR-CONTVRAC-FON-002]", TypeLog.BadConfig, $"Il n' y a pas de personnel correspondant a l'utilisateurId {utilisateurId}.");
        }

        public void LogSelectedSettings(ValidationPointageFactorySetting setting)
        {
            Log("[LOG-CONTVRAC-FON-001]", TypeLog.Info, $"Selection du flux {setting.FluxCode} et code du groupe concerné {setting.GroupeCode}.");
        }

        public string ErrorInInsertPointagesAndPrimes(Exception e, string currentQuery)
        {
            string errorMsg = "Erreur lors du déversement des pointages et des primes dans AS400. Requête : " + currentQuery;
            return Error("[ERR-CONTVRAC-FON-001]", TypeLog.Critique, errorMsg, e);
        }

        public string ErrorInExecuteControleVracWithAnael(Exception e, string query)
        {
            string errorMsg = "Erreur lors de l'appel à la procédure AS400 du Contrôle vrac. Requête : " + query;
            return Error("[ERR-CONTVRAC-FON-002]", TypeLog.Critique, errorMsg, e);
        }

        public string ErrorInRetrieveAnaelControleVracInfo(Exception e, string query)
        {
            string errorMsg = "Erreur lors de la récupération des erreurs du Contrôle Vrac. Requête : " + query;
            return Error("[ERR-CONTVRAC-FON-003]", TypeLog.Critique, errorMsg, e);
        }

        public string LogControleVracGlobalError(Exception e)
        {
            string errorMsg = "Erreur lors de l'exécution du contrôle vrac. ";
            return Error("[ERR-CONTVRAC-FON-004]", TypeLog.Critique, errorMsg, e);
        }

        public string ErrorInExecuteJobFluxInactif(string code)
        {
            string errorMsg = "Flux inactif. Ce flux : " + code + " n'est pas activé.";
            return Error("[ERR-CONTVRAC-FON-005]", TypeLog.BadConfig, errorMsg);
        }

        public string ErrorInExecuteJobFluxNotFound(string fluxCode)
        {
            string errorMsg = "Flux inexistant. Ce flux : " + fluxCode + " n'a pas été trouvé en base de données.";
            return Error("[ERR-CONTVRAC-FON-006]", TypeLog.BadConfig, errorMsg);
        }

        public string ErrorInExecuteRemonteeVracWithAnael(Exception e, string query)
        {
            string errorMsg = "Erreur lors de l'appel à la procédure AS400 de la Remontée vrac. Requête : " + query;
            return Error("[ERR-REMONTEVRAC-FON-001]", TypeLog.Critique, errorMsg, e);
        }

        public string ErrorInRetrieveAnaelRemonteeVracErrors(Exception e, string query)
        {
            string errorMsg = "Erreur lors de la récupération des erreurs de la remontée Vrac. Requête : " + query;
            return Error("[ERR-REMONTEVRAC-FON-002]", TypeLog.Critique, errorMsg, e);
        }

        public string LogRemonteeVracGlobalError(Exception e)
        {
            string errorMsg = "Erreur lors de l'exécution de la remontée vrac.";
            return Error("[ERR-REMONTEVRAC-FON-003]", TypeLog.Critique, errorMsg, e);
        }

        public string ErrorInExecuteRemonteeVracFluxNotFound(string fluxCode)
        {
            string errorMsg = "Flux inexistant. Ce flux : " + fluxCode + " n'a pas été trouvé en base de données.";
            return Error("[ERR-REMONTEVRAC-FON-004]", TypeLog.BadConfig, errorMsg);
        }

        public string ErrorInExecuteRemonteeVracFluxInactif(string code)
        {
            string errorMsg = "Flux inactif. Ce flux : " + code + " n'est pas activé.";
            return Error("[ERR-REMONTEVRAC-FON-005]", TypeLog.BadConfig, errorMsg);
        }
        public void ErrorPersonnelNotFoundForAddRemonteeVracErreur(string matricule)
        {
            string errorMsg = "Erreur lors de l'insertion des erreurs de la remontée vrac dans fred : matricule  " + matricule + " non trouvé .";
            Error("[ERR-REMONTEVRAC-FON-006]", TypeLog.Error, errorMsg);
        }
    }
}