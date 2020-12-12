using Fred.Entities.Directory;
using Fred.Web.Models.Authentification;
using System;
using System.Collections.Generic;

namespace Fred.Business.Directory
{
    /// <summary>
    ///   Gestionnaire de l'external directory.
    /// </summary>
    public interface IExternalDirectoryManager : IManager<ExternalDirectoryEnt>
    {
        /// <summary>
        /// Permet de mettre à jour le guid de l'external directory
        /// </summary>
        /// <param name="externalDirectoryEnt">external directory</param>
        /// <param name="guid">unique identifier</param>
        void UpdateGuid(ExternalDirectoryEnt externalDirectoryEnt, Guid guid);

        /// <summary>
        /// Permet de vérifier si le GUID est valide et non expiré
        /// </summary>
        /// <param name="guid">Identifiant global unique</param>
        /// <returns>Le model NewPasswordViewModel</returns>
        NewPasswordViewModel VerifyGuidValid(string guid);

        /// <summary>
        /// Permet de mettre à jour le mot de passe de l'utilisateur via la page NewPassword.cshtml
        /// </summary>
        /// <param name="model">NewPasswordViewModel</param>
        /// <returns>Le model NewPasswordViewModel</returns>
        NewPasswordViewModel UpdatePassword(NewPasswordViewModel model);

        /// <summary>
        /// Modifie une liste d'External diroctory liste
        /// </summary>
        /// <param name="externalDirectoriesToUpdate">Liste à modifier</param>
        void UpdateExternalDirectoryList(List<ExternalDirectoryEnt> externalDirectoriesToUpdate);
    }
}
