using System;
using System.Data;
using System.Data.Common;
using Fred.Framework.Exceptions;

namespace Fred.ImportExport.DataAccess.Common
{
    /// <summary>
    ///   Classe d'accès à la base de données
    /// </summary>
    public class Database : IDisposable
    {
        private bool disposed;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="Database"/>.
        /// </summary>
        /// <param name="connector">Chaine du connecteur relatif au type de base de données</param>
        /// <param name="connexionString">Chaine de connexion à la base de données.</param>
        internal Database(string connector, string connexionString)
        {
            try
            {
                this.Factory = DbProviderFactories.GetFactory(connector);
                this.Connection = this.Factory.CreateConnection();
                this.Connection.ConnectionString = connexionString;
                this.Connection.Open();
            }
            catch (Exception ex)
            {
                throw new FredTechnicalException("Connexion à la base échouée", ex);
            }
        }

        /// <summary>
        /// Durée allouée (en seconde) pour le traitement d'une commande.
        /// Si null, la durée par défaut sera de 30s.
        /// </summary>
        public int? CommandeTimeout { get; set; }

        /// <summary>
        ///   Obtient ou définit la connexion à la base de données courante.
        /// </summary>
        private IDbConnection Connection { get; set; }

        /// <summary>
        ///   Obtient ou définit la fabrique de connexion.
        /// </summary>
        private DbProviderFactory Factory { get; set; }

        /// <summary>
        ///   Exécution d'une requête SQL avec un retour sour forme de IDataReader.
        /// </summary>
        /// <param name="query">Requête SQL à exécuter.</param>
        /// <param name="parameters">Paramètres à injecter dans la requête.</param>
        /// <returns>Retourne un IDataReader contenant le résultat de la requête passé en paramètre.</returns>
        public IDataReader ExecuteReader(string query, params string[] parameters)
        {
            IDataReader dataReader = null;
            try
            {
                this.SecureParameters(ref parameters);
                using (IDbCommand command = this.Connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(query, parameters);
                    if (CommandeTimeout.HasValue)
                    {
                        command.CommandTimeout = CommandeTimeout.Value;
                    }
                    dataReader = command.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw new FredTechnicalException(ex.Message, ex);
            }

            return dataReader;
        }

        /// <summary>
        ///   Exécution d'une instruction SQL n'ayant pas de résultat.
        /// </summary>
        /// <param name="query">Instruction SQL à exécuter</param>
        /// <param name="parameters">Paramètres à injecter dans l'instruction SQL</param>
        /// <returns>Statut (int)</returns>
        public int ExecuteNonQuery(string query, params string[] parameters)
        {
            try
            {
                this.SecureParameters(ref parameters);
                using (IDbCommand command = this.Connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = string.Format(query, parameters);
                    if (CommandeTimeout.HasValue)
                    {
                        command.CommandTimeout = CommandeTimeout.Value;
                    }
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new FredTechnicalException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Commence une transaction de base de données.
        /// </summary>
        /// <returns>Objet représentant la nouvelle transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            return Connection.BeginTransaction();
        }

        /// <summary>
        /// Sécurise les paramètres SQL.
        /// </summary>
        /// <param name="parameters">Liste des paramètres à sécuriser.</param>
        private void SecureParameters(ref string[] parameters)
        {
            for (int i = 0; i <= parameters.Length - 1; i++)
            {
                parameters[i] = parameters[i].Replace("'", "''");
            }
        }

        /// <summary>
        ///   Libère les ressources non managées et - managées - en option.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Libère les ressources non managées et - managées - en option.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> pour libérer les ressources non managées et managées.; <c>false</c> pour libérer
        ///   uniquement les ressources non managées.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.Connection.Dispose();
            }

            this.disposed = true;
        }
    }

}

