using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Fred.Business.CI;
using Fred.DataAccess.Interfaces;
using Fred.Entities.VerificationPointage;

namespace Fred.Business.VerificationPointage
{
    /// <summary>
    ///   Inerface Gestionnaire Checking Pointing
    /// </summary>
    public class ChekingPointingManager : IChekingPointingManager
    {
        private readonly IChekingPointingReposity chekingPointingReposity;
        private readonly ICIManager ciManager;
        private IEnumerable<ChekingPointing> checkPointing;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ChekingPointingManager" />.
        /// </summary>    
        /// <param name="chekingPointingReposity">Reposity ChekingPointage</param>
        /// <param name="ciManager">Manager CI</param>
        public ChekingPointingManager(
            IChekingPointingReposity chekingPointingReposity,
            ICIManager ciManager
            )
        {
            this.chekingPointingReposity = chekingPointingReposity;
            this.ciManager = ciManager;
        }

        /// <summary>
        /// Ramener lees données dont on a besoin 
        /// </summary>
        /// <param name="param">Liste des filtres </param>
        /// <returns>une liste des données </returns>
        public byte[] ChekingPointing(FilterChekingPointing param)
        {
            //ramène Tous les donénes dont on a besoin
            checkPointing = chekingPointingReposity.GetChekingPointing(param);
            // ramène les données à plat avec un dictionnaire de jours de 1 au 31 
            List<ChekingPointingMonth> chekingPointingMonths = QueryModelchekingpointingResult(param.TypePointing);
            ChekingpointingReport reportchekingpointing = new ChekingpointingReport(ciManager);
            return reportchekingpointing.ExportInExcelChekingPointing(chekingPointingMonths,param) ;
        }

        private List<ChekingPointingMonth> QueryModelchekingpointingResult(int display)
        {
            switch (display)
            {
                case (int)DisplayTypePointing.Personnels:
                    return checkPointing.Where(x => !x.HeureTravail.Equals(0))
                        .GroupBy(x => new { x.InfoPerso, x.InfoCi })
                        .Select(x => new ChekingPointingMonth
                        {
                            InfoLabelle = x.Key.InfoPerso,
                            InfoCI = x.Key.InfoCi,
                            DayWorks = x.GroupBy(gp => gp.DateChantier.Day).Select(s => new { key = s.Key, valeur = s.Sum(tt => tt.HeureTravail) }).ToDictionary(v => v.key, v => v.valeur)
                        }).ToList();

                default:

                    return checkPointing.Select(x => new
                    {
                        datarow = new List<Tuple<DateTime, string, string, string, double>>()
                        {
                            Tuple.Create(x.DateChantier,x.InfoMateriel,x.InfoCi, "Marche", x.MaterielMarche),
                            Tuple.Create(x.DateChantier,x.InfoMateriel,x.InfoCi, "Panne", x.MaterielPanne),
                            Tuple.Create(x.DateChantier,x.InfoMateriel,x.InfoCi, "Arret", x.MaterielArret),
                            Tuple.Create(x.DateChantier,x.InfoMateriel,x.InfoCi, "Intemperie", x.MaterielIntemperie),
                        }
                    })
                    //get one result list
                    .SelectMany(x => x.datarow)
                    //group data by infoMateriel,infoCI,EtatMachine
                    .GroupBy(x => new { x.Item2, x.Item3, x.Item4 })
                    .Select(grp => new ChekingPointingMonth
                    {
                        InfoLabelle = grp.Key.Item2,
                        InfoCI = grp.Key.Item3,
                        EtatMachine = grp.Key.Item4,
                        DayWorks = grp.GroupBy(gp => gp.Item1.Day).Select(s => new { key = s.Key, valeur = s.Sum(tt => tt.Item5) }).ToDictionary(v => v.key, v => v.valeur)
                    }).ToList();
            }
        }
    }
}
