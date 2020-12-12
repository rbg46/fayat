using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonServiceLocator;
using Fred.Business.AffectationMoyen;
using Fred.Business.Moyen;
using Fred.Entities;
using Fred.Entities.Moyen;
using Fred.Entities.Referential;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Business.CI.ImportMoyenEtl.Output
{
    /// <summary>
    /// Processus etl : Execution de la sortie de l'import des Moyens
    /// </summary>
    internal class ImportMoyenOutput : IEtlOutput<MaterielEnt>
    {
        private readonly string logLocation = "[FLUX MOYEN][IMPORT DANS FRED][OUTPUT]";

        private readonly Lazy<IMoyenManager> moyenManager = new Lazy<IMoyenManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IMoyenManager>();
        });

        private readonly Lazy<IAffectationMoyenManager> affectationMoyenManager = new Lazy<IAffectationMoyenManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IAffectationMoyenManager>();
        });

        private readonly Business.Etl.Process.EtlExecutionLogger etlExecutionLogger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="etlExecutionLogger">Logger de l'exection de l'etl</param>
        public ImportMoyenOutput(Business.Etl.Process.EtlExecutionLogger etlExecutionLogger)
        {
            this.etlExecutionLogger = etlExecutionLogger;
        }

        /// <summary>
        /// Appelé par l'ETL
        /// </summary>
        /// <param name="result">liste des fournisseurs à envoyer à Fred</param>
        public async Task ExecuteAsync(IEtlResult<MaterielEnt> result)
        {
            await Task.Run(() =>
            {
                etlExecutionLogger.LogAndSerialize($"{logLocation} : INFO : Insertion dans Fred.", result.Items);

                List<AffectationMoyenEnt> affectationMoyens = affectationMoyenManager.Value.GetAffectationMoyens().ToList();


                foreach (var item in result.Items)
                {
                    var materiel = moyenManager.Value.AddOrUpdateMoyen(item);

                    // On vérifie si le moyen a déjà une affectation. Si non alors on crée une affectation par défaut.
                    AffectationMoyenEnt affectationMoyen = affectationMoyens.FirstOrDefault(x => x.MaterielId == materiel.MaterielId);
                    if (affectationMoyen == null)
                    {
                        affectationMoyen = new AffectationMoyenEnt()
                        {
                            MaterielId = materiel.MaterielId,
                            DateDebut = DateTime.UtcNow,
                            AffectationMoyenTypeId = (int)AffectationMoyenTypeCode.NoAffectation,
                            IsActive = true
                        };

                        affectationMoyenManager.Value.AddAffectationMoyen(affectationMoyen);
                    }
                }
            });
        }
    }
}
