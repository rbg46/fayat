namespace Fred.Web.Shared.Models.Referential
{
    /// <summary>
    /// Prime affectation
    /// </summary>
    public class MajorationAffectationModel
    {
        /// <summary>
        /// Obtient le code du prime
        /// </summary>
        public string CodeMajoration { get; set; }

        /// <summary>
        ///  Obtient Le jour d'affectation 
        /// </summary>
        public int AffectationDay { get; set; }

        /// <summary>
        /// Obtient ou definit le Ci identifier
        /// </summary>
        public int CiId { get; set; }
    }
}
