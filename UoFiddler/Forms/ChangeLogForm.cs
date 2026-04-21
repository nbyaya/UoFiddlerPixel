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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UoFiddler.Forms
{
    public partial class ChangeLogForm : Form
    {
        #region [ ChangeLogForm ]
        public ChangeLogForm()
        {
            InitializeComponent();

            // 为 ToolStripTextBox 的 KeyDown 事件添加事件处理程序
            toolStripTextBox1.KeyDown += (s, args) =>
            {
                if (args.KeyCode == Keys.Enter)
                {
                    args.SuppressKeyPress = true; // 防止按下回车时发出“叮”的声音

                    string searchText = toolStripTextBox1.Text;
                    if (string.IsNullOrWhiteSpace(searchText)) return; // 防止空搜索

                    // 移除之前的搜索高亮
                    ResetHighlighting();

                    string richText = richTextBox1.Text;
                    int index = 0;

                    // 在文本中搜索所有匹配项（不区分大小写）
                    while ((index = richText.IndexOf(searchText, index, StringComparison.OrdinalIgnoreCase)) != -1)
                    {
                        // 高亮当前找到的匹配项
                        richTextBox1.Select(index, searchText.Length);
                        richTextBox1.SelectionBackColor = Color.Yellow;

                        // 将索引向前移动以继续搜索
                        index += searchText.Length;
                    }

                    // 如果搜索到匹配项，则将焦点设置到 RichTextBox
                    if (richText.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        richTextBox1.Focus();
                    }
                }
            };
        }
        #endregion

        #region [ 重置高亮 ]
        private void ResetHighlighting()
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = richTextBox1.BackColor; // 重置背景色
        }
        #endregion

        #region [ ChangeLogForm_Load ]
        private void ChangeLogForm_Load(object sender, EventArgs e)
        {
            // 读取 "Changelog.txt" 文件的内容到字符串
            string changelogText = File.ReadAllText("Changelog.txt", Encoding.UTF8);
            changelogText = LocalizeChangelogText(changelogText);

            // 将文本拆分为行数组
            string[] lines = changelogText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            // 遍历每一行
            for (int i = 0; i < lines.Length; i++)
            {
                // 检查行是否以 "-" 开头
                if (lines[i].StartsWith("-"))
                {
                    // 将 "-" 替换为 "•"
                    lines[i] = "•" + lines[i].Substring(1);
                }
            }

            // 将行重新合并为单个字符串
            changelogText = string.Join(Environment.NewLine, lines);

            // 将一些特定字符串替换为其他字符串
            changelogText = changelogText.Replace("#XG1", "Nikodemus");
            changelogText = changelogText.Replace("#XG2", "AsYlum");
            changelogText = changelogText.Replace("#XG3", "Alathair");

            // 将文本转换为 RTF 格式
            string changelogRtf = ConvertToRtf(changelogText);
            // 将 richTextBox1 的 RTF 属性设置为 RTF 文本
            richTextBox1.Rtf = changelogRtf;
        }
        #endregion

        #region [ LocalizeChangelogText ]
        private static string LocalizeChangelogText(string text)
        {
            string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = TranslateChangelogLine(lines[i]);
            }

            return string.Join(Environment.NewLine, lines);
        }
        #endregion

        #region [ TranslateChangelogLine ]
        private static string TranslateChangelogLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return line;
            }

            string trimmed = line.TrimStart();
            string indent = line.Substring(0, line.Length - trimmed.Length);

            if (trimmed.StartsWith("Version", StringComparison.OrdinalIgnoreCase))
            {
                return indent + Regex.Replace(trimmed, @"^Version\s*:?", "版本：", RegexOptions.IgnoreCase);
            }

            string prefix = string.Empty;
            string content = trimmed;

            if (trimmed.StartsWith("- "))
            {
                prefix = "- ";
                content = trimmed.Substring(2);
            }
            else if (trimmed.StartsWith("– "))
            {
                prefix = "– ";
                content = trimmed.Substring(2);
            }
            else if (trimmed.StartsWith("•"))
            {
                prefix = "•";
                content = trimmed.Substring(1);
            }

            var exactTranslations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Compare Maps revised"] = "地图对比功能已改进",
                ["Made `CalculateDiffs()` asynchronous (`Task.Run`) to prevent UI freezing"] = "将 `CalculateDiffs()` 改为异步执行（`Task.Run`），避免界面卡死",
                ["Fixed null‑check bug in `CalculateDiffs`"] = "修复 `CalculateDiffs` 中的空值检查问题",
                ["Added CSV export for diff list"] = "新增差异列表 CSV 导出功能",
                ["Added screenshot feature to save current viewport as PNG"] = "新增截图功能，可将当前视口保存为 PNG",
                ["Added Next‑Diff / Previous‑Diff navigation"] = "新增“上一处差异 / 下一处差异”导航",
                ["Added diff statistics to the top toolbar"] = "在顶部工具栏新增差异统计信息",
                ["Added manual zoom via text input"] = "新增通过文本输入进行手动缩放",
                ["Introduced modern dark UI"] = "引入现代化深色界面",
                ["Added “Diff List…” dialog"] = "新增“差异列表...”对话框",
                ["ParticleGray Tool."] = "新增 ParticleGray 工具。",
                ["Added a search function in Amindata based on hex addresses and names."] = "AminData 新增基于十六进制地址和名称的搜索功能。",
                ["Added a Artwork Gallery Tool in Artworks Items."] = "在 Artworks Items 中新增 Artwork Gallery 工具。",
                ["Fixed importing animation frames from gif files."] = "修复从 GIF 文件导入动画帧的问题。",
                ["Added PNG format when inserting or replacing graphics."] = "插入或替换图像时新增 PNG 格式支持。",
                ["Bugs fixed and revised."] = "修复并改进了若干问题。",
                ["Add Animation XML Editor."] = "新增动画 XML 编辑器。",
                ["Changelog revised search revised."] = "更新日志与搜索功能已改进。",
                ["Added backup directory copy feature (Path Settings)."] = "在路径设置中新增备份目录复制功能。",
                ["Added BtnLogDir Open Log Dir (Options)."] = "在选项中新增 BtnLogDir，可打开日志目录。",
                ["Added map cordinate to clipboard (map)"] = "新增将地图坐标复制到剪贴板的功能（地图）。",
                ["Added Gump Mirror function."] = "新增 Gump 镜像功能。",
                ["Added AnimationEdit Lists all IDs in a text file."] = "AnimationEdit 新增将所有 ID 输出到文本文件的功能。",
                ["Items: Added Mass Removal and Select with Ctrl."] = "Items 新增批量删除，以及按 Ctrl 进行选择的功能。"
            };

            if (exactTranslations.TryGetValue(content, out string translated))
            {
                return indent + prefix + translated;
            }

            string localized = content
                .Replace("Added ", "新增")
                .Replace("Add ", "新增")
                .Replace("Fixed ", "修复")
                .Replace("Update ", "更新")
                .Replace("Updated ", "已更新")
                .Replace("Removed ", "移除")
                .Replace("Implemented ", "实现")
                .Replace("revised", "改进")
                .Replace("search function", "搜索功能")
                .Replace("Search function", "搜索功能")
                .Replace("function", "功能")
                .Replace("Tool.", "工具。")
                .Replace("directory", "目录")
                .Replace("clipboard", "剪贴板")
                .Replace("background color", "背景颜色")
                .Replace("dark UI", "深色界面")
                .Replace("manual zoom", "手动缩放")
                .Replace("screenshot", "截图")
                .Replace("diff list", "差异列表")
                .Replace("tooltip", "工具提示")
                .Replace("bitmap", "位图")
                .Replace("memory leaks", "内存泄漏");

            return indent + prefix + localized;
        }
        #endregion

        /***********************************************************************
        * ConvertToRtf 方法接收一个纯文本字符串作为输入，并返回一个 RTF 格式的字符串。*
        * 该方法使用 StringBuilder 来构建 RTF 文本。它首先将 RTF 头部和颜色表信息   *
        * 追加到 StringBuilder。颜色表定义了 #XC1 到 #XC7 标签对应的颜色。           *
        *                                                                      *
        * #XC1 到 #XC7 的颜色缩写分别为：                                         *
        * #XC1 = 橙色                                                            *
        * #XC2 = 红色                                                            *
        * #XC3 = 蓝色                                                            *
        * #XC4 = 黄色                                                            *
        * #XC5 = 绿色                                                            *
        * #XC6 = 紫色                                                            *
        * #XC7 = 粉红色                                                          *
        *                                                                      *
        * 然后该方法将输入文本拆分为行数组，并循环处理每一行。如果某行以 "Version :" 开头，*
        * 则追加 RTF 代码使文本变为粗体，然后追加该行，再追加结束粗体的 RTF 代码。      *
        * 否则，将该行拆分为单词数组，并循环处理每个单词。如果某个单词以 "#XC" 开头，    *
        * 则提取 "#XC" 后面的数字，并检查其是否在 1 到 7 之间。如果是，则追加设置文本颜色  *
        * 为颜色表中对应颜色的 RTF 代码，追加下一个单词，然后追加将文本颜色重置为默认颜色的 *
        * RTF 代码。同时还追加使彩色文本变为粗体的 RTF 代码。                         *
        *                                                                      *
        * 处理完所有行后，该方法追加 RTF 结尾标记，并将 RTF 文本作为字符串返回。        *
        ***********************************************************************/

        #region [ ConvertToRtf ]
        private string ConvertToRtf(string text)
        {
            // 创建一个新的 StringBuilder 来构建 RTF 文本
            var rtf = new StringBuilder();
            // 追加 RTF 头部和颜色表
            rtf.Append(@"{\rtf1\ansi\deff0{\colortbl;\red255\green165\blue0;\red255\green0\blue0;\red0\green0\blue255;\red255\green255\blue0;\red0\green128\blue0;\red128\green0\blue128;\red255\green192\blue203;\red0\green255\blue255;\red255\green105\blue180;}");
            // 将输入文本拆分为行数组
            string[] lines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            // 循环处理每一行
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                // 检查行是否以 "Version :" 开头
                if (line.StartsWith("Version :"))
                {
                    // 追加使文本变为粗体的 RTF 代码
                    rtf.Append(@"\b ");
                    // 追加该行
                    rtf.Append(line);
                    // 追加结束粗体的 RTF 代码
                    rtf.Append(@"\b0 ");
                }
                else
                {
                    string[] words = line.Split(' ');
                    for (int j = 0; j < words.Length; j++)
                    {
                        string word = words[j];
                        if (word.StartsWith("#XC"))
                        {
                            int colorIndex = 0;
                            if (int.TryParse(word.Substring(3), out colorIndex) && colorIndex >= 1 && colorIndex <= 7)
                            {
                                rtf.Append(@"\cf" + colorIndex + " ");
                                rtf.Append(@"\b ");
                                if (words.Length > j + 1)
                                {
                                    rtf.Append(words[j + 1]);
                                    rtf.Append(@"\b0 ");
                                    rtf.Append(@"\cf0 ");
                                    j++;
                                }
                            }
                            else
                            {
                                rtf.Append(word);
                            }
                        }
                        else
                        {
                            rtf.Append(word);
                        }
                        if (j < words.Length - 1)
                        {
                            rtf.Append(" ");
                        }
                    }
                }
                // 如果不是最后一行
                if (i < lines.Length - 1)
                {
                    // 追加换行的 RTF 代码
                    rtf.Append(@"\line ");
                }
            }
            // 追加 RTF 结尾标记
            rtf.Append("}");
            // 返回 RTF 文本作为字符串
            return rtf.ToString();
        }
        #endregion

        #region [ searchToolStripMenuItem_Click ]
        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 将焦点设置到 ToolStripTextBox
            toolStripTextBox1.Focus();
        }
        #endregion

        #region [ richTextBox1_MouseDown ]
        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // 检查是否按下了鼠标右键
            if (e.Button == MouseButtons.Right)
            {
                // 在当前鼠标位置显示上下文菜单
                contextMenuStrip1.Show(richTextBox1, e.Location);
            }
        }
        #endregion
    }
}
