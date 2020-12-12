using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Organisation;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;

namespace Fred.Business.Referential
{
    public class DeviseManager : Manager<DeviseEnt, IDeviseRepository>, IDeviseManager
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ISocieteManager societeManager;
        private readonly IOrganisationManager orgaManager;
        private readonly IRepository<CIDeviseEnt> ciDeviseRepository;
        private readonly ISocieteDeviseRepository societeDeviseRepository;

        public DeviseManager(
            IUnitOfWork uow,
            IDeviseRepository deviseRepository,
            IUtilisateurManager utilisateurManager,
            ISocieteManager societeManager,
            IOrganisationManager orgaManager,
            IRepository<CIDeviseEnt> ciDeviseRepository,
            ISocieteDeviseRepository societeDeviseRepository)
          : base(uow, deviseRepository)
        {
            this.utilisateurManager = utilisateurManager;
            this.societeManager = societeManager;
            this.orgaManager = orgaManager;
            this.ciDeviseRepository = ciDeviseRepository;
            this.societeDeviseRepository = societeDeviseRepository;
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'une devise.
        /// </summary>
        /// <param name="text">Texte recherché dans les devises</param>
        /// <param name="filter">Filtres pour la recherche</param>
        /// <returns>Retourne la condition de recherche des devises</returns>
        public Expression<Func<DeviseEnt, bool>> GetPredicate(string text, SearchDeviseEnt filter)
        {
            return p => (string.IsNullOrEmpty(text)
                         || filter.IsoCode && p.IsoCode.ToLower().Contains(text.ToLower())
                         || filter.IsoNombre && p.IsoNombre.ToLower().Contains(text.ToLower())
                         || filter.Symbole && p.Symbole.ToLower().Contains(text.ToLower())
                         || filter.Libelle && p.Libelle.ToLower().Contains(text.ToLower()))
                        && !p.DateSuppression.HasValue;
        }

        /// <inheritdoc/>
        public IEnumerable<DeviseEnt> GetList()
        {
            return Repository.GetList().OrderBy(d => d.Libelle);
        }

        /// <summary>
        ///   Retourne la liste des devises modifiés ou créer depuiis une date si celle-ci est passé en paramètre.
        ///   sinon retourner la liste complètes
        /// </summary>
        /// <param name="sinceDate">date de prise en compte des modifications</param>
        /// <returns>Liste des devises.</returns>
        public IQueryable<DeviseEnt> GetAll(DateTime? sinceDate = null)
        {
            var query = Repository.GetAll();
            if (sinceDate.HasValue)
            {
                query = query.Where(d =>
                                      !d.DateCreation.HasValue || d.DateCreation >= sinceDate.Value
                                      || !d.DateModification.HasValue || d.DateModification >= sinceDate.Value
                                      || !d.DateSuppression.HasValue || d.DateSuppression >= sinceDate.Value);
            }

            return query;
        }

        /// <inheritdoc/>
        public DeviseEnt GetById(int id)
        {
            return Repository.FindById(id);
        }

        /// <summary>
        ///   Retourne l'identifiant de la devise portant le code devise indiqué.
        /// </summary>
        /// <param name="code">Code de la devise à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon null.</returns>
        public int? GetDeviseIdByCode(string code)
        {
            return Repository.GetDeviseIdByCode(code);
        }

        /// <summary>
        ///   Vérifie si le code ISO est disponible
        /// </summary>
        /// <param name="codeIso">Code Iso Devise à vérifier</param>
        /// <param name="deviseId"> Code de la devise pour éventuellement exclusion</param>
        /// <returns>True si le Code Iso est disponible, sinon False pour spécifier que le code Iso est non disponible.</returns>
        public bool CheckUnicityCodeIso(string codeIso, int? deviseId)
        {
            bool rst;

            if (deviseId.HasValue && deviseId.Value > 0)
            {
                rst = Repository.GetList().Any(x => x.IsoCode == codeIso && x.DeviseId != deviseId);
            }
            else
            {
                rst = Repository.GetList().Any(x => x.IsoCode == codeIso);
            }

            return rst;
        }

        /// <summary>
        ///   Ajout une nouvelle devise
        /// </summary>
        /// <param name="item">Devise à ajouter</param>
        /// <returns>L'identifiant de la devise ajoutée</returns>
        public int Add(DeviseEnt item)
        {
            // Affectation Donnée Système
            item.DateCreation = DateTime.Now;
            item.AuteurCreation = this.utilisateurManager.GetContextUtilisateurId();

            // Database
            Repository.Insert(item);
            Save();

            return item.DeviseId;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'une devise
        /// </summary>
        /// <param name="item">Devise à modifier</param>
        public void Update(DeviseEnt item)
        {
            // Affectation Donnée Système
            item.DateModification = DateTime.Now;
            item.AuteurModification = this.utilisateurManager.GetContextUtilisateurId();

            // Database
            Repository.Update(item);
            Save();
        }

        /// <summary>
        ///   Supprime une devise
        /// </summary>
        /// <param name="item">La devise à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        public bool DeleteById(DeviseEnt item)
        {
            if (IsDeletable(item))
            {
                Repository.Delete(item);
                Save();

                return true;
            }

            return false;
        }

        /// <summary>
        ///   Vérifie s'il est possible de supprimer une devise
        /// </summary>
        /// <param name="item">Devise à supprimer</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(DeviseEnt item)
        {
            return Repository.IsDeletable(item);
        }

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        ///   Si l'on passe une organisationId, on vérifie si ce n'est pas un CI, 
        ///     - dans ce cas là, on ne récupère que les devises du CI. 
        ///     - sinon, on récupère la société de l'organisation passé en paramètre, puis on récupère les devises de la société
        /// </summary>
        /// <param name="ciId">L'identifiant du CI (optionnel pour le filtrage)</param>
        /// <param name="societeId">L'identiant de la société (optionnel pour le filtrage)</param>
        /// <param name="organisationId">Identifiant organisation (optionnel pour le filtrage)</param>
        /// <param name="text">Le texte recherché</param>
        /// <param name="page">La page courante</param>
        /// <param name="pageSize">La taille de la page</param>
        /// <returns>Une liste d' items de référentiel</returns>
        public IEnumerable<DeviseEnt> SearchLight(int? ciId, int? societeId, int? organisationId, string text, int page, int pageSize)
        {
            IQueryable<DeviseEnt> query = null;

            if (organisationId.HasValue)
            {
                OrganisationEnt orga = this.orgaManager.GetOrganisationById(organisationId.Value);
                if (orga.CI != null)
                {
                    ciId = orga.CI.CiId;
                }
                else
                {
                    SocieteEnt societe = this.societeManager.GetSocieteParentByOrgaId(organisationId.Value);
                    societeId = societe?.SocieteId;
                }
            }

            if (ciId.HasValue)
            {
                query = ciDeviseRepository
                               .Query()
                               .Include(x => x.Devise)
                               .Filter(x => x.CiId == ciId)
                               .Filter(x => !x.Devise.IsDeleted)
                               .Filter(x => x.Devise.Active)
                               .Filter(
                                       x => string.IsNullOrEmpty(text) ||
                                            x.Devise.Libelle.ToLower().Contains(text.ToLower()) ||
                                            x.Devise.IsoCode.ToLower().Contains(text.ToLower()))
                               .OrderBy(list => list.OrderBy(x => x.Devise.IsoCode))
                               .GetPage(page, pageSize)
                               .Select(x => x.Devise);
            }
            else if (societeId.HasValue)
            {
                query = societeDeviseRepository
                             .Query()
                             .Include(x => x.Devise)
                             .Filter(x => x.SocieteId == societeId)
                             .Filter(x => !x.Devise.IsDeleted)
                             .Filter(x => x.Devise.Active)
                             .Filter(
                               x => string.IsNullOrEmpty(text) ||
                               x.Devise.Libelle.ToLower().Contains(text.ToLower()) ||
                               x.Devise.IsoCode.ToLower().Contains(text.ToLower()))
                             .OrderBy(list => list.OrderBy(x => x.Devise.IsoCode))
                             .GetPage(page, pageSize)
                             .Select(x => x.Devise);
            }
            else
            {
                query = Repository
                               .Query()
                               .Filter(x => !x.IsDeleted)
                               .Filter(x => x.Active)
                               .Filter(
                                       x => string.IsNullOrEmpty(text) ||
                                            x.Libelle.ToLower().Contains(text.ToLower()) ||
                                            x.IsoCode.ToLower().Contains(text.ToLower()))
                               .OrderBy(list => list.OrderBy(x => x.IsoCode))
                               .GetPage(page, pageSize)
                               .Select(x => x);
            }

            return query;
        }

        /// <summary>
        ///   Permet de récupérer la liste des devises en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les devises</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré des devises</returns>
        public IEnumerable<DeviseEnt> SearchDeviseWithFilters(string text, SearchDeviseEnt filters)
        {
            return Repository.SearchDeviseWithFilters(GetPredicate(text, filters));
        }

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié à une devise
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'une devise</returns>
        public SearchDeviseEnt GetDefaultFilter()
        {
            return new SearchDeviseEnt
            {
                IsoCode = true,
                IsoNombre = true,
                Symbole = true,
                Libelle = true,
            };
        }

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de devise.
        /// </summary>
        /// <returns>Nouvelle instance de devise intialisée</returns>
        public DeviseEnt GetNewDevise()
        {
            return new DeviseEnt { Active = true };
        }

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return Repository.IsAlreadyUsed(id);
        }

        /// <inheritdoc/>
        public DeviseEnt GetDevise(string isoCode)
        {
            try
            {
                return Repository.Get().FirstOrDefault(x => x.IsoCode == isoCode);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne la liste de devise pour une liste de code de devise
        /// </summary>
        /// <param name="codeDevises">Liste de code de devise</param>
        /// <returns>Liste de devise</returns>
        public IReadOnlyList<DeviseEnt> GetDevises(List<string> codeDevises)
        {
            return Repository.Get().Where(q => codeDevises.Contains(q.IsoCode)).ToList();
        }
    }
}
