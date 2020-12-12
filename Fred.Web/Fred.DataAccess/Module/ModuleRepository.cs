using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Module;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Module
{
    public class ModuleRepository : FredRepository<ModuleEnt>, IModuleRepository
    {
        public ModuleRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        ///   Retourne la liste des modules non supprimées.
        /// </summary>
        /// <returns>Liste des modules.</returns>
        public IEnumerable<ModuleEnt> GetModuleList()
        {
            var query = Query()
              .Filter(m => m.DateSuppression == null)
              .Get()
              .AsNoTracking()
              .Select(m => new { ModuleEnt = m, Fonctionnalites = m.Fonctionnalites.Where(f => f.DateSuppression == null) })
              .ToList();

            var result = new List<ModuleEnt>();
            foreach (var m in query)
            {
                m.ModuleEnt.Fonctionnalites = m.Fonctionnalites.ToList();
                result.Add(m.ModuleEnt);
            }

            return result;
        }

        /// <summary>
        ///   Récupère un module en fonction de son identifiant.
        /// </summary>
        /// <param name="moduleId">Identifiant du module à retrouver.</param>
        /// <returns>Le module retrouvé, sinon null.</returns>
        public ModuleEnt GetModuleById(int moduleId)
        {
            return GetModuleList().SingleOrDefault(m => m.ModuleId == moduleId);
        }

        /// <summary>
        ///   Récupère un module en fonction de son code.
        /// </summary>
        /// <param name="code">Code du module à retrouver.</param>
        /// <returns>Le module retrouvé, sinon null.</returns>
        public ModuleEnt GetModuleByCode(string code)
        {
            return GetModuleList().SingleOrDefault(m => m.Code == code);
        }

        /// <summary>
        ///   Détermine si un module peut être supprimé
        /// </summary>
        /// <param name="moduleId">Identifiant du Module à vérifier</param>
        /// <returns>True si le module peut être supprimé, sinon Faux</returns>
        public bool IsDeletable(int moduleId)
        {
            return CheckModuleDependencies(moduleId);
        }

        /// <summary>
        ///   Appel la procédure stockée VerificationDeDependance qui permet de vérifier les dépendances d'un module
        /// </summary>
        /// <param name="moduleId">Identifiant du module à vérifier</param>
        /// <returns>Retourne Vrai si le rôle est supprimable, sinon Faux</returns>
        private bool CheckModuleDependencies(int moduleId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("origTableName", "FRED_MODULE"),
                new SqlParameter("dependance", "FRED_FONCTIONNALITE"),
                new SqlParameter("exclusion", string.Empty),
                new SqlParameter("origineId", moduleId),
                new SqlParameter("delimiter", string.Empty),
                new SqlParameter("resu", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            // ReSharper disable once CoVariantArrayConversion
            Context.Database.ExecuteSqlCommand("VerificationDeDependance @origTableName, @exclusion, @dependance, @origineId, @delimiter, @resu OUTPUT", parameters);

            // Vérifie s'il y a aucune dépendances (paramètre "resu")
            SqlParameter firstOrDefault = parameters.FirstOrDefault(x => x.ParameterName == "resu");
            return firstOrDefault != null && Convert.ToInt32(firstOrDefault.Value) == 0;
        }
    }
}
