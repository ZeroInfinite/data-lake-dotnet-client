﻿using System;
using MSADLA = Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Models
{
    public class JobInfo
    {
        // things that identify the job
        public readonly Guid JobId;
        public readonly AnalyticsAccountRef Account;

        // Information about the job provided by user
        public readonly string Name;
        public readonly int? DegreeOfParallelism;
        public readonly int? Priority;
        public readonly string Submitter;
        public readonly MSADLA.Models.JobType Type;

        // State and Timings of the Job
        public readonly MSADLA.Models.JobResult? Result;
        public readonly MSADLA.Models.JobState? State ;
        public readonly DateTimeOffset? SubmitTime;
        public readonly DateTimeOffset? StartTime;
        public readonly DateTimeOffset? EndTime;

        // Deubgging Resources
        public readonly string LogFolder;

        public TimeSpan? ExecutionDuration
        {
            get
            {
                if (this.StartTime.HasValue && this.EndTime.HasValue)
                {
                    return this.EndTime.Value - this.StartTime.Value;
                }
                return null;
            }
        }

        public double? AUSeconds
        {
            get
            {
                var dur = this.ExecutionDuration;
                if (this.DegreeOfParallelism.HasValue && dur.HasValue)
                {
                    return (this.DegreeOfParallelism.Value * dur.Value.TotalSeconds);
                }
                return null;
            }
        }


        public JobRef GetJobReference()
        {
            return new JobRef(this.JobId, this.Account);
        }       

        internal JobInfo(MSADLA.Models.JobInformation job, AnalyticsAccountRef acct)
        {
            this.Account = acct;
            this.Name = job.Name;
            this.LogFolder = job.LogFolder;
            this.DegreeOfParallelism = job.DegreeOfParallelism;
            this.EndTime = job.EndTime;
            this.JobId = job.JobId.Value;
            this.Priority = job.Priority;
            this.Result = job.Result;
            this.StartTime = job.StartTime;
            this.State = job.State;
            this.SubmitTime = job.SubmitTime;
            this.Type = job.Type;
            this.Submitter = job.Submitter;
        }
    }
}
 