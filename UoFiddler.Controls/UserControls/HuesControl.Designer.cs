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
    partial class HuesControl
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HuesControl));
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ReplaceText = new System.Windows.Forms.ToolStripTextBox();
            exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExportAllHueNamesListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            vScrollBar = new System.Windows.Forms.VScrollBar();
            pictureBox = new System.Windows.Forms.PictureBox();
            HuesTopMenuToolStrip = new System.Windows.Forms.ToolStrip();
            HueIndexToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            HueIndexToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            HueNameToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            HueNameToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            SearchNameToolStripButton = new System.Windows.Forms.ToolStripButton();
            contextMenuStrip1.SuspendLayout();
            toolStripContainer.ContentPanel.SuspendLayout();
            toolStripContainer.TopToolStripPanel.SuspendLayout();
            toolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            HuesTopMenuToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { saveToolStripMenuItem, replaceToolStripMenuItem, exportToolStripMenuItem, importToolStripMenuItem, ExportAllHueNamesListToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(203, 114);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = Properties.Resources.save;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            saveToolStripMenuItem.Text = "保存";
            saveToolStripMenuItem.ToolTipText = "保存颜色。";
            saveToolStripMenuItem.Click += OnClickSave;
            // 
            // replaceToolStripMenuItem
            // 
            replaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ReplaceText });
            replaceToolStripMenuItem.Image = Properties.Resources.replace;
            replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            replaceToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            replaceToolStripMenuItem.Text = "替换为...";
            replaceToolStripMenuItem.ToolTipText = "替换为";
            // 
            // ReplaceText
            // 
            ReplaceText.Name = "ReplaceText";
            ReplaceText.Size = new System.Drawing.Size(100, 23);
            ReplaceText.KeyDown += OnKeyDownReplace;
            ReplaceText.TextChanged += OnTextChangedReplace;
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Image = Properties.Resources.Export;
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            exportToolStripMenuItem.Text = "导出...";
            exportToolStripMenuItem.ToolTipText = "导出颜色。";
            exportToolStripMenuItem.Click += OnExport;
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.Image = Properties.Resources.import;
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            importToolStripMenuItem.Text = "导入...";
            importToolStripMenuItem.ToolTipText = "导入颜色。";
            importToolStripMenuItem.Click += OnImport;
            // 
            // ExportAllHueNamesListToolStripMenuItem
            // 
            ExportAllHueNamesListToolStripMenuItem.Image = Properties.Resources.Text;
            ExportAllHueNamesListToolStripMenuItem.Name = "ExportAllHueNamesListToolStripMenuItem";
            ExportAllHueNamesListToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            ExportAllHueNamesListToolStripMenuItem.Text = "导出所有色调名称列表";
            ExportAllHueNamesListToolStripMenuItem.ToolTipText = "将整个色调名称列表导出到一个列表。";
            ExportAllHueNamesListToolStripMenuItem.Click += ExportAllHueNamesListToolStripMenuItem_Click;
            // 
            // toolStripContainer
            // 
            toolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer.ContentPanel
            // 
            toolStripContainer.ContentPanel.AutoScroll = true;
            toolStripContainer.ContentPanel.Controls.Add(vScrollBar);
            toolStripContainer.ContentPanel.Controls.Add(pictureBox);
            toolStripContainer.ContentPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            toolStripContainer.ContentPanel.Size = new System.Drawing.Size(740, 359);
            toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripContainer.LeftToolStripPanelVisible = false;
            toolStripContainer.Location = new System.Drawing.Point(1, 1);
            toolStripContainer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            toolStripContainer.Name = "toolStripContainer";
            toolStripContainer.RightToolStripPanelVisible = false;
            toolStripContainer.Size = new System.Drawing.Size(740, 390);
            toolStripContainer.TabIndex = 9;
            toolStripContainer.Text = "toolStripContainer";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            toolStripContainer.TopToolStripPanel.Controls.Add(HuesTopMenuToolStrip);
            // 
            // vScrollBar
            // 
            vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            vScrollBar.Location = new System.Drawing.Point(723, 0);
            vScrollBar.Name = "vScrollBar";
            vScrollBar.Size = new System.Drawing.Size(17, 359);
            vScrollBar.TabIndex = 4;
            vScrollBar.Scroll += OnScroll;
            // 
            // pictureBox
            // 
            pictureBox.ContextMenuStrip = contextMenuStrip1;
            pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBox.Location = new System.Drawing.Point(0, 0);
            pictureBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new System.Drawing.Size(740, 359);
            pictureBox.TabIndex = 3;
            pictureBox.TabStop = false;
            pictureBox.SizeChanged += OnResize;
            pictureBox.Paint += OnPaint;
            pictureBox.MouseClick += OnMouseClick;
            pictureBox.MouseDoubleClick += OnMouseDoubleClick;
            // 
            // HuesTopMenuToolStrip
            // 
            HuesTopMenuToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            HuesTopMenuToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            HuesTopMenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { HueIndexToolStripLabel, HueIndexToolStripTextBox, HueNameToolStripLabel, HueNameToolStripTextBox, SearchNameToolStripButton });
            HuesTopMenuToolStrip.Location = new System.Drawing.Point(0, 0);
            HuesTopMenuToolStrip.Name = "HuesTopMenuToolStrip";
            HuesTopMenuToolStrip.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            HuesTopMenuToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            HuesTopMenuToolStrip.Size = new System.Drawing.Size(740, 31);
            HuesTopMenuToolStrip.Stretch = true;
            HuesTopMenuToolStrip.TabIndex = 0;
            // 
            // HueIndexToolStripLabel
            // 
            HueIndexToolStripLabel.Name = "HueIndexToolStripLabel";
            HueIndexToolStripLabel.Size = new System.Drawing.Size(64, 20);
            HueIndexToolStripLabel.Text = "色调索引：";
            // 
            // HueIndexToolStripTextBox
            // 
            HueIndexToolStripTextBox.Name = "HueIndexToolStripTextBox";
            HueIndexToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            HueIndexToolStripTextBox.KeyUp += HueIndexToolStripTextBox_KeyUp;
            // 
            // HueNameToolStripLabel
            // 
            HueNameToolStripLabel.Name = "HueNameToolStripLabel";
            HueNameToolStripLabel.Size = new System.Drawing.Size(67, 20);
            HueNameToolStripLabel.Text = "色调名称：";
            // 
            // HueNameToolStripTextBox
            // 
            HueNameToolStripTextBox.Name = "HueNameToolStripTextBox";
            HueNameToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            HueNameToolStripTextBox.KeyUp += HueNameToolStripTextBox_KeyUp;
            // 
            // SearchNameToolStripButton
            // 
            SearchNameToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            SearchNameToolStripButton.Image = (System.Drawing.Image)resources.GetObject("SearchNameToolStripButton.Image");
            SearchNameToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            SearchNameToolStripButton.Name = "SearchNameToolStripButton";
            SearchNameToolStripButton.Size = new System.Drawing.Size(60, 20);
            SearchNameToolStripButton.Text = "查找下一个";
            SearchNameToolStripButton.Click += SearchNameToolStripButton_Click;
            // 
            // HuesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(toolStripContainer);
            DoubleBuffered = true;
            ForeColor = System.Drawing.SystemColors.ControlText;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "HuesControl";
            Padding = new System.Windows.Forms.Padding(1);
            Size = new System.Drawing.Size(742, 392);
            Load += OnLoad;
            contextMenuStrip1.ResumeLayout(false);
            toolStripContainer.ContentPanel.ResumeLayout(false);
            toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer.TopToolStripPanel.PerformLayout();
            toolStripContainer.ResumeLayout(false);
            toolStripContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            HuesTopMenuToolStrip.ResumeLayout(false);
            HuesTopMenuToolStrip.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox ReplaceText;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStrip HuesTopMenuToolStrip;
        private System.Windows.Forms.ToolStripLabel HueIndexToolStripLabel;
        private System.Windows.Forms.ToolStripTextBox HueIndexToolStripTextBox;
        private System.Windows.Forms.ToolStripLabel HueNameToolStripLabel;
        private System.Windows.Forms.ToolStripTextBox HueNameToolStripTextBox;
        private System.Windows.Forms.ToolStripButton SearchNameToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem ExportAllHueNamesListToolStripMenuItem;
    }
}