using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential.TypeRattachement;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Import;
using Fred.Entities.Personnel.Imports;
using Fred.ImportExport.Business.Personnel.Etl.Process;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes
{
    public class PersonnelTransformExecutor
    {
        private readonly PersonnelMapper personnelMapper;
        private readonly PersonnelComparator personnelComparator;
        private readonly PersonnelEtlParameter parameter;

        public PersonnelTransformExecutor(PersonnelEtlParameter parameter)
        {
            this.personnelMapper = new PersonnelMapper();
            this.personnelComparator = new PersonnelComparator();
            this.parameter = parameter;
        }

        public List<PersonnelAffectationResult> ExecuteTransformAllPersonnel(IEnumerable<PersonnelModel> personnelModels, ImportPersonnelsGlobalData importPersonnelsSocieteData)
        {
            parameter.Logger.LogStartTransormPersonnel();

            var personnelTransformResults = new List<PersonnelAffectationResult>();

            foreach (var model in personnelModels)
            {
                PersonnelAffectationResult newOrUpdatedPersonnel = ChangeSimplePropertyForPersonnel(importPersonnelsSocieteData, model);

                if (newOrUpdatedPersonnel.Personnel != null && newOrUpdatedPersonnel.PersonnelModel != null)
                {
                    personnelTransformResults.Add(newOrUpdatedPersonnel);
                }
            }

            return personnelTransformResults;
        }

        private PersonnelAffectationResult ChangeSimplePropertyForPersonnel(ImportPersonnelsGlobalData importPersonnelsGlobalData, PersonnelModel model)
        {
            PersonnelAffectationResult result = new PersonnelAffectationResult();

            var societeData = importPersonnelsGlobalData.GetSocieteData(model.CodeSocietePaye);
            if (societeData != null)
            {
                PersonnelEnt oldPersonnel = PersonnelEntityDataProvider.GetPersonnelFromDataBase(model, societeData.FredPersonnelsForSociete, societeData.Societe.SocieteId);
                PersonnelEnt newPersonnel = personnelMapper.Map(new PersonnelEnt(), model, importPersonnelsGlobalData);

                if (personnelComparator.ExistInFred(oldPersonnel))
                {
                    if (personnelComparator.IsModified(oldPersonnel, newPersonnel))
                    {
                        PersonnelEnt updatedPersonnel = personnelMapper.Map(oldPersonnel, model, importPersonnelsGlobalData);
                        updatedPersonnel.DateModification = DateTime.UtcNow;
                        updatedPersonnel.UtilisateurIdModification = importPersonnelsGlobalData.FredIe.UtilisateurId;

                        result.Personnel = updatedPersonnel;
                        result.PersonnelModel = model;

                        result.PersonnelIsNew = false;
                        result.PersonnelIsModified = true;
                        result.PersonnelIsNotModified = false;
                    }
                    else
                    {
                        result.Personnel = oldPersonnel;
                        result.PersonnelModel = model;

                        result.PersonnelIsNew = false;
                        result.PersonnelIsModified = false;
                        result.PersonnelIsNotModified = true;
                    }
                }
                else
                {
                    newPersonnel.DateCreation = DateTime.UtcNow;
                    newPersonnel.UtilisateurIdCreation = importPersonnelsGlobalData.FredIe.UtilisateurId;
                    newPersonnel.IsInterne = true;
                    newPersonnel.TypeRattachement = TypeRattachement.Agence;
                    result.Personnel = newPersonnel;
                    result.PersonnelModel = model;

                    result.PersonnelIsNew = true;
                    result.PersonnelIsModified = false;
                    result.PersonnelIsNotModified = false;
                }

                result.Personnel.EtablissementPaie = societeData.EtablissementPaiesForSociete.FirstOrDefault(e => e.EtablissementPaieId == result.Personnel.EtablissementPaieId);
            }

            return result;
        }
    }
}
