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
using Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac.Interfaces;
using Fred.ImportExport.DataAccess.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac
{
    public class RemonteeVracFtpQueryExecutor : IRemonteeVracFtpQueryExecutor
    {
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IRemonteeVracFtpQueryBuilder remonteeVracAnaelQueryBuilder;
        private readonly ISocieteManager societeManager;
        private readonly IEtablissementPaieManager etablissementPaieManager;
        private readonly IPersonnelManager personnelManager;

        public RemonteeVracFtpQueryExecutor(
            IRemonteeVracManager remonteeVracManager,
            IRemonteeVracFtpQueryBuilder remonteeVracAnaelQueryBuilder,
            ISocieteManager societeManager,
            IEtablissementPaieManager etablissementPaieManager,
            IPersonnelManager personnelManager)
        {
            this.remonteeVracManager = remonteeVracManager;
            this.remonteeVracAnaelQueryBuilder = remonteeVracAnaelQueryBuilder;
            this.societeManager = societeManager;
            this.etablissementPaieManager = etablissementPaieManager;
            this.personnelManager = personnelManager;
        }

        /// <summary>
        /// Execute le remontée VRAC
        /// </summary>
        /// <param name="globalData">Données globale à la remontée vrac</param>
        /// <param name="remonteeVrac">remonteeVrac</param>
        /// <param name="query">Requete</param>
        public void ExectuteRemonteeVrac(ValidationPointageContextData globalData, RemonteeVracEnt remonteeVrac, string query)
        {
            using (var destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, globalData.ConnexionChaineSource))
            {
                // Appel de la procédure effectuant la remontée vrac
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
                string errorMsg = "Erreur lors de l'appel à la procédure AS400 de la Remontée vrac. Requête : " + query;
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                remonteeVrac.DateFin = DateTime.UtcNow;
                this.remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Failed.ToIntValue());

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
        public void GetRemonteeVracErreur(RemonteeVracEnt remonteeVrac, string nomUtilisateur, string jobId, DataAccess.Common.Database destinationDatabase)
        {
            var query = remonteeVracAnaelQueryBuilder.GeRemonteeVracErreurQuery(nomUtilisateur, jobId);

            try
            {
                List<SocieteEnt> societes = this.societeManager.GetSocieteList().ToList();
                List<EtablissementPaieEnt> etablissements = this.etablissementPaieManager.GetEtablissementPaieList().ToList();
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
                string errorMsg = "Erreur lors de la récupération des erreurs de la Remontée Vrac. Requête : " + query;
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                remonteeVrac.DateFin = DateTime.UtcNow;
                this.remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Failed.ToIntValue());
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
                string errorMsg = "[ERR-REMONTEVRAC-FTP] Erreur lors de l'insertion des erreurs de la remontée vrac dans fred : matricule " + matricule + " non trouvé .";
                NLog.LogManager.GetCurrentClassLogger().Error(errorMsg);
            }
        }
    }
}
