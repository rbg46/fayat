
using Fred.Entities;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente l'interface de la classe d'accès aux images du personnel
  /// </summary>
  public interface IPersonnelImageRepository : IRepository<PersonnelImageEnt>
  {
    /// <summary>
    ///   Récupération des images d'un personnel
    /// </summary>
    /// <param name="personnelId">Identifiant du personnel</param>
    /// <returns>Entité personnelImage</returns>
    PersonnelImageEnt Get(int personnelId);

    /// <summary>
    ///   Ajout de nouvelles images
    /// </summary>
    /// <param name="images">Entité personnelImage</param>
    /// <returns>vrai si opération réussie, sinon faux</returns>
    PersonnelImageEnt AddOrUpdate(PersonnelImageEnt images);

    /// <summary>
    ///   Supprime les images d'un personnel
    /// </summary>
    /// <param name="personnelImageId">Identifiant du personnelImage</param>
    /// <returns>vrai si opération réussie, sinon faux</returns>
    bool Delete(int personnelImageId);
  }
}