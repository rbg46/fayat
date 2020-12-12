using Fred.Entities.Referential;
using Fred.Web.Shared.App_LocalResources;
using System.Diagnostics;

namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Modèle de base pour les tâches dans le cadre d'un chargement du détail d'un budget.
    /// </summary>
    [DebuggerDisplay("Tâche {Code} {Libelle} (id = {TacheId})")]
    public class TacheResultModel : ResultModelBase
    {
        /// <summary>
        /// Identifiant de la tâche.
        /// </summary>
        public int TacheId { get; private set; }

        /// <summary>
        /// Code de la tâche.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Libellé de la tâche.
        /// </summary>
        public string Libelle { get; private set; }

        /// <summary>
        /// True si la tache est active, false sinon
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Contient les messages d'avertissement à afficher
        /// </summary>
        public string Warnings { get; set; } = string.Empty;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="tacheEnt">Entité concernée.</param>
        public TacheResultModel(TacheEnt tacheEnt)
        {
            TacheId = tacheEnt.TacheId;
            Code = tacheEnt.Code;
            Libelle = tacheEnt.Libelle;
            IsActive = tacheEnt.Active;
        }
    }
}
