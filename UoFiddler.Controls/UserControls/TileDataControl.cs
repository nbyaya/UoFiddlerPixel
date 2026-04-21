/***************************************************************************
 *
 * $作者: Turley
 *
 * "啤酒软件许可协议"
 * 只要您保留此声明，就可以对此软件做任何您想做的事情。
 * 如果我们有一天相遇，并且您认为这个软件值得，
 * 您可以请我喝一杯啤酒作为回报。
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Forms;
using UoFiddler.Controls.Helpers;
using System.Xml;

namespace UoFiddler.Controls.UserControls
{
    public partial class TileDataControl : UserControl
    {
        private Image _image;        

        // TileFlag中英文映射字典
        private static readonly Dictionary<string, string> TileFlagTranslations = new Dictionary<string, string>
        {
            {"Background", "背景"},
            {"Weapon", "武器"},
            {"Transparent", "透明"},
            {"Translucent", "半透明"},
            {"Wall", "墙壁"},
            {"Damaging", "造成伤害"},
            {"Impassable", "不可通行"},
            {"Wet", "潮湿"},
            {"Unknown1", "未知1"},
            {"Surface", "表面"},
            {"Bridge", "桥梁"},
            {"Generic", "通用"},
            {"Window", "窗户"},
            {"NoShoot", "不可射击"},
            {"ArticleA", "冠词A"},
            {"ArticleAn", "冠词An"},
            {"ArticleThe", "冠词The"},
            {"Foliage", "植物"},
            {"PartialHue", "部分色调"},
            {"NoHouse", "不可建造房屋"},
            {"Map", "地图"},
            {"Container", "容器"},
            {"Wearable", "可穿戴"},
            {"LightSource", "光源"},
            {"Animation", "动画"},
            {"HoverOver", "悬停时高亮"},
            {"NoDiagonal", "无对角线"},
            {"Armor", "护甲"},
            {"Roof", "屋顶"},
            {"Door", "门"},
            {"StairBack", "楼梯背面"},
            {"StairRight", "楼梯右侧"},
            {"AlphaBlend", "Alpha混合"},
            {"UseNewArt", "使用新艺术"},
            {"ArtUsed", "艺术已使用"},
            {"Unused8", "未使用8"},
            {"NoShadow", "无阴影"},
            {"PixelBleed", "像素出血"},
            {"PlayAnimOnce", "动画仅播放一次"},
            {"MultiMovable", "可多重移动"}
        };

        #region [ TileDataControl ]
        public TileDataControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            AssignToolTipsToLabels();

            toolStripComboBox1.ComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            toolStripComboBox1.ComboBox.DrawItem += new DrawItemEventHandler(ToolStripComboBox1_DrawItem);
            LoadImages();  // 调用加载图像的方法

            toolStripComboBox1.SelectedIndexChanged += ToolStripComboBox1_SelectedIndexChanged;

            _refMarker = this;

            treeViewItem.BeforeSelect += TreeViewItemOnBeforeSelect;

            saveDirectlyOnChangesToolStripMenuItem.Checked = Options.TileDataDirectlySaveOnChange;
            saveDirectlyOnChangesToolStripMenuItem.CheckedChanged += SaveDirectlyOnChangesToolStripMenuItemOnCheckedChanged;

            ControlEvents.FilePathChangeEvent += OnFilePathChangeEvent;
            ControlEvents.TileDataChangeEvent += OnTileDataChangeEvent;

            LabelDecimalAdress.Text = "";

            tbClassicUOPfad.Text = Properties.Settings.Default.CUOTestPath; // 加载设置路径
        }
        #endregion

        #region [ InitLandTilesFlagsCheckBoxes ]
        private void InitLandTilesFlagsCheckBoxes()
        {
            checkedListBox2.BeginUpdate();
            try
            {
                checkedListBox2.Items.Clear();

                string[] enumNames = Enum.GetNames(typeof(TileFlag));
                int maxLength = Art.IsUOAHS() ? enumNames.Length : (enumNames.Length / 2) + 1;
                for (int i = 1; i < maxLength; ++i)
                {
                    string englishName = enumNames[i];
                    string chineseName = GetChineseTileFlagName(englishName);
                    checkedListBox2.Items.Add(chineseName, false);
                }

                // TODO: 目前我们呈现所有标志。需要研究土地瓦片是否只使用选定的标志还是全部？
                // TODO: 看起来只使用了一小部分，但仍然与下面这5个不同
                //checkedListBox2.Items.Add(GetChineseTileFlagName("Damaging"), false);
                //checkedListBox2.Items.Add(GetChineseTileFlagName("Wet"), false);
                //checkedListBox2.Items.Add(GetChineseTileFlagName("Impassable"), false);
                //checkedListBox2.Items.Add(GetChineseTileFlagName("Wall"), false);
                //checkedListBox2.Items.Add(GetChineseTileFlagName("NoDiagonal"), false);
            }
            finally
            {
                checkedListBox2.EndUpdate();
            }
        }
        #endregion

        #region ItemControl 选择 PictureBox 刷新
        public void RefreshPictureBoxItem()
        {
            pictureBoxItem.Refresh();
        }
        #endregion

        #region [ InitItemsFlagsCheckBoxes ]
        private void InitItemsFlagsCheckBoxes()
        {
            checkedListBox1.BeginUpdate();
            try
            {
                checkedListBox1.Items.Clear();

                string[] enumNames = Enum.GetNames(typeof(TileFlag));
                int maxLength = Art.IsUOAHS() ? enumNames.Length : (enumNames.Length / 2) + 1;
                for (int i = 1; i < maxLength; ++i)
                {
                    string englishName = enumNames[i];
                    string chineseName = GetChineseTileFlagName(englishName);
                    checkedListBox1.Items.Add(chineseName, false);
                }
            }
            finally
            {
                checkedListBox1.EndUpdate();
            }
        }
        #endregion

        #region [ GetChineseTileFlagName ]
        private string GetChineseTileFlagName(string englishName)
        {
            if (TileFlagTranslations.TryGetValue(englishName, out string chineseName))
            {
                return chineseName;
            }
            return englishName; // 如果没有找到对应的翻译，返回原英文名称
        }
        #endregion

        #region [ GetEnglishTileFlagName ]
        private string GetEnglishTileFlagName(string chineseName)
        {
            foreach (var pair in TileFlagTranslations)
            {
                if (pair.Value == chineseName)
                {
                    return pair.Key;
                }
            }
            return chineseName; // 如果没有找到对应的英文，返回原中文名称
        }
        #endregion

        private Settings _copiedSettings = null; // 存储复制设置的全局变量
        private List<TreeNode> _selectedNodes = new List<TreeNode>(); // 多选变量
        private LandSettings _copiedLandSettings = null; // 存储复制土地设置的全局变量

        private static TileDataControl _refMarker;
        private bool _changingIndex;

        public bool IsLoaded { get; private set; }

        private int? _reselectGraphic;
        private bool? _reselectGraphicLand;

        #region [ Select ]
        public static void Select(int graphic, bool land)
        {
            if (!_refMarker.IsLoaded)
            {
                _refMarker.OnLoad(_refMarker, EventArgs.Empty);
                _refMarker._reselectGraphic = graphic;
                _refMarker._reselectGraphicLand = land;
            }

            SearchGraphic(graphic, land);
        }
        #endregion

        #region [ SearchGraphic ]
        public static bool SearchGraphic(int graphic, bool land)
        {
            const int index = 0;
            if (land)
            {
                for (int i = index; i < _refMarker.treeViewLand.Nodes.Count; ++i)
                {
                    TreeNode node = _refMarker.treeViewLand.Nodes[i];
                    if (node.Tag == null || (int)node.Tag != graphic)
                    {
                        continue;
                    }

                    _refMarker.tabcontrol.SelectTab(1);
                    _refMarker.treeViewLand.SelectedNode = node;
                    node.EnsureVisible();
                    return true;
                }
            }
            else
            {
                for (int i = index; i < _refMarker.treeViewItem.Nodes.Count; ++i)
                {
                    for (int j = 0; j < _refMarker.treeViewItem.Nodes[i].Nodes.Count; ++j)
                    {
                        TreeNode node = _refMarker.treeViewItem.Nodes[i].Nodes[j];
                        if (node.Tag == null || (int)node.Tag != graphic)
                        {
                            continue;
                        }

                        _refMarker.tabcontrol.SelectTab(0);
                        _refMarker.treeViewItem.SelectedNode = node;
                        node.EnsureVisible();
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region [ SearchName ]
        public static bool SearchName(string name, bool next, bool land)
        {
            int index = 0;

            var searchMethod = SearchHelper.GetSearchMethod();

            if (land)
            {
                if (next)
                {
                    if (_refMarker.treeViewLand.SelectedNode.Index >= 0)
                    {
                        index = _refMarker.treeViewLand.SelectedNode.Index + 1;
                    }

                    if (index >= _refMarker.treeViewLand.Nodes.Count)
                    {
                        index = 0;
                    }
                }

                for (int i = index; i < _refMarker.treeViewLand.Nodes.Count; ++i)
                {
                    TreeNode node = _refMarker.treeViewLand.Nodes[i];
                    if (node.Tag == null)
                    {
                        continue;
                    }

                    var searchResult = searchMethod(name, TileData.LandTable[(int)node.Tag].Name);
                    if (!searchResult.EntryFound)
                    {
                        continue;
                    }

                    _refMarker.tabcontrol.SelectTab(1);
                    _refMarker.treeViewLand.SelectedNode = node;
                    node.EnsureVisible();
                    return true;
                }
            }
            else
            {
                int sIndex = 0;
                if (next && _refMarker.treeViewItem.SelectedNode != null)
                {
                    if (_refMarker.treeViewItem.SelectedNode.Parent != null)
                    {
                        index = _refMarker.treeViewItem.SelectedNode.Parent.Index;
                        sIndex = _refMarker.treeViewItem.SelectedNode.Index + 1;
                    }
                    else
                    {
                        index = _refMarker.treeViewItem.SelectedNode.Index;
                        sIndex = 0;
                    }
                }

                for (int i = index; i < _refMarker.treeViewItem.Nodes.Count; ++i)
                {
                    for (int j = sIndex; j < _refMarker.treeViewItem.Nodes[i].Nodes.Count; ++j)
                    {
                        TreeNode node = _refMarker.treeViewItem.Nodes[i].Nodes[j];
                        if (node.Tag == null)
                        {
                            continue;
                        }

                        var searchResult = searchMethod(name, TileData.ItemTable[(int)node.Tag].Name);
                        if (!searchResult.EntryFound)
                        {
                            continue;
                        }

                        _refMarker.tabcontrol.SelectTab(0);
                        _refMarker.treeViewItem.SelectedNode = node;
                        node.EnsureVisible();
                        return true;
                    }

                    sIndex = 0;
                }
            }

            return false;
        }
        #endregion

        #region [ ApplyFilterItem ]
        public void ApplyFilterItem(ItemData item)
        {
            treeViewItem.BeginUpdate();
            treeViewItem.Nodes.Clear();

            var nodes = new List<TreeNode>();
            var nodesSa = new List<TreeNode>();
            var nodesHsa = new List<TreeNode>();

            for (int i = 0; i < TileData.ItemTable.Length; ++i)
            {
                if (!string.IsNullOrEmpty(item.Name) && TileData.ItemTable[i].Name.IndexOf(item.Name, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (item.Animation != 0 && TileData.ItemTable[i].Animation != item.Animation)
                {
                    continue;
                }

                if (item.Weight != 0 && TileData.ItemTable[i].Weight != item.Weight)
                {
                    continue;
                }

                if (item.Quality != 0 && TileData.ItemTable[i].Quality != item.Quality)
                {
                    continue;
                }

                if (item.Quantity != 0 && TileData.ItemTable[i].Quantity != item.Quantity)
                {
                    continue;
                }

                if (item.Hue != 0 && TileData.ItemTable[i].Hue != item.Hue)
                {
                    continue;
                }

                if (item.StackingOffset != 0 && TileData.ItemTable[i].StackingOffset != item.StackingOffset)
                {
                    continue;
                }

                if (item.Value != 0 && TileData.ItemTable[i].Value != item.Value)
                {
                    continue;
                }

                if (item.Height != 0 && TileData.ItemTable[i].Height != item.Height)
                {
                    continue;
                }

                if (item.MiscData != 0 && TileData.ItemTable[i].MiscData != item.MiscData)
                {
                    continue;
                }

                if (item.Unk2 != 0 && TileData.ItemTable[i].Unk2 != item.Unk2)
                {
                    continue;
                }

                if (item.Unk3 != 0 && TileData.ItemTable[i].Unk3 != item.Unk3)
                {
                    continue;
                }

                if (item.Flags != 0 && (TileData.ItemTable[i].Flags & item.Flags) == 0)
                {
                    continue;
                }

                TreeNode node = new TreeNode(string.Format("0x{0:X4} ({0}) {1}", i, TileData.ItemTable[i].Name))
                {
                    Tag = i
                };

                if (i < 0x4000)
                {
                    nodes.Add(node);
                }
                else if (i < 0x8000)
                {
                    nodesSa.Add(node);
                }
                else
                {
                    nodesHsa.Add(node);
                }
            }

            if (nodes.Count > 0)
            {
                treeViewItem.Nodes.Add(new TreeNode("AOS - ML", nodes.ToArray()));
            }

            if (nodesSa.Count > 0)
            {
                treeViewItem.Nodes.Add(new TreeNode("Stygian Abyss", nodesSa.ToArray()));
            }

            if (nodesHsa.Count > 0)
            {
                treeViewItem.Nodes.Add(new TreeNode("Adventures High Seas", nodesHsa.ToArray()));
            }

            treeViewItem.EndUpdate();

            if (treeViewItem.Nodes.Count > 0 && _refMarker.treeViewItem.Nodes[0].Nodes.Count > 0)
            {
                treeViewItem.SelectedNode = _refMarker.treeViewItem.Nodes[0].Nodes[0];
            }
        }
        #endregion

        #region [ ApplyFilterLand ]
        public static void ApplyFilterLand(LandData land)
        {
            _refMarker.treeViewLand.BeginUpdate();
            _refMarker.treeViewLand.Nodes.Clear();
            var nodes = new List<TreeNode>();
            for (int i = 0; i < TileData.LandTable.Length; ++i)
            {
                if (!string.IsNullOrEmpty(land.Name) && TileData.ItemTable[i].Name.IndexOf(land.Name, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (land.TextureId != 0 && TileData.LandTable[i].TextureId != land.TextureId)
                {
                    continue;
                }

                if (land.Flags != 0 && (TileData.LandTable[i].Flags & land.Flags) == 0)
                {
                    continue;
                }

                TreeNode node = new TreeNode(string.Format("0x{0:X4} ({0}) {1}", i, TileData.LandTable[i].Name))
                {
                    Tag = i
                };
                nodes.Add(node);
            }

            _refMarker.treeViewLand.Nodes.AddRange(nodes.ToArray());
            _refMarker.treeViewLand.EndUpdate();

            if (_refMarker.treeViewLand.Nodes.Count > 0)
            {
                _refMarker.treeViewLand.SelectedNode = _refMarker.treeViewLand.Nodes[0];
            }
        }
        #endregion

        #region [ Reload ]
        private void Reload()
        {
            if (IsLoaded)
            {
                OnLoad(this, new MyEventArgs(MyEventArgs.Types.ForceReload));
            }
        }
        #endregion

        #region [ OnLoad ]
        public void OnLoad(object sender, EventArgs e)
        {
            if (IsAncestorSiteInDesignMode || FormsDesignerHelper.IsInDesignMode())
            {
                return;
            }

            if (_reselectGraphic != null && _reselectGraphicLand != null)
            {
                SearchGraphic(_reselectGraphic.Value, _reselectGraphicLand.Value);
                _reselectGraphic = null;
                _reselectGraphicLand = null;
            }

            if (IsLoaded && (!(e is MyEventArgs args) || args.Type != MyEventArgs.Types.ForceReload))
            {
                return;
            }

            InitItemsFlagsCheckBoxes();
            InitLandTilesFlagsCheckBoxes();

            Cursor.Current = Cursors.WaitCursor;
            Options.LoadedUltimaClass["TileData"] = true;
            Options.LoadedUltimaClass["Art"] = true;
            treeViewItem.BeginUpdate();
            treeViewItem.Nodes.Clear();
            if (TileData.ItemTable != null)
            {
                var nodes = new TreeNode[0x4000];
                for (int i = 0; i < 0x4000; ++i)
                {
                    nodes[i] = new TreeNode(string.Format("0x{0:X4} ({0}) {1}", i, TileData.ItemTable[i].Name))
                    {
                        Tag = i
                    };
                }
                treeViewItem.Nodes.Add(new TreeNode("黑暗纪元 - 月光遗迹：AOS - ML", nodes));

                if (TileData.ItemTable.Length > 0x4000) // SA
                {
                    nodes = new TreeNode[0x4000];
                    for (int i = 0; i < 0x4000; ++i)
                    {
                        int j = i + 0x4000;
                        nodes[i] = new TreeNode(string.Format("0x{0:X4} ({0}) {1}", j, TileData.ItemTable[j].Name))
                        {
                            Tag = j
                        };
                    }
                    treeViewItem.Nodes.Add(new TreeNode("冥河深渊-Stygian Abyss", nodes));
                }

                if (TileData.ItemTable.Length > 0x8000) // AHS
                {
                    nodes = new TreeNode[0x8000];
                    for (int i = 0; i < 0x8000; ++i)
                    {
                        int j = i + 0x8000;
                        nodes[i] = new TreeNode(string.Format("0x{0:X4} ({0}) {1}", j, TileData.ItemTable[j].Name))
                        {
                            Tag = j
                        };
                    }
                    treeViewItem.Nodes.Add(new TreeNode("无尽的航程-Adventures High Seas", nodes));
                }
                else
                {
                    treeViewItem.ExpandAll();
                }
            }
            treeViewItem.EndUpdate();

            treeViewLand.BeginUpdate();
            treeViewLand.Nodes.Clear();
            if (TileData.LandTable != null)
            {
                var nodes = new TreeNode[TileData.LandTable.Length];
                for (int i = 0; i < TileData.LandTable.Length; ++i)
                {
                    nodes[i] = new TreeNode(string.Format("0x{0:X4} ({0}) {1}", i, TileData.LandTable[i].Name))
                    {
                        Tag = i
                    };
                }
                treeViewLand.Nodes.AddRange(nodes);
            }
            treeViewLand.EndUpdate();

            IsLoaded = true;
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region [ OnFilePathChangeEvent ]
        private void OnFilePathChangeEvent()
        {
            Reload();
        }
        #endregion

        #region [ OnTileDataChangeEvent ]
        private void OnTileDataChangeEvent(object sender, int index)
        {
            if (!IsLoaded)
            {
                return;
            }

            if (sender.Equals(this))
            {
                return;
            }

            if (index > 0x3FFF) // 物品
            {
                if (treeViewItem.SelectedNode == null)
                {
                    return;
                }

                if ((int)treeViewItem.SelectedNode.Tag == index)
                {
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    AfterSelectTreeViewItem(this, new TreeViewEventArgs(treeViewItem.SelectedNode));
                }
                else
                {
                    foreach (TreeNode parentNode in treeViewItem.Nodes)
                    {
                        foreach (TreeNode node in parentNode.Nodes)
                        {
                            if ((int)node.Tag != index)
                            {
                                continue;
                            }

                            node.ForeColor = Color.Red;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (treeViewLand.SelectedNode == null)
                {
                    return;
                }

                if ((int)treeViewLand.SelectedNode.Tag == index)
                {
                    treeViewLand.SelectedNode.ForeColor = Color.Red;
                    AfterSelectTreeViewLand(this, new TreeViewEventArgs(treeViewLand.SelectedNode));
                }
                else
                {
                    foreach (TreeNode node in treeViewLand.Nodes)
                    {
                        if ((int)node.Tag != index)
                        {
                            continue;
                        }

                        node.ForeColor = Color.Red;
                        break;
                    }
                }
            }
        }
        #endregion        

        #region [ AfterSelectTreeViewItem ]
        private void AfterSelectTreeViewItem(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag == null)
            {
                return;
            }

            // 检查是否按下了Ctrl键
            bool isCtrlPressed = (Control.ModifierKeys & Keys.Control) == Keys.Control;

            if (isCtrlPressed)
            {
                // 多选：从列表中添加或删除节点
                if (_selectedNodes.Contains(e.Node))
                {
                    // 如果节点已被选中，则移除
                    _selectedNodes.Remove(e.Node);
                    e.Node.BackColor = treeViewItem.BackColor;  // 取消选中
                }
                else
                {
                    // 如果节点未选中，则添加
                    _selectedNodes.Add(e.Node);
                    e.Node.BackColor = Color.LightBlue;  // 视觉高亮
                }
            }
            else
            {
                // 没有按Ctrl的正常选择：清除之前的选择
                ClearPreviousSelection();
                _selectedNodes.Add(e.Node);
                e.Node.BackColor = Color.LightBlue;  // 视觉高亮
            }

            // 对所有选中的节点应用逻辑
            foreach (var node in _selectedNodes)
            {
                ApplySettingsToNode(node);
            }
        }
        #endregion

        #region ClearPreviousSelection
        private void ClearPreviousSelection()
        {
            foreach (TreeNode node in _selectedNodes)
            {
                node.BackColor = treeViewItem.BackColor; // 将背景色重置为默认
            }
            _selectedNodes.Clear();
        }
        #endregion

        #region [ ApplySettingsToNode ]
        private void ApplySettingsToNode(TreeNode node)
        {
            // 旧逻辑
            int index = (int)node.Tag;

            Bitmap bit = Art.GetStatic(index);
            if (bit != null)
            {
                Bitmap newBit = new Bitmap(pictureBoxItem.Size.Width, pictureBoxItem.Size.Height);
                using (Graphics newGraph = Graphics.FromImage(newBit))
                {
                    newGraph.Clear(Color.FromArgb(-1));
                    newGraph.DrawImage(bit, (pictureBoxItem.Size.Width - bit.Width) / 2, 1);
                }

                pictureBoxItem.Image?.Dispose();
                pictureBoxItem.Image = newBit;

                _originalImage = pictureBoxItem.Image; // 更新原始图像 -> 缩放

                // 更新缩放窗体中的图像
                if (_zoomForm != null && _zoomForm.Visible)
                {
                    UpdateZoomFormImage();
                }

            }
            else
            {
                pictureBoxItem.Image = null;
            }

            ItemData data = TileData.ItemTable[index];
            _changingIndex = true;
            textBoxName.Text = data.Name;
            textBoxAnim.Text = data.Animation.ToString();
            textBoxWeight.Text = data.Weight.ToString();
            textBoxQuality.Text = data.Quality.ToString();
            textBoxQuantity.Text = data.Quantity.ToString();
            textBoxHue.Text = data.Hue.ToString();
            textBoxStackOff.Text = data.StackingOffset.ToString();
            textBoxValue.Text = data.Value.ToString();
            textBoxHeigth.Text = data.Height.ToString();
            textBoxUnk1.Text = data.MiscData.ToString();
            textBoxUnk2.Text = data.Unk2.ToString();
            textBoxUnk3.Text = data.Unk3.ToString();

            Array enumValues = Enum.GetValues(typeof(TileFlag));
            int maxLength = Art.IsUOAHS() ? enumValues.Length : (enumValues.Length / 2) + 1;
            for (int i = 1; i < maxLength; ++i)
            {
                string englishName = Enum.GetName(typeof(TileFlag), enumValues.GetValue(i));
                string chineseName = GetChineseTileFlagName(englishName);
                int itemIndex = checkedListBox1.Items.IndexOf(chineseName);
                if (itemIndex >= 0)
                {
                    checkedListBox1.SetItemChecked(itemIndex, (data.Flags & (TileFlag)enumValues.GetValue(i)) != 0);
                }
            }
            _changingIndex = false;
        }
        #endregion

        #region [ AfterSelectTreeViewLand ]
        private void AfterSelectTreeViewLand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }

            int index = (int)e.Node.Tag;

            LabelDecimalAdress.Text = index.ToString();

            Bitmap bit = Art.GetLand(index);
            if (bit != null)
            {
                Bitmap newBit = new Bitmap(pictureBoxLand.Size.Width, pictureBoxLand.Size.Height);
                using (Graphics newGraph = Graphics.FromImage(newBit))
                {
                    newGraph.Clear(Color.FromArgb(-1));
                    newGraph.DrawImage(bit, (pictureBoxLand.Size.Width - bit.Width) / 2, 1);
                }

                pictureBoxLand.Image?.Dispose();
                pictureBoxLand.Image = newBit;
            }
            else
            {
                pictureBoxLand.Image = null;
            }

            LandData data = TileData.LandTable[index];
            _changingIndex = true;
            textBoxNameLand.Text = data.Name;
            textBoxTexID.Text = data.TextureId.ToString();

            Array enumValues = Enum.GetValues(typeof(TileFlag));
            int maxLength = Art.IsUOAHS() ? enumValues.Length : (enumValues.Length / 2) + 1;
            for (int i = 1; i < maxLength; ++i)
            {
                string englishName = Enum.GetName(typeof(TileFlag), enumValues.GetValue(i));
                string chineseName = GetChineseTileFlagName(englishName);
                int itemIndex = checkedListBox2.Items.IndexOf(chineseName);
                if (itemIndex >= 0)
                {
                    checkedListBox2.SetItemChecked(itemIndex, (data.Flags & (TileFlag)enumValues.GetValue(i)) != 0);
                }
            }

            _changingIndex = false;
        }
        #endregion

        #region [ OnClickSaveTiledata ]
        private void OnClickSaveTiledata(object sender, EventArgs e)
        {
            string path = Options.OutputPath;
            string fileName = Path.Combine(path, "tiledata.mul");
            TileData.SaveTileData(fileName);
            MessageBox.Show($"TileData已保存到 {fileName}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            Options.ChangedUltimaClass["TileData"] = false;
        }
        #endregion

        #region [ OnClickSaveChanges ] 
        private void OnClickSaveChanges(object sender, EventArgs e)
        {
            if (tabcontrol.SelectedIndex == 0) // 物品
            {
                // 检查是否有节点被标记
                if (_selectedNodes.Count == 0)
                {
                    MessageBox.Show("未选择任何节点。");
                    return;
                }

                // 为每个选中的节点保存更改
                foreach (TreeNode node in _selectedNodes)
                {
                    if (node?.Tag == null)
                    {
                        continue;
                    }

                    int index = (int)node.Tag;
                    ItemData item = TileData.ItemTable[index];
                    string name = textBoxName.Text;
                    if (name.Length > 20)
                    {
                        name = name.Substring(0, 20);
                    }

                    item.Name = name;
                    node.Text = string.Format("0x{0:X4} ({0}) {1}", index, name);

                    if (short.TryParse(textBoxAnim.Text, out short shortRes))
                    {
                        item.Animation = shortRes;
                    }

                    if (byte.TryParse(textBoxWeight.Text, out byte byteRes))
                    {
                        item.Weight = byteRes;
                    }

                    if (byte.TryParse(textBoxQuality.Text, out byteRes))
                    {
                        item.Quality = byteRes;
                    }

                    if (byte.TryParse(textBoxQuantity.Text, out byteRes))
                    {
                        item.Quantity = byteRes;
                    }

                    if (byte.TryParse(textBoxHue.Text, out byteRes))
                    {
                        item.Hue = byteRes;
                    }

                    if (byte.TryParse(textBoxStackOff.Text, out byteRes))
                    {
                        item.StackingOffset = byteRes;
                    }

                    if (byte.TryParse(textBoxValue.Text, out byteRes))
                    {
                        item.Value = byteRes;
                    }

                    if (byte.TryParse(textBoxHeigth.Text, out byteRes))
                    {
                        item.Height = byteRes;
                    }

                    if (short.TryParse(textBoxUnk1.Text, out shortRes))
                    {
                        item.MiscData = shortRes;
                    }

                    if (byte.TryParse(textBoxUnk2.Text, out byteRes))
                    {
                        item.Unk2 = byteRes;
                    }

                    if (byte.TryParse(textBoxUnk3.Text, out byteRes))
                    {
                        item.Unk3 = byteRes;
                    }

                    // 设置标志
                    item.Flags = TileFlag.None;
                    for (int i = 0; i < checkedListBox1.Items.Count; ++i)
                    {
                        if (checkedListBox1.GetItemChecked(i))
                        {
                            string chineseName = checkedListBox1.Items[i].ToString();
                            string englishName = GetEnglishTileFlagName(chineseName);
                            if (Enum.TryParse(englishName, out TileFlag flag))
                            {
                                item.Flags |= flag;
                            }
                        }
                    }

                    TileData.ItemTable[index] = item;
                    node.ForeColor = Color.Red; // 将节点标记为"已更改"

                    // 触发事件和选项更改
                    Options.ChangedUltimaClass["TileData"] = true;
                    ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
                }

                // 保存提示
                if (_toolStripButton7IsActive && memorySaveWarningToolStripMenuItem.Checked)
                {
                    if (_playCustomSound) // 如果使用声音而不是消息框
                    {
                        SoundPlayer player = new SoundPlayer();
                        player.SoundLocation = "sound.wav";
                        player.Play();
                    }
                    else
                    {
                        // 关于保存的提示
                        MessageBox.Show("更改已保存到内存。点击'保存Tiledata'写入文件。", "已保存",
                            MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
            }
            else // 土地
            {
                if (treeViewLand.SelectedNode == null)
                {
                    return;
                }

                int index = (int)treeViewLand.SelectedNode.Tag;
                LandData land = TileData.LandTable[index];
                string name = textBoxNameLand.Text;
                if (name.Length > 20)
                {
                    name = name.Substring(0, 20);
                }

                land.Name = name;
                treeViewLand.SelectedNode.Text = $"0x{index:X4} {name}";
                if (ushort.TryParse(textBoxTexID.Text, out ushort shortRes))
                {
                    land.TextureId = shortRes;
                }

                land.Flags = TileFlag.None;
                for (int i = 0; i < checkedListBox2.Items.Count; ++i)
                {
                    if (checkedListBox2.GetItemChecked(i))
                    {
                        string chineseName = checkedListBox2.Items[i].ToString();
                        string englishName = GetEnglishTileFlagName(chineseName);
                        if (Enum.TryParse(englishName, out TileFlag flag))
                        {
                            land.Flags |= flag;
                        }
                    }
                }

                TileData.LandTable[index] = land;
                Options.ChangedUltimaClass["TileData"] = true;
                ControlEvents.FireTileDataChangeEvent(this, index);
                treeViewLand.SelectedNode.ForeColor = Color.Red;

                if (_toolStripButton7IsActive && memorySaveWarningToolStripMenuItem.Checked)
                {
                    if (_playCustomSound)
                    {
                        SoundPlayer player = new SoundPlayer();
                        player.SoundLocation = "sound.wav";
                        player.Play();
                    }
                    else
                    {
                        MessageBox.Show("更改已保存到内存。点击'保存Tiledata'写入文件。", "已保存",
                            MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
            }
        }
        #endregion

        #region [ SaveDirectlyOnChangesToolStripMenuItemOnCheckedChanged ]
        private void SaveDirectlyOnChangesToolStripMenuItemOnCheckedChanged(object sender, EventArgs eventArgs)
        {
            Options.TileDataDirectlySaveOnChange = saveDirectlyOnChangesToolStripMenuItem.Checked;
        }
        #endregion

        #region [ OnTextChangedItemAnim ]
        private void OnTextChangedItemAnim(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!short.TryParse(textBoxAnim.Text, out short shortRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.Animation = shortRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemName ]
        private void OnTextChangedItemName(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            string name = textBoxName.Text;
            if (name.Length == 0)
            {
                return;
            }

            if (name.Length > 20)
            {
                name = name.Substring(0, 20);
            }

            item.Name = name;

            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ TreeViewItemOnBeforeSelect ]
        private void TreeViewItemOnBeforeSelect(object sender, TreeViewCancelEventArgs treeViewCancelEventArgs)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];

            string itemText = string.Format("0x{0:X4} ({0}) {1}", index, item.Name);
            if (treeViewItem.SelectedNode.Text != itemText)
            {
                treeViewItem.SelectedNode.Text = string.Format("0x{0:X4} ({0}) {1}", index, item.Name);
            }
        }
        #endregion

        #region [ OnTextChangedItemWeight ]
        private void OnTextChangedItemWeight(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!byte.TryParse(textBoxWeight.Text, out byte byteRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.Weight = byteRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemQuality ]
        private void OnTextChangedItemQuality(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!byte.TryParse(textBoxQuality.Text, out byte byteRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.Quality = byteRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemQuantity ]
        private void OnTextChangedItemQuantity(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!byte.TryParse(textBoxQuantity.Text, out byte byteRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.Quantity = byteRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemHue ]
        private void OnTextChangedItemHue(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!byte.TryParse(textBoxHue.Text, out byte byteRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.Hue = byteRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemStackOff ]
        private void OnTextChangedItemStackOff(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!byte.TryParse(textBoxStackOff.Text, out byte byteRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.StackingOffset = byteRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemValue ]
        private void OnTextChangedItemValue(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!byte.TryParse(textBoxValue.Text, out byte byteRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.Value = byteRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemHeight ]
        private void OnTextChangedItemHeight(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!byte.TryParse(textBoxHeigth.Text, out byte byteRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.Height = byteRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemMiscData ]
        private void OnTextChangedItemMiscData(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!short.TryParse(textBoxUnk1.Text, out short shortRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.MiscData = shortRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemUnk2 ]
        private void OnTextChangedItemUnk2(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!byte.TryParse(textBoxUnk2.Text, out byte byteRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.Unk2 = byteRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedItemUnk3 ]
        private void OnTextChangedItemUnk3(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            if (!byte.TryParse(textBoxUnk3.Text, out byte byteRes))
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            item.Unk3 = byteRes;
            TileData.ItemTable[index] = item;
            treeViewItem.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
        }
        #endregion

        #region [ OnTextChangedLandName ]
        private void OnTextChangedLandName(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewLand.SelectedNode?.Tag == null)
            {
                return;
            }

            int index = (int)treeViewLand.SelectedNode.Tag;
            LandData land = TileData.LandTable[index];
            string name = textBoxNameLand.Text;
            if (name.Length == 0)
            {
                return;
            }

            if (name.Length > 20)
            {
                name = name.Substring(0, 20);
            }

            land.Name = name;
            treeViewLand.SelectedNode.Text = string.Format("0x{0:X4} ({0}) {1}", index, name);
            TileData.LandTable[index] = land;
            treeViewLand.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index);
        }
        #endregion

        #region OnTextChangedLandTexID
        private void OnTextChangedLandTexID(object sender, EventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (treeViewLand.SelectedNode == null)
            {
                return;
            }

            if (!ushort.TryParse(textBoxTexID.Text, out ushort shortRes))
            {
                return;
            }

            int index = (int)treeViewLand.SelectedNode.Tag;
            LandData land = TileData.LandTable[index];
            land.TextureId = shortRes;
            TileData.LandTable[index] = land;
            treeViewLand.SelectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index);
        }
        #endregion

        #region [ OnFlagItemCheckItems ]
        private void OnFlagItemCheckItems(object sender, ItemCheckEventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (e.CurrentValue == e.NewValue)
            {
                return;
            }

            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            ItemData item = TileData.ItemTable[index];
            string chineseName = checkedListBox1.Items[e.Index].ToString();
            string englishName = GetEnglishTileFlagName(chineseName);
            
            if (Enum.TryParse(englishName, out TileFlag changeFlag))
            {
                if ((item.Flags & changeFlag) != 0) // 最好再检查一次
                {
                    if (e.NewValue != CheckState.Unchecked)
                    {
                        return;
                    }

                    item.Flags ^= changeFlag;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
                }
                else if ((item.Flags & changeFlag) == 0)
                {
                    if (e.NewValue != CheckState.Checked)
                    {
                        return;
                    }

                    item.Flags |= changeFlag;
                    TileData.ItemTable[index] = item;
                    treeViewItem.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    ControlEvents.FireTileDataChangeEvent(this, index + 0x4000);
                }
            }
        }
        #endregion

        #region [ OnFlagItemCheckLandTiles ]
        private void OnFlagItemCheckLandTiles(object sender, ItemCheckEventArgs e)
        {
            if (!saveDirectlyOnChangesToolStripMenuItem.Checked)
            {
                return;
            }

            if (_changingIndex)
            {
                return;
            }

            if (e.CurrentValue == e.NewValue)
            {
                return;
            }

            if (treeViewLand.SelectedNode == null)
            {
                return;
            }

            int index = (int)treeViewLand.SelectedNode.Tag;
            LandData land = TileData.LandTable[index];
            string chineseName = checkedListBox2.Items[e.Index].ToString();
            string englishName = GetEnglishTileFlagName(chineseName);
            
            if (Enum.TryParse(englishName, out TileFlag changeFlag))
            {
                if ((land.Flags & changeFlag) != 0)
                {
                    if (e.NewValue != CheckState.Unchecked)
                    {
                        return;
                    }

                    land.Flags ^= changeFlag;
                    TileData.LandTable[index] = land;
                    treeViewLand.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    ControlEvents.FireTileDataChangeEvent(this, index);
                }
                else if ((land.Flags & changeFlag) == 0)
                {
                    if (e.NewValue != CheckState.Checked)
                    {
                        return;
                    }

                    land.Flags |= changeFlag;
                    TileData.LandTable[index] = land;
                    treeViewLand.SelectedNode.ForeColor = Color.Red;
                    Options.ChangedUltimaClass["TileData"] = true;
                    ControlEvents.FireTileDataChangeEvent(this, index);
                }
            }
        }
        #endregion

        #region [ OnClickExport ]
        private void OnClickExport(object sender, EventArgs e)
        {
            string path = Options.OutputPath;
            if (tabcontrol.SelectedIndex == 0) // 物品
            {
                string fileName = Path.Combine(path, "ItemData.csv");
                TileData.ExportItemDataToCsv(fileName);
                MessageBox.Show($"ItemData已保存到 {fileName}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                string fileName = Path.Combine(path, "LandData.csv");
                TileData.ExportLandDataToCsv(fileName);
                MessageBox.Show($"LandData已保存到 {fileName}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        #endregion

        #region [ OnClickSearch ]
        private TileDataSearchForm _showForm1;
        private TileDataSearchForm _showForm2;

        private void OnClickSearch(object sender, EventArgs e)
        {
            if (tabcontrol.SelectedIndex == 0) // 物品
            {
                if (_showForm1?.IsDisposed == false)
                {
                    return;
                }

                _showForm1 = new TileDataSearchForm(false, SearchGraphic, SearchName)
                {
                    TopMost = true
                };
                _showForm1.Show();
            }
            else // 土地瓦片
            {
                if (_showForm2?.IsDisposed == false)
                {
                    return;
                }

                _showForm2 = new TileDataSearchForm(true, SearchGraphic, SearchName)
                {
                    TopMost = true
                };
                _showForm2.Show();
            }
        }
        #endregion

        #region [ OnClickSelectItem ]
        private void OnClickSelectItem(object sender, EventArgs e)
        {
            if (treeViewItem.SelectedNode?.Tag == null)
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            var found = ItemsControl.SearchGraphic(index);
            if (!found)
            {
                MessageBox.Show("您需要先加载物品选项卡。", "信息");
            }
        }
        #endregion

        #region [ OnClickSelectInLandTiles ]
        private void OnClickSelectInLandTiles(object sender, EventArgs e)
        {
            if (treeViewLand.SelectedNode == null)
            {
                return;
            }

            int index = (int)treeViewLand.SelectedNode.Tag;
            var found = LandTilesControl.SearchGraphic(index);
            if (!found)
            {
                MessageBox.Show("您需要先加载LandTiles选项卡。", "信息");
            }
        }
        #endregion

        #region [ OnClickSelectRadarItem ]
        private void OnClickSelectRadarItem(object sender, EventArgs e)
        {
            if (treeViewItem.SelectedNode == null)
            {
                return;
            }

            int index = (int)treeViewItem.SelectedNode.Tag;
            RadarColorControl.Select(index, false);
        }
        #endregion

        #region [ OnClickSelectRadarLand ]
        private void OnClickSelectRadarLand(object sender, EventArgs e)
        {
            if (treeViewLand.SelectedNode == null)
            {
                return;
            }

            int index = (int)treeViewLand.SelectedNode.Tag;
            RadarColorControl.Select(index, true);
        }
        #endregion

        #region [ OnClickImport ]
        private void OnClickImport(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "选择要导入的csv文件",
                CheckFileExists = true,
                Filter = "csv 文件 (*.csv)|*.csv"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Options.ChangedUltimaClass["TileData"] = true;
                if (tabcontrol.SelectedIndex == 0) // 物品
                {
                    TileData.ImportItemDataFromCsv(dialog.FileName);
                    AfterSelectTreeViewItem(this, new TreeViewEventArgs(treeViewItem.SelectedNode));
                }
                else
                {
                    TileData.ImportLandDataFromCsv(dialog.FileName);
                    AfterSelectTreeViewLand(this, new TreeViewEventArgs(treeViewLand.SelectedNode));
                }
            }
            dialog.Dispose();
        }
        #endregion

        #region [ OnClickSetFilter ]
        private TileDataFilterForm _filterFormForm;

        private void OnClickSetFilter(object sender, EventArgs e)
        {
            if (_filterFormForm?.IsDisposed == false)
            {
                return;
            }

            _filterFormForm = new TileDataFilterForm(ApplyFilterItem, ApplyFilterLand)
            {
                TopMost = true
            };
            _filterFormForm.Show();
        }
        #endregion

        #region [ OnItemDataNodeExpanded ]
        private void OnItemDataNodeExpanded(object sender, TreeViewCancelEventArgs e)
        {
            // 解决65536个项目的微软bug的变通方法
            if (treeViewItem.Nodes.Count == 3)
            {
                treeViewItem.CollapseAll();
            }
        }
        #endregion

        #region [ TileData_KeyUp ]
        private void TileData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.F || !e.Control)
            {
                return;
            }

            OnClickSearch(sender, e);
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
        #endregion

        #region [ SelectInGumpsTab ]
        private const int _maleGumpOffset = 50_000;
        private const int _femaleGumpOffset = 60_000;

        private static void SelectInGumpsTab(int tiledataIndex, bool female = false)
        {
            int gumpOffset = female ? _femaleGumpOffset : _maleGumpOffset;
            var animation = TileData.ItemTable[tiledataIndex].Animation;

            GumpControl.Select(animation + gumpOffset);
        }
        #endregion

        #region [ SelectInGumpsTabMaleToolStripMenuItem_Click ]
        private void SelectInGumpsTabMaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedItemTag = treeViewItem.SelectedNode?.Tag;
            if (selectedItemTag is null || (int)selectedItemTag <= 0)
            {
                return;
            }

            SelectInGumpsTab((int)selectedItemTag);
        }
        #endregion

        #region [ SelectInGumpsTabFemaleToolStripMenuItem_Click ]
        private void SelectInGumpsTabFemaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedItemTag = treeViewItem.SelectedNode?.Tag;
            if (selectedItemTag is null || (int)selectedItemTag <= 0)
            {
                return;
            }

            SelectInGumpsTab((int)selectedItemTag, true);
        }
        #endregion

        #region [ ItemsContextMenuStrip_Opening ]
        private void ItemsContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var selectedItemTag = treeViewItem.SelectedNode?.Tag;
            if (selectedItemTag is null || (int)selectedItemTag <= 0)
            {
                selectInGumpsTabMaleToolStripMenuItem.Enabled = false;
                selectInGumpsTabFemaleToolStripMenuItem.Enabled = false;
            }
            else
            {
                var itemData = TileData.ItemTable[(int)selectedItemTag];

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

        #region [ TextBoxTexID_DoubleClick ]
        /// <summary>
        /// TextBoxTexID上的双击事件处理程序。将TexID设置为节点的Tag值
        /// 即 0x256 (598) 熔岩 -> 598。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxTexID_DoubleClick(object sender, EventArgs e)
        {
            if (!setTextureOnDoubleClickToolStripMenuItem.Checked)
            {
                return;
            }

            int index = (int)treeViewLand.SelectedNode.Tag;
            if (!int.TryParse(textBoxTexID.Text, out int texIdValue) || texIdValue == index)
            {
                return;
            }

            textBoxTexID.Text = $"{index}";
        }
        #endregion

        #region [ SetTextureMenuItem ]
        /// <summary>
        /// "设置纹理"菜单项的点击事件处理程序。将所有土地瓦片的TextureId设置为其索引。
        /// 这是在假设每个LandTileID == TextureId的情况下编写的。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTextureMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "您要为所有土地瓦片设置TexID吗？\n\n" +
                "此操作假设土地瓦片索引值等于纹理索引值。\n\n" +
                "它只会考虑TexID为0的土地瓦片。\n\n继续吗？",
                "设置纹理",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var updated = 0;
            for (int i = 0; i < TileData.LandTable.Length; ++i)
            {
                if (!Textures.TestTexture(i) || TileData.LandTable[i].TextureId != 0)
                {
                    continue;
                }

                TileData.LandTable[i].TextureId = (ushort)i;

                var node = treeViewLand.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Tag.Equals(i));
                if (node != null)
                {
                    node.ForeColor = Color.Red;
                }

                updated++;

                Options.ChangedUltimaClass["TileData"] = true;
            }

            MessageBox.Show(updated > 0 ? $"已更新 {updated} 个土地瓦片。" : "没有更新任何内容。", "设置纹理");
        }
        #endregion

        #region ToolStripComboBox 及更多

        #region LoadImages
        void LoadImages()
        {
            if (!_icons.ContainsKey("Stairs"))
            {
                _icons.Add("Stairs-S", Properties.Resources.Stairs01);
                _icons.Add("Stairs-E", Properties.Resources.Stairs02);
                _icons.Add("Stairs-N", Properties.Resources.Stairs03);
                _icons.Add("Stairs-W", Properties.Resources.Stairs04);
                _icons.Add("Stairs-E-S", Properties.Resources.Stairs05);
                _icons.Add("Stairs-N-E", Properties.Resources.Stairs06);
                _icons.Add("Stairs-S-W", Properties.Resources.Stairs07);
                _icons.Add("Stairs-W-N", Properties.Resources.Stairs08);
            }
        }
        #endregion

        #region [ 定义使用Bitmap而不是Icon的字典 ]
        // 定义使用Bitmap而不是Icon的字典
        Dictionary<string, Bitmap> _icons = new Dictionary<string, Bitmap>();
        #endregion

        #region [ toolStripComboBox1_DrawItem ]
        void ToolStripComboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            string name = toolStripComboBox1.Items[e.Index].ToString();
            // 首先绘制文本
            e.Graphics.DrawString(name, e.Font, new SolidBrush(e.ForeColor), e.Bounds.Left, e.Bounds.Top);
            if (_icons.TryGetValue(name, out Bitmap bitmap))  // 使用TryGetValue检查并获取图像
            {
                // 将图像缩放到行的高度
                int width = Convert.ToInt32(e.Graphics.MeasureString(name, e.Font).Width);
                Rectangle destRect = new Rectangle(e.Bounds.Left + width, e.Bounds.Top, e.Bounds.Height, e.Bounds.Height);
                e.Graphics.DrawImage(bitmap, destRect);
            }
        }
        #endregion

        #region [ ToolStripComboBox1_SelectedIndexChanged ]
        private void ToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 将所有元素设置为未选中
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }

            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, false);
            }
            // 根据选定的预设更新选中的项目和重量和高度值
            if (toolStripComboBox1.SelectedItem != null)
            {
                string selectedItem = toolStripComboBox1.SelectedItem.ToString();
                switch (selectedItem)
                {
                    case "Wall":  //1
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "20";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Wall")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        break;
                    case "Door-E-L": //2
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "20";
                        textBoxQuality.Text = "1";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Wall")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Door")), true);
                        break;
                    case "Door-E-R": //3
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "20";
                        textBoxQuality.Text = "2";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Wall")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Door")), true);
                        break;
                    case "Door-S-L": //4
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "20";
                        textBoxQuality.Text = "2";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Wall")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Door")), true);
                        break;
                    case "Door-S-R": //5
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "20";
                        textBoxQuality.Text = "3";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Wall")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Door")), true);
                        break;
                    case "Window": //6
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "20";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Wall")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Window")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        break;
                    case "Roof": //7
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "3";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Roof")), true);
                        break;
                    case "Floor": //8
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "0";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Background")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        break;
                    case "Water": //9
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "0";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Background")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Translucent")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        break;
                    case "Stairs-W": //10
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "5";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("StairBack")), true);
                        break;
                    case "Stairs-N": //11
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "5";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("StairBack")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("StairRight")), true);
                        break;
                    case "Stairs-E": //12
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "5";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("StairRight")), true);
                        break;
                    case "Stairs-S": //13
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "5";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        break;
                    case "Stairs-W-N": //14
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "5";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("StairBack")), true);
                        break;
                    case "Stairs-N-E": //15
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "5";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("StairRight")), true);
                        break;
                    case "Stairs-E-S": //16
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "5";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        break;
                    case "Stairs-S-W": //17
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "5";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        break;
                    case "Stairs-Block": //18
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "10";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Bridge")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        break;
                    case "Container": //19
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "5";
                        textBoxStackOff.Text = "15";
                        textBoxUnk1.Text = "1";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Container")), true);
                        break;
                    case "Lamp Post": //20
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "15";
                        textBoxQuality.Text = "29";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("LightSource")), true);
                        break;
                    case "Fence": //21
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "10";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Wall")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        break;
                    case "Cave Wall": //22
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "24";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Wall")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoShoot")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleAn")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoHouse")), true);
                        break;
                    case "Clothing": //23
                        textBoxWeight.Text = "10";
                        textBoxHeigth.Text = "1";
                        textBoxQuality.Text = "10"; //层
                        textBoxStackOff.Text = "6"; //堆叠偏移
                        textBoxUnk1.Text = "582"; //杂项数据
                        textBoxAnim.Text = "599"; //动画
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Weapon")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Wearable")), true);
                        break;
                    case "Plant": //24
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "1";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Background")), true);
                        break;
                    case "Chair-Small-5": //25
                        textBoxWeight.Text = "20";
                        textBoxHeigth.Text = "1";
                        textBoxStackOff.Text = "5"; //堆叠偏移
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        break;
                    case "Chair-35": //26
                        textBoxWeight.Text = "20";
                        textBoxHeigth.Text = "1";
                        textBoxStackOff.Text = "35"; //堆叠偏移
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        break;
                    case "Chair-Wood-8": //27
                        textBoxWeight.Text = "20";
                        textBoxHeigth.Text = "1";
                        textBoxStackOff.Text = "8"; //堆叠偏移
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("PartialHue")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoDiagonal")), true);
                        break;
                    case "Chair-Throne-15": //27
                        textBoxWeight.Text = "20";
                        textBoxHeigth.Text = "1";
                        textBoxStackOff.Text = "8"; //堆叠偏移
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleA")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("PartialHue")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("NoDiagonal")), true);
                        break;
                    case "LandTile Land": //29
                        checkedListBox2.SetItemChecked(checkedListBox2.Items.IndexOf(GetChineseTileFlagName("Background")), true);
                        checkedListBox2.SetItemChecked(checkedListBox2.Items.IndexOf(GetChineseTileFlagName("Surface")), true);
                        break;
                    case "LandTile Water": //30
                        checkedListBox2.SetItemChecked(checkedListBox2.Items.IndexOf(GetChineseTileFlagName("Translucent")), true);
                        break;
                    case "LandTile Mountain": //31
                        checkedListBox2.SetItemChecked(checkedListBox2.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        break;
                    case "Tree": //32
                        textBoxName.Text = "树";
                        textBoxWeight.Text = "255";
                        textBoxHeigth.Text = "20";
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("Impassable")), true);
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(GetChineseTileFlagName("ArticleAn")), true);
                        break;
                    case "Clear": //33
                        textBoxName.Text = "";
                        textBoxWeight.Text = "";
                        textBoxHeigth.Text = "";
                        textBoxHue.Text = "";
                        textBoxUnk3.Text = "";
                        textBoxQuality.Text = "";
                        textBoxStackOff.Text = "";
                        textBoxUnk1.Text = "";
                        textBoxQuantity.Text = "";
                        textBoxValue.Text = "";
                        textBoxUnk2.Text = "";
                        textBoxAnim.Text = "";
                        // 使用字典中的中文名称来取消选中所有复选框
                        foreach (var translation in TileFlagTranslations)
                        {
                            string chineseName = translation.Value;
                            int index1 = checkedListBox1.Items.IndexOf(chineseName);
                            if (index1 >= 0)
                            {
                                checkedListBox1.SetItemChecked(index1, false);
                            }
                            
                            int index2 = checkedListBox2.Items.IndexOf(chineseName);
                            if (index2 >= 0)
                            {
                                checkedListBox2.SetItemChecked(index2, false);
                            }
                        }
                        break;
                        // 在此处添加更多预设
                }
            }
        }
        #endregion
        #endregion

        #region [ 搜索和声音 ]
        private bool _playCustomSound = false;

        #region [ toolStripButton6 ]
        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            toolStripButton6.Checked = !toolStripButton6.Checked;
            _playCustomSound = !_playCustomSound;
        }
        #endregion        

        bool _toolStripButton7IsActive = false;
        List<int> _savedCheckedIndices = new List<int>();
        Dictionary<TextBox, string> _savedTextBoxTexts = new Dictionary<TextBox, string>();

        #region [ toolStripButton7 ]
        private void ToolStripButton7_Click(object sender, EventArgs e)
        {
            if (!_toolStripButton7IsActive)
            {
                // 保存选中的索引
                foreach (int index in checkedListBox1.CheckedIndices)
                {
                    _savedCheckedIndices.Add(index);
                }

                // 保存文本框文本
                _savedTextBoxTexts.Add(textBoxName, textBoxName.Text);
                _savedTextBoxTexts.Add(textBoxWeight, textBoxWeight.Text);
                _savedTextBoxTexts.Add(textBoxHue, textBoxHue.Text);
                _savedTextBoxTexts.Add(textBoxHeigth, textBoxHeigth.Text);
                _savedTextBoxTexts.Add(textBoxUnk3, textBoxUnk3.Text);
                _savedTextBoxTexts.Add(textBoxQuality, textBoxQuality.Text);
                _savedTextBoxTexts.Add(textBoxStackOff, textBoxStackOff.Text);
                _savedTextBoxTexts.Add(textBoxUnk1, textBoxUnk1.Text);
                _savedTextBoxTexts.Add(textBoxQuantity, textBoxQuantity.Text);
                _savedTextBoxTexts.Add(textBoxValue, textBoxValue.Text);
                _savedTextBoxTexts.Add(textBoxUnk2, textBoxUnk2.Text);
                _savedTextBoxTexts.Add(textBoxAnim, textBoxAnim.Text);

                // 更新toolStripButton7外观
                toolStripButton7.Checked = true;

                // 更新toolStripButton7IsActive
                _toolStripButton7IsActive = true;
            }
            else
            {
                // 恢复选中的索引
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (_savedCheckedIndices.Contains(i))
                    {
                        checkedListBox1.SetItemChecked(i, true);
                    }
                    else
                    {
                        checkedListBox1.SetItemChecked(i, false);
                    }
                }

                // 清除savedCheckedIndices
                _savedCheckedIndices.Clear();

                // 恢复文本框文本
                foreach (KeyValuePair<TextBox, string> pair in _savedTextBoxTexts)
                {
                    pair.Key.Text = pair.Value;
                }

                // 清除savedTextBoxTexts
                _savedTextBoxTexts.Clear();

                // 更新toolStripButton7外观
                toolStripButton7.Checked = false;

                // 更新toolStripButton7IsActive
                _toolStripButton7IsActive = false;
            }
        }
        #endregion

        #region [ toolStripPushMarkedButton8 ]
        private void ToolStripPushMarkedButton8_Click(object sender, EventArgs e)
        {
            if (_toolStripButton7IsActive)
            {
                // 将保存的设置传输到选定位置。

                // 传输选中的索引。
                foreach (int index in _savedCheckedIndices)
                {
                    checkedListBox1.SetItemChecked(index, true);
                }

                // 传输文本框文本。
                foreach (KeyValuePair<TextBox, string> pair in _savedTextBoxTexts)
                {
                    pair.Key.Text = pair.Value;
                }

                // 保存更改（无消息框）。
                OnClickSaveChanges(null, EventArgs.Empty);
            }
        }
        #endregion

        #region [ SearchByIdToolStripTextBox_KeyUp ]
        private void SearchByIdToolStripTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Utils.ConvertStringToInt(searchByIdToolStripTextBox.Text, out int indexValue, 0, Art.GetMaxItemId()))
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

            var landTilesSelected = tabcontrol.SelectedIndex != 0;

            SearchGraphic(indexValue, landTilesSelected);
        }
        #endregion

        #region [ SearchByNameToolStripTextBox_KeyUp ]
        private void SearchByNameToolStripTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var landTilesSelected = tabcontrol.SelectedIndex != 0;

            SearchName(searchByNameToolStripTextBox.Text, false, landTilesSelected);
        }
        #endregion

        #region [ SearchByNameToolStripButton_Click ]
        private void SearchByNameToolStripButton_Click(object sender, EventArgs e)
        {
            var landTilesSelected = tabcontrol.SelectedIndex != 0;

            SearchName(searchByNameToolStripTextBox.Text, true, landTilesSelected);
        }
        #endregion
        #endregion        

        #region 鼠标中键复制tiledata中的设置

        private void TreeViewItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (treeViewItem.SelectedNode != null && toolStripButton7.Checked)
                {
                    ToolStripPushMarkedButton8_Click(null, null);
                }
            }
        }

        private void TreeViewItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Y && treeViewItem.SelectedNode != null)
            {
                ToolStripPushMarkedButton8_Click(null, null);
            }
        }
        #endregion

        #region 标签瓦片十进制地址
        private void LabelDecimalAdress_Click(object sender, EventArgs e)
        {
            if (treeViewLand.SelectedNode?.Tag == null)
            {
                return;
            }

            int index = (int)treeViewLand.SelectedNode.Tag;
            textBoxTexID.Text = index.ToString();
        }
        #endregion

        #region 删除按钮
        private void ToolStripButton8Clear_Click(object sender, EventArgs e)
        {
            // 将所有文本框的文本设置为空字符串
            textBoxName.Text = "";
            textBoxWeight.Text = "";
            textBoxHeigth.Text = "";
            textBoxHue.Text = "";
            textBoxUnk3.Text = "";
            textBoxQuality.Text = "";
            textBoxStackOff.Text = "";
            textBoxUnk1.Text = "";
            textBoxQuantity.Text = "";
            textBoxValue.Text = "";
            textBoxUnk2.Text = "";
            textBoxAnim.Text = "";

            // 使用字典中的中文名称来取消选中所有复选框
            foreach (var translation in TileFlagTranslations)
            {
                string chineseName = translation.Value;
                
                // 取消checkedListBox1中所有复选框的选中
                int index1 = checkedListBox1.Items.IndexOf(chineseName);
                if (index1 >= 0)
                {
                    checkedListBox1.SetItemChecked(index1, false);
                }
                
                // 取消checkedListBox2中所有复选框的选中
                int index2 = checkedListBox2.Items.IndexOf(chineseName);
                if (index2 >= 0)
                {
                    checkedListBox2.SetItemChecked(index2, false);
                }
            }

            // 当playCustomSound设置为true时播放声音
            if (_playCustomSound)
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = "sound.wav";
                player.Play();
            }
        }
        #endregion

        #region 缩放
        private bool _isZoomed = false; // 缩放状态
        private Image _originalImage; // 原始图像
        private Form _zoomForm; // 缩放窗体
        private PictureBox _zoomPictureBox; // 缩放窗体中的PictureBox

        private void ZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查是否选择了图像
            if (pictureBoxItem.Image == null)
            {
                MessageBox.Show("请先选择一个图像。");
                return;
            }

            if (_originalImage == null) // 如果尚未保存原始图像
            {
                _originalImage = pictureBoxItem.Image; // 保存原始图像
            }

            if (!_isZoomed) // 如果未缩放
            {
                // 将图像放大到原始尺寸的两倍
                Bitmap zoomedImage = new Bitmap(_originalImage, _originalImage.Width * 2, _originalImage.Height * 2);

                // 将原始图像绘制到新位图上，调整其大小
                using (Graphics g = Graphics.FromImage(zoomedImage))
                {
                    g.DrawImage(_originalImage, 0, 0, zoomedImage.Width, zoomedImage.Height);
                }

                // 如果尚未创建缩放窗体和PictureBox，则创建它们
                if (_zoomForm == null)
                {
                    _zoomForm = new Form();
                    _zoomPictureBox = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        SizeMode = PictureBoxSizeMode.Normal // 将SizeMode属性设置为Normal
                    };
                    _zoomForm.Controls.Add(_zoomPictureBox);
                    _zoomForm.FormClosed += (s, ev) => { _zoomForm = null; _zoomPictureBox = null; }; // 窗体关闭时将zoomForm和zoomPictureBox重置为null

                    // 将窗体大小设置为690, 580
                    _zoomForm.Size = new Size(690, 580);
                }

                // 将缩放窗体中PictureBox的图像设置为新位图
                _zoomPictureBox.Image = zoomedImage;

                // 将窗体大小调整为PictureBox的大小
                _zoomForm.ClientSize = _zoomPictureBox.Size;

                // 显示缩放窗体
                if (!_zoomForm.Visible)
                {
                    _zoomForm.Show(this); // 将窗体显示为模态对话框
                }

                _isZoomed = true; // 更新缩放状态

                // 添加一个点击事件处理程序来调整窗体大小
                _zoomPictureBox.Click += (s, ev) =>
                {
                    if (_zoomForm.Size.Width == 690 && _zoomForm.Size.Height == 580)
                    {
                        _zoomForm.Size = new Size(1432, 870);
                    }
                    else
                    {
                        _zoomForm.Size = new Size(690, 580);
                    }
                };
            }
            else // 当已缩放时
            {
                // 将图像重置为原始大小
                pictureBoxItem.Image = _originalImage;
                _isZoomed = false; // 更新缩放状态

                // 关闭缩放窗体
                if (_zoomForm != null)
                {
                    _zoomForm.Close();
                }
            }
        }

        // UpdateZoomFormImage
        private void UpdateZoomFormImage()
        {
            if (_zoomForm != null && _zoomPictureBox != null && _originalImage != null && _isZoomed)
            {
                // 将图像放大到原始尺寸的两倍
                Bitmap zoomedImage = new Bitmap(_originalImage, _originalImage.Width * 2, _originalImage.Height * 2);

                // 将原始图像绘制到新位图上，调整其大小
                using (Graphics g = Graphics.FromImage(zoomedImage))
                {
                    g.DrawImage(_originalImage, 0, 0, zoomedImage.Width, zoomedImage.Height);
                }

                // 将缩放窗体中PictureBox的图像设置为新位图
                _zoomPictureBox.Image = zoomedImage;
            }
        }

        // 缩放图像
        private void ZoomImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查是否选择了图像
            if (pictureBoxItem.Image == null)
            {
                MessageBox.Show("请先选择一个图像。");
                return;
            }

            if (_originalImage == null) // 如果尚未保存原始图像
            {
                _originalImage = pictureBoxItem.Image; // 保存原始图像
            }

            if (!_isZoomed) // 如果未缩放
            {
                // 将图像放大到原始尺寸的两倍
                Bitmap zoomedImage = new Bitmap(_originalImage.Width * 2, _originalImage.Height * 2);

                // 将原始图像绘制到新位图上，调整其大小
                using (Graphics g = Graphics.FromImage(zoomedImage))
                {
                    g.DrawImage(_originalImage, 0, 0, zoomedImage.Width, zoomedImage.Height);
                }

                // 将PictureBox的图像设置为新位图
                pictureBoxItem.Image = zoomedImage;

                // 在面板中居中PictureBox
                pictureBoxItem.Left = (splitContainer2.Panel2.Width - pictureBoxItem.Width) / 2;
                pictureBoxItem.Top = (splitContainer2.Panel2.Height - pictureBoxItem.Height) / 2;

                _isZoomed = true; // 更新缩放状态
            }
            else // 当已缩放时
            {
                // 将图像重置为原始大小
                pictureBoxItem.Image = _originalImage;
                _isZoomed = false; // 更新缩放状态
            }
        }
        #endregion

        #region [ buttonLoadTxt ]
        private void ButtonLoadTxt_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                // 将tbClassicUOPfad的文本设置为选定路径
                tbClassicUOPfad.Text = folderBrowserDialog.SelectedPath;

                // 在设置中保存路径
                Properties.Settings.Default.CUOTestPath = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region [ 字典 ]
        Dictionary<string, string> _infoTexts = new Dictionary<string, string>()
        {
            { "chair", "2865,0,0,0,0,0,0,4 = 2865是椅子的物品编号。\n\n这一行中的前四个零告诉你的角色，\n无论你从哪个方向来，当你坐在这张椅子上时，你总是会面朝北方。\n\n北向椅子 = 2865,0,0,0,0,\n\n东向椅子 = 2863,2,2,2,2,6,6 \n前四个数字2总是朝东。\n\n南向椅子 = 2862,4,4,4,4,0,0 \n前四个4总是朝西南。\n\n定向椅子 = 2864,6,6,6,6,-8,8 \n\n前四个6总是面向你。\n西向凳子 = 2910,0,2,4,6,-8,-8 \n\n凳子是多方位的。\n这意味着，通过添加\n所有4个方向 = ,0,2,4,6, \n\n最后两个数字与角色的定位有关。" },
            { "seasons", "在此处输入信息文本" },
            { "lights", "在此处输入信息文本" },
            { "lightshaders", "在此处输入信息文本" },
            { "tree", "在此处输入信息文本" },
            { "vegetation", "在此处输入信息文本" },
            { "cave", "在此处输入信息文本" },
            { "containers", "在此处输入信息文本" }
        };
        #endregion

        #region [ comboBoxLoadText_SelectedIndexChanged ]
        private void ComboBoxLoadText_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 从tbClassicUOPath获取路径
            string path = tbClassicUOPfad.Text;

            // 从comboBoxLoadText获取选定的文件
            string selectedFile = comboBoxLoadText.SelectedItem.ToString();

            // 创建选定文件的完整路径
            string fullPath = Path.Combine(path, selectedFile + ".txt");

            // 检查文件是否存在
            if (File.Exists(fullPath))
            {
                // 将文件内容加载到richTextBoxEdit中
                richTextBoxEdit.Text = File.ReadAllText(fullPath);

                // 检查是否有选定文件的信息文本
                if (_infoTexts.TryGetValue(selectedFile, out string infoText))
                {
                    // 在textBoxInfoCuo中显示信息文本
                    richTextInfoCuo.Text = infoText;
                }
                else
                {
                    // 如果没有选定文件的信息文本，则清除textBoxInfoCuo
                    richTextInfoCuo.Text = "";
                }
            }
            else
            {
                MessageBox.Show("文件不存在。");
            }
        }

        #endregion

        #region [ btSaveTxtCuo ]
        private void BtSaveTxtCuo_Click(object sender, EventArgs e)
        {
            // 从tbClassicUOPath获取路径
            string path = tbClassicUOPfad.Text;

            // 从comboBoxLoadText获取选定的文件
            string selectedFile = comboBoxLoadText.SelectedItem.ToString();

            // 创建选定文件的完整路径
            string fullPath = Path.Combine(path, selectedFile + ".txt");

            // 从richTextBoxEdit获取文本
            string textToSave = richTextBoxEdit.Text;

            // 将文本写入文件
            File.WriteAllText(fullPath, textToSave);

            // 声音效果
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = "sound.wav";
            player.Play();
        }
        #endregion

        #region [ lbChairInfo_Click ]
        private void LbChairInfo_Click(object sender, EventArgs e)
        {
            Image image = Properties.Resources.MakeChairsUseable;

            Form form = new Form
            {
                ClientSize = image.Size,
                ShowIcon = false
            };

            PictureBox pictureBox = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.AutoSize
            };

            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            panel.Controls.Add(pictureBox);

            form.Controls.Add(panel);

            form.Show();
        }
        #endregion

        #region [ findToolStripMenuItem ]
        private void FindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string searchText = toolStripTextBoxFindText.Text;

            int index = richTextBoxEdit.Find(searchText);

            if (index != -1)
            {
                richTextBoxEdit.Select(index, searchText.Length);
                richTextBoxEdit.Focus();
            }
        }
        #endregion

        #region [ copySettingsToolStripMenuItem ]
        private void CopySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建Settings的新实例并从文本框和复选列表中复制值
            _copiedSettings = new Settings
            {
                Name = textBoxName.Text,
                Weight = textBoxWeight.Text,
                Anim = textBoxAnim.Text,
                Quality = textBoxQuality.Text,
                Quantity = textBoxQuantity.Text,
                Hue = textBoxHue.Text,
                StackOff = textBoxStackOff.Text,
                Value = textBoxValue.Text,
                Height = textBoxHeigth.Text,
                Unk1 = textBoxUnk1.Text,
                Unk2 = textBoxUnk2.Text,
                Unk3 = textBoxUnk3.Text,
                CheckedList = new List<bool>()
            };

            // 通过遍历每个项目保存CheckedListBox1的状态
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                // 如果项目被选中则添加true，否则添加false
                _copiedSettings.CheckedList.Add(checkedListBox1.GetItemChecked(i));
            }

            // 可选：显示消息指示复制成功
            // MessageBox.Show("设置复制成功！");

            // 复制设置后播放声音效果
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = "sound.wav";
            player.Play();
        }
        #endregion

        #region [ insertSettingsToolStripMenuItem ]
        private void InsertSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 在尝试插入设置之前检查是否已复制任何设置
            if (_copiedSettings == null)
            {
                MessageBox.Show("尚未复制任何设置。");
                return;
            }

            // 通过将值分配回文本框来恢复设置
            textBoxName.Text = _copiedSettings.Name;
            textBoxWeight.Text = _copiedSettings.Weight;
            textBoxAnim.Text = _copiedSettings.Anim;
            textBoxQuality.Text = _copiedSettings.Quality;
            textBoxQuantity.Text = _copiedSettings.Quantity;
            textBoxHue.Text = _copiedSettings.Hue;
            textBoxStackOff.Text = _copiedSettings.StackOff;
            textBoxValue.Text = _copiedSettings.Value;
            textBoxHeigth.Text = _copiedSettings.Height;
            textBoxUnk1.Text = _copiedSettings.Unk1;
            textBoxUnk2.Text = _copiedSettings.Unk2;
            textBoxUnk3.Text = _copiedSettings.Unk3;

            // 从复制的设置中恢复CheckedListBox1的状态
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, _copiedSettings.CheckedList[i]);
            }

            // 可选：显示消息指示插入成功
            // MessageBox.Show("设置插入成功！");

            // 插入设置后播放声音效果
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = "sound.wav";
            player.Play();
        }
        #endregion

        #region [ class Settings ] 
        public class Settings // 设置复制
        {
            public string Name { get; set; }
            public string Weight { get; set; }
            public string Anim { get; set; }
            public string Quality { get; set; }
            public string Quantity { get; set; }
            public string Hue { get; set; }
            public string StackOff { get; set; }
            public string Value { get; set; }
            public string Height { get; set; }
            public string Unk1 { get; set; }
            public string Unk2 { get; set; }
            public string Unk3 { get; set; }
            public List<bool> CheckedList { get; set; } // 存储CheckedListBox值
        }
        #endregion

        #region [ copySettingsLandToolStripMenuItem ]
        private void CopySettingsLandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _copiedLandSettings = new LandSettings
            {
                Name = textBoxNameLand.Text,
                CheckedList = new List<bool>(),
                TextureId = ushort.TryParse(textBoxTexID.Text, out ushort texId) ? texId : (ushort)0
            };

            // 保存CheckedListBox2的状态
            foreach (int index in checkedListBox2.CheckedIndices)
            {
                _copiedLandSettings.CheckedList.Add(true);
            }
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                if (!checkedListBox2.CheckedIndices.Contains(i))
                {
                    _copiedLandSettings.CheckedList.Add(false);
                }
            }
            
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = "sound.wav";
            player.Play();
        }
        #endregion

        #region [ insertSettingsLandToolStripMenuItem ]
        private void InsertSettingsLandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_copiedLandSettings == null)
            {
                MessageBox.Show("尚未复制任何设置。");
                return;
            }

            if (treeViewLand.SelectedNode == null)
            {
                MessageBox.Show("未选择任何节点。");
                return;
            }

            // 获取当前选中的节点
            TreeNode selectedNode = treeViewLand.SelectedNode;
            int index = (int)selectedNode.Tag;

            // 将复制的设置传输到当前土地项目
            LandData land = TileData.LandTable[index];
            land.Name = _copiedLandSettings.Name;
            land.TextureId = _copiedLandSettings.TextureId;

            // 重置CheckedListBox2的状态
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, _copiedLandSettings.CheckedList[i]);
            }

            // 节点的视觉更新
            selectedNode.Text = $"0x{index:X4} {land.Name}";

            // 保存更改
            TileData.LandTable[index] = land;

            // 可选：将节点标记为已更改并更新事件
            selectedNode.ForeColor = Color.Red;
            Options.ChangedUltimaClass["TileData"] = true;
            ControlEvents.FireTileDataChangeEvent(this, index);
            
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = "sound.wav";
            player.Play();
        }
        #endregion

        #region [ class LandSettings ]
        public class LandSettings
        {
            public string Name { get; set; }
            public ushort TextureId { get; set; }
            public List<bool> CheckedList { get; set; } // 存储CheckedListBox值
        }
        #endregion

        #region [ AssignToolTipsToLabels ]
        private void AssignToolTipsToLabels()
        {
            _image = Properties.Resources.MakeChairsUseable;

            // 静态
            toolTipComponent.SetToolTip(nameLabel, GetDescription(nameLabel));
            toolTipComponent.SetToolTip(animLabel, GetDescription(animLabel));
            toolTipComponent.SetToolTip(weightLabel, GetDescription(weightLabel));
            toolTipComponent.SetToolTip(layerLabel, GetDescription(layerLabel));
            toolTipComponent.SetToolTip(quantityLabel, GetDescription(quantityLabel));
            toolTipComponent.SetToolTip(valueLabel, GetDescription(valueLabel));
            toolTipComponent.SetToolTip(stackOffLabel, GetDescription(stackOffLabel));
            toolTipComponent.SetToolTip(hueLabel, GetDescription(hueLabel));
            toolTipComponent.SetToolTip(unknown2Label, GetDescription(unknown2Label));
            toolTipComponent.SetToolTip(miscDataLabel, GetDescription(miscDataLabel));
            toolTipComponent.SetToolTip(heightLabel, GetDescription(heightLabel));
            toolTipComponent.SetToolTip(unknown3Label, GetDescription(unknown3Label));

            // 土地瓦片
            toolTipComponent.SetToolTip(landNameLabel, GetDescription(landNameLabel));
            toolTipComponent.SetToolTip(landTexIdLabel, GetDescription(landTexIdLabel));

            // 编辑Cou文件
            toolTipComponent.SetToolTip(lbcomboBoxLoadText, GetDescription(lbcomboBoxLoadText));
        }
        #endregion

        #region [ GetDescription ]
        private string GetDescription(object sender)
        {
            string description = string.Empty;

            if (sender == nameLabel)
            {
                description = "此字段用于物品名称，最多20个字符。";
            }
            else if (sender == animLabel)
            {
                description = "此字段用于物品关联的动画ID。";
            }
            else if (sender == weightLabel)
            {
                description = "此字段用于物品的重量。";
            }
            else if (sender == layerLabel)
            {
                description = new StringBuilder()
                    .AppendLine("此字段用于物品的层：")
                    .AppendLine("")
                    .AppendLine("1 单手武器")
                    .AppendLine("2 双手武器、盾牌或杂项")
                    .AppendLine("3 鞋子")
                    .AppendLine("4 裤子")
                    .AppendLine("5 衬衫")
                    .AppendLine("6 头盔/帽子")
                    .AppendLine("7 手套")
                    .AppendLine("8 戒指")
                    .AppendLine("9 护身符")
                    .AppendLine("10 项链")
                    .AppendLine("11 头发")
                    .AppendLine("12 腰部（半围裙）")
                    .AppendLine("13 躯干（内层）（胸甲）")
                    .AppendLine("14 手镯")
                    .AppendLine("15 未使用（但背包客的背包转到21）")
                    .AppendLine("16 面部毛发")
                    .AppendLine("17 躯干（中层）（外衣、束腰外衣、全围裙、腰带）")
                    .AppendLine("18 耳环")
                    .AppendLine("19 手臂")
                    .AppendLine("20 背部（披风）")
                    .AppendLine("21 背包")
                    .AppendLine("22 躯干（外层）（长袍）")
                    .AppendLine("23 腿部（外层）（裙子/苏格兰短裙）")
                    .AppendLine("24 腿部（内层）（腿甲）")
                    .AppendLine("25 坐骑（马、鸵鸟等）")
                    .AppendLine("26 NPC购买补货容器")
                    .AppendLine("27 NPC购买不补货容器")
                    .AppendLine("28 NPC出售容器")
                    .ToString();
            }
            else if (sender == quantityLabel)
            {
                description = "此字段用于物品的数量。";
            }
            else if (sender == valueLabel)
            {
                description = "此字段用于物品的价值。";
            }
            else if (sender == stackOffLabel)
            {
                description = new StringBuilder()
                    .AppendLine("StackOff指多个物品堆叠时的堆叠偏移（像素）。")
                    .AppendLine("StackOff值越高，意味着堆叠中的物品彼此间距离越远。")
                    .ToString();
            }
            else if (sender == hueLabel)
            {
                description = "此字段用于物品的色调（颜色）。";
            }
            else if (sender == unknown2Label)
            {
                description = "此字段用于第二个未知值。";
            }
            else if (sender == miscDataLabel)
            {
                description = "旧UO演示版武器模板定义";
            }
            else if (sender == heightLabel)
            {
                description = "此字段用于物品的高度。";
            }
            else if (sender == unknown3Label)
            {
                description = "此字段用于第三个未知值。";
            }
            else if (sender == landNameLabel)
            {
                description = "此字段用于土地瓦片的名称，最多20个字符。";
            }
            else if (sender == landTexIdLabel)
            {
                description = "此字段用于土地瓦片关联的纹理ID。";
            }
            else if (sender == lbcomboBoxLoadText)
            {
                description = new StringBuilder()
                         .AppendLine("这是如何将文本文件加载到富文本框中以编辑文本文件。")
                         .AppendLine("这允许您为客户端嵌入新椅子、新颜色、容器等。")
                         .ToString();
            }

            return description;
        }
        #endregion
    }
}