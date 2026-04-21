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

namespace UoFiddler.Controls.UserControls
{
    partial class MultiMapControl
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiMapControl));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.multiMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facet00ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facet01ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facet02ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facet03ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facet04ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facet05ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.multiMapFromImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facetFromImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.asBmpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.asTiffToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.asPngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asPngToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 25);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(631, 369);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            this.pictureBox.Resize += new System.EventHandler(this.OnResize);
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.Location = new System.Drawing.Point(0, 377);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(614, 17);
            this.hScrollBar.TabIndex = 1;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HandleScroll);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(614, 25);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 369);
            this.vScrollBar.TabIndex = 2;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HandleScroll);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2,
            this.toolStripDropDownButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(631, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.multiMapToolStripMenuItem,
            this.facet00ToolStripMenuItem,
            this.facet01ToolStripMenuItem,
            this.facet02ToolStripMenuItem,
            this.facet03ToolStripMenuItem,
            this.facet04ToolStripMenuItem,
            this.facet05ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(52, 22);
            this.toolStripDropDownButton1.Text = "加载...";
            // 
            // multiMapToolStripMenuItem
            // 
            this.multiMapToolStripMenuItem.Name = "multiMapToolStripMenuItem";
            this.multiMapToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.multiMapToolStripMenuItem.Text = "多重地图";
            this.multiMapToolStripMenuItem.Click += new System.EventHandler(this.ShowImage);
            // 
            // facet00ToolStripMenuItem
            // 
            this.facet00ToolStripMenuItem.Name = "facet00ToolStripMenuItem";
            this.facet00ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.facet00ToolStripMenuItem.Text = "面 00";
            this.facet00ToolStripMenuItem.Click += new System.EventHandler(this.ShowImage);
            // 
            // facet01ToolStripMenuItem
            // 
            this.facet01ToolStripMenuItem.Name = "facet01ToolStripMenuItem";
            this.facet01ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.facet01ToolStripMenuItem.Tag = "";
            this.facet01ToolStripMenuItem.Text = "面 01";
            this.facet01ToolStripMenuItem.Click += new System.EventHandler(this.ShowImage);
            // 
            // facet02ToolStripMenuItem
            // 
            this.facet02ToolStripMenuItem.Name = "facet02ToolStripMenuItem";
            this.facet02ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.facet02ToolStripMenuItem.Tag = "";
            this.facet02ToolStripMenuItem.Text = "面 02";
            this.facet02ToolStripMenuItem.Click += new System.EventHandler(this.ShowImage);
            // 
            // facet03ToolStripMenuItem
            // 
            this.facet03ToolStripMenuItem.Name = "facet03ToolStripMenuItem";
            this.facet03ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.facet03ToolStripMenuItem.Tag = "";
            this.facet03ToolStripMenuItem.Text = "面 03";
            this.facet03ToolStripMenuItem.Click += new System.EventHandler(this.ShowImage);
            // 
            // facet04ToolStripMenuItem
            // 
            this.facet04ToolStripMenuItem.Name = "facet04ToolStripMenuItem";
            this.facet04ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.facet04ToolStripMenuItem.Tag = "";
            this.facet04ToolStripMenuItem.Text = "面 04";
            this.facet04ToolStripMenuItem.Click += new System.EventHandler(this.ShowImage);
            // 
            // facet05ToolStripMenuItem
            // 
            this.facet05ToolStripMenuItem.Name = "facet05ToolStripMenuItem";
            this.facet05ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.facet05ToolStripMenuItem.Tag = "";
            this.facet05ToolStripMenuItem.Text = "面 05";
            this.facet05ToolStripMenuItem.Click += new System.EventHandler(this.ShowImage);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.multiMapFromImageToolStripMenuItem,
            this.facetFromImageToolStripMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(73, 22);
            this.toolStripDropDownButton2.Text = "生成...";
            // 
            // multiMapFromImageToolStripMenuItem
            // 
            this.multiMapFromImageToolStripMenuItem.Name = "multiMapFromImageToolStripMenuItem";
            this.multiMapFromImageToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.multiMapFromImageToolStripMenuItem.Text = "从图像生成多重地图";
            this.multiMapFromImageToolStripMenuItem.Click += new System.EventHandler(this.OnClickGenerateRLE);
            // 
            // facetFromImageToolStripMenuItem
            // 
            this.facetFromImageToolStripMenuItem.Name = "facetFromImageToolStripMenuItem";
            this.facetFromImageToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.facetFromImageToolStripMenuItem.Text = "从图像生成面";
            this.facetFromImageToolStripMenuItem.Click += new System.EventHandler(this.OnClickGenerateFacetFromImage);
            // 
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asBmpToolStripMenuItem1,
            this.asTiffToolStripMenuItem1,
            this.asPngToolStripMenuItem,
            this.asPngToolStripMenuItem1});
            this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(60, 22);
            this.toolStripDropDownButton3.Text = "导出...";
            // 
            // asBmpToolStripMenuItem1
            // 
            this.asBmpToolStripMenuItem1.Name = "asBmpToolStripMenuItem1";
            this.asBmpToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.asBmpToolStripMenuItem1.Text = "导出为 BMP...";
            this.asBmpToolStripMenuItem1.Click += new System.EventHandler(this.OnClickExportBmp);
            // 
            // asTiffToolStripMenuItem1
            // 
            this.asTiffToolStripMenuItem1.Name = "asTiffToolStripMenuItem1";
            this.asTiffToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.asTiffToolStripMenuItem1.Text = "导出为 TIFF...";
            this.asTiffToolStripMenuItem1.Click += new System.EventHandler(this.OnClickExportTiff);
            // 
            // asPngToolStripMenuItem
            // 
            this.asPngToolStripMenuItem.Name = "asPngToolStripMenuItem";
            this.asPngToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.asPngToolStripMenuItem.Text = "导出为 JPEG...";
            this.asPngToolStripMenuItem.Click += new System.EventHandler(this.OnClickExportJpg);
            // 
            // asPngToolStripMenuItem1
            // 
            this.asPngToolStripMenuItem1.Name = "asPngToolStripMenuItem1";
            this.asPngToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.asPngToolStripMenuItem1.Text = "导出为 PNG...";
            this.asPngToolStripMenuItem1.Click += new System.EventHandler(this.OnClickExportPng);
            // 
            // MultiMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hScrollBar);
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MultiMapControl";
            this.Size = new System.Drawing.Size(631, 394);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem asBmpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem asPngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asPngToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem asTiffToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem facet00ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facet01ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facet02ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facet03ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facet04ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facet05ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facetFromImageToolStripMenuItem;
        private System.Windows.Forms.HScrollBar hScrollBar;
        private System.Windows.Forms.ToolStripMenuItem multiMapFromImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multiMapToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.VScrollBar vScrollBar;
    }
}