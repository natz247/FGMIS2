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
    public partial class ReportSelector : Form
    {
        private Main _main;
        ReportSelectorHelper reportSelectorHelper = new ReportSelectorHelper();
        List<Report> reportList;
        List<ReportListItem> reportListItem;

        private Form dash = null;
        public ReportSelector(Main _main)
        {
            InitializeComponent();
            this._main = _main;
        }

        private void ReportSelector_Load(object sender, EventArgs e)
        {
            panel3.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            reportList = reportSelectorHelper.GetReportList();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            panel3.Visible = false;
            button1.Enabled = true;
            button2.Enabled = true;
            comboBox1.DataSource = reportList;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            startActivity(comboBox1.SelectedIndex, -1);
        }
        private void startActivity(int index, int reportId)
        {
            if (index == 0)
            {
                if (dash != null && dash.GetType() == typeof(Report1))
                    dash.Activate();
                else
                    showReport1(reportId);
            }
            else
            {
                MessageBox.Show("Selected index: " + index);
            }

            this.Close();
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
        private void showReport1(int reportId)
        {
            if (dash == null)
            {
                dash = new Report1(comboBox1.Text, reportId);
                SetUpForm(dash);
                dash.Show();
            }
            else
            {
                if (dash.IsDisposed)
                {
                    dash = new Report1(comboBox1.Text, reportId);
                    SetUpForm(dash);
                    dash.Show();
                }
                else
                    dash.Activate();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Report report = (Report)reportList[comboBox1.SelectedIndex];
            //MessageBox.Show(activity.TableName);
            GetData(report.TableName);
        }

        private void GetData(string tableName)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            if (reportSelectorHelper.TableExists(tableName))
            {
                //MessageBox.Show("Table exists");
                reportListItem = reportSelectorHelper.GetActivityList(tableName);
                //dataGridView1.DataSource = activityList;
                for (int i = 0; i < reportListItem.Count; i++)
                {
                    string[] row = { reportListItem[i].LocalTimeStamp.ToString(), reportListItem[i].Title };
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
                    startActivity(0, ((ReportListItem)reportListItem[selectedIndex]).Aid);
                }
            }
        }
    }
}
