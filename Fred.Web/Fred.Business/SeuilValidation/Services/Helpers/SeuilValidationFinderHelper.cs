using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.SeuilValidation.Models;
using Fred.Entities.Organisation;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;

namespace Fred.Business.SeuilValidation.Services.Helpers
{
    /// <summary>
    /// Helper qui permet de trouver le seuil de validation 
    /// - Soit sur l'utilisateur
    /// - soit sur l'orga
    /// - soit sur le role
    /// </summary>
    public class SeuilValidationFinderHelper
    {
        /// <summary>
        /// Permet de de trouver le seuil de validation  sur l'utilisateur
        /// </summary>
        /// <param name="utilisateurId">l'id de l utilisateur</param>
        /// <param name="deviseId">La devise</param>
        /// <param name="parentsOfCi">les orga parent du ci</param>
        /// <param name="affectationSeuilUtilisateurOnTree">la liste des AffectationSeuilUtilisateurEnt</param>
        /// <returns>SeuilValidationForUserResult</returns>
        public SeuilValidationForUserResult GetSeuilValidationNiveauUtilisateurOnTree(int utilisateurId, int deviseId, List<OrganisationBase> parentsOfCi, List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurOnTree)
        {
            SeuilValidationForUserResult seuilValidationForUserResult = CreateDefaultSeuilValidationForUserResult(utilisateurId, deviseId);

            foreach (var organisation in parentsOfCi)
            {
                var seuilValidationNiveauUtilisateur = GetSeuilValidationNiveauUtilisateur(utilisateurId, organisation.OrganisationId, deviseId, affectationSeuilUtilisateurOnTree);

                if (seuilValidationNiveauUtilisateur.HasValue)
                {
                    seuilValidationForUserResult.Seuil = seuilValidationNiveauUtilisateur.Value;
                    break;
                }
            }
            return seuilValidationForUserResult;
        }

        private decimal? GetSeuilValidationNiveauUtilisateur(int utilisateurId, int organisationId, int deviseId, List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurs)
        {
            AffectationSeuilUtilisateurEnt affectation = affectationSeuilUtilisateurs.FirstOrDefault(x => x.UtilisateurId == utilisateurId && x.OrganisationId == organisationId && x.DeviseId == deviseId);

            return affectation?.CommandeSeuil;
        }

        /// <summary>
        /// Permet de de trouver le seuil de validation  sur l'organisation
        /// </summary>
        /// <param name="utilisateurId">l'id de l utilisateur</param>
        /// <param name="deviseId">La devise</param>
        /// <param name="parentsOfCi">les orga parent du ci</param>
        /// <param name="affectationSeuilUtilisateurOnTree">la liste des AffectationSeuilUtilisateurEnt</param>
        /// <param name="affectationSeuilOrgas">Liste des AffectationSeuilOrgaEnt</param>
        /// <returns>SeuilValidationForUserResult</returns>
        public SeuilValidationForUserResult GetSeuilValidationNiveauOrganisationOnTree(int utilisateurId, int deviseId, List<OrganisationBase> parentsOfCi, List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurOnTree, List<AffectationSeuilOrgaEnt> affectationSeuilOrgas)
        {
            SeuilValidationForUserResult seuilValidationForUserResult = CreateDefaultSeuilValidationForUserResult(utilisateurId, deviseId);

            foreach (var organisation in parentsOfCi)
            {
                var seuilValidationNiveauOrganisation = GetSeuilValidationNiveauOrganisation(utilisateurId, organisation.OrganisationId, deviseId, affectationSeuilUtilisateurOnTree, affectationSeuilOrgas);

                if (seuilValidationNiveauOrganisation.HasValue)
                {
                    seuilValidationForUserResult.Seuil = seuilValidationNiveauOrganisation.Value;
                    break;
                }

            }
            return seuilValidationForUserResult;
        }

        private decimal? GetSeuilValidationNiveauOrganisation(int utilisateurId, int organisationId, int deviseId, List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurs, List<AffectationSeuilOrgaEnt> affectationSeuilOrgas)
        {
            var affectation = affectationSeuilUtilisateurs.FirstOrDefault(x => x.UtilisateurId == utilisateurId && x.OrganisationId == organisationId && x.DeviseId == deviseId);

            if (affectation != null)
            {
                var affectationOfOrganisation = affectationSeuilOrgas.FirstOrDefault(x => x.RoleId == affectation.RoleId && x.OrganisationId == organisationId && x.DeviseId == deviseId);

                if (affectationOfOrganisation != null)
                {
                    return affectationOfOrganisation.Seuil;
                }
            }
            return null;
        }

        /// <summary>
        /// Permet de de trouver le seuil de validation  sur le role
        /// </summary>
        /// <param name="utilisateurId">l'id de l utilisateur</param>
        /// <param name="deviseId">La devise</param>
        /// <param name="parentsOfCi">les parent des ci</param>
        /// <param name="affectationSeuilUtilisateurOnTree">affectationSeuilUtilisateurOnTree</param>
        /// <param name="seuilValidations">seuilValidations</param>      
        /// <returns>SeuilValidationForUserResult</returns>
        public SeuilValidationForUserResult GetSeuilValidationNiveauRoleOnTree(int utilisateurId, int deviseId, List<OrganisationBase> parentsOfCi, List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurOnTree, List<SeuilValidationEnt> seuilValidations)
        {
            SeuilValidationForUserResult seuilValidationForUserResult = CreateDefaultSeuilValidationForUserResult(utilisateurId, deviseId);

            foreach (var organisation in parentsOfCi)
            {
                var seuilValidationNiveauOrganisation = GetSeuilValidationNiveauRole(utilisateurId, organisation.OrganisationId, deviseId, affectationSeuilUtilisateurOnTree, seuilValidations);

                if (seuilValidationNiveauOrganisation.HasValue)
                {
                    seuilValidationForUserResult.Seuil = seuilValidationNiveauOrganisation.Value;
                    break;
                }
            }
            return seuilValidationForUserResult;
        }

        private decimal? GetSeuilValidationNiveauRole(int utilisateurId, int organisationId, int deviseId, List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurs, List<SeuilValidationEnt> seuilValidations)
        {
            var affectation = affectationSeuilUtilisateurs.Find(x => x.UtilisateurId == utilisateurId && x.OrganisationId == organisationId);

            if (affectation != null)
            {
                SeuilValidationEnt seuilValidation = seuilValidations.Find(x => x.RoleId == affectation.RoleId && x.DeviseId == deviseId);

                if (seuilValidation != null)
                {
                    return Convert.ToInt32(seuilValidation.Montant);
                }
            }
            return null;
        }

        private SeuilValidationForUserResult CreateDefaultSeuilValidationForUserResult(int utilisateurId, int deviseId)
        {
            SeuilValidationForUserResult seuilValidationForUserResult = new SeuilValidationForUserResult();
            seuilValidationForUserResult.UtilisateurId = utilisateurId;
            seuilValidationForUserResult.DeviseId = deviseId;
            seuilValidationForUserResult.Seuil = null;
            return seuilValidationForUserResult;
        }
    }
}
