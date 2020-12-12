using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel.Interimaire;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Personnel.Interimaire
{
    /// <summary>
    ///   Référentiel de données pour les zones de travail
    /// </summary>
    public class ZoneDeTravailRepository : FredRepository<ZoneDeTravailEnt>, IZoneDeTravailRepository
    {

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ZoneDeTravailRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public ZoneDeTravailRepository(FredDbContext context)
        : base(context) { }

        /// <summary>
        /// Permet de récupérer une liste des zones de travail en fonction d'un contrat id
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat</param>
        /// <returns>Liste des zones de travail</returns>
        public List<ZoneDeTravailEnt> GetZoneDeTravailByContratId(int contratInterimaireId)
        {
            return Query()
              .Include(d => d.Contrat)
              .Include(d => d.EtablissementComptable)
              .Get()
              .Where(d => d.ContratInterimaireId == contratInterimaireId)
              .ToList();
        }

        /// <summary>
        /// Permet de récupérer une liste des zones de travail en fonction d'un contrat id
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat</param>
        /// <returns>Liste des zones de travail sans objets complexes</returns>
        public List<ZoneDeTravailEnt> GetOnlyZonesDeTravailListByContratId(int contratInterimaireId)
        {
            return Context
                .ZonesDeTravail
                .Where(d => d.ContratInterimaireId == contratInterimaireId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Permet d'ajouter un zone de travail
        /// </summary>
        /// <param name="etablissementComptableId">Identifiant unique d'un établissement comptable</param>
        /// <param name="contratInterimaireId">Identifiant unique d'un Contrat Intérimaire</param>
        /// <returns>Zone de Travail enregistré</returns>
        public ZoneDeTravailEnt AddZoneDeTravail(int? etablissementComptableId, int contratInterimaireId)
        {

            ZoneDeTravailEnt zoneDeTravailEnt = new ZoneDeTravailEnt();
            zoneDeTravailEnt.EtablissementComptableId = (int)etablissementComptableId;
            zoneDeTravailEnt.ContratInterimaireId = contratInterimaireId;

            Insert(zoneDeTravailEnt);

            return zoneDeTravailEnt;
        }

        /// <summary>
        /// Permet de supprimer un contrat d'intérimaire en fonction de son identifiant
        /// </summary>
        /// <param name="zoneDeTravailEnt">Zone de travail</param>
        public void DeleteZoneDeTravail(ZoneDeTravailEnt zoneDeTravailEnt)
        {
            Delete(zoneDeTravailEnt);
        }

        /// <summary>
        /// Permet de supprimer les zone de travail
        /// </summary>
        /// <param name="contratInterimaireId">Liste des zones de travail</param>
        public void DeleteZonesDeTravailList(List<ZoneDeTravailEnt> zonesDeTravail)
        {
            if (zonesDeTravail != null)
            {
                foreach (var zone in zonesDeTravail)
                {
                    Delete(zone);
                }
            }
        }

        /// <summary>
        /// Retourne les organisations (Etablissement) appartenant lié à un contrat intérimaire pour picklist
        /// </summary>
        /// <param name="contratInterimaireId">identifiant unique du contrat intérimaire</param>
        /// <returns>Liste des zones de travail appartenant un contrat intérimaire</returns>
        public IEnumerable<ZoneDeTravailEnt> SearchLightForContratInterimaireId(int contratInterimaireId)
        {
            return Query()
              .Include(z => z.EtablissementComptable)
              .Include(z => z.EtablissementComptable.Organisation)
              .Include(z => z.EtablissementComptable.Organisation.TypeOrganisation)
              .Filter(p => p.ContratInterimaireId == contratInterimaireId)
              .OrderBy(p => p.OrderBy(l => l.EtablissementComptable.Code))
              .Get()
              .ToList();

        }

    }
}
