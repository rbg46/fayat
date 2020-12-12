using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred.Entities.Personnel.Import;
using Fred.ImportExport.Business.Log;

namespace Fred.ImportExport.Business.Personnel.Logs
{
    public class ImportPersonnelFonLogs : BaseFredIELogger, IImportPersonnelLogs
    {
        public ImportPersonnelFonLogs()
          : base("IMPORT", "PERSONNEL_FON")
        {
        }

        public void LogStartRequestAllData()
        {
            NotThrowErrorInLog(() =>
          {
              Log("LOG-PFON-001]", "[RECUPERATION DES DONNEES NECESSAIRES A L IMPORT]");
          });
        }

        public void LogStartTransormPersonnel()
        {
            NotThrowErrorInLog(() =>
           {
               Log("LOG-PFON-002]", "[DEMARRAGE DE LA TRANSFORMATION ANAEL VERS FRED]");
           });
        }

        public void LogDetectionModificationOfPersonnel(List<PersonnelAffectationResult> personnelAffectationResults)
        {
            NotThrowErrorInLog(() =>
            {
                var nb = personnelAffectationResults.Where(par => par.PersonnelIsModified).ToList();
                Log("LOG-PFON-003]", $"[DETECTION MODIFICATION DU PERSONNEL] Nombre de personnels modifiés {nb.Count}");
            });
        }

        public void LogNoModificationOfPersonnel(List<PersonnelAffectationResult> personnelAffectationResults)
        {
            NotThrowErrorInLog(() =>
            {
                var nb = personnelAffectationResults.Where(par => par.PersonnelIsNotModified).ToList();
                Log("LOG-PFON-004]", $"[PAS DE MODIFICATION DU PERSONNELS] Nombre de personnels non modifiés {nb.Count}.");
            });
        }

        public void LogDetectionCreationPersonnel(List<PersonnelAffectationResult> personnelAffectationResults)
        {
            NotThrowErrorInLog(() =>
             {
                 var nb = personnelAffectationResults.Where(par => par.PersonnelIsNew).ToList();
                 Log("[LOG-PFON-005]", $"[DETECTION CREATION PRESONNELS] Nombre de personnels non crées {nb.Count}.");
             });
        }

        public string ErrorNoSocieteWithCodeSocietePaie(List<string> codeSocietePaies)
        {
            return NotThrowErrorInLog(() =>
            {
                var warmMessage = new StringBuilder();
                var codeSocietePaiesToString = codeSocietePaies.Aggregate((i, j) => i + "-" + j);
                warmMessage.AppendLine("L'import a échoué.");
                warmMessage.AppendLine($"Aucune société trouvée pour ce(s) codeSocietePaie(s). ({codeSocietePaiesToString})");
                var message = warmMessage.ToString();
                Error("[ERR-PFON-001]", message);
                return message;
            });
        }

        public string ErrorDifferentGroupe(List<string> codeSocietePaies)
        {
            return NotThrowErrorInLog(() =>
             {
                 var warmMessage = new StringBuilder();
                 warmMessage.AppendLine(" L'import a échoué.");
                 warmMessage.AppendLine(" Les sociétés doivent appartenir au meme groupe.");
                 foreach (var row in codeSocietePaies)
                 {
                     warmMessage.AppendLine($" - {row} ");
                 }
                 var message = warmMessage.ToString();
                 Error("[ERR-PFON-002]", message);
                 return message;
             });
        }

        public string ErrorSocietesMissing(List<string> codeSocietePaies)
        {
            return NotThrowErrorInLog(() =>
             {
                 var warmMessage = new StringBuilder();
                 warmMessage.AppendLine("L'import a échoué.");
                 warmMessage.AppendLine("Certaines societés n'ont pas été trouvées pour ce(s)codeSocietePaie(s)");
                 warmMessage.AppendLine(" Societes : ");
                 foreach (var row in codeSocietePaies)
                 {
                     warmMessage.AppendLine($" - {row} ");
                 }
                 var message = warmMessage.ToString();
                 Warn("[ERR-PFON-003]", message);
                 return message;
             });
        }

        public void WarnManagerNotFound(List<PersonnelAffectationResult> personnelAffectationResults)
        {
            NotThrowErrorInLog(() =>
            {
                var warmMessage = new StringBuilder();
                var affectedRows = personnelAffectationResults.Where(par => par.ManagerCanNotBeProcessed).ToList();
                if (affectedRows.Any())
                {
                    warmMessage.AppendLine(" Il n'a pas été possible de trouvé le(s) manager(s) :");
                    warmMessage.AppendLine(" Soit le manager appartient a une societe non importée, soit le matricule ne correspond pas un aun personnel.");
                    warmMessage.AppendLine(" Personnel(Matricule - Societe)  Manager(Matricule - Societe)");
                    foreach (var row in affectedRows)
                    {
                        var matriculeManager = row.PersonnelModel.MatriculeManager;
                        var societeManager = row.PersonnelModel.SocieteManager;
                        warmMessage.AppendLine($" ({row.PersonnelModel.Matricule} - {row.PersonnelModel.CodeSocietePaye}) ({matriculeManager} - {societeManager}).");
                    }
                }
                if (!string.IsNullOrEmpty(warmMessage.ToString()))
                {
                    Warn("[WARN-PFON-001]", warmMessage.ToString());
                }
            });
        }

        public void WarnEtablissementPaieNotFound(List<PersonnelAffectationResult> personnelAffectationResults)
        {
            NotThrowErrorInLog(() =>
            {
                StringBuilder warmMessage = new StringBuilder();
                IEnumerable<PersonnelAffectationResult> affectedRows = personnelAffectationResults.Where(par => par.Personnel.EtablissementPaieId == null);
                foreach (var row in affectedRows)
                {
                    warmMessage.AppendLine(" Il n'y a pas d'etablissement de paie pour le(s) personnel(s) suivant (Matricule-Societe) :");
                    warmMessage.AppendLine(" Le(s) personnel(s) n'auront pas de d'établissement de rattachement non plus.");
                    warmMessage.AppendLine($" {row.PersonnelModel.Matricule} {row.PersonnelModel.CodeSocietePaye}).");
                }
                if (!string.IsNullOrEmpty(warmMessage.ToString()))
                {
                    Warn("[WARN-PFON-002]", warmMessage.ToString());
                }
            });
        }

        public void WarnRessourceNotFound(List<PersonnelAffectationResult> personnelAffectationResults)
        {
            NotThrowErrorInLog(() =>
            {
                var warmMessage = new StringBuilder();
                var affectedRows = personnelAffectationResults.Where(par => par.Personnel.RessourceId == null).ToList();

                warmMessage.AppendLine(" Il n'y a pas  de Ressource 'Fred' pour le(s) personnel(s) suivant (Matricule - Societe - codeEmploi) :");
                foreach (var row in affectedRows)
                {
                    warmMessage.AppendLine($"{row.PersonnelModel.Matricule} - {row.PersonnelModel.CodeSocietePaye} - {row.PersonnelModel.CodeEmploi}.");
                }
                if (!string.IsNullOrEmpty(warmMessage.ToString()))
                {
                    Warn("[WARN-PFON-003]", warmMessage.ToString());
                }
            });
        }

        public void WarnNoCiFound(List<PersonnelAffectationResult> personnelAffectationResults)
        {
            NotThrowErrorInLog(() =>
            {
                var warmMessage = new StringBuilder();
                List<PersonnelAffectationResult> affectedRows = new List<PersonnelAffectationResult>();
                foreach (PersonnelAffectationResult personnelAffectationResult in personnelAffectationResults)
                {
                    if (personnelAffectationResult.Affectations.Any(x => x.CiId == 0 && x.AffectationIsNewOrModified))
                    {
                        affectedRows.Add(personnelAffectationResult);

                    }
                }

                warmMessage.AppendLine(" Il n'y a pas  de CI 'Fred' pour le(s) personnel(s) suivant (Matricule - Societe - CI) :");

                foreach (var row in affectedRows)
                {
                    warmMessage.AppendLine($"({row.PersonnelModel.Matricule} - {row.PersonnelModel.CodeSocietePaye}  - {row.PersonnelModel.Section}).");
                }
                var message = warmMessage.ToString();
                if (!string.IsNullOrEmpty(message))
                {
                    Warn("[WARN-PFON-004]", message);
                }
            });
        }
    }
}
