using Fred.Entities.JobStatut;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using System;

namespace Fred.ImportExport.Business.Common
{
    /// <summary>
    /// Classe utilitaire pour avoir des infos sur les jobs
    /// </summary>
    public static class JobInfoProvider
    {


        /// <summary>
        /// Retourne le status du job en fonction de son Id
        /// Cette methode doit etre préférée.
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <returns>JobStatutModel</returns>
        public static JobStatutModel GetJobStatus(string jobId)
        {
            IStorageConnection connection = JobStorage.Current.GetConnection();
            JobData jobData = connection.GetJobData(jobId);
            return new JobStatutModel()
            {
                IsRunning = jobData.State == ProcessingState.StateName,
                IsEnqueued = jobData.State == EnqueuedState.StateName,
            };
        }

        /// <summary>
        ///  Retourne le status du job en fonction du nom du type de du manager qui execute le job et du nom la methode qui execute le job
        ///  Péférer la methode avec comme parametre le jobid si possible.
        /// </summary>
        /// <param name="jobTypeName"> nom du type de du manager qui execute le job</param>
        /// <param name="jobMethodeName">nom la methode qui execute le job</param>
        /// <param name="predicateSelector">predicateSelector</param>
        /// <returns>JobStatutModel</returns>
        public static JobStatutModel GetJobStatus(string jobTypeName, string jobMethodeName, Predicate<Job> predicateSelector = null)
        {
            var result = new JobStatutModel();
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var processingCount = monitoringApi.ProcessingCount();

            if (processingCount > 0)
            {
                var processingJobs = monitoringApi.ProcessingJobs(0, (int)processingCount);
                var processingResult = GetJobModel(jobTypeName, jobMethodeName, processingJobs, predicateSelector);
                if (processingResult != null)
                {
                    return processingResult;
                }

            }

            var queues = monitoringApi.Queues();
            if (queues.Count > 0)
            {
                foreach (var queue in queues)
                {
                    var name = queue.Name;
                    var enqueuedCount = monitoringApi.EnqueuedCount(name);

                    if (enqueuedCount > 0)
                    {
                        var enqueuedJobs = monitoringApi.EnqueuedJobs(name, 0, (int)enqueuedCount);
                        var enqueuedResult = GetJobModel(jobTypeName, jobMethodeName, enqueuedJobs, predicateSelector);
                        if (enqueuedResult != null)
                        {
                            return enqueuedResult;
                        }
                    }
                }
            }


            return result;
        }

        /// <summary>
        /// Créer un GetJobModel en fonction de la liste des job qui sont en queues
        /// </summary>
        /// <param name="jobTypeName"> nom du type de du manager qui execute le job</param>
        /// <param name="jobMethodeName">nom la methode qui execute le job</param>
        /// <param name="enqueuedJobs">la liste des job qui sont en queues</param>
        /// <param name="predicateSelector">predicateSelector</param>
        /// <returns>JobStatutModel</returns>
        private static JobStatutModel GetJobModel(string jobTypeName, string jobMethodeName, JobList<EnqueuedJobDto> enqueuedJobs, Predicate<Job> predicateSelector = null)
        {
            var jobStatutModel = new JobStatutModel();

            foreach (var processingJob in enqueuedJobs)
            {
                var job = GetJob(jobTypeName, jobMethodeName, processingJob.Value.Job, predicateSelector);
                if (job != null)
                {
                    jobStatutModel.IsEnqueued = true;
                    return jobStatutModel;
                }

            }
            return null;
        }

        /// <summary>
        /// Créer un GetJobModel en fonction de la liste des job qui sont en cours
        /// </summary>
        /// <param name="jobTypeName"> nom du type de du manager qui execute le job</param>
        /// <param name="jobMethodeName">nom la methode qui execute le job</param>
        /// <param name="processingJobs">la liste des job qui sont en cours</param>
        /// <param name="predicateSelector">predicateSelector</param>
        /// <returns>JobStatutModel</returns>
        private static JobStatutModel GetJobModel(string jobTypeName, string jobMethodeName, JobList<ProcessingJobDto> processingJobs, Predicate<Job> predicateSelector = null)
        {
            var jobStatutModel = new JobStatutModel();

            foreach (var processingJob in processingJobs)
            {
                var job = GetJob(jobTypeName, jobMethodeName, processingJob.Value.Job, predicateSelector);
                if (job != null)
                {
                    jobStatutModel.IsRunning = true;
                    return jobStatutModel;
                }

            }
            return null;
        }


        private static Job GetJob(string jobTypeName, string jobMethodeName, Job job, Predicate<Job> predicateSelector = null)
        {
            if (job.Type.Name == jobTypeName && job.Method.Name == jobMethodeName)
            {
                if (predicateSelector != null)
                {
                    if (predicateSelector(job))
                    {
                        return job;
                    }
                }
                else
                {
                    return job;
                }

            }
            return null;
        }

    }
}

