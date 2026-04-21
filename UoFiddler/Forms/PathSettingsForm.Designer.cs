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
    partial class PathSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathSettingsForm));
            pgPaths = new System.Windows.Forms.PropertyGrid();
            contextMenuPath = new System.Windows.Forms.ContextMenuStrip(components);
            newDirAndMulToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            loadSingleMulFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeleteLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tsPathSettingsMenu = new System.Windows.Forms.ToolStrip();
            tsBtnReloadPaths = new System.Windows.Forms.ToolStripButton();
            tsBtnSetPathManual = new System.Windows.Forms.ToolStripButton();
            tsTbRootPath = new System.Windows.Forms.ToolStripTextBox();
            tsBtnBackup = new System.Windows.Forms.ToolStripButton();
            contextMenuPath.SuspendLayout();
            tsPathSettingsMenu.SuspendLayout();
            SuspendLayout();
            // 
            // pgPaths
            // 
            pgPaths.ContextMenuStrip = contextMenuPath;
            pgPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            pgPaths.HelpVisible = false;
            pgPaths.Location = new System.Drawing.Point(0, 25);
            pgPaths.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pgPaths.Name = "pgPaths";
            pgPaths.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            pgPaths.Size = new System.Drawing.Size(744, 396);
            pgPaths.TabIndex = 0;
            pgPaths.ToolbarVisible = false;
            // 
            // contextMenuPath
            // 
            contextMenuPath.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { newDirAndMulToolStripMenuItem, loadSingleMulFileToolStripMenuItem, DeleteLineToolStripMenuItem });
            contextMenuPath.Name = "contextMenuStrip1";
            contextMenuPath.Size = new System.Drawing.Size(205, 70);
            // 
            // newDirAndMulToolStripMenuItem
            // 
            newDirAndMulToolStripMenuItem.Image = Properties.Resources.reload_files;
            newDirAndMulToolStripMenuItem.Name = "newDirAndMulToolStripMenuItem";
            newDirAndMulToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            newDirAndMulToolStripMenuItem.Text = "重新加载所有文件并新建";
            newDirAndMulToolStripMenuItem.ToolTipText = "重新加载并从目录加载所有 .mul 文件";
            newDirAndMulToolStripMenuItem.Click += newDirAndMulToolStripMenuItem_Click;
            // 
            // loadSingleMulFileToolStripMenuItem
            // 
            loadSingleMulFileToolStripMenuItem.Image = Properties.Resources.Directory;
            loadSingleMulFileToolStripMenuItem.Name = "loadSingleMulFileToolStripMenuItem";
            loadSingleMulFileToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            loadSingleMulFileToolStripMenuItem.Text = "加载单个 Mul 文件";
            loadSingleMulFileToolStripMenuItem.ToolTipText = "从目录加载单个文件";
            loadSingleMulFileToolStripMenuItem.Click += loadSingleMulFileToolStripMenuItem_Click;
            // 
            // DeleteLineToolStripMenuItem
            // 
            DeleteLineToolStripMenuItem.Image = Properties.Resources.indentLeft;
            DeleteLineToolStripMenuItem.Name = "DeleteLineToolStripMenuItem";
            DeleteLineToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            DeleteLineToolStripMenuItem.Text = "删除行";
            DeleteLineToolStripMenuItem.ToolTipText = "移除行";
            DeleteLineToolStripMenuItem.Click += DeleteLineToolStripMenuItem_Click;
            // 
            // tsPathSettingsMenu
            // 
            tsPathSettingsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            tsPathSettingsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsBtnReloadPaths, tsBtnSetPathManual, tsTbRootPath, tsBtnBackup });
            tsPathSettingsMenu.Location = new System.Drawing.Point(0, 0);
            tsPathSettingsMenu.Name = "tsPathSettingsMenu";
            tsPathSettingsMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            tsPathSettingsMenu.Size = new System.Drawing.Size(744, 25);
            tsPathSettingsMenu.TabIndex = 1;
            tsPathSettingsMenu.Text = "toolStrip1";
            // 
            // tsBtnReloadPaths
            // 
            tsBtnReloadPaths.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsBtnReloadPaths.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsBtnReloadPaths.Name = "tsBtnReloadPaths";
            tsBtnReloadPaths.Size = new System.Drawing.Size(79, 22);
            tsBtnReloadPaths.Text = "重新加载路径";
            tsBtnReloadPaths.Click += ReloadPath;
            // 
            // tsBtnSetPathManual
            // 
            tsBtnSetPathManual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsBtnSetPathManual.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsBtnSetPathManual.Name = "tsBtnSetPathManual";
            tsBtnSetPathManual.Size = new System.Drawing.Size(97, 22);
            tsBtnSetPathManual.Text = "手动设置路径";
            tsBtnSetPathManual.Click += OnClickManual;
            // 
            // tsTbRootPath
            // 
            tsTbRootPath.Name = "tsTbRootPath";
            tsTbRootPath.Size = new System.Drawing.Size(408, 25);
            tsTbRootPath.KeyDown += OnKeyDownDir;
            // 
            // tsBtnBackup
            // 
            tsBtnBackup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsBtnBackup.Image = (System.Drawing.Image)resources.GetObject("tsBtnBackup.Image");
            tsBtnBackup.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsBtnBackup.Name = "tsBtnBackup";
            tsBtnBackup.Size = new System.Drawing.Size(50, 22);
            tsBtnBackup.Text = "备份";
            tsBtnBackup.ToolTipText = "将目录文件夹完整复制到目标文件夹";
            tsBtnBackup.Click += tsBtnBackup_Click;
            // 
            // PathSettingsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(744, 421);
            Controls.Add(pgPaths);
            Controls.Add(tsPathSettingsMenu);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximumSize = new System.Drawing.Size(791, 917);
            MinimumSize = new System.Drawing.Size(744, 340);
            Name = "PathSettingsForm";
            Text = "路径设置";
            contextMenuPath.ResumeLayout(false);
            tsPathSettingsMenu.ResumeLayout(false);
            tsPathSettingsMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgPaths;
        private System.Windows.Forms.ToolStripButton tsBtnReloadPaths;
        private System.Windows.Forms.ToolStripButton tsBtnSetPathManual;
        private System.Windows.Forms.ToolStrip tsPathSettingsMenu;
        private System.Windows.Forms.ContextMenuStrip contextMenuPath;
        private System.Windows.Forms.ToolStripMenuItem newDirAndMulToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSingleMulFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteLineToolStripMenuItem;
        public System.Windows.Forms.ToolStripTextBox tsTbRootPath;
        private System.Windows.Forms.ToolStripButton tsBtnBackup;
    }
}