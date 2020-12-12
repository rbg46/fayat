using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using Fred.Entities;
using Fred.Entities.Personnel.Imports;
using Fred.Framework.Tool;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Personnel.Etl.Process;
using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Exceptions;

namespace Fred.ImportExport.Business.Personnel.Etl.Input
{
    public class PersonnelInputFon : IEtlInput<PersonnelModel>
    {
        private readonly string chaineConnexionAnael = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GFON"];
        private readonly PersonnelEtlParameter personnelFluxParameter;

        public PersonnelInputFon(PersonnelEtlParameter personnelFluxParameter)
        {
            this.personnelFluxParameter = personnelFluxParameter;
        }

        /// <summary>
        /// Contient le resultat de l'importation Anael
        /// </summary>
        public IList<PersonnelModel> Items { get; set; }

        /// <inheritdoc/>
        /// Appelé par l'ETL
        public void Execute()
        {
            if (!string.IsNullOrEmpty(chaineConnexionAnael))
            {
                Items = GetPersonnelFromAnael();
            }
            else
            {
                Items = new List<PersonnelModel>();
            }
        }

        /// <summary>
        ///   Récupération des personnels d'AS400 
        /// </summary>   
        /// <returns>Liste des personnels</returns>
        public IList<PersonnelModel> GetPersonnelFromAnael()
        {
            bool bypassDate = this.personnelFluxParameter.ByPassDate;

            var codeSocietePayes = PersonnelCodeSocietePayeSpliter.GetCodeSocietePayesInFlux(personnelFluxParameter.Flux);

            string codePayesSql = codeSocietePayes.Aggregate(string.Empty, (soFar, next) =>
                                        string.Format("{0}{1}'{2}'",
                                        soFar, soFar == string.Empty ? soFar : ", ", next));

            string sqlScriptPath = this.personnelFluxParameter.SqlScriptPath;
            DateTime? dateDerniereExecution = this.personnelFluxParameter.Flux.DateDerniereExecution;

            var personnelModels = new List<PersonnelModel>();
            try
            {
                //Connexion à la base de données source(ANAËL FINANCE)
                using (var sourceDB = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    int byPassFiltreDate = 1;
                    string dateComparaison = "0";

                    // Test d'existence d'une date de dernière exécution
                    if (!bypassDate && dateDerniereExecution.HasValue)
                    {
                        byPassFiltreDate = 0;
                        dateComparaison = dateDerniereExecution.Value.ToLocalTime().ToString("yyyyMMddHHmm");
                    }
                    string query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), sqlScriptPath);
                    query = string.Format(query, codePayesSql, byPassFiltreDate, dateComparaison);

                    var results = sourceDB.ExecuteReader(query);
                    while (results.Read())
                    {
                        personnelModels.Add(ReadLine(results));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FredIeBusinessException(string.Format(IEBusiness.FluxErreurRecuperation, this.personnelFluxParameter.CodeFlux), ex);
            }

            return personnelModels;
        }

        /// <summary>
        /// Lis une ligne de données retournée par Anael.
        /// </summary>
        /// <param name="line">Une ligne dans la base Anael.</param>
        /// <returns>Une entité ci.</returns>
        protected virtual PersonnelModel ReadLine(IDataReader line)
        {
            return new PersonnelModel()
            {
                CodeSocietePaye = Convert.ToString(line["CodeSocietePaye"]).Trim(),
                CodeEtablissement = Convert.ToString(line["CodeEtablissement"]).Trim(),
                Matricule = Convert.ToString(line["Matricule"]).Trim(),
                Nom = Convert.ToString(line["Nom"]).Trim(),
                Prenom = Convert.ToString(line["Prenom"]).Trim(),
                CategoriePerso = Convert.ToString(line["CategoriePerso"]).Trim(),
                Statut = Convert.ToString(line["Statut"]).Trim(),
                CodeEmploi = Convert.ToString(line["CodeEmploi"]).Trim(),
                DateEntree = line["DateEntree"] != DBNull.Value ? Convert.ToDateTime(line["DateEntree"]) : default(DateTime?),
                DateSortie = line["DateSortie"] != DBNull.Value ? Convert.ToDateTime(line["DateSortie"]) : default(DateTime?),
                DateModification = line["DateModification"] != DBNull.Value ? Convert.ToDateTime(line["DateModification"]) : default(DateTime?),
                Email = Convert.ToString(line["Email"]).Trim(),
                Section = Convert.ToString(line["Section"]).Trim(),
                Entreprise = Convert.ToString(line["Entreprise"]).Trim(),
                SocieteManager = Convert.ToString(line["SocieteManager"]).Trim(),
                MatriculeManager = Convert.ToString(line["MatriculeManager"]).Trim(),
            };
        }
    }
}
