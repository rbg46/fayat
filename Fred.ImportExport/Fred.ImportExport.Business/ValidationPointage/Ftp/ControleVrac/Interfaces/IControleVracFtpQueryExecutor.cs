using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac.Interfaces
{
    public interface IControleVracFtpQueryExecutor
    {
        /// <summary>
        /// Execute le Controle Vrac
        /// </summary>
        /// <param name="globalData">Données globale du contrôle vrac</param>
        /// <param name="controlePointageEnt">Controle pointage</param>
        /// <param name="query">Requete</param>
        void ExecuteControleVrac(ValidationPointageContextData globalData, ControlePointageEnt controlePointageEnt, string query);
    }
}
