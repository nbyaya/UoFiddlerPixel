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

#nullable enable annotations

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ultima;
using System.Media;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Forms;
using UoFiddler.Controls.Helpers;
using UoFiddler.Controls.UserControls.TileView;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace UoFiddler.Controls.UserControls
{
    public partial class ItemsControl : UserControl
    {
        private TileDataControl tileDataControl = new TileDataControl(); // 刷新图像 pictureBoxItem TiledataControl

        private int occupiedItemCount = 0; // 物品计数器
        private bool isDrawingRhombusActive = false; // 绘制菱形

        private Form imageForm; // DetailPictureBox_MouseDoubleClick
        private PictureBox imagePictureBox; // DetailPictureBox_MouseDoubleClick

        public ItemsControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            RefMarker = this;
            DetailTextBox.AddBasicContextMenu();
        }

        private List<int> _itemList = new List<int>();
        private bool _showFreeSlots;

        private int _selectedGraphicId = -1;

        #region [ SelectedGraphicId ]
        public int SelectedGraphicId
        {
            get => _selectedGraphicId;
            set
            {
                _selectedGraphicId = value < 0 ? 0 : value;
                ItemsTileView.FocusIndex = _itemList.Count == 0 ? -1 : _itemList.IndexOf(_selectedGraphicId);

                UpdateToolStripLabels(_selectedGraphicId);
                UpdateDetail(_selectedGraphicId);
            }
        }
        #endregion

        public IReadOnlyList<int> ItemList { get => _itemList.AsReadOnly(); }
        public static ItemsControl RefMarker { get; private set; }
        public static TileViewControl TileView => RefMarker.ItemsTileView;
        public bool IsLoaded { get; private set; }

        #region [ UpdateTileView ]
        /// <summary>
        /// 当 TileSize 更改时更新
        /// </summary>
        /// 
        public void UpdateTileView()
        {
            var newSize = new Size(Options.ArtItemSizeWidth, Options.ArtItemSizeHeight);

            ItemsTileView.TileBorderColor = Options.RemoveTileBorder
                ? Color.Transparent
                : Color.Gray;

            if (Options.OverrideBackgroundColorFromTile)
            {
                ItemsTileView.BackColor = _backgroundColorItem;
            }

            var sameTileSize = ItemsTileView.TileSize == newSize;
            var sameFocusColor = ItemsTileView.TileFocusColor == Options.TileFocusColor;
            var sameSelectionColor = ItemsTileView.TileHighlightColor == Options.TileSelectionColor;
            if (sameTileSize && sameFocusColor && sameSelectionColor)
            {
                return;
            }

            ItemsTileView.TileFocusColor = Options.TileFocusColor;
            ItemsTileView.TileHighlightColor = Options.TileSelectionColor;

            ItemsTileView.TileSize = newSize;
            ItemsTileView.Invalidate();

            if (_selectedGraphicId != -1)
            {
                UpdateDetail(_selectedGraphicId);
            }
        }
        #endregion

        #region [ SearchGraphic ]
        /// <summary>
        /// 搜索图形编号并选择它
        /// </summary>
        /// <param name="graphic"></param>
        /// <returns></returns>
        /// 
        public static bool SearchGraphic(int graphic)
        {
            if (!RefMarker.IsLoaded)
            {
                RefMarker.OnLoad(RefMarker, EventArgs.Empty);
            }

            if (RefMarker._itemList.All(t => t != graphic))
            {
                return false;
            }

            // 我们必须使焦点无效，以便滚动到项目
            RefMarker.ItemsTileView.FocusIndex = -1;
            RefMarker.SelectedGraphicId = graphic;

            return true;
        }
        #endregion

        #region [ SearchName ]
        /// <summary>
        /// 搜索名称并选择
        /// </summary>
        /// <param name="name"></param>
        /// <param name="next">从当前选中的开始</param>
        /// <returns></returns>
        /// 
        public static bool SearchName(string name, bool next)
        {
            int index = 0;
            if (next)
            {
                if (RefMarker._selectedGraphicId >= 0)
                {
                    index = RefMarker._itemList.IndexOf(RefMarker._selectedGraphicId) + 1;
                }

                if (index >= RefMarker._itemList.Count)
                {
                    index = 0;
                }
            }

            var searchMethod = SearchHelper.GetSearchMethod();

            for (int i = index; i < RefMarker._itemList.Count; ++i)
            {
                var searchResult = searchMethod(name, TileData.ItemTable[RefMarker._itemList[i]].Name);
                if (searchResult.HasErrors)
                {
                    break;
                }

                if (!searchResult.EntryFound)
                {
                    continue;
                }

                // 我们必须使焦点无效，以便滚动到项目
                RefMarker.ItemsTileView.FocusIndex = -1;
                RefMarker.SelectedGraphicId = RefMarker._itemList[i];

                return true;
            }

            return false;
        }
        #endregion

        #region [ OnLoad ]
        public void OnLoad(object sender, EventArgs e)
        {
            if (IsAncestorSiteInDesignMode || FormsDesignerHelper.IsInDesignMode())
            {
                return;
            }

            if (IsLoaded && (!(e is MyEventArgs args) || args.Type != MyEventArgs.Types.ForceReload))
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            Options.LoadedUltimaClass["TileData"] = true;
            Options.LoadedUltimaClass["Art"] = true;
            Options.LoadedUltimaClass["Animdata"] = true;
            Options.LoadedUltimaClass["Hues"] = true;

            if (!IsLoaded) // 仅一次
            {
                Plugin.PluginEvents.FireModifyItemShowContextMenuEvent(TileViewContextMenuStrip);
            }

            UpdateTileView();

            _showFreeSlots = false;
            showFreeSlotsToolStripMenuItem.Checked = false;

            var prevSelected = SelectedGraphicId;

            int staticLength = Art.GetMaxItemId();
            _itemList = new List<int>(staticLength);
            for (int i = 0; i <= staticLength; ++i)
            {
                if (Art.IsValidStatic(i))
                {
                    _itemList.Add(i);
                }
            }

            ItemsTileView.VirtualListSize = _itemList.Count;

            if (prevSelected >= 0)
            {
                SelectedGraphicId = _itemList.Contains(prevSelected) ? prevSelected : 0;
            }

            if (!IsLoaded)
            {
                ControlEvents.FilePathChangeEvent += OnFilePathChangeEvent;
                ControlEvents.ItemChangeEvent += OnItemChangeEvent;
                ControlEvents.TileDataChangeEvent += OnTileDataChangeEvent;
            }

            IsLoaded = true;
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region [ Reload ]
        /// <summary>
        /// 如果已加载则重新加载
        /// </summary>
        /// 
        private void Reload()
        {
            if (IsLoaded)
            {
                OnLoad(this, new MyEventArgs(MyEventArgs.Types.ForceReload));
            }
        }
        #endregion

        #region [ OnFilePathChangeEvent ]
        private void OnFilePathChangeEvent()
        {
            Reload();
        }
        #endregion

        #region [ OnTileDataChangeEvent ]
        private void OnTileDataChangeEvent(object sender, int id)
        {
            if (!IsLoaded)
            {
                return;
            }

            if (sender.Equals(this))
            {
                return;
            }

            if (id < 0x4000)
            {
                return;
            }

            id -= 0x4000;

            if (_selectedGraphicId != id)
            {
                return;
            }

            UpdateToolStripLabels(id);
            UpdateDetail(id);
        }
        #endregion

        #region [ OnItemChangeEvent ]
        private void OnItemChangeEvent(object sender, int index)
        {
            if (!IsLoaded)
            {
                return;
            }

            if (sender.Equals(this))
            {
                return;
            }

            if (Art.IsValidStatic(index))
            {
                bool done = false;
                for (int i = 0; i < _itemList.Count; ++i)
                {
                    if (index < _itemList[i])
                    {
                        _itemList.Insert(i, index);
                        done = true;
                        break;
                    }

                    if (index != _itemList[i])
                    {
                        continue;
                    }

                    done = true;
                    break;
                }

                if (!done)
                {
                    _itemList.Add(index);
                }
            }
            else
            {
                if (_showFreeSlots)
                {
                    return;
                }

                _itemList.Remove(index);
            }

            ItemsTileView.VirtualListSize = _itemList.Count;
            ItemsTileView.Invalidate();
        }
        #endregion

        #region [ ChangeBackgroundColorToolStripMenuItem ]

        private Color _backgroundColorItem = Color.White;
        private void ChangeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _backgroundColorItem = colorDialog.Color;

            if (Options.OverrideBackgroundColorFromTile)
            {
                ItemsTileView.BackColor = _backgroundColorItem;
            }

            ItemsTileView.Invalidate();
        }
        #endregion

        #region [ UpdateDetail ]

        private Color _backgroundDetailColor = Color.White;
        private void UpdateDetail(int graphic)
        {
            if (IsAncestorSiteInDesignMode || FormsDesignerHelper.IsInDesignMode())
            {
                return;
            }

            if (!IsLoaded)
            {
                return;
            }

            if (_scrolling)
            {
                return;
            }

            ItemData item = TileData.ItemTable[graphic];
            Bitmap bit = Art.GetStatic(graphic);

            int xMin = 0;
            int xMax = 0;
            int yMin = 0;
            int yMax = 0;

            const int defaultSplitterDistance = 180;
            if (bit == null)
            {
                splitContainer2.SplitterDistance = defaultSplitterDistance;
                Bitmap newBit = new Bitmap(DetailPictureBox.Size.Width, DetailPictureBox.Size.Height);
                using (Graphics newGraph = Graphics.FromImage(newBit))
                {
                    newGraph.Clear(_backgroundDetailColor);
                }

                DetailPictureBox.Image?.Dispose();
                DetailPictureBox.Image = newBit;
            }
            else
            {
                var distance = bit.Size.Height + 10;
                splitContainer2.SplitterDistance = distance < defaultSplitterDistance ? defaultSplitterDistance : distance;

                Bitmap newBit = new Bitmap(DetailPictureBox.Size.Width, DetailPictureBox.Size.Height);
                using (Graphics newGraph = Graphics.FromImage(newBit))
                {
                    newGraph.Clear(_backgroundDetailColor);
                    newGraph.DrawImage(bit, (DetailPictureBox.Size.Width - bit.Width) / 2, 5);
                }

                DetailPictureBox.Image?.Dispose();
                DetailPictureBox.Image = newBit;

                Art.Measure(bit, out xMin, out yMin, out xMax, out yMax);
            }

            var sb = new StringBuilder();
            sb.AppendLine($"名称: {item.Name}");
            sb.AppendLine($"图形: 0x{graphic:X4}");
            sb.AppendLine($"十进制 ID: {graphic}");   // 新增行
            sb.AppendLine($"高度/容量: {item.Height}");
            sb.AppendLine($"重量: {item.Weight}");
            sb.AppendLine($"动画: {item.Animation}");
            sb.AppendLine($"质量/层/光照: {item.Quality}");
            sb.AppendLine($"数量: {item.Quantity}");
            sb.AppendLine($"色调: {item.Hue}");
            sb.AppendLine($"堆叠偏移/未知4: {item.StackingOffset}");
            sb.AppendLine($"标志: {item.Flags}");
            sb.AppendLine($"图形像素尺寸 宽度, 高度: {bit?.Width ?? 0} {bit?.Height ?? 0} ");
            sb.AppendLine($"图形像素偏移 xMin, yMin, xMax, yMax: {xMin} {yMin} {xMax} {yMax}");

            if ((item.Flags & TileFlag.Animation) != 0)
            {
                Animdata.AnimdataEntry info = Animdata.GetAnimData(graphic);
                if (info != null)
                {
                    sb.AppendLine($"动画帧数: {info.FrameCount} 间隔: {info.FrameInterval}");
                }
            }

            DetailTextBox.Clear();
            DetailTextBox.AppendText(sb.ToString());

            // 如果复选框被选中，则应用颜色更改 = 粒子灰度
            InsertNewImage((Image)DetailPictureBox.Image);
        }
        #endregion

        #region [ ChangeBackgroundColorToolStripMenuItemDetail ]
        private void ChangeBackgroundColorToolStripMenuItemDetail_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _backgroundDetailColor = colorDialog.Color;
            if (_selectedGraphicId != -1)
            {
                UpdateDetail(_selectedGraphicId);
            }
        }
        #endregion

        #region [ OnSearchClick ]
        private ItemSearchForm _showForm;
        private bool _scrolling;

        private void OnSearchClick(object sender, EventArgs e)
        {
            if (_showForm?.IsDisposed == false)
            {
                return;
            }

            _showForm = new ItemSearchForm(SearchGraphic, SearchName)
            {
                TopMost = true
            };
            _showForm.Show();
        }
        #endregion

        #region [ OnClickFindFree ]
        private void OnClickFindFree(object sender, EventArgs e)
        {
            if (_showFreeSlots)
            {
                int i = _selectedGraphicId > -1 ? _itemList.IndexOf(_selectedGraphicId) + 1 : 0;
                for (; i < _itemList.Count; ++i)
                {
                    if (Art.IsValidStatic(_itemList[i]))
                    {
                        continue;
                    }

                    SelectedGraphicId = _itemList[i];
                    ItemsTileView.Invalidate();
                    break;
                }
            }
            else
            {
                int id, i;

                if (_selectedGraphicId > -1)
                {
                    id = _selectedGraphicId + 1;
                    i = _itemList.IndexOf(_selectedGraphicId) + 1;
                }
                else
                {
                    id = 0;
                    i = 0;
                }

                for (; i < _itemList.Count; ++i, ++id)
                {
                    if (id >= _itemList[i])
                    {
                        continue;
                    }

                    SelectedGraphicId = _itemList[i];
                    ItemsTileView.Invalidate();
                    break;
                }
            }
        }
        #endregion

        #region [ OnClickReplace ]
        private void OnClickReplace(object sender, EventArgs e)
        {
            if (_selectedGraphicId < 0)
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

                    Art.ReplaceStatic(_selectedGraphicId, bitmap);

                    ControlEvents.FireItemChangeEvent(this, _selectedGraphicId);

                    ItemsTileView.Invalidate();
                    UpdateToolStripLabels(_selectedGraphicId);
                    UpdateDetail(_selectedGraphicId);

                    Options.ChangedUltimaClass["Art"] = true;
                }
            }
        }
        #endregion

        #region [ OnClickRemove ]
        private void OnClickRemove(object sender, EventArgs e)
        {
            // 检查是否选择了多个艺术品
            if (ItemsTileView.SelectedIndices.Count > 1)
            {
                DialogResult result = MessageBox.Show($"您确定要删除选定的艺术品吗？", "删除艺术品", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // 创建要删除的索引列表
                var indicesToRemove = ItemsTileView.SelectedIndices.Cast<int>().OrderByDescending(i => i).ToList();

                // 遍历所有选定的索引并删除艺术品
                foreach (int selectedIndex in indicesToRemove)
                {
                    int graphicId = _itemList[selectedIndex];
                    if (Art.IsValidStatic(graphicId))
                    {
                        Art.RemoveStatic(graphicId);
                        ControlEvents.FireItemChangeEvent(this, graphicId);
                    }

                    // 从列表中删除
                    _itemList.RemoveAt(selectedIndex);
                }

                // 更新 UI
                ItemsTileView.VirtualListSize = _itemList.Count;
                SelectedGraphicId = _itemList.Count > 0 ? _itemList[0] : 0; // 如果列表为空，则不选择
                ItemsTileView.Invalidate();
            }
            else
            {
                // 删除单个艺术品（原始逻辑）
                if (!Art.IsValidStatic(_selectedGraphicId))
                {
                    return;
                }

                DialogResult result = MessageBox.Show($"您确定要删除艺术品 0x{_selectedGraphicId:X} 吗？", "删除艺术品", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                Art.RemoveStatic(_selectedGraphicId);
                ControlEvents.FireItemChangeEvent(this, _selectedGraphicId);

                if (!_showFreeSlots)
                {
                    _itemList.Remove(_selectedGraphicId);
                    ItemsTileView.VirtualListSize = _itemList.Count;
                    var moveToIndex = --_selectedGraphicId;
                    SelectedGraphicId = moveToIndex <= 0 ? 0 : _selectedGraphicId; // TODO: 最后一个可见索引，而不是仅仅当前减一
                }
                ItemsTileView.Invalidate();
            }

            Options.ChangedUltimaClass["Art"] = true;
        }
        #endregion

        #region [ OnTextChangedInsert ]
        private void OnTextChangedInsert(object sender, EventArgs e)
        {
            if (Utils.ConvertStringToInt(InsertText.Text, out int index, 0, Art.GetMaxItemId()))
            {
                InsertText.ForeColor = Art.IsValidStatic(index) ? Color.Red : Color.Black;
            }
            else
            {
                InsertText.ForeColor = Color.Red;
            }
        }
        #endregion

        #region [ OnKeyDownInsertText ]
        private void OnKeyDownInsertText(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (!Utils.ConvertStringToInt(InsertText.Text, out int index, 0, Art.GetMaxItemId()))
            {
                return;
            }

            if (Art.IsValidStatic(index))
            {
                return;
            }

            TileViewContextMenuStrip.Close();

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = $"选择从 0x{index:X} 开始替换的图像";
                dialog.CheckFileExists = true;
                dialog.Filter = "图像文件 (*.tif;*.tiff;*.bmp;*.png)|*.tif;*.tiff;*.bmp;*.png";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                AddSingleItem(dialog.FileName, index);
            }
        }
        #endregion

        #region [ UpdateToolStripLabels ]
        private void UpdateToolStripLabels(int graphic)
        {
            if (IsAncestorSiteInDesignMode || FormsDesignerHelper.IsInDesignMode())
            {
                return;
            }

            if (!IsLoaded)
            {
                return;
            }

            if (_scrolling)
            {
                return;
            }

            NameLabel.Text = !Art.IsValidStatic(graphic) ? "名称: 空闲" : $"名称: {TileData.ItemTable[graphic].Name}";
            GraphicLabel.Text = $"图形十六进制: 0x{graphic:X4} ";
            toolStripStatusLabelGraficDecimal.Text = $"图形十进制: {graphic}";
        }
        #endregion

        #region [ OnClickSave ]
        /*private void OnClickSave(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定吗？这需要一些时间", "保存", MessageBoxButtons.YesNo,
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
        }*/

        private void OnClickSave(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定吗？这需要一些时间", "保存", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            int totalItems = Art.GetIdxLength();
            var progressBarDialog = new ProgressBarDialog(totalItems, "正在保存 Art...");
            bool isCancelled = false;

            progressBarDialog.CancelClicked += () => isCancelled = true;

            progressBarDialog.Show();

            Task.Run(() =>
            {
                try
                {
                    Art.Save(Options.OutputPath); // 原始保存

                    if (!isCancelled)
                    {
                        progressBarDialog.Invoke((Action)progressBarDialog.Close);
                        Options.ChangedUltimaClass["Art"] = false; // 将更改标记为已保存
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误: {ex.Message}", "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            });
        }
        #endregion

        #region [ OnClickShowFreeSlots ]
        // 此方法是按钮或控件单击的事件处理程序

        private void OnClickShowFreeSlots(object sender, EventArgs e)
        {
            // 切换 _showFreeSlots 变量的值
            _showFreeSlots = !_showFreeSlots;
            // 如果 _showFreeSlots 为 true
            if (_showFreeSlots)
            {
                // 循环遍历最大项目 ID 之前的所有可能项目 ID
                for (int j = 0; j <= Art.GetMaxItemId(); ++j)
                {
                    // 检查项目是否已在 _itemList 中
                    if (_itemList.Count > j)
                    {
                        // 如果项目不在 _itemList 中，则在当前位置插入它
                        if (_itemList[j] != j)
                        {
                            _itemList.Insert(j, j);
                        }
                    }
                    else
                    {
                        // 如果项目不在 _itemList 中，则在当前位置插入它
                        _itemList.Insert(j, j);
                    }
                }

                // 存储先前选定的项目 ID
                var prevSelected = SelectedGraphicId;

                // 更新 ItemsTileView 控件的 VirtualListSize 属性以反映新项目数
                ItemsTileView.VirtualListSize = _itemList.Count;

                // 如果有先前选定的项目，尝试重新选择它
                if (prevSelected >= 0)
                {
                    SelectedGraphicId = prevSelected;
                }

                // 强制 ItemsTileView 控件重绘
                ItemsTileView.Invalidate();
            }
            else
            {
                // 如果 _showFreeSlots 为 false，则调用 Reload 方法
                Reload();
            }
        }
        #endregion

        #region [ Save format ]

        private void Extract_Image_ClickBmp(object sender, EventArgs e)
        {
            // 检查 ItemsTileView 中是否选择了任何项目
            if (ItemsTileView.SelectedIndices.Count == 0)
            {
                return;
            }

            // 遍历选定的索引
            foreach (int selectedIndex in ItemsTileView.SelectedIndices)
            {
                // 获取选定项目的图形
                Bitmap bitmap = Art.GetStatic(_itemList[selectedIndex]);
                // 检查图形是否存在
                if (bitmap != null)
                {
                    // 保存图形
                    ExportItemImage(_itemList[selectedIndex], ImageFormat.Bmp);
                }
            }
        }


        private void Extract_Image_ClickTiff(object sender, EventArgs e)
        {
            // 检查 ItemsTileView 中是否选择了任何项目
            if (ItemsTileView.SelectedIndices.Count == 0)
            {
                return;
            }

            // 遍历选定的索引
            foreach (int selectedIndex in ItemsTileView.SelectedIndices)
            {
                // 获取选定项目的图形
                Bitmap bitmap = Art.GetStatic(_itemList[selectedIndex]);
                // 检查图形是否存在
                if (bitmap != null)
                {
                    // 保存图形
                    ExportItemImage(_itemList[selectedIndex], ImageFormat.Tiff);
                }
            }
        }

        private void Extract_Image_ClickJpg(object sender, EventArgs e)
        {
            // 检查 ItemsTileView 中是否选择了任何项目
            if (ItemsTileView.SelectedIndices.Count == 0)
            {
                return;
            }

            // 遍历选定的索引
            foreach (int selectedIndex in ItemsTileView.SelectedIndices)
            {
                // 获取选定项目的图形
                Bitmap bitmap = Art.GetStatic(_itemList[selectedIndex]);
                // 检查图形是否存在
                if (bitmap != null)
                {
                    // 保存图形
                    ExportItemImage(_itemList[selectedIndex], ImageFormat.Jpeg);
                }
            }
        }

        private void Extract_Image_ClickPng(object sender, EventArgs e)
        {
            // 检查 ItemsTileView 中是否选择了任何项目
            if (ItemsTileView.SelectedIndices.Count == 0)
            {
                return;
            }

            // 遍历选定的索引
            foreach (int selectedIndex in ItemsTileView.SelectedIndices)
            {
                // 获取选定项目的图形
                Bitmap bitmap = Art.GetStatic(_itemList[selectedIndex]);
                // 检查图形是否存在
                if (bitmap != null)
                {
                    // 保存图形
                    ExportItemImage(_itemList[selectedIndex], ImageFormat.Png);
                }
            }
        }

        private static void ExportItemImage(int index, ImageFormat imageFormat)
        {
            if (!Art.IsValidStatic(index))
            {
                return;
            }

            string fileExtension = Utils.GetFileExtensionFor(imageFormat);
            string fileName = Path.Combine(Options.OutputPath, $"Item 0x{index:X4}.{fileExtension}");

            using (Bitmap bit = new Bitmap(Art.GetStatic(index)))
            {
                bit.Save(fileName, imageFormat);
            }

            MessageBox.Show($"项目已保存到 {fileName}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        #endregion

        #region [ OnClickSelectTiledata ]
        private void OnClickSelectTiledata(object sender, EventArgs e)
        {
            if (_selectedGraphicId == -1)
            {
                tileDataControl.RefreshPictureBoxItem(); // 刷新图片框
            }

            if (_selectedGraphicId >= 0)
            {
                TileDataControl.Select(_selectedGraphicId, false);
                //tileDataControl.RefreshPictureBoxItem(); // 选择 pictureBoxItem TileDataControl
            }
        }
        #endregion

        #region [ OnClickSelectRadarCol ]
        private void OnClickSelectRadarCol(object sender, EventArgs e)
        {
            if (_selectedGraphicId >= 0)
            {
                RadarColorControl.Select(_selectedGraphicId, false);
            }
        }
        #endregion

        #region [ Misc Save ] // 以指定图像格式保存所有项目
        private void OnClick_SaveAllBmp(object sender, EventArgs e)
        {
            ExportAllItemImages(ImageFormat.Bmp);
        }

        private void OnClick_SaveAllTiff(object sender, EventArgs e)
        {
            ExportAllItemImages(ImageFormat.Tiff);
        }

        private void OnClick_SaveAllJpg(object sender, EventArgs e)
        {
            ExportAllItemImages(ImageFormat.Jpeg);
        }

        private void OnClick_SaveAllPng(object sender, EventArgs e)
        {
            ExportAllItemImages(ImageFormat.Png);
        }
        #endregion

        // 此方法以指定图像格式导出所有项目图像
        #region [ ExportAllItemImages ]
        private CancellationTokenSource _cancellationTokenSource;

        private void ExportAllItemImages(ImageFormat imageFormat)
        {
            // 获取指定图像格式的文件扩展名
            string fileExtension = Utils.GetFileExtensionFor(imageFormat);

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择目录";
                dialog.ShowNewFolderButton = true;
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _cancellationTokenSource = new CancellationTokenSource(); // 初始化中止令牌

                Cursor.Current = Cursors.WaitCursor;

                int itemsExported = 0; // 导出项目的计数变量

                using (var progressBarDialog = new ProgressBarDialog2(_itemList.Count, $"导出到 {fileExtension}", false))
                {
                    progressBarDialog.CancelClicked += () => _cancellationTokenSource.Cancel(); // 链接取消逻辑

                    try
                    {
                        foreach (var artItemIndex in _itemList)
                        {
                            // 检查终止
                            if (_cancellationTokenSource.Token.IsCancellationRequested || progressBarDialog.IsCancelled)
                            {
                                MessageBox.Show("导出已中止。", "取消", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            // 更新进度条
                            ControlEvents.FireProgressChangeEvent();
                            Application.DoEvents();

                            int index = artItemIndex;
                            if (index < 0)
                            {
                                continue;
                            }

                            // 检索并保存图形
                            var artBitmap = Art.GetStatic(index);
                            if (artBitmap is null)
                            {
                                continue;
                            }

                            string fileName = Path.Combine(dialog.SelectedPath, $"Item 0x{index:X4}.{fileExtension}");
                            using (Bitmap bit = new Bitmap(artBitmap))
                            {
                                bit.Save(fileName, imageFormat);
                            }

                            itemsExported++; // 增加计数器
                        }
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                        progressBarDialog.Dispose();
                    }
                }

                // 最后显示导出的项目数
                MessageBox.Show($"所有项目已保存到 {dialog.SelectedPath}\n导出的项目总数: {itemsExported}", "已保存",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        #endregion        

        #region [ OnClickPreLoad ]
        private void OnClickPreLoad(object sender, EventArgs e)
        {
            if (PreLoader.IsBusy)
            {
                return;
            }

            ProgressBar.Minimum = 1;
            ProgressBar.Maximum = _itemList.Count;
            ProgressBar.Step = 1;
            ProgressBar.Value = 1;
            ProgressBar.Visible = true;
            PreLoader.RunWorkerAsync();
        }
        #endregion

        #region [ PreLoaderDoWork ]
        private void PreLoaderDoWork(object sender, DoWorkEventArgs e)
        {
            foreach (int item in _itemList)
            {
                Art.GetStatic(item);
                PreLoader.ReportProgress(1);
            }
        }
        #endregion

        #region [ PreLoaderProgressChanged ]
        private void PreLoaderProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.PerformStep();
        }
        #endregion

        #region [ PreLoaderCompleted ]
        private void PreLoaderCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Visible = false;
        }
        #endregion

        #region [ ItemsTileView_DrawItem ]
        private void ItemsTileView_DrawItem(object sender, TileViewControl.DrawTileListItemEventArgs e)
        {
            if (IsAncestorSiteInDesignMode || FormsDesignerHelper.IsInDesignMode())
            {
                return;
            }

            Point itemPoint = new Point(e.Bounds.X + ItemsTileView.TilePadding.Left, e.Bounds.Y + ItemsTileView.TilePadding.Top);

            Rectangle rect = new Rectangle(itemPoint, ItemsTileView.TileSize);

            var previousClip = e.Graphics.Clip;

            e.Graphics.Clip = new Region(rect);

            var selected = ItemsTileView.SelectedIndices.Contains(e.Index);
            if (!selected)
            {
                e.Graphics.Clear(_backgroundColorItem);
            }

            var bitmap = Art.GetStatic(_itemList[e.Index], out bool patched);
            if (bitmap == null)
            {
                e.Graphics.Clip = new Region(rect);

                rect.X += 5;
                rect.Y += 5;

                rect.Width -= 10;
                rect.Height -= 10;

                e.Graphics.FillRectangle(Brushes.Red, rect);
                e.Graphics.Clip = previousClip;
            }
            else
            {
                if (patched && !selected)
                {
                    e.Graphics.FillRectangle(Brushes.LightCoral, rect);
                }

                if (Options.ArtItemClip)
                {
                    e.Graphics.DrawImage(bitmap, itemPoint);
                }
                else
                {
                    int width = bitmap.Width;
                    int height = bitmap.Height;
                    if (width > ItemsTileView.TileSize.Width)
                    {
                        width = ItemsTileView.TileSize.Width;
                        height = ItemsTileView.TileSize.Height * bitmap.Height / bitmap.Width;
                    }

                    if (height > ItemsTileView.TileSize.Height)
                    {
                        height = ItemsTileView.TileSize.Height;
                        width = ItemsTileView.TileSize.Width * bitmap.Width / bitmap.Height;
                    }

                    e.Graphics.DrawImage(bitmap, new Rectangle(itemPoint, new Size(width, height)));
                }

                e.Graphics.Clip = previousClip;
            }
        }
        #endregion

        #region [ ItemsTileView_ItemSelectionChanged ]
        private void ItemsTileView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected)
            {
                return;
            }

            UpdateSelection(e.ItemIndex);
            //ItemsTileView.Focus(e.ItemIndex);

            // 调用更新 Datagridview 颜色的方法 => colorsImageToolStripMenuItem
            UpdateColors();
        }
        #endregion

        #region [ ItemsTileView_FocusSelectionChanged ]
        private void ItemsTileView_FocusSelectionChanged(object sender, TileViewControl.ListViewFocusedItemSelectionChangedEventArgs e)
        {
            if (!e.IsFocused)
            {
                return;
            }

            UpdateSelection(e.FocusedItemIndex);
        }
        #endregion

        #region [ UpdateSelection ]
        private void UpdateSelection(int itemIndex)
        {
            // 当选择新图像时更新 currentImageID - 网格
            currentImageID = itemIndex;

            if (_itemList.Count == 0)
            {
                return;
            }

            SelectedGraphicId = itemIndex < 0 || itemIndex > _itemList.Count
                ? _itemList[0]
                : _itemList[itemIndex];
        }
        #endregion

        #region [ ItemsTileView_MouseDoubleClick ]
        public void ItemsTileView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ItemsTileView.SelectedIndices.Count == 0)
            {
                return;
            }

            ItemDetailForm f = new ItemDetailForm(_itemList[ItemsTileView.SelectedIndices[0]])
            {
                TopMost = true
            };
            f.Show();
        }
        #endregion

        #region [ ItemsTileView_KeyDown ]
        private void ItemsTileView_KeyDown(object sender, KeyEventArgs e)
        {
            // 检查是否按下了 Ctrl+V 组合键
            if (e.Control && e.KeyCode == Keys.V)
            {
                // 调用 importToolStripclipboardMenuItem_Click 方法从剪贴板导入图形
                ImportToolStripclipboardMenuItem_Click(sender, e);
            }
            // 检查是否按下了 Ctrl+X 组合键
            else if (e.Control && e.KeyCode == Keys.X)
            {
                // 调用 cutToolStripclipboardMenuItem_Click 方法剪切选定区域
                CopyToolStripMenuItem_Click(sender, e);
            }
            // 检查是否按下了 Page Down 或 Page Up 键
            else if (e.KeyData == Keys.PageDown || e.KeyData == Keys.PageUp)
            {
                _scrolling = true;
            }
            // 检查是否按下了 Ctrl+F3 组合键
            else if (e.Control && e.KeyCode == Keys.F3)
            {
                // 调用 searchByNameToolStripButton_Click 方法
                SearchByNameToolStripButton_Click(sender, e);
            }
            // 检查是否按下了 Ctrl+J 组合键
            else if (e.Control && e.KeyCode == Keys.J)
            {
                // 调用 goToMarkedPositionToolStripMenuItem_Click 方法跳转到标记位置
                GoToMarkedPositionToolStripMenuItem_Click(sender, e);
            }
        }
        #endregion

        #region [ ItemsTileView_KeyUp ]
        private void ItemsTileView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.PageDown && e.KeyData != Keys.PageUp)
            {
                return;
            }

            _scrolling = false;

            if (ItemsTileView.FocusIndex > 0)
            {
                UpdateToolStripLabels(_selectedGraphicId);
                UpdateDetail(_selectedGraphicId);
            }
        }
        #endregion

        #region [ SelectInGumpsTab ]
        private const int _maleGumpOffset = 50_000;
        private const int _femaleGumpOffset = 60_000;

        private static void SelectInGumpsTab(int graphicId, bool female = false)
        {
            int gumpOffset = female ? _femaleGumpOffset : _maleGumpOffset;
            var itemData = TileData.ItemTable[graphicId];

            GumpControl.Select(itemData.Animation + gumpOffset);
        }
        #endregion

        #region [ SelectInGumpsTabMaleToolStripMenuItem ]
        private void SelectInGumpsTabMaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedGraphicId <= 0)
            {
                return;
            }

            SelectInGumpsTab(SelectedGraphicId);
        }
        #endregion

        #region [ SelectInGumpsTabFemaleToolStripMenuItem ]
        private void SelectInGumpsTabFemaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedGraphicId <= 0)
            {
                return;
            }

            SelectInGumpsTab(SelectedGraphicId, true);
        }
        #endregion

        #region [ TileViewContextMenuStrip ]
        private void TileViewContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (SelectedGraphicId <= 0)
            {
                selectInGumpsTabMaleToolStripMenuItem.Enabled = false;
                selectInGumpsTabFemaleToolStripMenuItem.Enabled = false;
            }
            else
            {
                var itemData = TileData.ItemTable[SelectedGraphicId];

                if (itemData.Animation > 0)
                {
                    selectInGumpsTabMaleToolStripMenuItem.Enabled =
                        GumpControl.HasGumpId(itemData.Animation + _maleGumpOffset);

                    selectInGumpsTabFemaleToolStripMenuItem.Enabled =
                        GumpControl.HasGumpId(itemData.Animation + _femaleGumpOffset);
                }
                else
                {
                    selectInGumpsTabMaleToolStripMenuItem.Enabled = false;
                    selectInGumpsTabFemaleToolStripMenuItem.Enabled = false;
                }
            }
        }
        #endregion

        #region [ ReplaceStartingFromText_KeyDown ]
        private void ReplaceStartingFromText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (!Utils.ConvertStringToInt(ReplaceStartingFromText.Text, out int index, 0, Art.GetMaxItemId()))
            {
                return;
            }

            TileViewContextMenuStrip.Close();

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Title = $"选择从 0x{index:X} 开始替换的图像文件";
                dialog.CheckFileExists = true;
                dialog.Filter = "图像文件 (*.tif;*.tiff;*.bmp;*.png)|*.tif;*.tiff;*.bmp;*.png";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                for (int i = 0; i < dialog.FileNames.Length; i++)
                {
                    var currentIdx = index + i;

                    if (IsIndexValid(currentIdx))
                    {
                        AddSingleItem(dialog.FileNames[i], currentIdx);
                    }
                }

                ItemsTileView.VirtualListSize = _itemList.Count;
                ItemsTileView.Invalidate();

                SelectedGraphicId = index;

                UpdateToolStripLabels(index);
                UpdateDetail(index);
            }
        }
        #endregion

        #region [ AddSingleItem ]
        /// <summary>
        /// 添加单个静态项目。
        /// </summary>
        /// <param name="fileName">要添加的图像文件名</param>
        /// <param name="index">将添加静态项目的索引</param>
        /// 
        private void AddSingleItem(string fileName, int index)
        {
            using (var bmpTemp = new Bitmap(fileName))
            {
                Bitmap bitmap = new Bitmap(bmpTemp);

                if (fileName.Contains(".bmp"))
                {
                    bitmap = Utils.ConvertBmp(bitmap);
                }

                Art.ReplaceStatic(index, bitmap);

                ControlEvents.FireItemChangeEvent(this, index);

                Options.ChangedUltimaClass["Art"] = true;

                if (_showFreeSlots)
                {
                    SelectedGraphicId = index;

                    UpdateToolStripLabels(index);
                    UpdateDetail(index);
                }
                else
                {
                    bool done = false;

                    for (int i = 0; i < _itemList.Count; ++i)
                    {
                        if (index > _itemList[i])
                        {
                            continue;
                        }

                        _itemList[i] = index;

                        done = true;

                        break;
                    }

                    if (!done)
                    {
                        _itemList.Add(index);
                    }

                    ItemsTileView.VirtualListSize = _itemList.Count;
                    ItemsTileView.Invalidate();

                    SelectedGraphicId = index;

                    UpdateToolStripLabels(index);
                    UpdateDetail(index);
                }
            }
        }
        #endregion

        #region [ IsIndexValid ]
        /// <summary>
        /// 检查是否为有效的土地瓷砖索引。土地瓷砖有固定大小 0x4000。
        /// </summary>
        /// <param name="index">起始索引</param>
        /// 
        private static bool IsIndexValid(int index)
        {
            return index >= 0 && index <= Art.GetMaxItemId();
        }
        #endregion

        #region [ Copy clipboard ]
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查 ItemsTileView 中是否选择了任何项目
            if (ItemsTileView.SelectedIndices.Count == 0)
            {
                return;
            }

            // 遍历选定的索引
            foreach (int selectedIndex in ItemsTileView.SelectedIndices)
            {
                // 获取选定项目的图形
                Bitmap bitmap = Art.GetStatic(_itemList[selectedIndex]);
                // 检查图形是否存在
                if (bitmap != null)
                {
                    // 将颜色 #D3D3D3 更改为 #FFFFFF
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            Color pixelColor = bitmap.GetPixel(x, y);
                            if (pixelColor.R == 211 && pixelColor.G == 211 && pixelColor.B == 211)
                            {
                                bitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                            }
                        }
                    }

                    // 将图像转换为 16 位颜色深度
                    Bitmap bmp16bit = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
                    using (Graphics g = Graphics.FromImage(bmp16bit))
                    {
                        g.DrawImage(bitmap, new Rectangle(0, 0, bmp16bit.Width, bmp16bit.Height));
                    }

                    // 将图形复制到剪贴板
                    Clipboard.SetImage(bmp16bit);
                    MessageBox.Show($"图像 {selectedIndex} 已复制到剪贴板！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // 显示一个 MessageBox 通知用户图像已成功复制
                    MessageBox.Show($"索引 {selectedIndex} 没有图像可复制！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region [ Import clipbord image ]
        private void ImportToolStripclipboardMenuItem_Click(object sender, EventArgs e)
        {
            // 检查剪贴板是否包含图像
            if (Clipboard.ContainsImage())
            {
                using (Image image = Clipboard.GetImage())
                {
                    Size imageSize = image.Size;
                    int bytesPerPixel = 4; // 假设为 32 位图像
                    int imageSizeInBytes = imageSize.Width * imageSize.Height * bytesPerPixel;
                }
                // 从剪贴板检索图像
                using (Bitmap bmp = new Bitmap(Clipboard.GetImage()))
                {   // 从 ItemsTileView 获取选定索引
                    int index = SelectedGraphicId;

                    if (index >= 0 && index < Art.GetMaxItemId())
                    {   // 创建一个与剪贴板图像大小相同的新位图
                        Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
                        // 将新位图的分辨率设置为 96 DPI
                        newBmp.SetResolution(96, 96);
                        // 定义要转换的颜色
                        Color[] colorsToConvert = new Color[]
                        {
                            Color.FromArgb(211, 211, 211), // #D3D3D3 => #000000
                            Color.FromArgb(0, 0, 0),       // #000000 => #000000
                            Color.FromArgb(255, 255, 255), // #FFFFFF => #000000
                            Color.FromArgb(254, 254, 254)  // #FEFEFE => #000000
                        };
                        // 遍历图像的每个像素
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            for (int y = 0; y < bmp.Height; y++)
                            {   // 获取当前像素的颜色
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
                        // 创建具有指定像素格式的新位图（32 位）
                        Bitmap finalBmp = newBmp.Clone(new Rectangle(0, 0, newBmp.Width, newBmp.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                        // 在主程序相同目录中创建 "clipboardTemp" 目录
                        string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clipboardTemp");
                        Directory.CreateDirectory(directoryPath);

                        // 将最终位图保存到 "clipboardTemp" 目录中的文件，文件名包含选定索引和额外名称 "Arts"
                        string fileName = $"Art_Hex_Adress_{index:X}.bmp";
                        string filePath = Path.Combine(directoryPath, fileName);
                        finalBmp.Save(filePath);

                        // 导入保存的位图
                        using (var bmpTemp = new Bitmap(filePath))
                        {
                            Bitmap bitmap = new Bitmap(bmpTemp);

                            if (filePath.Contains(".bmp"))
                            {
                                bitmap = Utils.ConvertBmp(bitmap);
                            }

                            Art.ReplaceStatic(index, bitmap);

                            ControlEvents.FireItemChangeEvent(this, index);

                            if (!_itemList.Contains(index))
                            {
                                _itemList.Add(index);
                                _itemList.Sort();
                            }
                            ItemsTileView.VirtualListSize = _itemList.Count;
                            ItemsTileView.Invalidate();
                            SelectedGraphicId = index;
                            Options.ChangedUltimaClass["Art"] = true;
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
        #endregion

        #region [ Mirror Image ]
        private void MirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查 ItemsTileView 中是否选择了任何项目
            if (ItemsTileView.SelectedIndices.Count == 0)
            {
                return;
            }

            // 遍历选定的索引
            foreach (int selectedIndex in ItemsTileView.SelectedIndices)
            {
                // 获取选定项目的图像
                Bitmap bitmap = Art.GetStatic(_itemList[selectedIndex]);

                // 检查图像是否可用
                if (bitmap != null)
                {
                    // 水平镜像图像
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);

                    // 用镜像图像替换原始图像
                    Art.ReplaceStatic(_itemList[selectedIndex], bitmap);
                }
            }

            // 更新 DetailPictureBox
            UpdateDetail(_selectedGraphicId);

            // 更新 ItemsTileView
            ItemsTileView.Invalidate();
        }
        #endregion

        #region [ new Search ]
        private void SearchByIdToolStripTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Utils.ConvertStringToInt(searchByIdToolStripTextBox.Text, out int indexValue))
            {
                return;
            }

            var maximumIndex = Art.GetMaxItemId();

            if (indexValue < 0)
            {
                indexValue = 0;
            }

            if (indexValue > maximumIndex)
            {
                indexValue = maximumIndex;
            }

            // 我们必须使焦点无效，以便滚动到项目
            ItemsTileView.FocusIndex = -1;
            SelectedGraphicId = indexValue;
        }
        private void SearchByNameToolStripTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchName(searchByNameToolStripTextBox.Text, false);
        }
        private void SearchByNameToolStripButton_Click(object sender, EventArgs e)
        {
            SearchName(searchByNameToolStripTextBox.Text, true);
            // 正向搜索后更新 _reverseSearchIndex
            _reverseSearchIndex = _itemList.IndexOf(SelectedGraphicId);
        }
        #endregion

        #region [ Select ID to Hex ]
        private void SelectIDToHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedGraphicId >= 0)
            {
                // 将选定的 ID 转换为十六进制地址
                string hexAddress = $"0x{_selectedGraphicId:X4}";

                // 将十六进制地址复制到剪贴板
                Clipboard.SetText(hexAddress);
            }
        }
        #endregion

        #region [ Image swap ]
        private void ImageSwapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 确保恰好选择了两个项目
            if (ItemsTileView.SelectedIndices.Count != 2)
            {
                MessageBox.Show("请选择恰好两个项目进行交换。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 获取选定的索引
            int index1 = ItemsTileView.SelectedIndices[0];
            int index2 = ItemsTileView.SelectedIndices[1];

            // 临时保存图形
            Bitmap ArtTempImage1 = Art.GetStatic(_itemList[index1]);
            Bitmap ArtTempImage2 = Art.GetStatic(_itemList[index2]);

            // 交换图形
            ReplaceStaticSwap(_itemList[index1], ArtTempImage1, _itemList[index2], ArtTempImage2);

            // 更新视图和标签
            ItemsTileView.Invalidate();
            UpdateToolStripLabels(_selectedGraphicId);
            UpdateDetail(_selectedGraphicId);

            Options.ChangedUltimaClass["Art"] = true;
        }

        private void ReplaceStaticSwap(int index1, Bitmap newGraphic1, int index2, Bitmap newGraphic2)
        {
            // 将索引 'index1' 处的图形替换为 'newGraphic2'
            _selectedGraphicId = index1;
            OnClickReplace(newGraphic2);

            // 将索引 'index2' 处的图形替换为 'newGraphic1'
            _selectedGraphicId = index2;
            OnClickReplace(newGraphic1);

        }

        private void OnClickReplace(Bitmap bitmap)
        {
            Art.ReplaceStatic(_selectedGraphicId, bitmap);
            ControlEvents.FireItemChangeEvent(this, _selectedGraphicId);
        }
        #endregion

        #region [ reverse search ]

        // 存储向后搜索当前索引的全局变量        
        private int _reverseSearchIndex = -1;

        private void ReverseSearchByName(string name)
        {
            // 检查名称是否为空
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            // 如果 _reverseSearchIndex 为 -1 或者执行了正向搜索，则用 _itemList 的最后一个索引初始化它
            if (_reverseSearchIndex == -1 || _reverseSearchIndex >= _itemList.Count)
            {
                _reverseSearchIndex = _itemList.Count - 1;
            }

            // 从 _reverseSearchIndex 开始反向循环遍历 _itemList
            for (int i = _reverseSearchIndex; i >= 0; i--)
            {
                // 获取当前位置的项目
                var item = _itemList[i];

                // 检查项目名称是否包含搜索的名称（部分匹配）
                if (TileData.ItemTable[item].Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    // 如果是，将 SelectedGraphicId 设置为找到的项目的索引并终止循环
                    SelectedGraphicId = item;

                    // 为下一次搜索更新 _reverseSearchIndex
                    _reverseSearchIndex = i - 1;
                    break;
                }
            }

            // 当遍历完整个 _itemList 时，将 _reverseSearchIndex 设置回 -1
            if (_reverseSearchIndex < 0)
            {
                _reverseSearchIndex = -1;
            }
        }

        private void ReverseSearchToolStripButton_Click(object sender, EventArgs e)
        {
            // 从 TextBox 获取名称
            string name = searchByNameToolStripTextBox.Text;

            // 执行反向搜索
            ReverseSearchByName(name);
        }
        #endregion

        #region [ Paricle Gray Shadow ]
        private void ParticleGraylToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // 检查是否有图像
            if (DetailPictureBox.Image != null)
            {
                // 从 DetailPictureBox 获取选定的图像
                Bitmap bmp = new Bitmap(DetailPictureBox.Image);

                // 要更改的颜色列表
                List<string> colorsToChange = new List<string>
                {
                    // 在此处添加任何其他颜色...
                    "030303", "040404", "050505", "060606", "070707", "080808", "090909", "0A0A0A", "0B0B0B", "0C0C0C", "0D0D0D", "0E0E0E", "0F0F0F",
                    "101010", "111111", "121212", "131313", "141414", "151515", "161616", "171717", "181818", "191919", "1A1A1A", "1B1B1B", "1C1C1C",
                    "1D1D1D", "1E1E1E", "1F1F1F", "202020", "212121", "222222", "232323", "242424", "252525", "262626", "272727", "282828", "292929",
                    "2A2A2A", "2B2B2B", "2C2C2C", "2D2D2D", "2E2E2E", "2F2F2F", "303030", "313131", "323232", "333333", "343434", "353535", "363636",
                    "373737", "383838", "393939", "3A3A3A", "3B3B3B", "3C3C3C", "3D3D3D", "3E3E3E", "3F3F3F", "404040", "414141", "424242", "434343",
                    "444444", "454545", "464646", "474747", "484848", "494949", "4A4A4A", "4B4B4B", "4C4C4C", "4D4D4D", "4E4E4E", "4F4F4F", "505050",
                    "515151", "525252", "535353", "545454", "555555", "565656", "575757", "585858", "595959", "5A5A5A", "5B5B5B", "5C5C5C", "5D5D5D",
                    "5E5E5E", "5F5F5F", "606060", "616161", "626262", "636363", "646464", "656565", "666666", "676767", "686868", "696969", "6A6A6A",
                    "6B6B6B", "6C6C6C", "6D6D6D", "6E6E6E", "6F6F6F", "707070", "717171", "727272", "737373", "747474", "757575", "767676", "777777",
                    "787878", "797979", "7A7A7A", "7B7B7B", "7C7C7C", "7D7D7D", "7E7E7E", "7F7F7F", "808080", "818181", "828282", "838383", "848484",
                    "858585", "868686", "878787", "888888", "898989", "8A8A8A", "8B8B8B", "8C8C8C", "8D8D8D", "8E8E8E", "8F8F8F", "909090", "919191",
                    "929292", "939393", "949494", "959595", "969696", "979797", "989898", "999999", "9A9A9A", "9B9B9B", "9C9C9C", "9D9D9D", "9E9E9E",
                    "9F9F9F", "A0A0A0", "A1A1A1", "A2A2A2", "A3A3A3", "A4A4A4", "A5A5A5", "A6A6A6", "A7A7A7", "A8A8A8", "A9A9A9", "AAAAAA", "ABABAB",
                    "ACACAC", "ADADAD", "AEAEAE", "AFAFAF", "B0B0B0", "B1B1B1", "B2B2B2", "B3B3B3", "B4B4B4", "B5B5B5", "B6B6B6", "B7B7B7", "B8B8B8",
                    "B9B9B9", "BABABA", "BBBBBB", "BCBCBC", "BDBDBD", "BEBEBE", "BFBFBF", "C0C0C0", "C1C1C1", "C2C2C2", "C3C3C3", "C4C4C4", "C5C5C5",
                    "C6C6C6", "C7C7C7", "C8C8C8", "C9C9C9", "CACACA", "CBCBCB", "CCCCCC", "CDCDCD", "CECECE", "CFCFCF", "D0D0D0", "D1D1D1", "D2D2D2",
                    "D3D3D3", "D4D4D4", "D5D5D5", "D6D6D6", "D7D7D7", "D8D8D8", "D9D9D9", "DADADA", "DBDBDB", "DCDCDC", "DDDDDD", "DEDEDE", "DFDFDF",
                    "E0E0E0", "E1E1E1", "E2E2E2", "E3E3E3", "E4E4E4", "E5E5E5", "E6E6E6", "E7E7E7", "E8E8E8", "E9E9E9", "EAEAEA", "EBEBEB", "ECECEC",
                    "EDEDED", "EEEEEE", "EFEFEF", "F0F0F0", "F1F1F1", "F2F2F2", "F3F3F3", "F4F4F4", "F5F5F5", "F6F6F6", "F7F7F7", "F8F8F8", "F9F9F9",
                    "FAFAFA", "FBFBFB"
                };

                // 新颜色
                // Color newColor = Color.Blue;

                // 新颜色
                Color newColor = selectedColor; // 使用用户选择的颜色

                // 循环遍历像素
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        // 获取坐标处的像素颜色
                        Color oldColor = bmp.GetPixel(x, y);

                        // 检查是否是要更改的颜色之一
                        string colorHex = oldColor.R.ToString("X2") + oldColor.G.ToString("X2") + oldColor.B.ToString("X2");
                        if (colorsToChange.Contains(colorHex))
                        {
                            // 获取原始颜色的亮度（0-1）
                            float brightness = oldColor.GetBrightness();

                            // 创建一个新颜色，它是原始颜色根据亮度调整的
                            int newR = (int)(newColor.R * brightness);
                            int newG = (int)(newColor.G * brightness);
                            int newB = (int)(newColor.B * brightness);

                            Color newShadedColor = Color.FromArgb(newR, newG, newB);

                            // 将其更改为新的阴影颜色
                            bmp.SetPixel(x, y, newShadedColor);
                        }
                    }
                }

                // 设置新图像
                DetailPictureBox.Image = bmp;
            }
            else
            {
                // 处理没有图像的情况...
                // MessageBox.Show("没有选择图像。请先选择一个图像。");
            }
        }

        private void ChkApplyColorChange_CheckedChanged(object sender, EventArgs e)
        {
            ApplyColorChange();
        }

        private void InsertNewImage(Image newImage)
        {
            // 设置新图像
            DetailPictureBox.Image = newImage;

            // 如果复选框被选中，则应用颜色更改
            ApplyColorChange();
        }

        private void ApplyColorChange()
        {
            // 检查复选框是否被选中
            if (chkApplyColorChange.Checked)
            {
                // 直接调用颜色更改方法
                ParticleGraylToolStripMenuItem_Click(null, null);
            }
        }
        #endregion

        #region [ Particle Gray ColorDialog ]
        private Color selectedColor = Color.Blue; // 默认颜色
        private void ParticleGrayColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedColor = colorDialog.Color;
                }
            }
        }
        #endregion

        #region [ drawRhombus ]
        private void DrawRhombusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 确保 PictureBox 中有图像
            if (DetailPictureBox.Image == null)
            {
                return;
            }

            // 从现有图像创建一个新的 Graphics 对象
            using (Graphics g = Graphics.FromImage(DetailPictureBox.Image))
            {
                // 定义上部菱形的点
                Point[] pointsUpper = new Point[4];
                pointsUpper[0] = new Point(DetailPictureBox.Image.Width / 2, 0); // 中心
                pointsUpper[1] = new Point(DetailPictureBox.Image.Width / 2 + 22, 22); // 右下
                pointsUpper[2] = new Point(DetailPictureBox.Image.Width / 2, 44); // 下方
                pointsUpper[3] = new Point(DetailPictureBox.Image.Width / 2 - 22, 22); // 左下

                // 绘制上部菱形
                g.DrawPolygon(Pens.Black, pointsUpper);

                // 从上部菱形的角向上绘制线条
                g.DrawLine(Pens.Black, pointsUpper[0], new Point(pointsUpper[0].X, 0)); // 从中间向上
                g.DrawLine(Pens.Black, pointsUpper[1], new Point(pointsUpper[1].X, 0)); // 从右下到顶部
                g.DrawLine(Pens.Black, pointsUpper[3], new Point(pointsUpper[3].X, 0)); // 从左下到顶部

                // 计算水平线的 X 坐标
                int lineWidth = 100;
                int lineStartX = (DetailPictureBox.Image.Width - lineWidth) / 2;
                int lineEndX = lineStartX + lineWidth;

                // 注意图像的高度用于下部菱形的位置
                int imageHeight = DetailPictureBox.Image.Height;

                // 在下部菱形顶部绘制一条水平线
                g.DrawLine(Pens.Black, new Point(lineStartX, imageHeight - 66), new Point(lineEndX, imageHeight - 66));

                // 定义下部菱形的点
                Point[] pointsLower = new Point[4];
                pointsLower[0] = new Point(DetailPictureBox.Image.Width / 2, imageHeight - 66); // 中心
                pointsLower[1] = new Point(DetailPictureBox.Image.Width / 2 + 22, imageHeight - 88); // 右下
                pointsLower[2] = new Point(DetailPictureBox.Image.Width / 2, imageHeight - 110); // 下方
                pointsLower[3] = new Point(DetailPictureBox.Image.Width / 2 - 22, imageHeight - 88); // 左下

                // 绘制下部菱形
                g.DrawPolygon(Pens.Black, pointsLower);

                // 从下部菱形的角向上绘制线条
                g.DrawLine(Pens.Black, pointsLower[0], new Point(pointsLower[0].X, pointsLower[0].Y - 22)); // 从中间向上
                g.DrawLine(Pens.Black, pointsLower[1], new Point(pointsLower[1].X, pointsLower[1].Y - 22)); // 从右下到顶部
                g.DrawLine(Pens.Black, pointsLower[3], new Point(pointsLower[3].X, pointsLower[3].Y - 22)); // 从左下到顶部

                // 连接上下菱形的线条
                g.DrawLine(Pens.Black, pointsUpper[0], pointsLower[0]); // 连接中点
                g.DrawLine(Pens.Black, pointsUpper[1], pointsLower[1]); // 连接右点
                g.DrawLine(Pens.Black, pointsUpper[3], pointsLower[3]); // 连接左点
            }

            // 刷新 PictureBox 以反映更改
            DetailPictureBox.Invalidate();
        }
        #endregion

        #region [ GridPictureToolStripMenuItem ]

        private int currentImageID;

        private void GridPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查是否选择了图像
            if (currentImageID >= 0)
            {
                // 调用 ShowImageWithBackground 方法显示选定的图像
                ShowImageWithBackground(currentImageID);
            }
            else
            {
                // 如果未选择图像，将收到错误消息
                MessageBox.Show("请先从 ItemsTileView 中选择一个图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region [ ShowImageWithBackground ]
        private void ShowImageWithBackground(int imageIndex)
        {
            // 加载要显示的图像
            Image foregroundImage = Art.GetStatic(_itemList[imageIndex]);

            // 从资源中下载壁纸
            Image backgroundImage = Properties.Resources.rasterpink_png;

            // 更改背景图像的颜色
            backgroundImage = ChangeImageColor(backgroundImage, Color.FromArgb(244, 101, 255), selectedColorGrid);

            // 创建一个足够大的新位图以容纳两张图像
            Bitmap combinedImage = new Bitmap(Math.Max(backgroundImage.Width, foregroundImage.Width), Math.Max(backgroundImage.Height, foregroundImage.Height));

            // 创建一个 Graphics 对象以便在位图上绘制
            using (Graphics g = Graphics.FromImage(combinedImage))
            {
                // 首先绘制前景图像
                g.DrawImage(foregroundImage, (combinedImage.Width - foregroundImage.Width) / 2, (combinedImage.Height - foregroundImage.Height));

                // 在计算的位置绘制背景图像
                g.DrawImage(backgroundImage, (combinedImage.Width - backgroundImage.Width) / 2, (combinedImage.Height - backgroundImage.Height));
            }

            // 将合并的图像分配给 PictureBox
            DetailPictureBox.Image = combinedImage;
        }

        #endregion

        #region [ Copy Clipboard DetailPictureBox ]
        private void CopyClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查 DetailPictureBox 中是否显示图像
            if (DetailPictureBox.Image != null)
            {
                // 将图像复制到剪贴板
                Clipboard.SetImage(DetailPictureBox.Image);
            }
            else
            {
                // 如果没有显示图像，将收到错误消息
                MessageBox.Show("未显示任何图像。请先选择一个图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region [ Grid Color ]
        private Color selectedColorGrid = Color.FromArgb(244, 101, 255); // 默认颜色 #f465ff

        private void SelectColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                // 将初始颜色设置为当前选定的颜色
                colorDialog.Color = selectedColorGrid;

                // 显示对话框并验证用户是否单击了“确定”
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    // 更新选定的颜色
                    selectedColorGrid = colorDialog.Color;
                }
            }
        }

        private Image ChangeImageColor(Image image, Color oldColor, Color newColor)
        {
            Bitmap bmp = new Bitmap(image);

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);

                    // 检查当前像素是否为旧颜色
                    if (pixelColor == oldColor)
                    {
                        // 如果是，则将像素的颜色更改为新颜色
                        bmp.SetPixel(x, y, newColor);
                    }
                }
            }

            return bmp;
        }
        #endregion

        #region [ colorsImageToolStripMenuIte ]
        // DataGridView 和窗体的全局变量
        DataGridView colorGrid;
        Form colorForm;
        private void ColorsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建一个新窗体
            colorForm = new Form();
            colorForm.Text = "图像颜色";
            // 将 FormBorderStyle 设置为 FixedDialog 以固定窗体大小
            colorForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            // 禁用最大化按钮
            colorForm.MaximizeBox = false;
            // 确保窗体始终保持在最前面
            colorForm.TopMost = true;

            // 创建一个新的 DataGridView 并将其添加到窗体
            colorGrid = new DataGridView();
            colorGrid.Dock = DockStyle.Fill;
            colorForm.Controls.Add(colorGrid);

            // 向 DataGridView 添加一列
            colorGrid.Columns.Add("Color", "颜色");

            // 添加单元格单击事件处理程序
            colorGrid.CellClick += (s, args) =>
            {
                // 检查是否单击了单元格
                if (args.RowIndex >= 0 && args.ColumnIndex >= 0)
                {
                    // 获取单击单元格的值
                    string colorHex = (string)colorGrid.Rows[args.RowIndex].Cells[args.ColumnIndex].Value;

                    // 将值复制到剪贴板
                    Clipboard.SetText(colorHex);
                }
            };

            // 显示窗体
            colorForm.Show();

            // 更新颜色
            UpdateColors();
        }
        #endregion

        #region [ UpdateColors Datagridview ]
        private void UpdateColors()
        {
            // 检查 colorGrid 和 colorForm 是否已初始化并且窗体可见
            if (colorGrid == null || colorForm == null || !colorForm.Visible)
            {
                return;
            }

            // 删除所有先前的行
            colorGrid.Rows.Clear();

            // 获取选定的图像
            Bitmap selectedImage = (Bitmap)DetailPictureBox.Image;

            // 创建一个 HashSet 来存储唯一颜色
            HashSet<string> uniqueColors = new HashSet<string>();

            // 循环遍历图像中的每个像素
            for (int x = 0; x < selectedImage.Width; x++)
            {
                for (int y = 0; y < selectedImage.Height; y++)
                {
                    // 获取像素的颜色
                    Color pixelColor = selectedImage.GetPixel(x, y);

                    // 将颜色转换为十六进制代码
                    string colorHex = pixelColor.R.ToString("X2") + pixelColor.G.ToString("X2") + pixelColor.B.ToString("X2");

                    // 将颜色添加到 HashSet
                    uniqueColors.Add(colorHex);
                }
            }

            // 循环遍历每种唯一颜色并将其添加到 DataGridView
            foreach (string color in uniqueColors)
            {
                // 向 DataGridView 添加新行
                int rowIndex = colorGrid.Rows.Add();

                // 将单元格的背景颜色设置为该颜色
                colorGrid.Rows[rowIndex].Cells[0].Style.BackColor = ColorTranslator.FromHtml("#" + color);

                // 将单元格的文本设置为颜色的十六进制代码
                colorGrid.Rows[rowIndex].Cells[0].Value = color;
            }
        }
        #endregion

        #region [ markToolStripMenuItem ]
        // 存储标记位置的变量
        private int markedPosition = -1;

        // 标记位置的方法
        private void MarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemsTileView.SelectedIndices.Count > 0)
            {
                markedPosition = ItemsTileView.SelectedIndices[0];
            }
        }
        #endregion

        #region [ goToMarkedPositionToolStripMenuItem ]
        // 返回标记位置的方法
        private void GoToMarkedPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (markedPosition >= 0 && markedPosition < ItemsTileView.VirtualListSize)
            {
                // 将选定位置设置为标记位置
                ItemsTileView.SelectedIndices.Clear();
                ItemsTileView.SelectedIndices.Add(markedPosition);
            }
        }
        #endregion

        #region [ TileViewContextMenuStrip_Closing ]
        private void TileViewContextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            // 检查 ItemsTileView 中是否选择了任何项目
            if (ItemsTileView.SelectedIndices.Count > 0)
            {
                // 将焦点设置为第一个选定的项目
                ItemsTileView.FocusIndex = ItemsTileView.SelectedIndices[0];
            }
        }
        #endregion

        #region [ grayscaleToolStripMenuItem ] 
        #region [ ArrayList grayscaleColors ]
        private ArrayList grayscaleColors =
        [
            ColorTranslator.FromHtml("#030303"), ColorTranslator.FromHtml("#040404"),
            ColorTranslator.FromHtml("#050505"), ColorTranslator.FromHtml("#060606"),
            ColorTranslator.FromHtml("#070707"), ColorTranslator.FromHtml("#080808"),
            ColorTranslator.FromHtml("#090909"), ColorTranslator.FromHtml("#0A0A0A"),
            ColorTranslator.FromHtml("#0B0B0B"), ColorTranslator.FromHtml("#0C0C0C"),
            ColorTranslator.FromHtml("#0D0D0D"), ColorTranslator.FromHtml("#0E0E0E"),
            ColorTranslator.FromHtml("#0F0F0F"), ColorTranslator.FromHtml("#101010"),
            ColorTranslator.FromHtml("#111111"), ColorTranslator.FromHtml("#121212"),
            ColorTranslator.FromHtml("#131313"), ColorTranslator.FromHtml("#141414"),
            ColorTranslator.FromHtml("#151515"), ColorTranslator.FromHtml("#161616"),
            ColorTranslator.FromHtml("#171717"), ColorTranslator.FromHtml("#181818"),
            ColorTranslator.FromHtml("#191919"), ColorTranslator.FromHtml("#1A1A1A"),
            ColorTranslator.FromHtml("#1B1B1B"), ColorTranslator.FromHtml("#1C1C1C"),
            ColorTranslator.FromHtml("#1D1D1D"), ColorTranslator.FromHtml("#1E1E1E"),
            ColorTranslator.FromHtml("#1F1F1F"), ColorTranslator.FromHtml("#202020"),
            ColorTranslator.FromHtml("#212121"), ColorTranslator.FromHtml("#222222"),
            ColorTranslator.FromHtml("#232323"), ColorTranslator.FromHtml("#242424"),
            ColorTranslator.FromHtml("#252525"), ColorTranslator.FromHtml("#262626"),
            ColorTranslator.FromHtml("#272727"), ColorTranslator.FromHtml("#282828"),
            ColorTranslator.FromHtml("#292929"), ColorTranslator.FromHtml("#2A2A2A"),
            ColorTranslator.FromHtml("#2B2B2B"), ColorTranslator.FromHtml("#2C2C2C"),
            ColorTranslator.FromHtml("#2D2D2D"), ColorTranslator.FromHtml("#2E2E2E"),
            ColorTranslator.FromHtml("#2F2F2F"), ColorTranslator.FromHtml("#303030"),
            ColorTranslator.FromHtml("#313131"), ColorTranslator.FromHtml("#323232"),
            ColorTranslator.FromHtml("#333333"), ColorTranslator.FromHtml("#343434"),
            ColorTranslator.FromHtml("#353535"), ColorTranslator.FromHtml("#363636"),
            ColorTranslator.FromHtml("#373737"), ColorTranslator.FromHtml("#383838"),
            ColorTranslator.FromHtml("#393939"), ColorTranslator.FromHtml("#3A3A3A"),
            ColorTranslator.FromHtml("#3B3B3B"), ColorTranslator.FromHtml("#3C3C3C"),
            ColorTranslator.FromHtml("#3D3D3D"), ColorTranslator.FromHtml("#3E3E3E"),
            ColorTranslator.FromHtml("#3F3F3F"), ColorTranslator.FromHtml("#404040"),
            ColorTranslator.FromHtml("#414141"), ColorTranslator.FromHtml("#424242"),
            ColorTranslator.FromHtml("#434343"), ColorTranslator.FromHtml("#444444"),
            ColorTranslator.FromHtml("#454545"), ColorTranslator.FromHtml("#464646"),
            ColorTranslator.FromHtml("#474747"), ColorTranslator.FromHtml("#484848"),
            ColorTranslator.FromHtml("#494949"), ColorTranslator.FromHtml("#4A4A4A"),
            ColorTranslator.FromHtml("#4B4B4B"), ColorTranslator.FromHtml("#4C4C4C"),
            ColorTranslator.FromHtml("#4D4D4D"), ColorTranslator.FromHtml("#4E4E4E"),
            ColorTranslator.FromHtml("#4F4F4F"), ColorTranslator.FromHtml("#505050"),
            ColorTranslator.FromHtml("#515151"), ColorTranslator.FromHtml("#525252"),
            ColorTranslator.FromHtml("#535353"), ColorTranslator.FromHtml("#545454"),
            ColorTranslator.FromHtml("#555555"), ColorTranslator.FromHtml("#565656"),
            ColorTranslator.FromHtml("#575757"), ColorTranslator.FromHtml("#585858"),
            ColorTranslator.FromHtml("#595959"), ColorTranslator.FromHtml("#5A5A5A"),
            ColorTranslator.FromHtml("#5B5B5B"), ColorTranslator.FromHtml("#5C5C5C"),
            ColorTranslator.FromHtml("#5D5D5D"), ColorTranslator.FromHtml("#5E5E5E"),
            ColorTranslator.FromHtml("#5F5F5F"), ColorTranslator.FromHtml("#606060"),
            ColorTranslator.FromHtml("#616161"), ColorTranslator.FromHtml("#626262"),
            ColorTranslator.FromHtml("#636363"), ColorTranslator.FromHtml("#646464"),
            ColorTranslator.FromHtml("#656565"), ColorTranslator.FromHtml("#666666"),
            ColorTranslator.FromHtml("#676767"), ColorTranslator.FromHtml("#686868"),
            ColorTranslator.FromHtml("#696969"), ColorTranslator.FromHtml("#6A6A6A"),
            ColorTranslator.FromHtml("#6B6B6B"), ColorTranslator.FromHtml("#6C6C6C"),
            ColorTranslator.FromHtml("#6D6D6D"), ColorTranslator.FromHtml("#6E6E6E"),
            ColorTranslator.FromHtml("#6F6F6F"), ColorTranslator.FromHtml("#707070"),
            ColorTranslator.FromHtml("#717171"), ColorTranslator.FromHtml("#727272"),
            ColorTranslator.FromHtml("#737373"), ColorTranslator.FromHtml("#747474"),
            ColorTranslator.FromHtml("#757575"), ColorTranslator.FromHtml("#767676"),
            ColorTranslator.FromHtml("#777777"), ColorTranslator.FromHtml("#787878"),
            ColorTranslator.FromHtml("#797979"), ColorTranslator.FromHtml("#7A7A7A"),
            ColorTranslator.FromHtml("#7B7B7B"), ColorTranslator.FromHtml("#7C7C7C"),
            ColorTranslator.FromHtml("#7D7D7D"), ColorTranslator.FromHtml("#7E7E7E"),
            ColorTranslator.FromHtml("#7F7F7F"), ColorTranslator.FromHtml("#808080"),
            ColorTranslator.FromHtml("#818181"), ColorTranslator.FromHtml("#828282"),
            ColorTranslator.FromHtml("#838383"), ColorTranslator.FromHtml("#848484"),
            ColorTranslator.FromHtml("#858585"), ColorTranslator.FromHtml("#868686"),
            ColorTranslator.FromHtml("#878787"), ColorTranslator.FromHtml("#888888"),
            ColorTranslator.FromHtml("#898989"), ColorTranslator.FromHtml("#8A8A8A"),
            ColorTranslator.FromHtml("#8B8B8B"), ColorTranslator.FromHtml("#8C8C8C"),
            ColorTranslator.FromHtml("#8D8D8D"), ColorTranslator.FromHtml("#8E8E8E"),
            ColorTranslator.FromHtml("#8F8F8F"), ColorTranslator.FromHtml("#909090"),
            ColorTranslator.FromHtml("#919191"), ColorTranslator.FromHtml("#929292"),
            ColorTranslator.FromHtml("#939393"), ColorTranslator.FromHtml("#949494"),
            ColorTranslator.FromHtml("#959595"), ColorTranslator.FromHtml("#969696"),
            ColorTranslator.FromHtml("#979797"), ColorTranslator.FromHtml("#989898"),
            ColorTranslator.FromHtml("#999999"), ColorTranslator.FromHtml("#9A9A9A"),
            ColorTranslator.FromHtml("#9B9B9B"), ColorTranslator.FromHtml("#9C9C9C"),
            ColorTranslator.FromHtml("#9D9D9D"), ColorTranslator.FromHtml("#9E9E9E"),
            ColorTranslator.FromHtml("#9F9F9F"), ColorTranslator.FromHtml("#A0A0A0"),
            ColorTranslator.FromHtml("#A1A1A1"), ColorTranslator.FromHtml("#A2A2A2"),
            ColorTranslator.FromHtml("#A3A3A3"), ColorTranslator.FromHtml("#A4A4A4"),
            ColorTranslator.FromHtml("#A5A5A5"), ColorTranslator.FromHtml("#A6A6A6"),
            ColorTranslator.FromHtml("#A7A7A7"), ColorTranslator.FromHtml("#A8A8A8"),
            ColorTranslator.FromHtml("#A9A9A9"), ColorTranslator.FromHtml("#AAAAAA"),
            ColorTranslator.FromHtml("#ABABAB"), ColorTranslator.FromHtml("#ACACAC"),
            ColorTranslator.FromHtml("#ADADAD"), ColorTranslator.FromHtml("#AEAEAE"),
            ColorTranslator.FromHtml("#AFAFAF"), ColorTranslator.FromHtml("#B0B0B0"),
            ColorTranslator.FromHtml("#B1B1B1"), ColorTranslator.FromHtml("#B2B2B2"),
            ColorTranslator.FromHtml("#B3B3B3"), ColorTranslator.FromHtml("#B4B4B4"),
            ColorTranslator.FromHtml("#B5B5B5"), ColorTranslator.FromHtml("#B6B6B6"),
            ColorTranslator.FromHtml("#B7B7B7"), ColorTranslator.FromHtml("#B8B8B8"),
            ColorTranslator.FromHtml("#B9B9B9"), ColorTranslator.FromHtml("#BABABA"),
            ColorTranslator.FromHtml("#BBBBBB"), ColorTranslator.FromHtml("#BCBCBC"),
            ColorTranslator.FromHtml("#BDBDBD"), ColorTranslator.FromHtml("#BEBEBE"),
            ColorTranslator.FromHtml("#BFBFBF"), ColorTranslator.FromHtml("#C0C0C0"),
            ColorTranslator.FromHtml("#C1C1C1"), ColorTranslator.FromHtml("#C2C2C2"),
            ColorTranslator.FromHtml("#C3C3C3"), ColorTranslator.FromHtml("#C4C4C4"),
            ColorTranslator.FromHtml("#C5C5C5"), ColorTranslator.FromHtml("#C6C6C6"),
            ColorTranslator.FromHtml("#C7C7C7"), ColorTranslator.FromHtml("#C8C8C8"),
            ColorTranslator.FromHtml("#C9C9C9"), ColorTranslator.FromHtml("#CACACA"),
            ColorTranslator.FromHtml("#CBCBCB"), ColorTranslator.FromHtml("#CCCCCC"),
            ColorTranslator.FromHtml("#CDCDCD"), ColorTranslator.FromHtml("#CECECE"),
            ColorTranslator.FromHtml("#CFCFCF"), ColorTranslator.FromHtml("#D0D0D0"),
            ColorTranslator.FromHtml("#D1D1D1"), ColorTranslator.FromHtml("#D2D2D2"),
            ColorTranslator.FromHtml("#D3D3D3"), ColorTranslator.FromHtml("#D4D4D4"),
            ColorTranslator.FromHtml("#D5D5D5"), ColorTranslator.FromHtml("#D6D6D6"),
            ColorTranslator.FromHtml("#D7D7D7"), ColorTranslator.FromHtml("#D8D8D8"),
            ColorTranslator.FromHtml("#D9D9D9"), ColorTranslator.FromHtml("#DADADA"),
            ColorTranslator.FromHtml("#DBDBDB"), ColorTranslator.FromHtml("#DCDCDC"),
            ColorTranslator.FromHtml("#DDDDDD"), ColorTranslator.FromHtml("#DEDEDE"),
            ColorTranslator.FromHtml("#DFDFDF"), ColorTranslator.FromHtml("#E0E0E0"),
            ColorTranslator.FromHtml("#E1E1E1"), ColorTranslator.FromHtml("#E2E2E2"),
            ColorTranslator.FromHtml("#E3E3E3"), ColorTranslator.FromHtml("#E4E4E4"),
            ColorTranslator.FromHtml("#E5E5E5"), ColorTranslator.FromHtml("#E6E6E6"),
            ColorTranslator.FromHtml("#E7E7E7"), ColorTranslator.FromHtml("#E8E8E8"),
            ColorTranslator.FromHtml("#E9E9E9"), ColorTranslator.FromHtml("#EAEAEA"),
            ColorTranslator.FromHtml("#EBEBEB"), ColorTranslator.FromHtml("#ECECEC"),
            ColorTranslator.FromHtml("#EDEDED"), ColorTranslator.FromHtml("#EEEEEE"),
            ColorTranslator.FromHtml("#EFEFEF"), ColorTranslator.FromHtml("#F0F0F0"),
            ColorTranslator.FromHtml("#F1F1F1"), ColorTranslator.FromHtml("#F2F2F2"),
            ColorTranslator.FromHtml("#F3F3F3"), ColorTranslator.FromHtml("#F4F4F4"),
            ColorTranslator.FromHtml("#F5F5F5"), ColorTranslator.FromHtml("#F6F6F6"),
            ColorTranslator.FromHtml("#F7F7F7"), ColorTranslator.FromHtml("#F8F8F8"),
            ColorTranslator.FromHtml("#F9F9F9"), ColorTranslator.FromHtml("#FAFAFA"),
            ColorTranslator.FromHtml("#FBFBFB")
        ];
        #endregion

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // 从 DetailPictureBox 获取当前图像
            Bitmap? image = DetailPictureBox.Image as Bitmap;

            if (image == null)
            {
                MessageBox.Show("未选择图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            Color originalColor;

            // 将图像转换为灰度
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    originalColor = image.GetPixel(x, y);

                    // 跳过 #000000 和 #FFFFFF
                    if (originalColor.ToArgb() == Color.Black.ToArgb() || originalColor.ToArgb() == Color.White.ToArgb())
                    {
                        continue;
                    }

                    int brightness = (int)(originalColor.GetBrightness() * (grayscaleColors.Count - 1)); // 确保索引在范围内

                    brightness = Math.Clamp(brightness, 0, grayscaleColors.Count - 1); // 确保索引在边界内

                    if (grayscaleColors[brightness] is Color color)
                    {
                        image.SetPixel(x, y, color);
                    }
                }
            }

            // 使用新的灰度图像更新 DetailPictureBox
            DetailPictureBox.Image = image;

            DetailPictureBox.Update();
        }
        #endregion

        #region [ SaveImageNameAndHexToTempToolStripMenuItem ]
        private void SaveImageNameAndHexToTempToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查是否选择了一个或多个图形
            if (ItemsTileView.SelectedIndices.Count == 0)
            {
                MessageBox.Show("未选择任何图形。");
                return;
            }

            // 获取临时目录路径
            string programDirectory = Application.StartupPath;
            string tempDirectory = Path.Combine(programDirectory, "tempGrafic");

            // 如果目录不存在则创建
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            // 保存的文件路径列表
            List<string> savedFiles = new List<string>();

            // 遍历选定的图形并保存
            foreach (int selectedIndex in ItemsTileView.SelectedIndices)
            {
                int graphicId = _itemList[selectedIndex];
                ItemData itemData = TileData.ItemTable[graphicId];

                // 获取图形和详细信息
                Bitmap bitmap = Art.GetStatic(graphicId);
                if (bitmap == null)
                {
                    MessageBox.Show($"无法获取 ID {graphicId} 的图形。");
                    continue;
                }

                string hexAddress = $"0x{graphicId:X}";
                string imageName = itemData.Name;
                string fileName = $"{hexAddress}_{imageName}.bmp";

                // 保存图形
                string filePath = Path.Combine(tempDirectory, fileName);
                bitmap.Save(filePath, ImageFormat.Bmp);
                savedFiles.Add(filePath);
            }

            // 播放声音
            string soundPath = Path.Combine(programDirectory, "Sound.wav");
            if (File.Exists(soundPath))
            {
                using (SoundPlayer player = new SoundPlayer(soundPath))
                {
                    player.Play();
                }
            }
            else
            {
                MessageBox.Show("找不到声音文件。");
            }

            // 显示包含存储位置的消息框
            string message = "以下图形已保存：\n" + string.Join("\n", savedFiles);
            MessageBox.Show(message);
        }
        #endregion

        #region [ Items Counter ]
        #region [ UpdateOccupiedItemCount ]
        private void UpdateOccupiedItemCount()
        {
            toolStripStatusLabelItemHowMuch.Text = $"占用物品数: {occupiedItemCount}";
        }
        #endregion

        #region [ CountOccupiedItems ]
        private void CountOccupiedItems()
        {
            occupiedItemCount = 0;
            foreach (var itemId in _itemList)
            {
                if (IsItemOccupied(itemId))
                {
                    occupiedItemCount++;
                }
            }
            UpdateOccupiedItemCount();
        }
        #endregion

        #region [ IsItemOccupied ]
        private bool IsItemOccupied(int itemId)
        {
            // 检查项目是否有关联的图形
            var bitmap = Art.GetStatic(itemId, out bool _);
            return bitmap != null;
        }
        #endregion

        #region [ countItemsToolStripMenuItem ]
        private void countItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CountOccupiedItems();
        }
        #endregion
        #endregion

        #region [ toolStripButtonColorImage ]
        private void toolStripButtonColorImage_Click(object sender, EventArgs e)
        {
            // 从 DetailPictureBox 获取选定的图像
            Bitmap selectedImage = DetailPictureBox.Image as Bitmap;

            if (selectedImage == null)
            {
                MessageBox.Show("DetailPictureBox 中未选择图像。", "无图像", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 创建快照以避免对原始位图的跨线程访问
            Bitmap imageCopy = new Bitmap(selectedImage);

            Form colorForm = new Form
            {
                Text = "选定图像的颜色值",
                Width = 1000,
                Height = 970,
                ShowIcon = false
            };

            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical
            };
            colorForm.Controls.Add(splitContainer);

            PictureBox pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.AutoSize
            };

            Panel picturePanel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill
            };
            picturePanel.Controls.Add(pictureBox);

            RichTextBox colorBox = new RichTextBox
            {
                Dock = DockStyle.Fill
            };

            splitContainer.Panel1.Controls.Add(picturePanel);
            splitContainer.Panel2.Controls.Add(colorBox);

            // 计算放大的图像
            int zoomFactor = 10;
            int zoomedWidth = imageCopy.Width * zoomFactor;
            int zoomedHeight = imageCopy.Height * zoomFactor;

            Bitmap zoomedImage = new Bitmap(zoomedWidth, zoomedHeight);
            using (Graphics g = Graphics.FromImage(zoomedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.DrawImage(imageCopy, new Rectangle(0, 0, zoomedWidth, zoomedHeight));
            }
            pictureBox.Image = zoomedImage;

            // 当窗体关闭时释放 zoomedImage
            colorForm.FormClosed += (s, ev) =>
            {
                zoomedImage.Dispose();
                imageCopy.Dispose();
            };

            // lineIndices[x, y] = colorBox 中的行索引，如果跳过则为 -1
            int[,] lineIndices = new int[imageCopy.Width, imageCopy.Height];

            // 在启动后台任务之前显示窗体，以便存在句柄
            colorForm.Show();

            // 存储先前高亮显示的行索引及其像素坐标的变量
            int previousLineIndex = -1;
            int previousPixelX = -1;
            int previousPixelY = -1;

            // 在后台填充 colorBox
            Task.Run(() =>
            {
                int currentLineIndex = 0;
                for (int y = 0; y < imageCopy.Height; y++)
                {
                    for (int x = 0; x < imageCopy.Width; x++)
                    {
                        Color pixelColor = imageCopy.GetPixel(x, y);
                        string hexColor = ColorTranslator.ToHtml(pixelColor);

                        if (hexColor.Equals("#FFFFFF", StringComparison.OrdinalIgnoreCase) ||
                            hexColor.Equals("#000000", StringComparison.OrdinalIgnoreCase))
                        {
                            lineIndices[x, y] = -1;
                            continue;
                        }

                        string colorText = $"像素 ({x}, {y}): 颜色 [R={pixelColor.R}, G={pixelColor.G}, B={pixelColor.B}], 十六进制: {hexColor}";
                        lineIndices[x, y] = currentLineIndex++;

                        // 捕获循环变量以供闭包使用
                        Color capturedColor = pixelColor;
                        string capturedText = colorText;

                        // 错误修复：防止窗体/控件已释放
                        if (colorBox.IsDisposed || !colorBox.IsHandleCreated)
                        {
                            return;
                        }

                        colorBox.Invoke((Action)(() =>
                        {
                            if (colorBox.IsDisposed)
                            {
                                return;
                            }

                            // 追加文本
                            colorBox.AppendText(capturedText);

                            // 5 个空格作为分隔符
                            colorBox.AppendText("     ");

                            // 5 个空格，其背景色为像素颜色作为色样
                            int colorStart = colorBox.Text.Length;
                            colorBox.AppendText("     ");
                            colorBox.Select(colorStart, 5);
                            colorBox.SelectionBackColor = capturedColor;

                            colorBox.AppendText("\n");
                        }));
                    }
                }
            });

            pictureBox.MouseClick += (s, evt) =>
            {
                int x = evt.X / zoomFactor;
                int y = evt.Y / zoomFactor;

                if (x < 0 || x >= imageCopy.Width || y < 0 || y >= imageCopy.Height)
                {
                    return;
                }

                int lineIndex = lineIndices[x, y];

                if (lineIndex >= 0 && lineIndex < colorBox.Lines.Length)
                {
                    // --- 清除先前的高亮 ---
                    if (previousLineIndex >= 0 && previousLineIndex < colorBox.Lines.Length)
                    {
                        int prevStart = colorBox.GetFirstCharIndexFromLine(previousLineIndex);
                        string prevLine = colorBox.Lines[previousLineIndex];
                        int prevLength = prevLine.IndexOf("Hex:") + 10;
                        colorBox.Select(prevStart, prevLength);
                        colorBox.SelectionBackColor = colorBox.BackColor;

                        // 错误修复：使用先前像素的颜色恢复色样，而不是当前像素的颜色
                        Match prevMatch = Regex.Match(prevLine, @"Hex: (#?[0-9A-Fa-f]{6})");
                        if (prevMatch.Success && previousPixelX >= 0 && previousPixelY >= 0)
                        {
                            // 色样位置：文本之后 + 5 个分隔空格
                            int prevSwatchStart = prevStart + prevLine.Length + 5;
                            colorBox.Select(prevSwatchStart, 5);
                            Color prevPixelColor = imageCopy.GetPixel(previousPixelX, previousPixelY);
                            colorBox.SelectionBackColor = prevPixelColor;
                        }
                    }

                    // --- 高亮新行 ---
                    int start = colorBox.GetFirstCharIndexFromLine(lineIndex);
                    int length = colorBox.Lines[lineIndex].IndexOf("Hex:") + 10;
                    colorBox.Select(start, length);
                    colorBox.SelectionBackColor = Color.LightGray;
                    colorBox.ScrollToCaret();

                    // 将十六进制颜色复制到剪贴板
                    string lineText = colorBox.Lines[lineIndex];
                    Match match = Regex.Match(lineText, @"Hex: (#?[0-9A-Fa-f]{6})");
                    if (match.Success)
                    {
                        Clipboard.SetText(match.Groups[1].Value);
                    }

                    previousLineIndex = lineIndex;
                    previousPixelX = x;
                    previousPixelY = y;
                }
                else
                {
                    // 黑色或白色像素：仍将十六进制复制到剪贴板，不弹出消息框
                    Color pixelColor = imageCopy.GetPixel(x, y);
                    string hexColor = ColorTranslator.ToHtml(pixelColor);
                    Clipboard.SetText(hexColor);
                }
            };
        }
        #endregion

        #region [ class PixelBox ]
        public class PixelBox : Control
        {
            public Bitmap Image { get; set; }
            public event Action<int, int> PixelSelected;

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                if (Image != null)
                {
                    for (int x = 0; x < Image.Width; x++)
                    {
                        for (int y = 0; y < Image.Height; y++)
                        {
                            Color pixelColor = Image.GetPixel(x, y);
                            using (Brush brush = new SolidBrush(pixelColor))
                            {
                                e.Graphics.FillRectangle(brush, x * 10, y * 10, 10, 10);
                            }
                        }
                    }
                }
            }

            #region OnMouseDown
            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);

                int x = e.X / 10;
                int y = e.Y / 10;

                PixelSelected?.Invoke(x, y);
            }
            #endregion
        }
        #endregion

        #region [ toolStripButtondrawRhombus ]
        private void toolStripButtondrawRhombus_Click(object sender, EventArgs e)
        {
            ToggleRhombusDrawing();

            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = "sound.wav";
            player.Play();
        }
        #endregion

        #region [ ToggleRhombusDrawing ]
        private void ToggleRhombusDrawing()
        {
            isDrawingRhombusActive = !isDrawingRhombusActive;
            toolStripButtondrawRhombus.Checked = isDrawingRhombusActive;
            DetailPictureBox.Invalidate(); // 强制重绘以显示/隐藏菱形
        }
        #endregion

        #region [ DetailPictureBox_Paint ]
        private void DetailPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (isDrawingRhombusActive)
            {
                DrawRhombus(e.Graphics);
            }
        }
        #endregion

        #region [ DrawRhombus ]
        private void DrawRhombus(Graphics g)
        {
            if (DetailPictureBox.Image == null)
            {
                return;
            }

            Point[] pointsUpper = new Point[4];
            pointsUpper[0] = new Point(DetailPictureBox.Image.Width / 2, 0);
            pointsUpper[1] = new Point(DetailPictureBox.Image.Width / 2 + 22, 22);
            pointsUpper[2] = new Point(DetailPictureBox.Image.Width / 2, 44);
            pointsUpper[3] = new Point(DetailPictureBox.Image.Width / 2 - 22, 22);

            g.DrawPolygon(Pens.Black, pointsUpper);
            g.DrawLine(Pens.Black, pointsUpper[0], new Point(pointsUpper[0].X, 0));
            g.DrawLine(Pens.Black, pointsUpper[1], new Point(pointsUpper[1].X, 0));
            g.DrawLine(Pens.Black, pointsUpper[3], new Point(pointsUpper[3].X, 0));

            int lineWidth = 100;
            int lineStartX = (DetailPictureBox.Image.Width - lineWidth) / 2;
            int lineEndX = lineStartX + lineWidth;
            int imageHeight = DetailPictureBox.Image.Height;
            g.DrawLine(Pens.Black, new Point(lineStartX, imageHeight - 66), new Point(lineEndX, imageHeight - 66));

            Point[] pointsLower = new Point[4];
            pointsLower[0] = new Point(DetailPictureBox.Image.Width / 2, imageHeight - 66);
            pointsLower[1] = new Point(DetailPictureBox.Image.Width / 2 + 22, imageHeight - 88);
            pointsLower[2] = new Point(DetailPictureBox.Image.Width / 2, imageHeight - 110);
            pointsLower[3] = new Point(DetailPictureBox.Image.Width / 2 - 22, imageHeight - 88);

            g.DrawPolygon(Pens.Black, pointsLower);
            g.DrawLine(Pens.Black, pointsLower[0], new Point(pointsLower[0].X, pointsLower[0].Y - 22));
            g.DrawLine(Pens.Black, pointsLower[1], new Point(pointsLower[1].X, pointsLower[1].Y - 22));
            g.DrawLine(Pens.Black, pointsLower[3], new Point(pointsLower[3].X, pointsLower[3].Y - 22));

            g.DrawLine(Pens.Black, pointsUpper[0], pointsLower[0]);
            g.DrawLine(Pens.Black, pointsUpper[1], pointsLower[1]);
            g.DrawLine(Pens.Black, pointsUpper[3], pointsLower[3]);
        }
        #endregion

        #region [ DetailPictureBox_MouseDoubleClick ] // 双击 - 图片框 1000x1000 并缩放
        private float zoomFactor = 1.0f; // 初始缩放因子
        private const float zoomStep = 0.1f; // 缩放增量/减量

        #region [ DetailPictureBox_MouseDoubleClick ]
        private void DetailPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 检查 ItemsTileView 中是否选择了任何图像
            if (ItemsTileView.SelectedIndices.Count == 0)
            {
                MessageBox.Show("请从列表中选择一个图像。", "未选择图像", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 如果窗体未打开或已释放，则创建一个新的
            if (imageForm == null || imageForm.IsDisposed)
            {
                // 初始化一个新窗体以显示图像
                imageForm = new Form
                {
                    Text = "放大的图像视图",
                    Size = new Size(1020, 1020), // 1000x1000 加上边距
                    StartPosition = FormStartPosition.Manual,
                    Location = new Point(this.Location.X + this.Width + 10, this.Location.Y), // 定位在主窗体旁边
                    FormBorderStyle = FormBorderStyle.FixedToolWindow, // 简单窗口，无最大化按钮
                    ShowIcon = false // 禁用标题栏中的图标
                };

                // 初始化一个新的 PictureBox 以显示选定的图像
                imagePictureBox = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom, // 启用缩放支持
                };

                // 添加 MouseWheel 事件处理程序以实现缩放功能
                imagePictureBox.MouseWheel += ImagePictureBox_MouseWheel;

                imageForm.Controls.Add(imagePictureBox);
                imageForm.Show(); // 将窗体显示为非模态窗口

                // 添加选择更改事件以在更改选择时动态更新图像
                ItemsTileView.ItemSelectionChanged += (s, ev) =>
                {
                    // 仅当选择新项目且窗体打开时才更新
                    if (ev.IsSelected && imageForm != null && !imageForm.IsDisposed)
                    {
                        UpdateImageInForm();
                    }
                };
            }

            // 更新 PictureBox 中图像的方法
            void UpdateImageInForm()
            {
                int selectedIndex = ItemsTileView.SelectedIndices[0];
                var bitmap = Art.GetStatic(_itemList[selectedIndex]);

                if (bitmap != null)
                {
                    // 加载新图像时重置缩放因子
                    zoomFactor = 1.0f;
                    imagePictureBox.Image = bitmap;
                    AdjustZoom();
                }
                else
                {
                    MessageBox.Show("无法加载选定的图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 初始图像加载
            UpdateImageInForm();
        }
        #endregion

        #region [ ImagePictureBox_MouseWheel ]
        // 处理 MouseWheel 事件以实现放大和缩小
        private void ImagePictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            // 根据滚动方向更新缩放因子
            if (e.Delta > 0)
            {
                zoomFactor += zoomStep; // 放大
            }
            else if (e.Delta < 0 && zoomFactor > zoomStep)
            {
                zoomFactor -= zoomStep; // 缩小
            }
            AdjustZoom();
        }
        #endregion

        #region [ AdjustZoom ]
        // 根据当前缩放因子调整 PictureBox 并使其居中
        private void AdjustZoom()
        {
            if (imagePictureBox.Image != null)
            {
                int newWidth = (int)(imagePictureBox.Image.Width * zoomFactor);
                int newHeight = (int)(imagePictureBox.Image.Height * zoomFactor);

                // 根据缩放设置大小
                imagePictureBox.Size = new Size(newWidth, newHeight);

                // 将 PictureBox 置于窗体中心
                imagePictureBox.Location = new Point(
                    (imageForm.ClientSize.Width - newWidth) / 2,
                    (imageForm.ClientSize.Height - newHeight) / 2
                );
            }
        }
        #endregion
        #endregion

        #region [ RemoveAllToolStripMenuItem ]
        private void RemoveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("您确定要删除所有项目吗？", "删除所有元素", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }

            // 遍历 _itemList 中的所有项目并删除
            foreach (var itemId in _itemList.ToList())
            {
                if (Art.IsValidStatic(itemId))
                {
                    Art.RemoveStatic(itemId);
                    ControlEvents.FireItemChangeEvent(this, itemId);
                }
            }

            // 清空列表并刷新 UI
            _itemList.Clear();
            ItemsTileView.VirtualListSize = _itemList.Count;
            SelectedGraphicId = 0; // 无选择
            ItemsTileView.Invalidate();

            // 更新选项
            Options.ChangedUltimaClass["Art"] = true;
            MessageBox.Show("所有元素已被删除。", "删除完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion


        #region [ toolStripButtonArtWorkGallery_Click ]
        private ArtworkGallery artworkGalleryInstance;
        private void toolStripButtonArtWorkGallery_Click(object sender, EventArgs e)
        {
            if (artworkGalleryInstance == null || artworkGalleryInstance.IsDisposed)
            {
                artworkGalleryInstance = new ArtworkGallery();
                artworkGalleryInstance.Show();
                artworkGalleryInstance.InitializeGallery(_selectedGraphicId); // 新增
            }
            else
            {
                artworkGalleryInstance.Focus();
                artworkGalleryInstance.InitializeGallery(_selectedGraphicId); // 新增
            }
        }
        #endregion
    }
}
