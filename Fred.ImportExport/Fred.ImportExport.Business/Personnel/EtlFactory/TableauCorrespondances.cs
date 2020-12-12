using System.Collections.Generic;
using System.Linq;

namespace Fred.ImportExport.Business.Personnel.EtlFactory
{
    public static class TableauCorrespondances
    {
        private static readonly List<FluxPersonnelCorrespondance> Mapping = new List<FluxPersonnelCorrespondance>()
            {
                  new FluxPersonnelCorrespondance()
                  {
                    CodeFlux = PersonnelFluxCode.CodeFluxRzb ,
                    Domaine = "@razel-bec.fayat.com",
                    SqlScriptPath ="Personnel.SELECT_PERSONNEL.sql",
                  },
                   new FluxPersonnelCorrespondance()
                  {
                    CodeFlux =  PersonnelFluxCode.CodeFluxGrzb ,
                    Domaine = string.Empty,
                    SqlScriptPath ="Personnel.SELECT_PERSONNEL_GRZB.sql",
                  },
                   new FluxPersonnelCorrespondance()
                  {
                    CodeFlux =  PersonnelFluxCode.CodeFluxFes ,
                    Domaine = string.Empty,
                    SqlScriptPath ="Personnel.SELECT_PERSONNEL_FES.sql",
                  },
                    new FluxPersonnelCorrespondance()
                  {
                    CodeFlux =  PersonnelFluxCode.CodeFluxFtp ,
                    Domaine = string.Empty,
                    SqlScriptPath ="Personnel.SELECT_PERSONNEL_FTP.sql",
                  },
                    new FluxPersonnelCorrespondance()
                  {
                    CodeFlux =  PersonnelFluxCode.CodeFluxFon ,
                    Domaine = string.Empty,
                    SqlScriptPath ="Personnel.SELECT_PERSONNEL_FON.sql",
                  }
            };
        public static FluxPersonnelCorrespondance GetCorrespondance(string codeFlux, string codeSocietePaie)
        {

            if (string.IsNullOrEmpty(codeSocietePaie))
            {
                return Mapping.FirstOrDefault(x => x.CodeFlux == codeFlux);
            }
            else
            {
                if (codeFlux == PersonnelFluxCode.CodeFluxGrzb && codeSocietePaie == "MTP")
                {
                    return new FluxPersonnelCorrespondance()
                    {
                        CodeFlux = PersonnelFluxCode.CodeFluxGrzb,
                        Domaine = "@moulin-btp.fr",
                        SqlScriptPath = "Personnel.SELECT_PERSONNEL_GRZB.sql",
                    };
                }
                else
                {
                    return Mapping.FirstOrDefault(x => x.CodeFlux == codeFlux);
                }

            }

        }
    }
}
