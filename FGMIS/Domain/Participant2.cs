using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Participant2 : Participant
    {
        string gradelevel;

        public string Gradelevel
        {
            get { return gradelevel; }
            set { gradelevel = value; }
        }
        public Participant2()
        {
            
        }
    }
}
