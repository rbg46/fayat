namespace Fred.Web.Shared.Models.Param
{
    public class ParamValueModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de param value.
        /// </summary>
        public int ParamValueId { get; set; }

        /// <summary>
        /// Obtient ou définit la clef de parametre.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Obtient ou définit la valeur de ParamValue.
        /// </summary>
        public string Valeur { get; set; }
    }
}
