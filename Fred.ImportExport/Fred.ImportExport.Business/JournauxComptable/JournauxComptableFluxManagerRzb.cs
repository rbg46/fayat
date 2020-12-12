using System.Configuration;
using Fred.Business.Journal;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.ImportExport.Business.Flux;

namespace Fred.ImportExport.Business.JournauxComptable
{
    public class JournauxComptableFluxManagerRzb : JournauxComptableFluxManager
    {
        public override string FluxCode => ConfigurationManager.AppSettings["flux:journaux:comptable:rzb"];

        public JournauxComptableFluxManagerRzb(
            IFluxManager fluxManager,
            ISocieteManager societeManager,
            IJournalManager journalComptableManager,
            IUtilisateurManager utilisateurManager)
            : base(fluxManager, societeManager, journalComptableManager, utilisateurManager)
        {
        }
    }
}
