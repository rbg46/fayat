using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;

namespace Fred.Business.IndemniteDeplacement
{

    /// <summary>
    /// Fonctionnalités Create des indemnités de déplacement qui nécessitent un calcul
    /// </summary>
    public interface ICrudWithCalculFeature
    {
        /// <summary>
        ///   Méthode de récupération ou création d'une indemnité de déplacement en foncion d'un personnel et d'un Ci
        /// </summary>
        /// <param name="personnel">Personnel pour lequel réupérer l'indemnité</param>
        /// <param name="ci">ci pour lequel récupérer l'indemnité.</param>
        /// <param name="refresh">Indique s'il s'agit d'un rafraichissement.</param>
        /// <returns>Retourne l'indemnité de déplacement calculée.</returns>
        IndemniteDeplacementEnt GetOrCreateIndemniteDeplacementByPersonnelAndCi(PersonnelEnt personnel, CIEnt ci, bool refresh);
    }
}
