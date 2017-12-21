using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Participant8 : Participant
    {
        string gradelevel;
        string statusofretention;
        string remark;

        public string Gradelevel
        {
            get { return gradelevel; }
            set { gradelevel = value; }
        }

        public string Statusofretention
        {
            get { return statusofretention; }
            set { statusofretention = value; }
        }

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        
        public Participant8()
        {
            
        }
    }
}
