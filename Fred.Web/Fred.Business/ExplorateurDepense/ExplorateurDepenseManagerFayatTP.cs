using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense;
using Fred.Business.Facturation;
using Fred.Business.FeatureFlipping;
using Fred.Business.OperationDiverse;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Referential.Nature;
using Fred.Business.ReferentielEtendu;
using Fred.Business.Unite;
using Fred.Business.Valorisation;
using Fred.Entities.Commande;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.Valorisation;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;

namespace Fred.Business.ExplorateurDepense
{
    public class ExplorateurDepenseManagerFayatTP : ExplorateurDepenseManager, IExplorateurDepenseManagerFayatTP
    {
        private readonly ICommandeManager commandeManager;
        private readonly INatureManager natureManager;
        private readonly IPointageManager pointageManager;
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IDepenseManager depenseManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IReferentielEtenduManager referentielEtenduManager;
        private readonly IUniteManager uniteManager;
        private readonly IFeatureFlippingManager featureFlippingManager;

        public ExplorateurDepenseManagerFayatTP(ICIManager cIManager,
                                                IDatesClotureComptableManager datesClotureComptableManager,
                                                ICommandeManager commandeManager,
                                                INatureManager natureManager,
                                                IPointageManager pointageManager,
                                                IOperationDiverseManager operationDiverseManager,
                                                IDepenseManager depenseManager,
                                                IValorisationManager valorisationManager,
                                                IReferentielEtenduManager referentielEtenduManager,
                                                IDepenseAchatService depenseAchatService,
                                                IRemplacementTacheManager remplacementTacheManager,
                                                IFeatureFlippingManager featureFlippingManager,
                                                IFacturationManager facturationManager,
                                                IUniteManager uniteManager) : base(cIManager,
                                                                                                  datesClotureComptableManager,
                                                                                                  operationDiverseManager,
                                                                                                  depenseManager,
                                                                                                  valorisationManager,
                                                                                                  depenseAchatService,
                                                                                                  remplacementTacheManager,
                                                                                                  featureFlippingManager,
                                                                                                  facturationManager,
                                                                                                  uniteManager)
        {
            this.commandeManager = commandeManager;
            this.natureManager = natureManager;
            this.pointageManager = pointageManager;
            this.operationDiverseManager = operationDiverseManager;
            this.depenseManager = depenseManager;
            this.valorisationManager = valorisationManager;
            this.referentielEtenduManager = referentielEtenduManager;
            this.featureFlippingManager = featureFlippingManager;
        }

        /// <summary>
        /// Récupération des dépenses selon deux axes
        /// </summary>
        /// <param name="filtre">Filtre permettant de récupérer les dépenses choisies</param>    
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des dépenses + autres données</returns>
        public override async Task<ExplorateurDepenseResult> GetDepensesAsync(SearchExplorateurDepense filtre, int? page, int? pageSize)
        {
            IEnumerable<ExplorateurDepenseFayatTPModel> depenses = await GetAllDepensesAsync(filtre).ConfigureAwait(false);
            ExplorateurDepenseResult result = new ExplorateurDepenseResult();
            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();

            List<ExplorateurDepenseGeneriqueModel> deps = HandleDepenses(filtre, ConvertToExplorateurDepenseGeneriqueList(depenses.ToList()), result, explorateurDepenseHelper);

            IEnumerable<ExplorateurDepenseGeneriqueModel> ensemble = ApplyTri(filtre, deps);

            // Pagination
            if (page.HasValue && pageSize.HasValue)
            {
                ensemble = ensemble.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            result.Depenses = ensemble.ToList();

            return result;
        }

        /// <summary>
        /// Récupération du tableau de byte du fichier excel
        /// </summary>
        /// <param name="filtre">Filtre explorateur dépense</param>
        /// <returns>Tableau byte excel</returns>
        public override async Task<byte[]> GetExplorateurDepensesExcelAsync(SearchExplorateurDepense filtre)
        {
            ExplorateurDepenseResult depenses = await GetDepensesForExportAsync(filtre).ConfigureAwait(false);
            return ExplorateurDepenseExport.ToExcel(depenses.Depenses);
        }

        /// <summary>
        /// Récupération des dépenses selon deux axes pour un export
        /// </summary>
        /// <param name="filtre">Filtre permettant de récupérer les dépenses choisies</param>    
        /// <returns>Liste des dépenses + autres données</returns>
        public override async Task<ExplorateurDepenseResult> GetDepensesForExportAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseFayatTPModel> depenses = await GetAllDepenseForExportAsync(filtre).ConfigureAwait(false);
            List<ExplorateurDepenseGeneriqueModel> deps = new List<ExplorateurDepenseGeneriqueModel>();
            ExplorateurDepenseResult result = new ExplorateurDepenseResult();
            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();

            deps = HandleDepenses(filtre, ConvertToExplorateurDepenseGeneriqueList(depenses.ToList()), result, explorateurDepenseHelper);

            IEnumerable<ExplorateurDepenseGeneriqueModel> ensemble = deps;

            // RG_3691_022
            deps = new List<ExplorateurDepenseGeneriqueModel>();
            ensemble.ForEach(x => ProcessTransfertTache(deps, x));

            ensemble = ApplyTri(filtre, deps);

            result.Depenses = ensemble.ToList();

            return result;
        }

        /// <summary>
        /// Retourne la liste des dépenses
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseFayatTPModel"/>ExplorateurDepenseFayatTP</returns>
        public new async Task<IEnumerable<ExplorateurDepenseFayatTPModel>> GetAllDepensesAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> expDepAchats, expDepValos, expDepOds;
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filtre);

            IEnumerable<DepenseAchatEnt> depenses = await depenseManager.GetDepenseListAsync(filtre.CiId).ConfigureAwait(false);
            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filtre, depenses);
            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filtre).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();
            expDepAchats = explorateurDepenseHelper.ConvertDepenseAchats(depAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList());
            expDepValos = explorateurDepenseHelper.ConvertValorisation(valos.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, dateClotureComptableList);
            expDepValos = SetUniteForValorisationMontantGreaterThanZero(expDepValos);

            bool useOdLibelleCourt = featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000);
            expDepOds = explorateurDepenseHelper.ConvertOperationDiverse(ods.ToList(), useOdLibelleCourt, dateClotureComptableList);

            // RG_8073_008
            PopulateOperationDiverseForFayatTP(expDepOds, ods.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, explorateurDepenseHelper);

            expDepAchats = ApplyFilterForMontants(filtre, expDepAchats);

            // Fusion des trois listes de dépenses différentes 
            List<ExplorateurDepenseGeneriqueModel> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();
            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filtre.PeriodeFin.Value).ConfigureAwait(false);

            IEnumerable<ExplorateurDepenseGeneriqueModel> fullDepenseListFiltered = fullDepenseList.Where(filtre.GetPredicateWhere().Compile());
            return ConvertToExplorateurFayatTPList(fullDepenseListFiltered);
        }

        /// <summary>
        /// Retourne la liste des dépenses pour un export
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseFayatTPModel"/>ExplorateurDepenseFayatTP</returns>
        public new async Task<IEnumerable<ExplorateurDepenseFayatTPModel>> GetAllDepenseForExportAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> expDepAchats, expDepValos, expDepOds;
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filtre);

            IEnumerable<DepenseAchatEnt> depenses = await depenseManager.GetDepenseListAsync(filtre.CiId).ConfigureAwait(false);
            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filtre, depenses);
            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filtre).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            await SetRemplacementTachesAsync(depAchats, ods, valos).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();
            expDepAchats = explorateurDepenseHelper.ConvertForExportDepenseAchat(depAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList());
            expDepValos = explorateurDepenseHelper.ConvertForExportValorisation(valos.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, dateClotureComptableList);
            expDepValos = SetUniteForValorisationMontantGreaterThanZero(expDepValos);

            bool useOdLibelleCourt = featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000);
            expDepOds = explorateurDepenseHelper.ConvertForExportOperationDiverse(ods.ToList(), useOdLibelleCourt, dateClotureComptableList);

            // RG_8073_008
            PopulateOperationDiverseForFayatTP(expDepOds, ods.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, explorateurDepenseHelper);

            expDepAchats = ApplyFilterForMontants(filtre, expDepAchats);

            // Fusion des trois listes de dépenses différentes 
            List<ExplorateurDepenseGeneriqueModel> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();
            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filtre.PeriodeFin.Value).ConfigureAwait(false);

            IEnumerable<ExplorateurDepenseGeneriqueModel> fullDepenseListFiltered = fullDepenseList.Where(filtre.GetPredicateWhere().Compile());
            return ConvertToExplorateurFayatTPList(fullDepenseListFiltered);
        }

        /// <summary>
        /// Retourne la liste des dépenses pour un export avec tache et ressources
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseFayatTPModel"/>ExplorateurDepenseFayatTP</returns>
        public async new Task<IEnumerable<ExplorateurDepenseFayatTPModel>> GetAllDepenseForExportWithTacheAndRessourceAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> expDepAchats, expDepValos, expDepOds;
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filtre);

            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filtre, await depenseManager.GetDepensesListWithMinimumIncludesAsync(filtre.CiId).ConfigureAwait(false));
            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filtre).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            await SetRemplacementTachesAsync(depAchats, ods, valos).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();
            expDepAchats = explorateurDepenseHelper.ConvertForExportDepenseAchat(depAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList());
            expDepValos = explorateurDepenseHelper.ConvertForExportValorisation(valos.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, dateClotureComptableList);

            bool useOdLibelleCourt = featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000);
            expDepOds = explorateurDepenseHelper.ConvertForExportOperationDiverse(ods.ToList(), useOdLibelleCourt, dateClotureComptableList);

            // RG_8073_008
            PopulateOperationDiverseForFayatTP(expDepOds, ods.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, explorateurDepenseHelper);

            expDepAchats = ApplyFilterForMontants(filtre, expDepAchats);

            // Fusion des trois listes de dépenses différentes
            List<ExplorateurDepenseGeneriqueModel> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();
            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filtre.PeriodeFin.Value).ConfigureAwait(false);

            IEnumerable<ExplorateurDepenseGeneriqueModel> fullDepenseListFiltered = fullDepenseList.Where(filtre.GetPredicateWhere().Compile());
            return ConvertToExplorateurFayatTPList(fullDepenseListFiltered);
        }

        /// <summary>
        /// Retourne la liste des dépenses pour avec tache et ressources
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseFayatTPModel"/>ExplorateurDepenseFayatTP</returns>
        public async new Task<IEnumerable<ExplorateurDepenseFayatTPModel>> GetAllDepenseWithTacheAndRessourceAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> expDepAchats, expDepValos, expDepOds;
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filtre);

            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filtre, await depenseManager.GetDepensesListWithMinimumIncludesAsync(filtre.CiId).ConfigureAwait(false));
            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filtre).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();
            expDepAchats = explorateurDepenseHelper.ConvertDepenseAchats(depAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList());
            expDepValos = explorateurDepenseHelper.ConvertValorisation(valos.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, dateClotureComptableList);

            bool useOdLibelleCourt = featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000);
            expDepOds = explorateurDepenseHelper.ConvertOperationDiverse(ods.ToList(), useOdLibelleCourt, dateClotureComptableList);

            // RG_8073_008
            PopulateOperationDiverseForFayatTP(expDepOds, ods.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, explorateurDepenseHelper);

            expDepAchats = ApplyFilterForMontants(filtre, expDepAchats);

            // Fusion des trois listes de dépenses différentes 
            List<ExplorateurDepenseGeneriqueModel> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();
            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filtre.PeriodeFin.Value).ConfigureAwait(false);

            IEnumerable<ExplorateurDepenseGeneriqueModel> fullDepenseListFiltered = fullDepenseList.Where(filtre.GetPredicateWhere().Compile());
            return ConvertToExplorateurFayatTPList(fullDepenseListFiltered);
        }

        private void PopulateOperationDiverseForFayatTP(IEnumerable<ExplorateurDepenseGeneriqueModel> expDepOds, List<OperationDiverseEnt> ods, DateTime? periodeDebut, DateTime? periodeFin, ExplorateurDepenseHelper explorateurDepenseHelper)
        {
            NatureEnt odNature = null;
            OperationDiverseEnt od = null;
            ReferentielEtenduEnt referentielEtendu = null;

            foreach (ExplorateurDepenseGeneriqueModel expDep in expDepOds)
            {
                od = ods.Find(o => o.OperationDiverseId == expDep.DepenseId);
                odNature = od.EcritureComptable?.Nature;
                // Si on a pas d'EcritureComptable sur l'OD, on prend la Nature liée à la Ressource+Societe de cet OD, via le Référentiel Etendu
                if (od.EcritureComptable == null)
                {
                    referentielEtendu = referentielEtenduManager.GetByRessourceIdAndSocieteId(od.RessourceId, od.CI.SocieteId.Value);
                    odNature = natureManager.GetNatureById(referentielEtendu.NatureId.Value);
                }

                if (od.RapportLigneId.HasValue)
                {
                    PopulateOperationDiverseLikeValorisationForFayatTP(periodeDebut, periodeFin, explorateurDepenseHelper, expDep, od, odNature);
                }
                else if (od.CommandeId.HasValue)
                {
                    PopulateOperationDiverseLikeReceptionForFayatTP(expDep, od, odNature);
                }
                else if (!od.CommandeId.HasValue && !od.RapportLigneId.HasValue)
                {
                    // Pour les OD sans numéro de ligne de pointage et de commande : alimenter seulement la nature analytique avec "Code nature - Libellé nature"
                    PopulateNatureForFayatTP(expDep, odNature);
                }
            }
        }

        private void PopulateOperationDiverseLikeReceptionForFayatTP(ExplorateurDepenseGeneriqueModel expDep, OperationDiverseEnt od, NatureEnt odNature)
        {
            // Pour les OD liées à un numéro de commande : alimenter les champs en prenant les mêmes règles que celles de types "Réception"
            CommandeEnt odCommande = commandeManager.GetCommandeById(od.CommandeId.Value);

            expDep.Code = odCommande?.Numero;
            expDep.Libelle1 = odCommande?.Fournisseur?.Libelle;
            expDep.Libelle2 = od.Libelle;

            PopulateNatureForFayatTP(expDep, odNature);
        }

        private void PopulateOperationDiverseLikeValorisationForFayatTP(DateTime? periodeDebut, DateTime? periodeFin, ExplorateurDepenseHelper explorateurDepenseHelper, ExplorateurDepenseGeneriqueModel expDep, OperationDiverseEnt od, NatureEnt odNature)
        {
            List<Expression<Func<RapportLigneEnt, object>>> includes = new List<Expression<Func<RapportLigneEnt, object>>> { x => x.Personnel.ContratInterimaires, x => x.Materiel, x => x.Personnel.Societe };
            RapportLigneEnt odRapportLigne = pointageManager.Get(od.RapportLigneId.Value, includes);

            // Pour les OD liées à un numéro de ligne de pointage : alimenter les champs en prenant les mêmes règles que les lignes de types "Valorisation"
            expDep.Code = odRapportLigne.PersonnelId.HasValue ? odRapportLigne.Personnel.Matricule : odRapportLigne.Materiel.Code;
            expDep.Libelle1 = odRapportLigne.PersonnelId.HasValue ? odRapportLigne.Personnel.PrenomNom : odRapportLigne.Materiel.Libelle;
            expDep.Libelle2 = string.Concat(odRapportLigne.PersonnelId.HasValue ? odRapportLigne.Personnel.Societe.Code : odRapportLigne.Materiel.Societe.Code, " - ", ExplorateurDepenseResources.Rapport, " #", odRapportLigne.RapportId);

            PopulateNatureForFayatTP(expDep, odNature);

            // Gestion de ces champs dans le cas d'un Intérimaire-
            if (odRapportLigne.Personnel?.IsInterimaire == true)
            {
                SetExplorateurDepenseForInterim(expDep, odRapportLigne, periodeDebut, periodeFin, explorateurDepenseHelper);
            }
        }

        private void SetExplorateurDepenseForInterim(ExplorateurDepenseGeneriqueModel expDep, RapportLigneEnt rapportLigne, DateTime? periodeDebut, DateTime? periodeFin, ExplorateurDepenseHelper explorateurDepenseHelper)
        {
            List<string> fournisseurCode = new List<string>();
            List<ContratInterimaireEnt> contrats = rapportLigne.Personnel.ContratInterimaires.ToList();
            fournisseurCode = explorateurDepenseHelper.GetFournisseurCodeFromContrats(contrats, periodeDebut, periodeFin);
            fournisseurCode.ForEach(x => expDep.Commentaire += fournisseurCode.Last() == x ? string.Format("{0}", x) : string.Format("{0}-", x));
        }

        private void PopulateNatureForFayatTP(ExplorateurDepenseGeneriqueModel expDep, NatureEnt odNature)
        {
            expDep.NatureId = odNature?.NatureId ?? 0;
            expDep.Nature = odNature;
        }

        private List<ExplorateurDepenseGeneriqueModel> ConvertToExplorateurDepenseGeneriqueList(List<ExplorateurDepenseFayatTPModel> explorateurDepenseFayatTPList)
        {
            List<ExplorateurDepenseGeneriqueModel> result = new List<ExplorateurDepenseGeneriqueModel>();

            foreach (ExplorateurDepenseFayatTPModel explorateurDepenseFayatTP in explorateurDepenseFayatTPList)
            {
                result.Add(explorateurDepenseFayatTP.ConvertToExplorateurDepenseGenerique());
            }
            return result;
        }

        private IEnumerable<ExplorateurDepenseFayatTPModel> ConvertToExplorateurFayatTPList(IEnumerable<ExplorateurDepenseGeneriqueModel> explorateurDepenseGeneriqueList)
        {
            List<ExplorateurDepenseFayatTPModel> result = new List<ExplorateurDepenseFayatTPModel>();

            foreach (ExplorateurDepenseGeneriqueModel explorateurDepenseGenerique in explorateurDepenseGeneriqueList)
            {
                result.Add(explorateurDepenseGenerique.ConvertToExplorateurDepenseFayatTP());
            }
            return result;
        }

        private async Task<IEnumerable<OperationDiverseEnt>> ComputeOdAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<OperationDiverseEnt> ods = await operationDiverseManager.GetOperationDiverseListWithNatureAsync(filtre.CiId).ConfigureAwait(false);

            ods = operationDiverseManager.ComputeOdsWithoutCorrectTache(ods);
            return ods;
        }
    }
}
