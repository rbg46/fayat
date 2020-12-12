using System;
using System.Configuration;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Etablissement.Etl.Input;
using Fred.ImportExport.Business.Etablissement.Etl.Output;
using Fred.ImportExport.Business.Etablissement.Etl.Result;
using Fred.ImportExport.Business.Etablissement.Etl.Transform;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Etl.Engine.Builder;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Etablissement;

namespace Fred.ImportExport.Business.Etablissement.Etl.Process
{
    public class EtablissementComptableEtlProcess : EtlProcessBase<EtablissementComptableAnaelModel, EtablissementComptableEnt>, IEtablissementComptableEtlProcess
    {
        private readonly IFluxManager fluxManager;
        private readonly ISocieteManager societeManager;
        private readonly IEtablissementComptableManager etsComptaManager;
        private readonly FluxEnt flux;
        private readonly string importJobId = ConfigurationManager.AppSettings["flux:etablissement:comptable"];
        private readonly string chaineConnexionAnael = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];


        /// <summary>
        /// Ctor
        /// N'est pas référencé par Unity, 
        /// Est instancié par un new dans le manager Fred.IE des établissements comptables.
        /// </summary>
        /// <param name="fluxManager">Manager Fred.IE des Flux</param>
        /// <param name="societeManager">Manager Fred des sociétés</param>
        /// <param name="etsComptaManager">Manager Fred des établissements comptables</param>
        public EtablissementComptableEtlProcess(IFluxManager fluxManager, ISocieteManager societeManager, IEtablissementComptableManager etsComptaManager)
        {
            this.fluxManager = fluxManager;
            this.societeManager = societeManager;
            this.etsComptaManager = etsComptaManager;
            flux = fluxManager.GetByCode(importJobId);
        }

        /// <inheritdoc/>
        public override void Build()
        {
            var builder = new EtlBuilder<EtablissementComptableAnaelModel, EtablissementComptableEnt>(Config);
            builder
              .Input(new EtablissementComptableInput(flux.SocieteCode, chaineConnexionAnael))
              .Transform(new EtablissementComptableTransform(societeManager.GetSocieteIdByCodeSocieteComptable(flux.SocieteCode)))
              .Result(new EtablissementComptableResult())
              .Output(new EtablissementComptableOuput(etsComptaManager));
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