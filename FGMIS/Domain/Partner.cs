using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Partner : Organization
    {
        int organizationid;

        public int Organizationid
        {
            get { return organizationid; }
            set { organizationid = value; }
        }
        
        public Partner()
        {

        }
    }
}
