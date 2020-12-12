using System;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.Anael;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac.Interfaces;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac
{
    public class ControleVracFtpQueryBuilder : IControleVracFtpQueryBuilder
    {
        private const string CallControleVracQuery = "CALL INTFTP.VRAFR29C ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
        private const string GetControleVracErreurQuery = "SELECT EDTETB, EDTAFF, EDTMAT, EDTNOM, EDTPRE, EDTAAP, EDTMMP, EDTJJP, EDTERR FROM INTFTP.ERR_{0}";
        private const int CodeSocieteComptaLenght = 5;
        private const char ZeroPadChar = '0';
        private const char SpacePadChar = ' ';
        private const int EtablissementCodeLenght = 10;
        private const int JobIdLenght = 10;

        private readonly IEtablissementFormator etablissementFormator;

        public ControleVracFtpQueryBuilder(IEtablissementFormator etablissementFormator)
        {
            this.etablissementFormator = etablissementFormator;
        }

        /// <summary>
        /// Construction de la requête d'appel au programme AS400 permettant de recupere les erreurs du Controle Vrac
        /// </summary>
        /// <param name="nomUtilisateur">nomUtilisateur</param>
        /// <returns>Requête</returns>
        public string GeControleVracErreurQuery(string nomUtilisateur)
        {
            string tableErreurPrefixe = (nomUtilisateur.Length > 6) ? nomUtilisateur.Substring(0, 6) : nomUtilisateur;
            return string.Format(GetControleVracErreurQuery, tableErreurPrefixe);
        }

        /// <summary>
        ///   Construction de la requête d'appel au programme exécutant le contrôle vrac AS400
        /// </summary>    
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="filtre">Filtre pointage</param>    
        /// <returns>Requête</returns>
        public string BuildControleVracQuery(ValidationPointageContextData globalData, DateTime periode, PointageFiltre filtre)
        {
            var etsCodeList = etablissementFormator.ConcatEtablissementPaieCode(filtre.EtablissementPaieIdList).PadRight(EtablissementCodeLenght, SpacePadChar);
            periode = periode.ToLocalTime();
            var nomUtilisateur = globalData.NomUtilisateur;
            var jobId = globalData.JobId.PadLeft(JobIdLenght, SpacePadChar);
            var codeSocieteComptable = globalData.CodeSocieteComptable.PadLeft(CodeSocieteComptaLenght, ZeroPadChar);
            nomUtilisateur = (nomUtilisateur.Length > 10) ? nomUtilisateur.Substring(0, 10) : nomUtilisateur;

            return string.Format(CallControleVracQuery,
                           globalData.CodeSocietePaye,
                           periode.Month.ToString("00"),
                           periode.Year,
                           codeSocieteComptable,
                           etsCodeList,
                           nomUtilisateur.FormatUsername(),
                           jobId);
        }
    }
}
