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
    partial class CompareGumpControl
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
            this.components = new System.ComponentModel.Container();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tiffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bmpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyGump2To1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxSecondDir = new System.Windows.Forms.TextBox();
            // 新增按钮
            this.btLeftMoveItem = new System.Windows.Forms.Button();
            this.btLeftMoveItemMore = new System.Windows.Forms.Button();
            this.btremoveitemfromindex = new System.Windows.Forms.Button();

            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();

            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 60;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(174, 320);
            this.listBox1.TabIndex = 0;
            this.listBox1.Tag = 1;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Listbox1_DrawItem);
            this.listBox1.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.Listbox_measureItem);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.Listbox_SelectedChange);
            // 
            // listBox2
            // 
            this.listBox2.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.IntegralHeight = false;
            this.listBox2.ItemHeight = 60;
            this.listBox2.Location = new System.Drawing.Point(556, 0);
            this.listBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(174, 320);
            this.listBox2.TabIndex = 1;
            this.listBox2.Tag = 2;
            // 支持多选（Ctrl/Shift）
            this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Listbox1_DrawItem);
            this.listBox2.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.Listbox_measureItem);
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.Listbox_SelectedChange);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractAsToolStripMenuItem,
            this.copyGump2To1ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(171, 48);
            // 
            // extractAsToolStripMenuItem
            // 
            this.extractAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tiffToolStripMenuItem,
            this.bmpToolStripMenuItem});
            this.extractAsToolStripMenuItem.Name = "extractAsToolStripMenuItem";
            this.extractAsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.extractAsToolStripMenuItem.Text = "导出图片...";
            // 
            // tiffToolStripMenuItem
            // 
            this.tiffToolStripMenuItem.Name = "tiffToolStripMenuItem";
            this.tiffToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.tiffToolStripMenuItem.Text = "BMP 格式";
            this.tiffToolStripMenuItem.Click += new System.EventHandler(this.Export_Bmp);
            // 
            // bmpToolStripMenuItem
            // 
            this.bmpToolStripMenuItem.Name = "bmpToolStripMenuItem";
            this.bmpToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.bmpToolStripMenuItem.Text = "TIFF 格式";
            this.bmpToolStripMenuItem.Click += new System.EventHandler(this.Export_Tiff);
            // 
            // copyGump2To1ToolStripMenuItem
            // 
            this.copyGump2To1ToolStripMenuItem.Name = "copyGump2To1ToolStripMenuItem";
            this.copyGump2To1ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.copyGump2To1ToolStripMenuItem.Text = "将界面2复制到界面1";
            this.copyGump2To1ToolStripMenuItem.Click += new System.EventHandler(this.OnClickCopy);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(174, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(382, 320);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(4, 3);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(374, 154);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(4, 163);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(374, 154);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel1.Controls.Add(this.listBox2);
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkBox1);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxSecondDir);
            // 添加新按钮
            this.splitContainer1.Panel2.Controls.Add(this.btLeftMoveItem);
            this.splitContainer1.Panel2.Controls.Add(this.btLeftMoveItemMore);
            this.splitContainer1.Panel2.Controls.Add(this.btremoveitemfromindex);
            this.splitContainer1.Size = new System.Drawing.Size(730, 378);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 3;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(493, 18);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(101, 19);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "仅显示差异项";//界面
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.ShowDiff_OnClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(399, 14);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 27);
            this.button2.TabIndex = 2;
            this.button2.Text = "加载";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Load_Click);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.Location = new System.Drawing.Point(362, 14);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Browse_OnClick);
            // 
            // textBoxSecondDir
            // 
            this.textBoxSecondDir.Location = new System.Drawing.Point(175, 16);
            this.textBoxSecondDir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxSecondDir.Name = "textBoxSecondDir";
            this.textBoxSecondDir.Size = new System.Drawing.Size(179, 23);
            this.textBoxSecondDir.TabIndex = 0;
            // 
            // btLeftMoveItem
            // 
            this.btLeftMoveItem.Image = global::UoFiddler.Plugin.Compare.Properties.Resources.left; // 改为左箭头
            this.btLeftMoveItem.Location = new System.Drawing.Point(600, 12);
            this.btLeftMoveItem.Name = "btLeftMoveItem";//"btLeftMoveItem"
            this.btLeftMoveItem.Size = new System.Drawing.Size(52, 30);
            this.btLeftMoveItem.TabIndex = 4;
            this.btLeftMoveItem.UseVisualStyleBackColor = true;
            this.btLeftMoveItem.Click += new System.EventHandler(this.BtLeftMoveItem_Click);
            // 
            // btLeftMoveItemMore
            // 
            this.btLeftMoveItemMore.Image = global::UoFiddler.Plugin.Compare.Properties.Resources.left2;
            this.btLeftMoveItemMore.Location = new System.Drawing.Point(660, 12);
            this.btLeftMoveItemMore.Name = "btLeftMoveItemMore";
            this.btLeftMoveItemMore.Size = new System.Drawing.Size(52, 30);
            this.btLeftMoveItemMore.TabIndex = 5;
            this.btLeftMoveItemMore.UseVisualStyleBackColor = true;
            this.btLeftMoveItemMore.Click += new System.EventHandler(this.BtLeftMoveItemMore_Click);
            // 
            // btremoveitemfromindex
            // 
            this.btremoveitemfromindex.Image = global::UoFiddler.Plugin.Compare.Properties.Resources.right;
            this.btremoveitemfromindex.Location = new System.Drawing.Point(720, 12);
            this.btremoveitemfromindex.Name = "btremoveitemfromindex";// "btremoveitemfromindex"
            this.btremoveitemfromindex.Size = new System.Drawing.Size(52, 30);
            this.btremoveitemfromindex.TabIndex = 6;
            this.btremoveitemfromindex.UseVisualStyleBackColor = true;
            this.btremoveitemfromindex.Click += new System.EventHandler(this.Btremoveitemfromindex_Click);
            // 
            // CompareGumpControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CompareGumpControl";
            this.Size = new System.Drawing.Size(730, 378);
            this.Load += new System.EventHandler(this.OnLoad);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem bmpToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyGump2To1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractAsToolStripMenuItem;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxSecondDir;
        private System.Windows.Forms.ToolStripMenuItem tiffToolStripMenuItem;
        // 新增按钮字段
        private System.Windows.Forms.Button btLeftMoveItem;
        private System.Windows.Forms.Button btLeftMoveItemMore;
        private System.Windows.Forms.Button btremoveitemfromindex;
    }
}