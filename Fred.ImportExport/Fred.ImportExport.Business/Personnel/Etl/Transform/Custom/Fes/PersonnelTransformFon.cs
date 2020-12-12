﻿using System.Collections.Generic;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Entities.Personnel.Import;
using Fred.Entities.Personnel.Imports;
using Fred.ImportExport.Business.Personnel.Etl.Process;
using Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes;
using Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes.TransformExecutor;
using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom
{
    public class PersonnelTransformFon : IEtlTransform<PersonnelModel, PersonnelAffectationResult>
    {
        private readonly PersonnelEtlParameter parameter;

        private readonly PersonnelGlobalDataProvider personnelGlobalDataProvider;
        private readonly PersonnelTransformExecutor personnelTransformExecutor;
        private readonly ManagerTransformExecutor managerTransformExecutor;
        private readonly AffectationTransformExecutor affectationTransformExecutor;

        public PersonnelTransformFon(PersonnelEtlParameter parameter, IImportPersonnelManager importPersonnelFesManager)
        {
            this.parameter = parameter;
            this.personnelGlobalDataProvider = new PersonnelGlobalDataProvider(parameter, importPersonnelFesManager);
            this.personnelTransformExecutor = new PersonnelTransformExecutor(parameter);
            this.managerTransformExecutor = new ManagerTransformExecutor();
            this.affectationTransformExecutor = new AffectationTransformExecutor();
        }

        /// <summary>
        /// Execute l'ensemble des transformations
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="result">result</param>
        public void Execute(IEtlInput<PersonnelModel> input, ref IEtlResult<PersonnelAffectationResult> result)
        {
            List<string> codeSocietePayes = PersonnelCodeSocietePayeSpliter.GetCodeSocietePayesInFlux(parameter.Flux);
            IEnumerable<PersonnelModel> models = input.Items;
            if (models.Any())
            {
                IEnumerable<string> ciCodes = models.Select(a => a.Section);
                ImportPersonnelsGlobalData importPersonnelsGlobalData = personnelGlobalDataProvider.GetGlobalData(codeSocietePayes, ciCodes);
                List<PersonnelAffectationResult> personnelTransformResults = personnelTransformExecutor.ExecuteTransformAllPersonnel(models, importPersonnelsGlobalData);

                parameter.Logger.LogNoModificationOfPersonnel(personnelTransformResults);
                parameter.Logger.LogDetectionModificationOfPersonnel(personnelTransformResults);
                parameter.Logger.LogDetectionCreationPersonnel(personnelTransformResults);
                parameter.Logger.WarnEtablissementPaieNotFound(personnelTransformResults);
                parameter.Logger.WarnRessourceNotFound(personnelTransformResults);

                var managerTransformResults = managerTransformExecutor.ExecuteTransformManagerOfPersonnels(personnelTransformResults, importPersonnelsGlobalData);

                parameter.Logger.WarnManagerNotFound(personnelTransformResults);

                var affectationTransformResults = affectationTransformExecutor.ExecuteAffectationTransforms(managerTransformResults, importPersonnelsGlobalData);

                affectationTransformExecutor.ClearEtablissementPaieFromPersonnel(affectationTransformResults);

                parameter.Logger.WarnNoCiFound(personnelTransformResults);

                foreach (var affectationTransformResult in affectationTransformResults)
                {
                    result.Items.Add(affectationTransformResult);
                }
            }
        }
    }
}
