using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.CI;
using Fred.Web.Shared.Models.Personnel;

namespace Fred.Business.Equipe
{
    /// <summary>
    /// Equipe manager class
    /// </summary>
    public class EquipeManager : Manager<EquipeEnt, IEquipeRepository>, IEquipeManager
    {
        #region private attributes

        private readonly IUtilisateurManager utilisateurManager;
        private readonly IPointageManager pointageManager;

        #endregion

        #region constructor

        public EquipeManager(
            IUnitOfWork uow,
            IEquipeRepository equipeRepository,
            IUtilisateurManager utilisateurManager,
            IPointageManager pointageManager)
            : base(uow, equipeRepository)
        {
            this.utilisateurManager = utilisateurManager;
            this.pointageManager = pointageManager;
        }

        #endregion

        #region public methode

        /// <summary>
        /// Creation d'une equipe
        /// </summary>
        /// <returns>Equipe identifier</returns>
        public int? CreateEquipe()
        {
            int proprietaireId = this.utilisateurManager.GetContextUtilisateurId();
            EquipeEnt equipe = new EquipeEnt { ProprietaireId = proprietaireId };
            this.Repository.CreateEquipe(equipe);
            Save();
            return equipe.EquipeId;
        }

        /// <summary>
        /// Get une equipe par le proprietaire identifier
        /// </summary>
        /// <returns>Aquipe entity</returns>
        public IEnumerable<PersonnelEnt> GetEquipePersonnelsByProprietaireId()
        {
            int proprietaireId = this.utilisateurManager.GetContextUtilisateurId();
            return this.Repository.GetEquipePersonnelsByProprietaireId(proprietaireId);
        }

        /// <summary>
        /// Manage les personnels d'une equipe
        /// </summary>
        /// <param name="personnelsIdToAdd">List des identifiers des personnels a jouter</param>
        /// <param name="personnelsIdToDelete">List des identifier des personnels a supprimer</param>
        public void ManageEquipePersonnels(List<int> personnelsIdToAdd, List<int> personnelsIdToDelete)
        {
            if (personnelsIdToAdd != null && personnelsIdToAdd.Any())
            {
                AddPersonnelsToEquipeFavorite(personnelsIdToAdd);
            }

            if (personnelsIdToDelete != null && personnelsIdToDelete.Any())
            {
                DeletePersonnelsEquipe(personnelsIdToDelete);
            }
        }

        /// <summary>
        /// Ajouter des personnels a une equipe
        /// </summary>
        /// <param name="personnelsId">List des identifiants des personnels</param>
        public void AddPersonnelsToEquipeFavorite(IList<int> personnelsId)
        {
            if (personnelsId != null && personnelsId.Any())
            {
                int proprietaireId = this.utilisateurManager.GetContextUtilisateurId();
                EquipeEnt equipe = Repository.GetEquipeByProprietaireId(proprietaireId);
                if (equipe != null)
                {
                    List<int> personnelsToAdd = new List<int>();
                    foreach (int id in personnelsId)
                    {
                        bool isInTheTeam = Repository.IsPersonnelInTeam(id, equipe.EquipeId);
                        if (!isInTheTeam)
                        {
                            personnelsToAdd.Add(id);
                        }
                    }

                    Repository.AddPersonnelsToEquipeFavorite(equipe.EquipeId, personnelsToAdd);
                }
                else
                {
                    int? equipeId = this.CreateEquipe();
                    if (equipeId.HasValue)
                    {
                        Repository.AddPersonnelsToEquipeFavorite(equipeId.Value, personnelsId);
                    }
                }

                Save();
            }
        }

        /// <summary>
        /// Suppression des personnels d'un equipe
        /// </summary>
        /// <param name="personnelsId">List des personnels</param>
        public void DeletePersonnelsEquipe(IList<int> personnelsId)
        {
            if (personnelsId != null && personnelsId.Any())
            {
                int proprietaireId = this.utilisateurManager.GetContextUtilisateurId();
                EquipeEnt equipe = Repository.GetEquipeByProprietaireId(proprietaireId);
                if (equipe != null)
                {
                    List<EquipePersonnelEnt> equipePersonnels = new List<EquipePersonnelEnt>();
                    foreach (int personnelId in personnelsId)
                    {
                        EquipePersonnelEnt equipePersonnel = Repository.GetEquipePersonnel(equipe.EquipeId, personnelId);
                        if (equipePersonnel != null)
                        {
                            equipePersonnels.Add(equipePersonnel);
                        }
                    }

                    try
                    {
                        Repository.DeletePersonnelsEquipe(equipe.EquipeId, equipePersonnels);
                        SaveWithTransaction();
                    }
                    catch (Exception exep)
                    {
                        throw new FredRepositoryException(exep.Message, exep);
                    }
                }
            }
        }

        /// <summary>
        /// Get equipe by proprietaire identifier
        /// </summary>
        /// <param name="proprietaireId">Proprietaire identifier</param>
        /// <returns>Equipe entity object</returns>
        public EquipeEnt GetEquipeByProprietaireId(int proprietaireId)
        {
            return Repository.GetEquipeByProprietaireId(proprietaireId);
        }

        /// <summary>
        /// Retourne la liste des personnel dont l'utilisateur en cours est responsable par statut de personnel .
        /// </summary>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>IEnumerable of PersonnelSummaryPointageModel</returns>
        public IEnumerable<PersonnelSummaryPointageModel> GetResponsablePersonnelListSummary(string personnelStatut, DateTime mondayDate)
        {
            List<AffectationEnt> affectationList = utilisateurManager.GetAffectationList(personnelStatut).ToList();
            if (affectationList.IsNullOrEmpty())
            {
                return new List<PersonnelSummaryPointageModel>();
            }
            List<int> personnelBlocked = new List<int>();
            foreach (var item in affectationList)
            {
                if (item.IsDelete && Managers.Pointage.CheckPointageByPersonnelAndCi(item.PersonnelId, item.CiId, mondayDate) || !item.IsDelete)
                {
                    personnelBlocked.Add(item.PersonnelId);
                }
            }
            personnelBlocked = personnelBlocked.Distinct().ToList();

            List<AffectationEnt> affectationListPersonnel = Managers.Affectation.GetAffectationByListPersonnelId(personnelBlocked);
            var ListDesaffecte = affectationListPersonnel.Where(x => x.IsDelete).ToList();
            List<KeyValuePair<int, int>> CiAndPersonnelDesaffecte = new List<KeyValuePair<int, int>>();
            foreach (var item in ListDesaffecte)
            {
                CiAndPersonnelDesaffecte.Add(new KeyValuePair<int, int>(item.CiId, item.PersonnelId));
            }

            IEnumerable<PersonnelEnt> team = GetEquipePersonnelsByProprietaireId();
            IEnumerable<PersonnelRapportSummaryEnt> personnelSummary = pointageManager.GetPersonnelPointageSummary(personnelBlocked, mondayDate);

            // Il faudra diviser les jointures pour avoir le bon résultat
            IEnumerable<PersonnelSummaryPointageModel> personnelTeamJoin = JoinPersonnelAndTeam(affectationListPersonnel, team);
            IEnumerable<PersonnelSummaryPointageModel> result = JoinPersonnelTeamAndSummary(personnelTeamJoin, personnelSummary);
            return result.Where(x => !CiAndPersonnelDesaffecte.Any(zz => zz.Key == x.CiId && zz.Value == x.PersonnelId) || CiAndPersonnelDesaffecte.Any(zz => zz.Key == x.CiId && zz.Value == x.PersonnelId) && x.TotalHeure != null).ToList();
        }

        /// <summary>
        /// Retourne la liste des ci d'un responsable donné
        /// </summary>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>Une liste de CiPointageSummary</returns>
        public IEnumerable<CiSummaryPointageModel> GetResponsableCiListSummary(string personnelStatut, DateTime mondayDate)
        {
            IEnumerable<CIEnt> ciList = utilisateurManager.GetCiListOfResponsable();
            if (ciList.IsNullOrEmpty())
            {
                return new List<CiSummaryPointageModel>();
            }

            List<int> ciIdList = ciList.Select(o => o.CiId).ToList();
            IEnumerable<CiPointageSummaryEnt> ciSummaryList = pointageManager.GetCiPointageSummary(ciIdList, personnelStatut, mondayDate);

            return (from ci in ciList
                    join cs in ciSummaryList on ci.CiId equals cs.CiId into jResult
                    from j in jResult.DefaultIfEmpty()
                    select new CiSummaryPointageModel
                    {
                        CiId = ci.CiId,
                        CiCode = ci.Code,
                        Libelle = ci.Libelle,
                        CiEtablissementComptableCode = ci.EtablissementComptable?.Code,
                        CiSocieteCode = ci.Societe?.Code,
                        CiTypeRessourceKey = ci.CIType?.RessourceKey,
                        TotalHeure = j != null ? j.TotalHeures : 0,
                        TotalHeureSup = j != null ? j.TotalHeuresNormalesSup : 0
                    });
        }

        /// <summary>
        /// Faire la jointure en la liste des personnels et l'équipe favorite 
        /// </summary>
        /// <param name="affectationList">Affectation list</param>
        /// <param name="team">Team list</param>
        /// <returns>IEnumerable de PersonnelSummaryPointageModel</returns>
        private IEnumerable<PersonnelSummaryPointageModel> JoinPersonnelAndTeam(IEnumerable<AffectationEnt> affectationList, IEnumerable<PersonnelEnt> team)
        {
            if (affectationList.IsNullOrEmpty())
            {
                return new List<PersonnelSummaryPointageModel>();
            }

            // Pour assuer un comportement de left join
            if (team == null)
            {
                team = new List<PersonnelEnt>();
            }

            return from p in affectationList
                   join e in team on p.PersonnelId equals e.PersonnelId into pe
                   from r in pe.DefaultIfEmpty()
                   select new PersonnelSummaryPointageModel
                   {
                       PersonnelId = p.PersonnelId,
                       CiId = p.CiId,
                       CiCode = p.CI?.Code,
                       CiEtablissementComptableCode = p.CI.EtablissementComptable?.Code,
                       CiSocieteCode = p.CI.Societe?.Code,
                       CiTypeRessourceKey = p.CI.CIType?.RessourceKey,
                       IsAbsence = p.CI.IsAbsence,
                       Nom = p.Personnel?.Nom,
                       Prenom = p.Personnel?.Prenom,
                       Matricule = p.Personnel?.Matricule,
                       SocieteCode = p.Personnel?.Societe?.Code,
                       IsInFavouriteTeam = (r != null)
                   };
        }

        /// <summary>
        /// Join the personnel team object to the summary list object
        /// </summary>
        /// <param name="personnelTeam">Personnel team</param>
        /// <param name="personnelSummary">Personnel summary</param>
        /// <returns>IEnumerable de PersonnelSummaryPointageModel</returns>
        private IEnumerable<PersonnelSummaryPointageModel> JoinPersonnelTeamAndSummary(IEnumerable<PersonnelSummaryPointageModel> personnelTeam, IEnumerable<PersonnelRapportSummaryEnt> personnelSummary)
        {
            if (personnelTeam.IsNullOrEmpty())
            {
                return new List<PersonnelSummaryPointageModel>();
            }

            // Pour assuer un comportement de left join
            if (personnelSummary == null)
            {
                personnelSummary = new List<PersonnelRapportSummaryEnt>();
            }

            return from p in personnelTeam
                   join s in personnelSummary on new { p.PersonnelId, p.CiId } equals new { s.PersonnelId, s.CiId } into ps
                   from r in ps.DefaultIfEmpty()
                   select new PersonnelSummaryPointageModel
                   {
                       PersonnelId = p.PersonnelId,
                       CiId = p.CiId,
                       CiCode = p.CiCode,
                       IsAbsence = p.IsAbsence,
                       Nom = p.Nom,
                       Prenom = p.Prenom,
                       IsInFavouriteTeam = p.IsInFavouriteTeam,
                       Matricule = p.Matricule,
                       SocieteCode = p.SocieteCode,
                       TotalHeure = r?.TotalHeures,
                       TotalHeureAbsence = r?.TotalHeuresAbsence,
                       TotalHeureSup = r?.TotalHeuresNormalesSup
                   };
        }

        /// <summary>
        /// Retourne la liste des ouvriers et Cis pour le pointage d'un utilisateur donné
        /// </summary>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>Rapport hebdo entree summary</returns>
        public RapportHebdoEntreeSummary GetUserPointageHebdoSummary(string personnelStatut, DateTime mondayDate)
        {
            return new RapportHebdoEntreeSummary
            {
                CiPointageSummaryList = GetResponsableCiListSummary(personnelStatut, mondayDate),
                PersonnelSummaryList = GetResponsablePersonnelListSummary(personnelStatut, mondayDate)
            };
        }

        #endregion
    }
}
