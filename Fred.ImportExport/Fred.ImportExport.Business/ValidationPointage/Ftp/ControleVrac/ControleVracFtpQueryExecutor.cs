using System;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac.Interfaces;
using Fred.ImportExport.DataAccess.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac
{
    public class ControleVracFtpQueryExecutor : IControleVracFtpQueryExecutor
    {
        private readonly IControleVracFtpQueryBuilder controleVracAnaelQueryBuilder;
        private readonly IPersonnelManager personnelManager;
        private readonly IControlePointageManager controlePointageManager;

        public ControleVracFtpQueryExecutor(
            IControleVracFtpQueryBuilder controleVracAnaelQueryBuilder,
            IPersonnelManager personnelManager,
            IControlePointageManager controlePointageManager)
        {
            this.controleVracAnaelQueryBuilder = controleVracAnaelQueryBuilder;
            this.personnelManager = personnelManager;
            this.controlePointageManager = controlePointageManager;
        }

        /// <summary>
        /// Execute le Controle Vrac
        /// </summary>
        /// <param name="globalData">Données globale du contrôle vrac</param>
        /// <param name="controlePointageEnt">Controle pointage</param>
        /// <param name="query">Requete</param>
        public void ExecuteControleVrac(ValidationPointageContextData globalData, ControlePointageEnt controlePointageEnt, string query)
        {
            using (var destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, globalData.ConnexionChaineSource))
            {
                // Appel de la procédure effectuant le contrôle vrac
                CallControleVracAS400(controlePointageEnt, query, destinationDatabase);

                // Récupération des erreurs et enregistrement dans la base Fred.Database
                GetControleVracErreur(globalData, controlePointageEnt, destinationDatabase);
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
                string errorMsg = "Erreur lors de l'appel à la procédure AS400 du Contrôle vrac. Requête : " + query;
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Failed.ToIntValue());

                throw new FredBusinessException(errorMsg, e);
            }
        }

        /// <summary>
        ///   Récupère les erreurs après un contrôle vrac dans la base AS400
        /// </summary>
        /// <param name="globalData">Données globale du contrôle vrac</param>
        /// <param name="ctrlPointage">Controle pointage</param>
        /// <param name="destinationDatabase">Base de données</param>           
        private void GetControleVracErreur(ValidationPointageContextData globalData, ControlePointageEnt ctrlPointage, DataAccess.Common.Database destinationDatabase)
        {
            string query = controleVracAnaelQueryBuilder.GeControleVracErreurQuery(globalData.NomUtilisateur);

            try
            {
                int day = 0, month = 0, year = 0;
                DateTime? dateRapport;
                var personnels = personnelManager.GetPersonnelListByCodeSocietePaye(globalData.CodeSocietePaye).ToList();
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
                                controlePointageManager.AddControlePointageErreur(erreur);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string errorMsg = "Erreur lors de la récupération des erreurs du Contrôle Vrac. Requête : " + query;
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Failed.ToIntValue());
                throw new FredBusinessException(errorMsg, e);
            }
        }
    }
}
