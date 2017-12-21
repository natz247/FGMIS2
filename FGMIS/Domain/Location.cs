using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Location
    {
        int lid;
        string name;
        int uid;

       
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        public int Lid
        {
            get { return lid; }
            set { lid = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Location(string name, int uid)
        {
            this.name = name;
            this.uid = uid;
        }

    }
}
