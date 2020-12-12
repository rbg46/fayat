using System.Collections.Generic;
using System.Linq;
using Fred.Business.SeuilValidation.Models;
using Fred.Entities.Personnel;
using Fred.Entities.Societe;

namespace Fred.Business.SeuilValidation.Services.Helpers
{
    /// <summary>
    /// Helper pour mapper les données de personnel vers PersonnelWithPermissionAndSeuilValidationResult
    /// et de peupler les certaines proprietes de PersonnelWithPermissionAndSeuilValidationResult
    /// </summary>
    public class PermissionAndSeuilValidationMapperHelper
    {

        /// <summary>
        /// Convertie des PersonnelEnt en   PersonnelWithPermissionAndSeuilValidationResult
        /// </summary>
        /// <param name="personnelsPageds">personnelsPageds</param>
        /// <returns>Liste de PersonnelWithPermissionAndSeuilValidationResult</returns>
        public List<PersonnelWithPermissionAndSeuilValidationResult> MapToPersonnelWithPermissionAndSeuilValidationResult(List<PersonnelEnt> personnelsPageds)
        {
            var result = new List<PersonnelWithPermissionAndSeuilValidationResult>();

            foreach (var personnel in personnelsPageds)
            {
                result.Add(new PersonnelWithPermissionAndSeuilValidationResult()
                {
                    PersonnelId = personnel.PersonnelId,
                    Email = personnel.Email,
                    Nom = personnel.Nom,
                    Prenom = personnel.Prenom,
                    Matricule = personnel.Matricule,
                });
            }
            return result;
        }



        /// <summary>
        /// Peuple le champ societe code
        /// </summary>
        /// <param name="personnelWithPermissionAndSeuils">personnelWithPermissionAndSeuils</param>
        /// <param name="societes">societes</param>
        /// <param name="personnels">personnels</param>
        /// <returns>le resultat</returns>
        public List<PersonnelWithPermissionAndSeuilValidationResult> AddTagSocieteCode(List<PersonnelWithPermissionAndSeuilValidationResult> personnelWithPermissionAndSeuils,
                                                                                        List<SocieteEnt> societes,
                                                                                        List<PersonnelEnt> personnels)
        {
            foreach (var personnelWithPermissionAndSeuil in personnelWithPermissionAndSeuils)
            {
                var personnel = personnels.FirstOrDefault(x => x.PersonnelId == personnelWithPermissionAndSeuil.PersonnelId);

                var societe = societes.FirstOrDefault(x => x.SocieteId == personnel.SocieteId);

                personnelWithPermissionAndSeuil.SocieteCode = societe.Code;
            }

            return personnelWithPermissionAndSeuils;
        }

        /// <summary>
        ///  Peuple le champ 'HaveMinimunSeuilValidation'
        /// </summary>
        /// <param name="personnelWithPermissionAndSeuils">personnelWithPermissionAndSeuils</param>
        /// <param name="seuilsValidations">seuilsValidations</param>
        /// <param name="seuilMinimum">seuilMinimum</param>
        /// <returns>le resultat</returns>
        public List<PersonnelWithPermissionAndSeuilValidationResult> AddTagHaveMinimunSeuilValidation(List<PersonnelWithPermissionAndSeuilValidationResult> personnelWithPermissionAndSeuils,
                                                                                                        List<SeuilValidationForUserResult> seuilsValidations,
                                                                                                        decimal seuilMinimum)
        {
            foreach (var personnelWithPermissionAndSeuil in personnelWithPermissionAndSeuils)
            {
                var seuil = seuilsValidations.FirstOrDefault(x => x.UtilisateurId == personnelWithPermissionAndSeuil.PersonnelId);

                personnelWithPermissionAndSeuil.HaveMinimunSeuilValidation = seuil.Seuil > seuilMinimum;
            }

            return personnelWithPermissionAndSeuils;
        }


    }
}
