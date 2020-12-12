﻿using Fred.Business.Referential.IndemniteDeplacement.Features;
using Fred.Entities.CI;
using Fred.Entities.Personnel;

namespace Fred.Business.Referential.IndemniteDeplacement.Strategie
{
    /// <summary>
    /// Classe de calcul des indemnités déplacement suivant type rattachement secteur
    /// </summary>
    /// <typeparam name="TManiereCalcul">manière Orthrodromique, routière ou autre</typeparam>
    public class CalculDomicileSecteur<TManiereCalcul> : ManiereCalcul<TManiereCalcul>, ITypeRattachementCalcul
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
            var domicile = IndemniteDeplacementCalculator.GetGeographicCoordinate(personnel);
            var secteur = IndemniteDeplacementCalculator.GetGeographicCoordinate(personnel.EtablissementRattachement);

            return CalculKilometre(domicile, secteur);
        }
    }
}
