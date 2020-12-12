using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Commande
{
    /// <summary>
    ///   Représente un statut de commande
    /// </summary>
    [Serializable]
    public class StatutCommandeEnt
    {
        /// <summary>
        ///   Constante statut commande : Brouillon
        /// </summary>
        private static readonly string CommandeStatutBr = "BR";

        /// <summary>
        ///   Constante statut commande : couleur Brouillon
        /// </summary>
        private static readonly string ColorBr = "#696969";

        /// <summary>
        ///   Constante statut commande : A Valider
        /// </summary>
        private static readonly string CommandeStatutAv = "AV";

        /// <summary>
        ///   Constante statut commande : couleur A Valider
        /// </summary>
        private static readonly string ColorAv = "#ff8c00";

        /// <summary>
        ///   Constante statut commande : Validé
        /// </summary>
        private static readonly string CommandeStatutVa = "VA";

        /// <summary>
        ///   Constante statut commande : Validé
        /// </summary>
        private static readonly string CommandeStatutManuelleVa = "MVA";

        /// <summary>
        ///   Constante statut commande : couleur Validé
        /// </summary>
        private static readonly string ColorVa = "#1e90ff";

        /// <summary>
        ///   Constante statut commande : couleur Manuelle Validé
        /// </summary>
        private static readonly string ColorMva = "#32cd32";

        /// <summary>
        ///   Constante statut commande : Clôturé
        /// </summary>
        private static readonly string CommandeStatutCl = "CL";

        /// <summary>
        ///   Constante statut commande : couleur Clôturé
        /// </summary>
        private static readonly string ColorCl = "#e51400";

        /// <summary>
        ///   Constante statut commande : Externe
        /// </summary>
        private static readonly string CommandeStatutEx = "EX";

        /// <summary>
        ///   Constante statut commande : couleur Externe
        /// </summary>
        private static readonly string ColorEx = "#6a5acd";

        /// <summary>
        ///   Obtient le statut commande : Brouillon
        /// </summary>
        public static string CommandeStatutBR => CommandeStatutBr;

        /// <summary>
        ///   Obtient le statut commande : Brouillon
        /// </summary>
        public static string ColorBR => ColorBr;

        /// <summary>
        ///   Obtient le statut commande : A Valider
        /// </summary>
        public static string CommandeStatutAV => CommandeStatutAv;

        /// <summary>
        ///   Obtient le statut commande : A Valider
        /// </summary>
        public static string ColorAV => ColorAv;

        /// <summary>
        ///   Obtient le statut commande : Validée
        /// </summary>
        public static string CommandeStatutVA => CommandeStatutVa;

        /// <summary>
        ///   Obtient le statut commande : Validée
        /// </summary>
        public static string ColorVA => ColorVa;

        /// <summary>
        ///   Obtient le statut commande manuelle: Validée
        /// </summary>
        public static string CommandeStatutMVA => CommandeStatutManuelleVa;

        /// <summary>
        ///   Obtient le statut commande manuelle : Validée
        /// </summary>
        public static string ColorMVA => ColorMva;

        /// <summary>
        ///   Obtient le statut commande : Clôturée
        /// </summary>
        public static string CommandeStatutCL => CommandeStatutCl;

        /// <summary>
        ///   Obtient le statut commande : Clôturée
        /// </summary>
        public static string ColorCL => ColorCl;

        /// <summary>
        ///   Obtient le statut commande : Externe
        /// </summary>
        public static string CommandeStatutEX => CommandeStatutEx;

        /// <summary>
        ///   Obtient le statut commande : Externe
        /// </summary>
        public static string ColorEX => ColorEx;

        /// <summary>
        ///   Obtient le statut commande : Externe
        /// </summary>
        public static string CommandeStatutPREP => "PREP";

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un statut de commande.
        /// </summary>
        public int StatutCommandeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un statut de commande.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un statut de commande.
        /// </summary>
        public string Libelle { get; set; }


        ///////////////////////////////////////////////////////////////////////////
        // AJOUT LORS DE LE MIGRATION CODE FIRST 
        ///////////////////////////////////////////////////////////////////////////

        // Reverse navigation

        /// <summary>
        /// Child Commandes where [FRED_COMMANDE].[StatutCommandeId] point to this entity (FK_COMMANDE_COMMANDE_STATUT)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<CommandeEnt> Commandes { get; set; } // FRED_COMMANDE.FK_COMMANDE_COMMANDE_STATUT

    }
}
