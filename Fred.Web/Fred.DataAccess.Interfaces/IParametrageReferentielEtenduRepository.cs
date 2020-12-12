
using Fred.Entities.ReferentielEtendu;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente le repo de l'entité ParametrageReferentielEtendu
  /// </summary>
  public interface IParametrageReferentielEtenduRepository : IRepository<ParametrageReferentielEtenduEnt>
  {

    /// <summary>
    /// Creation d'un parametrageReferentielEtenduEnt
    /// </summary>
    /// <param name="parametrageReferentielEtenduEnt">parametrageReferentielEtenduEnt</param>
    /// <returns>parametrageReferentielEtendu </returns>
    ParametrageReferentielEtenduEnt InsertParametrageReferentielEtendu(ParametrageReferentielEtenduEnt parametrageReferentielEtenduEnt);


    /// <summary>
    /// Mise a jour d'un parametrageReferentielEtenduEnt
    /// </summary>
    /// <param name="parametrageReferentielEtenduEnt">parametrageReferentielEtenduEnt</param>
    /// <returns>parametrageReferentielEtendu </returns>
    ParametrageReferentielEtenduEnt UpdateParametrageReferentielEtendu(ParametrageReferentielEtenduEnt parametrageReferentielEtenduEnt);
   
  }
}