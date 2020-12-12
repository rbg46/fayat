using System.Collections.Generic;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente un statut de rapport
    /// </summary>
    public class RapportStatutEnt
    {
        /// <summary>
        /// Obtient l'identtifiant et le code du statut "En cours"
        /// </summary>
        public static KeyValuePair<int, string> RapportStatutEnCours => new KeyValuePair<int, string>(1, "EC");

        /// <summary>
        /// Obtient l'identtifiant et le code du statut "Validé 1"
        /// </summary>
        public static KeyValuePair<int, string> RapportStatutValide1 => new KeyValuePair<int, string>(2, "V1");

        /// <summary>
        /// Obtient l'identtifiant et le code du statut "Validé 2"
        /// </summary>
        public static KeyValuePair<int, string> RapportStatutValide2 => new KeyValuePair<int, string>(3, "V2");

        /// <summary>
        /// Obtient l'identtifiant et le code du statut "Validé 3"
        /// </summary>
        public static KeyValuePair<int, string> RapportStatutValide3 => new KeyValuePair<int, string>(4, "V3");

        /// <summary>
        /// Obtient l'identtifiant et le code du statut "Verrouillé"
        /// </summary>
        public static KeyValuePair<int, string> RapportStatutVerrouille => new KeyValuePair<int, string>(5, "VE");

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un statut de rapport.
        /// </summary>
        public int RapportStatutId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un statut
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un statut
        /// </summary>
        public string Libelle { get; set; }

        ///////////////////////////////////////////////////////////////////////////
        // AJOUT LORS DE LE MIGRATION CODE FIRST 
        ///////////////////////////////////////////////////////////////////////////

        // Reverse navigation

        /// <summary>
        /// Child Rapports where [FRED_RAPPORT].[RapportStatutId] point to this entity (FK_FRED_RAPPORT_FRED_RAPPORT_STATUT)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<RapportEnt> Rapports { get; set; } // FRED_RAPPORT.FK_FRED_RAPPORT_FRED_RAPPORT_STATUT

        /// <summary>
        /// Child Rapports where [FRED_RAPPORT_Ligne].[RapportStatutId] point to this entity
        /// </summary>
        public virtual System.Collections.Generic.ICollection<RapportLigneEnt> RapportLignes { get; set; } // FRED_RAPPORT.FK_FRED_RAPPORT_LIGNE_FRED_RAPPORT_STATUT
    }
}
