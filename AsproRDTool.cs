using AsproRDTool.Data;
using AsproRDTool.ServiceContracts;
using AsproRDTool.ServiceContracts.Models;
using AsproRDTool.Services;
using MaterialSkin.Controls;
using System.ComponentModel;
using System.Diagnostics;

namespace AsproRDTool
{
    public partial class mainWindow : MaterialForm
    {
        private System.Windows.Forms.Timer timer;
        private Stopwatch stopwatch;
        private DateTime timerStartTime = DateTime.Now;
        private JsonBreakerDetail detail = new JsonBreakerDetail();
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        IJsonBreaker jsonBreaker = new JsonBreakerBase();

        public mainWindow()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }
        private async void cmbx_Server_SelectedIndexChanged(object sender, EventArgs e)
        {
            detail.server = cmbx_Server.SelectedItem.ToString();
            cmbx_Server.SelectedItem = detail.server;
            DBService dBService = new DBService();
            List<string> itemList = await dBService.GetDBDetails(detail.server);
            cmbx_Db.DataSource = itemList;
        }
        private async void mtBtn_Execute_Click(object sender, EventArgs e)
        {
            if (VerifyInput())
            {
                if (mtBtn_Execute.Text == "Execute")
                {
                    cmbx_Server.Enabled = false;
                    cmbx_Db.Enabled = false;
                    mtBtn_Execute.Text = "Cancel";
                    mtBtn_Execute.Enabled = true;
                    SetUpTimer();
                    detail.dbName = cmbx_Db.SelectedItem.ToString();
                    backgroundWorker.RunWorkerAsync(detail);
                }
                else if (mtBtn_Execute.Text == "Cancel")
                {
                    jsonBreaker.CancelTask();
                    mtlbl_Status.ForeColor = Color.Red;
                    mtlbl_Status.Text = "Cancelled by User";
                    StopTimer();
                    cmbx_Server.Enabled = true;
                    cmbx_Db.Enabled = true;
                    mtBtn_Execute.Text = "Execute";
                }
            }
            else
            {
                MessageBox.Show("Plese fill the Server & DB!!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void SetUpTimer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            timerStartTime = DateTime.Now;
            timer = new System.Windows.Forms.Timer() { Interval = 1000 };
            timer.Tick += UpdateElapsedTime;
            timer.Start();
        }

        private void StopTimer()
        {
            timer.Tick -= UpdateElapsedTime;
        }

        private void UpdateElapsedTime(object sender, EventArgs evarg)
        {
            var diff = DateTime.Now.Subtract(timerStartTime);
            mtlbl_timer.Text = $"{diff.ToString(@"hh\:mm\:ss")}";
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            jsonBreaker.CancellationTokenSource = new CancellationTokenSource();
            jsonBreaker.InitiateJsonModification((JsonBreakerDetail)e.Argument, worker, e);
            if (jsonBreaker.CancellationTokenSource.Token.IsCancellationRequested)
            {
                e.Cancel = true;
                worker.CancelAsync();
            }
            mtlbl_Status.Text = jsonBreaker.ProgressStatus.Status;
            stopwatch?.Stop();
            timer.Stop();
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                mtlbl_Status.Text = ((JsonBreakerStatus)e.UserState).Status;
            }
            mtPrgsBr.Value = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                StopTimer();
                MessageBox.Show(e.Error.Message);
                mtlbl_Status.Text = "Failed";
            }
            else if (e.Cancelled)
            {
                StopTimer();
                mtlbl_Status.Text = "Cancelled!";

                DialogResult result = MessageBox.Show("Execution cancelled by user", "Abort", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    mtBtn_Execute.Text = "Execute";
                    mtlbl_timer.Text = "00:00:00";
                }
            }
            else
            {
                StopTimer();
                mtlbl_Status.Text = "Done!";
                DialogResult result = MessageBox.Show("Json Tables Created in DB", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    mtBtn_Execute.Text = "Execute";
                    mtlbl_timer.Text = "00:00:00";
                }
            }
        }
        private void InitializeBackgroundWorker()
        {
            backgroundWorker.DoWork += bgWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += bgWorker_ProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
        }
        private bool VerifyInput()
        {
            bool result = false;
            if (cmbx_Server.SelectedItem != null || cmbx_Db.SelectedItem != null)
            {
                result = true;
            }
            return result;
        }
    }
}