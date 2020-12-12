using Fred.Entities.IndemniteDeplacement;
using System;
using System.Data;

namespace Fred.ImportExport.Business.Kilometre
{
  /// <summary>
  /// Représente une indemnité de déplacement à exporter dans ANAEL.
  /// </summary>
  internal class AnaelIndemniteDeplacement
  {
    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <remarks>Ne pas utiliser directement, utiliser à la place les fonctions FromRvg ou FromFred.</remarks>
    private AnaelIndemniteDeplacement()
    { }

    /// <summary>
    /// Source de l'indemnité de déplacement.
    /// </summary>
    public enum SourceType
    {
      /// <summary>
      /// La source est FRED.
      /// </summary>
      FRED,

      /// <summary>
      /// La source est RVG.
      /// </summary>
      RVG
    }

    /// <summary>
    /// Obtient la société de paie.
    /// </summary>
    public string SocietePaie { get; private set; }

    /// <summary>
    /// Obtient le code affaire.
    /// </summary>
    public string CodeAffaire { get; private set; }

    /// <summary>
    /// Obtient le matricule du personnel.
    /// </summary>
    public string MatriculePersonnel { get; private set; }

    /// <summary>
    /// Obtient la source, FRED ou RVG.
    /// </summary>
    public SourceType Source { get; private set; }

    /// <summary>
    /// Obtient le code de déplacement.
    /// </summary>
    public string CodeDeplacement { get; private set; }

    /// <summary>
    /// Obtient le code de la zone de déplacement.
    /// </summary>
    public string CodeZoneDeplacement { get; private set; }

    /// <summary>
    /// Obtient le kilométrage reel entre le domicile et le chantier.
    /// </summary>
    public double KilometrageReelDomicileChantier { get; private set; }

    /// <summary>
    /// Véhiculé ?
    /// </summary>
    public bool Vehicule { get; private set; }

    /// <summary>
    /// Indique s'il s'agit d'une indemnité de voyage détente.
    /// </summary>
    public bool IVD { get; private set; }

    /// <summary>
    /// Obtient le kilométrage vol d'oiseau entre le domicile et l'établissement de rattachement.
    /// </summary>
    public double KilometrageVolOiseauChantierRattachement { get; private set; }

    /// <summary>
    /// Obtient le kilométrage vole d'oiseau entre le domicile et le chantier.
    /// </summary>
    public double KilometrageVolOiseauDomicileChantier { get; private set; }

    /// <summary>
    /// Crée une indemnité de déplacement pour Anael en provenance de RVG.
    /// </summary>
    /// <param name="reader">Le reader en cours.</param>
    /// <returns>L'indemnité de déplacement pour Anael.</returns>
    public static AnaelIndemniteDeplacement FromRvg(IDataReader reader)
    {
      var readerZoneId = reader["ZONE_ID"] as string;
      var readerKm = reader["KM"];
      var readerVehicule = reader["VEHICULE"];
      var readerIvd = reader["IVD"];
      var readerRatChaKmvo = reader["RAT_CHA_KMVO"];
      var readerDomChaKmvo = reader["DOM_CHA_KMVO"];

      return new AnaelIndemniteDeplacement()
      {
        SocietePaie = reader["ENTRE_SOC_PAYE"] as string,
        CodeAffaire = reader["AFFAI_ID"] as string,
        MatriculePersonnel = (reader["PERSO_ID"] as string)?.TrimEnd(),
        Source = SourceType.RVG,
        CodeDeplacement = reader["CODEP_ID"] as string ?? string.Empty,
        CodeZoneDeplacement = readerZoneId?.Substring(0, Math.Min(20, readerZoneId.Length)) ?? string.Empty,
        KilometrageReelDomicileChantier = readerKm != DBNull.Value ? (short)readerKm : 0,
        Vehicule = readerVehicule != DBNull.Value ? (bool)readerVehicule : false,
        IVD = readerIvd != DBNull.Value ? (bool)readerIvd : false,
        KilometrageVolOiseauChantierRattachement = readerRatChaKmvo != DBNull.Value ? (short)readerRatChaKmvo : 0,
        KilometrageVolOiseauDomicileChantier = readerDomChaKmvo != DBNull.Value ? (short)readerDomChaKmvo : 0
      };
    }

    /// <summary>
    /// Crée une indemnité de déplacement pour Anael en provenance de FRED.
    /// </summary>
    /// <param name="indemniteDeplacement">L'indemnité de déplacement concernée.</param>
    /// <returns>L'indemnité de déplacement pour Anael.</returns>
    public static AnaelIndemniteDeplacement FromFred(IndemniteDeplacementEnt indemniteDeplacement)
    {
      return new AnaelIndemniteDeplacement()
      {
        SocietePaie = indemniteDeplacement.Personnel.Societe.CodeSocietePaye ?? string.Empty,
        CodeAffaire = indemniteDeplacement.CI.Code ?? string.Empty,
        MatriculePersonnel = (indemniteDeplacement.Personnel.Matricule)?.TrimEnd() ?? string.Empty,
        Source = SourceType.FRED,
        CodeDeplacement = indemniteDeplacement.CodeDeplacement?.Code ?? string.Empty,             // NPI : actif / inactif ?
        CodeZoneDeplacement = indemniteDeplacement.CodeZoneDeplacement?.Code ?? string.Empty,     // NPI : actif / inactif ?
        KilometrageReelDomicileChantier = indemniteDeplacement.NombreKilometres,
        Vehicule = false,
        IVD = indemniteDeplacement.IVD,
        KilometrageVolOiseauChantierRattachement = indemniteDeplacement.NombreKilometreVOChantierRattachement ?? 0,
        KilometrageVolOiseauDomicileChantier = indemniteDeplacement.NombreKilometreVODomicileChantier ?? 0
      };
    }
  }
}
