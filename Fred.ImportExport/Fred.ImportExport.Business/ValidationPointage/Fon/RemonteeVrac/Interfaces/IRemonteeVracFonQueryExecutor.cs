using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac.Interfaces
{
    public interface IRemonteeVracFonQueryExecutor
    {
        /// <summary>
        /// Execute le remontée VRAC
        /// </summary>
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="remonteeVrac">remonteeVrac</param>
        /// <param name="query">Requete</param>
        void ExectuteRemonteeVrac(ValidationPointageContextData globalData, RemonteeVracEnt remonteeVrac, string query);
    }
}
