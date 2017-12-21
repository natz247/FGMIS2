using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Participant10 : Participant
    {
        string girlcode;
        string background;
        string reson;
        string referredto;
        string referredfrom;
        string feedback;

        public string Girlcode
        {
            get { return girlcode; }
            set { girlcode = value; }
        }

        public string Background
        {
            get { return background; }
            set { background = value; }
        }

        public string Reson
        {
            get { return reson; }
            set { reson = value; }
        }

        public string Referredto
        {
            get { return referredto; }
            set { referredto = value; }
        }

        public string Referredfrom
        {
            get { return referredfrom; }
            set { referredfrom = value; }
        }

        public string Feedback
        {
            get { return feedback; }
            set { feedback = value; }
        }
        public Participant10()
        {
            
        }
    }
}
