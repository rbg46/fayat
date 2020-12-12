using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.DatesCalendrierPaie;

namespace Fred.Business.DatesCalendrierPaie
{
    /// <summary>
    ///   Gestionnaire des DatesCalendrierPaie.
    /// </summary>
    public class DatesCalendrierPaieManager : Manager<DatesCalendrierPaieEnt, IDatesCalendrierPaieRepository>, IDatesCalendrierPaieManager
    {
        public DatesCalendrierPaieManager(IUnitOfWork uow, IDatesCalendrierPaieRepository datesCalendrierPaieRepository)
          : base(uow, datesCalendrierPaieRepository)
        {
        }

        /// <summary>
        ///   Ajoute une liste de DatesCalendrierPaieEnt en base
        /// </summary>
        /// <param name="listDcp">Une liste des calendriers mensuels</param>
        public void AddDatesCalendrierPaie(IEnumerable<DatesCalendrierPaieEnt> listDcp)
        {
            this.Repository.AddDatesCalendrierPaie(listDcp);
        }

        /// <summary>
        ///   Ajoute ou met à jour une liste de DatesCalendrierPaieEnt en fonction de leur existence
        /// </summary>
        /// <param name="listDcp">Une liste des calendriers mensuels</param>
        public void AddOrUpdateDatesCalendrierPaie(IEnumerable<DatesCalendrierPaieEnt> listDcp)
        {
            foreach (var dcp in listDcp)
            {
                AddOrUpdateDatesCalendrierPaie(dcp);
            }
        }

        /// <summary>
        ///   Retourne un objet DatesCalendrierPaieEnt avec des données à null
        /// </summary>
        /// <param name="societeId">Un id de la société</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Renvoi un calendrier vide</returns>
        public DatesCalendrierPaieEnt GetNewDatesCalendrierPaieEnt(int societeId, int year, int month)
        {
            return new DatesCalendrierPaieEnt
            {
                SocieteId = societeId,
                DateFinPointages = null,
                DateTransfertPointages = null
            };
        }

        /// <summary>
        ///   Retourne une liste de DatesCalendrierPaieEnt par société et année
        /// </summary>
        /// <param name="societeId">Un id de la société</param>
        /// <param name="year">Une année</param>
        /// <returns>Renvoi une liste de calendriers</returns>
        public IEnumerable<DatesCalendrierPaieEnt> GetSocieteListDatesCalendrierPaieByIdAndYear(int societeId, int year)
        {
            var listDcp = this.Repository.GetSocieteListDatesCalendrierPaieByIdAndYear(societeId, year);

            //listDcp est censé contenir 12 DatesCalendrierPaie (liste des calendriers par année)
            //Si listDcp ne contient pas 12 éléments, on va créer une liste de 12 élément
            if (listDcp == null || listDcp.Count() < 12)
            {
                var listDcpFull = new List<DatesCalendrierPaieEnt>();

                //On boucle sur 12 mois et on renseigne la liste avec les calendriers existant et des calendriers vide si non existants
                for (int month = 1; month <= 12; month++)
                {
                    listDcpFull.Add(listDcp.FirstOrDefault(o => o.DateFinPointages.HasValue && o.DateFinPointages.Value.Month == month) ?? GetNewDatesCalendrierPaieEnt(societeId, year, month));
                }

                return listDcpFull;
            }

            return listDcp;
        }

        /// <summary>
        ///   Retourne une liste de DatesCalendrierPaieEnt par société, année et mois
        /// </summary>
        /// <param name="societeId">Un id de la société</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Renvoi un calendrier</returns>
        public DatesCalendrierPaieEnt GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(int societeId, int year, int month)
        {
            return this.Repository.GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(societeId, year, month) ?? GetNewDatesCalendrierPaieEnt(societeId, year, month);
        }

        /// <summary>
        ///   Ajout un DatesCalendrierPaieEnt en base
        /// </summary>
        /// <param name="dcp">Un calendrier mensuel</param>
        /// <returns>Renvoi l'id technique></returns>
        public int AddDatesCalendrierPaie(DatesCalendrierPaieEnt dcp)
        {
            return this.Repository.AddDatesCalendrierPaie(dcp);
        }

        /// <summary>
        ///   Met à jour un DatesCalendrierPaieEnt en base
        /// </summary>
        /// <param name="dcp">Un calendrier mensuel</param>
        public void UpdateDatesCalendrierPaie(DatesCalendrierPaieEnt dcp)
        {
            this.Repository.UpdateDatesCalendrierPaie(dcp);
        }

        /// <summary>
        ///   Ajoute ou met à jour un DatesCalendrierPaieEnt en fonction de son existence
        /// </summary>
        /// <param name="dcp">Un calendrier mensuel</param>
        public void AddOrUpdateDatesCalendrierPaie(DatesCalendrierPaieEnt dcp)
        {
            //On test l'IDENTITY pour savoir si c'est nouevel élément ou non
            if (dcp.DatesCalendrierPaieId == 0)
            {
                this.Repository.AddDatesCalendrierPaie(dcp);
            }
            else
            {
                this.Repository.UpdateDatesCalendrierPaie(dcp);
            }
        }

        /// <summary>
        ///   Supprime tout le paramétrage d'une année pour une société
        /// </summary>
        /// <param name="societeId">Un id de la société</param>
        /// <param name="year">Une année</param>
        public void DeleteSocieteDatesCalendrierPaieByIdAndYear(int societeId, int year)
        {
            this.Repository.DeleteSocieteDatesCalendrierPaieByIdAndYear(societeId, year);
        }

        /// <summary>
        ///   Renvoi vrai si la date de fin de pointage est inférieure à la date de transfert des pointages
        /// </summary>
        /// <param name="dcp">Un calendrier mensuel</param>
        /// <returns>Renvoi vrai si les dates sont bonnes</returns>
        public bool IsDateFinPtgLowerThanDateTransfertPtg(DatesCalendrierPaieEnt dcp)
        {
            if (dcp.DateFinPointages == null || dcp.DateTransfertPointages == null)
            {
                return true;
            }

            return dcp.DateFinPointages < dcp.DateTransfertPointages;
        }
    }
}