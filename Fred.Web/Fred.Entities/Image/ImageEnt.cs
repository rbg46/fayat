using System.Collections.Generic;
using Fred.Entities.Societe;

namespace Fred.Entities.Image
{
    /// <summary>
    /// Represent l'entité ImageEnt
    /// </summary>
    public class ImageEnt
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// Le chemin 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Credit 
        /// </summary>
        public string Credit { get; set; }

        /// <summary>
        /// Type d'image voir Enumeration TypeImage
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Determine si l'image est l'image par defaut.
        /// </summary>
        public bool? IsDefault { get; set; }

        /// <summary>
        /// Liste de sociétés ratachées a l'image logo
        /// </summary>
        public virtual ICollection<SocieteEnt> LogoSocietes { get; set; }

        /// <summary>
        /// Liste de sociétés ratachées a l'image login
        /// </summary>
        public virtual ICollection<SocieteEnt> LoginSocietes { get; set; }

    }
}
