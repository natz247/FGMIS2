using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Participant5 : Participant
    {
        DateTime pdate;
        string code;
        string town;
        DateTime dateofvisit;
        string identifiedcase;
        int referralhealth;
        int referralpolice;
        int referrallegal;
        int referralpsycho;
        string others;

        public DateTime Pdate
        {
            get { return pdate; }
            set { pdate = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Town
        {
            get { return town; }
            set { town = value; }
        }

        public DateTime Dateofvisit
        {
            get { return dateofvisit; }
            set { dateofvisit = value; }
        }

        public string Identifiedcase
        {
            get { return identifiedcase; }
            set { identifiedcase = value; }
        }

        public int Referralhealth
        {
            get { return referralhealth; }
            set { referralhealth = value; }
        }

        public int Referralpolice
        {
            get { return referralpolice; }
            set { referralpolice = value; }
        }

        public int Referrallegal
        {
            get { return referrallegal; }
            set { referrallegal = value; }
        }

        public int Referralpsycho
        {
            get { return referralpsycho; }
            set { referralpsycho = value; }
        }

        public string Others
        {
            get { return others; }
            set { others = value; }
        }

        public string GetReferrals()
        {
            string referrals = "";
            if (Referralhealth == 1)
                referrals += "Health facilities, ";
            if (Referralpolice == 1)
                referrals += "Police, ";
            if (Referrallegal == 1)
                referrals += "Legal aid, ";
            if (Referralpsycho == 1)
                referrals += "Psyco-social service, ";

            return referrals;
        }

        
        public Participant5()
        {
            
        }
    }
}
