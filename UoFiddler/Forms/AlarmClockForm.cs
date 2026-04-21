// /***************************************************************************
//  *
//  * $Author: Turley
//  * 
//  * "啤酒许可证"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯啤酒作为回报。
//  *
//  ***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace UoFiddler.Forms
{
    public partial class AlarmClockForm : Form
    {
        private Timer timer; // 尚未赋值
        private Timer realTimeTimer;
        private DateTime alarmTime;
        private SoundPlayer alarmSound = new SoundPlayer(@"C:\Windows\Media\Alarm01.wav"); // 闹钟声音文件路径 默认闹钟声音文件

        #region AlarmClockForm
        public AlarmClockForm()
        {
            InitializeComponent();
            timer1 = new Timer();
            timer1.Interval = 1000; // 设置定时器为1秒
            timer1.Tick += Timer_Tick;

            realTimeTimer = new Timer();
            realTimeTimer.Interval = 1000; // 设置定时器为1秒
            realTimeTimer.Tick += RealTimeTimer_Tick;
            realTimeTimer.Start();

            timer = new Timer();
            timer.Interval = 1000; // 设置定时器为1秒
            timer.Tick += SnoozeTimer_Tick;

            this.FormClosing += AlarmClockForm_FormClosing;
            this.Shown += AlarmClockForm_Shown;            

        }
        #endregion

        #region startButton
        private void startButton_Click(object sender, EventArgs e)
        {
            // 将闹钟时间设置为 dateTimePicker1 中选择的时间
            alarmTime = dateTimePicker1.Value;

            timer1.Start();
        }
        #endregion

        #region stopButton
        private void stopButton_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            alarmSound.Stop();
        }
        #endregion

        #region Timer_Tick
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = alarmTime - DateTime.Now;

            if (remainingTime.TotalSeconds <= 0)
            {
                timer1.Stop();
                timeLabel.Text = "00:00:00";

                // 播放闹钟声音
                alarmSound.PlayLooping();
            }
            else
            {
                timeLabel.Text = remainingTime.ToString(@"hh\:mm\:ss");
            }
        }
        #endregion

        #region RealTimeTimer
        private void RealTimeTimer_Tick(object sender, EventArgs e)
        {
            LabelTimeReal.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        #endregion

        #region AlarmClockForm_FormClosing
        private void AlarmClockForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            alarmSound.Stop();
            Properties.Settings.Default.FormLocationAlarm = this.Location;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region btLoadWave
        private void btLoadWave_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogWave = new OpenFileDialog();
            openFileDialogWave.Filter = "WAV文件|*.wav";

            if (openFileDialogWave.ShowDialog() == DialogResult.OK)
            {
                alarmSound.SoundLocation = openFileDialogWave.FileName;
                alarmSound.LoadAsync();
            }
        }
        #endregion

        #region AlarmClockForm_Shown
        private void AlarmClockForm_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FormLocationAlarm != Point.Empty)
            {
                this.Location = Properties.Settings.Default.FormLocationAlarm;
            }
        }
        #endregion

        #region SnoozeTimer
        private void SnoozeTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = alarmTime - DateTime.Now; // 计算剩余贪睡时间

            if (remainingTime.TotalSeconds <= 0)
            {
                timer.Stop();
                timeLabel.Text = "00:00:00";
                alarmSound.PlayLooping();
            }
            else
            {
                timeLabel.Text = remainingTime.ToString(@"hh\:mm\:ss");
            }
        }
        #endregion

        #region snoozeButton
        private void snoozeButton_Click(object sender, EventArgs e)
        {
            timer1.Stop(); // 停止原始闹钟定时器
            alarmSound.Stop(); // 停止闹钟声音
            alarmTime = DateTime.Now.AddMinutes(5); // 将闹钟时间设置为从现在起5分钟后
            timer.Start(); // 启动贪睡定时器
            timeLabel.Text = "05:00:00"; // 显示初始贪睡时间
        }
        #endregion
    }
}