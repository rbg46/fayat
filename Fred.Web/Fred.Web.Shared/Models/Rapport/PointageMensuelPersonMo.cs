using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Web.Models.CI;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  public class PointageMensuelPersonModel
  {
    /// <summary>
    /// Obtient ou définit le OrdreTri
    /// </summary>
    public int OrdreTri { get; set; }

    /// <summary>
    /// Obtient ou définit le Evenement
    /// </summary>
    public string Evenement { get; set; }

    /// <summary>
    /// Obtient ou définit le EvenementValue
    /// </summary>
    public string EvenementValue { get; set; }

    /// <summary>
    /// Obtient ou définit le Matricule
    /// </summary>
    public string Matricule { get; set; }

    /// <summary>
    /// Obtient ou définit le PrenomNom
    /// </summary>

    public string PrenomNom { get; set; }

    /// <summary>
    /// Obtient ou définit le PrenomNom
    /// </summary>
    public string Fonction { get; set; }

    /// <summary>
    /// Obtient ou définit le PrenomNom
    /// </summary>
    public bool Interim { get; set; }

    /// <summary>
    /// Obtient ou définit le CodeSociete
    /// </summary>

    public string CodeSociete { get; set; }

    /// <summary>
    /// Obtient ou définit le Date
    /// </summary>
    public DateTime DateComptable { get; set; }

    /// <summary>
    /// Obtient ou définit le CiId
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou défini le CI
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité personnel
    /// </summary>
    public int? PersonnelId { get; set; }

    /// <summary>
    /// Obtient ou définit le personnel
    /// </summary>
    public PersonnelModel Personnel { get; set; }

    /// <summary>
    /// Obtient ou définit le SumHeureNonMaj
    /// </summary>
    public double SumHeureMois { get; set; }

    /// <summary>
    /// Obtient ou définit le TotalHeuresTravailles
    /// </summary>
    public double TotalHeuresTravaillees { get; set; }

    /// <summary>
    /// Obtient ou définit le Absence
    /// </summary>
    public double Absence { get; set; }

    /// <summary>
    /// Obtient ou définit le TotalHeuresAbsences
    /// </summary>
    public double TotalHeuresAbsences { get; set; }

    /// <summary>
    /// Obtient ou définit le temps total MaterielMarche
    /// </summary>
    public double TotalMaterielMarche { get; set; }

    /// <summary>
    /// Obtient ou définit le temps total MaterielMarche
    /// </summary>
    public double TotalHeuresAPied { get; set; }

    /// <summary>
    /// Obtient ou définit le TotalHeuresPointees
    /// </summary>
    public double TotalHeuresPointees { get; set; }

    /// <summary>
    /// Obtient ou définit le Deplacement
    /// </summary>
    public double Deplacement { get; set; }

    /// <summary>
    /// Obtient ou définit le Prime
    /// </summary>
    public double Prime { get; set; }

    /// <summary>
    /// Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe { get; set; }

    /// <summary>
    /// Obtient ou défini la somme des heures
    /// </summary>
    public double SumHeuresTravailleesMois { get; set; }

    public Dictionary<int, PointageMensuelPersonProperties> PropertiesPerDay { get; set; }

    public class PointageMensuelPersonProperties
    {
      /// <summary>
      /// Obtient ou définit le HeureNonMaj1
      /// </summary>
      public double HeureNonMaj { get; set; }

      /// <summary>
      /// Obtient ou définit le HeureMaj1
      /// </summary>
      public double HeureMaj { get; set; }

      /// <summary>
      /// Obtient ou définit le HeureAPied1
      /// </summary>
      public double HeureAPied { get; set; }

      /// <summary>
      /// Obtient ou définit le TotalHeuresTravaillees1
      /// </summary>
      public double TotalHeuresTravaillees { get; set; }

      /// <summary>
      /// Obtient ou définit le AbsenceNum1
      /// </summary>
      public double AbsenceNum { get; set; }

      /// <summary>
      /// Obtient ou définit le MaterielMarche1
      /// </summary>
      public double MaterielMarche { get; set; }

      /// <summary>
      /// Obtient ou définit le TotalHeuresAbsence1
      /// </summary>
      public double TotalHeuresAbsence { get; set; }

      /// <summary>
      /// Obtient ou définit le TotalHeuresPointees1
      /// </summary>
      public double TotalHeuresPointees { get; set; }

      /// <summary>
      /// Obtient ou définit le DeplacementNum1
      /// </summary>
      public bool Deplacement { get; set; }

      /// <summary>
      /// Obtient ou définit le DeplacementNum1
      /// </summary>
      public double DeplacementNum { get; set; }

      /// <summary>
      /// Obtient ou définit le PrimeNum1
      /// </summary>
      public double PrimeNum { get; set; }

      /// <summary>
      /// Obtient ou définit le EvenementValue1
      /// </summary>
      public string EvenementValue { get; set; }

      /// <summary>
      /// Obtient ou défini le type du pointage (Anticipe lu reel)
      /// </summary>
      public bool IsAnticipe { get; set; }
    }
  }
}