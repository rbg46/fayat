using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fred.Business.Societe
{
    /// <summary>
    /// Interface Sep Service
    /// </summary>
    public interface ISepService : IService
    {
        /// <summary>
        /// Récupération de la société gérante d'une SEP
        /// </summary>
        /// <param name="societeSepId">Identifiant de la société SEP</param>
        /// <returns>Société Gérante</returns>
        SocieteEnt GetSocieteGerante(int societeSepId);

        /// <summary>
        /// RG_5403_001 : Récupération de la liste des « sociétés participantes » d’une société SEP
        /// Les « sociétés participantes » d’une SEP sont toutes les sociétés associées en niveau 1 et en niveau 2 à la SEP.
        /// </summary>
        /// <param name="societeSepId">Identifiant de la société SEP</param>
        /// <returns>Liste des sociétés participantes</returns>
        List<SocieteEnt> GetSocieteParticipantes(int societeSepId);

        /// <summary>
        /// Récupération de tous les fournisseurs dans la SEP
        /// </summary>
        /// <param name="societeSepId">Identifiant de la société SEP</param>
        /// <returns>Liste de fournisseurs</returns>
        List<FournisseurEnt> GetFournisseurs(int societeSepId);

        /// <summary>
        /// SearchLight pour Lookup des CI Sep
        /// CI visibles par l’utilisateur ET rattachés à une société de type SEP.
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="searchedText">Texte a rechercher</param>
        /// <returns>Liste de CI</returns>
        List<CIEnt> SearchLightCiSep(int page, int pageSize, string searchedText);

        /// <summary>
        /// SearchLight pour Lookup des fournisseurs SEP
        /// les Fournisseurs liés à une ou plusieurs sociétés SEP du Groupe de l’utilisateur
        /// (dans la table [FRED_ASSOCIE_SEP], rechercher tous les fournisseurs liés à des SEP du Groupe de l'utilisateur).
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="searchedText">Texte a rechercher</param>
        /// <param name="ciId">CI Id</param>       
        /// <returns>Liste des Fournisseurs</returns>
        List<FournisseurEnt> SearchLightFournisseurSep(int page, int pageSize, string searchedText, int? ciId);

        /// <summary>
        /// récupére  la liste des Sociétés SEP
        /// </summary>
        /// <returns>retourne une liste de société AssocieSepEnt</returns>
        IEnumerable<AssocieSepEnt> GetAll();

        /// <summary>
        /// récupére la liste des Sociétés Sep d'un société participant 
        /// </summary>
        /// <param name="societeId">id société</param>
        /// <returns>retourne une liste de société AssocieSepEnt</returns>
        List<int> GetSepSocieteParticipant(int societeId);

        /// <summary>
        /// récupére la liste des Sociétés Sep d'un société participant dont la société est gérante et partenaire
        /// </summary>
        /// <param name="societeId">id société gérante</param>
        /// <returns>retourne une liste de société AssocieSepEnt Partenaire et Gerante</returns>
        List<int> GetSepSocieteParticipantForContratIterimaire(int societeId);

        /// <summary>
        /// récupére la liste des Sociétés Sep d'un société participant dont la société est gérante
        /// </summary>
        /// <param name="societeId">id société gérante</param>
        /// <returns>retourne une liste de société AssocieSepEnt</returns>
        List<int> GetSepSocieteParticipantWhenSocieteIsGerante(int societeId);

        /// <summary>
        /// Retourne les societes de type sep parmis les societes dont l'id est contenu dans la liste societeIds
        /// </summary>
        /// <param name="societeIds">societeIds</param>
        /// <returns>Liste de societe de tupe Sep</returns>
        List<SocieteEnt> GetSocietesThatAreSep(List<int> societeIds);

        /// <summary>
        /// Récupération des sociétés gérantes d'une SEP en fonction des identifiants des sociétés SEP
        /// </summary>
        /// <param name="societeIds">Identifiant des sociétés SEP</param>
        /// <returns>Sociétés Gérantes</returns>
        Dictionary<int, SocieteEnt> GetSocieteGerantes(List<int> societeIds);

        /// <summary>
        /// Vérifie si une société est de type SEP ou non
        /// </summary>
        /// <param name="societe">Societe</param>
        /// <returns>Renvoie true si la société est de type de SEP, sinon faux</returns>
        bool IsSep(SocieteEnt societe);

        /// <summary>
        /// IsSepAsync
        /// </summary>
        /// <param name="societe">societe param</param>
        /// <returns>bool</returns>
        Task<bool> IsSepAsync(SocieteEnt societe);

        /// <summary>
        /// Get societe associe id for societe when it's Gerant
        /// </summary>
        /// <param name="societe">Societe</param>
        /// <returns>societe associe id</returns>
        Task<int> GetSocieteAssocieIdGerantAsync(SocieteEnt societe);

        /// <summary>
        /// Vérifie si une CI est attaché une société SEP ou non
        /// </summary>
        /// <param name="ci">Ci</param>
        /// <returns>Renvoie true si le CI est attaché à une société SEP sinon faux</returns>
        bool IsSep(int ci);

        /// <summary>
        /// Retourne  societe gérante pour un CI SEP 
        /// </summary>
        /// <param name="societe">Societe ENT</param>
        /// <returns>retourne  société gérante</returns>
        SocieteEnt GetSocieteGeranteForSep(SocieteEnt societe);

        /// <summary>
        /// Retourne  societe gérante pour un CI SEP 
        /// </summary>
        /// <param name="ci">CI</param>
        /// <returns>retourne societé sep</returns>
        SocieteEnt GetSocieteGeranteForSep(int ci);

        /// <summary>
        /// Récupération d'une query Associés SEP
        /// </summary>
        /// <param name="filters">Filtres choisis</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="includeProperties">Tables Inclus</param>
        /// <returns>Query</returns>
        IEnumerable<AssocieSepEnt> GetAssocieswithfilter(List<Expression<Func<AssocieSepEnt, bool>>> filters, Func<IQueryable<AssocieSepEnt>, IOrderedQueryable<AssocieSepEnt>> orderBy = null, List<Expression<Func<AssocieSepEnt, object>>> includeProperties = null);
    }
}
