using Fred.Business.IndemniteDeplacement;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeAbsence;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Business.Referential.Materiel;
using Fred.Business.Referential.Nature;
using Fred.Business.Referential.Tache;

namespace Fred.Business
{
    /// <summary>
    /// Représente tous les managers du référentiel.
    /// </summary>
    public class ManagersReferential : ManagersContainer
    {
        #region Champs

        // CodeAbsence
        private ICodeAbsenceManager codeAbsenceManager;

        // CodeDeplacement
        private ICodeDeplacementManager codeDeplacementManager;

        // CodeMajoration
        private ICodeMajorationManager codeMajorationManager;

        // CodeZoneDeplacement
        private ICodeZoneDeplacementManager codeZoneDeplacementManager;

        // Devise
        private IDeviseManager deviseManager;

        // EtablissementComptable
        private IEtablissementComptableManager etablissementComptableManager;

        // EtablissementPaie
        private IEtablissementPaieManager etablissementPaieManager;

        // Fournisseur
        private IFournisseurManager fournisseurManager;

        // IndemniteDeplacement
        private IIndemniteDeplacementManager indemniteDeplacementManager;

        // Materiel
        private IMaterielManager materielManager;

        // Nature
        private INatureManager natureManager;

        // Pays
        private IPaysManager paysManager;

        // Prime
        private IPrimeManager primeManager;

        // Tache
        private ITacheManager tacheManager;

        // TypeDepense
        private ITypeDepenseManager typeDepenseManager;

        // TypeRattachement
        private ITypeRattachementManager typeRattachementManager;

        #endregion

        #region CodeAbsence

        /// <summary>
        /// Gestionnaire des codes d'absence
        /// </summary>
        public ICodeAbsenceManager CodeAbsence
        {
            get
            {
                return LazyGetManager(ref codeAbsenceManager);
            }
            protected set
            {
                codeAbsenceManager = value;
            }
        }

        #endregion
        #region CodeDeplacement

        /// <summary>
        /// Gestionnaire des codes de déplacement.
        /// </summary>
        public ICodeDeplacementManager CodeDeplacement
        {
            get
            {
                return LazyGetManager(ref codeDeplacementManager);
            }
            protected set
            {
                codeDeplacementManager = value;
            }
        }

        #endregion
        #region CodeMajoration

        /// <summary>
        /// Gestionnaire des codes de majoration.
        /// </summary>
        public ICodeMajorationManager CodeMajoration
        {
            get
            {
                return LazyGetManager(ref codeMajorationManager);
            }
            protected set
            {
                codeMajorationManager = value;
            }
        }

        #endregion
        #region CodeZoneDeplacement

        /// <summary>
        /// Gestionnaire des codes zone de déplacement.
        /// </summary>
        public ICodeZoneDeplacementManager CodeZoneDeplacement
        {
            get
            {
                return LazyGetManager(ref codeZoneDeplacementManager);
            }
            protected set
            {
                codeZoneDeplacementManager = value;
            }
        }

        #endregion
        #region Devise

        /// <summary>
        /// Gestionnaire des devises.
        /// </summary>
        public IDeviseManager Devise
        {
            get
            {
                return LazyGetManager(ref deviseManager);
            }
            protected set
            {
                deviseManager = value;
            }
        }

        #endregion
        #region EtablissementComptable

        /// <summary>
        /// Gestionnaire des établissements comptables.
        /// </summary>
        public IEtablissementComptableManager EtablissementComptable
        {
            get
            {
                return LazyGetManager(ref etablissementComptableManager);
            }
            protected set
            {
                etablissementComptableManager = value;
            }
        }

        #endregion
        #region EtablissementPaie

        /// <summary>
        /// Gestionnaire des établissements de paie.
        /// </summary>
        public IEtablissementPaieManager EtablissementPaie
        {
            get
            {
                return LazyGetManager(ref etablissementPaieManager);
            }
            protected set
            {
                etablissementPaieManager = value;
            }
        }

        #endregion
        #region Fournisseur

        /// <summary>
        /// Gestionnaire des fournisseurs.
        /// </summary>
        public IFournisseurManager Fournisseur
        {
            get
            {
                return LazyGetManager(ref fournisseurManager);
            }
            protected set
            {
                fournisseurManager = value;
            }
        }

        #endregion
        #region IndemniteDeplacement

        /// <summary>
        /// Gestionnaire des indemnités de déplacement.
        /// </summary>
        public IIndemniteDeplacementManager IndemniteDeplacement
        {
            get
            {
                return LazyGetManager(ref indemniteDeplacementManager);
            }
            protected set
            {
                indemniteDeplacementManager = value;
            }
        }

        #endregion
        #region Materiel

        /// <summary>
        /// Gestionnaire des matériels.
        /// </summary>
        public IMaterielManager Materiel
        {
            get
            {
                return LazyGetManager(ref materielManager);
            }
            protected set
            {
                materielManager = value;
            }
        }

        #endregion
        #region Nature

        /// <summary>
        /// Gestionnaire des natures.
        /// </summary>
        public INatureManager Nature
        {
            get
            {
                return LazyGetManager(ref natureManager);
            }
            protected set
            {
                natureManager = value;
            }
        }

        #endregion
        #region Pays

        /// <summary>
        /// Gestionnaire des pays.
        /// </summary>
        public IPaysManager Pays
        {
            get
            {
                return LazyGetManager(ref paysManager);
            }
            protected set
            {
                paysManager = value;
            }
        }

        #endregion
        #region Prime

        /// <summary>
        /// Gestionnaire des primes.
        /// </summary>
        public IPrimeManager Prime
        {
            get
            {
                return LazyGetManager(ref primeManager);
            }
            protected set
            {
                primeManager = value;
            }
        }

        #endregion
        #region Tache

        /// <summary>
        /// Gestionnaire des tâches.
        /// </summary>
        public ITacheManager Tache
        {
            get
            {
                return LazyGetManager(ref tacheManager);
            }
            protected set
            {
                tacheManager = value;
            }
        }

        #endregion
        #region TypeDepense

        /// <summary>
        /// Gestionnaire des types de dépense.
        /// </summary>
        public ITypeDepenseManager TypeDepense
        {
            get
            {
                return LazyGetManager(ref typeDepenseManager);
            }
            protected set
            {
                typeDepenseManager = value;
            }
        }

        #endregion
        #region TypeRattachement

        /// <summary>
        /// Gestionnaire des types de rattachement.
        /// </summary>
        public ITypeRattachementManager TypeRattachement
        {
            get
            {
                return LazyGetManager(ref typeRattachementManager);
            }
            protected set
            {
                typeRattachementManager = value;
            }
        }

        #endregion
    }
}
