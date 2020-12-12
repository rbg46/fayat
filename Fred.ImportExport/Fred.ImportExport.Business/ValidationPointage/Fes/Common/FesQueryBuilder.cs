using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fred.Entities.Rapport;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers;
using Fred.Web.Shared.Models.ValidationPointage;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.Common
{
    /// <summary>
    /// Builder qui construit les requetes du controle vrac
    /// </summary>
    public class FesQueryBuilder : IFesQueryBuilder
    {
        public const string InsertPointageClause = "INSERT INTO INTEI.SRAPFREP(NOLOT, NOAFF, CDEVT,  CDABS, MATRI, CDDEP, VOYDET, HNM, H100, HABS, NSINT, RAPAA, RAPMM, RAPJJ, SOCIET, TYPERP, NOAFFP, NOLOP, RAPMOD, ZONE, CDMAJ, CUSER) VALUES ";
        public const string InsertPrimeClause = "INSERT INTO INTEI.PRIMNEWR(PRJOUP, PRMOIP, PRANNP, PRAFF, PRMATR, PRANNC, PRMOIC, PRPRIM, PRPRIL, PRQTE, PRUNI, NOLOP) VALUES ";

        private const string InsertPointageQuery = InsertPointageClause + "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}')";
        private const string InsertPrimeQuery = InsertPrimeClause + "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')";
        private readonly List<InsertQueryPrimeParametersModel> listPrimeParameters = new List<InsertQueryPrimeParametersModel>();
        private readonly List<InsertQueryPointageParametersModel> listPointageParameters = new List<InsertQueryPointageParametersModel>();

        #region Récupération des requêtes AS400

        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400
        /// </summary>
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="periode">Période choisie</param>       
        /// <param name="rapportLignes">Liste des pointages FRED à contrôler</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400</returns>
        public IEnumerable<QueryInfo> BuildAnaelInsertsQueries(ValidationPointageContextData globalData, DateTime periode, ICollection<RapportLigneEnt> rapportLignes, out List<InsertQueryPrimeParametersModel> listPrimeParameters, out List<InsertQueryPointageParametersModel> listPointageParameters)
        {
            var result = new List<QueryInfo>();

            var nomUtilisateur = globalData.NomUtilisateur;
            if (nomUtilisateur.Length > 10)
            {
                nomUtilisateur = globalData.NomUtilisateur.Substring(0, 10);
            }

            if (rapportLignes.Count > 0)
            {

                //analysers
                var pointageTypeEtudeAnalyser = new PointageTypeEtudeAnalyser();
                var pointageIpdAnalyzer = new PointageIpdAnalyzer();

                //Pointages
                var allPointages = rapportLignes.ToList();

                // Pointages Type Etude
                var pointageTypeEtudeResults = pointageTypeEtudeAnalyser.GetPointageTypeEtudeInfos(allPointages);
                result.Add(CreateQueryOrComment(false, "-- Insertion des Pointages type etude"));
                var insertsTypeEtudes = InsertPointagesTypeEtudes(globalData, periode, nomUtilisateur, allPointages, pointageTypeEtudeResults);
                result.AddRange(insertsTypeEtudes);

                // Pointages ipd             
                var pointageIpdResults = pointageIpdAnalyzer.GetPointageIpdInfos(allPointages);

                result.Add(CreateQueryOrComment(false, "-- Insertion des Pointages IPD "));
                var insertsTypeIpds = InsertPointageIpds(globalData, periode, nomUtilisateur, allPointages, pointageTypeEtudeResults, pointageIpdResults);
                result.AddRange(insertsTypeIpds);

                // Autres Pointages
                result.Add(CreateQueryOrComment(false, "-- Insertion des autres Pointages"));
                var insertsOthers = InsertOtherPointages(globalData, periode, rapportLignes, nomUtilisateur, pointageTypeEtudeAnalyser, pointageIpdAnalyzer);
                result.AddRange(insertsOthers);

                // Astreintes
                result.Add(CreateQueryOrComment(false, "-- Insertion des astreintes"));
                var astreinteAnalyser = new AstreinteAnalyser();

                var astreinteAnalyserResults = astreinteAnalyser.GetAstreinteInfos(rapportLignes.ToList());

                foreach (var astreinteAnalyserResult in astreinteAnalyserResults)
                {
                    var insertAstreinteSQLRequests = BuildInsertAstreinteSQLRequest(astreinteAnalyserResult, globalData.JobId, periode, nomUtilisateur, globalData.CodeSocietePaye);
                    result.Add(CreateQueryOrComment(true, insertAstreinteSQLRequests));
                    FillListAstreintesParameters(astreinteAnalyserResult, globalData.JobId, periode, nomUtilisateur, globalData.CodeSocietePaye);
                }

                //  Majorations
                result.Add(CreateQueryOrComment(false, "-- Insertion des Majorations"));
                var majorationAnalyzer = new MajorationAnalyzer();

                var majorationAnalyzerResults = majorationAnalyzer.GetMajorationInfos(rapportLignes.ToList());

                foreach (var majorationResult in majorationAnalyzerResults)
                {
                    var insertMajorationSQLRequests = BuildInsertMajorationSQLRequest(majorationResult, globalData.JobId, periode, nomUtilisateur, globalData.CodeSocietePaye);
                    result.Add(CreateQueryOrComment(true, insertMajorationSQLRequests));
                    FillListMajorationParameters(majorationResult, globalData.JobId, periode, nomUtilisateur, globalData.CodeSocietePaye);
                }

                //  Primes
                result.Add(CreateQueryOrComment(false, "-- Insertion des Prime"));
                var primeAnalyser = new PrimeAnalyser();

                var primeResults = primeAnalyser.GetPrimesInfos(rapportLignes.ToList());

                foreach (var primeResult in primeResults)
                {
                    var insertMajorationSQLRequests = BuildInsertPrimeSQLRequest(primeResult, globalData.JobId, periode);
                    result.Add(CreateQueryOrComment(true, insertMajorationSQLRequests, false));
                    FillListPrimeParameters(primeResult, globalData.JobId, periode);
                }
            }

            listPrimeParameters = this.listPrimeParameters;
            listPointageParameters = this.listPointageParameters;

            return result;
        }


        private List<QueryInfo> InsertPointagesTypeEtudes(ValidationPointageContextData globalData, DateTime periode, string nomUtilisateur, List<RapportLigneEnt> allPointages, List<PointageTypeEtudeResult> pointageTypeEtudeResults)
        {
            var result = new List<QueryInfo>();
            foreach (var pointageResult in pointageTypeEtudeResults)
            {
                var codeDeplacement = GetBiggestCodeDeplacement(allPointages, pointageResult.RapportLigne);

                var insertPointageSQLRequests = BuildInsertPointageTypeEtudeSQLRequests(pointageResult, globalData.JobId, periode, nomUtilisateur, globalData.CodeSocietePaye, codeDeplacement);

                result.Add(CreateQueryOrComment(true, insertPointageSQLRequests));
            }
            return result;
        }

        private List<QueryInfo> InsertPointageIpds(ValidationPointageContextData globalData, DateTime periode, string nomUtilisateur, List<RapportLigneEnt> allPointages, List<PointageTypeEtudeResult> pointageTypeEtudeResults, List<PointageIpdResult> pointageIpdResults)
        {
            var result = new List<QueryInfo>();
            foreach (var pointageIpdResult in pointageIpdResults)
            {
                var pointageIpd = InsertPointageIpd(globalData, periode, nomUtilisateur, allPointages, pointageTypeEtudeResults, pointageIpdResult);
                if (pointageIpd != null)
                {
                    result.Add(pointageIpd);
                }
            }
            return result;
        }

        private QueryInfo InsertPointageIpd(ValidationPointageContextData globalData, DateTime periode, string nomUtilisateur, List<RapportLigneEnt> allPointages, List<PointageTypeEtudeResult> pointageTypeEtudeResults, PointageIpdResult pointageResult)
        {
            var typeEtudesRapportLignes = pointageTypeEtudeResults.Select(r => r.RapportLigne).ToList();
            //je ne remet pas le rapport lignes dans le resultat des ipd puisqu'il est deja dans les type etude
            if (!typeEtudesRapportLignes.Contains(pointageResult.RapportLigne))
            {
                var codeDeplacement = GetBiggestCodeDeplacement(allPointages, pointageResult.RapportLigne);

                // si j'ai deja un type etude, ou j'ai mis l'ipd le plus grand alors je ne remet pas l'ipd
                if (AlreadyContainedInTypeEtudeResultsOnSameDay(typeEtudesRapportLignes, pointageResult.RapportLigne))
                {
                    codeDeplacement = "00";
                }
                var ipdRequest = BuildInsertPointageIpdSQLRequests(pointageResult, globalData.JobId, periode, nomUtilisateur, globalData.CodeSocietePaye, codeDeplacement);

                return CreateQueryOrComment(true, ipdRequest);
            }
            return null;
        }

        private List<QueryInfo> InsertOtherPointages(ValidationPointageContextData globalData, DateTime periode, ICollection<RapportLigneEnt> rapportLignes, string nomUtilisateur, PointageTypeEtudeAnalyser pointageTypeEtudeAnalyser, PointageIpdAnalyzer pointageIpdAnalyzer)
        {
            var result = new List<QueryInfo>();
            var otherPointages = rapportLignes.Where(rl => !pointageIpdAnalyzer.MustApplyIpdRules(rapportLignes.ToList(), rl) && !pointageTypeEtudeAnalyser.MustApplyCiTypeEtudeRules(rl)).ToList();

            foreach (var pointage in otherPointages)
            {
                var ipdRequest = BuildInsertPointageSQLRequest(pointage, globalData.JobId, periode, nomUtilisateur, globalData.CodeSocietePaye);
                result.Add(CreateQueryOrComment(true, ipdRequest));
                FillListPointageParameters(pointage, globalData.JobId, periode, nomUtilisateur, globalData.CodeSocietePaye);
            }
            return result;
        }

        private bool AlreadyContainedInTypeEtudeResultsOnSameDay(IEnumerable<RapportLigneEnt> allPointages, RapportLigneEnt rapportLigne)
        {
            var ids = allPointages.Where(p =>
                                            p.DatePointage.Year == rapportLigne.DatePointage.Year &&
                                            p.DatePointage.Month == rapportLigne.DatePointage.Month &&
                                            p.DatePointage.Day == rapportLigne.DatePointage.Day)
                .Select(p => p.RapportLigneId).ToList();
            return ids.Count > 0;
        }

        public string GetBiggestCodeDeplacement(IEnumerable<RapportLigneEnt> allPointages, RapportLigneEnt rapportLigne)
        {
            var pointageIpdAnalyzer = new PointageIpdAnalyzer();
            var result = pointageIpdAnalyzer.GetBiggestCodeDeplacement(allPointages.ToList(), rapportLigne);
            return result;
        }

        private QueryInfo CreateQueryOrComment(bool isQuery, string queryOrComment, bool isPointage = true)
        {
            Debug.WriteLine(queryOrComment);
            return new QueryInfo()
            {
                Comment = !isQuery ? queryOrComment : string.Empty,
                Query = isQuery ? queryOrComment : string.Empty,
                IsPointage = isPointage
            };
        }

        private string BuildInsertPointageTypeEtudeSQLRequests(PointageTypeEtudeResult pointageTypeEtude, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye, string codeDeplacementOrBiggestIpd)
        {
            var result = string.Empty;

            if (pointageTypeEtude.IsFistResult)
            {
                result = BuildFirstInsertPointageOfTypeEtudeSQLRequest(pointageTypeEtude, jobId, periode, nomUtilisateur, codeSocietePaye, codeDeplacementOrBiggestIpd);
                FillListPointageEtudeParameters(pointageTypeEtude, jobId, periode, nomUtilisateur, codeSocietePaye, codeDeplacementOrBiggestIpd);
            }
            else
            {
                result = BuildPointageOnlyForTacheSQLRequest(pointageTypeEtude, jobId, periode, nomUtilisateur, codeSocietePaye);
                FillListPointageOnlyTacheParameters(pointageTypeEtude, jobId, periode, nomUtilisateur, codeSocietePaye);
            }

            return result;
        }



        private string BuildInsertPointageIpdSQLRequests(PointageIpdResult pointage, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye, string codeDeplacementOrBiggestIpd)
        {
            var result = string.Empty;

            if (pointage.IsBigestIpd)
            {
                result = BuildInsertPointageBiggestIpdRequest(pointage.RapportLigne, jobId, periode, nomUtilisateur, codeSocietePaye, codeDeplacementOrBiggestIpd);
                FillListPointageIpdParameters(pointage.RapportLigne, jobId, periode, nomUtilisateur, codeSocietePaye, codeDeplacementOrBiggestIpd);
            }
            else
            {
                result = BuildInsertPointageWithoutIpdRequest(pointage.RapportLigne, jobId, periode, nomUtilisateur, codeSocietePaye);
                FillListPointageWithoutIpdParameters(pointage.RapportLigne, jobId, periode, nomUtilisateur, codeSocietePaye);
            }
            return result;
        }

        private string BuildInsertPointageBiggestIpdRequest(RapportLigneEnt pointage, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye, string codeDeplacementOrBiggestIpd)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            return string.Format(InsertPointageQuery,
                           numLot,
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),
                            "T",// o CDEVT
                           pointage.CodeAbsence?.Code.Trim(),
                           pointage.Personnel.Matricule.Trim(),
                           codeDeplacementOrBiggestIpd, // 00 = sans code déplacement. 
                           Convert.ToInt32(pointage.DeplacementIV),
                           pointage.HeureNormale.ToString().Replace(",", "."),
                           pointage.HeureMajoration.ToString().Replace(",", "."),
                           pointage.HeureAbsence.ToString().Replace(",", "."),
                           Convert.ToInt32(pointage.NumSemaineIntemperieAbsence),
                           dateRapport.Year,
                           dateRapport.Month,
                           dateRapport.Day,
                           codeSocietePaye, // pointage.Ci.SocieteData.Code,
                           "R",
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), // "NOAFFP" : affaire prestation. Pour l'instant Code CI
                           jobId.PadLeft(10, ' '),
                           1, // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                           pointage.CodeZoneDeplacement?.Code,
                           pointage.CodeMajoration?.Code,
                           nomUtilisateur);
        }

        private string BuildInsertPointageWithoutIpdRequest(RapportLigneEnt pointage, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            return string.Format(InsertPointageQuery,
                           numLot,
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),
                            "T",// o CDEVT
                           pointage.CodeAbsence?.Code.Trim(),
                           pointage.Personnel.Matricule.Trim(),
                           "00", // 00 = sans code déplacement. 
                           Convert.ToInt32(pointage.DeplacementIV),
                           pointage.HeureNormale.ToString().Replace(",", "."),
                           pointage.HeureMajoration.ToString().Replace(",", "."),
                           pointage.HeureAbsence.ToString().Replace(",", "."),
                           Convert.ToInt32(pointage.NumSemaineIntemperieAbsence),
                           dateRapport.Year,
                           dateRapport.Month,
                           dateRapport.Day,
                           codeSocietePaye, // pointage.Ci.SocieteData.Code,
                           "R",
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), // "NOAFFP" : affaire prestation. Pour l'instant Code CI
                           jobId.PadLeft(10, ' '),
                           1, // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                           string.Empty,
                           pointage.CodeMajoration?.Code,
                           nomUtilisateur);
        }



        private string BuildFirstInsertPointageOfTypeEtudeSQLRequest(PointageTypeEtudeResult pointageTypeEtudeResult, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye, string codeDeplacementOrBiggestIpd)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointageTypeEtudeResult.RapportLigne.Rapport.DateChantier.ToLocalTime();

            return string.Format(InsertPointageQuery,
                           numLot,
                           pointageTypeEtudeResult.Code.Trim().PadLeft(6, ' '),
                           "T",// o CDEVT
                           pointageTypeEtudeResult.RapportLigne.CodeAbsence?.Code.Trim(),
                           pointageTypeEtudeResult.RapportLigne.Personnel.Matricule.Trim(),
                           codeDeplacementOrBiggestIpd,
                           Convert.ToInt32(pointageTypeEtudeResult.RapportLigne.DeplacementIV),
                           pointageTypeEtudeResult.Quantite, //pointage.HeureNormale.ToString().Replace(",", "."),
                           pointageTypeEtudeResult.RapportLigne.HeureMajoration.ToString().Replace(",", "."),
                           pointageTypeEtudeResult.RapportLigne.HeureAbsence.ToString().Replace(",", "."),
                           Convert.ToInt32(pointageTypeEtudeResult.RapportLigne.NumSemaineIntemperieAbsence),
                           dateRapport.Year,
                           dateRapport.Month,
                           dateRapport.Day,
                           codeSocietePaye, // pointage.Ci.SocieteData.Code,
                           "R",
                           pointageTypeEtudeResult.Code.Trim().PadLeft(6, ' '),
                           jobId.PadLeft(10, ' '),
                           1, // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                           pointageTypeEtudeResult.RapportLigne.CodeZoneDeplacement?.Code,
                           pointageTypeEtudeResult.RapportLigne.CodeMajoration?.Code,
                           nomUtilisateur);
        }


        private string BuildPointageOnlyForTacheSQLRequest(PointageTypeEtudeResult pointageTypeEtudeResult, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointageTypeEtudeResult.RapportLigne.Rapport.DateChantier.ToLocalTime();

            return string.Format(InsertPointageQuery,
                           numLot,
                           pointageTypeEtudeResult.Code.Trim().PadLeft(6, ' '),
                           "T",// o CDEVT
                           string.Empty,
                           pointageTypeEtudeResult.RapportLigne.Personnel.Matricule.Trim(),
                           "00",
                           0,
                           pointageTypeEtudeResult.Quantite.Replace(",", "."),
                           0,
                           0,
                           0,
                           dateRapport.Year,
                           dateRapport.Month,
                           dateRapport.Day,
                           codeSocietePaye, // pointage.Ci.SocieteData.Code,
                           "R",
                           pointageTypeEtudeResult.Code.Trim().PadLeft(6, ' '), // "NOAFFP" : affaire prestation. Pour l'instant Code CI
                           jobId.PadLeft(10, ' '),
                           1, // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                           string.Empty,//pointage.CodeZoneDeplacement?.Code,
                           string.Empty,//pointage.CodeMajoration?.Code,
                           nomUtilisateur);
        }

        private string BuildInsertPointageSQLRequest(RapportLigneEnt pointage, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            return string.Format(InsertPointageQuery,
                           numLot,
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),
                           "T",// o CDEVT
                           pointage.CodeAbsence?.Code.Trim(),
                           pointage.Personnel.Matricule.Trim(),
                           pointage.CodeDeplacement != null ? pointage.CodeDeplacement.Code.Trim() : "00", // 00 = sans code déplacement. 
                           Convert.ToInt32(pointage.DeplacementIV),
                           pointage.HeureNormale.ToString().Replace(",", "."),
                           pointage.HeureMajoration.ToString().Replace(",", "."),
                           pointage.HeureAbsence.ToString().Replace(",", "."),
                           Convert.ToInt32(pointage.NumSemaineIntemperieAbsence),
                           dateRapport.Year,
                           dateRapport.Month,
                           dateRapport.Day,
                           codeSocietePaye, // pointage.Ci.SocieteData.Code,
                           "R",
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), // "NOAFFP" : affaire prestation. Pour l'instant Code CI
                           jobId.PadLeft(10, ' '),
                           1, // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                           pointage.CodeZoneDeplacement?.Code,
                           pointage.CodeMajoration?.Code,
                           nomUtilisateur);
        }

        private string BuildInsertAstreinteSQLRequest(AstreinteResult astreinteResult, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            RapportLigneEnt pointage = astreinteResult.RapportLigne;
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            return string.Format(InsertPointageQuery,
                           numLot,//NOLOT
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),// o NOAFF  code du CI pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           astreinteResult.Code.Trim(),// o CDEVT  code de la sortie d’astreinte dans la table FRED_CODE_ASTREINTE liée à la sortie d’astreinte
                           string.Empty,// o    CDABS  toujours la valeur ‘’
                           pointage.Personnel.Matricule.Trim(), // o    MATRI  matricule du personnel pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           "00",// o    CDDEP  toujours la valeur ‘00’
                           0,// o   VOYDET  toujours la valeur ‘0’
                           astreinteResult.Quantite,// o   HNM  la somme des sorties d’astreintes de la journée sur le CI pour un personnel (total du nombre d’heure entres les champs DateFinAstreinte et DateDebutAstreinte de la table FRED_RAPPORT_LIGNE_ASTREINTE d’un rapport ligne)
                           0,//o    H100  toujours la valeur ‘0’
                           0,// o   H100  toujours la valeur ‘0
                           0,   // o    NSINT  toujours la valeur ‘0’                       
                           dateRapport.Year, //o    RAPAA  année de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           dateRapport.Month, //o   RAPMM  mois de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           dateRapport.Day, //o RAPJJ  jour de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           codeSocietePaye, // o    SOCIET  code paye de la société du personnel pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           "R", //o    TYPERP  toujours la valeur ‘R’
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), //  NOAFFP  code du CI pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           jobId.PadLeft(10, ' '),// o NOLOP  numéro de lot du rapport de pointage
                           1, //o   RAPMOD  toujours la valeur ‘1’
                           string.Empty,//o ZONE  toujours la valeur ‘’
                           string.Empty,//o CDMAJ  toujours la valeur ‘’
                           nomUtilisateur);//o  CUSER  matricule de l’utilisateur exécutant la remontée ou le contrôle vrac
        }

        private string BuildInsertMajorationSQLRequest(MajorationResult majorationResult, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            RapportLigneEnt pointage = majorationResult.RapportLigne;
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            return string.Format(InsertPointageQuery,
                           numLot,//NOLOT
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),// o NOAFF  code du CI pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           majorationResult.Code.Trim(),// o CDEVT code de la sortie d’astreinte dans la table FRED_CODE_ASTREINTE liée à la sortie d’astreinte
                           string.Empty,// o    CDABS  toujours la valeur ‘’
                           pointage.Personnel.Matricule.Trim(), // o    MATRI  matricule du personnel pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           "00",// o    CDDEP  toujours la valeur ‘00’
                           0,// o   VOYDET  toujours la valeur ‘0’
                           majorationResult.Quantite,// o   HNM  la somme des sorties d’astreintes de la journée sur le CI pour un personnel (total du nombre d’heure entres les champs DateFinAstreinte et DateDebutAstreinte de la table FRED_RAPPORT_LIGNE_ASTREINTE d’un rapport ligne)
                           0,//o    H100  toujours la valeur ‘0’
                           0,// o   H100  toujours la valeur ‘0
                           0,   // o    NSINT  toujours la valeur ‘0’                       
                           dateRapport.Year, //o    RAPAA  année de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           dateRapport.Month, //o   RAPMM  mois de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           dateRapport.Day, //o RAPJJ  jour de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           codeSocietePaye, // o    SOCIET  code paye de la société du personnel pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           "R", //o    TYPERP  toujours la valeur ‘R’
                           pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), //  NOAFFP  code du CI pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                           jobId.PadLeft(10, ' '),// o NOLOP  numéro de lot du rapport de pointage
                           1, //o   RAPMOD  toujours la valeur ‘1’
                           string.Empty,//o ZONE  toujours la valeur ‘’
                           string.Empty,//o CDMAJ  toujours la valeur ‘’
                           nomUtilisateur);//o  CUSER  matricule de l’utilisateur exécutant la remontée ou le contrôle vrac
        }

        private string BuildInsertPrimeSQLRequest(PrimeResult primeResult, string jobId, DateTime periode)
        {
            DateTime dateRapport = primeResult.RapportLigne.Rapport.DateChantier.ToLocalTime();

            return string.Format(InsertPrimeQuery,
                          dateRapport.Day,
                          dateRapport.Month,
                          dateRapport.Year,
                          primeResult.RapportLigne.Ci.CodeExterne?.Trim().PadLeft(6, ' '),
                          primeResult.RapportLigne.Personnel.Matricule.Trim(),
                          periode.ToLocalTime().Year,
                          periode.ToLocalTime().Month,
                          primeResult.Code,
                          primeResult.Libelle.Trim().Replace("'", "''"),
                          primeResult.Quantite,
                          primeResult.Unite,
                          jobId.PadLeft(10, ' '));
        }

        private void FillListPrimeParameters(PrimeResult primeResult, string jobId, DateTime periode)
        {
            DateTime dateRapport = primeResult.RapportLigne.Rapport.DateChantier.ToLocalTime();

            var primeParameters = new InsertQueryPrimeParametersModel()
            {
                PRJOUP = dateRapport.Day.ToString(),
                PRMOIP = dateRapport.Month.ToString(),
                PRANNP = dateRapport.Year.ToString(),
                PRAFF = primeResult.RapportLigne.Ci.CodeExterne?.Trim().PadLeft(6, ' '),
                PRMATR = primeResult.RapportLigne.Personnel.Matricule.Trim(),
                PRANNC = periode.ToLocalTime().Year.ToString(),
                PRMOIC = periode.ToLocalTime().Month.ToString(),
                PRPRIM = primeResult.Code,
                PRPRIL = primeResult.Libelle.Trim().Replace("'", "''"),
                PRQTE = primeResult.Quantite,
                PRUNI = primeResult.Unite,
                NOLOP = jobId.PadLeft(10, ' ')
            };

            listPrimeParameters.Add(primeParameters);
        }

        private void FillListPointageIpdParameters(RapportLigneEnt pointage, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye, string codeDeplacementOrBiggestIpd)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            var pointageParameters = new InsertQueryPointageParametersModel()
            {
                NOLOT = numLot,
                NOAFF = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),
                CDEVT = "T",// o CDEVT
                CDABS = pointage.CodeAbsence?.Code.Trim(),
                MATRI = pointage.Personnel.Matricule.Trim(),
                CDDEP = codeDeplacementOrBiggestIpd, // 00 = sans code déplacement. 
                VOYDET = Convert.ToInt32(pointage.DeplacementIV).ToString(),
                HNM = pointage.HeureNormale.ToString().Replace(",", "."),
                H100 = pointage.HeureMajoration.ToString().Replace(",", "."),
                HABS = pointage.HeureAbsence.ToString().Replace(",", "."),
                NSINT = Convert.ToInt32(pointage.NumSemaineIntemperieAbsence).ToString(),
                RAPAA = dateRapport.Year.ToString(),
                RAPMM = dateRapport.Month.ToString(),
                RAPJJ = dateRapport.Day.ToString(),
                SOCIET = codeSocietePaye, // pointage.Ci.SocieteData.Code,
                TYPERP = "R",
                NOAFFP = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), // "NOAFFP" : affaire prestation. Pour l'instant Code CI
                NOLOP = jobId.PadLeft(10, ' '),
                RAPMOD = "1", // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                ZONE = pointage.CodeZoneDeplacement?.Code,
                CDMAJ = pointage.CodeMajoration?.Code,
                CUSER = nomUtilisateur
            };

            listPointageParameters.Add(pointageParameters);
        }

        private void FillListPointageWithoutIpdParameters(RapportLigneEnt pointage, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            var pointageParameters = new InsertQueryPointageParametersModel()
            {
                NOLOT = numLot,
                NOAFF = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),
                CDEVT = "T",// o CDEVT
                CDABS = pointage.CodeAbsence?.Code.Trim(),
                MATRI = pointage.Personnel.Matricule.Trim(),
                CDDEP = "00",
                VOYDET = Convert.ToInt32(pointage.DeplacementIV).ToString(),
                HNM = pointage.HeureNormale.ToString().Replace(",", "."),
                H100 = pointage.HeureMajoration.ToString().Replace(",", "."),
                HABS = pointage.HeureAbsence.ToString().Replace(",", "."),
                NSINT = Convert.ToInt32(pointage.NumSemaineIntemperieAbsence).ToString(),
                RAPAA = dateRapport.Year.ToString(),
                RAPMM = dateRapport.Month.ToString(),
                RAPJJ = dateRapport.Day.ToString(),
                SOCIET = codeSocietePaye, // pointage.Ci.SocieteData.Code,
                TYPERP = "R",
                NOAFFP = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), // "NOAFFP" : affaire prestation. Pour l'instant Code CI
                NOLOP = jobId.PadLeft(10, ' '),
                RAPMOD = "1", // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                ZONE = string.Empty,
                CDMAJ = pointage.CodeMajoration?.Code,
                CUSER = nomUtilisateur
            };

            listPointageParameters.Add(pointageParameters);
        }

        private void FillListPointageEtudeParameters(PointageTypeEtudeResult pointageTypeEtudeResult, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye, string codeDeplacementOrBiggestIpd)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointageTypeEtudeResult.RapportLigne.Rapport.DateChantier.ToLocalTime();

            var pointageParameters = new InsertQueryPointageParametersModel()
            {
                NOLOT = numLot,
                NOAFF = pointageTypeEtudeResult.Code.Trim().PadLeft(6, ' '),
                CDEVT = "T",// o CDEVT
                CDABS = pointageTypeEtudeResult.RapportLigne.CodeAbsence?.Code.Trim(),
                MATRI = pointageTypeEtudeResult.RapportLigne.Personnel.Matricule.Trim(),
                CDDEP = codeDeplacementOrBiggestIpd,
                VOYDET = Convert.ToInt32(pointageTypeEtudeResult.RapportLigne.DeplacementIV).ToString(),
                HNM = pointageTypeEtudeResult.Quantite, //pointage.HeureNormale.ToString().Replace(",", "."),
                H100 = pointageTypeEtudeResult.RapportLigne.HeureMajoration.ToString().Replace(",", "."),
                HABS = pointageTypeEtudeResult.RapportLigne.HeureAbsence.ToString().Replace(",", "."),
                NSINT = Convert.ToInt32(pointageTypeEtudeResult.RapportLigne.NumSemaineIntemperieAbsence).ToString(),
                RAPAA = dateRapport.Year.ToString(),
                RAPMM = dateRapport.Month.ToString(),
                RAPJJ = dateRapport.Day.ToString(),
                SOCIET = codeSocietePaye, // pointage.Ci.SocieteData.Code,
                TYPERP = "R",
                NOAFFP = pointageTypeEtudeResult.Code.Trim().PadLeft(6, ' '),
                NOLOP = jobId.PadLeft(10, ' '),
                RAPMOD = "1", // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                ZONE = pointageTypeEtudeResult.RapportLigne.CodeZoneDeplacement?.Code,
                CDMAJ = pointageTypeEtudeResult.RapportLigne.CodeMajoration?.Code,
                CUSER = nomUtilisateur
            };

            listPointageParameters.Add(pointageParameters);
        }

        private void FillListPointageOnlyTacheParameters(PointageTypeEtudeResult pointageTypeEtudeResult, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointageTypeEtudeResult.RapportLigne.Rapport.DateChantier.ToLocalTime();

            var pointageParameters = new InsertQueryPointageParametersModel()
            {
                NOLOT = numLot,
                NOAFF = pointageTypeEtudeResult.Code.Trim().PadLeft(6, ' '),
                CDEVT = "T",// o CDEVT
                CDABS = string.Empty,
                MATRI = pointageTypeEtudeResult.RapportLigne.Personnel.Matricule.Trim(),
                CDDEP = "00",
                VOYDET = "0",
                HNM = pointageTypeEtudeResult.Quantite.Replace(",", "."),
                H100 = "0",
                HABS = "0",
                NSINT = "0",
                RAPAA = dateRapport.Year.ToString(),
                RAPMM = dateRapport.Month.ToString(),
                RAPJJ = dateRapport.Day.ToString(),
                SOCIET = codeSocietePaye, // pointage.Ci.SocieteData.Code,
                TYPERP = "R",
                NOAFFP = pointageTypeEtudeResult.Code.Trim().PadLeft(6, ' '), // "NOAFFP" : affaire prestation. Pour l'instant Code CI
                NOLOP = jobId.PadLeft(10, ' '),
                RAPMOD = "1", // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                ZONE = string.Empty,//pointage.CodeZoneDeplacement?.Code,
                CDMAJ = string.Empty,//pointage.CodeMajoration?.Code,
                CUSER = nomUtilisateur
            };

            listPointageParameters.Add(pointageParameters);
        }

        private void FillListPointageParameters(RapportLigneEnt pointage, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            var pointageParameters = new InsertQueryPointageParametersModel()
            {
                NOLOT = numLot,
                NOAFF = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),
                CDEVT = "T",// o CDEVT
                CDABS = pointage.CodeAbsence?.Code.Trim(),
                MATRI = pointage.Personnel.Matricule.Trim(),
                CDDEP = pointage.CodeDeplacement != null ? pointage.CodeDeplacement.Code.Trim() : "00", // 00 = sans code déplacement.
                VOYDET = Convert.ToInt32(pointage.DeplacementIV).ToString(),
                HNM = pointage.HeureNormale.ToString().Replace(",", "."),
                H100 = pointage.HeureMajoration.ToString().Replace(",", "."),
                HABS = pointage.HeureAbsence.ToString().Replace(",", "."),
                NSINT = Convert.ToInt32(pointage.NumSemaineIntemperieAbsence).ToString(),
                RAPAA = dateRapport.Year.ToString(),
                RAPMM = dateRapport.Month.ToString(),
                RAPJJ = dateRapport.Day.ToString(),
                SOCIET = codeSocietePaye, // pointage.Ci.SocieteData.Code,
                TYPERP = "R",
                NOAFFP = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), // "NOAFFP" : affaire prestation. Pour l'instant Code CI
                NOLOP = jobId.PadLeft(10, ' '),
                RAPMOD = "1", // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                ZONE = pointage.CodeZoneDeplacement?.Code,
                CDMAJ = pointage.CodeMajoration?.Code,
                CUSER = nomUtilisateur
            };

            listPointageParameters.Add(pointageParameters);
        }

        private void FillListAstreintesParameters(AstreinteResult astreinteResult, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            RapportLigneEnt pointage = astreinteResult.RapportLigne;
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            var pointageParameters = new InsertQueryPointageParametersModel()
            {
                NOLOT = numLot,//NOLOT
                NOAFF = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),// o NOAFF  code du CI pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                CDEVT = astreinteResult.Code.Trim(),// o CDEVT  code de la sortie d’astreinte dans la table FRED_CODE_ASTREINTE liée à la sortie d’astreinte
                CDABS = string.Empty,// o    CDABS  toujours la valeur ‘’
                MATRI = pointage.Personnel.Matricule.Trim(), // o    MATRI  matricule du personnel pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                CDDEP = "00",// o    CDDEP  toujours la valeur ‘00’
                VOYDET = "0",// o   VOYDET  toujours la valeur ‘0’
                HNM = astreinteResult.Quantite,// o   HNM  la somme des sorties d’astreintes de la journée sur le CI pour un personnel (total du nombre d’heure entres les champs DateFinAstreinte et DateDebutAstreinte de la table FRED_RAPPORT_LIGNE_ASTREINTE d’un rapport ligne)
                H100 = "0",//o    H100  toujours la valeur ‘0’
                HABS = "0",// o   H100  toujours la valeur ‘0
                NSINT = "0",   // o    NSINT  toujours la valeur ‘0’                       
                RAPAA = dateRapport.Year.ToString(), //o    RAPAA  année de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                RAPMM = dateRapport.Month.ToString(), //o   RAPMM  mois de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                RAPJJ = dateRapport.Day.ToString(), //o RAPJJ  jour de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                SOCIET = codeSocietePaye, // o    SOCIET  code paye de la société du personnel pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                TYPERP = "R", //o    TYPERP  toujours la valeur ‘R’
                NOAFFP = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), //  NOAFFP  code du CI pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                NOLOP = jobId.PadLeft(10, ' '),// o NOLOP  numéro de lot du rapport de pointage
                RAPMOD = "1", //o   RAPMOD  toujours la valeur ‘1’
                ZONE = string.Empty,//o ZONE  toujours la valeur ‘’
                CDMAJ = string.Empty,//o CDMAJ  toujours la valeur ‘’
                CUSER = nomUtilisateur//o  CUSER  matricule de l’utilisateur exécutant la remontée ou le contrôle vrac
            };

            listPointageParameters.Add(pointageParameters);
        }

        private void FillListMajorationParameters(MajorationResult majorationResult, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            RapportLigneEnt pointage = majorationResult.RapportLigne;
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            var pointageParameters = new InsertQueryPointageParametersModel()
            {
                NOLOT = numLot,//NOLOT
                NOAFF = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '),// o NOAFF  code du CI pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                CDEVT = majorationResult.Code.Trim(),// o CDEVT code de la sortie d’astreinte dans la table FRED_CODE_ASTREINTE liée à la sortie d’astreinte
                CDABS = string.Empty,// o    CDABS  toujours la valeur ‘’
                MATRI = pointage.Personnel.Matricule.Trim(), // o    MATRI  matricule du personnel pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                CDDEP = "00",// o    CDDEP  toujours la valeur ‘00’
                VOYDET = "0",// o   VOYDET  toujours la valeur ‘0’
                HNM = majorationResult.Quantite,// o   HNM  la somme des sorties d’astreintes de la journée sur le CI pour un personnel (total du nombre d’heure entres les champs DateFinAstreinte et DateDebutAstreinte de la table FRED_RAPPORT_LIGNE_ASTREINTE d’un rapport ligne)
                H100 = "0",//o    H100  toujours la valeur ‘0’
                HABS = "0",// o   H100  toujours la valeur ‘0
                NSINT = "0",   // o    NSINT  toujours la valeur ‘0’                       
                RAPAA = dateRapport.Year.ToString(), //o    RAPAA  année de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                RAPMM = dateRapport.Month.ToString(), //o   RAPMM  mois de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                RAPJJ = dateRapport.Day.ToString(), //o RAPJJ  jour de la date de pointage dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                SOCIET = codeSocietePaye, // o    SOCIET  code paye de la société du personnel pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                TYPERP = "R", //o    TYPERP  toujours la valeur ‘R’
                NOAFFP = pointage.Ci.CodeExterne?.Trim().PadLeft(6, ' '), //  NOAFFP  code du CI pointé dans la table FRED_RAPPORT_LIGNE liée à la sortie d’astreinte
                NOLOP = jobId.PadLeft(10, ' '),// o NOLOP  numéro de lot du rapport de pointage
                RAPMOD = "1", //o   RAPMOD  toujours la valeur ‘1’
                ZONE = string.Empty,//o ZONE  toujours la valeur ‘’
                CDMAJ = string.Empty,//o CDMAJ  toujours la valeur ‘’
                CUSER = nomUtilisateur//o  CUSER  matricule de l’utilisateur exécutant la remontée ou le contrôle vrac
            };

            listPointageParameters.Add(pointageParameters);
        }
        #endregion
    }
}
