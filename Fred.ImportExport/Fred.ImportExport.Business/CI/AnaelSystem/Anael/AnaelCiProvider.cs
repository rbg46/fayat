using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using Fred.Entities;
using Fred.Framework.Tool;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Anael
{
    public class AnaelCiProvider
    {
        private readonly string sqlScriptPathSelectByCodeSocietComptableAndCiCode = "CI.AnaelSystem.Anael.GRZB.Sql.SELECT_CI_BY_CODE_CI.sql";
        private readonly string sqlScriptPathSelectBySosieteComptableAndDate = "CI.AnaelSystem.Anael.GRZB.Sql.SELECT_CI_BY_CODE_SOCIETE.sql";
        private readonly string chaineConnexionAnael = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];

        /// <summary>
        ///  Récupération des CI d'Anael
        /// </summary>
        /// <param name="codeSocieteComptable">codeSocieteComptable exemple :  '1000' pour la societe RB ou '550' pour MBTP</param>
        /// <param name="codesCis">Liste de code CI, exemple :  '234952','234953'</param>
        /// <param name="applyFilterOnResult">Permet de savoir si on applique la filtre sur les données recue d'anael</param>        
        /// <returns>Liste des CI</returns>
        public List<CiAnaelModel> GetCisFromAnael(string codeSocieteComptable, List<string> codesCis, bool applyFilterOnResult)
        {
            // exemple de resultat de listCodesCisQuery: '234952','234953'
            var listCodesCisQuery = codesCis.Select(x => "'" + x + "'").Aggregate((accumulator, piece) => accumulator + "," + piece);

            var getAllCis = "0";//=> = false 

            var scriptSqlFormat = GetSqlFileContent(sqlScriptPathSelectByCodeSocietComptableAndCiCode);

            var query = string.Format(scriptSqlFormat, codeSocieteComptable, getAllCis, listCodesCisQuery);

            var ciModels = InternalGetCIFromAnael(query);

            if (applyFilterOnResult)
            {
                ciModels = ciModels.Where(x => CanAddAnaelCi(x)).ToList();
            }

            return ciModels;
        }

        /// <summary>
        /// Récupération des CI d'Anael
        /// </summary>
        /// <param name="codeSocieteComptable">codeSocieteComptable exemple :  '1000' pour la societe RB ou '550' pour MBTP</param>
        /// <param name="isFullImport">Es ce que l'import est complet</param>
        /// <param name="dateDerniereExecution">Date de derniere execution</param>
        /// <param name="applyFilterOnResult">Permet de savoir si on applique la filtre sur les données recue d'anael</param>        
        /// <returns>Liste des CI de la societe</returns>
        public List<CiAnaelModel> GetCisFromAnael(string codeSocieteComptable, bool isFullImport, DateTime? dateDerniereExecution, bool applyFilterOnResult)
        {
            int byPassFiltreDate = 1;

            string dateComparaison = "0";

            if (!isFullImport && dateDerniereExecution.HasValue)
            {
                byPassFiltreDate = 0;
                dateComparaison = dateDerniereExecution.Value.ToLocalTime().ToString("yyyyMMddHHmm");
            }

            var scriptSql = GetSqlFileContent(sqlScriptPathSelectBySosieteComptableAndDate);

            var query = string.Format(scriptSql, codeSocieteComptable, byPassFiltreDate, dateComparaison);

            var ciModels = InternalGetCIFromAnael(query);

            if (applyFilterOnResult)
            {
                ciModels = ciModels.Where(x => CanAddAnaelCi(x)).ToList();
            }

            return ciModels;
        }

        private string GetSqlFileContent(string path)
        {
            return SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), path);
        }

        private List<CiAnaelModel> InternalGetCIFromAnael(string query)
        {
            var ciModels = new List<CiAnaelModel>();
            try
            {
                // Connexion à la base de données source (ANAËL FINANCE)
                using (var sourceDB = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    // Récupération des CI
                    var resultsCI = sourceDB.ExecuteReader(query);
                    while (resultsCI.Read())
                    {
                        var ci = ReadLine(resultsCI);
                        ciModels.Add(ci);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FredIeBusinessException("Erreur lors de la recuperation des ci de Anael", ex);
            }

            return ciModels;
        }



        private bool CanAddAnaelCi(CiAnaelModel ci)
        {
            if (ci.ChantierFRED == "01")
            {
                return true;
            }
            else
            {
                // TFS 6713 
                if (!string.IsNullOrEmpty(ci.CodeAffaire) && ci.CodeAffaire.StartsWith("3"))
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(ci.CodeAffaire) && ci.CodeAffaire.StartsWith("4"))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Lis une ligne de données retournée par Anael.
        /// </summary>
        /// <param name="line">Une ligne dans la base Anael.</param>
        /// <returns>Une entité ci.</returns>
        private CiAnaelModel ReadLine(IDataReader line)
        {
            return new CiAnaelModel()
            {
                CodeSociete = Convert.ToString(line["CodeSociete"]).Trim(),
                CodeEtablissement = Convert.ToString(line["CodeEtablissement"]).Trim(),
                CodeAffaire = Convert.ToString(line["CodeAffaire"]).Trim(),
                LibelleLong = Convert.ToString(line["LibelleLong"]).Trim(),
                Libelle = Convert.ToString(line["Libelle"]).Trim(),
                ChantierFRED = Convert.ToString(line["ChantierFRED"]).Trim(),
                DateOuverture = line["DateOuverture"] != DBNull.Value ? Convert.ToDateTime(line["DateOuverture"]) : default(DateTime?),
                DateFermeture = line["DateFermeture"] != DBNull.Value ? Convert.ToDateTime(line["DateFermeture"]) : default(DateTime?)
            };
        }

    }
}
