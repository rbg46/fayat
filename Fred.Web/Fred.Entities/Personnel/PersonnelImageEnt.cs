namespace Fred.Entities
{
    /// <summary>
    ///   Entité représentant les images associées à un personnel (Signature et photo de profil)
    /// </summary>
    public class PersonnelImageEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant
        /// </summary>
        public int PersonnelImageId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        ///   Obtient ou définit la signature scannée du salarié
        /// </summary>
        public byte[] Signature { get; set; }

        /// <summary>
        ///   Obtient ou définit la photo de profil scannée du salarié
        /// </summary>
        public byte[] PhotoProfil { get; set; }
    }
}
