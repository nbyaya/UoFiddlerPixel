/***************************************************************************
 *
 * $作者: Turley
 * 
 * "啤酒软件许可协议"
 * 只要你保留此声明，你可以随意使用本代码。
 * 如果我们某天相遇，你觉得这个工具不错，
 * 可以请我喝一杯啤酒作为回报。
 *
 ***************************************************************************/

namespace UoFiddler.Plugin.Compare.UserControls
{
    partial class CompareTextureControl
    {
        /// <summary> 
        /// 设计器必需的变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理正在使用的所有资源。
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改 
        /// 此方法的内容使用代码编辑器。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            listBoxOrg = new System.Windows.Forms.ListBox();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            pictureBoxSec = new System.Windows.Forms.PictureBox();
            pictureBoxOrg = new System.Windows.Forms.PictureBox();
            textBoxSecondDir = new System.Windows.Forms.TextBox();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            listBoxSec = new System.Windows.Forms.ListBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            exportImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            asBmpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            asTiffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyLandTile2To1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkBox1 = new System.Windows.Forms.CheckBox();
            button1 = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            btRemoveIdImage = new System.Windows.Forms.Button();
            btMoveIdImage = new System.Windows.Forms.Button();
            CopyAddOnly = new System.Windows.Forms.Button();
            FromLeftToRight = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSec).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOrg).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // listBoxOrg
            // 
            listBoxOrg.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxOrg.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            listBoxOrg.FormattingEnabled = true;
            listBoxOrg.IntegralHeight = false;
            listBoxOrg.Location = new System.Drawing.Point(4, 3);
            listBoxOrg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxOrg.Name = "listBoxOrg";
            listBoxOrg.Size = new System.Drawing.Size(227, 368);
            listBoxOrg.TabIndex = 0;
            listBoxOrg.DrawItem += DrawitemOrg;
            listBoxOrg.MeasureItem += MeasureOrg;
            listBoxOrg.SelectedIndexChanged += OnIndexChangedOrg;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(pictureBoxSec, 0, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxOrg, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(239, 3);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(383, 368);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // pictureBoxSec
            // 
            pictureBoxSec.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBoxSec.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxSec.Location = new System.Drawing.Point(5, 187);
            pictureBoxSec.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxSec.Name = "pictureBoxSec";
            pictureBoxSec.Size = new System.Drawing.Size(373, 177);
            pictureBoxSec.TabIndex = 3;
            pictureBoxSec.TabStop = false;
            // 
            // pictureBoxOrg
            // 
            pictureBoxOrg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBoxOrg.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxOrg.Location = new System.Drawing.Point(5, 4);
            pictureBoxOrg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxOrg.Name = "pictureBoxOrg";
            pictureBoxOrg.Size = new System.Drawing.Size(373, 176);
            pictureBoxOrg.TabIndex = 2;
            pictureBoxOrg.TabStop = false;
            // 
            // textBoxSecondDir
            // 
            textBoxSecondDir.Location = new System.Drawing.Point(28, 13);
            textBoxSecondDir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxSecondDir.Name = "textBoxSecondDir";
            textBoxSecondDir.Size = new System.Drawing.Size(168, 23);
            textBoxSecondDir.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            tableLayoutPanel2.Controls.Add(listBoxOrg, 0, 0);
            tableLayoutPanel2.Controls.Add(listBoxSec, 2, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel1, 1, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new System.Drawing.Size(862, 374);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // listBoxSec
            // 
            listBoxSec.ContextMenuStrip = contextMenuStrip1;
            listBoxSec.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxSec.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            listBoxSec.FormattingEnabled = true;
            listBoxSec.IntegralHeight = false;
            listBoxSec.Location = new System.Drawing.Point(630, 3);
            listBoxSec.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxSec.Name = "listBoxSec";
            listBoxSec.Size = new System.Drawing.Size(228, 368);
            listBoxSec.TabIndex = 1;
            listBoxSec.DrawItem += DrawItemSec;
            listBoxSec.MeasureItem += MeasureSec;
            listBoxSec.SelectedIndexChanged += OnIndexChangedSec;
            listBoxSec.KeyDown += ListBoxSec_KeyDown;
            listBoxSec.MouseDoubleClick += CopyToLeft_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { exportImageToolStripMenuItem, copyLandTile2To1ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(182, 48);
            // 
            // exportImageToolStripMenuItem
            // 
            exportImageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { asBmpToolStripMenuItem, asTiffToolStripMenuItem });
            exportImageToolStripMenuItem.Name = "exportImageToolStripMenuItem";
            exportImageToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            exportImageToolStripMenuItem.Text = "导出图片..";
            // 
            // asBmpToolStripMenuItem
            // 
            asBmpToolStripMenuItem.Name = "asBmpToolStripMenuItem";
            asBmpToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            asBmpToolStripMenuItem.Text = "BMP 格式";
            asBmpToolStripMenuItem.Click += ExportAsBmp;
            // 
            // asTiffToolStripMenuItem
            // 
            asTiffToolStripMenuItem.Name = "asTiffToolStripMenuItem";
            asTiffToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            asTiffToolStripMenuItem.Text = "TIFF 格式";
            asTiffToolStripMenuItem.Click += ExportAsTiff;
            // 
            // copyLandTile2To1ToolStripMenuItem
            // 
            copyLandTile2To1ToolStripMenuItem.Name = "copyLandTile2To1ToolStripMenuItem";
            copyLandTile2To1ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            copyLandTile2To1ToolStripMenuItem.Text = "复制纹理贴图2到1";
            copyLandTile2To1ToolStripMenuItem.Click += OnClickCopy;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(343, 15);
            checkBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(101, 19);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "仅显示差异项";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.Click += OnChangeShowDiff;
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.Location = new System.Drawing.Point(238, 10);
            button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(99, 29);
            button1.TabIndex = 5;
            button1.Text = "加载第二个文件";
            button1.UseVisualStyleBackColor = true;
            button1.Click += OnClickLoadSecond;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btRemoveIdImage);
            splitContainer1.Panel2.Controls.Add(btMoveIdImage);
            splitContainer1.Panel2.Controls.Add(CopyAddOnly);
            splitContainer1.Panel2.Controls.Add(FromLeftToRight);
            splitContainer1.Panel2.Controls.Add(button2);
            splitContainer1.Panel2.Controls.Add(textBoxSecondDir);
            splitContainer1.Panel2.Controls.Add(checkBox1);
            splitContainer1.Panel2.Controls.Add(button1);
            splitContainer1.Size = new System.Drawing.Size(862, 442);
            splitContainer1.SplitterDistance = 374;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 10;
            // 
            // btRemoveIdImage
            // 
            btRemoveIdImage.Image = Properties.Resources.right;
            btRemoveIdImage.Location = new System.Drawing.Point(740, 3);
            btRemoveIdImage.Name = "btRemoveIdImage";
            btRemoveIdImage.Size = new System.Drawing.Size(54, 55);
            btRemoveIdImage.UseVisualStyleBackColor = true;
            btRemoveIdImage.Click += btRemoveIdImage_Click;
            // 
            // btMoveIdImage
            // 
            btMoveIdImage.Image = Properties.Resources.left;
            btMoveIdImage.Location = new System.Drawing.Point(685, 3);
            btMoveIdImage.Name = "btMoveIdImage";
            btMoveIdImage.Size = new System.Drawing.Size(54, 55);
            btMoveIdImage.UseVisualStyleBackColor = true;
            btMoveIdImage.Click += btMoveIdImage_Click;
            // 
            // CopyAddOnly
            // 
            CopyAddOnly.Location = new System.Drawing.Point(590, 12);
            CopyAddOnly.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyAddOnly.Name = "CopyAddOnly";
            CopyAddOnly.Size = new System.Drawing.Size(88, 27);
            CopyAddOnly.TabIndex = 9;
            CopyAddOnly.Text = "仅复制新增项";//纹理
            CopyAddOnly.UseVisualStyleBackColor = true;
            CopyAddOnly.Click += CopyAddOnly_Click;
            // 
            // FromLeftToRight
            // 
            FromLeftToRight.Location = new System.Drawing.Point(494, 12);
            FromLeftToRight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FromLeftToRight.Name = "FromLeftToRight";
            FromLeftToRight.Size = new System.Drawing.Size(88, 27);
            FromLeftToRight.TabIndex = 8;
            FromLeftToRight.Text = "复制所有差异";
            FromLeftToRight.UseVisualStyleBackColor = true;
            FromLeftToRight.Click += CopyAll_Click;
            // 
            // button2
            // 
            button2.AutoSize = true;
            button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button2.Location = new System.Drawing.Point(204, 12);
            button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(26, 25);
            button2.TabIndex = 7;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += BrowseOnClick;
            // 
            // CompareTextureControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CompareTextureControl";
            Size = new System.Drawing.Size(862, 442);
            Load += OnLoad;
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxSec).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOrg).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox listBoxOrg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBoxSec;
        private System.Windows.Forms.PictureBox pictureBoxOrg;
        private System.Windows.Forms.TextBox textBoxSecondDir;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ListBox listBoxSec;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asBmpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asTiffToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem copyLandTile2To1ToolStripMenuItem;
        private System.Windows.Forms.Button FromLeftToRight;
        private System.Windows.Forms.Button CopyAddOnly;
        private System.Windows.Forms.Button btMoveIdImage;
        private System.Windows.Forms.Button btRemoveIdImage;
    }
}