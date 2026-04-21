// /***************************************************************************
//  *
//  * $Author: Nikodemus
//  * Advanced Nikodemus
//  * 
//  * \"啤酒-葡萄酒许可证\"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯啤酒和葡萄酒作为回报。
//  *
//  ***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UoFiddler.Forms
{
    public partial class LineConverterForm : Form
    {
        private string _originalText;

        public LineConverterForm()
        {
            InitializeComponent();

            // 文本变化事件处理程序
            TextBoxInputOutput.TextChanged += TextBoxInputOutput_TextChanged;
        }

        #region BtnConvert
        private void BtnConvert_Click(object sender, EventArgs e)
        {
            _originalText = TextBoxInputOutput.Text;
            if (chkAddSpaces.Checked)
            {
                TextBoxInputOutput.Text = ConvertParagraphsToLinesWithSpaces(TextBoxInputOutput.Text);
            }
            else
            {
                TextBoxInputOutput.Text = ConvertParagraphsToLines(TextBoxInputOutput.Text);
            }
            lbCounter.Text = TextBoxInputOutput.Text.Length.ToString();
        }
        #endregion

        #region BtnConvert2
        private void BtnConvert2_Click(object sender, EventArgs e)
        {
            _originalText = TextBoxInputOutput.Text;
            TextBoxInputOutput.Text = ConvertParagraphsToLines2(TextBoxInputOutput.Text);
            lbCounter.Text = TextBoxInputOutput.Text.Length.ToString();
        }
        #endregion

        private void BtnConvertParagraphsToLines2WithoutComments_Click(object sender, EventArgs e)
        {
            _originalText = TextBoxInputOutput.Text;
            TextBoxInputOutput.Text = ConvertParagraphsToLines2WithoutComments(TextBoxInputOutput.Text);
            lbCounter.Text = TextBoxInputOutput.Text.Length.ToString();
        }

        #region BtnNewConversion
        private void BtnNewConversion_Click(object sender, EventArgs e)
        {
            _originalText = TextBoxInputOutput.Text;
            TextBoxInputOutput.Text = NewConversionMethod(TextBoxInputOutput.Text);
            lbCounter.Text = TextBoxInputOutput.Text.Length.ToString();
        }
        #endregion

        #region ConvertParagraphsToLines
        private static string ConvertParagraphsToLines(string input)
        {
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var result = new StringBuilder();
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("//"))
                {
                    result.Append(trimmedLine + " ");
                }
                else if (trimmedLine.EndsWith(";") || trimmedLine.EndsWith("{") || trimmedLine.EndsWith("}"))
                {
                    result.Append(trimmedLine + " ");
                }
                else
                {
                    result.Append(trimmedLine);
                }
            }
            return result.ToString();
        }
        #endregion

        #region ConvertParagraphsToLines2
        private static string ConvertParagraphsToLines2(string input)
        {
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var result = new StringBuilder();
            foreach (var line in lines)
            {
                var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    result.Append(word);
                    if (word.EndsWith(";") || word.EndsWith("{") || word.EndsWith("}"))
                    {
                        result.Append(" ");
                    }
                }
            }
            return result.ToString();
        }
        #endregion

        #region ConvertParagraphsToLinesWithSpaces
        private static string ConvertParagraphsToLinesWithSpaces(string input)
        {
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var result = new StringBuilder();
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (trimmedLine.EndsWith(";") || trimmedLine.EndsWith("{") || trimmedLine.EndsWith("}"))
                {
                    result.Append(trimmedLine + " ");
                }
                else
                {
                    result.Append(trimmedLine);
                }
            }
            return result.ToString();
        }
        #endregion

        #region ConvertParagraphsToLines2WithoutComments
        private static string ConvertParagraphsToLines2WithoutComments(string input)
        {
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var result = new StringBuilder();
            bool isCommentBlock = false;

            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("///***************************************************************************") || line.Trim().StartsWith("// /***************************************************************************"))
                {
                    isCommentBlock = true;
                }

                if (!isCommentBlock)
                {
                    var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        result.Append(word);
                        if (word.EndsWith(";") || word.EndsWith("{") || word.EndsWith("}"))
                        {
                            result.Append(" ");
                        }
                    }
                }

                if (line.Trim().EndsWith("***************************************************************************/"))
                {
                    isCommentBlock = false;
                }
            }

            return result.ToString();
        }
        #endregion

        #region BtnConvertWithBlocks
        private void BtnConvertWithBlocks_Click(object sender, EventArgs e)
        {
            _originalText = TextBoxInputOutput.Text;
            string convertedText = ConvertParagraphsToLines2WithoutComments(TextBoxInputOutput.Text);

            // 验证 TBBlockCount 中的输入是否为有效数字且大于 500
            if (!int.TryParse(TBBlockCount.Text, out int blockSize) || blockSize < 500)
            {
                // 如果 TBBlockCount 不包含有效的块大小，则使用复选框
                blockSize = chkBlockSize4000.Checked ? 4000 : 8000;
            }

            StringBuilder sb = new StringBuilder();
            int blockCount = 0;
            for (int i = 0; i < convertedText.Length; i += blockSize)
            {
                int length = Math.Min(blockSize, convertedText.Length - i);
                sb.AppendLine(convertedText.Substring(i, length));
                sb.AppendLine();
                blockCount++;
            }

            TextBoxInputOutput.Text = sb.ToString();
            lbCounter.Text = TextBoxInputOutput.Text.Length.ToString();
            lblBlockCount.Text = "块计数: " + blockCount.ToString();
        }
        #endregion

        #region NewConversionMethod
        private static string NewConversionMethod(string input)
        {
            // 将多个空格替换为单个空格
            string text = Regex.Replace(input, @"\s+", " ");

            // 在运算符周围和逗号后添加空格
            text = Regex.Replace(text, @"([^\s])([=+\-*/%<>!&|])([^\s])", "$1 $2 $3");
            text = text.Replace(",", ", ");

            // 在每个分号、左大括号和右大括号后插入换行符
            text = text.Replace(";", ";").Replace("{", "{").Replace("}", "}");

            // 在每个双斜杠（注释）前插入换行符
            text = text.Replace("//", "//");

            // 删除所有换行符
            text = text.Replace("\n", "");

            return text;
        }
        #endregion

        #region BtnClear
        private void BtnClear_Click(object sender, EventArgs e)
        {
            // 清空文本框内容
            TextBoxInputOutput.Clear();
            lbCounter.Text = "0";
        }
        #endregion

        #region BtnCopy
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            // 将文本框内容复制到剪贴板
            Clipboard.SetText(TextBoxInputOutput.Text);
        }
        #endregion

        #region BtnRestore
        private void BtnRestore_Click(object sender, EventArgs e)
        {
            // 恢复原始文本
            TextBoxInputOutput.Text = _originalText;
            lbCounter.Text = _originalText.Length.ToString();
        }
        #endregion

        #region ToolStripTextBoxSearch
        private void ToolStripTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            // 从 ToolStripTextBoxSearch 获取搜索文本
            string searchText = ToolStripTextBoxSearch.Text;

            // 如果搜索文本不为空，则搜索并定位
            if (!string.IsNullOrEmpty(searchText))
            {
                int startIndex = TextBoxInputOutput.Text.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase);
                if (startIndex != -1)
                {
                    TextBoxInputOutput.Select(startIndex, searchText.Length);
                    TextBoxInputOutput.ScrollToCaret();
                }
            }
        }
        #endregion

        #region BtnBullBlockSize
        private void BtnBullBlockSize_Click(object sender, EventArgs e)
        {
            int blockSize;
            if (int.TryParse(TBBlockCount.Text, out int inputBlockSize) && inputBlockSize >= 500)
            {
                blockSize = inputBlockSize; // 如果 TBBlockCount 中的值有效且大于等于 500，则将其设置为块大小
            }
            else
            {
                blockSize = chkBlockSize4000.Checked ? 4000 : 8000; // 如果复选框激活，则块大小设置为 4000，否则为 8000
            }

            if (TextBoxInputOutput.Text.Length > 0)
            {
                int currentBlockSize = Math.Min(blockSize, TextBoxInputOutput.Text.Length); // 取 'blockSize' 或文本剩余长度中的较小值

                string textToTransfer = TextBoxInputOutput.Text.Substring(0, currentBlockSize); // 从文本框中取出前 'currentBlockSize' 个字符

                Clipboard.SetText(textToTransfer); // 将文本复制到剪贴板

                TextBoxInputOutput.Text = TextBoxInputOutput.Text.Remove(0, currentBlockSize); // 从文本框中删除前 'currentBlockSize' 个字符

                System.Threading.Thread.Sleep(5000); // 等待 5 秒
            }
        }
        #endregion

        #region TextBoxInputOutput_TextChanged
        private void TextBoxInputOutput_TextChanged(object sender, EventArgs e)
        {
            // 在标签中更新字符数
            LbCharactercounter.Text = TextBoxInputOutput.Text.Length.ToString();
        }
        #endregion

        #region loadToolStripMenuItem
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt 文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取选中的文件路径
                    string filePath = openFileDialog.FileName;

                    // 将文件中的文本加载到文本框
                    TextBoxInputOutput.Text = System.IO.File.ReadAllText(filePath);
                }
            }
        }
        #endregion

        #region saveToolStripMenuItem
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "文本文件 (*.txt)|*.txt|C# 文件 (*.cs)|*.cs|XML 文件 (*.xml)|*.xml|所有文件 (*.*)|*.*";
                saveFileDialog.FilterIndex = 4;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取选中的文件路径
                    string filePath = saveFileDialog.FileName;

                    // 将文本框中的文本保存到文件
                    System.IO.File.WriteAllText(filePath, TextBoxInputOutput.Text);
                }
            }
        }
        #endregion

        #region textReplacementToolStripMenuItem        
        private int _currentStartIndex = 0; // 全局变量，用于存储下一次搜索的起始索引

        private void TextReplacementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查 TextBoxInputOutput 中的文本是否为空
            if (string.IsNullOrEmpty(TextBoxInputOutput.Text))
            {
                MessageBox.Show("请输入文本。");
                return;
            }

            // 创建一个新窗体
            Form form = new Form();
            form.Size = new Size(200, 230); // 设置宽度为 150，高度为 230
            form.Text = "搜索和替换"; // 设置窗体标题
            form.FormBorderStyle = FormBorderStyle.FixedSingle; // 将 FormBorderStyle 设置为 FixedSingle
            form.ShowIcon = false; // 隐藏图标
            form.MaximizeBox = false; // 禁用最大化
            form.TopMost = true; // 将窗体置于最前

            // 创建文本框、按钮和标签
            TextBox txtSearchText = new TextBox() { Top = 10, Left = 10 };
            TextBox txtReplaceWith = new TextBox() { Top = 40, Left = 10 };
            Button btnReplace = new Button() { Text = "替换", Top = 70, Left = 10 };
            Button btnReplaceAll = new Button() { Text = "全部替换", Top = 100, Left = 10 };
            Button btnCancel = new Button() { Text = "取消", Top = 130, Left = 10 };
            Label lblCount = new Label() { Top = 160, Left = 10 }; // 添加一个标签

            // 将控件添加到窗体
            form.Controls.Add(txtSearchText);
            form.Controls.Add(txtReplaceWith);
            form.Controls.Add(btnReplace);
            form.Controls.Add(btnReplaceAll);
            form.Controls.Add(btnCancel);
            form.Controls.Add(lblCount); // 将标签添加到窗体

            // 添加事件处理程序
            btnReplace.Click += (s, ea) =>
            {
                string searchText = txtSearchText.Text;
                string replaceWithText = txtReplaceWith.Text;

                if (_currentStartIndex < TextBoxInputOutput.Text.Length && _currentStartIndex != -1)
                {
                    _currentStartIndex = TextBoxInputOutput.Text.IndexOf(searchText, _currentStartIndex, StringComparison.CurrentCultureIgnoreCase);

                    if (_currentStartIndex != -1)
                    {
                        TextBoxInputOutput.Select(_currentStartIndex, searchText.Length);
                        TextBoxInputOutput.ScrollToCaret();

                        // 替换文本并转到下一个匹配项
                        TextBoxInputOutput.SelectedText = replaceWithText;
                        _currentStartIndex += replaceWithText.Length;
                    }
                }

                lblCount.Text = "替换次数: " + TextBoxInputOutput.Text.Split(new[] { replaceWithText }, StringSplitOptions.None).Length;
            };

            btnReplaceAll.Click += (s, ea) =>
            {
                string searchText = txtSearchText.Text;
                string replaceWithText = txtReplaceWith.Text;

                // 替换所有出现的文本
                TextBoxInputOutput.Text = TextBoxInputOutput.Text.Replace(searchText, replaceWithText);

                // 更新标签
                int count = TextBoxInputOutput.Text.Split(new[] { replaceWithText }, StringSplitOptions.None).Length - 1;
                lblCount.Text = "替换次数: " + count;
            };

            btnCancel.Click += (s, ea) =>
            {
                // 关闭对话框
                form.Close();
            };

            // 显示窗体
            form.Show(); // 将窗体显示为非模态对话框
        }
        #endregion        
    }
}