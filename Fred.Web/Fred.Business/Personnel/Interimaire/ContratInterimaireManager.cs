using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Personnel.Interimaire;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Personnel.Interimaire;
using MoreLinq;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///   Gestionnaire des contrats d'intérimaires
    /// </summary>
    public class ContratInterimaireManager : Manager<ContratInterimaireEnt, IContratInterimaireRepository>, IContratInterimaireManager
    {
        private readonly IUtilisateurManager utilisateurMgr;
        private readonly IExternalDirectoryRepository externalDirectoryRepo;
        private readonly IZoneDeTravailManager zoneDeTravailManager;

        public ContratInterimaireManager(
            IUnitOfWork uow,
            IContratInterimaireRepository contratInterimaireRepository,
            IContratInterimaireValidator validator,
            IUtilisateurManager utilisateurMgr,
            IExternalDirectoryRepository externalDirectoryRepo, IZoneDeTravailManager zoneDeTravailManager)
         : base(uow, contratInterimaireRepository, validator)
        {
            this.utilisateurMgr = utilisateurMgr;
            this.externalDirectoryRepo = externalDirectoryRepo;
            this.zoneDeTravailManager = zoneDeTravailManager;
        }

        /// <summary>
        /// Permet de récupérer une liste de contrat intérimaire en fonction d'un personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Liste des contrats intérimaire appartenant au personnel id</returns>
        public List<ContratInterimaireEnt> GetContratInterimaireByPersonnelId(int personnelId)
        {
            try
            {
                var result = Repository.GetContratInterimaireByPersonnelId(personnelId);
                IgnoreSelfReferencing(result);

                return result;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        private void IgnoreSelfReferencing(List<ContratInterimaireEnt> result)
        {
            foreach (var contrat in result)
            {
                if (contrat.Societe != null)
                {
                    contrat.Societe.ContratInterimaires = null;
                }
                if (contrat.Ci != null && contrat.Ci.Societe != null)
                {
                    contrat.Ci.Societe.ContratInterimaires = null;
                }
                if (contrat.CommandeContratInterimaires != null && contrat.CommandeContratInterimaires.Any())
                {
                    contrat.CommandeContratInterimaires.ForEach(x => x.Contrat = null);
                }
                if (contrat.Interimaire != null)
                {
                    contrat.Interimaire.ContratInterimaires = null;
                    if ((contrat.Interimaire.ContratActif != null))
                    {
                        contrat.Interimaire.ContratActif.Interimaire = null;
                    }
                }
                if (contrat.ZonesDeTravail != null && contrat.ZonesDeTravail.Any())
                {
                    contrat.ZonesDeTravail.ForEach(x => x.Contrat = null);
                }
            }
        }

        /// <summary>
        /// Permet de récupérer une liste de motif de remplacement
        /// </summary>
        /// <returns>Liste des Motifs de remplacement</returns>
        public List<MotifRemplacementEnt> GetMotifRemplacement()
        {
            try
            {
                return Repository.GetMotifRemplacement();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer une liste de contrat intérimaire en fonction d'un personnel id
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat d'interimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireById(int contratInterimaireId)
        {
            try
            {
                return Repository.GetContratInterimaireById(contratInterimaireId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer un contrat intérimaire en fonction de son numéro de contrat
        /// </summary>
        /// <param name="numeroContrat">Numéro du contrat d'interimaire</param>
        /// <param name="contratInterimaireId">Identifiant unique du contrat d'interimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByNumeroContrat(string numeroContrat, int contratInterimaireId)
        {
            try
            {
                return Repository.GetContratInterimaireByNumeroContrat(numeroContrat, contratInterimaireId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer le contrat actif d'un intérimaire
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant Unique du Contrat Intérimaire</param>
        /// <param name="interimaireId">Identifiant Unique de l'Intérimaire</param>
        /// <param name="dateDebut">Date de début du Contrat Intérimaire</param>
        /// <param name="dateFin">Date de fin du Contrat Intérimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireAlreadyActif(int contratInterimaireId, int interimaireId, DateTime dateDebut, DateTime dateFin)
        {
            try
            {
                return Repository.GetContratInterimaireAlreadyActif(contratInterimaireId, interimaireId, dateDebut, dateFin);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="datePointage">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByDatePointage(int? interimaireId, DateTime datePointage)
        {
            try
            {
                return Repository.GetContratInterimaireByDatePointage(interimaireId, datePointage);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet d'ajouter un contrat à un personnel intérimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Intérimaire</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        public ContratInterimaireEnt AddContratInterimaire(ContratInterimaireEnt contratInterimaireEnt, int? userId = null)
        {
            BusinessValidation(contratInterimaireEnt);
            try
            {
                var zoneDeTravailList = contratInterimaireEnt.ZonesDeTravail?.ToList();
                Repository.AddContratInterimaire(contratInterimaireEnt);
                Save();

                contratInterimaireEnt = Repository.GetContratInterimaireById(contratInterimaireEnt.ContratInterimaireId);

                AddZonesDeTravail(contratInterimaireEnt, zoneDeTravailList);

                IEnumerable<ContratInterimaireEnt> contrats = GetContratInterimaireByPersonnelId(contratInterimaireEnt.InterimaireId).Where(c => c.ContratInterimaireId != contratInterimaireEnt.ContratInterimaireId);
                if (!contrats.Any())
                {
                    contratInterimaireEnt.Interimaire = Managers.Personnel.GetPersonnel(contratInterimaireEnt.InterimaireId);
                    contratInterimaireEnt.Interimaire.DateEntree = contratInterimaireEnt.DateDebut;
                    Managers.Personnel.Update(contratInterimaireEnt.Interimaire, userId);
                }
                else
                {
                    bool isCurrentContractNewer = CheckIfInterimContractIsNewer(contratInterimaireEnt, userId, contrats);
                    if (isCurrentContractNewer)
                    {
                        UpdateDateEntreePersonnelInterimaire(contratInterimaireEnt, userId);
                    }
                }

                CheckContratInterimaireAndExpirationDate(contratInterimaireEnt.InterimaireId);

                return contratInterimaireEnt;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        private bool CheckIfInterimContractIsNewer(ContratInterimaireEnt contratInterimaireEnt, int? userId, IEnumerable<ContratInterimaireEnt> contrats)
        {
            return contrats.Any(x => contratInterimaireEnt.DateDebut < x.DateDebut);
        }

        private void UpdateDateEntreePersonnelInterimaire(ContratInterimaireEnt contratInterimaireEnt, int? userId)
        {
            contratInterimaireEnt.Interimaire.DateEntree = contratInterimaireEnt.DateDebut;
            Managers.Personnel.UpdateDateEntreePersonnelInterimaire(contratInterimaireEnt.Interimaire.PersonnelId, contratInterimaireEnt.Interimaire.DateEntree, userId);
        }

        private void AddZonesDeTravail(ContratInterimaireEnt contratInterimaireEnt, ICollection<ZoneDeTravailEnt> zoneDeTravaiList)
        {
            if (zoneDeTravaiList == null || !zoneDeTravaiList.Any())
            {
                AddDefaultZoneDeTravail(contratInterimaireEnt);
            }
            else
            {
                AddListOfZoneDeTravail(contratInterimaireEnt, zoneDeTravaiList);
            }
        }

        private void AddListOfZoneDeTravail(ContratInterimaireEnt contratInterimaireEnt, ICollection<ZoneDeTravailEnt> zoneDeTravailList)
        {
            foreach (var zoneDeTravail in zoneDeTravailList)
            {
                zoneDeTravail.ContratInterimaireId = contratInterimaireEnt.ContratInterimaireId;
                zoneDeTravailManager.AddZoneDeTravail(zoneDeTravail);
            }
        }

        private void AddDefaultZoneDeTravail(ContratInterimaireEnt contratInterimaireEnt)
        {
            if (contratInterimaireEnt.Ci == null)
            {
                contratInterimaireEnt.Ci = Repository.GetContratInterimaireById(contratInterimaireEnt.ContratInterimaireId).Ci;
            }

            if (contratInterimaireEnt.Ci != null && contratInterimaireEnt.Ci.EtablissementComptableId.HasValue)
            {
                ZoneDeTravailEnt zoneDeTravailEnt = new ZoneDeTravailEnt()
                {
                    EtablissementComptableId = (int)contratInterimaireEnt.Ci.EtablissementComptableId,
                    ContratInterimaireId = contratInterimaireEnt.ContratInterimaireId
                };

                zoneDeTravailManager.AddZoneDeTravail(zoneDeTravailEnt);
            }
        }


        /// <summary>
        /// Permet de modifier un contrat d'intérimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Intérimaire</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        public ContratInterimaireEnt UpdateContratInterimaire(ContratInterimaireEnt contratInterimaireEnt)
        {
            BusinessValidation(contratInterimaireEnt);
            try
            {
                var zoneDeTravaiList = contratInterimaireEnt.ZonesDeTravail?.ToList();

                // Manage Zone de travail
                ManageZoneDeTravailList(contratInterimaireEnt, zoneDeTravaiList);

                Repository.UpdateContratInterimaire(contratInterimaireEnt);
                Save();

                return GetContratInterimaireById(contratInterimaireEnt.ContratInterimaireId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        private void ManageZoneDeTravailList(ContratInterimaireEnt contratInterimaireEnt, List<ZoneDeTravailEnt> zoneDeTravaiList)
        {
            // Supprimer les anciennes zones de travail
            zoneDeTravailManager.DeleteZonesDeTravailByContratInterimaire(contratInterimaireEnt.ContratInterimaireId);

            // Insérer les nouvelles zones de travail
            AddZonesDeTravail(contratInterimaireEnt, zoneDeTravaiList);
        }

        /// <summary>
        /// Permet de supprimer un contrat d'intérimaire en fonction de son identifiant
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique d'un contrat intérimaire</param>
        public void DeleteContratInterimaireById(int contratInterimaireId)
        {
            try
            {
                ContratInterimaireEnt contratInterimaireEnt = GetContratInterimaireById(contratInterimaireId);
                IList<ZoneDeTravailEnt> zonesDeTravail = contratInterimaireEnt.ZonesDeTravail.ToList();

                for (int i = 0; i < zonesDeTravail.Count; i++)
                {
                    zoneDeTravailManager.DeleteZoneDeTravail(zonesDeTravail[i]);
                }

                Repository.DeleteContratInterimaireById(contratInterimaireId);
                Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne la liste des CI pour un intérimaire et une date donnée
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="date">Date du pointage</param>
        /// <returns>La liste des CI pour un intérimaire et ue date donnée</returns>
        public IEnumerable<CIEnt> GetCIList(int personnelId, DateTime date)
        {
            try
            {
                return Repository.GetCIList(personnelId, date);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public DatesMaxInterimaireModel GetDatesMax(int personnelId, DateTime date)
        {
            try
            {
                var datesMax = new DatesMaxInterimaireModel();
                var contrat = GetContratInterimaireByDatePointage(personnelId, date);
                if (contrat != null)
                {
                    datesMax.dateMinContrat = contrat.DateDebut;
                    var dateMinNextContrat = GetDateDebutNextContrat(personnelId, date);
                    if (dateMinNextContrat.HasValue && contrat.DateFin.AddDays(contrat.Souplesse) >= dateMinNextContrat.Value)
                    {
                        datesMax.dateMaxContrat = dateMinNextContrat.Value.AddDays(-1);
                    }
                    else
                    {
                        datesMax.dateMaxContrat = contrat.DateFin.AddDays(contrat.Souplesse);
                    }
                }
                return datesMax;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        private DateTime? GetDateDebutNextContrat(int personnelId, DateTime date)
        {
            try
            {
                return Repository.GetDateDebutNextContrat(personnelId, date);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne une liste de libelle de ci sur lesquels l'intérimaire a été pointé lors de sa période de contrat souplesse incluse
        /// </summary>
        /// <param name="contratInterimaireEnt">contrat intérimaire</param>
        /// <returns>liste des libelle des ci</returns>
        public List<string> GetCiInRapportLigneByDateContratInterimaire(ContratInterimaireEnt contratInterimaireEnt)
        {
            try
            {
                DatesMaxInterimaireModel periode = GetDatesMax(contratInterimaireEnt.InterimaireId, contratInterimaireEnt.DateFin);
                if (contratInterimaireEnt.CiId.HasValue)
                {
                    return Repository.GetCiInRapportLigneByDateContratInterimaire(contratInterimaireEnt.InterimaireId, periode, contratInterimaireEnt.CiId.Value);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée en comprenant la souplesse
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="date">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByDatePointageAndSouplesse(int? interimaireId, DateTime date)
        {
            try
            {
                List<ContratInterimaireEnt> listContratsActifs = Repository.GetContratInterimaireByDatePointageAndSouplesse(interimaireId, date);
                return listContratsActifs.OrderByDescending(c => c.DateDebut).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée pour les réceptions intérimaires
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="datePointage">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByDatePointageForReceptionInterimaire(int? interimaireId, DateTime datePointage)
        {
            try
            {
                return Repository.GetContratInterimaireByDatePointage(interimaireId, datePointage);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée en comprenant la souplesse pour les réceptions intérimaires
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="date">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByDatePointageAndSouplesseForReceptionInterimaire(int? interimaireId, DateTime date)
        {
            try
            {
                List<ContratInterimaireEnt> listContratsActifs = Repository.GetContratInterimaireByDatePointageAndSouplesse(interimaireId, date);
                return listContratsActifs.OrderByDescending(c => c.DateDebut).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne la liste des jours ouverts dans les contrats de l'intérimaire
        /// </summary>
        /// <param name="interimaireId">Identifiant de l'intérimaire</param>
        /// <param name="period">Période</param>
        /// <returns>La liste des jours ouverts dans les contrats de l'intérimaire</returns>
        public List<int> GetListDaysAvailableInPeriod(int interimaireId, DateTime period)
        {
            var listDays = new List<int>();
            // On récupère tous les contrats ayant des journée incluses dans la période
            var listContrats = GetListContratInterimaireOpenOnPeriod(interimaireId, period);
            foreach (var contrat in listContrats)
            {
                listDays.AddRange(GetListDaysIncludeInDateRange(contrat.DateDebut, contrat.DateFin.AddDays(contrat.Souplesse), period));
            }

            // Suppression de doublon et tri
            listDays = listDays.Distinct().ToList();
            listDays.Sort();
            return listDays;
        }

        /// <summary>
        /// Retourne la liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée
        /// </summary>
        /// <param name="interimaireId">Identifiant de l'intérimaire</param>
        /// <param name="period">Période</param>
        /// <returns>La liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée</returns>
        public List<ContratInterimaireEnt> GetListContratInterimaireOpenOnPeriod(int interimaireId, DateTime period)
        {
            return Repository.GetListContratInterimaireOpenOnPeriod(interimaireId, period);
        }

        private List<int> GetListDaysIncludeInDateRange(DateTime beginDate, DateTime endDate, DateTime period)
        {
            var listDays = new List<int>();
            var minDateRange = new DateTime(period.Year, period.Month, 1);
            var maxDateRange = new DateTime(period.Year, period.Month, DateTime.DaysInMonth(period.Year, period.Month));

            if (beginDate < minDateRange && endDate >= minDateRange && endDate <= maxDateRange)
            {
                // Cas où le contrat commence au mois précédent
                listDays.AddRange(Enumerable.Range(1, endDate.Day));
            }
            if (beginDate >= minDateRange && beginDate <= maxDateRange && endDate > maxDateRange)
            {
                // Cas où le contrat termine au mois suivant
                listDays.AddRange(Enumerable.Range(beginDate.Day, maxDateRange.Day - beginDate.Day + 1));
            }
            if (beginDate >= minDateRange && endDate <= maxDateRange)
            {
                // Cas où le contrat est inclut dans l'intervalle de la période
                listDays.AddRange(Enumerable.Range(beginDate.Day, endDate.Day - beginDate.Day + 1));
            }
            if (beginDate <= minDateRange && maxDateRange <= endDate)
            {
                //Cas où le contrat commence au mois précédent et se termine au mois suivant
                listDays.AddRange(Enumerable.Range(minDateRange.Day, maxDateRange.Day));
            }
            return listDays;
        }

        /// <summary>
        /// Retourne la liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée
        /// </summary>
        /// <param name="personnelIds">Identifiant des personnel</param>        
        /// <param name="startDate">Date de debut de la periode</param>
        /// <param name="endDate">date de fin de la periode</param>       
        /// <returns>La liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée</returns>
        public List<ContratInterimaireEnt> GetListContratInterimaireOpenOnPeriod(List<int> personnelIds, DateTime startDate, DateTime endDate)
        {
            return Repository.GetListContratInterimaireOpenOnPeriod(personnelIds, startDate, endDate);
        }

        /// <summary>
        /// Permet de récupérer une liste de contrat intérimaire en fonction d'un personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Liste des contrats intérimaire appartenant au personnel id</returns>
        public ContratInterimaireEnt GetContratInterimaireActifByPersonnelId(int personnelId)
        {
            try
            {
                return Repository.GetContratInterimaireActifByPersonnelId(personnelId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }


        /// <summary>
        /// Pour un interiaire, vérifie si le compte est expiré 
        /// et met à jour la date d'expritation avec la date de fin de contrat interimaire
        /// </summary>
        /// <param name="userId">Udentifiant utilisateur</param>
        public void CheckContratInterimaireAndExpirationDate(int userId)
        {
            var user = utilisateurMgr.GetById(userId, true);

            if (user?.Personnel?.IsInterimaire == true)
            {
                var contratActif = GetContratInterimaireActifByPersonnelId(userId);

                if (contratActif != null &&
                    user.ExternalDirectory?.DateExpiration.HasValue == true &&
                    user.ExternalDirectory?.IsActived == true &&
                    user.ExternalDirectory?.DateExpiration.Value < contratActif.DateFin)
                {
                    var externalDirectory = externalDirectoryRepo.FindById(user.ExternalDirectory.FayatAccessDirectoryId);
                    externalDirectory.DateExpiration = contratActif.DateFin;
                    externalDirectoryRepo.Update(externalDirectory);
                    Save();
                }
            }
        }

        /// <summary>
        /// Récupére le contrat intérimaire par numéro de contrat et groupe code
        /// </summary>
        /// <param name="numeroContrat">Numero de contrat</param>
        /// <param name="groupeCode">Code du groupe</param>
        /// <returns>Un contrat interimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByNumeroContratAndGroupeCode(string numeroContrat, string groupeCode)
        {
            try
            {
                return Repository.GetContratInterimaireByNumeroContratAndGroupeCode(numeroContrat, groupeCode);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
