using Fred.Business.Personnel.Models;
using Fred.Web.Shared.Models.Personnel.Excel;
using System.Collections.Generic;

namespace Fred.Business.Personnel.Mapper
{
    /// <summary>
    /// Mappe les données des affectations vers le model excel
    /// </summary>
    public interface IAffectationToExcelModelMapper:IService
    {
        /// <summary>
        /// returne la liste de model excel à ajouter
        /// </summary>
        /// <param name="affectations">la liste des affectations à mapper</param>
        /// <returns>Liste des rapports à créer</returns>
        List<HabilitationsUtilisateursExcelModel> Transform(UtilOrgaRoleLists affectations);

        /// <summary>
        /// Returne la liste de model excel avec habilitation
        /// </summary>
        /// <param name="affectations">la liste des affectations à mapper</param>
        /// <returns>Liste des rapports à créer</returns>
        List<PersonnelExcelModel> TransformPersonnelWithHabilitation(UtilOrgaRoleLists affectations);
    }
}
