using System.Diagnostics;

namespace Fred.Entities.ActivitySummary
{
    /// <summary>
    /// Represente un liste d'activites a faire pour un utilisateur
    /// </summary>
    [DebuggerDisplay("UserId = {UserId} CiId = {CiId} NombreCommandeAValider = {NombreCommandeAValider} NombreRapportsAvalide1 = {NombreRapportsAvalide1} NombreReceptionsAviser = {NombreReceptionsAviser} NombreBudgetAvalider = {NombreBudgetAvalider}")]
    public class UserActivitySummary
    {

        /// <summary>
        /// L'id de l'utilisateur
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// CiId
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Nombre de Commande a Valider
        /// </summary>
        public int? NombreCommandeAValider { get; set; }

        /// <summary>
        /// Nombre de Rapports a valide1
        /// </summary>
        public int? NombreRapportsAvalide1 { get; set; }

        /// <summary>
        /// Nombre de Receptions A viser
        /// </summary>
        public int? NombreReceptionsAviser { get; set; }

        /// <summary>
        /// Nombre de Budget A valider
        /// </summary>
        public int? NombreBudgetAvalider { get; set; }

        /// <summary>
        /// Nombre d'Avancement Avalider
        /// </summary>
        public int? NombreAvancementAvalider { get; set; }

        /// <summary>
        /// Nombre de controle Budgetaire A valider
        /// </summary>
        public int? NombreControleBudgetaireAvalider { get; set; }


        /// <summary>
        ///  Color de Commande a Valider
        /// </summary>
        public ColorActivity ColorCommandeAValider { get; set; }

        /// <summary>
        /// Color de Rapports a valide1
        /// </summary>
        public ColorActivity ColorRapportsAvalide1 { get; set; }

        /// <summary>
        /// Color de Receptions A viser
        /// </summary>
        public ColorActivity ColorReceptionsAviser { get; set; }

        /// <summary>
        /// Color de Budget A valider
        /// </summary>
        public ColorActivity ColorBudgetAvalider { get; set; }

        /// <summary>
        /// Color d'Avancement Avalider
        /// </summary>
        public ColorActivity ColorAvancementAvalider { get; set; }

        /// <summary>
        /// Color de Controle Budgetaire A valider
        /// </summary>
        public ColorActivity ColorControleBudgetaireAvalider { get; set; }

        /// <summary>
        /// Le label qui  represente le nom du ci
        /// </summary>
        public string Labelle { get; set; }
    }
}
