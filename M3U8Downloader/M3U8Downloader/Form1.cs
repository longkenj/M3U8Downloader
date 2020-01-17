using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace M3U8Downloader
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr window, int message, int wparam, int lparam);

        private const int SbBottom = 0x7;
        private const int WmVscroll = 0x115;

        public string URLlink { get; private set; }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void ExecuteCommand(string command)
        {

            //Create process

            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "CMD.exe";
            pProcess.StartInfo.Arguments = command;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;

            //pProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
            // Start the asynchronous read
            pProcess.Start();
            pProcess.BeginOutputReadLine();
            pProcess.WaitForExit();
            pProcess.Close();
        }
       
        

        private void txtURL_MouseClick(object sender, MouseEventArgs e)
        {
            txtURL.SelectAll();
        }

        private void txtM3U8_MouseClick(object sender, MouseEventArgs e)
        {
            txtM3U8.SelectAll();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Text = "Downloading...";
            bgDownload.RunWorkerAsync();
        }

        public void Download()
        {
            string Referer = txtURL.Text;
            string m3u8 = txtM3U8.Text;
            string Title = "Video_"+ DateTime.Now.ToString("yyyyddMM_hhmmss");

            string cmd = "/C ffmpeg -headers \"Referer: "+Referer+"\" -i "+m3u8+" -c copy -bsf:a aac_adtstoasc "+Title+".MP4";
            ExecuteCommand(cmd);
        }

        private void bgDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            Download();
        }

        private void bgDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done");
            btnDownload.Text = "DOWNLOAD";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtURL_TextChanged(object sender, EventArgs e)
        {
            txtURL.ForeColor = Color.Black;
            txtURL.Font = new Font(txtURL.Font, FontStyle.Regular);
        }

        private void txtURL_Leave(object sender, EventArgs e)
        {
            txtURL.ForeColor = SystemColors.GrayText;
            txtURL.Font = new Font(txtURL.Font, FontStyle.Italic);
        }

        private void txtM3U8_TextChanged(object sender, EventArgs e)
        {
            txtM3U8.ForeColor = Color.Black;
            txtM3U8.Font = new Font(txtM3U8.Font, FontStyle.Regular);
        }

        private void txtM3U8_Leave(object sender, EventArgs e)
        {
            txtM3U8.ForeColor = SystemColors.GrayText;
            txtM3U8.Font = new Font(txtM3U8.Font, FontStyle.Italic);
        }
    }
}
