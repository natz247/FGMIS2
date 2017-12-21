using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Participant
    {
        int pid;
        int remoteid;
        string name;
        string kebele;
        string woreda;
        string sex;
        int age;
        int disabled;
        int activityId;
        //DateTime localTimeStamp;
        //DateTime remoteTimeStamp;
        //string mac;
        //int syncStatus;

        public int Pid
        {
            get
            {
                return pid;
            }

            set
            {
                pid = value;
            }
        }

        public int Remoteid
        {
            get
            {
                return remoteid;
            }

            set
            {
                remoteid = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
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

        public string Sex
        {
            get
            {
                return sex;
            }

            set
            {
                sex = value;
            }
        }

        public int Age
        {
            get
            {
                return age;
            }

            set
            {
                age = value;
            }
        }

        /*public DateTime LocalTimeStamp
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
        }*/

        public int ActivityId
        {
            get
            {
                return activityId;
            }

            set
            {
                activityId = value;
            }
        }

        public int Disabled
        {
            get
            {
                return disabled;
            }

            set
            {
                disabled = value;
            }
        }

        public Participant(int pid, int remoteid, string name, string kebele, string woreda, string sex, int age, int disabled, int activityId, DateTime localTimeStamp, DateTime remoteTimeStamp, string mac, int syncStatus)
        {
            this.Pid = pid;
            this.Remoteid = remoteid;
            this.Name = name;
            this.Kebele = kebele;
            this.Woreda = woreda;
            this.Sex = sex;
            this.Age = age;
            this.Disabled = disabled;
            this.ActivityId = activityId;
            //this.LocalTimeStamp = localTimeStamp;
            //this.RemoteTimeStamp = remoteTimeStamp;
            //this.Mac = mac;
            //this.SyncStatus = syncStatus;
        }

        public Participant()
        {
            
        }
    }
}
