using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Commande;
using Fred.Business.Referential.Fournisseur.Common;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.Linq;
using Fred.Framework.Security;

namespace Fred.Business.Referential
{
    /// <summary>
    /// Gestionnaire des fournisseurs
    /// </summary>
    public class FournisseurManager : Manager<FournisseurEnt, IFournisseurRepository>, IFournisseurManager
    {
        private readonly List<string> typeSequenceList = new List<string> { "TIERS", "TIERS2", "GROUPE" };
        private readonly ISecurityManager securityManager;
        private readonly IUtilisateurManager userManager;
        private readonly IStatutCommandeManager statutCommandeManager;
        private readonly IContratInterimaireRepository contratInterimaireRepository;

        public FournisseurManager(IUnitOfWork uow, IFournisseurRepository fournisseurRepository, ISecurityManager securityManager, IUtilisateurManager userManager, IStatutCommandeManager statutCommandeManager, IContratInterimaireRepository contratInterimaireRepository)
          : base(uow, fournisseurRepository)
        {
            this.securityManager = securityManager;
            this.userManager = userManager;
            this.statutCommandeManager = statutCommandeManager;
            this.contratInterimaireRepository = contratInterimaireRepository;
        }

        /// <summary>
        /// Retourne la liste des fournisseurs
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Liste des fournisseurs.</returns>
        public IEnumerable<FournisseurEnt> GetFournisseurList(int groupeId)
        {
            return Repository.GetFournisseurList(groupeId);
        }

        /// <summary>
        /// Retourne la liste des fournisseurs
        /// </summary>
        /// <returns>Liste des fournisseurs.</returns>
        public IEnumerable<FournisseurEnt> GetFournisseurList()
        {
            return Repository.GetFournisseurList();
        }

        /// <inheritdoc/>
        public int? GetAgenceIdByCodeAndGroupe(string agenceCode, int groupeId)
        {
            return Repository.GetAgenceIdByCodeAndGroupe(agenceCode, groupeId);
        }

        /// <summary>
        /// Retourne le fournisseur portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur à retrouver.</param>
        /// <param name="groupeId">Identyifiant du groupe</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        public FournisseurEnt GetFournisseur(int fournisseurId, int? groupeId)
        {
            if (groupeId != null)
            {
                return Repository.GetFournisseur(fournisseurId, (int)groupeId);
            }
            else
            {
                return Repository.GetFournisseur(fournisseurId);
            }
        }

        /// <summary>
        /// Retourne le fournisseur portant le code indiqué.
        /// </summary>
        /// <param name="fournisseurCode">Code du fournisseur dont l'identifiant est à retrouver.</param>
        /// <returns>Fournisseur retrouvé, null sinon</returns>
        public int? GetFournisseurIdByCode(string fournisseurCode)
        {
            return Repository.GetFournisseurIdByCode(fournisseurCode);
        }

        /// <summary>
        /// Ajoute un nouveau fournisseur
        /// </summary>
        /// <param name="fournisseurEnt">Fournisseur à ajouter</param>
        /// <returns>L'identifiant du fournisseur ajouté</returns>
        public FournisseurEnt AddFournisseur(FournisseurEnt fournisseurEnt)
        {
            Repository.AddFournisseur(fournisseurEnt);
            Save();

            return fournisseurEnt;
        }

        /// <summary>
        /// Sauvegarde les modifications d'un fournisseur
        /// </summary>
        /// <param name="fournisseurEnt">Fournisseur à modifier</param>
        /// <returns>Fournisseur mis à jour</returns>
        public FournisseurEnt UpdateFournisseur(FournisseurEnt fournisseurEnt)
        {
            Repository.UpdateFournisseur(fournisseurEnt);
            Save();

            return fournisseurEnt;
        }

        /// <summary>
        /// Supprime un fournisseur
        /// </summary>
        /// <param name="id">L'identifiant du fournisseur à supprimer</param>
        public void DeleteFournisseurById(int id)
        {
            Repository.DeleteFournisseurById(id);
            Save();
        }

        /// <summary>
        /// Chercher une liste de fournisseurs
        /// </summary>
        /// <param name="text"> Le texte a chercher dans les propriétés des fournisseurs </param>
        /// <param name="groupeId"> Identifiant du groupe </param>
        /// <returns> Une liste de fournisseurs </returns>
        public IEnumerable<FournisseurEnt> SearchFournisseurs(string text, int groupeId)
        {
            if (string.IsNullOrEmpty(text))
            {
                return GetFournisseurList(groupeId);
            }

            return Repository.SearchFournisseurs(text, groupeId);
        }

        /// <summary>
        /// Vérifie la validité et enregistre les fournisseurs importés depuis ANAËL Finances
        /// </summary>
        /// <param name="fournisseursAnael">Liste des entités dont il faut vérifier la validité</param>
        /// <param name="societeCode">Le code de la société comptable.</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>la liste de fournisseurs ajoute</returns>
        public IEnumerable<FournisseurEnt> ManageImportedFournisseurs(IEnumerable<FournisseurEnt> fournisseursAnael, string societeCode, int? groupeId)
        {
            if (groupeId == null)
            {
                return new List<FournisseurEnt>();
            }

            List<FournisseurEnt> fournisseurs = new List<FournisseurEnt>();
            List<FournisseurEnt> fredFournisseurs = Repository.GetFournisseurLight(groupeId.Value).ToList();

            foreach (FournisseurEnt fournisseurImport in fournisseursAnael.ToList())
            {
                fournisseurImport.GroupeId = groupeId.Value;

                // DMU : A amélioré : societeCode != "1000", si ce n'est pas RB alors on ne check pas la donnée.
                FournisseurEnt fFred = null;
                if (societeCode != "1000")
                {
                    fFred = fredFournisseurs.Find(f => fournisseurImport.Code == f.Code);
                }
                else if (CheckValidityBeforeImportation(fournisseurImport))
                {
                    // Test d'existence du fournisseur (+ récupération de son identifiant si déjà existant)
                    fFred = fredFournisseurs.Find(f => fournisseurImport.Code == f.Code && fournisseurImport.TypeSequence == f.TypeSequence);
                }

                // Si le fournisseur existe déjà
                if (fFred != null)
                {
                    if (!fournisseurImport.Equals(fFred))
                    {
                        fournisseurs.Add(HandleUpdateImportedFournisseur(fFred, fournisseurImport));
                    }
                }
                // Si le fournisseur n'existe pas et qu'il n'a pas de date clôture, on l'ajoute à FRED
                else if (!fournisseurImport.DateCloture.HasValue)
                {
                    fournisseurs.Add(fournisseurImport);
                }
            }

            if (fournisseurs.Count > 0)
            {
                Repository.AddOrUpdateFournisseurList(fournisseurs);
            }

            return fournisseurs;
        }

        private FournisseurEnt HandleUpdateImportedFournisseur(FournisseurEnt fFred, FournisseurEnt fAnael)
        {
            fFred.Libelle = fAnael.Libelle;
            fFred.Adresse = fAnael.Adresse;
            fFred.CodePostal = fAnael.CodePostal;
            fFred.Ville = fAnael.Ville;
            fFred.SIRET = fAnael.SIRET;
            fFred.SIREN = fAnael.SIREN;
            fFred.Telephone = fAnael.Telephone;
            fFred.Fax = fAnael.Fax;
            fFred.Email = fAnael.Email;
            fFred.TypeTiers = fAnael.TypeTiers; // I : Interim ou L : Locatier      
            fFred.ModeReglement = fAnael.ModeReglement;
            fFred.DateOuverture = fAnael.DateOuverture;
            fFred.DateCloture = fAnael.DateCloture;
            fFred.RegleGestion = fAnael.RegleGestion;
            fFred.PaysId = fAnael.PaysId;
            fFred.IsProfessionLiberale = fAnael.IsProfessionLiberale;
            fFred.CodeTVA = fAnael.CodeTVA;
            return fFred;
        }

        /// <summary>
        /// Vérifie la validité d'un fournisseur importé depuis ANAËL Finances
        /// </summary>
        /// <param name="fournisseurEnt">Entité dont il faut vérifier la validité</param>    
        /// <returns>Vrai si le fournisseur est valide, faux sinon</returns>
        public bool CheckValidityBeforeImportation(FournisseurEnt fournisseurEnt)
        {
            if (string.IsNullOrEmpty(fournisseurEnt.TypeSequence))
            {
                return false;
            }

            if (!typeSequenceList.Contains(fournisseurEnt.TypeSequence.ToUpper()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        public override void CheckAccessToEntity(FournisseurEnt entity)
        {
            base.CheckAccessToEntity(entity);
            int userId = securityManager.GetUtilisateurId();
            Repository.CheckAccessToEntity(entity, userId);
        }

        /// <summary>
        /// Moteur de recherche des fournisseurs pour picklist
        /// </summary>
        /// <param name="recherche">Texte de recherche sur le libellé du fournisseur</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="groupeId">L'identifiant du groupe</param>
        /// <param name="recherche2">Autres information de Recherche (Adresse, Code, SIREN)</param>
        /// <param name="ciId">Ci selectionner</param>
        /// <param name="withCommandValide">Fournisseur avec Commande Valide</param>
        /// <returns>Retourne une liste d'items de référentiel</returns>
        public IEnumerable<FournisseurEnt> SearchLight(string recherche, int page, int pageSize, int? groupeId, string recherche2, int? ciId, bool? withCommandValide = false)
        {
            UtilisateurEnt user = userManager.GetContextUtilisateur();
            if (!groupeId.HasValue)
            {
                groupeId = user?.Personnel?.Societe?.GroupeId;
            }

            IRepositoryQuery<FournisseurEnt> repoQuery = Repository.Query()
                             .Include(x => x.Pays)
                             .Include(x => x.Agences.Select(y => y.Adresse))
                             .Filter(f => (DateTime.Today.Date >= f.DateOuverture.Value.Date || !f.DateOuverture.HasValue)
                                          && (DateTime.Today.Date <= f.DateCloture.Value.Date || !f.DateCloture.HasValue))
                             .Filter(f => groupeId.HasValue && f.GroupeId == groupeId.Value);
            if (withCommandValide.HasValue && withCommandValide == true)
            {
                AddFilterWithCommande(repoQuery, user?.UtilisateurId, ciId);
            }
            IEnumerable<FournisseurEnt> query = FournisseurManagerHelper.GetFiltered(repoQuery, recherche, recherche2);
            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// GetActifRentersForMateriel
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="groupeId">L'identifiant du groupe</param>
        /// <returns>Retourne une liste d'items de référentiel</returns>
        /// <returns>l'ensemble des fournisseurs actifs (= la date du jour doit être comprise entre le date d'ouverture et la date de clôture du Fournisseur), de type Locatier (TypeTiers=L) et associé au groupe de la société de rattachement du Matériel.</returns>
        public IEnumerable<FournisseurEnt> GetActiveRentersForMateriel(string text, int page, int pageSize, int groupeId)
        {
            return Repository.Query()
                                  .Include(x => x.Pays)
                                  .Filter(f => f.TypeTiers == "L")
                                  .Filter(f => f.GroupeId.Equals(groupeId))
#pragma warning disable RCS1155 // Use StringComparison when comparing strings.
                            .Filter(f => string.IsNullOrEmpty(text) || f.Code.ToLower().Contains(text.ToLower()) || f.Libelle.ToLower().Contains(text.ToLower()))
#pragma warning restore RCS1155 // Use StringComparison when comparing strings.
                            .Filter(f => (f.DateOuverture <= DateTime.UtcNow && f.DateCloture == null) || (f.DateOuverture <= DateTime.UtcNow && DateTime.UtcNow < f.DateCloture))
                                  .OrderBy(o => o.OrderBy(f => f.Code).ThenBy(f => f.TypeSequence))
                                  .GetPage(page, pageSize)
                                  .ToList();
        }

        /// <summary>
        /// Récupère la liste du personnel intérimaire lié au fournisseur
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur</param>
        /// <returns>Liste du personnel intérimaire</returns>
        public IEnumerable<PersonnelEnt> GetPersonnelInterimaireList(int fournisseurId)
        {
            return contratInterimaireRepository
                    .Query()
                    .Include(p => p.Interimaire)
                    .Filter(f => f.FournisseurId.Equals(fournisseurId))
                    .Filter(p => p.Interimaire.DateSuppression == null)
                    .Get()
                    .Select(p => p.Interimaire);
        }

        /// <summary>
        /// Récupère le nombre de personnel intérimaire lié au fournisseur
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur</param>
        /// <returns>Nombre de personnels intérimaire</returns>
        public int GetCountPersonnelList(int fournisseurId)
        {
            return GetPersonnelInterimaireList(fournisseurId).Count();
        }

        /// <summary>
        /// Permet de récupérer la liste des fournisseurs en fonction des critères de recherche.
        /// </summary>
        /// <param name="filters">Filtres de recherche</param>
        /// <param name="page">Page actuelle</param>
        /// <param name="pageSize">Taille d'une page</param>
        /// <returns>Retourne la liste filtré des fournisseurs</returns>
        public IEnumerable<FournisseurEnt> SearchFournisseurWithFilters(SearchFournisseurEnt filters, int page, int pageSize)
        {
            var currentUser = userManager.GetContextUtilisateur();

            return Repository
                    .Query()
                    .Include(x => x.Pays)
                    .Filter(f => f.GroupeId.Equals(currentUser.Personnel.Societe.GroupeId))
                    .Filter(filters.GetPredicateWhere())
                    .OrderBy(filters.ApplyOrderBy)
                    .GetPage(page, pageSize);
        }

        /// <summary>
        /// Récupère un nouveau filtre (SearchFournisseurEnt)
        /// </summary>
        /// <returns>Filtre fournisseur</returns>
        public SearchFournisseurEnt GetFilter()
        {
            return new SearchFournisseurEnt();
        }

        /// <summary>
        /// Récupère un nouveau filtre (SearchFournisseurEnt)
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Filtre fournisseur</returns>
        public List<FournisseurEnt> GetFournisseurETT(int groupeId)
        {
            try
            {
                return Repository.GetFournisseurETT(groupeId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        private void AddFilterWithCommande(IRepositoryQuery<FournisseurEnt> repoQuery, int? userid, int? ciId)
        {
            StatutCommandeEnt statut = statutCommandeManager.GetByCode(StatutCommandeEnt.CommandeStatutVA);
            repoQuery.Filter(x => x.Commandes.Any(y => y.StatutCommandeId == statut.StatutCommandeId));
            if (ciId.HasValue)
            {
                repoQuery.Filter(x => x.Commandes.Any(y => y.CiId == ciId));
            }
            else
            {
                List<int> listcis = userManager.GetAllCIbyUser(userid.Value).ToList();
                repoQuery.Filter(x => x.Commandes.Any(y => listcis.Contains(y.CiId.Value)));
            }
        }

        /// <summary>
        /// Return A List Of FournisseurIdCodeModel By List code 
        /// </summary>
        /// <param name="listOfCode">List Of Code</param>
        /// <returns>List Of FournisseurIdCodeModel</returns>
        public List<FournisseurIdCodeModel> GetAllIdFournisseurForListOfCode(List<string> listOfCode)
        {
            List<FournisseurIdCodeModel> listOfCodeIdFournisseurModel = new List<FournisseurIdCodeModel>();
            IEnumerable<FournisseurEnt> listOfCodeIdFournisseur = Repository.GetAllIdFournisseurForListOfCode(listOfCode);
            listOfCodeIdFournisseur.ForEach(x => listOfCodeIdFournisseurModel.Add(new FournisseurIdCodeModel() { CodeFournisseur = x.Code, IdFournisseur = x.FournisseurId }));
            return listOfCodeIdFournisseurModel;
        }

        /// <summary>
        /// Retourne le fournisseur par fournisseur SIREN et code groupe.
        /// </summary>
        /// <param name="fournisseurSIREN">SIREN Fournisseur.</param>
        /// <param name="groupeCode">Code groupe.</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        public List<FournisseurEnt> GetBySirenAndGroupeCode(string fournisseurSIREN, string groupeCode)
        {
            return Repository.GetBySirenAndGroupeCode(fournisseurSIREN, groupeCode);
        }

        /// <summary>
        /// Retourne le fournisseur par reference systeme interimaire SIREN et code groupe.
        /// </summary>
        /// <param name="fournisseurSIREN">SIREN Fournisseur.</param>
        /// <param name="groupeCode">Code groupe.</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        public List<FournisseurEnt> GetByReferenceSystemInterimaireAndGroupeCode(string fournisseurSIREN, string groupeCode)
        {
            return Repository.GetByReferenceSystemInterimaireAndGroupeCode(fournisseurSIREN, groupeCode);
        }
    }
}
