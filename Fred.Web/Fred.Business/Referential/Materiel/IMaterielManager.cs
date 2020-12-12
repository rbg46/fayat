using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Referential;
using Fred.GroupSpecific.Infrastructure;
using Fred.Web.Shared.Models.Materiel.Search;

namespace Fred.Business.Referential.Materiel
{
    /// <summary>
    ///   L'interface Manager des matériels
    /// </summary>
    public interface IMaterielManager : IManager<MaterielEnt>, IGroupAwareService
    {
        /// <summary>
        ///   Retourne la liste de tous les matériels.
        /// </summary>
        /// <returns>List de toutes les matériels</returns>
        IEnumerable<MaterielEnt> GetMaterielListAll();

        /// <summary>
        ///   La liste de tous les matériels.
        /// </summary>
        /// <returns>Renvoie la liste des matériels actifs</returns>
        IEnumerable<MaterielEnt> GetMaterielList();

        /// <summary>
        ///   Sauvegarde les modifications d'un Materiel
        /// </summary>
        /// <param name="materiel">matériels à modifier</param>
        /// <returns>Le matériel mis à jour</returns>
        MaterielEnt UpdateMateriel(MaterielEnt materiel);

        /// <summary>
        ///   Ajout d'une code d'absence
        /// </summary>
        /// <param name="materiel">Code d'absence à ajouter</param>
        /// <returns>L'identifiant du code d'absence ajouté</returns>
        int AddMateriel(MaterielEnt materiel);

        /// <summary>
        ///   Supprime un code materiel
        /// </summary>
        /// <param name="materiel">Le code materiel à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        bool DeleteMaterielById(MaterielEnt materiel);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un code materiel
        /// </summary>
        /// <param name="materiel">Le code materiel à vérifier</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(MaterielEnt materiel);

        /// <summary>
        ///   Code d'absence via l'id
        /// </summary>
        /// <param name="id">Id du code d'absence</param>
        /// <returns>Renvoie un code d'absence</returns>
        MaterielEnt GetMaterielById(int id);

        /// <summary>
        ///   Matériel via l'id avec la société
        /// </summary>
        /// <param name="id">Id du matérial</param>
        /// <returns>Renvoie un matériel</returns>
        MaterielEnt GetMaterielByIdWithSociete(int id);

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de matériel.
        /// </summary>
        /// <param name="societeId">Id societe</param>
        /// <returns>Nouvelle instance de matériel initialisée</returns>
        MaterielEnt GetNewMateriel(int societeId);

        /// <summary>
        ///   Permet de connaître l'existence d'une société depuis son code.
        /// </summary>
        /// <param name="idCourant">id courant</param>
        /// <param name="codeMateriel">code Materiel</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        bool IsMaterielExistsByCode(int idCourant, string codeMateriel, int societeId);

        Task<IEnumerable<MaterielSearchModel>> SearchMaterielsAsync(MaterielSearchParameter parameter);

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="page">page courante</param>
        /// <param name="pageSize">la taille de la page</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="materielLocation">Indique s'il faut retourner les matériel externe ou interne</param>
        /// <param name="includeStorm">Indique s'il faut inclure ou non le matériel STORM. Si null, inclut le matériel STORM.</param>
        /// <returns>Une liste d' items de référentiel</returns>
        IEnumerable<MaterielEnt> SearchLight(string text, int page, int pageSize, int? societeId, int? ciId, bool materielLocation, bool? includeStorm = null);

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);

        /// <summary>
        /// Permet d'ajouter une liste de materiels
        /// Unicité sera sur le code
        /// </summary>
        /// <param name="materiels">La liste des materiels.</param>
        void InsertOrUpdate(IEnumerable<MaterielEnt> materiels);

        /// <summary>
        ///   Permet de récupérer la liste de tous les matériels en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <param name="predicate">Filtres de recherche sur tous les matériels</param>
        /// <returns>Retourne la liste filtrée de tous les matériels</returns>
        IEnumerable<MaterielEnt> GetMateriels(int societeId, Expression<Func<MaterielEnt, bool>> predicate);

        /// <summary>
        /// Permet d'ajouter  ou modifier une liste de materiels
        /// </summary>
        /// <param name="expression">An expression specifying the properties that should be used when determining whether an Add or Update operation should be performed.</param>
        /// <param name="materiels">Les Materiels a mettre a jour ou a inserer</param>
        void InsertOrUpdate(Func<MaterielEnt, object> expression, IEnumerable<MaterielEnt> materiels);

        Task<MaterielEnt> GetMaterielDetailByIdAsync(int id);
    }
}
