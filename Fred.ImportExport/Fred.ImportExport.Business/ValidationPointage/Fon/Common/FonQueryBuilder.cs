using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.Common
{
    /// <summary>
    /// Builder qui construit les requetes SQL 
    /// </summary>
    public class FonQueryBuilder : IFonQueryBuilder
    {
        public const string InsertPointageClause = "INSERT INTO INTFOND.SRAPFREP(NOLOT, NOAFF, CDABS, MATRI, CDDEP, VOYDET, HNM, H100, HABS, NSINT, RAPAA, RAPMM, RAPJJ, SOCIET, TYPERP, NOAFFP, NOLOP, RAPMOD, ZONE, CDMAJ, CUSER) VALUES ";
        public const string InsertPrimeClause = "INSERT INTO INTFOND.PRIMNEWR(PRJOUP, PRMOIP, PRANNP, PRAFF, PRMATR, PRANNC, PRMOIC, PRPRIM, PRPRIL, PRQTE, PRUNI, NOLOP) VALUES ";

        private const string InsertPointageQuery = InsertPointageClause + "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}')";
        private const string InsertPrimeQuery = InsertPrimeClause + "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')";

        #region Récupération des requêtes AS400

        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400
        /// </summary>
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="periode">Période choisie</param> 
        /// <param name="rapportLignePrime">Ligne rapport prime</param>
        /// <param name="rapportLignes">Liste des pointages FRED à contrôler</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400</returns>
        public IEnumerable<QueryInfo> BuildAnaelInsertsQueries(ValidationPointageContextData globalData, DateTime periode, List<RapportLignePrimeEnt> rapportLignePrime, ICollection<RapportLigneEnt> rapportLignes)
        {
            var result = new List<QueryInfo>();
            var nomUtilisateur = globalData.NomUtilisateur;
            if (nomUtilisateur.Length > 10)
            {
                nomUtilisateur = globalData.NomUtilisateur.Substring(0, 10);
            }

            if (rapportLignes.Count > 0)
            {
                result.Add(CreateQueryOrComment(false, "-- Insertion des Pointages"));

                foreach (var pointage in rapportLignes)
                {
                    var query = BuildInsertPointageSQLRequest(pointage, globalData.JobId, periode, nomUtilisateur, globalData.CodeSocietePaye);

                    result.Add(CreateQueryOrComment(true, query));
                }

                result.Add(CreateQueryOrComment(false, "-- Insertion des Prime"));
                var pointages = rapportLignes;

                foreach (var pointage in pointages)
                {
                    foreach (var prime in pointage.ListRapportLignePrimes)
                    {
                        var query = BuildInsertPrimeSQLRequest(pointage, prime, globalData.JobId, periode);
                        result.Add(CreateQueryOrComment(true, query, false));
                    }
                }
            }
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

        /// <summary>
        /// Construction de la requête d'insertion des pointages dans ANAEL
        /// </summary>
        /// <param name="pointage">Pointage à insérer</param>
        /// <param name="jobId">Identifiant du Job hangfire</param>
        /// <param name="periode">Période comptable</param>
        /// <param name="nomUtilisateur">Nom de l'utilisateur exécutant l'opération</param>
        /// <param name="codeSocietePaye">Code société payer sur laquelle l'opération est exécutée</param>
        /// <returns>Requête SQL</returns>
        private string BuildInsertPointageSQLRequest(RapportLigneEnt pointage, string jobId, DateTime periode, string nomUtilisateur, string codeSocietePaye)
        {
            string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

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
                                 jobId.PadLeft(10, ' '),
                                 1, // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                                 pointage.CodeZoneDeplacement?.Code,
                                 pointage.CodeMajoration?.Code,
                                 nomUtilisateur);
        }

        /// <summary>
        ///   Construction de la requête d'insertion SQL des primes dans AS400
        /// </summary>
        /// <param name="pointage">Pointage</param>
        /// <param name="rapportLignePrime">RapportLignePrime</param>
        /// <param name="jobId">Identifiant du job hangfire</param>
        /// <param name="periode">Période</param>
        /// <returns>Requête d'insertion d'une prime dans AS400</returns>
        private string BuildInsertPrimeSQLRequest(RapportLigneEnt pointage, RapportLignePrimeEnt rapportLignePrime, string jobId, DateTime periode)
        {
            DateTime dateRapport = pointage.Rapport.DateChantier.ToLocalTime();

            string unite = string.Empty;
            string heure = string.Empty;
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
                          jobId.PadLeft(10, ' '));
        }
        #endregion
    }
}
