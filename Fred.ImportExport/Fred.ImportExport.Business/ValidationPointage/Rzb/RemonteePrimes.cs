using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.Notification;
using Fred.Business.RapportPrime;
using Fred.Entities;
using Fred.Entities.Notification;
using Fred.Entities.RapportPrime;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.DataAccess.Common;
using Fred.Web.Shared.App_LocalResources;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Rzb
{
    public class RemonteePrimes
    {
        private const string CallRemonteePrimeQuery = "INSERT INTO INTRB.PRIMNEWR (PRJOUP, PRMOIP, PRANNP, PRAFF, PRMATR, PRMOIC, PRANNC, PRPRIM, PRPRIL, PRQTE, PRUNI, NOLOP) VALUES('{0}','{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}')";

        private readonly IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager;
        private readonly IRapportPrimeManager rapportPrimeManager;
        private readonly INotificationManager notificationManager;
        private readonly string chaineConnexionAnael = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];

        public RemonteePrimes(
            IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager,
            IRapportPrimeManager rapportPrimeManager,
            INotificationManager notificationManager)
        {
            this.rapportPrimeLignePrimeManager = rapportPrimeLignePrimeManager;
            this.rapportPrimeManager = rapportPrimeManager;
            this.notificationManager = notificationManager;
        }

        /// <summary>
        /// Execute la remontée des primes
        /// </summary>
        /// <param name="periode">Periode</param>
        /// <param name="utilisateurId">Utilisateur qui lance la remontée</param>
        public void Execute(DateTime periode, int utilisateurId)
        {
            BackgroundJob.Enqueue(() => RemonteePrimesJob(periode, utilisateurId, null));

        }

        [AutomaticRetry(Attempts = 0)]
        public async Task RemonteePrimesJob(DateTime periode, int utilisateurId, PerformContext context)
        {
            var parameter = new RemonteePrimesJobParameter { Periode = periode, UtilisateurId = utilisateurId, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("RemonteePrimesJob", Constantes.CodeGroupeRZB, parameter);
        }

        public async Task RemonteePrimesJobAsync(RemonteePrimesJobParameter parameter)
        {
            DateTime periode = parameter.Periode;
            int utilisateurId = parameter.UtilisateurId;
            string backgroundJobId = parameter.BackgroundJobId;

            var listRapportPrimeLignePrimeEnt = new List<RapportPrimeLignePrimeEnt>();
            //Etape 1 :  Récupération des rapports de primes pour le mois courant
            RapportPrimeEnt rapport = rapportPrimeManager.GetRapportPrime(periode, utilisateurId);

            //Etape 2 : Récuépration des lignes de primes du rapport de primes qui sont validées.
            if (rapport != null)
            {
                listRapportPrimeLignePrimeEnt = GetSendablesLines(rapport).Where(q => !q.IsSendToAnael).ToList();
            }

            //Etape 3 : Envoie des lignes vers Anael
            SendLinesToAnael(listRapportPrimeLignePrimeEnt, backgroundJobId);

            //Etape 4 : Mise à jour des lignes envoyées vers ANAEL
            UpdateIsSendToAnael(listRapportPrimeLignePrimeEnt.ToList());

            //Etape 5 : Envoie du mail
            PrimesMail.Send(listRapportPrimeLignePrimeEnt, backgroundJobId);

            await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_RemontePrimeJobSuccess, periode.ToLocalTime()));
        }


        /// <summary>
        /// Permet de mettre à jour les lignes qui viennent d'être envoyées vers ANAEL
        /// </summary>
        /// <param name="list">Liste de <see cref="RapportPrimeLignePrimeEnt"/></param>
        private void UpdateIsSendToAnael(List<RapportPrimeLignePrimeEnt> list)
        {
            if (list.Any())
            {
                list.ForEach(item =>
                {
                    item.IsSendToAnael = true;
                    var fieldToUpdate = new List<Expression<Func<RapportPrimeLignePrimeEnt, object>>> { x => x.IsSendToAnael };
                    rapportPrimeLignePrimeManager.QuickUpdate(item, fieldToUpdate);
                });

                rapportPrimeLignePrimeManager.Save();
            }
        }

        /// <summary>
        /// Envoies les lignes de rapports de primes vers Anael
        /// </summary>
        /// <param name="listRapportPrimeLignePrimeEnt">Liste des lignes de rapport de prime vers ANAEL</param>
        /// <param name="jobId">Identifiant du job Hangfire</param>
        private void SendLinesToAnael(List<RapportPrimeLignePrimeEnt> listRapportPrimeLignePrimeEnt, string jobId)
        {
            var querys = new List<string>();

            listRapportPrimeLignePrimeEnt.ForEach(ligne => querys.Add(BuildSqlQuery(ligne, jobId)));
            if (listRapportPrimeLignePrimeEnt.Any())
            {
                using (DataAccess.Common.Database destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    ExecuteAnaelQuery(querys, destinationDatabase);
                }
            }

        }

        /// <summary>
        /// Construis une requête à destination d'ANAEL a partir d'une ligne de prime d'un rapport de prime
        /// </summary>
        /// <param name="ligne"><see cref="RapportPrimeLignePrimeEnt"/></param>
        /// <param name="jobId">Identifiant du job Hangfire</param>
        /// <returns>Requête SQL</returns>
        private string BuildSqlQuery(RapportPrimeLignePrimeEnt ligne, string jobId)
        {
            int dayRapport = ligne.RapportPrimeLigne.DateCreation.Value.Day;
            int monthRapport = ligne.RapportPrimeLigne.DateCreation.Value.Month;
            int yearRapport = ligne.RapportPrimeLigne.DateCreation.Value.Year;
            string ciCode = ligne.RapportPrimeLigne.Ci.Code;
            string matricule = ligne.RapportPrimeLigne.Personnel.Matricule;
            int month = ligne.RapportPrimeLigne.DateCreation.Value.Month;
            int year = ligne.RapportPrimeLigne.DateCreation.Value.Year;
            string primeCode = ligne.Prime.Code;
            string primeLibelle = ligne.Prime.Libelle;
            double? montantPrime = ligne.Montant;
            var type = "Mois";
            return string.Format(CallRemonteePrimeQuery, dayRapport, monthRapport, yearRapport, ciCode, matricule, month, year, primeCode, primeLibelle, montantPrime.ToString().Replace(",", "."), type, jobId.PadLeft(10, ' '));
        }

        /// <summary>
        /// Execute une liste de requête SQL vers ANAEL
        /// </summary>
        /// <param name="querys">Liste des requêtes SQL</param>
        /// <param name="destinationDatabase">BDD de destination</param>
        private void ExecuteAnaelQuery(List<string> querys, DataAccess.Common.Database destinationDatabase)
        {
            try
            {
                querys.ForEach(q => destinationDatabase.ExecuteNonQuery(q.Replace("'", "\'")));
            }
            catch (Exception e)
            {
                var errorMsg = "Erreur lors de l'appel à la procédure AS400 de la Remontée prime";
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                throw new FredBusinessException(errorMsg, e);
            }
            finally
            {
                destinationDatabase.Dispose();
            }

        }

        /// <summary>
        /// Retourne la liste des lignes evnoyable vers ANAEL
        /// </summary>
        /// <param name="rapport">Rapport de Primes</param>
        /// <returns>Liste des lignes de prime qui seront envoyées vers ANAEL</returns>
        private List<RapportPrimeLignePrimeEnt> GetSendablesLines(RapportPrimeEnt rapport)
        {
            var listrapportPrimeLignePrimes = new List<RapportPrimeLignePrimeEnt>();
            foreach (RapportPrimeLigneEnt ligne in rapport.ListLignes)
            {
                foreach (RapportPrimeLignePrimeEnt lignePrime in ligne.ListPrimes)
                {
                    if (rapportPrimeLignePrimeManager.CanBeSendToAnael(lignePrime.RapportPrimeLigne) && !lignePrime.Montant.Value.Equals(0))
                    {
                        listrapportPrimeLignePrimes.Add(lignePrime);
                    }
                }
            }
            return listrapportPrimeLignePrimes;
        }
    }
}
