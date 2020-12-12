using System;
using System.Collections.Generic;
using Fred.Business.Reception.FredIe;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Depense;

namespace Fred.ImportExport.Business.Reception.RMigo
{
    public class RMigoManager : IRMigoManager
    {
        private readonly IReceptionSapProviderService receptionSapProviderService;
        private readonly IUtilisateurManager utilisateurManager;

        public RMigoManager(IReceptionSapProviderService receptionSapProviderService, IUtilisateurManager utilisateurManager)
        {
            this.receptionSapProviderService = receptionSapProviderService;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Réponse de STORM après export des réceptions (Retour MIGO de SAP)
        /// </summary>
        /// <param name="receptions">Liste des réceptions SAP</param>
        /// <returns>Vrai si l'opération se termine</returns>
        public bool ImportRetourReceptionsFromSap(List<ReceptionSapModel> receptions)
        {
            try
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_RMIGO] Receptions des RMIGO : " + DateTime.Now);

                List<DepenseAchatEnt> receptionList = new List<DepenseAchatEnt>();

                var fredie = utilisateurManager.GetByLogin("fred_ie");

                foreach (var receptionSap in receptions)
                {
                    NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_RMIGO] Réception ID' = {receptionSap.ReceptionId} ");

                    DepenseAchatEnt reception = ImportReception(receptionSap, fredie.UtilisateurId);


                    receptionList.Add(reception);
                }
                if (receptions.Count > 0)
                {
                    //On récupère l'fredie générique pour STORM.                   

                    receptionSapProviderService.UpdateAndSavesWithoutValidation(receptionList, fredie.UtilisateurId);
                }
                NLog.LogManager.GetCurrentClassLogger().Info("[RECEPTION][FLUX_RMIGO] Les receptions ont bien été prise en compte par Fred." + DateTime.Now);

                return true;
            }
            catch (Exception exception)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(exception, "[RECEPTION][FLUX_RMIGO][ERROR] " + DateTime.Now);
                throw new FredIeBusinessException(exception.Message, exception);
            }
        }

        /// <summary>
        /// Gestion d'un import du retour d'une reception (RMIGO)
        /// </summary>
        /// <param name="receptionSap">retour RECEPTION SAP</param>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <returns>La reception Fred</returns>
        private DepenseAchatEnt ImportReception(ReceptionSapModel receptionSap, int? utilisateurId)
        {
            DepenseAchatEnt reception = receptionSapProviderService.FindById(receptionSap.ReceptionId);

            // RG_3656_083 : Vérification de l’existence de la Réception dans FRED 
            if (reception == null)
            {
                throw new FredBusinessException($"[RECEPTION][FLUX_RMIGO] 'Réception ID' = {receptionSap.ReceptionId} non reconnu dans FRED.");
            }

            // RG_3656_012 : Vérification et correction de la Date comptable de la Réception visée
            if ((reception.DateComptable.HasValue && receptionSap.DateComptable.HasValue && reception.DateComptable.Value.Date != receptionSap.DateComptable.Value.Date)
                || !reception.DateComptable.HasValue)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"[RECEPTION][FLUX_RMIGO] Changement date comptable 'Réception ID' = {receptionSap.ReceptionId} DateComptable = ${ receptionSap.DateComptable }");
                reception.DateComptable = receptionSap.DateComptable;
            }

            // RG_3656_013: Vérification et correction du Mouvement FAR associé à la Réception visée
            if (reception.PUHT * reception.Quantite != receptionSap.MouvementFarHt)
            {
                NLog.LogManager.GetCurrentClassLogger().Info("[RECEPTION][FLUX_RMIGO] Status visa = Echec");
                reception.StatutVisaId = StatutVisa.Echec.ToIntValue();
            }
            else
            {
                NLog.LogManager.GetCurrentClassLogger().Info("[RECEPTION][FLUX_RMIGO] Status visa = Sucess");
                reception.StatutVisaId = StatutVisa.Succes.ToIntValue();
            }

            reception.DateModification = DateTime.UtcNow;

            reception.AuteurModificationId = utilisateurId;


            return reception;
        }
    }
}
