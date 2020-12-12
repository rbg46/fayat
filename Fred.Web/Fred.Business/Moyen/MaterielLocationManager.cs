using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Moyen;
using Fred.Framework.Exceptions;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.Moyen
{
    public class MaterielLocationManager : Manager<MaterielLocationEnt, IMaterielLocationRepository>, IMaterielLocationManager
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IMapper mapper;

        public MaterielLocationManager(
            IUnitOfWork uow,
            IMaterielLocationRepository materielLocationRepository,
            IUtilisateurManager utilisateurManager,
            IMapper mapper)
            : base(uow, materielLocationRepository)
        {
            this.utilisateurManager = utilisateurManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Add materiel location
        /// </summary>
        /// <param name="materielLocationEnt">Materiel location to add</param>
        /// <returns>Identifiant du materiel location ajouté</returns>
        public int AddMaterielLocation(MaterielLocationEnt materielLocationEnt)
        {
            if (materielLocationEnt == null)
            {
                throw new FredBusinessException(BusinessResources.ErrorMaterielLocationNull);
            }
            materielLocationEnt.AuteurCreationId = this.utilisateurManager.GetContextUtilisateurId();
            try
            {
                return Repository.AddMaterielLocation(materielLocationEnt);
            }
            catch (FredBusinessException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Update d'un matériel de type location
        /// </summary>
        /// <param name="materielLocation">Matériel de type location</param>
        /// <returns>Identifiant du matériel modifié</returns>
        public int UpdateMaterielLocation(MaterielLocationEnt materielLocation)
        {
            if (materielLocation == null)
            {
                throw new FredBusinessException(BusinessResources.ErrorMaterielLocationNull);
            }
            try
            {
                materielLocation.AuteurModificationId = this.utilisateurManager.GetContextUtilisateurId();
                materielLocation.DateModification = DateTime.UtcNow;
                return Repository.UpdateMaterielLocation(materielLocation);
            }
            catch (FredBusinessException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Chercher des immatriculations des moyens en fonction des critéres fournies en paramètres 
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="chapitresIds">les Ids des chapitres moyens </param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des moyens</returns>
        public IEnumerable<MoyenImmatriculationModel> SearchLightForImmatriculation(SearchImmatriculationMoyenEnt filters, IEnumerable<int> chapitresIds, int page = 1, int pageSize = 20)
        {
            return Repository.SearchLightForImmatriculation(filters, chapitresIds, page, pageSize)
                  .Select(m => new MoyenImmatriculationModel
                  {
                      Immatriculation = m.Immatriculation,
                      Libelle = m.Libelle
                  });
        }

        /// <summary>
        /// Retourne toutes les locations qui ont une date de suppression null 
        /// </summary>
        /// <returns>List des Locations</returns>
        public IEnumerable<MaterielLocationModelFull> GetAllActiveLocation()
        {
            return Repository.GetAllActiveLocation().Select(m => new MaterielLocationModelFull
            {
                Etablissement = m.Materiel.EtablissementComptable.Code,
                Societe = m.Materiel.Societe.CodeLibelle,
                Immatriculation = m.Immatriculation,
                Libelle = m.Libelle,
                NumeroParc = m.Materiel.Code,
                MaterielLocationId = m.MaterielLocationId,
                MaterielId = m.Materiel.MaterielId,
                AuteurCreationId = m.Materiel.AuteurCreationId,
                AuteurModificationId = m.Materiel.AuteurModificationId,
                AuteurSuppressionId = m.Materiel.AuteurSuppressionId,
                DateCreation = m.DateCreation,
                FicheGeneriqueModel = new FicheGeneriqueModel { Ressource = mapper.Map<RessourceModel>(m.Materiel.Ressource) },
                ModeleLocation = new ModeleLocationModel
                {
                    Libelle = m.Materiel.Libelle
                }
            });
        }

        /// <summary>
        /// Supprimer une location 
        /// </summary>
        /// <param name="materielLocationId">L'id du materiel a supprimer</param>
        public void DeleteMaterielLocation(int materielLocationId)
        {
            try
            {
                Repository.DeleteMaterielLocation(materielLocationId);
            }
            catch (FredBusinessException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
