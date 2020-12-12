using Fred.DataAccess.Interfaces;
using Fred.Entities.Avis;

namespace Fred.DataAccess.Avis
{
    /// <summary>
    /// Reposiory des avis
    /// </summary>
    public interface IAvisRepository : IFredRepository<AvisEnt>
    {
        /// <summary>
        /// Ajouter un avis
        /// </summary>
        /// <param name="avis">Avis à ajouter</param>
        /// <returns>Avis ajouté (attaché)</returns>
        AvisEnt Add(AvisEnt avis);
    }
}
