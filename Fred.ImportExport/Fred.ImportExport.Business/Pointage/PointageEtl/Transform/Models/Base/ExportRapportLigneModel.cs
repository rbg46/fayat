using System;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Transform
{
  /// <summary>
  /// Classe de base pour l'export des pointages vers SAP
  /// </summary>
  public class ExportRapportLigneModel
  {
    public int RapportId { get; set; }
    public int RapportLigneId { get; set; }
    public DateTime? DateModification { get; set; }
    public DateTime? DateSuppression { get; set; }
    public string SocieteCode { get; set; }
    /// <summary>
    /// correspond au matricule de fred
    /// </summary>
    public string Personnel { get; set; }

    public string CiCode { get; set; }

    public DateTime Date { get; internal set; }

    public string MajorationCode { get; set; }

    public double MajorationHeure { get; set; }

    public string AbsenceCode { get; set; }

    public double AbsenceHeure { get; set; }

    public string DeplacementCode { get; set; }

    public string DeplacementZone { get; set; }

    public string DeplacementIVD { get; set; }

    public string MatriculeSap { get; set; }
    public string MatriculePixid { get; set; }
    public string MatriculeDirectSkills { get; set; }




    public ExportRapportLigneAstreinteModel[] Astreintes { get; set; }
    public ExportRapportLignePrimeModel[] Prime { get; internal set; }
    public ExportRapportTacheModel[] Tache { get; internal set; }
  }
}
