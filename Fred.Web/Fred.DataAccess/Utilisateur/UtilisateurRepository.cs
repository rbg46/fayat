using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Utilisateur;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Web.Shared.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Utilisateur
{
    /// <summary>
    ///   Référentiel de données pour les Utilisateurs.
    /// </summary>
    public class UtilisateurRepository : FredRepository<UtilisateurEnt>, IUtilisateurRepository
    {
        private readonly ILogManager logManager;

        public UtilisateurRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Renvoi l'affectation passée en paramètre, après suppression de ces dépendances
        /// </summary>
        /// <param name="affectation">affectation dont on veut détacher les dépendances</param>
        private void DetachDependenciesAffectations(AffectationSeuilUtilisateurEnt affectation)
        {
            affectation.Devise = null;
            affectation.Role = null;
            affectation.Organisation = null;
            affectation.Utilisateur = null;
        }

        /// <summary>
        ///   Permet de détacher les entités dépendantes des commandes pour éviter de les prendre en compte dans la sauvegarde du
        ///   contexte.
        /// </summary>
        /// <param name="utilisateur">Commande dont les dépendances sont à détachées</param>
        private void DetachDependancies(UtilisateurEnt utilisateur)
        {
            utilisateur.ExternalDirectory = null;
            utilisateur.Personnel = null;
            utilisateur.AuteurCreation = null;
            utilisateur.AuteurModification = null;
            utilisateur.AuteurSuppression = null;
        }

        /// <summary>
        ///   Retourne la liste des tache.
        /// </summary>
        /// <returns>Liste des tache.</returns>
        public IEnumerable<UtilisateurEnt> GetList()
        {
            foreach (UtilisateurEnt utilisateur in Context.Utilisateurs.Include(s => s.ExternalDirectory))
            {
                yield return utilisateur;
            }
        }

        /// <summary>
        ///   Retourne la liste des utilisateurs pour la synchronisation vers le mobile.
        /// </summary>
        /// <returns>La liste des utilisateurs.</returns>
        public IEnumerable<UtilisateurEnt> GetListSync()
        {
            return Context.Utilisateurs.AsNoTracking();
        }

        /// <summary>
        ///   Retourne l'Utilisateur portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant de l'Utilisateur à trouver.</param>
        /// <returns>La fiche de l'Utilisateur, sinon nulle.</returns>
        public UtilisateurEnt GetById(int id)
        {
            UtilisateurEnt utilisateur =
              Context.Utilisateurs
                     .Include(e => e.ExternalDirectory)
                     .Include(p => p.Personnel)
                     .Include(a => a.AffectationsRole).ThenInclude(o => o.Organisation)
                     .Include(a => a.AffectationsRole).ThenInclude(r => r.Role)
                     .FirstOrDefault(u => u.UtilisateurId == id);

            //if (utilisateur?.AffectationsRole != null)
            //{
            //    foreach (AffectationSeuilUtilisateurEnt affectation in utilisateur.AffectationsRole)
            //    {
            //        affectation.Utilisateur = null;
            //    }
            //}

            return utilisateur;
        }

        /// <summary>
        ///   Retourne l'utilisateur portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à trouver.</param>
        /// <returns>La fiche de l'utilisateur, sinon nulle.</returns>
        public UtilisateurEnt GetByIdAsNoTracking(int id)
        {
            UtilisateurEnt utilisateur =
              Context.Utilisateurs
                     .Include(e => e.ExternalDirectory)
                     .Include(p => p.Personnel)
                     .Include(a => a.AffectationsRole).ThenInclude(o => o.Organisation)
                     .Include(a => a.AffectationsRole).ThenInclude(r => r.Role)
                     .AsNoTracking()
                     .FirstOrDefault(u => u.UtilisateurId == id);

            if (utilisateur?.AffectationsRole != null)
            {
                foreach (AffectationSeuilUtilisateurEnt affectation in utilisateur.AffectationsRole)
                {
                    affectation.Utilisateur = null;
                }
            }

            return utilisateur;
        }

        /// <summary>
        /// Vérifier si un personnel est un utilisateur Fred
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>Boolean indique si le personnel est un utilisateur Fred</returns>
        public bool IsFredUser(int personnelId)
        {
            return Context.Utilisateurs.Any(u => u.UtilisateurId == personnelId);
        }

        /// <summary>
        ///   Retourne l'Utilisateur portant le login de l'Utilisateur.
        /// </summary>
        /// <param name="login">Login de  l'Utilisateur à trouver.</param>
        /// <returns>La fiche de l'Utilisateur, sinon nulle.</returns>
        public UtilisateurEnt GetByLogin(string login)
        {
            return Context.Utilisateurs.Include(a => a.ExternalDirectory).FirstOrDefault(u => u.Login == login.Trim());
        }

        /// <summary>
        ///   Retourne l'utilisateur en fonction de son login.
        /// </summary>
        /// <param name="login">Login de l'utilisateur</param>
        /// <returns>L'utilisateur retrouvé en fonction de son login pour la vue ResetPassword</returns>
        public UtilisateurEnt GetByLoginForResetPassword(string login)
        {
            return Query()
                .Filter(u => u.Login == login.Trim())
                .Include(a => a.ExternalDirectory)
                .Include(a => a.Personnel)
                .Get()
                .FirstOrDefault();
        }

        /// <inheritdoc />
        public void DeleteUtilisateur(UtilisateurEnt utilisateur)
        {
            if (Context.Entry(utilisateur).State == EntityState.Detached)
            {
                DetachDependancies(utilisateur);
            }

            Update(utilisateur);
        }

        /// <summary>
        ///   Retourne l'utilisateur portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à trouver.</param>
        /// <returns>La fiche de l'utilisateur, sinon nulle.</returns>
        public UtilisateurEnt GetUtilisateurById(int id)
        {
            return
                Context.Utilisateurs.Where(x => x.UtilisateurId.Equals(id))
                    .Include(x => x.Personnel)
                    .Include(x => x.Personnel.EtablissementPaie)
                    .Include(x => x.Personnel.EtablissementRattachement)
                    .Include(x => x.Personnel.Societe)
                    .Include(x => x.Personnel.Societe.Organisation)
                    .Include(x => x.Personnel.Societe.Groupe)
                    .AsNoTracking()
                    .FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<UtilisateurEnt> GetUtilisateurByIdAsync(int id)
        {
            return await Context.Utilisateurs.Where(x => x.UtilisateurId.Equals(id))
                .Include(x => x.Personnel)
                .Include(x => x.Personnel.EtablissementPaie)
                .Include(x => x.Personnel.EtablissementRattachement)
                .Include(x => x.Personnel.Societe)
                .Include(x => x.Personnel.Societe.Organisation)
                .Include(x => x.Personnel.Societe.Groupe)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        /// <summary>
        ///   Retourne la liste des tache.
        /// </summary>
        /// <returns>Liste des tache.</returns>
        public IEnumerable<UtilisateurEnt> GetUtilisateurList()
        {
            return Context.Utilisateurs;
        }

        /// <summary>
        ///   Renvoi la liste des affectations de rôle pour un utilisateur
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur dont on veut les affectations</param>
        /// <returns>Une liste des affectations</returns>
        public IEnumerable<AffectationSeuilUtilisateurEnt> GetAffectationRoles(int utilisateurId)
        {
            var listAffectations = Context.UtilOrgaRoleDevises.Where(a => a.UtilisateurId == utilisateurId)
                                          .Include(a => a.Role)
                                          .Include(a => a.Organisation.TypeOrganisation)
                                          .Include(a => a.Organisation.CI)
                                          .Include(a => a.Organisation.Etablissement)
                                          .Include(a => a.Organisation.Societe)
                                          .Include(a => a.Organisation.OrganisationGenerique)
                                          .Include(a => a.Organisation.Groupe)
                                          .Include(a => a.Organisation.Pole)
                                          .Include(a => a.Organisation.Holding)
                                          .AsNoTracking()
                                          .ToList();

            foreach (AffectationSeuilUtilisateurEnt affectation in listAffectations)
            {
                if (affectation.Organisation != null)
                {
                    affectation.Organisation.ClearCircularReference();
                }
            }

            return listAffectations;
        }

        /// <summary>
        ///   Met à jour les rôle d'un Utilisateur
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur à mettre à jour</param>
        /// <param name="listAffectations">liste des affectations de l'utilisateur</param>
        public void UpdateRole(int utilisateurId, IEnumerable<AffectationSeuilUtilisateurEnt> listAffectations)
        {
            using (var contextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (AffectationSeuilUtilisateurEnt affectationRole in Context.UtilOrgaRoleDevises.Where(a => a.UtilisateurId == utilisateurId))
                    {
                        Context.UtilOrgaRoleDevises.Remove(affectationRole);
                    }

                    foreach (AffectationSeuilUtilisateurEnt affectation in listAffectations)
                    {
                        DetachDependenciesAffectations(affectation);
                        affectation.UtilisateurId = utilisateurId;
                        Context.UtilOrgaRoleDevises.Add(affectation);
                    }

                    Context.SaveChanges();
                    contextTransaction.Commit();
                }
                catch (Exception exception)
                {
                    contextTransaction.Rollback();
                    this.logManager.TraceException(exception.Message, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Update des roles pour une liste d'utilisateurs à la fois . Avec un seul appel au Save()
        /// </summary>
        /// <param name="affectationListByUser">Affectation list by user</param>
        public void UpdateRoleForUtilisateurList(Dictionary<int, List<AffectationSeuilUtilisateurEnt>> affectationListByUser)
        {
            using (var contextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    if (affectationListByUser == null || affectationListByUser.Count == 0)
                    {
                        return;
                    }

                    foreach (KeyValuePair<int, List<AffectationSeuilUtilisateurEnt>> item in affectationListByUser)
                    {
                        UpdateRoleForUser(item.Key, item.Value);
                    }

                    Context.SaveChanges();
                    contextTransaction.Commit();
                }
                catch (Exception exception)
                {
                    contextTransaction.Rollback();
                    this.logManager.TraceException(exception.Message, exception);
                    throw;
                }
            }

        }

        private void UpdateRoleForUser(int utilisateurId, IEnumerable<AffectationSeuilUtilisateurEnt> listAffectations)
        {
            List<AffectationSeuilUtilisateurEnt> userRoles = Context.UtilOrgaRoleDevises.Where(a => a.UtilisateurId == utilisateurId).ToList();

            IEnumerable<AffectationSeuilUtilisateurEnt> newAffectationList = listAffectations.Where(l =>
                            !userRoles.Any(u => u.OrganisationId == l.OrganisationId
                                                && u.RoleId == l.RoleId
                                                && u.UtilisateurId == l.UtilisateurId));

            foreach (AffectationSeuilUtilisateurEnt affectation in newAffectationList)
            {
                DetachDependenciesAffectations(affectation);
                affectation.UtilisateurId = utilisateurId;
                Context.UtilOrgaRoleDevises.Add(affectation);
            }
        }

        public void RemoveRoleAffectationForCiUserList(IEnumerable<int> utilisateurIdList, int organisationId, int roleId)
        {
            if (utilisateurIdList == null || !utilisateurIdList.Any())
            {
                return;
            }

            List<AffectationSeuilUtilisateurEnt> existingAffectations = Context.UtilOrgaRoleDevises.Where(a => utilisateurIdList
                                                                                        .Any(v => v == a.UtilisateurId)).ToList();

            List<AffectationSeuilUtilisateurEnt> affectationsToRemove = existingAffectations
                                                           .Where(a => a.RoleId == roleId && a.OrganisationId == organisationId)
                                                           .ToList();

            Context.UtilOrgaRoleDevises.RemoveRange(affectationsToRemove);
        }

        /// <summary>
        ///   Vérifie que l'utiisateur est SuperAdmin
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur</param>
        /// <returns>Vrai si l'utilisateur possède un rôle SuperAdmin</returns>
        public bool IsSuperAdmin(int utilisateurId)
        {
            return Context.Utilisateurs
                             .Where(u => u.UtilisateurId == utilisateurId && u.SuperAdmin)
                             .Any();
        }

        public bool DoesFolioExist(int userId, string folio, int userCompanyId)
        {
            return Context.Utilisateurs.Any(x => x.Folio != null &&
                                                 x.Folio.Equals(folio, StringComparison.CurrentCultureIgnoreCase) &&
                                                 x.Personnel.SocieteId == userCompanyId &&
                                                 x.UtilisateurId != userId);
        }

        /// <summary>
        /// Retourne la liste des Ci dont l'utilisateur est reponsable : Delegue ou responsable CI
        /// </summary>
        /// <param name="roles">Delegue role id</param>
        /// <param name="utilisateurId">Utilisateur Id</param>
        /// <returns>IEnumerable of Ci objects dont l'utilisateur est responsable</returns>
        public IEnumerable<CIEnt> GetCiListForRoles(IEnumerable<int> roles, int utilisateurId)
        {
            if (roles.IsNullOrEmpty())
            {
                return new List<CIEnt>();
            }

            var result = Context.UtilOrgaRoleDevises
                            .Include(v => v.Organisation)
                            .Where(u => u.UtilisateurId == utilisateurId
                                        && roles.Contains(u.RoleId)
                                        && u.Organisation != null
                                        && u.Organisation.CI != null)
                            .Select(o => o.Organisation.CI)
                            .Include(c => c.Societe)
                            .Include(c => c.EtablissementComptable)
                            .Include(c => c.CIType)
                            .ToList();

            return result;
        }

        /// <summary>
        /// Get ci list for delegue role
        /// </summary>
        /// <param name="utilisateurId">Utilisateur Id</param>
        /// <returns>IEnumerable of CiEnt</returns>
        public IEnumerable<CIEnt> GetCiListForDelegue(int utilisateurId)
        {
            var result = Context.Affectation
                            .Include(x => x.CI)
                            .Include(x => x.CI.CIType)
                            .Include(x => x.CI.EtablissementComptable)
                            .Include(x => x.CI.Societe)
                            .Where(a => a.IsDelegue && a.PersonnelId == utilisateurId && a.CI != null)
                            .Select(v => v.CI)
                            .ToList();

            return result;
        }

        /// <summary>
        /// Get affectation list of user (Delegue ou reponsable CI)
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="roles">Identifiant des rôles liés aux affectations</param>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <returns>IEnumerable of personnel Ent</returns>
        public IEnumerable<AffectationEnt> GetAffectationList(int userId, IEnumerable<int> roles, string personnelStatut)
        {
            IEnumerable<int> ciIdList = GetCiListForRoles(roles, userId)
                .Select(s => s.CiId);

            return Context.Affectation
                      .Include(a => a.Personnel)
                      .Include(a => a.Personnel.Societe)
                      .Include(a => a.Personnel.EtablissementPaie)
                      .Include(o => o.CI)
                      .Include(o => o.CI.CIType)
                      .Include(o => o.CI.EtablissementComptable)
                      .Include(o => o.CI.Societe)
                      .Where(a => a.CI != null && ciIdList.Contains(a.CiId) && a.Personnel != null && a.Personnel.Statut == personnelStatut)
                      .ToList();
        }

        // <summary>
        /// Modifie une liste d'utlidateur
        /// </summary>
        /// <param name="utilisateurList">Lsie d'utilisateur à modifier</param>
        /// <returns></returns>
        public void UpdateUtilisateurList(IEnumerable<UtilisateurEnt> utilisateurList)
        {
            if (utilisateurList != null && utilisateurList.Any())
            {
                foreach (var user in utilisateurList)
                {
                    Update(user);
                }
            }
        }
    }
}
