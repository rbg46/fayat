using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.Organisation.Tree;
using Fred.Business.Utilisateur;
using Fred.DataAccess.ExternalService.FredImportExport.Ci;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;

namespace Fred.Business.ExternalService.Ci
{
    public class CiManagerExterne : ICiManagerExterne
    {
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly ICIRepositoryExterne ciRepositoryExterne;
        private readonly ICIManager ciManager;
        private readonly IUtilisateurManager utilisateurManager;

        public CiManagerExterne(
            IOrganisationTreeService organisationTreeService,
            ICIRepositoryExterne ciRepositoryExterne,
            ICIManager ciManager,
            IUtilisateurManager utilisateurManager)
        {
            this.organisationTreeService = organisationTreeService;
            this.ciRepositoryExterne = ciRepositoryExterne;
            this.ciManager = ciManager;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Action qui est executée lors d'une mise a jour d'un ci depuis l'interface utilisateur
        /// Ici, je passe en parametre une Func de CIEnt car je ne voulais pas modifié le code existant 
        /// et execute le code apres avoir recupere le ci avant l'action de mise a jour
        /// </summary>
        /// <param name="ciId">L'id du ci</param>
        /// <param name="updateAction">Action de mise a jour cote fred</param>
        /// <returns>Le Ci mis a jour</returns>
        public async Task<CIEnt> OnUpdateCiAsync(int ciId, Func<CIEnt> updateAction)
        {
            CIEnt oldCi = ciManager.GetCiForCompare(ciId);
            CIEnt updatedCi = updateAction();
            bool dataToSynchroniseWithSapChanged = DataMustBeSynchronisedWithSapHasChanged(oldCi, updatedCi);
            if (!dataToSynchroniseWithSapChanged)
            {
                // je n'execute pas le reste du code dans un soucis de perf car ensuite on fait un appel a la base.
                return updatedCi;
            }

            // Seul le Groupe GRZB à cette fonctionnalité, cette regle sera aussi verifiée cote fred ie
            bool isInGroupWithFeacture = IsOnGroup(oldCi, Constantes.CodeGroupeRZB);
            if (isInGroupWithFeacture)
            {
                UtilisateurEnt userContext = utilisateurManager.GetContextUtilisateur();
                await UpdateCisAsync(userContext.UtilisateurId, new List<int>() { ciId });
            }
            return updatedCi;
        }

        /// <summary>
        /// Determine si le ci est dans un groupe donné
        /// </summary>
        /// <param name="ci">Le ci</param>
        /// <param name="groupCodes">Liste de code des groupes</param>
        /// <returns>true si le ci est dans un groupe donné</returns>
        private bool IsOnGroup(CIEnt ci, params string[] groupCodes)
        {
            OrganisationTree organisationTree = organisationTreeService.GetOrganisationTree();

            foreach (string codeGroupe in groupCodes)
            {
                OrganisationBase groupParent = organisationTree.GetGroupeParentOfCi(ci.CiId);
                bool isOnGroup = groupParent?.Code == codeGroupe;
                if (isOnGroup)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Ici, je compare les champs pour savoir si on doit faire une synchro
        /// </summary>
        /// <param name="oldCi">ancien ci</param>
        /// <param name="newCi">ci mis a jour depuis le front</param>
        /// <returns>true si un des champs est modifié</returns>
        private bool DataMustBeSynchronisedWithSapHasChanged(CIEnt oldCi, CIEnt newCi)
        {
            if (oldCi.ResponsableAdministratifId != newCi.ResponsableAdministratifId)
            {
                return true;
            }
            if (oldCi.ResponsableChantierId != newCi.ResponsableChantierId)
            {
                return true;
            }
            if (string.Compare(oldCi.Adresse, newCi.Adresse) != 0)
            {
                return true;
            }
            if (string.Compare(oldCi.Adresse2, newCi.Adresse2) != 0)
            {
                return true;
            }
            if (string.Compare(oldCi.Adresse3, newCi.Adresse3) != 0)
            {
                return true;
            }
            if (string.Compare(oldCi.Ville, newCi.Ville) != 0)
            {
                return true;
            }
            if (string.Compare(oldCi.CodePostal, newCi.CodePostal) != 0)
            {
                return true;
            }
            if (oldCi.PaysId != newCi.PaysId)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Envoie a fred IE
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <param name="ciIds">ciIds</param>
        private async Task UpdateCisAsync(int utilisateurId, List<int> ciIds)
        {
            try
            {
                await ciRepositoryExterne.UpdateCisAsync(utilisateurId, ciIds);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

    }
}
