using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ActivityListItem
    {
        int aid;
        DateTime localTimeStamp;
        string title;
        string tableName;

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

        public DateTime LocalTimeStamp
        {
            get
            {
                return localTimeStamp;
            }

            set
            {
                localTimeStamp = value;
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

        public ActivityListItem()
        {
        }

        public ActivityListItem(int aid, DateTime localTimeStamp, string title, string tableName)
        {
            this.Aid = aid;
            this.LocalTimeStamp = localTimeStamp;
            this.Title = title;
            this.TableName = tableName;
        }

        public override string ToString()
        {
            return Title;
        }

    }
}
