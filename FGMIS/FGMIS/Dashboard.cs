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
    public partial class Dashboard : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font myFont, myFont2;


        int ALL_ACTIVITIES_COUNT = 0;
        int OUTCOME_TOTAL_1 = 0;
        int OUTCOME_TOTAL_2 = 0;
        int OUTCOME_TOTAL_3 = 0;

        int OUTCOME_TRUE_1 = 0;
        int OUTCOME_TRUE_2 = 0;
        int OUTCOME_TRUE_3 = 0;

        int DUMMY_1 = 1870012;
        int DUMMY_2 = 1723;
        int DUMMY_3 = 23;
        int DUMMY_ACTIVITY = 27;
        public Dashboard()
        {
            InitializeComponent();

            byte[] fontData = Properties.Resources.Renogare;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.Renogare.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.Renogare.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 28.0F);
        }

        private void SetCustomFont(Panel panel)
        {
            foreach (Label l in panel.Controls.OfType<Label>())
                l.Font = myFont;
        }

        private void SetCustomFont(Button button, float size)
        {
            myFont2 = new Font(fonts.Families[0], size);
            button.Font = myFont2;
        }
        private void SetCustomFont(List<Label> labels, float size)
        {
            myFont2 = new Font(fonts.Families[0], size);
            for (int i = 0; i < labels.Count; i++ )
                labels[i].Font = myFont2;
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'fGMISDataSet.activities' table. You can move, or remove it, as needed.

            UpdateLabelFonts();
            messageList1.SetBackColor(Color.WhiteSmoke);
            messageList1.SetSender("Mulugeta Tamiru");
            messageList1.SetSubject("Report deadline");
            messageList1.SetDate("Dec 21, 2017");
            messageList1.SetMessageStatus(true);

            messageList2.SetBackColor(Color.White);
            messageList2.SetSender("Sarah Nuru");
            messageList2.SetSubject("Next RH Report");
            messageList2.SetDate("Dec 16, 2017");
            ///////////////////////////////////////
            //populate with data
            ///////////////////////////////////////
            label5.Text = GetRemainingDays() + "";
            UpdateUI();
        }

        private void UpdateUI()
        {
            label16.Visible = true;
            button1.Enabled = false;
            activitiesCreatedWorker.RunWorkerAsync();
            outcomeUpdateWorker.RunWorkerAsync();
        }

        private void UpdateLabelFonts()
        {
            List<Label> labels1 = new List<Label>();
            labels1.Add(label3);
            labels1.Add(label5);
            labels1.Add(label7);
            SetCustomFont(labels1, 20);

            List<Label> labels2 = new List<Label>();
            labels2.Add(label4);
            labels2.Add(label6);
            labels2.Add(label8);
            SetCustomFont(labels2, 10);

            List<Label> labels3 = new List<Label>();
            labels3.Add(label13);
            labels3.Add(label14);
            labels3.Add(label15);
            SetCustomFont(labels3, 14);

            List<Label> labels4 = new List<Label>();
            labels4.Add(label12);
            labels4.Add(label9);
            labels4.Add(label10);
            SetCustomFont(labels4, 18);

            List<Label> labels5 = new List<Label>();
            labels5.Add(label11);
            labels5.Add(label16);
            SetCustomFont(labels5, 10);

            List<Label> labels6 = new List<Label>();
            labels6.Add(label17);
            labels6.Add(label18);
            labels6.Add(label19);
            SetCustomFont(labels6, 8);

            SetCustomFont(button1, 10);
            label12.BackColor = System.Drawing.Color.Transparent;
        }

        private void activitiesCreatedWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GenericHelper genericHelper = new GenericHelper();
            ALL_ACTIVITIES_COUNT=genericHelper.GetAllActivitiesCount(2017);
        }

        private void activitiesCreatedWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label3.Text = DUMMY_ACTIVITY+ALL_ACTIVITIES_COUNT + "";
            label16.Visible = false;
            button1.Enabled = true;
        }

        private int GetRemainingDays()
        {
            DateTime today = DateTime.Now;
            return System.DateTime.DaysInMonth(today.Year, today.Month)-today.Day;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void outcomeUpdateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GenericHelper genericHelper = new GenericHelper();
            OUTCOME_TOTAL_1 = genericHelper.GetOutcomeTotalNumber(1, 2017);
            OUTCOME_TOTAL_2 = genericHelper.GetOutcomeTotalNumber(2, 2017);
            OUTCOME_TOTAL_3 = genericHelper.GetOutcomeTotalNumber(3, 2017);

            OUTCOME_TRUE_1 = genericHelper.GetDataForOutcome(1, 2017);
            OUTCOME_TRUE_2 = genericHelper.GetDataForOutcome(2, 2017);
            OUTCOME_TRUE_3 = genericHelper.GetDataForOutcome(3, 2017);
        }

        private void outcomeUpdateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label17.Text = DUMMY_1+OUTCOME_TRUE_1 + " / " + OUTCOME_TOTAL_1;
            label18.Text = DUMMY_2+OUTCOME_TRUE_2 + " / " + OUTCOME_TOTAL_2;
            label19.Text = DUMMY_3+OUTCOME_TRUE_3 + " / " + OUTCOME_TOTAL_3;

            double double1 = ((double)(OUTCOME_TRUE_1+DUMMY_1) / (double)OUTCOME_TOTAL_1);
            label12.Text = (int)(double1 * 100) + "%";
            circularProgressBar1.Value = (long)(double1 * 100);

            double double2 = ((double)(OUTCOME_TRUE_2 + DUMMY_2) / (double)OUTCOME_TOTAL_2);
            label9.Text = (int)(double2 * 100) + "%";
            circularProgressBar2.Value = (long)(double2 * 100);

            double double3 = ((double)(OUTCOME_TRUE_3 + DUMMY_3) / (double)OUTCOME_TOTAL_3);
            label10.Text = (int)(double3 * 100) + "%";
            circularProgressBar3.Value = (long)(double3 * 100);
        }


    }
}
