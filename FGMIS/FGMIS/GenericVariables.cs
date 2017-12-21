using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGMIS
{
    public class GenericVariables
    {
        public GenericVariables()
        {

        }
        public int GetUID()
        {
            return Properties.Settings.Default.UID;
        }
    }
}
