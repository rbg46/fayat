using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Gestionnaire des établissements de paie
    /// </summary>
    public class EtablissementPaieManager : Manager<EtablissementPaieEnt, IEtablissementPaieRepository>, IEtablissementPaieManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly ISocieteManager societeManager;

        public EtablissementPaieManager(
            IUnitOfWork uow,
            IEtablissementPaieRepository etablissementPaieRepo,
            IUtilisateurManager userManager,
            ISocieteManager societeManager)
          : base(uow, etablissementPaieRepo)
        {
            this.userManager = userManager;
            this.societeManager = societeManager;
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un code déplacement.
        /// </summary>
        /// <param name="text">Texte recherché dans les codes déplacement</param>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <returns>Retourne la condition de recherche des codes déplacement</returns>
        private Expression<Func<EtablissementPaieEnt, bool>> GetPredicate(string text, SearchEtablissementPaieEnt filters)
        {
            if (string.IsNullOrEmpty(text))
            {
                return p => !filters.Actif || p.Actif;
            }

            return p => (filters.Code && p.Code.ToLower().Contains(text.ToLower())
                         || filters.Libelle && p.Libelle.ToLower().Contains(text.ToLower()))
                        && (!filters.Actif || p.Actif);
        }

        /// <summary>
        ///   Retourne la liste des établissements de paie.
        /// </summary>
        /// <returns>Liste des établissements de paie.</returns>
        public IEnumerable<EtablissementPaieEnt> GetEtablissementPaieList()
        {
            return this.Repository.GetEtablissementPaieList() ?? new EtablissementPaieEnt[] { };
        }

        /// <summary>
        ///   Méthode GET de récupération de tous les établissements de paie éligibles à être une agence de rattachement
        /// </summary>
        /// <param name="currentEtabPaieId">ID de l'établissement de paie à exclure de la recherche</param>
        /// <returns>Retourne une nouvelle instance d'établissement de paie intialisée</returns>
        public IEnumerable<EtablissementPaieEnt> AgencesDeRattachement(int currentEtabPaieId)
        {
            return this.Repository.AgencesDeRattachement(currentEtabPaieId);
        }

        /// <summary>
        ///   Retourne l'établissement de paie dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="etablissementPaieId">Identifiant de l'établissement de paie à retrouver.</param>
        /// <returns>L'établissement de paie retrouvé, sinon nulle.</returns>
        public EtablissementPaieEnt GetEtablissementPaieById(int etablissementPaieId)
        {
            return this.Repository.GetEtablissementPaieById(etablissementPaieId);
        }

        /// <summary>
        ///   Permet de connaître l'existence d'un établissement de paie depuis son code.
        /// </summary>
        /// <param name="idCourant">Id courant</param>
        /// <param name="code">Le code</param>
        /// <param name="libelle">Le bibellé</param>
        /// <returns>Retourne vrai si le code + le libellés passés en paramètres existent déjà, faux sinon</returns>
        public bool IsEtablissementPaieExistsByCodeLibelle(int idCourant, string code, string libelle)
        {
            return this.Repository.IsEtablissementPaieExistsByCodeLibelle(idCourant, code, libelle);
        }

        /// <summary>
        ///   Permet de connaître l'existence d'un etab depuis son code.
        /// </summary>
        /// <param name="idCourant">L'Id courant</param>
        /// <param name="code">Le code de déplacement</param>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsCodeEtablissementPaieExistsByCode(int idCourant, string code, int societeId)
        {
            return this.Repository.IsCodeEtablissementPaieExistsByCode(idCourant, code, societeId);
        }

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance d'établissement de paie.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <returns>Nouvelle instance d'établissement de paie intialisée</returns>
        public EtablissementPaieEnt GetNewEtablissementPaie(int societeId)
        {
            return new EtablissementPaieEnt { Actif = true, SocieteId = societeId };
        }

        /// <summary>
        ///   Ajoute un nouvel établissement de paie
        /// </summary>
        /// <param name="etablissementPaieEnt">Etablissement de paie à ajouter</param>
        /// <returns>L'identifiant de l'établissement de paie ajouté</returns>
        public int AddEtablissementPaie(EtablissementPaieEnt etablissementPaieEnt)
        {
            return this.Repository.AddEtablissementPaie(etablissementPaieEnt);
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un établissement de paie
        /// </summary>
        /// <param name="etablissementPaieEnt">Etablissement de paie à modifier</param>
        public void UpdateEtablissementPaie(EtablissementPaieEnt etablissementPaieEnt)
        {
            this.Repository.UpdateEtablissementPaie(etablissementPaieEnt);
        }

        /// <summary>
        ///   Supprime un établissement de paie
        /// </summary>
        /// <param name="etablissementPaieEnt">L'établissement de paie à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        public bool DeleteEtablissementPaieById(EtablissementPaieEnt etablissementPaieEnt)
        {
            if (IsDeletable(etablissementPaieEnt))
            {
                this.Repository.Delete(etablissementPaieEnt);
                Save();
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un établissement comptable
        /// </summary>
        /// <param name="etablissementPaieEnt">L'établissement paie à supprimer</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(EtablissementPaieEnt etablissementPaieEnt)
        {
            return this.Repository.IsDeletable(etablissementPaieEnt);
        }

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">La taille de la page</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="agenceId">Identifiant agence</param>
        /// <param name="isHorsRegion">Est hors région</param>
        /// <param name="isAgenceRattachement">Est agence de rattachement</param>
        /// <returns>Une liste d' items de référentiel</returns>
        public IEnumerable<EtablissementPaieEnt> SearchLight(string text, int page, int pageSize, int? societeId = null, int? agenceId = null, bool? isHorsRegion = null, bool? isAgenceRattachement = null)
        {
            int? groupeId = null;
            if (!societeId.HasValue)
            {
                UtilisateurEnt currentUser = this.userManager.GetContextUtilisateur();
                groupeId = currentUser.Personnel.Societe.GroupeId;
            }
            var query = this.Repository.Query()
                      .Include(e => e.AgenceRattachement)
                      .Filter(e => e.Actif)
                      .Filter(e => string.IsNullOrEmpty(text) || e.Code.Contains(text) || e.Libelle.Contains(text))
                      .Filter(e => !agenceId.HasValue || e.EtablissementPaieId != agenceId)
                      .Filter(e => !societeId.HasValue || e.SocieteId == societeId)
                      .Filter(e => !groupeId.HasValue || e.Societe.GroupeId == groupeId)
                      .Filter(e => !isAgenceRattachement.HasValue || (isAgenceRattachement.Value && e.IsAgenceRattachement) || (!isAgenceRattachement.Value && !e.IsAgenceRattachement))
                      .Filter(e => !isHorsRegion.HasValue || (isHorsRegion.Value && e.HorsRegion) || (!isHorsRegion.Value && !e.HorsRegion))
                      .OrderBy(list => list.OrderBy(e => e.Code))
                      .GetPage(page, pageSize);

            return query;
        }

        /// <summary>
        ///   Retourne la liste des établissements de paie par societe ID.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Liste des établissements de paie.</returns>
        public IEnumerable<EtablissementPaieEnt> GetEtablissementPaieBySocieteId(int societeId)
        {
            var query = this.Repository.Query()
                            .Filter(e => e.SocieteId == societeId)
                            .Get();
            return query.ToList();
        }

        /// <summary>
        ///   Retourne la liste des établissements de paie par societe ID.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <returns>Liste des établissements de paie.</returns>
        public IEnumerable<EtablissementPaieEnt> GetEtablissementPaieByOrganisationId(int organisationId)
        {
            var query = this.Repository.Query()
                            .Filter(e => e.Societe.Organisation.OrganisationId == organisationId)
                            .Get();
            return query.ToList();
        }

        /// <summary>
        ///   retoune liste etab paie
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="text">Le texte recherché</param>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <returns>Liste d'établissement de paies</returns>
        public IEnumerable<EtablissementPaieEnt> SearchEtablissementPaieAllWithFilters(int societeId, string text, SearchEtablissementPaieEnt filters)
        {
            return this.Repository.SearchEtablissementPaieAllWithFilters(societeId, GetPredicate(text, filters));
        }

        /// <summary>
        /// Get etab paie list for validation pointage vrac Fes Async
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="societeId">Societe Id</param>
        /// <returns>Etab Paie List</returns>
        public async Task<IEnumerable<EtablissementPaieEnt>> GetEtabPaieListForValidationPointageVracFesAsync(int page, int pageSize, string recherche, int? societeId)
        {
            IEnumerable<EtablissementPaieEnt> etabs = new List<EtablissementPaieEnt>();
            if (societeId.HasValue)
            {
                etabs = GetEtablissementPaieBySocieteId(societeId.Value);
            }
            else
            {
                IEnumerable<SocieteEnt> societeList = await societeManager.GetSocietesListForRemonteeVracFesAsync(0, 0, string.Empty).ConfigureAwait(false);
                if (societeList.Any())
                {
                    List<int> societeIds = societeList.Select(x => x.SocieteId).ToList();
                    etabs = await Repository.GetEtabPaieBySocieteIdList(societeIds).ConfigureAwait(false);
                }
            }

            return etabs.Distinct().Where((o) => FilterText(recherche, o)).OrderBy(x => x.Code).Skip((page - 1) * pageSize).Take(pageSize);
        }

        private bool FilterText(string text, EtablissementPaieEnt o)
        {
            if (string.IsNullOrEmpty(text) || (o.Libelle != null && ComparatorHelper.ComplexContains(o.Libelle, text)) ||
               (o.Code != null && ComparatorHelper.ComplexContains(o.Code, text)))
            {
                return true;
            }

            return false;
        }
    }
}
