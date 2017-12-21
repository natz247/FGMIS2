using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Activity3 : Activity0
    {
        string trainingtitle;
        string trainingfacilitator;

        public string Trainingtitle
        {
            get { return trainingtitle; }
            set { trainingtitle = value; }
        }

        public string Trainingfacilitator
        {
            get { return trainingfacilitator; }
            set { trainingfacilitator = value; }
        }
        public Activity3()
        {

        }
        
        
    }
}
