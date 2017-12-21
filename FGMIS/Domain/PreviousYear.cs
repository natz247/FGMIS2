using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PreviousYear
    {
        private int pid;

        public int Pid
        {
            get { return pid; }
            set { pid = value; }
        }
        private int year;
        private int output11;
        private int output12;
        private int output13;
        private int output21;
        private int output22;
        private int output23;
        private int output24;
        private int output25;
        private int output31;
        private int output32;
        private int uid;

        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public int Output11
        {
            get { return output11; }
            set { output11 = value; }
        }

        public int Output12
        {
            get { return output12; }
            set { output12 = value; }
        }

        public int Output13
        {
            get { return output13; }
            set { output13 = value; }
        }

        public int Output21
        {
            get { return output21; }
            set { output21 = value; }
        }

        public int Output22
        {
            get { return output22; }
            set { output22 = value; }
        }

        public int Output23
        {
            get { return output23; }
            set { output23 = value; }
        }

        public int Output24
        {
            get { return output24; }
            set { output24 = value; }
        }

        public int Output25
        {
            get { return output25; }
            set { output25 = value; }
        }

        public int Output31
        {
            get { return output31; }
            set { output31 = value; }
        }

        public int Output32
        {
            get { return output32; }
            set { output32 = value; }
        }

        public PreviousYear()
        {

        }

        public PreviousYear(int year)
        {
            this.year = year;
        }
    }
}
