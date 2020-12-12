using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Commande;
using Fred.Entities.Import;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Business.Commande
{
    /// <summary>
    ///   Gestionnaire des commandes.
    /// </summary>
    public interface ICommandeManager : IManager<CommandeEnt>
    {
        #region Ligne de commande

        /// <summary>
        ///   Retourne la ligne de commande portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="commandeLigneId">Identifiant de la ligne de commande à retrouver.</param>
        /// <returns>La ligne de commande retrouvée, sinon nulle.</returns>
        CommandeLigneEnt GetCommandeLigneById(int commandeLigneId);

        /// <summary>
        ///   Retourne une liste de ligne de commande portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande.</param>
        /// <returns>La ligne de commande retrouvée, sinon nulle.</returns>
        IEnumerable<CommandeLigneEnt> GetListCommandeLigneById(int commandeId);

        /// <summary>
        ///   Retourne la ligne de commande portant l'identifiant unique du materiel externe.
        /// </summary>
        /// <param name="materielId">Identifiant du materiel externe.</param>
        /// <returns>La ligne de commande retrouvée, sinon nulle.</returns>
        CommandeLigneEnt GetCommandeLigneByMaterielId(int materielId);
        #endregion

        #region Commande

        /// <summary>
        ///   Retourne la liste des commandes.
        /// </summary>
        /// <returns>Renvoie la liste des commandes.</returns>
        IEnumerable<CommandeEnt> GetCommandeList();

        /// <summary>
        /// Permet l'idenfiant du job Hangfire pour une commande.
        /// </summary>
        /// <param name="commandeId">L'identifant de la commande.</param>
        /// <param name="hangfireJobId">L'identifant du job Hanfire.</param>
        void UpdateHangfireJobId(int commandeId, string hangfireJobId);

        /// <summary>
        /// Valide commande
        /// </summary>
        /// <param name="commandeId">L'identifant de la commande.</param>
        /// <param name="statut">Status Commande</param>
        void ValidateCommande(int commandeId, StatutCommandeEnt statut);

        /// <summary>
        ///   liste commandes pour mobile
        /// </summary>
        /// <param name="sinceDate">The since date.</param>
        /// <param name="userId">Id utilisateur connecté.</param>
        /// <returns>Liste des commandes pour le mobile.</returns>
        IQueryable<CommandeEnt> GetCommandesMobile(DateTime? sinceDate = null, int? userId = null);

        /// <summary>
        ///   Retourne le commande portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande à retrouver.</param>
        /// <returns>Le commande retrouvée, sinon nulle.</returns>
        CommandeEnt GetCommandeById(int commandeId);

        /// <summary>
        ///   Initialise une nouvelle instance de Commande <see cref="CommandeEnt" /> selon les règles de gestion établies.
        /// </summary>
        /// <returns>Retourne une instance de commande.</returns>
        CommandeEnt GetNewCommande();

        /// <summary>
        ///   Initialise une nouvelle instance de la classe de recherche des commandes
        /// </summary>      
        /// <returns>Objet de filtrage + tri des commandes initialisé</returns>
        SearchCommandeEnt GetNewFilter();

        /// <summary>
        ///   Ajout une nouvelle commande.
        /// </summary>
        /// <param name="commande">Commande à ajouter.</param>
        /// <param name="setTacheParDefaut">Si true, affecte la tâche par défaut aux lignes qui n'en possèdent pas, sinon laisse vide.</param>
        /// <returns>Commande ajoutée</returns>
        int AddCommande(CommandeEnt commande, bool setTacheParDefaut = true);

        /// <summary>
        /// Ajout une commande validée d'un outil externe (SAP, Buyer).
        /// </summary>
        /// <param name="commande">La commande validée.</param>
        /// <returns>La commande ajoutée.</returns>
        CommandeEnt AddCommandeExterne(CommandeEnt commande);

        /// <summary>
        ///   Sauvegarde les modifications d'une commande.
        /// </summary>
        /// <param name="commande">Commande à modifier</param> 
        void UpdateCommande(CommandeEnt commande);

        /// <summary>
        /// Permet de valider l'entête de la commande
        /// </summary>
        /// <param name="commande">Commande à valider</param>
        /// <returns>Commande validée</returns>
        void ValidateHeaderCommande(CommandeEnt commande);

        /// <summary>
        ///   Supprime une commande.
        /// </summary>
        /// <param name="id">L'identifiant de la commande à supprimer.</param>
        void DeleteCommandeById(int id);

        /// <summary>
        ///   Duplique le commande
        /// </summary>
        /// <param name="id">Identifiant de la commande à dupliquer.</param>
        /// <returns>Le commande dupliquée, sinon nulle.</returns>
        CommandeEnt DuplicateCommande(int id);

        /// <summary>
        ///   Recherche le nombre de commandes BUYER à insérer
        /// </summary>
        /// <param name="codeEtab">code de l'étbablissement pour lequel on recherche des commandes BUYER</param>
        /// <param name="dateDebut">date de début des commandes</param>
        /// <param name="dateFin">date de fin des commandes</param>
        /// <returns>Le nombre de commandes récupérées</returns>
        int GetNombreCommandesBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        ///   Procède à l'insertion des commandes récupérées depuis BUYER
        /// </summary>
        /// <param name="codeEtab">code de l'étbablissement pour lequel on recherche des commandes BUYER</param>
        /// <param name="dateDebut">date de début des commandes</param>
        /// <param name="dateFin">date de fin des commandes</param>
        void ImporterCmdsBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Envoie une commande par mail
        /// </summary>
        /// <param name="commande">la commande</param>
        /// <param name="attachement">Pièce jointe à attacher à l'email représentant la commande au format PDF</param>
        /// <exception cref="FredBusinessException">throw en cas d'erreur</exception>
        void SendByMail(CommandeEnt commande, Stream attachement);

        /// <summary>
        ///   Customize le fichier excel contenant la liste des commandes
        /// </summary>
        /// <typeparam name="T">Commande</typeparam>  
        /// <param name="modelList">Liste de commandes</param>
        /// <returns>action de customisation d'un workbook</returns>
        string CustomizeExcelFileForExport<T>(IEnumerable<T> modelList, string path);

        /// <summary>
        /// Retourne une commande par son numéro.
        /// </summary>
        /// <param name="numero">Le numéro de la commande.</param>
        /// <returns>Une commande, sinon nulle.</returns>
        CommandeEnt GetCommande(string numero);

        CommandeEnt GetCommandeByNumberOrExternalNumber(string numero);

        /// <summary>
        /// Retourne la liste des commandes pour une liste de numéros de commandes
        /// </summary>
        /// <param name="numerosCommande">Liste de numéro de commande</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns><see cref="CommandeEcritureComptableOdModel"/></returns>
        IReadOnlyList<CommandeEcritureComptableOdModel> GetCommandeEcritureComptableOdModelByNumeros(List<string> numerosCommande, int ciId);

        /// <summary>
        ///   Clôture de la commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande à clôturer</param>
        /// <returns>La commande si la clôture est effectuée sinon null</returns>
        CommandeEnt CloturerCommande(int commandeId);

        /// <summary>
        ///   Déclôture de la commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande à déclôturer</param>
        /// <returns>La commande si la déclôture est effectuée sinon null</returns>
        CommandeEnt DecloturerCommande(int commandeId);

        /// <summary>
        /// Retourne les éléments à retourner à SAP.
        /// </summary>
        /// <param name="commandeId">L'identifiant de la commande concernée.</param>
        /// <param name="returnCommande">Indique s'il faut retourner la commande.</param>
        /// <param name="returnNumeroAvenants">Indique les numéros des avenants à retourner.</param>
        void GetItemsToReturnToSap(int commandeId, out bool returnCommande, out IEnumerable<int> returnNumeroAvenants);

        #endregion

        /// <summary>
        ///   Gestion de l'export pdf d'une commande
        /// </summary>
        /// <param name="commande">Commande à exporter</param>
        /// <returns>tableau de bytes</returns>
        byte[] ExportPdf(CommandeEnt commande);

        /// <summary>
        ///   Gestion de l'export d'un brouillon pdf d'une commande
        /// </summary>
        /// <param name="commande">Commande à exporter</param>
        /// <returns>tableau de bytes</returns>
        byte[] ExportBrouillonPdf(CommandeEnt commande);

        /// <summary>
        ///   Gestion de l'export pdf d'une commande brouillon avec fournisseur provisoire
        /// </summary>
        /// <param name="commande">Commande à exporter</param>
        /// <returns>tableau de bytes</returns>
        byte[] ExportPdfCommandeBrouillon(CommandeEnt commande);

        /// <summary>
        /// Retourne l'id de l'organisation associé au ci de la commande.
        /// </summary>
        /// <param name="commandeId">commandeId</param>
        /// <returns>l'id de l'organisation associé au ci de la commande</returns>
        int? GetOrganisationIdByCommandeId(int commandeId);

        /// <summary>
        ///   Génération des réceptions pour les commandes abonnement
        /// </summary>
        /// <returns>Liste des commandes abonnements éligibles à la génération auto des réceptions</returns>
        IEnumerable<CommandeEnt> GetCommandeAbonnementList();

        /// <summary>
        ///   Récupération de la dernière date (dernière échéance) de génération des réceptions automatiques des commandes abonnements
        /// </summary>
        /// <param name="dateProchaineReception">Date prochaine génération</param>
        /// <param name="frequenceAbo">Fréquence de l'abonnement</param>
        /// <param name="dureeAbo">Nombre de récetpion restant à générer</param>
        /// <returns>Date</returns>
        DateTime GetLastDateOfReceptionGeneration(DateTime dateProchaineReception, int frequenceAbo, int dureeAbo);

        /// <summary>
        /// Mise à jour rapide d'un commande champ à champ
        /// /!\ Aucune RG appliquée, à utiliser avec précaution... Merci!
        /// </summary>
        /// <param name="commande">Commande à mettre à jour</param>
        /// <param name="userId">Identifiant de l'utilisateur ayant effectué la modification</param>
        /// <param name="fieldsToUpdate">Champs à mettre à jour</param>
        /// <returns>Commande mise à jour</returns>
        CommandeEnt QuickUpdate(CommandeEnt commande, int userId, List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate);

        #region Avenants

        /// <summary>
        /// Enregistre l'avenant.
        /// </summary>
        /// <param name="model">L'avenant concerné.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        CommandeAvenantSave.ResultModel SaveAvenant(CommandeAvenantSave.Model model);

        /// <summary>
        /// Enregistre l'avenant et le valide.
        /// </summary>
        /// <param name="model">L'avenant concerné.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        CommandeAvenantSave.ResultModel ValideAvenant(CommandeAvenantSave.Model model);

        /// <summary>
        /// Met à jour l'idenfiant du job Hangfire pour un avenant de commande.
        /// </summary>
        /// <param name="commandeId">L'identifant de la commande.</param>
        /// <param name="numeroAvenant">Le numéro d'avenant.</param>
        /// <param name="hangfireJobId">L'identifant du job Hanfire.</param>
        void UpdateAvenantHangfireJobId(int commandeId, int numeroAvenant, string hangfireJobId);

        #endregion


        #region Commandes externes

        /// <summary>
        ///   Retourne la liste des systèmes externes.
        /// </summary>
        /// <returns>La liste des systèmes externes.</returns>
        IEnumerable<SystemeExterneEnt> GetCommandeSystemeExterneListPourUtilisateurCourant();

        #endregion
    }
}
