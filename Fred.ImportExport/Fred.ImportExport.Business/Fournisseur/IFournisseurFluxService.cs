using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur
{
    /// <summary>
    /// Interface pour gerer le flux fournisseur
    /// </summary>
    public interface IFournisseurFluxService : IService
    {
        /// <summary>
        /// Lance l'import des fournisseurs avec parametres d'entrees
        /// </summary>
        /// <param name="bypassDate">si import complet</param>
        /// <param name="codeSocieteComptable">code societe</param>
        /// <param name="typeSequences">type sequence</param>
        /// <param name="regleGestions">regle de gestion</param>
        /// <param name="isStormOutputDesactivated">si envoie a SAP</param>
        Task ImportationProcessAsync(bool bypassDate, string codeSocieteComptable, string typeSequences, string regleGestions, bool isStormOutputDesactivated = false);

        /// <summary>
        /// Import fournisseurs a partir d'une liste de fournisseurs
        /// </summary>
        /// <param name="fournisseurs">liste des fournisseurs</param>
        Task ImportFournisseursAsync(List<FournisseurModel> fournisseurs);
    }
}
