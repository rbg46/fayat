using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.DataAccess.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fred.ImportExport.Business.ValidationPointage
{
  /// <summary>
  /// Opérations communes au contrôle et à la remontée vrac RVG.
  /// </summary>
  public class RvgControleHelper
  {
    private readonly ISocieteManager societeManager;
    private readonly IEtablissementPaieManager etsPaieManager;
    private readonly Logger logger;

    #region Requêtes

    /// <summary>
    /// Requête RVG pour récupérer les pointages chantier pour des établissements.
    /// </summary>
    /// <remarks>
    /// {0} : le code société paie
    /// {1} : 1 pour récupérer les pointages chantier de tous les matricules, 0 pour récupérer ceux indiqués dans le paramètre {2}
    /// {2} : le matricule concerné (cf. {1})
    /// {3} : la période comptable
    /// {4} : établissement(s) concerné(s) au format "'35', '36', ..."
    /// </remarks>
    private const string SelectRvgPointagesChantierPourDesEtablissementsQuery = "SELECT * FROM dbo.GEP_SelectPointagesChantierPrev(0,'','',0,'{0}',{1},'','{2}','{3}') WHERE ETABL_ID IN ({4})";

    /// <summary>
    /// Requête RVG pour récupérer les pointages chantier pour tous les établissements.
    /// </summary>
    /// <remarks>
    /// {0} : le code société paie
    /// {1} : 1 pour récupérer les pointages chantier de tous les matricules, 0 pour récupérer ceux indiqués dans le paramètre {2}
    /// {2} : le matricule concerné (cf. {1})
    /// {3} : la période comptable
    /// {4} : établissement(s) concerné(s) au format "'35', '36', ..."
    /// </remarks>
    private const string SelectRvgPointagesChantierPourTousEtablissementsQuery = "SELECT * FROM dbo.GEP_SelectPointagesChantierPrev(0,'','',0,'{0}',{1},'','{2}','{3}')";

    /// <summary>
    /// Requête RVG pour récupérer les primes pour des établissements.
    /// </summary>
    /// <remarks>
    /// {0} : le code société paie
    /// {1} : '1' pour récupérer les primes de tous les matricules, '0' pour récupérer celles indiquées dans le paramètre {2}
    /// {2} : le matricule concerné (cf. {1})
    /// {3} : la période comptable
    /// {4} : établissement(s) concerné(s) au format "'35', '36', ..."
    /// </remarks>
    private const string SelectRvgPrimesPourDesEtablissementsQuery = "SELECT * FROM dbo.GEP_fnSelectPrimeReelPrev ('0','','','','{0}','{1}','','{2}','{3}') WHERE ETABL_ID IN ({4})";

    /// <summary>
    /// Requête RVG pour récupérer les primes pour tous les établissements.
    /// </summary>
    /// <remarks>
    /// {0} : le code société paie
    /// {1} : '1' pour récupérer les primes de tous les matricules, '0' pour récupérer celles indiquées dans le paramètre {2}
    /// {2} : le matricule concerné (cf. {1})
    /// {3} : la période comptable
    /// </remarks>
    private const string SelectRvgPrimesPourTousLesEtablissementsQuery = "SELECT * FROM dbo.GEP_fnSelectPrimeReelPrev ('0','','','','{0}','{1}','','{2}','{3}')";

    #endregion

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="societeManager">Gestionnaire des sociétés.</param>
    /// <param name="etsPaieManager">Gestionnaire des établissements de paie.</param>
    public RvgControleHelper(
      ISocieteManager societeManager,
      IEtablissementPaieManager etsPaieManager)
    {
      this.societeManager = societeManager;
      this.etsPaieManager = etsPaieManager;
      logger = LogManager.GetCurrentClassLogger();
    }

    /// <summary>
    /// Récupère les pointages et les primes de RVG.
    /// </summary>
    /// <param name="periode">La période concernée.</param>
    /// <param name="filtre">Le filtre à appliquer.</param>
    /// <param name="connexionChaine">La chaine de connexion RVG.</param>
    /// <returns>Les pointages et les primes de RVG.</returns>
    public RvgPointagesAndPrimes GetPointagesAndPrimes(DateTime periode, PointageFiltre filtre, string connexionChaine)
    {
      var ret = new RvgPointagesAndPrimes();

      // La société doit exister
      var societe = societeManager.FindById(filtre.SocieteId);
      if (societe == null)
      {
        logger.Error($"La société d'identifiant {filtre.SocieteId} n'existe pas");
        return ret;
      }

      // Le code de la société paie est nécessaire pour récupérer les pointages RVG
      var codeSocietePaye = societe.CodeSocietePaye;
      if (string.IsNullOrEmpty(codeSocietePaye))
      {
        logger.Error($"Le code société paie n'est pas renseigné pour la société d'identifiant {filtre.SocieteId}");
        return ret;
      }

      // Vérification du matricule
      if (filtre.TakeMatricule)
      {
        if (filtre.Matricule == null)
        {
          logger.Error($"Le matricule doit être fournit");
          return ret;
        }
      }
      else if (filtre.Matricule != null)
      {
        logger.Error($"Le matricule ne doit pas être fournit");
        return ret;
      }

      // Récupère les pointages chantier RVG
      var query = GetPointagesChantierQuery(codeSocietePaye, filtre.Matricule, periode, filtre.EtablissementPaieIdList);
      if (!string.IsNullOrEmpty(query))
      {
        using (var rvgDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.SqlServer, connexionChaine))
        {
          using (var reader = rvgDatabase.ExecuteReader(query))
          {
            while (reader.Read())
            {
              var rvgPointage = GetPointage(reader, periode);
              ret.Pointages.Add(rvgPointage);
            }
          }
        }
      }

      // Récupère les primes RVG
      query = GetPrimesQuery(codeSocietePaye, filtre.Matricule, periode, filtre.EtablissementPaieIdList);
      if (!string.IsNullOrEmpty(query))
      {
        using (var rvgDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.SqlServer, connexionChaine))
        {
          using (var reader = rvgDatabase.ExecuteReader(query))
          {
            while (reader.Read())
            {
              var rvgPointage = GetPrime(reader, periode);
              ret.Primes.Add(rvgPointage);
            }
          }
        }
      }

      return ret;
    }

    /// <summary>
    /// Récupère la requête de sélection des pointages chantier.
    /// </summary>
    /// <param name="codeSocietePaye">Le code de la société paie.</param>
    /// <param name="matricule">Le matricule ou null pour tous les matricules.</param>
    /// <param name="periode">La période comptable.</param>
    /// <param name="fredEtablissementPaieIds">Les établissements de paie FRED concernés ou null pour tous les établissements.</param>
    /// <returns>La requête de sélection des pointages chantier.</returns>
    private string GetPointagesChantierQuery(string codeSocietePaye, string matricule, DateTime periode, IEnumerable<int> fredEtablissementPaieIds)
    {
      string ret = null;
      var periodeComptable = ((periode.Year * 100) + periode.Month).ToString();   // Au format RVG : AAAAMM
      var tousLesMatricules = matricule == null ? '1' : '0';
      if (fredEtablissementPaieIds != null && fredEtablissementPaieIds.Any())
      {
        // Pour certains établissements
        var etablissementsParametre = GetEtablissementParametreForQuery(fredEtablissementPaieIds);
        if (!string.IsNullOrEmpty(etablissementsParametre))
        {
          ret = string.Format(SelectRvgPointagesChantierPourDesEtablissementsQuery, codeSocietePaye, tousLesMatricules, matricule, periodeComptable, etablissementsParametre);
        }
      }
      else
      {
        // Pour tous les établissements
        ret = string.Format(SelectRvgPointagesChantierPourTousEtablissementsQuery, codeSocietePaye, tousLesMatricules, matricule, periodeComptable);
      }
      return ret;
    }

    /// <summary>
    /// Récupère la requête de sélection des primes.
    /// </summary>
    /// <param name="codeSocietePaye">Le code de la société paie.</param>
    /// <param name="matricule">Le matricule ou null pour tous les matricules.</param>
    /// <param name="periode">La période comptable.</param>
    /// <param name="fredEtablissementPaieIds">Les établissements de paie FRED concernés ou null pour tous les établissements.</param>
    /// <returns>La requête de sélection des pointages chantier.</returns>
    private string GetPrimesQuery(string codeSocietePaye, string matricule, DateTime periode, IEnumerable<int> fredEtablissementPaieIds)
    {
      string ret = null;
      var periodeComptable = ((periode.Year * 100) + periode.Month).ToString();   // Au format RVG : AAAAMM
      var tousLesMatricules = matricule == null ? '1' : '0';
      if (fredEtablissementPaieIds != null && fredEtablissementPaieIds.Any())
      {
        // Pour certains établissements
        var etablissementsParametre = GetEtablissementParametreForQuery(fredEtablissementPaieIds);
        if (!string.IsNullOrEmpty(etablissementsParametre))
        {
          ret = string.Format(SelectRvgPrimesPourDesEtablissementsQuery, codeSocietePaye, tousLesMatricules, matricule, periodeComptable, etablissementsParametre);
        }
      }
      else
      {
        // Pour tous les établissements
        ret = string.Format(SelectRvgPrimesPourTousLesEtablissementsQuery, codeSocietePaye, tousLesMatricules, matricule, periodeComptable);
      }
      return ret;
    }

    /// <summary>
    /// Retourne la chaîne à utiliser dans les requêtes pour le paramètre 'établissements'.
    /// </summary>
    /// <param name="fredEtablissementPaieIds">Les identifiants des établissements de paie FRED concernés.</param>
    /// <returns>Lla chaîne à utiliser dans les requêtes pour le paramètre 'établissements'.</returns>
    private string GetEtablissementParametreForQuery(IEnumerable<int> fredEtablissementPaieIds)
    {
      var sb = new StringBuilder();
      foreach (var fredEtablissementPaieId in fredEtablissementPaieIds)
      {
        var ets = etsPaieManager.GetEtablissementPaieById(fredEtablissementPaieId);
        if (ets == null)
        {
          logger.Error($"L'établissement de paie d'identifiant {fredEtablissementPaieId} n'existe pas");
        }
        else if (string.IsNullOrEmpty(ets.Code))
        {
          logger.Error($"L'établissement de paie d'identifiant {fredEtablissementPaieId} n'a pas de code");
        }
        else
        {
          if (sb.Length > 0)
          {
            sb.Append(',');
          }
          sb.Append("'");
          sb.Append(ets.Code);
          sb.Append("'");
        }
      }
      return sb.ToString();
    }

    /// <summary>
    /// Retourne un pointage RVG.
    /// </summary>
    /// <param name="reader">Le reader courant.</param>
    /// <param name="periode">La période.</param>
    /// <returns>Le pointage RVG.</returns>
    private static RvgPointage GetPointage(IDataReader reader, DateTime periode)
    {
      var readerCodeAbsence = reader["coabs_id"] as string;
      var readerCodeDeplacement = reader["CODEP_ID"] as string;
      var readerHeureNormale = reader["LIGRJ_PNBHN"];
      var readerHeureMajoration = reader["LIGRJ_PNBHM"];
      var readerHeureAbsence = reader["LIGRJ_COABS_NBHA"];
      var readerAnneeRapport = reader["anneeRap"];
      var readerMoisRapport = reader["moisRap"];
      var readerJourRapport = reader["jourRap"];

      var numSemaineIntemperieAbsence = 0;
      var readerNumSemaineIntemperieAbsence = reader["LIGRJ_COABS_NUMINT"];
      if (readerNumSemaineIntemperieAbsence != DBNull.Value)
      {
        numSemaineIntemperieAbsence = (int)(decimal)readerNumSemaineIntemperieAbsence;
        if (numSemaineIntemperieAbsence < 0 || numSemaineIntemperieAbsence > 52)
        {
          numSemaineIntemperieAbsence = 0;
        }
      }

      return new RvgPointage
      {
        CodeAffaire = reader["AFFAIRE"] as string,
        CodeAffairePrestation = reader["AFFAI_PREST"] as string,
        MatriculePersonnel = reader["PERSO_ID"] as string,
        EtablissementPaieIdPersonnel = reader["ETABL_ID"] as string,
        CodeAbsence = readerCodeAbsence != "0" ? readerCodeAbsence : null,
        CodeDeplacement = readerCodeDeplacement != "0" ? readerCodeDeplacement : null,
        VoyageDetente = reader["LIGRJ_VOYDET"] as string ?? "0",
        HeureNormale = readerHeureNormale != DBNull.Value ? (decimal)readerHeureNormale : 0,
        HeureMajoration = readerHeureMajoration != DBNull.Value ? (decimal)readerHeureMajoration : 0,
        HeureAbsence = readerHeureAbsence != DBNull.Value ? (decimal)readerHeureAbsence : 0,
        NumSemaineIntemperieAbsence = numSemaineIntemperieAbsence,
        AnneeRapport = readerAnneeRapport != DBNull.Value ? (int)readerAnneeRapport : periode.Year,
        MoisRapport = readerMoisRapport != DBNull.Value ? (int)readerMoisRapport : periode.Month,
        JourRapport = readerJourRapport != DBNull.Value ? (int)readerJourRapport : periode.Day,
        CodeZoneDeplacement = reader["ZONE_ID"] as string,
        CodeMajoration = reader["COMAJ_ID"] as string
      };
    }

    /// <summary>
    /// Retourne une prime RVG.
    /// </summary>
    /// <param name="reader">Le reader courant.</param>
    /// <param name="periode">La période.</param>
    /// <returns>La prime RVG.</returns>
    private static RvgPrime GetPrime(IDataReader reader, DateTime periode)
    {
      var readerDate = reader["ligrj_date"];
      var readerQuantite = reader["lrjpr_qte"];

      var ret = new RvgPrime
      {
        CodeAffaire = reader["AFFAI_ID"] as string,
        MatriculePersonnel = reader["PERSO_ID"] as string,
        Date = readerDate != DBNull.Value ? (DateTime)readerDate : periode,
        Code = reader["primes_id"] as string,
        Libelle = reader["primes_lib"] as string,
        Quantite = readerQuantite != DBNull.Value ? (decimal)readerQuantite : 0,
        TypeHoraire = reader["primes_unite"] as string == "Heures",
      };

      return ret;
    }
  }
}
