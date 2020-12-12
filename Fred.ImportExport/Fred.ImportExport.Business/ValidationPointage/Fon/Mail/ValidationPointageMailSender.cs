using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred.Entities.Rapport;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fon.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.Mail
{
    public static class ValidationPointageMailSender
    {
        internal static void Send(ValidationPointageContextData globalData, IEnumerable<RapportLigneEnt> rapportLignes, string query, IEnumerable<QueryInfo> queries, Dictionary<string, string> recipients)
        {
            var body = new StringBuilder();
            body.Append($"<b>JobId : {globalData.JobId}</b><br /><br />");
            body.AddPointages(rapportLignes, globalData.SocieteId);
            body.Append("<br /><br />");
            body.AppendRequetesAS400(query, queries);

            // Envoie du mail
            body.Send(globalData.JobId, recipients);
        }

        /// <summary>
        /// Ajoute les pointages au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="pointages">Les pointages à ajouter.</param>
        /// <param name="societeId">L'identifiant de la société concernée.</param>
        public static void AddPointages(this StringBuilder body, IEnumerable<RapportLigneEnt> pointages, int societeId)
        {
            body.Append($"<b>Liste des pointages envoyés vers ANAEL ({pointages.Count()} pointage(s))</b><br />");

            if (pointages.Any())
            {
                body.Append(@"<table border=""1""><tbody>");
                body.AppendPointageEnTete();

                foreach (var pointage in pointages.OrderBy(p => p.DatePointage))
                {
                    body.AppendPointageLigne(pointage);
                }

                body.Append("</tbody></table>");
            }
        }

        /// <summary>
        /// Ajoute les requêtes envoyées à l'AS400.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="procQuery">Requête procédure AS400</param>      
        /// <param name="queries">Les requêtes pour l'insertion des prime et pointages.</param>
        private static void AppendRequetesAS400(this StringBuilder body, string procQuery, IEnumerable<QueryInfo> queries)
        {
            body.Append("<b>Requêtes SQL AS400</b><br />");
            body.Append("<br />");
            body.Append("Procédure : ").Append(procQuery);
            body.Append("<br />");

            foreach (var query in queries)
            {
                if (!query.IsComment)
                {
                    body.Append(query.Query);
                    body.Append("<br/><br/>");
                }
                else
                {
                    body.Append("-- ---------------------------------------");
                    body.Append(query.Comment);
                    body.Append("<br/><br/>");
                }
            }
        }
    }
}
