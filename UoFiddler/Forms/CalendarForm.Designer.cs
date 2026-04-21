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

namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    partial class CalendarForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalendarForm));
            monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            lbCalendarWeek = new System.Windows.Forms.Label();
            lbDate = new System.Windows.Forms.Label();
            lbTime = new System.Windows.Forms.Label();
            timer1 = new System.Windows.Forms.Timer(components);
            lbDaysTo = new System.Windows.Forms.Label();
            lbWeekendDays = new System.Windows.Forms.Label();
            lbWorkingDays = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // monthCalendar1
            // 
            monthCalendar1.ContextMenuStrip = contextMenuStrip1;
            monthCalendar1.Location = new System.Drawing.Point(7, 8);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 0;
            monthCalendar1.DateChanged += monthCalendar_DateChanged;
            monthCalendar1.DateSelected += monthCalendarForm_DateSelected;
            monthCalendar1.MouseHover += monthCalendar1_MouseHover;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // lbCalendarWeek
            // 
            lbCalendarWeek.AutoSize = true;
            lbCalendarWeek.Location = new System.Drawing.Point(12, 175);
            lbCalendarWeek.Name = "lbCalendarWeek";
            lbCalendarWeek.Size = new System.Drawing.Size(82, 15);
            lbCalendarWeek.TabIndex = 1;
            lbCalendarWeek.Text = "日历周";
            // 
            // lbDate
            // 
            lbDate.AutoSize = true;
            lbDate.Location = new System.Drawing.Point(12, 199);
            lbDate.Name = "lbDate";
            lbDate.Size = new System.Drawing.Size(31, 15);
            lbDate.TabIndex = 2;
            lbDate.Text = "日期";
            // 
            // lbTime
            // 
            lbTime.AutoSize = true;
            lbTime.Location = new System.Drawing.Point(12, 224);
            lbTime.Name = "lbTime";
            lbTime.Size = new System.Drawing.Size(33, 15);
            lbTime.TabIndex = 3;
            lbTime.Text = "时间";
            // 
            // lbDaysTo
            // 
            lbDaysTo.AutoSize = true;
            lbDaysTo.Location = new System.Drawing.Point(12, 250);
            lbDaysTo.Name = "lbDaysTo";
            lbDaysTo.Size = new System.Drawing.Size(32, 15);
            lbDaysTo.TabIndex = 5;
            lbDaysTo.Text = "天数";
            // 
            // lbWeekendDays
            // 
            lbWeekendDays.AutoSize = true;
            lbWeekendDays.Location = new System.Drawing.Point(12, 274);
            lbWeekendDays.Name = "lbWeekendDays";
            lbWeekendDays.Size = new System.Drawing.Size(84, 15);
            lbWeekendDays.TabIndex = 6;
            lbWeekendDays.Text = "周末天数";
            // 
            // lbWorkingDays
            // 
            lbWorkingDays.AutoSize = true;
            lbWorkingDays.Location = new System.Drawing.Point(12, 294);
            lbWorkingDays.Name = "lbWorkingDays";
            lbWorkingDays.Size = new System.Drawing.Size(80, 15);
            lbWorkingDays.TabIndex = 7;
            lbWorkingDays.Text = "工作日天数";
            // 
            // CalendarForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(192, 318);
            Controls.Add(lbWorkingDays);
            Controls.Add(lbWeekendDays);
            Controls.Add(lbDaysTo);
            Controls.Add(lbTime);
            Controls.Add(lbDate);
            Controls.Add(lbCalendarWeek);
            Controls.Add(monthCalendar1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CalendarForm";
            Text = "日历";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Label lbCalendarWeek;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label lbDaysTo;
        private System.Windows.Forms.Label lbWeekendDays;
        private System.Windows.Forms.Label lbWorkingDays;
    }
}