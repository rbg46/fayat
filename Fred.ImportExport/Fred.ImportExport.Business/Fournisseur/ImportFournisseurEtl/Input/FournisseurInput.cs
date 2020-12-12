using Fred.Entities;
using Fred.Framework.Tool;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Input
{

  /// <summary>
  /// Processus etl : Execution de l'input des Fournisseurs
  /// Va chercher dans Anael la liste des fournisseurs
  /// </summary>
  public class FournisseurInput : IEtlInput<FournisseurAnaelModel>
  {
    /// <summary>
    /// Ctor
    /// </summary>
    public FournisseurInput()
    {
    }

    /// <summary>
    ///   Obtient ou définit le code société du flux.
    /// </summary>
    public bool ByPassDate { get; set; }

    /// <summary>
    ///   Obtient ou définit le paramètre Code Société Comptable
    /// </summary>
    public string CodeSocieteComptables { get; set; }

    /// <summary>
    ///   Obtient ou définit le paramètre Règle Gestion
    /// </summary>
    public string RegleGestions { get; set; }

    /// <summary>
    ///   Obtient ou définit le paramètre Type Séquences
    /// </summary>
    public string TypeSequences { get; set; }

    /// <summary>
    ///   Obtient ou définit le Flux Fournisseur
    /// </summary>
    public FluxEnt Flux { get; set; }

    /// <summary>
    /// Chemin du script Sql de récupération des données dans Anael
    /// </summary>
    private string SqlScriptPath => "Fournisseur.SELECT_FOURNISSEUR.sql";

    /// <summary>
    ///   Obtient l'identifiant du job d'import (Code du flux d'import)
    /// </summary>
    private string ImportJobId { get; } = ConfigurationManager.AppSettings["flux:fournisseur"];

    /// <summary>
    /// Contient le resultat de l'importation Anael
    /// </summary>
    public IList<FournisseurAnaelModel> Items { get; set; }
    public int SocieteId { get; internal set; }

    /// <inheritdoc/>
    /// Appelé par l'ETL
    public void Execute()
    {
      Items = GetFournisseurFromAnael();
    }

    /// <summary>
    ///   Récupération des fournisseurs d'AS400 
    /// </summary>
    /// <returns>Liste des fournisseurs</returns>
    public IList<FournisseurAnaelModel> GetFournisseurFromAnael()
    {
      var fournisseurs = new List<FournisseurAnaelModel>();

      try
      {
        string query = GetQuery();

        // Connexion à la base de données source (ANAËL FINANCE)
        using (var sourceDb = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, Flux.ConnexionChaineSource))
        {
          // Récupération des fournisseurs
          IDataReader resultsEtab = sourceDb.ExecuteReader(query);

          // Pour chaque élément retourné, création d'un model
          while (resultsEtab.Read())
          {
            FournisseurAnaelModel etablissement = ReadLine(resultsEtab);
            fournisseurs.Add(etablissement);
          }
        }
      }
      catch (Exception ex)
      {
        throw new FredIeBusinessException(string.Format(IEBusiness.FluxErreurRecuperation, ImportJobId), ex);
      }

      return fournisseurs;
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
        CodeSociete = Convert.ToString(line["CodeSociete"]).Trim()
      };
    }

    /// <summary>
    /// Renvoie la requete SQL à exécuter dans Anael
    /// </summary>    
    /// <returns>script Sql</returns>
    private string GetQuery()
    {
      string sql = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SqlScriptPath);
      string query;
      int byPassFiltreDate = 1;
      string dateComparaison = "0";
      string societes = "'" + CodeSocieteComptables + "','" + this.Flux.SocieteModeleCode + "'";

      // Test d'existence d'une date de dernière exécution
      if (!ByPassDate && this.Flux.DateDerniereExecution.HasValue)
      {
        byPassFiltreDate = 0;
        dateComparaison = this.Flux.DateDerniereExecution.Value.ToString("yyyyMMddHHmm");
      }

      query = string.Format(sql, societes, TypeSequences, RegleGestions, byPassFiltreDate, dateComparaison);

      return query;
    }
  }
}
