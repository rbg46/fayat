using System;
using System.Collections.Generic;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Reception;
using Fred.Business.Reception.Services;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Depense;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Facturation.Validators;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation
{
    /// <summary>
    /// Gestionnaire des flux des facturations.
    /// </summary>
    public class FacturationFluxManager : AbstractFluxManager
    {
        private readonly FluxMiroImporter fluxMiroImporter;
        private readonly FluxFb60Importer fluxFb60Importer;
        private readonly FluxMr11Importer fluxMr11Importer;
        private readonly ICIManager ciMgr;
        private readonly IDatesClotureComptableManager datesClotureComptableMgr;
        private readonly IReceptionManager receptionMgr;
        private readonly IUtilisateurManager utilisateurMgr;
        private readonly IReceptionsProviderWithFilterService receptionsProviderWithFilterService;

        public FacturationFluxManager(
            IFluxManager fluxManager,
            FluxMiroImporter fluxMiroImporter,
            FluxFb60Importer fluxFb60Importer,
            FluxMr11Importer fluxMr11Importer,
            ICIManager ciMgr,
            IDatesClotureComptableManager datesClotureComptableMgr,
            IReceptionManager receptionMgr,
            IUtilisateurManager utilisateurMgr,
            IReceptionsProviderWithFilterService receptionsProviderWithFilterService)
            : base(fluxManager)
        {
            this.fluxMiroImporter = fluxMiroImporter;
            this.fluxFb60Importer = fluxFb60Importer;
            this.fluxMr11Importer = fluxMr11Importer;
            this.ciMgr = ciMgr;
            this.datesClotureComptableMgr = datesClotureComptableMgr;
            this.receptionMgr = receptionMgr;
            this.utilisateurMgr = utilisateurMgr;
            this.receptionsProviderWithFilterService = receptionsProviderWithFilterService;
        }

        /// <summary>
        /// Permet d'importer la date de transfert des FAR pour une liste de CI.
        /// </summary>
        /// <param name="dateTransfertFarModel">Une liste de model de facturation.</param>
        public void ImportDateTransfertFar(DateTransfertFarModel dateTransfertFarModel)
        {
            try
            {
                var datesClotureComptables = new List<DatesClotureComptableEnt>();
                var ciIds = new List<int>();

                NLog.LogManager.GetCurrentClassLogger().Info("[FACTURATION][FLUX_????] Traitement du flux de Notification de FAR");

                foreach (string ciCode in dateTransfertFarModel.CiCodes)
                {
                    //On récupére le CI Fred en fonction du code société et du code CI.
                    CIEnt ci = ciMgr.GetCI(ciCode, dateTransfertFarModel.SocieteCode);

                    if (ci == null)
                    {
                        throw new FredIeBusinessException($"[FACTURATION][FLUX_????] Notification de FAR : aucun CI trouvé pour le 'Code CI' ANAEL = {ciCode} et le 'Code Société' ANAEL = {dateTransfertFarModel.SocieteCode}");
                    }

                    ciIds.Add(ci.CiId);

                    datesClotureComptables.Add(new DatesClotureComptableEnt
                    {
                        Annee = dateTransfertFarModel.Annee,
                        Mois = dateTransfertFarModel.Mois,
                        AuteurSap = dateTransfertFarModel.AuteurSap,
                        CiId = ci.CiId,
                        DateTransfertFAR = DateTime.UtcNow.Date
                    });
                }

                // On insert en masse.
                datesClotureComptableMgr.BulkInsert(datesClotureComptables);

                // RG_2863_180: Bascule automatique des réceptions non visées lors de la notification par SAP du transfert des FAR
                ProcessReceptions(dateTransfertFarModel, ciIds);
            }
            catch (FredIeBusinessException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new FredIeBusinessException(exception.Message, exception);
            }
        }

        /// <summary>
        /// Permet d'importer une facturation dans Fred.
        /// </summary>
        /// <param name="facturationModels">Une liste de facturation.</param>    
        public void ImportFacturation(List<FacturationSapModel> facturationModels)
        {
            fluxMiroImporter.Import(facturationModels);
        }

        /// <summary>
        ///     Gestion du flux Annulation FAR / MR11
        /// </summary>
        /// <param name="facturationModels">Liste des facturations issues de STORM</param>    
        public void ImportAnnulationFar(List<FacturationSapModel> facturationModels)
        {
            fluxMr11Importer.Import(facturationModels);
        }

        /// <summary>
        ///     Gestion du flux Avoir Sans Commande / FB60
        /// </summary>
        /// <param name="facturationModels">Liste des facturations issues de STORM</param>    
        public void ImportAvoirSansCommande(List<FacturationSapModel> facturationModels)
        {
            fluxFb60Importer.Import(facturationModels);
        }

        #region Private

        /// <summary>
        ///     Gestion des réceptions des CI dont la date comptable est clôturée en FAR
        /// </summary>
        /// <param name="dateTransfertFarModel">Date transfert</param>
        /// <param name="ciIds">Liste des identifiants des CI</param>
        private void ProcessReceptions(DateTransfertFarModel dateTransfertFarModel, List<int> ciIds)
        {
            var date = new DateTime(dateTransfertFarModel.Annee, dateTransfertFarModel.Mois, 1);
            var filter = new SearchDepenseEnt
            {
                Cis = ciIds,
                AViser = true,
                Visees = false,
                Far = false,
                DateFrom = date,
                DateTo = date,
                DepenseTypeId = DepenseType.Reception.ToIntValue()
            };

            List<DepenseAchatEnt> receptionsNonVisees = receptionsProviderWithFilterService.GetReceptionsWithFilter(filter);

            foreach (DepenseAchatEnt r in receptionsNonVisees)
            {
                // RG_2863_110 : Vérifier si la période est "Bloquée en réception" pour le CI de la commande
                // La période est forcément bloquée en réception donc on récupère directement la bonne date comptable
                r.DateComptable = datesClotureComptableMgr.GetNextUnblockedInReceptionPeriod(r.CiId.Value, r.Date.Value);
            }

            // Mise à jour des réceptions sans passer par les RG 
            receptionMgr.UpdateAndSaveWithoutValidation(receptionsNonVisees, utilisateurMgr.GetByLogin("fred_ie").UtilisateurId);
        }

        #endregion
    }
}
