using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.LogImport;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.LogImport
{
    /// <summary>
    ///   Référentiel de données pour les logs d'import
    /// </summary>
    public class LogImportRepository : FredRepository<LogImportEnt>, ILogImportRepository
    {
        public LogImportRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        ///   Retourne une nouvelle instance de LogImport
        /// </summary>
        /// <returns>Retourne un objet initialisé de log import</returns>
        public LogImportEnt GetNew()
        {
            return new LogImportEnt();
        }

        /// <summary>
        ///   Retourne la liste de tout les logs import
        /// </summary>
        /// <returns>Une liste trié de log import</returns>
        public IEnumerable<LogImportEnt> GetAllLogImport()
        {
            foreach (LogImportEnt log in Context.LogImports.OrderBy(s => s.DateImport))
            {
                yield return log;
            }
        }

        /// <summary>
        ///   Retourne la liste des logs import pour un type d'import passé en parametre
        /// </summary>
        /// <param name="typeImport">Type d'import recherché</param>
        /// <returns>Une liste triée de log import</returns>
        public IEnumerable<LogImportEnt> GetLogImportByType(string typeImport)
        {
            return Context.LogImports.OrderBy(s => s.DateImport).Where(s => s.TypeImport.Equals(typeImport));
        }

        /// <summary>
        ///   Retourne la liste des logs import pour une date passée en parametre
        /// </summary>
        /// <param name="dateimport">Date recherché</param>
        /// <returns>Une liste trié de log import</returns>
        public IEnumerable<LogImportEnt> GetLogImportByDate(DateTime dateimport)
        {
            return Context.LogImports.OrderBy(s => s.DateImport).Where(s => s.DateImport.Equals(dateimport));
        }

        /// <summary>
        ///   Insertion en base d'un log import
        /// </summary>
        /// <param name="logImport">Le log import à enregistrer</param>
        /// <returns>Retourne l'identifiant unique du log</returns>
        public int Add(LogImportEnt logImport)
        {
            try
            {
                //Validation avant insertion
                if (logImport == null)
                {
                    throw new FredRepositoryException("Erreur d'insertion d'un log d'import : logImport is null");
                }
                if (string.IsNullOrEmpty(logImport.Data))
                {
                    throw new FredRepositoryException("Erreur d'insertion d'un log d'import : logImport.Data is null");
                }
                if (string.IsNullOrEmpty(logImport.MessageErreur))
                {
                    throw new FredRepositoryException("Erreur d'insertion d'un log d'import : logImport.MessageErreur is null");
                }
                if (string.IsNullOrEmpty(logImport.TypeImport))
                {
                    throw new FredRepositoryException("Erreur d'insertion d'un log d'import : logImport.TypeImport is null");
                }

                if (logImport.DateImport == DateTime.MinValue)
                {
                    logImport.DateImport = DateTime.Now;
                }

                //Insertion des données
                Context.LogImports.Add(logImport);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new FredRepositoryException("Erreur de concurence d'acces lors de l'insertion d'un log d'import : " + exception.Message, logImport, exception);
            }
            catch (Exception exception)
            {
                throw new FredRepositoryException("Erreur inconnue lors de l'insertion d'un log d'import : " + exception.Message, logImport, exception);
            }

            return logImport.LogImportId;
        }
    }
}