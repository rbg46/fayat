using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Business.Email.ActivitySummary.Builders;
using Fred.Business.Email.Subscription;
using Fred.Business.Organisation.Tree;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ActivitySummary;
using Fred.Entities.Email;
using Fred.Entities.Organisation.Tree;
using Fred.Framework.Exceptions;
using Fred.Framework.Services;
using RazorEngine;
using RazorEngine.Templating;

namespace Fred.Business.Email.ActivitySummary
{
    public class ActivitySummaryService : IActivitySummaryService
    {
        /// <summary>
        /// Chemin vers le template. Attention executer depuis fred Ie.
        /// </summary>
        private const string Filename = "ActivitiesAndJalonsTemplate.cshtml";
        private const string AucunPersonnelWithNecessaryDataMessage = "Aucun personnel n'a les données necessaires pour un envoie d'email.";
        private const string NoMailSendMessage = "Aucun mail ne sera envoyé car le mail a deja été envoyé a tous les personnels ou parceq'ils ne sont pas inscript a cette mailling liste 'ActivitySummary' ";
        private const string NoActifCisMessage = "Tout les utilisateurs concernés n’ont pas de CI actifs dans leur périmètre d’habilitations.";
        private const string TemplateNoFoundMessage = "Impossible de trouver le template de mail.";

        private readonly IActivitySummaryRepository activitySummaryRepository;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IEmailSubscriptionManager emailSubscriptionManager;

        public ActivitySummaryService(
            IActivitySummaryRepository activitySummaryRepository,
            IOrganisationTreeService organisationTreeService,
            IEmailSubscriptionManager emailSubscriptionManager)
        {
            this.activitySummaryRepository = activitySummaryRepository;
            this.organisationTreeService = organisationTreeService;
            this.emailSubscriptionManager = emailSubscriptionManager;
        }

        /// <summary>
        /// Execute l'analyse les activitées en cours pour tous les utilisateurs eyant souscrit a la mailling list 'ActivitySummary'
        /// genere les email pour chaque utilisateur et les envoie.
        /// </summary>
        /// <param name="dateOfExecution">date d'execution</param>
        public void Execute(DateTime dateOfExecution, string svcDir)
        {
            OrganisationTree mainOrganisationTree = organisationTreeService.GetOrganisationTree();

            List<int> subscribersIds = emailSubscriptionManager.GetSubscribersToMaillingList(EmailSouscriptionKey.ActivitySummary, dateOfExecution);
            if (subscribersIds?.Any() == true)
            {
                CalculateActivitiesAndSendEmailsForPersonnelsWithActifCis(dateOfExecution, mainOrganisationTree, subscribersIds, svcDir);
            }
            else
            {
                Log(NoMailSendMessage);
            }
        }

        private void CalculateActivitiesAndSendEmailsForPersonnelsWithActifCis(DateTime dateOfExecution, OrganisationTree mainOrganisationTree, List<int> subscribersIds, string svcDir)
        {
            // Ne pas envoyer d’email si l’utilisateur concerné n’a aucun CI dans son périmètre d’habilitations,
            // et qui sont actifs ce jour-là(pour qu’un CI soit considéré comme actif,
            // il faut que la date du jour soit supérieure ou égale à la date d’ouverture du CI,
            // et inférieure ou égale à sa date de fermeture).
            var subscribersIdsWithCiActifs = FilterUsersWithCiActifs(mainOrganisationTree, subscribersIds);
            if (subscribersIdsWithCiActifs?.Any() == true)
            {
                CalculateActivitiesAndSendEmails(dateOfExecution, mainOrganisationTree, subscribersIdsWithCiActifs, svcDir);
            }
            else
            {
                Log(NoActifCisMessage);
            }
        }

        private void CalculateActivitiesAndSendEmails(DateTime dateOfExecution, OrganisationTree mainOrganisationTree, List<int> subscribersIdsWithCiActifs, string svcDir)
        {
            List<ActivitySummaryResult> activitySummaryResults = CalculateActivitiesAndJalons(subscribersIdsWithCiActifs, mainOrganisationTree);

            List<PersonnelInfoForSendEmailResult> personnelInfoForSendEmails = activitySummaryRepository.GetPersonnelsDataForSendEmail(subscribersIdsWithCiActifs);

            List<PersonnelInfoForSendEmailResult> personnelWithEmailInfoForSendEmails = FilterPersonnelsWithoutEmail(personnelInfoForSendEmails);

            if (personnelWithEmailInfoForSendEmails?.Any() == true)
            {
                List<EmailGenerationResult> contentOfEmails = GenerateEmails(activitySummaryResults, svcDir);

                var subscribersIdsOnSuccess = SendEmails(personnelWithEmailInfoForSendEmails, contentOfEmails);

                emailSubscriptionManager.UpdateDateEnvoieOfMaillingList(EmailSouscriptionKey.ActivitySummary, subscribersIdsOnSuccess, dateOfExecution);

                ThrowErrorIfNecessaryForFailedEmails(personnelWithEmailInfoForSendEmails, subscribersIdsOnSuccess);
            }
            else
            {
                Log(AucunPersonnelWithNecessaryDataMessage);
            }
        }

        /// <summary>
        /// Le comparatif se fait entre les mails effectivement envoyés(subscribersIdsOnSuccess) 
        /// et les personnes ayant un CI attaché et ayant aussi un email de défini(personnelWithEmailInfoForSendEmails).
        /// </summary>
        /// <param name="personnelWithEmailInfoForSendEmails">personnes ayant un CI attaché et ayant aussi un email de défini</param>
        /// <param name="subscribersIdsOnSuccess">personnes avec emails effectivement envoyés</param>
        private void ThrowErrorIfNecessaryForFailedEmails(List<PersonnelInfoForSendEmailResult> personnelWithEmailInfoForSendEmails, List<int> subscribersIdsOnSuccess)
        {
            if (subscribersIdsOnSuccess.Count != personnelWithEmailInfoForSendEmails.Count)
            {
                var failedElements = personnelWithEmailInfoForSendEmails.Where(p => !subscribersIdsOnSuccess.Contains(p.PersonnelId)).Select(p => p.Email).ToList();
                var failed = string.Join(", ", failedElements.ToArray());
                throw new FredBusinessException($"[EMAIL][RECAPITULATIF_ACTIVITE] Le nombre de mails envoyé ne correspond pas a celui prevu (PersonnelIds failled '{failed}')");
            }
        }

        /// <summary>
        /// Calcule les travaux en cours et les jalons
        /// </summary>
        /// <param name="subscribersIds">Id des utilisateurs eyant souscrit</param>
        /// <param name="mainOrganisationTree">OrganisationTree de fred</param>
        /// <returns>Le resultat de toutes les activitées en cours pour tous les utilisateurs eyant souscrit a la mailling list 'ActivitySummary'</returns>
        public List<ActivitySummaryResult> CalculateActivitiesAndJalons(List<int> subscribersIds, OrganisationTree mainOrganisationTree)
        {
            List<ActivitySummaryResult> result = new List<ActivitySummaryResult>();

            if (subscribersIds.Count > 0)
            {
                List<ActivityRequestWithCountResult> countData = activitySummaryRepository.GetDataForCalculateWorkInProgress();

                List<ActivityRequestWithDateResult> jalonsData = activitySummaryRepository.GetJalons();

                var ciIds = activitySummaryRepository.GetCiActifs();

                countData = countData.Where(cd => ciIds.Contains(cd.CiId) || cd.RequestName == TypeActivity.ReceptionsAviser).ToList();

                var cisFREDIds = activitySummaryRepository.GetCiActifs(true);

                jalonsData = jalonsData.Where(cd => cisFREDIds.Contains(cd.CiId)).ToList();

                List<UserActivitySummary> usersActivities = new UserActivitiesBuilder().BuildUserActivitySummaries(mainOrganisationTree, subscribersIds, countData).OrderBy(x => x.Labelle).ToList();

                List<UserJalonSummary> usersCiJalons = new JalonsBuilder().BuildJalons(mainOrganisationTree, subscribersIds, jalonsData).OrderBy(x => x.Labelle).ToList();

                foreach (int subscribersId in subscribersIds)
                {
                    ActivitySummaryResult activitySummaryResult = new ActivitySummaryResult();

                    activitySummaryResult.UserId = subscribersId;

                    activitySummaryResult.UsersActivities = usersActivities.Where(ua => ua.UserId == subscribersId).ToList();

                    activitySummaryResult.ComputeTotalsAndTotalsColors();

                    activitySummaryResult.UsersCiJalons = usersCiJalons.Where(ua => ua.UserId == subscribersId).ToList();

                    result.Add(activitySummaryResult);
                }
            }

            return result;
        }

        private List<int> FilterUsersWithCiActifs(OrganisationTree mainOrganisationTree, List<int> subscribersIds)
        {
            List<int> result = new List<int>();

            var ciActifIds = activitySummaryRepository.GetCiActifs();

            foreach (int subscribersId in subscribersIds)
            {
                var ciOfUser = mainOrganisationTree.GetAllCiForUser(subscribersId).OrderByDescending(c => c).ToList();

                var intersection = ciOfUser.OrderByDescending(c => c).Intersect(ciActifIds).ToList();

                bool hasCiActif = intersection.Count > 0;

                if (hasCiActif)
                {
                    result.Add(subscribersId);
                }
                else
                {
                    Warn($"Le personnel avec l'id {subscribersId} n'a aucun ci dans son perimetre d'habilitation.");
                }
            }

            return result;
        }

        private List<PersonnelInfoForSendEmailResult> FilterPersonnelsWithoutEmail(List<PersonnelInfoForSendEmailResult> personnelInfoForSendEmails)
        {
            var personnelMatriculesWithoutMail = personnelInfoForSendEmails.Where(p => string.IsNullOrWhiteSpace(p.Email)).Select(p => p.Matricule).ToList();

            // Logger les utilisateurs sans adresse email
            if (personnelMatriculesWithoutMail.Any())
            {
                var personnelMatriculesWithoutMailFormatted = string.Join(", ", personnelMatriculesWithoutMail.ToArray());
                Warn($"Tous les emails n'ont pas pu être envoyés car les personnels suivants n'ont pas d'email renseignés, matricules : {personnelMatriculesWithoutMailFormatted}");
            }

            return personnelInfoForSendEmails.Where(p => !string.IsNullOrWhiteSpace(p.Email)).ToList();
        }

        private List<EmailGenerationResult> GenerateEmails(List<ActivitySummaryResult> activitySummaryResults, string svcDir)
        {
            List<EmailGenerationResult> result = new List<EmailGenerationResult>();

            //ajouter en cas le hangfire des envois des Emails est lancé par plusieurs envirennements
            string rootDir = System.IO.Directory.GetParent(svcDir).Parent.FullName;
            string absoluteTemplatePath = System.IO.Directory.EnumerateFiles(rootDir, Filename, SearchOption.AllDirectories).First();

            if (File.Exists(absoluteTemplatePath))
            {
                string template = File.ReadAllText(absoluteTemplatePath);

                result = BuildEmailGenerationResults(activitySummaryResults, template);
            }
            else
            {
                Warn(TemplateNoFoundMessage);
            }

            return result;
        }

        private List<EmailGenerationResult> BuildEmailGenerationResults(List<ActivitySummaryResult> activitySummaryResults, string template)
        {
            List<EmailGenerationResult> result = new List<EmailGenerationResult>();

            foreach (ActivitySummaryResult activitySummaryResult in activitySummaryResults)
            {
                const string nameOfTemplate = nameof(EmailSouscriptionKey.ActivitySummary);

                // verification si le template est deja dans le cache de razor engine
                // Si deja, cela provoque une erreur.
                bool isCached = Engine.Razor.IsTemplateCached(nameOfTemplate, typeof(ActivitySummaryResult));
                if (!isCached)
                {
                    // ici je chauffe et enregistre, pour que l'execution des autres compilations de razor engine  soit plus rapide
                    Engine.Razor.RunCompile(template, nameOfTemplate, typeof(ActivitySummaryResult), new ActivitySummaryResult());
                }

                EmailGenerationResult emailGenerationResult = new EmailGenerationResult();

                emailGenerationResult.PersonnelId = activitySummaryResult.UserId;

                emailGenerationResult.EmailContent = Engine.Razor.RunCompile(nameOfTemplate, typeof(ActivitySummaryResult), activitySummaryResult);

                result.Add(emailGenerationResult);

            }
            return result;
        }

        private List<int> SendEmails(List<PersonnelInfoForSendEmailResult> personnelInfoForSendEmails, List<EmailGenerationResult> contentOfEmails)
        {
            List<int> result = new List<int>();

            foreach (PersonnelInfoForSendEmailResult personnelInfoForSendEmail in personnelInfoForSendEmails)
            {
                var content = contentOfEmails.Find(c => c.PersonnelId == personnelInfoForSendEmail.PersonnelId);
                if (content != null)
                {
                    int? idOfPersonnelIfSuccess = SendEmail(personnelInfoForSendEmail.PersonnelId, personnelInfoForSendEmail.Email, content.EmailContent);
                    if (idOfPersonnelIfSuccess.HasValue)
                    {
                        result.Add(idOfPersonnelIfSuccess.Value);
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// Envoie l'email
        /// Renvoie l'id si l'envoie de mail est ok sinon null
        /// </summary>
        /// <param name="id">Personnel ID</param>
        /// <param name="nom">nom</param>
        /// <param name="prenom">prenom</param>
        /// <param name="email">email</param>
        /// <param name="content">content</param>
        /// <returns> renvoie l'id si l'envoie de mail est ok sinon null</returns>      
        public int? SendEmail(int id, string email, string content)
        {
            int? result = id;// renvoie l'id si l'envoie de mail est ok sinon null

            try
            {
                using (SendMail sender = new SendMail())
                {
                    sender.SendFormattedEmail("support.FRED@fayatit.fayat.com", email, BusinessResources.ActivitySummaryServiceSujetDuMail, content, true);
                }
                Log($"Envoie du mail récapitulatif des activitées : {email}.");
            }
            catch (FredTechnicalException e)
            {
                result = null;
                NLog.LogManager.GetCurrentClassLogger().Error(e, $"[EMAIL][RECAPITULATIF_ACTIVITE] Erreur lors de l'envoie du mail récapitulatif des activitées : {email}.");
            }
            return result;
        }

        private void Log(string message)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"[EMAIL][RECAPITULATIF_ACTIVITE] " + message);
        }

        private void Warn(string message)
        {
            NLog.LogManager.GetCurrentClassLogger().Warn($"[EMAIL][RECAPITULATIF_ACTIVITE] " + message);
        }
    }
}
