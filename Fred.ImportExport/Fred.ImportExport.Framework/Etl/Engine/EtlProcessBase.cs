using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Framework.Etl.Engine.Config;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Exceptions;
using NLog;


namespace Fred.ImportExport.Framework.Etl.Engine
{

    /// <inheritdoc/>
    public abstract class EtlProcessBase<TI, TR> : IEtlProcess<TI, TR>
    {
        /// <summary>
        /// ctor
        /// Protected car c'est une classe abstract
        /// </summary>
        protected EtlProcessBase()
        {
            Name = GetType().Name;
            Logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Pour logger...
        /// </summary>
        public Logger Logger { get; }

        /// <inheritdoc/>
        public string Name { get; protected set; }

        /// <inheritdoc/>
        public IEtlConfig<TI, TR> Config { get; protected set; } = new EtlConfig<TI, TR>();

        /// <inheritdoc/>
        public abstract void Build();


        /// <inheritdoc/>
        public async Task ExecuteAsync(IList<TI> input)
        {
            if (Config.Input != null)
            {
                Config.Input.Items = input;
            }

            await ExecuteAsync();
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync()
        {
            try
            {
                ProcessBegin();
                ProcessInput();
                ProcessTransforms();
                await ProcessOutputsAsync();
                ProcessSuccess();
            }
            catch (FredIeEtlException etlEx)
            {
                ProcessError(etlEx);
                throw;
            }
            catch (FredIeBusinessException businessEx)
            {
                ProcessError(businessEx);
                throw;
            }
            catch (FredException fredEx)
            {
                ProcessError(fredEx);
                throw new FredIeBusinessException(fredEx.Message, fredEx);
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                throw new FredIeBusinessException(ex.Message, ex);
            }
            finally
            {
                ProcessFinally();
            }
        }

        /// <summary>
        /// Appelé avant le lancement de l'etl
        /// </summary>
        private void ProcessBegin()
        {
            Logger.Info($"Etl : {Name} : Début du processus.");

            // Vérification
            Config.ValidateConfig();

            // Notification aux classes dérivées
            OnBegin();
        }




        /// <summary>
        /// Execute l'input
        /// </summary>
        private void ProcessInput()
        {
            Logger.Info($"Etl : {Name} : Processus d'entrée.");

            Config.Input?.Execute();
        }




        /// <summary>
        /// Execute les transformations
        /// </summary>
        private void ProcessTransforms()
        {
            Logger.Info($"Etl : {Name} : Processus de transformation.");

            if (Config.Transforms == null)
            {
                return;
            }

            // Result est passé par référence
            // Ce qui permet à une classe Transforme de l'instancier s'il est null
            foreach (var transform in Config.Transforms.Items)
            {
                IEtlResult<TR> result = Config.Result;
                transform.Execute(Config.Input, ref result);
                Config.Result = result;
            }
        }


        /// <summary>
        /// Execute les sorties
        /// </summary>
        private async Task ProcessOutputsAsync()
        {
            Logger.Info($"Etl : {Name} : Processus de sortie.");

            if (Config.OutPuts == null)
            {
                return;
            }

            foreach (var output in Config.OutPuts.Items)
            {
                await output.ExecuteAsync(Config.Result);
            }
        }




        /// <summary>
        /// Execute le processus Success
        /// </summary>
        private void ProcessSuccess()
        {
            Logger.Info($"Etl : {Name} : Fin du processus. Terminé avec succés.");

            // Notification aux classes dérivées
            OnSuccess();
        }



        /// <summary>
        /// Traitement des erreurs
        /// Appelé lorsqu'une exception est capturée
        /// </summary>
        /// <param name="ex">Exception</param>
        private void ProcessError(Exception ex)
        {
            if (ex != null)
            {
                Logger.Error(ex, $"Etl : {Name} : Fin du processus, Terminé avec l'erreur : {ex.FirstInnerException().Message}");
            }
            else
            {
                Logger.Error($"Etl : {Name} : Fin du processus, Terminé avec une erreur inconnue...");
            }

            // Notification aux classes dérivées
            OnError(ex);
        }




        /// <summary>
        /// Appelé dans tous les cas à la fin du process (même en cas d'exception)
        /// </summary>
        private void ProcessFinally()
        {
            Logger.Info($"Etl : {Name} : Fin du processus. Cloture");

            // Notification aux classes dérivées
            OnFinally();
        }


        /// <summary>
        /// Permet à la classe dérivée d'effectuer un traitement avant l'exécution du process
        /// </summary>
        protected virtual void OnBegin()
        {
            // rien ici, destinée aux classes dérivées
        }


        /// <summary>
        /// Permet à la classe dérivée d'effectuer un traitement en cas de succés
        /// </summary>
        /// <remarks>La fonction est abstraite car les classes dérivées doivent obligatoirement traiter la fin du process</remarks>
        protected abstract void OnSuccess();


        /// <summary>
        /// Permet à la classe dérivée d'effectuer un traitement en cas d'erreur
        /// L'exception est déjà logguée, ce n'est pas la peine de la relogger ou de faire un new throw, il est déjà fait
        /// </summary>
        /// <remarks>La fonction est abstraite car les classes dérivées doivent obligatoirement traiter les erreur</remarks>
        /// <param name="ex">exception</param>
        protected abstract void OnError(Exception ex);


        /// <summary>
        /// Permet à la classe dérivée d'effectuer un traitement à la fin de l'exécution du process
        /// </summary>
        protected virtual void OnFinally()
        {
            // rien ici, destinée aux classes dérivées
        }

    }
}
