using System;
using System.Collections.Generic;
using System.Configuration;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Fournisseur.Etl.Input;
using Fred.ImportExport.Business.Fournisseur.Etl.Output;
using Fred.ImportExport.Business.Fournisseur.Etl.Result;
using Fred.ImportExport.Business.Fournisseur.Etl.Transform;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Etl.Engine.Builder;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Process
{
    /// <summary>
    /// ETL d'import depuis Anael vers Fred des établissements comptables
    /// </summary>
    public class ImportFournisseurProcess : EtlProcessBase<FournisseurModel, FournisseurFredModel>
    {
        private readonly IFluxManager fluxManager;
        private readonly IPaysManager paysManager;
        private readonly IFournisseurManager fournisseurManager;
        private readonly ISocieteManager societeManager;
        private FluxEnt flux;
        private readonly string importJobId = ConfigurationManager.AppSettings["flux:fournisseur"];
        private ImportFournisseurInput input;

        /// <summary>
        /// Ctor
        /// N'est pas référencé par Unity, 
        /// Est instancié par un new dans le manager Fred.IE des Fournisseurs.
        /// </summary>
        /// <param name="fluxManager">Manager Fred.IE des Flux</param>
        /// <param name="paysManager">Manager Fred des Pays</param>
        /// <param name="fournisseurManager">Manager Fred des Fournisseurs</param>
        /// <param name="societeManager">Manager Fred des Sociétés</param>
        public ImportFournisseurProcess(IFluxManager fluxManager, IPaysManager paysManager, IFournisseurManager fournisseurManager, ISocieteManager societeManager)
        {
            this.fluxManager = fluxManager;
            this.paysManager = paysManager;
            this.fournisseurManager = fournisseurManager;
            this.societeManager = societeManager;
        }

        /// <inheritdoc/>
        public override void Build()
        {
            flux = fluxManager.GetByCode(importJobId);
            input = new ImportFournisseurInput();

            var builder = new EtlBuilder<FournisseurModel, FournisseurFredModel>(Config);
            builder
              .Input(input)
              .Transform(new ImportFournisseurTransform(paysManager))
              .Result(new ImportFournisseurResult())
              .Output(new ImportFournisseurOuput(fournisseurManager, societeManager));
        }

        /// <summary>
        ///   Renseigne les paramètres d'input
        /// </summary>
        /// <param name="fournisseurs">La liste des founisseurs.</param>
        public void SetParameter(List<FournisseurModel> fournisseurs)
        {
            input.Fournisseurs = fournisseurs;
            input.Flux = flux;
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

            if (flux == null)
            {
                msg = string.Format(IEBusiness.FluxInconnu, importJobId);
            }
            else if (!flux.IsActif)
            {
                msg = string.Format(IEBusiness.FluxInactif, importJobId);
            }

            return msg;
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
        }

        /// <inheritdoc />
        protected override void OnSuccess()
        {
            flux.DateDerniereExecution = DateTime.UtcNow;
            fluxManager.Update(flux);
        }

        /// <inheritdoc />
        protected override void OnError(Exception ex)
        {
            // Qu'est ce que l'on fait ?
        }
    }
}
