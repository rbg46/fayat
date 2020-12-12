using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.RoleFonctionnalite;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Entities.Role;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;

namespace Fred.Business.Role
{
    /// <summary>
    ///   Gestionnaire des rôles
    /// </summary>
    public class RoleManager : Manager<RoleEnt, IRoleRepository>, IRoleManager
    {
        private readonly ISeuilValidationValidator seuilValidationValidator;
        private readonly IRoleFonctionnaliteManager roleFonctionnaliteManager;

        public RoleManager(
            IUnitOfWork uow,
            IRoleRepository roleRepo,
            IRoleValidator roleValidator,
            IRoleFonctionnaliteManager roleFonctionnaliteManager,
            ISeuilValidationValidator seuilValidationValidator)
          : base(uow, roleRepo, roleValidator)
        {
            this.roleFonctionnaliteManager = roleFonctionnaliteManager;
            this.seuilValidationValidator = seuilValidationValidator;
        }

        /// <summary>
        ///   Compare les niveau de rôle Paie
        /// </summary>
        /// <param name="first">Premier rôle</param>
        /// <param name="second">Second rôle</param>
        /// <returns>
        ///   Entier représentant la hiérarchie entre les deux rôles :
        ///   -1 : X est supérieur
        ///   0 : égalité de rôle
        ///   1 : y est supérieur
        /// </returns>
        public static int CompareRoleNiveauPaie(RoleEnt first, RoleEnt second)
        {
            return CompareRoleNiveau(first.NiveauPaie, second.NiveauPaie);
        }

        /// <summary>
        ///   Compare les niveau de rôle Comptabilité
        /// </summary>
        /// <param name="first">Premier rôle</param>
        /// <param name="second">Second rôle</param>
        /// <returns>
        ///   Entier représentant la hiérarchie entre les deux rôles :
        ///   -1 : X est supérieur
        ///   0 : égalité de rôle
        ///   1 : y est supérieur
        /// </returns>
        public static int CompareRoleNiveauCompta(RoleEnt first, RoleEnt second)
        {
            return CompareRoleNiveau(first.NiveauCompta, second.NiveauCompta);
        }

        /// <summary>
        ///   Compare deux niveaux
        /// </summary>
        /// <param name="n1">Premier niveau</param>
        /// <param name="n2">Second niveau</param>
        /// <returns>
        ///   Entier représentant la hiérarchie entre les deux rôles :
        ///   -1 : X est supérieur
        ///   0 : égalité de rôle
        ///   1 : y est supérieur
        /// </returns>
        private static int CompareRoleNiveau(int n1, int n2)
        {
            if (n1 > n2)
            {
                return -1;
            }

            return n1 == n2 ? 0 : 1;
        }

        #region Gestion des rôles

        /// <inheritdoc />
        public IEnumerable<RoleEnt> GetRoles(string code)
        {
            return Repository.GetRoles(code);
        }


        /// <summary>
        ///   Retourne la liste des rôles par societe
        /// </summary>
        /// <param name="societeId">Identifiant du societe</param>
        /// <returns>Liste des rôles d'une societe</returns>
        public IEnumerable<RoleEnt> GetRoleListBySocieteId(int societeId)
        {
            return Repository.GetRoleListBySocieteId(societeId).OrderBy(r => r.CodeNomFamilier);
        }

        /// <summary>
        ///   Ajoute un nouveau rôle
        /// </summary>
        /// <param name="roleEnt">Rôle à ajouter</param>
        /// <returns> L'identifiant du rôle ajouté</returns>
        public RoleEnt AddRole(RoleEnt roleEnt)
        {
            BusinessValidation(roleEnt);
            Repository.Insert(roleEnt);
            Save();

            return roleEnt;
        }

        /// <summary>
        ///   Met à jour un rôle
        /// </summary>
        /// <param name="role">rôle à mettre à jour</param>
        /// <returns>Role mis à jour</returns>
        /// <exception cref="System.Exception">
        ///   Les caractères spéciaux ne sont pas autorisés.
        ///   or
        ///   Le code et le libellé sont obligatoires.
        /// </exception>
        public RoleEnt UpdateRole(RoleEnt role)
        {
            BusinessValidation(role);
            Repository.Update(role);
            Save();

            return role;
        }

        /// <summary>
        ///   Supprime un rôle
        /// </summary>
        /// <param name="roleId">Identifiant du rôle à supprimer</param>
        /// <exception cref="FredBusinessException">
        ///   Impossible de supprimer ce Rôle car soit il a au moins un Module et/ou Seuil
        ///   associé(s), soit une Devise a été surchargée pour ce rôle, soit ce rôle est associé à un utilisateur.
        /// </exception>
        public void DeleteRole(int roleId)
        {
            if (Repository.IsDeletable(roleId))
            {
                Repository.DeleteById(roleId);
                Save();
            }
            else
            {
                throw new FredBusinessMessageResponseException(RoleResources.err_RoleDeletion);
            }
        }

        /// <summary>
        ///   Duplique un rôle ainsi que toutes les associations connexes (modules et seuils)
        /// </summary>
        /// <param name="roleEnt">Rôle à dupliquer</param>
        /// <param name="copythreshold">copie des seuils</param>
        /// <param name="copyRoleFeature">copie des roleFonctionnalites</param>
        /// <returns> L'identifiant du rôle dupliqué</returns>
        public RoleEnt DuplicateRole(RoleEnt roleEnt, bool copythreshold, bool copyRoleFeature)
        {
            int roleToDuplicateId = roleEnt.RoleId;

            var roleCopy = roleEnt;
            roleCopy.RoleId = 0;
            roleCopy.SeuilsValidation = null;

            BusinessValidation(roleCopy);
            Repository.Insert(roleCopy);
            Save();

            if (copyRoleFeature)
            {
                roleFonctionnaliteManager.DuplicateRoleFonctionnalites(sourceRoleId: roleToDuplicateId, targetRoleId: roleCopy.RoleId);
                Save();
            }

            if (copythreshold)
            {
                DuplicateSeuilValidations(sourceRoleId: roleToDuplicateId, targetRoleId: roleCopy.RoleId);
            }

            return roleCopy;
        }

        /// <summary>
        /// Clone les roles
        /// </summary>
        /// <param name="societeSourceId">societeSourceId</param>
        /// <param name="societeTargetId">societeTargetId</param>
        /// <param name="copyfeatures">copyfeatures</param>
        /// <param name="copythreshold">copythreshold</param>
        /// <returns>Liste de role</returns>
        public IEnumerable<RoleEnt> CloneRoles(int societeSourceId, int societeTargetId, bool copyfeatures, bool copythreshold)
        {
            var result = new List<RoleEnt>();
            var sourceRoles = GetRoleListBySocieteId(societeSourceId).Where(r => r.Actif).ToList();
            foreach (var sourceRole in sourceRoles)
            {
                var clonedRole = CloneRoleProperties(sourceRole, societeTargetId);
                BusinessValidation(clonedRole);
                Repository.Insert(clonedRole);
                Save();
                result.Add(clonedRole);

                if (copyfeatures)
                {
                    roleFonctionnaliteManager.DuplicateRoleFonctionnalites(sourceRoleId: sourceRole.RoleId, targetRoleId: clonedRole.RoleId);
                    Save();
                }

                if (copythreshold)
                {
                    DuplicateSeuilValidations(sourceRoleId: sourceRole.RoleId, targetRoleId: clonedRole.RoleId);
                }
            }

            return result;
        }

        private void DuplicateSeuilValidations(int sourceRoleId, int targetRoleId)
        {
            var seuils = GetSeuilValidationListByRoleId(sourceRoleId).ToList();
            foreach (SeuilValidationEnt seuil in seuils)
            {
                SeuilValidationEnt newSeuil = new SeuilValidationEnt
                {
                    RoleId = targetRoleId,
                    DeviseId = seuil.DeviseId,
                    Montant = seuil.Montant
                };
                AddSeuilValidation(newSeuil);
            }
        }

        private RoleEnt CloneRoleProperties(RoleEnt sourceRole, int societeTargetId)
        {
            var clonedRole = new RoleEnt();
            clonedRole.SocieteId = societeTargetId;
            clonedRole.Libelle = sourceRole.Libelle;
            clonedRole.Code = sourceRole.Code;
            clonedRole.CodeNomFamilier = sourceRole.CodeNomFamilier;
            clonedRole.Actif = true;
            clonedRole.NiveauPaie = sourceRole.NiveauPaie;
            clonedRole.NiveauCompta = sourceRole.NiveauCompta;
            return clonedRole;
        }

        /// <summary>
        ///   Recherche de rôles dans le référentiel
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="societeId">Identifiant du groupe auquel appartient le rôle</param>
        /// <returns>Une liste des rôles trouvés</returns>
        public IEnumerable<RoleEnt> SearchLight(string text, int page, int pageSize, int? societeId)
        {
            text = text.ToLower();
            return Repository.Query()
                            .Filter(p => string.IsNullOrEmpty(text)
                              || p.CodeNomFamilier.ToLower().Contains(text)
                              || p.Code.ToLower().Contains(text)
                              || p.Libelle.ToLower().Contains(text))
                            .Filter(p => societeId.HasValue && p.SocieteId == societeId)
                            .OrderBy(list => list.OrderBy(pe => pe.CodeNomFamilier))
                            .GetPage(page, pageSize);
        }

        /// <summary>
        /// Get utilisateur role list
        /// </summary>
        /// <param name="utilisateurId">Utilisateur id</param>
        /// <param name="roleId">Role id</param>
        /// <returns>IEnumerable of AffectationSeuilUtilisateurEnt</returns>
        public IEnumerable<AffectationSeuilUtilisateurEnt> GetUtilisateurRoleList(int utilisateurId, int roleId)
        {
            return Repository.GetUtilisateurRoleList(utilisateurId, roleId);
        }

        /// <summary>
        /// Get delegue role by organisation id
        /// </summary>
        /// <param name="organisationId">Organisation id</param>
        /// <returns>Delegue role id by organisation</returns>
        public RoleEnt GetDelegueRoleByOrganisationId(int organisationId)
        {
            return Repository.GetDelegueRoleByOrganisationId(organisationId);
        }

        /// <summary>
        /// Vérifier si le role est un Délegue
        /// </summary>
        /// <param name="roleId">Role identifier</param>
        /// <returns>True si le role est delegue</returns>
        public bool IsRoleDelegue(int roleId)
        {
            return Repository.IsRoleDelegue(roleId);
        }

        #endregion

        #region Gestion des seuils

        /// <summary>
        ///   Retourne la liste des seuils de validation associés au rôle
        /// </summary>
        /// <param name="roleId">ID du rôle pour lequel on recherche les seuils</param>
        /// <returns>Liste de seuils de validation</returns>
        public IEnumerable<SeuilValidationEnt> GetSeuilValidationListByRoleId(int roleId)
        {
            return Repository.GetSeuilValidationListByRoleId(roleId);
        }

        /// <summary>
        ///   Retourne un seuil à partir de son identifiant
        /// </summary>
        /// <param name="seuilId">Identifiant du seuil recherché </param>
        /// <returns>Un seuil d'après son identifiant</returns>
        public SeuilValidationEnt GetSeuilValidationById(int seuilId)
        {
            return Repository.GetSeuilValidationById(seuilId);
        }

        /// <summary>
        ///   Ajoute un nouveau seuil de validation pour le rôle
        /// </summary>
        /// <param name="seuil">Seuil à ajouter</param>
        /// <exception cref="ValidationException">Validation Exceptions</exception>
        /// <returns> ID du seuil créé </returns>
        public SeuilValidationEnt AddSeuilValidation(SeuilValidationEnt seuil)
        {
            ValidationResult result = seuilValidationValidator.Validate(seuil);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            Repository.AddSeuilValidation(seuil);
            Save();

            return Repository.GetSeuilValidationById(seuil.SeuilValidationId);
        }

        /// <summary>
        ///   Met à rout un seuil de validation
        /// </summary>
        /// <param name="seuil">Seuil à mettre à jour</param>
        /// <exception cref="ValidationException">ValidationException</exception>
        /// <returns>Seuil de validation mis à jour</returns>
        public SeuilValidationEnt UpdateSeuilValidation(SeuilValidationEnt seuil)
        {
            ValidationResult result = seuilValidationValidator.Validate(seuil);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            Repository.UpdateSeuilValidation(seuil);
            Save();

            return Repository.GetSeuilValidationById(seuil.SeuilValidationId);
        }

        /// <summary>
        ///   Supprime un seuil
        /// </summary>
        /// <param name="seuilId">ID seuil à supprimer</param>
        public void DeleteSeuilValidationById(int seuilId)
        {
            Repository.DeleteSeuilValidationById(seuilId);
            Save();
        }

        #endregion
    }
}
