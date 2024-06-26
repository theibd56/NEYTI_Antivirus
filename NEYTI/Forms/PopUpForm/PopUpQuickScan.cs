﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Management;

namespace NEYTI.Forms.PopUpForm
{
    public partial class PopUpQuickScan : Form
    {
        [DllImport("Gdi32.DLL", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        public PopUpQuickScan() 
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        public List<string> Hashes = new List<string>(); //a list of SHA1 hashes for scanning
        public List<string> SuspHashes = new List<string>(); //a list of suspicious hashes
        public string GetSHA1(string filePath) //gets SHA1 hash from a file.
        {
            using (var sha1 = SHA1.Create())
            {
                using (var stream = File.OpenRead(filePath)) //do I need to try/catch here? What if the file is inaccessible?
                {
                    return BitConverter.ToString(sha1.ComputeHash(stream)).Replace("-", string.Empty); //Converts bits to string, removes all the dashes and returns it
                }
            }
        }

        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }




        private void btnScan_Click(object sender, EventArgs e)
        {
            lblScannedQuickSum.Text = null;
            //textFastLog.Clear(); //clears the log

            //string[] files1 = Directory.GetFiles(@"C:\Logs", "*", SearchOption.AllDirectories);
            //textFastLog.Text = "" + files1;

            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
            //ManagementObjectCollection collection = searcher.Get();
            //string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];

            string username = SystemInformation.UserName.ToString();

            //DirectoryInfo dr = new DirectoryInfo(@"C:\Users\"+username);
            DirectoryInfo dr = new DirectoryInfo(@"C:\Users\" + username + @"\Downloads");
            foreach (var d in dr.GetFiles())
            {
                Thread.Sleep(20);
                listFastLog.Items.Add(d.Name);
                lblScannedQuickSum.Text = listFastLog.Items.Count.ToString();
            }
            DirectoryInfo dr1 = new DirectoryInfo(@"C:\Windows\Temp");
            foreach (var d in dr1.GetFiles())
            {
                Thread.Sleep(20);
                listFastLog.Items.Add(d.Name);
                lblScannedQuickSum.Text = listFastLog.Items.Count.ToString();
            }
            DirectoryInfo dr2 = new DirectoryInfo(@"C:\Users\"+ username + @"\AppData\Roaming");
            foreach (var d in dr2.GetDirectories())
            {
                Thread.Sleep(20);
                listFastLog.Items.Add(d.Name);
                lblScannedQuickSum.Text = listFastLog.Items.Count.ToString();
            }
            DirectoryInfo dr3 = new DirectoryInfo(@"C:\Users\" + username + @"\AppData\Local\Temp");
            foreach (var d in dr3.GetFiles())
            {
                Thread.Sleep(20);
                listFastLog.Items.Add(d.Name);
                lblScannedQuickSum.Text = listFastLog.Items.Count.ToString();
                btnScan.Enabled = false;
            }

        }
    }
}
