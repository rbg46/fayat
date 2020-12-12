using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Web.Shared.App_LocalResources;
namespace Fred.Business.Societe
{
    public class AssocieSepManager : Manager<AssocieSepEnt, IAssocieSepRepository>, IAssocieSepManager
    {
        private readonly ITypeParticipationSepManager typeParticipationSepManager;

        public AssocieSepManager(
            IUnitOfWork uow,
            IAssocieSepRepository associeSepRepository,
            IAssocieSepValidator validator,
            ITypeParticipationSepManager typeParticipationSepManager)
            : base(uow, associeSepRepository, validator)
        {
            this.typeParticipationSepManager = typeParticipationSepManager;
        }

        /// <summary>
        ///  Récupération tous les  associés SEP
        /// </summary>
        /// <returns>Liste des associés SEP</returns>
        public IQueryable<AssocieSepEnt> GetAll()
        {
            return Repository.Get();
        }

        /// <summary>
        ///     Récupération des associés SEP
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Liste des associés SEP</returns>
        public IEnumerable<AssocieSepEnt> GetAll(int societeId)
        {
            var filters = new List<Expression<Func<AssocieSepEnt, bool>>> { x => x.SocieteId == societeId, x => !x.AssocieSepParentId.HasValue };
            var includes = new List<Expression<Func<AssocieSepEnt, object>>>
            {
                x => x.Fournisseur,
                x => x.SocieteAssociee,
                x => x.TypeParticipationSep,
                x => x.AssocieSepChildren.Select(y => y.SocieteAssociee)
            };

            return Repository.Search(filters, null, includes, null, null).ToList();
        }

        /// <summary>
        ///   Retourner la requête de récupération des associés SEP
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        public List<AssocieSepEnt> Search(List<Expression<Func<AssocieSepEnt, bool>>> filters,
                                          Func<IQueryable<AssocieSepEnt>, IOrderedQueryable<AssocieSepEnt>> orderBy = null,
                                          List<Expression<Func<AssocieSepEnt, object>>> includeProperties = null,
                                          int? page = null,
                                          int? pageSize = null,
                                          bool asNoTracking = false)
        {
            return Repository.Search(filters, orderBy, includeProperties, page, pageSize, asNoTracking);
        }

        /// <summary>
        ///     Insertion d'une liste d'associés SEP
        /// </summary>        
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="associeSeps">Liste d'associés SEP</param>
        /// <returns>Nouvelle liste d'associés SEP</returns>
        public List<AssocieSepEnt> CreateOrUpdateRange(int societeId, List<AssocieSepEnt> associeSeps)
        {
            RulesForAssocieSepList(societeId, associeSeps);

            foreach (AssocieSepEnt associeSep in associeSeps)
            {
                BusinessValidation(associeSep);
                associeSep.ClearProperties();

                if (associeSep.AssocieSepId > 0)
                {
                    Repository.Update(associeSep);
                }
                else
                {
                    Repository.Insert(associeSep);
                }
            }

            Save();

            return associeSeps;
        }

        /// <summary>
        /// Suppression d'une liste d'associés SEP par leur identifiants        
        /// </summary>
        /// <param name="associeSepIds">Liste d'identifiants d'associés SEP</param>
        public void DeleteRange(List<int> associeSepIds)
        {
            foreach (int associeSepId in associeSepIds)
            {
                Repository.DeleteById(associeSepId);
            }

            Save();
        }

        /// <summary>
        /// Vérification sur une liste d'associés SEP
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="associeSeps">Liste d'associés SEP</param>
        private void RulesForAssocieSepList(int societeId, List<AssocieSepEnt> associeSeps)
        {
            if (societeId == 0)
            {
                throw new ArgumentNullException(nameof(societeId));
            }

            if (associeSeps == null || associeSeps.Count == 0)
            {
                throw new ArgumentNullException(nameof(associeSeps));
            }

            List<int> excludes = associeSeps.Select(x => x.AssocieSepId).ToList();

            List<AssocieSepEnt> savedAssocieSeps = Repository.Query()
                                                             .Filter(x => x.SocieteId == societeId && !x.AssocieSepParentId.HasValue)
                                                             .Filter(x => !excludes.Contains(x.AssocieSepId))
                                                             .Get()
                                                             .ToList();

            TypeParticipationSepEnt typeAssocie = typeParticipationSepManager.Get(Constantes.TypeParticipationSep.Associe);

            // Fusion de la liste déjà enregistrée en BD et celle issue du client Front
            List<AssocieSepEnt> associeSepsWihtoutChildren = savedAssocieSeps.Concat(associeSeps.Where(x => !x.AssocieSepParentId.HasValue && x.AssocieSepParent == null)).ToList();

            // Vérifier s'il n'y a pas de type de participation en doublons (sans regarder les enfants)
            bool isUniqueTypeParticipationSep = associeSepsWihtoutChildren.Count(x => x.TypeParticipationSepId != typeAssocie.TypeParticipationSepId) == associeSepsWihtoutChildren.Where(x => x.TypeParticipationSepId != typeAssocie.TypeParticipationSepId).Select(x => x.TypeParticipationSepId).Distinct().Count();

            List<ValidationFailure> validationFails = new List<ValidationFailure>();

            if (!isUniqueTypeParticipationSep)
            {
                validationFails.Add(new ValidationFailure("TypeParticipationSep", FeatureSociete.AssocieSep_TypeParticipation_Unique));
            }

            bool isTotalQuotePart100 = associeSepsWihtoutChildren.Sum(x => x.QuotePart) == 100;

            if (!isTotalQuotePart100)
            {
                validationFails.Add(new ValidationFailure("QuotePart", FeatureSociete.AssocieSep_QuotePart_Error));
            }

            if (validationFails.Count > 0)
            {
                throw new ValidationException(validationFails);
            }
        }
    }
}
