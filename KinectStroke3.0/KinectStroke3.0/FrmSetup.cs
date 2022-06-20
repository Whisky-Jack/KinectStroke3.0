using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MeepEngine
{
    public partial class FrmSetup : Form
    {
        public FrmSetup()
        {
            InitializeComponent();
        }

        private void FrmSetup_FormClosing(object sender, FormClosingEventArgs e)
        {
            EntSetup.formIsOpen = false;
        }

        private void FrmSetup_Load(object sender, EventArgs e)
        {
            txtDirectory.Text = EntSetup.dataDirectory;
            txtSubject.Text = EntSetup.subject;
            txtDay.Value = EntSetup.day;
            txtBlock.Value = EntSetup.block;

            cbxHand.SelectedIndex = EntSetup.handType;
            cbxDifficulty.SelectedIndex = EntSetup.difficulty;

            chkScore.Checked = EntSetup.showScore;
            chkFreePlay.Checked = EntSetup.freePlay;

            txtTotalAsteroids.Value = EntGameManager.totalAsteroids;

            txtVelocityThreshold.Value = (decimal)EntAsteroid.throwVel;
            txtRangeNWx.Value = (decimal)EntScaledKinect.nw.X;
            txtRangeNWy.Value = (decimal)EntScaledKinect.nw.Y;
            txtRangeNEx.Value = (decimal)EntScaledKinect.ne.X;
            txtRangeNEy.Value = (decimal)EntScaledKinect.ne.Y;
            txtRangeSWx.Value = (decimal)EntScaledKinect.sw.X;
            txtRangeSWy.Value = (decimal)EntScaledKinect.sw.Y;
            txtRangeSEx.Value = (decimal)EntScaledKinect.se.X;
            txtRangeSEy.Value = (decimal)EntScaledKinect.se.Y;
        }
        private void threader_helper() {
            DialogResult browseResult = pathBrowser.ShowDialog();
            while (browseResult == DialogResult.No) { }

            if (browseResult == DialogResult.OK)
                path_result = pathBrowser.SelectedPath;
        }
        Thread newThread;
        static String path_result = "";

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult browseResult = DialogResult.No;
            pathBrowser.SelectedPath = txtDirectory.Text;

            newThread = new Thread(new ThreadStart(threader_helper));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();

            //Let the new thread die
            while (newThread.IsAlive)
            {
                
            }
            txtDirectory.Text = path_result;
            //newThread = new Thread((ThreadStart)(() => { browseResult = pathBrowser.ShowDialog(); }));//new ThreadStart(browseResult = pathBrowser.ShowDialog()));
            //newThread.SetApartmentState(ApartmentState.STA);
            //newThread.Start();

            //while (browseResult == DialogResult.No) { }

            //if (browseResult == DialogResult.OK)
              //  txtDirectory.Text = pathBrowser.SelectedPath;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            EntSetup.dataDirectory = txtDirectory.Text;
            EntSetup.subject = txtSubject.Text;
            EntSetup.day = (int)txtDay.Value;
            EntSetup.block = (int)txtBlock.Value;

            EntSetup.handType = cbxHand.SelectedIndex;
            EntSetup.difficulty = cbxDifficulty.SelectedIndex;

            EntSetup.showScore = chkScore.Checked;
            EntSetup.freePlay = chkFreePlay.Checked;

            EntGameManager.totalAsteroids = (int)txtTotalAsteroids.Value;

            EntAsteroid.throwVel = (float)txtVelocityThreshold.Value;
            EntScaledKinect.nw.X = (float)txtRangeNWx.Value;
            EntScaledKinect.nw.Y = (float)txtRangeNWy.Value;
            EntScaledKinect.ne.X = (float)txtRangeNEx.Value;
            EntScaledKinect.ne.Y = (float)txtRangeNEy.Value;
            EntScaledKinect.sw.X = (float)txtRangeSWx.Value;
            EntScaledKinect.sw.Y = (float)txtRangeSWy.Value;
            EntScaledKinect.se.X = (float)txtRangeSEx.Value;
            EntScaledKinect.se.Y = (float)txtRangeSEy.Value;

            EntSetup.UpdateKinect();

            Close();
        }

        private void txtDirectory_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbxDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSubject_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
