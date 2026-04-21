/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE" (啤酒软件授权协议)
 * 只要您保留此声明，您就可以对本代码做任何处理。
 * 如果将来我们相遇，并且您认为这些东西有价值，
 * 您可以请我喝啤酒作为回报。
 *
 ***************************************************************************/

namespace UoFiddler.Controls.UserControls
{
    partial class SpeechControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpeechControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.IDEntry = new System.Windows.Forms.ToolStripTextBox();
            this.IDButton = new System.Windows.Forms.ToolStripButton();
            this.IDNextButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.KeyWordEntry = new System.Windows.Forms.ToolStripTextBox();
            this.KeyWordButton = new System.Windows.Forms.ToolStripButton();
            this.KeyWordNextButton = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.collapsibleSplitter1 = new UoFiddler.Controls.UserControls.CollapsibleSplitter();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.IDEntry,
            this.IDButton,
            this.IDNextButton,
            this.toolStripSeparator1,
            this.KeyWordEntry,
            this.KeyWordButton,
            this.KeyWordNextButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 33);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(736, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // IDEntry
            // 
            this.IDEntry.MaxLength = 10;
            this.IDEntry.Name = "IDEntry";
            this.IDEntry.Size = new System.Drawing.Size(116, 25);
            this.IDEntry.Text = "查找 ID...";
            this.IDEntry.Enter += new System.EventHandler(this.IDEntry_Enter);
            // 
            // IDButton
            // 
            this.IDButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.IDButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IDButton.Name = "IDButton";
            this.IDButton.Size = new System.Drawing.Size(34, 22);
            this.IDButton.Text = "查找";
            this.IDButton.Click += new System.EventHandler(this.OnClickFindID);
            // 
            // IDNextButton
            // 
            this.IDNextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.IDNextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IDNextButton.Name = "IDNextButton";
            this.IDNextButton.Size = new System.Drawing.Size(62, 22);
            this.IDNextButton.Text = "查找下一个";
            this.IDNextButton.Click += new System.EventHandler(this.OnClickNextID);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // KeyWordEntry
            // 
            this.KeyWordEntry.Name = "KeyWordEntry";
            this.KeyWordEntry.Size = new System.Drawing.Size(116, 25);
            this.KeyWordEntry.Text = "关键词...";
            this.KeyWordEntry.Enter += new System.EventHandler(this.KeyWordEntry_Enter);
            // 
            // KeyWordButton
            // 
            this.KeyWordButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.KeyWordButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.KeyWordButton.Name = "KeyWordButton";
            this.KeyWordButton.Size = new System.Drawing.Size(34, 22);
            this.KeyWordButton.Text = "查找";
            this.KeyWordButton.Click += new System.EventHandler(this.OnClickFindKeyWord);
            // 
            // KeyWordNextButton
            // 
            this.KeyWordNextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.KeyWordNextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.KeyWordNextButton.Name = "KeyWordNextButton";
            this.KeyWordNextButton.Size = new System.Drawing.Size(62, 22);
            this.KeyWordNextButton.Text = "查找下一个";
            this.KeyWordNextButton.Click += new System.EventHandler(this.OnClickNextKeyWord);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 58);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(736, 322);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellValueChanged);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnHeaderClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEntryToolStripMenuItem,
            this.deleteEntryToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 48);
            // 
            // addEntryToolStripMenuItem
            // 
            this.addEntryToolStripMenuItem.Name = "addEntryToolStripMenuItem";
            this.addEntryToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.addEntryToolStripMenuItem.Text = "添加条目";
            this.addEntryToolStripMenuItem.Click += new System.EventHandler(this.OnAddEntry);
            // 
            // deleteEntryToolStripMenuItem
            // 
            this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
            this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.deleteEntryToolStripMenuItem.Text = "删除条目";
            this.deleteEntryToolStripMenuItem.Click += new System.EventHandler(this.OnDeleteEntry);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator3,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(736, 25);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton1.Text = "保存";
            this.toolStripButton1.Click += new System.EventHandler(this.OnClickSave);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(83, 22);
            this.toolStripButton2.Text = "导出为 CSV";
            this.toolStripButton2.Click += new System.EventHandler(this.OnClickExport);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(100, 22);
            this.toolStripButton3.Text = "从 CSV 导入";
            this.toolStripButton3.Click += new System.EventHandler(this.OnClickImport);
            // 
            // collapsibleSplitter1
            // 
            this.collapsibleSplitter1.AnimationDelay = 20;
            this.collapsibleSplitter1.AnimationStep = 20;
            this.collapsibleSplitter1.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
            this.collapsibleSplitter1.ControlToHide = this.toolStrip2;
            this.collapsibleSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsibleSplitter1.ExpandParentForm = false;
            this.collapsibleSplitter1.Location = new System.Drawing.Point(0, 25);
            this.collapsibleSplitter1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.collapsibleSplitter1.Name = "collapsibleSplitter1";
            this.collapsibleSplitter1.Size = new System.Drawing.Size(736, 8);
            this.collapsibleSplitter1.TabIndex = 5;
            this.collapsibleSplitter1.TabStop = false;
            this.collapsibleSplitter1.UseAnimations = false;
            this.collapsibleSplitter1.VisualStyle = UoFiddler.Controls.UserControls.VisualStyles.DoubleDots;
            // 
            // SpeechControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.collapsibleSplitter1);
            this.Controls.Add(this.toolStrip2);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SpeechControl";
            this.Size = new System.Drawing.Size(736, 380);
            this.Load += new System.EventHandler(this.OnLoad);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem addEntryToolStripMenuItem;
        private UoFiddler.Controls.UserControls.CollapsibleSplitter collapsibleSplitter1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton IDButton;
        private System.Windows.Forms.ToolStripTextBox IDEntry;
        private System.Windows.Forms.ToolStripButton IDNextButton;
        private System.Windows.Forms.ToolStripButton KeyWordButton;
        private System.Windows.Forms.ToolStripTextBox KeyWordEntry;
        private System.Windows.Forms.ToolStripButton KeyWordNextButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}