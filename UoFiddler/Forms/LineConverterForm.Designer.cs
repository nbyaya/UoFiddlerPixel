// /***************************************************************************
//  *
//  * $Author: Turley
//  * Advanced Nikodemus
//  * 
//  * \"啤酒-葡萄酒许可证\"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯啤酒和葡萄酒作为回报。
//  *
//  ***************************************************************************/

namespace UoFiddler.Forms
{
    partial class LineConverterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineConverterForm));
            TextBoxInputOutput = new System.Windows.Forms.TextBox();
            ContextMenuStripCovert = new System.Windows.Forms.ContextMenuStrip(components);
            searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripTextBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            textReplacementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            BtnConvert = new System.Windows.Forms.Button();
            BtnConvert2 = new System.Windows.Forms.Button();
            lbCounter = new System.Windows.Forms.Label();
            BtnClear = new System.Windows.Forms.Button();
            BtnCopy = new System.Windows.Forms.Button();
            BtnRestore = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            chkAddSpaces = new System.Windows.Forms.CheckBox();
            BtnConvertParagraphsToLines2WithoutComments = new System.Windows.Forms.Button();
            BtnConvertWithBlocks = new System.Windows.Forms.Button();
            chkBlockSize4000 = new System.Windows.Forms.CheckBox();
            lblBlockCount = new System.Windows.Forms.Label();
            LbInfo = new System.Windows.Forms.Label();
            TBBlockCount = new System.Windows.Forms.TextBox();
            lbBlockSize = new System.Windows.Forms.Label();
            BtnBullBlockSize = new System.Windows.Forms.Button();
            LbCharactercounter = new System.Windows.Forms.Label();
            LbInfo2 = new System.Windows.Forms.Label();
            BtnNewConversion = new System.Windows.Forms.Button();
            ContextMenuStripCovert.SuspendLayout();
            SuspendLayout();
            // 
            // TextBoxInputOutput
            // 
            TextBoxInputOutput.ContextMenuStrip = ContextMenuStripCovert;
            TextBoxInputOutput.Location = new System.Drawing.Point(12, 89);
            TextBoxInputOutput.MaxLength = 2000000;
            TextBoxInputOutput.Multiline = true;
            TextBoxInputOutput.Name = "TextBoxInputOutput";
            TextBoxInputOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            TextBoxInputOutput.Size = new System.Drawing.Size(993, 318);
            TextBoxInputOutput.TabIndex = 0;
            TextBoxInputOutput.TextChanged += TextBoxInputOutput_TextChanged;
            // 
            // ContextMenuStripCovert
            // 
            ContextMenuStripCovert.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { searchToolStripMenuItem, textReplacementToolStripMenuItem, toolStripSeparator1, loadToolStripMenuItem, saveToolStripMenuItem });
            ContextMenuStripCovert.Name = "ContextMenuStripCovert";
            ContextMenuStripCovert.Size = new System.Drawing.Size(165, 98);
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripTextBoxSearch });
            searchToolStripMenuItem.Image = Properties.Resources.Mirror;
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            searchToolStripMenuItem.Text = "搜索";
            // 
            // ToolStripTextBoxSearch
            // 
            ToolStripTextBoxSearch.Name = "ToolStripTextBoxSearch";
            ToolStripTextBoxSearch.Size = new System.Drawing.Size(100, 23);
            ToolStripTextBoxSearch.TextChanged += ToolStripTextBoxSearch_TextChanged;
            // 
            // textReplacementToolStripMenuItem
            // 
            textReplacementToolStripMenuItem.Image = Properties.Resources.reload_files;
            textReplacementToolStripMenuItem.Name = "textReplacementToolStripMenuItem";
            textReplacementToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            textReplacementToolStripMenuItem.Text = "文本替换";
            textReplacementToolStripMenuItem.Click += TextReplacementToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Image = Properties.Resources.Directory;
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            loadToolStripMenuItem.Text = "加载";
            loadToolStripMenuItem.Click += LoadToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = Properties.Resources.notepad;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            saveToolStripMenuItem.Text = "保存";
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            // 
            // BtnConvert
            // 
            BtnConvert.Location = new System.Drawing.Point(12, 422);
            BtnConvert.Name = "BtnConvert";
            BtnConvert.Size = new System.Drawing.Size(75, 23);
            BtnConvert.TabIndex = 1;
            BtnConvert.Text = "转换";
            BtnConvert.UseVisualStyleBackColor = true;
            BtnConvert.Click += BtnConvert_Click;
            // 
            // BtnConvert2
            // 
            BtnConvert2.Location = new System.Drawing.Point(12, 451);
            BtnConvert2.Name = "BtnConvert2";
            BtnConvert2.Size = new System.Drawing.Size(75, 23);
            BtnConvert2.TabIndex = 2;
            BtnConvert2.Text = "转换";
            BtnConvert2.UseVisualStyleBackColor = true;
            BtnConvert2.Click += BtnConvert2_Click;
            // 
            // lbCounter
            // 
            lbCounter.AutoSize = true;
            lbCounter.Location = new System.Drawing.Point(425, 426);
            lbCounter.Name = "lbCounter";
            lbCounter.Size = new System.Drawing.Size(40, 15);
            lbCounter.TabIndex = 3;
            lbCounter.Text = "计数";
            // 
            // BtnClear
            // 
            BtnClear.Location = new System.Drawing.Point(182, 422);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new System.Drawing.Size(75, 23);
            BtnClear.TabIndex = 4;
            BtnClear.Text = "清除";
            BtnClear.UseVisualStyleBackColor = true;
            BtnClear.Click += BtnClear_Click;
            // 
            // BtnCopy
            // 
            BtnCopy.Location = new System.Drawing.Point(263, 422);
            BtnCopy.Name = "BtnCopy";
            BtnCopy.Size = new System.Drawing.Size(75, 23);
            BtnCopy.TabIndex = 5;
            BtnCopy.Text = "复制";
            BtnCopy.UseVisualStyleBackColor = true;
            BtnCopy.Click += BtnCopy_Click;
            // 
            // BtnRestore
            // 
            BtnRestore.Location = new System.Drawing.Point(344, 422);
            BtnRestore.Name = "BtnRestore";
            BtnRestore.Size = new System.Drawing.Size(75, 23);
            BtnRestore.TabIndex = 6;
            BtnRestore.Text = "恢复";
            BtnRestore.UseVisualStyleBackColor = true;
            BtnRestore.Click += BtnRestore_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(994, 75);
            label1.TabIndex = 7;
            label1.Text = resources.GetString("label1.Text");
            // 
            // chkAddSpaces
            // 
            chkAddSpaces.AutoSize = true;
            chkAddSpaces.Location = new System.Drawing.Point(93, 425);
            chkAddSpaces.Name = "chkAddSpaces";
            chkAddSpaces.Size = new System.Drawing.Size(87, 19);
            chkAddSpaces.TabIndex = 8;
            chkAddSpaces.Text = "添加空格";
            chkAddSpaces.UseVisualStyleBackColor = true;
            // 
            // BtnConvertParagraphsToLines2WithoutComments
            // 
            BtnConvertParagraphsToLines2WithoutComments.Location = new System.Drawing.Point(12, 497);
            BtnConvertParagraphsToLines2WithoutComments.Name = "BtnConvertParagraphsToLines2WithoutComments";
            BtnConvertParagraphsToLines2WithoutComments.Size = new System.Drawing.Size(75, 23);
            BtnConvertParagraphsToLines2WithoutComments.TabIndex = 9;
            BtnConvertParagraphsToLines2WithoutComments.Text = "转换";
            BtnConvertParagraphsToLines2WithoutComments.UseVisualStyleBackColor = true;
            BtnConvertParagraphsToLines2WithoutComments.Click += BtnConvertParagraphsToLines2WithoutComments_Click;
            // 
            // BtnConvertWithBlocks
            // 
            BtnConvertWithBlocks.Location = new System.Drawing.Point(89, 497);
            BtnConvertWithBlocks.Name = "BtnConvertWithBlocks";
            BtnConvertWithBlocks.Size = new System.Drawing.Size(150, 23);
            BtnConvertWithBlocks.TabIndex = 10;
            BtnConvertWithBlocks.Text = "转换块大小 8000";
            BtnConvertWithBlocks.UseVisualStyleBackColor = true;
            BtnConvertWithBlocks.Click += BtnConvertWithBlocks_Click;
            // 
            // chkBlockSize4000
            // 
            chkBlockSize4000.AutoSize = true;
            chkBlockSize4000.Location = new System.Drawing.Point(242, 500);
            chkBlockSize4000.Name = "chkBlockSize4000";
            chkBlockSize4000.Size = new System.Drawing.Size(105, 19);
            chkBlockSize4000.TabIndex = 11;
            chkBlockSize4000.Text = "块大小 4000";
            chkBlockSize4000.UseVisualStyleBackColor = true;
            // 
            // lblBlockCount
            // 
            lblBlockCount.AutoSize = true;
            lblBlockCount.Location = new System.Drawing.Point(425, 501);
            lblBlockCount.Name = "lblBlockCount";
            lblBlockCount.Size = new System.Drawing.Size(72, 15);
            lblBlockCount.TabIndex = 12;
            lblBlockCount.Text = "块计数";
            // 
            // LbInfo
            // 
            LbInfo.AutoSize = true;
            LbInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            LbInfo.Location = new System.Drawing.Point(12, 479);
            LbInfo.Name = "LbInfo";
            LbInfo.Size = new System.Drawing.Size(155, 15);
            LbInfo.TabIndex = 13;
            LbInfo.Text = "无注释块:";
            // 
            // TBBlockCount
            // 
            TBBlockCount.Location = new System.Drawing.Point(353, 496);
            TBBlockCount.Name = "TBBlockCount";
            TBBlockCount.Size = new System.Drawing.Size(66, 23);
            TBBlockCount.TabIndex = 14;
            // 
            // lbBlockSize
            // 
            lbBlockSize.AutoSize = true;
            lbBlockSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lbBlockSize.Location = new System.Drawing.Point(350, 478);
            lbBlockSize.Name = "lbBlockSize";
            lbBlockSize.Size = new System.Drawing.Size(107, 15);
            lbBlockSize.TabIndex = 15;
            lbBlockSize.Text = "手动块大小";
            // 
            // BtnBullBlockSize
            // 
            BtnBullBlockSize.Location = new System.Drawing.Point(182, 471);
            BtnBullBlockSize.Name = "BtnBullBlockSize";
            BtnBullBlockSize.Size = new System.Drawing.Size(156, 23);
            BtnBullBlockSize.TabIndex = 16;
            BtnBullBlockSize.Text = "复制剪贴板删除";
            BtnBullBlockSize.UseVisualStyleBackColor = true;
            BtnBullBlockSize.Click += BtnBullBlockSize_Click;
            // 
            // LbCharactercounter
            // 
            LbCharactercounter.AutoSize = true;
            LbCharactercounter.Location = new System.Drawing.Point(936, 426);
            LbCharactercounter.Name = "LbCharactercounter";
            LbCharactercounter.Size = new System.Drawing.Size(43, 15);
            LbCharactercounter.TabIndex = 17;
            LbCharactercounter.Text = "计数:";
            // 
            // LbInfo2
            // 
            LbInfo2.AutoSize = true;
            LbInfo2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            LbInfo2.Location = new System.Drawing.Point(820, 426);
            LbInfo2.Name = "LbInfo2";
            LbInfo2.Size = new System.Drawing.Size(115, 15);
            LbInfo2.TabIndex = 18;
            LbInfo2.Text = "字符计数器 :";
            // 
            // BtnNewConversion
            // 
            BtnNewConversion.Location = new System.Drawing.Point(89, 451);
            BtnNewConversion.Name = "BtnNewConversion";
            BtnNewConversion.Size = new System.Drawing.Size(75, 23);
            BtnNewConversion.TabIndex = 20;
            BtnNewConversion.Text = "转换";
            BtnNewConversion.UseVisualStyleBackColor = true;
            BtnNewConversion.Click += BtnNewConversion_Click;
            // 
            // LineConverterForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1017, 528);
            Controls.Add(BtnNewConversion);
            Controls.Add(LbInfo2);
            Controls.Add(LbCharactercounter);
            Controls.Add(BtnBullBlockSize);
            Controls.Add(lbBlockSize);
            Controls.Add(TBBlockCount);
            Controls.Add(LbInfo);
            Controls.Add(lblBlockCount);
            Controls.Add(chkBlockSize4000);
            Controls.Add(BtnConvertWithBlocks);
            Controls.Add(BtnConvertParagraphsToLines2WithoutComments);
            Controls.Add(chkAddSpaces);
            Controls.Add(label1);
            Controls.Add(BtnRestore);
            Controls.Add(BtnCopy);
            Controls.Add(BtnClear);
            Controls.Add(lbCounter);
            Controls.Add(BtnConvert2);
            Controls.Add(BtnConvert);
            Controls.Add(TextBoxInputOutput);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "LineConverterForm";
            Text = "段落转行转换器 - AI 压缩字符";
            ContextMenuStripCovert.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxInputOutput;
        private System.Windows.Forms.Button BtnConvert;
        private System.Windows.Forms.Button BtnConvert2;
        private System.Windows.Forms.Label lbCounter;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnCopy;
        private System.Windows.Forms.Button BtnRestore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkAddSpaces;
        private System.Windows.Forms.Button BtnConvertParagraphsToLines2WithoutComments;
        private System.Windows.Forms.Button BtnConvertWithBlocks;
        private System.Windows.Forms.CheckBox chkBlockSize4000;
        private System.Windows.Forms.Label lblBlockCount;
        private System.Windows.Forms.Label LbInfo;
        private System.Windows.Forms.TextBox TBBlockCount;
        private System.Windows.Forms.Label lbBlockSize;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStripCovert;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBoxSearch;
        private System.Windows.Forms.Button BtnBullBlockSize;
        private System.Windows.Forms.Label LbCharactercounter;
        private System.Windows.Forms.Label LbInfo2;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textReplacementToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button BtnNewConversion;
    }
}