using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Moyen.Common;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.Moyen;
using static Fred.Entities.Constantes;

namespace Fred.Business.Moyen.Features
{
    public class RapportMoyenService : IRapportMoyenService
    {
        private readonly IPointageManager pointageManager;
        private readonly IRapportManager rapportManager;

        public RapportMoyenService(IPointageManager pointageManager,
                                   IRapportManager rapportManager)
        {
            this.pointageManager = pointageManager;
            this.rapportManager = rapportManager;
        }

        /// <summary>
        /// Retourne la liste des pointage pour les personnels et les Cis envoyés en paramétres dans la plage des dates définies
        /// </summary>
        /// <param name="ciIdList">La liste des id des Cis</param>
        /// <param name="personnelIdList">La liste des id des personnels</param>
        /// <param name="startDate">Date de début limite</param>
        /// <param name="endDate">Date de fin limite</param>
        /// <returns>Retourne la liste des pointage</returns>
        public IEnumerable<RapportLigneEnt> GetPointageByCisPersonnelsAndDates(IEnumerable<int> ciIdList, IEnumerable<int> personnelIdList, DateTime startDate, DateTime endDate)
        {
            return pointageManager.GetPointageByCisPersonnelsAndDates(ciIdList, personnelIdList, startDate, endDate);
        }

        /// <summary>
        /// Renvoie les heures travaillées par un personnel donné
        /// </summary>
        /// <param name="rapportLignes">La liste des lignes de rapport</param>
        /// <param name="helper">Moyen pointage helper</param>
        /// <returns>Liste des PersonnelPointage</returns>
        public IEnumerable<PersonnelPointage> GetPersonnelPointageSummary(IEnumerable<RapportLigneEnt> rapportLignes, MoyenPointageHelper helper)
        {
            if (rapportLignes.IsNullOrEmpty())
            {
                return new List<PersonnelPointage>();
            }

            IEnumerable<PersonnelPointage> personnelLignes = from r in rapportLignes.Where(w => w.PersonnelId.HasValue && w.DateSuppression is null)
                                                             group r by new
                                                             {
                                                                 r.PersonnelId,
                                                                 DatePointage = r.DatePointage.Date
                                                             }
                into gr
                                                             select new PersonnelPointage
                                                             {
                                                                 PersonnelId = gr.Key.PersonnelId.Value,
                                                                 DatePointage = gr.Key.DatePointage,
                                                                 TotalHeuresAstreintes = gr.Sum(s => helper.GetAstreinteHours(s.ListRapportLigneAstreintes)),
                                                                 TotalHeuresMajorations = gr.Sum(s => s.ListRapportLigneMajorations?.Sum(d => d.HeureMajoration) ?? 0),
                                                                 TotalHeuresTaches = gr.Sum(s => s.ListRapportLigneTaches?.Sum(b => b.HeureTache)) ?? 0
                                                             };

            return personnelLignes;
        }

        /// <summary>
        /// Process de l'affectation à d'un moyen 
        /// </summary>
        /// <param name="request">Update moyen pointage input</param>
        /// <param name="affectationMoyen">L'affectation du moyen</param>
        /// <param name="dateTimeExtendManager">Date time extend manager</param>
        /// <param name="helper">Moyen pointage helper</param>
        /// <param name="affectationMoyenIdList">Liste des affectations Id dont le rapport ligne a ete cree</param>
        /// <returns>Réponse suite au traitement d'une affectation</returns>
        public ProcessAffectationResponse Process(UpdateMoyenPointageInput request, AffectationMoyenEnt affectationMoyen, IDateTimeExtendManager dateTimeExtendManager, MoyenPointageHelper helper, List<AffectationMoyenRapportModel> affectationMoyenIdList)
        {
            try
            {
                AffectationMoyenTypeCode typeCode = (AffectationMoyenTypeCode)affectationMoyen.AffectationMoyenTypeId;
                switch (typeCode)
                {
                    case AffectationMoyenTypeCode.Personnel:
                        return ProcessPersonnelAffectation(request, affectationMoyen, dateTimeExtendManager, helper, affectationMoyenIdList);
                    case AffectationMoyenTypeCode.CI:
                    case AffectationMoyenTypeCode.Parking:
                    case AffectationMoyenTypeCode.Depot:
                    case AffectationMoyenTypeCode.Stock:
                    case AffectationMoyenTypeCode.Reparation:
                    case AffectationMoyenTypeCode.Entretien:
                    case AffectationMoyenTypeCode.Controle:
                        return ProcessCiAffectation(request, affectationMoyen);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Process de l'affectation à un personnel
        /// </summary>
        /// <param name="request">Update moyen pointage input</param>
        /// <param name="affectationMoyen">L'affectation du moyen</param>
        /// <param name="dateTimeExtManager">Date time extend manager</param>
        /// <param name="helper">Moyen pointage helper</param>
        /// <param name="affectationMoyenIdList">Liste des affectations Id dont le rapport ligne a ete cree</param>
        /// <returns>Réponse suite au traitement d'une affectation</returns>
        private ProcessAffectationPersonnelResponse ProcessPersonnelAffectation(UpdateMoyenPointageInput request, AffectationMoyenEnt affectationMoyen, IDateTimeExtendManager dateTimeExtManager, MoyenPointageHelper helper, List<AffectationMoyenRapportModel> affectationMoyenIdList)
        {
            ProcessAffectationPersonnelResponse response = new ProcessAffectationPersonnelResponse();
            IEnumerable<DateTime> workingDays = null;
            if (request.CiAffectationsWorkingDaysDictionnary.ContainsKey(affectationMoyen.AffectationMoyenId))
            {
                workingDays = request.CiAffectationsWorkingDaysDictionnary[affectationMoyen.AffectationMoyenId];
            }
            else
            {
                workingDays = helper.GetWorkingDays(affectationMoyen, request.StartDate, request.EndDate, dateTimeExtManager);
            }

            if (workingDays.IsNullOrEmpty() || !affectationMoyen.PersonnelId.HasValue || affectationMoyen.Personnel == null)
            {
                return response;
            }

            foreach (DateTime date in workingDays)
            {
                List<AffectationMoyenRapportModel> affectationMoyenIdListToAdd = new List<AffectationMoyenRapportModel>();

                IEnumerable<RapportLigneEnt> rapportLignes = GetRapportLigneForPersonnelAffectation(request.AffectationsPersonnelsAndCisRapportLignes, date, affectationMoyen.PersonnelId.Value, affectationMoyen.AffectationMoyenId);
                var listAffectationForTheSamePerson = request.AffectationMoyenList.Where(x => x.AffectationMoyenId != affectationMoyen.AffectationMoyenId
                                                                              && x.PersonnelId == affectationMoyen.PersonnelId.Value && x.AffectationMoyenTypeId == (int)AffectationMoyenTypeCode.Personnel
                                                                              && x.DateDebut.Date <= date && (!x.DateFin.HasValue || x.DateFin.Value.Date >= date));


                rapportLignes.ToList().ForEach(l => listAffectationForTheSamePerson.ToList().ForEach(e =>
                {
                    if (!IsRapportLigneExist(request.AffectationsPersonnelsAndCisRapportLignes, e.AffectationMoyenId, l.CiId, l.DatePointage))
                    {
                        AffectationMoyenRapportModel affectationMoyenRapport = new AffectationMoyenRapportModel { AffectationMoyenId = e.AffectationMoyenId, DatePointage = l.DatePointage, CiId = l.CiId, MaterielId = e.MaterielId };
                        if (!affectationMoyenRapport.IsExist(affectationMoyenIdList))
                        {
                            affectationMoyenIdListToAdd.Add(affectationMoyenRapport);
                        }
                    };
                }
                ));

                if (rapportLignes.IsNullOrEmpty())
                {
                    // Si le personnel a au moins une ligne de rapport, même si ce n'est pas sur cette affectation, la date n'est pas ajouté à la liste d'erreur
                    response.PersonnelPointageError = HandlePersonnelErrorDate(request, affectationMoyen, response, date);
                    continue;
                }

                PersonnelPointage personnelPointage = GetPersonnelPointage(request.PersonnelPointageList, affectationMoyen.PersonnelId.Value, date);

                rapportLignes.ToList().ForEach(lin => ProcessLines(personnelPointage, lin, helper, affectationMoyen.AffectationMoyenId));
                rapportLignes.ToList().ForEach(lin => response.AddRapportLigneToUpdateList(lin));
                if (!affectationMoyenIdListToAdd.IsNullOrEmpty())
                {
                    AddLinesToRapportLigne(affectationMoyenIdListToAdd, rapportLignes, response, affectationMoyenIdList);
                }
            }
            return response;
        }

        private bool IsRapportLigneExist(IEnumerable<RapportLigneEnt> rapportLignes, int affectationMoyenId, int ciId, DateTime datePointage)
        {
            return rapportLignes.Any(a => a.AffectationMoyenId == affectationMoyenId && a.CiId == ciId && a.DatePointage == datePointage && a.DateSuppression is null);
        }

        /// <summary>
        /// Gère l'ajout ou non du personnel et de la date à la liste d'erreur
        /// </summary>
        /// <param name="request">Update moyen pointage input</param>
        /// <param name="affectationMoyen">L'affectation du moyen</param>
        /// <param name="response">ProcessAffectationPersonnelResponse</param>
        /// <param name="date">Date à vérifier</param>
        /// <returns>Erreur de pointage à une date</returns>
        private PersonnelPointageError HandlePersonnelErrorDate(UpdateMoyenPointageInput request, AffectationMoyenEnt affectationMoyen, ProcessAffectationPersonnelResponse response, DateTime date)
        {
            if (!request.AffectationsPersonnelsAndCisRapportLignes.Any(r => r.PersonnelId == affectationMoyen.PersonnelId.Value && r.DatePointage.Date == date.Date && !r.DateSuppression.HasValue
                                                                                    && (r.HeureNormale > 0 || r.HeureAbsence > 0 || r.ListeMajorations?.Count > 0 || r.ListePrimes?.Count > 0
                                                                                    || r.HeuresTotalAstreintes > 0 || r.ListRapportLigneAstreintes.Count > 0 || r.ListRapportLigneMajorations.Count > 0)))
            {
                response.AddPersonnelErrorDate(affectationMoyen.Personnel, date);
            }

            return response.PersonnelPointageError;
        }

        /// <summary>
        /// Ajout des lignes manquants dans le rapport ligne 
        /// </summary>
        /// <param name="affectationMoyenIdListToAdd">List des affectations sans ligne de rapport</param>
        /// <param name="lines">Lignes de rapport</param>
        /// <param name="response">Resultat du process</param>
        /// <param name="affectationMoyenIdList">Affectation moyen id list</param>
        private void AddLinesToRapportLigne(List<AffectationMoyenRapportModel> affectationMoyenIdListToAdd, IEnumerable<RapportLigneEnt> lines, ProcessAffectationPersonnelResponse response, List<AffectationMoyenRapportModel> affectationMoyenIdList)
        {
            foreach (RapportLigneEnt line in lines)
            {
                List<AffectationMoyenRapportModel> affectationMoyenIdListNotAdd = affectationMoyenIdListToAdd.Where(w => w.CiId == line.CiId && w.DatePointage == line.DatePointage).ToList();
                affectationMoyenIdListNotAdd.ToList().ForEach(t =>
                {
                    List<RapportLigneTacheEnt> listRapportLigneTache = new List<RapportLigneTacheEnt>();
                    line.ListRapportLigneTaches.ToList().ForEach(lrtache => FillListRapportLigneTache(listRapportLigneTache, lrtache));
                    RapportLigneEnt rapportLigneToAdd = new RapportLigneEnt
                    {
                        PointageId = line.PointageId,
                        RapportLigneId = 0,
                        RapportId = line.RapportId,
                        CiId = line.CiId,
                        AffectationMoyenId = t.AffectationMoyenId,
                        PrenomNomTemporaire = line.PrenomNomTemporaire,
                        MaterielId = t.MaterielId,
                        HeureNormale = line.HeureNormale,
                        HeureMajoration = line.HeureMajoration,
                        CodeMajorationId = line.CodeMajorationId,
                        CodeAbsenceId = line.CodeAbsenceId,
                        CodeAbsence = line.CodeAbsence,
                        HeureAbsence = line.HeureAbsence,
                        NumSemaineIntemperieAbsence = line.NumSemaineIntemperieAbsence,
                        CodeDeplacementId = line.CodeDeplacementId,
                        CodeZoneDeplacementId = line.CodeZoneDeplacementId,
                        DeplacementIV = line.DeplacementIV,
                        MaterielMarche = line.MaterielMarche,
                        MaterielArret = line.MaterielArret,
                        MaterielPanne = line.MaterielPanne,
                        MaterielIntemperie = line.MaterielIntemperie,
                        MaterielNomTemporaire = line.MaterielNomTemporaire,
                        AvecChauffeur = line.AvecChauffeur,
                        IsDeleted = line.IsDeleted,
                        HeuresMachine = line.HeuresMachine,
                        DatePointage = line.DatePointage,
                        LotPointageId = line.LotPointageId,
                        ListRapportLigneTaches = listRapportLigneTache,
                        IsGenerated = true
                    };

                    response.AddRapportLineToCreateList(rapportLigneToAdd);
                    AffectationMoyenRapportModel affectationMoyenCreated = new AffectationMoyenRapportModel { AffectationMoyenId = t.AffectationMoyenId, DatePointage = line.DatePointage, CiId = line.CiId, MaterielId = t.MaterielId };
                    affectationMoyenIdList.Add(affectationMoyenCreated);
                });
            }
        }

        private void FillListRapportLigneTache(List<RapportLigneTacheEnt> rapportLignetacheList, RapportLigneTacheEnt rapportLigneTache)
        {
            rapportLignetacheList.Add(new RapportLigneTacheEnt { HeureTache = rapportLigneTache.HeureTache, RapportLigneId = 0, RapportLigneTacheId = 0, TacheId = rapportLigneTache.TacheId });
        }

        /// <summary>
        /// Procces des lignes de rapport
        /// </summary>
        /// <param name="personnelPointage">Personnel pointage</param>
        /// <param name="line">Lignes de rapport</param>
        /// <param name="helper">Moyen helper</param>
        /// <param name="affectationMoyenId">Affectation moyen id</param>
        private void ProcessLines(PersonnelPointage personnelPointage, RapportLigneEnt line, MoyenPointageHelper helper, int affectationMoyenId)
        {
            if (line == null)
            {
                return;
            }
            line.HeuresMachine = helper.CalculHeureMachine(personnelPointage, line);
            line.AffectationMoyenId = affectationMoyenId;
        }

        /// <summary>
        /// Process de l'affectation du moyen à un Ci
        /// </summary>
        /// <param name="request">Update moyen pointage input</param>
        /// <param name="affectationMoyen">Affectation moyen</param>
        /// <returns>Réponse suite au traitement d'une affectation à un Ci</returns>
        private ProcessAffectationCiResponse ProcessCiAffectation(UpdateMoyenPointageInput request, AffectationMoyenEnt affectationMoyen)
        {
            int? ciId = null;
            ProcessAffectationCiResponse response = new ProcessAffectationCiResponse();

            if (affectationMoyen.AffectationMoyenTypeId == (int)AffectationMoyenTypeCode.CI)
            {
                ciId = affectationMoyen.CiId;
            }
            else
            {
                int societeId = affectationMoyen.Materiel.SocieteId;
                int? etablissementComptableId = affectationMoyen.Materiel.EtablissementComptableId;
                string ciCode = affectationMoyen.TypeAffectation.CiCode;

                ciId = GetCiIdByCodeAndSocieteIdAndEtablissementId(request.RestitutionAndMaintenanceCiList, societeId, etablissementComptableId, ciCode);
            }

            if (!ciId.HasValue)
            {
                return response;
            }

            IEnumerable<DateTime> ciAffectationDates = request.CiAffectationsWorkingDaysDictionnary[affectationMoyen.AffectationMoyenId];
            foreach (DateTime date in ciAffectationDates)
            {
                RapportLigneEnt ligne = GetRapportLigneForCiAffectation(request.AffectationsPersonnelsAndCisRapportLignes, affectationMoyen.AffectationMoyenId, date, ciId: ciId.Value);

                if (ligne != null)
                {
                    ligne.HeuresMachine = Commun.Constantes.DefaultHourMachine;
                }
                else
                {
                    AddNewLineToResponse(request, affectationMoyen, ciId, response, date);
                }
            }
            return response;
        }

        private void AddNewLineToResponse(UpdateMoyenPointageInput request, AffectationMoyenEnt affectationMoyen, int? ciId, ProcessAffectationCiResponse response, DateTime date)
        {
            RapportEnt rapport = request.CiAffectationRapportList.FirstOrDefault(o => o.DateChantier.Date == date && o.CiId == ciId);
            IEnumerable<RapportLigneEnt> rapportLigne = request.AffectationsPersonnelsAndCisRapportLignes?.Where(r => (r.PersonnelId == affectationMoyen.PersonnelId || r.PersonnelId == null)
                                                          && r.DatePointage.Date == date.Date
                                                          && (r.AffectationMoyenId == affectationMoyen.AffectationMoyenId || r.AffectationMoyenId == null)
                                                          && !r.DateSuppression.HasValue);

            if (rapport == null)
            {
                rapport = rapportManager.AddRapportMaterialType(new RapportEnt
                {
                    CiId = ciId.Value,
                    DateChantier = date.Date,
                    RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key,
                    TypeStatutRapport = TypeRapportStatut.Material
                });

                request.CiAffectationRapportList.ToList().Add(rapport);
            }

            if (rapport != null)
            {
                if (rapportLigne.IsNullOrEmpty())
                {
                    RapportLigneEnt ligneToAdd = GetNewRapportLigne(affectationMoyen.PersonnelId, ciId.Value, rapport.RapportId, date, affectationMoyen.AffectationMoyenId);
                    response.AddRapportLineToCreateList(ligneToAdd);
                }
                else
                {
                    IEnumerable<RapportLigneEnt> existingLines = request.AffectationsPersonnelsAndCisRapportLignes.Where(q => q.CiId == ciId && q.AffectationMoyenId == affectationMoyen.AffectationMoyenId && q.DatePointage == date);
                    if (!existingLines.Any())
                    {
                        RapportLigneEnt ligneToAdd = GetNewRapportLigne(affectationMoyen.PersonnelId, ciId.Value, rapport.RapportId, date, affectationMoyen.AffectationMoyenId);
                        response.AddRapportLineToCreateList(ligneToAdd);
                    }
                }
            }
        }

        /// <summary>
        /// Obtention de la ligne du rapport en fonction de la date , ci et l'id de l'affectation du moyen 
        /// </summary>
        /// <param name="rapportLignes">Les lignes de rapport</param>
        /// <param name="affectationMoyenId">Affectation moyen id</param>
        /// <param name="date">Date de pointage</param>
        /// <param name="ciId">Id du ci</param>
        /// <returns>Rapport ligne</returns>
        private RapportLigneEnt GetRapportLigneForCiAffectation(IEnumerable<RapportLigneEnt> rapportLignes, int affectationMoyenId, DateTime date, int ciId)
        {
            return rapportLignes?.FirstOrDefault(r => r.CiId == ciId &&
                                                      r.AffectationMoyenId == affectationMoyenId &&
                                                      r.DatePointage.Year == date.Year &&
                                                      r.DatePointage.Month == date.Month &&
                                                      r.DatePointage.Day == date.Day &&
                                                      !r.DateSuppression.HasValue &&
                                                      (r.HeureNormale > 0 ||
                                                       r.HeureAbsence > 0 ||
                                                       r.HeureMajoration > 0 ||
                                                       r.HeuresTotalAstreintes > 0 ||
                                                       r.ListRapportLigneAstreintes.Count > 0 ||
                                                       r.ListRapportLigneMajorations.Count > 0 ||
                                                       r.HeuresMachine > 0)
                                                 );
        }

        /// <summary>
        /// Obtention des ligne du rapport en fonction de la date , ci ou personnel et l'id de l'affectation du moyen 
        /// </summary>
        /// <param name="rapportLignes">Les lignes de rapport</param>
        /// <param name="date">Date de pointage</param>
        /// <param name="personnelId">Id du personnel</param>
        /// <param name="affectationMoyenId">Id affectation moyen</param>
        /// <returns>Rapport ligne</returns>
        private IEnumerable<RapportLigneEnt> GetRapportLigneForPersonnelAffectation(IEnumerable<RapportLigneEnt> rapportLignes, DateTime date, int personnelId, int affectationMoyenId)
        {
            return rapportLignes?.Where(r => (r.PersonnelId == personnelId || r.PersonnelId == null) &&
                                             r.DatePointage.Year == date.Year &&
                                             r.DatePointage.Month == date.Month &&
                                             r.DatePointage.Day == date.Day &&
                                             (r.AffectationMoyenId == affectationMoyenId || r.AffectationMoyenId == null) &&
                                             (r.PersonnelId.HasValue || r.AffectationMoyenId.HasValue) &&
                                             !r.DateSuppression.HasValue);
        }

        /// <summary>
        /// Retounre le Personnel pointage en fonction de l'id du personnel et la date
        /// </summary>
        /// <param name="pointages">La liste des personnel pointage</param>
        /// <param name="personnelId">Le personnel id</param>
        /// <param name="date">La date de pointage</param>
        /// <returns>Personnel pointage</returns>
        private PersonnelPointage GetPersonnelPointage(IEnumerable<PersonnelPointage> pointages, int personnelId, DateTime date)
        {
            return pointages?.FirstOrDefault(p => p.PersonnelId == personnelId &&
                                                  p.DatePointage.Year == date.Year &&
                                                  p.DatePointage.Month == date.Month &&
                                                  p.DatePointage.Day == date.Day);
        }

        /// <summary>
        /// Retourne l'objet Ci en utilisant le code, la societedId l'établissement comptable id.
        /// </summary>
        /// <param name="ciList">La liste des Cis</param>
        /// <param name="societedId">Societe Id</param>
        /// <param name="etablissementComptableId">Etablissement comptable id</param>
        /// <param name="ciCode">Code du ci</param>
        /// <returns>Ci id , null si il n'existes aucun Ci avec le filtre </returns>
        private int? GetCiIdByCodeAndSocieteIdAndEtablissementId(IEnumerable<CIEnt> ciList, int societedId, int? etablissementComptableId, string ciCode)
        {
            CIEnt ci = ciList.FirstOrDefault(
                c => c.Code == ciCode
                && c.SocieteId == societedId
                && c.EtablissementComptableId == etablissementComptableId
                );

            if (ci != null)
            {
                return ci.CiId;
            }

            return null;
        }

        /// <summary>
        /// Renvoie une nouvelle ligne de rapport
        /// </summary>
        /// <param name="personnelId">Personnel id</param>
        /// <param name="ciId">Id du Ci</param>
        /// <param name="rapportId">Rapport id</param>
        /// <param name="dateChantiers">Date de chantier</param>
        /// <param name="affectationMoyenId">Affectation moyen id</param>
        /// <returns>Nouvelle ligne de rapport</returns>
        private RapportLigneEnt GetNewRapportLigne(int? personnelId, int ciId, int rapportId, DateTime dateChantiers, int affectationMoyenId)
        {
            RapportLigneEnt ligne = new RapportLigneEnt();

            ligne.PersonnelId = personnelId;
            ligne.CiId = ciId;
            ligne.DatePointage = dateChantiers;
            ligne.RapportId = rapportId;
            ligne.HeuresMachine = Commun.Constantes.DefaultHourMachine;
            ligne.ListRapportLigneAstreintes = new List<RapportLigneAstreinteEnt>();
            ligne.ListRapportLigneMajorations = new List<RapportLigneMajorationEnt>();
            ligne.ListRapportLignePrimes = new List<RapportLignePrimeEnt>();
            ligne.ListRapportLigneTaches = new List<RapportLigneTacheEnt>();
            ligne.AffectationMoyenId = affectationMoyenId;
            ligne.DateCreation = DateTime.UtcNow;

            return ligne;
        }

        /// <summary>
        /// Enregistrement des modifications au niveau du pointage
        /// </summary>
        /// <param name="lines">La liste des lignes de rapport</param>
        public void AddOrUpdatePointage(IEnumerable<RapportLigneEnt> lines)
        {
            pointageManager.AddOrUpdateRapportLigneList(lines);
        }

        /// <summary>
        /// Enregistrement des modifications au niveau du pointage
        /// </summary>
        /// <param name="linesToCreate">La liste des lignes de rapport à créer</param>
        /// <param name="linesToUpdate">La liste des lignes de rapport à modifier</param>
        public void UpdatePointageMoyen(IEnumerable<RapportLigneEnt> linesToCreate, IEnumerable<RapportLigneEnt> linesToUpdate)
        {
            try
            {
                List<RapportLigneEnt> pointageList = new List<RapportLigneEnt>();

                if (!linesToUpdate.IsNullOrEmpty())
                {
                    pointageList.AddRange(linesToUpdate);
                }

                if (!linesToCreate.IsNullOrEmpty())
                {
                    pointageList.AddRange(linesToCreate);
                }

                if (!pointageList.IsNullOrEmpty())
                {
                    pointageManager.AddOrUpdateRapportLigneList(pointageList);
                }
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
