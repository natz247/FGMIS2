using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Activity
    {
        int aid;
        string title;
        string tableName;

        public Activity()
        {
        }
        public Activity(int aid, string title)
        {
            this.aid = aid;
            this.title = title;
        }

        public int Aid
        {
            get
            {
                return aid;
            }

            set
            {
                aid = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public string TableName
        {
            get
            {
                return tableName;
            }

            set
            {
                tableName = value;
            }
        }

        public override string ToString()
        {
            return Title;
        }

    }
}
