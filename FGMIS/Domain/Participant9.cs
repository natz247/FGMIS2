using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Participant9 : Participant
    {
        int noofadolescent;
        int noofwomen;
        string typeofmaterial;
        string unit;
        int quantity;

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

        public string Typeofmaterial
        {
            get { return typeofmaterial; }
            set { typeofmaterial = value; }
        }

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        
        public Participant9()
        {
            
        }
    }
}
