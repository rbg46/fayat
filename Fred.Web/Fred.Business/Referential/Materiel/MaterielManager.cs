using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Materiel.Search;

namespace Fred.Business.Referential.Materiel
{
    public class MaterielManager : Manager<MaterielEnt, IMaterielRepository>, IMaterielManager
    {
        private readonly IUtilisateurManager utilisateurMgr;
        private readonly IValorisationManager valorisationMgr;
        private readonly ICIManager ciMgr;
        private readonly ISepService sepService;

        protected MaterielManager(
            IUnitOfWork uow,
            IMaterielRepository materielRepository,
            IUtilisateurManager utilisateurMgr,
            ICIManager ciMgr,
            IValorisationManager valorisationMgr,
            ISepService sepService)
            : base(uow, materielRepository)
        {
            this.utilisateurMgr = utilisateurMgr;
            this.valorisationMgr = valorisationMgr;
            this.ciMgr = ciMgr;
            this.sepService = sepService;
        }

        /// <summary>
        ///   Ajout d'un matériel
        /// </summary>
        /// <param name="materiel">matériel à ajouter</param>
        /// <returns>Renvoie l'identifiant du matériel ajouté</returns>
        public int AddMateriel(MaterielEnt materiel)
        {
            materiel.AuteurCreationId = this.utilisateurMgr.GetContextUtilisateurId();
            materiel.DateCreation = DateTime.UtcNow;
            Repository.AddMateriel(materiel);
            Save();
            return materiel.MaterielId;
        }

        /// <summary>
        ///   Supprime un matériel
        /// </summary>
        /// <param name="materiel">Le matériel à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        public bool DeleteMaterielById(MaterielEnt materiel)
        {
            if (IsDeletable(materiel))
            {
                Repository.Delete(materiel);
                Save();
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un code materiel
        /// </summary>
        /// <param name="materiel">Le code materiel à supprimer</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(MaterielEnt materiel)
        {
            return Repository.IsDeletable(materiel);
        }

        /// <summary>
        ///   Matériel via l'id
        /// </summary>
        /// <param name="id">Id du matériel</param>
        /// <returns>Renvoie un matériel</returns>
        public MaterielEnt GetMaterielById(int id)
        {
            return Repository.GetMaterielById(id);
        }

        /// <summary>
        ///   Matériel via l'id avec la société
        /// </summary>
        /// <param name="id">Id du matérial</param>
        /// <returns>Renvoie un matériel</returns>
        public MaterielEnt GetMaterielByIdWithSociete(int id)
        {
            return Repository.GetMaterielByIdWithSociete(id);
        }

        /// <summary>
        ///   La liste de tous les matériels actifs.
        /// </summary>
        /// <returns>Renvoie la liste des matériels actifs</returns>
        public IEnumerable<MaterielEnt> GetMaterielList()
        {
            return Repository.GetMaterielList() ?? new MaterielEnt[] { };
        }

        /// <summary>
        ///   Retourne la liste de tous les matériels.
        /// </summary>
        /// <returns>List de toutes les matériels</returns>
        public IEnumerable<MaterielEnt> GetMaterielListAll()
        {
            return Repository.GetMaterielListAll() ?? new MaterielEnt[] { };
        }

        /// <summary>
        ///   Permet de connaître l'existence d'un matériel depuis son code.
        /// </summary>
        /// <param name="idCourant">id courant</param>
        /// <param name="codeMateriel">code Materiel</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsMaterielExistsByCode(int idCourant, string codeMateriel, int societeId)
        {
            return Repository.IsMaterielExistsByCode(idCourant, codeMateriel, societeId);
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un materiel
        /// </summary>
        /// <param name="materiel">Matériel à modifier</param>
        public MaterielEnt UpdateMateriel(MaterielEnt materiel)
        {
            int oldRessourceId = Repository.GetRessourceIdByMaterielId(materiel.MaterielId);

            if (materiel.Actif)
            {
                materiel.AuteurModificationId = this.utilisateurMgr.GetContextUtilisateurId();
                materiel.DateModification = DateTime.UtcNow;
            }
            else
            {
                materiel.AuteurSuppressionId = this.utilisateurMgr.GetContextUtilisateurId();
                materiel.DateSuppression = DateTime.UtcNow;
            }

            Repository.UpdateMateriel(materiel);

            MaterielEnt materielMaj = GetMaterielById(materiel.MaterielId);

            UpdateValorisationMateriel(materiel, oldRessourceId);

            return materielMaj;
        }

        private void UpdateValorisationMateriel(MaterielEnt materiel, int oldRessourceId)
        {
            if (materiel.Actif && oldRessourceId != materiel.RessourceId)
            {
                valorisationMgr.NewValorisationJob(materiel.MaterielId, materiel.AuteurModificationId.Value, valorisationMgr.UpdateValorisationFromMateriel);
            }
        }

        public async Task<IEnumerable<MaterielSearchModel>> SearchMaterielsAsync(MaterielSearchParameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter.SocieteId <= 0)
                throw new ArgumentException("Societe id must be greather than 0", nameof(parameter.SocieteId));
            if (parameter.PageIndex < 0)
                throw new ArgumentException("Page index must be a positive number", nameof(parameter.PageIndex));
            if (parameter.PageSize <= 0)
                throw new ArgumentException("Page size must must be greather than 0", nameof(parameter.PageSize));

            return await Repository.SearchMaterielsAsync(parameter).ConfigureAwait(false);
        }

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de matériel.
        /// </summary>
        /// <param name="societeId">Id societe</param>
        /// <returns>Nouvelle instance de matériel initialisée</returns>
        public MaterielEnt GetNewMateriel(int societeId)
        {
            return new MaterielEnt
            {
                //Code = string.Empty,
                //Libelle = string.Empty,
                SocieteId = societeId,
                Actif = true
            };
        }

        /// <inheritdoc />
        public IEnumerable<MaterielEnt> SearchLight(string text, int page, int pageSize, int? societeId, int? ciId, bool materielLocation, bool? includeStorm = null)
        {
            List<int> societeIdList = new List<int>();

            if (ciId.HasValue)
            {
                SocieteEnt societe = ciMgr.GetSocieteByCIId(ciId.Value);

                societeIdList.Add(societe.SocieteId);

                List<SocieteEnt> societeParticipantes = sepService.GetSocieteParticipantes(societe.SocieteId);

                if (societeParticipantes.Count > 0)
                {
                    societeIdList.AddRange(societeParticipantes.Select(x => x.SocieteId));
                }
            }

            if (societeId.HasValue)
            {
                societeIdList.Add(societeId.Value);
            }

            var stormIncluded = includeStorm ?? true;

            var query = Repository
                            .Query()
                            .Include(m => m.Societe)
                            .Filter(x => societeIdList.Contains(x.SocieteId))
                            .Filter(m => m.Actif)
                            .Filter(m => string.IsNullOrEmpty(text) || m.Code.Contains(text) || m.Libelle.Contains(text))
                            .Filter(m => stormIncluded || !m.IsStorm)
                            .Filter(m => m.MaterielLocation == materielLocation);

            if (materielLocation)
            {
                query = query
                            .Include(m => m.CommandeLignes.Select(l => l.Commande))
                            .Include(m => m.CommandeLignes.Select(l => l.Commande.StatutCommande))
                            .Filter(m => m.CommandeLignes.Select(l => l.Commande).Select(c => c.CiId).Contains(ciId))
                            .Filter(m => m.CommandeLignes.Select(l => l.Commande).Select(c => c.StatutCommande.Code).Contains(StatutCommandeEnt.CommandeStatutCL) == false);
            }

            return query.OrderBy(list => list.OrderBy(m => m.Code)).GetPage(page, pageSize);
        }

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return this.Repository.IsAlreadyUsed(id);
        }

        public virtual void InsertOrUpdate(IEnumerable<MaterielEnt> materiels)
        {
            try
            {
                Dictionary<(string code, int societeId), int> existingMateriels = Repository.GetMaterielIdsByCodeAndSocieteId(materiels);

                foreach (var materiel in materiels)
                {
                    if (existingMateriels.TryGetValue((materiel.Code, materiel.SocieteId), out int materielId))
                    {
                        materiel.MaterielId = materielId;
                        Repository.Update(materiel);
                    }
                    else
                    {
                        Repository.Insert(materiel);
                    }
                }

                Save();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les matériels en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <param name="predicate">Filtres de recherche sur tous les matériels</param>
        /// <returns>Retourne la liste filtrée de tous les matériels</returns>
        public IEnumerable<MaterielEnt> GetMateriels(int societeId, Expression<Func<MaterielEnt, bool>> predicate)
        {
            return Repository.GetMateriels(societeId, predicate).ToList();
        }

        /// <summary>
        /// Permet d'ajouter  ou modifier une liste de materiels
        /// </summary>
        /// <param name="expression">An expression specifying the properties that should be used when determining whether an Add or Update operation should be performed.</param>
        /// <param name="materiels">Les Materiels a mettre a jour ou a inserer</param>
        public void InsertOrUpdate(Func<MaterielEnt, object> expression, IEnumerable<MaterielEnt> materiels)
        {
            try
            {
                Repository.InsertOrUpdate(expression, materiels);
                Save();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        public Task<MaterielEnt> GetMaterielDetailByIdAsync(int id)
        {
            return Repository.GetMaterielDetailByIdAsync(id);
        }
    }
}

