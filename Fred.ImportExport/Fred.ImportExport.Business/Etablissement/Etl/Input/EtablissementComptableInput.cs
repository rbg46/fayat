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
using Fred.ImportExport.Models.Etablissement;

namespace Fred.ImportExport.Business.Etablissement.Etl.Input
{

    /// <summary>
    /// Processus etl : Execution de l'input des EtablissementComptable
    /// Va chercher dans Anael la liste des établissements comptables
    /// </summary>
    public class EtablissementComptableInput : IEtlInput<EtablissementComptableAnaelModel>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="societeCode">code société du flux</param>
        /// <param name="connexionString">chaine de connexion du flux</param>
        public EtablissementComptableInput(string societeCode, string connexionString)
        {
            SocieteCode = societeCode;
            ConnectionString = connexionString;
        }

        /// <summary>
        ///   Obtient ou définit le code société du flux.
        /// </summary>
        private string SocieteCode { get; }

        /// <summary>
        ///   Obtient ou définit la chaine de connexionString à la source de données Anael.
        /// </summary>
        private string ConnectionString { get; }

        /// <summary>
        /// Chemin du script Sql de récupération des données dans Anael
        /// </summary>
        private string SqlScriptPath => "Etablissement.SELECT_ETABLISSEMENT_COMPTABLE.sql";

        /// <summary>
        ///   Obtient l'identifiant du job d'import (Code du flux d'import)
        /// </summary>
        private string ImportJobId { get; } = ConfigurationManager.AppSettings["flux:etablissement:comptable"];

        /// <summary>
        /// Contient le resultat de l'importation Anael
        /// </summary>
        public IList<EtablissementComptableAnaelModel> Items { get; set; }

        /// <inheritdoc/>
        /// Appelé par l'ETL
        public void Execute()
        {
            Items = GetEtablissementFromAnael();
        }

        /// <summary>
        ///   Récupération des Etablissements comptables d'AS400 
        /// </summary>
        /// <returns>Liste des Etablissements comptables</returns>
        public IList<EtablissementComptableAnaelModel> GetEtablissementFromAnael()
        {
            var etablissements = new List<EtablissementComptableAnaelModel>();

            try
            {
                int exercice = CalculExercice();
                string query = GetQuery(exercice, SocieteCode);

                // Connexion à la base de données source (ANAËL FINANCE)
                using (var sourceDb = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, ConnectionString))
                {
                    // Récupération des établissements comptables
                    using (var resultsEtab = sourceDb.ExecuteReader(query))
                    {

                        // Pour chaque élément retourné, création d'un model
                        while (resultsEtab.Read())
                        {
                            EtablissementComptableAnaelModel etablissement = ReadLine(resultsEtab);
                            etablissements.Add(etablissement);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FredIeBusinessException(string.Format(IEBusiness.FluxErreurRecuperation, ImportJobId), ex);
            }

            return etablissements;
        }

        /// <summary>
        /// Lis une ligne de données retournée par Anael
        /// </summary>
        /// <param name="line">Une ligne dans la base Anael</param>
        /// <returns>Une entité Etablissement Comptable</returns>
        private EtablissementComptableAnaelModel ReadLine(IDataReader line)
        {
            return new EtablissementComptableAnaelModel()
            {
                CodeEtablissement = Convert.ToString(line["CodeEtablissement"]),
                Libelle = Convert.ToString(line["Libelle"]),
            };
        }

        /// <summary>
        /// Calcul de l'exercice M-1
        /// </summary>
        /// <returns>exercice</returns>
        private static int CalculExercice()
        {
            // TO DO : AXFILE.FAN015P1 permet de récupérer l'exercice en fonction d'une date de début / fin
            return Convert.ToInt32((DateTime.Now.Year * 10) - 10);
        }

        /// <summary>
        /// Renvoie la requete SQL à exécuter dans Anael
        /// </summary>
        /// <param name="exercice">exercice sur lequel effectuer la recherche</param>
        /// <param name="societeCode">Code société Anael sur lequel effectuer la rechercher</param>
        /// <returns>script Sql</returns>
        private string GetQuery(int exercice, string societeCode)
        {
            string sql = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SqlScriptPath);
            string query = string.Format(sql, exercice, societeCode);
            return query;
        }
    }
}
