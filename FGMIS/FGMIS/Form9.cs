using Domain;
using Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FGMIS
{
    public partial class Form9 : Form
    {
        private int ACTIVITY_TYPE=9;
        private string title;
        private int activityId;

        private string originalValue = "";
        private string originalValue2 = "";

        private DataTable regionTable = null, zoneTable = null, woredaTable = null, kebeleTable = null, kebeleTable2 = null, materialTable=null, unitTable=null;

        Activity0Helper syncHelper;
        List<Activity0> activityList;
        List<Participant> participantsList;
        int syncStatus = -1, participantsSyncStatus = -1;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font myFont, myFont2;

        public Form9(string title, int activityId)
        {
            InitializeComponent();
            this.title = title;
            this.activityId = activityId;


            byte[] fontData = Properties.Resources.Renogare;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.Renogare.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.Renogare.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 14.0F);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";
            dateTimePicker1.ShowUpDown = true;


            label1.Font = myFont;
            label1.Text = title;
            comboBox7.SelectedIndex = 0;

            textBox7.Text = Properties.Settings.Default.USERFIRSTNAME + " " + Properties.Settings.Default.USERLASTNAME;
            textBox6.Text = Properties.Settings.Default.USERPOSITION;

            if (activityId != -1)
            {
                //disable Fields and load data of activity with id=activityId
                //DisableFields();
                button4.Enabled = false;//can't manipulate data
                button5.Enabled = false;
                button9.Enabled = false;
                button2.Visible = true;
                PopulateFields(activityId);
            }
            else
            {
                //do this in background
                DisableFields(false);
                label16.Text = "Please wait, data loading...";
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void PopulateFields(int activityId)
        {
            Activity0Helper helper = new Activity0Helper();
            Activity0 activity0 = helper.GetActivityData(ACTIVITY_TYPE, activityId);
            Activity9 activity = (Activity9)activity0;


            if (activity != null)
            {
                comboBox1.Text = activity.Region;
                comboBox2.Text = activity.Zone;
                comboBox3.Text = activity.Woreda;
                comboBox5.Text = activity.Kebele;
                dateTimePicker1.Value = activity.ActivityDate;
                textBox3.Text = activity.IssuesRaised;
                textBox4.Text = activity.AgreedActionPoints;
                textBox7.Text = activity.FacilitatorName;
                textBox6.Text = activity.Position;
            }
            List<Participant> participantsList = helper.GetParticpantData(ACTIVITY_TYPE, activityId);
            for (int i = 0; i < participantsList.Count; i++)
            {
                Participant participant0 = participantsList[i];
                Participant9 participant = (Participant9)participant0;

                string[] row = { dataGridView2.RowCount + "", participant.Name, participant.Kebele, GetStringFromInt(participant.Disabled), participant.Noofadolescent.ToString(), participant.Noofwomen.ToString(), participant.Typeofmaterial, participant.Unit, participant.Quantity.ToString()};
                dataGridView2.Rows.Add(row);
                
            }
        }

        private string GetStringFromInt(int value)
        {
            if (value == 0)
                return "No";
            else
                return "Yes";
        }

        private bool GetBoolFromInt(int value)
        {
            if (value == 0)
                return false;
            else
                return true;
        }

        private void DisableFields(bool value)
        {
            scrollablePanel.Enabled = value;
            panel10.Visible = !value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            GenericHelper genericHelper = new GenericHelper();
            ActivitySelectorHelper activityHelper = new ActivitySelectorHelper();
            if (activityHelper.TableExists("Region"))
                regionTable = genericHelper.GetList("Region");
            if (activityHelper.TableExists("Zonne"))
                zoneTable = genericHelper.GetList("Zonne");
            if (activityHelper.TableExists("Woreda"))
                woredaTable = genericHelper.GetList("Woreda");
            if (activityHelper.TableExists("Kebele"))
            {
                kebeleTable = genericHelper.GetList("Kebele");
                kebeleTable2 = genericHelper.GetList("Kebele");
            }
            if (activityHelper.TableExists("Material"))
                materialTable = genericHelper.GetList("Material");
            if (activityHelper.TableExists("Unit"))
                unitTable = genericHelper.GetList("Unit");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BindComboBox(comboBox1, regionTable);
            BindComboBox(comboBox2, zoneTable);
            BindComboBox(comboBox3, woredaTable);
            BindComboBox(comboBox5, kebeleTable);
            BindComboBox(comboBox4, kebeleTable2);

            BindComboBox(comboBox6, materialTable);
            BindComboBox(comboBox8, unitTable);
            
            DisableFields(true);
        }

        private void BindComboBox(ComboBox comboBox, DataTable dataTable)
        {
            GenericHelper genericHelper = new GenericHelper();
            //MessageBox.Show(""+genericHelper.GetList("Region").Count);
            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox.DataSource = dataTable;
            comboBox.BindingContext = this.BindingContext;
            comboBox.DisplayMember = "title";
            comboBox.ValueMember = "title";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (fieldsValid())
            {
                string[] row = { dataGridView2.RowCount + "", textBox5.Text, comboBox4.Text, comboBox7.Text, numericUpDown3.Value.ToString(), numericUpDown1.Value.ToString(), comboBox6.Text, numericUpDown2.Value.ToString() };
                dataGridView2.Rows.Add(row);
                textBox5.Text = string.Empty;
            }
        }

        
        private bool IsChecked(CheckBox checkBox)
        {
            if (checkBox.Checked)
                return true;
            else 
                return false;
        }

        

        private Boolean fieldsValid()
        {
            string name = (textBox5.Text).Trim();
            string kebele = (comboBox4.Text).Trim();
            string typeofmaterial = (comboBox6.Text).Trim();
            string unit = (comboBox8.Text).Trim();

            if (name.CompareTo(string.Empty) == 0)
            {
                MessageBox.Show("Please enter a valid participant name!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (kebele.CompareTo(string.Empty) == 0)
            {
                MessageBox.Show("Please enter a valid kebele!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (typeofmaterial.CompareTo(string.Empty) == 0)
            {
                MessageBox.Show("Please enter a valid type of material!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (unit.CompareTo(string.Empty) == 0)
            {
                MessageBox.Show("Please enter a valid unit!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView2.SelectedRows)
            {
                if ((dataGridView2.Rows.Count - 1) != item.Index)
                    dataGridView2.Rows.RemoveAt(item.Index);
                RearrangeNumbers();
            }
        }

        private void RearrangeNumbers()
        {
            int counter = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {

                counter += 1;
                if (counter < dataGridView2.Rows.Count)
                    row.Cells[0].Value = counter + "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveData(0);//0 so, save only
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

        private Boolean mainFieldsValid()
        {

            string region = (comboBox1.Text).Trim();
            string zone = (comboBox2.Text).Trim();
            string woreda = (comboBox3.Text).Trim();
            string kebele = (comboBox5.Text).Trim();

            if (!validator(region, "Region"))
            {
                return false;
            }
            else if (!validator(zone, "Zone"))
            {
                return false;
            }
            else if (!validator(woreda, "Woreda"))
            {
                return false;
            }
            else if (!validator(kebele, "Kebele"))
            {
                return false;
            }
            else if (dataGridView2.RowCount <= 1)
            {
                MessageBox.Show("Please add at least 1 participant!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private int GetDisabledInt(string disabledText)
        {
            if (disabledText.ToLower().CompareTo("no") == 0)
                return 0;
            else
                return 1;
        }

        private int GetIntFromBool(bool value)
        {
            if (value)
                return 1;
            else
                return 0;
        }

        private void SaveData(int saveOrUpdate)
        {
            //mainFieldsValid();
            if (mainFieldsValid())
            {
                
                Activity0Helper helper = new Activity0Helper();
                Activity9 activity = new Activity9();
                
                activity.Region = comboBox1.Text;
                activity.Zone = comboBox2.Text;
                activity.Woreda = comboBox3.Text;
                activity.Kebele = comboBox5.Text;
                activity.ActivityDate = dateTimePicker1.Value;
                activity.UserId = Properties.Settings.Default.UID;//change this using property stored id of currently logged in user
                activity.FacilitatorName = textBox7.Text;//make name of logged in user available in this field
                activity.Position = textBox6.Text;
                activity.LocalTimeStamp = DateTime.Now;
                activity.Mac = StringCipher.GetMacAddress();

                activity.IssuesRaised = textBox3.Text;
                activity.AgreedActionPoints = textBox4.Text;


                List<Participant> participantsList = new List<Participant>();
                if (dataGridView2.RowCount > 1)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)//-1 to avoid empty row
                    {
                        Participant9 participant = new Participant9();
                        if (dataGridView2.Rows[i].Cells[1].Value != null)
                            participant.Name = dataGridView2.Rows[i].Cells[1].Value.ToString();
                        
                        if (dataGridView2.Rows[i].Cells[2].Value != null)
                            participant.Kebele = dataGridView2.Rows[i].Cells[2].Value.ToString();
                        
                        if (dataGridView2.Rows[i].Cells[3].Value != null)
                            participant.Disabled = GetDisabledInt(dataGridView2.Rows[i].Cells[3].Value.ToString());

                        if (dataGridView2.Rows[i].Cells[4].Value != null)
                            participant.Noofadolescent = Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value.ToString());
                        
                        if (dataGridView2.Rows[i].Cells[5].Value != null)
                            participant.Noofwomen = Convert.ToInt32(dataGridView2.Rows[i].Cells[5].Value.ToString());
                        
                        if (dataGridView2.Rows[i].Cells[6].Value != null)
                            participant.Typeofmaterial = dataGridView2.Rows[i].Cells[6].Value.ToString();

                        if (dataGridView2.Rows[i].Cells[7].Value != null)
                            participant.Unit = dataGridView2.Rows[i].Cells[7].Value.ToString();

                        if (dataGridView2.Rows[i].Cells[8].Value != null)
                            participant.Quantity = Convert.ToInt32(dataGridView2.Rows[i].Cells[8].Value.ToString());

                        participantsList.Add(participant);

                    }

                }
                //MessageBox.Show(activity.Position);
                int id = helper.Insert(ACTIVITY_TYPE, activity, participantsList);

                if (id > 0)
                {
                    InsertRegionAndKebeleData();
                    //MessageBox.Show("Activity data added successfully!");
                    MessageBox.Show("Activity data added successfully!", "Successfully Addition", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //check internet connection before syncing data
                    if (saveOrUpdate == 1)//update button clicked
                    {
                        //if (StringCipher.CheckInternet("http://" + Session.Properties.Settings.Default.RemoteDatabaseAddress))
                            SyncData();
                    }
                    //this.Close(); //or Reset fields

                    this.Close();
                }


            }
        }

        private void InsertRegionAndKebeleData()
        {
            GenericHelper genericHelper = new GenericHelper();
            string region = comboBox1.Text;
            string zone = comboBox2.Text;
            string woreda = comboBox3.Text;
            string kebele = comboBox5.Text;
            string material = comboBox6.Text;
            string unit = comboBox8.Text;

            genericHelper.Insert("Region", region, Properties.Settings.Default.UID);
            genericHelper.Insert("Zonne", zone, Properties.Settings.Default.UID);
            genericHelper.Insert("Woreda", woreda, Properties.Settings.Default.UID);
            genericHelper.Insert("Kebele", kebele, Properties.Settings.Default.UID);
            genericHelper.Insert("Material", material, Properties.Settings.Default.UID);
            genericHelper.Insert("Unit", unit, Properties.Settings.Default.UID);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to delete this activity?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (confirmResult == DialogResult.Yes)
            {
                //delete record
                DeleteActivity();
            }
        }

        private void DeleteActivity()
        {
            Activity0Helper helper = new Activity0Helper();
            int result = helper.DeleteActivity(ACTIVITY_TYPE, activityId);
            if (result > 0)
            {
                MessageBox.Show("Activity data deleted successfully!", "Successfully Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Activity data deletetion failed!", "Uh oh!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (activityId == -1)//data addition
                SaveData(1);//0 so, save and update
            else
            {
                if (StringCipher.CheckInternet("http://" + Session.Properties.Settings.Default.RemoteDatabaseAddress))
                    SyncData();
                else
                {
                    MessageBox.Show("No internet connection. Activity data sync failed!", "Sync Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        public void SyncData()
        {
            panel10.Visible = true;
            DisableFields(false);
            label16.Text = "Please wait, data sync in progress...";
            syncBackgroundWorker.RunWorkerAsync();
        }

        private void syncBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            syncHelper = new Activity0Helper();
            activityList = syncHelper.GetUnsyncedActivityList(ACTIVITY_TYPE);
            participantsList = syncHelper.GetUnsyncedParticpansList(ACTIVITY_TYPE);
            if (activityList.Count > 0)
            {
                //sync data
                syncStatus = syncHelper.SendActivityListToServer(ACTIVITY_TYPE, activityList);

            }
            if (participantsList.Count > 0)
            {
                //sync data
                participantsSyncStatus = syncHelper.SendParticipantsListToServer(ACTIVITY_TYPE, participantsList);

            }
        }

        private void syncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (syncStatus > 0)
                MessageBox.Show("Activity synced successfully!", "Successfully Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Activity data sync failed!", "Sync Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            panel10.Visible = false;
            DisableFields(true);
            this.Close();
        }



        
    }
}
