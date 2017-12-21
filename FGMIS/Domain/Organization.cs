using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Organization
    {
        int oid;
        string name;
        string address;
        string phone1;
        string phone2;
        string email;
        string website;

        DateTime localTimeStamp;
        int remoteid;
        DateTime remoteTimeStamp;
        string mac;
        int syncStatus;
        int uid;
        int canbedeleted;

        public int Canbedeleted
        {
            get { return canbedeleted; }
            set { canbedeleted = value; }
        }

        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        public int Oid
        {
            get { return oid; }
            set { oid = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Phone1
        {
            get { return phone1; }
            set { phone1 = value; }
        }

        public string Phone2
        {
            get { return phone2; }
            set { phone2 = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Website
        {
            get { return website; }
            set { website = value; }
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


    }
}
