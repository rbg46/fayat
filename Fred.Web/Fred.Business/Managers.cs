using Fred.Business.Affectation;
using Fred.Business.CI;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Pointage;
using Fred.Business.ReferentielFixe;
using Fred.Business.Utilisateur;

namespace Fred.Business
{
    public class Managers : ManagersContainer
    {
        // Affectation
        private IAffectationManager affectationManager;

        // CI
        private ICIManager ciManager;

        // Personnel
        private IPersonnelManager personnelManager;

        // Rapport
        private IPointageManager pointageManager;

        // ReferentielFixe
        private IReferentielFixeManager referentielFixeManager;

        // Utilisateur
        private IUtilisateurManager utilisateurManager;



        public IAffectationManager Affectation
        {
            get
            {
                return LazyGetManager(ref affectationManager);
            }
            protected set
            {
                affectationManager = value;
            }
        }

        public ICIManager CI
        {
            get
            {
                return LazyGetManager(ref ciManager);
            }
            protected set
            {
                ciManager = value;
            }
        }

        public IPersonnelManager Personnel
        {
            get
            {
                return LazyGetManager(ref personnelManager);
            }
            protected set
            {
                personnelManager = value;
            }
        }

        public IPointageManager Pointage
        {
            get
            {
                return LazyGetManager(ref pointageManager);
            }
            protected set
            {
                pointageManager = value;
            }
        }

        public IReferentielFixeManager ReferentielFixe
        {
            get
            {
                return LazyGetManager(ref referentielFixeManager);
            }
            protected set
            {
                referentielFixeManager = value;
            }
        }

        public IUtilisateurManager Utilisateur
        {
            get
            {
                return LazyGetManager(ref utilisateurManager);
            }
            protected set
            {
                utilisateurManager = value;
            }
        }
    }
}
