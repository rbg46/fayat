using System;
using System.Collections.Generic;
using Fred.Entities.Commande;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Business.Avenant
{
    /// <summary>
    /// Interface Avenant Manager
    /// </summary>
    public interface IAvenantManager : IManager<CommandeAvenantEnt>
    {
        /// <summary>
        /// Liste des avenants d'une commande
        /// </summary>
        /// <param name="commandeId">commande Id</param>
        /// <returns>Liste des avenants</returns>
        IEnumerable<CommandeAvenantEnt> GetAvenantByCommandeId(int commandeId);

        /// <summary>
        /// Liste des avenants d'une commande
        /// </summary>
        /// <param name="commandeId">commande id</param>
        /// <returns>retourne avenant valider</returns>
        CommandeAvenantEnt ValideAvenant(int commandeId);

        /// <summary>
        /// Met à jour l'idenfiant du job Hangfire pour un avenant de commande.
        /// </summary>
        /// <param name="commandeId">L'identifant de la commande.</param>
        /// <param name="numeroAvenant">Le numéro d'avenant.</param>
        /// <param name="hangfireJobId">L'identifant du job Hanfire.</param>
        void UpdateAvenantHangfireJobId(int commandeId, int numeroAvenant, string hangfireJobId);


        /// <summary>
        /// Enregistre l'avenant.
        /// </summary>
        /// <param name="model">Le model de l'avenant concerné.</param>
        /// <param name="date">La date à utiliser pour la création, la modification ou la validation.</param>
        /// <param name="commande">La commande concernée.</param>
        /// <param name="avenant">L'avenant concerné.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        CommandeAvenantSave.ResultModel SaveAvenant(CommandeAvenantSave.Model model, DateTime date, out CommandeEnt commande, out CommandeAvenantEnt avenant);
    }
}
