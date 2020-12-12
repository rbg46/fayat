using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation;
using Fred.Entities.Personnel.Interimaire;
using Fred.Framework.Exceptions;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///   Gestionnaire des zones de travail
    /// </summary>
    public class ZoneDeTravailManager : Manager<ZoneDeTravailEnt, IZoneDeTravailRepository>, IZoneDeTravailManager
    {
        public ZoneDeTravailManager(IUnitOfWork uow, IZoneDeTravailRepository zoneDeTravailRepository)
         : base(uow, zoneDeTravailRepository)
        { }

        /// <summary>
        /// Permet de récupérer une liste des zones de travail en fonction d'un contrat id
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat</param>
        /// <returns>Liste des zones de travail</returns>
        public List<ZoneDeTravailEnt> GetZoneDeTravailByContratId(int contratInterimaireId)
        {
            try
            {
                return Repository.GetZoneDeTravailByContratId(contratInterimaireId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }


        /// <summary>
        /// Permet d'ajouter un zone de travail
        /// </summary>
        /// <param name="zoneDeTravailEnt">Zone De Travail</param>
        /// <returns>Zone de Travail enregistré</returns>
        public ZoneDeTravailEnt AddZoneDeTravail(ZoneDeTravailEnt zoneDeTravailEnt)
        {
            try
            {
                Repository.AddZoneDeTravail(zoneDeTravailEnt.EtablissementComptableId, zoneDeTravailEnt.ContratInterimaireId);
                Save();

                return zoneDeTravailEnt;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de supprimer un contrat d'intérimaire en fonction de son identifiant
        /// </summary>
        /// <param name="zoneDeTravailEnt">Zone de travail</param>
        public void DeleteZoneDeTravail(ZoneDeTravailEnt zoneDeTravailEnt)
        {
            try
            {
                Repository.DeleteZoneDeTravail(zoneDeTravailEnt);
                Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de supprimer les zone de travail d'un contrat interimaire
        /// </summary>
        /// <param name="contratInterimaireId">Contrat interimaire Id</param>
        public void DeleteZonesDeTravailByContratInterimaire(int contratInterimaireId)
        {
            try
            {
                var zoneDeTravailList = Repository.GetOnlyZonesDeTravailListByContratId(contratInterimaireId);
                Repository.DeleteZonesDeTravailList(zoneDeTravailList);
                Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne les organisations (Etablissement et Ci) appartenant lié à un contrat intérimaire pour picklist
        /// </summary>
        /// <param name="contratInterimaireId">identifiant unique du contrat intérimaire</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">taille de la page</param>
        /// <param name="recherche">texte de recherche</param>
        /// <returns>Liste des organisations appartenant à un contrat intérimaire</returns>
        public IEnumerable<OrganisationEnt> SearchLightForContratInterimaireId(int contratInterimaireId, int page = 1, int pageSize = 20, string recherche = "")
        {
            try
            {
                IEnumerable<OrganisationEnt> etablissements = Repository.SearchLightForContratInterimaireId(contratInterimaireId).Select(o => o.EtablissementComptable.Organisation);
                IEnumerable<OrganisationEnt> cis = new List<OrganisationEnt>();
                IEnumerable<OrganisationEnt> organisations;

                foreach (var etablissement in etablissements)
                {
                    cis = cis.Concat(Managers.CI.SearchLightOrganisationCiByOrganisationPereId(etablissement.OrganisationId, page, pageSize));
                }
                if (page == 1)
                {
                    organisations = etablissements.Concat(cis);
                }
                else
                {
                    organisations = cis;
                }
                organisations = organisations.Where(p => string.IsNullOrEmpty(recherche) || p.Code.ToLower().Contains(recherche.ToLower()) || p.Libelle.ToLower().Contains(recherche.ToLower())
                              || p.TypeOrganisation.Libelle.ToLower().Contains(recherche.ToLower()));

                return organisations;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

        }
    }
}
