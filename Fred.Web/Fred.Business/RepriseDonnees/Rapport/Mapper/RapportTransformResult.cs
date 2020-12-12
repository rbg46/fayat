using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities.Rapport;

namespace Fred.Business.RepriseDonnees.Rapport.Mapper
{
    /// <summary>
    /// Container du resultat de la creation des rapports lignes et les rapport ligne taches
    /// </summary>
    public class RapportTransformResult
    {
        /// <summary>
        /// Contient les rapports  créés
        /// </summary>
        public List<RapportEnt> Rapports { get; internal set; } = new List<RapportEnt>();

        /// <summary>
        /// Contient les rapports lignes créés
        /// </summary>
        public List<RapportLigneEnt> RapportLignes { get; internal set; } = new List<RapportLigneEnt>();

    }
}
