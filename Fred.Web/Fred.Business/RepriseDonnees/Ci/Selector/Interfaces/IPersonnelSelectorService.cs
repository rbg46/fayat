using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Entities.Personnel;

namespace Fred.Business.RepriseDonnees.Ci.Selector
{
    /// <summary>
    /// Service qui recuper le personnel de la base en fonction des données contextuelle a l'import
    /// </summary>
    public interface IPersonnelSelectorService : IService
    {
        /// <summary>
        ///  Recupere le personnel de fred, en fonction du codeSociete et du matricule
        /// </summary>
        /// <param name="codeSociete">code de la societe</param>
        /// <param name="matricule">matricule du personnel</param>
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>le personne lde la base de fred</returns>
        PersonnelEnt GetPersonnel(string codeSociete, string matricule, ContextForImportCi context);
    }
}
