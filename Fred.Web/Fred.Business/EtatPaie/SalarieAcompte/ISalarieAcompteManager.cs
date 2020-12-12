using System.Collections.Generic;
using System.IO;
using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.Models.EtatPaie;

namespace Fred.Business.EtatPaie.ControlePointage
{
    /// <summary>
    /// Interface Controle des Pointages
    /// </summary>
    public interface ISalarieAcompteManager : IManager
    {
        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="mensuelPerson">Liste des models de pointage mensuel</param>
        /// <param name="finallist">Liste des modèles résumés de pointage mensuel</param>
        /// <param name="user">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        MemoryStream GenerateMemoryStreamSalarieAcompte(EtatPaieExportModel etatPaieExportModel, List<PointageMensuelPersonEnt> mensuelPerson, List<SummaryMensuelPersoModel> finallist, UtilisateurEnt user, string templateFolderPath);
    }
}
