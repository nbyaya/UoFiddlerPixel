/***************************************************************************
 *
 * $Author: Turley
 * 
 * "啤酒许可证"
 * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
 * 如果我们某天相遇，并且你认为这个东西有价值，
 * 你可以请我喝杯啤酒作为回报。
 *
 ***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using AnimatedGif;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Forms;
using UoFiddler.Controls.Helpers;

namespace UoFiddler.Controls.UserControls
{
    public partial class AnimationListControl : UserControl
    {

        public AnimationListControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        #region [ 获取动作名称 ]
        public string[][] GetActionNames { get; } = {
            // 怪物
            new[]
            {
                "行走",
                "空闲",
                "死亡1",
                "死亡2",
                "攻击1",
                "攻击2",
                "攻击3",
                "弓攻击",
                "弩攻击",
                "投掷攻击",
                "受击",
                "掠夺",
                "踩踏",
                "施法2",
                "施法3",
                "右格挡",
                "左格挡",
                "空闲",
                "烦躁",
                "飞行",
                "起飞",
                "空中受击"
            },
            // 海洋生物
            new[]
            {
                "行走",
                "奔跑",
                "空闲",
                "空闲",
                "烦躁",
                "攻击1",
                "攻击2",
                "受击",
                "死亡1"
            },
            // 动物
            new[]
            {
                "行走",
                "奔跑",
                "空闲",
                "进食",
                "警觉",
                "攻击1",
                "攻击2",
                "受击",
                "死亡1",
                "空闲",
                "烦躁",
                "躺下",
                "死亡2"
            },
            // 人类
            new[]
            {
                "行走_01",
                "持杖行走_01",
                "奔跑_01",
                "持杖奔跑_01",
                "空闲_01",
                "空闲_01",
                "打哈欠/伸懒腰_01",
                "单手战斗空闲_01",
                "单手战斗空闲_01",
                "单手挥砍攻击_01",
                "单手穿刺攻击_01",
                "单手钝击攻击_01",
                "双手钝击攻击_01",
                "双手挥砍攻击_01",
                "双手穿刺攻击_01",
                "单手战斗前进_01",
                "法术1",
                "法术2",
                "弓攻击_01",
                "弩攻击_01",
                "受击_前/高_01",
                "死亡_硬直前倒_01",
                "死亡_硬直后倒_01",
                "骑马行走_01",
                "骑马奔跑_01",
                "骑马空闲_01",
                "骑马单手右挥砍攻击_01",
                "骑马弓攻击_01",
                "骑马弩攻击_01",
                "骑马双手右挥砍攻击_01",
                "盾牌格挡_硬_01",
                "拳击_刺拳_01",
                "鞠躬_小_01",
                "单手武器敬礼_01",
                "进食_吞咽_01"
            }
        };
        #endregion

        private Bitmap _mainPicture;
        private int _currentSelect;
        private int _currentSelectAction;
        private bool _animate;
        private int _frameIndex;
        private Bitmap[] _animationList;
        private bool _imageInvalidated = true;
        private Timer _timer;
        private AnimationFrame[] _frames;
        private int _customHue;
        private int _defHue;
        private int _facing = 1;
        private bool _sortAlpha;
        private int _displayType;
        private bool _loaded;

        #region  [ 重新加载 ]
        /// <summary>
        /// 如果已加载则重新加载
        /// </summary>
        private void Reload()
        {
            if (!_loaded)
            {
                return;
            }

            _mainPicture = null;
            _currentSelect = 0;
            _currentSelectAction = 0;
            _animate = false;
            _imageInvalidated = true;
            StopAnimation();
            _frames = null;
            _customHue = 0;
            _defHue = 0;
            _facing = 1;
            _sortAlpha = false;
            _displayType = 0;
            OnLoad(this, EventArgs.Empty);
        }
        #endregion

        #region [ OnLoad ]
        private void OnLoad(object sender, EventArgs e)
        {
            if (IsAncestorSiteInDesignMode || FormsDesignerHelper.IsInDesignMode())
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            Options.LoadedUltimaClass["Animations"] = true;
            Options.LoadedUltimaClass["Hues"] = true;

            TreeViewMobs.TreeViewNodeSorter = new GraphicSorter();

            if (!LoadXml())
            {
                Cursor.Current = Cursors.Default;
                return;
            }

            LoadListView();
            _currentSelect = 0;
            _currentSelectAction = 0;

            if (TreeViewMobs.Nodes[0].Nodes.Count > 0)
            {
                TreeViewMobs.SelectedNode = TreeViewMobs.Nodes[0].Nodes[0];
            }

            FacingBar.Value = (_facing + 3) & 7;

            if (!_loaded)
            {
                ControlEvents.FilePathChangeEvent += OnFilePathChangeEvent;
            }

            _loaded = true;
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region  [ OnFilePathChangeEvent ]
        private void OnFilePathChangeEvent()
        {
            Reload();
        }
        #endregion

        #region [ 更改色调 ]
        /// <summary>
        /// 更改当前生物的色调
        /// </summary>
        /// <param name="select"></param>
        public void ChangeHue(int select)
        {
            _customHue = select + 1;
            CurrentSelect = CurrentSelect;
        }
        #endregion

        #region [ 是否已定义 ]
        /// <summary>
        /// 图形是否已经在树视图中
        /// </summary>
        /// <param name="graphic"></param>
        /// <returns></returns>
        public bool IsAlreadyDefined(int graphic)
        {
            return TreeViewMobs.Nodes[0].Nodes.Cast<TreeNode>().Any(node => ((int[])node.Tag)[0] == graphic) ||
                   TreeViewMobs.Nodes[1].Nodes.Cast<TreeNode>().Any(node => ((int[])node.Tag)[0] == graphic);
        }
        #endregion

        #region [ 添加图形 ]
        /// <summary>
        /// 将图形及其类型和名称添加到列表中
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void AddGraphic(int graphic, int type, string name)
        {
            TreeViewMobs.BeginUpdate();
            TreeViewMobs.TreeViewNodeSorter = null;
            TreeNode nodeParent = new TreeNode(name)
            {
                Tag = new[] { graphic, type },
                ToolTipText = Animations.GetFileName(graphic)
            };

            if (type == 4)
            {
                TreeViewMobs.Nodes[1].Nodes.Add(nodeParent);
                type = 3;
            }
            else
            {
                TreeViewMobs.Nodes[0].Nodes.Add(nodeParent);
            }

            for (int i = 0; i < GetActionNames[type].GetLength(0); ++i)
            {
                if (!Animations.IsActionDefined(graphic, i, 0))
                {
                    continue;
                }

                TreeNode node = new TreeNode($"{i} {GetActionNames[type][i]}")
                {
                    Tag = i
                };

                nodeParent.Nodes.Add(node);
            }

            TreeViewMobs.TreeViewNodeSorter = !_sortAlpha
                ? new GraphicSorter()
                : (IComparer)new AlphaSorter();

            TreeViewMobs.Sort();
            TreeViewMobs.EndUpdate();
            LoadListView();
            TreeViewMobs.SelectedNode = nodeParent;
            nodeParent.EnsureVisible();
        }
        #endregion

        #region [ 动画 ]
        private bool Animate
        {
            get => _animate;
            set
            {
                if (_animate == value)
                {
                    return;
                }

                _animate = value;
                StopAnimation();
                _imageInvalidated = true;
                MainPictureBox.Invalidate();
            }
        }
        #endregion

        #region [ 停止动画 ]
        private void StopAnimation()
        {
            if (_timer != null)
            {
                if (_timer.Enabled)
                {
                    _timer.Stop();
                }

                _timer.Dispose();
                _timer = null;
            }

            if (_animationList != null)
            {
                foreach (var animationBmp in _animationList)
                {
                    animationBmp?.Dispose();
                }
            }

            _animationList = null;
            _frameIndex = 0;
        }
        #endregion

        #region [ 当前选中 ]
        private int CurrentSelect
        {
            get => _currentSelect;
            set
            {
                _currentSelect = value;
                if (_timer != null)
                {
                    if (_timer.Enabled)
                    {
                        _timer.Stop();
                    }

                    _timer.Dispose();
                    _timer = null;
                }
                SetPicture();
                MainPictureBox.Invalidate();
            }
        }
        #endregion

        #region [ 设置图片 ]
        private void SetPicture()
        {
            _frames = null;
            _mainPicture?.Dispose();
            if (_currentSelect == 0)
            {
                return;
            }

            if (Animate)
            {
                _mainPicture = DoAnimation();
            }
            else
            {
                int body = _currentSelect;
                Animations.Translate(ref body);
                int hue = _customHue;
                if (hue != 0)
                {
                    _frames = Animations.GetAnimation(_currentSelect, _currentSelectAction, _facing, ref hue, true, false);
                }
                else
                {
                    _frames = Animations.GetAnimation(_currentSelect, _currentSelectAction, _facing, ref hue, false, false);
                    _defHue = hue;
                }

                if (_frames != null)
                {
                    if (_frames[0].Bitmap != null)
                    {
                        _mainPicture = new Bitmap(_frames[0].Bitmap);
                        BaseGraphicLabel.Text = $"基础图形: {body} (0x{body:X})";
                        GraphicLabel.Text = $"图形: {_currentSelect} (0x{_currentSelect:X})";
                        HueLabel.Text = $"色调: {hue + 1} (0x{hue + 1:X})";
                    }
                    else
                    {
                        _mainPicture = null;
                    }
                }
                else
                {
                    _mainPicture = null;
                }
            }
        }
        #endregion

        #region [ Bitmap 动画 ]
        private Bitmap DoAnimation()
        {
            if (_timer != null)
            {
                return _animationList[_frameIndex] != null
                    ? new Bitmap(_animationList[_frameIndex])
                    : null;
            }

            int body = _currentSelect;
            Animations.Translate(ref body);
            int hue = _customHue;
            if (hue != 0)
            {
                _frames = Animations.GetAnimation(_currentSelect, _currentSelectAction, _facing, ref hue, true, false);
            }
            else
            {
                _frames = Animations.GetAnimation(_currentSelect, _currentSelectAction, _facing, ref hue, false, false);
                _defHue = hue;
            }

            if (_frames == null)
            {
                return null;
            }

            BaseGraphicLabel.Text = $"基础图形: {body} (0x{body:X})";
            GraphicLabel.Text = $"图形: {_currentSelect} (0x{_currentSelect:X})";
            HueLabel.Text = $"色调: {hue + 1} (0x{hue + 1:X})";
            int count = _frames.Length;
            _animationList = new Bitmap[count];

            for (int i = 0; i < count; ++i)
            {
                _animationList[i] = _frames[i].Bitmap;
            }

            // 检查无效计数，防止除零
            if (count <= 0)
            {
                count = 1;
            }

            _timer = new Timer
            {
                Interval = 1000 / count
            };
            _timer.Tick += AnimTick;
            _timer.Start();
            _frameIndex = 0;

            LoadListViewFrames(); // 重新加载帧

            return _animationList[0] != null ? new Bitmap(_animationList[0]) : null;
        }
        #endregion

        #region [ 动画时钟 ]
        private void AnimTick(object sender, EventArgs e)
        {
            ++_frameIndex;

            if (_frameIndex == _animationList.Length)
            {
                _frameIndex = 0;
            }

            _imageInvalidated = true;

            MainPictureBox.Invalidate();
        }
        #endregion

        #region [ OnPaint_MainPicture ]
        private void OnPaint_MainPicture(object sender, PaintEventArgs e)
        {
            if (_imageInvalidated)
            {
                SetPicture();
            }

            if (_mainPicture != null)
            {
                Point location = Point.Empty;
                Size size = _mainPicture.Size;
                location.X = (MainPictureBox.Width - _mainPicture.Width) / 2;
                location.Y = (MainPictureBox.Height - _mainPicture.Height) / 2;

                Rectangle destRect = new Rectangle(location, size);

                e.Graphics.DrawImage(_mainPicture, destRect, 0, 0, _mainPicture.Width, _mainPicture.Height, GraphicsUnit.Pixel);
            }
            else
            {
                _mainPicture = null;
            }
        }
        #endregion

        #region [ 加载显示帧和更多 listView1 ]
        private void TreeViewMobs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                if (e.Node.Parent.Name == "Mobs" || e.Node.Parent.Name == "Equipment")
                {
                    _currentSelectAction = 0;
                    CurrentSelect = ((int[])e.Node.Tag)[0];
                    if (e.Node.Parent.Name == "Mobs" && _displayType == 1)
                    {
                        _displayType = 0;
                        LoadListView();
                    }
                    else if (e.Node.Parent.Name == "Equipment" && _displayType == 0)
                    {
                        _displayType = 1;
                        LoadListView();
                    }

                    // 在此调用 DoAnimation() 方法来初始化 _animationList
                    DoAnimation();
                }
                else
                {
                    _currentSelectAction = (int)e.Node.Tag;
                    CurrentSelect = ((int[])e.Node.Parent.Tag)[0];
                    if (e.Node.Parent.Parent.Name == "Mobs" && _displayType == 1)
                    {
                        _displayType = 0;
                        LoadListView();
                    }
                    else if (e.Node.Parent.Parent.Name == "Equipment" && _displayType == 0)
                    {
                        _displayType = 1;
                        LoadListView();
                    }

                    // 在此调用 DoAnimation() 方法来初始化 _animationList
                    DoAnimation();
                }
            }
            else
            {
                if (e.Node.Name == "Mobs" && _displayType == 1)
                {
                    _displayType = 0;
                    LoadListView();
                }
                else if (e.Node.Name == "Equipment" && _displayType == 0)
                {
                    _displayType = 1;
                    LoadListView();
                }
                TreeViewMobs.SelectedNode = e.Node.Nodes[0];
            }

            // 这里可以添加代码来更新 toolStripStatusAnimLabel 的文本
            string fileName = Animations.GetFileName(_currentSelect);
            toolStripStatusAminLabel.Text = $"源文件: {fileName}";
        }
        #endregion

        #region [ 动画按钮点击 ]
        private void Animate_Click(object sender, EventArgs e)
        {
            Animate = !Animate;
        }
        #endregion

        #region [ 加载Xml ]
        private bool LoadXml()
        {
            string fileName = Path.Combine(Options.AppDataPath, "Animationlist.xml");
            if (!File.Exists(fileName))
            {
                return false;
            }

            TreeViewMobs.BeginUpdate();
            try
            {
                TreeViewMobs.Nodes.Clear();

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.Load(fileName);
                }
                catch (XmlException ex)
                {
                    MessageBox.Show("加载 XML 文件时出现问题：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                XmlElement xMobs = dom["Graphics"];
                List<TreeNode> nodes = new List<TreeNode>();
                TreeNode node;
                TreeNode typeNode;

                TreeNode rootNode = new TreeNode("生物")
                {
                    Name = "Mobs",
                    Tag = -1
                };
                nodes.Add(rootNode);

                foreach (XmlElement xMob in xMobs.SelectNodes("Mob"))
                {
                    string name = xMob.GetAttribute("name");
                    int value = int.Parse(xMob.GetAttribute("body"));
                    int type = int.Parse(xMob.GetAttribute("type"));
                    node = new TreeNode($"{name} (0x{value:X})")
                    {
                        Tag = new[] { value, type },
                        ToolTipText = Animations.GetFileName(value)
                    };
                    rootNode.Nodes.Add(node);

                    for (int i = 0; i < GetActionNames[type].GetLength(0); ++i)
                    {
                        if (!Animations.IsActionDefined(value, i, 0))
                        {
                            continue;
                        }

                        typeNode = new TreeNode($"{i} {GetActionNames[type][i]}")
                        {
                            Tag = i
                        };
                        node.Nodes.Add(typeNode);
                    }
                }

                rootNode = new TreeNode("装备")
                {
                    Name = "Equipment",
                    Tag = -2
                };
                nodes.Add(rootNode);

                foreach (XmlElement xMob in xMobs.SelectNodes("Equip"))
                {
                    string name = xMob.GetAttribute("name");
                    int value = int.Parse(xMob.GetAttribute("body"));
                    int type = int.Parse(xMob.GetAttribute("type"));
                    node = new TreeNode(name)
                    {
                        Tag = new[] { value, type },
                        ToolTipText = Animations.GetFileName(value)
                    };
                    rootNode.Nodes.Add(node);

                    for (int i = 0; i < GetActionNames[type].GetLength(0); ++i)
                    {
                        if (!Animations.IsActionDefined(value, i, 0))
                        {
                            continue;
                        }

                        typeNode = new TreeNode($"{i} {GetActionNames[type][i]}")
                        {
                            Tag = i
                        };
                        node.Nodes.Add(typeNode);
                    }
                }
                TreeViewMobs.Nodes.AddRange(nodes.ToArray());
                nodes.Clear();
            }
            finally
            {
                TreeViewMobs.EndUpdate();
            }

            return true;
        }
        #endregion

        #region [ 加载列表视图 ]
        private void LoadListView()
        {
            listView.BeginUpdate();
            try
            {
                listView.Clear();
                foreach (TreeNode node in TreeViewMobs.Nodes[_displayType].Nodes)
                {
                    ListViewItem item = new ListViewItem($"({((int[])node.Tag)[0]})", 0)
                    {
                        Tag = ((int[])node.Tag)[0]
                    };
                    listView.Items.Add(item);
                }
            }
            finally
            {
                listView.EndUpdate();
            }
        }
        #endregion

        #region [ listView 选择更改 ]
        private void SelectChanged_listView(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                TreeViewMobs.SelectedNode = TreeViewMobs.Nodes[_displayType].Nodes[listView.SelectedItems[0].Index];
            }
        }
        #endregion

        #region [ ListView 双击 ]
        private void ListView_DoubleClick(object sender, MouseEventArgs e)
        {
            tabControl1.SelectTab(tabPage1);
        }
        #endregion

        #region [ ListView 绘制项 ]       
        private void ListViewDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            int graphic = (int)e.Item.Tag;
            int hue = 0;
            _frames = Animations.GetAnimation(graphic, 0, 1, ref hue, false, true);

            if (_frames == null)
            {
                return;
            }

            Bitmap bmp = _frames[0].Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;

            if (width > e.Bounds.Width)
            {
                width = e.Bounds.Width;
            }

            if (height > e.Bounds.Height)
            {
                height = e.Bounds.Height;
            }


            if (listView.SelectedItems.Contains(e.Item))
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
            }

            e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y, width, height);
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);

            if (listView.SelectedItems.Contains(e.Item))
            {
                e.DrawFocusRectangle();
            }
            else
            {
                using (var pen = new Pen(Color.Gray))
                {
                    e.Graphics.DrawRectangle(pen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                }
            }
        }
        #endregion

        #region [ 色调按钮点击 ]
        private HuePopUpForm _showForm;
        private void OnClick_Hue(object sender, EventArgs e)
        {
            if (_showForm?.IsDisposed == false)
            {
                return;
            }

            _showForm = _customHue == 0
                ? new HuePopUpForm(ChangeHue, _defHue + 1)
                : new HuePopUpForm(ChangeHue, _customHue - 1);

            _showForm.TopMost = true;
            _showForm.Show();
        }
        #endregion

        #region [ 加载列表视图帧 ]
        private void LoadListViewFrames()
        {
            if (_animationList == null || _animationList.Length == 0)
            {
                return;
            }

            listView1.BeginUpdate();
            try
            {
                listView1.Items.Clear();
                for (int frame = 0; frame < _animationList.Length; ++frame)
                {
                    ListViewItem item = new ListViewItem(frame.ToString(), 0)
                    {
                        Tag = frame
                    };
                    listView1.Items.Add(item);
                }
            }
            finally
            {
                listView1.EndUpdate();
                listView1.Invalidate(); // 确保 ListView 更新
            }
        }
        #endregion

        #region [ 帧列表视图绘制项 ]
        private void Frames_ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (_animationList == null)
            {
                return;
            }

            Bitmap bmp = _animationList[(int)e.Item.Tag];
            int width = bmp.Width;
            int height = bmp.Height;

            if (width > e.Bounds.Width)
            {
                width = e.Bounds.Width;
            }
            if (height > e.Bounds.Height)
            {
                height = e.Bounds.Height;
            }

            // 验证当前项是否被选中
            if (listView1.SelectedItems.Contains(e.Item))
            {
                // 更改选中项的背景颜色
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
            }

            e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y, width, height);
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);

            using (var pen = new Pen(Color.Gray))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
        }
        #endregion

        #region [ 滚动条方向 ]
        private void OnScrollFacing(object sender, EventArgs e)
        {
            _facing = (FacingBar.Value - 3) & 7;
            CurrentSelect = CurrentSelect;

            // 为当前选中的元素重新加载帧
            DoAnimation();

            if (tabControl1.SelectedTab == tabPage3)
            {
                LoadListViewFrames();
                listView1.Invalidate(); // 确保 ListView 更新
            }
            else
            {
                SetPicture();
                MainPictureBox.Invalidate();
            }
        }
        #endregion

        #region [ 选项卡选择更改 ]
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPage3)
            {
                LoadListViewFrames();
                listView1.Invalidate();
            }
            else
            {
                SetPicture();
                MainPictureBox.Invalidate();
            }
        }
        #endregion

        #region [ 排序按钮点击 ]
        private void OnClick_Sort(object sender, EventArgs e)
        {
            _sortAlpha = !_sortAlpha;

            TreeViewMobs.BeginUpdate();
            try
            {
                TreeViewMobs.TreeViewNodeSorter = !_sortAlpha ? new GraphicSorter() : (IComparer)new AlphaSorter();
                TreeViewMobs.Sort();
            }
            finally
            {
                TreeViewMobs.EndUpdate();
            }

            LoadListView();
        }
        #endregion

        #region [ 移除按钮点击 ]
        private void OnClickRemove(object sender, EventArgs e)
        {
            TreeNode node = TreeViewMobs.SelectedNode;
            if (node?.Parent == null)
            {
                return;
            }

            if (node.Parent.Name != "Mobs" && node.Parent.Name != "Equipment")
            {
                node = node.Parent;
            }

            node.Remove();
            LoadListView();
        }
        #endregion

        #region [ 动画编辑按钮点击 ]
        private AnimationEditForm _animEditFormEntry;
        private void OnClickAnimationEdit(object sender, EventArgs e)
        {
            if (_animEditFormEntry?.IsDisposed == false)
            {
                return;
            }

            _animEditFormEntry = new AnimationEditForm();
            //animEditEntry.TopMost = true; // TODO: 是否应该置顶？
            _animEditFormEntry.Show();
        }
        #endregion

        #region [ 查找新条目按钮点击 ]
        private AnimationListNewEntriesForm _animNewEntryForm;
        private void OnClickFindNewEntries(object sender, EventArgs e)
        {
            if (_animNewEntryForm?.IsDisposed == false)
            {
                return;
            }

            _animNewEntryForm = new AnimationListNewEntriesForm(IsAlreadyDefined, AddGraphic, GetActionNames)
            {
                TopMost = true
            };
            _animNewEntryForm.Show();
        }
        #endregion

        #region [ 重写Xml ]
        private void RewriteXml(object sender, EventArgs e)
        {
            TreeViewMobs.BeginUpdate();
            try
            {
                TreeViewMobs.TreeViewNodeSorter = new GraphicSorter();
                TreeViewMobs.Sort();
            }
            finally
            {
                TreeViewMobs.EndUpdate();
            }

            string fileName = Path.Combine(Options.OutputPath, "Animationlist.xml"); // 正确的保存路径

            XmlDocument dom = new XmlDocument();
            XmlDeclaration decl = dom.CreateXmlDeclaration("1.0", "utf-8", null);
            dom.AppendChild(decl);
            XmlElement sr = dom.CreateElement("Graphics");
            XmlComment comment = dom.CreateComment("生物选项卡中的条目");
            sr.AppendChild(comment);
            comment = dom.CreateComment("Name=显示的名称");
            sr.AppendChild(comment);
            comment = dom.CreateComment("body=图形");
            sr.AppendChild(comment);
            comment = dom.CreateComment("type=0:怪物, 1:海洋生物, 2:动物, 3:人类/装备");
            sr.AppendChild(comment);

            XmlElement elem;
            foreach (TreeNode node in TreeViewMobs.Nodes[0].Nodes)
            {
                elem = dom.CreateElement("Mob");
                elem.SetAttribute("name", node.Text);
                elem.SetAttribute("body", ((int[])node.Tag)[0].ToString());
                elem.SetAttribute("type", ((int[])node.Tag)[1].ToString());

                sr.AppendChild(elem);
            }

            foreach (TreeNode node in TreeViewMobs.Nodes[1].Nodes)
            {
                elem = dom.CreateElement("Equip");
                elem.SetAttribute("name", node.Text);
                elem.SetAttribute("body", ((int[])node.Tag)[0].ToString());
                elem.SetAttribute("type", ((int[])node.Tag)[1].ToString());
                sr.AppendChild(elem);
            }

            dom.AppendChild(sr);

            // 将 XML 文件保存到 Options.OutputPath 目录
            dom.Save(fileName);

            MessageBox.Show("XML 已保存", "重写", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
        #endregion

        #region [ 重写Xml2 ]
        private void RewriteXml2(object sender, EventArgs e)
        {
            TreeViewMobs.BeginUpdate();
            try
            {
                TreeViewMobs.TreeViewNodeSorter = new GraphicSorter();
                TreeViewMobs.Sort();
            }
            finally
            {
                TreeViewMobs.EndUpdate();
            }

            string fileName = Path.Combine(Options.OutputPath, "Animationlist.xml"); // 正确的保存路径

            string CleanNodeName(string nodeName)
            {
                // 检查节点名称是否包含 (0x1)、(0x2) 等
                // 如果有，则删除它们以及括号内的 ID
                int index;
                while ((index = nodeName.IndexOf(" (0x")) >= 0)
                {
                    nodeName = nodeName.Substring(0, index);
                }
                return nodeName;
            }

            XmlDocument dom = new XmlDocument();
            XmlDeclaration decl = dom.CreateXmlDeclaration("1.0", "utf-8", null);
            dom.AppendChild(decl);
            XmlElement sr = dom.CreateElement("Graphics");
            XmlComment comment = dom.CreateComment("生物选项卡中的条目");
            sr.AppendChild(comment);
            comment = dom.CreateComment("Name=显示的名称");
            sr.AppendChild(comment);
            comment = dom.CreateComment("body=图形");
            sr.AppendChild(comment);
            comment = dom.CreateComment("type=0:怪物, 1:海洋生物, 2:动物, 3:人类/装备");
            sr.AppendChild(comment);

            XmlElement elem;
            foreach (TreeNode node in TreeViewMobs.Nodes[0].Nodes)
            {
                string nodeNameCleaned = CleanNodeName(node.Text);
                int nodeId = ((int[])node.Tag)[0]; // 假设 ID 存储在 node.Tag 数组的第一个元素中
                elem = dom.CreateElement("Mob");
                elem.SetAttribute("name", $"{nodeNameCleaned}");
                elem.SetAttribute("body", nodeId.ToString());
                elem.SetAttribute("type", ((int[])node.Tag)[1].ToString());

                sr.AppendChild(elem);
            }

            foreach (TreeNode node in TreeViewMobs.Nodes[1].Nodes)
            {
                string nodeNameCleaned = CleanNodeName(node.Text);
                int nodeId = ((int[])node.Tag)[0]; // 假设 ID 存储在 node.Tag 数组的第一个元素中
                elem = dom.CreateElement("Equip");
                elem.SetAttribute("name", $"{nodeNameCleaned} ({nodeId})");
                elem.SetAttribute("body", nodeId.ToString());
                elem.SetAttribute("type", ((int[])node.Tag)[1].ToString());
                sr.AppendChild(elem);
            }

            dom.AppendChild(sr);

            // 将 XML 文件保存到 Options.OutputPath 目录
            dom.Save(fileName);

            MessageBox.Show("XML 已保存", "重写", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
        #endregion

        #region [ 提取图像为Bmp ]
        private void Extract_Image_ClickBmp(object sender, EventArgs e)
        {
            ExtractImage(ImageFormat.Bmp);
        }
        #endregion

        #region [ 提取图像为Tiff ]
        private void Extract_Image_ClickTiff(object sender, EventArgs e)
        {
            ExtractImage(ImageFormat.Tiff);
        }
        #endregion

        #region [ 提取图像为Jpg ]
        private void Extract_Image_ClickJpg(object sender, EventArgs e)
        {
            ExtractImage(ImageFormat.Jpeg);
        }
        #endregion

        #region [ 提取图像为Png ]
        private void Extract_Image_ClickPng(object sender, EventArgs e)
        {
            ExtractImage(ImageFormat.Png);
        }
        #endregion

        #region [ 提取图像 ]
        /*private void ExtractImage(ImageFormat imageFormat)
        {
            string what = "Mob";
            if (_displayType == 1)
            {
                what = "Equipment";
            }

            string fileExtension = Utils.GetFileExtensionFor(imageFormat);
            string fileName = Path.Combine(Options.OutputPath, $"{what} {_currentSelect}.{fileExtension}");

            Bitmap sourceBitmap = Animate ? _animationList[0] : _mainPicture;
            using (Bitmap newBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height))
            {
                using (Graphics newGraph = Graphics.FromImage(newBitmap))
                {
                    newGraph.FillRectangle(Brushes.White, 0, 0, newBitmap.Width, newBitmap.Height);
                    newGraph.DrawImage(sourceBitmap, new Point(0, 0));
                    newGraph.Save();
                }

                newBitmap.Save(fileName, imageFormat);
            }

            MessageBox.Show($"{what} saved to {fileName}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }*/

        private void ExtractImage(ImageFormat imageFormat)
        {
            string what = _displayType == 1 ? "装备" : "生物";
            string fileExtension = Utils.GetFileExtensionFor(imageFormat);
            string fileName = Path.Combine(Options.OutputPath, $"{what} {_currentSelect}.{fileExtension}");

            Bitmap sourceBitmap = Animate ? _animationList[0] : _mainPicture;
            using (Bitmap newBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height))
            {
                using (Graphics newGraph = Graphics.FromImage(newBitmap))
                {
                    newGraph.FillRectangle(Brushes.White, 0, 0, newBitmap.Width, newBitmap.Height);
                    newGraph.DrawImage(sourceBitmap, new Point(0, 0));
                    newGraph.Save();
                }

                newBitmap.Save(fileName, imageFormat);
            }

            PlayCustomSound();

            MessageBox.Show($"{what} 已保存至 {fileName}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        #endregion

        #region [ 提取动画为Bmp ]
        private void OnClickExtractAnimBmp(object sender, EventArgs e)
        {
            ExportAnimationFrames(ImageFormat.Bmp);
        }
        #endregion

        #region [ 提取动画为Tiff ]
        private void OnClickExtractAnimTiff(object sender, EventArgs e)
        {
            ExportAnimationFrames(ImageFormat.Tiff);
        }
        #endregion

        #region [ 提取动画为Jpg ]
        private void OnClickExtractAnimJpg(object sender, EventArgs e)
        {
            ExportAnimationFrames(ImageFormat.Jpeg);
        }
        #endregion

        #region [ 提取动画为Png ]
        private void OnClickExtractAnimPng(object sender, EventArgs e)
        {
            ExportAnimationFrames(ImageFormat.Png);
        }
        #endregion

        #region [ 导出动画帧 ]
        private void ExportAnimationFrames(ImageFormat imageFormat)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("没有可导出的动画帧。", "错误", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            string what = _displayType == 1 ? "装备" : "生物";
            string fileExtension = Utils.GetFileExtensionFor(imageFormat);
            string fileName = Path.Combine(Options.OutputPath, $"{what} {_currentSelect}");

            int exportedCount = 0;

            try
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Tag == null || !(item.Tag is int index) || index < 0 || index >= _animationList.Length)
                        continue;

                    Bitmap bmp = _animationList[index];
                    using (Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height))
                    {
                        using (Graphics newGraph = Graphics.FromImage(newBitmap))
                        {
                            newGraph.FillRectangle(Brushes.White, 0, 0, newBitmap.Width, newBitmap.Height);
                            newGraph.DrawImage(bmp, new Point(0, 0));
                            newGraph.Save();
                        }

                        string finalFileName = $"{fileName}-{index}.{fileExtension}";
                        newBitmap.Save(finalFileName, imageFormat);
                        exportedCount++;
                    }
                }

                PlayCustomSound();

                MessageBox.Show($"{exportedCount} 帧已成功导出到 '{fileName}-X.{fileExtension}'", "导出完成", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出帧时出错：{ex.Message}", "导出错误", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }
        #endregion

        #region [ 导出单帧为Bmp ]
        private void OnClickExportFrameBmp(object sender, EventArgs e)
        {
            ExportSingleFrame(ImageFormat.Bmp);
        }
        #endregion

        #region [ 导出单帧为Tiff ]
        private void OnClickExportFrameTiff(object sender, EventArgs e)
        {
            ExportSingleFrame(ImageFormat.Tiff);
        }
        #endregion

        #region [ 导出单帧为Jpg ]
        private void OnClickExportFrameJpg(object sender, EventArgs e)
        {
            ExportSingleFrame(ImageFormat.Jpeg);
        }
        #endregion

        #region [ 导出单帧为Png ]
        private void OnClickExportFramePng(object sender, EventArgs e)
        {
            ExportSingleFrame(ImageFormat.Png);
        }
        #endregion

        #region [ 导出单帧 ]
        private void ExportSingleFrame(ImageFormat imageFormat)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                return;
            }

            string what = "生物";
            if (_displayType == 1)
            {
                what = "装备";
            }

            string fileExtension = Utils.GetFileExtensionFor(imageFormat);
            string fileName = Path.Combine(Options.OutputPath, $"{what} {_currentSelect}");

            Bitmap bit = _animationList[(int)listView1.SelectedItems[0].Tag];
            using (Bitmap newBitmap = new Bitmap(bit.Width, bit.Height))
            {
                using (Graphics newGraph = Graphics.FromImage(newBitmap))
                {
                    newGraph.FillRectangle(Brushes.White, 0, 0, newBitmap.Width, newBitmap.Height);
                    newGraph.DrawImage(bit, new Point(0, 0));
                    newGraph.Save();
                }

                newBitmap.Save($"{fileName}-{(int)listView1.SelectedItems[0].Tag}.{fileExtension}", imageFormat);
            }
        }
        #endregion

        #region [ 复制帧到剪贴板 ]
        private void CopyFrameToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查 listView1 中是否选中了帧
            if (listView1.SelectedItems.Count > 0)
            {
                // 将选中的帧复制到剪贴板
                int selectedFrameIndex = (int)listView1.SelectedItems[0].Tag;
                if (selectedFrameIndex >= 0 && selectedFrameIndex < _animationList.Length)
                {
                    Clipboard.SetImage(_animationList[selectedFrameIndex]);

                    // 显示消息框确认图形已复制
                    MessageBox.Show("图形已复制到剪贴板。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion

        #region [ 导入图像 ]
        private void ImportImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查 listView1 中是否选中了帧
            if (listView1.SelectedItems.Count > 0)
            {
                // 检查缓存中是否有图像
                if (Clipboard.ContainsImage())
                {
                    int selectedFrameIndex = (int)listView1.SelectedItems[0].Tag;
                    if (selectedFrameIndex >= 0 && selectedFrameIndex < _animationList.Length)
                    {
                        // 从剪贴板导入图形到选中的 ListViewItem（帧）
                        Image imageFromClipboard = Clipboard.GetImage();
                        _animationList[selectedFrameIndex] = (Bitmap)imageFromClipboard;

                        // 这里可以执行进一步的操作，例如查看或保存更新后的图形
                        // ...

                        // 刷新 ListView 以显示更改
                        LoadListViewFrames();

                        // 显示消息框确认图形已导入
                        MessageBox.Show("图形已导入并保存在帧中。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // 如果缓存中没有图像，显示错误消息
                    MessageBox.Show("缓存中没有图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region [ 动画列表编辑 ]
        private AnimationListEditorForm _editorForm = null;

        private void AnimationlistEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = Path.Combine(Options.AppDataPath, "Animationlist.xml");

            if (_editorForm == null || _editorForm.IsDisposed)
            {
                _editorForm = new AnimationListEditorForm(fileName);
                _editorForm.Show();
            }
            else
            {
                // 窗体已打开，将其置于前台
                _editorForm.BringToFront();
            }
        }
        #endregion

        #region [ 从列表视图加载帧 ]
        private void LoadFramesFromListView()
        {
            // ListView 中的条目数
            int count = listView1.Items.Count;

            // 使用 ListView 中的条目数初始化 _frames 数组
            _frames = new AnimationFrame[count];

            for (int i = 0; i < count; i++)
            {
                // 获取 ListView 项的标签，该标签代表帧的索引
                int frameIndex = (int)listView1.Items[i].Tag;

                // 从 _animationList 加载对应的帧
                Bitmap frameBitmap = _animationList[frameIndex];

                // 用位图初始化 AnimationFrame
                _frames[i] = new AnimationFrame { Bitmap = frameBitmap };
            }
        }
        #endregion

        #region [ 导出动画GIF ]
        /*private void ExportAnimatedGif(bool looping)
        {
            // 检查帧是否已加载
            if (_frames == null || _frames.Length == 0)
            {
                MessageBox.Show("帧未加载。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string baseFileName = $"{(_displayType == 1 ? "Equipment" : "Mob")} {_currentSelect}.gif";
            string outputFile = Path.Combine(Options.OutputPath, baseFileName);
            int fileIndex = 1;

            // 检查路径是否存在且可写
            if (!Directory.Exists(Options.OutputPath))
            {
                MessageBox.Show($"输出路径 {Options.OutputPath} 不存在。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 如果文件已存在，则递增文件名
            while (File.Exists(outputFile))
            {
                outputFile = Path.Combine(Options.OutputPath, $"{(_displayType == 1 ? "Equipment" : "Mob")} {_currentSelect} ({fileIndex}).gif");
                fileIndex++;
            }

            try
            {
                var maxFrameSize = new Size(0, 0);

                foreach (var frame in _frames)
                {
                    if (frame?.Bitmap != null)
                    {
                        maxFrameSize.Width = Math.Max(maxFrameSize.Width, frame.Bitmap.Width);
                        maxFrameSize.Height = Math.Max(maxFrameSize.Height, frame.Bitmap.Height);
                    }
                }

                using (var gif = AnimatedGif.AnimatedGif.Create(outputFile, delay: 150))
                {
                    foreach (var frame in _frames)
                    {
                        if (frame?.Bitmap == null)
                        {
                            continue;
                        }

                        using (Bitmap target = new Bitmap(maxFrameSize.Width, maxFrameSize.Height))
                        {
                            using (Graphics g = Graphics.FromImage(target))
                            {
                                g.DrawImage(frame.Bitmap, 0, 0);
                            }
                            gif.AddFrame(target, delay: -1, quality: GifQuality.Bit8);
                        }
                    }
                }

                if (!looping)
                {
                    using (var stream = new FileStream(outputFile, FileMode.Open, FileAccess.Write))
                    {
                        stream.Seek(28, SeekOrigin.Begin);
                        stream.WriteByte(0);
                    }
                }

                MessageBox.Show($"游戏内动画已保存至 {outputFile}", "已保存", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建 GIF 时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

        private void ExportAnimatedGif(bool looping)
        {
            if (_frames == null || _frames.Length == 0)
            {
                MessageBox.Show("帧未加载。", "错误", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            string baseFileName = $"{(_displayType == 1 ? "装备" : "生物")} {_currentSelect}.gif";
            string outputFile = Path.Combine(Options.OutputPath, baseFileName);
            int fileIndex = 1;

            if (!Directory.Exists(Options.OutputPath))
            {
                MessageBox.Show($"输出路径 {Options.OutputPath} 不存在。", "错误", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            while (File.Exists(outputFile))
            {
                outputFile = Path.Combine(Options.OutputPath, $"{(_displayType == 1 ? "装备" : "生物")} {_currentSelect} ({fileIndex}).gif");
                fileIndex++;
            }

            try
            {
                var maxFrameSize = new Size(0, 0);

                foreach (var frame in _frames)
                {
                    if (frame?.Bitmap != null)
                    {
                        maxFrameSize.Width = Math.Max(maxFrameSize.Width, frame.Bitmap.Width);
                        maxFrameSize.Height = Math.Max(maxFrameSize.Height, frame.Bitmap.Height);
                    }
                }

                using (var gif = AnimatedGif.AnimatedGif.Create(outputFile, delay: 150))
                {
                    foreach (var frame in _frames)
                    {
                        if (frame?.Bitmap == null)
                        {
                            continue;
                        }

                        using (Bitmap target = new Bitmap(maxFrameSize.Width, maxFrameSize.Height))
                        {
                            using (Graphics g = Graphics.FromImage(target))
                            {
                                g.DrawImage(frame.Bitmap, 0, 0);
                            }
                            gif.AddFrame(target, delay: -1, quality: GifQuality.Bit8);
                        }
                    }
                }

                if (!looping)
                {
                    using (var stream = new FileStream(outputFile, FileMode.Open, FileAccess.Write))
                    {
                        stream.Seek(28, SeekOrigin.Begin);
                        stream.WriteByte(0);
                    }
                }

                PlayCustomSound();

                MessageBox.Show($"游戏内动画已保存至 {outputFile}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建 GIF 时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }
        #endregion

        #region [ 播放自定义音效 ]
        private void PlayCustomSound()
        {
            string soundFilePath = Path.Combine(Application.StartupPath, "Sound.wav");
            if (File.Exists(soundFilePath))
            {
                using (System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundFilePath))
                {
                    player.PlaySync();
                }
            }
        }
        #endregion

        #region [ 提取动画GIF（循环） ]
        private void OnClickExtractAnimGifLooping(object sender, EventArgs e)
        {
            // 从 ListView 加载帧
            LoadFramesFromListView();
            ExportAnimatedGif(true);
        }
        #endregion

        #region [ 提取动画GIF（不循环） ]
        private void OnClickExtractAnimGifNoLooping(object sender, EventArgs e)
        {
            LoadFramesFromListView();
            ExportAnimatedGif(false);
        }
        #endregion

        #region [ XML编辑器 ]
        private EditorXML _editorXmlInstance;

        private void xMLEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string outputPath = Options.OutputPath; // 从选项获取输出路径

            if (_editorXmlInstance == null || _editorXmlInstance.IsDisposed)
            {
                // 将 outputPath 传递给构造函数
                _editorXmlInstance = new EditorXML(outputPath);
                _editorXmlInstance.Show();
            }
            else
            {
                _editorXmlInstance.BringToFront();
            }
        }

        #endregion

        #region [ 显示帧边框 ]
        private bool PaintEventAttached = false;

        private void frameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PaintEventAttached)
            {
                MainPictureBox.Paint += DrawFrameBounds;
                PaintEventAttached = true;
            }
            else
            {
                MainPictureBox.Paint -= DrawFrameBounds;
                PaintEventAttached = false;
            }

            MainPictureBox.Invalidate(); // 立即重绘 PictureBox
        }
        #endregion

        #region [ 绘制帧边界 ]
        private void DrawFrameBounds(object sender, PaintEventArgs e)
        {
            if (_animationList == null || _animationList.Length == 0 || listView1.SelectedItems.Count == 0)
                return; // 如果没有帧或没有选中帧

            // 确定当前选中的帧的索引
            if (!(listView1.SelectedItems[0].Tag is int index) || index < 0 || index >= _animationList.Length)
                return;

            Bitmap currentFrame = _animationList[index];

            // 计算在 PictureBox 中的位置
            int frameWidth = currentFrame.Width;
            int frameHeight = currentFrame.Height;

            // 确定帧的实际位置（例如，如果不是左上角对齐）
            int x = (MainPictureBox.ClientSize.Width - frameWidth) / 2;
            int y = (MainPictureBox.ClientSize.Height - frameHeight) / 2;

            using (Pen pen = new Pen(Color.Red, 2)) // 红色边框，2 像素宽
            {
                e.Graphics.DrawRectangle(pen, x, y, frameWidth - 1, frameHeight - 1);
            }
        }
        #endregion

        #region [ listView1 选中索引更改 ]
        // 此方法确保当帧更改时更新边框
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PaintEventAttached)
            {
                MainPictureBox.Invalidate(); // 强制重绘 PictureBox
            }
        }
        #endregion


    }

    #region [ class AlphaSorter ]
    public class AlphaSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            if (tx.Parent == null) // 不更改“生物”和“装备”的顺序
            {
                return (int)tx.Tag == -1 ? -1 : 1;
            }
            if (tx.Parent.Parent != null)
            {
                return (int)tx.Tag - (int)ty.Tag;
            }

            return string.CompareOrdinal(tx.Text, ty.Text);
        }
    }
    #endregion

    #region [ class GraphicSorter ]
    public class GraphicSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            if (tx.Parent == null)
            {
                return (int)tx.Tag == -1 ? -1 : 1;
            }

            if (tx.Parent.Parent != null)
            {
                return (int)tx.Tag - (int)ty.Tag;
            }

            int[] ix = (int[])tx.Tag;
            int[] iy = (int[])ty.Tag;

            if (ix[0] == iy[0])
            {
                return 0;
            }

            if (ix[0] < iy[0])
            {
                return -1;
            }

            return 1;
        }
    }
    #endregion
}