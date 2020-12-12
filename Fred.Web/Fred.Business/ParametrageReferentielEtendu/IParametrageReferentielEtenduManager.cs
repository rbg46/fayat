using Fred.Entities.ReferentielEtendu;

namespace Fred.Business
{
  /// <summary>
  ///  Interface du Gestionnaire des ParametrageReferentielEtendus
  /// </summary>
  public interface IParametrageReferentielEtenduManager : IManager<ParametrageReferentielEtenduEnt>
  {
    /// <summary>
    ///   Supprime un ParametrageReferentielEtendu
    /// </summary>
    /// <param name="parametrageReferentielEtenduId">ID du ParametrageReferentielEtendu à supprimé</param>
    void DeleteById(int parametrageReferentielEtenduId);

    /// <summary>
    ///   Ajoute ou met à jour un nouveau ParametrageReferentielEtenduEnt
    /// </summary>
    /// <param name="parametrageReferentielEtendu"> Paramétrage à ajouter ou mettre à jour </param>
    /// <returns> ParametrageReferentielEtenduEnt ajouté ou mis à jour </returns>
    ParametrageReferentielEtenduEnt AddOrUpdateParametrageReferentielEtendu(ParametrageReferentielEtenduEnt parametrageReferentielEtendu);
  }
}