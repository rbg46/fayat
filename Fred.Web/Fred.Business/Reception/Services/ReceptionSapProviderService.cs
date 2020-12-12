using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Fred.Framework.Exceptions;

namespace Fred.Business.Reception.FredIe
{
    /// <summary>
    /// Service qui fournie les Receptions a envoyé a sap
    /// </summary>
    public class ReceptionSapProviderService : IReceptionSapProviderService
    {
        private readonly IDepenseRepository depenseRepository;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uow">Uow</param>
        public ReceptionSapProviderService(IUnitOfWork uow, IDepenseRepository depenseRepository)
        {
            this.Uow = uow;
            this.depenseRepository = depenseRepository;
        }

        /// <summary>
        /// Uow
        /// </summary>       
        public IUnitOfWork Uow { get; set; }


        /// <summary>
        /// Permet de récupérer une liste de réceptions en fonction des identifiants filtrés pour sap
        /// (on ignore les receptions dont la quantité est égale à 0)
        /// </summary>
        /// <param name="receptionIds">La liste des identifants à récupérer</param>
        /// <returns>Une liste de dépenses.</returns>
        public IEnumerable<DepenseAchatEnt> GetReceptionsFilteredForSap(List<int> receptionIds)
        {
            // RG_2860_052
            return ExecuteInTryCatch(() => depenseRepository.GetDepenses(receptionIds).Where(x => x.Quantite > 0));
        }

        public IEnumerable<DepenseAchatEnt> GetReceptionsPositivesAndNegativesFilteredForSap(List<int> receptionIds)
        {
            return ExecuteInTryCatch(() => depenseRepository.GetDepenses(receptionIds).Where(x => x.Quantite != 0));
        }


        private IEnumerable<DepenseAchatEnt> ExecuteInTryCatch(Func<IEnumerable<DepenseAchatEnt>> action)
        {
            try
            {
                return action();
            }
            catch (Exception exception)
            {
                throw new FredBusinessException(exception.Message, exception);
            }
        }

        /// <summary>
        /// Permet de récupérer une liste de réceptions en fonction des identifiants
        /// </summary>
        /// <param name="receptionIds">La liste des identifants à récupérer</param>
        /// <returns>Une liste de dépenses.</returns>
        public IEnumerable<DepenseAchatEnt> GetReceptions(List<int> receptionIds)
        {
            try
            {
                return depenseRepository.GetDepenses(receptionIds);
            }
            catch (Exception exception)
            {
                throw new FredBusinessException(exception.Message, exception);
            }
        }

        /// <summary>
        ///  Mise à jour d'une liste de réceptions sans passer par la validation 
        /// </summary>
        /// <param name="receptions">Liste de réceptions à mettre à jour</param>
        /// <param name="auteurModificationId">Identifaint de l'utilisateur effectuant la modification</param>
        public void UpdateAndSavesWithoutValidation(IEnumerable<DepenseAchatEnt> receptions, int? auteurModificationId)
        {
            try
            {
                foreach (var reception in receptions)
                {
                    InternalUpdateWithoutValidation(reception, auteurModificationId);
                }

                Uow.Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        ///  Mise à jour d'une liste de réceptions sans passer par la validation (methode interne commune)
        /// </summary>
        /// <param name="reception">Réception à mettre à jour</param>
        /// <param name="auteurModificationId">Identifaint de l'utilisateur effectuant la modification</param>
        private void InternalUpdateWithoutValidation(DepenseAchatEnt reception, int? auteurModificationId)
        {
            try
            {
                reception.DateModification = DateTime.UtcNow;
                reception.AuteurModificationId = auteurModificationId;
                depenseRepository.UpdateDepenseWithoutSave(reception);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }


        /// <summary>
        /// Recupere une DepenseAchatEnt par sont Id
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>une DepenseAchatEnt</returns>
        public DepenseAchatEnt FindById(int rapportId)
        {
            return depenseRepository.FindById(rapportId);
        }
    }
}
