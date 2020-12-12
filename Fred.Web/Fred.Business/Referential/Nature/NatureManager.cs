using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Nature;

namespace Fred.Business.Referential.Nature
{
    public class NatureManager : Manager<NatureEnt, INatureRepository>, INatureManager
    {
        private readonly IUtilisateurManager utilisateurManager;

        public NatureManager(
            IUnitOfWork uow,
            INatureRepository natureRepository,
            IUtilisateurManager utilisateurMgr)
            : base(uow, natureRepository)
        {
            this.utilisateurManager = utilisateurMgr;
        }

        /// <summary>
        /// Vérifie s'il est possible de supprimer une prime
        /// </summary>
        /// <param name="natureEnt">La prime à supprimer</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(NatureEnt natureEnt)
        {
            return Repository.IsDeletable(natureEnt);
        }

        /// <summary>
        /// Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="args">Les arguments</param>
        /// <returns>Une liste d'items de référentiel</returns>
        public IEnumerable<NatureEnt> SearchLight(string text, IDictionary<string, object> args)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return GetNatureList();
            }

            return Repository.SearchLight(text);
        }

        /// <summary>
        /// Permet de récupérer la liste de tous les natures en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans tous les natures</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré de tous les natures</returns>
        public IEnumerable<NatureEnt> SearchNatureAllWithFilters(string text, SearchCriteriaEnt<NatureEnt> filters)
        {
            return Repository.SearchNatureWithFilters(GetPredicate(text, filters)) ?? new NatureEnt[] { };
        }

        /// <summary>
        /// Retourne le prédicat de recherche d'une nature
        /// </summary>
        /// <param name="text">Texte recherché dans les natures</param>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <returns>Retourne la condition de recherche des natures</returns>
        private Expression<Func<NatureEnt, bool>> GetPredicate(string text, SearchCriteriaEnt<NatureEnt> filters)
        {
            return p => (string.IsNullOrEmpty(text) || filters.Code && p.Code.Contains(text)
                         || filters.Libelle && p.Libelle.Contains(text))
                        && !filters.Actif
                        && (filters.SocieteId == null || p.SocieteId == filters.SocieteId);
        }

        /// <summary>
        /// Méthode d'ajout d'une nature
        /// </summary>
        /// <param name="nature">objet Nature à ajouter</param>
        /// <returns>Identifiant de la nature ajoutée</returns>
        public int AddNature(NatureEnt nature)
        {
            nature.DateCreation = DateTime.Now;
            nature.AuteurCreationId = this.utilisateurManager.GetContextUtilisateurId();

            Repository.AddNature(nature);
            Save();

            return nature.NatureId;
        }

        /// <summary>
        /// Méthode de sauvegarde des modifications d'une nature
        /// </summary>
        /// <param name="nature">Objet Nature modifié</param>
        /// <param name="fieldsToUpdate">Liste des champs à mettre à jour</param>
        public void UpdateNature(NatureEnt nature, List<Expression<Func<NatureEnt, object>>> fieldsToUpdate)
        {
            nature.DateModification = DateTime.UtcNow;
            nature.AuteurModificationId = utilisateurManager.GetContextUtilisateurId();

            fieldsToUpdate.Add(x => x.DateModification);
            fieldsToUpdate.Add(x => x.AuteurModificationId);

            Repository.Update(nature, fieldsToUpdate);
            Save();
        }

        /// <summary>
        /// Méthod permettant de supprimer physiquement une nature
        /// </summary>
        /// <param name="natureEnt">La nature à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        public bool DeleteNatureById(NatureEnt natureEnt)
        {
            if (!IsDeletable(natureEnt))
                return false;

            Repository.Delete(natureEnt);
            Save();

            return true;
        }

        /// <summary>
        /// Retourne une nature précise via son identifiant
        /// </summary>
        /// <param name="natureId">Identifiant d'une nature</param>
        /// <returns>Objet Nature correspondant</returns>
        public NatureEnt GetNatureById(int natureId)
        {
            return Repository.GetNatureById(natureId);
        }

        /// <summary>
        /// Retourne l'identifiant de la nature portant le code devise indiqué.
        /// </summary>
        /// <param name="natureCode">Code de la nature à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon null.</returns>
        public int? GetNatureIdByCode(string natureCode)
        {
            return Repository.GetNatureIdByCode(natureCode);
        }

        /// <summary>
        /// Retourne la liste des code natures pour une liste de code pour une société donnée
        /// </summary>
        /// <param name="codes">Liste de code</param>
        /// <param name="societeIds">Liste d'identifiant de societe</param>
        /// <returns>Liste de <see cref="NatureFamilleOdModel"/></returns>
        public IReadOnlyList<NatureFamilleOdModel> GetCodeNatureAndFamilliesOD(List<string> codes, List<int> societeIds)
        {
            return Repository.GetNatureList(codes, societeIds).Select(x =>
            new NatureFamilleOdModel
            {
                Nature = x,
                SocieteId = x.SocieteId,
                SocieteCode = x.Societe.Code,
                NatureAnalytique = x.Code,
                ParentFamilyODWithOrder = x.ParentFamilyODWithOrder,
                ParentFamilyODWithoutOrder = x.ParentFamilyODWithoutOrder,
            }).ToList();
        }

        /// <summary>
        /// Retourne la liste de toutes les natures à l'exception des supprimées
        /// </summary>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> GetNatureList()
        {
            return Repository.GetNatureList() ?? new NatureEnt[] { };
        }

        /// <summary>
        /// Retourne la liste de toutes les natures avec les supprimées.
        /// </summary>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> GetNatureListAll()
        {
            return Repository.GetNatureListAll() ?? new NatureEnt[] { };
        }

        /// <summary>
        /// Retourne la liste de toutes les natures d'une société donnée
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> GetNatureBySocieteId(int societeId)
        {
            return Repository.GetNatureBySocieteId(societeId) ?? new NatureEnt[] { };
        }

        /// <summary>
        /// Retourne la liste de toutes les natures actives d'une société donnée
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureFamilleOdModel> GetNatureActiveFamilleOds(int societeId)
        {
            List<NatureFamilleOdModel> natureFamilleODModels = new List<NatureFamilleOdModel>();
            List<NatureEnt> natureEnts = GetNatureActiveBySocieteId(societeId).ToList();

            foreach (NatureEnt nature in natureEnts)
            {
                natureFamilleODModels.Add(new NatureFamilleOdModel
                {
                    IdNature = nature.NatureId,
                    NatureAnalytique = nature.Code,
                    Libelle = nature.Libelle,
                    ParentFamilyODWithOrder = nature.ParentFamilyODWithOrder,
                    ParentFamilyODWithoutOrder = nature.ParentFamilyODWithoutOrder,
                    DateCreation = nature.DateCreation
                });
            }
            return natureFamilleODModels;
        }

        /// <summary>
        /// Retourne la liste des natures d'une société donnée
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> GetNatureActiveBySocieteId(int societeId)
        {
            return GetNatureBySocieteId(societeId).Where(n => n.IsActif);
        }

        /// <summary>
        /// Permet de récupérer une nature.
        /// </summary>
        /// <param name="code">Le code.</param>
        /// <param name="societeId">L'identifiant de la société.</param>
        /// <returns>Une nature.</returns>
        public NatureEnt GetNatureActive(string code, int societeId)
        {
            try
            {
                return Repository.Query().Get().FirstOrDefault(x => x.Code == code && x.SocieteId == societeId && x.IsActif);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Méthode vérifiant l'existence d'une nature via son code.
        /// </summary>
        /// <param name="natureId">Identifiant courant</param>
        /// <param name="natureCode">Code Nature</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne vrai si la nature existe, faux sinon</returns>
        public bool IsNatureExistsByCode(int natureId, string natureCode, int societeId)
        {
            return Repository.IsNatureExistsByCode(natureId, natureCode, societeId);
        }

        /// <summary>
        /// Retourne une liste de natures filtrées selon des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les natures</param>
        /// <param name="filters">Filtres de recherche à prendre en compte</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> SearchNatureWithFilters(string text, SearchCriteriaEnt<NatureEnt> filters)
        {
            return Repository.SearchNatureWithFilters(GetPredicate(text, filters));
        }

        /// <summary>
        /// Permet l'initialisation d'une nouvelle instance de code absence.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Nouvelle instance de code absence intialisée</returns>
        public NatureEnt GetNewNature(int societeId)
        {
            return new NatureEnt { IsActif = true, SocieteId = societeId };
        }

        /// <summary>
        /// Permet de récupérer les champs de recherche lié à un code d'absence.
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'uncode d'absence</returns>
        public SearchCriteriaEnt<NatureEnt> GetDefaultFilter()
        {
            SearchCriteriaEnt<NatureEnt> recherche = new SearchCriteriaEnt<NatureEnt>();
            recherche.Code = true;
            recherche.Libelle = true;
            return recherche;
        }

        /// <summary>
        /// Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return Repository.IsAlreadyUsed(id);
        }

        /// <summary>
        /// Retourne une nature précise via son identifiant et le code société comptabilité
        /// </summary>
        /// <param name="code">Code Nature</param>
        /// <param name="codeSocieteCompta">Code société comptable (code anael)</param>
        /// <returns>Objet Nature correspondant</returns>
        public NatureEnt GetNature(string code, string codeSocieteCompta)
        {
            return Repository.Query()
                             .Get()
                             .FirstOrDefault(x => x.Code == code && x.Societe.CodeSocieteComptable == codeSocieteCompta && x.IsActif);
        }

        /// <summary>
        /// Retourne la liste des natures qui ne possèdent pas de famille.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des natures qui ne possèdent pas de famille</returns>
        public IEnumerable<NatureEnt> GetListNaturesWithoutFamille(int societeId)
        {
            return Repository.GetListNaturesWithoutFamille(societeId);
        }

        /// <summary>
        /// Retourne une nature en fonction de sont code de sa société
        /// </summary>
        /// <param name="code">Code de la nature</param>
        /// <param name="societeId">Identifiant du CI</param>
        /// <returns><see cref="NatureEnt" /></returns>
        public NatureEnt GetNature(string code, int societeId)
        {
            return Repository.Query()
                             .Include(x => x.ReferentielEtendus)
                             .Get()
                             .FirstOrDefault(x => x.Code == code && x.Societe.SocieteId == societeId && x.IsActif);
        }

        /// <summary>
        /// Retourne une nature en fonction de son code de sa société
        /// </summary>
        /// <param name="codeNatures">Code de la nature</param>
        /// <param name="societeIds">Liste d'identifiant du CI</param>
        /// <returns>Liste de <see cref="NatureEnt" /></returns>
        public IReadOnlyList<NatureEnt> GetNatures(List<string> codeNatures, List<int> societeIds)
        {
            return Repository.GetNatures(codeNatures, societeIds).ToList();
        }

        /// <summary>
        /// Retourne une liste de nature en fonction de leurs codes et d'une societé
        /// </summary>
        /// <param name="natureIds">Liste de codes</param>
        /// <param name="societeIds">Identifiant de la société</param>
        /// <returns>Une liste de <see cref="NatureEnt" /></returns>
        public IReadOnlyList<NatureEnt> GetNatures(List<int> natureIds, List<int> societeIds)
        {
            return Repository.Query()
                     .Include(x => x.ReferentielEtendus)
                     .Get()
                     .Where(x => natureIds.Contains(x.NatureId) && societeIds.Contains(x.Societe.SocieteId) && x.IsActif).ToList();
        }

        /// <summary>
        /// Mets à jour une liste de natures
        /// </summary>
        /// <param name="natures"><see cref="NatureFamilleOdModel" /></param>
        public void UpdateNatures(List<NatureFamilleOdModel> natures)
        {
            List<Expression<Func<NatureEnt, object>>> fieldsToUpdate = new List<Expression<Func<NatureEnt, object>>>
            {
                x => x.ParentFamilyODWithOrder,
                x => x.ParentFamilyODWithoutOrder,
                x => x.AuteurModificationId,
                x => x.DateModification,
                x => x.Libelle,
                x => x.DateCreation
            };

            List<NatureEnt> natureEnts = ConvertModelToEnt(natures);

            if (natureEnts.Count != 0)
            {
                foreach (NatureEnt nature in natureEnts)
                {
                    Repository.Update(nature, fieldsToUpdate);
                }

                Save();
            }
        }

        private List<NatureEnt> ConvertModelToEnt(List<NatureFamilleOdModel> natures)
        {
            List<NatureEnt> convertNatures = new List<NatureEnt>();

            foreach (NatureFamilleOdModel nature in natures)
            {
                convertNatures.Add(new NatureEnt
                {
                    AuteurModificationId = utilisateurManager.GetContextUtilisateurId(),
                    DateModification = DateTime.UtcNow,
                    ParentFamilyODWithOrder = nature.ParentFamilyODWithOrder,
                    ParentFamilyODWithoutOrder = nature.ParentFamilyODWithoutOrder,
                    NatureId = nature.IdNature,
                    Code = nature.NatureAnalytique,
                    Libelle = nature.Libelle,
                    DateCreation = nature.DateCreation
                });
            }
            return convertNatures;
        }

        /// <summary>
        /// Retourne la liste de toutes les natures pour une ressource donnée
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        public IEnumerable<NatureEnt> GetNatureListByRessourceId(int ressourceId)
        {
            return Repository.GetNatureListByRessourceId(ressourceId) ?? new NatureEnt[] { };
        }
    }
}

