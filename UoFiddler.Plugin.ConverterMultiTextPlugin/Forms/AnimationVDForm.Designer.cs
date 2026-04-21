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
    partial class AnimationVDForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationVDForm));
            pictureBoxAminImage = new System.Windows.Forms.PictureBox();
            contextMenuStripPictureBox = new System.Windows.Forms.ContextMenuStrip(components);
            importImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            createGifToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            panel1 = new System.Windows.Forms.Panel();
            btEmptyImages = new System.Windows.Forms.Button();
            ButtonOpenTempGrafic = new System.Windows.Forms.Button();
            labelSpeed = new System.Windows.Forms.Label();
            btStopAminID = new System.Windows.Forms.Button();
            checkBoxLoop = new System.Windows.Forms.CheckBox();
            trackBarSpeedAmin = new System.Windows.Forms.TrackBar();
            btPlayAminID = new System.Windows.Forms.Button();
            btLoadAminID = new System.Windows.Forms.Button();
            checkedListBoxAminID = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAminImage).BeginInit();
            contextMenuStripPictureBox.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarSpeedAmin).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxAminImage
            // 
            pictureBoxAminImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pictureBoxAminImage.ContextMenuStrip = contextMenuStripPictureBox;
            pictureBoxAminImage.Location = new System.Drawing.Point(20, 15);
            pictureBoxAminImage.Name = "pictureBoxAminImage";
            pictureBoxAminImage.Size = new System.Drawing.Size(148, 316);
            pictureBoxAminImage.TabIndex = 0;
            pictureBoxAminImage.TabStop = false;
            // 
            // contextMenuStripPictureBox
            // 
            contextMenuStripPictureBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { importImageToolStripMenuItem, loadImageToolStripMenuItem, toolStripSeparator1, playToolStripMenuItem, stopToolStripMenuItem, toolStripSeparator2, createGifToolStripMenuItem });
            contextMenuStripPictureBox.Name = "contextMenuStripPictureBox";
            contextMenuStripPictureBox.Size = new System.Drawing.Size(202, 126);
            // 
            // importImageToolStripMenuItem
            // 
            importImageToolStripMenuItem.Image = Properties.Resources.import;
            importImageToolStripMenuItem.Name = "importImageToolStripMenuItem";
            importImageToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            importImageToolStripMenuItem.Text = "从剪贴板导入图像";
            importImageToolStripMenuItem.ToolTipText = "从缓存中导入图形";
            importImageToolStripMenuItem.Click += importImageToolStripMenuItem_Click;
            // 
            // loadImageToolStripMenuItem
            // 
            loadImageToolStripMenuItem.Image = Properties.Resources.Load;
            loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            loadImageToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            loadImageToolStripMenuItem.Text = "加载图像";
            loadImageToolStripMenuItem.ToolTipText = "从目录加载图像";
            loadImageToolStripMenuItem.Click += btLoadAminID_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // playToolStripMenuItem
            // 
            playToolStripMenuItem.Image = Properties.Resources.right;
            playToolStripMenuItem.Name = "playToolStripMenuItem";
            playToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            playToolStripMenuItem.Text = "播放";
            playToolStripMenuItem.ToolTipText = "播放动画";
            playToolStripMenuItem.Click += btPlayAminID_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Image = Properties.Resources.uomc02;
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            stopToolStripMenuItem.Text = "停止";
            stopToolStripMenuItem.ToolTipText = "停止动画";
            stopToolStripMenuItem.Click += btStopAminID_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(198, 6);
            // 
            // createGifToolStripMenuItem
            // 
            createGifToolStripMenuItem.Image = Properties.Resources.uomc33;
            createGifToolStripMenuItem.Name = "createGifToolStripMenuItem";
            createGifToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            createGifToolStripMenuItem.Text = "创建 GIF";
            createGifToolStripMenuItem.ToolTipText = "创建 GIF 图像";
            createGifToolStripMenuItem.Click += createGifToolStripMenuItem_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(btEmptyImages);
            panel1.Controls.Add(ButtonOpenTempGrafic);
            panel1.Controls.Add(labelSpeed);
            panel1.Controls.Add(btStopAminID);
            panel1.Controls.Add(checkBoxLoop);
            panel1.Controls.Add(trackBarSpeedAmin);
            panel1.Controls.Add(btPlayAminID);
            panel1.Controls.Add(btLoadAminID);
            panel1.Controls.Add(checkedListBoxAminID);
            panel1.Controls.Add(pictureBoxAminImage);
            panel1.Location = new System.Drawing.Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(342, 346);
            panel1.TabIndex = 1;
            // 
            // btEmptyImages
            // 
            btEmptyImages.Location = new System.Drawing.Point(246, 87);
            btEmptyImages.Name = "btEmptyImages";
            btEmptyImages.Size = new System.Drawing.Size(75, 23);
            btEmptyImages.TabIndex = 9;
            btEmptyImages.Text = "清空";
            btEmptyImages.UseVisualStyleBackColor = true;
            btEmptyImages.Click += btEmptyImages_Click;
            // 
            // ButtonOpenTempGrafic
            // 
            ButtonOpenTempGrafic.Location = new System.Drawing.Point(246, 257);
            ButtonOpenTempGrafic.Name = "ButtonOpenTempGrafic";
            ButtonOpenTempGrafic.Size = new System.Drawing.Size(75, 23);
            ButtonOpenTempGrafic.TabIndex = 8;
            ButtonOpenTempGrafic.Text = "临时图形目录";
            ButtonOpenTempGrafic.UseVisualStyleBackColor = true;
            ButtonOpenTempGrafic.Click += ButtonOpenTempGrafic_Click_1;
            // 
            // labelSpeed
            // 
            labelSpeed.AutoSize = true;
            labelSpeed.Location = new System.Drawing.Point(188, 316);
            labelSpeed.Name = "labelSpeed";
            labelSpeed.Size = new System.Drawing.Size(39, 15);
            labelSpeed.TabIndex = 7;
            labelSpeed.Text = "速度";
            // 
            // btStopAminID
            // 
            btStopAminID.Location = new System.Drawing.Point(246, 63);
            btStopAminID.Name = "btStopAminID";
            btStopAminID.Size = new System.Drawing.Size(75, 23);
            btStopAminID.TabIndex = 6;
            btStopAminID.Text = "停止";
            btStopAminID.UseVisualStyleBackColor = true;
            btStopAminID.Click += btStopAminID_Click;
            // 
            // checkBoxLoop
            // 
            checkBoxLoop.AutoSize = true;
            checkBoxLoop.Checked = true;
            checkBoxLoop.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxLoop.Location = new System.Drawing.Point(188, 261);
            checkBoxLoop.Name = "checkBoxLoop";
            checkBoxLoop.Size = new System.Drawing.Size(53, 19);
            checkBoxLoop.TabIndex = 5;
            checkBoxLoop.Text = "循环";
            checkBoxLoop.UseVisualStyleBackColor = true;
            // 
            // trackBarSpeedAmin
            // 
            trackBarSpeedAmin.Location = new System.Drawing.Point(188, 286);
            trackBarSpeedAmin.Maximum = 5;
            trackBarSpeedAmin.Minimum = 1;
            trackBarSpeedAmin.Name = "trackBarSpeedAmin";
            trackBarSpeedAmin.Size = new System.Drawing.Size(104, 45);
            trackBarSpeedAmin.TabIndex = 4;
            trackBarSpeedAmin.Value = 3;
            // 
            // btPlayAminID
            // 
            btPlayAminID.Location = new System.Drawing.Point(246, 39);
            btPlayAminID.Name = "btPlayAminID";
            btPlayAminID.Size = new System.Drawing.Size(75, 23);
            btPlayAminID.TabIndex = 3;
            btPlayAminID.Text = "播放";
            btPlayAminID.UseVisualStyleBackColor = true;
            btPlayAminID.Click += btPlayAminID_Click;
            // 
            // btLoadAminID
            // 
            btLoadAminID.Location = new System.Drawing.Point(246, 15);
            btLoadAminID.Name = "btLoadAminID";
            btLoadAminID.Size = new System.Drawing.Size(75, 23);
            btLoadAminID.TabIndex = 2;
            btLoadAminID.Text = "加载";
            btLoadAminID.UseVisualStyleBackColor = true;
            btLoadAminID.Click += btLoadAminID_Click;
            // 
            // checkedListBoxAminID
            // 
            checkedListBoxAminID.FormattingEnabled = true;
            checkedListBoxAminID.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            checkedListBoxAminID.Location = new System.Drawing.Point(188, 15);
            checkedListBoxAminID.Name = "checkedListBoxAminID";
            checkedListBoxAminID.Size = new System.Drawing.Size(39, 184);
            checkedListBoxAminID.TabIndex = 1;
            checkedListBoxAminID.ItemCheck += CheckedListBoxAminID_ItemCheck;
            // 
            // AnimationVDForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(362, 372);
            Controls.Add(panel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "AnimationVDForm";
            Text = "动画播放器";
            FormClosing += AnimationVDForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pictureBoxAminImage).EndInit();
            contextMenuStripPictureBox.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarSpeedAmin).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxAminImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btLoadAminID;
        private System.Windows.Forms.CheckedListBox checkedListBoxAminID;
        private System.Windows.Forms.Button btPlayAminID;
        private System.Windows.Forms.CheckBox checkBoxLoop;
        private System.Windows.Forms.TrackBar trackBarSpeedAmin;
        private System.Windows.Forms.Button btStopAminID;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPictureBox;
        private System.Windows.Forms.ToolStripMenuItem importImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createGifToolStripMenuItem;
        private System.Windows.Forms.Button ButtonOpenTempGrafic;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btEmptyImages;
    }
}