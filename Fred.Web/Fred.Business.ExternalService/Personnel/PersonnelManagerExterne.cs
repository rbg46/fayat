using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.CI.Services;
using Fred.Business.Organisation.Tree;
using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.DataAccess.ExternalService.FredImportExport.Personnel;
using Fred.Entities;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Permission;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Models.Societe;

namespace Fred.Business.ExternalService.Personnel
{
    /// <summary>
    /// Manager externe pour les cis
    /// </summary>
    public class PersonnelManagerExterne : IPersonnelManagerExterne
    {
        private readonly IPersonnelManager personnelManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICIManager ciManager;
        private readonly ICisAccessiblesService cisAccessiblesService;
        private readonly IPersonnelRepositoryExterne personnelRepositoryExterne;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="personnelRepositoryExterne">personnelRepositoryExterne</param>
        /// <param name="habilitationManager">habilitationManager</param>
        /// <param name="personnelManager">personnelManager</param>
        /// <param name="organisationTreeService">organisationTreeService</param>
        /// <param name="utilisateurManager">utilisateurManager</param>
        /// <param name="ciManager">ciManager</param>
        /// <param name="cisAccessiblesService">cisAccessiblesService</param>
        public PersonnelManagerExterne(IPersonnelRepositoryExterne personnelRepositoryExterne,
             IPersonnelManager personnelManager,
             IOrganisationTreeService organisationTreeService,
             IUtilisateurManager utilisateurManager,
             ICIManager ciManager,
             ICisAccessiblesService cisAccessiblesService
            )
        {
            this.personnelManager = personnelManager;
            this.organisationTreeService = organisationTreeService;
            this.utilisateurManager = utilisateurManager;
            this.ciManager = ciManager;
            this.cisAccessiblesService = cisAccessiblesService;
            this.personnelRepositoryExterne = personnelRepositoryExterne;
        }


        /// <summary>
        /// Action qui est executée lors d'une mise a jour d'un personnel depuis l'interface utilisateur
        /// Ici, je passe en parametre une Func de PersonnelEnt car je ne voulais pas modifié le code existant 
        /// et execute le code apres avoir recupere le ci avant l'action de mise a jour
        /// </summary>
        /// <param name="personnelId">L'id du personnel</param>
        /// <param name="updateAction">Action de mise a jour cote fred</param>
        /// <returns>Le personnel mis a jour</returns>
        public async Task<PersonnelEnt> OnUpdatePersonnelAsync(int personnelId, Func<PersonnelEnt> updateAction)
        {
            PersonnelEnt oldPersonnel = personnelManager.GetPersonnelForCompare(personnelId);
            PersonnelEnt updatedPersonnel = updateAction();

            //le flux PA30 ne doit être déclenché que lors de la mise à jour d’un Personnel interne
            if (updatedPersonnel.IsInterne == false)
            {
                return updatedPersonnel;
            }
            bool dataToSynchroniseWithSapChanged = DataMustBeSynchronisedWithSapHasChanged(oldPersonnel, updatedPersonnel);
            if (!dataToSynchroniseWithSapChanged)
            {
                // je n'execute pas le reste du code dans un soucis de perf car ensuite on fait un appel a la base.
                return updatedPersonnel;
            }
            // Seul les Groupe GRZB et GFTP ont   cette fonctionnalité, cette regle sera aussi verifiée cote fred ie
            bool isInGroupWithFeacture = IsOnGroup(oldPersonnel, Constantes.CodeGroupeRZB, Constantes.CodeGroupeFTP);
            if (isInGroupWithFeacture)
            {
                await UpdatePersonnelsAsync(new List<int>() { personnelId });
            }

            return updatedPersonnel;
        }

        /// <summary>
        /// Determine si la societé du personnel est dans un groupe donné
        /// </summary>
        /// <param name="personnel">Le personnel</param>
        /// <param name="groupCodes">Liste de code des groupes</param>
        /// <returns>true si la societé du personnel est dans un groupe donné</returns>
        private bool IsOnGroup(PersonnelEnt personnel, params string[] groupCodes)
        {
            OrganisationTree organisationTree = organisationTreeService.GetOrganisationTree();
            OrganisationBase groupParent = organisationTree.GetGroupeParentOfSociete(personnel.SocieteId ?? 0);
            foreach (string codeGroupe in groupCodes)
            {
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
        /// <param name="oldPersonnel">ancien ci</param>
        /// <param name="newPersonnel">ci mis a jour depuis le front</param>
        /// <returns>true si un des champs est modifié</returns>
        private bool DataMustBeSynchronisedWithSapHasChanged(PersonnelEnt oldPersonnel, PersonnelEnt newPersonnel)
        {
            if (oldPersonnel.Statut != newPersonnel.Statut)
            {
                return true;
            }
            if (oldPersonnel.EtablissementPaieId != newPersonnel.EtablissementPaieId)
            {
                return true;
            }
            if (string.Compare(oldPersonnel.Adresse1, newPersonnel.Adresse1) != 0)
            {
                return true;
            }
            if (string.Compare(oldPersonnel.CodePostal, newPersonnel.CodePostal) != 0)
            {
                return true;
            }
            if (string.Compare(oldPersonnel.Ville, newPersonnel.Ville) != 0)
            {
                return true;
            }
            if (oldPersonnel.PaysId != newPersonnel.PaysId)
            {
                return true;
            }
            if (oldPersonnel.Email != newPersonnel.Email)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Envoie a fred IE
        /// </summary>
        /// <param name="personnelsIds">ciIds</param>
        private async Task UpdatePersonnelsAsync(List<int> personnelsIds)
        {
            try
            {
                await personnelRepositoryExterne.UpdatePersonnelsAsync(personnelsIds);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <summary>
        /// Export des réceptions Intérimaires
        /// </summary>
        /// <param name="societes">Liste des sociétés</param>
        public async Task ExportReceptionInterimairesAsync(List<SocieteModel> societes)
        {
            UtilisateurEnt user = await utilisateurManager.GetContextUtilisateurAsync();
            int userId = user.UtilisateurId;
            try
            {
                if (user.SuperAdmin)
                {
                    await personnelRepositoryExterne.ExportReceptionInterimairesAsync(societes.Select(s => s.SocieteId).ToList(), userId);
                }
                else
                {
                    //un job par societe
                    foreach (int societeId in societes.Select(s => s.SocieteId))
                    {
                        var ciIds = new List<int>();

                        List<int> cisForUser = cisAccessiblesService.GetCisAccessiblesForUserAndPermission(userId, PermissionKeys.AffichageMenuCIIndex)
                                                            .Select(ci => ci.Id)
                                                            .ToList();

                        ciIds.AddRange(ciManager.GetCiIdListBySocieteId(societeId).Where(c => cisForUser.Contains(c)));
                        // N'envoyer que les ci avec rapport vérouillé et reception non envoyé
                        ciIds = cisAccessiblesService.GetCiIdsAvailablesForReceptionInterimaire(ciIds);

                        await personnelRepositoryExterne.ExportReceptionInterimairesByCisAsync(ciIds, userId);
                    }
                }
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

    }
}
