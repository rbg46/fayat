using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Entities.Personnel;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.DataAccess.ExternalService.ExportPersonnelProxy;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Common;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Personnel;

namespace Fred.ImportExport.Business.Personnel
{
    public class PersonnelFluxTibcoSystemService : IPersonnelFluxTibcoSystemService
    {
        /// <summary>
        /// Obtient l'identifiant du job d'exprot (Code du flux d'export).
        /// </summary>
        public string ExportJobId { get; } = ConfigurationManager.AppSettings["Tibco:ExportPersonnel"];

        private readonly IPersonnelManager personnelManager;

        public PersonnelFluxTibcoSystemService(IPersonnelManager personnelManager)
        {
            this.personnelManager = personnelManager;
        }

        /// <summary>
        /// Exporte le personnel Fes dernièrement modifier vers Tibco
        /// </summary>
        /// <param name="byPassDate">indique s'il prend ou ignore la dernière date d'execution</param>
        /// <param name="lastExecutionDate">DateTime dernière date d'execution du flux</param>
        public void ExportPersonnelToTibco(bool byPassDate, DateTime? lastExecutionDate)
        {
            try
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"[EXPORT_PERSONNEL_FES][FIGGO] Lancement de l'export personnel vers TIBCO");

                IEnumerable<PersonnelEnt> modifiedPersonnelList = personnelManager.GetPersonnelFesForExportToTibco(byPassDate, lastExecutionDate);

                if (modifiedPersonnelList.Any())
                {
                    PersonnelManagerInRecord[] recordsToSend = Array.ConvertAll(modifiedPersonnelList.ToArray(), x => CreateModelPersonnelForTibco(x));

                    PersonnelTibcoRepositoryExterne personnelRepoExterne = new PersonnelTibcoRepositoryExterne();
                    TibcoReturnResult result = personnelRepoExterne.SendPointageToTibco(recordsToSend, ExportJobId);
                    if (result.Code == TibcoReturnCode.Success)
                    {
                        NLog.LogManager.GetCurrentClassLogger().Info($"[EXPORT_PERSONNEL_FES][FIGGO] L'export vers TIBCO s'est déroulé avec succès. Total personnel(s) envoyé(s) :" + recordsToSend.Count());
                    }
                    else
                    {
                        var msg = string.Format(IEBusiness.FluxErreurExport, ExportJobId);
                        var exception = new FredBusinessException(msg, result.Exception);
                        NLog.LogManager.GetCurrentClassLogger().Error($"[EXPORT_PERSONNEL_FES][FIGGO] L'export vers TIBCO est en échec");
                        throw exception;
                    }
                }
                else
                {
                    NLog.LogManager.GetCurrentClassLogger().Warn($"[EXPORT_PERSONNEL_FES][FIGGO] Aucun personnel à envoyer vers TIBCO");
                }
            }
            catch (Exception e)
            {
                var msg = string.Format(IEBusiness.FluxErreurExport, ExportJobId);
                var exception = new FredBusinessException(msg, e);
                NLog.LogManager.GetCurrentClassLogger().Error($"[EXPORT_PERSONNEL_FES][FIGGO] L'export vers TIBCO est en échec");
                throw exception;
            }
        }

        private PersonnelManagerInRecord CreateModelPersonnelForTibco(PersonnelEnt personnel)
        {
            return new PersonnelManagerInRecord()
            {
                CodeSocieteCompta = personnel.Societe?.CodeSocieteComptable,
                CodeSocietePaie = personnel.Societe?.CodeSocietePaye,
                CodeEtabCompta = personnel.EtablissementPaie?.EtablissementComptable?.Code,
                CodeEtabPaie = personnel.EtablissementPaie?.Code,
                MatriculePersonnel = personnel.Matricule,
                NomPersonnel = personnel.Nom,
                PrenomPersonnel = personnel.Prenom,
                EmailPersonnel = personnel.Email,
                IsUserFRED = personnel.Utilisateur != null ? "true" : "false",
                LoginPersonnel = personnel.Utilisateur?.Login,
                IsUserManager = personnelManager.PersonnelIsManager(personnel.PersonnelId).ToString(),
                CodeSocieteComptaManager = personnel.Manager?.Societe?.CodeSocieteComptable,
                CodeSocietePaieManager = personnel.Manager?.Societe?.CodeSocietePaye,
                CodeEtabComptaManager = personnel.Manager?.EtablissementPaie?.EtablissementComptable?.Code,
                CodeEtabPaieManager = personnel.Manager?.EtablissementPaie?.Code,
                MatriculeManager = personnel.Manager?.Matricule,
                NomManager = personnel.Manager?.Nom,
                PrenomManager = personnel.Manager?.Prenom,
                IsManagerUserFRED = personnel.Manager?.Utilisateur != null ? "true" : "false",
                LoginManager = personnel.Manager?.Utilisateur?.Login
            };
        }

    }
}
