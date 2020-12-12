using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac.Interfaces
{
    public interface IRemonteeVracFtpQueryExecutor
    {
        /// <summary>
        /// Execute le remontée VRAC
        /// </summary>
        /// <param name="globalData">Données globale de la remontée vrac</param>
        /// <param name="remonteeVrac">remonteeVrac</param>
        /// <param name="query">Requete</param>
        void ExectuteRemonteeVrac(ValidationPointageContextData globalData, RemonteeVracEnt remonteeVrac, string query);
    }
}
