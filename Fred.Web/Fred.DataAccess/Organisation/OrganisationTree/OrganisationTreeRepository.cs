using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation.Tree;
using System.Collections.Generic;
using System.Linq;

namespace Fred.DataAccess.OrganisationTree
{
    /// <summary>
    /// Repository qui permet de recuperer les organisation ainsi que les affections
    /// </summary>
    public class OrganisationTreeRepository : IOrganisationTreeRepository
    {
        private readonly EntityFramework.FredDbContext fredDbContext;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="fredDbContext">fredDbContext</param>
        public OrganisationTreeRepository(EntityFramework.FredDbContext fredDbContext)
        {
            this.fredDbContext = fredDbContext;
        }

        /// <summary>
        /// Recuperes les organisations de fred avec les organisations et les affectations de tous les utilisateurs       
        /// </summary>
        /// <returns>Une liste d'organisation avec les information de base pour créer un arbre</returns>
        public List<OrganisationBase> GetOrganisations()
        {

            IQueryable<OrganisationBase> query = (from o in this.fredDbContext.Organisations
                                                  select new OrganisationBase()
                                                  {
                                                      OrganisationId = o.OrganisationId,
                                                      PereId = o.PereId,
                                                      TypeOrganisationId = o.TypeOrganisationId,
                                                      Affectations = this.fredDbContext.UtilOrgaRoleDevises.Where(uord => uord.OrganisationId == o.OrganisationId)
                                                      .Select(a => new AffectationBase
                                                      {
                                                          OrganisationId = o.OrganisationId,
                                                          AffectationId = a.AffectationRoleId,
                                                          RoleId = a.RoleId,
                                                          UtilisateurId = a.UtilisateurId,
                                                      }).ToList()
                                                  });



            List<OrganisationBase> orgas = query.ToList();


            Dictionary<int, EntiteOrganisationBase> holdings = GetHoldings();

            Dictionary<int, EntiteOrganisationBase> groupes = GetSocietes();

            Dictionary<int, EntiteOrganisationBase> societes = GetGroupes();

            Dictionary<int, EntiteOrganisationBase> etablissements = GetEtablissementsComptables();

            Dictionary<int, EntiteOrganisationBase> cis = GetCis();

            Dictionary<int, EntiteOrganisationBase> poles = GetPoles();

            Dictionary<int, EntiteOrganisationBase> organisationsGeneriques = GetOrganisationsGeneriques();

            SetEntities(orgas, holdings);
            SetEntities(orgas, groupes);
            SetEntities(orgas, societes);
            SetEntities(orgas, etablissements);
            SetEntities(orgas, cis);
            SetEntities(orgas, poles);
            SetEntities(orgas, organisationsGeneriques);


            return orgas;
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
        private void SetEntities(List<OrganisationBase> orgas, Dictionary<int, EntiteOrganisationBase> cis)
        {
            foreach (var orga in orgas)
            {
                bool ci = cis.ContainsKey(orga.OrganisationId);
                if (ci)
                {
                    EntiteOrganisationBase entiteOrganisationBase = cis[orga.OrganisationId];
                    orga.Id = entiteOrganisationBase.Id;
                    orga.Code = entiteOrganisationBase.Code;
                    orga.Libelle = entiteOrganisationBase.Libelle;
                }

            }
        }


    }

}
