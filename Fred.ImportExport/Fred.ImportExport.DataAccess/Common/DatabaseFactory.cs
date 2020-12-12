using Fred.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Fred.ImportExport.DataAccess.Common
{
  /// <summary>
  ///   Fabrique d'instance d'accès à une base de données
  /// </summary>
  public static class DatabaseFactory
  {
    /// <summary>
    ///   Chaine du connecteur SQL Server.
    /// </summary>
    private const string SqlServerConnector = "System.Data.SqlClient";

    /// <summary>
    ///   Chaine du connecteur DB2.
    /// </summary>
    private const string DB2Connector = "IBM.Data.DB2.iSeries";

    /// <summary>
    ///   Permet de récupérer les chaine des connecteurs disponibles.
    /// </summary>
    /// <returns>Liste des chaines de connecteurs disponibles.</returns>
    public static IEnumerable<string> GetAllowedConnectors()
    {
      List<string> connectors = new List<string>();
      DataTable tbl = DbProviderFactories.GetFactoryClasses();

      foreach (DataRow row in tbl.Rows)
      {
        connectors.Add(string.Format("{0} ({1})", row["Name"], row["InvariantName"]));
      }

      return connectors;
    }

    /// <summary>
    ///   Permet la création d'une nouvelle instance de base de données.
    /// </summary>
    /// <param name="enumTypeBdd">Type de base de données désiré.</param>
    /// <param name="connexionString">Chaine de connexion à la base de données désirée.</param>
    /// <returns>Retourne une instance de base de données initialisée.</returns>
    public static Database GetNewDatabase(TypeBdd enumTypeBdd, string connexionString)
    {
      Database dB = null;
      try
      {
        dB = new Database(GetConnector(enumTypeBdd), connexionString);
      }
      catch (Exception ex)
      {
        NLog.LogManager.GetCurrentClassLogger().Error(ex, $"Erreur lors de la connexion à une base de type {enumTypeBdd}. ConnexionString = {connexionString}");
        throw;
      }

      return dB;
    }

    /// <summary>
    ///   Permet de récupérer le connecteur depuis une énumération de types de base de données.
    /// </summary>
    /// <param name="enumTypeBdd">Type de base de données désiré.</param>
    /// <returns>Retourne la chaine du connecteur désirée.</returns>
    private static string GetConnector(TypeBdd enumTypeBdd)
    {
      switch (enumTypeBdd)
      {
        case TypeBdd.SqlServer:
          return SqlServerConnector;
        case TypeBdd.Db2:
          return DB2Connector;
        default:
          return string.Empty;
      }
    }
  }
}
