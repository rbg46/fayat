using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Personnel;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.PointagePersonnel;

namespace Fred.Business.Rapport
{
    public class RapportManager : Manager<RapportEnt, IRapportRepository>, IRapportManager
    {
        private readonly IRapportLigneCodeAstreinteRepository rapportLigneCodeAstreinteRepository;
        private readonly IPersonnelManager personnelManager;

        private ISearchFeature Search { get; }
        private ICrudFeature Crud { get; }

        public RapportManager(
            IUnitOfWork uow,
            IRapportRepository rapportRepository,
            IRapportValidator validator,
            ISearchFeature searchFeature,
            ICrudFeature crudFeature,
            IRapportLigneCodeAstreinteRepository rapportLigneCodeAstreinteRepository,
            IPersonnelManager personnelManager)
            : base(uow, rapportRepository, validator)
        {
            this.rapportLigneCodeAstreinteRepository = rapportLigneCodeAstreinteRepository;
            this.personnelManager = personnelManager;

            Search = searchFeature;
            Crud = crudFeature;
        }

        /// <summary>
        ///   Teste un pointage quel que soit son type (personnel et/ou materiel)
        /// </summary>
        /// <param name="rapport">Le pointage que l'on vient de vérifier</param>
        /// <returns>Retourne le rapport </returns>
        public RapportEnt CheckRapport(RapportEnt rapport)
        {
            rapport.ListErreurs = new List<string>();
            ((IRapportValidator)this.Validator).CheckRapport(rapport);
            return rapport;
        }

        /// <summary>
        ///   Liste les identifiants de rapport en erreur
        /// </summary>
        /// <param name="rapportIds">Liste des rapports à vérifier</param>
        /// <returns>Retourne la liste des identifiants de rapport en erreur</returns>
        public List<int> GetListRapportIdWithError(List<int> rapportIds)
        {
            var listRapportId = new List<int>();
            foreach (var id in rapportIds)
            {
                var rapport = GetRapportById(id, false);
                rapport.ListErreurs = new List<string>();
                ((IRapportValidator)this.Validator).CheckRapport(rapport);
                if (rapport.ListErreurs.Count > 0)
                {
                    listRapportId.Add(rapport.RapportId);
                }
            }

            return listRapportId;
        }

        /// <inheritdoc/>
        public SearchRapportEnt GetFiltersList(int utilisateurId)
        {
            return Search.GetFiltersList(utilisateurId);
        }

        /// <inheritdoc/>
        public SearchRapportListWithFilterResult SearchRapportWithFilter(SearchRapportEnt filter, int? page = 1, int? pageSize = 20)
        {
            return Search.SearchRapportWithFilter(filter, page, pageSize);
        }

        /// <inheritdoc/>
        public bool RapportCanBeDeleted(RapportEnt rapport)
        {
            return Search.RapportCanBeDeleted(rapport);
        }

        /// <inheritdoc/>
        public void AddOrUpdateRapport(RapportEnt rapport)
        {
            Crud.AddOrUpdateRapport(rapport);
        }

        /// <inheritdoc/>
        public RapportEnt AddRapport(RapportEnt rapport)
        {
            return Crud.AddRapport(rapport);
        }

        /// <inheritdoc/>
        public RapportEnt AddRapportMaterialType(RapportEnt rapport)
        {
            return Crud.AddRapportMaterialType(rapport);
        }

        /// <inheritdoc/>
        public RapportEnt AddNewPointageReelToRapport(RapportEnt rapport)
        {
            return Crud.AddNewPointageReelToRapport(rapport);
        }

        /// <inheritdoc/>
        public RapportEnt AddPrimeToRapport(RapportEnt rapport, PrimeEnt prime)
        {
            return Crud.AddPrimeToRapport(rapport, prime);
        }

        /// <inheritdoc/>
        public RapportEnt AddTacheToRapport(RapportEnt rapport, TacheEnt tache)
        {
            return Crud.AddTacheToRapport(rapport, tache);
        }

        /// <inheritdoc/>
        public RapportEnt UpdateRapport(RapportEnt rapport)
        {
            Crud.UpdateRapport(rapport);
            return GetRapportById(rapport.RapportId, true);
        }

        /// <inheritdoc/>
        public void CheckRapportStatutChangedInDb(RapportEnt rapport, RapportEnt rapportBeforeUpdate)
        {
            Crud.CheckRapportStatutChangedInDb(rapport, rapportBeforeUpdate);
        }

        /// <inheritdoc/>
        public bool CheckRapportLignesMaterielChanged(RapportEnt rapport, RapportEnt rapportBeforeUpdate)
        {
            return Crud.CheckRapportLignesMaterielChanged(rapport, rapportBeforeUpdate);
        }

        /// <inheritdoc/>
        public bool CheckRapportLignesPersonnelChanged(RapportEnt rapport, RapportEnt rapportBeforeUpdate)
        {
            return Crud.CheckRapportLignesPersonnelChanged(rapport, rapportBeforeUpdate);
        }

        /// <inheritdoc/>
        public void DeleteRapport(RapportEnt rapport, int suppresseurId, bool fromListeRapport = false)
        {
            Crud.DeleteRapport(rapport, suppresseurId, fromListeRapport);
        }

        /// <inheritdoc/>
        public IEnumerable<RapportEnt> GetRapportList()
        {
            return Crud.GetRapportList();
        }

        /// <inheritdoc/>
        public IEnumerable<RapportEnt> GetRapportsMobile(DateTime? sinceDate = default(DateTime?), int? userId = default(int?))
        {
            return Crud.GetRapportsMobile(sinceDate, userId);
        }

        /// <inheritdoc/>
        public IEnumerable<RapportEnt> GetRapportLightList(int utilisateurId, DateTime? periode)
        {
            return Crud.GetRapportLightList(utilisateurId, periode);
        }

        /// <inheritdoc/>
        public RapportEnt GetRapportById(int rapportId, bool forWebUse)
        {
            return Crud.GetRapportById(rapportId, forWebUse);
        }

        /// <inheritdoc/>
        public RapportEnt GetRapportByIdWithoutValidation(int rapportId)
        {
            return Crud.GetRapportByIdWithoutValidation(rapportId);
        }

        /// <inheritdoc/>
        public RapportEnt GetNewRapport(int? ciId = default(int?))
        {
            return Crud.GetNewRapport(ciId);
        }

        /// <inheritdoc/>
        public RapportEnt ApplyValuesRgRapport(RapportEnt rapport, string domaine)
        {
            return Crud.ApplyValuesRgRapport(rapport, domaine);
        }

        /// <inheritdoc/>
        public virtual RapportEnt VerrouillerRapport(RapportEnt rapport, int valideurId)
        {
            return Crud.VerrouillerRapport(rapport, valideurId);
        }

        /// <inheritdoc/>
        public virtual void DeverrouillerRapport(RapportEnt rapport, int valideurId)
        {
            Crud.DeverrouillerRapport(rapport, valideurId);
        }

        /// <inheritdoc/>
        public virtual LockRapportResponse VerrouillerListeRapport(IEnumerable<int> rapportIds, int valideurId, IEnumerable<int> reportNotToLock, SearchRapportEnt filter, string groupe)
        {
            return Crud.VerrouillerListeRapport(rapportIds, valideurId, reportNotToLock, filter, groupe);
        }

        /// <inheritdoc/>
        public virtual void DeverrouillerListeRapport(IEnumerable<int> rapportIds, int valideurId)
        {
            Crud.DeverrouillerListeRapport(rapportIds, valideurId);
        }

        /// <summary>
        ///   Duplique un rapport sur une periode
        /// </summary>
        /// <param name="rapportId">L'id du rapport à dupliquer</param>
        /// <param name="startDate">date de depart de la duplication</param>
        /// <param name="endDate">date de fin de la duplication</param>
        /// <returns>DuplicateRapportResult</returns>
        public DuplicateRapportResult DuplicateRapport(int rapportId, DateTime startDate, DateTime endDate)
        {
            return Crud.DuplicateRapport(rapportId, startDate, endDate);
        }

        /// <inheritdoc/>
        public RapportEnt DuplicateRapport(RapportEnt rapport)
        {
            return Crud.DuplicateRapport(rapport);
        }

        /// <summary>
        ///   Duplique un rapport pour un autre ci
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        public RapportEnt DuplicateRapportForNewCi(RapportEnt rapport)
        {
            return Crud.DuplicateRapportForNewCi(rapport);
        }

        /// <inheritdoc/>
        public bool ValidationRapport(RapportEnt rapport, int valideurId)
        {
            return Crud.ValidationRapport(rapport, valideurId);
        }

        /// <summary>
        /// Ajoute, met à jour ou supprime les pointages de la liste lors d'une duplication
        /// </summary>
        /// <param name="listPointages">Liste des pointages à traîter</param>
        /// <param name="duplicatedPointageId">Id du pointage dupliqué</param>
        /// <param name="rapportsAdded">Liste des rapports ajoutés</param>
        /// <param name="rapportsUpdated">Liste des rapports à mettre à jour</param>
        /// <returns>Le résultat de l'enregistrement</returns>
        public PointagePersonnelSaveResultModel SaveListDuplicatedPointagesPersonnel(IEnumerable<RapportLigneEnt> listPointages, int duplicatedPointageId, out List<RapportEnt> rapportsAdded, out List<RapportEnt> rapportsUpdated)
        {
            return Crud.SaveListDuplicatedPointagesPersonnel(listPointages, duplicatedPointageId, out rapportsAdded, out rapportsUpdated);
        }

        /// <summary>
        /// Ajoute, met à jour ou supprime les pointages de la liste
        /// </summary>
        /// <param name="listPointages">Liste des pointages à traîter</param>
        /// <param name="rapportsAdded">Liste des rapports ajoutés</param>
        /// <param name="rapportsUpdated">Liste des rapports à mettre à jour</param>
        /// <returns>Le résultat de l'enregistrement</returns>
        public PointagePersonnelSaveResultModel SaveListPointagesPersonnel(IEnumerable<RapportLigneEnt> listPointages, out List<RapportEnt> rapportsAdded, out List<RapportEnt> rapportsUpdated)
        {
            return Crud.SaveListPointagesPersonnel(listPointages, out rapportsAdded, out rapportsUpdated);
        }

        /// <summary>
        /// Retourne Vrai si la date du chantier est dans une période clôturée
        /// </summary>
        /// <param name="rapport">Rapoort de chantier</param>
        /// <returns>Vrai si la date du chantier est dans une période clôturée</returns>
        public bool IsDateChantierInPeriodeCloture(RapportEnt rapport)
        {
            return ((IRapportValidator)this.Validator).IsDateChantierInPeriodeCloture(rapport);
        }

        /// <summary>
        /// Retourne la liste des rapports
        /// </summary>
        /// <returns>La liste des rapports</returns>
        public IEnumerable<object> GetRapportListAllSync()
        {
            return Repository.GetAllSync();
        }

        /// <summary>
        /// Permet d'extraire une liste de rapports pour l'exportation
        /// </summary>
        /// <param name="filterRapport">filtres des rapports</param>
        /// <returns>La liste des rapports</returns>
        public IEnumerable<RapportEnt> GetRapportsExportApi(FilterRapportFesExport filterRapport)
        {
            return this.Repository.GetRapportsExportApi(filterRapport);
        }

        /// <summary>
        /// Méthode d'initisalition des informations des sorties astreintes pour un rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        public void InitializeAstreintesInformations(RapportEnt rapport)
        {
            if (rapport != null && rapport.ListLignes.Any())
            {
                foreach (RapportLigneEnt rapportLigne in rapport.ListLignes)
                {
                    var rapportLigneAstreinteListe = this.Repository.GetRapportLigneAstreintes(rapportLigne.RapportLigneId);
                    if (rapportLigneAstreinteListe != null && rapportLigneAstreinteListe.Any())
                    {
                        rapportLigneCodeAstreinteRepository.DeletePrimesAstreinteByLigneAstreinteList(rapportLigneAstreinteListe.Select(x => x.RapportLigneAstreinteId).ToList());
                    }

                    this.Repository.DeleteRapportLigneAstreintes(rapportLigne.RapportLigneId);
                }

                this.Save();
            }
        }

        /// <summary>
        /// Remplir les informations des sorties astreintes
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <returns>Le rapport remplit</returns>
        public RapportEnt FulfillAstreintesInformations(RapportEnt rapport)
        {
            return Crud.FulfillAstreintesInformations(rapport);
        }

        /// <summary>
        /// Check rapport for societe FES
        /// </summary>
        /// <param name="ciId">Ci Identifier</param>
        /// <param name="dateChantier">Date du chantier</param>
        /// <returns>True if Rapport exist </returns>
        public int CheckRepportsForFES(int ciId, DateTime dateChantier)
        {
            RapportEnt rapport = this.Repository.CheckRepportsForFES(ciId, dateChantier);
            return rapport != null ? rapport.RapportId : 0;
        }

        /// <summary>
        /// Retourne un rapport avec son CIid
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <returns>Rapport</returns>
        /// <remarks>GROS FIX EN URGENCE !</remarks>
        public RapportEnt GetRapportCi(int rapportId)
        {
            return Repository.GetRapportsCis().SingleOrDefault(c => c.RapportId.Equals(rapportId));
        }

        /// <summary>
        /// Retourne liste de rapport ligne par Ci après une date de cloture étant vérouiller et contenant des intérimaires
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <param name="statut">Statut du ci</param>
        /// <returns>Liste de rapport ligne</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLigneVerrouillerByCiIdForReceptionInterimaire(int ciId, int statut)
        {
            try
            {
                return Repository.GetRapportLigneVerrouillerByCiIdForReceptionInterimaire(ciId, statut);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne liste de rapport ligne par Ci après une date de cloture étant vérouiller et contenant des intérimaires
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <param name="statut">Statut du ci</param>
        /// <returns>Liste de rapport ligne</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLigneVerrouillerByCiIdForReceptionMaterielExterne(int ciId, int statut)
        {
            try
            {
                return Repository.GetRapportLigneVerrouillerByCiIdForReceptionMaterielExterne(ciId, statut);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public IEnumerable<RapportEnt> GetRapportList(IEnumerable<int> rapportIds)
        {
            return Crud.GetRapportList(rapportIds);
        }


        /// <summary>
        /// Récupération de liste des rapports en fonction d'une liste d'identifiants de rapport
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport</param>
        /// <returns>Liste de rapport</returns>
        public IEnumerable<RapportEnt> GetRapportListWithRapportLignesNoTracking(IEnumerable<int> rapportIds)
        {
            return Crud.GetRapportListWithRapportLignesNoTracking(rapportIds);
        }


        /// <summary>
        /// Add or update la liste des rapports
        /// </summary>
        /// <param name="rapports">La liste des rapports</param>
        public void AddOrUpdateRapportList(IEnumerable<RapportEnt> rapports)
        {
            Crud.AddOrUpdateRapportList(rapports);
        }

        /// <summary>
        /// Get la liste des rapports entre une date de début et une date de fin
        /// </summary>
        /// <param name="ciList">La liste des Cis</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Liste des rapports</returns>
        public IEnumerable<RapportEnt> GetRapportListBetweenDatesByCiList(IEnumerable<int> ciList, DateTime startDate, DateTime endDate)
        {
            return Crud.GetRapportListBetweenDatesByCiList(ciList, startDate, endDate);
        }

        /// <summary>
        /// Ajout en masse de rapports
        /// </summary>
        /// <param name="rapportList">Liste de rapports</param>
        public void AddRangeRapportList(IEnumerable<RapportEnt> rapportList)
        {
            Crud.AddRangeRapportList(rapportList);
        }

        /// <summary>
        /// Retourne la liste des personnels auteur de rapport
        /// </summary>
        /// <param name="search">Object de recherche</param>
        /// <param name="groupeId">Groupe de l'utilisateur courant</param>
        /// <param name="listOrga">Liste des organisations disponible pour l'utilisateur connecté</param>
        /// <returns>La liste des personnels auteur de rapport</returns>
        public IEnumerable<PersonnelEnt> SearchRapportAuthor(SearchLightPersonnelModel search, int? groupeId, IEnumerable<int> listOrga)
        {
            var listUserId = GetAuthorUserId(search.AuthorType, listOrga);
            return personnelManager.SearchRapportAuthor(search, groupeId, listUserId);
        }

        /// <summary>
        /// Recherche de personnels ayant créé, validé ou verrouillé (en fonction du type d'auteur) un rapport dans le référentiel
        /// </summary>
        /// <param name="authorType">Type d'auteur recherché</param>
        /// <param name="listOrga">Liste des organisations accessible à l'utilisateur connecté</param>
        /// <returns>Une liste de personnel</returns>
        private List<int> GetAuthorUserId(string authorType, IEnumerable<int> listOrga)
        {
            return this.Repository.Query()
                              .Include(r => r.AuteurCreation)
                              .Include(r => r.ValideurCDC)
                              .Include(r => r.ValideurCDT)
                              .Include(r => r.ValideurDRC)
                              .Include(r => r.AuteurVerrou)
                              .Include(c => c.CI.Organisation)
                              .Filter(r => !r.DateSuppression.HasValue && !r.IsGenerated)
                              .Filter(r => listOrga.Contains(r.CI.Organisation.OrganisationId))
                              .Filter(GetFilterByAuthorType(authorType))
                              .Get()
                              .Select(GetSelectByAuthorType(authorType))
                              .Distinct().ToList();
        }

        /// <summary>
        /// Prédicat de recherche sur le type d'auteur recherché
        /// </summary>
        /// <param name="authorType">Type d'auteur recherché</param>
        /// <returns>Expression</returns>
        private Expression<Func<RapportEnt, bool>> GetFilterByAuthorType(string authorType)
        {
            switch (authorType)
            {
                case "AuteurCreation":
                    return r => r.AuteurCreationId.HasValue;
                case "ValideurCDC":
                    return r => r.ValideurCDCId.HasValue;
                case "ValideurCDT":
                    return r => r.ValideurCDTId.HasValue;
                case "ValideurDRC":
                    return r => r.ValideurDRCId.HasValue;
                case "AuteurVerrou":
                    return r => r.AuteurVerrouId.HasValue;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Prédicat de selection sur le type d'auteur recherché
        /// </summary>
        /// <param name="authorType">Type d'auteur recherché</param>
        /// <returns>Expression</returns>
        private Expression<Func<RapportEnt, int>> GetSelectByAuthorType(string authorType)
        {
            switch (authorType)
            {
                case "AuteurCreation":
                    return r => r.AuteurCreationId.Value;
                case "ValideurCDC":
                    return r => r.ValideurCDCId.Value;
                case "ValideurCDT":
                    return r => r.ValideurCDTId.Value;
                case "ValideurDRC":
                    return r => r.ValideurDRCId.Value;
                case "AuteurVerrou":
                    return r => r.AuteurVerrouId.Value;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Renvoie la liste des rapport entre 2 dates . Les rapports retournés conçernent les ci envoyés
        /// </summary>
        /// <param name="ciList">Liste des Cis</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>List de rapports en les 2 dates</returns>
        public IEnumerable<RapportEnt> GetRapportListWithRapportLignesBetweenDatesByCiList(IEnumerable<int> ciList, DateTime startDate, DateTime endDate)
        {
            return Repository.GetRapportListWithRapportLignesBetweenDatesByCiList(ciList, startDate, endDate);
        }


        /// <summary>
        /// Retourne une liste de pointages réels en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <param name="personnelId">identifiant du persoonel</param>
        /// <returns>Une liste de pointages </returns>
        public RapportEnt GetRapportsByPersonnelIdAndDatePointagesFiggo(int ciId, DateTime datePointage, int personnelId, int typePersonnel)
        {
            return Repository.GetRapportByPersonnelIdAndDatePointagesFiggo(ciId, datePointage, personnelId, typePersonnel);
        }


        /// <summary>
        /// Retourne  un pointage réels en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <returns>Une liste de pointages </returns>
        public RapportEnt GetRapportsByCilIdAndDatePointagesFiggo(int ciId, DateTime datePointage, int typePersonnel)
        {
            return Repository.GetRapportsByCilIdAndDatePointagesFiggo(ciId, datePointage, typePersonnel);
        }


        /// <summary>
        /// Récupere une liste de rapportLigne définit par sa date et un personnelId et un ciId
        /// </summary>
        /// <param name="date">date du pointage</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="ciId">identifiant du ci</param>
        /// <returns>List des rapportLigne</returns>
        public List<RapportLigneEnt> GetRapportLigneByDateAndPersonnelAndCi(DateTime date, int personnelId, int ciId)
        {
            return Repository.GetRapportLigneByDateAndPersonnelAndCi(date, personnelId, ciId);
        }

        /// <summary>
        /// Permet d'inserer un nouveau rapport creer a partir de figgo
        /// </summary>
        /// <param name="rapport">rapport a inserer</param>
        /// <returns>rapport inserer</returns>
        public RapportEnt AddRapportFiggo(RapportEnt rapport)
        {
            return Crud.AddRapportFiggo(rapport);
        }

        /// <summary>
        /// Retourne une liste de pointages réels en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <param name="personnelId">identifiant du persoonel</param>
        /// <returns>Une liste de pointages </returns>
        public List<RapportEnt> GetAllRapportsByPersonnelIdAndDatePointagesFiggo(List<int> ciId, DateTime datePointage, int personnelId, int typePersonnel)
        {
            return Repository.GetAllRapportByPersonnelIdAndDatePointagesFiggo(ciId, datePointage, personnelId, typePersonnel);
        }

        /// <summary>
        /// Récupere une liste de rapportLigne définit par sa date et un personnelId
        /// </summary>
        /// <param name="ciId">identifiant du ci</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="date">date d'absence</param>
        /// <returns>List des rapportLigne</returns>
        public List<RapportLigneEnt> GetRapportLigneByCiAndPersonnels(int ciId, int personnelId, DateTime date)
        {
            return Repository.GetRapportLigneByCiAndPersonnel(ciId, personnelId, date);
        }

        /// <summary>
        ///   Ajoute une tache au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <param name="tache">La tache</param>
        /// <param name="heureTache">heure tache</param>
        /// <returns>Un rapport</returns>
        public RapportEnt AddTacheToRapportFiggo(RapportEnt rapport, TacheEnt tache, double heureTache)
        {
            return Crud.AddTacheToRapportFiggo(rapport, tache, heureTache);
        }

    }
}
