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
    public partial class Form2 : Form
    {
        private string title;
        private int activityId;

        private DataTable regionTable = null, zoneTable = null, woredaTable = null, kebeleTable = null;

        Activity0Helper syncHelper;
        List<Activity0> activityList;
        List<Participant> participantsList;
        int syncStatus = -1, participantsSyncStatus = -1;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font myFont, myFont2;

        public Form2(string title, int activityId)
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
            label1.Font = myFont;
            label1.Text = title;
            comboBox5.SelectedIndex=0;
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
            Activity0 activity0 = helper.GetActivityData(2, activityId);
            Activity2 activity = (Activity2)activity0;


            if (activity != null)
            {
                comboBox1.Text = activity.Region;
                comboBox2.Text = activity.Zone;
                textBox1.Text = activity.Schoolname;
                textBox2.Text = activity.Clubname;
                comboBox3.Text = activity.Woreda;
                dateTimePicker1.Value = activity.ActivityDate;
                textBox3.Text = activity.IssuesRaised;
                textBox4.Text = activity.AgreedActionPoints;
                textBox7.Text = activity.FacilitatorName;
                textBox6.Text = activity.Position;
            }
            List<Participant> participantsList = helper.GetParticpantData(2, activityId);
            for (int i = 0; i < participantsList.Count; i++)
            {
                Participant participant0 = participantsList[i];
                Participant2 participant = (Participant2)participant0;
                string disabled = "No";
                if (participant.Disabled == 0)
                    disabled = "No";
                else
                    disabled = "Yes";
                string[] row = { dataGridView2.RowCount + "", participant.Name, participant.Gradelevel, participant.Sex, participant.Age.ToString(), disabled };
                dataGridView2.Rows.Add(row);
            }
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
                kebeleTable = genericHelper.GetList("Kebele");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BindComboBox(comboBox1, regionTable);
            BindComboBox(comboBox2, zoneTable);
            BindComboBox(comboBox3, woredaTable);

            

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
                string[] row = { dataGridView2.RowCount + "", textBox5.Text, textBox8.Text, comboBox7.Text, numericUpDown1.Value.ToString(), comboBox5.Text };
                dataGridView2.Rows.Add(row);
                textBox5.Text = string.Empty;
            }
        }

        private Boolean fieldsValid()
        {
            ErrorProvider errorProvider = new ErrorProvider();
            string name = (textBox5.Text).Trim();
            string grade = (textBox8.Text).Trim();
            if (name.CompareTo(string.Empty) == 0)
            {
                MessageBox.Show("Please enter a valid participant name!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //errorProvider.SetError(textBox5, "Please enter valid name!");
                return false;
            }
            else if (grade.CompareTo(string.Empty) == 0)
            {
                MessageBox.Show("Please enter a valid participant grade!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //errorProvider.SetError(comboBox5, "Please enter valid kebele!");
                return false;
            }
            else
            {
                //errorProvider.SetError(textBox5, null);
                //errorProvider.SetError(comboBox5, null);
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
            string schoolName = (textBox1.Text).Trim();
            string clubName = (textBox2.Text).Trim();
            string woreda = (comboBox3.Text).Trim();
            string facilitator = (textBox7.Text).Trim();
            string position = (textBox6.Text).Trim();

            if (!validator(region, "Region"))
            {
                return false;
            }
            else if (!validator(zone, "Zone"))
            {
                return false;
            }
            else if (!validator(schoolName, "School name"))
            {
                return false;
            }
            else if (!validator(clubName, "Club name"))
            {
                return false;
            }
            else if (!validator(woreda, "Woreda"))
            {
                return false;
            }
            else if (!validator(facilitator, "Facilitator name"))
            {
                return false;
            }
            else if (!validator(position, "Position"))
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

        private void SaveData(int saveOrUpdate)
        {
            //mainFieldsValid();
            if (mainFieldsValid())
            {
                //MessageBox.Show("All fields valid. Ready to submit data to database.");
                Activity0Helper helper = new Activity0Helper();
                Activity2 activity = new Activity2();
                //(region, zone, woreda, kebele, activity_date, user_id, facilitator_name, position, localtimestamp, mac)
                activity.Region = comboBox1.Text;
                activity.Zone = comboBox2.Text;
                activity.Woreda = comboBox3.Text;
                activity.Schoolname = textBox1.Text;
                activity.Clubname = textBox2.Text;
                activity.ActivityDate = dateTimePicker1.Value;
                activity.UserId = Properties.Settings.Default.UID;//change this using property stored id of currently logged in user
                activity.FacilitatorName = textBox7.Text;//make name of logged in user available in this field
                activity.Clubleadername = textBox7.Text;
                activity.Position = textBox6.Text;
                activity.LocalTimeStamp = DateTime.Now;
                activity.Mac = StringCipher.GetMacAddress();

                activity.IssuesRaised = textBox3.Text;
                activity.AgreedActionPoints = textBox4.Text;


                List<Participant> participantsList = new List<Participant>();
                if (dataGridView2.RowCount > 1)
                {
                    string disabled = "no";
                    for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)//-1 to avoid empty row
                    {
                        Participant2 participant = new Participant2();
                        if (dataGridView2.Rows[i].Cells[1].Value != null)
                            participant.Name = dataGridView2.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView2.Rows[i].Cells[2].Value != null)
                            participant.Gradelevel = dataGridView2.Rows[i].Cells[2].Value.ToString();
                        if (dataGridView2.Rows[i].Cells[3].Value != null)
                            participant.Sex = dataGridView2.Rows[i].Cells[3].Value.ToString();
                        if (dataGridView2.Rows[i].Cells[4].Value != null)
                            participant.Age = Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value.ToString());
                        if (dataGridView2.Rows[i].Cells[5].Value != null)
                        {
                            disabled = dataGridView2.Rows[i].Cells[5].Value.ToString().ToLower();
                            if(disabled.CompareTo("no")==0)
                                participant.Disabled = 0;
                            else
                                participant.Disabled = 1;
                        }
                        //participant.Age = 2;
                        participantsList.Add(participant);

                        //MessageBox.Show(dataGridView2.Rows[i].Cells[1].Value.ToString());
                    }

                }
                //MessageBox.Show(activity.Position);
                int id = helper.Insert(2, activity, participantsList);

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

            genericHelper.Insert("Region", region, Properties.Settings.Default.UID);
            genericHelper.Insert("Zonne", zone, Properties.Settings.Default.UID);
            genericHelper.Insert("Woreda", woreda, Properties.Settings.Default.UID);
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
            int result = helper.DeleteActivity(2,activityId);
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
            activityList = syncHelper.GetUnsyncedActivityList(2);
            participantsList = syncHelper.GetUnsyncedParticpansList(2);
            if (activityList.Count > 0)
            {
                //sync data
                syncStatus = syncHelper.SendActivityListToServer(2,activityList);

            }
            if (participantsList.Count > 0)
            {
                //sync data
                participantsSyncStatus = syncHelper.SendParticipantsListToServer(2,participantsList);

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
