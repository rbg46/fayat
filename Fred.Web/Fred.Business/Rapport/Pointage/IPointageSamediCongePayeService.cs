using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    /// Service qui gere la generation des samedi en CP
    /// </summary>
    public interface IPointageSamediCongePayeService : IService
    {
        /// <summary>
        ///   Génération des samedi en CP
        /// </summary>
        /// <param name="pointage">La base du pointage</param>
        void GenerationPointageSamediCP(RapportLigneEnt pointage);

        /// <summary>
        /// Indique si le pointage est un samedi en congés payé.
        /// </summary>
        /// <param name="pointage">Le pointage concerné.</param>
        /// <returns>True si le pointage est un samedi en congés payé, sinon false.</returns>
        bool IsSamediCP(RapportLigneEnt pointage);
    }
}
