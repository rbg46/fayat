using System.Linq;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Database.ImportExport;
using Fred.ImportExport.Entities;

namespace Fred.ImportExport.DataAccess.WorkflowLogicielTiers
{
    public class LogicielTiersRepository : AbstractRepository<LogicielTiersEnt>, ILogicielTiersRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public LogicielTiersRepository(IUnitOfWork unitOfWork, ImportExportContext context)
            : base(context)
        {
            this.unitOfWork = unitOfWork;
        }

        public LogicielTiersEnt CreateLogicielTiers(string nomLogiciel, string nomServeur, string mandant)
        {
            Add(new LogicielTiersEnt
            {
                Mandant = mandant,
                NomLogiciel = nomLogiciel,
                NomServeur = nomServeur
            });

            unitOfWork.Save();

            return GetLogicielTiers(nomLogiciel, nomServeur, mandant);
        }

        public LogicielTiersEnt GetLogicielTiers(string nomLogiciel, string nomServeur, string mandant)
        {
            return Get()
                .SingleOrDefault(lt => lt.NomLogiciel == nomLogiciel && lt.NomServeur == nomServeur && lt.Mandant == mandant);
        }


    }
}
