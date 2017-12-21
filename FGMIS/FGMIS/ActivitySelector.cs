using Domain;
using Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FGMIS
{
    public partial class ActivitySelector : Form
    {
        private Main _main;
        ActivitySelectorHelper activitySelectorHelper = new ActivitySelectorHelper();
        List<Activity> activityList;
        List<ActivityListItem> activityListItem;

        private Form dash=null;

        public ActivitySelector(Main _main)
        {
            InitializeComponent();
            this._main = _main;
        }

        private void ActivitySelector_Load(object sender, EventArgs e)
        {
            panel3.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            activityList = activitySelectorHelper.GetActivityList();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            panel3.Visible = false;
            button1.Enabled = true;
            button2.Enabled = true;
            comboBox1.DataSource = activityList;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            startActivity(comboBox1.SelectedIndex, -1);
        }

        private void startActivity(int index, int activityId)
        {
            if(index==0)
            {
                if(dash!=null && dash.GetType()==typeof(Form1))
                    dash.Activate();
                else
                    showForm(index, activityId);
            } 
            else if (index == 1)
            {
                if (dash != null && dash.GetType() == typeof(Form2))
                    dash.Activate();
                else
                    showForm(index, activityId);
            }
            else if (index == 2)
            {
                if (dash != null && dash.GetType() == typeof(Form3))
                    dash.Activate();
                else
                    showForm(index, activityId);
            }
            else if (index == 3)
            {
                if (dash != null && dash.GetType() == typeof(Form4))
                    dash.Activate();
                else
                    showForm(index, activityId);
            }
            else if (index == 4)
            {
                if (dash != null && dash.GetType() == typeof(Form5))
                    dash.Activate();
                else
                    showForm(index, activityId);
            }
            else if (index == 5)
            {
                if (dash != null && dash.GetType() == typeof(Form6))
                    dash.Activate();
                else
                    showForm(index, activityId);
            }
            else if (index == 6)
            {
                if (dash != null && dash.GetType() == typeof(Form7))
                    dash.Activate();
                else
                    showForm(index, activityId);
            }
            else if (index == 7)
            {
                if (dash != null && dash.GetType() == typeof(Form8))
                    dash.Activate();
                else
                    showForm(index, activityId);
            }
            else if (index == 8)
            {
                if (dash != null && dash.GetType() == typeof(Form9))
                    dash.Activate();
                else
                    showForm(index, activityId);
            }
            else
            {
                MessageBox.Show("Selected index: " + index);
            }

            this.Close();
        }

        private void showForm(int index, int activityId)
        {
             if (dash == null)
            {
                GetMeAForm(index, activityId).Show();
            }
            else
            {
                if (dash.IsDisposed)
                {
                    GetMeAForm(index, activityId).Show();
                }
                else
                    dash.Activate();
            }

        }

        private Form GetMeAForm(int index, int activityId)
        {
            if (index == 0)
                dash = new Form1(comboBox1.Text, activityId);
            else if (index == 1)
                dash = new Form2(comboBox1.Text, activityId);
            else if (index == 2)
                dash = new Form3(comboBox1.Text, activityId);
            else if (index == 3)
                dash = new Form4(comboBox1.Text, activityId);
            else if (index == 4)
                dash = new Form5(comboBox1.Text, activityId);
            else if (index == 5)
                dash = new Form6(comboBox1.Text, activityId);
            else if (index == 6)
                dash = new Form7(comboBox1.Text, activityId);
            else if (index == 7)
                dash = new Form8(comboBox1.Text, activityId);
            else if (index == 8)
                dash = new Form9(comboBox1.Text, activityId);
            else
                dash = new Form1(comboBox1.Text, activityId);

            SetUpForm(dash);
            return dash;
        }

        private void SetUpForm(Form form)
        {
            form.MdiParent = _main;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.ControlBox = false;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowIcon = false;
            form.Text = "";
            form.Dock = DockStyle.Fill;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Activity activity = (Activity)activityList[comboBox1.SelectedIndex];
            //MessageBox.Show(activity.TableName);
            GetData(activity.TableName);
        }

        private void GetData(string tableName)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            if (activitySelectorHelper.TableExists(tableName))
            {
                //MessageBox.Show("Table exists");
                activityListItem = activitySelectorHelper.GetActivityList(tableName);
                //dataGridView1.DataSource = activityList;
                for (int i = 0; i<activityListItem.Count; i++)
                {
                    string[] row = { activityListItem[i].LocalTimeStamp.ToString(), activityListItem[i].Title };
                    dataGridView1.Rows.Add(row);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            { 
                int selectedIndex = dataGridView1.CurrentCell.RowIndex;
                int rowsCount = dataGridView1.Rows.Count;
                //MessageBox.Show(selectedIndex + " | Rows count: " + rowsCount);
                if (rowsCount - 1 != selectedIndex)
                {
                    //showForm1(((ActivityListItem)activityListItem[selectedIndex]).Aid);
                    startActivity(comboBox1.SelectedIndex, ((ActivityListItem)activityListItem[selectedIndex]).Aid);
                }
             }
        }

    }
}
