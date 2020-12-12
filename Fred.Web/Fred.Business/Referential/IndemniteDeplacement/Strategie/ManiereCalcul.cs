using Fred.Framework.Tool;

namespace Fred.Business.Referential.IndemniteDeplacement
{
    /// <summary>
    /// Classe generic des manières de calcul des indemnités de déplacement
    /// </summary>
    /// <typeparam name="TManiereCalcul">manière Orthrodromique, routière ou autre</typeparam>
    public class ManiereCalcul<TManiereCalcul> : IManiereCalcul
        where TManiereCalcul : IManiereCalcul, new()
    {
        private readonly IManiereCalcul maniere = new TManiereCalcul();

        /// <summary>
        /// Permet de calculer le kilométrage selon la manière choisi 
        /// </summary>
        /// <param name="chantier">coordonnée du chantier</param>
        /// <param name="rattachement">coordonnée du rattachement</param>
        /// <returns>nombre décimale</returns>
        public double CalculKilometre(GeographicCoordinate chantier, GeographicCoordinate rattachement)
        {
            return maniere.CalculKilometre(chantier, rattachement);
        }
    }
}
