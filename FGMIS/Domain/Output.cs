using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Output
    {
        private int oid;
        private string title;
        private int type;
        private int activityid;
        private int reportid;
        private int plannedm;
        private int plannedf;
        private int actualm;
        private int actualf;
        private string results;
        private string deviation;

        public Output()
        {

        }

        public int Oid
        {
            get { return oid; }
            set { oid = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public int Activityid
        {
            get { return activityid; }
            set { activityid = value; }
        }

        public int Reportid
        {
            get { return reportid; }
            set { reportid = value; }
        }

        public int Plannedm
        {
            get { return plannedm; }
            set { plannedm = value; }
        }

        public int Plannedf
        {
            get { return plannedf; }
            set { plannedf = value; }
        }

        public int Actualm
        {
            get { return actualm; }
            set { actualm = value; }
        }

        public int Actualf
        {
            get { return actualf; }
            set { actualf = value; }
        }

        public string Results
        {
            get { return results; }
            set { results = value; }
        }

        public string Deviation
        {
            get { return deviation; }
            set { deviation = value; }
        }


    }
}
