using Fred.Entities.Referential;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Models.Etablissement;

namespace Fred.ImportExport.Business.Etablissement.Etl.Process
{
    public interface IEtablissementComptableEtlProcess : IEtlProcess<EtablissementComptableAnaelModel, EtablissementComptableEnt>
    {
    }
}