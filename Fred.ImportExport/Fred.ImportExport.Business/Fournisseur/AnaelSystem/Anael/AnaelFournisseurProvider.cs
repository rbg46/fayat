using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using Fred.Entities;
using Fred.Framework.Tool;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Anael
{
    public class AnaelFournisseurProvider
    {
        private readonly string sqlScriptPathSelectByCodeSocietComptableAndFournisseurCode = "Fournisseur.AnaelSystem.Anael.GRZB.Sql.SELECT_FOURNISSEUR_BY_CODE.sql";
        private readonly string sqlScriptPathSelectBySosieteComptableAndDate = "Fournisseur.AnaelSystem.Anael.GRZB.Sql.SELECT_FOURNISSEUR_BY_DATE.sql";
        private readonly string chaineConnexionAnael = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];

        /// <summary>
        ///   Obtient l'identifiant du job d'import (Code du flux d'import)
        /// </summary>
        private string ImportJobId { get; } = ConfigurationManager.AppSettings["flux:fournisseur"];

        /// <summary>
        ///   Récupération des fournisseurs d'AS400 
        /// </summary>
        /// <param name="codeSocieteComptable">codeSocieteComptable exemple :  '1000' pour la societe RB ou '550' pour MBTP</param>
        /// <param name="typeSequences">type de sequence/param>
        /// <param name="regleGestion">regle de gestion</param>
        /// <param name="codesFournisseurs">code fournisseurs</param>  
        /// <param name="modelSociete">model de societe</param>
        /// <returns>Liste des fournisseurs</returns>
        public List<FournisseurAnaelModel> GetFournisseurFromAnael(string codeSocieteComptable, List<string> typeSequences, string regleGestion, List<string> codesFournisseurs, string modelSociete)
        {
            // exemple de resultat de listquery: '234952','234953'
            var listCodesFournisseursQuery = codesFournisseurs.Select(x => "'" + x + "'").Aggregate((accumulator, piece) => accumulator + "," + piece);
            var listTypeSequenceQuery = typeSequences.Select(x => "'" + x + "'").Aggregate((accumulator, piece) => accumulator + "," + piece);
            var societeFilter = "'" + codeSocieteComptable + "','" + modelSociete + "'";

            var scriptSqlFormat = GetSqlFileContent(sqlScriptPathSelectByCodeSocietComptableAndFournisseurCode);

            var query = string.Format(scriptSqlFormat, societeFilter, listTypeSequenceQuery, regleGestion, listCodesFournisseursQuery);

            var fournisseurModels = GetInternalFournisseurFromAnael(query);

            return fournisseurModels;
        }

        /// <summary>
        /// Récupération des CI d'Anael
        /// </summary>
        /// <param name="codeSocieteComptable">codeSocieteComptable exemple :  '1000' pour la societe RB ou '550' pour MBTP</param>
        /// <param name="isFullImport">Es ce que l'import est complet</param>
        /// <param name="dateDerniereExecution">Date de derniere execution</param>
        /// <param name="applyFilterOnResult">Permet de savoir si on applique la filtre sur les données recue d'anael</param>        
        /// <returns>Liste des CI de la societe</returns>
        public List<FournisseurAnaelModel> GetFournisseurFromAnael(string codeSocieteComptable, bool isFullImport, DateTime? dateDerniereExecution, string typeSequences, string regleGestion)
        {
            int byPassFiltreDate = 1;

            string dateComparaison = "0";

            if (!isFullImport && dateDerniereExecution.HasValue)
            {
                byPassFiltreDate = 0;
                dateComparaison = dateDerniereExecution.Value.ToLocalTime().ToString("yyyyMMddHHmm");
            }

            // exemple de resultat de listquery: '234952','234953'
            var listTypeSequenceQuery = typeSequences.Select(x => "'" + x + "'").Aggregate((accumulator, piece) => accumulator + "," + piece);

            var scriptSql = GetSqlFileContent(sqlScriptPathSelectBySosieteComptableAndDate);

            var query = string.Format(scriptSql, codeSocieteComptable, listTypeSequenceQuery, regleGestion, byPassFiltreDate, dateComparaison);

            var fournisseurModels = GetInternalFournisseurFromAnael(query);

            return fournisseurModels;
        }

        private List<FournisseurAnaelModel> GetInternalFournisseurFromAnael(string query)
        {
            var fournisseurs = new List<FournisseurAnaelModel>();

            try
            {
                // Connexion à la base de données source (ANAËL FINANCE)
                using (var sourceDb = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    // Récupération des fournisseurs
                    using (var resultsEtab = sourceDb.ExecuteReader(query))
                    {

                        // Pour chaque élément retourné, création d'un model
                        while (resultsEtab.Read())
                        {
                            FournisseurAnaelModel etablissement = ReadLine(resultsEtab);
                            fournisseurs.Add(etablissement);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FredIeBusinessException(string.Format(IEBusiness.FluxErreurRecuperation, ImportJobId), ex);
            }

            return fournisseurs;
        }

        private string GetSqlFileContent(string path)
        {
            return SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), path);
        }

        /// <summary>
        /// Lis une ligne de données retournée par Anael
        /// </summary>
        /// <param name="line">Une ligne dans la base Anael</param>
        /// <returns>Une entité Fournisseur</returns>
        private FournisseurAnaelModel ReadLine(IDataReader line)
        {
            return new FournisseurAnaelModel()
            {
                Code = Convert.ToString(line["Code"]).Trim(),
                TypeSequence = Convert.ToString(line["TypeSequence"]).Trim(),
                Libelle = Convert.ToString(line["Libelle"]).Trim(),
                Adresse = Convert.ToString(line["Adresse"]).Trim(),
                CodePostal = Convert.ToString(line["CodePostal"]).Trim(),
                Ville = Convert.ToString(line["Ville"]).Trim(),
                Telephone = Convert.ToString(line["Telephone"]).Trim(),
                Fax = Convert.ToString(line["Fax"]).Trim(),
                Email = Convert.ToString(line["Email"]).Trim(),
                SIRET = Convert.ToString(line["SIRET"]).Trim(),
                SIREN = Convert.ToString(line["SIREN"]).Trim(),
                ModeReglement = Convert.ToString(line["ModeReglement"]).Trim(),
                RegleGestion = Convert.ToString(line["RegleGestion"]).Trim(),
                CodePays = Convert.ToString(line["CodePays"]).Trim(),
                DateOuverture = line["DateOuverture"] != DBNull.Value ? Convert.ToDateTime(line["DateOuverture"]) : default(DateTime?),
                DateCloture = line["DateFermeture"] != DBNull.Value ? Convert.ToDateTime(line["DateFermeture"]) : default(DateTime?),
                CC1 = Convert.ToString(line["CC1"]).Trim(),
                CC2 = Convert.ToString(line["CC2"]).Trim(),
                CC3 = Convert.ToString(line["CC3"]).Trim(),
                CC4 = Convert.ToString(line["CC4"]).Trim(),
                CC5 = Convert.ToString(line["CC5"]).Trim(),
                CC6 = Convert.ToString(line["CC6"]).Trim(),
                CC7 = Convert.ToString(line["CC7"]).Trim(),
                CC8 = Convert.ToString(line["CC8"]).Trim(),
                CritereRecherche = Convert.ToString(line["CritereRecherche"]).Trim(),
                IsoTVA = Convert.ToString(line["IsoTVA"]).Trim(),
                NumeroTVA = Convert.ToString(line["NumeroTVA"]).Trim(),
                CodeSociete = Convert.ToString(line["CodeSociete"]).Trim(),
                IsProfessionLiberale = Convert.ToString(line["IsProfessionLiberale"]).Trim()
            };
        }
    }
}
