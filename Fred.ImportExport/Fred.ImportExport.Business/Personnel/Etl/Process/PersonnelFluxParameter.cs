using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.Business.Personnel.Etl.Process
{
    public class PersonnelFluxParameter : IPersonnelEtlParameter
    {
        private FluxEnt flux;

        public PersonnelFluxParameter(string codeSocietePaie, bool bypassDate, string codeFlux, string domain, string sqlScriptPath, IFluxManager fluxManager)
        {

            if (codeFlux == null)
            {
                throw new System.ArgumentNullException(nameof(codeFlux));
            }

            if (codeSocietePaie == null)
            {
                throw new System.ArgumentNullException(nameof(codeSocietePaie));
            }

            if (domain == null)
            {
                throw new System.ArgumentNullException(nameof(domain));
            }

            if (sqlScriptPath == null)
            {
                throw new System.ArgumentNullException(nameof(sqlScriptPath));
            }

            FluxManager = fluxManager;
            CodeSocietePaie = codeSocietePaie;
            ByPassDate = bypassDate;
            CodeFlux = codeFlux;
            Domain = domain;
            SqlScriptPath = sqlScriptPath;
        }


        /// <summary>
        /// C'est le chemin du script SQL a jouer pour la recuperation des données ANAEL
        /// </summary>
        public string SqlScriptPath { get; set; }

        /// <summary>
        /// C'est le Code du flux 
        /// exemple : PERSONNEL_RZB
        /// </summary>
        public string CodeFlux { get; set; }

        /// <summary>
        /// C'est le code de la CodeSocietePaye de la table societe
        /// Attention le flux peux avoir plusieur valeur, par contre cette classe ne contient qu'un element.
        /// </summary>
        public string CodeSocietePaie { get; set; }


        /// <summary>
        /// ByPassDate
        /// </summary>
        public bool ByPassDate { get; set; }


        /// <summary>
        /// Le domaine est utile pour créer l'adresse email de l'utilisateur 
        /// exemple : "@razel-bec.fayat.com"
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// FluxManager
        /// </summary>
        public IFluxManager FluxManager { get; set; }

        /// <summary>
        /// Flux
        /// </summary>
        public FluxEnt Flux
        {
            get
            {
                if (flux != null)
                {
                    return flux;
                }
                flux = FluxManager.GetByCode(CodeFlux);
                return flux;
            }
        }

    }
}
