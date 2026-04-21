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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Plugin.Compare.Classes;

namespace UoFiddler.Plugin.Compare.UserControls
{
    public partial class CompareItemControl : UserControl
    {
        private readonly Dictionary<int, bool> _compare = new Dictionary<int, bool>();
        private readonly ImageConverter _ic = new ImageConverter();
        private readonly SHA256 _sha256 = SHA256.Create();

        private string _lastSelectedPath;
        private string _settingsDirectory = Path.Combine("data", "DirectoryisSettings"); // 创建 data/DirectoryisSettings 目录
        private string _settingsFileName = "CompareiItemsDirectoryisSettings.txt"; // 配置文件名
        private string _settingsFilePath;

        #region [ 构造函数 ]
        public CompareItemControl()
        {
            InitializeComponent();
            listBoxSec.SelectedIndexChanged += OnSelectedIndexChangedSec; //替换图片
            listBoxSec.KeyDown += ListBoxSec_KeyDown;

            EnsureSettingsDirectoryExists(); // 启动时确保目录存在
            LoadComboBoxSaveDir(); // 启动时加载历史目录到下拉框
        }
        #endregion

        #region [ 加载事件 ]
        private void OnLoad(object sender, EventArgs e)
        {
            listBoxOrg.Items.Clear();
            listBoxOrg.BeginUpdate();
            List<object> cache = new List<object>();
            int staticsLength = Art.GetMaxItemId() + 1;
            for (int i = 0; i < staticsLength; i++)
            {
                cache.Add(i);
            }
            listBoxOrg.Items.AddRange(cache.ToArray());
            listBoxOrg.EndUpdate();

            // 加载上次使用的目录
            if (!Directory.Exists(_settingsDirectory))
            {
                Directory.CreateDirectory(_settingsDirectory);
            }
            _settingsFilePath = Path.Combine(_settingsDirectory, _settingsFileName);

            if (File.Exists(_settingsFilePath))
            {
                _lastSelectedPath = File.ReadAllText(_settingsFilePath);
                textBoxSecondDir.Text = _lastSelectedPath;
            }

            // 加载下拉框历史目录
            LoadSettingsDirectoryComboBox();
        }
        #endregion

        #region [ 加载目录配置下拉框 ]
        private void LoadSettingsDirectoryComboBox()
        {
            string settingsDirectory = "DirectoryisSettings";
            string lastDirectoriesFileName = "CompareiItemsLastDirectories.txt";
            string lastDirectoriesFilePath = Path.Combine(settingsDirectory, lastDirectoriesFileName);
            if (File.Exists(lastDirectoriesFilePath))
            {
                string[] lines = File.ReadAllLines(lastDirectoriesFilePath);
                foreach (string line in lines)
                {
                    comboBoxSaveDir.Items.Add(line);
                }
            }
        }
        #endregion

        #region [ 左侧列表选中变化 ]
        private void OnIndexChangedOrg(object sender, EventArgs e)
        {
            if (listBoxOrg.SelectedIndex == -1 || listBoxOrg.Items.Count < 1)
            {
                return;
            }

            int i = int.Parse(listBoxOrg.Items[listBoxOrg.SelectedIndex].ToString());
            if (listBoxSec.Items.Count > 0)
            {
                int pos = listBoxSec.Items.IndexOf(i);
                if (pos >= 0)
                {
                    listBoxSec.SelectedIndex = pos;
                }
            }

            pictureBoxOrg.BackgroundImage = Art.IsValidStatic(i)
                ? Art.GetStatic(i)
                : null;

            // 设置搜索框为选中项的十六进制值
            searchTextBox.Text = $"0x{i:X}";

            listBoxOrg.Invalidate();
        }
        #endregion

        #region [ 左侧列表绘制 ]
        private void DrawItemOrg(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            Brush fontBrush = Brushes.Gray;

            int i = int.Parse(listBoxOrg.Items[e.Index].ToString());
            if (listBoxOrg.SelectedIndex == e.Index)
            {
                e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }

            if (!Art.IsValidStatic(i))
            {
                fontBrush = Brushes.Red;
            }
            else if (listBoxSec.Items.Count > 0)
            {
                if (!Compare(i))
                {
                    fontBrush = Brushes.Blue;
                }
            }

            e.Graphics.DrawString($"0x{i:X} ({i})", Font, fontBrush,
                new PointF(5,
                e.Bounds.Y + ((e.Bounds.Height / 2) -
                (e.Graphics.MeasureString($"0x{i:X} ({i})", Font).Height / 2))));
        }
        #endregion

        #region [ 左侧列表项高度 ]
        private void MeasureOrg(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 13;
        }
        #endregion

        #region [ 加载第二个资源文件 ]
        private void OnClickLoadSecond(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSecondDir.Text))
            {
                return;
            }

            string path = textBoxSecondDir.Text;
            string mulFile = Path.Combine(path, "art.mul");
            string idxFile = Path.Combine(path, "artidx.mul");
            //string uopFile = Path.Combine(path, "artLegacyMUL.uop");
            if (File.Exists(mulFile) && File.Exists(idxFile))
            {
                SecondArt.SetFileIndex(idxFile, mulFile); //加载.mul文件
                LoadSecond();
            }
            /*else if (File.Exists(uopFile)) //加载.uop文件
            {
                SecondArt.SetFileIndex(idxFile, uopFile); 
                LoadSecond();
            }*/
        }
        #endregion

        #region [ 加载右侧列表 ]
        private void LoadSecond()
        {
            _compare.Clear();
            listBoxSec.BeginUpdate();
            listBoxSec.Items.Clear();
            List<object> cache = new List<object>();
            int staticLength = SecondArt.GetMaxItemId() + 1;
            for (int i = 0; i < staticLength; i++)
            {
                cache.Add(i);
            }
            listBoxSec.Items.AddRange(cache.ToArray());
            listBoxSec.EndUpdate();
        }
        #endregion        

        #region [ 右侧列表绘制 ]
        private void DrawItemSec(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            Brush fontBrush = Brushes.Gray;
            Brush selectionBrush = Brushes.LightSteelBlue; // 首次选择颜色
            Brush secondSelectionBrush = Brushes.Yellow; // 二次选择颜色

            int i = int.Parse(listBoxSec.Items[e.Index].ToString());

            if (listBoxSec.SelectedIndices.Contains(e.Index))
            {
                if (ModifierKeys.HasFlag(Keys.Control)) // 按住Ctrl键时
                {
                    e.Graphics.FillRectangle(secondSelectionBrush, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                }
                else
                {
                    e.Graphics.FillRectangle(selectionBrush, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                }
            }

            if (!SecondArt.IsValidStatic(i))
            {
                fontBrush = Brushes.Red;
            }
            else if (!Compare(i))
            {
                fontBrush = Brushes.Blue;
            }

            e.Graphics.DrawString($"0x{i:X} ({i})", Font, fontBrush,
                new PointF(5,
                e.Bounds.Y + ((e.Bounds.Height / 2) -
                (e.Graphics.MeasureString($"0x{i:X} ({i})", Font).Height / 2))));
        }
        #endregion

        #region [ 右侧列表项绘制 ]
        private void ListBoxSec_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            Brush fontBrush = Brushes.Gray;
            Brush backBrush = Brushes.White;

            int i = int.Parse(listBoxSec.Items[e.Index].ToString());

            if (e.State.HasFlag(DrawItemState.Selected))
            {
                backBrush = Brushes.LightSteelBlue;
            }

            if (Control.ModifierKeys == Keys.Control && listBoxSec.SelectedIndices.Contains(e.Index))
            {
                backBrush = Brushes.LightGreen;
            }

            if (!SecondArt.IsValidStatic(i))
            {
                fontBrush = Brushes.Red;
            }
            else if (!Compare(i))
            {
                fontBrush = Brushes.Blue;
            }

            e.Graphics.FillRectangle(backBrush, e.Bounds);
            e.Graphics.DrawString($"0x{i:X}", Font, fontBrush,
                new PointF(5,
                e.Bounds.Y + ((e.Bounds.Height / 2) -
                (e.Graphics.MeasureString($"0x{i:X}", Font).Height / 2))));
        }
        #endregion

        #region [ 右侧列表项高度 ]
        private void MeasureSec(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 13;
        }
        #endregion

        #region [ 右侧列表选中变化 ]
        private void OnIndexChangedSec(object sender, EventArgs e)
        {
            if (listBoxSec.SelectedIndex == -1 || listBoxSec.Items.Count < 1)
            {
                return;
            }

            int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndex].ToString());
            int pos = listBoxOrg.Items.IndexOf(i);
            if (pos >= 0)
            {
                listBoxOrg.SelectedIndex = pos;
            }

            pictureBoxSec.BackgroundImage = SecondArt.IsValidStatic(i)
                ? SecondArt.GetStatic(i)
                : null;

            listBoxSec.Invalidate();
        }
        #endregion

        #region [ 图片对比 ]
        private bool Compare(int index)
        {
            if (_compare.ContainsKey(index))
            {
                return _compare[index];
            }

            Bitmap bitorg = Art.GetStatic(index);
            Bitmap bitsec = SecondArt.GetStatic(index);
            if (bitorg == null && bitsec == null)
            {
                _compare[index] = true;
                return true;
            }
            if (bitorg == null || bitsec == null
                               || bitorg.Size != bitsec.Size)
            {
                _compare[index] = false;
                return false;
            }

            byte[] btImage1 = new byte[1];
            btImage1 = (byte[])_ic.ConvertTo(bitorg, btImage1.GetType());
            byte[] btImage2 = new byte[1];
            btImage2 = (byte[])_ic.ConvertTo(bitsec, btImage2.GetType());

            byte[] checksum1 = _sha256.ComputeHash(btImage1);
            byte[] checksum2 = _sha256.ComputeHash(btImage2);
            bool res = true;
            for (int j = 0; j < checksum1.Length; ++j)
            {
                if (checksum1[j] != checksum2[j])
                {
                    res = false;
                    break;
                }
            }
            _compare[index] = res;
            return res;
        }
        #endregion

        #region [ 仅显示差异项 ]
        private void OnChangeShowDiff(object sender, EventArgs e)
        {
            if (_compare.Count < 1)
            {
                if (checkBox1.Checked)
                {
                    MessageBox.Show("未加载第二个物品资源文件！");
                    checkBox1.Checked = false;
                }
                return;
            }

            listBoxOrg.BeginUpdate();
            listBoxSec.BeginUpdate();
            listBoxOrg.Items.Clear();
            listBoxSec.Items.Clear();
            List<object> cache = new List<object>();
            int staticLength = Math.Max(Art.GetMaxItemId(), SecondArt.GetMaxItemId());
            if (checkBox1.Checked)
            {
                for (int i = 0; i < staticLength; i++)
                {
                    if (!Compare(i))
                    {
                        cache.Add(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < staticLength; i++)
                {
                    cache.Add(i);
                }
            }
            listBoxOrg.Items.AddRange(cache.ToArray());
            listBoxSec.Items.AddRange(cache.ToArray());
            listBoxOrg.EndUpdate();
            listBoxSec.EndUpdate();
        }
        #endregion

        #region 导出BMP + TIFF (单个)
        private void ExportAsBmp(object sender, EventArgs e)
        {
            if (listBoxSec.SelectedIndex == -1)
            {
                return;
            }

            int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndex].ToString());
            if (!SecondArt.IsValidStatic(i))
            {
                return;
            }

            string path = Options.OutputPath;
            string fileName = Path.Combine(path, $"物品(右侧) 0x{i:X}.bmp");
            SecondArt.GetStatic(i).Save(fileName, ImageFormat.Bmp);
            MessageBox.Show(
                $"物品已保存至 {fileName}",
                "保存成功",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void ExportAsTiff(object sender, EventArgs e)
        {
            if (listBoxSec.SelectedIndex == -1)
            {
                return;
            }

            int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndex].ToString());
            if (!SecondArt.IsValidStatic(i))
            {
                return;
            }

            string path = Options.OutputPath;
            string fileName = Path.Combine(path, $"物品(右侧) 0x{i:X}.tiff");
            SecondArt.GetStatic(i).Save(fileName, ImageFormat.Tiff);
            MessageBox.Show(
                $"物品已保存至 {fileName}",
                "保存成功",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
        #endregion

        #region 批量导出图片
        private void ExportAsBmp2(object sender, EventArgs e)
        {
            // 检查是否至少选择2个项目
            if (listBoxSec.SelectedIndices.Count < 2)
            {
                MessageBox.Show("请至少选择2个项目。");
                return;
            }

            // 确定起始和结束地址
            int startAddress = int.MaxValue;
            int endAddress = int.MinValue;
            foreach (int index in listBoxSec.SelectedIndices)
            {
                int address = int.Parse(listBoxSec.Items[index].ToString());
                if (address < startAddress) startAddress = address;
                if (address > endAddress) endAddress = address;
            }

            // 保存范围内的BMP文件
            for (int i = startAddress; i <= endAddress; i++)
            {
                if (!SecondArt.IsValidStatic(i)) continue;
                string path = Options.OutputPath;
                string fileName = Path.Combine(path, $"物品(右侧) 0x{i:X}.bmp");
                SecondArt.GetStatic(i).Save(fileName, ImageFormat.Bmp);
            }

            MessageBox.Show($"图片已保存至 {Options.OutputPath}", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportAsTiff2(object sender, EventArgs e)
        {
            // 检查是否至少选择2个项目
            if (listBoxSec.SelectedIndices.Count < 2)
            {
                MessageBox.Show("请至少选择2个项目。");
                return;
            }

            // 确定起始和结束地址
            int startAddress = int.MaxValue;
            int endAddress = int.MinValue;
            foreach (int index in listBoxSec.SelectedIndices)
            {
                int address = int.Parse(listBoxSec.Items[index].ToString());
                if (address < startAddress) startAddress = address;
                if (address > endAddress) endAddress = address;
            }

            // 保存范围内的TIFF文件
            for (int i = startAddress; i <= endAddress; i++)
            {
                if (!SecondArt.IsValidStatic(i)) continue;
                string path = Options.OutputPath;
                string fileName = Path.Combine(path, $"物品(右侧) 0x{i:X}.tiff");
                SecondArt.GetStatic(i).Save(fileName, ImageFormat.Tiff);
            }

            MessageBox.Show($"图片已保存至 {Options.OutputPath}", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region 复制物品
        private void OnClickCopy(object sender, EventArgs e)
        {
            if (listBoxSec.SelectedIndex == -1)
            {
                return;
            }

            int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndex].ToString());
            if (!SecondArt.IsValidStatic(i))
            {
                return;
            }

            int staticLength = Art.GetMaxItemId() + 1;
            if (i >= staticLength)
            {
                return;
            }

            Bitmap copy = new Bitmap(SecondArt.GetStatic(i));
            Art.ReplaceStatic(i, copy);
            Options.ChangedUltimaClass["Art"] = true;
            ControlEvents.FireItemChangeEvent(this, i);
            _compare[i] = true;
            listBoxOrg.BeginUpdate();
            bool done = false;

            for (int id = 0; id < staticLength; id++)
            {
                if (id > i)
                {
                    listBoxOrg.Items.Insert(id, i);
                    done = true;
                    break;
                }

                if (id == i)
                {
                    done = true;
                    break;
                }
            }

            if (!done)
            {
                listBoxOrg.Items.Add(i);
            }

            listBoxOrg.EndUpdate();
            listBoxOrg.Invalidate();
            listBoxSec.Invalidate();
            OnIndexChangedOrg(this, null);
        }
        #endregion

        #region 替换图片        

        private void OnClickReplace(object sender, EventArgs e)
        {
            // 打开对话框让用户选择替换区域
            using (var form = new Form())
            {
                form.Text = "选择要替换的区域";
                var label = new Label { Text = "区域:", Left = 10, Top = 10 };
                var textBox = new TextBox { Left = label.Right + 10, Top = label.Top };
                var button = new Button { Text = "确定", Left = textBox.Right + 10, Top = textBox.Top };
                button.Click += (s, e) =>
                {
                    // 验证用户输入的区域是否有效
                    string[] areas = textBox.Text.Split(',');
                    bool validAreas = true;
                    foreach (string area in areas)
                    {
                        if (!int.TryParse(area.Trim(), out int position) || position < 0 || position >= listBoxOrg.Items.Count)
                        {
                            validAreas = false;
                            break;
                        }
                    }

                    if (validAreas)
                    {
                        // 将用户选择的区域替换为右侧列表的选中项
                        foreach (string area in areas)
                        {
                            int position = int.Parse(area.Trim());
                            listBoxOrg.Items[position] = listBoxSec.SelectedItem;
                        }
                        form.Close();
                    }
                    else
                    {
                        MessageBox.Show("无效区域，请输入有效的区域编号。");
                    }
                };
                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(button);
                form.ShowDialog();
            }
        }

        private void OnClickShowSelection(object sender, EventArgs e)
        {
            UpdateContextMenu();
        }

        /*private void UpdateContextMenu()
        {
            // 清空右键菜单
            contextMenuStrip2.Items.Clear();

            // 将右侧列表选中项添加到菜单
            foreach (int index in listBoxSec.SelectedIndices)
            {
                int i = int.Parse(listBoxSec.Items[index].ToString());
                Bitmap image = SecondArt.GetStatic(i);
                string text = $"0x{i:X}";
                ToolStripMenuItem item = new ToolStripMenuItem(text);
                item.ImageScaling = ToolStripItemImageScaling.None; // 禁用图片缩放
                item.Image = new Bitmap(image, new Size(image.Width * 2, image.Height * 2)); // 放大图片
                item.Tag = i;
                item.Click += Item_Click;
                contextMenuStrip2.Items.Add(item);
            }

            // 添加确定按钮
            ToolStripButton okButton = new ToolStripButton("确定");
            okButton.Click += OnClickOkButton;
            contextMenuStrip2.Items.Add(okButton);
        }*/

        private void UpdateContextMenu()
        {
            // 清空右键菜单
            contextMenuStrip2.Items.Clear();

            // 将右侧列表选中项添加到菜单
            foreach (int index in listBoxSec.SelectedIndices)
            {
                int i = int.Parse(listBoxSec.Items[index].ToString());
                Bitmap image = SecondArt.GetStatic(i);
                if (image == null) // 跳过空图片
                {
                    continue;
                }
                string text = $"0x{i:X}";
                ToolStripMenuItem item = new ToolStripMenuItem(text);
                item.ImageScaling = ToolStripItemImageScaling.None; // 禁用图片缩放
                item.Image = new Bitmap(image, new Size(image.Width * 2, image.Height * 2)); // 放大图片
                item.Tag = i;
                item.Click += Item_Click;
                contextMenuStrip2.Items.Add(item);
            }

            // 添加确定按钮
            ToolStripButton okButton = new ToolStripButton("确定");
            okButton.Click += OnClickOkButton;
            contextMenuStrip2.Items.Add(okButton);
        }


        private void Item_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is int i)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                if (selectedIndex != -1)
                {
                    Bitmap image = SecondArt.GetStatic(i);
                    Art.ReplaceStatic(selectedIndex, image);
                    Options.ChangedUltimaClass["Art"] = true;
                    ControlEvents.FireItemChangeEvent(this, selectedIndex);

                    // 更新左侧预览图
                    pictureBoxOrg.BackgroundImage = Art.GetStatic(selectedIndex);

                    // 取消右侧列表选中状态
                    listBoxSec.SelectedIndices.Remove(listBoxSec.Items.IndexOf(i));
                }
            }
        }

        private void OnClickOkButton(object sender, EventArgs e)
        {
            // 用右侧第一个选中项替换左侧选中项
            int selectedIndex = listBoxOrg.SelectedIndex;
            if (selectedIndex != -1 && listBoxSec.SelectedIndices.Count > 0)
            {
                int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndices[0]].ToString());
                Bitmap image = SecondArt.GetStatic(i);
                Art.ReplaceStatic(selectedIndex, image);
                Options.ChangedUltimaClass["Art"] = true;
                ControlEvents.FireItemChangeEvent(this, selectedIndex);

                // 更新左侧预览图
                pictureBoxOrg.BackgroundImage = Art.GetStatic(selectedIndex);
            }
        }

        private void OnSelectedIndexChangedSec(object sender, EventArgs e)
        {
            UpdateContextMenu();
        }
        #endregion

        #region 十六进制搜索
        private void OnClickSearch_Click(object sender, EventArgs e)
        {
            string addressText = searchTextBox.Text;
            int address;
            if (addressText.StartsWith("0x") || System.Text.RegularExpressions.Regex.IsMatch(addressText, @"\A\b[0-9a-fA-F]+\b\Z"))
            {
                string hexText = addressText.StartsWith("0x") ? addressText.Substring(2) : addressText;
                if (int.TryParse(hexText, System.Globalization.NumberStyles.HexNumber, null, out address))
                {
                    int index = FindIndexByHexValue(listBoxOrg, hexText);
                    if (index != -1)
                    {
                        listBoxOrg.SelectedIndex = index;
                    }
                    else
                    {
                        MessageBox.Show("未找到该地址。");
                    }
                }
                else
                {
                    MessageBox.Show("无效地址，请输入有效的十六进制地址。");
                }
            }
            else
            {
                MessageBox.Show("无效地址，请输入有效的十六进制地址。");
            }
        }

        private int FindIndexByHexValue(ListBox listBox, string hexValue)
        {
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i] is int intValue && intValue.ToString("X").Equals(hexValue, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion

        #region [ 确保配置目录存在 ]      

        private void EnsureSettingsDirectoryExists()
        {
            if (!Directory.Exists(_settingsDirectory))
            {
                Directory.CreateDirectory(_settingsDirectory);
            }
            _settingsFilePath = Path.Combine(_settingsDirectory, _settingsFileName);
        }
        #endregion

        #region [ 浏览目录 ]
        private void OnClickBrowse(object sender, EventArgs e)
        {
            EnsureSettingsDirectoryExists();

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择包含美术资源文件的目录";
                dialog.ShowNewFolderButton = false;
                if (!string.IsNullOrEmpty(_lastSelectedPath))
                {
                    dialog.SelectedPath = _lastSelectedPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxSecondDir.Text = dialog.SelectedPath;
                    _lastSelectedPath = dialog.SelectedPath;
                    File.WriteAllText(_settingsFilePath, _lastSelectedPath); // 保存路径到文件
                    SaveLastDirectory(_lastSelectedPath); // 保存到历史目录列表
                    LoadComboBoxSaveDir(); // 刷新下拉框
                }
            }
        }
        #endregion        

        #region [ 目录下拉框选择变化 ]
        private void ComboBoxSaveDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSaveDir.SelectedIndex != -1)
            {
                textBoxSecondDir.Text = comboBoxSaveDir.SelectedItem.ToString();
            }
        }
        #endregion

        #region [ 保存历史目录 ]
        private void SaveLastDirectory(string directory)
        {
            string lastDirectoriesFileName = "CompareiItemsLastDirectories.txt";
            string lastDirectoriesFilePath = Path.Combine(_settingsDirectory, lastDirectoriesFileName);
            List<string> lastDirectories = new List<string>();

            // 读取已有目录
            if (File.Exists(lastDirectoriesFilePath))
            {
                lastDirectories.AddRange(File.ReadAllLines(lastDirectoriesFilePath));
            }

            // 确保目录唯一并置顶
            lastDirectories.Remove(directory);
            lastDirectories.Insert(0, directory);

            // 最多保存10条记录
            if (lastDirectories.Count > 10)
            {
                lastDirectories = lastDirectories.Take(10).ToList();
            }

            // 写回文件
            File.WriteAllLines(lastDirectoriesFilePath, lastDirectories);
        }
        #endregion

        #region [ 加载目录下拉框 ]
        private void LoadComboBoxSaveDir()
        {
            comboBoxSaveDir.Items.Clear(); // 清空现有项

            string lastDirectoriesFileName = "CompareiItemsLastDirectories.txt";
            string lastDirectoriesFilePath = Path.Combine(_settingsDirectory, lastDirectoriesFileName);

            if (File.Exists(lastDirectoriesFilePath))
            {
                string[] lastDirectories = File.ReadAllLines(lastDirectoriesFilePath);
                foreach (string dir in lastDirectories)
                {
                    comboBoxSaveDir.Items.Add(dir);
                }
            }
        }
        #endregion

        #region [ 向右移动项目 ]
        private void BtLeftMoveItem_Click(object sender, EventArgs e)
        {
            if (listBoxSec.SelectedIndex == -1)
            {
                return;
            }
            int i = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndex].ToString());
            if (!SecondArt.IsValidStatic(i))
            {
                return;
            }
            int staticLength = Art.GetMaxItemId() + 1;
            if (i >= staticLength)
            {
                return;
            }
            Bitmap copy = new Bitmap(SecondArt.GetStatic(i));
            Art.ReplaceStatic(i, copy);
            Options.ChangedUltimaClass["Art"] = true;
            ControlEvents.FireItemChangeEvent(this, i);
            _compare[i] = true;
            listBoxOrg.BeginUpdate();
            bool done = false;
            for (int id = 0; id < staticLength; id++)
            {
                if (id > i)
                {
                    listBoxOrg.Items.Insert(id, i);
                    done = true;
                    break;
                }
                if (id == i)
                {
                    done = true;
                    break;
                }
            }
            if (!done)
            {
                listBoxOrg.Items.Add(i);
            }
            listBoxOrg.EndUpdate();
            listBoxOrg.Invalidate();
            listBoxSec.Invalidate();

            // 更新左侧预览图
            pictureBoxOrg.BackgroundImage = Art.GetStatic(i);
        }
        #endregion

        #region [ 批量向右移动 ]
        private void BtLeftMoveItemMore_Click(object sender, EventArgs e)
        {
            if (listBoxSec.SelectedIndices.Count == 0)
            {
                return;
            }

            int staticLength = Art.GetMaxItemId() + 1;
            listBoxOrg.BeginUpdate();

            foreach (int selectedIndex in listBoxSec.SelectedIndices)
            {
                int i = int.Parse(listBoxSec.Items[selectedIndex].ToString());
                if (!SecondArt.IsValidStatic(i))
                {
                    continue;
                }
                if (i >= staticLength)
                {
                    continue;
                }
                Bitmap copy = new Bitmap(SecondArt.GetStatic(i));
                Art.ReplaceStatic(i, copy);
                Options.ChangedUltimaClass["Art"] = true;
                ControlEvents.FireItemChangeEvent(this, i);
                _compare[i] = true;

                bool done = false;
                for (int id = 0; id < staticLength; id++)
                {
                    if (id > i)
                    {
                        listBoxOrg.Items.Insert(id, i);
                        done = true;
                        break;
                    }
                    if (id == i)
                    {
                        done = true;
                        break;
                    }
                }
                if (!done)
                {
                    listBoxOrg.Items.Add(i);
                }
            }

            listBoxOrg.EndUpdate();
            listBoxOrg.Invalidate();
            listBoxSec.Invalidate();

            // 更新预览图为第一个选中项
            int firstSelectedIndex = int.Parse(listBoxSec.Items[listBoxSec.SelectedIndices[0]].ToString());
            pictureBoxOrg.BackgroundImage = Art.GetStatic(firstSelectedIndex);
        }
        #endregion

        #region [ 右侧列表键盘快捷键 ]
        private void ListBoxSec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                BtLeftMoveItem_Click(sender, e);
                e.Handled = true; // 阻止方向键默认行为
            }
            if (e.KeyCode == Keys.Right)
            {
                Btremoveitemfromindex_Click(sender, e);
                e.Handled = true; // 阻止方向键默认行为
            }
        }
        #endregion

        #region [ 移除索引项目 ]
        private void Btremoveitemfromindex_Click(object sender, EventArgs e)
        {
            if (listBoxOrg.SelectedIndex != -1)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                // 将选中项置为空
                Art.ReplaceStatic(selectedIndex, null);
                Options.ChangedUltimaClass["Art"] = true;
                ControlEvents.FireItemChangeEvent(this, selectedIndex);

                // 清空预览图
                pictureBoxOrg.BackgroundImage = null;
            }
        }
        #endregion

        #region [ 打开配置目录 ]
        private void BtDataDirectoryisSettings_Click(object sender, EventArgs e)
        {
            string settingsDirectory = Path.Combine("Data", "DirectoryisSettings"); // 配置目录
            if (Directory.Exists(settingsDirectory))
            {
                System.Diagnostics.Process.Start("explorer.exe", settingsDirectory);
            }
            else
            {
                MessageBox.Show("目录不存在。");
            }
        }
        #endregion
    }
}