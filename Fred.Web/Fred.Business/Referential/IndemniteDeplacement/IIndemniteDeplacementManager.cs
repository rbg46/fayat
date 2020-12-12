using System.Collections.Generic;
using Fred.Business.Referential.IndemniteDeplacement.Features;
using Fred.Entities.IndemniteDeplacement;

namespace Fred.Business.IndemniteDeplacement
{
    /// <summary>
    ///   Interface des gestionnaires des indemnites de deplacement
    /// </summary>
    public interface IIndemniteDeplacementManager : IManager<IndemniteDeplacementEnt>,
                                                    ICalculFeature,
                                                    ISearchFeature,
                                                    ICrudFeature,
                                                    ICrudWithCalculFeature,
                                                    IExportKlm
    {
        /// <summary>
        /// Retourne les types de calcul des indemnités de déplacement.
        /// </summary>
        /// <returns>Les types de calcul des indemnités de déplacement.</returns>
        IEnumerable<IndemniteDeplacementCalculTypeEnt> GetCalculTypes();
    }
}
