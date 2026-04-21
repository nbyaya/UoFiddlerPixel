/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
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
    public partial class CompareGumpControl : UserControl
    {
        public CompareGumpControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private readonly Dictionary<int, bool> _compare = new Dictionary<int, bool>();
        private readonly SHA256 _sha256 = SHA256.Create();

        private bool _loaded;

        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Options.LoadedUltimaClass["Gumps"] = true;

            // 确保两个列表框都支持多选和自定义绘制（设计器已设置，但此处再确保一次）
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            listBox2.SelectionMode = SelectionMode.MultiExtended;
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox2.DrawMode = DrawMode.OwnerDrawFixed;

            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            List<object> cache = new List<object>();
            for (int i = 0; i < 0x10000; i++)
            {
                cache.Add(i);
            }
            listBox1.Items.AddRange(cache.ToArray());
            listBox1.EndUpdate();
            listBox2.Items.Clear();
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
            }

            if (!_loaded)
            {
                ControlEvents.FilePathChangeEvent += OnFilePathChangeEvent;
            }

            _loaded = true;
            Cursor.Current = Cursors.Default;
        }

        private void OnFilePathChangeEvent()
        {
            Reload();
        }

        private void Reload()
        {
            if (_loaded)
            {
                OnLoad(EventArgs.Empty);
            }
        }

        // 统一绘制方法（支持多选高亮），与设计器绑定的 Listbox1_DrawItem 名称一致
        private void Listbox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (e.Index < 0)
            {
                return;
            }

            Brush fontBrush = Brushes.Gray;

            int i = (int)listBox.Items[e.Index];

            // 多选高亮：使用 SelectedIndices 判断是否选中
            bool isSelected = listBox.SelectedIndices.Contains(e.Index);
            if (isSelected)
            {
                e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }

            bool valid = (int)listBox.Tag == 1 ? Gumps.IsValidIndex(i) : SecondGump.IsValidIndex(i);

            if (valid)
            {
                Bitmap bmp = (int)listBox.Tag == 1 ? Gumps.GetGump(i) : SecondGump.GetGump(i);

                if (bmp != null)
                {
                    if (listBox2.Items.Count > 0)
                    {
                        if (!Compare(i))
                        {
                            fontBrush = Brushes.Blue;
                        }
                    }
                    int width = bmp.Width > 80 ? 80 : bmp.Width;
                    int height = bmp.Height > 54 ? 54 : bmp.Height;

                    e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, width, height));
                }
                else
                {
                    fontBrush = Brushes.Red;
                }
            }
            else
            {
                fontBrush = Brushes.Red;
            }

            e.Graphics.DrawString($"0x{i:X}", Font, fontBrush,
                new PointF(85,
                e.Bounds.Y + ((e.Bounds.Height / 2) -
                (e.Graphics.MeasureString($"0x{i:X}", Font).Height / 2))));
        }

        private void Listbox_measureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 60;
        }

        private void Listbox_SelectedChange(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex == -1)
            {
                return;
            }

            int i = (int)listBox.Items[listBox.SelectedIndex];
            bool valid;
            if ((int)listBox.Tag == 1)
            {
                valid = Gumps.IsValidIndex(i);
                if (listBox2.Items.Count > 0)
                {
                    listBox2.SelectedIndex = listBox2.Items.IndexOf(i);
                }
            }
            else
            {
                valid = SecondGump.IsValidIndex(i);
                listBox1.SelectedIndex = listBox1.Items.IndexOf(i);
            }
            if (valid)
            {
                Bitmap bmp = (int)listBox.Tag == 1 ? Gumps.GetGump(i) : SecondGump.GetGump(i);

                if (bmp != null)
                {
                    if ((int)listBox.Tag == 1)
                    {
                        pictureBox1.BackgroundImage = bmp;
                    }
                    else
                    {
                        pictureBox2.BackgroundImage = bmp;
                    }
                }
                else
                {
                    if ((int)listBox.Tag == 1)
                    {
                        pictureBox1.BackgroundImage = null;
                    }
                    else
                    {
                        pictureBox2.BackgroundImage = null;
                    }
                }
            }
            else
            {
                if ((int)listBox.Tag == 1)
                {
                    pictureBox1.BackgroundImage = null;
                }
                else
                {
                    pictureBox2.BackgroundImage = null;
                }
            }
            listBox.Invalidate();
        }

        private void Browse_OnClick(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select directory containing the gump files";
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxSecondDir.Text = dialog.SelectedPath;
                }
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            if (textBoxSecondDir.Text == null)
            {
                return;
            }

            string path = textBoxSecondDir.Text;
            string file = Path.Combine(path, "gumpart.mul");
            string file2 = Path.Combine(path, "gumpidx.mul");
            if (File.Exists(file) && File.Exists(file2))
            {
                SecondGump.SetFileIndex(file2, file);
                LoadSecond();
            }
        }

        private void LoadSecond()
        {
            _compare.Clear();
            listBox2.BeginUpdate();
            listBox2.Items.Clear();
            List<object> cache = new List<object>();
            for (int i = 0; i < 0x10000; i++)
            {
                cache.Add(i);
            }
            listBox2.Items.AddRange(cache.ToArray());
            listBox2.EndUpdate();
            listBox1.Invalidate();
        }

        private bool Compare(int index)
        {
            if (_compare.TryGetValue(index, out bool value))
            {
                return value;
            }

            byte[] org = Gumps.GetRawGump(index, out int width1, out int height1);
            byte[] sec = SecondGump.GetRawGump(index, out int width2, out int height2);
            bool res = false;

            if (org == null && sec == null)
            {
                res = true;
            }
            else if (org == null || sec == null
                                || org.Length != sec.Length)
            {
                res = false;
            }
            else if (width1 != width2 || height1 != height2)
            {
                res = false;
            }
            else
            {
                string hash1String = BitConverter.ToString(_sha256.ComputeHash(org));
                string hash2String = BitConverter.ToString(_sha256.ComputeHash(sec));
                if (hash1String == hash2String)
                {
                    res = true;
                }
            }
            _compare[index] = res;
            return res;
        }

        private void ShowDiff_OnClick(object sender, EventArgs e)
        {
            if (_compare.Count < 1)
            {
                if (checkBox1.Checked)
                {
                    MessageBox.Show("Second Gump file is not loaded!");
                    checkBox1.Checked = false;
                }
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            listBox1.BeginUpdate();
            listBox2.BeginUpdate();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            List<object> cache = new List<object>();
            if (checkBox1.Checked)
            {
                for (int i = 0; i < 0x10000; i++)
                {
                    if (!Compare(i))
                    {
                        cache.Add(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 0x10000; i++)
                {
                    cache.Add(i);
                }
            }
            listBox1.Items.AddRange(cache.ToArray());
            listBox2.Items.AddRange(cache.ToArray());
            listBox1.EndUpdate();
            listBox2.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        private void Export_Bmp(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                return;
            }

            int i = int.Parse(listBox2.Items[listBox2.SelectedIndex].ToString());
            if (!SecondGump.IsValidIndex(i))
            {
                return;
            }

            string path = Options.OutputPath;
            string fileName = Path.Combine(path, $"Gump(Sec) 0x{i:X}.bmp");
            SecondGump.GetGump(i).Save(fileName, ImageFormat.Bmp);
            MessageBox.Show(
                $"Gump saved to {fileName}",
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void Export_Tiff(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                return;
            }

            int i = int.Parse(listBox2.Items[listBox2.SelectedIndex].ToString());
            if (!SecondGump.IsValidIndex(i))
            {
                return;
            }

            string path = Options.OutputPath;
            string fileName = Path.Combine(path, $"Gump(Sec) 0x{i:X}.tiff");
            SecondGump.GetGump(i).Save(fileName, ImageFormat.Tiff);
            MessageBox.Show(
                $"Gump saved to {fileName}",
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void OnClickCopy(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                return;
            }

            int i = (int)listBox2.Items[listBox2.SelectedIndex];
            if (!SecondGump.IsValidIndex(i))
            {
                return;
            }

            CopyGumpFromSecondToFirst(i);
        }

        /// <summary>
        /// 将右侧（第二个文件）的指定索引的 gump 复制到左侧（第一个文件）
        /// </summary>
        private void CopyGumpFromSecondToFirst(int index)
        {
            if (!SecondGump.IsValidIndex(index))
                return;

            Bitmap copy = new Bitmap(SecondGump.GetGump(index));
            Gumps.ReplaceGump(index, copy);
            Options.ChangedUltimaClass["Gumps"] = true;
            ControlEvents.FireGumpChangeEvent(this, index);
            _compare[index] = true; // 标记为相同，消除差异
        }

        #region 按钮事件处理（一键复制差异功能）

        private void BtLeftMoveItem_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;

            int index = (int)listBox2.Items[listBox2.SelectedIndex];
            CopyGumpFromSecondToFirst(index);
            ShowDiff_OnClick(sender, e);
        }

        private void BtLeftMoveItemMore_Click(object sender, EventArgs e)
        {
            var selectedIndices = listBox2.SelectedIndices.Cast<int>().OrderByDescending(i => i).ToList();
            if (selectedIndices.Count == 0)
                return;

            foreach (int selIndex in selectedIndices)
            {
                int index = (int)listBox2.Items[selIndex];
                CopyGumpFromSecondToFirst(index);
            }

            ShowDiff_OnClick(sender, e);
        }

        private void Btremoveitemfromindex_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;

            int index = (int)listBox2.Items[listBox2.SelectedIndex];
            CopyGumpFromSecondToFirst(index);
            ShowDiff_OnClick(sender, e);
        }

        #endregion
    }
}