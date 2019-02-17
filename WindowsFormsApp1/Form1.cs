using DisableSleep;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisableSleepForm
{
    public partial class FormMain : Form
    {
        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(uint esFlags);
        // 选项所用到的常数
        const uint ES_AWAYMODE_REQUIRED = 0x00000040;
        const uint ES_CONTINUOUS = 0x80000000;
        const uint ES_DISPLAY_REQUIRED = 0x00000002;
        const uint ES_SYSTEM_REQUIRED = 0x00000001;
        System.Timers.Timer pTimer;
        Icon awakeIcon;
        Icon sleepIcon;
        public FormMain()
        {
            InitializeComponent();
            pTimer = new System.Timers.Timer(30000);
            pTimer.Elapsed += Timer_Elapsed;
            pTimer.AutoReset = true;
            awakeIcon = Resource1.Awake;
            sleepIcon = Resource1.Asleep;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            pTimer.Enabled = true;
            Control.CheckForIllegalCrossThreadCalls = false;
            this.notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.ShowInTaskbar = false;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);
        }
        private void NotifyIcon1_MouseDoubleClick(object sender, EventArgs e)
        {
            //do nothing
        }
        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (pTimer.Enabled == true)
            {
                notifyIcon1.Icon = awakeIcon;
                startToolStripMenuItem1.Enabled = false;
                stopToolStripMenuItem1.Enabled = true;
            }
            else if (pTimer.Enabled == false)
            {
                notifyIcon1.Icon = sleepIcon;
                startToolStripMenuItem1.Enabled = true;
                stopToolStripMenuItem1.Enabled = false;
            }
            else
            {
                startToolStripMenuItem1.Enabled = true;
                stopToolStripMenuItem1.Enabled = true;
            }
        }
        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pTimer.Enabled = false;
            this.Close();
        }
        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pTimer.Enabled = true;
            notifyIcon1.Icon = awakeIcon;
        }
        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pTimer.Enabled = false;
            notifyIcon1.Icon = sleepIcon;
        }
    }
}
