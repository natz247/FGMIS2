using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Participant7 : Participant
    {
        string groupname;
        int noofadolescent;
        int noofwomen;
        string typeoftraining;
        string capital;
        string typeofiga;
        string statusofiga;
        string amountofsaving;
        string remark;

        public string Groupname
        {
            get { return groupname; }
            set { groupname = value; }
        }

        public int Noofadolescent
        {
            get { return noofadolescent; }
            set { noofadolescent = value; }
        }

        public int Noofwomen
        {
            get { return noofwomen; }
            set { noofwomen = value; }
        }

        public string Typeoftraining
        {
            get { return typeoftraining; }
            set { typeoftraining = value; }
        }

        public string Capital
        {
            get { return capital; }
            set { capital = value; }
        }

        public string Typeofiga
        {
            get { return typeofiga; }
            set { typeofiga = value; }
        }

        public string Statusofiga
        {
            get { return statusofiga; }
            set { statusofiga = value; }
        }

        public string Amountofsaving
        {
            get { return amountofsaving; }
            set { amountofsaving = value; }
        }

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        
        public Participant7()
        {
            
        }
    }
}
