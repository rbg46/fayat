using Fred.Entities.ValidationPointage;

namespace Fred.ImportExport.Business.ValidationPointage.Common
{
    /// <summary>
    /// Provider qui fournie les données relative au controle du pointage
    /// </summary>
    public interface IControlePointageProvider
    {
        /// <summary>
        ///   Création du controlePointageEnt
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur exécutant le contrôle</param>
        /// <param name="lotPointageId">Identifiant du lot de pointage contrôlé</param>
        /// <param name="typeControle">Type de contrôle</param>
        /// <returns>ControlePointageEnt</returns>
        ControlePointageEnt CreateControlePointage(int utilisateurId, int lotPointageId, int typeControle);
    }
}
