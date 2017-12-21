using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Activity4 : Activity0
    {
        string stage;
        string ccfacilitatorname;
        string duration;

        
        public string Stage
        {
            get { return stage; }
            set { stage = value; }
        }

        public string Ccfacilitatorname
        {
            get { return ccfacilitatorname; }
            set { ccfacilitatorname = value; }
        }

        public string Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        
        public Activity4()
        {

        }
        
        
    }
}
