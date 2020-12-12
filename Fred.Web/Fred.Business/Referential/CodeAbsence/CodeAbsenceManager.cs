using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.CI;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using static Fred.Entities.Constantes;

namespace Fred.Business.Referential.CodeAbsence
{
    /// <summary>
    ///   Gestionnaire des codes d'absence
    /// </summary>
    public class CodeAbsenceManager : Manager<CodeAbsenceEnt, ICodeAbsenceRepository>, ICodeAbsenceManager
    {
        private readonly ICIManager ciManager;

        public CodeAbsenceManager(IUnitOfWork uow, ICodeAbsenceRepository codeAbsRepo, ICIManager ciManager)
          : base(uow, codeAbsRepo)
        {
            this.ciManager = ciManager;
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un code absence.
        /// </summary>
        /// <param name="text">Texte recherché dans les codes absences</param>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <returns>Retourne la condition de recherche des codes absences</returns>
        private Expression<Func<CodeAbsenceEnt, bool>> GetPredicate(string text, SearchCodeAbsenceEnt filters)
        {
            return p => (string.IsNullOrEmpty(text)
                         || filters.Code && p.Code.ToLower().Contains(text.ToLower())
                         || filters.Libelle && p.Libelle.ToLower().Contains(text.ToLower()))
                        && (!filters.Actif || p.Actif)
                        && (!filters.Intemperie || p.Intemperie);
        }

        /// <summary>
        ///   Ajout d'une code d'absence
        /// </summary>
        /// <param name="codeAbs">Code d'absence à ajouter</param>
        /// <returns>L'identifiant du code d'absence ajouté</returns>
        public int AddCodeAbsence(CodeAbsenceEnt codeAbs)
        {
            return this.Repository.AddCodeAbsence(codeAbs);
        }

        /// <summary>
        ///   Supprime un code d'absence
        /// </summary>
        /// <param name="codeAbsenceEnt">Code d'absence à supprimer</param>
        public void DeleteCodeAbsenceById(CodeAbsenceEnt codeAbsenceEnt)
        {
            try
            {
                if (this.Repository.IsDeletable(codeAbsenceEnt))
                {
                    this.Repository.Delete(codeAbsenceEnt);
                    Save();
                }
                else
                {
                    throw new FredBusinessException("Impossible de supprimer cet élément car il est déjà utilisé.");
                }
            }
            catch (FredRepositoryException repoEx)
            {
                throw new FredBusinessException(repoEx.Message);
            }
        }

        /// <summary>
        ///   La liste de tous les codes d'absence.
        /// </summary>
        /// <returns>Renvoie la liste de des codes d'absence active</returns>
        public IEnumerable<CodeAbsenceEnt> GetCodeAbsList()
        {
            return this.Repository.GetCodeAbsList();
        }

        /// <summary>
        ///   Retourne la liste de tous les codes d'absence.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        public IEnumerable<CodeAbsenceEnt> GetCodeAbsListAll()
        {
            return this.Repository.GetCodeAbsListAll();
        }


        /// <summary>
        ///   Retourne la liste de tous les codes d'absence pour la synchronisation mobille.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        public IEnumerable<CodeAbsenceEnt> GetCodeAbsListAllSync()
        {
            return this.Repository.GetCodeAbsListAllSync();
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeAbsence
        /// </summary>
        /// <param name="codeAbs">Code absence à modifier</param>
        public void UpdateCodeAbsence(CodeAbsenceEnt codeAbs)
        {
            this.Repository.UpdateCodeAbsence(codeAbs);
        }

        /// <summary>
        ///   Code d'absence via l'id
        /// </summary>
        /// <param name="id">Id du code d'absence</param>
        /// <returns>Renvoie un code d'absence</returns>
        public CodeAbsenceEnt GetCodeAbsenceById(int id)
        {
            return this.Repository.GetCodeAbsenceById(id);
        }

        /// <summary>
        ///   Retourne le codeAbsence correspondant au code
        /// </summary>
        /// <param name="code">Le code de l'absence</param>
        /// <returns>Renvoie un code d'absence</returns>
        public CodeAbsenceEnt GetCodeAbsenceByCode(string code)
        {
            return this.Repository.GetCodeAbsenceByCode(code);
        }

        /// <summary>
        ///   Code d'absence via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <returns>Renvoie un code d'absence</returns>
        public IEnumerable<CodeAbsenceEnt> GetCodeAbsenceBySocieteId(int societeId)
        {
            return this.Repository.GetCodeAbsenceBySocieteId(societeId);
        }

        /// <summary>
        ///   Import des codes absences automatiques depuis la Holding
        /// </summary>
        /// <param name="holdingId"> Id du Holding</param>
        /// <param name="idNewSociete"> Id de la nouvelle société</param>
        /// <returns>Renvoie un int</returns>
        public int ImportCodeAbsFromHolding(int holdingId, int idNewSociete)
        {
            return this.Repository.ImportCodeAbsFromHolding(holdingId, idNewSociete);
        }

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de code absence.
        /// </summary>
        /// <param name="societeId">Id de la societe</param>
        /// <returns>Nouvelle instance de code absence intialisée</returns>
        public CodeAbsenceEnt GetNewCodeAbsence(int societeId)
        {
            return new CodeAbsenceEnt { Actif = true, Intemperie = false, SocieteId = societeId };
        }

        /// <summary>
        ///   Permet de récupérer la liste des codes d'absence en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les sociétés</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré des sociétés</returns>
        public IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceWithFilters(string text, SearchCodeAbsenceEnt filters)
        {
            return this.Repository.SearchCodeAbsenceWithFilters(GetPredicate(text, filters));
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes absences en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="filters">Filtres de recherche sur tous les codes absences</param>
        /// <param name="societeId">Id de la societe</param>
        /// <param name="text">Le text de recherche</param>
        /// <returns>Retourne la liste filtrée de tous les codes absences</returns>
        public IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceAllBySocieteIdWithFilters(SearchCodeAbsenceEnt filters, int societeId, string text)
        {
            return this.Repository.SearchCodeAbsenceAllBySocieteIdWithFilters(GetPredicate(text, filters), societeId);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes absences en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans tous les codes absences</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré de tous les codes absences</returns>
        public IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceAllWithFilters(string text, SearchCodeAbsenceEnt filters)
        {
            return this.Repository.SearchCodeAbsenceAllWithFilters(GetPredicate(text, filters));
        }

        /// <summary>
        ///   Permet de connaître l'existence d'une société depuis son code.
        /// </summary>
        /// <param name="idCourant">L'Id courant</param>
        /// <param name="codeCodeAbsence">Le code de codeCodeAbsence</param>
        /// <param name="societeId">Le code de la société</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsCodeAbsenceExistsByCode(int idCourant, string codeCodeAbsence, int societeId)
        {
            return this.Repository.IsCodeAbsenceExistsByCode(idCourant, codeCodeAbsence, societeId);
        }

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié à un code d'absence.
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'uncode d'absence</returns>
        public SearchCodeAbsenceEnt GetDefaultFilter()
        {
            var recherche = new SearchCodeAbsenceEnt();
            recherche.Code = true;
            recherche.Libelle = true;
            return recherche;
        }

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Une liste d' items de référentiel</returns>
        public IEnumerable<CodeAbsenceEnt> SearchLight(string text, int page, int pageSize, int ciId = 0, int? societeId = 0, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null)
        {
            if (!societeId.HasValue || societeId.Value == 0)
            {
                var ci = ciManager.GetCiById(ciId, true);
                if (ci == null)
                {
                    return new List<CodeAbsenceEnt>();
                }

                if (ci.Societe?.TypeSociete?.Code == TypeSociete.Sep)
                {
                    var societeGerante = ci.Societe.AssocieSeps.SingleOrDefault(a => a.TypeParticipationSep.Code == TypeParticipationSep.Gerant && a.AssocieSepParentId == null);
                    societeId = societeGerante.SocieteAssocieeId;
                }
                else
                {
                    societeId = ci.SocieteId;
                }
            }

            var codesAbsences = this.Repository.Query()
                                .Filter(c => c.Actif)
                                .Filter(c => c.GroupeId == null)
                                .Filter(p => !isCadre.HasValue || (isCadre.HasValue && p.IsCadre == isCadre.HasValue))
                                .Filter(p => !isOuvrier.HasValue || (isOuvrier.HasValue && p.IsOuvrier == isOuvrier.HasValue))
                                .Filter(p => !isEtam.HasValue || (isEtam.HasValue && p.IsETAM == isEtam.HasValue))
                                .Filter(c => !societeId.HasValue || c.SocieteId == societeId)
                                .Filter(c => string.IsNullOrEmpty(text) || c.Code.ToLower().Contains(text.ToLower()) || c.Libelle.ToLower().Contains(text.ToLower()))
                                .OrderBy(ls => ls.OrderBy(c => c.Code))
                                .GetPage(page, pageSize).ToList();

            return codesAbsences;
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
    }
}
