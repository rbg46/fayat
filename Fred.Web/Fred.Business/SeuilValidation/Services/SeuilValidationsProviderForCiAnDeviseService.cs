using System.Collections.Generic;
using System.Linq;
using Fred.Business.AffectationSeuilOrga;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.Organisation.Tree;
using Fred.Business.SeuilValidation.Manager;
using Fred.Business.SeuilValidation.Models;
using Fred.Business.SeuilValidation.Services.Helpers;
using Fred.Business.SeuilValidation.Services.Interfaces;

namespace Fred.Business.SeuilValidation.Services
{
    /// <summary>
    /// Service qui Recupere toutes les seuils de validation pour un ci et une devise donnée
    /// </summary>
    public class SeuilValidationsProviderForCiAnDeviseService : ISeuilValidationsProviderForCiAnDeviseService
    {
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager;
        private readonly IAffectationSeuilOrgaManager affectationSeuilOrgaManager;
        private readonly ISeuilValidationManager seuilValidationManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="organisationTreeService">organisationTreeService</param>
        /// <param name="affectationSeuilUtilisateurManager">affectationSeuilUtilisateurManager</param>
        /// <param name="affectationSeuilOrgaManager">affectationSeuilOrgaManager</param>
        /// <param name="seuilValidationManager">seuilValidationManager</param>
        public SeuilValidationsProviderForCiAnDeviseService(IOrganisationTreeService organisationTreeService,
            IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager,
            IAffectationSeuilOrgaManager affectationSeuilOrgaManager,
            ISeuilValidationManager seuilValidationManager)
        {
            this.organisationTreeService = organisationTreeService;
            this.affectationSeuilUtilisateurManager = affectationSeuilUtilisateurManager;
            this.affectationSeuilOrgaManager = affectationSeuilOrgaManager;
            this.seuilValidationManager = seuilValidationManager;
        }

        /// <summary>
        /// Recupere toutes les seuilS de validation pour un ci et une devise donnée
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="deviseId">deviseId</param>
        /// <returns>liste de SeuilValidationForUserResult </returns>
        public List<SeuilValidationForUserResult> GetUsersWithSeuilValidationsOnCi(int ciId, int deviseId)
        {
            var result = new List<SeuilValidationForUserResult>();

            var organisationTree = this.organisationTreeService.GetOrganisationTree();

            var parentsOfCi = organisationTree.GetAllParentsOfCi(ciId);

            // Recuperation des ids pour faire les requetes

            var affectationSeuilUtilisateurIds = parentsOfCi.SelectMany(x => x.Affectations).Select(x => x.AffectationId).ToList();

            var rolesOfAffectations = parentsOfCi.SelectMany(x => x.Affectations).Select(x => x.RoleId).Distinct().ToList();

            var parentOrganisationIdsOfCi = parentsOfCi.Select(x => x.OrganisationId).ToList();

            var utilisateurIds = parentsOfCi.SelectMany(x => x.Affectations).Select(x => x.UtilisateurId).OrderBy(x => x).Distinct().ToList();

            // Execution des requetes

            var affectationSeuilOrgas = affectationSeuilOrgaManager.Get(deviseId, parentOrganisationIdsOfCi, rolesOfAffectations);

            var seuilValidations = seuilValidationManager.Get(deviseId, rolesOfAffectations);

            var affectationSeuilUtilisateurOnTree = affectationSeuilUtilisateurManager.Get(affectationSeuilUtilisateurIds);

            // execution de l'aglo de recherche de seuil

            var finder = new SeuilValidationFinderHelper();

            foreach (var utilisateurId in utilisateurIds)
            {
                var seuilValidationForUserResult = finder.GetSeuilValidationNiveauUtilisateurOnTree(utilisateurId, deviseId, parentsOfCi, affectationSeuilUtilisateurOnTree);

                if (HasSeuilValidation(seuilValidationForUserResult))
                {
                    result.Add(seuilValidationForUserResult);
                    continue;
                }

                seuilValidationForUserResult = finder.GetSeuilValidationNiveauOrganisationOnTree(utilisateurId, deviseId, parentsOfCi, affectationSeuilUtilisateurOnTree, affectationSeuilOrgas);

                if (HasSeuilValidation(seuilValidationForUserResult))
                {
                    result.Add(seuilValidationForUserResult);
                    continue;
                }

                seuilValidationForUserResult = finder.GetSeuilValidationNiveauRoleOnTree(utilisateurId, deviseId, parentsOfCi, affectationSeuilUtilisateurOnTree, seuilValidations);

                if (HasSeuilValidation(seuilValidationForUserResult))
                {
                    result.Add(seuilValidationForUserResult);
                }

            }
            return result;
        }

        private bool HasSeuilValidation(SeuilValidationForUserResult seuilValidationForUserResult)
        {
            if (seuilValidationForUserResult.Seuil != null)
            {
                return true;
            }
            return false;
        }

    }
}
