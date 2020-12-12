using System;
using Fred.Entities.CI;

namespace Fred.Entities.Rapport
{
  /// <summary>
  ///   Le PointageJournalierEnt
  /// </summary>
  public class PointageMensuelEnt
  {
    private DateTime dateComptable;

    /// <summary>
    ///   Obtient ou définit le Date
    /// </summary>
    public DateTime DateComptable
    {
      get
      {
        return DateTime.SpecifyKind(dateComptable, DateTimeKind.Utc);
      }
      set
      {
        dateComptable = DateTime.SpecifyKind(value, DateTimeKind.Utc);
      }
    }

    /// <summary>
    ///   Obtient ou définit le Filtre
    /// </summary>
    public bool Domaine { get; set; }

    /// <summary>
    ///   Obtient ou définit le Tri
    /// </summary>
    public bool Tri { get; set; }

    /// <summary>
    ///   Obtient ou définit l'OrganisationId
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    ///   Obtient ou définit le CI du pointage
    /// </summary>
    public CIEnt CI { get; set; }

    /// <summary>
    ///   Obtient ou définit le Matricule
    /// </summary>
    public string Matricule { get; set; }

    /// <summary>
    ///   Obtient ou définit le PrenomNom
    /// </summary>
    public string PrenomNom { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure
    /// </summary>
    public double NbHeure { get; set; }

    /// <summary>
    ///   Obtient ou définit le CodeSociete
    /// </summary>
    public string CodeSociete { get; set; }

    /// <summary>
    ///   Obtient ou définit le CiId
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'entité personnel
    /// </summary>
    public int? PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe { get; set; }

    /// <summary>
    ///   Obtient ou défini la somme des heures par ligne
    /// </summary>
    public double Sum { get; set; }

    #region NbHeureParJour

    /// <summary>
    ///   Obtient ou définit le NbHeure1
    /// </summary>
    public double NbHeure1 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure2
    /// </summary>
    public double NbHeure2 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure3
    /// </summary>
    public double NbHeure3 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure4
    /// </summary>
    public double NbHeure4 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure5
    /// </summary>
    public double NbHeure5 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure6
    /// </summary>
    public double NbHeure6 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure7
    /// </summary>
    public double NbHeure7 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure8
    /// </summary>
    public double NbHeure8 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure9
    /// </summary>
    public double NbHeure9 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure10
    /// </summary>
    public double NbHeure10 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure11
    /// </summary>
    public double NbHeure11 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure12
    /// </summary>
    public double NbHeure12 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure13
    /// </summary>
    public double NbHeure13 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure14
    /// </summary>
    public double NbHeure14 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure15
    /// </summary>
    public double NbHeure15 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure16
    /// </summary>
    public double NbHeure16 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure17
    /// </summary>
    public double NbHeure17 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure18
    /// </summary>
    public double NbHeure18 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure19
    /// </summary>
    public double NbHeure19 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure20
    /// </summary>
    public double NbHeure20 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure21
    /// </summary>
    public double NbHeure21 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure22
    /// </summary>
    public double NbHeure22 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure23
    /// </summary>
    public double NbHeure23 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure24
    /// </summary>
    public double NbHeure24 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure25
    /// </summary>
    public double NbHeure25 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure26
    /// </summary>
    public double NbHeure26 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure27
    /// </summary>
    public double NbHeure27 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure28
    /// </summary>
    public double NbHeure28 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure29
    /// </summary>
    public double NbHeure29 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure30
    /// </summary>
    public double NbHeure30 { get; set; }

    /// <summary>
    ///   Obtient ou définit le NbHeure31
    /// </summary>
    public double NbHeure31 { get; set; }

    #endregion

    #region JourDuMois

    /// <summary>
    ///   Obtient ou définit le Jour1
    /// </summary>
    public int Jour1 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour2
    /// </summary>
    public int Jour2 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour3
    /// </summary>
    public int Jour3 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour4
    /// </summary>
    public int Jour4 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour5
    /// </summary>
    public int Jour5 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour6
    /// </summary>
    public int Jour6 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour7
    /// </summary>
    public int Jour7 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour8
    /// </summary>
    public int Jour8 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour9
    /// </summary>
    public int Jour9 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour10
    /// </summary>
    public int Jour10 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour11
    /// </summary>
    public int Jour11 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour12
    /// </summary>
    public int Jour12 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour13
    /// </summary>
    public int Jour13 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour14
    /// </summary>
    public int Jour14 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour15
    /// </summary>
    public int Jour15 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour16
    /// </summary>
    public int Jour16 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour17
    /// </summary>
    public int Jour17 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour18
    /// </summary>
    public int Jour18 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour19
    /// </summary>
    public int Jour19 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour20
    /// </summary>
    public int Jour20 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour21
    /// </summary>
    public int Jour21 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour22
    /// </summary>
    public int Jour22 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour23
    /// </summary>
    public int Jour23 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour24
    /// </summary>
    public int Jour24 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour25
    /// </summary>
    public int Jour25 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour26
    /// </summary>
    public int Jour26 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour27
    /// </summary>
    public int Jour27 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour28
    /// </summary>
    public int Jour28 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour29
    /// </summary>
    public int Jour29 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour30
    /// </summary>
    public int Jour30 { get; set; }

    /// <summary>
    ///   Obtient ou définit le Jour31
    /// </summary>
    public int Jour31 { get; set; }

    #endregion

    #region IsAnticipeParJour

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe1 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe2 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe3 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe4 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe5 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe6 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe7 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe8 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe9 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe10 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe11 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe12 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe13 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe14 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe15 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe16 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe17 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe18 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe19 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe20 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe21 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe22 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe23 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe24 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe25 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe26 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe27 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe28 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe29 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe30 { get; set; }

    /// <summary>
    ///   Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe31 { get; set; }

    #endregion
  }
}