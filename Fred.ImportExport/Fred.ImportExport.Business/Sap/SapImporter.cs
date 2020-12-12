using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.Depense;
using Fred.Business.Facturation;
using Fred.Business.Import;
using Fred.Business.Reception;
using Fred.Business.Referential;
using Fred.Business.Referential.Nature;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Import;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.ImportExport.Framework.Exceptions;
using NLog;

namespace Fred.ImportExport.Business.Sap
{
    /// <summary>
    /// Permet d'importer des éléments de SAP.
    /// </summary>
    /// <typeparam name="TModel">Le modèle de donnée à importer.</typeparam>
    public abstract class SapImporter<TModel>
    {
        #region Membres

        private readonly Lazy<IUtilisateurManager> utilisateurMgr = new Lazy<IUtilisateurManager>(() => ServiceLocator.Current.GetInstance<IUtilisateurManager>());

        private readonly Lazy<ISocieteManager> societeMgr = new Lazy<ISocieteManager>(() => ServiceLocator.Current.GetInstance<ISocieteManager>());

        private readonly Lazy<ICIManager> ciMgr = new Lazy<ICIManager>(() => ServiceLocator.Current.GetInstance<ICIManager>());

        private readonly Lazy<IFournisseurManager> fournisseurMgr = new Lazy<IFournisseurManager>(() => ServiceLocator.Current.GetInstance<IFournisseurManager>());

        private readonly Lazy<ISystemeImportManager> systemeImportMgr = new Lazy<ISystemeImportManager>(() => ServiceLocator.Current.GetInstance<ISystemeImportManager>());

        private readonly Lazy<ISystemeExterneManager> systemeExterneMgr = new Lazy<ISystemeExterneManager>(() => ServiceLocator.Current.GetInstance<ISystemeExterneManager>());

        private readonly Lazy<ITranscoImportManager> transcoImportMgr = new Lazy<ITranscoImportManager>(() => ServiceLocator.Current.GetInstance<ITranscoImportManager>());

        private readonly Lazy<IReferentielFixeManager> referentielFixeMgr = new Lazy<IReferentielFixeManager>(() => ServiceLocator.Current.GetInstance<IReferentielFixeManager>());

        private readonly Lazy<IUniteManager> uniteMgr = new Lazy<IUniteManager>(() => ServiceLocator.Current.GetInstance<IUniteManager>());

        private readonly Lazy<IDeviseManager> deviseMgr = new Lazy<IDeviseManager>(() => ServiceLocator.Current.GetInstance<IDeviseManager>());

        private readonly Lazy<IPaysManager> paysMgr = new Lazy<IPaysManager>(() => ServiceLocator.Current.GetInstance<IPaysManager>());

        private readonly Lazy<INatureManager> natureMgr = new Lazy<INatureManager>(() => ServiceLocator.Current.GetInstance<INatureManager>());

        private readonly Lazy<IFacturationManager> facturationMgr = new Lazy<IFacturationManager>(() => ServiceLocator.Current.GetInstance<IFacturationManager>());

        private readonly Lazy<ICommandeManager> commandeMgr = new Lazy<ICommandeManager>(() => ServiceLocator.Current.GetInstance<ICommandeManager>());

        private readonly Lazy<ITacheManager> tacheMgr = new Lazy<ITacheManager>(() => ServiceLocator.Current.GetInstance<ITacheManager>());

        private readonly Lazy<IReceptionManager> receptionMgr = new Lazy<IReceptionManager>(() => ServiceLocator.Current.GetInstance<IReceptionManager>());

        private readonly Lazy<IRemplacementTacheManager> remplacementTacheMgr = new Lazy<IRemplacementTacheManager>(() => ServiceLocator.Current.GetInstance<IRemplacementTacheManager>());
        #endregion
        #region Constructeur

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="contexte">Le contexte du flux.</param>
        /// <param name="code">Le code du flux.</param>
        /// <param name="libelle">Le libellé du flux.</param>
        protected SapImporter(string contexte, string code, string libelle)
        {
            Contexte = contexte;
            Code = code;
            Libelle = libelle;
            Logger = LogManager.GetCurrentClassLogger();
            BaseMessageErreur = $"[{Contexte}][{Code}] {Libelle} : Erreur intégration : ";
        }

        #endregion
        #region Propriétés

        /// <summary>
        /// Le contexte du flux.
        /// </summary>
        public string Contexte { get; }

        /// <summary>
        /// Le code du flux.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Le libellé du flux.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Le logger.
        /// </summary>
        public Logger Logger { get; }

        /// <summary>
        ///   Message d'erreur de base
        /// </summary>
        public string BaseMessageErreur { get; }

        #endregion
        #region Fonctions

        /// <summary>
        /// Importe.
        /// </summary>
        /// <param name="model">Le modèle à importer.</param>
        public void Import(TModel model)
        {
            try
            {
                ImportModel(model);
                Logger.Info($"[{Contexte}][{Code}] {Libelle} : Succès intégration");
            }
            catch (FredIeBusinessException ex)
            {
                // On re-créé l'exception FredIeBusiness pour modifier le message d'erreur (pas ouf mais solution rapide)
                // a faire : Mettre en place le système de log d'AJO lorsque la dev sera en prod !
                throw new FredIeBusinessException(BaseMessageErreur + ex.Message);
            }
            catch (Exception ex)
            {
                throw new FredIeBusinessException(BaseMessageErreur + ex.Message, ex);
            }
        }

        /// <summary>
        /// Importe.
        /// </summary>
        /// <param name="model">Le modèle à importer.</param>
        protected abstract void ImportModel(TModel model);

        /// <summary>
        /// Retourne l'utilisateur FRED IE.
        /// </summary>
        /// <returns>L'utilisateur FRED IE.</returns>
        public UtilisateurEnt GetFredIeUtilisateur()
        {
            return utilisateurMgr.Value.GetByLogin("fred_ie");
        }

        /// <summary>
        /// Retourne une société.
        /// </summary>
        /// <param name="societeComptableCode">Le code comptable de la société.</param>
        /// <returns>La société.</returns>
        public SocieteEnt GetSociete(string societeComptableCode)
        {
            var societe = societeMgr.Value.GetSocieteByCodeSocieteComptable(societeComptableCode);
            if (societe == null)
            {
                throw new FredIeBusinessException($"'Société Comptable Code' \"{societeComptableCode}\" non reconnu dans FRED");
            }
            return societe;
        }

        /// <summary>
        /// Retourne un CI.
        /// </summary>
        /// <param name="ciCode">Le code du CI.</param>
        /// <param name="societeComptableCode">Le code comptable de la société.</param>
        /// <returns>Le CI.</returns>
        public CIEnt GetCI(string ciCode, string societeComptableCode)
        {
            var ci = ciMgr.Value.GetCI(ciCode, societeComptableCode);
            if (ci == null)
            {
                throw new FredIeBusinessException($"'CI Code' \"{ciCode}\" non reconnu dans FRED");
            }
            return ci;
        }

        /// <summary>
        /// Retourne un fournisseur.
        /// </summary>
        /// <param name="fournisseurCode">Le code du fournisseur.</param>
        /// <param name="societe">La société.</param>
        /// <returns>Le fournisseur.</returns>
        public FournisseurEnt GetFournisseur(string fournisseurCode, SocieteEnt societe)
        {
            var fournisseur = fournisseurMgr.Value.GetFournisseurList().FirstOrDefault(x => x.Code == fournisseurCode && x.GroupeId == societe.GroupeId);
            if (fournisseur == null)
            {
                throw new FredIeBusinessException($"'Fournisseur Code' \"{fournisseurCode}\" non reconnu dans FRED");
            }
            return fournisseur;
        }

        public int GetAgence(string agenceCode, int groupeId)
        {
            var agenceId = fournisseurMgr.Value.GetAgenceIdByCodeAndGroupe(agenceCode, groupeId);

            if (!agenceId.HasValue)
            {
                throw new FredIeBusinessException($"'Agence Code' \"{agenceCode}\" non reconnu dans FRED");
            }

            return agenceId.Value;
        }

        /// <summary>
        /// Retourne un système d'import.
        /// </summary>
        /// <param name="systemImportCode">Le code du système d'import.</param>
        /// <returns>Le système d'import.</returns>
        public SystemeImportEnt GetSystemImport(string systemImportCode)
        {
            var systemeImport = systemeImportMgr.Value.GetSystemeImport(systemImportCode);
            if (systemeImport == null)
            {
                throw new FredIeBusinessException($"System d'import non trouvé pour {systemImportCode}");
            }
            return systemeImport;
        }

        /// <summary>
        /// Retourne un système externe.
        /// </summary>
        /// <param name="systemeExterneCode">Le code du système externe.</param>
        /// <returns>Le système externe.</returns>
        public SystemeExterneEnt GetSystemeExterne(string systemeExterneCode)
        {
            var systemeExterne = systemeExterneMgr.Value.GetSystemeExterne(systemeExterneCode);
            if (systemeExterne == null)
            {
                throw new FredIeBusinessException($"System externe non trouvé pour {systemeExterneCode}");
            }
            return systemeExterne;
        }

        /// <summary>
        /// Retourne un transocodage.
        /// </summary>
        /// <param name="codeExterne">Le code externe.</param>
        /// <param name="societe">La société.</param>
        /// <param name="systemeImport">Le système d'import.</param>
        /// <returns>Le transocodage.</returns>
        public TranscoImportEnt GetTranscoImport(string codeExterne, SocieteEnt societe, SystemeImportEnt systemeImport)
        {
            var transcoImport = transcoImportMgr.Value.GetTranscoImport(codeExterne, societe.SocieteId, systemeImport.SystemeImportId);
            if (transcoImport == null)
            {
                throw new FredIeBusinessException($"aucun Code Interne FRED trouvé pour le 'Code Externe' \"{codeExterne}\"");
            }
            return transcoImport;
        }

        /// <summary>
        /// Retourne une ressource.
        /// </summary>
        /// <param name="transcoImport">Le transcodage.</param>
        /// <param name="societe">La société.</param>
        /// <returns>La ressource.</returns>
        public RessourceEnt GetRessource(TranscoImportEnt transcoImport, SocieteEnt societe)
        {
            var ressources = referentielFixeMgr.Value.GetRessources(transcoImport.CodeInterne, societe.GroupeId).ToList();
            if (ressources.Count == 0)
            {
                throw new FredIeBusinessException($"aucun ID Ressource FRED trouvé pour le 'Code Ressource' \"{transcoImport.CodeInterne}\" et le 'Société Comptable Code' \"{societe.CodeSocieteComptable}\"");
            }
            else if (ressources.Count > 1)
            {
                throw new FredIeBusinessException($"plusieurs ID Ressource FRED trouvés pour le 'Code Ressource' \"{transcoImport.CodeInterne}\" et le 'Société Comptable Code' \"{societe.CodeSocieteComptable}\"");
            }
            return ressources[0];
        }

        /// <summary>
        /// Retourne un référentiel étendu.
        /// </summary>
        /// <param name="ressource">La ressource.</param>
        /// <param name="nature">La nature.</param>
        /// <param name="societe">La société.</param>
        /// <returns>Le référentiel étendu.</returns>
        public ReferentielEtenduEnt GetReferentielEtendu(RessourceEnt ressource, NatureEnt nature, SocieteEnt societe)
        {
            var referentielEtendu = ressource.ReferentielEtendus.Where(re => re.SocieteId == societe.SocieteId && re.NatureId == nature.NatureId);
            var count = referentielEtendu.Count();
            if (count == 0)
            {
                throw new FredIeBusinessException($"'l'ID Ressource FRED' \"{ressource.RessourceId}\" ne correspond pas au 'Nature Code' \"{nature.Code}\" et 'Societe Code' \"{societe.Code}\"");
            }
            else if (count > 1)
            {
                throw new FredIeBusinessException($"'l'ID Ressource FRED' \"{ressource.RessourceId}\" correspond à plusieurs 'Nature Code' \"{nature.Code}\" et 'Societe Code' \"{societe.Code}\"");
            }
            return referentielEtendu.First();
        }

        /// <summary>
        /// Retourne une unité.
        /// </summary>
        /// <param name="uniteCode">Le code de l'unité.</param>
        /// <returns>L'unité.</returns>
        public UniteEnt GetUnite(string uniteCode)
        {
            var unite = uniteMgr.Value.GetUnite(uniteCode);
            if (unite == null)
            {
                throw new FredIeBusinessException($"'Unite Code' \"{uniteCode}\" non reconnu dans FRED");
            }
            return unite;
        }

        /// <summary>
        /// Retourne une devise.
        /// </summary>
        /// <param name="deviseIsoCode">Le code ISO de la devise.</param>
        /// <returns>La devise.</returns>
        public DeviseEnt GetDevise(string deviseIsoCode)
        {
            var devise = deviseMgr.Value.GetDevise(deviseIsoCode);
            if (devise == null)
            {
                throw new FredIeBusinessException($"'Devise Iso Code' \"{deviseIsoCode}\" non reconnu dans FRED");
            }
            return devise;
        }

        /// <summary>
        /// Retourne un pays.
        /// </summary>
        /// <param name="paysCode">Le code du pays.</param>
        /// <returns>Le pays.</returns>
        public PaysEnt GetPays(string paysCode)
        {
            var pays = paysMgr.Value.GetByCode(paysCode);
            if (pays == null)
            {
                throw new FredIeBusinessException($"'Pays Code' \"{paysCode}\" non reconnu dans FRED");
            }
            return pays;
        }

        /// <summary>
        /// Retourne une nature.
        /// </summary>
        /// <param name="natureCode">Le code de la nature.</param>
        /// <param name="societe">La société.</param>
        /// <returns>La nature.</returns>
        public NatureEnt GetNature(string natureCode, SocieteEnt societe)
        {
            var nature = natureMgr.Value.GetNature(natureCode, societe.CodeSocieteComptable);
            if (nature == null)
            {
                throw new FredIeBusinessException($"'Nature Code' \"{natureCode}\" pour la société {societe.CodeSocieteComptable} non reconnu dans FRED");
            }
            return nature;
        }

        /// <summary>
        /// Vérifie qu'une devise est autorisée pour un CI.
        /// </summary>
        /// <param name="ci">Le CI concerné.</param>
        /// <param name="devise">La devise concernée.</param>
        public void CheckDeviseCI(CIEnt ci, DeviseEnt devise)
        {
            if (!ciMgr.Value.GetCIDevise(ci.CiId).Any(d => d.DeviseId == devise.DeviseId))
            {
                throw new FredIeBusinessException($"'Devise Iso Code' \"{devise.IsoCode}\" non autorisée dans FRED pour le CI  \"{ci.Code}\"");
            }
        }

        public IEnumerable<FacturationEnt> GetFacturations(string numFactureSap)
        {
            List<FacturationEnt> facturations = facturationMgr.Value.GetList(numFactureSap).ToList();

            // RG_3656_108
            if (facturations?.Any() == true)
            {
                throw new FredIeBusinessException($"'Numero Facture SAP' \"{numFactureSap}\" déjà existant dans FRED");
            }
            return facturations;
        }

        public IEnumerable<FacturationEnt> GetFacturationsByReceptionID(int receptionId, decimal montantHt, DateTime dateSaisie)
        {
            List<FacturationEnt> facturations = facturationMgr.Value.GetListByReceptionID(receptionId, montantHt, dateSaisie).ToList();

            // RG_3656_111
            if (facturations?.Any() == true)
            {
                throw new FredIeBusinessException($"mouvement FAR associé à la 'RéceptionID' \"{receptionId}\" déjà existant dans FRED");
            }
            return facturations;
        }

        public CommandeEnt GetCommande(string commandeNumero)
        {
            CommandeEnt commande = commandeMgr.Value.GetCommande(commandeNumero);
            if (commande == null)
            {
                throw new FredIeBusinessException($"'Commande Numero' \"{commandeNumero}\" non reconnu dans FRED");
            }
            return commande;
        }

        public CommandeEnt GetCommandeByNumberOrExternalNumber(string commandeNumero)
        {
            CommandeEnt commande = commandeMgr.Value.GetCommandeByNumberOrExternalNumber(commandeNumero);
            if (commande == null)
            {
                throw new FredIeBusinessException($"'Commande Numero' \"{commandeNumero}\" non reconnu dans FRED");
            }
            return commande;
        }

        public TacheEnt GetTacheLitigeParDefaut(CIEnt ci, SocieteEnt societe)
        {
            //RG_3656_033
            TacheEnt tache = tacheMgr.Value.GetTacheLitigeByCiId(ci.CiId) ?? tacheMgr.Value.GetTacheParDefaut(ci.CiId);

            if (tache == null)
            {
                throw new FredIeBusinessException($"Aucune Tâche par défaut ou litige FRED trouvée pour le 'CI Code' = {ci.Code} et le 'Societe Code' = {societe.CodeSocieteComptable} fournis.");
            }
            return tache;
        }

        public DepenseAchatEnt GetReception(int receptionId, DateTime dateComptable)
        {
            DepenseAchatEnt reception = receptionMgr.Value.GetReception(receptionId);
            if (reception == null)
            {
                throw new FredIeBusinessException($"'Reception ID' \"{receptionId}\" non reconnu dans FRED");
            }
            else if (reception.GroupeRemplacementTacheId > 0)
            {
                RemplacementTacheEnt rt = remplacementTacheMgr.Value.GetLast(reception.GroupeRemplacementTacheId.Value);

                if (rt != null)
                {
                    if (rt.DateComptableRemplacement.Value.Month > dateComptable.Month && rt.DateComptableRemplacement.Value.Year > dateComptable.Year)
                    {
                        throw new FredIeBusinessException("Opération antérieure à un remplacement de tâche de la Réception dans FRED.");
                    }
                    else
                    {
                        reception.TacheId = rt.TacheId;
                        reception.Tache = rt.Tache;
                        rt.Annulable = false;
                        remplacementTacheMgr.Value.Update(rt);
                    }
                }
            }

            return reception;
        }
        #endregion
    }
}
