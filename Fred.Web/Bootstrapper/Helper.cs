using Fred.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Bootstrapper
{
    /// <summary>
    /// Helper class
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Get utilisateur actuel
        /// </summary>
        /// <returns>User identifier</returns>
        public static int GetCurrentUserId()
        {
            int id = 0;
            try
            {
                ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var claims = identity.Claims;
                string nameIdentifier = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
                id = Convert.ToInt32(nameIdentifier);
            }
            catch (Exception)
            {
                return id;
            }

            return id;
        }

        /// <summary>
        /// Get personnel statut by statut id
        /// </summary>
        /// <param name="statutId">Statut identifier</param>
        /// <returns>Statut</returns>
        public static string GetPersonnelStatut(string statutId)
        {
            switch (statutId)
            {
                case "1":
                    return Constantes.PersonnelStatutValue.Ouvrier;
                case "2":
                    return Constantes.PersonnelStatutValue.ETAM;
                case "3":
                    return Constantes.PersonnelStatutValue.Cadre;
                case "4":
                    return Constantes.PersonnelStatutValue.ETAM;
                case "5":
                    return Constantes.PersonnelStatutValue.ETAM;
                default:
                    return string.Empty;
            }
        }
    }
}
