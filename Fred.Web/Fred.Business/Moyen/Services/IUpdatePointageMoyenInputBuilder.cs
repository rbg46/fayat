using System;
using System.Collections.Generic;
using Fred.Entities.Moyen;
using Fred.Framework.DateTimeExtend;

namespace Fred.Business.Moyen
{
    /// <summary>
    /// Cette classe est responsable de générer la requéte de génération des pointage des moyens
    /// La génération aura besoin de pas mal de données pour fonctionner sans des allers retours à la base 
    /// </summary>
    public interface IUpdatePointageMoyenInputBuilder : IService
    {
        /// <summary>
        /// Génération de la requéte pour initier l'update des pointage des moyens
        /// Cette méthode vérifie qu'on a des rapports pour les Cis de génération dans l'interval des dates , 
        /// si les rapports n'existent pas elle les crée .
        /// </summary>
        /// <param name="startDate">Date de début de génération</param>
        /// <param name="endDate">Date de fin de génération</param>
        /// <param name="affectationMoyenList">La liste des affectations</param>
        /// <param name="dateTimeExtendManager">Date time extension manager</param>
        /// <param name="moyenPointageHelper">Moyen pointage helper</param>
        /// <returns>La requéte pour générer le pointage des moyens</returns>
        UpdateMoyenPointageInput GetPointageMoyenRequest(
            DateTime startDate,
            DateTime endDate,
            IEnumerable<AffectationMoyenEnt> affectationMoyenList,
            IDateTimeExtendManager dateTimeExtendManager,
            MoyenPointageHelper moyenPointageHelper);
    }
}
