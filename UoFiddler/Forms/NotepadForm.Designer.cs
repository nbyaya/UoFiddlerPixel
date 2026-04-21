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

namespace UoFiddler.Forms
{
    partial class NotepadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotepadForm));
            btSaveText = new System.Windows.Forms.Button();
            richTextBoxNotPad = new System.Windows.Forms.RichTextBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            dateTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripTextBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            btBold = new System.Windows.Forms.Button();
            cbFontSize = new System.Windows.Forms.ComboBox();
            noteId = new System.Windows.Forms.TextBox();
            cBTextListing = new System.Windows.Forms.ComboBox();
            textBoxHeadLine = new System.Windows.Forms.TextBox();
            btDelete = new System.Windows.Forms.Button();
            checkBox1 = new System.Windows.Forms.CheckBox();
            BtUnderline = new System.Windows.Forms.Button();
            BtItalic = new System.Windows.Forms.Button();
            btEdit = new System.Windows.Forms.Button();
            btFont = new System.Windows.Forms.Button();
            checkBoxLines = new System.Windows.Forms.CheckBox();
            listBoxLineNumbers = new System.Windows.Forms.ListBox();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            btLoad = new System.Windows.Forms.Button();
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            colorButton = new System.Windows.Forms.Button();
            dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            btnAlignLeft = new System.Windows.Forms.Button();
            btnAlignCenter = new System.Windows.Forms.Button();
            btnAlignRight = new System.Windows.Forms.Button();
            btnBullet = new System.Windows.Forms.Button();
            btnLineNumber = new System.Windows.Forms.Button();
            btnDecreaseIndent = new System.Windows.Forms.Button();
            btnIncreaseIndent = new System.Windows.Forms.Button();
            btFontColor = new System.Windows.Forms.Button();
            btSaveAs = new System.Windows.Forms.Button();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            PrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            printDialog = new System.Windows.Forms.PrintDialog();
            contextMenuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btSaveText
            // 
            btSaveText.Location = new System.Drawing.Point(634, 122);
            btSaveText.Name = "btSaveText";
            btSaveText.Size = new System.Drawing.Size(48, 23);
            btSaveText.TabIndex = 1;
            btSaveText.Text = "保存";
            btSaveText.UseVisualStyleBackColor = true;
            btSaveText.Click += btSaveText_Click;
            // 
            // richTextBoxNotPad
            // 
            richTextBoxNotPad.ContextMenuStrip = contextMenuStrip1;
            richTextBoxNotPad.Location = new System.Drawing.Point(63, 125);
            richTextBoxNotPad.Name = "richTextBoxNotPad";
            richTextBoxNotPad.Size = new System.Drawing.Size(565, 334);
            richTextBoxNotPad.TabIndex = 2;
            richTextBoxNotPad.Text = "";
            richTextBoxNotPad.SelectionChanged += richTextBoxNotPad_SelectionChanged;
            richTextBoxNotPad.VScroll += richTextBoxNotPad_VScroll;
            richTextBoxNotPad.TextChanged += richTextBoxNotPad_TextChanged_1;
            richTextBoxNotPad.KeyPress += richTextBoxNotPad_KeyPress;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { dateTimeToolStripMenuItem, searchToolStripMenuItem, deleteToolStripMenuItem, undoToolStripMenuItem, replaceToolStripMenuItem, selectAllToolStripMenuItem, copyToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(130, 158);
            // 
            // dateTimeToolStripMenuItem
            // 
            dateTimeToolStripMenuItem.Name = "dateTimeToolStripMenuItem";
            dateTimeToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            dateTimeToolStripMenuItem.Text = "日期/时间";
            dateTimeToolStripMenuItem.Click += dateTimeToolStripMenuItem_Click;
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripTextBoxSearch });
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            searchToolStripMenuItem.Text = "搜索";
            // 
            // toolStripTextBoxSearch
            // 
            toolStripTextBoxSearch.Name = "toolStripTextBoxSearch";
            toolStripTextBoxSearch.Size = new System.Drawing.Size(100, 23);
            toolStripTextBoxSearch.TextChanged += toolStripTextBoxSearch_TextChanged;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            deleteToolStripMenuItem.Text = "删除";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            undoToolStripMenuItem.Text = "撤销";
            undoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
            // 
            // replaceToolStripMenuItem
            // 
            replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            replaceToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            replaceToolStripMenuItem.Text = "替换";
            replaceToolStripMenuItem.Click += replaceToolStripMenuItem_Click;
            // 
            // selectAllToolStripMenuItem
            // 
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            selectAllToolStripMenuItem.Text = "全选";
            selectAllToolStripMenuItem.Click += selectAllToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            copyToolStripMenuItem.Text = "复制";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // btBold
            // 
            btBold.Location = new System.Drawing.Point(63, 67);
            btBold.Name = "btBold";
            btBold.Size = new System.Drawing.Size(44, 22);
            btBold.TabIndex = 3;
            btBold.Text = "粗体";
            btBold.UseVisualStyleBackColor = true;
            btBold.Click += BtBold_Click;
            // 
            // cbFontSize
            // 
            cbFontSize.FormattingEnabled = true;
            cbFontSize.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" });
            cbFontSize.Location = new System.Drawing.Point(113, 67);
            cbFontSize.Name = "cbFontSize";
            cbFontSize.Size = new System.Drawing.Size(43, 23);
            cbFontSize.TabIndex = 4;
            cbFontSize.Text = "12";
            cbFontSize.SelectedIndexChanged += CbFontSize_SelectedIndexChanged;
            // 
            // noteId
            // 
            noteId.Location = new System.Drawing.Point(12, 97);
            noteId.Name = "noteId";
            noteId.Size = new System.Drawing.Size(45, 23);
            noteId.TabIndex = 5;
            // 
            // cBTextListing
            // 
            cBTextListing.FormattingEnabled = true;
            cBTextListing.Location = new System.Drawing.Point(287, 97);
            cBTextListing.Name = "cBTextListing";
            cBTextListing.Size = new System.Drawing.Size(52, 23);
            cBTextListing.TabIndex = 6;
            cBTextListing.SelectedIndexChanged += cBTextListing_SelectedIndexChanged;
            // 
            // textBoxHeadLine
            // 
            textBoxHeadLine.Location = new System.Drawing.Point(63, 97);
            textBoxHeadLine.Name = "textBoxHeadLine";
            textBoxHeadLine.Size = new System.Drawing.Size(218, 23);
            textBoxHeadLine.TabIndex = 7;
            // 
            // btDelete
            // 
            btDelete.Location = new System.Drawing.Point(634, 151);
            btDelete.Name = "btDelete";
            btDelete.Size = new System.Drawing.Size(48, 23);
            btDelete.TabIndex = 8;
            btDelete.Text = "删除";
            btDelete.UseVisualStyleBackColor = true;
            btDelete.Click += btDelete_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(345, 41);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(106, 19);
            checkBox1.TabIndex = 9;
            checkBox1.Text = "粗体文本切换";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // BtUnderline
            // 
            BtUnderline.Location = new System.Drawing.Point(158, 67);
            BtUnderline.Name = "BtUnderline";
            BtUnderline.Size = new System.Drawing.Size(19, 22);
            BtUnderline.TabIndex = 10;
            BtUnderline.Text = "_";
            BtUnderline.UseVisualStyleBackColor = true;
            BtUnderline.Click += BtUnderline_Click;
            // 
            // BtItalic
            // 
            BtItalic.Location = new System.Drawing.Point(177, 67);
            BtItalic.Name = "BtItalic";
            BtItalic.Size = new System.Drawing.Size(19, 22);
            BtItalic.TabIndex = 11;
            BtItalic.UseVisualStyleBackColor = true;
            BtItalic.Click += BtItalic_Click;
            // 
            // btEdit
            // 
            btEdit.Location = new System.Drawing.Point(634, 180);
            btEdit.Name = "btEdit";
            btEdit.Size = new System.Drawing.Size(48, 23);
            btEdit.TabIndex = 12;
            btEdit.Text = "编辑";
            btEdit.UseVisualStyleBackColor = true;
            btEdit.Click += btEdit_Click;
            // 
            // btFont
            // 
            btFont.Location = new System.Drawing.Point(287, 67);
            btFont.Name = "btFont";
            btFont.Size = new System.Drawing.Size(52, 22);
            btFont.TabIndex = 13;
            btFont.Text = "字体";
            btFont.UseVisualStyleBackColor = true;
            btFont.Click += btFont_Click;
            // 
            // checkBoxLines
            // 
            checkBoxLines.AutoSize = true;
            checkBoxLines.Checked = true;
            checkBoxLines.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxLines.Location = new System.Drawing.Point(12, 69);
            checkBoxLines.Name = "checkBoxLines";
            checkBoxLines.Size = new System.Drawing.Size(53, 19);
            checkBoxLines.TabIndex = 14;
            checkBoxLines.Text = "行号";
            checkBoxLines.UseVisualStyleBackColor = true;
            checkBoxLines.CheckedChanged += checkBoxLines_CheckedChanged;
            // 
            // listBoxLineNumbers
            // 
            listBoxLineNumbers.FormattingEnabled = true;
            listBoxLineNumbers.ItemHeight = 15;
            listBoxLineNumbers.Location = new System.Drawing.Point(12, 125);
            listBoxLineNumbers.Name = "listBoxLineNumbers";
            listBoxLineNumbers.Size = new System.Drawing.Size(45, 334);
            listBoxLineNumbers.TabIndex = 15;
            listBoxLineNumbers.MouseClick += listBoxLineNumbers_MouseClick;
            listBoxLineNumbers.SelectedIndexChanged += richTextBoxNotPad_TextChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new System.Drawing.Point(0, 471);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(704, 22);
            statusStrip1.TabIndex = 16;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            toolStripStatusLabel1.Text = "状态";
            // 
            // btLoad
            // 
            btLoad.Location = new System.Drawing.Point(634, 209);
            btLoad.Name = "btLoad";
            btLoad.Size = new System.Drawing.Size(48, 23);
            btLoad.TabIndex = 17;
            btLoad.Text = "加载";
            btLoad.UseVisualStyleBackColor = true;
            btLoad.Click += btLoad_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // colorButton
            // 
            colorButton.Location = new System.Drawing.Point(247, 67);
            colorButton.Name = "colorButton";
            colorButton.Size = new System.Drawing.Size(40, 22);
            colorButton.TabIndex = 18;
            colorButton.Text = "背景色";
            colorButton.UseVisualStyleBackColor = true;
            colorButton.Click += colorButton_Click;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new System.Drawing.Point(181, 38);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new System.Drawing.Size(158, 23);
            dateTimePicker1.TabIndex = 19;
            dateTimePicker1.CloseUp += dateTimePicker_CloseUp;
            // 
            // btnAlignLeft
            // 
            btnAlignLeft.Image = Properties.Resources.align_text_Left;
            btnAlignLeft.Location = new System.Drawing.Point(339, 68);
            btnAlignLeft.Name = "btnAlignLeft";
            btnAlignLeft.Size = new System.Drawing.Size(37, 31);
            btnAlignLeft.TabIndex = 20;
            btnAlignLeft.UseVisualStyleBackColor = true;
            btnAlignLeft.Click += btnAlignLeft_Click;
            // 
            // btnAlignCenter
            // 
            btnAlignCenter.Image = Properties.Resources.align_text_middle;
            btnAlignCenter.Location = new System.Drawing.Point(378, 67);
            btnAlignCenter.Name = "btnAlignCenter";
            btnAlignCenter.Size = new System.Drawing.Size(33, 32);
            btnAlignCenter.TabIndex = 21;
            btnAlignCenter.UseVisualStyleBackColor = true;
            btnAlignCenter.Click += btnAlignCenter_Click;
            // 
            // btnAlignRight
            // 
            btnAlignRight.Image = Properties.Resources.align_text_Right;
            btnAlignRight.Location = new System.Drawing.Point(413, 68);
            btnAlignRight.Name = "btnAlignRight";
            btnAlignRight.Size = new System.Drawing.Size(37, 31);
            btnAlignRight.TabIndex = 22;
            btnAlignRight.UseVisualStyleBackColor = true;
            btnAlignRight.Click += btnAlignRight_Click;
            // 
            // btnBullet
            // 
            btnBullet.Image = Properties.Resources.Dot;
            btnBullet.Location = new System.Drawing.Point(452, 67);
            btnBullet.Name = "btnBullet";
            btnBullet.Size = new System.Drawing.Size(24, 32);
            btnBullet.TabIndex = 23;
            btnBullet.UseVisualStyleBackColor = true;
            btnBullet.Click += btnBullet_Click;
            // 
            // btnLineNumber
            // 
            btnLineNumber.Image = Properties.Resources.Numbers;
            btnLineNumber.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            btnLineNumber.Location = new System.Drawing.Point(477, 67);
            btnLineNumber.Name = "btnLineNumber";
            btnLineNumber.Size = new System.Drawing.Size(40, 32);
            btnLineNumber.TabIndex = 24;
            btnLineNumber.UseVisualStyleBackColor = true;
            btnLineNumber.Click += btnLineNumber_Click;
            // 
            // btnDecreaseIndent
            // 
            btnDecreaseIndent.Image = Properties.Resources.indentRight;
            btnDecreaseIndent.Location = new System.Drawing.Point(559, 67);
            btnDecreaseIndent.Name = "btnDecreaseIndent";
            btnDecreaseIndent.Size = new System.Drawing.Size(40, 32);
            btnDecreaseIndent.TabIndex = 25;
            btnDecreaseIndent.UseVisualStyleBackColor = true;
            btnDecreaseIndent.Click += btnDecreaseIndent_Click;
            // 
            // btnIncreaseIndent
            // 
            btnIncreaseIndent.Image = Properties.Resources.indentLeft;
            btnIncreaseIndent.Location = new System.Drawing.Point(518, 67);
            btnIncreaseIndent.Name = "btnIncreaseIndent";
            btnIncreaseIndent.Size = new System.Drawing.Size(40, 32);
            btnIncreaseIndent.TabIndex = 26;
            btnIncreaseIndent.UseVisualStyleBackColor = true;
            btnIncreaseIndent.Click += btnIncreaseIndent_Click;
            // 
            // btFontColor
            // 
            btFontColor.Location = new System.Drawing.Point(195, 67);
            btFontColor.Name = "btFontColor";
            btFontColor.Size = new System.Drawing.Size(53, 22);
            btFontColor.TabIndex = 27;
            btFontColor.Text = "字体颜色";
            btFontColor.UseVisualStyleBackColor = true;
            btFontColor.Click += btFontColor_Click;
            // 
            // btSaveAs
            // 
            btSaveAs.Location = new System.Drawing.Point(634, 238);
            btSaveAs.Name = "btSaveAs";
            btSaveAs.Size = new System.Drawing.Size(58, 23);
            btSaveAs.TabIndex = 28;
            btSaveAs.Text = "另存为";
            btSaveAs.UseVisualStyleBackColor = true;
            btSaveAs.Click += btSaveAs_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1 });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(704, 24);
            menuStrip1.TabIndex = 29;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { loadToolStripMenuItem, saveToolStripMenuItem, PrintToolStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(46, 20);
            toolStripMenuItem1.Text = "文件";
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            loadToolStripMenuItem.Text = "加载";
            loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            saveToolStripMenuItem.Text = "保存";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // PrintToolStripMenuItem
            // 
            PrintToolStripMenuItem.Name = "PrintToolStripMenuItem";
            PrintToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            PrintToolStripMenuItem.Text = "打印";
            PrintToolStripMenuItem.Click += PrintToolStripMenuItem_Click;
            // 
            // printDialog
            // 
            printDialog.UseEXDialog = true;
            // 
            // NotepadForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(704, 493);
            Controls.Add(btSaveAs);
            Controls.Add(btFontColor);
            Controls.Add(btnIncreaseIndent);
            Controls.Add(btnDecreaseIndent);
            Controls.Add(btnLineNumber);
            Controls.Add(btnBullet);
            Controls.Add(btnAlignRight);
            Controls.Add(btnAlignCenter);
            Controls.Add(btnAlignLeft);
            Controls.Add(dateTimePicker1);
            Controls.Add(colorButton);
            Controls.Add(btLoad);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(listBoxLineNumbers);
            Controls.Add(checkBoxLines);
            Controls.Add(btFont);
            Controls.Add(btEdit);
            Controls.Add(BtItalic);
            Controls.Add(BtUnderline);
            Controls.Add(checkBox1);
            Controls.Add(btDelete);
            Controls.Add(textBoxHeadLine);
            Controls.Add(cBTextListing);
            Controls.Add(noteId);
            Controls.Add(cbFontSize);
            Controls.Add(btBold);
            Controls.Add(richTextBoxNotPad);
            Controls.Add(btSaveText);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "NotepadForm";
            Text = "记事本";
            contextMenuStrip1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btSaveText;
        private System.Windows.Forms.RichTextBox richTextBoxNotPad;
        private System.Windows.Forms.Button btBold;
        private System.Windows.Forms.ComboBox cbFontSize;
        private System.Windows.Forms.TextBox noteId;
        private System.Windows.Forms.ComboBox cBTextListing;
        private System.Windows.Forms.TextBox textBoxHeadLine;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button BtUnderline;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateTimeToolStripMenuItem;
        private System.Windows.Forms.Button BtItalic;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSearch;
        private System.Windows.Forms.Button btEdit;
        private System.Windows.Forms.Button btFont;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxLines;
        private System.Windows.Forms.ListBox listBoxLineNumbers;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.Button btnAlignLeft;
        private System.Windows.Forms.Button btnAlignCenter;
        private System.Windows.Forms.Button btnAlignRight;
        private System.Windows.Forms.Button btnBullet;
        private System.Windows.Forms.Button btnLineNumber;
        private System.Windows.Forms.Button btnDecreaseIndent;
        private System.Windows.Forms.Button btnIncreaseIndent;
        private System.Windows.Forms.Button btFontColor;
        private System.Windows.Forms.Button btSaveAs;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PrintToolStripMenuItem;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}