using System;
using System.Reflection;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Tool;
using Fred.ImportExport.Business.Anael;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac.Interfaces;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac
{
    public class RemonteeVracFtpQueryBuilder : IRemonteeVracFtpQueryBuilder
    {
        private const string CallRemonteeVracQuery = "CALL INTFTP.SVRFRE01C ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')";
        private readonly string sqlScriptPath = "ValidationPointage.Ftp.SELECT_ERREUR_REMONTEE_VRAC.sql";
        private const string AllMatricule = "******";

        private readonly char spacePadChar = ' ';
        private readonly int etablissementCodeLenght = 10;
        private readonly int jobIdLenght = 10;

        private readonly IEtablissementFormator etablissementFormator;

        public RemonteeVracFtpQueryBuilder(IEtablissementFormator etablissementFormator)
        {
            this.etablissementFormator = etablissementFormator;
        }

        /// <summary>
        /// Construction de la requête d'appel au programme AS400 permettant de recupere les erreurs la Remontée Vrac
        /// </summary>
        /// <param name="nomUtilisateur">nomUtilisateur</param>
        /// <param name="jobId">jobId</param>
        /// <returns>Requête</returns>
        public string GeRemonteeVracErreurQuery(string nomUtilisateur, string jobId)
        {
            string query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), sqlScriptPath);
            return string.Format(query, nomUtilisateur, jobId);
        }

        /// <summary>
        ///   Construction de la requête d'appel au programme AS400 permettant de lancer la Remontée Vrac
        /// </summary>     
        /// <param name="globalData">Données globale à la remontée vrac</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="filtre">Filtre remontée vrac</param>
        /// <returns>Requête</returns>  
        public string BuildRemonteeVracQuery(ValidationPointageContextData globalData, DateTime periode, PointageFiltre filtre)
        {
            var etsCodeList = etablissementFormator.ConcatEtablissementPaieCode(filtre.EtablissementPaieIdList).PadRight(etablissementCodeLenght, spacePadChar);
            var localPeriode = periode.ToLocalTime();
            var nomUtilisateur = (globalData.NomUtilisateur.Length > 10) ? globalData.NomUtilisateur.Substring(0, 10) : globalData.NomUtilisateur;

            var jobId = globalData.JobId.PadLeft(jobIdLenght, spacePadChar);

            return string.Format(CallRemonteeVracQuery,
                          globalData.CodeSocietePaye,
                          localPeriode.Month.ToString("00"),
                          localPeriode.Year,
                          string.IsNullOrEmpty(filtre.Matricule) ? AllMatricule : filtre.Matricule.Trim(),
                          "S",
                          "O", // Remise à blanc (toujours à Oui)
                          filtre.UpdateAbsence ? "O" : "N", // Mise à jour des absences
                          etsCodeList,
                          nomUtilisateur.FormatUsername(),
                          jobId);
        }
    }
}
