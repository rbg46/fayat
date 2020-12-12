using System.Linq;

namespace Fred.ImportExport.DataAccess.Common
{
    /// <summary>
    ///   Interface du Repository de base.
    /// </summary>
    /// <typeparam name="T">Type de l'entité à gérer</typeparam>
    public interface IRepository<T>
  {
    /// <summary>
    ///   Permet d'ajouter une nouvelle entité
    /// </summary>
    /// <param name="entity">L'entité à ajouter</param>
    void Add(T entity);

    /// <summary>
    ///   Permet de mettre à jour une entité
    /// </summary>
    /// <param name="entity">L'entité à mettre à jour</param>
    void Update(T entity);

    /// <summary>
    ///   Permet de supprimer une entité
    /// </summary>
    /// <param name="entity">Entité à supprimer</param>
    void Delete(T entity);

    /// <summary>
    ///   Permet de récupérer une entité
    /// </summary>
    /// <param name="id">L'identifiant de l'entité</param>
    /// <returns>Une entité</returns>
    T GetById(int id);

    /// <summary>
    ///   Permet de requêter  une entité
    /// </summary>
    /// <returns>Un IQueryable</returns>
    IQueryable<T> Get();
    }
}
