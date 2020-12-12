using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Affectation;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;
using Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom
{
    /// <summary>
    /// Classe qui stock les infos necessaire a l'import du personnel
    /// </summary>
    public class ImportPersonnelsGlobalData
    {
        public List<RessourceEnt> RessourcesForGroupe { get; set; }
        public List<ImportPersonnelsSocieteData> SocietesData { get; internal set; }
        public List<AffectationEnt> Affectations { get; internal set; }
        public UtilisateurEnt FredIe { get; internal set; }
        public bool IsFes { get; internal set; } = false;

        /// <summary>
        /// Retourne les infos pour une societe par code société paie
        /// </summary>
        /// <param name="codeSocietePaye">codeSocietePaye</param>
        /// <returns>ImportPersonnelsSocieteData</returns>
        public ImportPersonnelsSocieteData GetSocieteData(string codeSocietePaye)
        {
            return this.SocietesData.FirstOrDefault(sd => sd.Societe.CodeSocietePaye == codeSocietePaye);
        }

        /// <summary>
        /// Retourne les infos pour une societe par code
        /// </summary>
        /// <param name="codeSociete">Code societe</param>
        /// <returns>ImportPersonnelsSocieteData</returns>
        public ImportPersonnelsSocieteData GetSocieteDataByCode(string codeSociete)
        {
            return this.SocietesData.FirstOrDefault(sd => sd.Societe.Code == codeSociete);
        }



    }
}
