using System.Collections.Generic;
using Fred.Business.SeuilValidation.Models;

namespace Fred.Business.SeuilValidation.Services.Interfaces
{
    /// <summary>
    /// Service qui fournie les utilisateur eyant une permission et un seuil de validation necessaire
    /// </summary>
    public interface IUtilisateursWithPermissionAndSeuilValidationProviderService : IService
    {

        /// <summary>
        /// fournie les utilisateur eyant une permission de voir la page de commande detail et un seuil de validation necessaire
        /// </summary>
        /// <param name="permissionAndSeuilValidationRequest">Donnés necessaire a la requetes</param>
        /// <returns>
        /// liste d'utilisateur eyant les drois et le seuil necessaire.
        /// Si la propriete IncludePersonnelWithoutSeuil est a true on retournera aussi les personnels qui n'ont pas la seuil 
        /// </returns>
        List<PersonnelWithPermissionAndSeuilValidationResult> GetUtilisateursWithPermissionToShowCommandeAndWithMinimunSeuilValidation(PersonnelWithPermissionAndSeuilValidationRequest permissionAndSeuilValidationRequest);
    }
}
