using Fred.Business.Personnel;
using Fred.ImportExport.Business.Personnel.Etl.Process;
using Fred.ImportExport.Business.Personnel.Etl.Process.Custom;

namespace Fred.ImportExport.Business.Personnel.EtlFactory
{
    public class PersonnelEtlFactory
    {
        private readonly IImportPersonnelManager importPersonnelFesManager;
        private readonly IPersonnelManager personnelManager;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="importPersonnelFesManager">import personnel Manager</param>
        /// <param name="personnelManager">personnel Manager fred web</param>
        public PersonnelEtlFactory(IImportPersonnelManager importPersonnelFesManager, IPersonnelManager personnelManager)
        {
            this.importPersonnelFesManager = importPersonnelFesManager;
            this.personnelManager = personnelManager;
        }

        /// <summary>
        /// Retourne un etl en fonction de sont codeFlux
        /// </summary>
        /// <param name="correspondance">correspondance</param>
        /// /// <param name="parameter">etl parametre</param>
        /// <returns>Un etl</returns>
        public IPersonnelEtl GetEtl(FluxPersonnelCorrespondance correspondance, PersonnelEtlParameter parameter)
        {
            if (correspondance.CodeFlux == PersonnelFluxCode.CodeFluxFes)
            {
                return new PersonnelProcessFes(importPersonnelFesManager, personnelManager, parameter) as IPersonnelEtl;
            }
            else if (correspondance.CodeFlux == PersonnelFluxCode.CodeFluxFon)
            {
                return new PersonnelProcessFon(importPersonnelFesManager, personnelManager, parameter) as IPersonnelEtl;
            }

            return null;
        }
    }
}
