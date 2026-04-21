// /***************************************************************************
//  *
//  * $Author: Nikodemus
//  * 
//  * "葡萄酒许可证"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯葡萄酒作为回报。
//  *
//  ***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ultima;
using System.Security.Cryptography;
using UoFiddler.Controls.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Xml.Linq;
using UoFiddler.Controls.Forms;

namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    public partial class UOArtMergeForm : Form
    {
        private readonly Dictionary<int, bool> _mCompare = new Dictionary<int, bool>();
        private readonly ImageConverter _ic = new ImageConverter();
        private readonly SHA256 _sha256 = SHA256.Create();

        private Dictionary<int, Bitmap> selectedImages = new Dictionary<int, Bitmap>();

        #region UOArtMergeForm
        public UOArtMergeForm()
        {
            InitializeComponent();
            LoadOrg();

            listBoxOrg.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxOrg.DrawItem += ListBoxOrg_DrawItem;

            listBoxLeft.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxRight.DrawMode = DrawMode.OwnerDrawFixed;

            LoadDirectoriesIntoComboBox();
            this.Load += UOArtMergeForm_Load;
        }
        #endregion

        #region LoadOrg
        private void LoadOrg()
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
        }
        #endregion

        #region OnIndexChangedOrg
        private void OnIndexChangedOrg(object sender, EventArgs e)
        {
            if (listBoxOrg.SelectedIndex == -1 || listBoxOrg.Items.Count < 1)
            {
                return;
            }
            int i = int.Parse(listBoxOrg.Items[listBoxOrg.SelectedIndex].ToString());
            pictureBoxOrg.BackgroundImage = Art.IsValidStatic(i) ? Art.GetStatic(i) : null;
            listBoxOrg.Invalidate();

            pictureBoxOrg.BackgroundImage = Art.IsValidStatic(i)
                ? Art.GetStatic(i)
                : null;

            if (checkBoxSameHeight.Checked)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                if (selectedIndex < listBoxLeft.Items.Count)
                {
                    listBoxLeft.SelectedIndex = selectedIndex;
                }
                if (selectedIndex < listBoxRight.Items.Count)
                {
                    listBoxRight.SelectedIndex = selectedIndex;
                }
            }

            // 将搜索文本框的文本设置为选中项的十六进制表示
            searchTextBox.Text = $"0x{i:X}";

            // 检查 listBoxOrg 中是否有选中项
            if (listBoxOrg.SelectedIndex != -1)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                // 将选中的索引转换为十六进制地址
                string hexAddress = $"0x{selectedIndex:X}";
                // 更新 lbIndex，显示十六进制地址和 ID
                lbIndex.Text = $"十六进制地址: {hexAddress}, ID: {selectedIndex}";
            }

            // 获取 listBoxOrg 中的项目总数（ID 数量）
            int totalIDs = listBoxOrg.Items.Count;
            // 更新 lbIndex，显示 ID 总数
            lbCountOrg.Text = $"ID 总数: {totalIDs}";

            // 检查 listBoxOrg 中是否有选中项
            if (listBoxOrg.SelectedIndex != -1)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                if (!Art.IsValidStatic(selectedIndex))
                {
                    return;
                }

                // 获取当前图像及其关联数据
                Bitmap currentImage = Art.GetStatic(selectedIndex);
                ItemData item = TileData.ItemTable[selectedIndex];

                // 创建一个 StringBuilder 来存储图像的详细信息
                var sb = new StringBuilder();
                sb.AppendLine($"名称: {item.Name}");
                sb.AppendLine($"图形: 0x{selectedIndex:X4}");
                sb.AppendLine($"高度/容量: {item.Height}");
                sb.AppendLine($"重量: {item.Weight}");
                sb.AppendLine($"动画: {item.Animation}");
                sb.AppendLine($"品质/层级/光照: {item.Quality}");
                sb.AppendLine($"数量: {item.Quantity}");
                sb.AppendLine($"色调: {item.Hue}");
                sb.AppendLine($"堆叠偏移/未知4: {item.StackingOffset}");
                sb.AppendLine($"标志: {item.Flags}");
                sb.AppendLine($"图形像素尺寸 宽,高: {currentImage?.Width ?? 0} {currentImage?.Height ?? 0}");

                // 将详细信息粘贴到 DetailTextBox 中
                DetailTextBox.Clear();
                DetailTextBox.AppendText(sb.ToString());
            }
        }
        #endregion

        #region [ ListBoxOrg_DrawItem ]
        private void ListBoxOrg_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            e.DrawBackground();

            int i = int.Parse(listBoxOrg.Items[e.Index].ToString());
            string hexValue = $"0x{i:X}";
            string displayValue = $"{hexValue} ({i})"; // 同时显示十六进制地址和 ID 地址

            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(displayValue, e.Font, brush, e.Bounds);
            }
        }
        #endregion

        #region [ BtDirLeft ]
        private void BtDirLeft_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择包含美术文件的目录";
                dialog.ShowNewFolderButton = false;
                if (!string.IsNullOrEmpty(textBoxLeftDir.Text))
                {
                    dialog.SelectedPath = textBoxLeftDir.Text;
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxLeftDir.Text = dialog.SelectedPath;
                }
            }
        }
        #endregion

        #region [ BtLeftLoad ]
        private void BtLeftLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLeftDir.Text))
            {
                return;
            }

            string path = textBoxLeftDir.Text;
            string mulFile = Path.Combine(path, "art.mul");
            string idxFile = Path.Combine(path, "artidx.mul");
            if (File.Exists(mulFile) && File.Exists(idxFile))
            {
                SecondArt.SetFileIndex(idxFile, mulFile); //加载 .mul 文件
                LoadLeft();
            }
        }
        #endregion

        #region LoadLeft
        private void LoadLeft()
        {
            listBoxLeft.Items.Clear();
            listBoxLeft.BeginUpdate();
            List<object> cache = new List<object>();
            int staticLength = SecondArt.GetMaxItemId() + 1;
            for (int i = 0; i < staticLength; i++)
            {
                cache.Add(i);
            }
            listBoxLeft.Items.AddRange(cache.ToArray());
            listBoxLeft.EndUpdate();
        }
        #endregion

        #region [ ListBoxLeft_SelectedIndexChanged ]
        private void ListBoxLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxLeft.SelectedIndex == -1)
            {
                return;
            }

            int i = int.Parse(listBoxLeft.Items[listBoxLeft.SelectedIndex].ToString());
            pictureBoxLeft.Image = SecondArt.IsValidStatic(i) ? SecondArt.GetStatic(i) : null;

            if (checkBoxSameHeight.Checked)
            {
                int selectedIndex = listBoxLeft.SelectedIndex;
                if (selectedIndex < listBoxOrg.Items.Count)
                {
                    listBoxOrg.SelectedIndex = selectedIndex;
                }
                if (selectedIndex < listBoxLeft.Items.Count)
                {
                    listBoxLeft.SelectedIndex = selectedIndex;
                }
            }

            // 检查 listBoxLeft 中是否有选中项
            if (listBoxLeft.SelectedIndex != -1)
            {
                int selectedIndex = listBoxLeft.SelectedIndex;
                // 将选中的索引转换为十六进制地址
                string hexAddress = $"0x{selectedIndex:X}";
                // 更新 lbIndexLeft，显示十六进制地址和 ID
                lbIndexLeft.Text = $"十六进制地址: {hexAddress}, ID: {selectedIndex}";
            }

            // 获取 listBoxLeft 中的项目总数（ID 数量）
            int totalIDs = listBoxLeft.Items.Count;
            // 更新 lbIndex，显示 ID 总数
            lbCountLeft.Text = $"ID 总数: {totalIDs}";
        }
        #endregion

        #region [ ListBoxLeft_DrawItem ]
        private void ListBoxLeft_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            e.DrawBackground();

            int i = int.Parse(listBoxLeft.Items[e.Index].ToString());
            string hexValue = $"0x{i:X}";
            string displayValue = $"{hexValue} ({i})"; // 同时显示十六进制地址和 ID 地址

            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(displayValue, e.Font, brush, e.Bounds);
            }
        }
        #endregion

        #region [ BtDirRight ]
        private void BtDirRight_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择包含美术文件的目录";
                dialog.ShowNewFolderButton = false;
                if (!string.IsNullOrEmpty(textBoxRightDir.Text))
                {
                    dialog.SelectedPath = textBoxRightDir.Text;
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxRightDir.Text = dialog.SelectedPath;                    
                }
            }
        }
        #endregion

        #region [ BtRightLoad ]
        private void BtRightLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxRightDir.Text))
            {
                return;
            }

            string path = textBoxRightDir.Text;
            string mulFile = Path.Combine(path, "art.mul");
            string idxFile = Path.Combine(path, "artidx.mul");
            if (File.Exists(mulFile) && File.Exists(idxFile))
            {
                SecondArt.SetFileIndex(idxFile, mulFile); //加载 .mul 文件
                LoadRight();
            }
        }
        #endregion

        #region LoadRight
        private void LoadRight()
        {
            listBoxRight.Items.Clear();
            listBoxRight.BeginUpdate();
            List<object> cache = new List<object>();
            int staticLength = SecondArt.GetMaxItemId() + 1;
            for (int i = 0; i < staticLength; i++)
            {
                cache.Add(i);
            }
            listBoxRight.Items.AddRange(cache.ToArray());
            listBoxRight.EndUpdate();
        }
        #endregion

        #region [ ListBoxRight_SelectedIndexChanged ]
        private void ListBoxRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxRight.SelectedIndex == -1)
            {
                return;
            }

            int i = int.Parse(listBoxRight.Items[listBoxRight.SelectedIndex].ToString());
            pictureBoxRight.Image = SecondArt.IsValidStatic(i) ? SecondArt.GetStatic(i) : null;

            if (checkBoxSameHeight.Checked)
            {
                int selectedIndex = listBoxRight.SelectedIndex;
                if (selectedIndex < listBoxOrg.Items.Count)
                {
                    listBoxOrg.SelectedIndex = selectedIndex;
                }
                if (selectedIndex < listBoxRight.Items.Count)
                {
                    listBoxRight.SelectedIndex = selectedIndex;
                }
            }

            // 检查 listBoxRight 中是否有选中项
            if (listBoxRight.SelectedIndex != -1)
            {
                int selectedIndex = listBoxRight.SelectedIndex;
                // 将选中的索引转换为十六进制地址
                string hexAddress = $"0x{selectedIndex:X}";
                // 更新 lbIndexRight，显示十六进制地址和 ID
                lbIndexRight.Text = $"十六进制地址: {hexAddress}, ID: {selectedIndex}";
            }

            // 获取 listBoxRight 中的项目总数（ID 数量）
            int totalIDs = listBoxRight.Items.Count;
            // 更新 lbIndex，显示 ID 总数
            lbCountRight.Text = $"ID 总数: {totalIDs}";
        }
        #endregion

        #region [ ListBoxRight_DrawItem ]
        private void ListBoxRight_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }

            e.DrawBackground();

            int i = int.Parse(listBoxRight.Items[e.Index].ToString());
            string hexValue = $"0x{i:X}";
            string displayValue = $"{hexValue} ({i})"; // 同时显示十六进制地址和 ID 地址

            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(displayValue, e.Font, brush, e.Bounds);
            }
        }
        #endregion

        #region OnChangeShowDiff
        private void OnChangeShowDiff(object sender, EventArgs e)
        {
            // 对 listBoxOrg 中的每个项目调用 Compare
            for (int i = 0; i < listBoxOrg.Items.Count; i++)
            {
                Compare(i);
            }

            if (_mCompare.Count < 1)
            {
                if (checkBoxOnChangeShowDiff.Checked)
                {
                    MessageBox.Show("未加载第二个项目文件！");
                    checkBoxOnChangeShowDiff.Checked = false;
                }
                return;
            }

            listBoxOrg.BeginUpdate();
            listBoxRight.BeginUpdate();
            listBoxLeft.BeginUpdate();
            listBoxOrg.Items.Clear();
            listBoxRight.Items.Clear();
            listBoxLeft.Items.Clear();
            List<object> cache = new List<object>();
            int staticLength = Math.Max(Art.GetMaxItemId(), SecondArt.GetMaxItemId());
            if (checkBoxOnChangeShowDiff.Checked)
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
            listBoxRight.Items.AddRange(cache.ToArray());
            listBoxLeft.Items.AddRange(cache.ToArray());
            listBoxOrg.EndUpdate();
            listBoxRight.EndUpdate();
            listBoxLeft.EndUpdate();
        }
        #endregion

        #region Compare
        private bool Compare(int index)
        {
            if (_mCompare.ContainsKey(index))
            {
                return _mCompare[index];
            }

            Bitmap bitorg = Art.GetStatic(index);
            Bitmap bitsec = SecondArt.GetStatic(index);
            if (bitorg == null && bitsec == null)
            {
                _mCompare[index] = true;
                return true;
            }
            if (bitorg == null || bitsec == null
                               || bitorg.Size != bitsec.Size)
            {
                _mCompare[index] = false;
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
            _mCompare[index] = res;
            return res;
        }
        #endregion

        #region [ BtRightMoveItem ]
        private void BtRightMoveItem_Click(object sender, EventArgs e)
        {
            if (listBoxRight.SelectedIndex == -1)
            {
                return;
            }
            int i = int.Parse(listBoxRight.Items[listBoxRight.SelectedIndex].ToString());
            if (!SecondArt.IsValidStatic(i))
            {
                return;
            }
            Bitmap copy = new Bitmap(SecondArt.GetStatic(i));

            // 如果启用了 checkBoxFreeIDchoice，则将图像插入到 listBoxOrg 中选中的 ID 位置
            if (checkBoxfFreeIDchoice.Checked)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                if (selectedIndex != -1)
                {
                    Art.ReplaceStatic(selectedIndex, copy);
                    Options.ChangedUltimaClass["Art"] = true;
                    ControlEvents.FireItemChangeEvent(this, selectedIndex);
                    _mCompare[selectedIndex] = true;

                    // 使用选中的图像更新 pictureBoxOrg
                    pictureBoxOrg.BackgroundImage = Art.GetStatic(selectedIndex);
                }
            }
            else
            {
                int staticLength = Art.GetMaxItemId() + 1;
                if (i >= staticLength)
                {
                    return;
                }
                Art.ReplaceStatic(i, copy);
                Options.ChangedUltimaClass["Art"] = true;
                ControlEvents.FireItemChangeEvent(this, i);
                _mCompare[i] = true;
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
                listBoxRight.Invalidate();

                // 使用选中的项目更新 pictureBoxOrg
                pictureBoxOrg.BackgroundImage = Art.GetStatic(i);
            }
        }
        #endregion

        #region [ CheckBoxSameHeight_CheckedChanged ]
        private void CheckBoxSameHeight_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSameHeight.Checked)
            {
                checkBoxfFreeIDchoice.Checked = false;
            }
        }
        #endregion

        #region [ CheckBoxfFreeIDchoice_CheckedChanged ]
        private void CheckBoxfFreeIDchoice_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxfFreeIDchoice.Checked)
            {
                checkBoxSameHeight.Checked = false;
            }
        }
        #endregion

        #region [ BtLeftMoveItem ]
        private void BtLeftMoveItem_Click(object sender, EventArgs e)
        {
            if (listBoxLeft.SelectedIndex == -1)
            {
                return;
            }
            int i = int.Parse(listBoxLeft.Items[listBoxLeft.SelectedIndex].ToString());
            if (!SecondArt.IsValidStatic(i))
            {
                return;
            }
            Bitmap copy = new Bitmap(SecondArt.GetStatic(i));

            // 如果启用了 checkBoxFreeIDchoice，则将图像插入到 listBoxOrg 中选中的 ID 位置
            if (checkBoxfFreeIDchoice.Checked)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                if (selectedIndex != -1)
                {
                    Art.ReplaceStatic(selectedIndex, copy);
                    Options.ChangedUltimaClass["Art"] = true;
                    ControlEvents.FireItemChangeEvent(this, selectedIndex);
                    _mCompare[selectedIndex] = true;

                    // 使用选中的图像更新 pictureBoxOrg
                    pictureBoxOrg.BackgroundImage = Art.GetStatic(selectedIndex);
                }
            }
            else
            {
                int staticLength = Art.GetMaxItemId() + 1;
                if (i >= staticLength)
                {
                    return;
                }
                Art.ReplaceStatic(i, copy);
                Options.ChangedUltimaClass["Art"] = true;
                ControlEvents.FireItemChangeEvent(this, i);
                _mCompare[i] = true;
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
                listBoxLeft.Invalidate();

                // 使用选中的项目更新 pictureBoxOrg
                pictureBoxOrg.BackgroundImage = Art.GetStatic(i);
            }
        }
        #endregion

        #region [ BtSaveXML ]
        private void BtSaveXML_Click(object sender, EventArgs e)
        {
            // 如果目录不存在则创建
            string settingsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DirectoryisSettings");
            if (!Directory.Exists(settingsDirectory))
            {
                Directory.CreateDirectory(settingsDirectory);
            }

            // 创建 XML 文件
            string xmlFilePath = Path.Combine(settingsDirectory, "XMLSaveDirUAArtMerge.xml");

            // 如果 XML 文件存在则加载，否则新建
            XDocument doc;
            if (File.Exists(xmlFilePath))
            {
                doc = XDocument.Load(xmlFilePath);
            }
            else
            {
                doc = new XDocument(new XElement("Directories"));
            }

            // 统计现有目录条目的数量
            int directoryCount = doc.Root.Elements("Directory").Count();

            // 将目录添加到 XML 文件中，并赋予唯一 ID
            doc.Root.Add(
                new XElement("Directory",
                    new XAttribute("id", directoryCount + 1),
                    new XAttribute("name", "LeftDir"),
                    new XAttribute("path", textBoxLeftDir.Text)),
                new XElement("Directory",
                    new XAttribute("id", directoryCount + 2),
                    new XAttribute("name", "RightDir"),
                    new XAttribute("path", textBoxRightDir.Text))
            );
            doc.Save(xmlFilePath);

            // 更新 comboBoxSaveDir
            comboBoxSaveDir.Items.Clear();
            comboBoxSaveDir.Items.Add(textBoxLeftDir.Text);
            comboBoxSaveDir.Items.Add(textBoxRightDir.Text);
        }
        #endregion

        #region LoadDirectoriesIntoComboBox
        private void LoadDirectoriesIntoComboBox()
        {
            // 创建 XML 文件的路径
            string settingsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DirectoryisSettings");
            string xmlFilePath = Path.Combine(settingsDirectory, "XMLSaveDirUAArtMerge.xml");

            // 检查 XML 文件是否存在
            if (!File.Exists(xmlFilePath))
            {
                return;
            }

            // 读取 XML 文件
            XDocument doc = XDocument.Load(xmlFilePath);
            var directories = doc.Root.Elements("Directory");

            // 将每个路径添加到 comboBoxSaveDir 和 comboBoxSaveDir2
            comboBoxSaveDir.Items.Clear();
            comboBoxSaveDir2.Items.Clear();
            foreach (var directory in directories)
            {
                string path = directory.Attribute("path").Value;
                comboBoxSaveDir.Items.Add(path);
                comboBoxSaveDir2.Items.Add(path);  // 将路径添加到 comboBoxSaveDir2
            }
        }
        #endregion

        #region UOArtMergeForm_Load
        private void UOArtMergeForm_Load(object sender, EventArgs e)
        {
            LoadIDsIntoListBox();
        }
        #endregion

        #region LoadIDsIntoListBox
        private void LoadIDsIntoListBox()
        {
            // 创建 XML 文件的路径
            string settingsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DirectoryisSettings");
            string xmlFilePath = Path.Combine(settingsDirectory, "XMLSaveDirUAArtMerge.xml");

            // 检查 XML 文件是否存在
            if (!File.Exists(xmlFilePath))
            {
                MessageBox.Show("找不到 XML 文件。");
                return;
            }

            // 读取 XML 文件
            XDocument doc;
            try
            {
                doc = XDocument.Load(xmlFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取 XML 文件时出错：{ex.Message}");
                return;
            }

            var directories = doc.Root.Elements("Directory");

            // 将每个 ID 添加到 tbIDNr
            tbIDNr.Items.Clear();
            foreach (var directory in directories)
            {
                string id = directory.Attribute("id")?.Value;
                if (id != null)
                {
                    tbIDNr.Items.Add(id);
                }
            }
        }
        #endregion

        #region [ ComboBoxSaveDir_SelectedIndexChanged ]
        private void ComboBoxSaveDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSaveDir.SelectedItem != null)
            {
                string selectedPath = comboBoxSaveDir.SelectedItem.ToString();
                textBoxLeftDir.Text = selectedPath;
            }
        }
        #endregion

        #region [ ComboBoxSaveDir2_SelectedIndexChanged ]
        private void ComboBoxSaveDir2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSaveDir2.SelectedItem != null)
            {
                string selectedPath = comboBoxSaveDir2.SelectedItem.ToString();
                textBoxRightDir.Text = selectedPath;
            }
        }
        #endregion

        #region DeleteDirectoryById       

        private void DeleteDirectoryById(int id)
        {
            string settingsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DirectoryisSettings");
            string xmlFilePath = Path.Combine(settingsDirectory, "XMLSaveDirUAArtMerge.xml");

            if (!File.Exists(xmlFilePath))
            {
                MessageBox.Show("找不到 XML 文件。");
                return;
            }

            XDocument doc;
            try
            {
                doc = XDocument.Load(xmlFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取 XML 文件时出错：{ex.Message}");
                return;
            }

            var directoryToDelete = doc.Root.Elements("Directory").FirstOrDefault(d => (int)d.Attribute("id") == id);
            if (directoryToDelete != null)
            {
                directoryToDelete.Remove();

                // 重新分配 ID
                int newId = 1;
                foreach (var directory in doc.Root.Elements("Directory"))
                {
                    directory.Attribute("id").Value = newId.ToString();
                    newId++;
                }

                doc.Save(xmlFilePath);

                // 从 tbIDNr 中移除该 ID
                tbIDNr.Items.Remove(id.ToString());
            }
            else
            {
                MessageBox.Show($"未找到 ID {id}。");
            }
        }

        #endregion

        #region [ BtDelete ]
        private void BtDelete_Click(object sender, EventArgs e)
        {
            // 从文本框中读取 ID
            if (!int.TryParse(tbIDNr.Text, out int id))
            {
                MessageBox.Show("请输入有效的 ID。");
                return;
            }

            // 删除指定 ID 的目录
            DeleteDirectoryById(id);

            // 更新 comboBoxSaveDir
            LoadDirectoriesIntoComboBox();
        }
        #endregion

        #region [ Btremoveitemfromindex ]
        private void Btremoveitemfromindex_Click(object sender, EventArgs e)
        {
            if (listBoxOrg.SelectedIndex != -1)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                if (!Art.IsValidStatic(selectedIndex))
                {
                    return;
                }

                DialogResult result = MessageBox.Show($"确定要移除 0x{selectedIndex:X} 吗？", "保存",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // 从 Art 中移除选中的项
                Art.RemoveStatic(selectedIndex);
                Options.ChangedUltimaClass["Art"] = true;
                ControlEvents.FireItemChangeEvent(this, selectedIndex);

                // 更新 pictureBoxOrg
                pictureBoxOrg.BackgroundImage = null;

                // 刷新 listBoxOrg 以反映更改
                listBoxOrg.Invalidate();
            }
        }
        #endregion

        #region OnClickSave
        private void OnClickSave(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要保存吗？这可能需要一段时间。", "保存", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            ProgressBarDialog barDialog = new ProgressBarDialog(Art.GetIdxLength(), "保存");
            Art.Save(Options.OutputPath);
            barDialog.Dispose();
            Cursor.Current = Cursors.Default;
            Options.ChangedUltimaClass["Art"] = false;
            MessageBox.Show($"已保存到 {Options.OutputPath}", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
        #endregion

        #region Search Textbox Hex
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
                        MessageBox.Show("未找到地址。");
                    }
                }
                else
                {
                    MessageBox.Show("无效地址。请输入有效的十六进制地址。");
                }
            }
            else
            {
                MessageBox.Show("无效地址。请输入有效的十六进制地址。");
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

        #region [ MirrorToolStripMenuItem_Click ]
        private void MirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxOrg.SelectedIndex != -1)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                if (!Art.IsValidStatic(selectedIndex))
                {
                    return;
                }

                // 获取当前图像
                Bitmap currentImage = Art.GetStatic(selectedIndex);
                if (currentImage != null)
                {
                    // 创建一个当前图像的镜像副本
                    Bitmap mirroredImage = new Bitmap(currentImage);
                    mirroredImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

                    // 在 Art 中用镜像图像替换当前图像
                    Art.ReplaceStatic(selectedIndex, mirroredImage);
                    Options.ChangedUltimaClass["Art"] = true;
                    ControlEvents.FireItemChangeEvent(this, selectedIndex);

                    // 更新 pictureBoxOrg
                    pictureBoxOrg.BackgroundImage = mirroredImage;

                    // 刷新 listBoxOrg 以反映更改
                    listBoxOrg.Invalidate();
                }
            }
        }
        #endregion

        #region [ CopyToolStripMenuItem_Click ]
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxOrg.SelectedIndex != -1)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                if (!Art.IsValidStatic(selectedIndex))
                {
                    return;
                }

                // 获取当前图像
                Bitmap currentImage = Art.GetStatic(selectedIndex);
                if (currentImage != null)
                {
                    // 将图像复制到剪贴板
                    Clipboard.SetImage(currentImage);
                }
            }
        }
        #endregion

        #region [ CopyToolStripMenuItemListBoxRight_Click ]

        private void CopyToolStripMenuItemListBoxRight_Click(object sender, EventArgs e)
        {
            if (listBoxRight.SelectedIndex != -1)
            {
                int selectedIndex = listBoxRight.SelectedIndex;
                if (!Art.IsValidStatic(selectedIndex))
                {
                    return;
                }

                // 获取当前图像
                Bitmap currentImage = Art.GetStatic(selectedIndex);
                if (currentImage != null)
                {
                    // 将图像复制到剪贴板
                    Clipboard.SetImage(currentImage);
                }
            }
        }
        #endregion

        #region [ CopyToolStripMenuItemListBoxLeft_Click ]
        private void CopyToolStripMenuItemListBoxLeft_Click(object sender, EventArgs e)
        {
            if (listBoxLeft.SelectedIndex != -1)
            {
                int selectedIndex = listBoxLeft.SelectedIndex;
                if (!Art.IsValidStatic(selectedIndex))
                {
                    return;
                }

                // 获取当前图像
                Bitmap currentImage = Art.GetStatic(selectedIndex);
                if (currentImage != null)
                {
                    // 将图像复制到剪贴板
                    Clipboard.SetImage(currentImage);
                }
            }
        }
        #endregion

        #region [ ImportToolStripclipboardMenuItem_Click }
        private void ImportToolStripclipboardMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxOrg.SelectedIndex != -1)
            {
                int selectedIndex = listBoxOrg.SelectedIndex;
                if (!Art.IsValidStatic(selectedIndex))
                {
                    return;
                }

                // 检查剪贴板是否包含图像
                if (Clipboard.ContainsImage())
                {
                    // 从剪贴板检索图像
                    using (Bitmap bmp = new Bitmap(Clipboard.GetImage()))
                    {
                        // 创建一个与剪贴板图像相同大小的新位图
                        Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);

                        // 定义要转换的颜色
                        Color[] colorsToConvert = new Color[]
                        {
                    Color.FromArgb(211, 211, 211), // #D3D3D3 => #000000
                    Color.FromArgb(0, 0, 0), // #000000 => #000000
                    Color.FromArgb(255, 255, 255), // #FFFFFF => #000000
                    Color.FromArgb(254, 254, 254) // #FEFEFE => #000000
                        };

                        // 遍历图像的每个像素
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            for (int y = 0; y < bmp.Height; y++)
                            {
                                // 获取当前像素的颜色
                                Color pixelColor = bmp.GetPixel(x, y);
                                if (colorsToConvert.Contains(pixelColor))
                                {
                                    newBmp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                                }
                                else
                                {
                                    newBmp.SetPixel(x, y, pixelColor);
                                }
                            }
                        }

                        // 在 Art 中用新图像替换选中的项
                        Art.ReplaceStatic(selectedIndex, newBmp);
                        Options.ChangedUltimaClass["Art"] = true;
                        ControlEvents.FireItemChangeEvent(this, selectedIndex);

                        // 更新 pictureBoxOrg
                        pictureBoxOrg.BackgroundImage = newBmp;

                        // 刷新 listBoxOrg 以反映更改
                        listBoxOrg.Invalidate();
                    }
                }
                else
                {
                    MessageBox.Show("剪贴板中没有图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion


    }
}