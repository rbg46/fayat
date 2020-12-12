using System.Collections.Generic;
using System.Linq;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.Organisation;
using Fred.Business.Personnel.Extensions;
using Fred.Business.Personnel.Models;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation;
using Fred.Entities.Utilisateur;
using Fred.Web.Shared.Models.Personnel.Excel;

namespace Fred.Business.Personnel.Mapper
{
    /// <summary>
    /// Mappe les données des affectations vers le model excel
    /// </summary>
    public class AffectationToExcelModelMapper : IAffectationToExcelModelMapper
    {
        /// <summary>
        /// unit of work
        /// </summary>
        protected readonly IUnitOfWork uow;
        private readonly IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager;
        private readonly IOrganisationManager organisationManager;

        /// <summary>
        /// Constructeur unique du manager
        /// </summary>
        /// <param name="affectationSeuilUtilisateurManager">Gestionnaire d'affectations</param>
        /// <param name="organisationManager">manager organisation</param>
        /// <param name="uow">gestionnaire de l'unit of work</param>
        public AffectationToExcelModelMapper(
            IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager,
            IOrganisationManager organisationManager,
            IUnitOfWork uow)
        {
            this.affectationSeuilUtilisateurManager = affectationSeuilUtilisateurManager;
            this.organisationManager = organisationManager;
            this.uow = uow;
        }

        /// <summary>
        /// returne la liste de model excel à ajouter
        /// </summary>
        /// <param name="affectations">la liste des affectations à mapper</param>
        /// <returns>Liste des rapports à créer</returns>
        public List<HabilitationsUtilisateursExcelModel> Transform(UtilOrgaRoleLists affectations)
        {
            List<AffectationSeuilUtilisateurEnt> utilRoleOrgaList = affectationSeuilUtilisateurManager.GetAllByUtilAndRoleAndOrgaLists(
                                                                     affectations.UtilisateursIds,
                                                                     affectations.RolesIds,
                                                                     affectations.OrganisationsIds);

            List<HabilitationsUtilisateursExcelModel> excelModelList = MapAffectationListToExcelModelList(utilRoleOrgaList, affectations.OrganisationsSimples);

            return excelModelList;
        }

        /// <summary>
        /// Returne la liste de model excel avec habilitation
        /// </summary>
        /// <param name="affectations">la liste des affectations à mapper</param>
        /// <returns>Liste des rapports à créer</returns>
        public List<PersonnelExcelModel> TransformPersonnelWithHabilitation(UtilOrgaRoleLists affectations)
        {
            List<AffectationSeuilUtilisateurEnt> utilRoleOrgaList = affectationSeuilUtilisateurManager.GetAllByUtilAndRoleAndOrgaLists(
                                                                     affectations.UtilisateursIds,
                                                                     affectations.RolesIds,
                                                                     affectations.OrganisationsIds);

            List<PersonnelExcelModel> excelModelList = AffectationListToPersonnelWithHabilitationExcelModel(utilRoleOrgaList, affectations.OrganisationsSimples);

            return excelModelList;
        }

        private List<HabilitationsUtilisateursExcelModel> MapAffectationListToExcelModelList(List<AffectationSeuilUtilisateurEnt> utilRoleOrgaList, List<OrganisationSimple> organisationSimples)
        {
            List<HabilitationsUtilisateursExcelModel> excelModelList = new List<HabilitationsUtilisateursExcelModel>();
            foreach (var item in utilRoleOrgaList)
            {
                var organisationSimple = organisationSimples?.FirstOrDefault(o => o.OrganisationId == item.OrganisationId);
                var personnel = item.Utilisateur.Personnel;
                excelModelList.Add(
                   new HabilitationsUtilisateursExcelModel
                   {
                       // Informations d'utilisateur
                       Nom = personnel?.Nom,
                       Prenom = personnel?.Prenom,
                       Matricule = personnel?.Matricule,
                       Ressource = personnel?.Ressource != null ? string.Format("{0} - {1}", personnel?.Ressource.Code, personnel?.Ressource.Libelle) : string.Empty,
                       EtablissementPaie = personnel?.EtablissementPaie != null ? string.Format("{0} - {1}", personnel?.EtablissementPaie.Code, personnel?.EtablissementPaie.Libelle) : string.Empty,
                       DateEntree = personnel?.DateEntree != null ? personnel?.DateEntree?.ToString("dd/MM/yyyy") : string.Empty,
                       DateSortie = personnel?.DateSortie != null ? personnel.DateSortie?.ToString("dd/MM/yyyy") : string.Empty,
                       // Habilitations d'utilisateur
                       Societe = GetSocieteByOrganisation(item.Organisation),
                       Role = string.Format("{0} - {1}", item.Role.CodeNomFamilier, item.Role.Libelle),
                       Organisation = organisationSimple?.CodeLibelle,
                       Devise = (item.Devise?.Libelle) ?? string.Empty,
                       Seuil = string.Format("{0:0.00}", item.CommandeSeuil)
                   });
            }
            return excelModelList;
        }

        private List<PersonnelExcelModel> AffectationListToPersonnelWithHabilitationExcelModel(List<AffectationSeuilUtilisateurEnt> utilRoleOrgaList, List<OrganisationSimple> organisationSimples)
        {
            List<PersonnelExcelModel> excelModelList = new List<PersonnelExcelModel>();
            foreach (var item in utilRoleOrgaList)
            {
                var organisationSimple = organisationSimples?.FirstOrDefault(o => o.OrganisationId == item.OrganisationId);
                var personnel = item.Utilisateur.Personnel;
                excelModelList.Add(
                   new PersonnelExcelModel
                   {
                       // Informations d'utilisateur
                       Societe = personnel.Societe?.Libelle,
                       Nom = personnel?.Nom,
                       Prenom = personnel?.Prenom,
                       Matricule = personnel?.Matricule,
                       Ressource = personnel?.Ressource != null ? string.Format("{0} - {1}", personnel?.Ressource.Code, personnel?.Ressource.Libelle) : string.Empty,
                       EtablissementPaie = personnel?.EtablissementPaie != null ? string.Format("{0} - {1}", personnel?.EtablissementPaie.Code, personnel?.EtablissementPaie.Libelle) : string.Empty,
                       DateEntree = personnel?.DateEntree != null ? personnel?.DateEntree?.ToString("dd/MM/yyyy") : string.Empty,
                       DateSortie = personnel?.DateSortie != null ? personnel.DateSortie?.ToString("dd/MM/yyyy") : string.Empty,
                       IsActif = personnel.GetPersonnelIsActive() ? "O" : "N",
                       IsInterne = personnel.IsInterne ? "O" : "N",
                       IsInterimaire = personnel.IsInterimaire ? "O" : "N",
                       IsUtilisateur = personnel.IsUtilisateur() ? "O" : "N",
                       Statut = personnel.GetPersonnelStatutString(),
                       Adresse1 = personnel.Adresse1,
                       Adresse2 = personnel.Adresse2,
                       Adresse3 = personnel.Adresse3,
                       CodePostal = personnel.CodePostal,
                       Ville = personnel.Ville,
                       Pays = personnel.Pays?.Libelle,
                       Email = personnel.Email,
                       // Habilitations d'utilisateur
                       SocieteHabilitation = GetSocieteByOrganisation(item.Organisation),
                       Role = string.Format("{0} - {1}", item.Role.CodeNomFamilier, item.Role.Libelle),
                       Organisation = organisationSimple?.CodeLibelle,
                       Device = (item.Devise?.Libelle) ?? string.Empty,
                       Seuil = string.Format("{0:0.00}", item.CommandeSeuil)
                   });
            }

            return excelModelList;
        }

        private string GetSocieteByOrganisation(OrganisationEnt organisation)
        {
            string typeOrganisation = organisation.TypeOrganisation.Code;
            switch (typeOrganisation)
            {
                case TypeOrganisationEnt.CodeCi:
                    if (organisation.CI != null)
                    {
                        return organisationManager.GetOrganisationParentByOrganisationId(organisation.CI.Organisation.OrganisationId, TypeOrganisationEnt.TypeOrganisationSociete)
                             .Find(x => x.TypeOrganisation.Code == TypeOrganisationEnt.CodeSociete)
                             .Societe.CodeLibelle;
                    }
                    else
                    {
                        return string.Empty;
                    }
                case TypeOrganisationEnt.CodeEtablissement:
                    if (organisation.Etablissement != null)
                    {
                        return organisationManager.GetOrganisationParentByOrganisationId(organisation.Etablissement.Organisation.OrganisationId, TypeOrganisationEnt.TypeOrganisationSociete)
                            .Find(x => x.TypeOrganisation.Code == TypeOrganisationEnt.CodeSociete)
                            .Societe.CodeLibelle;
                    }
                    else
                    {
                        return string.Empty;
                    }
                case TypeOrganisationEnt.CodeSociete:
                    return organisation.Societe != null ? organisation.Societe.CodeLibelle : string.Empty;
                case TypeOrganisationEnt.CodePuo:
                case TypeOrganisationEnt.CodeUo:
                    return organisation.Pere != null ? GetSocieteByOrganisation(organisation.Pere) : string.Empty;
                default: return string.Empty;
            }
        }
    }
}
