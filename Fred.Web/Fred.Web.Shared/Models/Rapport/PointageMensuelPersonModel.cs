using Fred.Entities.Personnel;
using System.Collections.Generic;

namespace Fred.Entities.Rapport
{
  /// <summary>
  ///   Le PointageJournalierEnt
  /// </summary>
  public class PointageMensuelPersonModel
  {
    /// <summary>
    /// Obtient ou définit le Année
    /// </summary>
    public int Annee { get; set; }

    /// <summary>
    /// Obtient ou définit le Mois
    /// </summary>
    public int Mois { get; set; }

    /// <summary>
    /// Obtient ou définit le personnel
    /// </summary>
    public PersonnelEnt Personnel { get; set; }

    /// <summary>
    /// Obtient ou définit les heures normales
    /// </summary>
    public HeuresNormales ListHeuresNormales { get; set; }

    /// <summary>
    /// Obtient ou définit les heures majoration
    /// </summary>
    public HeuresMajo ListHeuresMajo { get; set; }

    /// <summary>
    /// Obtient ou définit les heures à pied
    /// </summary>
    public HeuresAPied ListHeuresAPied { get; set; }

    /// <summary>
    /// Obtient ou définit les heures absence
    /// </summary>
    public HeuresAbsence ListHeuresAbsence { get; set; }

    /// <summary>
    /// Obtient ou définit les heures pointées
    /// </summary>
    public HeuresPointees ListHeuresPointees { get; set; }

    /// <summary>
    /// Obtient ou définit les heures travaillées
    /// </summary>
    public HeuresTravaillees ListHeuresTravaillees { get; set; }

    /// <summary>
    /// Obtient ou définit les primes
    /// </summary>
    public List<Primes> ListPrimes { get; set; } = new List<Primes>();

    /// <summary>
    /// Obtient ou définit les déplacements
    /// </summary>
    public List<Deplacements> ListDeplacements { get; set; } = new List<Deplacements>();

    /// <summary>
    /// Défini la classe des heures normales
    /// </summary>
    public class HeuresNormales
    {
      /// <summary>
      /// Obtient ou définit le libelle
      /// </summary>
      public string Libelle
      {
        get { return "Heures normales"; }
      }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour1 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour2 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour3 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour4 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour5 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour6 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour7 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour8 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour9 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour10 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour11 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour12 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour13 { get; set; }


      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour14 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour15 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour16 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour17 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour18 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour19 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour20 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour21 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour22 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour23 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour24 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour25 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour26 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour27 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour28 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour29 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour30 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour31 { get; set; }
    }

    public void LoadHeuresNormales(Dictionary<int,string> listeHoraireJournalier)
    {
      HeuresNormales heuresNormales = new HeuresNormales();

      for (int i = 1; i <= 31; i++ )
      {
        heuresNormales.GetType().GetProperty("Jour" + i).SetValue(heuresNormales, listeHoraireJournalier[i]);
      }

      ListHeuresNormales = heuresNormales;
    }

    /// <summary>
    /// Défini la classe des heures de majorations
    /// </summary>
    public class HeuresMajo
    {
      /// <summary>
      /// Obtient ou définit le libelle
      /// </summary>
      public string Libelle
      {
        get { return "Heures majorées"; }
      }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour1 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour2 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour3 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour4 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour5 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour6 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour7 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour8 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour9 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour10 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour11 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour12 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour13 { get; set; }


      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour14 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour15 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour16 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour17 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour18 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour19 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour20 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour21 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour22 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour23 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour24 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour25 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour26 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour27 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour28 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour29 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour30 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure majorées
      /// </summary>
      public string Jour31 { get; set; }
    }

    public void LoadHeuresMajo(Dictionary<int, string> listeHoraireJournalier)
    {
      HeuresMajo heuresMajo = new HeuresMajo();

      for (int i = 1; i <= 31; i++)
      {
        heuresMajo.GetType().GetProperty("Jour" + i).SetValue(heuresMajo, listeHoraireJournalier[i]);
      }

      ListHeuresMajo = heuresMajo;
    }

    /// <summary>
    /// Défini la classe des heures à pied
    /// </summary>
    public class HeuresAPied
    {
      /// <summary>
      /// Obtient ou définit le libelle
      /// </summary>
      public string Libelle
      {
        get { return "Heures à pied"; }
      }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour1 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour2 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour3 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour4 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour5 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour6 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour7 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour8 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour9 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour10 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour11 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour12 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour13 { get; set; }


      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour14 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour15 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour16 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour17 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour18 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour19 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour20 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour21 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour22 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour23 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour24 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour25 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour26 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour27 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour28 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour29 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure à pied
      /// </summary>
      public string Jour30 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure normales
      /// </summary>
      public string Jour31 { get; set; }
    }

    public void LoadHeuresAPied(Dictionary<int, string> listeHoraireJournalier)
    {
      HeuresAPied heuresAPied = new HeuresAPied();

      for (int i = 1; i <= 31; i++)
      {
        heuresAPied.GetType().GetProperty("Jour" + i).SetValue(heuresAPied, listeHoraireJournalier[i]);
      }

      ListHeuresAPied = heuresAPied;
    }

    /// <summary>
    /// Défini la classe des heures travaillées
    /// </summary>
    public class HeuresTravaillees
    {
      /// <summary>
      /// Obtient ou définit le libelle
      /// </summary>
      public string Libelle
      {
        get { return "Heures travaillées"; }
      }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour1 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour2 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour3 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour4 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour5 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour6 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour7 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour8 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour9 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour10 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour11 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour12 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour13 { get; set; }


      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour14 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour15 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour16 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour17 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour18 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour19 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour20 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour21 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour22 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour23 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour24 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour25 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour26 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour27 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour28 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour29 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour30 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure travaillées
      /// </summary>
      public string Jour31 { get; set; }
    }

    public void LoadHeuresTravaillees(Dictionary<int, string> listeHoraireJournalier)
    {
      HeuresTravaillees heuresTravaillees = new HeuresTravaillees();

      for (int i = 1; i <= 31; i++)
      {
        heuresTravaillees.GetType().GetProperty("Jour" + i).SetValue(heuresTravaillees, listeHoraireJournalier[i]);
      }

      ListHeuresTravaillees = heuresTravaillees;
    }

    /// <summary>
    /// Défini la classe des heures absence
    /// </summary>
    public class HeuresAbsence
    {
      /// <summary>
      /// Obtient ou définit le libelle
      /// </summary>
      public string Libelle
      {
        get { return "Heures d'absence"; }
      }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour1 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour2 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour3 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour4 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour5 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour6 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour7 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour8 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour9 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour10 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour11 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour12 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour13 { get; set; }


      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour14 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour15 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour16 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour17 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour18 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour19 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour20 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour21 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour22 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour23 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour24 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour25 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour26 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour27 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour28 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour29 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour30 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure d'absence
      /// </summary>
      public string Jour31 { get; set; }
    }

    public void LoadHeuresAbsence(Dictionary<int, string> listeHoraireJournalier)
    {
      HeuresAbsence heuresAbsence = new HeuresAbsence();

      for (int i = 1; i <= 31; i++)
      {
        heuresAbsence.GetType().GetProperty("Jour" + i).SetValue(heuresAbsence, listeHoraireJournalier[i]);
      }

      ListHeuresAbsence = heuresAbsence;
    }

    /// <summary>
    /// Défini la classe des heures pointées
    /// </summary>
    public class HeuresPointees
    {
      /// <summary>
      /// Obtient ou définit le libelle
      /// </summary>
      public string Libelle
      {
        get { return "Heures pointées"; }
      }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour1 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour2 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour3 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour4 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour5 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour6 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour7 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour8 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour9 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour10 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour11 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour12 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour13 { get; set; }


      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour14 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour15 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour16 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour17 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour18 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour19 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour20 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour21 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour22 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour23 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour24 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour25 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour26 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour27 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour28 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour29 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour30 { get; set; }

      /// <summary>
      /// Obtient ou définit le nombre d'heure pointées
      /// </summary>
      public string Jour31 { get; set; }
    }

    public void LoadHeuresPointees(Dictionary<int, string> listeHoraireJournalier)
    {
      HeuresPointees heuresPointees = new HeuresPointees();

      for (int i = 1; i <= 31; i++)
      {
        heuresPointees.GetType().GetProperty("Jour" + i).SetValue(heuresPointees, listeHoraireJournalier[i]);
      }

      ListHeuresPointees = heuresPointees;
    }

    /// <summary>
    /// Défini la classe des heures de majorations
    /// </summary>
    public class Primes
    {
      /// <summary>
      /// Obtient ou définit le libelle
      /// </summary>
      public string Libelle { get; set; }

      /// <summary>
      ///  Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour1 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour2 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour3 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour4 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour5 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour6 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour7 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour8 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour9 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour10 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour11 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour12 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour13 { get; set; } = string.Empty;


      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour14 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour15 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour16 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour17 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour18 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour19 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour20 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour21 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour22 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour23 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour24 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour25 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour26 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour27 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour28 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour29 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour30 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'une prime pour ce jour
      /// </summary>
      public string Jour31 { get; set; } = string.Empty;
    }

    public void LoadPrimes(Dictionary<int, string> listeHoraireJournalier, string CodePrime)
    {
      Primes primes = new Primes();

      primes.Libelle = "Prime " + CodePrime;
      for (int i = 1; i <= 31; i++)
      {
        primes.GetType().GetProperty("Jour" + i).SetValue(primes, listeHoraireJournalier[i]);
      }

      ListPrimes.Add(primes);
    }

    /// <summary>
    /// Défini la classe des heures de majorations
    /// </summary>
    public class Deplacements
    {
      /// <summary>
      /// Obtient ou définit le libelle
      /// </summary>
      public string Libelle { get; set; }

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour1 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour2 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour3 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour4 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour5 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour6 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour7 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour8 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour9 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour10 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour11 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour12 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour13 { get; set; } = string.Empty;


      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour14 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour15 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour16 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour17 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour18 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour19 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour20 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour21 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour22 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour23 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour24 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour25 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour26 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour27 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour28 { get; set; } = string.Empty;

      /// <summary>
      ///  Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour29 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour30 { get; set; } = string.Empty;

      /// <summary>
      /// Obtient ou définit la présence d'un déplacment pour ce jour
      /// </summary>
      public string Jour31 { get; set; } = string.Empty;
    }

    public void LoadDeplacement(Dictionary<int, string> listeHoraireJournalier, string CodeDeplacement)
    {
      Deplacements deplacements = new Deplacements();

      deplacements.Libelle = "Déplacement " + CodeDeplacement;
      for (int i = 1; i <= 31; i++)
      {
        deplacements.GetType().GetProperty("Jour" + i).SetValue(deplacements, listeHoraireJournalier[i]);
      }

      ListDeplacements.Add(deplacements);
    }
  }
}
