using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Fred.Business.Referential;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.DataAccess.Common;

namespace Fred.ImportExport.Business.ValidationPointage
{
    /// <summary>
    ///   Classe ControleHelper
    ///   => Opérations communes au contrôle et à la remontée vrac : 
    ///     - Insertion de chaque pointage
    ///     - Insertion de chaque prime
    /// </summary>
    public class ControleHelper
    {
        public const string InsertPointageClause = "INSERT INTO INTRB.SRAPFREP(NOLOT, NOAFF, CDABS, MATRI, CDDEP, VOYDET, HNM, H100, HABS, NSINT, RAPAA, RAPMM, RAPJJ, SOCIET, TYPERP, NOAFFP, NOLOP, RAPMOD, ZONE, CDMAJ, CUSER) VALUES ";
        public const string InsertPrimeClause = "INSERT INTO INTRB.PRIMNEWR(PRJOUP, PRMOIP, PRANNP, PRAFF, PRMATR, PRANNC, PRMOIC, PRPRIM, PRPRIL, PRQTE, PRUNI, NOLOP) VALUES ";

        #region Membres

        private readonly IControlePointageManager controlePointageManager;
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IEtablissementPaieManager etsPaieManager;
        private readonly RvgControleHelper rvgControleHelper;

        private const string InsertPointageQuery = InsertPointageClause + "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')";
        private const string InsertPrimeQuery = InsertPrimeClause + "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')";

        private readonly string chaineConnexionAnael = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];
        private readonly string chaineConnexionRVG = ConfigurationManager.AppSettings["RVG:ConnectionString:Groupe:GRZB"];

        #endregion

        #region Constructeurs

        /// <summary>
        ///   Constructeur
        /// </summary>
        /// <param name="controlePointageManager">Gestionnaire de controle pointage</param>
        /// <param name="remonteeVracManager">Gestionnaire de remontée vrac</param>        
        /// <param name="etsPaieManager">Gestionnaire des établissements de paie</param>
        /// <param name="rvgControleHelper">Opérations communes au contrôle et à la remontée vrac pour RVG</param>
        public ControleHelper(IControlePointageManager controlePointageManager,
                              IRemonteeVracManager remonteeVracManager,
                              IEtablissementPaieManager etsPaieManager,
                              RvgControleHelper rvgControleHelper)
        {
            this.controlePointageManager = controlePointageManager;
            this.remonteeVracManager = remonteeVracManager;
            this.etsPaieManager = etsPaieManager;
            this.rvgControleHelper = rvgControleHelper;
        }

        #endregion

        #region Récupération des données

        /// <summary>
        ///   Applique le filtre sur les lignes de rapports (pointages) de FRED
        /// </summary>
        /// <param name="rapportLigneList">Liste des lignes de rapport</param>
        /// <param name="filtre">Filtre</param>
        /// <returns>Liste triée</returns>
        public IEnumerable<RapportLigneEnt> ApplyRapportLigneFilter(IEnumerable<RapportLigneEnt> rapportLigneList, PointageFiltre filtre)
        {
            return rapportLigneList
                   .Where(x => (!x.DateSuppression.HasValue && x.Personnel != null && x.Personnel.IsInterne)
                               && ((filtre.TakeMatricule && filtre.Matricule != null) || filtre.SocieteId > 0 && x.Personnel.SocieteId == filtre.SocieteId)
                               && (!filtre.EtablissementPaieIdList.Any() || (x.Personnel.EtablissementPaieId.HasValue && filtre.EtablissementPaieIdList.Contains(x.Personnel.EtablissementPaieId.Value)))
                               && ((!filtre.TakeMatricule || filtre.Matricule == null) || x.Personnel.Matricule == filtre.Matricule)
                               && x.PersonnelId.HasValue
                               && x.Personnel.Societe != null && x.Personnel.Societe.Groupe != null && x.Personnel.Societe.Groupe.Code == Fred.Entities.Constantes.CodeGroupeRZB)
                   .ToList();
        }

        /// <summary>
        ///     Filtre les primes
        /// </summary>
        /// <param name="rapportLignePrimes">Liste de ligne rapport/prime</param>
        /// <returns>Liste de rapport ligne prime</returns>
        public IEnumerable<RapportLignePrimeEnt> ApplyRapportLignePrimeFilter(IEnumerable<RapportLignePrimeEnt> rapportLignePrimes)
        {
            if (rapportLignePrimes == null)
            {
                return new List<RapportLignePrimeEnt>();
            }

            return rapportLignePrimes.Where(x => (x.IsChecked && x.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere)
                                                 || (x.HeurePrime > 0 && x.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire)
                                                 || (x.Prime.PrimeType == ListePrimeType.PrimeTypeMensuelle));
        }

        /// <summary>
        /// Récupère les pointages et les primes de RVG.
        /// </summary>
        /// <param name="periode">La période concernée.</param>
        /// <param name="filtre">Le filtre à appliquer.</param>
        /// <returns>Les pointages et les primes de RVG.</returns>
        public RvgPointagesAndPrimes GetRvgPointagesAndPrimes(DateTime periode, PointageFiltre filtre)
        {
            return rvgControleHelper.GetPointagesAndPrimes(periode, filtre, chaineConnexionRVG);
        }

        #endregion

        #region Insertion des données dans l'AS400

        /// <summary>
        ///   Déversement des lignes de pointages et des primes dans l'AS400
        /// </summary>
        /// <typeparam name="T">ControlePointageEnt ou RemonteeVracEnt</typeparam>    
        /// <param name="jobId">Identifiant du job hangfire</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="nomUtilisateur">Nom de l'utilisateur ayant lancé l'opération</param>
        /// <param name="fredPointages">Liste des pointages FRED à contrôler</param>
        /// <param name="rvgPointagesAndPrimes">Les pointages et les primes de RVG. Peut être null.</param>
        /// <param name="instance">Représente soit ControlePointageEnt ou RemonteeVracEnt</param>       
        /// <param name="codeSocietePaye">Code Société Paye sélectionnée où l'on effectue le contrôle</param>
        public void InsertPointageAndPrime<T>(string jobId, DateTime periode, string nomUtilisateur, IEnumerable<RapportLigneEnt> fredPointages, RvgPointagesAndPrimes rvgPointagesAndPrimes, T instance, string codeSocietePaye) where T : class
        {
            try
            {
                using (DataAccess.Common.Database destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    (IEnumerable<string> pointageQueries, IEnumerable<string> primesQueries) = GetPointageAndPrimeQueries(jobId, periode, nomUtilisateur, fredPointages, rvgPointagesAndPrimes, codeSocietePaye);

                    SendInsertQueries(destinationDatabase, InsertPointageClause, pointageQueries);
                    SendInsertQueries(destinationDatabase, InsertPrimeClause, primesQueries);
                }
            }
            catch (Exception e)
            {
                string errorMsg = "Erreur lors du déversement des pointages et des primes dans AS400.";
                HandleErrors(e, errorMsg, instance);
            }

            #region Local functions

            void SendInsertQueries(DataAccess.Common.Database destinationDatabase, string insertClause, IEnumerable<string> queriesInfos)
            {
                for (var i = 0; i < queriesInfos.Count(); i += 1000)
                {
                    IEnumerable<string> queries = queriesInfos.Skip(i).Take(1000).ToList();

                    string valuesClauses = string.Join($",{Environment.NewLine}", queries.Select(q => q.Substring(insertClause.Length)));
                    string insertQuery = string.Concat(insertClause, Environment.NewLine, valuesClauses);

                    destinationDatabase.ExecuteNonQuery(insertQuery);
                }
            }

            #endregion
        }

        #endregion

        #region Récupération des requêtes AS400

        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes FRED et RVG dans l'AS400
        /// </summary>
        /// <param name="jobId">Identifiant du job hangfire</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="nomUtilisateur">Nom de l'utilisateur ayant lancé l'opération</param>
        /// <param name="fredPointages">Liste des pointages FRED à contrôler</param>
        /// <param name="rvgPointagesAndPrimes">Les pointages et les primes de RVG. Peut être null.</param>
        /// <param name="codeSocietePaye">Code Société Paye sélectionnée où l'on effectue le contrôle</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes FRED et RVG dans l'AS400</returns>
        public (IEnumerable<string>, IEnumerable<string>) GetPointageAndPrimeQueries(string jobId, DateTime periode, string nomUtilisateur, IEnumerable<RapportLigneEnt> fredPointages, RvgPointagesAndPrimes rvgPointagesAndPrimes, string codeSocietePaye)
        {
            // Le champ CUSER (le nom utilisateur) est limité à 10 caractères dans la requête AS400
            if (nomUtilisateur.Length > 10)
            {
                nomUtilisateur = nomUtilisateur.Substring(0, 10);
            }

            (IEnumerable<string> fredPointageQueries, IEnumerable<string> fredPrimeQueries) = GetPointageAndPrimeQueries(jobId, periode, nomUtilisateur, fredPointages, codeSocietePaye);
            (IEnumerable<string> rvgPointageQueries, IEnumerable<string> rvgPrimeQueries) = GetPointageAndPrimeQueries(jobId, periode, nomUtilisateur, rvgPointagesAndPrimes, codeSocietePaye);

            IEnumerable<string> pointageQueries = fredPointageQueries.Concat(rvgPointageQueries);
            IEnumerable<string> primeQueries = fredPrimeQueries.Concat(rvgPrimeQueries);

            return (pointageQueries, primeQueries);
        }

        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400
        /// </summary>
        /// <param name="jobId">Identifiant du job hangfire</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="nomUtilisateur">Nom de l'utilisateur ayant lancé l'opération</param>
        /// <param name="fredPointages">Liste des pointages FRED à contrôler</param>
        /// <param name="codeSocietePaye">Code Société Paye sélectionnée où l'on effectue le contrôle</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes FRED dans l'AS400</returns>
        private (IEnumerable<string>, IEnumerable<string>) GetPointageAndPrimeQueries(string jobId, DateTime periode, string nomUtilisateur, IEnumerable<RapportLigneEnt> fredPointages, string codeSocietePaye)
        {
            if (fredPointages == null)
                return (Enumerable.Empty<string>(), Enumerable.Empty<string>());

            IEnumerable<string> pointageQueries = fredPointages.Select(GetPointageInsertQuery);
            IEnumerable<string> primeQueries = fredPointages.SelectMany(pointage => pointage.ListRapportLignePrimes.Select(prime => GetPrimeInsertQuery(pointage, prime)));

            return (pointageQueries, primeQueries);

            #region Local functions

            string GetPointageInsertQuery(RapportLigneEnt pointage)
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

            string GetPrimeInsertQuery(RapportLigneEnt pointage, RapportLignePrimeEnt rapportLignePrime)
            {
                string unite;
                string heure;

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

        /// <summary>
        /// Retourne les requêtes à utiliser pour l'insertion des pointages et des primes RVG dans l'AS400
        /// </summary>
        /// <param name="jobId">Identifiant du job hangfire</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="nomUtilisateur">Nom de l'utilisateur ayant lancé l'opération</param>
        /// <param name="rvgPointagesAndPrimes">Les pointages et les primes de RVG. Peut être null</param>
        /// <param name="codeSocietePaye">Code Société Paye sélectionnée où l'on effectue le contrôle</param>
        /// <returns>Les requêtes à utiliser pour l'insertion des pointages et des primes RVG dans l'AS400</returns>
        private (IEnumerable<string>, IEnumerable<string>) GetPointageAndPrimeQueries(string jobId, DateTime periode, string nomUtilisateur, RvgPointagesAndPrimes rvgPointagesAndPrimes, string codeSocietePaye)
        {
            if (rvgPointagesAndPrimes == null)
                return (Enumerable.Empty<string>(), Enumerable.Empty<string>());

            IEnumerable<string> pointageQueries = rvgPointagesAndPrimes.Pointages.Select(GetPointageInsertQuery);
            IEnumerable<string> primeQueries = rvgPointagesAndPrimes.Primes.Select(GetPrimeInsertQuery);

            return (pointageQueries, primeQueries);

            #region Local functions

            string GetPointageInsertQuery(RvgPointage pointage)
            {
                string numLot = string.Concat(codeSocietePaye, periode.ToLocalTime().Year, periode.ToLocalTime().Month.ToString("00"));

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
                    jobId.PadLeft(10, ' '),
                    1, // "RAPMOD" : "est modifié" : mettre à 1 pour l'instant
                    pointage.CodeZoneDeplacement,
                    pointage.CodeMajoration,
                    nomUtilisateur);
            }

            string GetPrimeInsertQuery(RvgPrime prime)
            {
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
                    jobId.PadLeft(10, ' '));
            }

            #endregion
        }

        #endregion

        #region Autre

        /// <summary>
        ///   Gestion des erreurs
        /// </summary>
        /// <typeparam name="T">ControlePointageEnt ou RemonteeVracEnt</typeparam>
        /// <param name="e">Exception</param>
        /// <param name="message">Message d'erreur fonctionnel</param>
        /// <param name="instance">Représente soit ControlePointageEnt ou RemonteeVracEnt</param>
        private void HandleErrors<T>(Exception e, string message, T instance) where T : class
        {
            NLog.LogManager.GetCurrentClassLogger().Error(e, message);

            switch (instance)
            {
                case ControlePointageEnt controlePointageEnt:
                    controlePointageEnt.DateFin = DateTime.UtcNow;
                    controlePointageManager.UpdateControlePointage(controlePointageEnt, FluxStatus.Failed.ToIntValue());
                    break;
                case RemonteeVracEnt remonteeVracEnt:
                    remonteeVracEnt.DateFin = DateTime.UtcNow;
                    remonteeVracManager.UpdateRemonteeVrac(remonteeVracEnt, FluxStatus.Failed.ToIntValue());
                    break;
                default:
                    throw new FredBusinessException("Cette instance n'est pas reconnu");
            }

            throw new FredBusinessException(message, e);
        }

        /// <summary>
        ///   Concaténation des codes établissements de paie
        /// </summary>
        /// <param name="etablissementPaieIdList">Liste d'identifiant des établissements de paie</param>
        /// <returns>Concaténation des codes sous forme de chaine de caractères</returns>
        public string ConcatEtablissementPaieCode(IEnumerable<int> etablissementPaieIdList)
        {
            var etsCodeList = new StringBuilder();

            if (etablissementPaieIdList?.Any() == true)
            {
                foreach (var etsPaieId in etablissementPaieIdList.ToList())
                {
                    var ets = this.etsPaieManager.GetEtablissementPaieById(etsPaieId);
                    if (ets != null)
                    {
                        etsCodeList.Append(ets.Code.Trim());
                    }
                }
            }
            return etsCodeList.ToString();
        }

        #endregion
    }
}
