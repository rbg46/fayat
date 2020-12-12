using System.Diagnostics;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Classe de mapping entre ciid, societeId et SocieteGeranteId
    /// </summary>
    [DebuggerDisplay("CiId = {CiId} SocieteId = {SocieteId} SocieteGeranteId = {SocieteGeranteId}")]
    public class CiSocieteSocieteGeranteMapping
    {
        /// <summary>
        /// CiId
        /// </summary>
        public int CiId { get; internal set; }
        /// <summary>
        /// SocieteId
        /// </summary>
        public int SocieteId { get; internal set; }
        /// <summary>
        /// SocieteGeranteId
        /// </summary>
        public int? SocieteGeranteId { get; internal set; }
    }
}
