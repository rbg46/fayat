using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.FonctionnaliteDesactive;
using Fred.Business.Module;
using Fred.Business.PermissionFonctionnalite;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Fonctionnalite;

namespace Fred.Business.Fonctionnalite
{
    /// <summary>
    /// Manager des fonctionnalites
    /// </summary>
    public class FonctionnaliteManager : Manager<FonctionnaliteEnt, IFonctionnaliteRepository>, IFonctionnaliteManager
    {
        private readonly IPermissionFonctionnaliteManager permissionFonctionnaliteManager;
        private readonly IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager;

        public FonctionnaliteManager(
            IUnitOfWork uow,
            IFonctionnaliteRepository fonctionnaliteRepository,
            IFonctionnaliteValidator fonctionnaliteValidator,
            IPermissionFonctionnaliteManager permissionFonctionnaliteManager,
            IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager)
          : base(uow, fonctionnaliteRepository, fonctionnaliteValidator)
        {
            this.permissionFonctionnaliteManager = permissionFonctionnaliteManager;
            this.fonctionnaliteDesactiveManager = fonctionnaliteDesactiveManager;
        }

        /// <summary>
        /// Retourne la liste des commandes
        /// </summary>
        /// <returns>Liste des commandes.</returns>
        public IEnumerable<FonctionnaliteEnt> GetFeatureList()
        {
            return Repository.GetFeatureList().OrderBy(c => c.Code);
        }

        /// <summary>
        /// Retourne la fonctionnalité portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="featureId">Identifiant d'une fonctionnalité à retrouver.</param>
        /// <returns>La fonctionnalité retrouvée, sinon null.</returns>
        public FonctionnaliteEnt GetFeatureById(int featureId)
        {
            return Repository.GetFeatureById(featureId);
        }

        /// <summary>
        /// Retourne une liste de fonctionnalités via l'identifiant du module lié.
        /// </summary>
        /// <param name="moduleId">Identifiant du module lié aux fonctionnalités à retrouver.</param>
        /// <returns>Une liste de fonctionnalités.</returns>
        public IEnumerable<FonctionnaliteEnt> GetFeatureListByModuleId(int moduleId)
        {
            return Repository.GetFeatureListByModuleId(moduleId).OrderBy(c => c.Code);
        }

        /// <summary>
        /// Ajoute une nouvelle fonctionnalité
        /// </summary>
        /// <param name="feature">fonctionnalite à ajouter</param>
        /// <returns>L'identifiant d'une fonctionnalite ajoutée</returns>
        public FonctionnaliteEnt AddFeature(FonctionnaliteEnt feature)
        {
            ValidationResult result = Validator.Validate(feature);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            Repository.AddFeature(feature);
            this.Save();

            return feature;
        }

        /// <summary>
        /// Supprime une fonctionnalite.
        /// </summary>
        /// <param name="id">L'identifiant de la fonctionnalite à supprimer</param>
        public void DeleteFeatureById(int id)
        {
            if (!permissionFonctionnaliteManager.GetPermissionFonctionnalites(id).Any())
            {
                Repository.DeleteFeatureById(id);
                Save();
                permissionFonctionnaliteManager.DeletePermissionFonctionnaliteListByFonctionnaliteId(id);
            }
            else
            {
                ThrowBusinessValidationException("Exist", "Impossible de supprimer cette fonctionnalité car des permissions lui sont associées");
            }
        }

        /// <summary>
        /// Met à jour une fonctionnalite.
        /// </summary>
        /// <param name="feature">L'identifiant de la fonctionnalite à mettre à jour</param>
        /// <returns>Renvoie un booléean</returns>
        public FonctionnaliteEnt UpdateFeature(FonctionnaliteEnt feature)
        {
            ValidationResult result = Validator.Validate(feature);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            Repository.UpdateFeature(feature);
            Save();

            return feature;
        }

        /// <summary>
        /// Supprime toutes les fonctionnaltiés liées à un module
        /// </summary>
        /// <param name="moduleId">L'identifiant du module dont les fonctionnalités sont à supprimer</param>
        public void DeleteFeatureListByModuleId(int moduleId)
        {
            var feactureList = GetFeatureListByModuleId(moduleId);

            foreach (FonctionnaliteEnt f in feactureList.ToList())
            {
                DeleteFeatureById(f.FonctionnaliteId);
            }
        }

        /// <summary>
        /// Recupere les fonctionnalites pour un role donné.
        /// le role doit etre actif sinon rien n'est retourné.
        /// Le module lié a la fonctionnalite ne doit pas etre desactive voir table FRED_MODULE_DESACTIVE sinon aucune fonctionnalite du module n'est retournée.
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>Liste de fonctionnalités 'active'</returns>
        public IEnumerable<FonctionnaliteEnt> GetFonctionnalitesForRoleId(int roleId)
        {
            return Repository.GetFonctionnalitesForRoleId(roleId);
        }

        /// <summary>
        /// Recupere les fonctionnalites pour un utilisateur donné.
        /// le role doit etre actif sinon rien n'est retourné.
        /// Le module lié a la fonctionnalite ne doit pas etre desactive voir table FRED_MODULE_DESACTIVE sinon aucune fonctionnalite du module n'est retournée.
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <returns>Liste de fonctionnalités 'active'</returns>
        public IEnumerable<FonctionnaliteEnt> GetFonctionnalitesForUtilisateur(int utilisateurId)
        {
            return Repository.GetFonctionnalitesForUtilisateur(utilisateurId);
        }

        /// <summary>
        /// Retourne la liste des fonctionnalites disponibles pour la societe et le module.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="moduleId">moduleId</param>
        /// <returns>La liste des fonctionnalites disponibles pour la societe.</returns>
        public IEnumerable<FonctionnaliteEnt> GetFonctionnaliteAvailablesForSocieteIdAndModuleId(int societeId, int moduleId)
        {
            var result = new List<FonctionnaliteEnt>();
            var fonctionnaliteDesactivesIds = this.fonctionnaliteDesactiveManager.GetInactifFonctionnalitesForSocieteId(societeId).Select(fd => fd.FonctionnaliteId);
            var fonctionnalites = this.Repository.GetFeatureListByModuleId(moduleId).ToList();
            foreach (var fonctionnalite in fonctionnalites)
            {
                if (!fonctionnaliteDesactivesIds.Contains(fonctionnalite.FonctionnaliteId))
                {
                    result.Add(fonctionnalite);
                }
            }
            return result;
        }
    }
}
