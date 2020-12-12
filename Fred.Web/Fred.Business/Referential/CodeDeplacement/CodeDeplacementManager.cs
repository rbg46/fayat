using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.CI;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using static Fred.Entities.Constantes;

namespace Fred.Business.Referential.CodeDeplacement
{
    /// <summary>
    ///   Représente un référentiel de données pour les codes déplacement
    /// </summary>
    public class CodeDeplacementManager : Manager<CodeDeplacementEnt, ICodeDeplacementRepository>, ICodeDeplacementManager
    {
        private readonly ICIManager ciManager;

        public CodeDeplacementManager(IUnitOfWork uow, ICodeDeplacementRepository codeDepRepo, ICodeDeplacementValidator validator, ICIManager ciManager)
          : base(uow, codeDepRepo, validator)
        {
            this.ciManager = ciManager;
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un code déplacement.
        /// </summary>
        /// <param name="text">Texte recherché dans les codes déplacement</param>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <returns>Retourne la condition de recherche des codes déplacement</returns>
        private Expression<Func<CodeDeplacementEnt, bool>> GetPredicate(string text, SearchCodeDeplacementEnt filters)
        {
            if (string.IsNullOrEmpty(text))
            {
                return p => !filters.Actif || p.Actif;
            }

            return p => (filters.Code && p.Code.ToLower().Contains(text.ToLower()) || filters.Libelle && p.Libelle.ToLower().Contains(text.ToLower()))
                        && (!filters.Actif || p.Actif);
        }

        /// <summary>
        ///   liste des codes déplacement pour mobile
        /// </summary>
        /// <param name="sinceDate">The since date.</param>
        /// <param name="userId">Id utilisateur connecté.</param>
        /// <returns>Liste des codes déplacement pour le mobile.</returns>
        public IQueryable<CodeDeplacementEnt> GetAllMobile(DateTime? sinceDate = null, int? userId = null)
        {
            var repository = Repository;
            var query = repository
              .Query()
              ////.Filter(r => (sinceDate == null || r.DateCreation >= sinceDate.Value || r.DateModification >= sinceDate.Value || r.DateSuppression >= sinceDate.Value))
              .Get();

            return query;
        }

        /// <summary>
        ///   Retourne la liste des codesDeplacement.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <param name="actif">True pour les actifs, false pour les inactifs, null pour tous.</param>
        /// <returns>La liste des codesDeplacement.</returns>
        public IEnumerable<CodeDeplacementEnt> GetCodeDeplacementList(int societeId, bool? actif = null)
        {
            var query = this.Repository.Query().Filter(cd => cd.SocieteId == societeId && (!actif.HasValue || cd.Actif == actif.Value));
            return query.Get().ToList();
        }

        /// <summary>
        ///   Permet de recuperer le code déplacement depuis son code et sa societeId.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <param name="code">Le code de déplacement</param>
        /// <returns>Le CodeDeplacement eyant le meme code pour une sociétéId donnée</returns>
        public CodeDeplacementEnt GetBySocieteIdAndCode(int societeId, string code)
        {
            return this.Repository.GetBySocieteIdAndCode(societeId, code);
        }

        /// <summary>
        ///   Permet de connaître l'existence d'un code déplacement depuis son code.
        /// </summary>
        /// <param name="idCourant">L'Id courant</param>
        /// <param name="code">Le code de déplacement</param>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool CodeDeplacementExistForSocieteIdAndCode(int idCourant, string code, int societeId)
        {
            return this.Repository.CodeDeplacementExistForSocieteIdAndCode(idCourant, code, societeId);
        }

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de code déplacement.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Nouvelle instance de code déplacement intialisée</returns>
        public CodeDeplacementEnt GetNewCodeDeplacement(int societeId)
        {
            return new CodeDeplacementEnt { Actif = true, SocieteId = societeId };
        }

        /// <summary>
        ///   Ajout un nouveau CodeDeplacement
        /// </summary>
        /// <param name="codeDeplacementEnt"> CodeDeplacement à ajouter</param>
        /// <returns> L'identifiant du CodeDeplacement ajouté</returns>
        public CodeDeplacementEnt AddCodeDeplacement(CodeDeplacementEnt codeDeplacementEnt)
        {
            this.Repository.Insert(codeDeplacementEnt);
            Save();
            return codeDeplacementEnt;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeDeplacement
        /// </summary>
        /// <param name="codeDeplacementEnt">CodeDeplacement à modifier</param>
        /// <returns>CodeDeplacementEnt</returns>
        public CodeDeplacementEnt UpdateCodeDeplacement(CodeDeplacementEnt codeDeplacementEnt)
        {
            this.Repository.Update(codeDeplacementEnt);
            Save();
            return codeDeplacementEnt;
        }

        /// <summary>
        ///   Supprime un CodeDeplacement
        /// </summary>
        /// <param name="codeDeplacementId">ID du CodeDeplacement à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        public bool DeleteCodeDeplacementById(int codeDeplacementId)
        {
            if (IsDeletable(codeDeplacementId))
            {
                this.Repository.DeleteById(codeDeplacementId);
                Save();
                return true;
            }

            throw new FredBusinessException(CodeDeplacementResources.CodeDeplacement_ImpossibleToDelete);
        }

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un CodeDeplacement
        /// </summary>
        /// <param name="codeDeplacementId">Code deplacement à supprimer</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(int codeDeplacementId)
        {
            return this.Repository.IsDeletable(codeDeplacementId);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes déplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <param name="text"> Texte recherché dans tous les codes déplacement </param>
        /// <param name="filters"> Filtres de recherche </param>
        /// <returns> Retourne la liste filtré de tous les codes déplacement </returns>
        public IEnumerable<CodeDeplacementEnt> SearchCodeDepAllWithFilters(int societeId, string text, SearchCodeDeplacementEnt filters)
        {
            return this.Repository.SearchCodeDepAllWithFilters(societeId, GetPredicate(text, filters));
        }

        /// <summary>
        ///   Permet de récupérer la liste des codes déplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les codes déplacement</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré des codes déplacement</returns>
        public IEnumerable<CodeDeplacementEnt> SearchCodeDepWithFilters(string text, SearchCodeDeplacementEnt filters)
        {
            return this.Repository.SearchCodeDepWithFilters(GetPredicate(text, filters));
        }

        /// <summary>
        ///   Méthode de recherche des codes déplacement dans le référentiel
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>Une liste de code déplacement</returns>
        public IEnumerable<CodeDeplacementEnt> SearchLight(string text, int page, int pageSize, int? ciId = null, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null)
        {
            int? societeId = null;
            if (ciId.HasValue)
            {
                var ci = ciManager.GetCiById(ciId.Value, true);
                if (ci.Societe.TypeSociete.Code == TypeSociete.Sep)
                {
                    var societeGerante = ci.Societe.AssocieSeps.SingleOrDefault(a => a.TypeParticipationSep.Code == TypeParticipationSep.Gerant && a.AssocieSepParentId == null);
                    societeId = societeGerante.SocieteAssocieeId;
                }
                else
                {
                    societeId = ci.SocieteId;
                }
            }

            return this.Repository
                       .Query()
                       .Filter(c => c.Actif)
                       .Filter(c => !isCadre.HasValue || isCadre.HasValue && c.IsCadre == isCadre)
                       .Filter(c => !isEtam.HasValue || isEtam.HasValue && c.IsETAM == isCadre)
                       .Filter(c => !isOuvrier.HasValue || isOuvrier.HasValue && c.IsOuvrier == isOuvrier)
                       .Filter(c => !societeId.HasValue || c.SocieteId == societeId)
                       .Filter(c => string.IsNullOrEmpty(text) || c.Code.ToLower().Contains(text.ToLower()) || c.Libelle.ToLower().Contains(text.ToLower()))
                       .OrderBy(ls => ls.OrderBy(c => c.Code))
                       .GetPage(page, pageSize);
        }

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return this.Repository.IsAlreadyUsed(id);
        }


        /// <summary>
        ///   Retourne le CodeDeplacement portant le code indiqué.
        /// </summary>
        /// <param name="codeDeplacement">Code déplacement à retrouver.</param>
        /// <returns>Le code déplacement retrouvé, sinon nulle.</returns>
        public CodeDeplacementEnt GetCodeDeplacementByCode(string codeDeplacement)
        {
            return this.Repository.GetCodeDeplacementByCode(codeDeplacement);
        }
    }
}
