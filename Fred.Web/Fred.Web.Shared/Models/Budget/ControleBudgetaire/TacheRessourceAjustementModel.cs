using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Budget.ControleBudgetaire
{
    /// <summary>
    /// Model utilisable pour représenter le lien entre une tache, une ressource et un montant d'ajustement
    /// </summary>
    public class TacheRessourceAjustementModel
    {
        /// <summary>
        /// Montant de l'ajustement pour cette tache et cette ressource
        /// </summary>
        public decimal Ajustement { get; set; }

        /// <summary>
        /// L'id de la tache de niveau 3 pour ce montant d'ajustement
        /// </summary>
        public int Tache3Id { get; set; }

        /// <summary>
        /// L'id de la ressource pour ce montant d'ajustement
        /// </summary>
        public int RessourceId { get; set; }
    }
}
