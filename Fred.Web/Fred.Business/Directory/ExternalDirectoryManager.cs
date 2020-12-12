using System;
using System.Collections.Generic;
using Fred.Business.Authentification;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Directory;
using Fred.Web.Models.Authentification;

namespace Fred.Business.Directory
{
    /// <summary>
    ///   Gestionnaire de l'external directory.
    /// </summary>
    public class ExternalDirectoryManager : Manager<ExternalDirectoryEnt, IExternalDirectoryRepository>, IExternalDirectoryManager
    {
        public ExternalDirectoryManager(IUnitOfWork uow, IExternalDirectoryRepository externalDirectoryRepository)
         : base(uow, externalDirectoryRepository)
        { }

        /// <summary>
        /// Permet de mettre à jour le guid de l'external directory
        /// </summary>
        /// <param name="externalDirectoryEnt">external directory</param>
        /// <param name="guid">unique identifier</param>
        public void UpdateGuid(ExternalDirectoryEnt externalDirectoryEnt, Guid guid)
        {
            externalDirectoryEnt.Guid = guid.ToString();
            externalDirectoryEnt.DateExpirationGuid = DateTime.Now.AddMinutes(30);
            UpdateExternalDirectory(externalDirectoryEnt);
        }

        /// <summary>
        /// Permet de mettre à jour l'external directory
        /// </summary>
        /// <param name="externalDirectoryEnt">external directory</param>
        /// <returns>External directory modifié</returns>
        public ExternalDirectoryEnt UpdateExternalDirectory(ExternalDirectoryEnt externalDirectoryEnt)
        {
            Repository.UpdateExternalDirectory(externalDirectoryEnt);
            Save();

            return Repository.GetExternalDirectoryById(externalDirectoryEnt.FayatAccessDirectoryId);
        }

        /// <summary>
        /// Permet de vérifier si le GUID est valide et non expiré
        /// </summary>
        /// <param name="guid">Identifiant global unique</param>
        /// <returns>Le model NewPasswordViewModel</returns>
        public NewPasswordViewModel VerifyGuidValid(string guid)
        {
            ExternalDirectoryEnt externalDirectoryEnt = Repository.GetExternalDirectoryByGuid(guid);
            NewPasswordViewModel newPasswordViewModel = new NewPasswordViewModel();
            if (externalDirectoryEnt != null)
            {
                newPasswordViewModel.GuidIsValid = true;
                if (externalDirectoryEnt.DateExpirationGuid < DateTime.Now)
                {
                    newPasswordViewModel.GuidIsExpired = true;
                }
            }

            return newPasswordViewModel;
        }

        /// <summary>
        /// Permet de mettre à jour le mot de passe de l'utilisateur via la page NewPassword.cshtml
        /// </summary>
        /// <param name="model">NewPasswordViewModel</param>
        /// <returns>Le model NewPasswordViewModel</returns>
        public NewPasswordViewModel UpdatePassword(NewPasswordViewModel model)
        {
            ExternalDirectoryEnt externalDirectoryEnt = Repository.GetExternalDirectoryByGuid(model.Guid);
            externalDirectoryEnt.MotDePasse = model.Password;
            externalDirectoryEnt.Guid = null;
            externalDirectoryEnt.DateExpirationGuid = null;

            try
            {
                UpdateExternalDirectory(externalDirectoryEnt);
                model.Success = true;
                model.Message = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.UpdatePasswordSuccess);
            }
            catch (Exception)
            {
                model.Success = false;
                model.Message = AuthentificationManagerHelper.ConvertConnexionStatusToString(ConnexionStatus.TechnicalError);
            }

            return model;
        }

        /// <summary>
        /// Modifie une liste d'External diroctory liste
        /// </summary>
        /// <param name="externalDirectoriesToUpdate">Liste à modifier</param>
        public void UpdateExternalDirectoryList(List<ExternalDirectoryEnt> externalDirectoriesToUpdate)
        {
            Repository.UpdateExternalDirectoryList(externalDirectoriesToUpdate);
            Save();
        }
    }
}
