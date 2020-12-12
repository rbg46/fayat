using System;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Exceptions;

namespace Fred.ImportExport.Business.Personnel.Etl.Process
{
    /// <summary>
    /// ETL d'import depuis Anael vers Fred des Personnels
    /// </summary>
    /// <typeparam name="TI">Input Template</typeparam>
    /// <typeparam name="TR">Result Template</typeparam>
    public abstract class AbstractBasePersonnelProcess<TI, TR> : EtlProcessBase<TI, TR>
    {
        private readonly string logLocation = "[IMPORT][FLUX_PERSONNEL][PROCESS]";
        private IFluxManager fluxManager;
        private PersonnelEtlParameter parameter;

        public void Init(PersonnelEtlParameter parameter, IFluxManager fluxManager)
        {
            this.parameter = parameter;
            this.fluxManager = fluxManager;
        }

        /// <inheritdoc/>
        protected override void OnBegin()
        {
            // Si l'import ne peut pas être exécuté, alors arrêt de l'etl
            string error = CanExecuteImport();
            if (!string.IsNullOrEmpty(error))
            {
                throw new FredIeEtlStopException(error);
            }
            Logger.Info($"{logLocation} : INFO : Démarrage de l'import du personnel.");
        }

        /// <inheritdoc />
        protected override void OnSuccess()
        {
            var flux = fluxManager.GetByCode(parameter.CodeFlux);
            flux.DateDerniereExecution = DateTime.UtcNow;
            fluxManager.Update(flux);
            var message = $"{logLocation} : SUCCESS : Insertion ou modification des Personnels dans FRED réussie.";
            Logger.Info(message);
        }

        /// <inheritdoc />
        protected override void OnError(Exception ex)
        {
            var errorMessage = $"{logLocation} : ERROR  " + ex.Message;
            Logger.Error(errorMessage);

            var error = $"{logLocation} : ERROR : L'insertion ou la modification des Personnels dans FRED a échouée.";
            Logger.Error(error);
        }

        /// <summary>
        /// Vérification du flux
        /// </summary>
        /// <returns>
        /// si la chaine est vide alors le flux peut être exécuté
        /// si la chaine n'est pas vide, retourne un msg d'erreur</returns>
        private string CanExecuteImport()
        {
            string msg = string.Empty;
            var flux = fluxManager.GetByCode(parameter.CodeFlux);

            if (flux == null)
            {
                msg = string.Format(IEBusiness.FluxInconnu, parameter.CodeFlux);
            }
            else if (!flux.IsActif)
            {
                msg = string.Format(IEBusiness.FluxInactif, flux.Code);
            }

            return msg;
        }

    }
}
