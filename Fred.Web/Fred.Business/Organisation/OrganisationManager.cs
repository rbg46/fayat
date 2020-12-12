using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Role;
using Fred.Business.RoleFonctionnalite;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Entities.RoleFonctionnalite;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.Organisation
{
    public class OrganisationManager : Manager<OrganisationEnt, IOrganisationRepository>, IOrganisationManager
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IRoleFonctionnaliteManager roleFonctionnaliteManager;
        private readonly ICIRepository ciRepository;
        private readonly ITypeOrganisationRepository typeOrgaRepo;
        private readonly ISocieteRepository societeRepository;

        public OrganisationManager(
            IUnitOfWork uow,
            IOrganisationRepository organisationRepository,
            IUtilisateurManager utilisateurManager,
            IRoleFonctionnaliteManager roleFonctionnaliteManager,
            ICIRepository ciRepository,
            ITypeOrganisationRepository typeOrgaRepo,
            ISocieteRepository societeRepository)
            : base(uow, organisationRepository)
        {
            this.utilisateurManager = utilisateurManager;
            this.roleFonctionnaliteManager = roleFonctionnaliteManager;
            this.ciRepository = ciRepository;
            this.typeOrgaRepo = typeOrgaRepo;
            this.societeRepository = societeRepository;
        }

        /// <summary>
        ///   Retourne la liste des organisations.
        /// </summary>
        /// <returns>Liste des organisation.</returns>
        public IEnumerable<OrganisationEnt> GetList()
        {
            return Repository.GetList();
        }

        /// <summary>
        ///   Renvoi la liste des organisations d'un Utilisateur
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="types">Types d'organisation</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="organisationIdPere">Identifiant de l'organisation parente</param>
        /// <returns>Une liste d'organisations</returns>
        [Obsolete("Ne plus utiliser. Utiliser une methode de l'object OrganisationTree à la place - Par l'intermediaire de l'OrganisationTreeService.")]
        public IEnumerable<OrganisationLightEnt> GetOrganisationsAvailable(string text = null, List<int> types = null, int? utilisateurId = null, int? organisationIdPere = null)
        {
            return Repository.GetOrganisationsAvailable(text, types, utilisateurId, organisationIdPere).OrderBy(to => to.TypeOrganisationId).ThenBy(c => c.Code);
        }

        /// <summary>
        ///   Retourne la organisation dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="organisationId">Identifiant de la organisation à retrouver.</param>
        /// <returns>La organisation retrouvée, sinon nulle.</returns>
        public OrganisationEnt GetOrganisationById(int organisationId)
        {
            return Repository.GetOrganisationById(organisationId);
        }

        /// <summary>
        /// Retourne l'organisation correspondante aux paramètres
        /// </summary>
        /// <param name="codeOrganisation">Code de l'organisation</param>
        /// <param name="codeTypeOrganisation">Type de l'organisation</param>
        /// <returns>Organisation</returns>
        public OrganisationEnt GetOrganisationByCodeAndType(string codeOrganisation, string codeTypeOrganisation)
        {
            var typeOrgaId = GetTypeOrganisationIdByCode(codeTypeOrganisation);
            var orgaGenerique = Repository.Get().AsNoTracking().Where(o => o.OrganisationGenerique.Code == codeOrganisation && o.TypeOrganisationId == typeOrgaId).FirstOrDefault();
            return orgaGenerique;
        }

        /// <summary>
        ///   Retourne les seuils de comande définit pour une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation à retrouver.</param>
        /// <param name="roleId">Identifiant du role.</param>
        /// <returns>liste des seuils de l'organisation choisi, sinon nulle.</returns>
        public IEnumerable<AffectationSeuilOrgaEnt> GetSeuilByOrganisationId(int organisationId, int? roleId)
        {
            return Repository.GetSeuilByOrganisationId(organisationId, roleId).OrderBy(a => a.Devise.Libelle);
        }

        /// <summary>
        ///   Sauvegarde les modifications d'une organisation
        /// </summary>
        /// <param name="organisationEnt">organisation à modifier</param>
        /// <param name="mereId">organisation mère</param>
        /// <returns>Organisation mise à jour</returns>
        public OrganisationEnt UpdateOrganisation(OrganisationEnt organisationEnt, int? mereId)
        {
            return Repository.UpdateOrganisation(organisationEnt, mereId);
        }

        /// <summary>
        ///   Retourne la liste des Types d'organisation.
        /// </summary>
        /// <returns>Liste des types d'organisation.</returns>
        public IEnumerable<TypeOrganisationEnt> GetOrganisationTypesList()
        {
            return Repository.GetOrganisationTypesList();
        }

        /// <summary>
        ///   Ajouter une surcharge de devise
        /// </summary>
        /// <param name="threshold">Entité association ROLE_ORGANISATION_DEVISE</param>
        /// <returns>Un identifiant d'association</returns>
        public AffectationSeuilOrgaEnt AddOrganisationThreshold(AffectationSeuilOrgaEnt threshold)
        {
            if (threshold.Seuil > 0 && threshold.Seuil <= 9999999)
            {
                if (IsSeuilWithDeviseUnique(threshold))
                {
                    Repository.AddOrganisationThreshold(threshold);
                    Save();

                    return threshold;
                }

                throw new FredBusinessException(RoleResources.SeuilValidation_SurchargeUnique);
            }

            throw new FredBusinessException(RoleResources.SeuilValidation_LimiteMontant);
        }

        /// <summary>
        ///   Vérifie si l'association (Rôle,Organisation,Devise) est unique dans FRED_ROLE_ORGANISATION_DEVISE
        /// </summary>
        /// <param name="threshold">Entité de l'association</param>
        /// <returns>Un booléen d'existence d'a ssociation</returns>
        private bool IsSeuilWithDeviseUnique(AffectationSeuilOrgaEnt threshold)
        {
            var orgaRoleThreshold = GetSeuilByOrganisationId((int)threshold.OrganisationId, null);

            if (orgaRoleThreshold.Any())
            {
                var tmp = orgaRoleThreshold.Where(s => s.DeviseId == threshold.DeviseId && s.OrganisationId == threshold.OrganisationId && s.RoleId == threshold.RoleId).ToList();
                if (tmp.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///   Mise à jour d'une surcharge de devise
        /// </summary>
        /// <param name="threshold">devise</param>
        /// <exception cref="System.Exception">Lève une exception si le seuil de validation n'est pas compris entre 0 et 9 999 999</exception>
        /// <returns>Association mise à jour</returns>
        public AffectationSeuilOrgaEnt UpdateThresholdOrganisation(AffectationSeuilOrgaEnt threshold)
        {
            if (threshold.Seuil > 0 && threshold.Seuil <= 9999999)
            {
                Repository.UpdateThresholdOrganisation(threshold);
                Save();

                return threshold;
            }
            else
            {
                throw new FredBusinessException(RoleResources.SeuilValidation_LimiteMontant);
            }
        }

        /// <summary>
        ///   Supprimer une surcharge de devise
        /// </summary>
        /// <param name="thresholdOrganisationId">Identifiant de la surcharge à supprimer</param>
        public void DeleteThresholdOrganisationById(int thresholdOrganisationId)
        {
            Repository.DeleteThresholdOrganisationById(thresholdOrganisationId);
        }


        /// <summary>
        ///   Retourne une liste d'organisations parentes d'une organisation fille
        /// </summary>
        /// <param name="organisationEnfantId">Organisation fille</param>
        /// <param name="orgaTypeIdToStop">code de l'organisation parent où l'on doit stopper la recherche</param>
        /// <returns>Une liste d'organisations</returns>
        [Obsolete("Methode recursive, ne plus utiliser. Utiliser le OrganisationTreeService ou OrganisationTreeRepository à la place")]
        public List<OrganisationEnt> GetOrganisationParentByOrganisationId(int organisationEnfantId, int? orgaTypeIdToStop = null)
        {
            return Repository.GetOrganisationParentList(organisationEnfantId, orgaTypeIdToStop);
        }

        /// <summary>
        ///   Récupère la liste des organisations disponibles pour un utilisateur en fonction des types d'organisations choisis
        /// </summary>
        /// <param name="page">Numéro page</param>
        /// <param name="pageSize">Taille page</param>
        /// <param name="text">Texte recherché</param>    
        /// <param name="typeOrgaList">Liste des types d'organisation</param>
        /// <param name="bypassUser">par utilisateur ou non: Si on ignore l'utilisateur, on récupère toutes les organisations diposnibles. 
        /// Sinon, on récupère les orga dont il est habilité (Panel Habilitation dans Detail Personnel)</param>
        /// <param name="onlyCiNoClose"> seulement avec des ci non cloturé </param>
        /// <returns>Liste Light des organisations</returns>
        public IEnumerable<OrganisationLightEnt> SearchLightOrganisation(int page, int pageSize, string text, List<string> typeOrgaList, bool bypassUser = false, bool onlyCiNoClose = true)
        {
            UtilisateurEnt currentUser = Managers.Utilisateur.GetContextUtilisateur();
            List<int> types;
            types = typeOrgaRepo.Query().Filter(t => typeOrgaList.Contains(t.Code)).Get().Select(t => t.TypeOrganisationId).ToList();
            int? utilisateurId = bypassUser ? default(int?) : this.utilisateurManager.GetContextUtilisateurId();
            var list = GetOrganisationsAvailable(text, types, utilisateurId: utilisateurId);

            if (typeOrgaList.Contains(Constantes.OrganisationType.CodeCi) && onlyCiNoClose)
            {
                List<OrganisationLightEnt> listWithoutCi = list.Where(l => l.TypeOrganisationId != OrganisationType.Ci.ToIntValue()).ToList();
                List<OrganisationLightEnt> listOnlyCi = list.Where(l => l.TypeOrganisationId == OrganisationType.Ci.ToIntValue()).ToList();

                List<OrganisationLightEnt> listOnlyCiNoClose = FilterCiNoCloseWithListCiOrganisation(listOnlyCi);

                list = listWithoutCi.Concat(listOnlyCiNoClose);
            }

            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        /// <summary>
        /// Renvoie une liste d'organisation ci uniquement non cloturé
        /// </summary>
        /// <param name="listCi">Liste des organisations ci à filtrer</param>
        /// <returns>Liste des organisations ci filtrés</returns>
        private List<OrganisationLightEnt> FilterCiNoCloseWithListCiOrganisation(List<OrganisationLightEnt> listCi)
        {
            List<int> listCodeCiClose = ciRepository.GetOrganisationIdCiClose();
            List<OrganisationLightEnt> listCiNoClose = new List<OrganisationLightEnt>();

            foreach (var ci in listCi)
            {
                if (!listCodeCiClose.Contains(ci.OrganisationId))
                {
                    listCiNoClose.Add(ci);
                }
            }

            return listCiNoClose;
        }

        /// <summary>
        /// Retourne l'arbo a partir de pole 
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="poleOrganisationId">OrganisationI du pole</param>
        /// <returns>Liste a plat d'organisations</returns>
        public IEnumerable<OrganisationLightEnt> GetOrganisationsForPole(int page, int pageSize, int poleOrganisationId)
        {
            List<string> typeOrgaList = new List<string>
            {
                TypeOrganisationEnt.CodePole,
                TypeOrganisationEnt.CodeGroupe,
                TypeOrganisationEnt.CodeSociete,
                TypeOrganisationEnt.CodePuo,
                TypeOrganisationEnt.CodeUo,
                TypeOrganisationEnt.CodeEtablissement,
                TypeOrganisationEnt.CodeCi
            };

            var types = typeOrgaRepo.Query()
                                    .Filter(t => typeOrgaList.Contains(t.Code))
                                    .Get()
                                    .Select(t => t.TypeOrganisationId)
                                    .ToList();

            var list = GetOrganisationsAvailable(string.Empty, types, null, organisationIdPere: poleOrganisationId);

            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Recherche les orgas a partir d'une societeId
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="recherche">recherche</param>
        /// <param name="typeOrganisation">typeOrganisation</param>
        /// <param name="societeId">societeId</param>
        /// <returns>etouner une liste  d'organisation</returns>
        public IEnumerable<OrganisationLightEnt> SearchLightForSocieteId(int page, int pageSize, string recherche, List<string> typeOrganisation, int? societeId)
        {
            if (societeId == null)
            {
                return new List<OrganisationLightEnt>();
            }
            var organisation = societeRepository
                                    .Query()
                                    .Include(s => s.Organisation)
                                    .Filter(s => s.SocieteId == societeId)
                                    .Get()
                                    .Select(s => s.Organisation)
                                    .FirstOrDefault();

            if (organisation == null)
            {
                return new List<OrganisationLightEnt>();
            }

            List<int> types = typeOrgaRepo.Query()
                                    .Filter(t => typeOrganisation.Contains(t.Code))
                                    .Get()
                                    .Select(t => t.TypeOrganisationId)
                                    .ToList();

            IEnumerable<OrganisationLightEnt> list = GetOrganisationsAvailable(recherche, types, null, organisation.OrganisationId);
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Le texte de recherche</param>
        /// <returns>Retourne une liste des référentiels</returns>
        public IEnumerable<OrganisationLightEnt> SearchLight(int page, int pageSize, string text)
        {
            var types = typeOrgaRepo.Query().Get().Select(t => t.TypeOrganisationId).ToList();
            var list = GetOrganisationsAvailable(text, types);
            var result = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return result;
        }

        /// <summary>
        ///   Retourne une nouvelle organisation
        /// </summary>
        /// <param name="codeOrganisation"> code du type d'organisation</param>
        /// <param name="pereId"> identifiant de l'organisation du parent</param>
        /// <returns> L'organisation générée</returns>
        public OrganisationEnt GenerateOrganisation(string codeOrganisation, int? pereId)
        {
            return Repository.GenerateOrganisation(codeOrganisation, pereId);
        }

        /// <summary>
        ///   Retourne une nouvelle organisation
        /// </summary>
        /// <param name="codeOrganisation"> code du type d'organisation</param>
        /// <param name="pere">l'organisation du parent</param>
        /// <returns> L'organisation générée</returns>
        public OrganisationEnt GenerateOrganisation(string codeOrganisation, OrganisationEnt pere)
        {
            return Repository.GenerateOrganisation(codeOrganisation, pere);
        }

        /// <summary>
        ///   Retourne l'identifiant du type d'organisation portant le code indiqué.
        /// </summary>
        /// <param name="codeTypeOrganisation">Code du type d'organisation.</param>
        /// <returns>l'id du type d'organisation retrouvé</returns>
        public int GetTypeOrganisationIdByCode(string codeTypeOrganisation)
        {
            return Repository.GetTypeOrganisationIdByCode(codeTypeOrganisation);
        }

        /// <summary>
        /// Récuperer l'ensemble des societes dont l'utilisateur est habilité ou habilité sur leurs etab comptables
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="text">text</param>
        /// <returns>List des organisation (societe)</returns>
        public async Task<IEnumerable<OrganisationLightEnt>> SearchLightSocieteOrganisationForEtabComptable(int page, int pageSize, string text)
        {
            int utilisateurId = Managers.Utilisateur.GetContextUtilisateurId();
            List<OrganisationLightEnt> result = new List<OrganisationLightEnt>();
            List<RoleFonctionnaliteEnt> roleFonctionnalites = await roleFonctionnaliteManager.GetRoleFonctionnaliteByUserIdAsync(utilisateurId, Constantes.FonctionnaliteLibelle.ExportAnalytiqueBoutons).ConfigureAwait(false);
            if (roleFonctionnalites.Any())
            {
                IEnumerable<OrganisationEnt> organisationList = roleFonctionnalites.Where(x => x.Role.AffectationSeuilUtilisateurs.Any()).SelectMany(x => x.Role.AffectationSeuilUtilisateurs)
                                                                                   .Where(x => x.UtilisateurId == utilisateurId).Select(x => x.Organisation);
                if (organisationList.Any())
                {
                    IEnumerable<SocieteEnt> listSocietesNv1 = organisationList.Where(x => x.TypeOrganisation.Code.Equals(Constantes.OrganisationType.CodeSociete)).Select(x => x.Societe);
                    IEnumerable<SocieteEnt> listSocietesNv2 = organisationList.Where(x => x.TypeOrganisation.Code.Equals(Constantes.OrganisationType.CodeEtablissement) && x.Pere != null).Select(x => x.Pere.Societe);
                    if (listSocietesNv1.Union(listSocietesNv2).Any())
                    {
                        result = SocieteListToOrganisationLightEnt(listSocietesNv1.Union(listSocietesNv2));
                    }
                }
            }

            return result.Distinct().Where((o) => FilterText(text, o)).OrderBy(x => x.Code).Skip((page - 1) * pageSize).Take(pageSize);
        }

        private List<OrganisationLightEnt> SocieteListToOrganisationLightEnt(IEnumerable<SocieteEnt> societeList)
        {
            List<OrganisationLightEnt> result = new List<OrganisationLightEnt>();
            if (!societeList.Any())
            {
                return result;
            }

            foreach (SocieteEnt societe in societeList)
            {
                if (societe == null) continue;
                result.Add(new OrganisationLightEnt()
                {
                    Code = societe.Code,
                    Libelle = societe.Libelle?.Trim(),
                    TypeOrganisationId = societe.Organisation != null ? societe.Organisation.TypeOrganisationId : 0,
                    PereId = societe?.Organisation?.PereId,
                    OrganisationId = societe.OrganisationId,
                    TypeOrganisation = societe?.Organisation?.TypeOrganisation?.Libelle,
                });
            }

            return result;
        }

        private bool FilterText(string text, OrganisationLightEnt o)
        {
            if (string.IsNullOrEmpty(text) || (o.Libelle != null && ComparatorHelper.ComplexContains(o.Libelle, text)) ||
               (o.Code != null && ComparatorHelper.ComplexContains(o.Code, text)) || (o.CodeParent != null && ComparatorHelper.ComplexContains(o.CodeParent, text)))
            {
                return true;
            }

            return false;
        }
    }
}
