using System;
using System.Collections.Generic;
using AutoMapper;
using Fred.Business.Rapport.Pointage;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Common;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Moyen;
using Fred.ImportExport.Models.Rapport;
using Fred.Web.Shared.Models.Rapport;
using Moq;

namespace Fred.Common.Tests.Data.Rapport.Pointage.Mock
{
    /// <summary>
    /// Classe static des éléments fictives du pointage
    /// </summary>
    public class PointageFakes
    {
        public IEnumerable<RapportLigneEnt> PointagesExpected => pointagesExpected ?? (pointagesExpected = GetFakeDbSet());

        private IEnumerable<RapportLigneEnt> pointagesExpected;

        /// <summary>
        /// ctor
        /// </summary>
        public PointageFakes()
        {
        }

        /// <summary>
        /// personnel fictif
        /// </summary>
        private PersonnelEnt fakePersonnel;
        /// <summary>
        /// Obtient un des personnels du rapport
        /// </summary>
        public PersonnelEnt FakePersonnel
        {
            get
            {
                return fakePersonnel ?? (fakePersonnel = new PersonnelEnt { PersonnelId = 1, Nom = "3-Contrats", Prenom = "Intérimaire" });
            }
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de RapportLigneEnt</returns>
        public FakeDbSet<RapportLigneEnt> GetFakeDbSet()
        {
            return new FakeDbSet<RapportLigneEnt>
            {
                new RapportLigneEnt{
                    CiId = 1,
                    PersonnelId = FakePersonnel.PersonnelId,
                    Personnel = FakePersonnel,
                    DatePointage = new DateTime(2019, 06, 05)
                },
                new RapportLigneEnt{
                    CiId = 1,
                    PersonnelId = FakePersonnel.PersonnelId,
                    Personnel = FakePersonnel,
                    DatePointage = new DateTime(2019, 06, 15)
                },
                new RapportLigneEnt{
                    CiId = 1,
                    PersonnelId = FakePersonnel.PersonnelId,
                    Personnel = FakePersonnel,
                    DatePointage = new DateTime(2019, 06, 25)
                }
            };
        }

        public RapportLigneEnt FakePointageHorsContrats()
        {
            return new RapportLigneEnt
            {
                CiId = 1,
                PersonnelId = FakePersonnel.PersonnelId,
                Personnel = FakePersonnel,
                DatePointage = new DateTime(2019, 05, 02)
            };
        }

        /// <summary>
        /// Obtient un Validator fictif
        /// </summary>
        /// <returns>Validator</returns>
        public Mock<IPointageValidator> GetFakeValidator()
        {
            var validatorMock = new Mock<IPointageValidator>();
            //setup

            return validatorMock;
        }

        /// <summary>
        /// Obtient un repo fictif
        /// </summary>
        /// <returns>repo fictif</returns>
        public Mock<IPointageRepository> GetFakeRepository()
        {
            var repoMock = new Mock<IPointageRepository>();
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

        /// <summary>
        /// Retourne une liste de pointages pour une semaine à partir d'une date de début
        /// </summary>
        /// <param name="startDate">date de début de semaine</param>
        /// <returns>Liste de RapportLigne</returns>
        public List<RapportLigneEnt> GetFakePointagesSemaine(DateTime startDate, List<int> ciIds)
        {
            var rapportLignes = new List<RapportLigneEnt>();

            for (int day = 0; day < 6; day++)
            {
                foreach (var ciId in ciIds)
                {
                    rapportLignes.Add(new RapportLigneEnt
                    {
                        CiId = ciId,
                        DatePointage = startDate.AddDays(day),
                        ListRapportLigneMajorations = new List<RapportLigneMajorationEnt> {
                                    new RapportLigneMajorationEnt {
                                        CodeMajoration = new CodeMajorationEnt {
                                            Code = "TNH1"
                                        }
                                    }
                        }

                    });
                }
            }
            return rapportLignes;

        }

        private IMapper fakeMapper;

        /// <summary>
        /// Obtient ou définit l'automapper
        /// </summary>
        public IMapper FakeMapper
        {
            get { return fakeMapper ?? (fakeMapper = GetMapperConfig().CreateMapper()); }
            set { fakeMapper = value; }
        }

        private MapperConfiguration GetMapperConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EnvoiPointageMoyenResultModel, EnvoiPointageMoyenResult>().ReverseMap();
                cfg.CreateMap<ExportPointagePersonnelFilterModel, ExportPointagePersonnelFilterModel>().ReverseMap();
            });
        }
    }
}
