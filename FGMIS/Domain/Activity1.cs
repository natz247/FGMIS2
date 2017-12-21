using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Activity1 : Activity0
    {
        int aid;
        string place;
        string duration;

        public string Place
        {
            get
            {
                return place;
            }

            set
            {
                place = value;
            }
        }

        public string Duration
        {
            get
            {
                return duration;
            }

            set
            {
                duration = value;
            }
        }

        public Activity1(int aid, string place, string duration)
        {
            this.Aid = aid;
            this.Place = place;
            this.Duration = duration;
        }
        public Activity1()
        {
            
        }
    }
}
