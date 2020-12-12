using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.Avancement.Excel;

namespace Fred.Business.Budget.Avancement.Excel
{
    /// <summary>
    /// Interface définissant le fonctionnement d'un manager pour l'expor excel du modèle de l'avancement
    /// </summary>
    public interface IAvancementExportExcelManager : IManager<AvancementEnt>
    {
        /// <summary>
        /// Retourne l'export excel généré sous forme de tableau de byte
        /// </summary>
        /// <param name="model">Model contenant les informations nécessaires à la génération d'un export excel</param>
        /// <returns>un tableau de byte contenant l'export excel</returns>
        byte[] GetExportExcel(AvancementExcelLoadModel model);

    }
}
