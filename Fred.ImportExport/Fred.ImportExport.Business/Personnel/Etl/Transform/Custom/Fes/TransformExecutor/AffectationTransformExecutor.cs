using System.Collections.Generic;
using Fred.Entities.Affectation;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Import;
using Fred.Entities.Personnel.Imports;
using Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes.Comparator;
using Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes.EntityProvider;
namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes
{
    public class AffectationTransformExecutor
    {
        private readonly AffectationMapper affectationMapper;
        private readonly AffectationComparator affectationComparator;

        /// <summary>
        /// ctor
        /// </summary>   
        public AffectationTransformExecutor()
        {
            this.affectationMapper = new AffectationMapper();
            this.affectationComparator = new AffectationComparator();
        }

        /// <summary>
        /// Execute les transformation pour les affectation du personnel
        /// </summary>
        /// <param name="personnelTransformResults">personnelTransformResults</param>
        /// <param name="importPersonnelsSocieteData">importPersonnelsSocieteData</param>
        /// <returns>liste PersonnelAffectationResult</returns>
        public List<PersonnelAffectationResult> ExecuteAffectationTransforms(List<PersonnelAffectationResult> personnelTransformResults,
            ImportPersonnelsGlobalData importPersonnelsSocieteData)
        {
            foreach (var personnelTransformResult in personnelTransformResults)
            {
                ExecuteAffectationTransform(importPersonnelsSocieteData, personnelTransformResult);
            }
            return personnelTransformResults;
        }

        /// <summary>
        /// Fonction pour enlever les objets etablissements paie des objet personnels.
        /// En effet, Entity Framework peut essayer d'inserer ces sous objets de personnel lors d'un ajout d'un personnel.
        /// </summary>
        /// <param name="listAffectationSource">Liste des affectations</param>
        public void ClearEtablissementPaieFromPersonnel(List<PersonnelAffectationResult> listAffectationSource)
        {
            if (listAffectationSource == null || listAffectationSource.Count == 0)
            {
                return;
            }

            foreach (var affectation in listAffectationSource)
            {
                if (affectation.Personnel != null)
                {
                    affectation.Personnel.EtablissementPaie = null;
                }
            }
        }

        private void ExecuteAffectationTransform(ImportPersonnelsGlobalData importPersonnelsGlobalData, PersonnelAffectationResult personnelTransformResult)
        {
            PersonnelModel model = personnelTransformResult.PersonnelModel;
            PersonnelEnt personnel = personnelTransformResult.Personnel;
            AffectationEnt dbDefaultAffectation = AffectationDataProvider.GetDefaultAffectation(personnel.PersonnelId, importPersonnelsGlobalData.Affectations);
            AffectationEnt newAffectation = affectationMapper.Map(new AffectationEnt(), model, personnel, importPersonnelsGlobalData);
            personnelTransformResult.Affectations = new List<AffectationEnt>();

            if (affectationComparator.ExistInFred(dbDefaultAffectation))
            {
                if (affectationComparator.IsModified(dbDefaultAffectation, newAffectation))
                {
                    dbDefaultAffectation.AffectationIsNewOrModified = true;
                    dbDefaultAffectation.IsDefault = false;
                    personnelTransformResult.Affectations.Add(dbDefaultAffectation);

                    newAffectation.AffectationIsNewOrModified = true;
                    personnelTransformResult.Affectations.Add(newAffectation);
                }
                else
                {
                    dbDefaultAffectation.AffectationIsNewOrModified = false;
                    personnelTransformResult.Affectations.Add(dbDefaultAffectation);
                }
            }
            else
            {
                AffectationEnt dbaffectation = AffectationDataProvider.GetAffectation(personnel, model, importPersonnelsGlobalData);
                if (affectationComparator.ExistInFred(dbaffectation))
                {
                    dbaffectation.IsDefault = true;
                    dbaffectation.AffectationIsNewOrModified = true;
                    personnelTransformResult.Affectations.Add(dbaffectation);
                }
                else
                {
                    newAffectation.AffectationIsNewOrModified = true;
                    personnelTransformResult.Affectations.Add(newAffectation);
                }

            }
        }
    }
}
