using System;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fon.ControleVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fon.Logs;
using Fred.ImportExport.DataAccess.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.ControleVrac
{
    public class ControleVracFonQueryExecutor : IControleVracFonQueryExecutor
    {
        private readonly IControlePointageManager controlePointageManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IControleVracFonQueryBuilder controleVracAnaelQueryBuilder;
        private readonly IValidationPointageFonLogger logger;

        public ControleVracFonQueryExecutor(
            IControlePointageManager controlePointageManager,
            IPersonnelManager personnelManager,
            IControleVracFonQueryBuilder controleVracAnaelQueryBuilder,
            IValidationPointageFonLogger logger)
        {
            this.controlePointageManager = controlePointageManager;
            this.personnelManager = personnelManager;
            this.controleVracAnaelQueryBuilder = controleVracAnaelQueryBuilder;
            this.logger = logger;
        }

        #region Execution des requetes du controle vrac
        /// <summary>
        /// Execute le controle VRAC
        /// </summary>
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="ctrlPointage">ctrlPointage</param>
        /// <param name="query">Requete</param>
        public void ExectuteControleVrac(ValidationPointageContextData globalData, ControlePointageEnt ctrlPointage, string query)
        {
            using (var destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, globalData.ConnexionChaineSource))
            {
                // Appel de la procédure effectuant le contrôle vrac
                CallControleVracAS400(ctrlPointage, query, destinationDatabase);

                // Récupération des erreurs et enregistrement dans la base Fred.Database
                GetControleVracErreur(ctrlPointage, globalData.NomUtilisateur, destinationDatabase, globalData.CodeSocietePaye);
            }
        }

        /// <summary>
        ///   Appel de la procédure de contrôle vrac côté AS400
        /// </summary>    
        /// <param name="ctrlPointage">Controle Pointage</param>
        /// <param name="query">Requête programme contrôle vrac</param>
        /// <param name="destinationDatabase">Base de données</param>    
        private void CallControleVracAS400(ControlePointageEnt ctrlPointage, string query, DataAccess.Common.Database destinationDatabase)
        {
            try
            {
                destinationDatabase.ExecuteNonQuery(query);
            }
            catch (Exception e)
            {
                var errorMsg = this.logger.ErrorInExecuteControleVracWithAnael(e, query);

                this.controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Failed.ToIntValue());

                throw new FredBusinessException(errorMsg, e);
            }
        }

        /// <summary>
        ///   Récupère les erreurs après un contrôle vrac dans la base AS400
        /// </summary>
        /// <param name="ctrlPointage">Controle pointage</param>
        /// <param name="nomUtilisateur">Nom de l'utilisateur</param>    
        /// <param name="destinationDatabase">Base de données</param>
        /// <param name="codeSocietePaye">codeSocietePaye</param>   
        private void GetControleVracErreur(ControlePointageEnt ctrlPointage, string nomUtilisateur, DataAccess.Common.Database destinationDatabase, string codeSocietePaye)
        {
            string query = controleVracAnaelQueryBuilder.GetControleVracErreurQuery(nomUtilisateur);

            try
            {
                int day = 0, month = 0, year = 0;
                DateTime? dateRapport;
                var personnels = this.personnelManager.GetPersonnelListByCodeSocietePaye(codeSocietePaye).ToList();
                using (var reader = destinationDatabase.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(reader["EDTMAT"])))
                        {
                            var personnel = personnels.FirstOrDefault(p => p.Matricule == Convert.ToString(reader["EDTMAT"]));
                            if (personnel != null)
                            {
                                day = Convert.ToInt32(reader["EDTJJP"]);
                                month = Convert.ToInt32(reader["EDTMMP"]);
                                year = Convert.ToInt32(reader["EDTAAP"]);
                                dateRapport = day > 0 && month > 0 && year > 0 ? new DateTime(year, month, day) : default(DateTime?);

                                var erreur = new ControlePointageErreurEnt
                                {
                                    DateRapport = dateRapport,
                                    Message = Convert.ToString(reader["EDTERR"]).Trim(),
                                    ControlePointageId = ctrlPointage.ControlePointageId,
                                    PersonnelId = personnel.PersonnelId,
                                    CodeCi = Convert.ToString(reader["EDTAFF"]).Trim()
                                };
                                this.controlePointageManager.AddControlePointageErreur(erreur);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var errorMsg = this.logger.ErrorInRetrieveAnaelControleVracInfo(e, query);

                this.controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Failed.ToIntValue());

                throw new FredBusinessException(errorMsg, e);
            }
        }

        #endregion
    }
}
