using Fred.ImportExport.Models.Figgo;
using System.Threading.Tasks;

namespace Fred.ImportExport.Business.Figgo
{
    public interface IFiggoManager
    {
        /// <summary>
        /// Gestion des Absences Reçu par Figgo
        /// </summary>
        /// <param name="absence">absence reçu par figgo</param>
        /// <returns>Dictionnaire contenant un jsonErrorFiggo et true si il y'a une erreur</returns>
        Task<JsonErrorFiggo> ImportAbsenceFiggoAsync(FiggoAbsenceModel absence);
    }
}
