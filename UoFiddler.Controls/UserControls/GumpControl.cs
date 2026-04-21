/***************************************************************************
 *
 * $Author: Turley
 *
 * "THE BEER-WARE LICENSE" (啤酒软件授权协议)
 * 只要您保留此声明，您就可以对本代码做任何处理。
 * 如果将来我们相遇，并且您认为这些东西有价值，
 * 您可以请我喝啤酒作为回报。
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Forms;
using UoFiddler.Controls.Helpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace UoFiddler.Controls.UserControls
{
    public partial class GumpControl : UserControl
    {
        private Dictionary<string, string> idNames = new Dictionary<string, string>(); // XML

        #region [ GumpControl ]
        public GumpControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint,
                true);
            if (!Files.CacheData)
            {
                Preload.Visible = false;
            }

            ProgressBar.Visible = false;

            _refMarker = this;
            
            listBox.KeyDown += ListBox_KeyDown;
        }
        #endregion

        private static GumpControl _refMarker;
        private bool _loaded;
        private bool _showFreeSlots;
        private GumpSearchForm _showForm; // _showForm
        private AddressListForm addressListForm; // 用于列出地址的窗体

        #region [ Reload ]

        /// <summary>
        /// 当加载后（文件更改）重新加载
        /// </summary> 
        private void Reload()
        {
            if (!_loaded)
            {
                return;
            }

            _loaded = false;
            OnLoad(EventArgs.Empty);
        }
        #endregion

        #region [ OnLoad ]
        protected override void OnLoad(EventArgs e)
        {
            if (IsAncestorSiteInDesignMode || FormsDesignerHelper.IsInDesignMode())
            {
                return;
            }

            if (_loaded)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            Options.LoadedUltimaClass["Gumps"] = true;
            _showFreeSlots = false;
            showFreeSlotsToolStripMenuItem.Checked = false;

            LoadIdNamesFromXml();

            PopulateListBox(true);

            if (!_loaded)
            {
                ControlEvents.FilePathChangeEvent += OnFilePathChangeEvent;
                ControlEvents.GumpChangeEvent += OnGumpChangeEvent;
            }

            _loaded = true;
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region [ LoadIdNamesFromXml ]
        private void LoadIdNamesFromXml()
        {
            // XML 文件路径
            string xmlFilePath = Path.Combine(Application.StartupPath, "IDGumpNames.xml");

            XDocument doc;

            // 检查 XML 文件是否存在
            if (File.Exists(xmlFilePath))
            {
                // 加载 XML 文件
                doc = XDocument.Load(xmlFilePath);
            }
            else
            {
                // 创建带有根元素 "IDNames" 的新 XML 文件
                doc = new XDocument(new XElement("IDNames"));
                doc.Save(xmlFilePath);
            }

            foreach (XElement idElement in doc.Root.Elements("ID"))
            {
                string id = idElement.Attribute("value").Value;
                string name = idElement.Attribute("name").Value;

                // 将名称添加到字典中
                idNames[id] = name;
            }
        }
        #endregion

        #region [ PopulateListBox ]
        private void PopulateListBox(bool showOnlyValid)
        {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            List<object> cache = new List<object>();
            int maxGumpID = 0;

            for (int i = 0; i < Gumps.GetCount(); i++)
            {
                if (Gumps.IsValidIndex(i))
                {
                    maxGumpID = i;
                }
            }

            for (int i = 0; i < maxGumpID; i++)
            {
                if (showOnlyValid && !_showFreeSlots)
                {
                    if (Gumps.IsValidIndex(i))
                    {
                        string name = idNames.TryGetValue(i.ToString(), out string idName) ? idName : "";
                        cache.Add($"{i} - {name}");
                    }
                }
                else
                {
                    string name = idNames.TryGetValue(i.ToString(), out string idName) ? idName : "";
                    cache.Add($"{i} - {name}");
                }
            }

            if (_showFreeSlots)
            {
                for (int i = maxGumpID + 1; i <= Gumps.GetCount(); i++)
                {
                    string name = idNames.TryGetValue(i.ToString(), out string idName) ? idName : "";
                    cache.Add($"{i} - {name}");
                }
            }

            listBox.Items.AddRange(cache.ToArray());
            listBox.EndUpdate();

            if (listBox.Items.Count > 0)
            {
                listBox.SelectedIndex = 0;
            }
            listBox.Refresh();
        }
        #endregion

        #region [ OnFilePathChangeEven ]
        private void OnFilePathChangeEvent()
        {
            Reload();
        }
        #endregion

        #region [ OnGumpChangeEvent ]
        private void OnGumpChangeEvent(object sender, int index)
        {
            if (!_loaded)
            {
                return;
            }

            if (sender.Equals(this))
            {
                return;
            }

            if (Gumps.IsValidIndex(index))
            {
                bool done = false;
                for (int i = 0; i < listBox.Items.Count; ++i)
                {
                    int j = int.Parse(listBox.Items[i].ToString());
                    if (j > index)
                    {
                        listBox.Items.Insert(i, index);
                        listBox.SelectedIndex = i;
                        done = true;
                        break;
                    }

                    if (j == index)
                    {
                        done = true;
                        break;
                    }
                }

                if (!done)
                {
                    listBox.Items.Add(index);
                }
            }
            else
            {
                for (int i = 0; i < listBox.Items.Count; ++i)
                {
                    int j = int.Parse(listBox.Items[i].ToString());
                    if (j == index)
                    {
                        listBox.Items.RemoveAt(i);
                        break;
                    }
                }

                listBox.Invalidate();
            }
        }
        #endregion

        #region [ ListBox_DrawItem ]
        private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            Brush fontBrush = Brushes.Gray;

            string itemString = listBox.Items[e.Index].ToString();
            string idString = itemString.Split('-')[0].Trim();
            int i = int.Parse(idString);

            if (Gumps.IsValidIndex(i))
            {
                Bitmap bmp = Gumps.GetGump(i, out bool patched);

                if (bmp != null)
                {
                    int width = bmp.Width > 100 ? 100 : bmp.Width;
                    int height = bmp.Height > 54 ? 54 : bmp.Height;

                    if (listBox.SelectedIndex == e.Index)
                    {
                        e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X, e.Bounds.Y, 105, 60);
                    }
                    else if (patched)
                    {
                        e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, 105, 60);
                    }

                    e.Graphics.DrawImage(bmp, new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, width, height));
                }
                else
                {
                    fontBrush = Brushes.Red;
                }
            }
            else
            {
                if (listBox.SelectedIndex == e.Index)
                {
                    e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds.X, e.Bounds.Y, 105, 60);
                }

                fontBrush = Brushes.Red;
            }
            // 从字典中检索名称
            string name = idNames.TryGetValue(i.ToString(), out string idName) ? idName : "";

            // 绘制 ID、十六进制地址和名称
            e.Graphics.DrawString($"0x{i:X} ({i}) {name}", Font, fontBrush,
                new PointF(105,
                    e.Bounds.Y + ((e.Bounds.Height / 2) -
                                  (e.Graphics.MeasureString($"0x{i:X} ({i}) {name}", Font).Height / 2))));

            // 用于列出地址
            if (e.Index == listBox.SelectedIndex)
            {
                string address = itemString.Split('-')[0].Trim();
                addressListForm?.AddAddress(address);
            }
        }
        #endregion

        #region [ SelectAddress ]
        public void SelectAddress(string address)
        {
            int addressIndex = listBox.Items.Cast<string>().ToList().FindIndex(item => item.StartsWith(address));
            if (addressIndex >= 0)
            {
                listBox.SelectedIndex = addressIndex;
                listBox.TopIndex = addressIndex;
            }
        }
        #endregion

        #region [ ListBox_MeasureItem ]
        private void ListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 60;
        }
        #endregion

        #region [ ListBox_SelectedIndexChanged ]
        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
            {
                return;
            }

            string itemString = listBox.Items[listBox.SelectedIndex].ToString();
            string idString = itemString.Split('-')[0].Trim();
            int i = int.Parse(idString);

            if (Gumps.IsValidIndex(i))
            {
                Bitmap bmp = Gumps.GetGump(i);

                if (bmp != null)
                {
                    pictureBox.BackgroundImage = bmp;
                    string name = idNames.TryGetValue(i.ToString(), out string idName) ? idName : "";
                    IDLabel.Text = $"ID: 0x{i:X} ({i}) {name}";
                    SizeLabel.Text = $"大小: {bmp.Width},{bmp.Height}";
                }
                else
                {
                    pictureBox.BackgroundImage = null;
                }
            }
            else
            {
                pictureBox.BackgroundImage = null;
            }

            listBox.Invalidate();
            JumpToMaleFemaleInvalidate();
        }
        #endregion

        #region [ JumpToMaleFemaleInvalistate ]
        private void JumpToMaleFemaleInvalidate()
        {
            if (listBox.SelectedIndex == -1)
            {
                return;
            }

            string itemString = listBox.SelectedItem.ToString();
            string idString = itemString.Split('-')[0].Trim();
            int gumpId = int.Parse(idString);
            if (gumpId >= 50000)
            {
                if (gumpId >= 60000)
                {
                    jumpToMaleFemale.Text = "跳转到男性";
                    jumpToMaleFemale.Enabled = HasGumpId(gumpId - 10000);
                }
                else
                {
                    jumpToMaleFemale.Text = "跳转到女性";
                    jumpToMaleFemale.Enabled = HasGumpId(gumpId + 10000);
                }
            }
            else
            {
                jumpToMaleFemale.Enabled = false;
                jumpToMaleFemale.Text = "跳转到男性/女性";
            }
        }
        #endregion

        #region [ OnClickReplace ]
        private void OnClickReplace(object sender, EventArgs e)
        {
            if (listBox.SelectedItems.Count != 1)
            {
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = "选择要替换的图像文件";
                dialog.CheckFileExists = true;
                dialog.Filter = "图像文件 (*.tif;*.tiff;*.bmp;*.png)|*.tif;*.tiff;*.bmp;*.png";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                using (var bmpTemp = new Bitmap(dialog.FileName))
                {
                    Bitmap bitmap = new Bitmap(bmpTemp);

                    if (dialog.FileName.Contains(".bmp"))
                    {
                        bitmap = Utils.ConvertBmp(bitmap);
                    }

                    // 在连字符位置分割字符串，只使用第一部分
                    string itemString = listBox.Items[listBox.SelectedIndex].ToString();
                    string idString = itemString.Split('-')[0].Trim();
                    int i = int.Parse(idString);

                    Gumps.ReplaceGump(i, bitmap);

                    ControlEvents.FireGumpChangeEvent(this, i);

                    listBox.Invalidate();
                    ListBox_SelectedIndexChanged(this, EventArgs.Empty);

                    Options.ChangedUltimaClass["Gumps"] = true;

                    // 如果 isSoundMessageActive 为 true，则播放声音
                    if (isSoundMessageActive)
                    {
                        if (playCustomSound)
                        {
                            player.Play();
                        }
                        else
                        {
                            player.SoundLocation = "sound.wav";
                            player.Play();
                        }
                    }
                }
            }
        }

        #endregion

        #region [ OnClickSave ]
        private void OnClickSave(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要保存吗？这需要一些时间", "保存", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            Gumps.Save(Options.OutputPath);
            Cursor.Current = Cursors.Default;
            MessageBox.Show($"已保存到 {Options.OutputPath}", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            Options.ChangedUltimaClass["Gumps"] = false;
        }
        #endregion

        #region [ OnClickRemove ]
        private void OnClickRemove(object sender, EventArgs e)
        {
            // 在连字符位置分割字符串，只使用第一部分
            string itemString = listBox.Items[listBox.SelectedIndex].ToString();
            string idString = itemString.Split('-')[0].Trim();
            int i = int.Parse(idString);

            DialogResult result = MessageBox.Show($"确定要移除 {i} 吗？", "移除", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
            {
                return;
            }

            Gumps.RemoveGump(i);
            ControlEvents.FireGumpChangeEvent(this, i);
            if (!_showFreeSlots)
            {
                listBox.Items.RemoveAt(listBox.SelectedIndex);
            }

            pictureBox.BackgroundImage = null;
            listBox.Invalidate();
            Options.ChangedUltimaClass["Gumps"] = true;

            // 如果 isSoundMessageActive 为 true，则播放声音
            if (isSoundMessageActive)
            {
                if (playCustomSound)
                {
                    player.Play();
                }
                else
                {
                    player.SoundLocation = "sound.wav";
                    player.Play();
                }
            }
        }
        #endregion

        #region [ OnClickFindFree ]
        private void OnClickFindFree(object sender, EventArgs e)
        {
            // 在连字符位置分割字符串，只使用第一部分
            string itemString = listBox.Items[listBox.SelectedIndex].ToString();
            string idString = itemString.Split('-')[0].Trim();
            int id = int.Parse(idString);
            ++id;

            for (int i = listBox.SelectedIndex + 1; i < listBox.Items.Count; ++i, ++id)
            {
                itemString = listBox.Items[i].ToString();
                idString = itemString.Split('-')[0].Trim();
                if (id < int.Parse(idString))
                {
                    listBox.SelectedIndex = i;
                    break;
                }

                if (!_showFreeSlots)
                {
                    continue;
                }

                if (!Gumps.IsValidIndex(int.Parse(idString)))
                {
                    listBox.SelectedIndex = i;
                    break;
                }
            }

            // 如果未找到空 ID 且 _showFreeSlots 已启用，
            // 则将新 ID 添加到 ListBox 末尾
            if (listBox.SelectedIndex == -1 && _showFreeSlots)
            {
                int newId = Gumps.GetCount();
                listBox.Items.Add(newId);
                listBox.SelectedIndex = listBox.Items.Count - 1;
            }

            // 如果 isSoundMessageActive 为 true，则播放声音
            if (isSoundMessageActive)
            {
                if (playCustomSound)
                {
                    player.Play();
                }
                else
                {
                    player.SoundLocation = "sound.wav";
                    player.Play();
                }
            }
        }
        #endregion

        #region [ AddShowAllFreeSlotsButton ]
        private void AddShowAllFreeSlotsButton_Click(object sender, EventArgs e)
        {

            _showFreeSlots = !_showFreeSlots;
            PopulateListBox(!_showFreeSlots);
        }
        #endregion

        #region [ OnTextChanged_InsertAt ]
        private void OnTextChanged_InsertAt(object sender, EventArgs e)
        {
            if (Utils.ConvertStringToInt(InsertText.Text, out int index, 0, Gumps.GetCount()))
            {
                InsertText.ForeColor = Gumps.IsValidIndex(index) ? Color.Red : Color.Black;
            }
            else
            {
                InsertText.ForeColor = Color.Red;
            }
        }
        #endregion

        #region [ OnKeydown_InserText ]
        private void OnKeydown_InsertText(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            // 在连字符位置分割字符串，只使用第一部分
            string itemString = InsertText.Text;
            string idString = itemString.Split('-')[0].Trim();
            if (!Utils.ConvertStringToInt(idString, out int index, 0, Gumps.GetCount()))
            {
                return;
            }

            if (Gumps.IsValidIndex(index))
            {
                return;
            }

            contextMenuStrip.Close();
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = $"选择要在 0x{index:X} 处插入的图像文件";
                dialog.CheckFileExists = true;
                dialog.Filter = "图像文件 (*.tif;*.tiff;*.bmp;*.png)|*.tif;*.tiff;*.bmp;*.png";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                using (var bmpTemp = new Bitmap(dialog.FileName))
                {
                    Bitmap bitmap = new Bitmap(bmpTemp);

                    if (dialog.FileName.Contains(".bmp"))
                    {
                        bitmap = Utils.ConvertBmp(bitmap);
                    }

                    Gumps.ReplaceGump(index, bitmap);

                    ControlEvents.FireGumpChangeEvent(this, index);

                    bool done = false;
                    for (int i = 0; i < listBox.Items.Count; ++i)
                    {
                        itemString = listBox.Items[i].ToString();
                        idString = itemString.Split('-')[0].Trim();
                        int j = int.Parse(idString);
                        if (j > index)
                        {
                            listBox.Items.Insert(i, index);
                            listBox.SelectedIndex = i;
                            done = true;
                            break;
                        }

                        if (!_showFreeSlots)
                        {
                            continue;
                        }

                        if (!Gumps.IsValidIndex(j))
                        {
                            listBox.SelectedIndex = i;
                            break;
                        }
                    }

                    if (!done)
                    {
                        listBox.Items.Add(index);
                        listBox.SelectedIndex = listBox.Items.Count - 1;
                    }

                    Options.ChangedUltimaClass["Gumps"] = true;
                }
            }
        }
        #endregion

        #region [ Extract_Image_ClickBmp ]
        private void Extract_Image_ClickBmp(object sender, EventArgs e)
        {
            string itemString = listBox.Items[listBox.SelectedIndex].ToString();
            string idString = itemString.Split('-')[0].Trim();
            if (Int32.TryParse(idString, out int i))
            {
                ExportGumpImage(i, ImageFormat.Bmp);
            }
        }
        #endregion

        #region [ Extract_Image_ClickTiff ]
        private void Extract_Image_ClickTiff(object sender, EventArgs e)
        {
            string itemString = listBox.Items[listBox.SelectedIndex].ToString();
            string idString = itemString.Split('-')[0].Trim();
            if (Int32.TryParse(idString, out int i))
            {
                ExportGumpImage(i, ImageFormat.Tiff);
            }
        }
        #endregion

        #region [ Extract_Image_ClickJpg ]
        private void Extract_Image_ClickJpg(object sender, EventArgs e)
        {
            string itemString = listBox.Items[listBox.SelectedIndex].ToString();
            string idString = itemString.Split('-')[0].Trim();
            if (Int32.TryParse(idString, out int i))
            {
                ExportGumpImage(i, ImageFormat.Jpeg);
            }
        }
        #endregion

        #region [ Extract_Image_ClickPng ]
        private void Extract_Image_ClickPng(object sender, EventArgs e)
        {
            string itemString = listBox.Items[listBox.SelectedIndex].ToString();
            string idString = itemString.Split('-')[0].Trim();
            if (Int32.TryParse(idString, out int i))
            {
                ExportGumpImage(i, ImageFormat.Png);
            }
        }
        #endregion

        #region [ ExportGumpImage ]
        private static void ExportGumpImage(int index, ImageFormat imageFormat)
        {
            string fileExtension = Utils.GetFileExtensionFor(imageFormat);
            //string fileName = Path.Combine(Options.OutputPath, $"Gump 0x{index:X4}.{fileExtension}");
            string fileName = Path.Combine(Options.OutputPath, $"0x{index:X4}.{fileExtension}");


            using (Bitmap bit = new Bitmap(Gumps.GetGump(index)))
            {
                bit.Save(fileName, imageFormat);
            }

            MessageBox.Show(
                $"Gump 已保存到 {fileName}",
                "已保存",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
        #endregion

        #region [ OnClick_SaveAllBmp ]
        private void OnClick_SaveAllBmp(object sender, EventArgs e)
        {
            ExportAllGumps(ImageFormat.Bmp);
        }
        #endregion

        #region [ OnClick_SaveAllTiff ]
        private void OnClick_SaveAllTiff(object sender, EventArgs e)
        {
            ExportAllGumps(ImageFormat.Tiff);
        }
        #endregion

        #region [ OnClick_SaveAllJpg ]
        private void OnClick_SaveAllJpg(object sender, EventArgs e)
        {
            ExportAllGumps(ImageFormat.Jpeg);
        }
        #endregion

        #region [ OnClick_SaveAllPng ]
        private void OnClick_SaveAllPng(object sender, EventArgs e)
        {
            ExportAllGumps(ImageFormat.Png);
        }
        #endregion

        #region [ ExportAllGumps ]

        private void ExportAllGumps(ImageFormat imageFormat)
        {
            string fileExtension = Utils.GetFileExtensionFor(imageFormat);

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择目录";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var progressBarDialog = new ProgressBarDialog2(listBox.Items.Count, $"正在导出 Gump 到 {fileExtension}", false);
                progressBarDialog.CancelClicked += () =>
                {
                    // 仅在事件中显示消息
                    MessageBox.Show("导出已中止。", "取消", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                progressBarDialog.Show();

                Cursor.Current = Cursors.WaitCursor;

                Task.Run(() =>
                {
                    int exportedCount = 0;

                    try
                    {
                        for (int i = 0; i < listBox.Items.Count; ++i)
                        {
                            if (progressBarDialog.IsCancelled)
                            {
                                break; // 中止
                            }

                            string itemString = listBox.Items[i].ToString();
                            string idString = itemString.Split('-')[0].Trim();

                            if (!int.TryParse(idString, out int index) || index < 0)
                            {
                                continue;
                            }

                            string fileName = Path.Combine(dialog.SelectedPath, $"0x{index:X4}.{fileExtension}");

                            var gump = Gumps.GetGump(index);

                            if (gump is null)
                            {
                                continue;
                            }

                            using (Bitmap bit = new Bitmap(gump))
                            {
                                bit.Save(fileName, imageFormat);
                                exportedCount++;
                            }

                            Invoke((Action)(() => progressBarDialog.OnChangeEvent()));
                        }

                        // 标记进程完成
                        Invoke((Action)(() => progressBarDialog.MarkProcessFinished()));
                    }
                    finally
                    {
                        Invoke((Action)(() =>
                        {
                            Cursor.Current = Cursors.Default;
                            progressBarDialog.Close();

                            if (!progressBarDialog.IsCancelled)
                            {
                                MessageBox.Show($"所有 Gump 已保存到 {dialog.SelectedPath}\n" +
                                                $"导出的图像总数: {exportedCount}",
                                    "保存完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }));
                    }
                });
            }
        }
        #endregion

        #region [ OnClickShowFreeSlots ]
        private void OnClickShowFreeSlots(object sender, EventArgs e)
        {
            _showFreeSlots = !_showFreeSlots;
            PopulateListBox(!_showFreeSlots);
        }
        #endregion

        #region [ onClickPreload ]
        private void OnClickPreLoad(object sender, EventArgs e)
        {
            if (PreLoader.IsBusy)
            {
                return;
            }

            ProgressBar.Minimum = 1;
            ProgressBar.Maximum = Gumps.GetCount();
            ProgressBar.Step = 1;
            ProgressBar.Value = 1;
            ProgressBar.Visible = true;
            PreLoader.RunWorkerAsync();
        }

        private void PreLoaderDoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < Gumps.GetCount(); ++i)
            {
                Gumps.GetGump(i);
                PreLoader.ReportProgress(1);
            }
        }

        private void PreLoaderProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.PerformStep();
        }

        private void PreLoaderCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Visible = false;
        }
        #endregion

        #region [ Static Void Select ]
        internal static void Select(int gumpId)
        {
            if (!_refMarker._loaded)
            {
                _refMarker.OnLoad(EventArgs.Empty);
            }

            _refMarker.Search(gumpId.ToString());
        }
        #endregion

        #region [ HasGumpId ]

        public static bool HasGumpId(int gumpId) // HasGumpId 是跨类方法，用于识别其中的 Gump 项目。
        {
            if (!_refMarker._loaded)
            {
                _refMarker.OnLoad(EventArgs.Empty);
            }

            return _refMarker.listBox.Items.Cast<object>().Any(id =>
            {
                if (int.TryParse(id.ToString().Split('-')[0].Trim(), out int intId))
                {
                    return intId == gumpId;
                }
                return false;
            });
        }
        #endregion

        #region [ JumpToMaleFemale_Click ]        
        private void JumpToMaleFemale_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
            {
                return;
            }

            string itemString = listBox.SelectedItem.ToString();
            string idString = itemString.Split('-')[0].Trim();
            int gumpId = int.Parse(idString);

            gumpId = gumpId < 60000 ? (gumpId % 10000) + 60000 : (gumpId % 10000) + 50000;

            // 在 listBox 中查找新的 gumpId
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                itemString = listBox.Items[i].ToString();
                idString = itemString.Split('-')[0].Trim();
                if (int.Parse(idString) == gumpId)
                {
                    listBox.SelectedIndex = i;
                    break;
                }
            }
        }
        #endregion

        #region [ JumpToMaleFemale2_Click ]
        // 全部合为一个（已禁用）
        // 这里是一个将三个功能组合在一起的方法，但其中一个被停用。
        // 我将该函数作为示例保留：'JumpToMaleFemale, HasGumpId, JumpToMaleFemaleInvalidate'
        private void JumpToMaleFemale2_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
            {
                return;
            }

            string itemString = listBox.SelectedItem.ToString();
            string idString = itemString.Split('-')[0].Trim();
            if (!int.TryParse(idString, out int gumpId))
            {
                MessageBox.Show("无效的 Gump ID 格式。");
                return;
            }

            gumpId = gumpId < 60000 ? (gumpId % 10000) + 60000 : (gumpId % 10000) + 50000;

            // 在 listBox 中查找新的 gumpId
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                itemString = listBox.Items[i].ToString();
                idString = itemString.Split('-')[0].Trim();
                if (int.TryParse(idString, out int newGumpId) && newGumpId == gumpId)
                {
                    listBox.SelectedIndex = i;
                    break;
                }
            }

            // 更新按钮文本和启用状态
            if (gumpId >= 50000)
            {
                if (gumpId >= 60000)
                {
                    jumpToMaleFemale.Text = "跳转到男性";
                    jumpToMaleFemale.Enabled = listBox.Items.Cast<object>().Any(id =>
                    {
                        if (int.TryParse(id.ToString(), out int intId))
                        {
                            return intId == gumpId - 10000;
                        }
                        return false;
                    });
                }
                else
                {
                    jumpToMaleFemale.Text = "跳转到女性";
                    jumpToMaleFemale.Enabled = listBox.Items.Cast<object>().Any(id =>
                    {
                        if (int.TryParse(id.ToString(), out int intId))
                        {
                            return intId == gumpId + 10000;
                        }
                        return false;
                    });
                }
            }
            else
            {
                jumpToMaleFemale.Enabled = false;
                jumpToMaleFemale.Text = "跳转到男性/女性";
            }
        }
        #endregion        

        #region [ SearchWrapper ]
        public bool SearchWrapper(int id)
        {
            return Search(id.ToString());
        }
        #endregion

        #region [ Search Click ]
        private void Search_Click(object sender, EventArgs e)
        {
            if (_showForm?.IsDisposed == false)
            {
                return;
            }

            _showForm = new GumpSearchForm(SearchWrapper) { TopMost = true };
            _showForm.Show();
        }
        #endregion

        #region [ Search ]
        public bool Search(string searchQuery)
        {
            if (!_refMarker._loaded)
            {
                _refMarker.OnLoad(EventArgs.Empty);
            }

            string searchQueryLower = searchQuery.ToLower();

            for (int i = 0; i < _refMarker.listBox.Items.Count; ++i)
            {
                string itemString = _refMarker.listBox.Items[i].ToString();
                string[] parts = itemString.Split('-');
                string idString = parts[0].Trim();
                string nameString = parts.Length > 1 ? parts[1].Trim() : "";

                // 检查 ID 或名称是否与搜索查询匹配
                if (idString.ToLower() == searchQueryLower || nameString.ToLower().Contains(searchQueryLower))
                {
                    _refMarker.listBox.SelectedIndex = i;
                    _refMarker.listBox.TopIndex = i;
                    return true;
                }

                // 检查十六进制地址是否与搜索查询匹配
                if (Int32.TryParse(idString, out int id))
                {
                    string hexId = id.ToString("x");
                    if ("0x" + hexId.ToLower() == searchQueryLower)
                    {
                        _refMarker.listBox.SelectedIndex = i;
                        _refMarker.listBox.TopIndex = i;
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion

        #region [ Gump_KeyUp ]
        private void Gump_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.F || !e.Control)
            {
                return;
            }

            Search_Click(sender, e);
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
        #endregion

        #region [ InsertStartingFromTb_KeyDown ]]
        private void InsertStartingFromTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (!Int32.TryParse(InsertStartingFromTb.Text, out int index))
            {
                // 转换失败，在此进行错误处理
                MessageBox.Show("请输入有效的整数。");
                return;
            }

            contextMenuStrip.Close();
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Title = $"选择要在 0x{index:X} 处插入的图像文件";
                dialog.CheckFileExists = true;
                dialog.Filter = "图像文件 (*.tif;*.tiff;*.bmp;*.png)|*.tif;*.tiff;*.bmp;*.png";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var fileCount = dialog.FileNames.Length;
                if (CheckForIndexes(index, fileCount))
                {
                    for (int i = 0; i < fileCount; i++)
                    {
                        var currentIdx = index + i;
                        AddSingleGump(dialog.FileNames[i], currentIdx);
                    }

                    Search((index + (fileCount - 1)).ToString());

                }
            }

            Options.ChangedUltimaClass["Gumps"] = true;
        }
        #endregion

        #region [ CheckForIndexes ]
        /// <summary>
        /// 检查从 baseIndex 到 baseIndex + count 的所有索引是否有效
        /// </summary>
        /// <param name="baseIndex">起始索引</param>
        /// <param name="count">要检查的索引数量。</param>
        /// <returns></returns>

        private bool CheckForIndexes(int baseIndex, int count)
        {
            for (int i = baseIndex; i < baseIndex + count; i++)
            {
                if (i >= Gumps.GetCount() || Gumps.IsValidIndex(i))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region [ AddSingleGump ]

        /// <summary>
        /// 添加单个 Gump。
        /// </summary>
        /// <param name="fileName">要添加的 gump 的文件名</param>
        /// <param name="index">要添加 gump 的索引位置。</param>
        private void AddSingleGump(string fileName, int index)
        {
            using (var bmpTemp = new Bitmap(fileName))
            {
                Bitmap bitmap = new Bitmap(bmpTemp);
                if (fileName.Contains(".bmp"))
                {
                    bitmap = Utils.ConvertBmp(bitmap);
                }
                Gumps.ReplaceGump(index, bitmap);
                ControlEvents.FireGumpChangeEvent(this, index);
                bool done = false;
                for (int i = 0; i < listBox.Items.Count; ++i)
                {
                    string itemString = listBox.Items[i].ToString();
                    string idString = itemString.Split('-')[0].Trim();
                    if (!Int32.TryParse(idString, out int j))
                    {
                        continue;
                    }
                    if (j > index)
                    {
                        listBox.Items.Insert(i, index);
                        listBox.SelectedIndex = i;
                        done = true;
                        break;
                    }
                    if (!_showFreeSlots)
                    {
                        continue;
                    }
                    if (j != i)
                    {
                        continue;
                    }
                    done = true;
                    break;
                }
                if (!done)
                {
                    listBox.Items.Add(index);
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                }
            }
        }
        #endregion

        #region [ CopyToolStripMenuItem ]
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                string itemString = listBox.Items[listBox.SelectedIndex].ToString();
                string idString = itemString.Split('-')[0].Trim();
                if (Int32.TryParse(idString, out int i) && Gumps.IsValidIndex(i))
                {
                    Bitmap originalBmp = Gumps.GetGump(i);
                    if (originalBmp != null)
                    {
                        // 制作原始图像的副本
                        Bitmap bmp = new Bitmap(originalBmp);

                        // 内置颜色更改功能
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            for (int x = 0; x < bmp.Width; x++)
                            {
                                Color pixelColor = bmp.GetPixel(x, y);
                                if (pixelColor.R == 211 && pixelColor.G == 211 && pixelColor.B == 211) // 检查像素颜色是否为 #D3D3D3
                                {
                                    bmp.SetPixel(x, y, Color.Black); // 将像素颜色更改为黑色
                                }
                            }
                        }

                        // 将图像转换为 24 位颜色深度
                        Bitmap bmp24bit = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        using (Graphics g = Graphics.FromImage(bmp24bit))
                        {
                            g.DrawImage(bmp, new Rectangle(0, 0, bmp24bit.Width, bmp24bit.Height));
                        }

                        // 将图形复制到剪贴板
                        Clipboard.SetImage(bmp24bit);

                        // 如果 isSoundMessageActive 为 true，则播放声音
                        if (isSoundMessageActive)
                        {
                            if (playCustomSound)
                            {
                                player.Play();
                            }
                            else
                            {
                                player.SoundLocation = "sound.wav";
                                player.Play();
                            }
                        }

                        // 仅在 isSoundMessageActive 为 false 时显示消息
                        if (!isSoundMessageActive)
                        {
                            MessageBox.Show("图像已复制到剪贴板！");
                        }
                    }
                    else
                    {
                        // 仅在 isSoundMessageActive 为 false 时显示消息
                        if (!isSoundMessageActive)
                        {
                            MessageBox.Show("没有可复制的图像！");
                        }
                    }
                }
                else
                {
                    // 仅在 isSoundMessageActive 为 false 时显示消息
                    if (!isSoundMessageActive)
                    {
                        MessageBox.Show("没有可复制的图像！");
                    }
                }
            }
            else
            {
                // 仅在 isSoundMessageActive 为 false 时显示消息
                if (!isSoundMessageActive)
                {
                    MessageBox.Show("没有可复制的图像！");
                }
            }
        }
        #endregion

        #region [ ImportToolStripMenuItem = Import Import clipboard - Import graphics from clipboard ]
        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查剪贴板是否包含图像
            if (Clipboard.ContainsImage())
            {
                // 从剪贴板检索图像
                using (Bitmap bmp = new Bitmap(Clipboard.GetImage()))
                {
                    // 确定所选图形在 listBox 中的位置。
                    string itemString = listBox.Items[listBox.SelectedIndex].ToString();
                    string idString = itemString.Split('-')[0].Trim();
                    if (Int32.TryParse(idString, out int index) && index >= 0 && index < Gumps.GetCount())
                    {
                        // 创建一个与剪贴板图像大小相同的新位图
                        Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);

                        // 定义要忽略的颜色
                        Color[] colorsToIgnore = new Color[]
                        {
                    Color.FromArgb(211, 211, 211), // #D3D3D3
                    Color.FromArgb(0, 0, 0),       // #000000
                    Color.FromArgb(255, 255, 255)  // #FFFFFF
                        };

                        // 遍历图像的每个像素
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            for (int y = 0; y < bmp.Height; y++)
                            {
                                // 获取当前像素的颜色
                                Color pixelColor = bmp.GetPixel(x, y);

                                // 检查当前像素颜色是否属于要忽略的颜色之一
                                if (colorsToIgnore.Contains(pixelColor))
                                {
                                    // 将当前像素颜色设置为透明
                                    newBmp.SetPixel(x, y, Color.Transparent);
                                }
                                else
                                {
                                    // 将当前像素颜色设置为原始图像的颜色
                                    newBmp.SetPixel(x, y, pixelColor);
                                }
                            }
                        }

                        // 使用选定的图形 ID 和新位图调用 ReplaceGump 方法
                        Gumps.ReplaceGump(index, newBmp);
                        ControlEvents.FireGumpChangeEvent(this, index);

                        listBox.Invalidate();
                        ListBox_SelectedIndexChanged(this, EventArgs.Empty);

                        Options.ChangedUltimaClass["Gumps"] = true;

                        // 如果 isSoundMessageActive 为 true，则播放声音
                        if (isSoundMessageActive)
                        {
                            if (playCustomSound)
                            {
                                player.Play();
                            }
                            else
                            {
                                player.SoundLocation = "sound.wav";
                                player.Play();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("无效索引。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("剪贴板中没有图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 导入和导出 Ctrl+V 和 Ctrl+X
        private void GumpControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 检查是否按下了 Ctrl+V 组合键
            if (e.Control && e.KeyCode == Keys.V)
            {
                // 调用 importToolStripMenuItem_Click 方法从剪贴板导入图形。
                ImportToolStripMenuItem_Click(sender, e);
            }
            // 检查是否按下了 Ctrl+X 组合键
            else if (e.Control && e.KeyCode == Keys.X)
            {
                // 调用 copyToolStripMenuItem_Click 方法从剪贴板导入图形。
                CopyToolStripMenuItem_Click(sender, e);
            }
        }
        #endregion

        #region [ SearchByIdToolStripTextBox_KeyUp ]
        private void SearchByIdToolStripTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var searchQuery = searchByIdToolStripTextBox.Text;
            Search(searchQuery);
            if (e.KeyCode == Keys.Return)
            {
                _refMarker.listBox.Focus();
            }
        }

        #endregion

        #region [ AddIDNamesToolStripMenuItem = Add Id Names Form ]
        private void AddIDNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建一个新窗体
            Form form = new Form
            {
                Width = 300,
                Height = 200,
                Text = "添加 ID 名称"
            };

            // 为 ID 创建一个 TextBox
            TextBox idTextBox = new TextBox
            {
                Location = new Point(10, 10),
                Width = 200,
                Text = listBox.SelectedItem?.ToString().Split('-')[0].Trim() ?? "" // 将文本设置为 ListBox 中选定的 ID
            };
            // 添加事件处理程序以忽略非数字输入
            idTextBox.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
            form.Controls.Add(idTextBox);

            // 为名称创建一个 TextBox
            TextBox nameTextBox = new TextBox
            {
                Location = new Point(10, 40),
                Width = 200,
                Text = idNames.TryGetValue(idTextBox.Text, out string existingName) ? existingName : "" // 如果存在现有名称，则将其设置为文本
            };
            form.Controls.Add(nameTextBox);

            // 创建一个“确定”按钮
            Button okButton = new Button
            {
                Text = "确定",
                Location = new Point(10, 70)
            };

            okButton.Click += (sender, e) =>
            {
                string id = idTextBox.Text;
                string name = nameTextBox.Text;

                // 更新 XML 文件
                UpdateIdNameInXml(id, name);

                // 更新 ListBox
                PopulateListBox(true);

                form.Close();
            };

            form.Controls.Add(okButton);

            // 创建一个删除按钮
            Button deleteButton = new Button
            {
                Text = "删除",
                Location = new Point(okButton.Location.X + okButton.Width + 3, 70) // 将按钮放置在“确定”按钮的右侧
            };

            deleteButton.Click += (sender, e) =>
            {
                string id = idTextBox.Text;

                // 从 XML 文件中删除条目
                UpdateIdNameInXml(id, "");

                form.Close();
            };

            form.Controls.Add(deleteButton);

            // 当窗体显示时选中 nameTextBox
            form.Shown += (sender, e) => nameTextBox.Select();

            // 显示窗体
            form.ShowDialog();
        }
        #endregion

        #region [ UpdateIdNameInXml ]
        private void UpdateIdNameInXml(string id, string name)
        {
            // XML 文件路径
            string xmlFilePath = Path.Combine(Application.StartupPath, "IDGumpNames.xml");

            XDocument doc = XDocument.Load(xmlFilePath);

            XElement idElement = doc.Root.Elements("ID")
                                        .FirstOrDefault(el => el.Attribute("value").Value == id);

            if (idElement != null)
            {
                idElement.Attribute("name").Value = name;
            }
            else
            {
                XElement newIdElement = new XElement("ID",
                    new XAttribute("value", id),
                    new XAttribute("name", name));

                doc.Root.Add(newIdElement);
            }

            // 将更改保存到 XML 文件
            doc.Save(xmlFilePath);

            // 更新字典
            idNames[id] = name;
        }
        #endregion

        #region [ Sound Button ]
        private bool isSoundMessageActive = false;
        private bool playCustomSound = false; // 您可以使用它来选择任何声音
        private SoundPlayer player = new SoundPlayer();

        public void ToolStripButtonSoundMessage_Click(object sender, EventArgs e)
        {
            // 切换 isSoundMessageActive 的状态
            isSoundMessageActive = !isSoundMessageActive;

            // 根据状态更改按钮的背景颜色
            if (isSoundMessageActive)
            {
                // 当按钮激活时，将背景颜色更改为蓝色
                toolStripButtonSoundMessage.BackColor = Color.Blue;

                // 播放声音
                if (playCustomSound)
                {
                    player.Play();
                }
                else
                {
                    player.SoundLocation = "sound.wav";
                    player.Play();
                }
            }
            else
            {
                // 当按钮未激活时，将背景颜色更改为默认值
                toolStripButtonSoundMessage.BackColor = default(Color);
            }
        }
        #endregion

        #region [ Mark ]
        private string lastSelectedId = "50320"; // 默认值

        #region [ ListBox_MouseDouvleClick ]
        private void ListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 双击 ListBox 中的项目时，保存选定的 ID 值
            string itemString = listBox.SelectedItem.ToString();
            lastSelectedId = itemString.Split('-')[0].Trim();

            if (isSoundMessageActive)
            {
                if (playCustomSound)
                {
                    player.Play();
                }
                else
                {
                    player.SoundLocation = "sound.wav";
                    player.Play();
                }
            }
        }
        #endregion

        #region [ MarkToolStripMenuItem_Click ]
        private void MarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox.Items.Count; ++i)
            {
                string itemString = listBox.Items[i].ToString();
                string idString = itemString.Split('-')[0].Trim();

                if (idString == lastSelectedId)
                {
                    listBox.SelectedIndex = i;
                    break;
                }
            }

            if (isSoundMessageActive)
            {
                if (playCustomSound)
                {
                    player.Play();
                }
                else
                {
                    player.SoundLocation = "sound.wav";
                    player.Play();
                }
            }
        }
        #endregion

        #endregion

        #region [ CoustomSound ]
        private void CustomSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "波形声音文件 (*.wav)|*.wav";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                player.SoundLocation = openFileDialog.FileName;
                playCustomSound = true;
            }
        }
        #endregion

        #region [ ListBox_PreviewKeyDown ] // Pagedown and up 
        private void ListBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Next:  // PageDown
                case Keys.Prior: // PageUp
                    e.IsInputKey = true;
                    break;
            }
        }
        #endregion

        #region [ ListBox_KeyDown ] // second keydown as a replacement
        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            int visibleItems = listBox.ClientSize.Height / listBox.ItemHeight;

            switch (e.KeyCode)
            {
                case Keys.Next:  // PageDown
                    if (listBox.SelectedIndex + visibleItems < listBox.Items.Count)
                    {
                        listBox.SelectedIndex += visibleItems;
                    }
                    else
                    {
                        listBox.SelectedIndex = listBox.Items.Count - 1;
                    }
                    break;

                case Keys.Prior: // PageUp
                    if (listBox.SelectedIndex - visibleItems >= 0)
                    {
                        listBox.SelectedIndex -= visibleItems;
                    }
                    else
                    {
                        listBox.SelectedIndex = 0;
                    }
                    break;
            }
        }
        #endregion

        #region [ ExportAllIDsToTextToolStripMenuItem_Click ]
        private void ExportAllIDsToTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportAllGumpIDs();
        }
        #endregion

        #region [ ExportAllGumpIDs ]
        private void ExportAllGumpIDs()
        {
            var format = MessageBox.Show("您要以什么格式导出？十进制？", "选择格式", MessageBoxButtons.YesNo) == DialogResult.Yes ? "Decimal" : "Hex";
            var includeFree = MessageBox.Show("是否也要导出空闲 ID？", "包含空闲 ID", MessageBoxButtons.YesNo) == DialogResult.Yes;

            // 使用 "GumpExport" 和当前日期创建默认文件名
            string defaultFileName = $"GumpExport_{DateTime.Now:yyyyMMdd}.txt";

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "文本文件 (*.txt)|*.txt",
                Title = "Gump ID 保存",
                FileName = defaultFileName  // 设置默认文件名
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    int maxGumpID = Gumps.GetCount();  // 从 Gump mul 文件中获取最大 Gump ID
                    for (int gumpId = 0; gumpId < maxGumpID; gumpId++)
                    {
                        bool isOccupied = Gumps.IsValidIndex(gumpId);  // 检查 Gump 条目是否被占用

                        // 如果用户不想包含空闲 ID，则跳过空闲 ID
                        if (!isOccupied && !includeFree)
                            continue;

                        // 将 ID 转换为所需格式（十进制或十六进制）
                        string formattedID = format == "Decimal" ? gumpId.ToString() : $"0x{gumpId:X}";
                        writer.WriteLine($"{formattedID} - {(isOccupied ? "Gump 图像" : "空闲")}");
                    }
                }

                MessageBox.Show("导出成功完成！", "导出状态", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region [ ImportAllImagesFromTextToolStripMenuItem ] 
        private void ImportAllImagesFromTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "选择包含文本文件和 Gump 图像的目录";

                if (folderDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string selectedDirectory = folderDialog.SelectedPath;

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "文本文件 (*.txt)|*.txt",
                    Title = "选择包含 Gump 地址的文本文件",
                    InitialDirectory = selectedDirectory
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string textFilePath = openFileDialog.FileName;

                // 读取文本文件
                string[] lines = File.ReadAllLines(textFilePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('-');
                    if (parts.Length < 1)
                    {
                        continue;
                    }

                    string gumpAddress = parts[0].Trim();
                    int gumpId;

                    // 识别十六进制或十进制
                    if (gumpAddress.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!int.TryParse(gumpAddress.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out gumpId))
                        {
                            MessageBox.Show($"无效的十六进制地址: {gumpAddress}");
                            continue;
                        }
                    }
                    else if (!int.TryParse(gumpAddress, out gumpId))
                    {
                        MessageBox.Show($"无效的十进制地址: {gumpAddress}");
                        continue;
                    }

                    string imagePath = FindImageForGumpId(selectedDirectory, gumpAddress);
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        MessageBox.Show($"未找到 Gump ID 的图像: {gumpAddress}");
                        continue;
                    }

                    // 如果需要，转换为 BMP
                    using (var bmpTemp = new Bitmap(imagePath))
                    {
                        Bitmap bitmap = new Bitmap(bmpTemp);

                        if (imagePath.Contains(".bmp"))
                        {
                            bitmap = Utils.ConvertBmp(bitmap);
                        }

                        // 将图像导入到 Gump ID
                        Gumps.ReplaceGump(gumpId, bitmap);

                        // 触发 GumpChangeEvent 以确保 ListBox 更新
                        ControlEvents.FireGumpChangeEvent(this, gumpId);

                        // 更新 ListBox
                        UpdateListBoxWithGump(gumpId);
                    }
                }

                PopulateListBox(false); // 导入后重新加载 ListBox

                // 强制重绘 ListBox
                listBox.Invalidate();
                MessageBox.Show("导入完成！", "导入状态", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region [ FindImageForGumpId ]
        // 图像搜索的辅助方法
        private string FindImageForGumpId(string directory, string gumpAddress)
        {
            string[] imageFormats = { "bmp", "png", "jpg", "jpeg", "tif", "tiff" };
            foreach (var format in imageFormats)
            {
                string filePath = Path.Combine(directory, $"{gumpAddress}.{format}");
                if (File.Exists(filePath))
                {
                    return filePath;
                }
            }

            return null;
        }
        #endregion

        #region [ UpdateListBoxWithGump ]
        // ListBox 更新
        private void UpdateListBoxWithGump(int gumpId)
        {
            bool alreadyExists = false;
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                string item = listBox.Items[i].ToString().Split('-')[0].Trim();
                if (int.TryParse(item, System.Globalization.NumberStyles.HexNumber, null, out int listGumpId) && listGumpId == gumpId)
                {
                    listBox.Items[i] = $"0x{gumpId:X} - 已替换为新 Gump";
                    alreadyExists = true;
                    break;
                }
            }

            if (!alreadyExists)
            {
                listBox.Items.Add($"0x{gumpId:X} - 已替换为新 Gump");
            }

            listBox.Refresh();
        }
        #endregion

        #region [ MirrorToolStripMenuItem_Click ]
        private void MirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox.BackgroundImage == null)
            {
                MessageBox.Show("没有可用于镜像的图像。");
                return;
            }

            Bitmap originalImage = new Bitmap(pictureBox.BackgroundImage);

            Bitmap mirroredBitmap = new Bitmap(originalImage.Width, originalImage.Height);

            using (Graphics g = Graphics.FromImage(mirroredBitmap))
            {
                g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                    new Rectangle(originalImage.Width, 0, -originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);
            }

            pictureBox.BackgroundImage = mirroredBitmap;
        }
        #endregion

        #region [ ListingToolStripMenuItem_Click ]
        private void ListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addressListForm == null || addressListForm.IsDisposed)
            {
                addressListForm = new AddressListForm();
                addressListForm.AddressSelected += AddressListForm_AddressSelected;
                addressListForm.Show();
            }
        }
        #endregion

        #region [ AddressListForm_AddressSelected ]
        private void AddressListForm_AddressSelected(object sender, string address)
        {
            SelectAddress(address);
        }
        #endregion

        #region [ class AddressListForm : Form]
        public class AddressListForm : Form
        {
            private ListBox addressListBox;
            public event EventHandler<string> AddressSelected;

            public AddressListForm()
            {
                Initialize();
            }

            private void Initialize()
            {
                addressListBox = new ListBox() { Dock = DockStyle.Fill };
                addressListBox.MouseDoubleClick += (sender, e) =>
                {
                    if (addressListBox.SelectedItem != null)
                    {
                        string selectedAddress = addressListBox.SelectedItem.ToString();
                        AddressSelected?.Invoke(this, selectedAddress);
                    }
                };
                Controls.Add(addressListBox);
            }

            public void AddAddress(string address)
            {
                if (!addressListBox.Items.Contains(address))
                {
                    addressListBox.Items.Add(address);
                }
            }
        }
        #endregion
    }
}