using System.Collections.Generic;
using System.IO;
using Fred.Entities.Utilisateur;
using Fred.Web.Shared.Models.EtatPaie;

namespace Fred.Business.EtatPaie.ControlePointage
{
    /// <summary>
    /// Interface Controle des Pointages
    /// </summary>
    public interface IListeAbsencesMensuellesManager : IManager
    {
        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="listeAbsencesMensuels">Liste des models de pointage mensuel</param>
        /// <param name="user">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        MemoryStream GenerateMemoryStreamListeAbsencesMensuelles(EtatPaieExportModel etatPaieExportModel, List<AbsenceLigne> listeAbsencesMensuels, UtilisateurEnt user, string templateFolderPath);
    }
}
