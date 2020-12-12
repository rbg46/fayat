using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Role;
using Fred.Entities.RoleFonctionnalite;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Utilisateur
{
    /// <summary>
    ///   Interface des gestionnaires des utilisateurs
    /// </summary>
    public interface IUtilisateurManager : IManager<UtilisateurEnt>
    {
        /// <summary>
        ///   Retourne la liste des utilisateurs.
        /// </summary>
        /// <returns>Renvoie la liste des utilisateurs.</returns>
        IEnumerable<UtilisateurEnt> GetUtilisateurList();

        /// <summary>
        ///   Récupère la liste de tous les utilisateurs
        /// </summary>
        /// <returns>Renvoie la liste des utilisateurs</returns>
        IEnumerable<UtilisateurEnt> GetList();

        /// <summary>
        ///   Récupère la liste de tous les utilisateurs pour la synchronisation vers le mobile.
        /// </summary>
        /// <returns>Renvoie la liste des utilisateurs</returns>
        IEnumerable<UtilisateurEnt> GetListSync();

        /// <summary>
        ///   Retourne le utilisateurs dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur à retrouver.</param>
        /// <returns>L'utilisateur retrouvé, sinon vide</returns>
        UtilisateurEnt GetUtilisateurById(int utilisateurId);

        /// <summary>
        ///   Retourne l'utilisateur en fonction du Personnel ID.
        /// </summary>
        /// <param name="login">Identifiant du société à retrouver.</param>
        /// <returns>Le société retrouvée, sinon nulle.</returns>
        UtilisateurEnt GetByLogin(string login);

        /// <summary>
        ///   Retourne l'utilisateur en fonction de son login.
        /// </summary>
        /// <param name="login">Login de l'utilisateur</param>
        /// <returns>L'utilisateur retrouvé en fonction de son login pour la vue ResetPassword</returns>
        UtilisateurEnt GetByLoginForResetPassword(string login);

        /// <summary>
        ///   Ajoute un nouvel utilisateur.
        /// </summary>
        /// <param name="utilisateurEnt"> Utilisateur à ajouter.</param>
        /// <returns>Utilisateur ajouté.</returns>
        UtilisateurEnt AddUtilisateur(UtilisateurEnt utilisateurEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'un utilisateur.
        /// </summary>
        /// <param name="utilisateurEnt">Utilisateur à modifier</param>
        /// <returns>Utilisateur mis à jour</returns>
        UtilisateurEnt UpdateUtilisateur(UtilisateurEnt utilisateurEnt);

        /// <summary>
        /// Modifie une liste d'utlidateur
        /// </summary>
        /// <param name="utilisateurList">Liste d'utilisateur à modifier</param>
        /// <returns></returns>
        void UpdateUtilisateurList(IEnumerable<UtilisateurEnt> utilisateurList);

        /// <summary>
        ///   Mise à jour de la dernière date de connexion
        /// </summary>
        /// <param name="utilisateurId">Utilisateur connecté</param>
        /// <param name="dateDerniereCo">Date de dernière connexion</param>
        /// <returns>Vrai si la mise à jour est effectuée, sinon faux</returns>
        bool UpdateDateDerniereConnexion(int utilisateurId, DateTime dateDerniereCo);

        /// <summary>
        ///   Supprime un utilisateur.
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur à supprimer.</param>   
        void DeleteUtilisateurById(int utilisateurId);

        /// <summary>
        ///   Retourne l'utilisateur dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="id">Identifiant du UtilisateurFredId.</param>
        /// <param name="asNoTracking">Sans tracking par défaut à oui</param>
        /// <returns>Le détail utilisateur Fred  retrouvée, sinon nulle.</returns>
        UtilisateurEnt GetById(int id, bool asNoTracking = false);

        /// <summary>
        ///   Renvoi la liste des affectations de rôle pour un utilisateur regroupement par role ensuite par orga
        /// </summary>
        /// <param name="utilisateurId">L'identifiant utilisateur.</param>
        /// <returns>Liste de roles</returns>
        IEnumerable<RoleEnt> GetRoleOrganisationAffectations(int utilisateurId);
        /// <summary>
        ///   Met à jour les rôle d'un Utilisateur
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur à mettre à jour</param>
        /// <param name="listAffectations">liste des affectations de l'utilisateur</param>
        void UpdateRole(int utilisateurId, IEnumerable<AffectationSeuilUtilisateurEnt> listAffectations);

        /// <summary>
        /// Add or remove role delegue for ci personnel
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation du CI</param>
        /// <param name="utilisateurIdLisToAdd">Utilisateur id list to add delegation</param>
        /// <param name="utilisateurIdListToRemove">Utilisateur id list to remove delegation</param>
        void ManageRoleDelegueForCiPersonnel(int organisationId, IEnumerable<int> utilisateurIdLisToAdd, IEnumerable<int> utilisateurIdListToRemove);

        /// <summary>
        ///   Obtient  l'ID de l'utilisateur en cours dans le contexte Claims d'habilitation
        /// </summary>
        /// <returns>Identifiant de l'utilisateur</returns>
        int GetContextUtilisateurId();

        /// <summary>
        ///   Obtient le détail d'un utilisateur en cours dans le contexte Claims d'habilitation
        /// </summary>
        /// <returns>UtilisateurEnt Détail d'un utilisateur</returns>
        [Obsolete("Prefer use GetContextUtilisateurAsync instead")]
        UtilisateurEnt GetContextUtilisateur();

        /// <summary>
        ///   Obtient le détail d'un utilisateur en cours dans le contexte Claims d'habilitation de manière asynchrone
        /// </summary>
        /// <returns>UtilisateurEnt Détail d'un utilisateur</returns>
        Task<UtilisateurEnt> GetContextUtilisateurAsync();

        /// <summary>
        ///   Renvoi la liste des organisations d'un Utilisateur pour un type d'orgnisation
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur</param>
        /// <param name="typeOrganisationId">Id du type d'organisation</param>
        /// <returns>Une liste d'organisations</returns>
        IEnumerable<OrganisationLightEnt> GetOrganisationAvailableByUserAndByTypeOrganisation(int utilisateurId, int typeOrganisationId);

        /// <summary>
        ///   Retourne le seuil du montant de la commande pour un utilisateur donné
        /// </summary>
        /// <param name="userId"> Identifiant de l'utilisateur </param>
        /// <param name="ciId"> Identifiant du CI</param>
        /// <param name="deviseId"> Identifiant de la devise</param>
        /// <returns> Le seuil de validation de l'utilisateur sur cette commande </returns>
        decimal GetSeuilValidation(int userId, int ciId, int deviseId);

        /// <summary>
        ///   Retourne le plus haut niveau de paie d'un utilisateur
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="orgaId">Identifiant de l'organisation</param>
        /// <returns>Le plus haut niveau de paie de l'utilisateur parmis ces rôles</returns>
        int GetHigherPaieLevel(int userId, int? orgaId);

        /// <summary>
        ///   Renvoi vrai si l'utilisateur connecté a au moins le niveau du role définit
        /// </summary>
        /// <param name="niveauPaie"> Le niveau de paie </param>
        /// <returns> Renvoi vrai si l'utilisateur a au moins le niveau du role définit</returns>
        bool HasAtLeastThisPaieLevel(int niveauPaie);

        /// <summary>
        ///   Retourne la liste CI par personne
        /// </summary>
        /// <param name="userid">Identifiant de l'utilisateur</param>
        /// <param name="force">Force le rafraichissement de la liste des CI de l'utilisateur dans le cache</param>
        /// <param name="organisationPere">"OrganisationPere"</param>
        /// <returns>Renvoie la liste des CI par personne.</returns>
        IEnumerable<int> GetAllCIbyUser(int userid, bool force = false, int? organisationPere = null);

        /// <summary>
        ///   Retourne la liste CI par personne
        /// </summary>
        /// <param name="userid">Identifiant de l'utilisateur</param>
        /// <param name="force">Force le rafraichissement de la liste des CI de l'utilisateur dans le cache</param>
        /// <param name="organisationPere">"OrganisationPere"</param>
        /// <returns>Renvoie la liste des CI par personne.</returns>
        Task<IEnumerable<int>> GetAllCIbyUserAsync(int userid, bool force = false, int? organisationPere = null);

        /// <summary>
        ///   Retourne la liste CI du profil paie selon l'organisation choisie
        /// </summary>
        /// <param name="organisationId">OrganisationId choisie</param>
        /// <returns>Renvoie la liste des CI choisie par profil paie.</returns>
        IEnumerable<int> GetAllCIIdbyOrganisation(int organisationId);

        bool DoesFolioExist(int userId, string folio, int userCompanyId);

        /// <summary>
        /// Retourne le niveau de paie de l'utilisateur courant pour un CI donné ou général
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Le niveau de paie de utilisateur</returns>
        int GetUserPaieLevel(int userId, int? ciId);

        /// <summary>
        ///   Retourne vrai si l'utulisateur à des droits sur la paie > 3
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle dans la Paie, sinon nulle.</returns>
        bool IsRolePaie(int userId, int? ciId);

        /// <summary>
        ///   Retourne un dictionnaire (ciId, isRolePaie) déterminant pour chaque CI si l'utilisateur userId est RolePaie ou non
        /// </summary>
        /// <remarks>
        /// 3 appels à la BD :
        ///     - GetOrganisationIdByCiId
        ///     - 2 appels dans GetHigherPaieLevelByOrganisationIdList
        /// </remarks>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciIds">Liste d'identifiants de CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle dans la Paie, sinon nulle.</returns>
        Dictionary<int, bool> IsRolePaie(int userId, IEnumerable<int> ciIds);

        /// <summary>
        ///   Retourne si l'utulisateur fait parti de la paie ou non
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle de gestionnaire de Paie, sinon nulle.</returns>
        bool IsGSP(int userId, int? ciId);

        /// <summary>
        ///   Retourne vrai si l'utilisateur à des droits sur la paie > 0
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle lié au chantier, sinon nulle.</returns>
        bool IsRoleChantier(int userId, int? ciId);

        /// <summary>
        ///   Retourne vrai si l'utilisateur à des droits sur la paie égal 1
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle lié au chantier, sinon nulle.</returns>
        bool IsNiveauPaie1(int userId, int ciId);

        /// <summary>
        ///   Retourne vrai si l'utilisateur à des droits sur la paie égal 2
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle lié au chantier, sinon nulle.</returns>
        bool IsNiveauPaie2(int userId, int ciId);

        /// <summary>
        ///   Retourne vrai si l'utilisateur à des droits sur la paie égal 3
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède un rôle lié au chantier, sinon nulle.</returns>
        bool IsNiveauPaie3(int userId, int ciId);

        /// <summary>
        /// Evalue l'appartenance d'un ci au périmètre de l'utilisateur
        /// </summary>
        /// <param name="ciId">L'identifiant du Ci à évaluer</param>
        /// <returns>Retourne vrai si le CI fait parti du périmètre de l'utilisateur, faux sinon</returns>
        bool IsInMyPerimetre(int ciId);

        /// <summary>
        /// Retourne l'AffectationSeuilUtilisateurEnt correspondant a l'utilisateur et a l'organisation.
        /// S'il n'y a pas de d'affectation pour l'organisationId, on remonte l'abrbre des organisation parente,
        /// jusqu'a trouvé une affectation. Sinon on renvoi null.
        /// Si le role n'est pas actif pour l'affectation, alors on ne selectionne pas l'affectation
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <param name="organisationId">organisationId</param>
        /// <returns>AffectationSeuilUtilisateurEnt</returns>
        AffectationSeuilUtilisateurEnt GetFirstAffectationForOrganisationInTreeWithRoleActif(int utilisateurId, int organisationId);

        /// <summary>
        /// Vérifier si un personnel est un utilisateur Fred
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>Boolean indique si le personnel est un utilisateur Fred</returns>
        bool IsFredUser(int personnelId);

        /// <summary>
        /// Get ci list for delegue 
        /// </summary>
        /// <param name="userId">Optionnel : identifiant de l'utilisateur pour lequel récupérer les CI ; par défaut, l'utilisateur connecté</param>
        /// <returns>IEnumerable of CIEnt</returns>
        IEnumerable<CIEnt> GetCiListForDelegue(int? userId = null);

        /// <summary>
        /// Get affectation list of responsable (Delegue ou reponsable CI) . 
        /// personnel statut s'assure de retourner un type particuler d'employé : Ouvrier ,ETAM ou IAC etc ..
        /// </summary>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <returns>IEnumerable of Affectation Ent</returns>
        IEnumerable<AffectationEnt> GetAffectationList(string personnelStatut);

        /// <summary>
        /// Get ci list of responsable
        /// </summary>
        /// <returns>IEnumerable de CIEnt</returns>
        IEnumerable<CIEnt> GetCiListOfResponsable();

        /// <summary>
        /// Get ci list of responsable
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur</param>
        /// <returns>IEnumerable de CIEnt</returns>
        IEnumerable<CIEnt> GetCiListOfResponsable(int userId);

        /// <summary>
        /// Permet de récupéré les droite pour gérér le personnel en fonction roleId
        /// </summary>
        /// <param name="roleId">identifiant unique du role</param>
        /// <returns>RoleFornctionnalite</returns>
        RoleFonctionnaliteEnt GetRightPersonnelManagement(int roleId);

        /// <summary>
        ///   Vérifie que l'utilisateur est SuperAdmin
        /// </summary>
        /// <param name="utilisateurId">Id de l'Utilisateur</param>
        /// <returns>Vrai si l'utilisateur possède un rôle SuperAdmin</returns>
        bool IsSuperAdmin(int utilisateurId);

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir toute la liste des affectations des moyens
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir toute la liste des affectations des moyens</returns>
        bool HasPermissionToSeeAllAffectationMoyens();

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des affectations des moyens d'un manager des personnels
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des affectations des moyens d'un manager des personnels</returns>
        bool HasPermissionToSeeManagerPersonnelAffectationMoyens();

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des affectations des moyens d'un responsable CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des affectations des moyens d'un responsable CI</returns>
        bool HasPermissionToSeeResponsableCiAffectationMoyens();

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des affectations des moyens d'un délégué CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des affectations des moyens d'un délégué CI</returns>
        bool HasPermissionToSeeDelegueCiAffectationMoyens();

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir toute la liste des CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir toute la liste des CI</returns>
        bool HasPermissionToSeeAllCi();

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des CI d'un responsable CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des CI d'un responsable CI</returns>
        bool HasPermissionToSeeResponsableCiList();

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des CI d'un délégué CI
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des CI d'un délégué CI</returns>
        bool HasPermissionToSeeDelegueCiList();

        /// <summary>
        /// Vérifier si un utilisateur a le rôle gestionnaire des moyens
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <returns>Booléan indique si l'utilisateur a le rôle gestionnaire des moyens</returns>
        [Obsolete("Les rôles ne sont pas fixes ; il faut tester les permissions (ce n'est pas l'idéal, mais c'est mieux)")]
        bool IsUserGestionnaireMoyen(int utilisateurId);

        /// <summary>
        /// Vérifier si un utilisateur a le rôle manager des personnels
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <returns>Booléan indique si l'utilisateur a le rôle manager des personnels</returns>
        [Obsolete("Les rôles ne sont pas fixes ; il faut tester les permissions (ce n'est pas l'idéal, mais c'est mieux)")]
        bool IsUserManagerPersonnel(int utilisateurId);

        /// <summary>
        /// Vérifier si un utilisateur a le rôle responsable CI
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <returns>Booléan indique si l'utilisateur a le rôle responsable CI</returns>
        [Obsolete("Les rôles ne sont pas fixes ; il faut tester les permissions (ce n'est pas l'idéal, mais c'est mieux)")]
        bool IsUserResponsableCi(int utilisateurId);

        /// <summary>
        /// Indique si l'utilisateur connecté appartient au groupe indiqué.
        /// </summary>
        /// <param name="groupe">Le groupe (comme Constantes.CodeGroupeFES).</param>
        /// <returns>True si l'utilisateur appartient au groupe, sinon false.</returns>
        bool IsUtilisateurOfGroupe(string groupe);

        /// <summary>
        /// Vérifier si un utilisateur a le rôle gestionnaire de paie sans prendre en considération sans niveau de paie
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <param name="organisationId">l'identifiant de l'organisation</param>
        /// <returns>Booléan indique si l'utilisateur a le rôle gestionnaire de paie</returns>
        bool IsRoleGSPWithoutConsideringPaieLevel(int utilisateurId, int? organisationId);

        /// <summary>
        /// Retourne si l'utulisateur a la permission de voir menu edition
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne Vrai si l'utilisateur courant possède la permission, sinon nulle.</returns>
        bool IsUserHasMenuEditionPermission(int userId, int? ciId);

        /// <summary>
        /// Vérifier si un utilisateur a la permission pour voir la liste des personnels
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission pour voir la liste des personnels</returns>
        bool HasPermissionToSeePersonnelsList();

        /// <summary>
        /// Vérifier si un utilisateur a la permission de Créer des coummandes brouillons avec un fournisseur temporaire
        /// </summary>
        /// <returns>Booléan indique si l'utilisateur a la permission de Créer des coummandes brouillons avec un fournisseur temporaire</returns>
        bool HasPermissionToCreateBrouillonWithFournisseurTemporaire();

        /// <summary>
        /// Get Ci list for role . Pour un delegue merci de privilégier la méthode : GetCiListForDelegue
        /// </summary>
        /// <param name="roleSpecification">Role specification pour identifier un délégué ou un responsable de chantier</param>
        /// <returns>IEnumerable of CIEnt</returns>
        IEnumerable<CIEnt> GetCiListForRole(RoleSpecification roleSpecification);
    }
}
