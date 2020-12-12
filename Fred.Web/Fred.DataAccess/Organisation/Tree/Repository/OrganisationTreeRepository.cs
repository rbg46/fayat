using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Organisation.Tree.Models;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Entities.Organisation.Tree;
using Fred.EntityFramework;
using Fred.Framework.Cache;
using Fred.Framework.Extensions;

namespace Fred.DataAccess.Organisation.Tree.Repository
{
    /// <summary>
    /// Repository qui permet de recuperer les organisation ainsi que les affections
    /// </summary>
    public class OrganisationTreeRepository : IOrganisationTreeRepository
    {
        private readonly FredDbContext fredDbContext;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="fredDbContext">fredDbContext</param>        
        public OrganisationTreeRepository(FredDbContext fredDbContext)
        {
            this.fredDbContext = fredDbContext;
        }

        /// <summary>
        /// Retourne l'arbre des organisations de Fayat
        /// Construit l"abre des organisations de fred avec les organisations et les affectations de tous les utilisateurs
        /// IL NE FAUT PAS UTILISER CETTE FONCTIONS DANS UNE BOUCLE FOR OU FOREACH
        /// IL NE FAUT PAS RENVOYER L'OBJECT RETOURNER AU FRONT SAUF SI C'EST SUR UNE PAGE SECURISÉE => SUPER ADMIN SEULEMENT
        /// </summary>
        /// <returns>l'arbre des organisations de Fayat</returns>
        public OrganisationTree GetOrganisationTree()
        {
            var cache = new WaitToFinishMemoryCache();

            var organisationTree = cache.GetOrCreate(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey,
                                                    () => new OrganisationTree(GetOrganisations()),
                                                    new TimeSpan(0, 0, 3, 0, 0));
            return organisationTree;
        }

        /// <summary>
        /// Recuperes les organisations de fred avec les organisations et les affectations de tous les utilisateurs       
        /// </summary>
        /// <returns>Une liste d'organisation avec les information de base pour créer un arbre</returns>
        private List<OrganisationBase> GetOrganisations()
        {
            IQueryable<OrganisationBase> organisationsQuery = (from o in this.fredDbContext.Organisations
                                                               select new OrganisationBase()
                                                               {
                                                                   OrganisationId = o.OrganisationId,
                                                                   PereId = o.PereId,
                                                                   TypeOrganisationId = o.TypeOrganisationId
                                                               });

            var affectationsQuery = from o in this.fredDbContext.UtilOrgaRoleDevises
                                    select new AffectationBase()
                                    {
                                        OrganisationId = o.OrganisationId,
                                        AffectationId = o.AffectationRoleId,
                                        RoleId = o.RoleId,
                                        UtilisateurId = o.UtilisateurId,
                                    };

            var affectations = affectationsQuery.ToList();

            List<OrganisationBase> orgas = organisationsQuery.ToList();

            MapOrganisationsAndAffectations(affectations, orgas);

            SetEntities(orgas, GetHoldings);
            SetEntities(orgas, GetPoles);
            SetEntities(orgas, GetGroupes);
            SetEntities(orgas, GetSocietes);
            SetEntities(orgas, GetOrganisationsGeneriques);
            SetEntities(orgas, GetEtablissementsComptables);
            SetEntities(orgas, GetCis);

            return orgas;
        }

        private static void MapOrganisationsAndAffectations(List<AffectationBase> affectations, List<OrganisationBase> orgas)
        {
            var affectationsGroupedByOrganisationIds = affectations.GroupBy(x => x.OrganisationId);

            foreach (var affectationsGroupedByOrganisationId in affectationsGroupedByOrganisationIds)
            {
                var organisationId = affectationsGroupedByOrganisationId.Key;
                var organisation = orgas.FirstOrDefault(x => x.OrganisationId == organisationId);
                organisation.Affectations = affectationsGroupedByOrganisationId.ToList();
            }
        }

        private Dictionary<int, EntiteOrganisationBase> GetHoldings()
        {

            var query = (from o in this.fredDbContext.Holdings
                         select new EntiteOrganisationBase()
                         {
                             OrganisationId = o.Organisation.OrganisationId,
                             Id = o.HoldingId,
                             Code = o.Code,
                             Libelle = o.Libelle
                         });

            return query.ToDictionary(x => x.OrganisationId);
        }

        private Dictionary<int, EntiteOrganisationBase> GetGroupes()
        {
            return (from o in this.fredDbContext.Groupes
                    select new EntiteOrganisationBase()
                    {
                        OrganisationId = o.Organisation.OrganisationId,
                        Id = o.GroupeId,
                        Code = o.Code,
                        Libelle = o.Libelle
                    }).ToDictionary(x => x.OrganisationId);
        }

        private Dictionary<int, EntiteOrganisationBase> GetSocietes()
        {
            return (from o in this.fredDbContext.Societes
                    select new EntiteOrganisationBase()
                    {
                        OrganisationId = o.Organisation.OrganisationId,
                        Id = o.SocieteId,
                        Code = o.Code,
                        Libelle = o.Libelle
                    }).ToDictionary(x => x.OrganisationId);
        }

        private Dictionary<int, EntiteOrganisationBase> GetEtablissementsComptables()
        {
            return (from o in this.fredDbContext.EtablissementsComptables
                    select new EntiteOrganisationBase()
                    {
                        OrganisationId = o.Organisation.OrganisationId,
                        Id = o.EtablissementComptableId,
                        Code = o.Code,
                        Libelle = o.Libelle
                    }).ToDictionary(x => x.OrganisationId);
        }

        private Dictionary<int, EntiteOrganisationBase> GetCis()
        {
            return (from o in this.fredDbContext.CIs
                    select new EntiteOrganisationBase()
                    {
                        OrganisationId = o.Organisation.OrganisationId,
                        Id = o.CiId,
                        Code = o.Code,
                        Libelle = o.Libelle
                    }).ToDictionary(x => x.OrganisationId);

        }

        private Dictionary<int, EntiteOrganisationBase> GetPoles()
        {

            var query = (from o in this.fredDbContext.Poles
                         select new EntiteOrganisationBase()
                         {
                             OrganisationId = o.Organisation.OrganisationId,
                             Id = o.PoleId,
                             Code = o.Code,
                             Libelle = o.Libelle
                         });

            return query.ToDictionary(x => x.OrganisationId);
        }

        private Dictionary<int, EntiteOrganisationBase> GetOrganisationsGeneriques()
        {

            var query = (from o in this.fredDbContext.OrganisationsGeneriques
                         select new EntiteOrganisationBase()
                         {
                             OrganisationId = o.Organisation.OrganisationId,
                             Id = o.OrganisationGeneriqueId,
                             Code = o.Code,
                             Libelle = o.Libelle
                         });

            return query.ToDictionary(x => x.OrganisationId);
        }

        private void SetEntities(List<OrganisationBase> orgas, Func<Dictionary<int, EntiteOrganisationBase>> getEntitiesFunc)
        {
            Dictionary<int, EntiteOrganisationBase> entityDictionary = getEntitiesFunc();

            foreach (var orga in orgas)
            {
                bool ci = entityDictionary.ContainsKey(orga.OrganisationId);
                if (ci)
                {
                    EntiteOrganisationBase entiteOrganisationBase = entityDictionary[orga.OrganisationId];
                    orga.Id = entiteOrganisationBase.Id;
                    orga.Code = entiteOrganisationBase.Code;
                    orga.Libelle = entiteOrganisationBase.Libelle;
                }

            }
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
        public IEnumerable<OrganisationLightEnt> GetOrganisationsAvailable(
            string text = null,
            List<int> types = null,
            int? utilisateurId = null,
            int? organisationIdPere = null)
        {
            OrganisationTree mainOrganisationTree = GetOrganisationTree();

            OrganisationTree organisationTree = mainOrganisationTree;

            if (utilisateurId.HasValue)
            {
                organisationTree = organisationTree.GetOrganisationTreeForUser(utilisateurId.Value);
            }

            List<Node<OrganisationBase>> nodes = organisationTree.Nodes.ToList();

            if (organisationIdPere.HasValue)
            {
                nodes = nodes.Where(x => x.Data.OrganisationId == organisationIdPere).ToList();
            }

            var children = nodes.SelectMany(un => un.LevelOrder()).Distinct().ToList();

            Func<OrganisationBase, bool> filterAction = (OrganisationBase o) => types == null ? true : types.Contains(o.TypeOrganisationId);

            List<OrganisationBase> filterByTypes = children.Where(filterAction).ToList();

            List<OrganisationLightEnt> result = ToOrganisationLightEnts(filterByTypes);

            foreach (var organisationLight in result)
            {

                var organisationBasesParents = mainOrganisationTree.GetParents(organisationLight.OrganisationId);

                organisationLight.CodeSociete =
                    organisationBasesParents.FirstOrDefault(obp => obp.TypeOrganisationId == OrganisationType.Societe.ToIntValue())?.Code;

                organisationLight.CodeParent = organisationBasesParents.FirstOrDefault()?.Code ?? string.Empty;

                organisationLight.CodeParents = GetCodesParents(organisationBasesParents);

                organisationLight.IdParents = GetIdsParents(organisationBasesParents);
            }

            List<OrganisationLightEnt> filterByTexts = result.Where((o) => FilterText(text, o)).ToList();

            return filterByTexts.OrderBy(x => x.TypeOrganisationId).ThenBy(x => x.Code);
        }

        private bool FilterText(string text, OrganisationLightEnt o)
        {

            if (o.Libelle == null && o.Code == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(text))
            {
                return true;

            }

            if (o.Libelle != null && ComplexContains(o.Libelle, text))
            {
                return true;
            }

            if (o.Code != null && ComplexContains(o.Code, text))
            {
                return true;
            }

            if (o.CodeParent != null && ComplexContains(o.CodeParent, text))
            {
                return true;
            }
            return false;
        }

        private bool ComplexContains(string source, string value)
        {
            var index = CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                source, value, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            return index != -1;
        }


        private List<OrganisationLightEnt> ToOrganisationLightEnts(List<OrganisationBase> organisationBases)
        {
            return organisationBases.Select(x => ToOrganisationLightEnt(x)).ToList();
        }

        private OrganisationLightEnt ToOrganisationLightEnt(OrganisationBase organisationBase)
        {
            return new OrganisationLightEnt()
            {
                Code = organisationBase.Code,
                Libelle = organisationBase.Libelle?.Trim(),
                TypeOrganisationId = organisationBase.TypeOrganisationId,
                PereId = organisationBase.PereId,
                OrganisationId = organisationBase.OrganisationId,
                TypeOrganisation = organisationBase.GetOrganisationTypeLabel(),

            };
        }

        /// <summary>
        /// OBSOLETE : implementer pour une question de retrocompatibilité.
        /// </summary>
        /// <param name="organisationBases">organisationBases</param>     
        /// <returns>N/A</returns>
        private string GetCodesParents(List<OrganisationBase> organisationBases)
        {
            var reverse = organisationBases.OrderBy(x => x.TypeOrganisationId);

            return reverse.Select(x => x.Code).Aggregate((i, j) => i + "|" + j);
        }

        private string GetIdsParents(List<OrganisationBase> organisationBases)
        {
            var reverse = organisationBases.OrderBy(x => x.TypeOrganisationId);

            return reverse.Select(x => x.OrganisationId.ToString()).Aggregate((i, j) => i + "|" + j);
        }

    }

}
