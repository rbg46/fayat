using System;
using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;

namespace Fred.Business.IndemniteDeplacement
{

    /// <summary>
    /// Fonctionnalités de calculs des indemnités de déplacement
    /// </summary>
    public interface ICalculFeature
    {
        /// <summary>
        ///   Méthode de calcul d'une indemnité de déplacement pour un salarié et un CI suivant les règles de génération RAZEL-BEC
        /// </summary>
        /// <param name="personnel"> Personnel pour laquelle calculer l'indemnité </param>
        /// <param name="ci"> Affaire pour laquelle calculer l'indemnité </param>
        /// <returns> Retourne l'indemnité de déplacement calculée </returns>
        IndemniteDeplacementEnt CalculIndemniteDeplacement(PersonnelEnt personnel, CIEnt ci);

        /// <summary>
        ///   Méthode de calcul d'une indemnité de déplacement prévisionnelle pour un salarié et un CI suivant les règles de
        ///   génération RAZEL-BEC.
        /// </summary>
        /// <param name="dernierPointageReel">Dernier pointage réel de la période de paie pour un salarié</param>
        /// <param name="datePointagePrevisionnel">Date de génération du prochain pointage prévisionnelle</param>
        /// <returns>Retourne l'indemnité de déplacement prévisionnelle calculée.</returns>
        IndemniteDeplacementEnt CalculIndemniteDeplacementGenerationPrevisionnelle(RapportLigneEnt dernierPointageReel, DateTime datePointagePrevisionnel);

        /// <summary>
        ///   Permet de récupérer une zone de déplacement en fonction d'une distance kilométrique
        /// </summary>
        /// <param name="societeId">Identifiant de la société d'appartenance du salarié</param>
        /// <param name="distanceInKM">Distance destinée au calcul de la zone dse déplacement</param>
        /// <returns>
        ///   Retourne une zone de déplacement en fonction de la société et de la distance passée ne paramètre, peut
        ///   renvoyer null en fonction des BIZZRULES
        /// </returns>
        CodeZoneDeplacementEnt GetZoneByDistance(int societeId, double distanceInKM);


        /// <summary>
        ///   Méthode de récupération d'une Indemnite Deplacement via la méthode "Calcul"
        /// </summary>
        /// <param name="indemniteDeplacementCalcul">Indemnite Deplacement à calculer </param>
        /// <returns> Indemnite Deplacement Calculée </returns>
        IndemniteDeplacementEnt GetCalculIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacementCalcul);

        /// <summary>
        /// Indique s'il est possible de calculer les indemnités de déplacement.
        /// </summary>
        /// <param name="personnel">Le personnel concerné.</param>
        /// <param name="ci">Le Ci concerné.</param>
        /// <returns>True s'il est possible de faire le calcul, sinon false et la liste d'erreurs</returns>
        Tuple<bool, List<string>> CanCalculateIndemniteDeplacement(PersonnelEnt personnel, CIEnt ci);
    }
}
