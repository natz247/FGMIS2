using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Activity2 : Activity0
    {
        string schoolname;
        string clubname;
        string clubleadername;
        public string Clubleadername
        {
            get { return clubleadername; }
            set { clubleadername = value; }
        }

        public string Schoolname
        {
            get { return schoolname; }
            set { schoolname = value; }
        }

        public string Clubname
        {
            get { return clubname; }
            set { clubname = value; }
        }

        
        public Activity2()
        {
            
        }
    }
}
