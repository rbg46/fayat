using System;
using System.Collections.Generic;
using System.IO;
using Fred.Entities.Rapport;
using Fred.Web.Shared.Models.EtatPaie;

namespace Fred.Business.EtatPaie.ControlePointage
{
    /// <summary>
    /// Interface Controle des Pointages
    /// </summary>
    public interface IControlePointagesHebdomadaireManager : IManager
    {
        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="listePointageMensuel">Liste des models de pointage mensuel</param>
        /// <param name="firstDay">Date du premier jour de la semaine</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        MemoryStream GenerateMemoryStreamControlePointagesHebdomadaire(EtatPaieExportModel etatPaieExportModel, List<PointageMensuelPersonEnt> listePointageMensuel, DateTime firstDay, int userId, string templateFolderPath);
    }
}
