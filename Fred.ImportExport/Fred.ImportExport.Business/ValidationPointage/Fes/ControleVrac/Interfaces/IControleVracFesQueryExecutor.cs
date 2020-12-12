using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Interfaces
{
    public interface IControleVracFesQueryExecutor
    {
        /// <summary>
        /// Execute le controle VRAC
        /// </summary>
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="ctrlPointage">ctrlPointage</param>
        /// <param name="query">Requete</param>
        void ExectuteControleVrac(ValidationPointageContextData globalData, ControlePointageEnt ctrlPointage, string query);
    }
}
