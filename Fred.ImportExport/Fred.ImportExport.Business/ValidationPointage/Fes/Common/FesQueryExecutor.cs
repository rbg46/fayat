using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.Logs;
using Fred.ImportExport.DataAccess.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.Common
{
    public class FesQueryExecutor : IFesQueryExecutor
    {
        private readonly IControlePointageManager controlePointageManager;
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly ValidationPointageFesLogger logger;

        public FesQueryExecutor(
            IControlePointageManager controlePointageManager,
            IRemonteeVracManager remonteeVracManager,
            ValidationPointageFesLogger logger)
        {
            this.controlePointageManager = controlePointageManager;
            this.remonteeVracManager = remonteeVracManager;
            this.logger = logger;
        }

        #region  Insertion des données dans l'AS400

        public void ExecuteAnaelInserts<T>(ValidationPointageContextData globalData, IEnumerable<QueryInfo> pointagesAndPrimesQueries, T instance) where T : class
        {
            try
            {
                using (DataAccess.Common.Database destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, globalData.ConnexionChaineSource))
                {
                    SendInsertQueries(destinationDatabase, FesQueryBuilder.InsertPointageClause, pointagesAndPrimesQueries.Where(q => q.IsPointage));
                    SendInsertQueries(destinationDatabase, FesQueryBuilder.InsertPrimeClause, pointagesAndPrimesQueries.Where(q => !q.IsPointage));
                }
            }
            catch (Exception e)
            {
                string errorMsg = this.logger.ErrorInInsertPointagesAndPrimes(e);
                HandleErrors(e, errorMsg, instance);
            }

            #region Local functions

            void SendInsertQueries(DataAccess.Common.Database destinationDatabase, string insertClause, IEnumerable<QueryInfo> queriesInfos)
            {
                for (var i = 0; i < queriesInfos.Count(); i += 1000)
                {
                    IEnumerable<QueryInfo> queries = queriesInfos.Skip(i).Take(1000).ToList();

                    queries.ForEach(q => q.Query = q.Query.Substring(insertClause.Length));

                    string valuesClauses = string.Join($",{Environment.NewLine}", queries.Select(q => q.Query));
                    string insertQuery = string.Concat(insertClause, Environment.NewLine, valuesClauses);

                    destinationDatabase.ExecuteNonQuery(insertQuery);
                }
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
        #endregion
    }
}
