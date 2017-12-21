using Domain;
using Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FGMIS
{
    public partial class Report1 : Form
    {
        private string title;
        private int reportId;
        private DataTable partnerTable = null;


        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font myFont, myFont2;

        private string originalValue="";

        private List<DataGridView> output1=new List<DataGridView>();
        private List<DataGridView> output2 = new List<DataGridView>();
        private List<DataGridView> output3 = new List<DataGridView>();
        ReportClass1 report; 
        List<Output> outputsList;

        public Report1(string title, int reportId)
        {
            InitializeComponent();
            this.title = title;
            this.reportId = reportId;


            byte[] fontData = Properties.Resources.Renogare;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.Renogare.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.Renogare.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 14.0F);
        }

        private void Report1_Load(object sender, EventArgs e)
        {
            textBox4.Text = "0"; 
            textBox3.Text = "0";
            textBox6.Text = "0.00";
            label1.Font = myFont;

            output1.Add(dataGridView1);
            output1.Add(dataGridView2);
            output1.Add(dataGridView3);

            output2.Add(dataGridView6);
            output2.Add(dataGridView5);
            output2.Add(dataGridView4);
            output2.Add(dataGridView7);
            output2.Add(dataGridView8);

            output3.Add(dataGridView11);
            output3.Add(dataGridView10);

            PopulateComboBox();
            initialBackgroundWorker.RunWorkerAsync();
        }

        private void PopulateComboBox()
        {
            comboBox2.DataSource = Enumerable.Range(2016, 5).ToList();
            comboBox2.SelectedIndex = comboBox2.Items.IndexOf(DateTime.Now.Year);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void initialBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GenericHelper genericHelper = new GenericHelper();
            //partnerTable = genericHelper.GetRemoteList("Partner");
            partnerTable = genericHelper.GetList("Partner");
        }
        private void BindComboBox(ComboBox comboBox, DataTable dataTable)
        {
            comboBox.AutoCompleteMode = AutoCompleteMode.None;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox.DataSource = dataTable;
            comboBox.BindingContext = this.BindingContext;
            comboBox.DisplayMember = "title";
            comboBox.ValueMember = "title";
        }

        private void initialBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BindComboBox(comboBox1, partnerTable);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string partner = (comboBox1.Text).Trim();
            string pyear = (comboBox2.Text).Trim();
            string title = (textBox1.Text).Trim();
            string period = (textBox2.Text).Trim();
            string tpopulation = (textBox3.Text).Trim();
            string apopulation = (textBox4.Text).Trim();
            string plocation = (textBox5.Text).Trim();
            string budget = (textBox6.Text).Trim();
            string objective = (textBox7.Text).Trim();
            string goal = (textBox8.Text).Trim();
            string strategies = (textBox9.Text).Trim();
            string tgroups = (textBox10.Text).Trim();
            string description = (textBox12.Text).Trim();

            string participation = (textBox16.Text).Trim();
            string coordination = (textBox11.Text).Trim();
            string challenges = (textBox13.Text).Trim();
            string stories = (textBox14.Text).Trim();
            string engagement = (textBox15.Text).Trim();
            string issues = (textBox17.Text).Trim();
            string sustainability = (textBox18.Text).Trim();
            string risks = (textBox19.Text).Trim();
            string accountability = (textBox20.Text).Trim();
            string monitoring = (textBox21.Text).Trim();

            if(ValidateFields())
            {
                //save report data
                report = new ReportClass1();
                report.Partner = partner;
                report.Pyear = Convert.ToInt32(pyear);
                report.Title = title;
                report.Period = period;
                report.Tpopulation = Convert.ToInt32(tpopulation);
                report.Apopulation = Convert.ToInt32(apopulation);
                report.Plocation = plocation;
                report.Budget = budget;
                report.Objective = objective;
                report.Goal = goal;
                report.Strategies = strategies;
                report.Tgroups = tgroups;
                report.Description = description;
                report.Participation = participation;
                report.Coordination = coordination;
                report.Challenges = challenges;
                report.Stories = stories;
                report.Engagement = engagement;
                report.Issues = issues;
                report.Sustainability = sustainability;
                report.Risks = risks;
                report.Accountability = accountability;
                report.Monitoring = monitoring;

                report.Uid=Properties.Settings.Default.UID;
                report.Localtimestamp=DateTime.Now;
                report.Mac=GetMacAddress();

                outputsList = new List<Output>();
                ExtractDataGridViewData(output1, outputsList, 1);
                ExtractDataGridViewData(output2, outputsList, 2);
                ExtractDataGridViewData(output3, outputsList, 3);

                panel28.Visible = true;
                scrollabalePanel.Enabled = false;
                reportInsertBackgroundWorker.RunWorkerAsync();
            }
        }




        private void ExtractDataGridViewData(List<DataGridView> dataGridViewList, List<Output> bigOutputList, int multiplier)
        {
            for (int i = 0; i < dataGridViewList.Count; i++)
            {
                int outputType = (multiplier*10) + (i + 1);
                DataGridView currentDataGridView = dataGridViewList[i];

                if (currentDataGridView.RowCount > 1)
                    for (int j = 0; j < currentDataGridView.Rows.Count - 1; j++)
                    {
                        Output output = new Output();
                        output.Type = outputType;
                        if (currentDataGridView.Rows[j].Cells[1].Value != null)
                            output.Title = currentDataGridView.Rows[j].Cells[1].Value.ToString();
                        if (currentDataGridView.Rows[j].Cells[2].Value != null)
                            output.Plannedm = Convert.ToInt32(currentDataGridView.Rows[j].Cells[2].Value.ToString());
                        if (currentDataGridView.Rows[j].Cells[3].Value != null)
                            output.Plannedf = Convert.ToInt32(currentDataGridView.Rows[j].Cells[3].Value.ToString());
                        if (currentDataGridView.Rows[j].Cells[4].Value != null)
                            output.Actualm = Convert.ToInt32(currentDataGridView.Rows[j].Cells[4].Value.ToString());
                        if (currentDataGridView.Rows[j].Cells[5].Value != null)
                            output.Actualf = Convert.ToInt32(currentDataGridView.Rows[j].Cells[5].Value.ToString());
                        if (currentDataGridView.Rows[j].Cells[6].Value != null)
                            output.Results = currentDataGridView.Rows[j].Cells[6].Value.ToString();
                        if (currentDataGridView.Rows[j].Cells[7].Value != null)
                            output.Deviation = currentDataGridView.Rows[j].Cells[7].Value.ToString();
                        if (currentDataGridView.Rows[j].Cells[8].Value != null)
                            output.Activityid = Convert.ToInt32(currentDataGridView.Rows[j].Cells[8].Value.ToString());

                        bigOutputList.Add(output);

                    }
            }
        }


        public static string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)
                {
                    sMacAddress = adapter.GetPhysicalAddress().ToString();

                }
            }
            return sMacAddress;
        }

        private Boolean validator(string text, string errorMessage)
        {
            //string thisText = text.Trim();
            if (text.CompareTo(string.Empty) == 0)
            {
                MessageBox.Show("Please enter a valid " + errorMessage + "!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                return true;
        }


        private bool ValidateFields()
        {

            string partnerName = (comboBox1.Text).Trim();
            string projectYear = (comboBox2.Text).Trim();
            string projectTitle = (textBox1.Text).Trim();
            string projectPeriod = (textBox2.Text).Trim();
            string projectTarget = (textBox3.Text).Trim();
            string projectArea = (textBox4.Text).Trim();
            string partnerLocation = (textBox5.Text).Trim();
            string projectBudget = (textBox6.Text).Trim();
            string objective = (textBox7.Text).Trim();
            string goal = (textBox8.Text).Trim();
            string strategies = (textBox9.Text).Trim();
            string targetGroups = (textBox10.Text).Trim();
            string description = (textBox12.Text).Trim();

            if (!validator(partnerName, "Partner Name"))
            {
                return false;
            }
            else if (!validator(projectYear, "Project Year"))
            {
                return false;
            }
            else if (!validator(projectTitle, "Project Title"))
            {
                return false;
            }
            else if (!validator(projectPeriod, "Project Period"))
            {
                return false;
            }
            else if (!validator(projectTarget, "Project Target Population"))
            {
                return false;
            }
            else if (!validator(projectArea, "Project Area Population"))
            {
                return false;
            }
            else if (!validator(partnerLocation, "Project Location"))
            {
                return false;
            }
            else if (!validator(projectBudget, "Project Budget"))
            {
                return false;
            }
            else if (!validator(objective, "Objective"))
            {
                return false;
            }

            else if (!validator(goal, "Goal"))
            {
                return false;
            }
            else if (!validator(strategies, "Specific strategies used"))
            {
                return false;
            }
            else if (!validator(targetGroups, "Target groups"))
            {
                return false;
            }
            else if (!validator(description, "Project description"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            //only allow one .
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            originalValue = textBox6.Text;
            if (originalValue.Length == 0)
                (sender as TextBox).Text = "0";
            if (textBox6.Text.Length > 0)
            {
                if (textBox6.Text.IndexOf('.') == 0)
                    textBox6.Text = '0' + textBox6.Text;
                textBox6.Text = string.Format("{0:#,##0.00}", double.Parse(textBox6.Text));
            }
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            textBox6.Text = originalValue;
        }

        public DataGridView GetDataGridView(int outputId)
        {
            if(outputId==11)
                return dataGridView1;
            else if (outputId == 12)
                return dataGridView2;
            else if (outputId == 13)
                return dataGridView3;
            else if (outputId == 21)
                return dataGridView6;
            else if (outputId == 22)
                return dataGridView5;
            else if (outputId == 23)
                return dataGridView4;
            else if (outputId == 24)
                return dataGridView7;
            else if (outputId == 25)
                return dataGridView8;
            else if (outputId == 31)
                return dataGridView11;
            else if (outputId == 32)
                return dataGridView10;
            else
                return dataGridView1;

        }
        public void AddDataToGridView(string [] gridData, int outputId)
        {
            GetDataGridView(outputId).Rows.Add(gridData);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 11);
            browser.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView1);
        }

        private void RemoveGridItem(DataGridView dataGridView)
        {
            foreach (DataGridViewRow item in dataGridView.SelectedRows)
            {
                if ((dataGridView.Rows.Count - 1) != item.Index)
                    dataGridView.Rows.RemoveAt(item.Index);
                RearrangeNumbers(dataGridView);
            }
        }

        private void RearrangeNumbers(DataGridView dataGridView)
        {
            int counter = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {

                counter += 1;
                if (counter < dataGridView.Rows.Count)
                    row.Cells[0].Value = counter + "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView3);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView6);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView5);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView4);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView7);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView8);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView11);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            RemoveGridItem(dataGridView10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 12);
            browser.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 13);
            browser.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 21);
            browser.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 22);
            browser.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 23);
            browser.ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 24);
            browser.ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 25);
            browser.ShowDialog();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 31);
            browser.ShowDialog();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ActivityBrowser browser = new ActivityBrowser(this, 32);
            browser.ShowDialog();
        }

        int insertStatus = -1;
        private void reportInsertBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Report1Helper report1Helper = new Report1Helper();
            insertStatus=report1Helper.Insert(report, outputsList);
        }

        private void reportInsertBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            panel28.Visible = false;
            scrollabalePanel.Enabled = true;
            if(insertStatus>0)
                MessageBox.Show("Report data added successfully!", "Successfully Addition", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Unsuccessful report addition!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).Text = Convert.ToInt32((sender as TextBox).Text) + "";
                
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).Text = Convert.ToInt32((sender as TextBox).Text) + "";
        }

    }
}
