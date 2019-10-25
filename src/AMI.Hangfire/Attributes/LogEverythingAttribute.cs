using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace AMI.Hangfire.Attributes
{
    /// <summary>
    /// An attribute used to log everything related to Hangfire jobs.
    /// </summary>
    public class LogEverythingAttribute : JobFilterAttribute,
        IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        /// <inheritdoc/>
        public void OnCreating(CreatingContext context)
        {
            Logger.InfoFormat("Creating a job based on method `{0}`...", context.Job.Method.Name);
        }

        /// <inheritdoc/>
        public void OnCreated(CreatedContext context)
        {
            Logger.InfoFormat(
                "Job that is based on method `{0}` has been created with id `{1}`",
                context.Job.Method.Name,
                context.BackgroundJob?.Id);
        }

        /// <inheritdoc/>
        public void OnPerforming(PerformingContext context)
        {
            Logger.InfoFormat("Starting to perform job `{0}`", context.BackgroundJob.Id);
        }

        /// <inheritdoc/>
        public void OnPerformed(PerformedContext context)
        {
            Logger.InfoFormat("Job `{0}` has been performed", context.BackgroundJob.Id);
        }

        /// <inheritdoc/>
        public void OnStateElection(ElectStateContext context)
        {
            if (context.CandidateState is FailedState failedState)
            {
                Logger.WarnFormat(
                    "Job `{0}` has been failed due to an exception `{1}`",
                    context.BackgroundJob.Id,
                    failedState.Exception);
            }
        }

        /// <inheritdoc/>
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Logger.InfoFormat(
                "Job `{0}` state was changed from `{1}` to `{2}`",
                context.BackgroundJob.Id,
                context.OldStateName,
                context.NewState.Name);
        }

        /// <inheritdoc/>
        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Logger.InfoFormat(
                "Job `{0}` state `{1}` was unapplied.",
                context.BackgroundJob.Id,
                context.OldStateName);
        }
    }
}
