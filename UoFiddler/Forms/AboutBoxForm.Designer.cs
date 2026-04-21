/***************************************************************************
 *
 * $Author: Turley
 * 
 * "啤酒许可证"
 * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
 * 如果我们某天相遇，并且你认为这个东西有价值，
 * 你可以请我喝杯啤酒作为回报。
 *
 ***************************************************************************/

namespace UoFiddler.Forms
{
    partial class AboutBoxForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
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
            checkBoxCheckOnStart = new System.Windows.Forms.CheckBox();
            button1 = new System.Windows.Forms.Button();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            checkBoxFormState = new System.Windows.Forms.CheckBox();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            linkLabel2 = new System.Windows.Forms.LinkLabel();
            ShowRepoInfoButton = new System.Windows.Forms.Button();
            animationPanel = new System.Windows.Forms.Panel();
            SuspendLayout();
            // 
            // checkBoxCheckOnStart
            // 
            checkBoxCheckOnStart.AutoSize = true;
            checkBoxCheckOnStart.Location = new System.Drawing.Point(16, 515);
            checkBoxCheckOnStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxCheckOnStart.Name = "checkBoxCheckOnStart";
            checkBoxCheckOnStart.Size = new System.Drawing.Size(105, 19);
            checkBoxCheckOnStart.TabIndex = 1;
            checkBoxCheckOnStart.Text = "启动时检查更新";
            checkBoxCheckOnStart.UseVisualStyleBackColor = true;
            checkBoxCheckOnStart.CheckedChanged += OnChangeCheck;
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.Location = new System.Drawing.Point(139, 511);
            button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(118, 27);
            button1.TabIndex = 2;
            button1.Text = "检查更新";
            button1.UseVisualStyleBackColor = true;
            button1.Click += OnClickUpdate;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new System.Drawing.Point(14, 10);
            linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(91, 15);
            linkLabel1.TabIndex = 4;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "访问主页";
            linkLabel1.LinkClicked += OnClickLink;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            label1.ForeColor = System.Drawing.SystemColors.ControlText;
            label1.Location = new System.Drawing.Point(15, 290);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(151, 13);
            label1.TabIndex = 5;
            label1.Text = "项目作者与管理员";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(48, 309);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(39, 15);
            label2.TabIndex = 6;
            label2.Text = "Turley";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            label3.Location = new System.Drawing.Point(14, 333);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(121, 13);
            label3.TabIndex = 7;
            label3.Text = "特别贡献者";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(48, 351);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(250, 15);
            label4.TabIndex = 8;
            label4.Text = "MuadDib, Soulblighter, Nibbio, Andreew, Ares";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            label5.Location = new System.Drawing.Point(15, 377);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(95, 13);
            label5.TabIndex = 9;
            label5.Text = "特别感谢";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(48, 392);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(367, 15);
            label6.TabIndex = 10;
            label6.Text = "http://www.polserver.com 社区的所有反馈和使用";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(48, 407);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(253, 15);
            label7.TabIndex = 11;
            label7.Text = "UltimaSDK 开发人员，我们基于其框架进行了修改";
            // 
            // checkBoxFormState
            // 
            checkBoxFormState.AutoSize = true;
            checkBoxFormState.Location = new System.Drawing.Point(16, 489);
            checkBoxFormState.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxFormState.Name = "checkBoxFormState";
            checkBoxFormState.Size = new System.Drawing.Size(109, 19);
            checkBoxFormState.TabIndex = 12;
            checkBoxFormState.Text = "存储窗体状态";
            checkBoxFormState.UseVisualStyleBackColor = true;
            checkBoxFormState.CheckedChanged += OnChangeFormState;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            label8.Location = new System.Drawing.Point(16, 432);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(103, 13);
            label8.TabIndex = 13;
            label8.Text = "更多开发者";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(48, 450);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(115, 15);
            label9.TabIndex = 14;
            label9.Text = "AsYlum, Nikodemus";
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Location = new System.Drawing.Point(112, 10);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new System.Drawing.Size(83, 15);
            linkLabel2.TabIndex = 15;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "UoPixelFiddler";
            linkLabel2.LinkClicked += linkLabel2_LinkClicked;
            // 
            // ShowRepoInfoButton
            // 
            ShowRepoInfoButton.Location = new System.Drawing.Point(282, 511);
            ShowRepoInfoButton.Name = "ShowRepoInfoButton";
            ShowRepoInfoButton.Size = new System.Drawing.Size(133, 27);
            ShowRepoInfoButton.TabIndex = 16;
            ShowRepoInfoButton.Text = "显示仓库信息";
            ShowRepoInfoButton.UseVisualStyleBackColor = true;
            ShowRepoInfoButton.Click += ShowRepoInfoButton_Click;
            // 
            // animationPanel
            // 
            animationPanel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            animationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            animationPanel.Location = new System.Drawing.Point(13, 28);
            animationPanel.Name = "animationPanel";
            animationPanel.Size = new System.Drawing.Size(422, 252);
            animationPanel.TabIndex = 17;
            // 
            // AboutBoxForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(448, 551);
            Controls.Add(animationPanel);
            Controls.Add(ShowRepoInfoButton);
            Controls.Add(linkLabel2);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(checkBoxFormState);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(linkLabel1);
            Controls.Add(button1);
            Controls.Add(checkBoxCheckOnStart);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutBoxForm";
            Padding = new System.Windows.Forms.Padding(10);
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "关于";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxCheckOnStart;
        private System.Windows.Forms.CheckBox checkBoxFormState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Button ShowRepoInfoButton;
        private System.Windows.Forms.Panel animationPanel;
    }
}