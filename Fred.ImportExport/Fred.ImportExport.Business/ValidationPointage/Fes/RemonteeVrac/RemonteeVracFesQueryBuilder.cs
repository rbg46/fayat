using System;
using System.Reflection;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Tool;
using Fred.ImportExport.Business.Anael;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.RemonteeVrac.Interfaces;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.RemonteeVrac
{
    /// <summary>
    /// Builder qui créer les requetes pour la remontee vrac
    /// </summary>
    public class RemonteeVracFesQueryBuilder : IRemonteeVracFesQueryBuilder
    {
        private readonly string sqlScriptPath = "ValidationPointage.Fes.SELECT_ERREUR_REMONTEE_VRAC.sql";
        private const string CallRemonteeVracQuery = "CALL INTEI.SVRFRE01C ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')";
        private const string AllMatricule = "******";

        private readonly IEtablissementFormator etablissementFormator;

        public RemonteeVracFesQueryBuilder(IEtablissementFormator etablissementFormator)
        {
            this.etablissementFormator = etablissementFormator;
        }

        #region Récupération des requêtes AS400

        public string GeRemonteeVracErreurQuery(string nomUtilisateur, string jobId)
        {
            string query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), sqlScriptPath);
            query = string.Format(query, nomUtilisateur, jobId);
            return query;
        }

        /// <summary>
        ///   Construction de la requête d'appel au programme AS400 permettant de lancer la Remontée Vrac
        /// </summary>     
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="filtre">Filtre remontée vrac</param>
        /// <returns>Requête</returns>      
        public string BuildRemonteeVracQuery(ValidationPointageContextData globalData, DateTime periode, PointageFiltre filtre)
        {
            var etsCodeList = etablissementFormator.ConcatEtablissementPaieCode(filtre.EtablissementPaieIdList);
            var localPeriode = periode.ToLocalTime();
            var nomUtilisateur = (globalData.NomUtilisateur.Length > 10) ? globalData.NomUtilisateur.Substring(0, 10) : globalData.NomUtilisateur;

            // jobId : dentifiant du job hangfire en cours : utilisé pour avoir un identifiant unique lors de la remontée vrac. 
            // Contrainte du programme INTRB.INTVRACRB : l'identifiant du lot doit être unique à chaque exécution. 
            // Or dans FRED, le lot de pointage (LotPointageId) d'un utilisateur est unique pour un mois donné et ne change jamais.
            // Donc ici, on utilisera le jobId de Hangfire (qui est différent à chaque exécution) comme paramètre du programme AS400.            
            var jobId = globalData.JobId.PadLeft(10, ' ');

            return string.Format(CallRemonteeVracQuery,
                          globalData.CodeSocietePaye,
                          localPeriode.Month.ToString("00"),
                          localPeriode.Year,
                          string.IsNullOrEmpty(filtre.Matricule) ? AllMatricule : filtre.Matricule.Trim(),
                          "S",
                          "O", // Remise à blanc (toujours à Oui)
                          (filtre.UpdateAbsence) ? "O" : "N", // Mise à jour des absences
                          etsCodeList.PadRight(10, ' '),
                          nomUtilisateur.FormatUsername(),
                          jobId);
        }
        #endregion
    }
}
