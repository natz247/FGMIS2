using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Participant3 : Participant
    {
        string organization;
        string position;

        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }

        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        
        public Participant3()
        {
            
        }
    }
}
