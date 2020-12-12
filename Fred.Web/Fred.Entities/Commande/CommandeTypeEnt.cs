using System;

namespace Fred.Entities.Commande
{
    /// <summary>
    ///   Représente un type de commande.
    /// </summary>
    [Serializable]
    public class CommandeTypeEnt
    {
        /// <summary>
        ///   Constante type commande : Fourniture
        /// </summary>
        private static readonly string CommandeTypeFValue = "F";

        /// <summary>
        ///   Constante type commande : Location
        /// </summary>
        private static readonly string CommandeTypeLValue = "L";

        /// <summary>
        ///   Constante type commande : Prestation
        /// </summary>
        private static readonly string CommandeTypePValue = "P";

        /// <summary>
        ///   Constante type commande : Interimaire
        /// </summary>
        private static readonly string CommandeTypeIValue = "I";

        /// <summary>
        ///   Obtient la constante type commande Fourniture.
        /// </summary>
        public static string CommandeTypeF => CommandeTypeFValue;

        /// <summary>
        ///   Obtient la constante type commande Fourniture.
        /// </summary>
        public static string CommandeTypeL => CommandeTypeLValue;

        /// <summary>
        ///   Obtient la constante type commande Fourniture.
        /// </summary>
        public static string CommandeTypeP => CommandeTypePValue;

        /// <summary>
        ///   Obtient la constante type commande Interimaire.
        /// </summary>
        public static string CommandeTypeI => CommandeTypeIValue;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un type de commande.
        /// </summary>
        public int CommandeTypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un type de commande.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un type de commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Child Commandes where [FRED_COMMANDE].[TypeId] point to this entity (FK_COMMANDE_COMMANDE_TYPE)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<CommandeEnt> Commandes { get; set; }
    }
}
