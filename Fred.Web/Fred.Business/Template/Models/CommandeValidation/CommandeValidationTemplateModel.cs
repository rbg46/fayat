using System.Collections.Generic;
using Fred.Business.Template.Models.Avis;

namespace Fred.Business.Template.Models.CommandeValidation
{
    /// <summary>
    /// Modèle représenant une validation d'une commande
    /// </summary>
    public class CommandeValidationTemplateModel
    {
        /// <summary>
        /// Montant total en HT de la commande ou de l'avenant
        /// </summary>
        public decimal MontantTotalHt { get; set; }

        /// <summary>
        /// Identifiant d'une commande
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        /// Numéro d'une commande
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        /// Identifiant d'un avenant d'une commande
        /// </summary>
        public int? CommandeAvenantId { get; set; }

        /// <summary>
        /// Historique des avis
        /// </summary>
        public List<AvisTemplateModel> Avis { get; set; }

        /// <summary>
        /// Url de la commande concernée
        /// </summary>
        public string Url { get; set; }

    }
}
