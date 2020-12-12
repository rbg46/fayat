using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Rapport;
using Fred.Business.Valorisation;
using Fred.Entities.Rapport;
using Fred.Entities.Valorisation;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Rapport;

namespace Fred.ImportExport.Business.Rapport
{
    public class RapportFluxManager : AbstractFluxManager
    {
        private readonly IRapportManager rapportManager;
        private readonly IValorisationManager valorisationManager;

        public RapportFluxManager(IFluxManager fluxManager, IRapportManager rapportManager, IValorisationManager valorisationManager)
            : base(fluxManager)
        {
            this.rapportManager = rapportManager;
            this.valorisationManager = valorisationManager;
        }

        public IEnumerable<RapportFesModel> ExportFesRapport(DateTime dateDebut, DateTime dateFin, string codeSociete, string codeEtablissement)
        {
            try
            {
                var filterRapport = new FilterRapportFesExport
                {
                    DateDebut = dateDebut,
                    DateFin = dateFin,
                    CodeSociete = codeSociete
                };

                IEnumerable<RapportEnt> rapportEntities = rapportManager.GetRapportsExportApi(filterRapport);

                if (codeEtablissement != null)
                {
                    rapportEntities = rapportEntities.Where(r => r.CI.EtablissementComptable.Code == codeEtablissement);
                }

                return MapRapports(rapportEntities);
            }
            catch (Exception exception)
            {
                throw new FredIeBusinessException(exception.Message, exception);
            }
        }

        private IEnumerable<RapportFesModel> MapRapports(IEnumerable<RapportEnt> rapportEntities)
        {
            var exportModels = new List<RapportFesModel>();

            foreach (RapportEnt rapportEntity in rapportEntities)
            {
                exportModels.AddRange(MapRapport(rapportEntity, valorisationManager.GetValorisationByRapport(rapportEntity.RapportId)));
            }

            return exportModels;
        }

        private IEnumerable<RapportFesModel> MapRapport(RapportEnt rapport, IList<ValorisationEnt> valorisations)
        {
            const string codeMaterielDefault = "ZZZZZZZZZZ";
            const string codeEvenementTravaille = "T";

            return rapport.ListLignes.Select(l => new RapportFesModel
            {
                Annee = rapport.DateChantier.Year.ToString(),
                Mois = rapport.DateChantier.Month.ToString(),
                Jour = rapport.DateChantier.Day.ToString(),
                DatePiece = string.Empty,
                LibellePiece = string.Empty,
                DebitCredit = string.Empty,
                Montant = 0,
                CodeAffaire = rapport.CI.Code,
                CodeSousAffaire = string.Empty,
                Materiel = l.Materiel != null ? l.Materiel.Code : codeMaterielDefault,
                MontantQuantite = FindMontantQuantite(valorisations, l),
                CodeQuantite = string.Empty,
                Matricule = string.Empty,
                Taux = string.Empty,
                NatureAnalytique = string.Empty,
                EvenementPointage = l.CodeAbsence != null ? MapCodeAbsence(l.CodeAbsence.Code) : codeEvenementTravaille
            });
        }

        private static string MapCodeAbsence(string codeAbs)
        {
            var codeAbsences = new Dictionary<string, string>
            {
                { "01" , "T" },
                { "02" , "CP" },
                { "03" , "AANP" },
                { "04" , "ANP" },
                { "05" , "FERIE" },
                { "06" , "CRTT" },
                { "07" , "RTT/E" },
                { "08" , "MLP" },
                { "09" , "MPP" },
                { "10" , "ATP" },
                { "11" , "T" },
                { "12" , "I" },
                { "13" , "PTNP" },
                { "14" , "MTP" },
                { "15" , "HTJ" },
                { "16" , "FORM" },
                { "17" , "CCONV" },
                { "18" , "CPAR" }
            };
            const string codeAbsenceDefault = "INCONNU";

            string codeAbsModel = codeAbsences.ContainsKey(codeAbs) ? codeAbsences[codeAbs] : codeAbsenceDefault;

            return codeAbsModel;
        }

        private static decimal? FindMontantQuantite(IList<ValorisationEnt> valorisations, RapportLigneEnt ligneRapport)
        {
            if (valorisations == null || valorisations.Count == 0 || ligneRapport == null || ligneRapport.ListRapportLigneTaches.Count == 0)
            {
                return null;
            }

            int tacheId = ligneRapport.ListRapportLigneTaches.FirstOrDefault().Tache.TacheId;
            ValorisationEnt valorisation = valorisations.FirstOrDefault(v => v.TacheId == tacheId);

            return valorisation?.Montant;
        }
    }
}
