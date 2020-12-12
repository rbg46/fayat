using System;
using Fred.Business.Journal;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities.Journal;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.JournauxComptable.Etl.Input;
using Fred.ImportExport.Business.JournauxComptable.Etl.Output;
using Fred.ImportExport.Business.JournauxComptable.Etl.Result;
using Fred.ImportExport.Business.JournauxComptable.Etl.Transform;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Etl.Engine.Builder;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.JournauxComptable;

namespace Fred.ImportExport.Business.JournauxComptable.Etl.Process
{
    /// <summary>
    /// ETL d'import depuis Anael vers Fred des Journaux comptables
    /// </summary>
    public class JournauxComptableEtlProcess : EtlProcessBase<JournauxComptableAnaelModel, JournalEnt>
    {
        private readonly IFluxManager fluxManager;
        private readonly ISocieteManager societeManager;
        private readonly IJournalManager journalComptableManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly FluxEnt flux;

        /// <summary>
        /// Ctor
        /// N'est pas référencé par Unity, 
        /// Est instancié par un new dans le manager Fred.IE des Journaux comptables.
        /// </summary>
        /// <param name="fluxManager">Manager Fred.IE des Flux</param>
        /// <param name="societeManager">Manager Fred des sociétés</param>
        /// <param name="journalComptableManager">Manager des Journaux Comptables</param>
        /// <param name="utilisateurManager">Manager des Utilisateur</param>
        /// <param name="flux"> Flux </param>
        public JournauxComptableEtlProcess(IFluxManager fluxManager, ISocieteManager societeManager, IJournalManager journalComptableManager, IUtilisateurManager utilisateurManager, FluxEnt flux)
        {
            this.fluxManager = fluxManager;
            this.societeManager = societeManager;
            this.journalComptableManager = journalComptableManager;
            this.utilisateurManager = utilisateurManager;
            this.flux = flux;
        }

        /// <summary>
        /// Chaine de connection à ANAEL
        /// </summary>
        public string ChaineConnexionAnael { get; set; }

        public string ImportJobId { get; set; }

        /// <summary>
        /// Dans cette fonction, il faut construite le workflow
        /// </summary>
        public override void Build()
        {
            EtlBuilder<JournauxComptableAnaelModel, JournalEnt> builder = new EtlBuilder<JournauxComptableAnaelModel, JournalEnt>(Config);
            builder
              .Input(new JournauxComptableInput(flux.SocieteCode, ChaineConnexionAnael))
              .Transform(new JournauxComptableTransform(societeManager.GetSocieteIdByCodeSocieteComptable(flux.SocieteCode), utilisateurManager))
              .Result(new JournauxComptableResult())
              .Output(new JournauxComptableOuput(journalComptableManager));
        }

        /// <summary>
        /// Vérification du flux
        /// </summary>
        /// <returns>
        /// si la chaine est vide alors le flux peut être exécuté
        /// si la chaine n'est pas vide, retourne un msg d'erreur</returns>
        private string CanExecuteImport()
        {
            if (flux == null)
            {
                return string.Format(IEBusiness.FluxInconnu, ImportJobId);
            }
            else if (!flux.IsActif)
            {
                return string.Format(IEBusiness.FluxInactif, ImportJobId);
            }

            return string.Empty;
        }

        /// <summary>
        /// Permet à la classe dérivée d'effectuer un traitement avant l'exécution du process
        /// </summary>
        protected override void OnBegin()
        {
            // Si l'import ne peut pas être exécuté, alors arrêt de l'etl
            string error = CanExecuteImport();
            if (!string.IsNullOrEmpty(error))
            {
                throw new FredIeEtlStopException(error);
            }
        }

        /// <summary>
        /// Permet à la classe dérivée d'effectuer un traitement en cas de succés
        /// </summary>
        /// <remarks>La fonction est abstraite car les classes dérivées doivent obligatoirement traiter la fin du process</remarks>
        protected override void OnSuccess()
        {
            flux.DateDerniereExecution = DateTime.UtcNow;
            fluxManager.Update(flux);
        }

        /// <summary>
        /// Permet à la classe dérivée d'effectuer un traitement en cas d'erreur
        /// L'exception est déjà logguée, ce n'est pas la peine de la relogger ou de faire un new throw, il est déjà fait
        /// </summary>
        /// <remarks>La fonction est abstraite car les classes dérivées doivent obligatoirement traiter les erreur</remarks>
        /// <param name="ex">exception</param>
        protected override void OnError(Exception ex)
        {
            // Qu'est ce que l'on fait ?
        }
    }
}
