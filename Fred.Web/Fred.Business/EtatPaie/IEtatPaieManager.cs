using System.Collections.Generic;
using System.IO;
using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;
using Fred.Web.Shared.Models.EtatPaie;
using Fred.Web.Shared.Models.ValidationPointage;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    ///   Permet de gérer les états paie
    /// </summary>
    public interface IEtatPaieManager : IManager<RapportLigneEnt>
    {
        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        MemoryStream GenerateControlePointages(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath);

        /// <summary>
        /// Génère un pdf pour le controle des pointages Fes
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier pdf généré</returns>
        MemoryStream GenerateControlePointagesFes(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath);

        /// <summary>
        /// Génère un flux de mémoire pour la vérification des temps
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Identifiant de l'éditeur de l'édition</param>
        /// <returns>Le flux de mémoire généré</returns>
        MemoryStream GenerateVerificationTemps(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath);

        /// <summary>
        /// Génère un flux de mémoire pour la liste des primes
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Utilisateur qui édite le document</param>
        /// <returns>Le flux de mémoire généré</returns>
        MemoryStream GenerateDocumentListePrimes(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath);

        /// <summary>
        /// Génère un flux de mémoire pour la liste des IGD
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Utilisateur qui édite le document</param>
        /// <returns>Le flux de mémoire généré</returns>
        MemoryStream GenerateDocumentListeIGD(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath);

        /// <summary>
        /// Génère un flux de mémoire pour la liste des Code Majoration
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="user">Nom du l'utilisateur qui édite le document</param>
        /// <returns>Le flux de mémoire généré</returns>
        MemoryStream GenerateDocumentListeHeuresSpecifiques(EtatPaieExportModel etatPaieExportModel, UtilisateurEnt user, string templateFolderPath);

        /// <summary>
        /// Generate Excel for List Absences Mensuels
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="user">user</param>
        /// <returns>return MemoryStream</returns>
        MemoryStream GenerateListeAbsencesMensuelles(EtatPaieExportModel etatPaieExportModel, UtilisateurEnt user, string templateFolderPath);

        /// <summary>
        /// Génère un pdf pour le controle des pointages Hebdomadaire
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="user">user</param>
        /// <returns>Le fichier pdf généré</returns>
        MemoryStream GenerateControlePointagesHebdomadaire(EtatPaieExportModel etatPaieExportModel, UtilisateurEnt user, string templateFolderPath);

        /// <summary>
        /// Génère un flux de mémoire pour la liste des salarie acompte
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="user">Nom du l'utilisateur qui édite le document</param>
        /// <returns>Le flux de mémoire généré</returns>
        MemoryStream GenerateSalarieAcompte(EtatPaieExportModel etatPaieExportModel, UtilisateurEnt user, string templateFolderPath);

        /// <summary>
        /// Méthode de génération d'un excel pour les primes
        /// </summary>
        /// <param name="insertPrimeParameters">Liste des paramètres pour la requête d'insertion des primes</param>
        /// <returns>Un workbook</returns>
        string BuildInsertQueryPrimeParameters(List<InsertQueryPrimeParametersModel> insertPrimeParameters);

        /// <summary>
        /// Méthode de génération d'un excel pour les pointages
        /// </summary>
        /// <param name="insertPointageParameters">Liste des paramètres pour la requête d'insertion des pointages</param>
        /// <returns>Un workbook</returns>
        string BuildInsertQueryPointageParameters(List<InsertQueryPointageParametersModel> insertPointageParameters);
    }
}
