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
    public partial class Main : Form
    {
        private Dashboard dash=null;
        private Login loginForm=null;
        string REGION = "Region";
        string ZONNE = "Zonne";
        string WOREDA = "Woreda";
        string KEBELE = "Kebele";
        string MATERIAL = "Material";
        string UNIT = "Unit";
        
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font myFont;

        public Main(Login loginForm)
        {
            InitializeComponent();
            this.loginForm = loginForm;

            byte[] fontData = Properties.Resources.Renogare;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.Renogare.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.Renogare.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 14.0F);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            MDIClientSupport.SetBevel(this, false);
            //string encryptedstring = StringCipher.Encrypt("admin", "app");
            //textBox1.Text = encryptedstring;
            SetCustomFont();
            //button1.Font = myFont;
            label1.Text = Properties.Settings.Default.USERFIRSTNAME + " " + Properties.Settings.Default.USERLASTNAME + " (" + Properties.Settings.Default.UID+ ")";
            showDashboard();
            button1.BackColor = Color.LightGray;
        }

        private void SetCustomFont()
        {
            button1.Font = myFont;
            button2.Font = myFont;
            button3.Font = myFont;
            button4.Font = myFont;
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

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(loginForm!=null)
            {
                loginForm.Show();
                loginForm.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetButtonColor(sender as Button);
            ActivitySelector activitySelector = new ActivitySelector(this);
            activitySelector.ShowDialog();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showDashboard();
            SetButtonColor(sender as Button);
        }

        private void SetButtonColor(Button button)
        {
            Button[] buttons = { button1, button2, button3, button4};
            for(int i=0; i<buttons.Length; i++)
            {
                buttons[i].BackColor = Color.Transparent;
            }
            button.BackColor = Color.LightGray;
        }

        private void showDashboard()
        {
            if (dash == null)
            {
                dash = new Dashboard();
                dash.MdiParent = this;
                dash.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                dash.ControlBox = false;
                dash.MaximizeBox = false;
                dash.MinimizeBox = false;
                dash.ShowIcon = false;
                dash.Text = "";
                dash.Dock = DockStyle.Fill;
                dash.Show();
            }
            else
            {
                if (dash.IsDisposed)
                {
                    dash = new Dashboard();
                    dash.MdiParent = this;
                    dash.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    dash.ControlBox = false;
                    dash.MaximizeBox = false;
                    dash.MinimizeBox = false;
                    dash.ShowIcon = false;
                    dash.Text = "";
                    dash.Dock = DockStyle.Fill;
                    dash.Show();
                }
                else
                    dash.Activate();
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageUsers manageUsers = new ManageUsers();
            CheckInternet checkInternet = new CheckInternet(manageUsers);
            checkInternet.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetButtonColor(sender as Button);
            ReportSelector reportSelector = new ReportSelector(this);
            reportSelector.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetButtonColor(sender as Button);
        }

        private void organizationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageOrganizations manageOrganizations = new ManageOrganizations();
            CheckInternet checkInternet = new CheckInternet(manageOrganizations);
            checkInternet.ShowDialog();
        }

        private void partnersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManagePartners managePartners = new ManagePartners();
            CheckInternet checkInternet = new CheckInternet(managePartners);
            checkInternet.ShowDialog();
        }

        private void regionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocations manageLocations = new ManageLocations(REGION);
            manageLocations.ShowDialog();
        }

        private void zonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocations manageLocations = new ManageLocations(ZONNE);
            manageLocations.ShowDialog();
        }

        private void woredasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocations manageLocations = new ManageLocations(WOREDA);
            manageLocations.ShowDialog();
        }

        private void kebelesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocations manageLocations = new ManageLocations(KEBELE);
            manageLocations.ShowDialog();
        }

        private void materialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocations manageLocations = new ManageLocations(MATERIAL);
            manageLocations.ShowDialog();
        }

        private void unitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocations manageLocations = new ManageLocations(UNIT);
            manageLocations.ShowDialog();
        }

        private void previousYearDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManagePreviousYearData managePreviousYearData = new ManagePreviousYearData();
            CheckInternet checkInternet = new CheckInternet(managePreviousYearData);
            checkInternet.ShowDialog();
        }
    }
}
