using System.Collections.Generic;
using System.IO;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.Models.EtatPaie;

namespace Fred.Business.EtatPaie.ControlePointage
{
    /// <summary>
    /// Interface Controle des Pointages
    /// </summary>
    public interface IVerificationDesTempsManager : IManager
    {
        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="listePointageMensuel">Liste des models de pointage mensuel</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        MemoryStream GenerateMemoryStreamVerificationDesTemps(EtatPaieExportModel etatPaieExportModel, List<SummaryMensuelPersoModel> listePointageMensuel, int userId, string templateFolderPath);
    }
}
