using System.Diagnostics;

namespace Fred.Entities.ReferentielEtendu
{
  /// <summary>
  /// Cette Entité ne sert qu'a l'export du bareme budget et n'a pas besoin d'etre stockée en base.
  /// Elle correspond aux colonnes du fichier excel de sortie.
  /// ParametrageReferentielEtenduExportEnt
  /// </summary>
  [DebuggerDisplay("CodeRessource = {CodeRessource} MontantSociete = {MontantSociete} MontantPUO = {MontantPUO} MontantUO = {MontantUO} MontantEtablissement = {MontantEtablissement}")]
  public class ParametrageReferentielEtenduExportEnt
  {
    /// <summary>
    /// CodeChapitre
    /// </summary>
    public string CodeChapitre { get; set; }
    /// <summary>
    /// Chapitre
    /// </summary>
    public string Chapitre { get; set; }
    /// <summary>
    /// CodeSousChapitre
    /// </summary>
    public string CodeSousChapitre { get; set; }
    /// <summary>
    /// CodeSousChapitre
    /// </summary>
    public string SousChapitre { get; set; }
    /// <summary>
    /// CodeRessource
    /// </summary>
    public string CodeRessource { get; set; }

    /// <summary>
    /// CodeRessource
    /// </summary>
    public string Ressource { get; set; }
    /// <summary>
    /// Unite
    /// </summary>
    public string Unite { get; set; }
    /// <summary>
    /// MontantSociete
    /// </summary>
    public decimal? MontantSociete { get; set; }
    /// <summary>
    /// MontantPUO
    /// </summary>
    public decimal? MontantPUO { get; set; }
    /// <summary>
    /// MontantUO
    /// </summary>
    public decimal? MontantUO { get; set; }
    /// <summary>
    /// MontantEtablissement
    /// </summary>
    public decimal? MontantEtablissement { get; set; }
    /// <summary>
    /// Synthese
    /// </summary>
    public decimal? Synthese { get; set; }

  }
}
