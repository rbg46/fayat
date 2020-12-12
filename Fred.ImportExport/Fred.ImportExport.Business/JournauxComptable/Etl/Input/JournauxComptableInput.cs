using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using Fred.Entities;
using Fred.Framework.Tool;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.JournauxComptable;

namespace Fred.ImportExport.Business.JournauxComptable.Etl.Input
{
    /// <summary>
    /// Processus etl : Execution de l'input des JournauxComptable
    /// Va chercher dans Anael la liste des Journaux comptables
    /// </summary>
    public class JournauxComptableInput : IEtlInput<JournauxComptableAnaelModel>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="societeCode">code société du flux</param>
        /// <param name="connexionString">chaine de connexion du flux</param>
        public JournauxComptableInput(string societeCode, string connexionString)
        {
            SocieteCode = societeCode;
            ConnectionString = connexionString;
        }

        /// <summary>
        /// Obtient ou définit le code société du flux.
        /// </summary>
        private string SocieteCode { get; }

        /// <summary>
        /// Obtient ou définit la chaine de connexionString à la source de données Anael.
        /// </summary>
        private string ConnectionString { get; }

        /// <summary>
        /// Chemin du script Sql de récupération des données dans Anael
        /// </summary>
        private string SqlScriptPath => "JournauxComptable.SELECT_JOURNAUX_COMPTABLE.sql";

        /// <summary>
        /// Obtient l'identifiant du job d'import (Code du flux d'import)
        /// </summary>
        private string ImportJobId { get; } = ConfigurationManager.AppSettings["flux:journaux:comptable"];

        /// <summary>
        /// Contient le resultat de l'importation Anael
        /// </summary>
        public IList<JournauxComptableAnaelModel> Items { get; set; }

        /// <summary>
        /// Appelé par l'ETL
        /// Si besoin, permet de faire un traitement avant d'appeler les transformations
        /// (Ex : Aller chercher les datas dans une base )
        /// </summary>
        public void Execute()
        {
            Items = GetJournauxComptableFromAnael();
        }

        /// <summary>
        /// Récupération des Journaux comptables d'AS400 
        /// </summary>
        /// <returns>Liste des Journaux comptables</returns>
        public IList<JournauxComptableAnaelModel> GetJournauxComptableFromAnael()
        {
            List<JournauxComptableAnaelModel> journauxComptable = new List<JournauxComptableAnaelModel>();

            try
            {
                string query = GetQuery();
                // Connexion à la base de données source (ANAËL FINANCE)
                using (DataAccess.Common.Database sourceDb = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, ConnectionString))
                {
                    // Récupération des Journaux comptables
                    using (IDataReader resultsJournaux = sourceDb.ExecuteReader(query))
                    {
                        // Pour chaque élément retourné, création d'un model
                        while (resultsJournaux.Read())
                        {
                            JournauxComptableAnaelModel journalComptable = ReadLine(resultsJournaux);
                            journauxComptable.Add(journalComptable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FredIeBusinessException(string.Format(IEBusiness.FluxErreurRecuperation, ImportJobId), ex);
            }

            return journauxComptable;
        }

        /// <summary>
        /// Lis une ligne de données retournée par Anael
        /// </summary>
        /// <param name="line">Une ligne dans la base Anael</param>
        /// <returns>Une entité Journaux Comptable</returns>
        private JournauxComptableAnaelModel ReadLine(IDataReader line)
        {
            return new JournauxComptableAnaelModel()
            {
                CodeJournal = Convert.ToString(line["CodeJournal"]),
                CodeSociete = Convert.ToString(line["CodeSociete"]),
                NomJournal = Convert.ToString(line["NomJournal"]),
                TypeJournal = Convert.ToString(line["TypeJournal"]),
            };
        }

        /// <summary>
        /// Renvoie la requete SQL à exécuter dans Anael
        /// </summary>
        /// <returns>script Sql</returns>
        private string GetQuery()
        {
            string sql = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SqlScriptPath);
            string date = DateTime.UtcNow.Year + "-" + DateTime.UtcNow.Month + "-" + DateTime.UtcNow.Day;
            return string.Format(sql, SocieteCode, date);
        }
    }
}
