using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.Rapport;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Common.Interfaces;
using Fred.ImportExport.DataAccess.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.Common
{
    public class FtpQueryExecutor : IFtpQueryExecutor
    {
        private readonly IFtpQueryBuilder ftpQueryBuilder;
        private readonly IControlePointageManager controlePointageManager;
        private readonly IRemonteeVracManager remonteeVracManager;

        public FtpQueryExecutor(IFtpQueryBuilder ftpQueryBuilder, IControlePointageManager controlePointageManager, IRemonteeVracManager remonteeVracManager)
        {
            this.ftpQueryBuilder = ftpQueryBuilder;
            this.controlePointageManager = controlePointageManager;
            this.remonteeVracManager = remonteeVracManager;
        }

        /// <summary>
        ///   Déversement des lignes de pointages et des primes dans l'AS400
        /// </summary>
        /// <typeparam name="T">ControlePointageEnt ou RemonteeVracEnt</typeparam>    
        /// <param name="globalData">Données globales</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="fredPointages">Liste des pointages FRED à contrôler</param>
        /// <param name="rvgPointagesAndPrimes">Les pointages et les primes de RVG. Peut être null.</param>
        /// <param name="instance">Représente soit ControlePointageEnt ou RemonteeVracEnt</param>       
        public void InsertPointageAndPrime<T>(ValidationPointageContextData globalData, DateTime periode, IEnumerable<RapportLigneEnt> fredPointages, RvgPointagesAndPrimes rvgPointagesAndPrimes, T instance) where T : class
        {
            try
            {
                using (DataAccess.Common.Database destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, globalData.ConnexionChaineSource))
                {
                    (IEnumerable<string> pointageQueries, IEnumerable<string> primesQueries) = ftpQueryBuilder.GetPointageAndPrimeQueries(globalData, periode, fredPointages, rvgPointagesAndPrimes);

                    SendInsertQueries(destinationDatabase, FtpQueryBuilder.InsertPointageClause, pointageQueries);
                    SendInsertQueries(destinationDatabase, FtpQueryBuilder.InsertPrimeClause, primesQueries);
                }
            }
            catch (Exception e)
            {
                var errorMsg = "Erreur lors du déversement des pointages et des primes dans AS400.";
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

            if (instance is ControlePointageEnt)
            {
                (instance as ControlePointageEnt).DateFin = DateTime.UtcNow;
                controlePointageManager.UpdateControlePointage(instance as ControlePointageEnt, FluxStatus.Failed.ToIntValue());
            }
            else if (instance is RemonteeVracEnt)
            {
                (instance as RemonteeVracEnt).DateFin = DateTime.UtcNow;
                remonteeVracManager.UpdateRemonteeVrac(instance as RemonteeVracEnt, FluxStatus.Failed.ToIntValue());
            }
            else
            {
                throw new FredBusinessException("Cette instance n'est pas reconnu");
            }

            throw new FredBusinessException(message, e);
        }
    }
}
