using Fred.Framework.Tool;

namespace Fred.Business.Referential.IndemniteDeplacement
{
    /// <summary>
    /// interface permettant de calculer suivant la manière Orthrodromique, routière ou autre
    /// </summary>
    public interface IManiereCalcul
    {
        /// <summary>
        /// Permet de calculer le kilométarge selon la manière choisi 
        /// </summary>
        /// <param name="chantier">coordonnée du chantier</param>
        /// <param name="rattachement">coordonnée du rattachement</param>
        /// <returns>nombre décimale</returns>
        double CalculKilometre(GeographicCoordinate chantier, GeographicCoordinate rattachement);
    }
}
