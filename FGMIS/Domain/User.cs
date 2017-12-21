using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        int uid;
        int remoteid;
        string firstName;
        string lastName;
        string userName;
        string password;
        int accessLevel;
        int activeStatus;
        string email;
        string phone;
        DateTime localTimeStamp;
        DateTime remoteTimeStamp;
        string mac;
        int syncStatus;
        string organization;
        string partner;
        int uuid;

        public int Uuid
        {
            get { return uuid; }
            set { uuid = value; }
        }

        public User(int uid, int remoteid, string firstName, string lastName, string userName, string password, int accessLevel, int activeStatus, DateTime localTimeStamp, DateTime remoteTimeStamp, string mac, int syncStatus)
        {
            this.uid = uid;
            this.remoteid = remoteid;
            this.firstName = firstName;
            this.lastName = lastName;
            this.userName = userName;
            this.password = password;
            this.accessLevel = accessLevel;
            this.activeStatus = activeStatus;
            this.localTimeStamp = localTimeStamp;
            this.remoteTimeStamp = remoteTimeStamp;
            this.mac = mac;
            this.syncStatus = syncStatus;
        }

        public User()
        {

        }

        public int Uid
        {
            get
            {
                return uid;
            }

            set
            {
                uid = value;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                lastName = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public int AccessLevel
        {
            get
            {
                return accessLevel;
            }

            set
            {
                accessLevel = value;
            }
        }

        public int ActiveStatus
        {
            get
            {
                return activeStatus;
            }

            set
            {
                activeStatus = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public string Phone
        {
            get
            {
                return phone;
            }

            set
            {
                phone = value;
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
        public string Organization
        {
            get
            {
                return organization;
            }

            set
            {
                organization = value;
            }
        }
        public string Partner
        {
            get
            {
                return partner;
            }

            set
            {
                partner = value;
            }
        }
    }
}
