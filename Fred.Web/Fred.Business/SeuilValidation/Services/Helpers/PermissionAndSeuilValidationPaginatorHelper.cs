using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Personnel;

namespace Fred.Business.SeuilValidation.Services.Helpers
{
    /// <summary>
    /// Helper pour la pagination de personnel pour la recuperation des seuil de validation
    /// </summary>
    public class PermissionAndSeuilValidationPaginatorHelper
    {

        /// <summary>
        /// Pagine
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="recherche">recherche</param>
        /// <param name="personnelsActifs">personnelsActifs</param>
        /// <returns>Liste de personnel</returns>
        public List<PersonnelEnt> FilterAndPaginatePersonnels(int page, int pageSize, string recherche, List<PersonnelEnt> personnelsActifs)
        {
            List<PersonnelEnt> personnelsFiltereds = null;

            // filtre sur la recherche
            if (string.IsNullOrEmpty(recherche))
            {
                personnelsFiltereds = personnelsActifs;
            }
            else
            {
                personnelsFiltereds = personnelsActifs.Where(p => FilterField(p.Nom, recherche) ||
                                                                    FilterField(p.Prenom, recherche) ||
                                                                    FilterField(p.Matricule, recherche))
                                                .ToList();
            }

            // Pagination
            List<PersonnelEnt> personnelsPageds = personnelsFiltereds.OrderBy(c => c.Nom)
                                                                    .Skip((page - 1) * pageSize)
                                                                    .Take(pageSize)
                                                                    .ToList();



            return personnelsPageds;
        }


        private bool FilterField(string field, string recherche)
        {
            if (field == null)
            {
                return false;
            }
            if (field.IndexOf(recherche, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
