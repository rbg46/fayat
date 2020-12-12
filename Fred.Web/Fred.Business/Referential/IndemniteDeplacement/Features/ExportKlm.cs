using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.IndemniteDeplacement;

namespace Fred.Business.Referential.IndemniteDeplacement.Features
{
    /// <summary>
    /// Fonctionnalités d'export des indemnités de déplacement.
    /// </summary>
    public class ExportKlm : ManagerFeature<IIndemniteDeplacementRepository>, IExportKlm
    {
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="repository">Repository des indemnités de déplacment</param>
        /// <param name="uow">Unit of work</param>
        public ExportKlm(IIndemniteDeplacementRepository repository, IUnitOfWork uow)
          : base(uow, repository)
        { }

        /// <summary>
        /// Retourne les indemnités de déplacement à utiliser lors de l'export KLM.
        /// </summary>
        /// <param name="societeId">Identifiant de a société</param>
        /// <returns>Les indemnités de déplacement à utiliser lors de l'export KLM</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementForExportKlm(int societeId)
        {
            return Repository.GetIndemniteDeplacementForExportKlm(societeId);
        }
    }
}
