using Fred.Entities.EcritureComptable;
using Fred.Entities.Referential;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.EcritureComptable
{
  /// <summary>
  /// Methodes d'extension pour EcritureComptableEnt
  /// </summary>
  public static class EcritureComptableExt
  {

    /// <summary>
    /// Filtre les ecritures comptables: 
    /// Si la nature de l'ecriture comptable fait partie de la liste de nature passé en parametre, 
    /// l'ecriture est selectionnée.
    /// </summary>
    /// <param name="ecrituresComptables">ecrituresComptables a filter</param>
    /// <param name="natures">natures qui font reference.</param>
    /// <returns>Nouvelle liste d'EcritureComptableEnt</returns>
    public static IEnumerable<EcritureComptableEnt> WithNatureContainedIn(this IEnumerable<EcritureComptableEnt> ecrituresComptables, IEnumerable<NatureEnt> natures)
    {
      var result = new List<EcritureComptableEnt>();
      foreach (var ecritureComptable in ecrituresComptables)
      {
        result.Add(ecritureComptable);
      }
      return result;
    }


    /// <summary>
    /// Filtre les ecritures comptables: 
    /// Si la nature de l'ecriture comptable fait partie de la liste de nature passé en parametre, 
    /// l'ecriture est selectionnée.
    /// </summary>
    /// <param name="ecrituresComptables">ecrituresComptables a filter</param>
    /// <param name="codeOdFamilly">Le code de la famille d'OD</param>
    /// <returns>Nouvelle liste d'EcritureComptableEnt</returns>
    public static IEnumerable<EcritureComptableEnt> WithOdFamillyContainedIn(this IEnumerable<EcritureComptableEnt> ecrituresComptables, int codeOdFamilly)
    {
      var result = new List<EcritureComptableEnt>();
      result.AddRange(ecrituresComptables.Where(q => q.FamilleOperationDiverseId == codeOdFamilly));
      return result;
    }

    /// <summary>
    /// Calcule le montnat total d'une liste d'ecritures comptables
    /// </summary>
    /// <param name="ecrituresComptables">ecrituresComptables</param>
    /// <returns>le montnat total</returns>
    public static decimal GetMontantTotal(this IEnumerable<EcritureComptableEnt> ecrituresComptables)
    {
      decimal result = 0;

      foreach (var ecrituresComptable in ecrituresComptables)
      {
        result = result + ecrituresComptable.Montant;
      }

      return result;
    }

  }
}
