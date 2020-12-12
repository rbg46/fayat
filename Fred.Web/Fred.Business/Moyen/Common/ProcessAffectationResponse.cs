using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Rapport;

namespace Fred.Business.Moyen.Common
{
    /// <summary>
    /// Cette contient le résultat du process d'une affectation de moyen
    /// </summary>
    public class ProcessAffectationResponse
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public ProcessAffectationResponse()
        {
            RapportLigneEntsToCreate = new List<RapportLigneEnt>();
            RapportLigneEntsToUpdate = new List<RapportLigneEnt>();
        }
        
        /// <summary>
        /// La liste des ligne de rapports à créer
        /// </summary>
        public List<RapportLigneEnt> RapportLigneEntsToCreate { get; set; }

        /// <summary>
        /// La liste des lignes de rapports à modifier
        /// </summary>
        public List<RapportLigneEnt> RapportLigneEntsToUpdate { get; set; }

        /// <summary>
        /// Add rapport ligne
        /// </summary>
        /// <param name="ligneEnt">Rapport ligne à ajouter</param>
        public void AddRapportLineToCreateList(RapportLigneEnt ligneEnt)
        {
            if (ligneEnt != null)
            {
                ligneEnt.RapportLigneId = 0;
                RapportLigneEntsToCreate.Add(ligneEnt);
            }
        }

        /// <summary>
        /// Ajouter la ligne du rapports aux lignes à modifier
        /// </summary>
        /// <param name="ligneEnt">Ligne de rapport</param>
        public void AddRapportLigneToUpdateList(RapportLigneEnt ligneEnt)
        {
            if (ligneEnt == null)
            {
                return;
            }

            bool existingLine = RapportLigneEntsToUpdate.Any(v => v.RapportLigneId == ligneEnt.RapportLigneId);
            if (!existingLine)
            {
                RapportLigneEntsToUpdate.Add(ligneEnt);
            }
        }
    }
}
