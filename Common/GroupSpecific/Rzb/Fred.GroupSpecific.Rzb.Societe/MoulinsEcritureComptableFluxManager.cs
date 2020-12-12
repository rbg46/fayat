using System.Configuration;
using Fred.Business.CI;
using Fred.Business.EcritureComptable.Import;
using Fred.Business.Notification;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.OperationDiverse;
using Fred.ImportExport.DataAccess.Interfaces;

namespace Fred.GroupSpecific.Rzb.Societe
{
    public class MoulinsEcritureComptableFluxManager : EcritureComptableFluxManager
    {
        private static readonly string ChaineConnexionAnaelMoulins = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];
        public static string ImportJobIdMoulins => ConfigurationManager.AppSettings["flux:ecriture:comptable:moulins"];

        public MoulinsEcritureComptableFluxManager(
            IFluxManager fluxManager,
            IEcritureComptableImportManager ecritureComptableImportManager,
            ICIManager ciManager,
            ISocieteManager societeManager,
            INotificationManager notificationManager,
            IGroupeRepository groupRepository,
            ISocieteRepository societeRepository,
            IFluxRepository fluxRepository)
            : base(fluxManager, ChaineConnexionAnaelMoulins, ecritureComptableImportManager, ciManager, societeManager, notificationManager, groupRepository, societeRepository, fluxRepository)
        {
            Flux = FluxManager.GetByCode(ImportJobIdMoulins);
        }
    }
}
