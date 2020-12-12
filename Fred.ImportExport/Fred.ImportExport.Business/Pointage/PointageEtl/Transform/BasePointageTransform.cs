using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.ImportExport.Business.Pointage.PointageEtl.Parameter;
using Fred.ImportExport.Business.Pointage.PointageEtl.Result;
using Fred.ImportExport.Business.Pointage.PointageEtl.Transform.Features;
using Fred.ImportExport.Business.Pointage.PointageEtl.Transform.Models.Base;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using System.Collections.Generic;
using System.Linq;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Transform
{

    /// <summary>
    /// Classe qui transform une RapportLigneEnt en ExportRapportLigneModel
    /// </summary>
    public class BasePointageTransform : IEtlTransform<RapportLigneEnt, ExportRapportLigneModel>
    {
        private readonly EtlPointageParameter parameter;


        public BasePointageTransform(EtlPointageParameter parameter)
        {
            this.parameter = parameter;
        }

        public void Execute(IEtlInput<RapportLigneEnt> input, ref IEtlResult<ExportRapportLigneModel> result)
        {
            if (result == null)
            {
                result = new BasePointageResult();
            }
            foreach (var item in input.Items)
            {
                result.Items.Add(ConvertToModel(item));
                // US 7172 : Pour historisation dans la table Fred_WorkflowPointage
                parameter.ListRapportLignesEnt.Add(item);
            }
        }

        private ExportRapportLigneModel ConvertToModel(RapportLigneEnt input)
        {

            return new ExportRapportLigneModel
            {
                RapportId = input.RapportId,
                RapportLigneId = input.RapportLigneId,
                DateModification = input.DateSuppression.HasValue ? input.DateSuppression : input.DateModification,
                DateSuppression = input.DateSuppression,
                SocieteCode = parameter.CodeSocieteStorm,
                Personnel = input.GetPersonnel(),
                CiCode = input.GetCiCode(),
                Date = input.GetDate(),
                MajorationCode = input.GetMajorationCode(),
                MajorationHeure = input.GetMajorationHeure(),
                AbsenceCode = input.GetAbsenceCode(),
                AbsenceHeure = input.GetAbsenceHeure(),
                DeplacementCode = input.GetDeplacementCode(),
                DeplacementZone = input.GetDeplacementZone(),
                DeplacementIVD = input.GetDeplacementIVD(),
                Prime = MapLignesPrimes(input.ListRapportLignePrimes.GetRapportLignePrimesChecked()),
                Astreintes = MapLignesAstreintes(input.ListRapportLigneAstreintes),
                Tache = MapLignesTaches(input.ListRapportLigneTaches),
                MatriculeSap = MapPersonnel(input.Personnel).MatriculeSAP,
                MatriculePixid = MapPersonnel(input.Personnel).MatriculePixid,
                MatriculeDirectSkills = MapPersonnel(input.Personnel).MatriculeDirectSkills

            };
        }

        private ExportRapportTacheModel[] MapLignesTaches(ICollection<RapportLigneTacheEnt> listRapportLigneTaches)
        {
            return listRapportLigneTaches.Select(a => MapLigneTache(a)).ToArray();
        }

        private ExportRapportTacheModel MapLigneTache(RapportLigneTacheEnt rapportLigneTache)
        {
            return new ExportRapportTacheModel()
            {
                TacheCode = rapportLigneTache.GetTacheCode(),
                TacheHeure = rapportLigneTache.GetTacheHeure()
            };
        }

        private ExportRapportLignePrimeModel[] MapLignesPrimes(IEnumerable<RapportLignePrimeEnt> listRapportLignePrimes)
        {
            return listRapportLignePrimes.Select(a => MapLignePrime(a)).ToArray();
        }

        private ExportRapportLignePrimeModel MapLignePrime(RapportLignePrimeEnt rapportLignePrime)
        {
            if (rapportLignePrime.HeurePrime.HasValue)
            {
                return new ExportRapportLignePrimeHoraireModel()
                {
                    PrimeCode = rapportLignePrime.GetPrimeCode(),
                    PrimeHeure = rapportLignePrime.HeurePrime
                };
            }
            else
            {
                return new ExportRapportLignePrimeModel()
                {
                    PrimeCode = rapportLignePrime.GetPrimeCode()
                };
            }
        }

        private ExportPersonnelModel MapPersonnel(PersonnelEnt personnel)
        {
            var exportPersonnelModel = new ExportPersonnelModel();

            exportPersonnelModel.MatriculeSAP = personnel.MatriculeExterne.FirstOrDefault(m => m.Source == "SAP") == null ? string.Empty : personnel.MatriculeExterne.FirstOrDefault(m => m.Source == "SAP").Matricule;
            exportPersonnelModel.MatriculePixid = personnel.MatriculeExterne.FirstOrDefault(m => m.Source == "PIXID") == null ? string.Empty : personnel.MatriculeExterne.FirstOrDefault(m => m.Source == "PIXID").Matricule;
            exportPersonnelModel.MatriculeDirectSkills = personnel.MatriculeExterne.FirstOrDefault(m => m.Source == "DIRECTSKILLS") == null ? string.Empty : personnel.MatriculeExterne.FirstOrDefault(m => m.Source == "DIRECTSKILLS").Matricule;

            return exportPersonnelModel;
        }

        private ExportRapportLigneAstreinteModel[] MapLignesAstreintes(IEnumerable<RapportLigneAstreinteEnt> listRapportLigneAstreintes)
        {
            return listRapportLigneAstreintes.Select(a => MapLigneAstreinte(a)).ToArray();
        }

        private ExportRapportLigneAstreinteModel MapLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte)
        {
            return new ExportRapportLigneAstreinteModel()
            {
                AstreinteCode = rapportLigneAstreinte.GetAstreinteCode(),
                DateDebutAstreinte = rapportLigneAstreinte.DateDebutAstreinte.ToString(),
                DateFinAstreinte = rapportLigneAstreinte.DateFinAstreinte.ToString(),
            };
        }
    }
}
