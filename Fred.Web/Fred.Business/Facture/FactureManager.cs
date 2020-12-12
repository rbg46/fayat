using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Fred.Business.CI;
using Fred.Business.Journal;
using Fred.Business.Parametre;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Facture;
using Fred.Entities.Journal;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.Business.Facture
{
    /// <summary>
    /// Gestionnaire des factures
    /// </summary>
    public class FactureManager : Manager<FactureEnt, IFactureArRepository>, IFactureManager
    {
        private readonly ILogImportRepository repoLogImport;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICIManager ciManager;
        private readonly ISocieteManager societeManager;
        private readonly IJournalManager journalManager;
        private readonly IEtablissementComptableManager etabComptableManager;
        private readonly IParametreManager paramManager;

        public FactureManager(
            IUnitOfWork uow,
            IFactureArRepository factureArRepository,
            IUtilisateurManager userManager,
            ICIManager ciManager,
            ISocieteManager societeManager,
            IJournalManager journalManager,
            IEtablissementComptableManager etabComptableManager,
            IParametreManager paramManager,
            ILogImportRepository repoLogImport)
          : base(uow, factureArRepository)
        {
            this.utilisateurManager = userManager;
            this.repoLogImport = repoLogImport;
            this.ciManager = ciManager;
            this.societeManager = societeManager;
            this.journalManager = journalManager;
            this.etabComptableManager = etabComptableManager;
            this.paramManager = paramManager;
        }

        /// <summary>
        /// Enumeration de type d'import
        /// </summary>
        private enum TypeImport
        {
            /// <summary>
            /// Utilisé lors de l'import de facture à rapprocher
            /// </summary>
            FactureAR,

            /// <summary>
            /// Utilisé lors de l'import de ligne de facture à rapprocher
            /// </summary>
            FactureArLigne
        }

        /// <summary>
        /// Retourne une nouvelle instance de facture AR.
        /// </summary>
        /// <returns>Retourne un nouveau favori.</returns>
        public FactureEnt GetNewFactureAR()
        {
            return new FactureEnt();
        }

        /// <summary>
        /// Retourne une facture
        /// </summary>
        /// <param name="factureId">Identifiant de la facture</param>
        /// <returns>Retourne une Facture</returns>
        public FactureEnt GetFactureById(int factureId)
        {
            FactureEnt facture = Repository.FindById(factureId);
            SocieteEnt societe = GetSociete(facture);
            facture.ScanUrl = this.paramManager.GetScanShareUrl(societe.GroupeId);

            return facture;
        }

        /// <summary>
        /// Retourne la liste des factures AR
        /// </summary>
        /// <returns>Liste des favoris.</returns>
        public IEnumerable<FactureEnt> GetFactureARList()
        {
            return Repository.GetAllFactureAr();
        }

        /// <summary>
        /// Retourne la liste des factures d'une societe
        /// </summary>
        /// <param name="societeId">Id de la societe</param>
        /// <returns>Renvoie la liste des facture AR par id societe passé en parametre</returns>
        public IEnumerable<FactureEnt> GetFactureARList(int societeId)
        {
            return Repository.GetFactureArBySocieteId(societeId) ?? new FactureEnt[] { };
        }

        /// <summary>
        /// [TSA: 28/09/2017]
        /// Fonction temporaire. Les dates du filtre sont ramenés du front vers le back au format UTC. Hors les dates de l'objet FactureEnt sont
        /// issues de l'import AS400 au format local et donc sont insérées en date locale en BD. 
        /// Solutions : Avant d'effectuer l'insert des données dans FRED, convertir les dates au format UTC
        /// </summary>
        /// <param name="sfe">Filtre</param>
        private void SearchFactureToLocalDate(SearchFactureEnt sfe)
        {
            sfe.DateComptableFrom = (sfe.DateComptableFrom.HasValue) ? sfe.DateComptableFrom.Value.ToLocalTime() : default(DateTime?);
            sfe.DateComptableTo = (sfe.DateComptableTo.HasValue) ? sfe.DateComptableTo.Value.ToLocalTime() : default(DateTime?);
            sfe.DateEcheanceFrom = (sfe.DateEcheanceFrom.HasValue) ? sfe.DateEcheanceFrom.Value.ToLocalTime() : default(DateTime?);
            sfe.DateEcheanceTo = (sfe.DateEcheanceTo.HasValue) ? sfe.DateEcheanceTo.Value.ToLocalTime() : default(DateTime?);
            sfe.DateGestionFrom = (sfe.DateGestionFrom.HasValue) ? sfe.DateGestionFrom.Value.ToLocalTime() : default(DateTime?);
            sfe.DateGestionTo = (sfe.DateGestionTo.HasValue) ? sfe.DateGestionTo.Value.ToLocalTime() : default(DateTime?);
            sfe.DateFactureFrom = (sfe.DateFactureFrom.HasValue) ? sfe.DateFactureFrom.Value.ToLocalTime() : default(DateTime?);
            sfe.DateFactureTo = (sfe.DateFactureTo.HasValue) ? sfe.DateFactureTo.Value.ToLocalTime() : default(DateTime?);
        }

        /// <summary>
        /// Retourne la liste des factures filtrée, triée (avec pagination)
        /// </summary>
        /// <param name="filters">Objet de recherche et de tri des factures</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Retourne la liste des factures filtrées, triées et paginées</returns>
        public IEnumerable<FactureEnt> SearchFactureListWithFilterForRapprochement(SearchFactureEnt filters, int page, int pageSize)
        {
            // TSA : à enlever quand les dates importées AS400 seront insérées en UTC
            SearchFactureToLocalDate(filters);

            // Liste des CI associés à l'utilisateur courant
            List<int> userCiIdList = utilisateurManager.GetAllCIbyUser(utilisateurManager.GetContextUtilisateurId()).ToList();

            // Liste des CI de l'organisation parent (SOCIETE, PUO, UO, ETABLISSEMENT)
            List<int> ciIdListByOrgaParent = this.ciManager.GetCIList(filters.OrganisationId).Select(x => x.CiId).ToList();

            List<int> eligibleCiIdList = (filters.OrganisationId != 0) ? userCiIdList.Where(x => ciIdListByOrgaParent.Contains(x)).ToList() : userCiIdList;

            if (eligibleCiIdList.Count > 0)
            {
                IEnumerable<FactureEnt> factures = Repository.SearchFactureListWithFilter(eligibleCiIdList, filters, page, pageSize);
                foreach (FactureEnt f in factures.ToList())
                {
                    PopulateCalculatedFields(f);
                }
                return factures;
            }
            return null;
        }

        private void PopulateCalculatedFields(FactureEnt f)
        {
            // Ajout de la liste des Code-Libelle des CI d'une facture (Les lignes factures d'une facture peuvent avoir des CI différents)
            // Ajout du code CI ou Multi-CI à la facture
            f.CiCodeLibelles = new List<string>();
            List<CIEnt> allCi = f.ListLigneFacture.GroupBy(x => x.CI).Select(x => x.Key).ToList();

            if (allCi.Count > 0)
            {
                if (allCi.Count == 1)
                {
                    CIEnt ci = allCi.FirstOrDefault();
                    f.CICode = ci.Code.Trim();
                    f.CiCodeLibelles.Add(ci.CodeLibelle.Trim());
                }
                else
                {
                    f.CICode = "Multi-CI";
                    foreach (CIEnt ci in allCi)
                    {
                        if (ci != null)
                        {
                            f.CiCodeLibelles.Add(ci.CodeLibelle.Trim());
                        }
                    }
                }
            }
            else
            {
                f.CICode = "N/A";
            }
        }

        /// <summary>
        /// Retourne une facture par son numero
        /// </summary>
        /// <param name="noFacture">Numero de la facture</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Renvoie une facture par le numero passé en parametre</returns>
        public FactureEnt GetFactureARByNumero(string noFacture, int societeId)
        {
            return Repository.GetFactureArByNumero(noFacture, societeId);
        }

        /// <summary>
        /// Méthode d'enregistrement d'une facture
        /// </summary>
        /// <param name="factureAr"> La facture à enregistrer </param>
        /// <returns> Identifiant de la facture enregistrer </returns>
        public int Add(FactureEnt factureAr)
        {
            if (!IsValid(factureAr))
            {
                throw new OperationCanceledException("IMPORT FACTURE - Objet Invalide");
            }

            int resu;
            using (var tx = new TransactionScope())
            {
                resu = Repository.Add(factureAr);

                tx.Complete();
            }

            return resu;
        }

        /// <summary>
        /// Mise à jour d'une facture
        /// </summary>
        /// <param name="facture">La facture à mettre à jour</param>
        /// <returns>Le facture mise à jour</returns>
        public FactureEnt Update(FactureEnt facture)
        {
            facture.UtilisateurModificationId = utilisateurManager.GetContextUtilisateurId();
            facture.DateModification = DateTime.Now;

            Repository.UpdateFacture(facture);
            Save();

            return facture;
        }

        /// <summary>
        /// Vérificartion des règles de gestion avant enregistrement
        /// </summary>
        /// <param name="factureAr">Représente la facture à enregistrer</param>
        /// <returns>True : Facture valide pour enregistrement, False : Facture non valide pour enregistrement</returns>
        public bool IsValid(FactureEnt factureAr)
        {
            return IsUnique(factureAr) && IsExistJournal(factureAr);
        }

        /// <summary>
        /// RG_645_001 - Vérification de l'unicité de la facture pour une societe
        /// </summary>
        /// <param name="factureAr">Facture à vérifier</param>
        /// <returns>True : la facture n'existe pas, False : la facture existe</returns>
        public bool IsUnique(FactureEnt factureAr)
        {
            // Validation des parametres 
            if (factureAr == null)
            {
                WriteErrorLog("Argument null : factureAR", factureAr);
                return false;
            }

            StringBuilder messageErreur = new StringBuilder();

            if (string.IsNullOrEmpty(factureAr.NoFacture))
            {
                messageErreur.AppendLine("Argument attendu : factureAR.NoFacture");
            }

            if (factureAr.SocieteId <= 0)
            {
                messageErreur.AppendLine("Argument attendu : factureAR.SocieteId");
            }

            EtablissementComptableEnt etab = null;
            FactureEnt facture = null;
            if (factureAr.SocieteId.HasValue && factureAr.SocieteId.Value > 0)
            {
                facture = Repository.GetFactureArByNumero(factureAr.NoFacture, factureAr.SocieteId.Value);
            }
            else if (factureAr.EtablissementId.HasValue && factureAr.EtablissementId.Value > 0)
            {
                etab = this.etabComptableManager.GetEtablissementComptableById(factureAr.EtablissementId.Value);
                if (etab?.SocieteId.HasValue == true && etab.SocieteId.Value > 0)
                {
                    facture = Repository.GetFactureArByNumero(factureAr.NoFacture, etab.SocieteId.Value);
                }
            }

            if (facture != null)
            {
                messageErreur.AppendLine("Il n'est pas possible d'importer une facture existante");
            }

            if (messageErreur.Length > 0)
            {
                WriteErrorLog(messageErreur.ToString(), factureAr);
            }

            return messageErreur.Length == 0;
        }

        /// <summary>
        /// RG_645_003 - Vérifie que la facture fait partie d'un journal à importer pour la societe
        /// </summary>
        /// <param name="factureAR"> Facture à vérifier </param>
        /// <returns> True : la facture fait partie d'un journal, False : la facture ne fait pas partie d'un journal </returns>
        public bool IsExistJournal(FactureEnt factureAR)
        {
            bool resu = false;
            StringBuilder message = new StringBuilder();
            if (factureAR?.Journal != null)
            {
                resu = true;
            }
            else
            {
                message = CheckParameter(factureAR);
            }

            if (message.Length == 0 && factureAR != null)
            {
                JournalEnt journal = GetJournal(factureAR, message);

                if (journal?.JournalId > 0)
                {
                    resu = true;
                }
                else
                {
                    message.AppendLine("Argument attendu : journal");
                }
            }

            if (!resu)
            {
                WriteErrorLog(message.ToString(), factureAR);
            }
            return resu;
        }

        private JournalEnt GetJournal(FactureEnt factureAR, StringBuilder message)
        {
            JournalEnt journal = null;
            if ((!factureAR.SocieteId.HasValue || factureAR.SocieteId.HasValue && factureAR.SocieteId.Value <= 0) && factureAR.EtablissementId.HasValue && factureAR.EtablissementId.Value > 0)
            {
                EtablissementComptableEnt etab = etabComptableManager.GetEtablissementComptableById(factureAR.EtablissementId.Value);
                if (etab?.SocieteId.HasValue == true)
                {
                    journal = journalManager.GetJournal(etab.SocieteId.Value, factureAR.JournalId.Value);
                }
                else
                {
                    message.AppendLine("Argument attendu : factureAR.SocieteId || factureAR.EtablissementId");
                }
            }
            else
            {
                journal = journalManager.GetJournal(factureAR.SocieteId.Value, factureAR.JournalId.Value);
            }
            return journal;
        }

        private StringBuilder CheckParameter(FactureEnt factureAR)
        {
            StringBuilder message = new StringBuilder();
            // Validation des parametres 
            if (factureAR == null)
            {
                message.AppendLine("Argument null : factureAR");
            }
            else
            {
                if (factureAR.JournalId <= 0)
                {
                    message.AppendLine("Argument attendu : factureAR.JournalId");
                }
                if (!factureAR.SocieteId.HasValue || (factureAR.SocieteId.HasValue && factureAR.SocieteId.Value <= 0) &&
                    (!factureAR.EtablissementId.HasValue || (factureAR.EtablissementId.HasValue && factureAR.EtablissementId.Value <= 0)))
                {
                    message.AppendLine("Argument attendu : factureAR.SocieteId || factureAR.EtablissementId");
                }
            }
            return message;
        }

        private void WriteErrorLog(string message, FactureEnt factureAR)
        {
            Entities.LogImport.LogImportEnt log = repoLogImport.GetNew();
            log.TypeImport = TypeImport.FactureAR.ToString();
            log.DateImport = DateTime.Now;
            log.MessageErreur = message;
            log.Data = factureAR?.ToString();
            repoLogImport.Add(log);
        }

        /// <summary>
        /// Récupération de la societe à partir d'un objet facture AR
        /// </summary>
        /// <param name="factureAr">Représente une facture à rapprocher</param>
        /// <returns>Retourne la société de facturation</returns>
        public SocieteEnt GetSociete(FactureEnt factureAr)
        {
            SocieteEnt societe = null;

            //Récupération de la Societe
            if (factureAr.Societe?.SocieteId > 0)
            {
                societe = factureAr.Societe;
            }
            else if (factureAr.SocieteId > 0)
            {
                societe = societeManager.GetSocieteById(factureAr.SocieteId.Value);
            }
            else if (factureAr.Etablissement?.EtablissementComptableId > 0 && factureAr.Etablissement.Societe != null)
            {
                societe = factureAr.Etablissement.Societe;
            }
            else if (factureAr.EtablissementId.HasValue && factureAr.EtablissementId.Value > 0)
            {
                EtablissementComptableEnt etablissement = etabComptableManager.GetEtablissementComptableById(factureAr.EtablissementId.Value);
                if (etablissement?.Societe?.SocieteId > 0)
                {
                    societe = etablissement.Societe;
                }
                else if (etablissement?.SocieteId.HasValue == true && etablissement.SocieteId.Value > 0)
                {
                    societe = societeManager.GetSocieteById(etablissement.SocieteId.Value);
                }
            }
            return societe;
        }

        /// <summary>
        /// Vérifie la validité et enregistre les facture importés depuis ANAËL Finances
        /// </summary>
        /// <param name="factureEnt">Liste des entités dont il faut vérifier la validité</param>
        /// <returns>Retourne vrai si la facture peut être importé</returns>
        public bool ManageImportedFacture(FactureEnt factureEnt)
        {
            bool success = IsValid(factureEnt);

            if (success)
            {
                try
                {
                    Add(factureEnt);
                }
                catch (Exception)
                {
                    success = false;
                }
            }
            return success;
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe de recherche des factures
        /// </summary>
        /// <returns>Objet de filtrage + tri des factures initialisé</returns>
        public SearchFactureEnt GetFilter()
        {
            return new SearchFactureEnt
            {
                ValueText = string.Empty,
                NoFMFI = true,
                NoFacture = true,
                JournalCode = true,
                Folio = true,
                CompteGeneral = true,
                FournisseurCode = true,
                FournisseurLibelle = true,
                CI = true,
                NoFactureFournisseur = true,
                SocieteCode = true,
                EtablissementCode = true,
                SeulementMesFactures = true,
                AfficherFacturesRapprochees = true,
                AfficherFacturesCachees = true,
                FolioUtilisateurCourant = this.utilisateurManager.GetContextUtilisateur().Folio,

                DateComptableFrom = null,
                DateComptableTo = null,
                DateGestionFrom = null,
                DateGestionTo = null,
                DateFactureFrom = null,
                DateFactureTo = null,
                DateEcheanceFrom = null,
                DateEcheanceTo = null,

                DateComptableAsc = true // Initialisation business : tri sur Date
            };
        }

        /// <summary>
        /// Insertion en base d'une ligne de facture AR
        /// </summary>
        /// <param name="factureLigne">La ligne de facture AR à enregistrer</param>
        /// <returns>Retourne l'identifiant unique de la ligne de facture AR</returns>
        public int AddLigne(FactureLigneEnt factureLigne)
        {
            //Vérification des paramètres
            if (factureLigne == null)
            {
                throw new ArgumentNullException("factureLigne");
            }
            if (!factureLigne.FactureId.HasValue || factureLigne.FactureId.HasValue && factureLigne.FactureId.Value <= 0)
            {
                throw new ArgumentNullException("factureLigne.FactureId");
            }
            return Repository.AddLigne(factureLigne);
        }

        /// <summary>
        /// Retourne une nouvelle liste de factures AR
        /// </summary>
        /// <returns>Liste des favoris.</returns>
        public IEnumerable<FactureLigneEnt> GetNewLigneFactureARList()
        {
            return new FactureLigneEnt[] { };
        }
    }
}
