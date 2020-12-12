using Fred.Entities.Rapport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Transform.Features
{
  /// <summary>
  /// Classe permettant de géere les mapping avec SAP.
  /// Elle permet aussi de centraliser les regles metiers appliquer lors de la transformation.
  /// </summary>
  public static class PointagePersonnelFeatureHelper
  {

    /// <summary>
    /// Rg : ne prendre que les RapportLignePrimeEnt qui sont checked
    /// </summary>
    /// <param name="rapportLignePrimes">Liste de RapportLignePrimeEnt</param>
    /// <returns>une liste de RapportLignePrimeEnt qui sont checked</returns>
    public static IEnumerable<RapportLignePrimeEnt> GetRapportLignePrimesChecked(this IEnumerable<RapportLignePrimeEnt> rapportLignePrimes)
    {
      return rapportLignePrimes.Where(rlp => rlp.IsChecked).ToList();
    }


    public static string GetPersonnel(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne?.Personnel?.Matricule;
    }

    public static string GetCiCode(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne?.Ci?.Code;
    }


    public static DateTime GetDate(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne.DatePointage;
    }

    public static string GetMajorationCode(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne?.CodeMajoration?.Code;
    }


    public static double GetMajorationHeure(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne.HeureMajoration;
    }

    public static string GetAbsenceCode(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne?.CodeAbsence?.Code;
    }

    public static double GetAbsenceHeure(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne.HeureAbsence;
    }

    public static string GetDeplacementCode(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne?.CodeDeplacement?.Code;
    }
    public static string GetDeplacementZone(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne?.CodeDeplacement?.Libelle;
    }

    public static string GetDeplacementIVD(this RapportLigneEnt rapportLigne)
    {
      return rapportLigne.DeplacementIV.ToString();
    }

    public static string GetPrimeCode(this RapportLignePrimeEnt rapportLignePrime)
    {
      return rapportLignePrime?.Prime?.Code;
    }

    public static string GetTacheCode(this RapportLigneTacheEnt rapportLigneTache)
    {
      return rapportLigneTache?.Tache?.Code;
    }

    public static double GetTacheHeure(this RapportLigneTacheEnt rapportLigneTache)
    {
      return rapportLigneTache.HeureTache;
    }

    public static string GetAstreinteCode(this RapportLigneAstreinteEnt rapportLigneAstreinte)
    {
      return rapportLigneAstreinte?.AstreinteId.ToString();
    }

  }
}
