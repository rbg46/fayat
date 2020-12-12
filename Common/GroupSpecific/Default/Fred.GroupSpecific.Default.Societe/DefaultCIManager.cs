using AutoMapper;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Organisation;
using Fred.Business.Organisation.Tree;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;

namespace Fred.GroupSpecific.Default
{
    public class DefaultCIManager : CIManager
    {
        public DefaultCIManager(
            IUnitOfWork uow,
            ICIRepository ciRepository,
            ICIValidator validator,
            IEtablissementComptableManager etabComptaManager,
            IOrganisationManager organisationManager,
            ISocieteManager societeManager,
            IUtilisateurManager userManager,
            IDatesClotureComptableManager dateClotureComptableManager,
            IOrganisationTreeService organisationTreeService,
            ISepService sepService,
            IContratInterimaireManager contratInterimaireManager,
            IAffectationRepository affectationRepo,
            IMapper mapper)
            : base(
                uow,
                ciRepository,
                validator,
                etabComptaManager,
                organisationManager,
                societeManager,
                userManager,
                dateClotureComptableManager,
                organisationTreeService,
                sepService,
                contratInterimaireManager,
                affectationRepo,
                mapper)
        {
        }

        protected override void OnGetFilter(SearchCIEnt searchCi)
        {
            base.OnGetFilter(searchCi);
        }
    }
}
