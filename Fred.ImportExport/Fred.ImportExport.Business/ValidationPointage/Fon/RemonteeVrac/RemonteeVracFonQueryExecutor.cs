using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fon.Logs;
using Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac.Interfaces;
using Fred.ImportExport.DataAccess.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac
{
    public class RemonteeVracFonQueryExecutor : IRemonteeVracFonQueryExecutor
    {
        private readonly IPersonnelManager personnelManager;
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IEtablissementPaieManager etsPaieManager;
        private readonly ISocieteManager societeManager;
        private readonly IRemonteeVracFonQueryBuilder remonteeVracAnaelQueryBuilder;
        private readonly IValidationPointageFonLogger logger;

        public RemonteeVracFonQueryExecutor(
            IPersonnelManager personnelManager,
            IRemonteeVracManager remonteeVracManager,
            IEtablissementPaieManager etsPaieManager,
            ISocieteManager societeManager,
            IRemonteeVracFonQueryBuilder remonteeVracAnaelQueryBuilder,
            IValidationPointageFonLogger logger)
        {
            this.personnelManager = personnelManager;
            this.remonteeVracManager = remonteeVracManager;
            this.etsPaieManager = etsPaieManager;
            this.societeManager = societeManager;
            this.remonteeVracAnaelQueryBuilder = remonteeVracAnaelQueryBuilder;
            this.logger = logger;
        }

        #region Execution des requetes de la remontée vrac
        /// <summary>
        /// Execute le remontée VRAC
        /// </summary>
        /// <param name="globalData">Données globale au remontée vrac</param>
        /// <param name="remonteeVrac">remonteeVrac</param>
        /// <param name="query">Requete</param>
        public void ExectuteRemonteeVrac(ValidationPointageContextData globalData, RemonteeVracEnt remonteeVrac, string query)
        {
            using (var destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, globalData.ConnexionChaineSource))
            {
                // Appel de la procédure effectuant le contrôle vrac
                CallRemonteeVracAS400(remonteeVrac, query, destinationDatabase);

                // Récupération des erreurs et enregistrement dans la base Fred.Database
                GetRemonteeVracErreur(remonteeVrac, globalData.NomUtilisateur, globalData.JobId, destinationDatabase);
            }
        }

        /// <summary>
        ///   Appel de la procédure de la remontée vrac côté AS400
        /// </summary>    
        /// <param name="remonteeVrac">Remontee Vrac</param>
        /// <param name="query">Requête</param>    
        /// <param name="destinationDatabase">Base de données</param>       
        private void CallRemonteeVracAS400(RemonteeVracEnt remonteeVrac, string query, DataAccess.Common.Database destinationDatabase)
        {
            try
            {
                destinationDatabase.ExecuteNonQuery(query);
            }
            catch (Exception e)
            {
                remonteeVrac.DateFin = DateTime.UtcNow;

                this.remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Failed.ToIntValue());

                var errorMsg = this.logger.ErrorInExecuteRemonteeVracWithAnael(e, query);

                throw new FredBusinessException(errorMsg, e);
            }
        }

        /// <summary>
        ///   Récupère les erreurs après la remontée vrac dans la base AS400
        /// </summary>
        /// <param name="remonteeVrac">Remontee Vrac</param>    
        /// <param name="nomUtilisateur">Nom de l'utilisateur ayant lancé l'opération</param>
        /// <param name="jobId">jobId</param>
        /// <param name="destinationDatabase">Base de données</param>   
        private void GetRemonteeVracErreur(RemonteeVracEnt remonteeVrac, string nomUtilisateur, string jobId, DataAccess.Common.Database destinationDatabase)
        {
            var query = remonteeVracAnaelQueryBuilder.GeRemonteeVracErreurQuery(nomUtilisateur, jobId);

            try
            {
                List<SocieteEnt> societes = this.societeManager.GetSocieteList().ToList();
                List<EtablissementPaieEnt> etablissements = this.etsPaieManager.GetEtablissementPaieList().ToList();
                using (var reader = destinationDatabase.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        var matricule = Convert.ToString(reader["Matricule"]);
                        if (!string.IsNullOrEmpty(matricule))
                        {
                            int? societeId = societes.FirstOrDefault(s => s.CodeSocietePaye == Convert.ToString(reader["Societe"]))?.SocieteId;
                            var etablissementsOfSociete = etablissements.Where(e => e.SocieteId == societeId && societeId != null).ToList();
                            int? etablissementPaieId = etablissementsOfSociete.FirstOrDefault(x => x.Code == Convert.ToString(reader["Etablissement"]))?.EtablissementPaieId;

                            InsertRemonteeVracErreur(remonteeVrac, reader, matricule, societeId, etablissementPaieId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                remonteeVrac.DateFin = DateTime.UtcNow;

                this.remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Failed.ToIntValue());

                var errorMsg = this.logger.ErrorInRetrieveAnaelRemonteeVracErrors(e, query);

                throw new FredBusinessException(errorMsg, e);
            }
        }

        private void InsertRemonteeVracErreur(RemonteeVracEnt remonteeVrac, System.Data.IDataReader reader, string matricule, int? societeId, int? etablissementPaieId)
        {
            var personnel = this.personnelManager.GetPersonnel(societeId: societeId.Value, matricule: matricule);
            if (personnel != null && societeId.HasValue && etablissementPaieId.HasValue)
            {
                DateTime? dateDebut = reader["DateDebut"] != DBNull.Value ? Convert.ToDateTime(reader["DateDebut"]) : default(DateTime?);
                DateTime? dateFin = reader["DateFin"] != DBNull.Value ? Convert.ToDateTime(reader["DateFin"]) : default(DateTime?);

                var erreur = new RemonteeVracErreurEnt
                {
                    PersonnelId = personnel.PersonnelId,
                    SocieteId = societeId.Value,
                    EtablissementPaieId = etablissementPaieId.Value,
                    DateDebut = dateDebut ?? DateTime.UtcNow,
                    DateFin = dateFin,
                    RemonteeVracId = remonteeVrac.RemonteeVracId,
                    CodeAbsenceFred = Convert.ToString(reader["CodeAbsenceFred"]),
                    CodeAbsenceAnael = Convert.ToString(reader["CodeAbsenceAnael"])
                };
                this.remonteeVracManager.AddRemonteeVracErreur(erreur);
            }
            else
            {
                logger.ErrorPersonnelNotFoundForAddRemonteeVracErreur(matricule);
            }
        }

        #endregion
    }
}
