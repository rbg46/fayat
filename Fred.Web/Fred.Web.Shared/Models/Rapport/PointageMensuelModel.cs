using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Web.Models.CI;
using Fred.Web.Models.CodeAbsence;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  public class PointageMensuelModel
  {
    /// <summary>
    /// Obtient ou définit le MatCodPre
    /// </summary>
    public string InfoPerson
    {
      get {
        if (this.Matricule != null && this.PrenomNom != null)
          return this.Matricule + " - " + this.CodeSociete + " - " + this.PrenomNom;
        else
          return "";
      }
    }
    /// <summary>
    /// Obtient ou définit le Matricule
    /// </summary>
    public string Matricule { get; set; }

    /// <summary>
    /// Obtient ou définit le PrenomNom
    /// </summary>

    public string PrenomNom { get; set; }

    /// <summary>
    /// Obtient ou définit le CodeSociete
    /// </summary>

    public string CodeSociete { get; set; }

    /// <summary>
    /// Obtient ou définit le Date
    /// </summary>
    public DateTime DateComptable { get; set; }

    /// <summary>
    /// Obtient ou définit le Filtre
    /// </summary>
    public bool Domaine { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri
    /// </summary>
    public bool Tri { get; set; }

    /// <summary>
    /// Obtient ou définit le OrganisationId
    /// </summary>
    /// 
    public int OrganisationId { get; set; }

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
    /// Obtient ou définit le NbHeure
    /// </summary>
    public double NbHeure { get; set; }

    /// <summary>
    /// Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe { get; set; }

    /// <summary>
    /// Obtient ou défini la somme des heures par ligne
    /// </summary>
    public double Sum { get; set; }

    public Dictionary<int, PointageMensuelProperties> NbHeurePerDay { get; set; }

    public class PointageMensuelProperties
    {

      /// <summary>
      /// Obtient ou définit le NbHeure1
      /// </summary>
      public double NbHeure { get; set; }

      /// <summary>
      /// Obtient ou défini le type du pointage (Anticipe lu reel)
      /// </summary>
      public bool IsAnticipe { get; set; }
    }
  }
}