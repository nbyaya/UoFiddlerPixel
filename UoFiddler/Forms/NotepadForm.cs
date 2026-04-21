// /***************************************************************************
//  *
//  * $Author: Nikodemus
//  * 
//  * "啤酒许可证"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯啤酒作为回报。
//  *
//  ***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Windows.Documents;
using System.Drawing.Printing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Ultima;

namespace UoFiddler.Forms
{

    public partial class NotepadForm : Form
    {
        private XmlDocument doc = new XmlDocument();
        // 粗体状态
        private bool isBoldActive = false;
        // 下划线状态
        private bool isUnderlineActive = false;
        // 斜体状态
        private bool isItalicActive = false;
        // 项目符号状态
        private bool isBulletActive = false;
        // 行号状态
        bool isLineNumberActive = false;

        // 创建一个新的 DateTimePicker
        DateTimePicker dateTimePicker = new DateTimePicker();

        // 用于保存窗体的全局变量
        private System.Windows.Forms.Form replaceForm = null;

        public NotepadForm()
        {
            InitializeComponent();

            // 添加事件处理程序
            richTextBoxNotPad.TextChanged += new EventHandler(richTextBoxNotPad_TextChanged);
            listBoxLineNumbers.SelectedIndexChanged += new EventHandler(listBoxLineNumbers_SelectedIndexChanged);

            // 添加事件处理程序
            richTextBoxNotPad.TextChanged += new EventHandler(richTextBoxNotPad_TextChanged);

            // 创建一个 ToolTip 实例
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();

            // 为按钮设置工具提示
            ToolTip1.SetToolTip(this.btEdit, "用于编辑条目，重新保存所选 ID");
            ToolTip1.SetToolTip(this.btDelete, "用于删除已注册 ID 的条目");
            ToolTip1.SetToolTip(this.btSaveText, "用于保存当前条目");
            ToolTip1.SetToolTip(this.btBold, "切换粗体样式");
            ToolTip1.SetToolTip(this.BtUnderline, "切换下划线样式");
            ToolTip1.SetToolTip(this.BtItalic, "切换斜体样式");
            ToolTip1.SetToolTip(this.btLoad, "加载文本");
            ToolTip1.SetToolTip(this.btFont, "更改字体");
            ToolTip1.SetToolTip(this.btFontColor, "更改字体颜色");
            ToolTip1.SetToolTip(this.colorButton, "更改背景颜色");
            ToolTip1.SetToolTip(this.btnBullet, "切换项目符号样式");
            ToolTip1.SetToolTip(this.btnLineNumber, "切换行号样式");
            ToolTip1.SetToolTip(this.btnIncreaseIndent, "增加缩进");
            ToolTip1.SetToolTip(this.btnDecreaseIndent, "减少缩进");
            ToolTip1.SetToolTip(this.btnAlignLeft, "左对齐文本");
            ToolTip1.SetToolTip(this.btnAlignCenter, "居中对齐文本");
            ToolTip1.SetToolTip(this.btnAlignRight, "右对齐文本");
            ToolTip1.SetToolTip(this.btSaveAs, "将文本以相应格式保存到目标目录");
            //ToolTip1.SetToolTip(this.btPrint, "打印文本");

            // 将按钮文本设置为斜体 "i" 符号
            string italicIcon = "\uD835\uDC56";
            BtItalic.Text = italicIcon;

            // 检查文件是否存在
            if (!File.Exists("NotepadMessage.xml"))
            {
                // 如果文件不存在，则创建一个新文件
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
                doc.AppendChild(dec);

                // 创建主元素
                XmlElement mainElem = doc.CreateElement("Notes");
                doc.AppendChild(mainElem);

                // 保存空文档
                doc.Save("NotepadMessage.xml");
            }

            // 启动应用程序时加载现有笔记
            doc.Load("NotepadMessage.xml");
            int maxId = 0;
            foreach (XmlElement noteElement in doc.GetElementsByTagName("Note"))
            {
                cBTextListing.Items.Add(noteElement.GetAttribute("id"));
                int id = int.Parse(noteElement.GetAttribute("id"));
                if (id > maxId)
                    maxId = id;
            }
            noteId.Text = (maxId + 1).ToString();

            // 加载所选笔记的 RTF 文本
            if (cBTextListing.Items.Count > 0)
            {
                cBTextListing.SelectedIndex = 0;
                string selectedId = cBTextListing.SelectedItem.ToString();
                foreach (XmlElement noteElement in doc.GetElementsByTagName("Note"))
                {
                    if (noteElement.GetAttribute("id") == selectedId)
                    {
                        richTextBoxNotPad.Rtf = noteElement.GetAttribute("rtfText");
                        break;
                    }
                }
            }

            listBoxLineNumbers.SelectionMode = System.Windows.Forms.SelectionMode.One;

        }

        #region 粗体
        private void BtBold_Click(object sender, EventArgs e)
        {
            Font currentFont = richTextBoxNotPad.SelectionFont;
            FontStyle newFontStyle;

            if (richTextBoxNotPad.SelectionFont.Bold)
            {
                newFontStyle = FontStyle.Regular;
                btBold.BackColor = SystemColors.Control; // 默认背景色
            }
            else
            {
                newFontStyle = FontStyle.Bold;
                btBold.BackColor = Color.LightBlue; // 高亮背景色
            }

            richTextBoxNotPad.SelectionFont = new Font(
               currentFont.FontFamily,
               currentFont.Size,
               newFontStyle
           );

            isBoldActive = !isBoldActive; // 切换状态
        }
        #endregion

        #region 字体大小
        private void CbFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            float newSize;

            if (float.TryParse(cbFontSize.SelectedItem.ToString(), out newSize))
            {
                Font currentFont = richTextBoxNotPad.SelectionFont;
                richTextBoxNotPad.SelectionFont = new Font(
                   currentFont.FontFamily,
                   newSize,
                   currentFont.Style
               );
            }
        }
        #endregion
        #region 保存
        private void btSaveText_Click(object sender, EventArgs e)
        {
            // 为笔记创建新元素
            XmlElement noteElement = doc.CreateElement("Note");
            noteElement.SetAttribute("id", noteId.Text);
            noteElement.SetAttribute("headline", textBoxHeadLine.Text);

            // 保存 RTF 文本而不是普通文本
            noteElement.SetAttribute("rtfText", richTextBoxNotPad.Rtf);

            // 将元素添加到 XmlDocument
            doc.DocumentElement.AppendChild(noteElement);

            // 将 XmlDocument 保存到文件
            doc.Save("NotepadMessage.xml");

            // 将 ID 添加到 ComboBox
            cBTextListing.Items.Add(noteId.Text);

            // 为下一个笔记递增 ID
            noteId.Text = (int.Parse(noteId.Text) + 1).ToString();
        }
        #endregion
        #region RichTextBox 事件
        private void cBTextListing_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedId = cBTextListing.SelectedItem.ToString();

            foreach (XmlElement noteElement in doc.GetElementsByTagName("Note"))
            {
                if (noteElement.GetAttribute("id") == selectedId)
                {
                    textBoxHeadLine.Text = noteElement.GetAttribute("headline");
                    // 加载 RTF 文本而不是普通文本
                    richTextBoxNotPad.Rtf = noteElement.GetAttribute("rtfText");
                    break;
                }
            }
        }
        #endregion

        #region 删除按钮
        private void btDelete_Click(object sender, EventArgs e)
        {
            string selectedId = noteId.Text;
            XmlElement toDelete = null;

            // 查找要删除的项
            foreach (XmlElement noteElement in doc.GetElementsByTagName("Note"))
            {
                if (noteElement.GetAttribute("id") == selectedId)
                {
                    toDelete = noteElement;
                    break;
                }
            }

            // 如果找到该项
            if (toDelete != null)
            {
                // 显示确认对话框
                DialogResult dialogResult = MessageBox.Show("确定要删除此笔记吗？", "确认", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // 删除项
                    doc.DocumentElement.RemoveChild(toDelete);

                    // 重新排序 ID
                    int i = 1;
                    foreach (XmlElement noteElement in doc.GetElementsByTagName("Note"))
                    {
                        noteElement.SetAttribute("id", i.ToString());
                        i++;
                    }

                    // 保存 XmlDocument
                    doc.Save("NotepadMessage.xml");

                    // 更新 ComboBox
                    cBTextListing.Items.Clear();
                    foreach (XmlElement noteElement in doc.GetElementsByTagName("Note"))
                    {
                        cBTextListing.Items.Add(noteElement.GetAttribute("id"));
                    }
                }
            }
            // 将 noteId 更新为最后一个笔记的 ID
            noteId.Text = (cBTextListing.Items.Count > 0 ? cBTextListing.Items[cBTextListing.Items.Count - 1] : "1").ToString();
        }
        #endregion
        #region 复选框事件
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // 选中复选框时，将文本设为粗体并更改背景色
                richTextBoxNotPad.SelectionFont = new Font(richTextBoxNotPad.Font, FontStyle.Bold);
                checkBox1.BackColor = Color.LightBlue; // 背景色
            }
            else
            {
                // 取消选中复选框时，将文本设为常规并恢复背景色
                richTextBoxNotPad.SelectionFont = new Font(richTextBoxNotPad.Font, FontStyle.Regular);
                checkBox1.BackColor = SystemColors.Control; // 默认背景色
            }
        }
        #endregion

        #region 下划线
        private void BtUnderline_Click(object sender, EventArgs e)
        {
            Font currentFont = richTextBoxNotPad.SelectionFont;
            FontStyle newFontStyle;

            if (richTextBoxNotPad.SelectionFont.Underline)
            {
                newFontStyle = FontStyle.Regular;
                BtUnderline.BackColor = SystemColors.Control; // 默认背景色
            }
            else
            {
                newFontStyle = FontStyle.Underline;
                BtUnderline.BackColor = Color.LightBlue; // 高亮背景色
            }

            richTextBoxNotPad.SelectionFont = new Font(
               currentFont.FontFamily,
               currentFont.Size,
               newFontStyle
           );

            isUnderlineActive = !isUnderlineActive; // 切换状态
        }
        #endregion

        #region 日期时间
        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 获取当前日期和时间
            string dateTimeNow = DateTime.Now.ToString();

            // 在光标位置插入日期和时间
            richTextBoxNotPad.SelectedText = dateTimeNow;
        }
        #endregion

        #region 斜体
        private void BtItalic_Click(object sender, EventArgs e)
        {

            Font currentFont = richTextBoxNotPad.SelectionFont;
            FontStyle newFontStyle;

            if (richTextBoxNotPad.SelectionFont.Italic)
            {
                newFontStyle = FontStyle.Regular;
                BtItalic.BackColor = SystemColors.Control; // 默认背景色
            }
            else
            {
                newFontStyle = FontStyle.Italic;
                BtItalic.BackColor = Color.LightBlue; // 高亮背景色
            }

            richTextBoxNotPad.SelectionFont = new Font(
               currentFont.FontFamily,
               currentFont.Size,
               newFontStyle
           );

            isItalicActive = !isItalicActive; // 切换状态
        }
        #endregion
        #region 搜索
        private void toolStripTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            // 获取全部文本
            string text = richTextBoxNotPad.Text;

            // 获取当前搜索查询
            string query = toolStripTextBoxSearch.Text;

            // 重置文本并移除格式
            richTextBoxNotPad.Text = text;

            // 如果搜索查询不为空
            if (!string.IsNullOrEmpty(query))
            {
                // 遍历文本并查找所有出现的搜索查询
                int index = 0;
                while ((index = text.IndexOf(query, index)) != -1)
                {
                    // 选中并高亮找到的文本
                    richTextBoxNotPad.Select(index, query.Length);
                    richTextBoxNotPad.SelectionBackColor = Color.Yellow; // 更改为所需的高亮颜色

                    // 转到下一个出现位置
                    index += query.Length;
                }
            }
        }
        #endregion

        #region 编辑按钮
        private void btEdit_Click(object sender, EventArgs e)
        {
            // 获取所选 ID
            string selectedId = noteId.Text;

            // 查找要编辑的项
            XmlElement toEdit = null;
            foreach (XmlElement noteElement in doc.GetElementsByTagName("Note"))
            {
                if (noteElement.GetAttribute("id") == selectedId)
                {
                    toEdit = noteElement;
                    break;
                }
            }

            // 如果找到该项
            if (toEdit != null)
            {
                // 更新元素的属性
                toEdit.SetAttribute("headline", textBoxHeadLine.Text);
                toEdit.SetAttribute("rtfText", richTextBoxNotPad.Rtf);

                // 保存 XmlDocument
                doc.Save("NotepadMessage.xml");
            }
            // 将 noteId 更新为最后一个笔记的 ID
            noteId.Text = (cBTextListing.Items.Count > 0 ? cBTextListing.Items[cBTextListing.Items.Count - 1] : "1").ToString();

        }
        #endregion

        #region 字体
        private void btFont_Click(object sender, EventArgs e)
        {
            // 创建一个新的 FontDialog
            FontDialog fontDialog = new FontDialog();

            // 将当前字体设置为对话框中的选中字体
            fontDialog.Font = richTextBoxNotPad.SelectionFont;

            // 显示对话框并确认用户点击了确定
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                // 将选中的字体设置为所选文本的字体
                richTextBoxNotPad.SelectionFont = fontDialog.Font;
            }
        }
        #endregion

        #region 删除
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 确保已选中文本
            if (!string.IsNullOrEmpty(richTextBoxNotPad.SelectedText))
            {
                // 删除选中的文本
                richTextBoxNotPad.SelectedText = string.Empty;
            }
        }
        #endregion

        #region 撤销
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查是否可以撤销操作
            if (richTextBoxNotPad.CanUndo)
            {
                // 撤销上一步操作
                richTextBoxNotPad.Undo();
            }
        }
        #endregion

        #region 行计数器
        private void richTextBoxNotPad_TextChanged(object sender, EventArgs e)
        {
            // 仅当 checkBoxLines 被选中时，文本更改时更新行号
            if (checkBoxLines.Checked)
            {
                UpdateLineNumbers();
            }
        }

        private void richTextBoxNotPad_VScroll(object sender, EventArgs e)
        {
            // 仅当 checkBoxLines 被选中时，用户滚动时更新行号
            if (checkBoxLines.Checked)
            {
                UpdateLineNumbers();
            }
        }        
        private void UpdateLineNumbers()
        {
            // 保存当前选中的行号
            int selectedIndex = listBoxLineNumbers.SelectedIndex;

            // 统计 richTextBoxNotPad 中的行数
            int lineCount = richTextBoxNotPad.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;

            // 检查是否添加了行
            while (lineCount > listBoxLineNumbers.Items.Count)
            {
                listBoxLineNumbers.Items.Add((listBoxLineNumbers.Items.Count + 1).ToString());
            }

            // 检查是否删除了行
            while (lineCount < listBoxLineNumbers.Items.Count)
            {
                listBoxLineNumbers.Items.RemoveAt(listBoxLineNumbers.Items.Count - 1);
            }

            // 恢复选中的行号
            if (selectedIndex < listBoxLineNumbers.Items.Count)
            {
                listBoxLineNumbers.SelectedIndex = selectedIndex;
            }
        }

        #endregion

        #region 复选框行号
        private void checkBoxLines_CheckedChanged(object sender, EventArgs e)
        {
            // 检查复选框是否被选中
            if (checkBoxLines.Checked)
            {
                // 更新行号
                UpdateLineNumbers();
            }
            else
            {
                // 清除行号
                listBoxLineNumbers.Items.Clear();
            }
        }
        #endregion

        #region 状态栏     

        private void richTextBoxNotPad_SelectionChanged(object sender, EventArgs e)
        {
            // 计算当前行和列
            int index = richTextBoxNotPad.SelectionStart;
            int line = richTextBoxNotPad.GetLineFromCharIndex(index);
            int firstChar = richTextBoxNotPad.GetFirstCharIndexFromLine(line);
            int column = index - firstChar;

            // 统计文本中的行数
            int lineCount = richTextBoxNotPad.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;

            // 在状态栏中显示行和列
            toolStripStatusLabel1.Text = $"行: {lineCount}, 列: {column + 1}";

            // 将 listBoxLineNumbers 与 richTextBoxNotPad 同步
            if (listBoxLineNumbers.Items.Count > line)
            {
                listBoxLineNumbers.SelectedIndex = line;
            }
        }

        private bool IsUtf8(string text)
        {
            // 检查文本是否为 UTF-8
            // 这是一个简单的测试，可能不是 100% 准确
            var utf8 = Encoding.UTF8;
            byte[] encoded = utf8.GetBytes(text);
            string decoded = utf8.GetString(encoded);
            return text == decoded;
        }

        private void richTextBoxNotPad_TextChanged_1(object sender, EventArgs e)
        {
            // 检查文本格式
            bool isUtf8 = IsUtf8(richTextBoxNotPad.Text);

            // 在状态栏中显示文本格式
            toolStripStatusLabel1.Text += $", 格式: {(isUtf8 ? "UTF-8" : "非 UTF-8")}";
        }
        #endregion

        #region 加载
        private void btLoad_Click(object sender, EventArgs e)
        {
            // 创建一个新的 OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置用户可以打开的文件类型筛选器
            openFileDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";

            // 显示对话框并确认用户点击了确定
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 读取所选文件的内容
                string fileContent = File.ReadAllText(openFileDialog.FileName);

                // 在 RichTextBox 中显示文件内容
                richTextBoxNotPad.Text = fileContent;
            }
        }
        #endregion

        #region ListBoxLineNumbers
        private void listBoxLineNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 检查是否选中了一行
            if (listBoxLineNumbers.SelectedIndex != -1)
            {
                // 计算所选行的第一个字符的索引
                int line = listBoxLineNumbers.SelectedIndex;
                int index = richTextBoxNotPad.GetFirstCharIndexFromLine(line);

                // 将光标移动到所选行的开头
                richTextBoxNotPad.SelectionStart = index;
                richTextBoxNotPad.SelectionLength = 0;

                // 将焦点设置到 RichTextBox，以便用户可以立即输入
                richTextBoxNotPad.Focus();
            }
        }
        #endregion

        #region 日期时间选择器
        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            // 将所选日期格式化为字符串
            string dateString = dateTimePicker1.Value.ToString("dd.MM.yyyy");

            // 在 RichTextBox 的当前光标位置插入日期
            richTextBoxNotPad.SelectedText = dateString;
        }
        #endregion

        #region 替换搜索        
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查窗体是否已经打开
            if (replaceForm != null)
            {
                // 窗体已打开，不执行任何操作
                return;
            }
            // 创建一个新窗体
            replaceForm = new System.Windows.Forms.Form();
            replaceForm.TopMost = true;
            replaceForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            replaceForm.MaximizeBox = false;


            // 创建控件
            System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label() { Text = "查找内容:", Location = new System.Drawing.Point(10, 10) };
            System.Windows.Forms.TextBox txtSearch = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(10, 30) };

            System.Windows.Forms.Label lblReplace = new System.Windows.Forms.Label() { Text = "替换为:", Location = new System.Drawing.Point(10, 60) };
            System.Windows.Forms.TextBox txtReplace = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(10, 80) };

            System.Windows.Forms.Button btnSearchNext = new System.Windows.Forms.Button() { Text = "查找下一个", Location = new System.Drawing.Point(10, 110) };

            System.Windows.Forms.Button btnReplaceOne = new System.Windows.Forms.Button() { Text = "替换", Location = new System.Drawing.Point(100, 110) };

            System.Windows.Forms.Button btnReplaceAll = new System.Windows.Forms.Button() { Text = "全部替换", Location = new System.Drawing.Point(190, 110) };

            System.Windows.Forms.Button btnCancel = new System.Windows.Forms.Button() { Text = "取消", Location = new System.Drawing.Point(190, 140) };

            System.Windows.Forms.CheckBox chkMatchCase = new System.Windows.Forms.CheckBox() { Text = "大小写匹配", Location = new System.Drawing.Point(10, 140) };

            System.Windows.Forms.CheckBox chkWrapAround = new System.Windows.Forms.CheckBox() { Text = "循环查找", Location = new System.Drawing.Point(10, 160) };

            // 创建文本框
            System.Windows.Forms.TextBox txtCount = new System.Windows.Forms.TextBox()
            {
                Location = new System.Drawing.Point(120, 80),
                ReadOnly = true,
                MaxLength = 3,
                Width = 20 // 20 像素
            };

            // 将控件添加到窗体
            replaceForm.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblSearch,
                txtSearch,
                lblReplace,
                txtReplace,
                btnSearchNext,
                btnReplaceOne,
                btnReplaceAll,
                btnCancel,
                chkMatchCase,
                chkWrapAround,
                txtCount
            });

            // 为按钮添加事件处理程序
            btnSearchNext.Click += (s, ev) =>
            {
                if (richTextBoxNotPad.InvokeRequired)
                {
                    richTextBoxNotPad.Invoke(new Action(() =>
                    {
                        string searchText = txtSearch.Text;
                        int startIndex = richTextBoxNotPad.SelectionStart + richTextBoxNotPad.SelectionLength;
                        int index = richTextBoxNotPad.Text.IndexOf(searchText, startIndex, chkMatchCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                        if (index != -1)
                        {
                            richTextBoxNotPad.Select(index, searchText.Length);
                        }
                        else
                        {
                            // 如果未找到搜索文本，将起始索引设为 0 并重新搜索
                            index = richTextBoxNotPad.Text.IndexOf(searchText, 0, chkMatchCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                            if (index != -1)
                            {
                                richTextBoxNotPad.Select(index, searchText.Length);
                            }
                        }
                    }));
                }
                else
                {
                    string searchText = txtSearch.Text;
                    int startIndex = richTextBoxNotPad.SelectionStart + richTextBoxNotPad.SelectionLength;
                    int index = richTextBoxNotPad.Text.IndexOf(searchText, startIndex, chkMatchCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                    if (index != -1)
                    {
                        richTextBoxNotPad.Select(index, searchText.Length);
                    }
                    else
                    {
                        // 如果未找到搜索文本，将起始索引设为 0 并重新搜索
                        index = richTextBoxNotPad.Text.IndexOf(searchText, 0, chkMatchCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                        if (index != -1)
                        {
                            richTextBoxNotPad.Select(index, searchText.Length);
                        }
                    }
                }
                // 每次搜索文本或 richTextBoxNotPad 中的文本更改时更新文本框
                txtSearch.TextChanged += (s, ev) => UpdateCount();
                richTextBoxNotPad.TextChanged += (s, ev) => UpdateCount();
            };

            // 更新文本框的方法
            void UpdateCount()
            {
                if (richTextBoxNotPad.InvokeRequired)
                {
                    richTextBoxNotPad.Invoke(new Action(() => txtCount.Text = CountOccurrences(richTextBoxNotPad.Text, txtSearch.Text).ToString()));
                }
                else
                {
                    txtCount.Text = CountOccurrences(richTextBoxNotPad.Text, txtSearch.Text).ToString();
                }
            }

            // 统计文本中单词出现次数的方法
            int CountOccurrences(string text, string word)
            {
                int count = 0;
                int startIndex = 0;
                while ((startIndex = text.IndexOf(word, startIndex)) != -1)
                {
                    count++;
                    startIndex += word.Length;
                }
                return count;
            }

            btnReplaceOne.Click += (s, ev) =>
            {
                if (richTextBoxNotPad.InvokeRequired)
                {
                    richTextBoxNotPad.Invoke(new Action(() =>
                    {
                        if (richTextBoxNotPad.SelectionLength > 0)
                        {
                            richTextBoxNotPad.SelectedText = txtReplace.Text;
                        }
                    }));
                }
                else
                {
                    if (richTextBoxNotPad.SelectionLength > 0)
                    {
                        richTextBoxNotPad.SelectedText = txtReplace.Text;
                    }
                }
            };

            btnReplaceAll.Click += (s, ev) =>
            {
                string searchText = txtSearch.Text;
                string replaceText = txtReplace.Text;
                if (richTextBoxNotPad.InvokeRequired)
                {
                    richTextBoxNotPad.Invoke(new Action(() => richTextBoxNotPad.Text = richTextBoxNotPad.Text.Replace(searchText, replaceText)));
                }
                else
                {
                    richTextBoxNotPad.Text = richTextBoxNotPad.Text.Replace(searchText, replaceText);
                }
            };

            btnCancel.Click += (s, ev) =>
            {
                replaceForm.Close();
            };

            // 添加事件处理程序，在窗体关闭时将全局变量设为 null
            replaceForm.FormClosed += (s, ev) => { replaceForm = null; };
           
            replaceForm.Show();
        }

        #endregion

        #region 全选
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 选择 RichTextBox 中的所有文本
            richTextBoxNotPad.SelectAll();
        }
        #endregion

        #region 左对齐
        private void btnAlignLeft_Click(object sender, EventArgs e)
        {
            richTextBoxNotPad.SelectionAlignment = HorizontalAlignment.Left;
        }
        #endregion
        #region 居中对齐
        private void btnAlignCenter_Click(object sender, EventArgs e)
        {
            richTextBoxNotPad.SelectionAlignment = HorizontalAlignment.Center;
        }
        #endregion
        #region 右对齐      
        private void btnAlignRight_Click(object sender, EventArgs e)
        {
            richTextBoxNotPad.SelectionAlignment = HorizontalAlignment.Right;
        }
        #endregion
        #region 项目符号列表
        private void btnBullet_Click(object sender, EventArgs e)
        {
            isBulletActive = !isBulletActive;
            btnBullet.BackColor = isBulletActive ? Color.LightBlue : SystemColors.Control;

            if (isBulletActive)
            {
                // 在所选文本的每一行前插入项目符号
                string[] lines = richTextBoxNotPad.SelectedText.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = "\u2022 " + lines[i];
                }
                richTextBoxNotPad.SelectedText = string.Join("\n", lines);
            }
        }
        #endregion

        #region 按键事件
        private void richTextBoxNotPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isBulletActive && e.KeyChar == (char)Keys.Return)
            {
                // 在新行开头插入项目符号
                richTextBoxNotPad.AppendText("\u2022 ");

                // 将光标置于项目符号后
                richTextBoxNotPad.SelectionStart = richTextBoxNotPad.Text.Length;

                // 阻止 Enter 键的默认行为
                e.Handled = true;
            }
            else if (isLineNumberActive && e.KeyChar == (char)Keys.Return)
            {
                // 在新行开头插入行号
                richTextBoxNotPad.AppendText($"{lineNumber++} ");

                // 将光标置于行号后
                richTextBoxNotPad.SelectionStart = richTextBoxNotPad.Text.Length;

                // 阻止 Enter 键的默认行为
                e.Handled = true;
            }

        }
        #endregion

        #region 行号
        int lineNumber = 1;
        private void btnLineNumber_Click(object sender, EventArgs e)
        {
            btnLineNumber.Click += (s, ev) =>
            {
                // 切换行号状态
                isLineNumberActive = !isLineNumberActive;

                // 根据状态更改按钮颜色
                btnLineNumber.BackColor = isLineNumberActive ? Color.LightBlue : SystemColors.Control;

                if (isLineNumberActive)
                {
                    // 将所选文本按行分割
                    string[] lines = richTextBoxNotPad.SelectedText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                    // 在每一行前添加行号
                    for (int i = 0; i < lines.Length; i++)
                    {
                        lines[i] = (lineNumber++).ToString() + " " + lines[i];
                    }

                    // 用带编号的文本替换高亮文本
                    richTextBoxNotPad.SelectedText = string.Join("\r\n", lines);
                }
                else
                {
                    // 如果行号被禁用，重置行号
                    lineNumber = 1;
                }
            };
        }
        #endregion

        #region 增加缩进和减少缩进

        private void btnDecreaseIndent_Click(object sender, EventArgs e)
        {
            // 增加当前行或所选文本的缩进
            richTextBoxNotPad.SelectionIndent += 20;
        }

        private void btnIncreaseIndent_Click(object sender, EventArgs e)
        {
            // 减少当前行或所选文本的缩进
            richTextBoxNotPad.SelectionIndent = Math.Max(0, richTextBoxNotPad.SelectionIndent - 20);
        }
        #endregion

        #region 背景颜色
        private void colorButton_Click(object sender, EventArgs e)
        {
            // 创建一个新的 ColorDialog
            ColorDialog colorDialog = new ColorDialog();

            // 显示对话框并确认用户点击了确定
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // 将所选颜色设置为所选文本的背景色
                richTextBoxNotPad.SelectionBackColor = colorDialog.Color;
            }
        }
        #endregion

        #region 字体颜色按钮
        private void btFontColor_Click(object sender, EventArgs e)
        {
            // 创建一个新的 ColorDialog
            ColorDialog colorDialog = new ColorDialog();

            // 显示对话框并确认用户点击了确定
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // 将所选颜色设置为所选文本的颜色
                richTextBoxNotPad.SelectionColor = colorDialog.Color;
            }
        }
        #endregion

        #region 保存按钮
        private void btSaveAs_Click(object sender, EventArgs e)
        {
            // 创建一个新的 SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // 设置用户可以保存的文件类型筛选器
            saveFileDialog.Filter = "文本文件 (*.txt)|*.txt|富文本格式 (*.rtf)|*.rtf|C# 文件 (*.cs)|*.cs|XML 文件 (*.xml)|*.xml|SCP 文件 (*.scp)|*.scp|INI 文件 (*.ini)|*.ini|所有文件 (*.*)|*.*";

            // 显示对话框并确认用户点击了确定
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 将 RichTextBox 的内容保存到所选文件
                if (saveFileDialog.FilterIndex == 2) // RTF 格式
                {
                    richTextBoxNotPad.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);
                }
                else // 所有其他格式
                {
                    richTextBoxNotPad.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                }
            }
        }
        #endregion

        #region 加载菜单项
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建一个新的 OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置用户可以打开的文件类型筛选器
            openFileDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";

            // 显示对话框并确认用户点击了确定
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 读取所选文件的内容
                string fileContent = File.ReadAllText(openFileDialog.FileName);

                // 在 RichTextBox 中显示文件内容
                richTextBoxNotPad.Text = fileContent;
            }
        }
        #endregion

        #region 保存菜单项
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建一个新的 SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // 设置用户可以保存的文件类型筛选器
            saveFileDialog.Filter = "文本文件 (*.txt)|*.txt|富文本格式 (*.rtf)|*.rtf|C# 文件 (*.cs)|*.cs|XML 文件 (*.xml)|*.xml|SCP 文件 (*.scp)|*.scp|INI 文件 (*.ini)|*.ini|所有文件 (*.*)|*.*";

            // 显示对话框并确认用户点击了确定
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 将 RichTextBox 的内容保存到所选文件
                if (saveFileDialog.FilterIndex == 2) // RTF 格式
                {
                    richTextBoxNotPad.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);
                }
                else // 所有其他格式
                {
                    richTextBoxNotPad.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                }
            }
        }
        #endregion

        #region 打印
        private void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建一个新的 PrintDocument 实例
            PrintDocument printDocument = new PrintDocument();

            // 为 PrintPage 事件添加事件处理程序
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            // 创建一个新的 PrintDialog
            System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();

            // 为对话框设置 PrintDocument
            printDialog.Document = printDocument;

            // 显示对话框并确认用户点击了确定
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // 打印文档
                printDialog.Document.Print();
            }
        }

        // PrintPage 事件的事件处理程序方法
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // 将 RichTextBox 的文本绘制到页面上
            e.Graphics.DrawString(richTextBoxNotPad.Text, richTextBoxNotPad.Font, Brushes.Black, 10, 10);
        }
        #endregion

        #region 复制到剪贴板
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 将 RichTextBox 中的所有文本复制到剪贴板
            richTextBoxNotPad.SelectAll();
            richTextBoxNotPad.Copy();
        }
        #endregion

        #region 鼠标点击
        private void listBoxLineNumbers_MouseClick(object sender, MouseEventArgs e)
        {
            // 检查是否选中了行号
            if (listBoxLineNumbers.SelectedIndex != -1)
            {
                // 获取选中的行号
                int lineNumber = listBoxLineNumbers.SelectedIndex;

                // 跳转到 richTextBoxNotPad 中的对应行
                richTextBoxNotPad.SelectionStart = richTextBoxNotPad.GetFirstCharIndexFromLine(lineNumber);
                richTextBoxNotPad.ScrollToCaret();

                // 将焦点设置到 richTextBoxNotPad
                richTextBoxNotPad.Focus();
            }
        }
        #endregion
    }
}