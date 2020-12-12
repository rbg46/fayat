using System.Threading.Tasks;

namespace Fred.Business.ExternalService
{
    public interface IFournisseurManagerExterne : IManagerExterne
    {
        /// <summary>
        ///   Exécute l'import des fournisseur ANAEL vers FRED
        /// </summary>
        /// <param name="codeSocieteComptable">Code société comptable ANAEL</param>
        /// <exception cref="FredBusinessException">Erreur lors de la requête vers Fred Import/Export</exception>
        /// <returns>Vrai si l'opération s'est bien lancée</returns>
        Task<bool> ExecuteImport(string codeSocieteComptable);
    }
}
