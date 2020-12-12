using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Moyen;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.Materiel.Search;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les matériels
    /// </summary>
    public interface IMaterielRepository : IRepository<MaterielEnt>
    {
        /// <summary>
        ///   La liste des matériels avec la société incluse
        /// </summary>
        /// <returns>Renvoie la liste des matériels</returns>
        IQueryable<MaterielEnt> GetDefaultQuery();

        /// <summary>
        ///   Retourne la liste de tous les matériels(actif et inactif).
        /// </summary>
        /// <returns>List de tous les matériels</returns>
        IEnumerable<MaterielEnt> GetMaterielListAll();

        /// <summary>
        ///   Retourne la liste de tous les matériels(actif et inactif) pour la synchronisation mobile.
        /// </summary>
        /// <returns>List de tous les matériels</returns>
        IEnumerable<MaterielEnt> GetMaterielListAllSync();

        /// <summary>
        ///   La liste de tous les matériels.
        /// </summary>
        /// <returns>Renvoie la liste des matériels actifs</returns>
        IEnumerable<MaterielEnt> GetMaterielList();

        /// <summary>
        ///   Sauvegarde les modifications d'un materiel
        /// </summary>
        /// <param name="materiel">Matériel à modifier</param>
        void UpdateMateriel(MaterielEnt materiel);

        /// <summary>
        ///   Ajout d'un matériel
        /// </summary>
        /// <param name="materiel">matériel à ajouter</param>
        /// <returns>materiel</returns>
        MaterielEnt AddMateriel(MaterielEnt materiel);

        /// <summary>
        ///   Supprime un matériel
        /// </summary>
        /// <param name="id">L'identifiant du matériel à supprimer</param>
        void DeleteMaterielById(int id);

        /// <summary>
        ///   Matériel via l'id
        /// </summary>
        /// <param name="id">Id du matériel</param>
        /// <returns>Renvoie un matériel</returns>
        MaterielEnt GetMaterielById(int id);

        MaterielEnt GetMaterielByIdWithCommandes(int id);

        int GetRessourceIdByMaterielId(int id);

        /// <summary>
        ///   Matériel via l'id avec la société
        /// </summary>
        /// <param name="id">Id du matérial</param>
        /// <returns>Renvoie un matériel</returns>
        MaterielEnt GetMaterielByIdWithSociete(int id);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un code materiel
        /// </summary>
        /// <param name="materiel">Code materiele à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(MaterielEnt materiel);

        /// <summary>
        ///   Matériel via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <returns>Renvoie une liste de matériel</returns>
        IEnumerable<MaterielEnt> GetMaterielBySocieteId(int societeId);

        Task<IEnumerable<MaterielSearchModel>> SearchMaterielsAsync(MaterielSearchParameter parameter);

        /// <summary>
        ///   Permet de connaître l'existence d'un matériel depuis son code.
        /// </summary>
        /// <param name="idCourant">id courant</param>
        /// <param name="codeMateriel">code Materiel</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        bool IsMaterielExistsByCode(int idCourant, string codeMateriel, int societeId);

        /// <summary>
        ///   Ligne de recherche
        /// </summary>
        /// <param name="text">Le text recherché</param>
        /// <returns>Renvoie une liste</returns>
        IEnumerable<MaterielEnt> SearchLight(string text);

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);

        /// <summary>
        ///   Permet de récupérer la liste de tous les matériels en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <param name="predicate">Filtres de recherche sur tous les matériels</param>
        /// <returns>Retourne la liste filtrée de tous les matériels</returns>
        IEnumerable<MaterielEnt> GetMateriels(int societeId, Expression<Func<MaterielEnt, bool>> predicate);

        /// <summary>
        /// Retourne un moyen par son code.
        /// </summary>
        /// <param name="code">Le code du moyen.</param>
        /// <returns>Un moyen.</returns>
        MaterielEnt GetMoyen(string code);

        /// <summary>
        /// Permet d'ajouter ou de mettre à jour un moyen.
        /// </summary>
        /// <param name="material">Un moyen.</param>
        /// <returns>Le moyen ajouté ou mis à jour.</returns>
        MaterielEnt AddOrUpdateMoyen(MaterielEnt material);

        /// <summary>
        /// Chercher des moyens en fonction des critéres fournies en paramètres
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="chapitresIds">Liste des chapitres id pour FES</param>
        /// <returns>Liste des moyens</returns>
        IEnumerable<MaterielEnt> SearchLightForMoyen(SearchMoyenEnt filters, int page, int pageSize, IEnumerable<int> chapitresIds);

        /// <summary>
        /// Chercher des immatriculations des moyens en fonction des critéres fournies en paramètres 
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="chapitresIds">les Ids des chapitres moyens </param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des moyens</returns>
        IEnumerable<MaterielEnt> SearchLightForImmatriculation(SearchImmatriculationMoyenEnt filters, IEnumerable<int> chapitresIds, int page = 1, int pageSize = 20);


        /// <summary>
        /// Get moyen for fiche generique
        /// </summary>
        /// <param name="page">page to use </param>
        /// <param name="pageSize">Page size</param>
        /// <param name="text">text of serach by ressource  code</param>
        /// <returns>iEnumerable of MoyenEnt</returns>
        IEnumerable<MaterielEnt> GetMoyenForFicheGenerique(int page = 1, int pageSize = 30, string text = null);

        /// <summary>
        /// Récuération du moyen en fonction de son code et l'id de la société
        /// </summary>
        /// <param name="code">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <returns>Un objet materiel Ent qui corresponds au code et l'id envoyés</returns>
        MaterielEnt GetMoyen(string code, int societeId);

        /// <summary>
        /// Récuération du moyen en fonction de son code et l'id de la société
        /// </summary>
        /// <param name="code">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <param name="etablissementComptableId">Id de l'établiessement comptable</param>
        /// <returns>Un objet materiel Ent qui corresponds au code et l'id envoyés</returns>
        MaterielEnt GetMoyen(string code, int societeId, int? etablissementComptableId);

        Dictionary<(string code, int societeId), int> GetMaterielIdsByCodeAndSocieteId(IEnumerable<MaterielEnt> materiels);

        Task<MaterielEnt> GetMaterielDetailByIdAsync(int id);
    }
}
