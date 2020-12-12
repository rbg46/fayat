using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.Business.Personnel.Logs;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.Business.Personnel.Etl.Process
{
    public class PersonnelEtlParameter
    {
        public PersonnelEtlParameter(bool bypassDate, FluxEnt flux, string sqlScriptPath)
        {
            if (sqlScriptPath == null)
            {
                throw new System.ArgumentNullException(nameof(sqlScriptPath));
            }
            if (flux == null)
            {
                throw new System.ArgumentNullException(nameof(flux));
            }

            ByPassDate = bypassDate;
            SqlScriptPath = sqlScriptPath;
            Flux = flux;
            if (Flux.Code == PersonnelFluxCode.CodeFluxFon)
            {
                Logger = new ImportPersonnelFonLogs();
            }
            else
            {
                Logger = new ImportPersonnelFesLogs();
            }
        }


        /// <summary>
        /// C'est le chemin du script SQL a jouer pour la recuperation des données ANAEL
        /// </summary>
        public string SqlScriptPath { get; set; }

        /// <summary>
        /// C'est le Code du flux 
        /// exemple : PERSONNEL_RZB
        /// </summary>
        public string CodeFlux
        {
            get
            {
                return Flux.Code;

            }
        }
        /// <summary>
        /// ByPassDate
        /// </summary>
        public bool ByPassDate { get; set; }

        /// <summary>
        /// Flux
        /// </summary>
        public FluxEnt Flux { get; }

        /// <summary>
        /// Personnel Logger
        /// </summary>
        public IImportPersonnelLogs Logger { get; private set; }
    }
}
