namespace Fred.Entities.EmailSouscription.Souscription
{
    /// <summary>
    /// Resultat de la requette permettant de savoir si un personnel a souscrit a une mailling list
    /// </summary>
    public class HasSubscribeToMaillingListResult
    {
        /// <summary>
        /// Id du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// permettant de savoir si un personnel a souscrit a une mailling list
        /// </summary>
        public bool HasSusbcribeToMaillingList { get; set; }
    }
}
