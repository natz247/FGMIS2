using Domain;
using Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FGMIS
{
    public partial class Form1 : Form
    {
        private string title;
        private int activityId;


        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font myFont, myFont2;

        private string region, zone, place, kebele, woreda, duration;

        private DataTable regionTable = null, zoneTable = null, woredaTable = null, kebeleTable = null, kebeleTable2 = null;
        Activity1Helper syncHelper;
        List<Activity1> activityList;
        List<Participant1> participantsList;
        int syncStatus = -1, participantsSyncStatus=-1;

        private Loading loadingDialog = null;
        public Form1(string title, int activityId)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'fGMISDataSet.activities' table. You can move, or remove it, as needed.
            //this.activitiesTableAdapter.Fill(this.fGMISDataSet.activities);
            label1.Text = title;
            label1.Font = myFont;
            comboBox7.SelectedIndex = 0;
            textBox6.Text=Properties.Settings.Default.USERFIRSTNAME+" "+Properties.Settings.Default.USERLASTNAME;
            textBox7.Text = Properties.Settings.Default.USERPOSITION;
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
                panel10.Visible = true;
                DisableFields(false);
                label16.Text = "Please wait, data loading...";
                backgroundWorker1.RunWorkerAsync();
            }
            
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
            if (activityHelper.TableExists("Kebele"))
                kebeleTable2 = genericHelper.GetList("Kebele");


        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BindComboBox(comboBox1, regionTable);
            BindComboBox(comboBox2, zoneTable);
            BindComboBox(comboBox3, woredaTable);
            BindComboBox(comboBox4, kebeleTable);
            BindComboBox(comboBox5, kebeleTable2);

            panel10.Visible = false;

            DisableFields(true);
        }

        private void PopulateFields(int activityId)
        {
            Activity1Helper helper = new Activity1Helper();
            Activity1 activity = helper.GetActivityData(activityId);
            if(activity!=null)
            {
                comboBox1.Text = activity.Region;
                comboBox2.Text = activity.Zone;
                textBox1.Text = activity.Place;
                comboBox3.Text = activity.Woreda;
                comboBox4.Text = activity.Kebele;
                dateTimePicker1.Value = activity.ActivityDate;
                textBox2.Text = activity.Duration;
                textBox3.Text = activity.IssuesRaised;
                textBox4.Text = activity.AgreedActionPoints;
                textBox6.Text = activity.FacilitatorName;
                textBox7.Text = activity.Position;
            }
            List<Participant1> participantsList = helper.GetParticpantData(activityId);
            for (int i = 0; i < participantsList.Count; i++)
            {
                Participant1 participant = (Participant1)participantsList[i];
                string[] row = { dataGridView2.RowCount + "", participant.Name, participant.Kebele, participant.Sex, participant.Age.ToString() };
                dataGridView2.Rows.Add(row);
            }
        }

        private void DeleteActivity()
        {
            Activity1Helper helper = new Activity1Helper();
            int result= helper.DeleteActivity(activityId);
            if(result>0)
            {
                MessageBox.Show("Activity data deleted successfully!", "Successfully Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Activity data deletetion failed!", "Uh oh!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void DisableFields(bool value)
        {
            panel6.Enabled =value;
            groupBox2.Enabled = value;
            //panel11.Enabled = value;
            button4.Enabled = value;
            button1.Enabled = value;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void activitiesBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            activitiesTableAdapter.Update(this.fGMISDataSet);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //validate data before insertion
            if (fieldsValid())
            {
                string[] row = { dataGridView2.RowCount + "", textBox5.Text, comboBox5.Text, comboBox7.Text, numericUpDown1.Value.ToString() };
                dataGridView2.Rows.Add(row);
                textBox5.Text = string.Empty;
            }
        }

        private Boolean fieldsValid()
        {
            ErrorProvider errorProvider = new ErrorProvider();
            string name = (textBox5.Text).Trim();
            string kebele = (comboBox5.Text).Trim();
            if (name.CompareTo(string.Empty) == 0)
            {
                MessageBox.Show("Please enter a valid participant name!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //errorProvider.SetError(textBox5, "Please enter valid name!");
                return false;
            }
            else if(kebele.CompareTo(string.Empty)==0)
            {
                MessageBox.Show("Please enter a valid participant kebele!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private Boolean mainFieldsValid()
        {

            string region = (comboBox1.Text).Trim();
            string zone = (comboBox2.Text).Trim();
            string place = (textBox1.Text).Trim();
            string woreda = (comboBox3.Text).Trim();
            string kebele = (comboBox4.Text).Trim();
            string duration = (textBox2.Text).Trim();
            string facilitator = (textBox6.Text).Trim();
            string position = (textBox7.Text).Trim();

            if (!validator(region,"Region"))
            {
                return false;
            }
            else if (!validator(zone, "Zone"))
            {
                return false;
            }
            else if (!validator(place, "Place of gathering"))
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
            else if (!validator(duration, "Duration of program"))
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
            else if(dataGridView2.RowCount<=1)
            {
                MessageBox.Show("Please add at least 1 participant!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.RemoveAt(item.Index);
            }
        }

        private void dataGridView2_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            // For any other operation except, StateChanged, do nothing
            /*if (e.StateChanged == DataGridViewElementStates.Selected)
            {
                MessageBox.Show("Row selected");
            }
            else
                return;*/

        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView2.SelectedRows)
            {
                if((dataGridView2.Rows.Count-1)!= item.Index)
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
                if(counter< dataGridView2.Rows.Count)
                row.Cells[0].Value = counter+"";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to delete this activity?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if(confirmResult==DialogResult.Yes)
            {
                //delete record
                DeleteActivity();
            }
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

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {

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

        private void button4_Click(object sender, EventArgs e)
        {
            SaveData(0);//0 so, save only
        }

        private void SaveData(int saveOrUpdate)
        {
            if (mainFieldsValid())
            {
                //MessageBox.Show("All fields valid. Ready to submit data to database.");
                Activity1Helper helper = new Activity1Helper();
                Activity1 activity = new Activity1();
                //(region, zone, woreda, kebele, activity_date, user_id, facilitator_name, position, localtimestamp, mac)
                activity.Region = comboBox1.Text;
                activity.Zone = comboBox2.Text;
                activity.Woreda = comboBox3.Text;
                activity.Kebele = comboBox4.Text;
                activity.ActivityDate = dateTimePicker1.Value;
                activity.UserId = Properties.Settings.Default.UID;//change this using property stored id of currently logged in user
                activity.FacilitatorName = textBox6.Text;//make name of logged in user available in this field
                activity.Position = textBox7.Text;
                activity.LocalTimeStamp = DateTime.Now;
                activity.Mac = GetMacAddress();
                activity.Place = textBox1.Text;
                activity.Duration = textBox2.Text;

                activity.IssuesRaised = textBox3.Text;
                activity.AgreedActionPoints = textBox4.Text;

                List<Participant1> participantsList = new List<Participant1>();
                if (dataGridView2.RowCount > 1)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)//-1 to avoid empty row
                    {
                        Participant1 participant = new Participant1();
                        if (dataGridView2.Rows[i].Cells[1].Value != null)
                            participant.Name = dataGridView2.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView2.Rows[i].Cells[2].Value != null)
                            participant.Kebele = dataGridView2.Rows[i].Cells[2].Value.ToString();
                        if (dataGridView2.Rows[i].Cells[3].Value != null)
                            participant.Sex = dataGridView2.Rows[i].Cells[3].Value.ToString();
                        if (dataGridView2.Rows[i].Cells[4].Value != null)
                            participant.Age = Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value.ToString());
                        //participant.Age = 2;
                        participantsList.Add(participant);

                        //MessageBox.Show(dataGridView2.Rows[i].Cells[1].Value.ToString());
                    }

                }

                int id = helper.Insert(activity, participantsList);
                if (id > 0)
                {
                    InsertRegionAndKebeleData();
                    //MessageBox.Show("Activity data added successfully!");
                    MessageBox.Show("Activity data added successfully!", "Successfully Addition", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //check internet connection before syncing data
                    if (saveOrUpdate == 1)//update button clicked
                    {
                        if (CheckInternet("http://" + Session.Properties.Settings.Default.RemoteDatabaseAddress))
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
            string kebele = comboBox4.Text;

            genericHelper.Insert("Region", region, Properties.Settings.Default.UID);
            genericHelper.Insert("Zonne", zone, Properties.Settings.Default.UID);
            genericHelper.Insert("Woreda", woreda, Properties.Settings.Default.UID);
            genericHelper.Insert("Kebele", kebele, Properties.Settings.Default.UID);
        }
        public bool CheckInternet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;//15 seconds
            request.Method = "HEAD";
            try
            {
                using(HttpWebResponse response=(HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch(WebException)
            {
                return false;
            }
        }

        public void SyncData()
        {
            panel10.Visible = true;
            DisableFields(false);
            label16.Text = "Please wait, data sync in progress...";
            syncBackgroundWorker.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (activityId == -1)//data addition
            SaveData(1);//0 so, save and update
            else
            {
                if (CheckInternet("http://" + Session.Properties.Settings.Default.RemoteDatabaseAddress))
                    SyncData();
                else
                {
                    MessageBox.Show("No internet connection. Activity data sync failed!", "Sync Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void syncBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            syncHelper = new Activity1Helper();
            activityList = syncHelper.GetUnsyncedActivityList();
            participantsList = syncHelper.GetUnsyncedParticpansList();
            if (activityList.Count > 0)
            {
                //sync data
                syncStatus = syncHelper.SendActivityListToServer(activityList);
                
            }
            if (participantsList.Count > 0)
            {
                //sync data
                participantsSyncStatus = syncHelper.SendParticipantsListToServer(participantsList);

            }
        }

        private void syncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (syncStatus > 0 )
                MessageBox.Show("Activity synced successfully!", "Successfully Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Activity data sync failed!", "Sync Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            panel10.Visible = false;
            DisableFields(true);
            this.Close();
        }


        

       
    }
}
