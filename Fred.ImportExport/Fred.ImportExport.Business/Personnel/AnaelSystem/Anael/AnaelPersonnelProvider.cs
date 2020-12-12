using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using Fred.Entities;
using Fred.Entities.Societe;
using Fred.Framework.Tool;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Personnel;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Anael
{
    public class AnaelPersonnelProvider
    {
        private readonly string chaineConnexionAnael = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];
        private readonly string grzbSqlScriptPath = "Personnel.AnaelSystem.Anael.GRZB.SELECT_PERSONNEL_GRZB.sql";
        private readonly string ftpSqlScriptPath = "Personnel.AnaelSystem.Anael.GFTP.SELECT_PERSONNEL_GFTP.sql";
        /// <summary>
        ///   Récupération des personnels d'AS400 
        /// </summary>   
        /// <param name="groupeCode">le code du groupe parent</param>
        /// <param name="societe">La societe du personnel</param>
        /// <param name="isFullImport">Import complet ou non</param>
        /// <param name="dateDerniereExecution">La date de derniere execution du flux</param>
        /// <returns>Liste des personnels</returns>
        public List<PersonnelAnaelModel> GetPersonnelFromAnael(string groupeCode, SocieteEnt societe, bool isFullImport, DateTime? dateDerniereExecution)
        {
            var personnelModels = new List<PersonnelAnaelModel>();
            try
            {
                //Connexion à la base de données source(ANAËL FINANCE)
                using (var sourceDB = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    int byPassFiltreDate = 1;
                    string dateComparaison = "0";

                    // Test d'existence d'une date de dernière exécution
                    if (!isFullImport && dateDerniereExecution.HasValue)
                    {
                        byPassFiltreDate = 0;
                        dateComparaison = dateDerniereExecution.Value.ToLocalTime().ToString("yyyyMMddHHmm");
                    }

                    string query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), GetScript(groupeCode));
                    query = string.Format(query, societe.CodeSocietePaye, byPassFiltreDate, dateComparaison);

                    using (var results = sourceDB.ExecuteReader(query))
                    {
                        while (results.Read())
                        {
                            personnelModels.Add(ReadLine(results));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FredIeBusinessException("Erreur lors de la récuperation des personnels d'Anael", ex);
            }

            return personnelModels;
        }

        /// <summary>
        /// Lis une ligne de données retournée par Anael.
        /// </summary>
        /// <param name="line">Une ligne dans la base Anael.</param>
        /// <returns>Une entité ci.</returns>
        private PersonnelAnaelModel ReadLine(IDataReader line)
        {
            return new PersonnelAnaelModel()
            {
                CodeSocietePaye = Convert.ToString(line["CodeSocietePaye"]).Trim(),
                CodeSocieteCompta = Convert.ToString(line["CodeSocieteCompta"]).Trim(),
                CodeEtablissement = Convert.ToString(line["CodeEtablissement"]).Trim(),
                Matricule = Convert.ToString(line["Matricule"]).Trim(),
                Nom = Convert.ToString(line["Nom"]).Trim(),
                Prenom = Convert.ToString(line["Prenom"]).Trim(),
                CategoriePerso = Convert.ToString(line["CategoriePerso"]).Trim(),
                Statut = Convert.ToString(line["Statut"]).Trim(),
                CodeEmploi = Convert.ToString(line["CodeEmploi"]).Trim(),
                NumeroRue = Convert.ToString(line["NumeroRue"]).Trim(),
                TypeRue = Convert.ToString(line["TypeRue"]).Trim(),
                NomRue = Convert.ToString(line["NomRue"]).Trim(),
                NomLieuDit = Convert.ToString(line["NomLieuDit"]).Trim(),
                CodePostal = Convert.ToString(line["CodePostal"]).Trim(),
                Ville = Convert.ToString(line["Ville"]).Trim(),
                DateEntree = line["DateEntree"] != DBNull.Value ? Convert.ToDateTime(line["DateEntree"]) : default(DateTime?),
                DateSortie = line["DateSortie"] != DBNull.Value ? Convert.ToDateTime(line["DateSortie"]) : default(DateTime?),
                DateModification = line["DateModification"] != DBNull.Value ? Convert.ToDateTime(line["DateModification"]) : default(DateTime?),
                CodePays = Convert.ToString(line["CodePays"]).Trim()
            };
        }

        private string GetScript(string groupeCode)
        {
            switch (groupeCode)
            {
                case Constantes.CodeGroupeRZB:
                    return grzbSqlScriptPath;
                case Constantes.CodeGroupeFTP:
                    return ftpSqlScriptPath;
            }

            throw new FredIeBusinessException("Le systeme d'import du personnel n'est pas prevu pour fonctionner avec ce groupe : " + groupeCode);
        }
    }
}
