
namespace Fred.Web.Shared.Models.Import
{
    /// <summary>
    /// Représente un système externe.
    /// </summary>
    public class SystemeExterneLightModel
    {
        /// <summary>
        /// Identifiant du système externe.
        /// </summary>
        public int SystemeExterneId { get; set; }

        /// <summary>
        /// Le libellé affiché du système externe.
        /// </summary>
        public string LibelleAffiche { get; set; }

        /// <summary>
        /// Le code du système externe
        /// </summary>
        public string Code { get; set; }
    }
}
