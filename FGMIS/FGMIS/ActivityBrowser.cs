using Domain;
using Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FGMIS
{
    public partial class ActivityBrowser : Form
    {
        private Form _parent;
        private int outputId;
        private Button sender;

        ActivitySelectorHelper activitySelectorHelper = new ActivitySelectorHelper();
        List<Activity> activityList;
        List<Activity0> activity0List=null;
        List<string> tableNames = new List<string>();
        List<string> participantsTableNames = new List<string>();
        bool tableExists = false;

        List<int> activityIds=new List<int>();

        public ActivityBrowser(Form _parent, int outputId)
        {
            InitializeComponent();
            this._parent = _parent;
            this.outputId = outputId;
            for (int i = 1; i <= 10; i++)
                tableNames.Add("activity" + i);
            for (int i = 1; i <= 10; i++)
                participantsTableNames.Add("participants" + i);
        }

        private void ActivityBrowser_Load(object sender, EventArgs e)
        {
            panel5.Visible = true;
            button5.Enabled = false;
            button3.Enabled = false;
            
            activityBackgroundWorker.RunWorkerAsync();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string activity = comboBox2.Text;
            string plannedM = numericUpDown1.Value.ToString();
            string plannedF = numericUpDown2.Value.ToString();
            string actualM = numericUpDown3.Value.ToString();
            string actualF = numericUpDown4.Value.ToString();
            string results=textBox1.Text;
            string deviation = textBox2.Text;

            string[] row = { (_parent as Report1).GetDataGridView(outputId).Rows.Count + "", activity, plannedM, plannedF,actualM,actualF,results,deviation,activityIds[comboBox2.SelectedIndex]+"" };
            (_parent as Report1).AddDataToGridView(row, outputId);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void activityBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            activityList = activitySelectorHelper.GetActivityList();
        }

        private void activityBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //populate second combobox
            panel5.Visible = false;
            button5.Enabled = true;
            button3.Enabled = true;
            comboBox1.DataSource = activityList;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (activityIds != null)
                activityIds.Clear();
            PopulateComboBox((sender as ComboBox).SelectedIndex);
        }

        private void PopulateComboBox(int index)
        {
            ActivitySelectorHelper activitySelectorHelper = new ActivitySelectorHelper();
            tableExists = activitySelectorHelper.TableExists(tableNames[index]);
            if (tableExists)
            {
                //EnableFields(true);
                //MessageBox.Show(Properties.Settings.Default.USERORGANIZATION + " : " + Properties.Settings.Default.USERPARTNER);
                List<string> activityAsString = new List<string>();
                ActivityHelper activityHelper = new ActivityHelper();
                activity0List = activityHelper.GetActivityListByPartner(tableNames[index], Properties.Settings.Default.USERORGANIZATION, Properties.Settings.Default.USERPARTNER);
                if (activity0List != null)
                {
                    if (activity0List.Count > 0)
                    {
                        EnableFields(true);
                        for (int i = 0; i < activity0List.Count; i++)
                        {
                            Activity0 activity = activity0List[i];
                            string activityString = "[Activity "+index+" ~ ID: "+activity.RemoteId+"] ~ On " + activity.ActivityDate + ": " + activity.Region + ", " + activity.Zone + ", " + activity.Woreda + ", " + activity.Kebele;
                            activityAsString.Add(activityString);
                            activityIds.Add(activity.RemoteId);
                        }
                        comboBox2.DataSource = activityAsString;
                    }
                    else
                    {
                        EnableFields(false);
                    }
                }
            }
            else
            {
                MessageBox.Show("Table does not exist!");
                EnableFields(false);
            }
            
        }

        private void EnableFields(bool flag)
        {
            panel3.Enabled = flag;
            panel4.Enabled = flag;
            button5.Enabled = flag;
            groupBox2.Enabled = flag;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (activity0List != null)
            {
                
                    EnableFields(true);
                    ActivityHelper activityHelper = new ActivityHelper();
                    int index = comboBox1.SelectedIndex;//parent combo box
                    int index2 = comboBox2.SelectedIndex;//child combo box
                    List<int> maleFemaleDistribution = activityHelper.GetMaleFemaleParticipantsCount(participantsTableNames[index], activity0List[index2].RemoteId);
                    numericUpDown3.Value = maleFemaleDistribution[0];
                    numericUpDown4.Value = maleFemaleDistribution[1];
                    //MessageBox.Show(maleFemaleDistribution[0] + " : " + maleFemaleDistribution[1] + " ID: " + activity0List[index2].RemoteId);
                
            }
        }
    }
}
