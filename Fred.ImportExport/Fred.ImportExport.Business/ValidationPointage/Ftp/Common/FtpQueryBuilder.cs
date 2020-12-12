using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Common.Interfaces;
using Fred.ImportExport.DataAccess.Common;
using NLog;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.Common
{
    public class FtpQueryBuilder : IFtpQueryBuilder
    {
        public const string InsertPointageClause = "INSERT INTO INTFTP.SRAPFREP(NOLOT, NOAFF, CDABS, MATRI, CDDEP, VOYDET, HNM, H100, HABS, NSINT, RAPAA, RAPMM, RAPJJ, SOCIET, TYPERP, NOAFFP, NOLOP, RAPMOD, ZONE, CDMAJ, CUSER) VALUES";
        public const string InsertPrimeClause = "INSERT INTO INTFTP.PRIMNEWR(PRJOUP, PRMOIP, PRANNP, PRAFF, PRMATR, PRANNC, PRMOIC, PRPRIM, PRPRIL, PRQTE, PRUNI, NOLOP) VALUES ";

        private const string InsertPointageQuery = InsertPointageClause + "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')";
        private const string InsertPrimeQuery = InsertPrimeClause + "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')";
        private const char SpacePadChar = ' ';
        private const int JobIdLenght = 10;

        #region Requêtes

        /// <summary>
        /// Requête RVG pour récupérer les pointages chantier pour des établissements.
        /// </summary>
        /// <remarks>
        /// {0} : le code société paie
        /// {1} : 1 pour récupérer les pointages chantier de tous les matricules, 0 pour récupérer ceux indiqués dans le paramètre {2}
        /// {2} : le matricule concerné (cf. {1})
        /// {3} : la période comptable
        /// {4} : établissement(s) concerné(s) au format "'35', '36', ..."
        /// </remarks>
        private const string SelectRvgPointagesChantierPourDesEtablissementsQuery = "SELECT * FROM dbo.GEP_SelectPointagesChantierPrev(0,'','',0,'{0}',{1},'','{2}','{3}') WHERE ETABL_ID IN ({4})";

        /// <summary>
        /// Requête RVG pour récupérer les pointages chantier pour tous les établissements.
        /// </summary>
        /// <remarks>
        /// {0} : le code société paie
        /// {1} : 1 pour récupérer les pointages chantier de tous les matricules, 0 pour récupérer ceux indiqués dans le paramètre {2}
        /// {2} : le matricule concerné (cf. {1})
        /// {3} : la période comptable
        /// {4} : établissement(s) concerné(s) au format "'35', '36', ..."
        /// </remarks>
        private const string SelectRvgPointagesChantierPourTousEtablissementsQuery = "SELECT * FROM dbo.GEP_SelectPointagesChantierPrev(0,'','',0,'{0}',{1},'','{2}','{3}')";

        /// <summary>
        /// Requête RVG pour récupérer les primes pour des établissements.
        /// </summary>
        /// <remarks>
        /// {0} : le code société paie
        /// {1} : '1' pour récupérer les primes de tous les matricules, '0' pour récupérer celles indiquées dans le paramètre {2}
        /// {2} : le matricule concerné (cf. {1})
        /// {3} : la période comptable
        /// {4} : établissement(s) concerné(s) au format "'35', '36', ..."
        /// </remarks>
        private const string SelectRvgPrimesPourDesEtablissementsQuery = "SELECT * FROM dbo.GEP_fnSelectPrimeReelPrev ('0','','','','{0}','{1}','','{2}','{3}') WHERE ETABL_ID IN ({4})";

        /// <summary>
        /// Requête RVG pour récupérer les primes pour tous les établissements.
        /// </summary>
        /// <remarks>
        /// {0} : le code société paie
        /// {1} : '1' pour récupérer les primes de tous les matricules, '0' pour récupérer celles indiquées dans le paramètre {2}
        /// {2} : le matricule concerné (cf. {1})
        /// {3} : la période comptable
        /// </remarks>
        private const string SelectRvgPrimesPourTousLesEtablissementsQuery = "SELECT * FROM dbo.GEP_fnSelectPrimeReelPrev ('0','','','','{0}','{1}','','{2}','{3}')";

        #endregion

        private readonly string chaineConnexionRVG = ConfigurationManager.AppSettings["RVG:ConnectionString:Groupe:GFTP"];
        private readonly ISocieteManager societeManager;
        private readonly IEtablissementPaieManager etablissementPaieManager;

        public FtpQueryBuilder(ISocieteManager societeManager, IEtablissementPaieManager etablissementPaieManager)
        {
            this.societeManager = societeManager;
            this.etablissementPaieManager = etablissementPaieManager;
        }

        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes FRED et RVG dans l'AS400
        /// </summary>
        /// <param name="globalData">Données globales</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="fredPointages">Liste des pointages FRED à contrôler</param>
        /// <param name="rvgPointagesAndPrimes">Les pointages et les primes de RVG. Peut être null.</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes FRED et RVG dans l'AS400</returns>
        public (IEnumerable<string> pointageQueries, IEnumerable<string> primeQueries) GetPointageAndPrimeQueries(ValidationPointageContextData globalData, DateTime periode, IEnumerable<RapportLigneEnt> fredPointages, RvgPointagesAndPrimes rvgPointagesAndPrimes)
        {
            (IEnumerable<string> fredPointageQueries, IEnumerable<string> fredPrimeQueries) = GetPointageAndPrimeQueries(globalData, periode, fredPointages);
            (IEnumerable<string> rvgPointageQueries, IEnumerable<string> rvgPrimeQueries) = GetPointageAndPrimeQueries(globalData, periode, rvgPointagesAndPrimes);

            IEnumerable<string> pointageQueries = fredPointageQueries.Concat(rvgPointageQueries);
            IEnumerable<string> primeQueries = fredPrimeQueries.Concat(rvgPrimeQueries);

            return (pointageQueries, primeQueries);
        }

        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400
        /// </summary>
        /// <param name="globalData">Données globales</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="fredPointages">Liste des pointages FRED à contrôler</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400</returns>
        private (IEnumerable<string>, IEnumerable<string>) GetPointageAndPrimeQueries(ValidationPointageContextData globalData, DateTime periode, IEnumerable<RapportLigneEnt> fredPointages)
        {
            if (fredPointages == null)
                return (Enumerable.Empty<string>(), Enumerable.Empty<string>());

            IEnumerable<string> pointageQueries = fredPointages.Select(GetPointageInsertQuery);
            IEnumerable<string> primeQueries = fredPointages.SelectMany(pointage => pointage.ListRapportLignePrimes.Select(prime => GetPrimeInsertQuery(pointage, prime, globalData.JobId)));

            return (pointageQueries, primeQueries);

            #region Local functions

            string GetPointageInsertQuery(RapportLigneEnt pointage)
            {
                string nomUtilisateur = globalData.NomUtilisateur;
                string codeSocietePaye = globalData.CodeSocietePaye;
                string jobId = globalData.JobId.PadLeft(JobIdLenght, SpacePadChar);
                string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
                DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

                if (nomUtilisateur.Length > 10)
                {
                    nomUtilisateur = nomUtilisateur.Substring(0, 10);
                }

                return string.Format(InsertPointageQuery,
                               numLot,
                               pointage.Ci.CompteInterneSepId != null ? pointage.Ci.CompteInterneSep.Code : pointage.Ci.Code,
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
                               codeSocietePaye,
                               "R",
                               pointage.Ci.CompteInterneSepId != null ? pointage.Ci.CompteInterneSep.Code : pointage.Ci.Code, // "NOAFFP" : affaire prestation. Pour l'instant Code CI
                               jobId,
                               1, // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                               pointage.CodeZoneDeplacement?.Code,
                               pointage.CodeMajoration?.Code,
                               nomUtilisateur);
            }

            string GetPrimeInsertQuery(RapportLigneEnt pointage, RapportLignePrimeEnt rapportLignePrime, string jobId)
            {
                var unite = string.Empty;
                var heure = string.Empty;

                switch (rapportLignePrime.Prime.PrimeType)
                {
                    case ListePrimeType.PrimeTypeHoraire:
                        unite = "Heures";
                        heure = rapportLignePrime.HeurePrime.HasValue ? rapportLignePrime.HeurePrime.Value.ToString().Replace(",", ".") : "0.0";
                        break;
                    case ListePrimeType.PrimeTypeJournaliere:
                        unite = "Jours";
                        heure = "1.0";
                        break;
                    default:
                        // ListePrimeType.PrimeTypeMensuelle
                        unite = "Mois";
                        heure = "1.0";
                        break;
                }

                DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();
                jobId = jobId.PadLeft(JobIdLenght, SpacePadChar);

                return string.Format(InsertPrimeQuery,
                              dateRapport.Day,
                              dateRapport.Month,
                              dateRapport.Year,
                              pointage.Ci.CompteInterneSepId != null ? pointage.Ci.CompteInterneSep.Code : pointage.Ci.Code,
                              pointage.Personnel.Matricule.Trim(),
                              periode.ToLocalTime().Year,
                              periode.ToLocalTime().Month,
                              rapportLignePrime.Prime.Code.Trim(),
                              rapportLignePrime.Prime.Libelle.Trim().Replace("'", "''"),
                              heure,
                              unite,
                              jobId);
            }

            #endregion
        }

        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes RVG dans l'AS400
        /// </summary>
        /// <param name="globalData">Données globales</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="rvgPointagesAndPrimes">Les pointages et les primes de RVG. Peut être null</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes RVG dans l'AS400</returns>
        private (IEnumerable<string>, IEnumerable<string>) GetPointageAndPrimeQueries(ValidationPointageContextData globalData, DateTime periode, RvgPointagesAndPrimes rvgPointagesAndPrimes)
        {
            if (rvgPointagesAndPrimes == null)
                return (Enumerable.Empty<string>(), Enumerable.Empty<string>());

            IEnumerable<string> pointageQueries = rvgPointagesAndPrimes.Pointages.Select(GetPointageInsertQuery);
            IEnumerable<string> primeQueries = rvgPointagesAndPrimes.Primes.Select(GetPrimeInsertQuery);

            return (pointageQueries, primeQueries);

            #region Local functions

            string GetPointageInsertQuery(RvgPointage pointage)
            {
                string nomUtilisateur = globalData.NomUtilisateur;
                string codeSocietePaye = globalData.CodeSocietePaye;
                string jobId = globalData.JobId.PadLeft(JobIdLenght, SpacePadChar);
                string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));

                if (nomUtilisateur.Length > 10)
                {
                    nomUtilisateur = nomUtilisateur.Substring(0, 10);
                }

                return string.Format(InsertPointageQuery,
                    numLot,
                    pointage.CodeAffaire.Trim(),
                    pointage.CodeAbsence?.Trim(),
                    pointage.MatriculePersonnel.Trim(),
                    pointage.CodeDeplacement ?? "00", // 00 = sans code déplacement. 
                    pointage.VoyageDetente,
                    pointage.HeureNormale.ToString().Replace(",", "."),
                    pointage.HeureMajoration.ToString().Replace(",", "."),
                    pointage.HeureAbsence.ToString().Replace(",", "."),
                    pointage.NumSemaineIntemperieAbsence,
                    pointage.AnneeRapport,
                    pointage.MoisRapport,
                    pointage.JourRapport,
                    codeSocietePaye,
                    "R",
                    pointage.CodeAffairePrestation,
                    jobId,
                    1, // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                    pointage.CodeZoneDeplacement,
                    pointage.CodeMajoration,
                    nomUtilisateur);
            }

            string GetPrimeInsertQuery(RvgPrime prime)
            {
                string jobId = globalData.JobId.PadLeft(JobIdLenght, SpacePadChar);

                return string.Format(InsertPrimeQuery,
                    prime.Date.Day,
                    prime.Date.Month,
                    prime.Date.Year,
                    prime.CodeAffaire,
                    prime.MatriculePersonnel,
                    periode.ToLocalTime().Year,
                    periode.ToLocalTime().Month,
                    prime.Code,
                    prime.Libelle.Replace("'", "''"),
                    prime.TypeHoraire ? prime.Quantite.ToString().Replace(",", ".") : "1.0",
                    prime.TypeHoraire ? "Heures" : "Jours",
                    jobId);
            }

            #endregion
        }

        #region Pointages & Primes RVG

        /// <summary>
        /// Récupère les pointages et les primes de RVG.
        /// </summary>
        /// <param name="periode">La période concernée.</param>
        /// <param name="filtre">Le filtre à appliquer.</param>
        /// <returns>Les pointages et les primes de RVG.</returns>
        public RvgPointagesAndPrimes GetRvgPointagesAndPrimes(DateTime periode, PointageFiltre filtre)
        {
            var ret = new RvgPointagesAndPrimes();

            // La société doit exister
            var societe = societeManager.FindById(filtre.SocieteId);
            if (societe == null)
            {
                LogManager.GetCurrentClassLogger().Error($"La société d'identifiant {filtre.SocieteId} n'existe pas");
                return ret;
            }

            // Le code de la société paie est nécessaire pour récupérer les pointages RVG
            var codeSocietePaye = societe.CodeSocietePaye;
            if (string.IsNullOrEmpty(codeSocietePaye))
            {
                LogManager.GetCurrentClassLogger().Error($"Le code société paie n'est pas renseigné pour la société d'identifiant {filtre.SocieteId}");
                return ret;
            }

            // Vérification du matricule
            if (filtre.TakeMatricule)
            {
                if (filtre.Matricule == null)
                {
                    LogManager.GetCurrentClassLogger().Error($"Le matricule doit être fournit");
                    return ret;
                }
            }
            else if (filtre.Matricule != null)
            {
                LogManager.GetCurrentClassLogger().Error($"Le matricule ne doit pas être fournit");
                return ret;
            }

            // Récupère les pointages chantier RVG
            var query = GetRvgPointagesChantierQuery(codeSocietePaye, filtre.Matricule, periode, filtre.EtablissementPaieIdList);
            if (!string.IsNullOrEmpty(query))
            {
                using (var rvgDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.SqlServer, chaineConnexionRVG))
                {
                    using (var reader = rvgDatabase.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            var rvgPointage = GetRvgPointage(reader, periode);
                            ret.Pointages.Add(rvgPointage);
                        }
                    }
                }
            }

            // Récupère les primes RVG
            query = GetRvgPrimesQuery(codeSocietePaye, filtre.Matricule, periode, filtre.EtablissementPaieIdList);
            if (!string.IsNullOrEmpty(query))
            {
                using (var rvgDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.SqlServer, chaineConnexionRVG))
                {
                    using (var reader = rvgDatabase.ExecuteReader(query))
                    {
                        while (reader.Read())
                        {
                            var rvgPointage = GetRvgPrime(reader, periode);
                            ret.Primes.Add(rvgPointage);
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Récupère la requête de sélection des pointages chantier.
        /// </summary>
        /// <param name="codeSocietePaye">Le code de la société paie.</param>
        /// <param name="matricule">Le matricule ou null pour tous les matricules.</param>
        /// <param name="periode">La période comptable.</param>
        /// <param name="fredEtablissementPaieIds">Les établissements de paie FRED concernés ou null pour tous les établissements.</param>
        /// <returns>La requête de sélection des pointages chantier.</returns>
        private string GetRvgPointagesChantierQuery(string codeSocietePaye, string matricule, DateTime periode, IEnumerable<int> fredEtablissementPaieIds)
        {
            string ret = null;
            var periodeComptable = ((periode.Year * 100) + periode.Month).ToString();   // Au format RVG : AAAAMM
            var tousLesMatricules = matricule == null ? '1' : '0';
            if (fredEtablissementPaieIds != null && fredEtablissementPaieIds.Any())
            {
                // Pour certains établissements
                var etablissementsParametre = GetEtablissementParametreForQuery(fredEtablissementPaieIds);
                if (!string.IsNullOrEmpty(etablissementsParametre))
                {
                    ret = string.Format(SelectRvgPointagesChantierPourDesEtablissementsQuery, codeSocietePaye, tousLesMatricules, matricule, periodeComptable, etablissementsParametre);
                }
            }
            else
            {
                // Pour tous les établissements
                ret = string.Format(SelectRvgPointagesChantierPourTousEtablissementsQuery, codeSocietePaye, tousLesMatricules, matricule, periodeComptable);
            }
            return ret;
        }

        /// <summary>
        /// Retourne un pointage RVG.
        /// </summary>
        /// <param name="reader">Le reader courant.</param>
        /// <param name="periode">La période.</param>
        /// <returns>Le pointage RVG.</returns>
        private static RvgPointage GetRvgPointage(IDataReader reader, DateTime periode)
        {
            var readerCodeAbsence = reader["coabs_id"] as string;
            var readerCodeDeplacement = reader["CODEP_ID"] as string;
            var readerHeureNormale = reader["LIGRJ_PNBHN"];
            var readerHeureMajoration = reader["LIGRJ_PNBHM"];
            var readerHeureAbsence = reader["LIGRJ_COABS_NBHA"];
            var readerAnneeRapport = reader["anneeRap"];
            var readerMoisRapport = reader["moisRap"];
            var readerJourRapport = reader["jourRap"];

            var numSemaineIntemperieAbsence = 0;
            var readerNumSemaineIntemperieAbsence = reader["LIGRJ_COABS_NUMINT"];
            if (readerNumSemaineIntemperieAbsence != DBNull.Value)
            {
                numSemaineIntemperieAbsence = (int)(decimal)readerNumSemaineIntemperieAbsence;
                if (numSemaineIntemperieAbsence < 0 || numSemaineIntemperieAbsence > 52)
                {
                    numSemaineIntemperieAbsence = 0;
                }
            }

            return new RvgPointage
            {
                CodeAffaire = reader["AFFAIRE"] as string,
                CodeAffairePrestation = reader["AFFAI_PREST"] as string,
                MatriculePersonnel = reader["PERSO_ID"] as string,
                EtablissementPaieIdPersonnel = reader["ETABL_ID"] as string,
                CodeAbsence = readerCodeAbsence != "0" ? readerCodeAbsence : null,
                CodeDeplacement = readerCodeDeplacement != "0" ? readerCodeDeplacement : null,
                VoyageDetente = reader["LIGRJ_VOYDET"] as string ?? "0",
                HeureNormale = readerHeureNormale != DBNull.Value ? (decimal)readerHeureNormale : 0,
                HeureMajoration = readerHeureMajoration != DBNull.Value ? (decimal)readerHeureMajoration : 0,
                HeureAbsence = readerHeureAbsence != DBNull.Value ? (decimal)readerHeureAbsence : 0,
                NumSemaineIntemperieAbsence = numSemaineIntemperieAbsence,
                AnneeRapport = readerAnneeRapport != DBNull.Value ? (int)readerAnneeRapport : periode.Year,
                MoisRapport = readerMoisRapport != DBNull.Value ? (int)readerMoisRapport : periode.Month,
                JourRapport = readerJourRapport != DBNull.Value ? (int)readerJourRapport : periode.Day,
                CodeZoneDeplacement = reader["ZONE_ID"] as string
            };
        }


        /// <summary>
        /// Récupère la requête de sélection des primes.
        /// </summary>
        /// <param name="codeSocietePaye">Le code de la société paie.</param>
        /// <param name="matricule">Le matricule ou null pour tous les matricules.</param>
        /// <param name="periode">La période comptable.</param>
        /// <param name="fredEtablissementPaieIds">Les établissements de paie FRED concernés ou null pour tous les établissements.</param>
        /// <returns>La requête de sélection des pointages chantier.</returns>
        private string GetRvgPrimesQuery(string codeSocietePaye, string matricule, DateTime periode, IEnumerable<int> fredEtablissementPaieIds)
        {
            string ret = null;
            var periodeComptable = ((periode.Year * 100) + periode.Month).ToString();   // Au format RVG : AAAAMM
            var tousLesMatricules = matricule == null ? '1' : '0';
            if (fredEtablissementPaieIds != null && fredEtablissementPaieIds.Any())
            {
                // Pour certains établissements
                var etablissementsParametre = GetEtablissementParametreForQuery(fredEtablissementPaieIds);
                if (!string.IsNullOrEmpty(etablissementsParametre))
                {
                    ret = string.Format(SelectRvgPrimesPourDesEtablissementsQuery, codeSocietePaye, tousLesMatricules, matricule, periodeComptable, etablissementsParametre);
                }
            }
            else
            {
                // Pour tous les établissements
                ret = string.Format(SelectRvgPrimesPourTousLesEtablissementsQuery, codeSocietePaye, tousLesMatricules, matricule, periodeComptable);
            }
            return ret;
        }

        /// <summary>
        /// Retourne une prime RVG.
        /// </summary>
        /// <param name="reader">Le reader courant.</param>
        /// <param name="periode">La période.</param>
        /// <returns>La prime RVG.</returns>
        private static RvgPrime GetRvgPrime(IDataReader reader, DateTime periode)
        {
            var readerDate = reader["ligrj_date"];
            var readerQuantite = reader["lrjpr_qte"];

            var ret = new RvgPrime
            {
                CodeAffaire = reader["AFFAI_ID"] as string,
                MatriculePersonnel = reader["PERSO_ID"] as string,
                Date = readerDate != DBNull.Value ? (DateTime)readerDate : periode,
                Code = reader["primes_id"] as string,
                Libelle = reader["primes_lib"] as string,
                Quantite = readerQuantite != DBNull.Value ? (decimal)readerQuantite : 0,
                TypeHoraire = reader["primes_unite"] as string == "Heures",
            };

            return ret;
        }

        /// <summary>
        /// Retourne la chaîne à utiliser dans les requêtes pour le paramètre 'établissements'.
        /// </summary>
        /// <param name="fredEtablissementPaieIds">Les identifiants des établissements de paie FRED concernés.</param>
        /// <returns>Lla chaîne à utiliser dans les requêtes pour le paramètre 'établissements'.</returns>
        private string GetEtablissementParametreForQuery(IEnumerable<int> fredEtablissementPaieIds)
        {
            var sb = new StringBuilder();
            foreach (var fredEtablissementPaieId in fredEtablissementPaieIds)
            {
                var ets = etablissementPaieManager.GetEtablissementPaieById(fredEtablissementPaieId);
                if (ets == null)
                {
                    LogManager.GetCurrentClassLogger().Error($"L'établissement de paie d'identifiant {fredEtablissementPaieId} n'existe pas");
                }
                else if (string.IsNullOrEmpty(ets.Code))
                {
                    LogManager.GetCurrentClassLogger().Error($"L'établissement de paie d'identifiant {fredEtablissementPaieId} n'a pas de code");
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(',');
                    }
                    sb.Append("'");
                    sb.Append(ets.Code);
                    sb.Append("'");
                }
            }
            return sb.ToString();
        }

        #endregion
    }
}
