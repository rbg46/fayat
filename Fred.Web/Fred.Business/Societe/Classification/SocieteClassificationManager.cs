using System.Collections.Generic;
using System.Linq;
using Fred.Business.Societe.Interfaces;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Societe.Classification;

namespace Fred.Business.Societe.Classification
{
    /// <summary>
    /// Gestionnaire des classifications sociétés.
    /// </summary>
    public class SocieteClassificationManager : Manager<SocieteClassificationEnt, ISocieteClassificationRepository>, ISocieteClassificationManager
    {
        /// <summary>
        /// Injection du validator
        /// </summary>
        protected readonly ISocieteClassificationValidator societeClassificationValidator;

        public SocieteClassificationManager(IUnitOfWork uow, ISocieteClassificationRepository societeClassificationRepository, ISocieteClassificationValidator validator)
          : base(uow, societeClassificationRepository, validator)
        {
            societeClassificationValidator = validator;
        }

        /// <summary>
        /// Insertion et Mise à jour d'une liste des classifications
        /// </summary>        
        /// <param name="classifications">Liste des classifications à maj</param>
        /// <returns>Liste des classifications avec maj</returns>
        public IEnumerable<SocieteClassificationEnt> CreateOrUpdateRange(IEnumerable<SocieteClassificationEnt> classifications)
        {
            foreach (SocieteClassificationEnt classification in classifications.Select(Repository.PopulateSocietes))
            {
                BusinessValidation(classification);

                if (classification.SocieteClassificationId > 0)
                {
                    //Contrôle avant désactivation
                    if (classification.Statut.Equals(false))
                    {
                        societeClassificationValidator.DisablingValidation(classification);
                    }
                    Repository.Update(classification);
                }
                else
                {
                    societeClassificationValidator.AddingValidation(classifications, classification);
                    Repository.Insert(classification);
                }
            }

            Save();

            return classifications;
        }

        /// <summary>
        /// Suppression d'une liste des classifications par leur identifiants        
        /// </summary>
        /// <param name="classifications">Liste des classifications</param>
        public void DeleteRange(IEnumerable<SocieteClassificationEnt> classifications)
        {
            foreach (var classification in classifications.Select(Repository.PopulateSocietes))
            {
                societeClassificationValidator.DeletingValidation(classification);
                Repository.DeleteById(classification.SocieteClassificationId);
            }

            Save();
        }

        /// <summary>
        /// Retourne la liste des classifications Sociétés
        /// </summary>
        /// <param name="onlyActive">flag pour avoir que les actifs</param>
        /// <returns>Renvoie toute la liste des classifications sociétés.</returns>
        public IEnumerable<SocieteClassificationEnt> GetAll(bool onlyActive)
        {
            return onlyActive ? Repository.GetOnlyActive() : Repository.Get();
        }

        /// <summary>
        /// Retourne la liste des classifications Sociétés du groupe
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe de la société</param>
        /// <param name="onlyActive">flag pour avoir seulement les actifs</param>
        /// <returns>Renvoie la liste des classifications sociétés.</returns>
        public IEnumerable<SocieteClassificationEnt> GetByGroupeId(int groupeId, bool? onlyActive)
        {
            return Repository.GetByGroupeId(groupeId, onlyActive);
        }

        /// <summary>
        /// Retourner la requête de récupération des classifications 
        /// </summary>
        /// <param name="recherche">Le texte recherché</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <returns>La liste des classifications recherchées</returns>
        public IEnumerable<SocieteClassificationEnt> Search(string recherche, int? page = null, int? pageSize = null)
        {
            return Repository.Search(recherche, page, pageSize);
        }
    }
}
