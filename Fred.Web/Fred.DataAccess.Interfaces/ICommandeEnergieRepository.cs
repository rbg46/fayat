using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Rapport;
using Fred.Entities.Valorisation;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Interface Commande Energie Repository
    /// </summary>
    public interface ICommandeEnergieRepository : IRepository<CommandeEnt>
    {
        /// <summary>
        /// Récupération d'un CI avec ses Surcharge Bareme Exploitation et Bareme Exploitation
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="personnelIds">Liste d'identifiants des personnels</param>
        /// <param name="materielIds">Liste d'identifiants de matériels</param>
        /// <param name="referentielEtenduIds">Liste d'identifiants du référentiel étendu</param>
        /// <returns>CI</returns>
        CIEnt GetCi(int ciId, DateTime periode, List<int> personnelIds, List<int> materielIds, List<int> referentielEtenduIds);

        /// <summary>
        /// Récupération d'une query RapportLigneEnt
        /// </summary>
        /// <param name="filters">Filtres choisis</param>
        /// <returns>Query</returns>
        IQueryable<RapportLigneEnt> GetPointagesMateriels(Expression<Func<RapportLigneEnt, bool>> filters);

        /// <summary>
        /// Récupération d'une query RapportLigneEnt
        /// </summary>
        /// <param name="filters">Filtres choisis</param>
        /// <returns>Query</returns>
        IQueryable<RapportLigneEnt> GetPointagesPersonnels(Expression<Func<RapportLigneEnt, bool>> filters);

        /// <summary>
        /// Récupération d'une query RapportLigneEnt
        /// </summary>
        /// <param name="filters">Filtres choisis</param>
        /// <returns>Query</returns>
        IQueryable<RapportLigneEnt> GetPointagesInterimaires(Expression<Func<RapportLigneEnt, bool>> filters);

        /// <summary>
        /// Récupération du détail d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Commande énergie</returns>
        CommandeEnt GetCommandeEnergie(int commandeId);

        /// <summary>
        /// Vérifie si un numéro de commande externe existe déjà en BD
        /// </summary>
        /// <param name="numeroCommandeExterne">Numéro commande externe à vérifier</param>
        /// <returns>Vrai si existe en BD, sinon faux</returns>
        string GetLastByNumeroCommandeExterne(string numeroCommandeExterne);

        /// <summary>
        /// Récupération des lignes de commande énergie pour réception
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande énergie</param>
        /// <returns>Liste de lignes de commandes</returns>
        List<CommandeLigneEnt> GetCommandeLignesForReception(int commandeId);

        /// <summary>
        /// Ajout des réceptions en BD
        /// </summary>
        /// <param name="receptions">Liste des réceptions</param>       
        void AddRangeReception(List<DepenseAchatEnt> receptions);

        /// <summary>
        /// Récupération des réceptions éligibles à l'envoie vers SAP d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Liste de Réceptions</returns>
        List<DepenseAchatEnt> GetReceptionsForSap(int commandeId);

        /// <summary>
        /// Récupération des lignes de commandes pour l'annulation des valorisations après réception
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande énergie</param>        
        /// <returns>Liste des lignes de commandes</returns>
        List<CommandeLigneEnt> GetCommandeLignesForValorisation(int commandeId);

        /// <summary>
        /// Ajout des valorsations en BD
        /// </summary>
        /// <param name="valorisations">Liste des valorisations</param>
        /// <returns>Liste des valorisations ajoutées</returns>
        List<ValorisationEnt> AddRangeValorisation(List<ValorisationEnt> valorisations);
    }
}
