using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Rapport;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Moq;

namespace Fred.Common.Tests.Data.Rapport.Mock
{
    /// <summary>
    /// Classe static des éléments fictives du rapport
    /// </summary>
    public class RapportMocks
    {
        /// <summary>
        /// Ontient la liste fictives des sociétés
        /// </summary>
        public List<RapportEnt> RapportsStub => this.GetFakeDbSet().ToList();

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de RapportLigneEnt</returns>
        public FakeDbSet<RapportEnt> GetFakeDbSet()
        {
            return new FakeDbSet<RapportEnt>
            {
                new RapportEnt{
                    CiId = 1,
                    RapportId = 1,
                    RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key,
                    TypeRapport = (int)TypeRapport.Journee,
                    TypeRapportEnum = TypeRapport.Journee,
                    ValidationSuperieur = true,
                    CI = new Entities.CI.CIEnt()
                    {
                      Societe = new Entities.Societe.SocieteEnt()
                      {
                        Groupe = new Entities.Groupe.GroupeEnt()
                        { Code = Fred.Entities.Constantes.CodeGroupeFES }
                      }
                    },
                    DateChantier = DateTime.UtcNow,
                    HoraireDebutS = DateTime.UtcNow,
                    HoraireFinS = DateTime.UtcNow,
                    ListLignes = new List<RapportLigneEnt>() { new RapportLigneEnt() {RapportId = 1, ListErreurs = new List<string>() } }
                },
                new RapportEnt{
                    CiId = 2,
                    RapportId = 2,
                    RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key,
                    TypeRapport = (int)TypeRapport.Journee,
                    TypeRapportEnum = TypeRapport.Journee,
                    ValidationSuperieur = true
                },
                new RapportEnt{
                    CiId = 3,
                    RapportId = 3,
                    RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key,
                    TypeRapport = (int)TypeRapport.Journee,
                    TypeRapportEnum = TypeRapport.Journee,
                    ValidationSuperieur = true
                },
                new RapportEnt{
                    CiId = 4,
                    RapportId = 4,
                    RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key,
                    TypeRapport = (int)TypeRapport.Journee,
                    TypeRapportEnum = TypeRapport.Journee,
                    ValidationSuperieur = true
                }
            };
        }

        /// <summary>
        /// Obtient un Validator fictif
        /// </summary>
        /// <returns>Validator</returns>
        public Mock<IRapportValidator> GetFakeValidator()
        {
            var validatorMock = new Mock<IRapportValidator>();
            //setup

            return validatorMock;
        }

        /// <summary>
        /// Obtient un repo fictif
        /// </summary>
        /// <returns>repo fictif</returns>
        public Mock<IRapportRepository> GetFakeRepository()
        {
            var repoMock = new Mock<IRapportRepository>();
            //setup

            return repoMock;
        }

        /// <summary>
        /// Obtient un unit of work fictif
        /// </summary>
        /// <returns>unit of work fictif</returns>
        public Mock<IUnitOfWork> GetFakeUow()
        {
            var uowMock = new Mock<IUnitOfWork>();

            return uowMock;
        }
    }
}
