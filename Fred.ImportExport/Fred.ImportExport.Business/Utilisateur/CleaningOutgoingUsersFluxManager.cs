using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.Directory;
using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.Entities.Directory;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Hangfire;

namespace Fred.ImportExport.Business.Utilisateur
{
    /// <summary>
    /// Manager du job qui permet de supprimer les rôles et le login d’un personnel après qu’il est quitté sa société
    /// </summary>
    public class CleaningOutgoingUsersFluxManager : AbstractFluxManager
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager;
        private readonly IExternalDirectoryManager externalDirectoryManager;

        public CleaningOutgoingUsersFluxManager(IFluxManager fluxManager,
                    IUtilisateurManager utilisateurManager,
                    IPersonnelManager personnelManager,
                    IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager,
                    IExternalDirectoryManager externalDirectoryManager)
            : base(fluxManager)
        {
            this.utilisateurManager = utilisateurManager;
            this.personnelManager = personnelManager;
            this.affectationSeuilUtilisateurManager = affectationSeuilUtilisateurManager;
            this.externalDirectoryManager = externalDirectoryManager;
        }

        /// <summary>
        ///   Exécution de l'import du Suppression des rôles et des logins des personnels ayant quittés une société
        /// </summary>
        public void ExecuteImport()
        {
            try
            {
                BackgroundJob.Enqueue(() => ImportationProcess());
            }
            catch (FredBusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                var exception = new FredBusinessException(e.Message, e);
                NLog.LogManager.GetCurrentClassLogger().Error(exception);
                throw exception;
            }
        }


        /// <summary>
        ///   Planifier l'exécution de l'import du Suppression des rôles et des logins des personnels ayant quittés une société
        /// </summary>
        /// <param name="codeFlux">Le code du flux</param>
        public void ScheduleImportByCodeFlux(string codeFlux)
        {
            try
            {
                RecurringJob.AddOrUpdate(codeFlux, () => ImportationProcess(), Cron.Daily);
            }
            catch (FredBusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                var exception = new FredBusinessException(e.Message, e);
                NLog.LogManager.GetCurrentClassLogger().Error(exception);
                throw exception;
            }
        }

        /// <summary>
        ///   Exécution du l' ImportationProcess du Suppression des rôles et des logins des personnels ayant quittés une société
        /// </summary>
        /// <returns> representing the asynchronous operation</returns>
        public async Task ImportationProcess()
        {
            await Task.Run(() =>
            {
                RenameInactiveLoginAndDeleteAffectationRoles();
            });
        }

        /// <summary>
        /// permet de utiliser un login inactive et supprimer une ligne d'affectation des roles'
        /// </summary>
        /// <param name="suffixDisableLogin">Suffixe à mettre pour les login désactivé</param>
        private void RenameInactiveLoginAndDeleteAffectationRoles(string suffixDisableLogin = "-old")
        {
            var listPersonnels = personnelManager.GetOutgoingPersonnelsList(suffixDisableLogin)?.ToList();

            if (listPersonnels != null && listPersonnels.Any())
            {
                List<UtilisateurEnt> usersToUpdate = new List<UtilisateurEnt>();
                List<ExternalDirectoryEnt> externalDirectoriesToUpdate = new List<ExternalDirectoryEnt>();
                List<AffectationSeuilUtilisateurEnt> affectationsToDelete = new List<AffectationSeuilUtilisateurEnt>();
                foreach (var personnel in listPersonnels)
                {
                    //update login of personnel
                    personnel.Utilisateur.Login = $"{personnel.Utilisateur.Login}{suffixDisableLogin}";
                    personnel.Utilisateur.SuperAdmin = false;
                    personnel.Utilisateur.IsDeleted = true;
                    usersToUpdate.Add(personnel.Utilisateur);

                    //update external directory
                    if (personnel.Utilisateur.ExternalDirectory != null)
                    {
                        personnel.Utilisateur.ExternalDirectory.IsActived = false;
                        externalDirectoriesToUpdate.Add(personnel.Utilisateur.ExternalDirectory);
                    }

                    //delete affectation roles organisation devis
                    if (personnel.Utilisateur.AffectationsRole != null && personnel.Utilisateur.AffectationsRole.Any())
                    {
                        affectationsToDelete.AddRange(personnel.Utilisateur.AffectationsRole);
                    }
                }

                utilisateurManager.UpdateUtilisateurList(usersToUpdate);
                externalDirectoryManager.UpdateExternalDirectoryList(externalDirectoriesToUpdate);
                affectationSeuilUtilisateurManager.DeleteAffectationList(affectationsToDelete);
            }
        }
    }
}
