using Fred.Business.Rapport.Pointage.PointagePersonnel;
using Fred.Business.Rapport.Pointage.PointagePersonnel.Interfaces;
using Fred.Business.Rapport.Pointage.Validation;
using Fred.Business.Rapport.Pointage.Validation.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fes.RemonteeVrac;
using Fred.ImportExport.Business.ValidationPointage.Fes.RemonteeVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fon.Common;
using Fred.ImportExport.Business.ValidationPointage.Fon.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Fon.ControleVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac;
using Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Common.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac;
using Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac;
using Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac.Interfaces;

namespace Fred.DesignPatterns.DI.Transversal
{
    public class ImportExportBusinessHelpersRegistrar : DependencyRegistrar
    {
        public ImportExportBusinessHelpersRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            // Validation Pointage Common
            DependencyInjectionService.RegisterType<IControlePointageProvider, ControlePointageProvider>();
            DependencyInjectionService.RegisterType<IEtablissementFormator, EtablissementFormator>();
            DependencyInjectionService.RegisterType<IRemonteeVracProvider, RemonteeVracProvider>();
            DependencyInjectionService.RegisterType<IValidationPointageContextDataProvider, ValidationPointageContextDataProvider>();

            // Validation Pointage FES
            DependencyInjectionService.RegisterType<FesQueryExecutor, FesQueryExecutor>();
            DependencyInjectionService.RegisterType<IControleVracFesHangFireJob, ControleVracFesHangFireJob>();
            DependencyInjectionService.RegisterType<IControleVracFesQueryBuilder, ControleVracFesQueryBuilder>();
            DependencyInjectionService.RegisterType<IControleVracFesQueryExecutor, ControleVracFesQueryExecutor>();
            DependencyInjectionService.RegisterType<IFesQueryBuilder, FesQueryBuilder>();
            DependencyInjectionService.RegisterType<IRemonteeVracFesHangFireJob, RemonteeVracFesHangFireJob>();
            DependencyInjectionService.RegisterType<IRemonteeVracFesQueryBuilder, RemonteeVracFesQueryBuilder>();
            DependencyInjectionService.RegisterType<IRemonteeVracFesQueryExecutor, RemonteeVracFesQueryExecutor>();

            // Validation Pointage FAYAT TP
            DependencyInjectionService.RegisterType<IControleVracFtpHangFireJob, ControleVracFtpHangFireJob>();
            DependencyInjectionService.RegisterType<IControleVracFtpQueryBuilder, ControleVracFtpQueryBuilder>();
            DependencyInjectionService.RegisterType<IControleVracFtpQueryExecutor, ControleVracFtpQueryExecutor>();
            DependencyInjectionService.RegisterType<IFtpQueryBuilder, FtpQueryBuilder>();
            DependencyInjectionService.RegisterType<IFtpQueryExecutor, FtpQueryExecutor>();
            DependencyInjectionService.RegisterType<IRemonteeVracFtpHangFireJob, RemonteeVracFtpHangFireJob>();
            DependencyInjectionService.RegisterType<IRemonteeVracFtpQueryBuilder, RemonteeVracFtpQueryBuilder>();
            DependencyInjectionService.RegisterType<IRemonteeVracFtpQueryExecutor, RemonteeVracFtpQueryExecutor>();

            // Validation Pointage FONDATION
            DependencyInjectionService.RegisterType<IControleVracFonHangFireJob, ControleVracFonHangFireJob>();
            DependencyInjectionService.RegisterType<IControleVracFonQueryBuilder, ControleVracFonQueryBuilder>();
            DependencyInjectionService.RegisterType<IControleVracFonQueryExecutor, ControleVracFonQueryExecutor>();
            DependencyInjectionService.RegisterType<IFonQueryBuilder, FonQueryBuilder>();
            DependencyInjectionService.RegisterType<IFonQueryExecutor, FonQueryExecutor>();
            DependencyInjectionService.RegisterType<IRemonteeVracFonHangFireJob, RemonteeVracFonHangFireJob>();
            DependencyInjectionService.RegisterType<IRemonteeVracFonQueryBuilder, RemonteeVracFonQueryBuilder>();
            DependencyInjectionService.RegisterType<IRemonteeVracFonQueryExecutor, RemonteeVracFonQueryExecutor>();

            // Dependences de IPointageManager
            DependencyInjectionService.RegisterType<IPointagePersonnelGlobalDataProvider, PointagePersonnelGlobalDataProvider>();
            DependencyInjectionService.RegisterType<IPointagePersonnelTransformerForViewService, PointagePersonnelTransformerForViewService>();
            DependencyInjectionService.RegisterType<IRapportLigneErrorBuilder, RapportLigneErrorBuilder>();
            DependencyInjectionService.RegisterType<IRapportLignesValidationDataProvider, RapportLignesValidationDataProvider>();
        }
    }
}
