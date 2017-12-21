using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Activity0
    {
        int aid;
        string region;
        string zone;
        string woreda;
        string kebele;
        DateTime activityDate;
        DateTime submissionDate;
        string facilitatorName;
        string position;

        string issuesRaised;
        string agreedActionPoints;

        int userId;
        int submitStatus;
        DateTime localTimeStamp;
        string mac;
        int remoteId;
        DateTime remoteTimeStamp;
        int syncStatus;

        public int Aid
        {
            get
            {
                return aid;
            }

            set
            {
                aid = value;
            }
        }

        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
            }
        }

        public string Zone
        {
            get
            {
                return zone;
            }

            set
            {
                zone = value;
            }
        }

        public string Woreda
        {
            get
            {
                return woreda;
            }

            set
            {
                woreda = value;
            }
        }

        public string Kebele
        {
            get
            {
                return kebele;
            }

            set
            {
                kebele = value;
            }
        }

        public DateTime ActivityDate
        {
            get
            {
                return activityDate;
            }

            set
            {
                activityDate = value;
            }
        }

        public DateTime SubmissionDate
        {
            get
            {
                return submissionDate;
            }

            set
            {
                submissionDate = value;
            }
        }

        public int UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

        public int SubmitStatus
        {
            get
            {
                return submitStatus;
            }

            set
            {
                submitStatus = value;
            }
        }

        public DateTime LocalTimeStamp
        {
            get
            {
                return localTimeStamp;
            }

            set
            {
                localTimeStamp = value;
            }
        }

        public string Mac
        {
            get
            {
                return mac;
            }

            set
            {
                mac = value;
            }
        }

        public int RemoteId
        {
            get
            {
                return remoteId;
            }

            set
            {
                remoteId = value;
            }
        }

        public DateTime RemoteTimeStamp
        {
            get
            {
                return remoteTimeStamp;
            }

            set
            {
                remoteTimeStamp = value;
            }
        }

        public int SyncStatus
        {
            get
            {
                return syncStatus;
            }

            set
            {
                syncStatus = value;
            }
        }

        public string FacilitatorName
        {
            get
            {
                return facilitatorName;
            }

            set
            {
                facilitatorName = value;
            }
        }

        public string Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public string IssuesRaised
        {
            get
            {
                return issuesRaised;
            }

            set
            {
                issuesRaised = value;
            }
        }

        public string AgreedActionPoints
        {
            get
            {
                return agreedActionPoints;
            }

            set
            {
                agreedActionPoints = value;
            }
        }

        public Activity0()
        {
        }

        public Activity0(int aid, string region, string zone, string woreda, string kebele, DateTime activityDate, DateTime submissionDate, int userId, int submitStatus, DateTime localTimeStamp, string mac, int remoteId, DateTime remoteTimeStamp, int syncStatus)
        {
            this.Aid = aid;
            this.Region = region;
            this.Zone = zone;
            this.Woreda = woreda;
            this.Kebele = kebele;
            this.ActivityDate = activityDate;
            this.SubmissionDate = submissionDate;
            this.UserId = userId;
            this.SubmitStatus = submitStatus;
            this.LocalTimeStamp = localTimeStamp;
            this.Mac = mac;
            this.RemoteId = remoteId;
            this.RemoteTimeStamp = remoteTimeStamp;
            this.SyncStatus = syncStatus;
        }
    }
}
