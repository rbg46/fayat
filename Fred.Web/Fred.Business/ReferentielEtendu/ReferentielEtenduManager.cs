using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.Extensions;

namespace Fred.Business.ReferentielEtendu
{
    /// <summary>
    /// Gestionnaire du référentiel étendu
    /// </summary>
    public class ReferentielEtenduManager : Manager<ReferentielEtenduEnt, IReferentielEtenduRepository>, IReferentielEtenduManager
    {
        private readonly IUtilisateurManager usrMgr;
        private readonly IOrganisationManager orgaMgr;
        private readonly ISocieteManager societeMgr;
        private readonly IParametrageReferentielEtenduRepository parametrageReferentielEtenduRepository;
        private readonly IParametrageReferentielEtenduValidator parametrageReferentielEtenduValidator;

        public ReferentielEtenduManager(
            IUnitOfWork uow,
            IReferentielEtenduRepository referentielEtenduRepository,
            IUtilisateurManager utilisateurMgr,
            IOrganisationManager organisationMgr,
            ISocieteManager societeMgr,
            IParametrageReferentielEtenduRepository parametrageReferentielEtenduRepository,
            IParametrageReferentielEtenduValidator parametrageReferentielEtenduValidator)
          : base(uow, referentielEtenduRepository)
        {
            this.usrMgr = utilisateurMgr;
            this.orgaMgr = organisationMgr;
            this.societeMgr = societeMgr;
            this.parametrageReferentielEtenduRepository = parametrageReferentielEtenduRepository;
            this.parametrageReferentielEtenduValidator = parametrageReferentielEtenduValidator;
        }

        /// <summary>
        /// Initialise le référentiel étendu pour une société et retourne le référentiel étendu
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <param name="listReferentielEtendu">Liste de referentiel etendu</param>
        /// <returns>Liste des referentielEtendus.</returns>
        private IEnumerable<ReferentielEtenduEnt> DifferentialReferentielFixeReferentielEtendu(int societeId, List<ReferentielEtenduEnt> listReferentielEtendu)
        {
            return Repository.DifferentialReferentielFixeReferentielEtendu(societeId, listReferentielEtendu);
        }

        public IReadOnlyList<ReferentielEtenduEnt> Get(List<int> ressourceIds, int societeId)
        {
            return Repository.Get(ressourceIds, societeId);
        }


        #region Referentiel Etendu

        /// <summary>
        /// Retourne le referentielEtendu avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="referentielEtenduId">Identifiant du referentielEtendu à retrouver.</param>
        /// <returns>Le referentielEtendu retrouvé, sinon null.</returns>
        public ReferentielEtenduEnt GetById(int referentielEtenduId)
        {
            return Repository.GetById(referentielEtenduId);
        }

        /// <summary>
        /// Retourne la liste des referentielEtendus en fonction d'une société.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe.</param>
        /// <returns>Liste des referentielEtendus qui ont une nature.</returns>
        public IEnumerable<ReferentielEtenduEnt> GetList(int societeId)
        {
            return Repository.GetList(societeId);
        }

        /// <summary>
        /// Retourne la liste des referentielEtendus pour une societe spécifique.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Liste des referentielEtendus.</returns>
        public IEnumerable<ReferentielEtenduEnt> GetListBySocieteId(int societeId)
        {
            var list = Repository.GetListBySocieteId(societeId).ToList();
            return DifferentialReferentielFixeReferentielEtendu(societeId, list);
        }

        /// <summary>
        /// Retourne la liste des referentielEtendus pour une societe spécifique.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Liste des referentielEtendus.</returns>
        public IEnumerable<ChapitreEnt> GetAllReferentielEtenduAsChapitreList(int societeId)
        {
            var list = Repository.GetListBySocieteId(societeId).ToList();
            return GetChapitreListFromReferentielEtenduList(list, societeId);
        }

        /// <summary>
        /// Gets all referentiel etendu recommande as chapitre list.
        /// </summary>
        /// <param name="societeId">The societe identifier.</param>
        /// <returns>IEnumerable of ChapitreEnt</returns>
        public IEnumerable<ChapitreEnt> GetAllReferentielEtenduRecommandeAsChapitreList(int societeId)
        {
            var list = Repository.GetListRecommandeBySocieteId(societeId).ToList();

            var listeChapitre = list.GroupBy(c => c.Ressource.SousChapitre)
                       .GroupBy(s => s.Key.Chapitre)
                       .Select(c => c.Key)
                       .ToList();
            return listeChapitre;
        }

        /// <summary>
        /// Retourne la liste des Référentiels Etendus pour une société donnée.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="onlyActive">Indique si seules les ressources actives doivent être retrournées</param>
        /// <returns>Liste des referentielEtendus.</returns>
        public IEnumerable<ChapitreEnt> GetReferentielEtenduAsChapitreList(int societeId, bool onlyActive = true)
        {
            return Repository.GetReferentielEtenduAsChapitreList(societeId, onlyActive);
        }

        /// <summary>
        /// Retourne la liste des Référentiels Etendus pour une société donnée.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Liste des referentielEtendus.</returns>
        public List<ChapitreEnt> GetReferentielEtenduAsChapitreListLightBySocieteId(int societeId)
        {
            return Repository.GetReferentielEtenduAsChapitreListLightBySocieteId(societeId);
        }

        /// <summary>
        /// Retourne la liste des Référentiels Etendus pour une liste de ressources.
        /// </summary>
        /// <param name="ressourceIdList">Liste de ressource id</param>
        /// <returns>Liste des referentielEtendus.</returns>
        public List<ChapitreEnt> GetReferentielEtenduAsChapitreListLightByRessourceIdList(List<int> ressourceIdList)
        {
            return Repository.GetReferentielEtenduAsChapitreListLightByRessourceIdList(ressourceIdList);
        }

        /// <summary>
        /// Retourne la liste des Référentiels Etendus pour une société donnée sous la forme d'une liste de chapitres
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="codeTypeRessource">Code type de ressource</param>
        /// <returns>Liste des referentielEtendus.</returns>
        public IEnumerable<ChapitreEnt> GetReferentielEtenduAsChapitreList(int societeId, string codeTypeRessource)
        {
            return Repository.GetReferentielEtenduAsChapitreList(societeId, codeTypeRessource);
        }

        /// <summary>
        /// Retourne la liste des chapitres/sous-chapîtres listant les ressources ainsi que le référentiel étendu et le param associé pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="filter">filtre sur le libellé de la ressource</param>
        /// <returns>Liste des chapitres</returns>
        public IEnumerable<ChapitreEnt> GetChapitresFromReferentielEtendus(int societeId, string filter = "")
        {
            return Repository.GetChapitresFromReferentielEtendus(societeId, filter);
        }

        /// <summary>
        /// Recupere La liste des natures pour une societe et un code chapitre
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="chapitreCode">chapitreCode</param>
        /// <returns>Liste de nature</returns>
        public IEnumerable<NatureEnt> GetNaturesByChapitreCodeAndSocieteId(int societeId, string chapitreCode)
        {
            return Repository.GetNaturesByChapitreCodeAndSocieteId(societeId, chapitreCode);
        }

        /// <summary>
        /// Recupere La liste des natures pour une societe 
        /// </summary>
        /// <param name="societeId">societeId</param>   
        /// <returns>Liste de nature</returns>
        public IEnumerable<ChapitreEnt> GetAllNatures(int societeId)
        {
            return Repository.GetAllChapitresWithNatures(societeId);
        }

        /// <inheritdoc />
        public RessourceEnt GetRessourceWithRefEtenduAndParams(int ressourceId, int societeId)
        {
            return Repository.GetRessourceWithRefEtenduAndParams(ressourceId, societeId);
        }

        /// <summary>
        /// Permet de créer un détail de ressource.
        /// </summary>
        /// <param name="code">Code de la ressource.</param>
        /// <param name="groupeCode">Le code du groupe.</param>
        /// <returns>La ressource créée.</returns>
        public List<RessourceEnt> GetRessourceByCodeAndGroupeCode(string code, string groupeCode)
        {
            return Repository.GetRessourceByCodeAndGroupeCode(code, groupeCode);
        }


        /// <inheritdoc />
        public ReferentielEtenduEnt GetByRessourceIdAndSocieteId(int ressourceId, int societeId)
        {
            return Repository.GetByRessourceIdAndSocieteId(ressourceId, societeId);
        }

        /// <summary>
        /// Ajoute ou met à jour un nouveau referentielEtendu
        /// </summary>
        /// <param name="referentielEtendu">Rôle à ajouter</param>
        /// <returns>Référentiel Etendu mis à jour</returns>
        public ReferentielEtenduEnt Update(ReferentielEtenduEnt referentielEtendu)
        {
            Repository.UpdateReferentielEtendu(referentielEtendu);
            Save();

            var refEtendu = Repository.GetById(referentielEtendu.ReferentielEtenduId, true);
            refEtendu.AffectListeUnitesAbregees();
            return refEtendu;
        }

        /// <summary>
        /// Ajout / Mise à jour / Suppression d'un référentiel étendu
        /// </summary>
        /// <param name="refEtenduList">Référentiel Etendu</param>
        /// <returns>Liste des référentiels étendu</returns>
        public IEnumerable<ReferentielEtenduEnt> ManageReferentielEtenduList(IEnumerable<ReferentielEtenduEnt> refEtenduList)
        {
            List<ReferentielEtenduEnt> result = new List<ReferentielEtenduEnt>();

            if (refEtenduList.Any())
            {
                foreach (ReferentielEtenduEnt refEtendu in refEtenduList.ToList())
                {
                    if (refEtendu.NatureId == null)
                    {
                        DeleteById(refEtendu.ReferentielEtenduId);
                    }
                    else
                    {
                        if (refEtendu.ReferentielEtenduId.Equals(0))
                        {
                            result.Add(Repository.AddReferentielEtendu(refEtendu));
                            Save();
                        }
                        else
                        {
                            Repository.UpdateReferentielEtendu(refEtendu);
                            Save();

                            ReferentielEtenduEnt referentielEtendu = Repository.GetById(refEtendu.ReferentielEtenduId, true);
                            referentielEtendu.AffectListeUnitesAbregees();
                            result.Add(referentielEtendu);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Supprime un referentielEtendu
        /// </summary>
        /// <param name="referentielEtenduId">ID du referentielEtendu à supprimé</param>
        public void DeleteById(int referentielEtenduId)
        {
            Repository.DeleteReferentielEtendu(referentielEtenduId);
            Save();
        }

        /// <summary>
        /// Gets the chapitre list from referentiel etendu list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="societeId">The societe identifier.</param>
        /// <returns>IEnumerable of ChapitreEnt</returns>
        private IEnumerable<ChapitreEnt> GetChapitreListFromReferentielEtenduList(List<ReferentielEtenduEnt> list, int societeId)
        {
            var diff = DifferentialReferentielFixeReferentielEtendu(societeId, list);

            diff.ForEach(r => r.AffectListeUnitesAbregees());

            var listeChapitre = diff.GroupBy(c => c.Ressource.SousChapitre)
                       .GroupBy(s => s.Key.Chapitre)
                       .Select(c => c.Key)
                       .ToList();
            return listeChapitre;
        }

        /// <inheritdoc />
        public IEnumerable<ChapitreEnt> GetAllCIRessourceListAsChapitreList(int ciId, int societeId)
        {
            List<ChapitreEnt> chapitres = GetReferentielEtenduAsChapitreList(societeId, Constantes.TypeRessource.CodeTypeMateriel).ToList();
            // Tri sur les CIRessource du CI sélectionné
            chapitres.ForEach(c => c.SousChapitres.ToList().ForEach(ss => ss.Ressources.ToList().ForEach(r =>
            {
                if (r.CIRessources.Count > 0)
                {
                    r.CIRessources = r.CIRessources.Where(ci => ci.CiId.Equals(ciId)).ToList();
                }
                else
                {
                    r.CIRessources.Add(new CIRessourceEnt { CiId = ciId, RessourceId = r.RessourceId });
                }
            }
            )));
            return chapitres;
        }
        #endregion

        #region Paramétrage Tarif Référentiel Etendu

        /// <summary>
        /// Retourne la liste des paramétrage des référentiels etendus pour un eorganisation et une devise
        /// </summary>
        /// <param name="organisationId">ID du de l'organisation</param>
        /// <param name="deviseId">ID du de la devise</param>
        /// <param name="filter">filtre des ressources</param>
        /// <returns>liste des paramétrage des référentiels etendus de la société </returns>
        public Tuple<IEnumerable<OrganisationEnt>, IEnumerable<ChapitreEnt>> GetParametrageReferentielEtendu(int organisationId, int deviseId, string filter = "")
        {
            var orgaList = GetOrganisationParentList(organisationId);

            var listParamRefEtendus = GetParametrageReferentielEtenduMergedFlatList(organisationId, deviseId, filter);

            //On groupe les ParamRefEtendus par ressource
            var refEtendus = listParamRefEtendus.GroupBy(x => x.ReferentielEtendu).Select(x => x.Key).OrderBy(x => x.Ressource.Code).ToList();

            //On ordonne pour chaque RefEtendus la liste des ParamRefEtendu pour qu'ils s'affichent dans l'ordre du tableau. Ordonné solon le type d'organisation
            refEtendus.ForEach(r => r.ParametrageReferentielEtendus = r.ParametrageReferentielEtendus.OrderBy(p => p.Organisation.TypeOrganisationId).ToList());

            var chapitres = refEtendus
                .GroupBy(x => x.Ressource.SousChapitre)
                .GroupBy(x => x.Key.Chapitre)
                .Select(x => x.Key)
                .OrderBy(x => x.Code)
                .ToList();

            // Trie les sous-chapitres
            foreach (var chapitre in chapitres)
            {
                chapitre.SousChapitres = chapitre.SousChapitres.OrderBy(sc => sc.Code).ToList();
            }

            return new Tuple<IEnumerable<OrganisationEnt>, IEnumerable<ChapitreEnt>>(orgaList, chapitres);
        }

        /// <summary>
        /// Retourne la liste des paramétrage des référentiels etendus pour un eorganisation et une devise
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <param name="deviseId">ID du de la devise</param>
        /// <param name="filter">filtre des ressources</param>
        /// <returns>liste des paramétrage des référentiels etendus</returns>
        public IEnumerable<ParametrageReferentielEtenduEnt> GetParametrageReferentielEtenduMergedFlatList(int organisationId, int deviseId, string filter = "")
        {
            var orgaList = GetOrganisationParentList(organisationId);

            var societe = societeMgr.GetSocieteParentByOrgaId(organisationId);
            //On récupère ici la liste des ParamRefEtendus existants pour une devise (peu importe l'organisation)
            var list = Repository.GetParametrageReferentielEtendu(societe.SocieteId, deviseId, filter).ToList();

            //On copie dans une nouvelle liste les paramRefEntendus concernés par les organisations parents.
            var newList = new List<ParametrageReferentielEtenduEnt>();
            foreach (var orga in orgaList)
            {
                newList.AddRange(list.Where(o => o.OrganisationId == orga.OrganisationId).ToList());
            }

            //Par la méthode suivante on va merger l'intégralité des paramétrages Référentiels étendus (existants et non existants)
            return DifferentialParametrageReferentielFixeReferentielEtendu(orgaList, deviseId, newList, filter);
        }

        /// <summary>
        /// Permet de récupérer la liste des organisations parents
        /// </summary>
        /// <param name="organisationId">Id de l'organisation courante</param>
        /// <returns>Liste d'organisations</returns>
        public List<OrganisationEnt> GetOrganisationParentList(int organisationId)
        {
            //On récupère la liste des organisations Parent (organisation courante comprise) triée
            return orgaMgr.GetOrganisationParentByOrganisationId(organisationId, OrganisationType.Societe.ToIntValue()).OrderBy(o => o.TypeOrganisationId).ToList();
        }

        /// <summary>
        /// Retourne la liste des paramétrage des référentiels etendus pour un organisation et une devise et un référentiel
        /// étendu
        /// </summary>
        /// <param name="organisationId">ID du de l'organisation</param>
        /// <param name="deviseId">ID du de la devise</param>
        /// <param name="referentielId">ID du de la référentiel étendu</param>
        /// <returns>Un paramétrage de référentiel etendu de la société </returns>
        public ParametrageReferentielEtenduEnt GetParametrageReferentielEtendu(int organisationId, int deviseId, int referentielId)
        {
            var param = Repository.GetParametrageReferentielEtendu(organisationId, deviseId, referentielId);
            Repository.InitParametrageParentList(param);
            return param;
        }

        /// <summary>
        /// Ajoute ou met à jour un nouveau referentiel Etendu
        /// </summary>
        /// <param name="parametrageReferentielEtendu"> Paramétrage à ajouter ou mettre à jour </param>
        /// <returns> Identifiant du referentielEtendu ajouté ou mis à jour </returns>
        public ParametrageReferentielEtenduEnt AddOrUpdateParametrageReferentielEtendu(ParametrageReferentielEtenduEnt parametrageReferentielEtendu)
        {
            //Clean
            parametrageReferentielEtendu.Organisation = null;

            var auteurId = usrMgr.GetContextUtilisateurId();

            if (parametrageReferentielEtendu.ParametrageReferentielEtenduId == 0)
            {
                parametrageReferentielEtendu.AuteurCreationId = auteurId;
                parametrageReferentielEtendu.DateCreation = DateTime.Now;
                parametrageReferentielEtenduRepository.Insert(parametrageReferentielEtendu);
            }
            else
            {
                parametrageReferentielEtendu.AuteurModificationId = auteurId;
                parametrageReferentielEtendu.DateModification = DateTime.Now;
                parametrageReferentielEtenduRepository.Update(parametrageReferentielEtendu);
            }

            BusinessValidation(parametrageReferentielEtendu, parametrageReferentielEtenduValidator);

            Save();

            return parametrageReferentielEtendu;
        }

        /// <summary>
        /// Calcul le différentiel entre le référentiel étendu et son référentiel fixe, et l'ajoute au référentiel étendu
        /// </summary>
        /// <param name="orgaList">Liste des organisations parents</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <param name="listParam">liste de paramétrage référentiel étendu</param>
        /// <param name="filter">filtrage des ressources</param>
        /// <returns>retourne la liste des référentiels étendus</returns>
        public IEnumerable<ParametrageReferentielEtenduEnt> DifferentialParametrageReferentielFixeReferentielEtendu(List<OrganisationEnt> orgaList, int deviseId, List<ParametrageReferentielEtenduEnt> listParam, string filter = "")
        {
            return Repository.DifferentialParametrageReferentielFixeReferentielEtendu(orgaList, deviseId, listParam, filter);
        }

        /// <summary>
        /// Récupération du ReferentielEtendu à partir de la ressource et de la societe
        /// </summary>
        /// <param name="idRessource">Identifiant unique de la ressource</param>
        /// <param name="idSociete">Identifiant unique de la societe</param>
        /// <param name="withInclude">Si vrai on charge la ressource et le sous-chapitre dont elle dépend</param>
        /// <returns>Le referentiel etendu</returns>
        public ReferentielEtenduEnt GetReferentielEtenduByRessourceAndSociete(int idRessource, int idSociete, bool withInclude = false)
        {
            return Repository.GetReferentielEtenduByRessourceAndSociete(idRessource, idSociete, withInclude);
        }

        /// <summary>
        /// Création d'une liste d'entité pour l'export excel. 
        /// </summary>
        /// <param name="orgaId">Identifiant unique de l'organisation</param>
        /// <param name="deviseId">Identifiant unique de la devise</param>
        /// <param name="filter">filtre</param>
        /// <returns>une liste de  ParametrageReferentielEtenduExportEnt</returns>
        public List<ParametrageReferentielEtenduExportEnt> GenerateListForExportExcel(int orgaId, int deviseId, string filter)
        {
            var orgaList = GetOrganisationParentList(orgaId);

            var parametrageReferentielEtenduMergedFlatList = GetParametrageReferentielEtenduMergedFlatList(orgaId, deviseId, filter).ToList();

            //Mappage des données
            var parametrageReferentielEtenduExportList = from p in parametrageReferentielEtenduMergedFlatList
                                                         group p by p.ReferentielEtendu.RessourceId into g
                                                         let parameter = g.FirstOrDefault()
                                                         let unite = CalulateUnite(orgaList, g.ToList())
                                                         select new ParametrageReferentielEtenduExportEnt()
                                                         {
                                                             CodeChapitre = GetParamInfo(parameter, (param) => param.ReferentielEtendu.Ressource.SousChapitre.Chapitre.Code),
                                                             Chapitre = GetParamInfo(parameter, (param) => param.ReferentielEtendu.Ressource.SousChapitre.Chapitre.Libelle),
                                                             CodeSousChapitre = GetParamInfo(parameter, (param) => param.ReferentielEtendu.Ressource.SousChapitre.Code),
                                                             SousChapitre = GetParamInfo(parameter, (param) => param.ReferentielEtendu.Ressource.SousChapitre.Libelle),
                                                             CodeRessource = GetParamInfo(parameter, (param) => param.ReferentielEtendu.Ressource.Code),
                                                             Ressource = GetParamInfo(parameter, (param) => param.ReferentielEtendu.Ressource.Libelle),
                                                             Unite = unite != null ? unite.Libelle : string.Empty,
                                                             MontantSociete = CalulateMontant(g.ToList(), Entities.Constantes.OrganisationType.CodeSociete),
                                                             MontantPUO = CalulateMontant(g.ToList(), Entities.Constantes.OrganisationType.CodePuo),
                                                             MontantUO = CalulateMontant(g.ToList(), Entities.Constantes.OrganisationType.CodeUo),
                                                             MontantEtablissement = CalulateMontant(g.ToList(), Entities.Constantes.OrganisationType.CodeEtablissement),
                                                             Synthese = CalulateSynthese(orgaList, g.ToList())
                                                         };

            // trie du resultat
            var results = parametrageReferentielEtenduExportList.OrderBy(p => p.CodeChapitre).ThenBy(p => p.CodeSousChapitre).ToList();

            return results;
        }

        /// <summary>
        /// Recupere la valeur de la propriété d'un ParametrageReferentielEtenduEnt
        /// </summary>
        /// <param name="parameter">ParametrageReferentielEtenduEnt</param>
        /// <param name="propertyExtractor">fonction de recuperation de la propriété</param>
        /// <returns>La valeur de la proriete</returns>
        private string GetParamInfo(ParametrageReferentielEtenduEnt parameter, Func<ParametrageReferentielEtenduEnt, string> propertyExtractor)
        {
            var result = string.Empty;
            if (parameter != null)
            {
                return propertyExtractor(parameter);
            }
            return result;
        }

        /// <summary>
        /// Recupere le montant d'un ParametrageReferentielEtenduEnt correspondant a un type d'organistaion
        /// </summary>
        /// <param name="parameters">(iste de ParametrageReferentielEtenduEnt></param>
        /// <param name="typeOrganisation">typeOrganisation</param>
        /// <returns>montant du ParametrageReferentielEtenduEnt</returns>
        private decimal? CalulateMontant(IEnumerable<ParametrageReferentielEtenduEnt> parameters, string typeOrganisation)
        {
            decimal? result = null;
            var parametersForOrganisationOfType = parameters.FirstOrDefault(pre => pre.Organisation.TypeOrganisation.Code == typeOrganisation);
            if (parametersForOrganisationOfType != null)
            {
                return parametersForOrganisationOfType.Montant;
            }
            return result;
        }

        private decimal? CalulateSynthese(IEnumerable<OrganisationEnt> organisationList, IEnumerable<ParametrageReferentielEtenduEnt> parameters)
        {
            var currentOrganisation = organisationList.FirstOrDefault(o => o.TypeOrganisation.Code == Entities.Constantes.OrganisationType.CodeSociete);

            decimal? lastValueFound = null;

            while (currentOrganisation != null)
            {
                var parameterWithCurrentOrganisation = parameters.FirstOrDefault(pre => pre.Organisation.OrganisationId == currentOrganisation.OrganisationId);

                if (parameterWithCurrentOrganisation != null && parameterWithCurrentOrganisation.Montant != null)
                {
                    lastValueFound = parameterWithCurrentOrganisation.Montant;
                }

                currentOrganisation = organisationList.FirstOrDefault(o => o.PereId == currentOrganisation.OrganisationId);
            }
            return lastValueFound;
        }

        private UniteEnt CalulateUnite(IEnumerable<OrganisationEnt> organisationList, IEnumerable<ParametrageReferentielEtenduEnt> parameters)
        {
            var currentOrganisation = organisationList.FirstOrDefault(o => o.TypeOrganisation.Code == Entities.Constantes.OrganisationType.CodeSociete);

            UniteEnt lastValueFound = null;

            while (currentOrganisation != null)
            {
                var parameterWithCurrentOrganisation = parameters.FirstOrDefault(pre => pre.Organisation.OrganisationId == currentOrganisation.OrganisationId);

                if (parameterWithCurrentOrganisation != null && parameterWithCurrentOrganisation.Unite != null)
                {
                    lastValueFound = parameterWithCurrentOrganisation.Unite;
                }

                currentOrganisation = organisationList.FirstOrDefault(o => o.PereId == currentOrganisation.OrganisationId);
            }
            return lastValueFound;
        }
        #endregion
    }
}
