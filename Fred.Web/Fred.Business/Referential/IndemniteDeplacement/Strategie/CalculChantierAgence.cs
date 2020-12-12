using Fred.Business.Referential.IndemniteDeplacement.Features;
using Fred.Entities.CI;
using Fred.Entities.Personnel;

namespace Fred.Business.Referential.IndemniteDeplacement.Strategie
{
    /// <summary>
    /// Classe de calcul des indemnités déplacement suivant type rattachement agence
    /// </summary>
    /// <typeparam name="TManiereCalcul">manière Orthrodromique, routière ou autre</typeparam>
    public class CalculChantierAgence<TManiereCalcul> : ManiereCalcul<TManiereCalcul>, ITypeRattachementCalcul
        where TManiereCalcul : IManiereCalcul, new()
    {
        /// <summary>
        /// Permet de calculer le nombre de kilomètre
        /// </summary>
        /// <param name="ci">entre imputation</param>
        /// <param name="personnel">personnel</param>
        /// <returns>un nombre décimal</returns>
        public double CalculKilometre(CIEnt ci, PersonnelEnt personnel)
        {
            var chantier = IndemniteDeplacementCalculator.GetGeographicCoordinate(ci);
            var agence = IndemniteDeplacementCalculator.GetGeographicCoordinate(personnel.EtablissementPaie);

            return CalculKilometre(chantier, agence);
        }
    }
}
