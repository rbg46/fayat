
namespace Fred.Web.Shared.Models.RessourceRecommandee
{
    /// <summary>
    /// RessourceRecommandeeModel Class
    /// </summary>
    public class RessourceRecommandeeModel
    {
        /// <summary>
        /// Gets or sets the ressource recommandee identifier.
        /// </summary>
        /// <value>
        /// The ressource recommandee identifier.
        /// </value>
        public int RessourceRecommandeeId { get; set; }

        /// <summary>
        /// Gets or sets the organisation identifier.
        /// </summary>
        /// <value>
        /// The organisation identifier.
        /// </value>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Gets or sets the referentiel etendu identifier.
        /// </summary>
        /// <value>
        /// The referentiel etendu identifier.
        /// </value>
        public int ReferentielEtenduId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is recommandee.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is recommandee; otherwise, <c>false</c>.
        /// </value>
        public bool IsRecommandee { get; set; }
    }
}
