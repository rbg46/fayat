using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.Business.Societe
{
    /// <summary>
    /// Service de gestion Société SEP
    /// </summary>
    public class SepService : ISepService
    {
        private readonly ISocieteManager societeManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IAssocieSepManager associeSepManager;
        private readonly IAssocieSepRepository associeSepRepository;
        private readonly ITypeSocieteRepository typeSocieteRepository;
        private readonly ICIRepository ciRepository;

        /// <summary>
        /// Constructeur  SepService
        /// </summary>
        /// <param name="societeManager">gestionnaire de CI</param>
        /// <param name="utilisateurManager">Manager des Utilisateurs</param>
        /// <param name="associeSepManager">Manager des Associés SEP</param>
        /// <param name="associeSepRepository">AssocieSep repo</param>
        /// <param name="typeSocieteRepository">type societe</param>
        /// <param name="cIRepository">Repository CI</param>
        public SepService(
            ISocieteManager societeManager,
            IUtilisateurManager utilisateurManager,
            IAssocieSepManager associeSepManager,
            IAssocieSepRepository associeSepRepository,
            ITypeSocieteRepository typeSocieteRepository,
            ICIRepository cIRepository)
        {
            this.societeManager = societeManager;
            this.utilisateurManager = utilisateurManager;
            this.associeSepManager = associeSepManager;
            this.associeSepRepository = associeSepRepository;
            this.typeSocieteRepository = typeSocieteRepository;
            this.ciRepository = cIRepository;
        }

        /// <summary>
        /// Récupération de la société gérante d'une SEP en fonction de l'identifiant de la société SEP
        /// </summary>
        /// <param name="societeSepId">Identifiant de la société SEP</param>
        /// <returns>Société Gérante</returns>
        public SocieteEnt GetSocieteGerante(int societeSepId)
        {
            var filters = new List<Expression<Func<AssocieSepEnt, bool>>>
            {
                x => x.SocieteId == societeSepId,
                x => x.TypeParticipationSep.Code == Constantes.TypeParticipationSep.Gerant
            };

            var includes = new List<Expression<Func<AssocieSepEnt, object>>> { x => x.SocieteAssociee };

            return associeSepManager.Search(filters, includeProperties: includes).Select(x => x.SocieteAssociee).FirstOrDefault();
        }

        /// <summary>
        /// RG_5403_001 : Récupération de la liste des « sociétés participantes » d’une société SEP
        /// Les « sociétés participantes » d’une SEP sont toutes les sociétés associées en niveau 1 et en niveau 2 à la SEP.
        /// </summary>
        /// <param name="societeSepId">Identifiant de la société SEP</param>
        /// <returns>Liste des sociétés participantes</returns>
        public List<SocieteEnt> GetSocieteParticipantes(int societeSepId)
        {
            var filters = new List<Expression<Func<AssocieSepEnt, bool>>> { x => x.SocieteId == societeSepId };

            var includes = new List<Expression<Func<AssocieSepEnt, object>>> { x => x.SocieteAssociee };

            return associeSepManager.Search(filters, includeProperties: includes).Select(x => x.SocieteAssociee).ToList();
        }

        /// <summary>
        /// Récupération de tous les fournisseurs dans la SEP
        /// </summary>
        /// <param name="societeSepId">Identifiant de la société SEP</param>
        /// <returns>Liste de fournisseurs</returns>
        public List<FournisseurEnt> GetFournisseurs(int societeSepId)
        {
            var filters = new List<Expression<Func<AssocieSepEnt, bool>>> { x => x.SocieteId == societeSepId };

            var includes = new List<Expression<Func<AssocieSepEnt, object>>> { x => x.Fournisseur };

            return associeSepManager.Search(filters, includeProperties: includes).Select(x => x.Fournisseur).ToList();
        }

        /// <summary>
        /// SearchLight pour Lookup des CI Sep
        /// CI visibles par l’utilisateur ET rattachés à une société de type SEP.
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="searchedText">Texte a rechercher</param>
        /// <returns>Liste de CI</returns>
        public List<CIEnt> SearchLightCiSep(int page, int pageSize, string searchedText)
        {
            int userId = utilisateurManager.GetContextUtilisateurId();
            List<int> ciIdList = utilisateurManager.GetAllCIbyUser(userId).ToList();

            return ciRepository.SearchLightCiSep(page, pageSize, searchedText, ciIdList);
        }

        /// <summary>
        /// SearchLight pour Lookup des fournisseurs SEP
        /// les Fournisseurs liés à une ou plusieurs sociétés SEP du Groupe de l’utilisateur
        /// (dans la table [FRED_ASSOCIE_SEP], rechercher tous les fournisseurs liés à des SEP du Groupe de l'utilisateur).
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="searchedText">Texte a rechercher</param>
        /// <param name="ciId">CI Id</param>  
        /// <returns>Liste des Fournisseurs</returns>
        public List<FournisseurEnt> SearchLightFournisseurSep(int page, int pageSize, string searchedText, int? ciId)
        {
            var userGroupeId = utilisateurManager.GetContextUtilisateur().Personnel.Societe.GroupeId;
            int? idSocSep = null;
            if (ciId.HasValue)
            {
                idSocSep = ciRepository.Get(ciId.Value).SocieteId ?? null;
            }
            var filters = new List<Expression<Func<AssocieSepEnt, bool>>>
            {
                x => string.IsNullOrEmpty(searchedText)
                     || x.Fournisseur.Code.Contains(searchedText)
                     || x.Fournisseur.Libelle.Contains(searchedText),
                x => x.Fournisseur.GroupeId == userGroupeId,
                x => (!idSocSep.HasValue || x.SocieteId == idSocSep)
            };

            var includes = new List<Expression<Func<AssocieSepEnt, object>>> { x => x.Fournisseur };

            return associeSepManager.Search(filters, x => x.OrderBy(y => y.Fournisseur.Code), includeProperties: includes, page: page, pageSize: pageSize).Select(x => x.Fournisseur).Distinct().ToList();
        }

        /// <summary>
        /// récupére tous la liste des Société SEP
        /// </summary>
        /// <returns>retourne une liste de société AssocieSepEnt</returns>
        public IEnumerable<AssocieSepEnt> GetAll()
        {
            return associeSepManager.GetAll();
        }

        /// <summary>
        /// récupére la liste des Sociétés Sep d'un société participant 
        /// </summary>
        /// <param name="societeId">id société</param>
        /// <returns>retourne une liste de société AssocieSepEnt</returns>
        public List<int> GetSepSocieteParticipant(int societeId)
        {
            var filters = new List<Expression<Func<AssocieSepEnt, bool>>> { x => x.SocieteAssocieeId == societeId };
            return associeSepManager.Search(filters).Select(x => x.SocieteId).ToList();
        }

        /// <summary>
        /// récupére la liste des Sociétés Sep d'un société participant dont la société est gérante et partenaire
        /// </summary>
        /// <param name="societeId">id société gérante</param>
        /// <returns>retourne une liste de société AssocieSepEnt Partenaire et Gerante</returns>
        public List<int> GetSepSocieteParticipantForContratIterimaire(int societeId)
        {
            List<int> returnedValue = new List<int>();
            var partenaire = GetSepSocieteParticipant(societeId);
            var gerante = GetSepSocieteParticipantWhenSocieteIsGerante(societeId);
            if(partenaire.Any())
            {
                returnedValue.AddRange(partenaire);
            }
            if (gerante.Any())
            {
                returnedValue.AddRange(gerante);
            }
            return returnedValue;
        }

        /// <summary>
        /// récupére la liste des Sociétés Sep d'un société participant dont elle est gérante
        /// </summary>
        /// <param name="societeId">id société</param>
        /// <returns>retourne une liste de société AssocieSepEnt</returns>
        public List<int> GetSepSocieteParticipantWhenSocieteIsGerante(int societeId)
        {
            var filters = new List<Expression<Func<AssocieSepEnt, bool>>>
            {
                x => x.SocieteAssocieeId == societeId,
                x => x.TypeParticipationSep.Code == Constantes.TypeParticipationSep.Gerant,
                x => !x.AssocieSepParentId.HasValue
            };
            return associeSepManager.Search(filters).Select(x => x.SocieteId).ToList();
        }

        /// <summary>
        /// Retourne les societes de type sep parmis les societes dont l'id est contenu dans la liste societeIds
        /// </summary>
        /// <param name="societeIds">societeIds</param>
        /// <returns>Liste de societe de tupe Sep</returns>
        public List<SocieteEnt> GetSocietesThatAreSep(List<int> societeIds)
        {
            var filters = new List<Expression<Func<SocieteEnt, bool>>>
            {
                x => societeIds.Contains(x.SocieteId),
                x => x.TypeSociete.Code == Constantes.TypeSociete.Sep
            };

            return societeManager.Search(filters, asNoTracking: true);
        }

        /// <summary>
        /// Récupération des sociétés gérantes d'une SEP en fonction des identifiants des sociétés SEP
        /// </summary>
        /// <param name="societeIds">Identifiant des sociétés SEP</param>
        /// <returns>Sociétés Gérantes</returns>
        public Dictionary<int, SocieteEnt> GetSocieteGerantes(List<int> societeIds)
        {
            var filters = new List<Expression<Func<AssocieSepEnt, bool>>>
            {
                x =>societeIds.Contains(x.SocieteId),
                x => x.TypeParticipationSep.Code == Constantes.TypeParticipationSep.Gerant
            };

            var includes = new List<Expression<Func<AssocieSepEnt, object>>> { x => x.SocieteAssociee };

            var associeSeps = associeSepManager.Search(filters, includeProperties: includes, asNoTracking: true);

            var associeSepsGrouped = associeSeps.GroupBy(x => x.SocieteId);

            return associeSepsGrouped.ToDictionary(x => x.Key, x => x.First().SocieteAssociee);
        }

        /// <summary>
        /// Vérifie si une société est de type SEP ou non
        /// </summary>
        /// <param name="societe">Societe</param>
        /// <returns>Renvoie true si la société est de type de SEP, sinon faux</returns>
        public bool IsSep(SocieteEnt societe)
        {
            if (!societe.TypeSocieteId.HasValue && societe.TypeSociete == null)
            {
                return false;
            }
            else if (societe.TypeSociete == null)
            {
                societe.TypeSociete = typeSocieteRepository.FindById(societe.TypeSocieteId.Value);
            }

            return societe.TypeSociete.Code == Constantes.TypeSociete.Sep;
        }

        /// <inheritdoc/>
        public async Task<bool> IsSepAsync(SocieteEnt societe)
        {
            if (!societe.TypeSocieteId.HasValue && societe.TypeSociete == null)
            {
                return false;
            }

            if (societe.TypeSociete == null)
            {
                societe.TypeSociete = await typeSocieteRepository.FindByIdAsync(societe.TypeSocieteId.Value);
            }

            return societe.TypeSociete.Code == Constantes.TypeSociete.Sep;
        }

        /// <inheritdoc/>
        public async Task<int> GetSocieteAssocieIdGerantAsync(SocieteEnt societe)
        {
            var associes = societe.AssocieSeps;

            if (associes == null || associes.Count == 0)
            {
                associes = await associeSepRepository.GetAssocieSepAsync(societe.SocieteId);
            }

            return associes.FirstOrDefault(a => a.TypeParticipationSep.Code == Constantes.TypeParticipationSep.Gerant).SocieteAssociee.SocieteId;
        }


        /// <summary>
        /// Vérifie si une CI est attaché une société SEP ou non
        /// </summary>
        /// <param name="ci">Ci</param>
        /// <returns>Renvoie true si le CI est attaché à une société SEP sinon faux</returns>
        public bool IsSep(int ci)
        {
            SocieteEnt societe = ciRepository.GetCiById(ci, true).Societe;
            return this.IsSep(societe);
        }

        /// <summary>
        /// Retourne  societe gérante pour un CI SEP 
        /// </summary>
        /// <param name="societe">Societe ENT</param>
        /// <returns>retourne  societé gérante</returns>
        public SocieteEnt GetSocieteGeranteForSep(SocieteEnt societe)
        {
            if (societe != null)
            {
                if (this.IsSep(societe))
                {
                    return this.GetSocieteGerante(societe.SocieteId);
                }
                else
                {
                    return societe;
                }
            }
            return null;
        }

        /// <summary>
        /// Retourne id societe gérante pour un CI SEP 
        /// </summary>
        /// <param name="ci">Ci</param>
        /// <returns>retourne  societe Gérante</returns>
        public SocieteEnt GetSocieteGeranteForSep(int ci)
        {
            SocieteEnt societe = ciRepository.GetCiById(ci, true).Societe;
            return GetSocieteGeranteForSep(societe);
        }

        /// <summary>
        /// Récupération d'une query Associés SEP
        /// </summary>
        /// <param name="filters">Filtres choisis</param>
        /// <param name="orderBy">order By</param>
        /// <param name="includeProperties">liste tables inclus</param>
        /// <returns>Query</returns>
        public IEnumerable<AssocieSepEnt> GetAssocieswithfilter(List<Expression<Func<AssocieSepEnt, bool>>> filters,
             Func<IQueryable<AssocieSepEnt>, IOrderedQueryable<AssocieSepEnt>> orderBy = null,
            List<Expression<Func<AssocieSepEnt, object>>> includeProperties = null)
        {
            return associeSepManager.Search(filters, orderBy, includeProperties).ToList();
        }
    }
}
