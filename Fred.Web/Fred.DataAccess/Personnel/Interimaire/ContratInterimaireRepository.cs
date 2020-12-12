using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Personnel.Interimaire;
using Fred.EntityFramework;
using Fred.Web.Shared.Models.Personnel.Interimaire;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Personnel.Interimaire
{
    /// <summary>
    ///   Référentiel de données pour les Contrats Interimaire 
    /// </summary>
    public class ContratInterimaireRepository : FredRepository<ContratInterimaireEnt>, IContratInterimaireRepository
    {

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ContratInterimaireRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public ContratInterimaireRepository(FredDbContext context)
          : base(context) { }

        /// <summary>
        /// Permet de récupérer une liste de contrat intérimaire en fonction d'un personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Liste des contrats intérimaire appartenant au personnel id</returns>
        public List<ContratInterimaireEnt> GetContratInterimaireByPersonnelId(int personnelId)
        {
            return Context.ContratInterimaires
                          .Where(d => d.InterimaireId == personnelId)
                          .OrderBy(d => d.DateDebut)
                          .Include(d => d.Interimaire)
                          .Include(d => d.Fournisseur)
                          .Include(d => d.Societe.TypeSociete)
                          .Include(d => d.Ci.Societe.TypeSociete)
                          .Include(d => d.Unite)
                          .Include(d => d.Ressource)
                          .Include(d => d.MotifRemplacement)
                          .Include(d => d.PersonnelRemplace)
                          .Include(d => d.CommandeContratInterimaires)
                          .Include(d => d.ZonesDeTravail)
                          .ThenInclude(z => z.EtablissementComptable)
                          .AsNoTracking()
                          .ToList();
        }

        /// <summary>
        /// Permet de récupérer une liste des motifs de remplacement
        /// </summary>
        /// <returns>Liste des motifs de remplacement</returns>
        public List<MotifRemplacementEnt> GetMotifRemplacement()
        {
            return this.Context.MotifRemplacement.AsNoTracking().ToList();
        }

        /// <summary>
        /// Permet de récupérer une liste de contrat intérimaire en fonction d'un personnel id
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat d'interimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireById(int contratInterimaireId)
        {
            return Query()
              .Include(d => d.Interimaire)
              .Include(d => d.Fournisseur)
              .Include(d => d.Societe)
              .Include(d => d.Ci)
              .Include(d => d.Unite)
              .Include(d => d.Ressource)
              .Include(d => d.MotifRemplacement)
              .Include(d => d.PersonnelRemplace)
              .Include(d => d.ZonesDeTravail)
              .Filter(d => d.ContratInterimaireId == contratInterimaireId)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }

        /// <summary>
        /// Permet de récupérer un contrat intérimaire en fonction de son numéro de contrat
        /// </summary>
        /// <param name="numeroContrat">Numéro du contrat d'interimaire</param>
        /// <param name="contratInterimaireId">Identifiant unique du contrat d'interimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByNumeroContrat(string numeroContrat, int contratInterimaireId)
        {
            return Query()
              .Filter(c => c.ContratInterimaireId != contratInterimaireId)
              .Filter(d => d.NumContrat == numeroContrat)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
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
            dateDebut = dateDebut.Date;
            dateFin = dateFin.Date;

            return Query()
              .Filter(c => c.InterimaireId == interimaireId)
              .Filter(c => (c.DateDebut <= dateDebut && dateDebut <= c.DateFin) || (c.DateDebut <= dateFin && dateFin <= c.DateFin) || (dateDebut <= c.DateDebut && c.DateFin <= dateFin))
              .Filter(c => c.ContratInterimaireId != contratInterimaireId)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }


        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="datePointage">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByDatePointage(int? interimaireId, DateTime datePointage)
        {
            datePointage = datePointage.Date;

            return Query()
              .Include(d => d.Interimaire)
              .Include(d => d.Fournisseur)
              .Include(d => d.Societe)
              .Include(d => d.Societe.TypeSociete)
              .Include(d => d.Ci)
              .Include(d => d.Ci.CIDevises)
              .Include(d => d.Ci.Taches)
              .Include(d => d.Ci.Societe.TypeSociete)
              .Include(d => d.Unite)
              .Include(d => d.Ressource)
              .Include(d => d.MotifRemplacement)
              .Include(d => d.PersonnelRemplace)
              .Include(d => d.ZonesDeTravail)
              .Filter(c => c.InterimaireId == interimaireId)
              .Filter(c => c.DateDebut <= datePointage && c.DateFin >= datePointage)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }

        /// <summary>
        /// Permet d'ajouter un contrat à un personnel intérimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Intérimaire</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        public ContratInterimaireEnt AddContratInterimaire(ContratInterimaireEnt contratInterimaireEnt)
        {
            contratInterimaireEnt.DateDebut = contratInterimaireEnt.DateDebut.Date;
            contratInterimaireEnt.DateFin = contratInterimaireEnt.DateFin.Date;
            contratInterimaireEnt.ZonesDeTravail = null;
            Insert(contratInterimaireEnt);

            return contratInterimaireEnt;
        }

        /// <summary>
        /// Permet de modifier un contrat d'intérimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Intérimaire</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        public ContratInterimaireEnt UpdateContratInterimaire(ContratInterimaireEnt contratInterimaireEnt)
        {
            contratInterimaireEnt.DateDebut = contratInterimaireEnt.DateDebut.Date;
            contratInterimaireEnt.DateFin = contratInterimaireEnt.DateFin.Date;
            contratInterimaireEnt.ZonesDeTravail = null;
            Update(contratInterimaireEnt);

            return contratInterimaireEnt;
        }

        /// <summary>
        /// Permet de supprimer un contrat d'intérimaire en fonction de son identifiant
        /// </summary>
        /// <param name="id">Identifiant unique d'une délégation</param>
        public void DeleteContratInterimaireById(int id)
        {
            DeleteById(id);
        }

        /// <summary>
        /// Retourne la liste des Ci disponibles pour un intérimaire et une date donnée
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="date">Date</param>
        /// <returns>La liste des Ci disponibles pour un intérimaire et une date donnée</returns>
        public IEnumerable<CIEnt> GetCIList(int personnelId, DateTime date)
        {
            var contrat = this.Context.ContratInterimaires.Include(c => c.Ci)
                                                              .Include(c => c.ZonesDeTravail).ThenInclude(z => z.EtablissementComptable)
                                                              .FirstOrDefault(c => c.InterimaireId == personnelId && ((c.DateDebut <= date && date <= c.DateFin) || (!(c.DateDebut <= date && date <= c.DateFin) && c.DateFin < date && date <= c.DateFin.AddDays(c.Souplesse))));
            var ciList = new List<CIEnt>();
            var etablissementIdList = new List<int>();
            if (contrat != null)
            {
                etablissementIdList = GetListEtablissementId(contrat.ZonesDeTravail, etablissementIdList);

                foreach (var etablId in etablissementIdList)
                {
                    ciList.AddRange(this.Context.CIs.Where(c => c.EtablissementComptableId == etablId && c.DateFermeture == null || c.DateFermeture > date).ToList());
                }
                if (!ciList.Contains(contrat.Ci))
                {
                    ciList.Add(contrat.Ci);
                }
            }

            return ciList;
        }


        /// <summary>
        /// Retourne un truc
        /// </summary>
        /// <param name="zonesDeTravail">Zone de travail</param>
        /// <param name="etablissementIdList">Identifiants Etablissement</param>
        /// <returns>liste des identifiants des etablissemnt</returns>
        private List<int> GetListEtablissementId(ICollection<ZoneDeTravailEnt> zonesDeTravail, ICollection<int> etablissementIdList)
        {
            foreach (var zone in zonesDeTravail)
            {
                if (!etablissementIdList.Contains(zone.EtablissementComptableId))
                {
                    etablissementIdList.Add(zone.EtablissementComptableId);
                }
            }
            return etablissementIdList.ToList();
        }

        /// <inheritdoc />
        public DateTime? GetDateDebutNextContrat(int personnelId, DateTime date)
        {
            return Context.ContratInterimaires.Where(c => c.InterimaireId == personnelId && c.DateDebut > date).Min(c => (DateTime?)c.DateDebut);
        }

        /// <summary>
        /// Retourne une liste de libelle de ci sur lesquels l'intérimaire a été pointé lors de sa période de contrat souplesse incluse
        /// </summary>
        /// <param name="interimaireId">Identifiant Unique d'un intérimaire</param>
        /// <param name="periode">période du contrat</param>
        /// <param name="ciId">Identifiant unique d'un ci</param>
        /// <returns>liste des libelle des ci</returns>
        public List<string> GetCiInRapportLigneByDateContratInterimaire(int interimaireId, DatesMaxInterimaireModel periode, int ciId)
        {
            return Context.RapportLignes
              .Where(c => c.PersonnelId == interimaireId)
              .Where(c => c.DatePointage >= periode.dateMinContrat && c.DatePointage <= periode.dateMaxContrat)
              .Where(c => c.CiId != ciId)
              .Select(c => c.Ci.Libelle)
              .Distinct()
              .ToList();
        }

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée en comprenant la souplesse
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="date">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public List<ContratInterimaireEnt> GetContratInterimaireByDatePointageAndSouplesse(int? interimaireId, DateTime date)
        {
            date = date.Date;

            return Query()
              .Include(d => d.Interimaire)
              .Include(d => d.Fournisseur)
              .Include(d => d.Societe)
              .Include(d => d.Societe.TypeSociete)
              .Include(d => d.Ci)
              .Include(d => d.Ci.CIDevises)
              .Include(d => d.Ci.Taches)
              .Include(d => d.Ci.Societe.TypeSociete)
              .Include(d => d.Unite)
              .Include(d => d.Ressource)
              .Include(d => d.MotifRemplacement)
              .Include(d => d.PersonnelRemplace)
              .Include(d => d.ZonesDeTravail)
              .Filter(c => c.InterimaireId == interimaireId)
              .Filter(c => c.DateDebut <= date && c.DateFin.AddDays(c.Souplesse) >= date)
              .Get()
              .AsNoTracking()
              .ToList();

        }

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée pour les réception intérimaire
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="datePointage">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByDatePointageForReceptionInterimaire(int? interimaireId, DateTime datePointage)
        {
            return Query()
              .Include(d => d.Interimaire)
              .Include(d => d.Ci.Taches)
              .Filter(c => c.InterimaireId == interimaireId)
              .Filter(c => c.DateDebut <= datePointage && c.DateFin >= datePointage)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée en comprenant la souplesse pour les réception intérimaires
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="date">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByDatePointageAndSouplesseForReceptionInterimaire(int? interimaireId, DateTime date)
        {

            return Query()
              .Include(d => d.Interimaire)
              .Include(d => d.Ci.Taches)
              .Filter(c => c.InterimaireId == interimaireId)
              .Filter(c => c.DateDebut <= date && c.DateFin.AddDays(c.Souplesse) >= date)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();

        }

        /// <summary>
        /// Retourne la liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée
        /// </summary>
        /// <param name="interimaireId">Identifiant de l'intérimaire</param>
        /// <param name="period">Période</param>
        /// <returns>La liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée</returns>
        public List<ContratInterimaireEnt> GetListContratInterimaireOpenOnPeriod(int interimaireId, DateTime period)
        {
            var minDate = new DateTime(period.Year, period.Month, 1);
            var maxDate = new DateTime(period.Year, period.Month, DateTime.DaysInMonth(period.Year, period.Month));
            return Query()
            .Filter(c => c.InterimaireId == interimaireId)
            .Filter(c => minDate <= c.DateDebut
                && c.DateDebut <= maxDate
                || minDate <= c.DateFin.AddDays(c.Souplesse)
                && c.DateFin.AddDays(c.Souplesse) <= maxDate
                || c.DateDebut <= minDate
                && maxDate <= c.DateFin.AddDays(c.Souplesse))
            .Include(c => c.Fournisseur)
            .Include(c => c.Ressource)
            .Get()
            .ToList();
        }

        /// <summary>
        /// Retourne la liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée, concernant les personnels et sur un ci donné
        /// </summary>
        /// <param name="personnelIds">Les id des personnels ou l'on recherche les contrats</param>       
        /// <param name="startDate">Date de debut de la periode</param>
        /// <param name="endDate">date de fin de la periode</param>   
        /// <returns>La liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée</returns>
        public List<ContratInterimaireEnt> GetListContratInterimaireOpenOnPeriod(List<int> personnelIds, DateTime startDate, DateTime endDate)
        {
            var minDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var maxDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            return GetListContratInterimaireOpen(personnelIds, minDate, maxDate);
        }


        private List<ContratInterimaireEnt> GetListContratInterimaireOpen(List<int> personnelIds, DateTime minDate, DateTime maxDate)
        {
            return Query()
            .Include(d => d.ZonesDeTravail)
            .Filter(c => personnelIds.Contains(c.InterimaireId))
            .Filter(c => !(maxDate < c.DateDebut || minDate > c.DateFin.AddDays(c.Souplesse)))
            .Get()
            .ToList();
        }

        /// <summary>
        /// Permet de récupérer le contrat intérimaire actif en fonction d'un personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Le contrat intérimaire appartenant au personnel id</returns>
        public ContratInterimaireEnt GetContratInterimaireActifByPersonnelId(int personnelId)
        {
            return Query()
                .Filter(c => c.InterimaireId == personnelId && c.DateDebut <= DateTime.UtcNow && DateTime.UtcNow <= c.DateFin)
                .Get()
                .AsNoTracking()
                .FirstOrDefault();
        }

        /// <summary>
        /// Permet de récupérer les contrats intérimaires par date début et fin, par CiId et par Etablissement comptable Id
        /// </summary>
        /// <param name="dateDebutChantier">Date debut chantier</param>
        /// <param name="dateFinChantier">Date fin chantier</param>
        /// <returns>List des contrats intérimaires</returns>
        public List<ContratInterimaireEnt> GetListContratsInterimaires(DateTime? dateDebutChantier, DateTime? dateFinChantier)
        {
            DateTime? dateDebut = dateDebutChantier.HasValue ? dateDebutChantier.Value.Date : dateFinChantier;
            DateTime? dateFin = dateFinChantier.HasValue ? dateFinChantier.Value.Date : dateDebutChantier;
            if (!dateDebut.HasValue || !dateFin.HasValue)
            {
                return null;
            }

            return Context.ContratInterimaires
                    .Where(c => c.DateDebut.Date <= dateDebut.Value.Date && dateFin.Value.Date <= c.DateFin.AddDays(c.Souplesse).Date)
                    .Include(x => x.ZonesDeTravail)
                    .ToList();
        }

        /// <summary>
        /// Récupére le contrat intérimaire par numéro de contrat et groupe code
        /// </summary>
        /// <param name="numeroContrat">Numero de contrat</param>
        /// <param name="groupeCode">Code du groupe</param>
        /// <returns>Un contrat interimaire</returns>
        public ContratInterimaireEnt GetContratInterimaireByNumeroContratAndGroupeCode(string numeroContrat, string groupeCode)
        {
            return Query()
                .Filter(x => x.NumContrat == numeroContrat && x.Societe.Groupe.Code == groupeCode)
                .Include(x => x.ContratInterimaireImports)
                .Get()
                .AsNoTracking()
                .FirstOrDefault();
        }
    }
}
