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

namespace UoFiddler.Forms
{
    partial class AlarmClockForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，则为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            timeLabel = new System.Windows.Forms.Label();
            startButton = new System.Windows.Forms.Button();
            stopButton = new System.Windows.Forms.Button();
            LabelTimeReal = new System.Windows.Forms.Label();
            timer1 = new System.Windows.Forms.Timer(components);
            btLoadWave = new System.Windows.Forms.Button();
            openFileDialogWave = new System.Windows.Forms.OpenFileDialog();
            snoozeButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            dateTimePicker1.Location = new System.Drawing.Point(28, 12);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new System.Drawing.Size(115, 23);
            dateTimePicker1.TabIndex = 0;
            // 
            // timeLabel
            // 
            timeLabel.AutoSize = true;
            timeLabel.Location = new System.Drawing.Point(56, 64);
            timeLabel.Name = "timeLabel";
            timeLabel.Size = new System.Drawing.Size(66, 15);
            timeLabel.TabIndex = 1;
            timeLabel.Text = "倒计时";
            // 
            // startButton
            // 
            startButton.Location = new System.Drawing.Point(28, 86);
            startButton.Name = "startButton";
            startButton.Size = new System.Drawing.Size(47, 23);
            startButton.TabIndex = 2;
            startButton.Text = "开始";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // stopButton
            // 
            stopButton.Location = new System.Drawing.Point(148, 86);
            stopButton.Name = "stopButton";
            stopButton.Size = new System.Drawing.Size(44, 23);
            stopButton.TabIndex = 3;
            stopButton.Text = "停止";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += stopButton_Click;
            // 
            // LabelTimeReal
            // 
            LabelTimeReal.AutoSize = true;
            LabelTimeReal.Location = new System.Drawing.Point(56, 40);
            LabelTimeReal.Name = "LabelTimeReal";
            LabelTimeReal.Size = new System.Drawing.Size(55, 15);
            LabelTimeReal.TabIndex = 4;
            LabelTimeReal.Text = "当前时间";
            // 
            // btLoadWave
            // 
            btLoadWave.Location = new System.Drawing.Point(149, 12);
            btLoadWave.Name = "btLoadWave";
            btLoadWave.Size = new System.Drawing.Size(43, 23);
            btLoadWave.TabIndex = 5;
            btLoadWave.Text = "加载";
            btLoadWave.UseVisualStyleBackColor = true;
            btLoadWave.Click += btLoadWave_Click;
            // 
            // openFileDialogWave
            // 
            openFileDialogWave.FileName = "openFileDialogWave";
            // 
            // snoozeButton
            // 
            snoozeButton.Location = new System.Drawing.Point(81, 86);
            snoozeButton.Name = "snoozeButton";
            snoozeButton.Size = new System.Drawing.Size(57, 23);
            snoozeButton.TabIndex = 6;
            snoozeButton.Text = "5分钟";
            snoozeButton.UseVisualStyleBackColor = true;
            snoozeButton.Click += snoozeButton_Click;
            // 
            // AlarmClockForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(218, 118);
            Controls.Add(snoozeButton);
            Controls.Add(btLoadWave);
            Controls.Add(LabelTimeReal);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            Controls.Add(timeLabel);
            Controls.Add(dateTimePicker1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "AlarmClockForm";
            Text = "闹钟";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label LabelTimeReal;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btLoadWave;
        private System.Windows.Forms.OpenFileDialog openFileDialogWave;
        private System.Windows.Forms.Button snoozeButton;
    }
}