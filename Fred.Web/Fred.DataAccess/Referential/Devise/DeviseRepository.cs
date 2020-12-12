using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.Devise
{
    /// <summary>
    ///   Référentiel de données pour les pays.
    /// </summary>
    public class DeviseRepository : FredRepository<DeviseEnt>, IDeviseRepository
    {
        public DeviseRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        ///   Appel à une procédure stockée
        /// </summary>
        /// <param name="deviseId">Id de la devise a vérifier les dépendances</param>
        /// <returns>Renvoie un bool</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(int deviseId)
        {
            var ciDeviseId = GetCiDeviseIdByDdeviseId(deviseId);

            var commandeID = GetCommandeIdByDdeviseId(deviseId);

            int? factureID;
            factureID = GetFactureIdByDdeviseId(deviseId);

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "VerificationDeDependance";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_DEVISE"));
            sqlCommand.Parameters.Add(new SqlParameter("@exclusion", string.Empty));
            if (ciDeviseId.HasValue || commandeID.HasValue)
            {
                sqlCommand.Parameters.Add(new SqlParameter("@dependance", $"'FRED_CI_DEVISE',{ciDeviseId} | 'FRED_COMMANDE',{commandeID} | FRED_FACTURE,{factureID}"));
            }
            else
            {
                sqlCommand.Parameters.Add(new SqlParameter("@dependance", string.Empty));
            }
            sqlCommand.Parameters.Add(new SqlParameter("@origineId", deviseId));
            sqlCommand.Parameters.Add(new SqlParameter("@delimiter", '|'));
            sqlCommand.Parameters.Add(new SqlParameter("@resu", SqlDbType.Int) { Direction = ParameterDirection.Output });

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();

            int nbcmd = (int)sqlCommand.Parameters["@resu"].Value;

            if (nbcmd == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Permet de récupérer l'id d'un CI Devise lié au à la devise.
        /// </summary>
        /// <param name="id">Identifiant de la devise</param>
        /// <returns>Retourne l'identifiant du 1er pointage anticipé</returns>
        private int? GetCiDeviseIdByDdeviseId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.CIDevises.Where(s => s.DeviseId == id).Select(s => s.CiDeviseId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet de récupérer l'id d'une commande lié au à la devise.
        /// </summary>
        /// <param name="id">Identifiant de la devise</param>
        /// <returns>Retourne l'identifiant du 1er pointage anticipé</returns>
        private int? GetCommandeIdByDdeviseId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.Commandes.Where(s => s.DeviseId == id).Select(s => s.CommandeId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet de récupérer l'id d'une facture lié au à la devise.
        /// </summary>
        /// <param name="id">Identifiant de la devise</param>
        /// <returns>Retourne l'identifiant du 1er pointage anticipé</returns>
        private int? GetFactureIdByDdeviseId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.FactureARs.Where(s => s.DeviseId == id).Select(s => s.FactureId).FirstOrDefault();
        }

        /// <summary>
        ///   Retourne luen nouvelle instance de devise.
        /// </summary>
        /// <returns>Une nouvelle instance de devise.</returns>
        public DeviseEnt New()
        {
            return new DeviseEnt();
        }

        /// <summary>
        ///   Retourne la liste des devises.
        /// </summary>
        /// <returns>Liste des devises.</returns>
        public IEnumerable<DeviseEnt> GetList()
        {
            foreach (DeviseEnt i in Context.Devise.Where(c => !c.DateSuppression.HasValue))
            {
                yield return i;
            }
        }

        /// <summary>
        ///   Retourne la liste complète des devises.
        /// </summary>
        /// <returns>Liste des devises.</returns>
        public IQueryable<DeviseEnt> GetAll()
        {
            return Context.Devise;
        }

        /// <summary>
        ///   Retourne la devise portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la devise dont l'identifiant est à retrouver.</param>
        /// <returns>Devise retrouvé, sinon null.</returns>
        public DeviseEnt GetByCode(string code)
        {
            return Context.Devise.FirstOrDefault(d => d.IsoCode == code);
        }

        /// <summary>
        ///   Retourne la devise par id.
        /// </summary>
        /// <param name="id">Id de la devise à retrouver.</param>
        /// <returns>Devise retrouvé, sinon null.</returns>
        public DeviseEnt GetById(int id)
        {
            return Context.Devise.FirstOrDefault(d => d.DeviseId == id);
        }

        /// <summary>
        ///   Retourne l'identifiant de la devise portant le code devise indiqué.
        /// </summary>
        /// <param name="code">Code de la devise à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon null.</returns>
        public int? GetDeviseIdByCode(string code)
        {
            int? deviseId = null;
            DeviseEnt devise = Context.Devise.FirstOrDefault(d => d.IsoCode == code);

            if (devise != null)
            {
                deviseId = devise.DeviseId;
            }

            return deviseId;
        }

        /// <summary>
        ///   Vérifie qu'une devise peut être supprimé
        /// </summary>
        /// <param name="item">La à vérifier</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(DeviseEnt item)
        {
            return AppelTraitementSqlVerificationDesDependances(item.DeviseId);
        }

        /// <summary>
        ///   Permet de récupérer la liste des devises en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les devises</param>
        /// <returns>Retourne la liste filtré de devises</returns>
        public IEnumerable<DeviseEnt> SearchDeviseWithFilters(Expression<Func<DeviseEnt, bool>> predicate)
        {
            return Context.Devise.Where(predicate).OrderBy(s => s.IsoCode);
        }

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return !AppelTraitementSqlVerificationDesDependances(id);
        }
    }
}
