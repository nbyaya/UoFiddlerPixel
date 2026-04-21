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

#nullable enable annotations

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Models.Uop;
using UoFiddler.Controls.Models.Uop.Imaging;
using UoFiddler.Controls.Uop;
using Models = UoFiddler.Controls.Models;

namespace UoFiddler.Controls.Forms
{
    public partial class AnimationEditForm : Form
    {
        private static readonly int[] _animCx = new int[5];
        private static readonly int[] _animCy = new int[5];
        private bool _loaded;
        private int _fileType;
        private int _currentAction;
        private int _currentBody;
        private int _currentDir;
        private Point _framePoint; // 第一个动画（原始）的点
        private Point _additionalFramePoint; // 第二个动画的独立点，允许独立定位
        private bool _showOnlyValid;
        private static bool _drawEmpty;
        private static bool _drawFull;
        private static bool _drawBoundingBox;
        private static bool _drawCroppingBox;
        private static readonly Color _whiteConvert = Color.FromArgb(255, 255, 255, 255);

        private UopAnimationDataManager _uopManager;
        private bool _useUopMapping = true;

        private const int UOP_FILE_TYPE = 6;

        private static readonly Pen _blackUnDrawTransparent = new Pen(Color.FromArgb(0, 0, 0, 0), 1);
        private static readonly Pen _blackUnDrawOpaque = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
        private static Pen _blackUndraw = _blackUnDrawOpaque;

        private static readonly SolidBrush _whiteUnDrawTransparent = new SolidBrush(Color.FromArgb(0, 255, 255, 255));
        private static readonly SolidBrush _whiteUnDrawOpaque = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        private static SolidBrush _whiteUnDraw = _whiteUnDrawOpaque;
        private int _previousBody = -1;
        private int _previousAction = -1;

        // Gemini 添加的字段
        private float _zoomFactor = 1.0f;
        private bool _relativeMode = false;
        private int _accumulatedRelativeX = 0;
        private int _accumulatedRelativeY = 0;
        private int _lastRelativeX = 0;
        private int _lastRelativeY = 0;
        private bool _updatingUi = false;

        // 第二个动画
        private bool _secondAnimActivated = false;
        private int _secondAnimID = 0;
        private int _secondAnimFileIndex = 0; // 0=anim, 1=anim2...
        private bool _secondAnimPseudoVisu = false;
        private bool _isSecondAnimInFront = false;
        private UopAnimationDataManager _secondUopManager;

        // 序列选项卡 – 运行时状态        
        private System.ComponentModel.BindingList<SequenceViewModelItem> _sequenceViewModelList; // DataGridView 的 ViewModel
        private int _seqPreviewDirection; // 序列选项卡中预览的方向
        private int _seqPreviewFrameIndex; // 序列选项卡中预览的帧索引
        private int _seqCurrentAction = -1; // 当前正在编辑的序列的动作
        private Models.Uop.AnimationSequenceEntry _currentSeqEntry; // 当前正在编辑的序列条目

        private bool isAnimationVisible = false; // 第二个动画
        private AnimIdx additionalAnimation = null; // 第二个动画

        private static bool _drawCrosshair = true; // 默认 = 可见

        private HexCompareForm _hexCompare; // 用于在两个动画之间比较帧的十六进制比较窗体

        // 背景预览
        private Bitmap _backgroundImage = null;
        private string _backgroundMode = "None";


        #region [ 偏移量 人类 ]
        private static readonly int[][][] Offsets = new int[][][]
        {
            new int[][] // 方向 0
            {
                new int[] { -12, -53 }, // 帧 0
                new int[] { -13, -55 }, // 帧 1
                new int[] { -12, -55 }, // 帧 2
                new int[] { -12, -53 }, // 帧 3
                new int[] { -12, -53 }, // 帧 4
                new int[] { -12, -53 }, // 帧 5
                new int[] { -12, -55 }, // 帧 6
                new int[] { -11, -54 }, // 帧 7
                new int[] { -12, -53 }, // 帧 8
                new int[] { -12, -53 }  // 帧 9
            },
            new int[][] // 方向 1
            {
                new int[] { -16, -54 }, // 帧 0
                new int[] { -14, -55 }, // 帧 1
                new int[] { -13, -55 }, // 帧 2
                new int[] { -13, -55 }, // 帧 3
                new int[] { -16, -53 }, // 帧 4
                new int[] { -19, -53 }, // 帧 5
                new int[] { -12, -55 }, // 帧 6
                new int[] { -9, -56 },  // 帧 7
                new int[] { -10, -55 }, // 帧 8
                new int[] { -14, -54 }  // 帧 9
            },
            new int[][] // 方向 2
            {
                new int[] { -22, -54 }, // 帧 0
                new int[] { -14, -56 }, // 帧 1
                new int[] { -11, -57 }, // 帧 2
                new int[] { -13, -55 }, // 帧 3
                new int[] { -18, -55 }, // 帧 4
                new int[] { -22, -54 }, // 帧 5
                new int[] { -13, -56 }, // 帧 6
                new int[] { -14, -56 }, // 帧 7
                new int[] { -15, -56 }, // 帧 8
                new int[] { -16, -55 }  // 帧 9
            },
            new int[][] // 方向 3
            {
                new int[] { -17, -56 }, // 帧 0
                new int[] { -12, -56 }, // 帧 1
                new int[] { -13, -57 }, // 帧 2
                new int[] { -15, -57 }, // 帧 3
                new int[] { -15, -56 }, // 帧 4
                new int[] { -16, -56 }, // 帧 5
                new int[] { -16, -57 }, // 帧 6
                new int[] { -14, -57 }, // 帧 7
                new int[] { -14, -57 }, // 帧 8
                new int[] { -15, -56 }  // 帧 9
            },
            new int[][] // 方向 4
            {
                new int[] { -10, -56 }, // 帧 0
                new int[] { -11, -57 }, // 帧 1
                new int[] { -11, -58 }, // 帧 2
                new int[] { -11, -58 }, // 帧 3
                new int[] { -12, -57 }, // 帧 4
                new int[] { -12, -57 }, // 帧 5
                new int[] { -12, -58 }, // 帧 6
                new int[] { -12, -58 }, // 帧 7
                new int[] { -11, -57 }, // 帧 8
                new int[] { -10, -57 }  // 帧 9
            }
        };
        #endregion

        #region [ 马奔跑偏移量 ]
        private static readonly int[][][] HorseRunOffsets = new int[][][]
        {
            new int[][] // 方向 0
            {
                new int[] { -14, -73 }, // 帧 0
                new int[] { -16, -74 }, // 帧 1
                new int[] { -17, -69 }, // 帧 2
                new int[] { -16, -73 }, // 帧 3
                new int[] { -16, -74 }, // 帧 4                
            },
            new int[][] // 方向 1
            {
                new int[] { -18, -74 }, // 帧 0
                new int[] { -20, -74 }, // 帧 1
                new int[] { -21, -69 }, // 帧 2
                new int[] { -20, -73 }, // 帧 3
                new int[] { -18, -75 }, // 帧 4                
            },
            new int[][] // 方向 2
            {
                new int[] { -14, -75 }, // 帧 0
                new int[] { -15, -76 }, // 帧 1
                new int[] { -15, -71 }, // 帧 2
                new int[] { -15, -75 }, // 帧 3
                new int[] { -14, -76 }, // 帧 4                
            },
            new int[][] // 方向 3
            {
                new int[] { -18, -76 }, // 帧 0
                new int[] { -19, -77 }, // 帧 1
                new int[] { -20, -72 }, // 帧 2
                new int[] { -19, -76 }, // 帧 3
                new int[] { -19, -76 }, // 帧 4                
            },
            new int[][] // 方向 4
            {
                new int[] { -13, -76 }, // 帧 0
                new int[] { -14, -77 }, // 帧 1
                new int[] { -15, -73 }, // 帧 2
                new int[] { -14, -76 }, // 帧 3
                new int[] { -14, -77 }, // 帧 4                
            }
        };
        #endregion

        public AnimationEditForm()
        {
            InitializeComponent();
            Icon = Options.GetFiddlerIcon();

            FramesListView.MultiSelect = true;
            FramesListView.ItemSelectionChanged += FramesListView_ItemSelectionChanged;

            _fileType = 0;
            _currentDir = 0;
            _framePoint = new Point(AnimationPictureBox.Width / 2, AnimationPictureBox.Height / 2);
            _showOnlyValid = false;
            _loaded = false;

            InitializeSequenceTab();
        }

        private readonly string[][] _animNames =
        {
            new string[]
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
            }, //动物
            new string[]
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
            }, //怪物
            new string[]
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
            }, //人类
            new string[]
            {
                "行走", "奔跑", "空闲", "进食", "警觉", "攻击1", "攻击2", "受击", "死亡1", "空闲",
                "烦躁", "躺下", "死亡2", "攻击3", "弓攻击", "弩攻击", "投掷攻击",
                "掠夺", "踩踏", "施法2", "施法3", "右格挡", "左格挡", "飞行", "起飞", "空中受击"
            } // UOP 生物（扩展）
        };

        private void LoadMulAnimations()
        {
            AnimationListTreeView.BeginUpdate();
            try
            {
                AnimationListTreeView.Nodes.Clear();
                _uopManager = null;

                int count = Animations.GetAnimCount(_fileType);
                TreeNode[] nodes = new TreeNode[count];
                int animationCount = 0;

                for (int i = 0; i < count; ++i)
                {
                    int animLength = Animations.GetAnimLength(i, _fileType);
                    string type = animLength == 22 ? "H" : animLength == 13 ? "L" : "P";
                    TreeNode node = new TreeNode
                    {
                        Tag = i,
                        Text = $"{type}: {i} ({BodyConverter.GetTrueBody(_fileType, i)})"
                    };

                    bool valid = false;
                    for (int j = 0; j < animLength; ++j)
                    {
                        TreeNode treeNode = new TreeNode
                        {
                            Tag = j,
                            Text = string.Format("{0:D2} {1}", j,
                                _animNames[animLength == 22 ? 1 : animLength == 13 ? 0 : 2][j])
                        };

                        if (AnimationEdit.IsActionDefined(_fileType, i, j))
                        {
                            valid = true;
                        }
                        else
                        {
                            treeNode.ForeColor = Color.Red;
                        }

                        node.Nodes.Add(treeNode);
                    }

                    if (valid)
                    {
                        animationCount++;
                    }
                    else
                    {
                        if (_showOnlyValid)
                        {
                            continue;
                        }
                        // ✅ 考虑 checkBoxIDBlue
                        node.ForeColor = checkBoxIDBlue.Checked ? Color.Blue : Color.Red;
                    }

                    nodes[i] = node;
                }

                AnimationListTreeView.Nodes.AddRange(nodes.Where(n => n != null).ToArray());

                // ✅ 更新状态标签
                toolStripStatusDisplayLabelAnimation.Text =
                    $"动画数量: {animationCount}";
            }
            finally
            {
                AnimationListTreeView.EndUpdate();
            }

            if (AnimationListTreeView.Nodes.Count > 0)
            {
                AnimationListTreeView.SelectedNode = AnimationListTreeView.Nodes[0];
            }
        }

        #region [ OnLoad ]
        private void OnLoad(object sender, EventArgs e)
        {
            if (_loaded)
            {
                return;
            }
            _loaded = true;

            Options.LoadedUltimaClass["AnimationEdit"] = true;
            ControlEvents.FilePathChangeEvent += OnFilePathChangeEvent;

            // ── 主动画文件选择
            SelectFileToolStripComboBox.Items.Clear();
            SelectFileToolStripComboBox.Items.Add("选择动画文件");

            string root = Files.RootDir;
            if (!string.IsNullOrEmpty(root) && Directory.Exists(root))
            {
                var mulFiles = new List<string>();
                for (int i = 1; i <= 5; ++i)
                {
                    string fileName = (i == 1) ? "anim.mul" : $"anim{i}.mul";
                    if (!string.IsNullOrEmpty(Files.GetFilePath(fileName)))
                    {
                        mulFiles.Add(Path.GetFileNameWithoutExtension(fileName));
                    }
                }
                mulFiles.Sort();
                SelectFileToolStripComboBox.Items.AddRange(mulFiles.ToArray());

                var uopFiles = Directory.GetFiles(root, "AnimationFrame*.uop")
                                       .Select(Path.GetFileName)
                                       .ToList();
                uopFiles.Sort();
                SelectFileToolStripComboBox.Items.AddRange(uopFiles.ToArray());
            }
            SelectFileToolStripComboBox.SelectedIndex = 0;

            // ── 缩放
            ZoomNumericUpDown.Items.Clear();
            ZoomNumericUpDown.Items.AddRange(new object[] { "25%", "50%", "100%", "200%", "300%", "400%", "500%" });
            ZoomNumericUpDown.SelectedIndex = ZoomNumericUpDown.Items.IndexOf("100%");

            // ── 第二个动画文件
            SecondAnimFileComboBox.Items.Clear();
            if (!string.IsNullOrEmpty(root) && Directory.Exists(root))
            {
                var mulFiles = new List<string>();
                for (int i = 1; i <= 5; ++i)
                {
                    string fileName = (i == 1) ? "anim.mul" : $"anim{i}.mul";
                    if (!string.IsNullOrEmpty(Files.GetFilePath(fileName)))
                    {
                        mulFiles.Add(Path.GetFileNameWithoutExtension(fileName));
                    }
                }
                mulFiles.Sort();
                SecondAnimFileComboBox.Items.AddRange(mulFiles.ToArray());

                var uopFiles = Directory.GetFiles(root, "AnimationFrame*.uop")
                                       .Select(Path.GetFileName)
                                       .ToList();
                uopFiles.Sort();
                SecondAnimFileComboBox.Items.AddRange(uopFiles.ToArray());
            }
            if (SecondAnimFileComboBox.Items.Count > 0)
            {
                SecondAnimFileComboBox.SelectedIndex = 0;
            }

            // ── UOP 映射复选框
            /*var toolStrip = SelectFileToolStripComboBox.GetCurrentParent();
            if (toolStrip != null)
            {
                var mappingCheckBox = new ToolStripButton("映射 UOP")
                {
                    CheckOnClick = true,
                    Checked = _useUopMapping
                };
                mappingCheckBox.Click += (s, args) =>
                {
                    _useUopMapping = mappingCheckBox.Checked;
                    if (_uopManager != null)
                    {
                        _uopManager.IgnoreAnimationSequence = !_useUopMapping;
                        LoadUopAnimations();
                    }
                };
                toolStrip.Items.Insert(toolStrip.Items.IndexOf(SelectFileToolStripComboBox) + 1, mappingCheckBox);
            }*/

            SetupExportVdMenu();

            // ── TreeView 逻辑
            AnimationListTreeView.BeginUpdate();
            try
            {
                AnimationListTreeView.Nodes.Clear();

                if (_fileType != 0)
                {
                    int count = Animations.GetAnimCount(_fileType);
                    var nodes = new TreeNode[count];
                    int animationCount = 0;

                    for (int i = 0; i < count; ++i)
                    {
                        int animLength = Animations.GetAnimLength(i, _fileType);
                        string type = animLength switch
                        {
                            22 => "H",
                            13 => "L",
                            _ => "P"
                        };

                        var node = new TreeNode
                        {
                            Tag = i,
                            Text = $"{type}: {i} ({BodyConverter.GetTrueBody(_fileType, i)})"
                        };

                        bool valid = false;

                        for (int j = 0; j < animLength; ++j)
                        {
                            var treeNode = new TreeNode
                            {
                                Tag = j,
                                Text = string.Format("{0:D2} {1}", j,
                                    _animNames[animLength == 22 ? 1 : animLength == 13 ? 0 : 2][j])
                            };

                            if (AnimationEdit.IsActionDefined(_fileType, i, j))
                            {
                                valid = true;
                            }
                            else
                            {
                                treeNode.ForeColor = Color.Red;
                            }

                            node.Nodes.Add(treeNode);
                        }

                        if (valid)
                        {
                            animationCount++;
                        }
                        else
                        {
                            if (_showOnlyValid)
                            {
                                continue;
                            }

                            // 无效动画的颜色逻辑
                            node.ForeColor = checkBoxIDBlue.Checked ? Color.Blue : Color.Red;
                        }

                        nodes[i] = node;
                    }

                    AnimationListTreeView.Nodes.AddRange(nodes.Where(n => n != null).ToArray());

                    toolStripStatusDisplayLabelAnimation.Text = $"动画数量: {animationCount}"; // 用动画数量更新标签
                }
            }
            finally
            {
                AnimationListTreeView.EndUpdate();
            }

            if (AnimationListTreeView.Nodes.Count > 0)
            {
                AnimationListTreeView.SelectedNode = AnimationListTreeView.Nodes[0];
            }
        }
        #endregion

        private void SetupExportVdMenu()
        {
            // 搜索显示该名称是 tovdToolStripMenuItem
            // 此字段在 Designer.cs 文件中定义，但在此处可访问。
            if (this.tovdToolStripMenuItem != null)
            {
                tovdToolStripMenuItem.Click -= OnClickExportToVD;
                tovdToolStripMenuItem.DropDownItems.Clear();
                tovdToolStripMenuItem.Text = "导出为 vd..";

                var animalMul = new ToolStripMenuItem("动物 (mul)");
                animalMul.Tag = "animal_mul";
                animalMul.Click += OnClickExportVdRemap;

                var monsterMul = new ToolStripMenuItem("怪物 (mul)");
                monsterMul.Tag = "monster_mul";
                monsterMul.Click += OnClickExportVdRemap;

                var seaMonsterMul = new ToolStripMenuItem("海洋怪物 (mul)");
                seaMonsterMul.Tag = "sea_monster_mul";
                seaMonsterMul.Click += OnClickExportVdRemap;

                var equipmentMul = new ToolStripMenuItem("装备 (mul)");
                equipmentMul.Tag = "equipment_mul";
                equipmentMul.Click += OnClickExportVdRemap;

                var creaturesUop = new ToolStripMenuItem("生物 (UOP)");
                creaturesUop.Tag = "creatures_uop";
                creaturesUop.Click += OnClickExportVdRemap;

                var equipmentUop = new ToolStripMenuItem("装备 (UOP)");
                equipmentUop.Tag = "equipement_uop";
                equipmentUop.Click += OnClickExportVdRemap;

                tovdToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                    animalMul,
                    monsterMul,
                    seaMonsterMul,
                    equipmentMul,
                    new ToolStripSeparator(),
                    creaturesUop,
                    equipmentUop
                });

                // 添加“导出为 .bin”选项
                var toBinToolStripMenuItem = new ToolStripMenuItem("导出为 .bin");
                toBinToolStripMenuItem.Click += OnClickExportToBin;
                exportToolStripMenuItem1.DropDownItems.Add(toBinToolStripMenuItem);
            }
        }

        private void OnFilePathChangeEvent()
        {
            if (!_loaded)
            {
                return;
            }

            _fileType = 0;
            _currentDir = 0;
            _currentAction = 0;
            _currentBody = 0;
            SelectFileToolStripComboBox.SelectedIndex = 0;
            _framePoint = new Point(AnimationPictureBox.Width / 2, AnimationPictureBox.Height / 2);
            _showOnlyValid = false;
            ShowOnlyValidToolStripMenuItem.Checked = false;

            // 文件路径更改时清除 UOP 相关数据
            _uopManager = null;

            OnLoad(null);
        }

        private TreeNode GetNode(int tag)
        {
            foreach (TreeNode node in AnimationListTreeView.Nodes)
            {
                if (node.Tag is int val && val == tag) return node;
                if (node.Tag is ushort uval && uval == tag) return node;
            }
            return null;
        }

        private unsafe void SetPaletteBox()
        {
            if (_fileType == 0)
            {
                return;
            }

            const int bitmapWidth = 256;
            int bitmapHeight = PalettePictureBox.Height;

            if (_fileType == 6)
            {
                if (FramesListView.SelectedItems.Count == 0)
                {
                    PalettePictureBox.Image = null;
                    return;
                }
                int frameIndex = (int)FramesListView.SelectedItems[0].Tag;
                var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                if (uopAnim == null || frameIndex < 0 || frameIndex >= uopAnim.Frames.Count)
                {
                    PalettePictureBox.Image = null;
                    return;
                }
                var frame = uopAnim.Frames[frameIndex];
                if (frame == null)
                {
                    PalettePictureBox.Image = null;
                    return;
                }
                Bitmap bmp = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format16bppArgb1555);
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bitmapWidth, bitmapHeight), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;
                for (int y = 0; y < bd.Height; ++y, line += delta)
                {
                    ushort* cur = line;
                    for (int i = 0; i < bitmapWidth; ++i)
                    {
                        Color c = frame.Palette[i];
                        *cur++ = (ushort)((1 << 15) | ((c.R & 0xF8) << 7) | ((c.G & 0xF8) << 2) | (c.B >> 3));
                    }
                }
                bmp.UnlockBits(bd);
                PalettePictureBox.Image?.Dispose();
                PalettePictureBox.Image = bmp;
                return;
            }

            // TODO: 为什么 bitmapWidth 是常数而高度取自图片框？
            // TODO: 看起来该值与 AnimIdx 中调色板的数组大小相同
            AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            Bitmap bmp1 = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format16bppArgb1555);
            if (edit != null)
            {
                BitmapData bd = bmp1.LockBits(new Rectangle(0, 0, bitmapWidth, bitmapHeight), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;
                for (int y = 0; y < bd.Height; ++y, line += delta)
                {
                    ushort* cur = line;
                    for (int i = 0; i < bitmapWidth; ++i)
                    {
                        *cur++ = edit.Palette[i];
                    }
                }

                bmp1.UnlockBits(bd);
            }

            PalettePictureBox.Image?.Dispose();
            PalettePictureBox.Image = bmp1;
        }

        private void AfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            if (AnimationListTreeView.SelectedNode == null)
            {
                return;
            }

            // ✅ 处理 UOP 动画
            if (_fileType == 6)
            {
                // 在保存前确定新的 body/action
                int newBody = _currentBody;
                int newAction = _currentAction;

                if (AnimationListTreeView.SelectedNode.Parent == null)
                {
                    if (AnimationListTreeView.SelectedNode.Tag != null)
                        newBody = (int)(ushort)AnimationListTreeView.SelectedNode.Tag;

                    if (AnimationListTreeView.SelectedNode.Nodes.Count > 0)
                    {
                        newAction = (int)AnimationListTreeView.SelectedNode.Nodes[0].Tag;
                    }
                    else
                    {
                        newAction = 0;
                    }
                }
                else
                {
                    if (AnimationListTreeView.SelectedNode.Parent.Tag != null)
                        newBody = (int)(ushort)AnimationListTreeView.SelectedNode.Parent.Tag;
                    newAction = (int)AnimationListTreeView.SelectedNode.Tag;
                }

                // ✅ 在更改之前保存旧 body/action 的修改
                bool hasChanged = (_previousBody != -1 && (_previousBody != newBody || _previousAction != newAction));

                if (hasChanged && FramesListView.Items.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"💾 保存之前的动画: Body={_previousBody}, Action={_previousAction}, Direction={_currentDir}");
                    UpdateUopData(applyToAllFrames: true);
                }

                // 更新当前值
                _currentBody = newBody;
                _currentAction = newAction;

                PopulateSequenceGrid(_currentBody);

                // 保存以备下次使用
                _previousBody = _currentBody;
                _previousAction = _currentAction;

                DisplayUopAnimation();
                NotifyHexEditor();
                return; // ← 对 UOP 动画在此退出
            }

            // ✅ 处理 MUL 动画（其余现有代码）
            if (AnimationListTreeView.SelectedNode.Parent == null)
            {
                if (AnimationListTreeView.SelectedNode.Tag != null)
                {
                    _currentBody = (int)AnimationListTreeView.SelectedNode.Tag;
                }
                _currentAction = 0;
            }
            else
            {
                if (AnimationListTreeView.SelectedNode.Parent.Tag != null)
                {
                    _currentBody = (int)AnimationListTreeView.SelectedNode.Parent.Tag;
                }
                _currentAction = (int)AnimationListTreeView.SelectedNode.Tag;
            }

            FramesListView.BeginUpdate();
            try
            {
                FramesListView.Clear();
                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                if (edit != null)
                {
                    int width = 80;
                    int height = 110;
                    Bitmap[] currentBits = edit.GetFrames();
                    if (currentBits != null)
                    {
                        for (int i = 0; i < currentBits.Length; ++i)
                        {
                            if (currentBits[i] == null)
                            {
                                continue;
                            }

                            ListViewItem item = new ListViewItem(i.ToString(), 0)
                            {
                                Tag = i
                            };
                            FramesListView.Items.Add(item);

                            if (currentBits[i].Width > width)
                            {
                                width = currentBits[i].Width;
                            }

                            if (currentBits[i].Height > height)
                            {
                                height = currentBits[i].Height;
                            }
                        }
                        FramesListView.TileSize = new Size(width + 5, height + 5);

                        FramesTrackBar.Maximum = currentBits.Length - 1;
                        FramesTrackBar.Value = 0;
                        FramesTrackBar.Invalidate();

                        _updatingUi = true;
                        if (!_relativeMode)
                        {
                            CenterXNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.X;
                            CenterYNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.Y;
                        }
                        else
                        {
                            CenterXNumericUpDown.Value = 0;
                            CenterYNumericUpDown.Value = 0;
                            _lastRelativeX = 0;
                            _lastRelativeY = 0;
                            _accumulatedRelativeX = 0;
                            _accumulatedRelativeY = 0;
                        }
                        _updatingUi = false;
                    }
                    else
                    {
                        FramesTrackBar.Maximum = 0;
                        FramesTrackBar.Value = 0;
                        FramesTrackBar.Invalidate();
                    }
                }
            }
            finally
            {
                FramesListView.EndUpdate();
            }

            AnimationPictureBox.Invalidate();
            SetPaletteBox();

            // 为 MUL 填充序列选项卡
            PopulateSequenceGrid(_currentBody);
            NotifyHexEditor();
        }

        private void DisplayUopAnimation()
        {
            if (_uopManager == null) return;

            // ✅ 关键更改：使用 GetUopAnimation 而不是从文件加载！
            var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
            if (uopAnim == null)
            {
                FramesListView.Clear();
                return;
            }

            FramesListView.BeginUpdate();
            try
            {
                FramesListView.Clear();

                // ✅ 从缓存显示帧
                int width = 80;
                int height = 110;
                for (int i = 0; i < uopAnim.Frames.Count; i++)
                {
                    var frame = uopAnim.Frames[i];

                    ListViewItem item = new ListViewItem(i.ToString(), 0) { Tag = i };
                    FramesListView.Items.Add(item);

                    if (frame.Image.Width > width) width = frame.Image.Width;
                    if (frame.Image.Height > height) height = frame.Image.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = uopAnim.Frames.Count > 0 ? uopAnim.Frames.Count - 1 : 0;
                FramesTrackBar.Value = 0;

                if (uopAnim.Frames.Count > 0)
                {
                    _updatingUi = true;
                    if (!_relativeMode)
                    {
                        CenterXNumericUpDown.Value = uopAnim.Frames[0].Header.CenterX;
                        CenterYNumericUpDown.Value = uopAnim.Frames[0].Header.CenterY;
                    }
                    else
                    {
                        CenterXNumericUpDown.Value = 0;
                        CenterYNumericUpDown.Value = 0;
                        _lastRelativeX = 0;
                        _lastRelativeY = 0;
                        _accumulatedRelativeX = 0;
                        _accumulatedRelativeY = 0;
                    }
                    _updatingUi = false;
                    FramesListView.Items[0].Selected = true;
                }
            }
            finally
            {
                FramesListView.EndUpdate();
            }

            AnimationPictureBox.Invalidate();
            SetPaletteBox();
        }

        private void ApplyCenterChange(int? newX, int? newY, int? deltaX, int? deltaY)
        {
            if (_fileType == 0) return;

            int actionStart = _currentAction;
            int actionEnd = _currentAction;
            if (CheckBoxAllAction.Checked)
            {
                actionStart = 0;
                if (_fileType == 6)
                {
                    actionEnd = 100; // UOP 动作的常见上限
                }
                else
                {
                    actionEnd = Animations.GetAnimLength(_currentBody, _fileType) - 1;
                }
            }

            for (int action = actionStart; action <= actionEnd; action++)
            {
                int dirStart = _currentDir;
                int dirEnd = _currentDir;
                if (CheckBoxAction.Checked || CheckBoxAllAction.Checked)
                {
                    dirStart = 0;
                    dirEnd = 4;
                }

                for (int dir = dirStart; dir <= dirEnd; dir++)
                {
                    if (_fileType == 6 && _uopManager != null)
                    {
                        var uopAnim = _uopManager.GetUopAnimation(_currentBody, action, dir);
                        if (uopAnim != null)
                        {
                            for (int i = 0; i < uopAnim.Frames.Count; i++)
                            {
                                if (!CheckBoxAction.Checked && !CheckBoxAllAction.Checked && i != FramesTrackBar.Value) continue;

                                var frame = uopAnim.Frames[i];
                                int finalX = frame.Header.CenterX;
                                int finalY = frame.Header.CenterY;

                                if (newX.HasValue) finalX = newX.Value;
                                if (newY.HasValue) finalY = newY.Value;
                                if (deltaX.HasValue) finalX += deltaX.Value;
                                if (deltaY.HasValue) finalY += deltaY.Value;

                                uopAnim.ChangeCenter(i, finalX, finalY);
                            }
                        }
                    }
                    else if (_fileType != 6)
                    {
                        AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, action, dir);
                        if (edit != null && edit.Frames != null)
                        {
                            for (int i = 0; i < edit.Frames.Count; i++)
                            {
                                if (!CheckBoxAction.Checked && !CheckBoxAllAction.Checked && i != FramesTrackBar.Value) continue;

                                var frame = edit.Frames[i];
                                int finalX = frame.Center.X;
                                int finalY = frame.Center.Y;

                                if (newX.HasValue) finalX = newX.Value;
                                if (newY.HasValue) finalY = newY.Value;
                                if (deltaX.HasValue) finalX += deltaX.Value;
                                if (deltaY.HasValue) finalY += deltaY.Value;

                                frame.ChangeCenter(finalX, finalY);
                            }
                        }
                    }
                }
            }
            Options.ChangedUltimaClass["Animations"] = true;
            AnimationPictureBox.Invalidate();
        }

        private void OnCenterXValueChanged(object sender, EventArgs e)
        {
            if (_updatingUi)
            {
                _lastRelativeX = (int)CenterXNumericUpDown.Value;
                return;
            }

            if (_relativeMode)
            {
                int currentValue = (int)CenterXNumericUpDown.Value;
                int delta = currentValue - _lastRelativeX;
                _lastRelativeX = currentValue;

                if (delta == 0) return;

                _accumulatedRelativeX += delta;
                ApplyCenterChange(null, null, delta, null);
            }
            else
            {
                ApplyCenterChange((int)CenterXNumericUpDown.Value, null, null, null);
            }
        }



        private void DrawFrameItem(object sender, DrawListViewItemEventArgs e)
        {
            if (_fileType == 6)
            {
                int index = (int)e.Item.Tag;
                var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                if (uopAnim != null && index >= 0 && index < uopAnim.Frames.Count)
                {
                    var frame = uopAnim.Frames[index];
                    var penColor = FramesListView.SelectedItems.Contains(e.Item) ? Color.Red : Color.Gray;
                    e.Graphics.DrawRectangle(new Pen(penColor), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    e.Graphics.DrawImage(frame.Image, e.Bounds.X, e.Bounds.Y, frame.Image.Width, frame.Image.Height);
                }
                e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
                return;
            }
            AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            Bitmap[] currentBits = edit.GetFrames();
            Bitmap bmp = currentBits[(int)e.Item.Tag];
            var penColor1 = FramesListView.SelectedItems.Contains(e.Item) ? Color.Red : Color.Gray;
            e.Graphics.DrawRectangle(new Pen(penColor1), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y, bmp.Width, bmp.Height);
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
        }

        private void FramesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected)
                return;

            int frameIndex = (int)e.Item.Tag;

            if (_fileType == 6)
            {
                // UOP 动画
                var uopAnim = _uopManager?.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                if (uopAnim != null && frameIndex >= 0 && frameIndex < uopAnim.Frames.Count)
                {
                    var frame = uopAnim.Frames[frameIndex];
                    toolStripStatusLabelFrameSize.Text = $"帧 {frameIndex}: {frame.Image.Width} x {frame.Image.Height} 像素";
                }
                else
                {
                    toolStripStatusLabelFrameSize.Text = string.Empty;
                }
            }
            else if (_fileType != 0)
            {
                // MUL 动画
                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                if (edit != null)
                {
                    Bitmap[] currentBits = edit.GetFrames();
                    if (currentBits != null && frameIndex >= 0 && frameIndex < currentBits.Length && currentBits[frameIndex] != null)
                    {
                        Bitmap bmp = currentBits[frameIndex];
                        toolStripStatusLabelFrameSize.Text = $"帧 {frameIndex}: {bmp.Width} x {bmp.Height} 像素";
                    }
                    else
                    {
                        toolStripStatusLabelFrameSize.Text = string.Empty;
                    }
                }
            }
            else
            {
                toolStripStatusLabelFrameSize.Text = string.Empty;
            }
        }

        private void OnAnimChanged(object sender, EventArgs e)
        {
            if (SelectFileToolStripComboBox.SelectedIndex < 1)
            {
                _fileType = 0;
                _uopManager = null;
                AnimationListTreeView.Nodes.Clear();
                FramesListView.Clear();
                PalettePictureBox.Image = null;
                return;
            }

            string selection = SelectFileToolStripComboBox.SelectedItem.ToString();
            if (selection.EndsWith(".uop"))
            {
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                groupBox3.Visible = false;

                _fileType = UOP_FILE_TYPE;
                Cursor = Cursors.WaitCursor;
                try
                {
                    _uopManager = new UopAnimationDataManager();
                    if (_uopManager.LoadUopFiles())
                    {
                        _uopManager.ProcessUopData();
                        _uopManager.LoadMainMisc(); // 加载 MainMisc 但不要自动扫描所有
                        RefreshMainMiscButtonState();
                        LoadUopAnimations();
                    }
                    else
                    {
                        MessageBox.Show("未找到或无法加载 AnimationFrame*.uop 文件。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _uopManager = null;
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
            else // 这是 .mul 文件
            {
                groupBox1.Visible = true;
                groupBox2.Visible = true;
                groupBox3.Visible = true;

                _uopManager = null;
                if (selection == "anim")
                {
                    _fileType = 1;
                }
                else
                {
                    if (selection.StartsWith("anim") && !int.TryParse(selection.Substring(4), out _fileType))
                    {
                        _fileType = 0; // 后备
                    }
                }

                if (_fileType > 0)
                {
                    LoadMulAnimations();
                }
            }
        }

        private string GetUopMulMapping(int animId)
        {
            int body = animId;
            bool bodyDefUsed = false;

            if (BodyTable.Entries != null && BodyTable.Entries.TryGetValue(body, out BodyTableEntry entry))
            {
                if (!BodyConverter.Contains(body))
                {
                    body = entry.OldId;
                    bodyDefUsed = true;
                }
            }

            int originalMappedBody = body;
            int fileType = BodyConverter.Convert(ref body);

            string result = "";
            if (bodyDefUsed)
            {
                result += $" (身体 {originalMappedBody})";
            }

            if (fileType > 1)
            {
                result += $" (anim{fileType} {body})";
            }

            return result;
        }

        private void LoadUopAnimations()
        {
            if (_uopManager == null) return;

            // ❌ 已移除：不再自动清除缓存！
            // _uopManager.ClearCache();

            // ✅ 重新加载时重置之前的状态
            _previousBody = -1;
            _previousAction = -1;

            // 捕获当前 TreeView 状态
            List<string> expandedPaths = GetExpandedNodePaths(AnimationListTreeView);
            string? selectedPath = GetSelectedNodePath(AnimationListTreeView);

            AnimationListTreeView.BeginUpdate();
            try
            {
                AnimationListTreeView.Nodes.Clear();
                FramesListView.Clear();
                AnimationPictureBox.Invalidate();

                // ✅ 新增：首先扫描缓存中已导入的动画
                HashSet<int> animationsInCache = new HashSet<int>();
                foreach (var cacheKey in _uopManager._animCache.Keys)
                {
                    var parts = cacheKey.Split('_');
                    if (parts.Length >= 3 && int.TryParse(parts[0], out int animId))
                    {
                        animationsInCache.Add(animId);
                    }
                }

                for (int animId = 0; animId < UopConstants.MAX_ANIMATIONS_DATA_INDEX_COUNT; ++animId)
                {
                    List<int> availableActions = _uopManager.GetAvailableActions(animId);

                    // 1. 检查 UOP 中是否确实有实际动作（有帧的动作）
                    bool hasRealActions = false;

                    // ✅ 更改：首先检查缓存！
                    if (animationsInCache.Contains(animId))
                    {
                        hasRealActions = true;
                    }
                    else if (availableActions.Count > 0)
                    {
                        foreach (int action in availableActions)
                        {
                            if (_uopManager.IsActionReal(animId, action))
                            {
                                var fileInfo = _uopManager.GetAnimationData(animId, action, 0);
                                if (fileInfo != null)
                                {
                                    byte[] data = fileInfo.GetData();
                                    if (data != null && data.Length > 0)
                                    {
                                        using (var ms = new MemoryStream(data))
                                        using (var r = new BinaryReader(ms))
                                        {
                                            var header = Uop.UopAnimationDataManager.ReadUopBinHeader(r);
                                            if (header != null && header.FrameCount > 0)
                                            {
                                                hasRealActions = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // 2. 检查 MUL 是否存在（与之前相同）
                    int mulBody = animId;
                    if (BodyTable.Entries != null && BodyTable.Entries.TryGetValue(mulBody, out BodyTableEntry entry))
                    {
                        if (!BodyConverter.Contains(mulBody))
                        {
                            mulBody = entry.OldId;
                        }
                    }

                    int fileType = BodyConverter.Convert(ref mulBody);
                    int length = Animations.GetAnimLength(mulBody, fileType);
                    bool existsInMul = false;
                    for (int i = 0; i < length; ++i)
                    {
                        if (AnimationEdit.IsActionDefined(fileType, mulBody, i))
                        {
                            existsInMul = true;
                            break;
                        }
                    }

                    bool isValid = hasRealActions || existsInMul;
                    bool shouldShow = _showOnlyValid ? isValid : (animId <= 4096 || isValid);

                    if (shouldShow)
                    {
                        string mappingInfo = GetUopMulMapping(animId);
                        string nodeText = $"UOP ID: {animId}{mappingInfo}";

                        TreeNode node = new TreeNode
                        {
                            Tag = (ushort)animId,
                            Text = nodeText
                        };

                        if (!hasRealActions && existsInMul)
                        {
                            node.ForeColor = Color.Orange;
                        }
                        else if (!isValid)
                        {
                            node.ForeColor = Color.Red;
                        }

                        foreach (int action in availableActions)
                        {
                            bool isReal = _uopManager.IsActionReal(animId, action);
                            Color nodeColor = Color.Black;

                            if (!isReal) // 这是一个映射动作
                            {
                                nodeColor = _useUopMapping ? Color.Blue : Color.Red;
                            }

                            string uopFileNumber = "N/A";
                            Uop.IndexDataFileInfo fileInfo = _uopManager.GetAnimationData(animId, action, 0);
                            if (fileInfo != null && fileInfo.File != null && !string.IsNullOrEmpty(fileInfo.File.FilePath))
                            {
                                string fileName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.File.FilePath);
                                var match = System.Text.RegularExpressions.Regex.Match(fileName, @"AnimationFrame(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                if (match.Success)
                                {
                                    uopFileNumber = match.Groups[1].Value;
                                }
                            }

                            TreeNode actionNode = new TreeNode
                            {
                                Tag = action,
                                Text = $"{action:D2}_{GetActionDescription(animId, action)} ({uopFileNumber})",
                                ForeColor = nodeColor
                            };
                            node.Nodes.Add(actionNode);
                        }
                        AnimationListTreeView.Nodes.Add(node);
                    }
                }
            }
            finally
            {
                AnimationListTreeView.EndUpdate();
            }

            // 恢复 TreeView 状态
            ExpandNodesByPath(AnimationListTreeView, expandedPaths);
            SelectNodeByPath(AnimationListTreeView, selectedPath);

            if (AnimationListTreeView.Nodes.Count > 0 && AnimationListTreeView.SelectedNode == null)
            {
                AnimationListTreeView.SelectedNode = AnimationListTreeView.Nodes[0];
            }
        }

        #region [ OnDirectionChanged ] // 方向更改事件处理程序，对于 UOP 动画在切换前保存更改并正确更新显示至关重要
        private void OnDirectionChanged(object sender, EventArgs e)
        {
            // UOP: 保存更改
            if (_fileType == 6 && FramesListView.Items.Count > 0)
            {
                UpdateUopData(applyToAllFrames: true);
            }

            // 首先设置 _currentDir
            _currentDir = DirectionTrackBar.Value;

            if (checkBoxMount.Checked)
            {
                _currentAction = 24;
            }

            // 在 AfterSelectTreeView 之前处理第二个动画——就像旧版本一样
            if (isAnimationVisible)
            {
                string selectedGender = comboBoxMenWoman.SelectedItem?.ToString() ?? "men";
                int animId = selectedGender == "men" ? 400 : 401;

                additionalAnimation = AnimationEdit.GetAnimation(
                    _fileType, animId, _currentAction, _currentDir);

                AdjustAdditionalAnimationPosition();
                AnimationPictureBox.Invalidate();
            }

            // 最后调用 AfterSelectTreeView — 加载主动画，但不覆盖 isAnimationVisible
            AfterSelectTreeView(null, null);
            NotifyHexEditor();
        }
        #endregion

        #region [  AnimationPictureBox_OnSizeChanged ] // 大小更改时重新计算帧点以使其居中
        private void AnimationPictureBox_OnSizeChanged(object sender, EventArgs e)
        {
            _framePoint = new Point(AnimationPictureBox.Width / 2, AnimationPictureBox.Height / 2);
            AnimationPictureBox.Invalidate();
        }
        #endregion

        #region [ AdjustAdditionalAnimationPosition ]  
        private void AdjustAdditionalAnimationPosition()
        {
            if (additionalAnimation != null)
            {
                Bitmap[] additionalBits = additionalAnimation.GetFrames();
                if (additionalBits?.Length > 0 && FramesTrackBar.Value >= 0 && FramesTrackBar.Value < additionalBits.Length && additionalBits[FramesTrackBar.Value] != null)
                {
                    int[] offsets;
                    if (checkBoxMount.Checked)
                    {
                        if (_currentDir >= 0 && _currentDir < HorseRunOffsets.Length && FramesTrackBar.Value < HorseRunOffsets[_currentDir].Length)
                        {
                            offsets = HorseRunOffsets[_currentDir][FramesTrackBar.Value];
                        }
                        else
                        {
                            // 记录或处理错误
                            return;
                        }
                    }
                    else
                    {
                        if (_currentDir >= 0 && _currentDir < Offsets.Length && FramesTrackBar.Value < Offsets[_currentDir].Length)
                        {
                            offsets = Offsets[_currentDir][FramesTrackBar.Value];
                        }
                        else
                        {
                            // 记录或处理错误
                            return;
                        }
                    }

                    if (offsets.Length >= 2) // 确保至少有两个元素
                    {
                        int xOffset = offsets[0];
                        int yOffset = offsets[1];

                        int additionalX = _framePoint.X + xOffset;
                        int additionalY = _framePoint.Y + yOffset;

                        _additionalFramePoint = new Point(additionalX, additionalY);
                    }
                    else
                    {
                        // 记录或处理无效偏移量
                    }
                }
            }
        }
        #endregion


        private void AnimationPictureBox_OnPaintFrame(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            e.Graphics.Clear(Color.LightGray);

            // 平铺背景图像
            if (_backgroundImage != null)
            {
                using (var tb = new System.Drawing.TextureBrush(_backgroundImage,
                                    System.Drawing.Drawing2D.WrapMode.Tile))
                {
                    e.Graphics.FillRectangle(tb, e.ClipRectangle);
                }
            }

            if (_drawCrosshair)
            {
                e.Graphics.DrawLine(Pens.Black, new Point(_framePoint.X, 0), new Point(_framePoint.X, AnimationPictureBox.Height));
                e.Graphics.DrawLine(Pens.Black, new Point(0, _framePoint.Y), new Point(AnimationPictureBox.Width, _framePoint.Y));
            }

            // 男/女动画对齐
            if (isAnimationVisible && additionalAnimation != null)
            {
                AdjustAdditionalAnimationPosition(); // 确保调用此方法

                Bitmap[] additionalBits = additionalAnimation.GetFrames();
                if (additionalBits?.Length > 0 && FramesTrackBar.Value >= 0 && FramesTrackBar.Value < additionalBits.Length && additionalBits[FramesTrackBar.Value] != null)
                {
                    // 在指定位置绘制附加动画
                    e.Graphics.DrawImage(additionalBits[FramesTrackBar.Value], _additionalFramePoint.X, _additionalFramePoint.Y);
                }
            }
            // 男/女动画对齐

            if (_secondAnimActivated && !_isSecondAnimInFront) DrawSecondAnimation(e.Graphics);

            if (_fileType == 6)
            {
                if (FramesListView.Items.Count == 0 || FramesTrackBar.Value >= FramesListView.Items.Count)
                {
                    if (_secondAnimActivated && _isSecondAnimInFront) DrawSecondAnimation(e.Graphics);
                    return;
                }

                int frameIndex = (int)FramesListView.Items[FramesTrackBar.Value].Tag;
                var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                if (uopAnim == null || frameIndex < 0 || frameIndex >= uopAnim.Frames.Count)
                {
                    if (_secondAnimActivated && _isSecondAnimInFront) DrawSecondAnimation(e.Graphics);
                    return;
                }
                var frame = uopAnim.Frames[frameIndex];
                if (frame == null)
                {
                    if (_secondAnimActivated && _isSecondAnimInFront) DrawSecondAnimation(e.Graphics);
                    return;
                }

                if (_drawEmpty)
                {
                    rawDimensionsToolStripLabel.Text = $" {frame.Header.Width}x{frame.Header.Height}";
                }
                else
                {
                    rawDimensionsToolStripLabel.Text = "";
                }

                int x = _framePoint.X - (int)(frame.Header.CenterX * _zoomFactor);
                int y = _framePoint.Y - (int)(frame.Header.CenterY * _zoomFactor) - (int)(frame.Image.Height * _zoomFactor);
                int w = (int)(frame.Image.Width * _zoomFactor);
                int h = (int)(frame.Image.Height * _zoomFactor);

                if (_drawFull)
                {
                    using (var whiteTransparent = new SolidBrush(Color.FromArgb(160, 255, 255, 255)))
                    {
                        e.Graphics.FillRectangle(whiteTransparent, new Rectangle(x, y, w, h));
                    }
                }

                e.Graphics.DrawImage(frame.Image, new Rectangle(x, y, w, h));

                if (_drawEmpty)
                {
                    e.Graphics.DrawRectangle(Pens.Red, new Rectangle(x, y, w, h));
                }

                // 边界框：在所有方向的所有帧上计算
                if (_drawBoundingBox)
                {
                    float z = _zoomFactor;

                    // 初始值超出范围
                    int minLeft = int.MaxValue;
                    int maxRight = int.MinValue;
                    int minTop = int.MaxValue;
                    int maxBottom = int.MinValue;
                    bool foundAny = false;

                    // 遍历 5 个方向及其所有可用帧
                    for (int dir = 0; dir < 5; dir++)
                    {
                        var animDir = _uopManager.GetUopAnimation(_currentBody, _currentAction, dir);
                        if (animDir == null || animDir.Frames == null) continue;

                        foreach (var f in animDir.Frames)
                        {
                            if (f == null || f.Image == null || f.Header == null) continue;

                            int cX = f.Header.CenterX;
                            int cY = f.Header.CenterY;
                            int iw = f.Image.Width;
                            int ih = f.Image.Height;

                            // 相对于枢轴的坐标：如果图像向左/向上超出，左/上为负
                            int left = -cX;
                            int right = iw - cX;
                            int top = -cY - ih;
                            int bottom = -cY;

                            if (left < minLeft) minLeft = left;
                            if (right > maxRight) maxRight = right;
                            if (top < minTop) minTop = top;
                            if (bottom > maxBottom) maxBottom = bottom;

                            foundAny = true;
                        }
                    }

                    if (foundAny)
                    {
                        int bbX = _framePoint.X + (int)(minLeft * z);
                        int bbY = _framePoint.Y + (int)(minTop * z);
                        int bbW = (int)((maxRight - minLeft) * z);
                        int bbH = (int)((maxBottom - minTop) * z);

                        using (Pen bluePen = new Pen(Color.Blue, 2))
                        {
                            e.Graphics.DrawRectangle(bluePen, bbX, bbY, bbW, bbH);
                        }
                    }
                    else if (uopAnim.Header != null)
                    {
                        // 如果找不到帧，则使用现有标头作为后备
                        int bbWidth = (int)((uopAnim.Header.BoundRight - uopAnim.Header.BoundLeft) * z);
                        int bbHeight = (int)((uopAnim.Header.BoundBottom - uopAnim.Header.BoundTop) * z);
                        int bbX = x + (w / 2) - (bbWidth / 2);
                        int bbY = y + (h / 2) - (bbHeight / 2);

                        using (Pen bluePen = new Pen(Color.Blue, 2))
                        {
                            e.Graphics.DrawRectangle(bluePen, bbX, bbY, bbWidth, bbHeight);
                        }
                    }
                }

                // --- 用此块替换之前裁剪框 X/Y 的计算 ---
                // --- 裁剪框：修正计算以保持与图像绘制一致 ---
                if (_drawCroppingBox && frame.IndexInfo != null)
                {
                    int origLeft = (int)frame.IndexInfo.Left;
                    int origRight = (int)frame.IndexInfo.Right;
                    int origTop = (int)frame.IndexInfo.Top;
                    int origBottom = (int)frame.IndexInfo.Bottom;

                    int cw = (int)((origRight - origLeft) * _zoomFactor);
                    int ch = (int)((origBottom - origTop) * _zoomFactor);

                    // 使用 IndexInfo 中存储的直接相对枢轴坐标
                    int cx = _framePoint.X + (int)(origLeft * _zoomFactor);
                    int cy = _framePoint.Y + (int)(origTop * _zoomFactor);

                    using (Pen greenPen = new Pen(Color.Lime, 2))
                    {
                        e.Graphics.DrawRectangle(greenPen, cx, cy, cw, ch);
                    }
                }
            }
            else
            {
                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                if (edit != null)
                {
                    Bitmap[] currentBits = edit.GetFrames();

                    if (_drawEmpty && currentBits?.Length > 0 && currentBits[FramesTrackBar.Value] != null)
                    {
                        rawDimensionsToolStripLabel.Text = $" {currentBits[FramesTrackBar.Value].Width}x{currentBits[FramesTrackBar.Value].Height}";
                    }
                    else
                    {
                        rawDimensionsToolStripLabel.Text = "";
                    }

                    if (currentBits?.Length > 0 && currentBits[FramesTrackBar.Value] != null)
                    {
                        int varW = _drawEmpty ? currentBits[FramesTrackBar.Value].Width : 0;
                        int varH = _drawEmpty ? currentBits[FramesTrackBar.Value].Height : 0;
                        int varFw = _drawFull ? currentBits[FramesTrackBar.Value].Width : 0;
                        int varFh = _drawFull ? currentBits[FramesTrackBar.Value].Height : 0;

                        int x = _framePoint.X - (int)(edit.Frames[FramesTrackBar.Value].Center.X * _zoomFactor);
                        int y = _framePoint.Y - (int)(edit.Frames[FramesTrackBar.Value].Center.Y * _zoomFactor) - (int)(currentBits[FramesTrackBar.Value].Height * _zoomFactor);

                        int scaledW = (int)(currentBits[FramesTrackBar.Value].Width * _zoomFactor);
                        int scaledH = (int)(currentBits[FramesTrackBar.Value].Height * _zoomFactor);
                        int scaledFw = (int)(varFw * _zoomFactor);
                        int scaledFh = (int)(varFh * _zoomFactor);

                        using (var whiteTransparent = new SolidBrush(Color.FromArgb(160, 255, 255, 255)))
                        {
                            if (_drawFull) e.Graphics.FillRectangle(whiteTransparent, new Rectangle(x, y, scaledFw, scaledFh));
                        }

                        if (_drawEmpty) e.Graphics.DrawRectangle(Pens.Red, new Rectangle(x, y, scaledW, scaledH));
                        e.Graphics.DrawImage(currentBits[FramesTrackBar.Value], new Rectangle(x, y, scaledW, scaledH));
                    }
                }
            }

            if (_secondAnimActivated && _isSecondAnimInFront) DrawSecondAnimation(e.Graphics);

            // 绘制参考点箭头
            int refX = (int)(RefXNumericUpDown.Value * (decimal)_zoomFactor);
            int refY = (int)(RefYNumericUpDown.Value * (decimal)_zoomFactor);
            Point[] arrayPoints = {
                new Point(_framePoint.X - refX, _framePoint.Y - refY),
                new Point(_framePoint.X - refX, _framePoint.Y + (int)(17 * _zoomFactor) - refY),
                new Point(_framePoint.X + (int)(4 * _zoomFactor) - refX, _framePoint.Y + (int)(13 * _zoomFactor) - refY),
                new Point(_framePoint.X + (int)(7 * _zoomFactor) - refX, _framePoint.Y + (int)(18 * _zoomFactor) - refY),
                new Point(_framePoint.X + (int)(9 * _zoomFactor) - refX, _framePoint.Y + (int)(17 * _zoomFactor) - refY),
                new Point(_framePoint.X + (int)(7 * _zoomFactor) - refX, _framePoint.Y + (int)(12 * _zoomFactor) - refY),
                new Point(_framePoint.X + (int)(12 * _zoomFactor) - refX, _framePoint.Y + (int)(12 * _zoomFactor) - refY)
            };

            e.Graphics.FillPolygon(_whiteUnDraw, arrayPoints);
            e.Graphics.DrawPolygon(_blackUndraw, arrayPoints);

            // 如果复选框被选中，则在顶部绘制附加动画
            if (checkBoxMount.Checked && isAnimationVisible && additionalAnimation != null)
            {
                AdjustAdditionalAnimationPosition();

                Bitmap[] additionalBits = additionalAnimation.GetFrames();
                if (additionalBits?.Length > 0 && FramesTrackBar.Value >= 0 && FramesTrackBar.Value < additionalBits.Length && additionalBits[FramesTrackBar.Value] != null)
                {
                    e.Graphics.DrawImage(additionalBits[FramesTrackBar.Value], _additionalFramePoint.X, _additionalFramePoint.Y);
                }
            }
        }
        //Soulblighter 修改结束

        //Soulblighter 修改
        private void OnFrameCountBarChanged(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                return;
            }
            if (_fileType == 6)
            {
                if (FramesTrackBar.Value < FramesListView.Items.Count)
                {
                    FramesListView.SelectedItems.Clear();
                    var item = FramesListView.Items[FramesTrackBar.Value];
                    item.Selected = true;
                    item.EnsureVisible();

                    int frameIndex = (int)item.Tag;
                    var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                    if (uopAnim != null && frameIndex >= 0 && frameIndex < uopAnim.Frames.Count)
                    {
                        var frame = uopAnim.Frames[frameIndex];
                        _updatingUi = true;
                        if (!_relativeMode)
                        {
                            CenterXNumericUpDown.Value = frame.Header.CenterX;
                            CenterYNumericUpDown.Value = frame.Header.CenterY;
                        }
                        else
                        {
                            CenterXNumericUpDown.Value = 0;
                            CenterYNumericUpDown.Value = 0;
                            _lastRelativeX = 0;
                            _lastRelativeY = 0;
                            _accumulatedRelativeX = 0;
                            _accumulatedRelativeY = 0;
                        }
                        _updatingUi = false;
                    }
                }
                AnimationPictureBox.Invalidate();
                return;
            }

            AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            if (edit != null && edit.Frames.Count > FramesTrackBar.Value)
            {
                _updatingUi = true;
                if (!_relativeMode)
                {
                    CenterXNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.X;
                    CenterYNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.Y;
                }
                else
                {
                    CenterXNumericUpDown.Value = 0;
                    CenterYNumericUpDown.Value = 0;
                    _lastRelativeX = 0;
                    _lastRelativeY = 0;
                    _accumulatedRelativeX = 0;
                    _accumulatedRelativeY = 0;
                }
                _updatingUi = false;
            }

            AnimationPictureBox.Invalidate();
            NotifyHexEditor();
        }
        //Soulblighter 修改结束


        private void OnCenterYValueChanged(object sender, EventArgs e)
        {
            if (_updatingUi)
            {
                _lastRelativeY = (int)CenterYNumericUpDown.Value;
                return;
            }

            try
            {
                if (_fileType == 0) return;

                if (_relativeMode)
                {
                    int currentValue = (int)CenterYNumericUpDown.Value;
                    int delta = currentValue - _lastRelativeY;
                    _lastRelativeY = currentValue;

                    if (delta == 0) return;

                    _accumulatedRelativeY += delta;
                    ApplyCenterChange(null, null, null, delta);
                }
                else
                {
                    ApplyCenterChange(null, (int)CenterYNumericUpDown.Value, null, null);
                }
            }
            catch (Exception)
            {
                // 忽略
            }
        }

        private void OnClickExtractImages(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                return;
            }

            ToolStripMenuItem menu = (ToolStripMenuItem)sender;

            ImageFormat format;

            switch ((string)menu.Tag)
            {
                case ".tiff":
                    format = ImageFormat.Tiff;
                    break;
                case ".png":
                    format = ImageFormat.Png;
                    break;
                case ".jpg":
                    format = ImageFormat.Jpeg;
                    break;
                default:
                    format = ImageFormat.Bmp;
                    break;
            }

            string path = Options.OutputPath;

            int body;
            int action;

            if (AnimationListTreeView.SelectedNode.Parent == null)
            {
                if (AnimationListTreeView.SelectedNode.Tag is ushort ushortBody)
                    body = (int)ushortBody;
                else
                    body = (int)AnimationListTreeView.SelectedNode.Tag;
                action = -1;
            }
            else
            {
                // 正确地从 UInt16 (ushort) 转换为 int
                if (AnimationListTreeView.SelectedNode.Parent.Tag is ushort ushortBody)
                {
                    body = (int)ushortBody;
                }
                else
                {
                    body = (int)AnimationListTreeView.SelectedNode.Parent.Tag;
                }
                action = (int)AnimationListTreeView.SelectedNode.Tag;
            }

            if (_fileType == 6)
            {
                if (_uopManager == null) return;

                List<int> actionsToExport = new List<int>();
                if (action == -1)
                {
                    actionsToExport = _uopManager.GetAvailableActions(body);
                }
                else
                {
                    actionsToExport.Add(action);
                }

                foreach (int a in actionsToExport)
                {
                    for (int dir = 0; dir < 5; dir++)
                    {
                        var uopAnim = _uopManager.GetUopAnimation(body, a, dir);
                        if (uopAnim == null || uopAnim.Frames.Count == 0) continue;

                        for (int f = 0; f < uopAnim.Frames.Count; f++)
                        {
                            var frame = uopAnim.Frames[f];
                            if (frame.Image == null) continue;

                            string filename = $"animUOP_{body}_{a}_{dir}_{f}{menu.Tag}";
                            string file = Path.Combine(path, filename);

                            try
                            {
                                using (Bitmap bit = new Bitmap(frame.Image))
                                {
                                    bit.Save(file, format);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"保存图像 {filename} 失败：{ex.Message}");
                            }
                        }
                    }
                }

                MessageBox.Show($"图像已保存到 {path}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (action == -1)
            {
                for (int a = 0; a < Animations.GetAnimLength(body, _fileType); ++a)
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        AnimIdx edit = AnimationEdit.GetAnimation(_fileType, body, a, i);
                        Bitmap[] bits = edit?.GetFrames();
                        if (bits == null)
                        {
                            continue;
                        }

                        for (int j = 0; j < bits.Length; ++j)
                        {
                            if (bits[j] is null)
                            {
                                continue;
                            }

                            string filename = string.Format("anim{5}_{0}_{1}_{2}_{3}{4}", body, a, i, j, menu.Tag, _fileType);
                            string file = Path.Combine(path, filename);

                            using (Bitmap bit = new Bitmap(bits[j]))
                            {
                                bit.Save(file, format);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; ++i)
                {
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, body, action, i);
                    Bitmap[] bits = edit?.GetFrames();
                    if (bits == null)
                    {
                        continue;
                    }

                    for (int j = 0; j < bits.Length; ++j)
                    {
                        if (bits[j] is null)
                        {
                            continue;
                        }

                        string filename = string.Format("anim{5}_{0}_{1}_{2}_{3}{4}", body, action, i, j, menu.Tag, _fileType);
                        string file = Path.Combine(path, filename);

                        using (Bitmap bit = new Bitmap(bits[j]))
                        {
                            bit.Save(file, format);
                        }
                    }
                }
            }

            MessageBox.Show($"帧已保存到 {path}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        private void OnClickRemoveAction(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                return;
            }

            if (_fileType == 6)
            {
                if (_uopManager == null) return;

                if (AnimationListTreeView.SelectedNode.Parent == null)
                {
                    // 移除身体（动画）
                    DialogResult result = MessageBox.Show($"确定要移除 UOP 动画 {_currentBody} 吗？", "移除",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result != DialogResult.Yes) return;

                    AnimationListTreeView.SelectedNode.ForeColor = Color.Red;
                    foreach (TreeNode child in AnimationListTreeView.SelectedNode.Nodes)
                    {
                        child.ForeColor = Color.Red;
                    }

                    // 循环此身体的所有可能动作：标记为已修改（现在不提交）
                    bool anyModified = false;
                    for (int action = 0; action < 100; action++)
                    {
                        for (int dir = 0; dir < 5; dir++)
                        {
                            var uopAnim = _uopManager.GetUopAnimation(_currentBody, action, dir);
                            if (uopAnim != null)
                            {
                                uopAnim.Frames.Clear();
                                uopAnim.IsModified = true;
                                anyModified = true;
                            }
                        }
                    }

                    if (anyModified)
                    {
                        // 跟踪 AnimID，以便 SaveModifiedAnimationsToUopHybrid 稍后处理
                        UoFiddler.Controls.Uop.VdImportHelper.MarkAnimIdModified(_currentBody);
                        System.Diagnostics.Debug.WriteLine($"🔖 已移除身体 {_currentBody}：标记为待保存（延迟提交）。");
                    }
                }
                else
                {
                    // 移除动作
                    DialogResult result = MessageBox.Show($"确定要移除 UOP 动作 {_currentAction} 吗？", "移除",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result != DialogResult.Yes) return;

                    AnimationListTreeView.SelectedNode.ForeColor = Color.Red;

                    bool modified = false;
                    for (int dir = 0; dir < 5; dir++)
                    {
                        var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, dir);
                        if (uopAnim != null)
                        {
                            uopAnim.Frames.Clear();
                            uopAnim.IsModified = true;
                            modified = true;
                        }
                    }

                    if (modified)
                    {
                        // 标记待稍后保存（单次保存将持久化所有标记的 ID）
                        UoFiddler.Controls.Uop.VdImportHelper.MarkAnimIdModified(_currentBody);
                        System.Diagnostics.Debug.WriteLine($"🔖 已移除动作 {_currentAction}（身体 {_currentBody}）：标记为待保存（延迟提交）。");
                    }
                }

                if (_showOnlyValid && AnimationListTreeView.SelectedNode.ForeColor == Color.Red)
                {
                    if (AnimationListTreeView.SelectedNode.Parent == null)
                        AnimationListTreeView.SelectedNode.Remove();
                    else
                        AnimationListTreeView.SelectedNode.Parent.Remove();
                }

                Options.ChangedUltimaClass["Animations"] = true;
                AfterSelectTreeView(this, null);
                return;
            }

            if (AnimationListTreeView.SelectedNode.Parent == null)
            {
                DialogResult result = MessageBox.Show($"确定要移除动画 {_currentBody}", "移除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                AnimationListTreeView.SelectedNode.ForeColor = Color.Red;
                for (int i = 0; i < AnimationListTreeView.SelectedNode.Nodes.Count; ++i)
                {
                    AnimationListTreeView.SelectedNode.Nodes[i].ForeColor = Color.Red;
                    for (int d = 0; d < 5; ++d)
                    {
                        AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, i, d);
                        edit?.ClearFrames();
                    }
                }

                if (_showOnlyValid)
                {
                    AnimationListTreeView.SelectedNode.Remove();
                }

                Options.ChangedUltimaClass["Animations"] = true;
                AfterSelectTreeView(this, null);
            }
            else
            {
                DialogResult result = MessageBox.Show($"确定要移除动作 {_currentAction}", "移除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                for (int i = 0; i < 5; ++i)
                {
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, i);
                    edit?.ClearFrames();
                }

                AnimationListTreeView.SelectedNode.Parent.Nodes[_currentAction].ForeColor = Color.Red;
                bool valid = false;
                foreach (TreeNode node in AnimationListTreeView.SelectedNode.Parent.Nodes)
                {
                    if (node.ForeColor == Color.Red)
                    {
                        continue;
                    }

                    valid = true;
                    break;
                }

                if (!valid)
                {
                    if (_showOnlyValid)
                    {
                        AnimationListTreeView.SelectedNode.Parent.Remove();
                    }
                    else
                    {
                        AnimationListTreeView.SelectedNode.Parent.ForeColor = Color.Red;
                    }
                }

                Options.ChangedUltimaClass["Animations"] = true;
                AfterSelectTreeView(this, null);
            }
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            if (_fileType == 6)
            {
                SaveUopAnimation();
                return;
            }

            if (_fileType == 0)
            {
                return;
            }

            AnimationEdit.Save(_fileType, Options.OutputPath);
            Options.ChangedUltimaClass["Animations"] = false;

            MessageBox.Show($"动画文件已保存到 {Options.OutputPath}", "已保存", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        //我的 Soulblighter 修改
        private void OnClickRemoveFrame(object sender, EventArgs e)
        {
            if (_fileType == 6)
            {
                MessageBox.Show("UOP 格式不可用", "受限", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (FramesListView.SelectedItems.Count <= 0)
            {
                return;
            }

            int corrector = 0;
            int[] frameIndex = new int[FramesListView.SelectedItems.Count];
            for (int i = 0; i < FramesListView.SelectedItems.Count; i++)
            {
                frameIndex[i] = FramesListView.SelectedIndices[i] - corrector;
                corrector++;
            }

            foreach (var index in frameIndex)
            {
                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                if (edit == null)
                {
                    continue;
                }

                edit.RemoveFrame(index);
                FramesListView.Items.RemoveAt(FramesListView.Items.Count - 1);
                FramesTrackBar.Maximum = edit.Frames.Count != 0 ? edit.Frames.Count - 1 : 0;
                FramesListView.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
            }
        }
        //Soulblighter 修改结束

        private void OnClickReplace(object sender, EventArgs e)
        {
            if (FramesListView.SelectedItems.Count <= 0)
            {
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                int frameIndex = (int)FramesListView.SelectedItems[0].Tag;

                dialog.Multiselect = false;
                dialog.Title = $"选择图像文件以替换帧 {frameIndex}";
                dialog.CheckFileExists = true;
                dialog.Filter = "图像文件 (*.tif;*.tiff;*.bmp;*.png)|*.tif;*.tiff;*.bmp;*.png";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                using (var bmpTemp = new Bitmap(dialog.FileName))
                {
                    Bitmap bitmap = new Bitmap(bmpTemp);

                    if (_fileType == 6)
                    {
                        var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                        if (uopAnim != null && frameIndex >= 0 && frameIndex < uopAnim.Frames.Count)
                        {
                            var newFrame = new UoFiddler.Controls.Uop.DecodedUopFrame();
                            newFrame.Image = new Bitmap(bitmap);
                            newFrame.Header = new UoFiddler.Controls.Uop.UopFrameHeader
                            {
                                Width = (ushort)newFrame.Image.Width,
                                Height = (ushort)newFrame.Image.Height,
                                CenterX = (short)(newFrame.Image.Width / 2),
                                CenterY = (short)(newFrame.Image.Height)
                            };

                            var paletteEntries = UoFiddler.Controls.Uop.VdExportHelper.GenerateProperPaletteFromImage(new UoFiddler.Controls.Models.Uop.Imaging.DirectBitmap(newFrame.Image));
                            newFrame.Palette = paletteEntries.Select(p => Color.FromArgb(p.Alpha, p.R, p.G, p.B)).ToList();

                            uopAnim.Frames[frameIndex] = newFrame;
                            uopAnim.IsModified = true;

                            FramesListView.Invalidate();
                            Options.ChangedUltimaClass["Animations"] = true;
                        }
                    }
                    else
                    {
                        if (dialog.FileName.Contains(".bmp") || dialog.FileName.Contains(".tiff") ||
                            dialog.FileName.Contains(".png") || dialog.FileName.Contains(".jpeg") ||
                            dialog.FileName.Contains(".jpg"))
                        {
                            bitmap = ConvertBmpAnim(bitmap, (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value, (int)numericUpDownBlue.Value);
                        }

                        AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                        if (edit == null)
                        {
                            return;
                        }

                        edit.ReplaceFrame(bitmap, frameIndex);

                        FramesListView.Invalidate();

                        Options.ChangedUltimaClass["Animations"] = true;
                    }
                }
            }
        }

        private void OnClickAdd(object sender, EventArgs e)
        {
            if (_fileType == 6)
            {
                MessageBox.Show("UOP 格式不可用", "受限", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_fileType != 0)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = true;
                    dialog.Title = "选择要添加的图像文件";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "Gif 文件 (*.gif;)|*.gif; |位图文件 (*.bmp;)|*.bmp; |Tiff 文件 (*.tif;*.tiff)|*.tif;*.tiff; |Png 文件 (*.png;)|*.png; |Jpeg 文件 (*.jpeg;*.jpg;)|*.jpeg;*.jpg;";

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        FramesListView.BeginUpdate();
                        try
                        {
                            //我的 Soulblighter 修改
                            foreach (var fileName in dialog.FileNames)
                            {
                                using (var bmpTemp = new Bitmap(fileName))
                                {
                                    Bitmap bitmap = new Bitmap(bmpTemp);

                                    if (dialog.FileName.Contains(".bmp") || dialog.FileName.Contains(".tiff") ||
                                        dialog.FileName.Contains(".png") || dialog.FileName.Contains(".jpeg") ||
                                        dialog.FileName.Contains(".jpg"))
                                    {
                                        bitmap = ConvertBmpAnim(bitmap, (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value, (int)numericUpDownBlue.Value);

                                        //edit.GetImagePalette(bitmap);
                                    }

                                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                                    if (edit == null)
                                    {
                                        continue;
                                    }

                                    //Gif 特殊属性
                                    if (dialog.FileName.Contains(".gif"))
                                    {
                                        FrameDimension dimension = new FrameDimension(bitmap.FrameDimensionsList[0]);

                                        // 帧数
                                        int frameCount = bitmap.GetFrameCount(dimension);

                                        Bitmap[] bitBmp = new Bitmap[frameCount];

                                        bitmap.SelectActiveFrame(dimension, 0);
                                        UpdateGifPalette(bitmap, edit);

                                        ProgressBar.Maximum = frameCount;

                                        AddImageAtCertainIndex(frameCount, bitBmp, bitmap, dimension, edit);

                                        ProgressBar.Value = 0;
                                        ProgressBar.Invalidate();

                                        SetPaletteBox();

                                        _updatingUi = true;
                                        if (!_relativeMode)
                                        {
                                            CenterXNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.X;
                                            CenterYNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.Y;
                                        }
                                        else
                                        {
                                            CenterXNumericUpDown.Value = 0;
                                            CenterYNumericUpDown.Value = 0;
                                        }
                                        _updatingUi = false;

                                        Options.ChangedUltimaClass["Animations"] = true;
                                    }
                                    //Soulblighter 修改结束
                                    else
                                    {
                                        edit.AddFrame(bitmap);

                                        TreeNode node = GetNode(_currentBody);
                                        if (node != null)
                                        {
                                            node.ForeColor = Color.Black;
                                            node.Nodes[_currentAction].ForeColor = Color.Black;
                                        }

                                        int i = edit.Frames.Count - 1;
                                        var item = new ListViewItem(i.ToString(), 0)
                                        {
                                            Tag = i
                                        };

                                        FramesListView.Items.Add(item);

                                        int width = FramesListView.TileSize.Width - 5;
                                        if (bitmap.Width > FramesListView.TileSize.Width)
                                        {
                                            width = bitmap.Width;
                                        }

                                        int height = FramesListView.TileSize.Height - 5;
                                        if (bitmap.Height > FramesListView.TileSize.Height)
                                        {
                                            height = bitmap.Height;
                                        }

                                        FramesListView.TileSize = new Size(width + 5, height + 5);
                                        FramesTrackBar.Maximum = i;

                                        _updatingUi = true;
                                        if (!_relativeMode)
                                        {
                                            CenterXNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.X;
                                            CenterYNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.Y;
                                        }
                                        else
                                        {
                                            CenterXNumericUpDown.Value = 0;
                                            CenterYNumericUpDown.Value = 0;
                                        }
                                        _updatingUi = false;

                                        Options.ChangedUltimaClass["Animations"] = true;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            FramesListView.EndUpdate();
                        }

                        FramesListView.Invalidate();
                    }
                }
            }

            // 刷新列表
            _currentDir = DirectionTrackBar.Value;
            AfterSelectTreeView(null, null);
        }

        private void AddImageAtCertainIndex(int frameCount, Bitmap[] bitBmp, Bitmap bmp, FrameDimension dimension, AnimIdx edit)
        {
            // 返回特定索引的图像
            for (int index = 0; index < frameCount; index++)
            {
                bmp.SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnim(bmp, (int)numericUpDownRed.Value,
                    (int)numericUpDownGreen.Value, (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }
            }
        }

        private void OnClickExtractPalette(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                return;
            }

            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            if (edit == null)
            {
                return;
            }

            string name = $"palette_anim{_fileType}_{_currentBody}_{_currentAction}_{_currentDir}";
            if ((string)menu.Tag == "txt")
            {
                string path = Path.Combine(Options.OutputPath, name + ".txt");
                edit.ExportPalette(path, 0);
            }
            else
            {
                string path = Path.Combine(Options.OutputPath, name + "." + (string)menu.Tag);
                edit.ExportPalette(path, (string)menu.Tag == "bmp" ? 1 : 2);
            }

            MessageBox.Show($"调色板已保存到 {Options.OutputPath}", "已保存", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void OnClickImportPalette(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = "选择调色板文件";
                dialog.CheckFileExists = true;
                dialog.Filter = "txt 文件 (*.txt)|*.txt";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                if (edit == null)
                {
                    return;
                }

                using (StreamReader sr = new StreamReader(dialog.FileName))
                {
                    ushort[] palette = new ushort[Animations.PaletteCapacity];

                    int i = 0;
                    while (sr.ReadLine() is { } line)
                    {
                        if ((line = line.Trim()).Length == 0 || line.StartsWith('#'))
                        {
                            continue;
                        }

                        i++;

                        if (i >= Animations.PaletteCapacity)
                        {
                            break;
                        }

                        palette[i] = ushort.Parse(line);

                        // 我的 Soulblighter 修改
                        // 将颜色 0,0,0 转换为 0,0,8
                        // TODO: 找出为什么需要这个替换
                        if (palette[i] == 32768)
                        {
                            palette[i] = 32769;
                        }
                        // Soulblighter 修改结束
                    }

                    edit.ReplacePalette(palette);
                }

                SetPaletteBox();

                FramesListView.Invalidate();

                Options.ChangedUltimaClass["Animations"] = true;
            }
        }

        private void OnClickImportFromVD(object sender, EventArgs e)
        {
            if (_fileType == UOP_FILE_TYPE && _uopManager != null)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "选择要导入到 UOP 的 VD 文件";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "VD 文件 (*.vd)|*.vd";
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;

                    try
                    {
                        // ✅ 选择目标 UOP 文件
                        string targetUopPath = null;
                        using (var fileSelectForm = new UopFileSelectionForm(_uopManager.LoadedUopFiles))
                        {
                            if (fileSelectForm.ShowDialog() == DialogResult.OK)
                            {
                                targetUopPath = fileSelectForm.SelectedPath;
                            }
                            else
                            {
                                return;
                            }
                        }

                        // ✅ 导入 VD
                        bool success = VdImportHelper.ImportCreaturesVdToUop(dialog.FileName, _uopManager, _currentBody, targetUopPath);
                        if (!success)
                        {
                            MessageBox.Show("导入 VD 文件失败。", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // ✅ 以 HYBRID 模式保存
                        string destinationPath = Path.Combine(Options.OutputPath, Path.GetFileName(targetUopPath));
                        if (VdImportHelper.SaveModifiedAnimationsToUopHybrid(_uopManager, _currentBody, destinationPath))
                        {
                            MessageBox.Show(
                                $"✅ 动画 {_currentBody} 已成功以 HYBRID 模式导入！\n\n" +
                                $"📁 文件：{destinationPath}\n\n" +
                                $"🔹 创建了 32 个 Jenkins 条目（动作 0-31）\n" +
                                $"🔹 创建了 1 个数字条目（动作 0 - 客户端识别）",
                                "导入完成",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            _uopManager.ClearCache();
                            LoadUopAnimations();
                            Options.ChangedUltimaClass["Animations"] = false;
                        }
                        else
                        {
                            MessageBox.Show("❌ 导入成功但保存失败。请检查日志。",
                                "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            _uopManager.ClearCache();
                            LoadUopAnimations();
                            Options.ChangedUltimaClass["Animations"] = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ 导入 VD 时出错：{ex.Message}\n\n{ex.StackTrace}",
                            "导入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return;
            }

            // ================================================
            // === MUL / LEGACY 部分（保留旧的好功能） ===
            // ================================================
            if (_fileType == 0)
            {
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = "选择 VD 文件";
                dialog.CheckFileExists = true;
                dialog.Filter = "VD 文件 (*.vd)|*.vd";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                int animLength = Animations.GetAnimLength(_currentBody, _fileType);

                int currentType = animLength switch
                {
                    22 => 0,  // 怪物
                    13 => 1,  // 动物
                    35 => 2,  // 人类/装备
                    _ => -1
                };

                if (currentType == -1)
                {
                    MessageBox.Show($"未知的动画长度：{animLength}", "导入错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader bin = new BinaryReader(fs))
                    {
                        short firstShort;
                        short animType;

                        try
                        {
                            firstShort = bin.ReadInt16();
                            animType = bin.ReadInt16();
                        }
                        catch (EndOfStreamException)
                        {
                            MessageBox.Show("不是有效的 VD 文件（太短）。", "导入",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        toolStripStatusLabelVDAminInfo.Text = $"文件类型: {firstShort}, 动画类型: {animType}";

                        bool recognized = (firstShort == 0x5644) || (firstShort == 6);

                        if (!recognized)
                        {
                            toolStripStatusLabelVDAminInfo.Text += " - 不是动画文件。";
                            MessageBox.Show("不是动画文件。", "导入",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (animType != currentType)
                        {
                            toolStripStatusLabelVDAminInfo.Text += $" - 错误的动画 ID（类型）。预期: {currentType}, 得到: {animType}";

                            MessageBox.Show(
                                $"所选 .vd 文件的动画类型为 {animType}（得到: {animType}），\n" +
                                $"但程序预期动画类型为 {currentType}（预期: {currentType}）。\n\n" +
                                "这会导致“错误的动画 ID（类型）”错误。\n" +
                                "请检查 .vd 文件并确保其具有正确的动画类型。",
                                "错误的动画类型", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            return;
                        }

                        AnimationEdit.LoadFromVD(_fileType, _currentBody, bin);
                    }
                }

                // 更新树
                bool valid = false;
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    for (int j = 0; j < animLength; ++j)
                    {
                        if (AnimationEdit.IsActionDefined(_fileType, _currentBody, j))
                        {
                            node.Nodes[j].ForeColor = Color.Black;
                            valid = true;
                        }
                        else
                        {
                            node.Nodes[j].ForeColor = Color.Red;
                        }
                    }
                    node.ForeColor = valid ? Color.Black : Color.Red;
                }

                toolStripStatusLabelVDAminInfo.Text += $" - 成功导入到槽位 {_currentBody}";

                Options.ChangedUltimaClass["Animations"] = true;
                AfterSelectTreeView(this, null);

                MessageBox.Show("成功将动画导入到槽位", "导入",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void OnClickExportToVD(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                return;
            }

            double scale = 1.0;
            string input = Microsoft.VisualBasic.Interaction.InputBox("缩放 (%)", "导出 VD", "100");
            if (!string.IsNullOrEmpty(input))
            {
                if (double.TryParse(input, out double percent))
                {
                    scale = percent / 100.0;
                }
                else
                {
                    MessageBox.Show("无效的值。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                return;
            }

            string path = Options.OutputPath;
            string fileName = Path.Combine(path, $"anim{_fileType}_0x{_currentBody:X}.vd");
            AnimationEdit.ExportToVD(_fileType, _currentBody, fileName, scale);

            MessageBox.Show($"动画已保存到 {Options.OutputPath}", "导出", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void OnClickExportToBin(object sender, EventArgs e)
        {
            if (_fileType != 6 || _uopManager == null) return;

            int body;
            int action;

            if (AnimationListTreeView.SelectedNode.Parent == null)
            {
                MessageBox.Show("请选择一个具体的动作以导出为 .bin", "导出 .bin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (AnimationListTreeView.SelectedNode.Parent.Tag is ushort ushortBody)
                body = (int)ushortBody;
            else
                body = (int)AnimationListTreeView.SelectedNode.Parent.Tag;

            action = (int)AnimationListTreeView.SelectedNode.Tag;

            var fileInfo = _uopManager.GetAnimationData(body, action, 0);
            if (fileInfo == null)
            {
                MessageBox.Show("未找到此动作的数据。", "导出 .bin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // GetData(0) 检索 UOP (AMOU) 格式的完整 blob
            byte[] data = fileInfo.GetData(0);
            if (data == null || data.Length == 0)
            {
                MessageBox.Show("数据为空。", "导出 .bin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "BIN 文件 (*.bin)|*.bin";
                sfd.Title = $"保存动作 {action} 为 .bin";
                sfd.FileName = $"anim_{body}_{action}.bin";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(sfd.FileName, data);
                    MessageBox.Show($"已保存到 {sfd.FileName}", "导出 .bin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OnClickExportVdRemap(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            string targetType = (string)clickedItem.Tag;

            (short animType, int vdLength) = GetVdTargetType(targetType);

            // 1. 确定目标动作名称（左列标签）
            // 如果导出到特定的 MUL 格式，我们首选该类型的标准 MUL 动作名称。
            string[] targetSystemActionNames;
            if (targetType == "monster_mul") targetSystemActionNames = _animNames[1];
            else if (targetType == "animal_mul" || targetType == "sea_monster_mul") targetSystemActionNames = _animNames[0];
            else if (targetType == "equipment_mul") targetSystemActionNames = _animNames[2];
            else targetSystemActionNames = GetActionNameArray(animType);

            var targetActionNames = new List<string>();
            for (int i = 0; i < vdLength; i++)
            {
                targetActionNames.Add(i < targetSystemActionNames.Length && !string.IsNullOrEmpty(targetSystemActionNames[i])
                    ? $"{i:D2} {targetSystemActionNames[i]}"
                    : $"{i:D2} (动作)");
            }

            // 2. 确定源动作名称（右列下拉选项）
            Dictionary<int, string> sourceActionNames;
            if (_uopManager != null)
            {
                sourceActionNames = GetCreatureActionNames();
            }
            else
            {
                sourceActionNames = new Dictionary<int, string>();
                int sourceAnimLength = Animations.GetAnimLength(_currentBody, _fileType);
                string[] names = null;
                if (sourceAnimLength == 13) names = _animNames[0];
                else if (sourceAnimLength == 22) names = _animNames[1];
                else if (sourceAnimLength == 35) names = _animNames[2];

                if (names != null)
                {
                    for (int i = 0; i < names.Length; i++) sourceActionNames[i] = names[i];
                }
            }

            // 3. 准备映射数据
            var vdSlotToAbstractMap = new Dictionary<int, int[]>();
            bool isDirectUopMapping = false;

            if (_uopManager == null)
            {
                for (int i = 0; i < vdLength; i++) vdSlotToAbstractMap[i] = new[] { i };
            }
            else
            {
                switch (targetType)
                {
                    case "monster_mul":
                        vdSlotToAbstractMap = new Dictionary<int, int[]> {
                                        {0, new[] {22, 24}}, {1, new[] {25}}, {2, new[] {2}}, {3, new[] {3}}, {4, new[] {4}}, {5, new[] {5}}, {6, new[] {6}}, {7, new[] {7}}, {8, new[] {8}}, {9, new[] {9}}, {10, new[] {10}},
                                        {11, new[] {11}}, {12, new[] {12}}, {13, new[] {13}}, {14, new[] {14}}, {15, new[] {15}}, {16, new[] {16}}, {17, new[] {1}},  {18, new[] {26}}, {19, new[] {19}},  {20, new[] {20}}, {21, new[] {21}}
                                    };
                        break;
                    case "animal_mul":
                        vdSlotToAbstractMap = new Dictionary<int, int[]> {
                                        {  0, new[] { 22 } },
                                        {  1, new[] { 24 } },
                                        {  2, new[] { 25 } },
                                        {  3, new[] { 27 } },
                                        {  4, new[] { 23 } },
                                        {  5, new[] { 4 } },
                                        {  6, new[] { 5 } },
                                        {  7, new[] { 10 } },
                                        {  8, new[] { 2 } },
                                        {  9, new[] { 1 } },
                                        { 10, new[] { 26 } },
                                        { 11, new[] { 11 } },
                                        { 12, new[] { 3 } }
                                    };
                        break;
                    case "sea_monster_mul":
                        vdSlotToAbstractMap = new Dictionary<int, int[]> { { 0, new[] { 0 } }, { 1, new[] { 24 } }, { 2, new[] { 25 } }, { 3, new[] { 1 } }, { 4, new[] { 26 } }, { 5, new[] { 4 } }, { 6, new[] { 5 } }, { 7, new[] { 10 } }, { 8, new[] { 2 } } };
                        break;
                    case "equipment_mul":
                        for (int j = 0; j < vdLength; j++) vdSlotToAbstractMap[j] = new[] { j };
                        break;
                    case "creatures_uop":
                    case "equipement_uop":
                        isDirectUopMapping = true;
                        break;
                }
            }

            List<int> availableUopGroupIndexes;
            Dictionary<int, int> remapMap;

            if (_uopManager != null)
            {
                availableUopGroupIndexes = _uopManager.GetAvailableUopGroupIndexes(_currentBody);
                if (_uopManager.IgnoreAnimationSequence)
                {
                    remapMap = new Dictionary<int, int>();
                }
                else
                {
                    remapMap = _uopManager.GetUopRemapping(_currentBody) ?? new Dictionary<int, int>();
                }
            }
            else
            {
                availableUopGroupIndexes = new List<int>();
                int maxActions = Animations.GetAnimLength(_currentBody, _fileType);
                for (int i = 0; i < maxActions; i++)
                {
                    if (AnimationEdit.IsActionDefined(_fileType, _currentBody, i))
                    {
                        availableUopGroupIndexes.Add(i);
                    }
                }
                remapMap = new Dictionary<int, int>();
            }

            var uopGroupIndexOptions = new List<Models.Uop.UopIndexOption> { new Models.Uop.UopIndexOption { Id = -1, DisplayName = "--- 无 ---" } };
            uopGroupIndexOptions.AddRange(availableUopGroupIndexes.Select(uopGroupIndex =>
            {
                string name = sourceActionNames.TryGetValue(uopGroupIndex, out var n) ? n : "未知";
                return new Models.Uop.UopIndexOption { Id = uopGroupIndex, DisplayName = $"{uopGroupIndex} ({name})" };
            }));

            var initialMapping = new Dictionary<int, int>();

            for (int i = 0; i < vdLength; i++)
            {
                int abstractId;
                if (isDirectUopMapping)
                {
                    abstractId = i;
                }
                else
                {
                    if (!vdSlotToAbstractMap.TryGetValue(i, out int[] abstractIds) || abstractIds.Length == 0)
                    {
                        abstractId = -1;
                    }
                    else
                    {
                        abstractId = abstractIds[0];
                    }
                }

                int finalGroup = -1;
                if (abstractId != -1)
                {
                    int targetId;
                    if (remapMap.TryGetValue(abstractId, out int remappedId))
                    {
                        targetId = remappedId;
                    }
                    else
                    {
                        targetId = abstractId;
                    }

                    if (availableUopGroupIndexes.Contains(targetId))
                    {
                        finalGroup = targetId;
                    }
                }

                initialMapping[i] = finalGroup;
            }

            using (var remapperForm = new VdExportRemapperForm())
            {
                remapperForm.PopulateForm(targetActionNames, uopGroupIndexOptions, initialMapping);

                if (remapperForm.ShowDialog(this) == DialogResult.OK)
                {
                    double scale = 1.0;
                    string input = Microsoft.VisualBasic.Interaction.InputBox("缩放 (%)", "导出 VD", "100");
                    if (!string.IsNullOrEmpty(input))
                    {
                        if (double.TryParse(input, out double percent))
                        {
                            scale = percent / 100.0;
                        }
                        else
                        {
                            MessageBox.Show("无效的值。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                    var finalMapping = remapperForm.GetRemapping();
                    var targetActionNameToIndex = new Dictionary<string, int>();
                    for (int i = 0; i < targetActionNames.Count; ++i)
                    {
                        targetActionNameToIndex[targetActionNames[i]] = i;
                    }

                    var exportedAnimations = new Models.Uop.UOAnimation[vdLength];

                    foreach (var mappingEntry in finalMapping)
                    {
                        string targetActionName = mappingEntry.Key;
                        int sourceGroupId = mappingEntry.Value;

                        if (targetActionNameToIndex.TryGetValue(targetActionName, out int targetActionIndex) && sourceGroupId != -1)
                        {
                            if (targetActionIndex < vdLength)
                            {
                                if (_uopManager != null)
                                {
                                    exportedAnimations[targetActionIndex] = _uopManager.GetAnimationExportData(_currentBody, sourceGroupId);
                                }
                                else
                                {
                                    exportedAnimations[targetActionIndex] = CreateUoAnimationFromMulAction(_fileType, _currentBody, sourceGroupId);
                                }
                            }
                        }
                    }

                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "VD 文件 (*.vd)|*.vd";
                        sfd.Title = "保存 VD 文件";
                        sfd.FileName = $"anim_{targetType}_{_currentBody}.vd";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                            using (BinaryWriter writer = new BinaryWriter(fs))
                            {
                                if (animType == 4) // 生物 (UOP) - 使用优化导出
                                {
                                    var remapping = new Dictionary<int, int>();
                                    foreach (var kvp in finalMapping)
                                    {
                                        if (targetActionNameToIndex.TryGetValue(kvp.Key, out int targetIdx))
                                        {
                                            remapping[targetIdx] = kvp.Value;
                                        }
                                    }

                                    // 确保在序列化之前提交已修改的动画
                                    if (_uopManager != null)
                                    {
                                        _uopManager.CommitAllChanges();
                                        System.Diagnostics.Debug.WriteLine("导出 VD：在 VD 导出之前调用 CommitAllChanges 以序列化已修改的 UOP 动画。");
                                    }

                                    // 新：对于仅作为 UOAnimation 存在（从 MUL 生成）的导出动画，
                                    // 在缓存中创建 UopAnimIdx 并填充帧，以便 CommitChanges 可以对它们进行编码。
                                    if (_uopManager != null)
                                    {
                                        foreach (var kvp in remapping)
                                        {
                                            int targetIdx = kvp.Key;
                                            int sourceGroup = kvp.Value;

                                            if (sourceGroup < 0) continue;
                                            var exported = (targetIdx >= 0 && targetIdx < exportedAnimations.Length) ? exportedAnimations[targetIdx] : null;
                                            if (exported == null) continue;

                                            try
                                            {
                                                var fi = _uopManager.GetAnimationData(_currentBody, sourceGroup, 0);
                                                if (fi != null)
                                                {
                                                    // 已有源数据，无需操作
                                                    continue;
                                                }

                                                System.Diagnostics.Debug.WriteLine($"导出 VD：为 Anim={_currentBody} Action={sourceGroup} 创建内存 UOP 条目");

                                                // 确定每个方向的帧数
                                                int framesPerDir = exported.FramesPerDirection;
                                                if (framesPerDir <= 0)
                                                {
                                                    if (exported.Frames != null && exported.Frames.Count > 0)
                                                        framesPerDir = Math.Max(1, exported.Frames.Count / 5);
                                                    else
                                                        framesPerDir = 0;
                                                }

                                                for (int dir = 0; dir < 5; dir++)
                                                {
                                                    // 创建新的 UopAnimIdx 并确保在缓存中
                                                    var uopAnim = _uopManager.CreateNewUopAnimation(_currentBody, sourceGroup, dir);
                                                    uopAnim.Frames.Clear();

                                                    if (framesPerDir == 0) continue;

                                                    for (int f = 0; f < framesPerDir; f++)
                                                    {
                                                        int globalIndex = dir * framesPerDir + f;
                                                        if (globalIndex < 0 || exported.Frames == null || globalIndex >= exported.Frames.Count) break;

                                                        // 使用反射提取帧数据（名称不可靠：尝试常见变体）
                                                        object frameEntry = exported.Frames[globalIndex];
                                                        if (frameEntry == null) continue;

                                                        object frameData = frameEntry.GetType().GetProperty("FrameData")?.GetValue(frameEntry)
                                                                           ?? frameEntry.GetType().GetProperty("Data")?.GetValue(frameEntry)
                                                                           ?? frameEntry.GetType().GetProperty("Frame")?.GetValue(frameEntry);

                                                        if (frameData == null) continue;

                                                        // 提取图像（DirectBitmap 或 Bitmap）
                                                        object imageObj = frameData.GetType().GetProperty("Image")?.GetValue(frameData)
                                                                          ?? frameData.GetType().GetProperty("Bitmap")?.GetValue(frameData)
                                                                          ?? frameData.GetType().GetProperty("Img")?.GetValue(frameData);

                                                        Bitmap bmp = null;
                                                        if (imageObj is Bitmap b) bmp = new Bitmap(b);
                                                        else if (imageObj != null)
                                                        {
                                                            // 尝试属性 "Bitmap"
                                                            var p = imageObj.GetType().GetProperty("Bitmap")?.GetValue(imageObj);
                                                            if (p is Bitmap pb) bmp = new Bitmap(pb);
                                                            else
                                                            {
                                                                // 尝试 ToBitmap 方法
                                                                var mi = imageObj.GetType().GetMethod("ToBitmap") ?? imageObj.GetType().GetMethod("GetBitmap");
                                                                if (mi != null)
                                                                {
                                                                    var res = mi.Invoke(imageObj, null);
                                                                    if (res is Bitmap rb) bmp = new Bitmap(rb);
                                                                }
                                                            }
                                                        }

                                                        if (bmp == null)
                                                        {
                                                            System.Diagnostics.Debug.WriteLine($"导出 VD：无法为帧 {globalIndex}（Anim {_currentBody} Act {sourceGroup} Dir {dir}）检索 Bitmap");
                                                            continue;
                                                        }

                                                        var decoded = new UoFiddler.Controls.Uop.DecodedUopFrame();
                                                        decoded.Image = new Bitmap(bmp);

                                                        // 标头：尝试从 frameData 读取 CenterX/CenterY/Width/Height
                                                        short centerX = 0, centerY = 0;
                                                        ushort width = (ushort)decoded.Image.Width, height = (ushort)decoded.Image.Height;

                                                        try
                                                        {
                                                            var hdr = frameData.GetType().GetProperty("Header")?.GetValue(frameData);
                                                            if (hdr != null)
                                                            {
                                                                var cx = hdr.GetType().GetProperty("CenterX")?.GetValue(hdr);
                                                                var cy = hdr.GetType().GetProperty("CenterY")?.GetValue(hdr);
                                                                var w = hdr.GetType().GetProperty("Width")?.GetValue(hdr);
                                                                var h = hdr.GetType().GetProperty("Height")?.GetValue(hdr);
                                                                if (cx != null) centerX = Convert.ToInt16(cx);
                                                                if (cy != null) centerY = Convert.ToInt16(cy);
                                                                if (w != null) width = Convert.ToUInt16(w);
                                                                if (h != null) height = Convert.ToUInt16(h);
                                                            }
                                                            else
                                                            {
                                                                var cx = frameData.GetType().GetProperty("CenterX")?.GetValue(frameData);
                                                                var cy = frameData.GetType().GetProperty("CenterY")?.GetValue(frameData);
                                                                if (cx != null) centerX = Convert.ToInt16(cx);
                                                                if (cy != null) centerY = Convert.ToInt16(cy);
                                                            }
                                                        }
                                                        catch { /* 尽力而为 */ }

                                                        decoded.Header = new UoFiddler.Controls.Uop.UopFrameHeader
                                                        {
                                                            CenterX = centerX,
                                                            CenterY = centerY,
                                                            Width = width,
                                                            Height = height
                                                        };

                                                        // 调色板：尝试提取列表<Color>
                                                        try
                                                        {
                                                            var paletteObj = frameData.GetType().GetProperty("Palette")?.GetValue(frameData);
                                                            if (paletteObj is System.Collections.IEnumerable palEnum)
                                                            {
                                                                var palList = new List<Color>();
                                                                foreach (var entry in palEnum)
                                                                {
                                                                    if (entry is Color c) palList.Add(c);
                                                                    else
                                                                    {
                                                                        // 尝试属性 R,G,B,A
                                                                        var r = entry.GetType().GetProperty("R")?.GetValue(entry);
                                                                        var g = entry.GetType().GetProperty("G")?.GetValue(entry);
                                                                        var b2 = entry.GetType().GetProperty("B")?.GetValue(entry);
                                                                        var a = entry.GetType().GetProperty("Alpha")?.GetValue(entry) ?? entry.GetType().GetProperty("A")?.GetValue(entry);
                                                                        if (r != null && g != null && b2 != null)
                                                                        {
                                                                            palList.Add(Color.FromArgb(a != null ? Convert.ToInt32(a) : 255, Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b2)));
                                                                        }
                                                                    }
                                                                }
                                                                decoded.Palette = palList;
                                                            }
                                                        }
                                                        catch { /* 调色板不可用时忽略 */ }

                                                        uopAnim.Frames.Add(decoded);
                                                    } // framesPerDir
                                                    uopAnim.IsModified = true;
                                                    // 填充此动作的所有方向后，提交更改（将在下面执行）
                                                } // dir 循环

                                                // 填充此动作的缓存 UopAnimIdx 条目后，请求提交
                                                _uopManager.CommitChanges(_currentBody, sourceGroup);
                                                System.Diagnostics.Debug.WriteLine($"导出 VD：已提交 Anim={_currentBody} Action={sourceGroup} 的内存 UOP 数据");
                                            }
                                            catch (Exception ex)
                                            {
                                                System.Diagnostics.Debug.WriteLine($"导出 VD：为 Anim {_currentBody} Action {sourceGroup} 创建内存 UOP 条目失败：{ex.Message}");
                                            }
                                        } // foreach remapping
                                    } // if _uopManager != null

                                    Uop.VdExportHelper.WriteVDCreaturesUop(writer, _uopManager, _currentBody, remapping, scale);
                                }
                                else
                                {
                                    Uop.VdExportHelper.WriteVDHeader(writer, animType);
                                    Uop.VdExportHelper.WriteVDAnimations(writer, exportedAnimations, animType, scale);
                                }
                            }
                            MessageBox.Show($"成功导出到 {sfd.FileName}", "导出完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private Dictionary<int, string> GetCreatureActionNames()
        {
            return new Dictionary<int, string>
            {
                {0, "行走战斗"}, {1, "空闲战斗"}, {2, "向后死亡"}, {3, "向前死亡"}, {4, "攻击 1"},
                {5, "攻击 2"}, {6, "攻击 3"}, {7, "弓攻击"}, {8, "弩攻击"}, {9, "投掷攻击"},
                {10, "受击"}, {11, "翻找"}, {12, "施法"}, {13, "施法2"}, {14, "施法3"},
                {15, "右格挡"}, {16, "左格挡"}, {17, "空闲"}, {18, "烦躁"}, {19, "飞行"}, {20, "起飞"},
                {21, "空中受击"}, {22, "行走"}, {23, "特殊"}, {24, "奔跑"}, {25, "空闲"},
                {26, "烦躁"}, {27, "咆哮"}, {28, "和平转战斗"}, {29, "骑马 - 行走"}, {30, "骑马 - 奔跑"}, {31, "骑马 - 空闲"}
            };
        }

        private static Dictionary<int, string> _charActionNames;

        private static Dictionary<int, string> GetCharActionNamesDictionary()
        {
            if (_charActionNames == null)
            {
                _charActionNames = new Dictionary<int, string>();
                for (int i = 0; i < Uop.UopConstants.ActionNames.CharActions.Length; i++)
                {
                    if (!string.IsNullOrEmpty(Uop.UopConstants.ActionNames.CharActions[i]))
                    {
                        _charActionNames[i] = Uop.UopConstants.ActionNames.CharActions[i];
                    }
                }
            }
            return _charActionNames;
        }

        private string GetActionDescription(int animId, int actionId)
        {
            Dictionary<int, string> namesDictionary;
            // 直接访问 _uopManager，因为 GetActionDescription 不再是静态的
            List<int> availableActions = _uopManager.GetAvailableActions(animId);

            if (availableActions.Count > 32) // 基于用户输入启发式
            {
                namesDictionary = GetCharActionNamesDictionary();
            }
            else
            {
                namesDictionary = GetCreatureActionNames();
            }

            if (namesDictionary.TryGetValue(actionId, out string name))
            {
                return name;
            }
            return "未知动作";
        }


        private string[] GetActionNameArray(short animType)
        {
            switch (animType)
            {
                case 0: return Uop.UopConstants.ActionNames.MonsterActions;
                case 1: return Uop.UopConstants.ActionNames.AnimalActions;
                case 2: return Uop.UopConstants.ActionNames.HumanActions;
                case 3: return Uop.UopConstants.ActionNames.CharActions;
                case 4:
                    var creatureActions = new string[32];
                    var names = GetCreatureActionNames();
                    for (int i = 0; i < creatureActions.Length; i++)
                    {
                        creatureActions[i] = names.TryGetValue(i, out var name) ? name : "";
                    }
                    return creatureActions;
                default: return _animNames[1];
            }
        }

        private (short animType, int vdLength) GetVdTargetType(string targetType)
        {
            switch (targetType)
            {
                case "monster_mul":
                    return (0, 22);
                case "animal_mul":
                    return (1, 13);
                case "sea_monster_mul":
                    return (1, 13);
                case "equipment_mul":
                    return (2, 35);
                case "equipement_uop":
                    return (3, 78);
                case "creatures_uop":
                    return (4, 32);
                default:
                    return (0, 22);
            }
        }


        private void OnClickShowOnlyValid(object sender, EventArgs e)
        {
            _showOnlyValid = !_showOnlyValid;

            if (_fileType == UOP_FILE_TYPE)
            {
                LoadUopAnimations();
            }
            else
            {
                LoadMulAnimations();
            }
        }

        //我的 Soulblighter 修改
        private void SameCenterButton_Click(object sender, EventArgs e)
        {
            // TODO：相同中心按钮没有撤消功能
            try
            {
                if (_fileType == 0)
                {
                    return;
                }

                if (_relativeMode)
                {
                    int dX = _accumulatedRelativeX;
                    int dY = _accumulatedRelativeY;
                    _accumulatedRelativeX = 0;
                    _accumulatedRelativeY = 0;

                    if (dX == 0 && dY == 0) return;

                    if (_fileType == 6)
                    {
                        var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                        if (uopAnim != null)
                        {
                            for (int i = 0; i < uopAnim.Frames.Count; i++)
                            {
                                if (i == FramesTrackBar.Value) continue;
                                var f = uopAnim.Frames[i];
                                uopAnim.ChangeCenter(i, f.Header.CenterX + dX, f.Header.CenterY + dY);
                            }
                        }
                    }
                    else
                    {
                        AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                        if (edit != null)
                        {
                            for (int i = 0; i < edit.Frames.Count; i++)
                            {
                                if (i == FramesTrackBar.Value) continue;
                                var f = edit.Frames[i];
                                f.ChangeCenter(f.Center.X + dX, f.Center.Y + dY);
                            }
                        }
                    }
                    Options.ChangedUltimaClass["Animations"] = true;
                    AnimationPictureBox.Invalidate();
                    return;
                }

                if (_fileType == 6)
                {
                    if (FramesListView.Items.Count == 0) return;
                    int newCenterX = (int)CenterXNumericUpDown.Value;
                    int newCenterY = (int)CenterYNumericUpDown.Value;

                    UpdateUopData(newCenterX: newCenterX, newCenterY: newCenterY, applyToAllFrames: true);
                    Options.ChangedUltimaClass["Animations"] = true;
                    AnimationPictureBox.Invalidate();
                    return;
                }

                AnimIdx edit2 = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                if (edit2 == null || edit2.Frames.Count < FramesTrackBar.Value)
                {
                    return;
                }

                FrameEdit[] frame = new FrameEdit[edit2.Frames.Count];
                for (int index = 0; index < edit2.Frames.Count; index++)
                {
                    frame[index] = edit2.Frames[index];
                    frame[index].ChangeCenter((int)CenterXNumericUpDown.Value, (int)CenterYNumericUpDown.Value);
                    Options.ChangedUltimaClass["Animations"] = true;
                    AnimationPictureBox.Invalidate();
                }
            }
            catch (NullReferenceException)
            {
                // TODO：添加日志记录或修复？
                // 忽略
            }
        }
        //Soulblighter 修改结束

        //我的 Soulblighter 修改
        private void FromGifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = "选择调色板文件";
                dialog.CheckFileExists = true;
                dialog.Filter = "Gif 文件 (*.gif)|*.gif";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Bitmap bit = new Bitmap(dialog.FileName);
                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                if (edit == null)
                {
                    return;
                }

                FrameDimension dimension = new FrameDimension(bit.FrameDimensionsList[0]);
                // 帧数
                //int frameCount = bit.GetFrameCount(dimension); // TODO：未使用的变量？
                bit.SelectActiveFrame(dimension, 0);
                UpdateGifPalette(bit, edit);
                SetPaletteBox();
                FramesListView.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
            }
        }

        private void ReferencePointX(object sender, EventArgs e)
        {
            AnimationPictureBox.Invalidate();
        }

        private void ReferencePointY(object sender, EventArgs e)
        {
            AnimationPictureBox.Invalidate();
        }

        private static bool _lockButton;

        private void AnimationPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (_lockButton || !ToolStripLockButton.Enabled)
            {
                return;
            }

            RefXNumericUpDown.Value = (decimal)((_framePoint.X - e.X) / _zoomFactor);
            RefYNumericUpDown.Value = (decimal)((_framePoint.Y - e.Y) / _zoomFactor);

            AnimationPictureBox.Invalidate();
        }

        // 按键时更改帧中心
        private void TxtSendData_KeyDown(object sender, KeyEventArgs e)
        {
            if (AnimationTimer.Enabled)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Right:
                    {
                        CenterXNumericUpDown.Value--;
                        CenterXNumericUpDown.Invalidate();
                        break;
                    }
                case Keys.Left:
                    {
                        CenterXNumericUpDown.Value++;
                        CenterXNumericUpDown.Invalidate();
                        break;
                    }
                case Keys.Up:
                    {
                        CenterYNumericUpDown.Value++;
                        CenterYNumericUpDown.Invalidate();
                        break;
                    }
                case Keys.Down:
                    {
                        CenterYNumericUpDown.Value--;
                        CenterYNumericUpDown.Invalidate();
                        break;
                    }
            }
            AnimationPictureBox.Invalidate();
        }

        // 按键时更改参考点中心
        private void TxtSendData_KeyDown2(object sender, KeyEventArgs e)
        {
            if (_lockButton || !ToolStripLockButton.Enabled)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Right:
                    {
                        RefXNumericUpDown.Value--;
                        RefXNumericUpDown.Invalidate();
                        break;
                    }
                case Keys.Left:
                    {
                        RefXNumericUpDown.Value++;
                        RefXNumericUpDown.Invalidate();
                        break;
                    }
                case Keys.Up:
                    {
                        RefYNumericUpDown.Value++;
                        RefYNumericUpDown.Invalidate();
                        break;
                    }
                case Keys.Down:
                    {
                        RefYNumericUpDown.Value--;
                        RefYNumericUpDown.Invalidate();
                        break;
                    }
            }
            AnimationPictureBox.Invalidate();
        }

        private void ToolStripLockButton_Click(object sender, EventArgs e)
        {
            _lockButton = !_lockButton;
            RefXNumericUpDown.Enabled = !_lockButton;
            RefYNumericUpDown.Enabled = !_lockButton;
        }

        // 添加所有方向
        private void AllDirectionsAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fileType != 0)
            {
                // ✅ UOP：检查是否需要选择目标文件
                string targetUopPath = null;
                if (_fileType == 6 && _uopManager != null)
                {
                    bool animationExists = false;
                    for (int d = 0; d < 5; d++)
                    {
                        if (_uopManager.GetUopAnimation(_currentBody, _currentAction, d) != null)
                        {
                            animationExists = true;
                            break;
                        }
                    }

                    if (!animationExists)
                    {
                        using (var fileSelectForm = new UopFileSelectionForm(_uopManager.LoadedUopFiles))
                        {
                            if (fileSelectForm.ShowDialog() == DialogResult.OK)
                            {
                                targetUopPath = fileSelectForm.SelectedPath;
                            }
                            else
                            {
                                return; // 已取消
                            }
                        }
                    }
                }

                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = true;
                    dialog.Title = "选择图像（5 的倍数）添加到所有方向";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "图像文件 (*.bmp, *.jpg, *.png, *.tiff, *.gif)|*.bmp;*.jpg;*.png;*.tiff;*.gif";

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        if (dialog.FileNames.Length == 0 || dialog.FileNames.Length % 5 != 0)
                        {
                            MessageBox.Show("请选择 5 的倍数张图像（例如 5、10、15...）。\n图像将均匀分布在 5 个方向上。",
                                "无效选择", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        DirectionTrackBar.Enabled = false;
                        AddFilesAllDirections(dialog, targetUopPath);
                        DirectionTrackBar.Enabled = true;
                    }
                }
            }

            // 刷新列表
            _currentDir = DirectionTrackBar.Value;
            AfterSelectTreeView(null, null);
        }

        private void AddFilesAllDirections(OpenFileDialog dialog, string targetUopPath = null)
        {
            // 确保严格的字母顺序
            var fileList = dialog.FileNames.OrderBy(f => f).ToList();

            int totalFiles = fileList.Count;
            int filesPerDir = totalFiles / 5;

            if (_fileType == 6) // UOP - 批处理
            {
                for (int dir = 0; dir < 5; dir++)
                {
                    int startIndex = dir * filesPerDir;
                    int endIndex = startIndex + filesPerDir;

                    var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, dir);
                    if (uopAnim == null)
                    {
                        uopAnim = _uopManager.CreateNewUopAnimation(_currentBody, _currentAction, dir, targetUopPath);
                    }

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        string fileName = fileList[i];
                        if (!System.IO.File.Exists(fileName)) continue;

                        using (Bitmap bmpTemp = new Bitmap(fileName))
                        {
                            Bitmap bitmap = new Bitmap(bmpTemp);
                            FrameDimension dimension = new FrameDimension(bitmap.FrameDimensionsList[0]);
                            int frameCount = bitmap.GetFrameCount(dimension);

                            for (int f = 0; f < frameCount; f++)
                            {
                                bitmap.SelectActiveFrame(dimension, f);
                                using (var frameBmp = new Bitmap(bitmap))
                                {
                                    var frame = new UoFiddler.Controls.Uop.DecodedUopFrame();
                                    frame.Image = new Bitmap(frameBmp);
                                    frame.Header = new UoFiddler.Controls.Uop.UopFrameHeader
                                    {
                                        Width = (ushort)frame.Image.Width,
                                        Height = (ushort)frame.Image.Height,
                                        CenterX = (short)(frame.Image.Width / 2),
                                        CenterY = (short)(frame.Image.Height)
                                    };

                                    var paletteEntries = UoFiddler.Controls.Uop.VdExportHelper.GenerateProperPaletteFromImage(new UoFiddler.Controls.Models.Uop.Imaging.DirectBitmap(frame.Image));
                                    frame.Palette = paletteEntries.Select(p => Color.FromArgb(p.Alpha, p.R, p.G, p.B)).ToList();

                                    uopAnim.Frames.Add(frame);
                                    uopAnim.IsModified = true;
                                }
                            }
                        }
                    }
                }
                _uopManager.CommitChanges(_currentBody, _currentAction);
                _uopManager.ClearCache(); // 强制重新加载以更新 BB
            }
            else // MUL
            {
                for (int dir = 0; dir < 5; dir++)
                {
                    DirectionTrackBar.Value = dir;
                    _currentDir = dir;

                    int startIndex = dir * filesPerDir;
                    int endIndex = startIndex + filesPerDir;

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        string fileName = fileList[i];
                        if (!System.IO.File.Exists(fileName)) continue;

                        AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                        if (edit != null)
                        {
                            using (Bitmap bmpTemp = new Bitmap(fileName))
                            {
                                Bitmap bitmap = new Bitmap(bmpTemp);
                                FrameDimension dimension = new FrameDimension(bitmap.FrameDimensionsList[0]);
                                int frameCount = bitmap.GetFrameCount(dimension);

                                if (frameCount > 1 || fileName.ToLower().EndsWith(".gif"))
                                {
                                    bitmap.SelectActiveFrame(dimension, 0);
                                    UpdateGifPalette(bitmap, edit);
                                    Bitmap[] bitBmp = new Bitmap[frameCount];
                                    AddImageAtCertainIndex(frameCount, bitBmp, bitmap, dimension, edit);
                                }
                                else
                                {
                                    edit.AddFrame(bitmap);
                                }
                            }
                        }
                    }
                    FramesListView.Invalidate();
                }
            }
        }

        private void DrawEmptyRectangleToolStripButton_Click(object sender, EventArgs e)
        {
            _drawEmpty = !_drawEmpty;
            AnimationPictureBox.Invalidate();
        }

        private void DrawFullRectangleToolStripButton_Click(object sender, EventArgs e)
        {
            _drawFull = !_drawFull;
            AnimationPictureBox.Invalidate();
        }

        private void DrawBoundingBoxToolStripButton_Click(object sender, EventArgs e)
        {
            _drawBoundingBox = !_drawBoundingBox;
            AnimationPictureBox.Invalidate();
        }

        private void DrawCroppingBoxToolStripButton_Click(object sender, EventArgs e)
        {
            _drawCroppingBox = !_drawCroppingBox;
            AnimationPictureBox.Invalidate();
        }

        private void AnimationEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnimationTimer.Enabled = false;
            _drawFull = false;
            _drawEmpty = false;
            _lockButton = false;
            _loaded = false;

            ControlEvents.FilePathChangeEvent -= OnFilePathChangeEvent;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (FramesTrackBar.Value < FramesTrackBar.Maximum)
            {
                FramesTrackBar.Value++;
            }
            else
            {
                FramesTrackBar.Value = 0;
            }

            AnimationPictureBox.Invalidate();
        }

        private void ToolStripButtonPlayAnimation_Click(object sender, EventArgs e)
        {
            if (AnimationTimer.Enabled)
            {
                AnimationTimer.Enabled = false;
                FramesTrackBar.Enabled = true;
                SameCenterButton.Enabled = true;
                CenterXNumericUpDown.Enabled = true;
                CenterYNumericUpDown.Enabled = true;

                if (DrawReferencialPointToolStripButton.Checked)
                {
                    ToolStripLockButton.Enabled = false;
                    _blackUndraw = _blackUnDrawTransparent;
                    _whiteUnDraw = _whiteUnDrawTransparent;
                }
                else
                {
                    ToolStripLockButton.Enabled = true;
                    _blackUndraw = _blackUnDrawOpaque;
                    _whiteUnDraw = _whiteUnDrawOpaque;
                }

                if (ToolStripLockButton.Checked || DrawReferencialPointToolStripButton.Checked)
                {
                    RefXNumericUpDown.Enabled = false;
                    RefYNumericUpDown.Enabled = false;
                }
                else
                {
                    RefXNumericUpDown.Enabled = true;
                    RefYNumericUpDown.Enabled = true;
                }
            }
            else
            {
                AnimationTimer.Enabled = true;
                FramesTrackBar.Enabled = false;
                SameCenterButton.Enabled = false;

                CenterXNumericUpDown.Enabled = false;
                CenterYNumericUpDown.Enabled = false;
            }

            AnimationPictureBox.Invalidate();
        }

        private void AnimationSpeedTrackBar_ValueChanged(object sender, EventArgs e)
        {
            AnimationTimer.Interval = 50 + (AnimationSpeedTrackBar.Value * 30);
        }

        private void DrawReferencialPointToolStripButton_Click(object sender, EventArgs e)
        {
            if (!DrawReferencialPointToolStripButton.Checked)
            {
                _blackUndraw = _blackUnDrawOpaque;
                _whiteUnDraw = _whiteUnDrawOpaque;

                ToolStripLockButton.Enabled = true;
                if (ToolStripLockButton.Checked)
                {
                    RefXNumericUpDown.Enabled = false;
                    RefYNumericUpDown.Enabled = false;
                }
                else
                {
                    RefXNumericUpDown.Enabled = true;
                    RefYNumericUpDown.Enabled = true;
                }
            }
            else
            {
                _blackUndraw = _blackUnDrawTransparent;
                _whiteUnDraw = _whiteUnDrawTransparent;
                ToolStripLockButton.Enabled = false;
                RefXNumericUpDown.Enabled = false;
                RefYNumericUpDown.Enabled = false;
            }
            AnimationPictureBox.Invalidate();
        }

        // 所有方向带画布
        private void AllDirectionsAddWithCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fileType == 6)
            {
                MessageBox.Show("UOP 格式不可用", "受限", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (_fileType != 0)
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Multiselect = true;
                        dialog.Title = "选择 5 个 GIF 添加";
                        dialog.CheckFileExists = true;
                        dialog.Filter = "图像文件 (*.bmp, *.jpg, *.png, *.tiff, *.gif)|*.bmp;*.jpg;*.png;*.tiff;*.gif";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            Color customConvert = Color.FromArgb(255, (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value, (int)numericUpDownBlue.Value);
                            DirectionTrackBar.Enabled = false;
                            if (dialog.FileNames.Length == 5)
                            {
                                DirectionTrackBar.Value = 0;
                                AddSelectedFiles(dialog, customConvert);
                            }

                            // 如果 dialog.FileNames.Length != 5 则循环
                            while (dialog.FileNames.Length != 5)
                            {
                                if (dialog.ShowDialog() == DialogResult.Cancel)
                                {
                                    break;
                                }

                                if (dialog.FileNames.Length != 5)
                                {
                                    dialog.ShowDialog();
                                }

                                if (dialog.FileNames.Length != 5)
                                {
                                    continue;
                                }

                                DirectionTrackBar.Value = 0;
                                AddSelectedFiles(dialog, customConvert);
                            }

                            DirectionTrackBar.Enabled = true;
                        }
                    }
                }

                // 画布缩小后刷新列表
                _currentDir = DirectionTrackBar.Value;
                AfterSelectTreeView(null, null);
            }
            catch (OutOfMemoryException)
            {
                // TODO：添加日志记录或修复？
                // 忽略
            }
        }

        private void AddSelectedFiles(OpenFileDialog dialog, Color customConvert)
        {
            for (int w = 0; w < dialog.FileNames.Length; w++)
            {
                if (w >= 5)
                {
                    continue;
                }

                // dialog.Filename 替换为 dialog.FileNames[w]
                if (!System.IO.File.Exists(dialog.FileNames[w])) continue;

                using (Bitmap bmpTemp = new Bitmap(dialog.FileNames[w]))
                {
                    Bitmap bmp = new Bitmap(bmpTemp);

                    if (_fileType == 6)
                    {
                        AddAnimationX1Uop(customConvert, bmp);
                    }
                    else
                    {
                        AddAnimationX1(customConvert, bmp);
                    }
                }

                if ((w < 4) && (w < dialog.FileNames.Length - 1))
                {
                    DirectionTrackBar.Value++;
                    _currentDir = DirectionTrackBar.Value;
                }
            }
        }

        private void AddAnimationX1Uop(Color customConvert, Bitmap bmp)
        {
            var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
            if (uopAnim == null)
            {
                uopAnim = _uopManager.CreateNewUopAnimation(_currentBody, _currentAction, _currentDir);
            }

            FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
            int frameCount = bmp.GetFrameCount(dimension);
            ProgressBar.Maximum = frameCount;

            // 将帧提取到数组中
            Bitmap[] bitBmp = new Bitmap[frameCount];
            for (int index = 0; index < frameCount; index++)
            {
                bmp.SelectActiveFrame(dimension, index);
                bitBmp[index] = new Bitmap(bmp); // 克隆
            }

            // --- 画布算法（与 AddAnimationX1 逻辑重复）---
            int top = 0;
            int bottom = 0;
            int left = 0;
            int right = 0;

            int regressT = -1;
            int regressB = -1;
            int regressL = -1;
            int regressR = -1;

            bool var = true;
            bool breakOk = false;

            // 顶部
            for (int yf = 0; yf < bitBmp[0].Height; yf++)
            {
                for (int frameIdx = 0; frameIdx < frameCount; frameIdx++)
                {
                    for (int xf = 0; xf < bitBmp[0].Width; xf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (yf != 0)
                            {
                                regressT++;
                                yf--;
                                xf = -1;
                                frameIdx = 0;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressT == -1 &&
                            yf < bitBmp[0].Height - 9)
                        {
                            top += 10;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressT == -1 &&
                            yf >= bitBmp[0].Height - 9)
                        {
                            top++;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressT != -1)
                        {
                            top -= regressT;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk) break;
                }
                if (yf < bitBmp[0].Height - 9) yf += 9;
                if (breakOk) { breakOk = false; break; }
            }

            // 底部
            for (int yf = bitBmp[0].Height - 1; yf > 0; yf--)
            {
                for (int frameIdx = 0; frameIdx < frameCount; frameIdx++)
                {
                    for (int xf = 0; xf < bitBmp[0].Width; xf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0) var = true;

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (yf != bitBmp[0].Height - 1)
                            {
                                regressB++;
                                yf++;
                                xf = -1;
                                frameIdx = 0;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressB == -1 && yf > 9) bottom += 10;
                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressB == -1 && yf <= 9) bottom++;
                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressB != -1)
                        {
                            bottom -= regressB;
                            breakOk = true;
                            break;
                        }
                    }
                    if (breakOk) break;
                }
                if (yf > 9) yf -= 9;
                if (breakOk) { breakOk = false; break; }
            }

            // 左侧
            for (int xf = 0; xf < bitBmp[0].Width; xf++)
            {
                for (int frameIdx = 0; frameIdx < frameCount; frameIdx++)
                {
                    for (int yf = 0; yf < bitBmp[0].Height; yf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0) var = true;

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (xf != 0)
                            {
                                regressL++;
                                xf--;
                                yf = -1;
                                frameIdx = 0;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressL == -1 && xf < bitBmp[0].Width - 9) left += 10;
                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressL == -1 && xf >= bitBmp[0].Width - 9) left++;
                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressL != -1)
                        {
                            left -= regressL;
                            breakOk = true;
                            break;
                        }
                    }
                    if (breakOk) break;
                }
                if (xf < bitBmp[0].Width - 9) xf += 9;
                if (breakOk) { breakOk = false; break; }
            }

            // 右侧
            for (int xf = bitBmp[0].Width - 1; xf > 0; xf--)
            {
                for (int frameIdx = 0; frameIdx < frameCount; frameIdx++)
                {
                    for (int yf = 0; yf < bitBmp[0].Height; yf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0) var = true;

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (xf != bitBmp[0].Width - 1)
                            {
                                regressR++;
                                xf++;
                                yf = -1;
                                frameIdx = 0;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressR == -1 && xf > 9) right += 10;
                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressR == -1 && xf <= 9) right++;
                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressR != -1)
                        {
                            right -= regressR;
                            breakOk = true;
                            break;
                        }
                    }
                    if (breakOk) break;
                }
                if (xf > 9) xf -= 9;
                if (breakOk) { breakOk = false; break; }
            }

            for (int index = 0; index < frameCount; index++)
            {
                Rectangle rect = new Rectangle(left, top, bitBmp[index].Width - left - right, bitBmp[index].Height - top - bottom);
                if (rect.Width <= 0 || rect.Height <= 0) continue;
                bitBmp[index] = bitBmp[index].Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            // --- 画布算法结束 ---

            // 添加帧
            for (int index = 0; index < frameCount; index++)
            {
                var frame = new UoFiddler.Controls.Uop.DecodedUopFrame();
                frame.Image = new Bitmap(bitBmp[index]);
                frame.Header = new UoFiddler.Controls.Uop.UopFrameHeader
                {
                    Width = (ushort)frame.Image.Width,
                    Height = (ushort)frame.Image.Height,
                    CenterX = (short)(frame.Image.Width / 2),
                    CenterY = (short)(frame.Image.Height)
                };

                var paletteEntries = UoFiddler.Controls.Uop.VdExportHelper.GenerateProperPaletteFromImage(new UoFiddler.Controls.Models.Uop.Imaging.DirectBitmap(frame.Image));
                frame.Palette = paletteEntries.Select(p => Color.FromArgb(p.Alpha, p.R, p.G, p.B)).ToList();

                uopAnim.Frames.Add(frame);

                int newIndex = uopAnim.Frames.Count - 1;
                ListViewItem item = new ListViewItem(newIndex.ToString(), 0) { Tag = newIndex };
                FramesListView.Items.Add(item);

                // 更新 TileSize 以适合新图像
                int width = FramesListView.TileSize.Width - 5;
                if (frame.Image.Width > width) width = frame.Image.Width;
                int height = FramesListView.TileSize.Height - 5;
                if (frame.Image.Height > height) height = frame.Image.Height;
                FramesListView.TileSize = new Size(width + 5, height + 5);

                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }
            }
            uopAnim.IsModified = true;

            ProgressBar.Value = 0;
            ProgressBar.Invalidate();
            FramesListView.Invalidate();
            Options.ChangedUltimaClass["Animations"] = true;
        }

        private void ApplyTransparency(Bitmap bmp, Color keyColor)
        {
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* ptr = (byte*)bd.Scan0;
                int bytes = bd.Stride * bmp.Height;
                byte kR = keyColor.R;
                byte kG = keyColor.G;
                byte kB = keyColor.B;

                for (int i = 0; i < bytes; i += 4)
                {
                    // BGRA
                    byte b = ptr[i];
                    byte g = ptr[i + 1];
                    byte r = ptr[i + 2];

                    if (r == kR && g == kG && b == kB)
                    {
                        ptr[i + 3] = 0; // 透明
                    }
                    else
                    {
                        ptr[i + 3] = 255; // 不透明
                    }
                }
            }
            bmp.UnlockBits(bd);
        }

        private void AddAnimationX1(Color customConvert, Bitmap bmp)
        {
            AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            if (edit == null)
            {
                return;
            }

            FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);

            // 帧数
            int frameCount = bmp.GetFrameCount(dimension);
            ProgressBar.Maximum = frameCount;
            bmp.SelectActiveFrame(dimension, 0);
            UpdateGifPalette(bmp, edit);

            // 返回特定索引的图像
            Bitmap[] bitBmp = new Bitmap[frameCount];
            for (int index = 0; index < frameCount; index++)
            {
                bitBmp[index] = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
                bmp.SelectActiveFrame(dimension, index);
                bitBmp[index] = bmp;
            }

            // 画布算法
            int top = 0;
            int bottom = 0;
            int left = 0;
            int right = 0;

            int regressT = -1;
            int regressB = -1;
            int regressL = -1;
            int regressR = -1;

            bool var = true;
            bool breakOk = false;

            // 顶部
            for (int yf = 0; yf < bitBmp[0].Height; yf++)
            {
                for (int frameIdx = 0; frameIdx < frameCount; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int xf = 0; xf < bitBmp[0].Width; xf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (yf != 0)
                            {
                                regressT++;
                                yf--;
                                xf = -1;
                                frameIdx = 0;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressT == -1 &&
                            yf < bitBmp[0].Height - 9)
                        {
                            top += 10;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressT == -1 &&
                            yf >= bitBmp[0].Height - 9)
                        {
                            top++;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressT != -1)
                        {
                            top -= regressT;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (yf < bitBmp[0].Height - 9)
                {
                    yf += 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    breakOk = false;
                    break;
                }
            }

            // 底部
            for (int yf = bitBmp[0].Height - 1; yf > 0; yf--)
            {
                for (int frameIdx = 0; frameIdx < frameCount; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int xf = 0; xf < bitBmp[0].Width; xf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (yf != bitBmp[0].Height - 1)
                            {
                                regressB++;
                                yf++;
                                xf = -1;
                                frameIdx = 0;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressB == -1 &&
                            yf > 9)
                        {
                            bottom += 10;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressB == -1 &&
                            yf <= 9)
                        {
                            bottom++;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == frameCount - 1 && regressB != -1)
                        {
                            bottom -= regressB;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (yf > 9)
                {
                    yf -= 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    breakOk = false;
                    break;
                }
            }

            // 左侧
            for (int xf = 0; xf < bitBmp[0].Width; xf++)
            {
                for (int frameIdx = 0; frameIdx < frameCount; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int yf = 0; yf < bitBmp[0].Height; yf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (xf != 0)
                            {
                                regressL++;
                                xf--;
                                yf = -1;
                                frameIdx = 0;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressL == -1 &&
                            xf < bitBmp[0].Width - 9)
                        {
                            left += 10;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressL == -1 &&
                            xf >= bitBmp[0].Width - 9)
                        {
                            left++;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressL != -1)
                        {
                            left -= regressL;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (xf < bitBmp[0].Width - 9)
                {
                    xf += 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    breakOk = false;
                    break;
                }
            }

            // 右侧
            for (int xf = bitBmp[0].Width - 1; xf > 0; xf--)
            {
                for (int frameIdx = 0; frameIdx < frameCount; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int yf = 0; yf < bitBmp[0].Height; yf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (xf != bitBmp[0].Width - 1)
                            {
                                regressR++;
                                xf++;
                                yf = -1;
                                frameIdx = 0;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressR == -1 &&
                            xf > 9)
                        {
                            right += 10;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressR == -1 &&
                            xf <= 9)
                        {
                            right++;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == frameCount - 1 && regressR != -1)
                        {
                            right -= regressR;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (xf > 9)
                {
                    xf -= 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    //breakOk = false;
                    break;
                }
            }

            for (int index = 0; index < frameCount; index++)
            {
                Rectangle rect = new Rectangle(left, top, bitBmp[index].Width - left - right, bitBmp[index].Height - top - bottom);
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = bitBmp[index].Clone(rect, PixelFormat.Format16bppArgb1555);
            }

            // 画布算法结束

            for (int index = 0; index < frameCount; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnim(bitBmp[index], (int)numericUpDownRed.Value,
                    (int)numericUpDownGreen.Value, (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }
            }

            ProgressBar.Value = 0;
            ProgressBar.Invalidate();
            SetPaletteBox();
            FramesListView.Invalidate();

            _updatingUi = true;
            if (!_relativeMode)
            {
                CenterXNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.X;
                CenterYNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.Y;
            }
            else
            {
                CenterXNumericUpDown.Value = 0;
                CenterYNumericUpDown.Value = 0;
            }
            _updatingUi = false;

            Options.ChangedUltimaClass["Animations"] = true;
        }

        //带画布添加
        private void AddWithCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fileType == 6)
            {
                MessageBox.Show("UOP 格式不可用", "受限", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (_fileType != 0)
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Multiselect = false;
                        dialog.Title = "选择要添加的图像文件";
                        dialog.CheckFileExists = true;
                        dialog.Filter = "Gif 文件 (*.gif;)|*.gif;";
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            Color customConvert = Color.FromArgb(255, (int)numericUpDownRed.Value,
                                (int)numericUpDownGreen.Value, (int)numericUpDownBlue.Value);
                            //我的 Soulblighter 修改
                            for (int w = 0; w < dialog.FileNames.Length; w++)
                            {
                                // dialog.Filename 替换为 dialog.FileNames[w]
                                Bitmap bmp = new Bitmap(dialog.FileNames[w]);

                                // TODO：修复检查文件扩展名
                                // Gif 特殊属性
                                if (!dialog.FileNames[w].Contains(".gif"))
                                {
                                    continue;
                                }

                                AddAnimationX1(customConvert, bmp);
                            }
                        }
                    }
                }

                // 画布缩小后刷新列表
                _currentDir = DirectionTrackBar.Value;
                AfterSelectTreeView(null, null);
            }
            catch (OutOfMemoryException)
            {
                // TODO：添加日志记录或修复？
                // 忽略
            }
        }

        private void OnClickGeneratePalette(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Title = "选择要从中生成的图像";
                dialog.CheckFileExists = true;
                dialog.Filter = "图像文件 (*.tif;*.tiff;*.bmp;*.png;*.jpg;*.jpeg)|*.tif;*.tiff;*.bmp;*.png;*.jpg;*.jpeg";
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                foreach (string filename in dialog.FileNames)
                {
                    Bitmap bit = new Bitmap(filename);
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                    if (edit != null)
                    {
                        bit = ConvertBmpAnim(bit, (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value, (int)numericUpDownBlue.Value);
                        UpdateImagePalette(bit, edit);
                    }
                    SetPaletteBox();
                    FramesListView.Invalidate();
                    Options.ChangedUltimaClass["Animations"] = true;
                    SetPaletteBox();
                }
            }
        }
        //Soulblighter 修改结束

        private static unsafe Bitmap ConvertBmpAnim(Bitmap bmp, int red, int green, int blue)
        {
            //额外背景
            int extraBack = (red / 8 * 1024) + (green / 8 * 32) + (blue / 8) + 32768;

            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
            BitmapData bdNew = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);

            ushort* lineNew = (ushort*)bdNew.Scan0;
            int deltaNew = bdNew.Stride >> 1;

            for (int y = 0; y < bmp.Height; ++y, line += delta, lineNew += deltaNew)
            {
                ushort* cur = line;
                ushort* curNew = lineNew;
                for (int x = 0; x < bmp.Width; ++x)
                {
                    //我的 Soulblighter 修改
                    // 将颜色 0,0,0 转换为 0,0,8
                    if (cur[x] == 32768)
                    {
                        curNew[x] = 32769;
                    }

                    if (cur[x] != 65535 && cur[x] != extraBack && cur[x] > 32768) //真白 == 背景
                    {
                        curNew[x] = cur[x];
                    }
                    //Soulblighter 修改结束
                }
            }
            bmp.UnlockBits(bd);
            bmpNew.UnlockBits(bdNew);
            return bmpNew;
        }

        private void OnClickExportAllToVD(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                return;
            }

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择目录";
                dialog.ShowNewFolderButton = true;
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                for (int i = 0; i < AnimationListTreeView.Nodes.Count; ++i)
                {
                    int index = (int)AnimationListTreeView.Nodes[i].Tag;
                    if (index < 0 || AnimationListTreeView.Nodes[i].Parent != null ||
                        AnimationListTreeView.Nodes[i].ForeColor == Color.Red)
                    {
                        continue;
                    }

                    string fileName = Path.Combine(dialog.SelectedPath, $"anim{_fileType}_0x{index:X}.vd");
                    AnimationEdit.ExportToVD(_fileType, index, fileName);
                }

                MessageBox.Show($"所有动画已保存到 {dialog.SelectedPath}",
                    "导出", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void CbSaveCoordinates_CheckedChanged(object sender, EventArgs e)
        {
            // 获取数组中所有动画的位置
            if (SaveCoordinatesCheckBox.Checked)
            {
                DirectionTrackBar.Enabled = false;
                FramesTrackBar.Value = 0;
                SetCoordinatesButton.Enabled = true;
                for (int count = 0; count < 5;)
                {
                    if (DirectionTrackBar.Value < 4)
                    {
                        _animCx[DirectionTrackBar.Value] = (int)CenterXNumericUpDown.Value;
                        _animCy[DirectionTrackBar.Value] = (int)CenterYNumericUpDown.Value;
                        DirectionTrackBar.Value++;
                        count++;
                    }
                    else
                    {
                        _animCx[DirectionTrackBar.Value] = (int)CenterXNumericUpDown.Value;
                        _animCy[DirectionTrackBar.Value] = (int)CenterYNumericUpDown.Value;
                        DirectionTrackBar.Value = 0;
                        count++;
                    }
                }

                SaveCoordinatesLabel1.Text = $"1: {_animCx[0]}/{_animCy[0]}";
                SaveCoordinatesLabel2.Text = $"2: {_animCx[1]}/{_animCy[1]}";
                SaveCoordinatesLabel3.Text = $"3: {_animCx[2]}/{_animCy[2]}";
                SaveCoordinatesLabel4.Text = $"4: {_animCx[3]}/{_animCy[3]}";
                SaveCoordinatesLabel5.Text = $"5: {_animCx[4]}/{_animCy[4]}";

                DirectionTrackBar.Enabled = true;
            }
            else
            {
                SaveCoordinatesLabel1.Text = "1:    /    ";
                SaveCoordinatesLabel2.Text = "2:    /    ";
                SaveCoordinatesLabel3.Text = "3:    /    ";
                SaveCoordinatesLabel4.Text = "4:    /    ";
                SaveCoordinatesLabel5.Text = "5:    /    ";
                SetCoordinatesButton.Enabled = false;
            }
        }

        private void SetButton_Click(object sender, EventArgs e)
        {
            int originalDir = DirectionTrackBar.Value;
            DirectionTrackBar.Enabled = false;

            int max = DirectionTrackBar.Maximum;
            for (int i = 0; i <= max; i++)
            {
                // 为迭代设置当前方向
                DirectionTrackBar.Value = i;
                _currentDir = i;

                try
                {
                    if (_fileType == 0)
                    {
                        continue;
                    }

                    if (_fileType == UOP_FILE_TYPE)
                    {
                        // UOP：使用 UOP 管理器及其帧结构
                        if (_uopManager == null) continue;

                        var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                        if (uopAnim == null || uopAnim.Frames == null || uopAnim.Frames.Count == 0) continue;

                        for (int fi = 0; fi < uopAnim.Frames.Count; fi++)
                        {
                            uopAnim.ChangeCenter(fi, _animCx[i], _animCy[i]);
                        }

                        uopAnim.IsModified = true;
                        Options.ChangedUltimaClass["Animations"] = true;
                        AnimationPictureBox.Invalidate();
                    }
                    else
                    {
                        // MUL / legacy：保护对 Frames 的访问
                        AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                        if (edit == null || edit.Frames == null) continue;
                        if (FramesTrackBar.Value >= edit.Frames.Count) continue;

                        foreach (var editFrame in edit.Frames)
                        {
                            editFrame.ChangeCenter(_animCx[i], _animCy[i]);
                        }

                        Options.ChangedUltimaClass["Animations"] = true;
                        AnimationPictureBox.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"SetButton_Click error (dir={i}): {ex.Message}");
                }
            }

            // 恢复初始方向
            DirectionTrackBar.Value = originalDir;
            _currentDir = originalDir;
            DirectionTrackBar.Enabled = true;
        }

        // Gemini 添加的方法

        private void OnZoomChanged(object sender, EventArgs e)
        {
            if (ZoomNumericUpDown.SelectedItem == null) return;
            string val = ZoomNumericUpDown.SelectedItem.ToString().Replace("%", "");
            if (float.TryParse(val, out float result))
            {
                _zoomFactor = result / 100.0f;
                AnimationPictureBox.Invalidate();
            }
        }

        private void RelativeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _relativeMode = RelativeCheckBox.Checked;
            if (_relativeMode)
            {
                // 将 NUD 重置为 0
                _accumulatedRelativeX = 0;
                _accumulatedRelativeY = 0;
                _lastRelativeX = 0;
                _lastRelativeY = 0;
                _updatingUi = true;
                CenterXNumericUpDown.Value = 0;
                CenterYNumericUpDown.Value = 0;
                _updatingUi = false;
            }
            else
            {
                // 取消选中时，通常我们希望看到当前的绝对值。
                // 触发刷新
                AfterSelectTreeView(null, null);
            }
        }

        private void CheckBoxAction_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxAction.Checked)
            {
                CheckBoxAllAction.Checked = false;
                CheckBoxAllAction.Enabled = false;
                SameCenterButton.Enabled = false;
            }
            else
            {
                CheckBoxAllAction.Enabled = true;
                SameCenterButton.Enabled = true;
            }
        }

        private void CheckBoxAllAction_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxAllAction.Checked)
            {
                CheckBoxAction.Checked = false;
                CheckBoxAction.Enabled = false;
                SameCenterButton.Enabled = false;
            }
            else
            {
                CheckBoxAction.Enabled = true;
                SameCenterButton.Enabled = true;
            }
        }

        private void SecondAnimCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _secondAnimActivated = SecondAnimCheckBox.Checked;
            AnimationPictureBox.Invalidate();
        }

        private void SecondAnimFileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SecondAnimFileComboBox.SelectedItem == null) return;
            LoadSecondAnimationFile(SecondAnimFileComboBox.SelectedItem.ToString());
            AnimationPictureBox.Invalidate();
        }

        private void SecondAnimIdNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _secondAnimID = (int)SecondAnimIdNumericUpDown.Value;
            AnimationPictureBox.Invalidate();
        }

        private void SecondAnimPseudoVisuCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _secondAnimPseudoVisu = SecondAnimPseudoVisuCheckBox.Checked;
            AnimationPictureBox.Invalidate();
        }

        private void SecondAnimFirstPlanCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _isSecondAnimInFront = SecondAnimFirstPlanCheckBox.Checked;
            AnimationPictureBox.Invalidate();
        }

        private void ZoomNumericUpDown_Click(object sender, EventArgs e)
        {
            // 如果配置为按钮，可能会触发此事件，但我们使用 OnZoomChanged 处理 ComboBox
        }

        private void LoadSecondAnimationFile(string fileName)
        {
            string root = Files.RootDir;

            // 检查 UOP
            if (fileName.EndsWith(".uop", StringComparison.OrdinalIgnoreCase))
            {
                string fullPath = System.IO.Path.Combine(root, fileName);
                if (System.IO.File.Exists(fullPath))
                {
                    _secondUopManager = new UopAnimationDataManager();
                    _secondUopManager.LoadUopFiles();
                    _secondUopManager.ProcessUopData();
                    _secondAnimFileIndex = 6; // UOP 类型
                }
                return;
            }

            // 处理 MUL（组合框项目是“anim”、“anim2”等，没有扩展名）
            _secondUopManager = null;
            if (fileName.Equals("anim", StringComparison.OrdinalIgnoreCase))
            {
                _secondAnimFileIndex = 1;
            }
            else if (fileName.StartsWith("anim", StringComparison.OrdinalIgnoreCase))
            {
                // anim2, anim3...
                string numPart = fileName.Substring(4);
                if (int.TryParse(numPart, out int idx))
                {
                    _secondAnimFileIndex = idx;
                }
            }
        }

        private int GetAnimationType(int body, int fileType)
        {
            if (fileType == 6) return 3; // UOP 生物映射
            int length = Animations.GetAnimLength(body, fileType);
            if (length == 13) return 0; // 动物
            if (length == 22) return 1; // 怪物
            if (length == 35) return 2; // 人类
            return 1;
        }

        private int MapAction(int action, int sourceType, int targetType)
        {
            // 强制重映射：人类（2）<-> UOP 生物（3）

            // 情况 1：主动画是人类（MUL），第二个动画是 UOP 生物
            if (sourceType == 2 && targetType == 3)
            {
                if (action == 23) return 29; // Horse_Walk_01 -> MountWalk
                if (action == 24) return 30; // Horse_Run_01 -> MountRun
                if (action == 25) return 31; // Horse_Idle_01 -> MountIdle
            }

            // 情况 2：主动画是 UOP 生物，第二个动画是人类（MUL）
            if (sourceType == 3 && targetType == 2)
            {
                // 映射优先级 1
                if (action == 29) return 23; // MountWalk -> Horse_Walk_01
                if (action == 30) return 24; // MountRun -> Horse_Run_01
                if (action == 31) return 25; // MountIdle -> Horse_Idle_01

                // 映射优先级 2（后备请求）
                if (action == 22) return 23; // BlockLeft -> Horse_Walk_01
                if (action == 24) return 24; // TakeOff -> Horse_Run_01
                if (action == 25) return 25; // GetHitInAir -> Horse_Idle_01
            }

            if (sourceType == targetType) return action;
            if (sourceType < 0 || sourceType >= _animNames.Length) return action;
            if (targetType < 0 || targetType >= _animNames.Length) return action;
            if (action < 0 || action >= _animNames[sourceType].Length) return 0;

            string actionName = _animNames[sourceType][action];
            for (int i = 0; i < _animNames[targetType].Length; i++)
            {
                if (_animNames[targetType][i].Equals(actionName, StringComparison.OrdinalIgnoreCase)) return i;
            }
            return 0;
        }


        private void DrawSecondAnimation(Graphics graphics)
        {
            if (!_secondAnimActivated) return;

            int mainAnimType = GetAnimationType(_currentBody, _fileType);
            int secondAnimType = GetAnimationType(_secondAnimID, _secondAnimFileIndex);

            int mappedAction = MapAction(_currentAction, mainAnimType, secondAnimType);

            Bitmap frame = null;
            int centerX = 0;
            int centerY = 0;

            if (_secondAnimFileIndex == 6 && _secondUopManager != null)
            {
                // UOP
                var uopAnim = _secondUopManager.GetUopAnimation(_secondAnimID, mappedAction, _currentDir);
                if (uopAnim != null && uopAnim.Frames.Count > 0)
                {
                    int index = FramesTrackBar.Value;
                    if (index >= uopAnim.Frames.Count) index = 0; // 循环或钳位？
                    var f = uopAnim.Frames[index];
                    frame = f.Image;
                    centerX = f.Header.CenterX;
                    centerY = f.Header.CenterY;
                }
            }
            else
            {
                // MUL
                // 使用 _secondAnimFileIndex 调用 AnimationEdit.GetAnimation
                AnimIdx edit = AnimationEdit.GetAnimation(_secondAnimFileIndex, _secondAnimID, mappedAction, _currentDir);
                if (edit != null)
                {
                    Bitmap[] frames = edit.GetFrames();
                    if (frames != null && frames.Length > 0)
                    {
                        int index = FramesTrackBar.Value;
                        if (index >= frames.Length) index = 0;
                        frame = frames[index];
                        if (frame != null)
                        {
                            centerX = edit.Frames[index].Center.X;
                            centerY = edit.Frames[index].Center.Y;
                        }
                    }
                }
            }

            if (frame != null)
            {
                int x = _framePoint.X - (int)(centerX * _zoomFactor);
                int y = _framePoint.Y - (int)(centerY * _zoomFactor) - (int)(frame.Height * _zoomFactor);

                if (_secondAnimPseudoVisu)
                {
                    // 绘制彩色叠加层（半透明）
                    using (var attr = new ImageAttributes())
                    {
                        ColorMatrix matrix = new ColorMatrix(new float[][]{
                                new float[] {0, 0, 0, 0, 0},
                                new float[] {0, 1, 0, 0, 0}, // 绿色
                                new float[] {0, 0, 0, 0, 0},
                                new float[] {0, 0, 0, 0.5f, 0}, // Alpha 0.5
                                new float[] {0, 0, 0, 0, 1}
                            });
                        attr.SetColorMatrix(matrix);
                        graphics.DrawImage(frame, new Rectangle(x, y, (int)(frame.Width * _zoomFactor), (int)(frame.Height * _zoomFactor)),
                            0, 0, frame.Width, frame.Height, GraphicsUnit.Pixel, attr);
                    }
                }
                else
                {
                    graphics.DrawImage(frame, new Rectangle(x, y, (int)(frame.Width * _zoomFactor), (int)(frame.Height * _zoomFactor)));
                }
            }
        }

        // 带画布添加方向（CV5 样式 GIF）
        private void AddDirectionsAddWithCanvasUniqueImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fileType == 6)
            {
                MessageBox.Show("UOP 格式不可用", "受限", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (_fileType != 0)
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Multiselect = false;
                        dialog.Title = "选择 1 个 GIF（包含 CV5 样式的所有方向）添加";
                        dialog.CheckFileExists = true;
                        dialog.Filter = "Gif 文件 (*.gif;)|*.gif;";

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            Color customConvert = Color.FromArgb(255, (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value, (int)numericUpDownBlue.Value);

                            DirectionTrackBar.Enabled = false;
                            DirectionTrackBar.Value = 0;

                            AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);

                            if (edit != null)
                            {
                                // Gif 特殊属性
                                if (dialog.FileName.Contains(".gif"))
                                {
                                    using (Bitmap bmpTemp = new Bitmap(dialog.FileName))
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        bmpTemp.Save(ms, ImageFormat.Gif);
                                        Bitmap bitmap = new Bitmap(ms);

                                        FrameDimension dimension = new FrameDimension(bitmap.FrameDimensionsList[0]);

                                        // 帧数
                                        int frameCount = bitmap.GetFrameCount(dimension);

                                        ProgressBar.Maximum = frameCount;

                                        bitmap.SelectActiveFrame(dimension, 0);

                                        UpdateGifPalette(bitmap, edit);

                                        Bitmap[] bitBmp = new Bitmap[frameCount];

                                        // 返回特定索引的图像
                                        for (int index = 0; index < frameCount; index++)
                                        {
                                            bitBmp[index] = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format16bppArgb1555);
                                            bitmap.SelectActiveFrame(dimension, index);
                                            bitBmp[index] = bitmap;
                                        }

                                        Cv5CanvasAlgorithm(bitBmp, frameCount, dimension, customConvert);

                                        edit = Cv5AnimIdxPositions(frameCount, bitBmp, dimension, edit, bitmap);

                                        ProgressBar.Value = 0;
                                        ProgressBar.Invalidate();

                                        SetPaletteBox();
                                        FramesListView.Invalidate();

                                        CenterXNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.X;
                                        CenterYNumericUpDown.Value = edit.Frames[FramesTrackBar.Value].Center.Y;

                                        Options.ChangedUltimaClass["Animations"] = true;
                                    }
                                }
                            }

                            DirectionTrackBar.Enabled = true;
                        }
                    }
                }

                // 画布缩小后刷新列表
                _currentDir = DirectionTrackBar.Value;
                AfterSelectTreeView(null, null);
            }
            catch (NullReferenceException)
            {
                // TODO：添加日志记录或修复？
                DirectionTrackBar.Enabled = true;
            }
        }

        private AnimIdx Cv5AnimIdxPositions(int frameCount, Bitmap[] bitBmp, FrameDimension dimension, AnimIdx edit, Bitmap bmp)
        {
            // 位置 0
            for (int index = frameCount / 8 * 4; index < frameCount / 8 * 5; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimCv5(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }

                if (index == (frameCount / 8 * 5) - 1)
                {
                    DirectionTrackBar.Value++;
                }
            }

            // 位置 1
            edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            bmp.SelectActiveFrame(dimension, 0);
            UpdateGifPalette(bmp, edit);
            for (int index = 0; index < frameCount / 8; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimCv5(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                ListViewItem item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }

                if (index == (frameCount / 8) - 1)
                {
                    DirectionTrackBar.Value++;
                }
            }

            // 位置 2
            edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            bmp.SelectActiveFrame(dimension, 0);
            UpdateGifPalette(bmp, edit);
            for (int index = frameCount / 8 * 5; index < frameCount / 8 * 6; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimCv5(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }

                if (index == (frameCount / 8 * 6) - 1)
                {
                    DirectionTrackBar.Value++;
                }
            }

            // 位置 3
            edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            bmp.SelectActiveFrame(dimension, 0);
            UpdateGifPalette(bmp, edit);
            for (int index = frameCount / 8 * 1; index < frameCount / 8 * 2; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimCv5(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }

                if (index == (frameCount / 8 * 2) - 1)
                {
                    DirectionTrackBar.Value++;
                }
            }

            // 位置 4
            edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            bmp.SelectActiveFrame(dimension, 0);
            UpdateGifPalette(bmp, edit);
            for (int index = frameCount / 8 * 6; index < frameCount / 8 * 7; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimCv5(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }
            }

            return edit;
        }

        private static unsafe Bitmap ConvertBmpAnimCv5(Bitmap bmp, int red, int green, int blue)
        {
            //额外背景
            int extraBack = (red / 8 * 1024) + (green / 8 * 32) + (blue / 8) + 32768;

            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
            BitmapData bdNew = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);

            ushort* lineNew = (ushort*)bdNew.Scan0;
            int deltaNew = bdNew.Stride >> 1;

            for (int y = 0; y < bmp.Height; ++y, line += delta, lineNew += deltaNew)
            {
                ushort* cur = line;
                ushort* curNew = lineNew;
                for (int x = 0; x < bmp.Width; ++x)
                {
                    //我的 Soulblighter 修改
                    // 将颜色 0,0,0 转换为 0,0,8
                    if (cur[x] == 32768)
                    {
                        curNew[x] = 32769;
                    }

                    if (cur[x] != 65535 && cur[x] != 54965 && cur[x] != extraBack && cur[x] > 32768) //真白 == 背景
                    {
                        curNew[x] = cur[x];
                    }
                    //Soulblighter 修改结束
                }
            }
            bmp.UnlockBits(bd);
            bmpNew.UnlockBits(bdNew);
            return bmpNew;
        }

        private static readonly Color _greyConvert = Color.FromArgb(255, 170, 170, 170);

        private static void Cv5CanvasAlgorithm(Bitmap[] bitBmp, int frameCount, FrameDimension dimension, Color customConvert)
        {
            // TODO：需要更好的名称
            // TODO：此代码需要文档。此算法不易阅读

            // 调用顺序很重要
            // 看起来它对于来自 Diablo cv5 格式的 Gif/bmp 很重要
            // 关于 Diablo 格式的一些资料：
            // - https://d2mods.info/resources/infinitum/tut_files/dcc_tutorial/
            // - https://d2mods.info/resources/infinitum/tut_files/dcc_tutorial/chapter4.html
            //
            const int frameDivider = 8;

            // 位置 0
            Cv5ProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 4), GetMaximumFrameIndex(frameCount, frameDivider, 4));
            // 位置 1
            Cv5ProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 0), GetMaximumFrameIndex(frameCount, frameDivider, 0));
            // 位置 2
            Cv5ProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 5), GetMaximumFrameIndex(frameCount, frameDivider, 5));
            // 位置 3
            Cv5ProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 1), GetMaximumFrameIndex(frameCount, frameDivider, 1));
            // 位置 4
            Cv5ProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 6), GetMaximumFrameIndex(frameCount, frameDivider, 6));
        }

        private static int GetInitialFrameIndex(int frameCount, int frameDivider, int position)
        {
            return frameCount / frameDivider * position;
        }

        private static int GetMaximumFrameIndex(int frameCount, int frameDivider, int position)
        {
            return frameCount / frameDivider * (position + 1);
        }

        private static void Cv5ProcessFrames(Bitmap[] bitBmp, FrameDimension dimension, Color customConvert, int initialFrameIndex, int maximumFrameIndex)
        {
            int top = 0;
            int bottom = 0;
            int left = 0;
            int right = 0;

            int regressT = -1;
            int regressB = -1;
            int regressL = -1;
            int regressR = -1;

            bool var = true;
            bool breakOk = false;

            // 顶部
            for (int yf = 0; yf < bitBmp[initialFrameIndex].Height; yf++)
            {
                for (int frameIdx = initialFrameIndex; frameIdx < maximumFrameIndex; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int xf = 0; xf < bitBmp[initialFrameIndex].Width; xf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == _greyConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != _greyConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (yf != 0)
                            {
                                regressT++;
                                yf--;
                                xf = -1;
                                frameIdx = initialFrameIndex;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressT == -1 &&
                            yf < bitBmp[0].Height - 9)
                        {
                            top += 10;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressT == -1 &&
                            yf >= bitBmp[0].Height - 9)
                        {
                            top++;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressT != -1)
                        {
                            top -= regressT;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (yf < bitBmp[initialFrameIndex].Height - 9)
                {
                    yf += 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    breakOk = false;
                    break;
                }
            }

            // 底部
            for (int yf = bitBmp[initialFrameIndex].Height - 1; yf > 0; yf--)
            {
                for (int frameIdx = initialFrameIndex; frameIdx < maximumFrameIndex; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int xf = 0; xf < bitBmp[initialFrameIndex].Width; xf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == _greyConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != _greyConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (yf != bitBmp[initialFrameIndex].Height - 1)
                            {
                                regressB++;
                                yf++;
                                xf = -1;
                                frameIdx = initialFrameIndex;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressB == -1 &&
                            yf > 9)
                        {
                            bottom += 10;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressB == -1 &&
                            yf <= 9)
                        {
                            bottom++;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressB != -1)
                        {
                            bottom -= regressB;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (yf > 9)
                {
                    yf -= 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    breakOk = false;
                    break;
                }
            }

            // 左侧
            for (int xf = 0; xf < bitBmp[initialFrameIndex].Width; xf++)
            {
                for (int frameIdx = initialFrameIndex; frameIdx < maximumFrameIndex; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int yf = 0; yf < bitBmp[initialFrameIndex].Height; yf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == _greyConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != _greyConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (xf != 0)
                            {
                                regressL++;
                                xf--;
                                yf = -1;
                                frameIdx = initialFrameIndex;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 &&
                            regressL == -1 &&
                            xf < bitBmp[0].Width - 9)
                        {
                            left += 10;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 &&
                            regressL == -1 &&
                            xf >= bitBmp[0].Width - 9)
                        {
                            left++;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 && regressL != -1)
                        {
                            left -= regressL;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (xf < bitBmp[initialFrameIndex].Width - 9)
                {
                    xf += 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    breakOk = false;
                    break;
                }
            }

            // 右侧
            for (int xf = bitBmp[initialFrameIndex].Width - 1; xf > 0; xf--)
            {
                for (int frameIdx = initialFrameIndex; frameIdx < maximumFrameIndex; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int yf = 0; yf < bitBmp[initialFrameIndex].Height; yf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == _greyConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != _greyConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (xf != bitBmp[initialFrameIndex].Width - 1)
                            {
                                regressR++;
                                xf++;
                                yf = -1;
                                frameIdx = initialFrameIndex;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 &&
                            regressR == -1 &&
                            xf > 9)
                        {
                            right += 10;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 &&
                            regressR == -1 &&
                            xf <= 9)
                        {
                            right++;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 && regressR != -1)
                        {
                            right -= regressR;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (xf > 9)
                {
                    xf -= 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    //breakOk = false;
                    break;
                }
            }

            for (int index = initialFrameIndex; index < maximumFrameIndex; index++)
            {
                Rectangle rect = new Rectangle(left, top, bitBmp[index].Width - left - right, bitBmp[index].Height - top - bottom);
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = bitBmp[index].Clone(rect, PixelFormat.Format16bppArgb1555);
            }
        }

        private void CbLockColorControls_CheckedChanged(object sender, EventArgs e)
        {
            if (!LockColorControlsCheckBox.Checked)
            {
                numericUpDownRed.Enabled = true;
                numericUpDownGreen.Enabled = true;
                numericUpDownBlue.Enabled = true;
            }
            else
            {
                numericUpDownRed.Enabled = false;
                numericUpDownGreen.Enabled = false;
                numericUpDownBlue.Enabled = false;

                numericUpDownRed.Value = 255;
                numericUpDownGreen.Value = 255;
                numericUpDownBlue.Value = 255;
            }
        }

        // 所有方向添加 KRFrameViewer
        private void AllDirectionsAddWithCanvasKRFrameEditorColorCorrectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fileType == 6)
            {
                MessageBox.Show("UOP 格式不可用", "受限", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_fileType == 0)
            {
                return;
            }
        }

        private AnimIdx KrAnimIdxPositions(int frameCount, Bitmap[] bitBmp, FrameDimension dimension, AnimIdx edit, Bitmap bmp)
        {
            // 位置 0
            for (int index = frameCount / 5 * 0; index < frameCount / 5 * 1; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimKr(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }

                if (index == (frameCount / 5 * 1) - 1)
                {
                    DirectionTrackBar.Value++;
                }
            }

            // 位置 1
            edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            bmp.SelectActiveFrame(dimension, 0);
            UpdateGifPalette(bmp, edit);
            for (int index = frameCount / 5 * 1; index < frameCount / 5 * 2; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimKr(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }

                if (index == (frameCount / 5 * 2) - 1)
                {
                    DirectionTrackBar.Value++;
                }
            }

            // 位置 2
            edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            bmp.SelectActiveFrame(dimension, 0);
            UpdateGifPalette(bmp, edit);
            for (int index = frameCount / 5 * 2; index < frameCount / 5 * 3; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimKr(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }

                if (index == (frameCount / 5 * 3) - 1)
                {
                    DirectionTrackBar.Value++;
                }
            }

            // 位置 3
            edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            bmp.SelectActiveFrame(dimension, 0);
            UpdateGifPalette(bmp, edit);
            for (int index = frameCount / 5 * 3; index < frameCount / 5 * 4; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimKr(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }

                if (index == (frameCount / 5 * 4) - 1)
                {
                    DirectionTrackBar.Value++;
                }
            }

            // 位置 4
            edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            bmp.SelectActiveFrame(dimension, 0);
            UpdateGifPalette(bmp, edit);
            for (int index = frameCount / 5 * 4; index < frameCount / 5 * 5; index++)
            {
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = ConvertBmpAnimKr(bitBmp[index], (int)numericUpDownRed.Value, (int)numericUpDownGreen.Value,
                    (int)numericUpDownBlue.Value);
                edit.AddFrame(bitBmp[index], bitBmp[index].Width / 2);
                TreeNode node = GetNode(_currentBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    node.Nodes[_currentAction].ForeColor = Color.Black;
                }

                int i = edit.Frames.Count - 1;
                var item = new ListViewItem(i.ToString(), 0)
                {
                    Tag = i
                };
                FramesListView.Items.Add(item);
                int width = FramesListView.TileSize.Width - 5;
                if (bmp.Width > FramesListView.TileSize.Width)
                {
                    width = bmp.Width;
                }

                int height = FramesListView.TileSize.Height - 5;
                if (bmp.Height > FramesListView.TileSize.Height)
                {
                    height = bmp.Height;
                }

                FramesListView.TileSize = new Size(width + 5, height + 5);
                FramesTrackBar.Maximum = i;
                Options.ChangedUltimaClass["Animations"] = true;
                if (ProgressBar.Value < ProgressBar.Maximum)
                {
                    ProgressBar.Value++;
                    ProgressBar.Invalidate();
                }
            }

            return edit;
        }

        private static unsafe Bitmap ConvertBmpAnimKr(Bitmap bmp, int red, int green, int blue)
        {
            // 额外背景
            int extraBack = (red / 8 * 1024) + (green / 8 * 32) + (blue / 8) + 32768;

            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            ushort* line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppArgb1555);
            BitmapData bdNew = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);

            ushort* lineNew = (ushort*)bdNew.Scan0;
            int deltaNew = bdNew.Stride >> 1;

            for (int y = 0; y < bmp.Height; ++y, line += delta, lineNew += deltaNew)
            {
                ushort* cur = line;
                ushort* curNew = lineNew;
                for (int x = 0; x < bmp.Width; ++x)
                {
                    //if (cur[X] != 53235)
                    //{
                    // 转换回 RGB
                    int blueTemp = (cur[x] - 32768) / 32;
                    blueTemp *= 32;
                    blueTemp = cur[x] - 32768 - blueTemp;

                    int greenTemp = (cur[x] - 32768) / 1024;
                    greenTemp *= 1024;
                    greenTemp = cur[x] - 32768 - greenTemp - blueTemp;
                    greenTemp /= 32;

                    int redTemp = (cur[x] - 32768) / 1024;

                    // 移除绿色
                    if (greenTemp > blueTemp && greenTemp > redTemp && greenTemp > 10)
                    {
                        cur[x] = 65535;
                    }
                    //}

                    //我的 Soulblighter 修改
                    // 将颜色 0,0,0 转换为 0,0,8
                    if (cur[x] == 32768)
                    {
                        curNew[x] = 32769;
                    }

                    if (cur[x] != 65535 && cur[x] != 54965 && cur[x] != extraBack && cur[x] > 32768) //真白 == 背景
                    {
                        curNew[x] = cur[x];
                    }
                    //Soulblighter 修改结束
                }
            }
            bmp.UnlockBits(bd);
            bmpNew.UnlockBits(bdNew);
            return bmpNew;
        }

        private static void KrCanvasAlgorithm(Bitmap[] bitBmp, int frameCount, FrameDimension dimension, Color customConvert)
        {
            /*
             * TODO：两种方法都需要更好的名称。
             *
             *      这里的重复很大。现在已简化为带参数的一个方法。
             *      仍然需要进一步简化。
             *      可能可以将代码与 CV5 例程合并。
             */
            // TODO：需要更好的名称
            // TODO：此代码需要文档。此算法不易阅读

            // 调用顺序很重要
            // 看起来它对于来自 KR 客户端的 Gif/bmp 很重要
            const int frameDivider = 5;

            KrProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 0), GetMaximumFrameIndex(frameCount, frameDivider, 0));
            KrProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 1), GetMaximumFrameIndex(frameCount, frameDivider, 1));
            KrProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 2), GetMaximumFrameIndex(frameCount, frameDivider, 2));
            KrProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 3), GetMaximumFrameIndex(frameCount, frameDivider, 3));
            KrProcessFrames(bitBmp, dimension, customConvert, GetInitialFrameIndex(frameCount, frameDivider, 4), GetMaximumFrameIndex(frameCount, frameDivider, 4));
        }

        private static void KrProcessFrames(Bitmap[] bitBmp, FrameDimension dimension, Color customConvert, int initialFrameIndex, int maximumFrameIndex)
        {
            int top = 0;
            int bottom = 0;
            int left = 0;
            int right = 0;

            int regressT = -1;
            int regressB = -1;
            int regressL = -1;
            int regressR = -1;

            bool var = true;
            bool breakOk = false;

            // 顶部
            for (int yf = 0; yf < bitBmp[initialFrameIndex].Height; yf++)
            {
                for (int frameIdx = initialFrameIndex; frameIdx < maximumFrameIndex; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int xf = 0; xf < bitBmp[initialFrameIndex].Width; xf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (yf != 0)
                            {
                                regressT++;
                                yf--;
                                xf = -1;
                                frameIdx = initialFrameIndex;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressT == -1 &&
                            yf < bitBmp[0].Height - 9)
                        {
                            top += 10;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressT == -1 &&
                            yf >= bitBmp[0].Height - 9)
                        {
                            top++;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressT != -1)
                        {
                            top -= regressT;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (yf < bitBmp[initialFrameIndex].Height - 9)
                {
                    yf += 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    breakOk = false;
                    break;
                }
            }

            // 底部
            for (int yf = bitBmp[initialFrameIndex].Height - 1; yf > 0; yf--)
            {
                for (int frameIdx = initialFrameIndex; frameIdx < maximumFrameIndex; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int xf = 0; xf < bitBmp[initialFrameIndex].Width; xf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (yf != bitBmp[initialFrameIndex].Height - 1)
                            {
                                regressB++;
                                yf++;
                                xf = -1;
                                frameIdx = initialFrameIndex;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressB == -1 &&
                            yf > 9)
                        {
                            bottom += 10;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressB == -1 &&
                            yf <= 9)
                        {
                            bottom++;
                        }

                        if (var && xf == bitBmp[frameIdx].Width - 1 && frameIdx == maximumFrameIndex - 1 && regressB != -1)
                        {
                            bottom -= regressB;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (yf > 9)
                {
                    yf -= 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    breakOk = false;
                    break;
                }
            }

            // 左侧
            for (int xf = 0; xf < bitBmp[initialFrameIndex].Width; xf++)
            {
                for (int frameIdx = initialFrameIndex; frameIdx < maximumFrameIndex; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int yf = 0; yf < bitBmp[initialFrameIndex].Height; yf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (xf != 0)
                            {
                                regressL++;
                                xf--;
                                yf = -1;
                                frameIdx = initialFrameIndex;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 &&
                            regressL == -1 &&
                            xf < bitBmp[0].Width - 9)
                        {
                            left += 10;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 &&
                            regressL == -1 &&
                            xf >= bitBmp[0].Width - 9)
                        {
                            left++;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 && regressL != -1)
                        {
                            left -= regressL;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (xf < bitBmp[initialFrameIndex].Width - 9)
                {
                    xf += 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    breakOk = false;
                    break;
                }
            }

            // 右侧
            for (int xf = bitBmp[initialFrameIndex].Width - 1; xf > 0; xf--)
            {
                for (int frameIdx = initialFrameIndex; frameIdx < maximumFrameIndex; frameIdx++)
                {
                    bitBmp[frameIdx].SelectActiveFrame(dimension, frameIdx);
                    for (int yf = 0; yf < bitBmp[initialFrameIndex].Height; yf++)
                    {
                        Color pixel = bitBmp[frameIdx].GetPixel(xf, yf);
                        if (pixel == _whiteConvert || pixel == customConvert || pixel.A == 0)
                        {
                            var = true;
                        }

                        if (pixel != _whiteConvert && pixel != customConvert && pixel.A != 0)
                        {
                            var = false;
                            if (xf != bitBmp[initialFrameIndex].Width - 1)
                            {
                                regressR++;
                                xf++;
                                yf = -1;
                                frameIdx = initialFrameIndex;
                            }
                            else
                            {
                                breakOk = true;
                                break;
                            }
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 &&
                            regressR == -1 &&
                            xf > 9)
                        {
                            right += 10;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 &&
                            regressR == -1 &&
                            xf <= 9)
                        {
                            right++;
                        }

                        if (var && yf == bitBmp[frameIdx].Height - 1 && frameIdx == maximumFrameIndex - 1 && regressR != -1)
                        {
                            right -= regressR;
                            breakOk = true;
                            break;
                        }
                    }

                    if (breakOk)
                    {
                        break;
                    }
                }

                if (xf > 9)
                {
                    xf -= 9; // 1 of for + 9
                }

                if (breakOk)
                {
                    //breakOk = false;
                    break;
                }
            }

            for (int index = initialFrameIndex; index < maximumFrameIndex; index++)
            {
                Rectangle rect = new Rectangle(left, top, bitBmp[index].Width - left - right, bitBmp[index].Height - top - bottom);
                bitBmp[index].SelectActiveFrame(dimension, index);
                bitBmp[index] = bitBmp[index].Clone(rect, PixelFormat.Format16bppArgb1555);
            }
        }

        private void SetPaletteButton_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < 5; x++)
            {
                // RGB
                if (rbRGB.Checked)
                {
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                    PaletteConverter(1, edit);
                }

                // RBG
                if (rbRBG.Checked)
                {
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                    PaletteConverter(2, edit);
                }

                // GRB
                if (rbGRB.Checked)
                {
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                    PaletteConverter(3, edit);
                }

                // GBR
                if (rbGBR.Checked)
                {
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                    PaletteConverter(4, edit);
                }

                // BGR
                if (rbBGR.Checked)
                {
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                    PaletteConverter(5, edit);
                }

                // BRG
                if (rbBRG.Checked)
                {
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                    PaletteConverter(6, edit);
                }

                SetPaletteBox();
                FramesListView.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
                SetPaletteBox();
                if (DirectionTrackBar.Value != DirectionTrackBar.Maximum)
                {
                    DirectionTrackBar.Value++;
                }
                else
                {
                    DirectionTrackBar.Value = 0;
                }
            }
        }

        // TODO：检查为什么没有选择器 1 的 RadioButton1_CheckedChanged 事件？

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ConvertAndSetPalette(2);
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ConvertAndSetPalette(3);
        }

        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ConvertAndSetPalette(4);
        }

        private void RadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            ConvertAndSetPalette(5);
        }

        private void RadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            ConvertAndSetPalette(6);
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ConvertAndSetPaletteWithReducer();
        }

        private void ConvertAndSetPaletteWithReducer()
        {
            // TODO：除了调用 reducer 之外，这里的整个逻辑与 ConvertAndSetPalette() 相同
            for (int x = 0; x < 5; x++)
            {
                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                PaletteReducer((int)numericUpDown6.Value, (int)numericUpDown7.Value, (int)numericUpDown8.Value, edit);
                SetPaletteBox();
                FramesListView.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
                SetPaletteBox();
                if (DirectionTrackBar.Value != DirectionTrackBar.Maximum)
                {
                    DirectionTrackBar.Value++;
                }
                else
                {
                    DirectionTrackBar.Value = 0;
                }
            }
        }

        private void ConvertAndSetPalette(int selector)
        {
            for (int x = 0; x < 5; x++)
            {
                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                PaletteConverter(selector, edit);
                SetPaletteBox();
                FramesListView.Invalidate();
                Options.ChangedUltimaClass["Animations"] = true;
                SetPaletteBox();
                if (DirectionTrackBar.Value != DirectionTrackBar.Maximum)
                {
                    DirectionTrackBar.Value++;
                }
                else
                {
                    DirectionTrackBar.Value = 0;
                }
            }
        }

        public void UpdateGifPalette(Bitmap bit, AnimIdx animIdx)
        {
            // 重置调色板
            for (int k = 0; k < Animations.PaletteCapacity; k++)
            {
                animIdx.Palette[k] = 0;
            }

            List<Color> entries = new List<Color>();
            bool handled = false;

            // 尝试对 GIF 使用 WPF 解码器
            if (ImageFormat.Gif.Equals(bit.RawFormat))
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bit.Save(ms, ImageFormat.Gif);
                        ms.Position = 0;
                        GifBitmapDecoder decoder = new GifBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        if (decoder.Palette != null)
                        {
                            entries = decoder.Palette.Colors.Select(c => Color.FromArgb(c.A, c.R, c.G, c.B)).ToList();
                            handled = true;
                        }
                    }
                }
                catch { }
            }

            if (!handled)
            {
                // 如果有效，使用 GDI+ 调色板
                if (bit.Palette != null && bit.Palette.Entries.Length > 0)
                {
                    entries = bit.Palette.Entries.ToList();
                }
                else
                {
                    // 从图像内容生成调色板
                    var generated = UoFiddler.Controls.Uop.VdExportHelper.GenerateProperPaletteFromImage(new UoFiddler.Controls.Models.Uop.Imaging.DirectBitmap(bit));
                    entries = generated.Select(c => Color.FromArgb(c.Alpha, c.R, c.G, c.B)).ToList();
                }
            }

            int i = 0;
            while (i < Animations.PaletteCapacity && i < entries.Count)
            {
                int red = entries[i].R / 8;
                int green = entries[i].G / 8;
                int blue = entries[i].B / 8;

                int contaFinal = (0x400 * red) + (0x20 * green) + blue + 0x8000;
                if (contaFinal == 0x8000)
                {
                    contaFinal = 0x8001;
                }

                animIdx.Palette[i] = (ushort)contaFinal;
                i++;
            }

            for (i = 0; i < Animations.PaletteCapacity; i++)
            {
                if (animIdx.Palette[i] < 0x8000)
                {
                    animIdx.Palette[i] = 0x8000;
                }
            }
        }

        public unsafe void UpdateImagePalette(Bitmap bit, AnimIdx animIdx)
        {
            int count = 0;
            var bmp = new Bitmap(bit);
            BitmapData bd = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            var line = (ushort*)bd.Scan0;
            int delta = bd.Stride >> 1;

            int i = 0;
            while (i < Animations.PaletteCapacity)
            {
                animIdx.Palette[i] = 0;
                i++;
            }

            int y = 0;

            while (y < bmp.Height)
            {
                ushort* cur = line;
                for (int x = 0; x < bmp.Width; x++)
                {
                    ushort c = cur[x];
                    if (c == 0)
                    {
                        continue;
                    }

                    bool found = false;
                    i = 0;

                    while (i < animIdx.Palette.Length)
                    {
                        if (animIdx.Palette[i] == c)
                        {
                            found = true;
                            break;
                        }
                        i++;
                    }

                    if (!found)
                    {
                        animIdx.Palette[count++] = c;
                    }

                    if (count >= Animations.PaletteCapacity)
                    {
                        break;
                    }
                }

                for (i = 0; i < Animations.PaletteCapacity; i++)
                {
                    if (animIdx.Palette[i] < 0x8000)
                    {
                        animIdx.Palette[i] = 0x8000;
                    }
                }

                if (count >= Animations.PaletteCapacity)
                {
                    break;
                }

                y++;
                line += delta;
            }
        }

        public void PaletteConverter(int selector, AnimIdx animIdx)
        {
            int i;
            for (i = 0; i < Animations.PaletteCapacity; i++)
            {
                int blueTemp = (animIdx.Palette[i] - 0x8000) / 0x20;
                blueTemp *= 0x20;
                blueTemp = animIdx.Palette[i] - 0x8000 - blueTemp;

                int greenTemp = (animIdx.Palette[i] - 0x8000) / 0x400;
                greenTemp *= 0x400;
                greenTemp = animIdx.Palette[i] - 0x8000 - greenTemp - blueTemp;
                greenTemp /= 0x20;

                int redTemp = (animIdx.Palette[i] - 0x8000) / 0x400;

                int contaFinal = 0;
                switch (selector)
                {
                    case 1:
                        contaFinal = (((0x400 * redTemp) + (0x20 * greenTemp)) + blueTemp) + 0x8000;
                        break;
                    case 2:
                        contaFinal = (((0x400 * redTemp) + (0x20 * blueTemp)) + greenTemp) + 0x8000;
                        break;
                    case 3:
                        contaFinal = (((0x400 * greenTemp) + (0x20 * redTemp)) + blueTemp) + 0x8000;
                        break;
                    case 4:
                        contaFinal = (((0x400 * greenTemp) + (0x20 * blueTemp)) + redTemp) + 0x8000;
                        break;
                    case 5:
                        contaFinal = (((0x400 * blueTemp) + (0x20 * greenTemp)) + redTemp) + 0x8000;
                        break;
                    case 6:
                        contaFinal = (((0x400 * blueTemp) + (0x20 * redTemp)) + greenTemp) + 0x8000;
                        break;
                }

                if (contaFinal == 0x8000)
                {
                    contaFinal = 0x8001;
                }

                animIdx.Palette[i] = (ushort)contaFinal;
            }

            for (i = 0; i < Animations.PaletteCapacity; i++)
            {
                if (animIdx.Palette[i] < 0x8000)
                {
                    animIdx.Palette[i] = 0x8000;
                }
            }
        }

        public void PaletteReducer(int redP, int greenP, int blueP, AnimIdx animIdx)
        {
            int i;
            redP /= 8;
            greenP /= 8;
            blueP /= 8;
            for (i = 0; i < Animations.PaletteCapacity; i++)
            {
                int blueTemp = (animIdx.Palette[i] - 0x8000) / 0x20;
                blueTemp *= 0x20;
                blueTemp = animIdx.Palette[i] - 0x8000 - blueTemp;

                int greenTemp = (animIdx.Palette[i] - 0x8000) / 0x400;
                greenTemp *= 0x400;
                greenTemp = animIdx.Palette[i] - 0x8000 - greenTemp - blueTemp;
                greenTemp /= 0x20;

                int redTemp = (animIdx.Palette[i] - 0x8000) / 0x400;
                redTemp += redP;
                greenTemp += greenP;
                blueTemp += blueP;

                if (redTemp < 0)
                {
                    redTemp = 0;
                }

                if (redTemp > 0x1f)
                {
                    redTemp = 0x1f;
                }

                if (greenTemp < 0)
                {
                    greenTemp = 0;
                }

                if (greenTemp > 0x1f)
                {
                    greenTemp = 0x1f;
                }

                if (blueTemp < 0)
                {
                    blueTemp = 0;
                }

                if (blueTemp > 0x1f)
                {
                    blueTemp = 0x1f;
                }

                int contaFinal = (0x400 * redTemp) + (0x20 * greenTemp) + blueTemp + 0x8000;
                if (contaFinal == 0x8000)
                {
                    contaFinal = 0x8001;
                }

                animIdx.Palette[i] = (ushort)contaFinal;
            }

            for (i = 0; i < Animations.PaletteCapacity; i++)
            {
                if (animIdx.Palette[i] < 0x8000)
                {
                    animIdx.Palette[i] = 0x8000;
                }
            }
        }

        private static Color GetColorFromUltima16Bit(ushort color)
        {
            const int scale = 255 / 31;
            return Color.FromArgb(
                255,
                ((color >> 10) & 0x1F) * scale,
                ((color >> 5) & 0x1F) * scale,
                (color & 0x1F) * scale
                );
        }

        private Models.Uop.UOAnimation CreateUoAnimationFromMulAction(int fileType, int body, int action)
        {
            var allFrames = new List<FrameEntry>();
            var finalPalette = new List<ColourEntry>();
            uint totalFrames = 0;

            for (int dir = 0; dir < 5; dir++)
            {
                AnimIdx edit = AnimationEdit.GetAnimation(fileType, body, action, dir);
                if (edit == null || edit.Frames == null || edit.Frames.Count == 0)
                {
                    continue;
                }

                Bitmap[] bitmaps = edit.GetFrames();
                if (bitmaps == null) continue;

                totalFrames += (uint)bitmaps.Length;

                if (finalPalette.Count == 0)
                {
                    for (int i = 0; i < edit.Palette.Length; i++)
                    {
                        Color c = GetColorFromUltima16Bit(edit.Palette[i]);
                        finalPalette.Add(new ColourEntry(c.R, c.G, c.B, c.A));
                    }
                }

                for (int i = 0; i < bitmaps.Length; i++)
                {
                    if (bitmaps[i] == null) continue;

                    FrameEdit frameEdit = edit.Frames[i];
                    using (Bitmap frameBitmap = new Bitmap(bitmaps[i]))
                    {
                        var frameData = new UopFrameExportData
                        {
                            Image = new DirectBitmap(frameBitmap),
                            Palette = finalPalette,
                            CenterX = (short)frameEdit.Center.X,
                            CenterY = (short)frameEdit.Center.Y,
                            Width = (ushort)frameBitmap.Width,
                            Height = (ushort)frameBitmap.Height,
                            ID = (ushort)body,
                            Frame = (ushort)i
                        };
                        allFrames.Add(new FrameEntry(frameData));
                    }
                }
            }

            if (allFrames.Count == 0)
            {
                return null;
            }

            // MUL 没有全局坐标，因此我们传递 0。
            return new Models.Uop.UOAnimation((uint)body, action, 0, 0, 0, 0, 0, 0, allFrames, finalPalette, totalFrames);
        }

        private List<string> GetExpandedNodePaths(TreeView treeView)
        {
            var expandedPaths = new List<string>();
            foreach (TreeNode node in treeView.Nodes)
            {
                AddExpandedPaths(node, expandedPaths);
            }
            return expandedPaths;
        }

        private void AddExpandedPaths(TreeNode node, List<string> expandedPaths)
        {
            if (node.IsExpanded)
            {
                expandedPaths.Add(node.FullPath);
            }
            foreach (TreeNode childNode in node.Nodes)
            {
                AddExpandedPaths(childNode, expandedPaths);
            }
        }

        private string? GetSelectedNodePath(TreeView treeView)
        {
            return treeView.SelectedNode?.FullPath;
        }

        private void ExpandNodesByPath(TreeView treeView, List<string> expandedPaths)
        {
            treeView.BeginUpdate();
            foreach (TreeNode node in treeView.Nodes)
            {
                ExpandNodeByPath(node, expandedPaths);
            }
            treeView.EndUpdate();
        }

        private void ExpandNodeByPath(TreeNode node, List<string> expandedPaths)
        {
            if (expandedPaths.Contains(node.FullPath))
            {
                node.Expand();
            }
            foreach (TreeNode childNode in node.Nodes)
            {
                ExpandNodeByPath(childNode, expandedPaths);
            }
        }

        private void SelectNodeByPath(TreeView treeView, string? selectedPath)
        {
            if (string.IsNullOrEmpty(selectedPath)) return;

            foreach (TreeNode node in treeView.Nodes)
            {
                TreeNode? foundNode = FindNodeByPath(node, selectedPath);
                if (foundNode != null)
                {
                    treeView.SelectedNode = foundNode;
                    // 确保所选节点可见
                    foundNode.EnsureVisible();
                    break;
                }
            }
        }

        private TreeNode? FindNodeByPath(TreeNode node, string targetPath)
        {
            if (node.FullPath == targetPath)
            {
                return node;
            }
            foreach (TreeNode childNode in node.Nodes)
            {
                TreeNode? foundNode = FindNodeByPath(childNode, targetPath);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }
            return null;
        }
        private void UpdateUopData(int? newCenterX = null, int? newCenterY = null, bool applyToAllFrames = false)
        {
            if (_uopManager == null) return;

            if (applyToAllFrames && (newCenterX.HasValue || newCenterY.HasValue))
            {
                var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                if (uopAnim != null)
                {
                    foreach (var frame in uopAnim.Frames)
                    {
                        if (newCenterX.HasValue) frame.Header.CenterX = (short)newCenterX.Value;
                        if (newCenterY.HasValue) frame.Header.CenterY = (short)newCenterY.Value;
                    }
                    uopAnim.IsModified = true;
                }
            }

            try
            {
                _uopManager.CommitChanges(_currentBody, _currentAction);
                System.Diagnostics.Debug.WriteLine($"✅ UpdateUopData: 已提交 Body={_currentBody} Action={_currentAction} 的更改");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新 UOP 内存数据时出错：{ex.Message}");
            }
        }

        private void SaveUopAnimation()
        {
            if (_uopManager == null) return;

            string outputPath = Options.OutputPath;
            if (string.IsNullOrEmpty(outputPath))
            {
                MessageBox.Show("未配置输出路径。请在选项中设置。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            // 尝试查找源文件以构造输出文件名（如果未知则使用默认值）
            string uopFileName = "AnimationFrame1.uop";
            var fileInfo = _uopManager.GetAnimationData(_currentBody, _currentAction, 0);
            if (fileInfo != null && fileInfo.File != null)
            {
                uopFileName = Path.GetFileName(fileInfo.File.FilePath);
            }
            else
            {
                for (int a = 0; a < 100; a++)
                {
                    var fi = _uopManager.GetAnimationData(_currentBody, a, 0);
                    if (fi != null && fi.File != null)
                    {
                        uopFileName = Path.GetFileName(fi.File.FilePath);
                        break;
                    }
                }
            }

            string destinationUopFilePath = Path.Combine(outputPath, uopFileName);

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // 重要：在保存前不要调用 CommitAllChanges。
                // SaveModifiedAnimationsToUopHybrid 方法会检测修改的 ID
                //（参数 + 导入跟踪 + 缓存）并写入分组文件。
                bool success = VdImportHelper.SaveModifiedAnimationsToUopHybrid(_uopManager, -1, destinationUopFilePath);

                if (success)
                {
                    // 写入后，如果需要可以切换/重新加载内部状态。
                    // 如果必要，这里的 CommitAllChanges 确保内存一致性。
                    try
                    {
                        _uopManager.CommitAllChanges();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"警告：保存后 CommitAllChanges 失败：{ex.Message}");
                    }

                    Options.ChangedUltimaClass["Animations"] = false;
                    MessageBox.Show($"动画已保存到 {destinationUopFilePath}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 强制重新加载以应用写入的更改。
                    try
                    {
                        _uopManager.ClearCache();
                        LoadUopAnimations();
                    }
                    catch { /* 尽力重新加载 */ }
                }
                else
                {
                    MessageBox.Show("保存动画失败。请检查调试日志以获取详细信息。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }



        private void OnClickExportToSpritesheet(object sender, EventArgs e)
        {
            if (_fileType == 0) return;

            using (var form = new PackOptionsForm())
            {
                if (form.ShowDialog() != DialogResult.OK) return;

                // 询问缩放百分比
                double scale = 1.0;
                string input = ShowInputDialog("输入缩放百分比（例如 100 表示原始大小，50 表示一半大小）：", "缩放导出", "100");
                if (int.TryParse(input, out int percentage) && percentage > 0 && percentage != 100)
                {
                    scale = percentage / 100.0;
                }

                using (var fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() != DialogResult.OK) return;

                    if (form.ExportAllAnimations)
                    {
                        int actionCount = (_fileType == 6) ? 32 : Animations.GetAnimLength(_currentBody, _fileType);

                        for (int action = 0; action < actionCount; action++)
                        {
                            var frames = GetFramesForExport(_currentBody, action, form.SelectedDirections);
                            if (frames.Count > 0)
                            {
                                if (scale != 1.0) ResizeFrames(frames, scale); // 使用辅助函数或内联逻辑

                                string baseName = $"anim{_fileType}_{_currentBody}_{action:D2}";
                                AnimationPacker.ExportToSpritesheet(frames, fbd.SelectedPath, baseName, form.MaxWidth, form.FrameSpacing, form.OneRowPerDirection);
                            }
                        }
                        MessageBox.Show($"批量导出完成。（缩放：{scale:P0}）", "导出", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        var frames = GetFramesForExport(_currentBody, _currentAction, form.SelectedDirections);
                        if (frames.Count == 0)
                        {
                            MessageBox.Show("没有可导出的帧。", "导出", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (scale != 1.0) ResizeFrames(frames, scale);

                        string baseName = $"anim{_fileType}_{_currentBody}_{_currentAction}";
                        AnimationPacker.ExportToSpritesheet(frames, fbd.SelectedPath, baseName, form.MaxWidth, form.FrameSpacing, form.OneRowPerDirection);
                        MessageBox.Show($"导出完成。（缩放：{scale:P0}）", "导出", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void ResizeFrames(List<AnimationPacker.FrameInfo> frames, double scale)
        {
            foreach (var frame in frames)
            {
                int newWidth = (int)Math.Max(1, frame.Image.Width * scale);
                int newHeight = (int)Math.Max(1, frame.Image.Height * scale);

                var resized = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(resized))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    g.DrawImage(frame.Image, 0, 0, newWidth, newHeight);
                }

                frame.Image.Dispose();
                frame.Image = resized;
                frame.Center = new Point((int)(frame.Center.X * scale), (int)(frame.Center.Y * scale));
            }
        }

        private void OnClickImportFromSpritesheet(object sender, EventArgs e)
        {
            if (_fileType == UOP_FILE_TYPE && _uopManager != null)
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "JSON 文件 (*.json)|*.json";
                    ofd.Multiselect = true;
                    if (ofd.ShowDialog() != DialogResult.OK) return;

                    try
                    {
                        // 选择目标 UOP 文件
                        string targetUopPath = null;
                        using (var fileSelectForm = new UopFileSelectionForm(_uopManager.LoadedUopFiles))
                        {
                            if (fileSelectForm.ShowDialog() == DialogResult.OK)
                            {
                                targetUopPath = fileSelectForm.SelectedPath;
                            }
                            else
                            {
                                return;
                            }
                        }

                        int successCount = 0;
                        int failCount = 0;
                        int lastBody = -1;

                        foreach (string file in ofd.FileNames)
                        {
                            try
                            {
                                var info = ParseAnimationFileName(file);
                                int targetBody = _currentBody;
                                int targetAction = (info.action != -1) ? info.action : _currentAction;
                                lastBody = targetBody;

                                var frames = AnimationPacker.ImportFromSpritesheet(file);
                                ImportFramesToAnimation(frames, targetBody, targetAction, targetUopPath);
                                successCount++;
                            }
                            catch (Exception ex)
                            {
                                failCount++;
                                System.Diagnostics.Debug.WriteLine($"导入 {file} 失败：{ex.Message}");
                            }
                        }

                        if (successCount > 0)
                        {
                            // 以 HYBRID 模式保存
                            _uopManager.CommitAllChanges();
                            string destinationPath = Path.Combine(Options.OutputPath, Path.GetFileName(targetUopPath));

                            if (VdImportHelper.SaveModifiedAnimationsToUopHybrid(_uopManager, _currentBody, destinationPath))
                            {
                                MessageBox.Show(
                                    $"✅ {successCount} 个 Spritesheet 文件已以 HYBRID 模式导入！\n" +
                                    $"📁 文件：{destinationPath}",
                                    "导入完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                _uopManager.ClearCache();
                                LoadUopAnimations();
                                Options.ChangedUltimaClass["Animations"] = false;
                            }
                            else
                            {
                                MessageBox.Show("❌ 导入成功但保存失败。请检查日志。",
                                    "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                _uopManager.ClearCache();
                                LoadUopAnimations();
                                Options.ChangedUltimaClass["Animations"] = true;
                            }

                            if (lastBody != -1)
                            {
                                TreeNode node = GetNode(lastBody);
                                if (node != null)
                                {
                                    AnimationListTreeView.SelectedNode = node;
                                    node.Expand();
                                    node.EnsureVisible();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ 导入 Spritesheet 时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return;
            }

            // MUL 后备
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON 文件 (*.json)|*.json";
                ofd.Multiselect = true;
                if (ofd.ShowDialog() != DialogResult.OK) return;

                int successCount = 0;
                int failCount = 0;
                int lastBody = -1;

                foreach (string file in ofd.FileNames)
                {
                    try
                    {
                        var info = ParseAnimationFileName(file);
                        int targetBody = _currentBody;
                        int targetAction = (info.action != -1) ? info.action : _currentAction;
                        lastBody = targetBody;

                        var frames = AnimationPacker.ImportFromSpritesheet(file);
                        ImportFramesToAnimation(frames, targetBody, targetAction);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        System.Diagnostics.Debug.WriteLine($"导入 {file} 失败：{ex.Message}");
                    }
                }

                if (successCount > 0)
                {
                    Options.ChangedUltimaClass["Animations"] = true;
                    LoadMulAnimations();

                    if (lastBody != -1)
                    {
                        TreeNode node = GetNode(lastBody);
                        if (node != null)
                        {
                            AnimationListTreeView.SelectedNode = node;
                            node.Expand();
                            node.EnsureVisible();
                        }
                    }
                }

                if (failCount == 0)
                {
                    MessageBox.Show($"成功导入 {successCount} 个 Spritesheet。", "导入", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"已导入 {successCount} 个 Spritesheet。\n无法导入 {failCount} 个文件。", "导入警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void OnClickExportFramesXml(object sender, EventArgs e)
        {
            if (_fileType == 0) return;

            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() != DialogResult.OK) return;

                var frames = GetFramesForExport(_currentBody, _currentAction, new List<int> { 0, 1, 2, 3, 4 });
                if (frames.Count == 0)
                {
                    MessageBox.Show("没有可导出的帧。", "导出", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string baseName = $"anim{_fileType}_{_currentBody}_{_currentAction}";
                AnimationPacker.ExportToFramesXml(frames, fbd.SelectedPath, baseName);
                MessageBox.Show("导出完成。", "导出", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnClickExportAllFramesXml(object sender, EventArgs e)
        {
            if (_fileType == 0) return;

            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() != DialogResult.OK) return;

                // 询问缩放百分比
                double scale = 1.0;
                string input = ShowInputDialog("输入缩放百分比（例如 100 表示原始大小，50 表示一半大小）：", "缩放导出", "100");
                if (int.TryParse(input, out int percentage) && percentage > 0 && percentage != 100)
                {
                    scale = percentage / 100.0;
                }

                int actionCount = (_fileType == 6) ? 32 : Animations.GetAnimLength(_currentBody, _fileType);

                for (int action = 0; action < actionCount; action++)
                {
                    var frames = GetFramesForExport(_currentBody, action, new List<int> { 0, 1, 2, 3, 4 });
                    if (frames.Count > 0)
                    {
                        // 如果需要应用缩放
                        if (scale != 1.0)
                        {
                            foreach (var frame in frames)
                            {
                                int newWidth = (int)Math.Max(1, frame.Image.Width * scale);
                                int newHeight = (int)Math.Max(1, frame.Image.Height * scale);

                                var resized = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);
                                using (Graphics g = Graphics.FromImage(resized))
                                {
                                    // 使用最近邻插值以保留原始调色板颜色，避免模糊/伪影
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                                    g.DrawImage(frame.Image, 0, 0, newWidth, newHeight);
                                }

                                // 释放原始图像并用缩放后的图像替换
                                frame.Image.Dispose();
                                frame.Image = resized;

                                // 缩放中心坐标
                                frame.Center = new Point((int)(frame.Center.X * scale), (int)(frame.Center.Y * scale));
                            }
                        }

                        string baseName = $"anim{_fileType}_{_currentBody}_{action:D2}";
                        AnimationPacker.ExportToFramesXml(frames, fbd.SelectedPath, baseName);
                    }
                }
                MessageBox.Show($"批量导出完成。（缩放：{scale:P0}）", "导出", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnClickImportFramesXml(object sender, EventArgs e)
        {
            if (_fileType == UOP_FILE_TYPE && _uopManager != null)
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "XML 文件 (*.xml)|*.xml";
                    ofd.Multiselect = true;
                    if (ofd.ShowDialog() != DialogResult.OK) return;

                    try
                    {
                        // 选择目标 UOP 文件
                        string targetUopPath = null;
                        using (var fileSelectForm = new UopFileSelectionForm(_uopManager.LoadedUopFiles))
                        {
                            if (fileSelectForm.ShowDialog() == DialogResult.OK)
                            {
                                targetUopPath = fileSelectForm.SelectedPath;
                            }
                            else
                            {
                                return;
                            }
                        }

                        int successCount = 0;
                        int failCount = 0;
                        int lastBody = -1;

                        foreach (string file in ofd.FileNames)
                        {
                            try
                            {
                                var info = ParseAnimationFileName(file);
                                int targetBody = _currentBody;
                                int targetAction = (info.action != -1) ? info.action : _currentAction;
                                lastBody = targetBody;

                                var frames = AnimationPacker.ImportFromFramesXml(file);
                                ImportFramesToAnimation(frames, targetBody, targetAction, targetUopPath);
                                successCount++;
                            }
                            catch (Exception ex)
                            {
                                failCount++;
                                System.Diagnostics.Debug.WriteLine($"导入 {file} 失败：{ex.Message}");
                            }
                        }

                        if (successCount > 0)
                        {
                            // 以 HYBRID 模式保存（与 VD 相同）
                            _uopManager.CommitAllChanges();
                            string destinationPath = Path.Combine(Options.OutputPath, Path.GetFileName(targetUopPath));

                            if (VdImportHelper.SaveModifiedAnimationsToUopHybrid(_uopManager, _currentBody, destinationPath))
                            {
                                MessageBox.Show(
                                    $"✅ {successCount} 个 Spritesheet 文件已以 HYBRID 模式导入！\n" +
                                    $"📁 文件：{destinationPath}",
                                    "导入完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                _uopManager.ClearCache(); // 强制重新加载以更新边界框
                                LoadUopAnimations();
                                Options.ChangedUltimaClass["Animations"] = false;
                            }
                            else
                            {
                                MessageBox.Show("❌ 导入成功但保存失败。请检查日志。",
                                    "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                LoadUopAnimations();
                                Options.ChangedUltimaClass["Animations"] = true;
                            }

                            if (lastBody != -1)
                            {
                                TreeNode node = GetNode(lastBody);
                                if (node != null)
                                {
                                    AnimationListTreeView.SelectedNode = node;
                                    node.Expand();
                                    node.EnsureVisible();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ 导入 XML 时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return;
            }

            // MUL 文件的后备（原始行为）
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "XML 文件 (*.xml)|*.xml";
                ofd.Multiselect = true;
                if (ofd.ShowDialog() != DialogResult.OK) return;

                int successCount = 0;
                int failCount = 0;

                int lastBody = -1;

                foreach (string file in ofd.FileNames)
                {
                    try
                    {
                        var info = ParseAnimationFileName(file);
                        int targetBody = _currentBody;
                        int targetAction = (info.action != -1) ? info.action : _currentAction;
                        lastBody = targetBody;

                        var frames = AnimationPacker.ImportFromFramesXml(file);
                        ImportFramesToAnimation(frames, targetBody, targetAction);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        System.Diagnostics.Debug.WriteLine($"导入 {file} 失败：{ex.Message}");
                        if (failCount == 1)
                        {
                            MessageBox.Show($"导入文件 {Path.GetFileName(file)} 时出错：\n{ex.Message}\n\n堆栈跟踪：\n{ex.StackTrace}", "导入错误详情", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                if (successCount > 0)
                {
                    Options.ChangedUltimaClass["Animations"] = true;
                    LoadMulAnimations();

                    if (lastBody != -1)
                    {
                        TreeNode node = GetNode(lastBody);
                        if (node != null)
                        {
                            AnimationListTreeView.SelectedNode = node;
                            node.Expand();
                            node.EnsureVisible();
                        }
                    }
                }

                if (failCount == 0)
                {
                    MessageBox.Show($"成功导入 {successCount} 个 XML 文件。", "导入", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"已导入 {successCount} 个 XML 文件。\n无法导入 {failCount} 个文件。", "导入警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private List<AnimationPacker.FrameInfo> GetFramesForExport(int body, int action, List<int> directions)
        {
            var result = new List<AnimationPacker.FrameInfo>();

            if (_fileType == 6) // UOP
            {
                if (_uopManager == null) return result;
                foreach (int dir in directions)
                {
                    var uopAnim = _uopManager.GetUopAnimation(body, action, dir);
                    if (uopAnim != null)
                    {
                        for (int i = 0; i < uopAnim.Frames.Count; i++)
                        {
                            result.Add(new AnimationPacker.FrameInfo
                            {
                                Image = new Bitmap(uopAnim.Frames[i].Image),
                                Center = new Point(uopAnim.Frames[i].Header.CenterX, uopAnim.Frames[i].Header.CenterY),
                                Direction = dir,
                                Index = i
                            });
                        }
                    }
                }
            }
            else // MUL
            {
                foreach (int dir in directions)
                {
                    AnimIdx anim = AnimationEdit.GetAnimation(_fileType, body, action, dir);
                    if (anim != null)
                    {
                        Bitmap[] bitmaps = anim.GetFrames();
                        if (bitmaps != null)
                        {
                            for (int i = 0; i < bitmaps.Length; i++)
                            {
                                var frame = anim.Frames[i];
                                result.Add(new AnimationPacker.FrameInfo
                                {
                                    Image = new Bitmap(bitmaps[i]),
                                    Center = frame.Center,
                                    Direction = dir,
                                    Index = i
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        private (int type, int body, int action) ParseAnimationFileName(string fileName)
        {
            try
            {
                string name = Path.GetFileNameWithoutExtension(fileName);
                var match = System.Text.RegularExpressions.Regex.Match(name, @"anim(\d+)_(\d+)_(\d+)");
                if (match.Success)
                {
                    return (
                        int.Parse(match.Groups[1].Value),
                        int.Parse(match.Groups[2].Value),
                        int.Parse(match.Groups[3].Value)
                    );
                }
            }
            catch { }
            return (-1, -1, -1);
        }

        private void ImportFramesToAnimation(List<AnimationPacker.FrameInfo> frames, int body = -1, int action = -1, string targetUopPath = null)
        {
            int targetBody = (body != -1) ? body : _currentBody;
            int targetAction = (action != -1) ? action : _currentAction;

            // 步骤 1：准备数据（按方向分组）
            var framesByDir = frames.GroupBy(f => f.Direction).ToList();

            foreach (var group in framesByDir)
            {
                int direction = group.Key;

                if (_fileType == 6) // UOP
                {
                    // UOP 逻辑（现有）
                    var uopAnim = _uopManager.GetUopAnimation(targetBody, targetAction, direction);
                    if (uopAnim == null)
                    {
                        uopAnim = _uopManager.CreateNewUopAnimation(targetBody, targetAction, direction, targetUopPath);
                    }

                    foreach (var frame in group)
                    {
                        while (uopAnim.Frames.Count <= frame.Index)
                        {
                            var dummy = new UoFiddler.Controls.Uop.DecodedUopFrame();
                            dummy.Image = new Bitmap(1, 1);
                            uopAnim.Frames.Add(dummy);
                        }

                        var newFrame = new UoFiddler.Controls.Uop.DecodedUopFrame();
                        newFrame.Image = new Bitmap(frame.Image);
                        newFrame.Header = new UoFiddler.Controls.Uop.UopFrameHeader
                        {
                            Width = (ushort)newFrame.Image.Width,
                            Height = (ushort)newFrame.Image.Height,
                            CenterX = (short)frame.Center.X,
                            CenterY = (short)frame.Center.Y
                        };

                        var paletteEntries = UoFiddler.Controls.Uop.VdExportHelper.GenerateProperPaletteFromImage(new UoFiddler.Controls.Models.Uop.Imaging.DirectBitmap(newFrame.Image));
                        newFrame.Palette = paletteEntries.Select(p => Color.FromArgb(p.Alpha, p.R, p.G, p.B)).ToList();

                        uopAnim.Frames[frame.Index] = newFrame;
                        uopAnim.IsModified = true;
                    }
                }
                else // MUL
                {
                    AnimIdx anim = AnimationEdit.GetAnimation(_fileType, targetBody, targetAction, direction);
                    if (anim == null) continue;

                    // 调色板生成（修复空帧/崩溃）
                    // 检查是否需要新调色板（如果当前为空/黑色）
                    bool needsPalette = true;
                    for (int i = 0; i < 256; i++)
                    {
                        if (anim.Palette[i] != 0)
                        {
                            needsPalette = false; // 调色板存在，不要覆盖（除非强制？假设严格的附加/替换使用现有）
                            break;
                        }
                    }

                    // 如果是新动画（空调色板），则从此组中的所有帧生成一个
                    if (needsPalette)
                    {
                        var allImages = group.Select(f => f.Image).ToList();
                        ushort[] newPalette = GenerateMulPalette(allImages);
                        anim.ReplacePalette(newPalette);
                    }

                    // 处理帧
                    foreach (var frame in group)
                    {
                        int currentCount = (anim.Frames != null) ? anim.Frames.Count : 0;

                        // 转换为安全的 16bpp
                        using (Bitmap bmp16 = ConvertToUltima16Bpp(frame.Image))
                        {
                            if (frame.Index < currentCount)
                            {
                                // 在创建 FrameEdit 之前更新中心（ReplaceFrame 使用当前中心）
                                // 如果我们在之后更新，RawData 偏移量（使用旧中心烘焙） + 新中心会导致 GetFrames 越界写入。
                                anim.Frames[frame.Index].Center = frame.Center;
                                anim.ReplaceFrame(bmp16, frame.Index);
                            }
                            else
                            {
                                while (currentCount < frame.Index)
                                {
                                    using (Bitmap dummy = new Bitmap(1, 1, PixelFormat.Format16bppArgb1555))
                                    {
                                        anim.AddFrame(dummy, 0, 0);
                                    }
                                    currentCount++;
                                }

                                anim.AddFrame(bmp16, frame.Center.X, frame.Center.Y);
                            }
                        }
                    }
                }
            }

            // 步骤 2：更新目录树而不清除缓存
            if (_fileType == 6) // UOP
            {
                Options.ChangedUltimaClass["Animations"] = true;

                // 此处不调用 LoadUopAnimations()！我们只手动更新节点。
                AnimationListTreeView.BeginUpdate();
                try
                {
                    // 检查身体节点是否已存在于目录树中
                    TreeNode bodyNode = GetNode(targetBody);

                    if (bodyNode == null)
                    {
                        // 如果不存在，则创建新的身体节点
                        string mappingInfo = GetUopMulMapping(targetBody);
                        bodyNode = new TreeNode
                        {
                            Tag = (ushort)targetBody,
                            Text = $"UOP ID: {targetBody}{mappingInfo}",
                            ForeColor = Color.Black
                        };
                        AnimationListTreeView.Nodes.Add(bodyNode);
                        System.Diagnostics.Debug.WriteLine($"✅ 创建了新的身体节点：{targetBody}");
                    }
                    else
                    {
                        // 身体存在，切换为黑色（有效）
                        bodyNode.ForeColor = Color.Black;
                    }

                    // 检查此身体中是否已存在动作节点
                    TreeNode actionNode = null;
                    foreach (TreeNode child in bodyNode.Nodes)
                    {
                        if (child.Tag is int actionTag && actionTag == targetAction)
                        {
                            actionNode = child;
                            break;
                        }
                    }

                    if (actionNode == null)
                    {
                        // 如果不存在，则创建新的动作节点
                        actionNode = new TreeNode
                        {
                            Tag = targetAction,
                            Text = $"{targetAction:D2}_{GetActionDescription(targetBody, targetAction)} (已导入)",
                            ForeColor = Color.Black
                        };
                        bodyNode.Nodes.Add(actionNode);
                        System.Diagnostics.Debug.WriteLine($"✅ 为身体 {targetBody} 创建了新的动作节点：{targetAction}");
                    }
                    else
                    {
                        // 动作存在，切换为黑色（有效）
                        actionNode.ForeColor = Color.Black;
                        actionNode.Text = $"{targetAction:D2}_{GetActionDescription(targetBody, targetAction)} (已修改)";
                    }

                    // 展开身体节点以显示新动作
                    bodyNode.Expand();
                }
                finally
                {
                    AnimationListTreeView.EndUpdate();
                }
            }
            else // MUL
            {
                TreeNode node = GetNode(targetBody);
                if (node != null)
                {
                    node.ForeColor = Color.Black;
                    if (targetAction >= 0 && targetAction < node.Nodes.Count)
                    {
                        node.Nodes[targetAction].ForeColor = Color.Black;
                    }
                    node.Expand();
                    node.EnsureVisible();
                }
            }

            // ✅ 步骤 3：如果这是当前选中的动画，则刷新显示
            if (targetBody == _currentBody && targetAction == _currentAction)
            {
                DisplayUopAnimation();
                FramesListView.Invalidate();
                AnimationPictureBox.Invalidate();
            }
        }

        private ushort[] GenerateMulPalette(List<Bitmap> images)
        {
            HashSet<ushort> uniqueColors = new HashSet<ushort>();
            uniqueColors.Add(0); // 确保透明度存在

            foreach (var img in images)
            {
                if (img == null) continue;

                // 先转换为 16bpp 以确保获得 Ultima 将看到的准确颜色
                using (var bmp16 = ConvertToUltima16Bpp(img))
                {
                    BitmapData bd = bmp16.LockBits(new Rectangle(0, 0, bmp16.Width, bmp16.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                    unsafe
                    {
                        ushort* ptr = (ushort*)bd.Scan0;
                        int stride = bd.Stride / 2;
                        for (int y = 0; y < bmp16.Height; y++)
                        {
                            for (int x = 0; x < bmp16.Width; x++)
                            {
                                ushort color = ptr[y * stride + x];
                                if (color != 0) // 忽略透明，已经添加
                                {
                                    uniqueColors.Add(color);
                                }
                            }
                        }
                    }
                    bmp16.UnlockBits(bd);
                }

                if (uniqueColors.Count >= 256) break; // 达到限制（优化）
            }

            ushort[] palette = new ushort[256];
            int i = 0;
            foreach (var c in uniqueColors.Take(256))
            {
                palette[i++] = c;
            }
            return palette;
        }

        private Bitmap ConvertToUltima16Bpp(Bitmap source)
        {
            if (source == null || source.Width == 0 || source.Height == 0)
            {
                return new Bitmap(1, 1, PixelFormat.Format16bppArgb1555);
            }

            try
            {
                // 使用 Clone 安全转换像素格式（处理索引 -> RGB 和 32bpp -> 16bpp）
                return source.Clone(new Rectangle(0, 0, source.Width, source.Height), PixelFormat.Format16bppArgb1555);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"转换位图时出错：{ex.Message}");
                // 后备：创建一个空的 16bpp 位图并尝试绘制（较慢但安全的后备，尽管 Clone 通常有效）
                Bitmap bmp = new Bitmap(source.Width, source.Height, PixelFormat.Format16bppArgb1555);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                    // DrawImage 也处理格式转换，但如果我们在源上创建 Graphics，可能会在索引源上 OOM。
                    // 这里我们在目标（16bpp）上创建 Graphics，这是安全的。
                    g.DrawImage(source, 0, 0, source.Width, source.Height);
                }
                return bmp;
            }
        }

        #region [ 编辑 Ultima Online Bodyconv.def 和 mobtypes.txt ]
        private EditUoBodyconvMobtypes editUoBodyconvMobtypesForm;

        private void editUoBodyconvAndMobtypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查窗体是否已打开
            if (editUoBodyconvMobtypesForm == null || editUoBodyconvMobtypesForm.IsDisposed)
            {
                // 如果尚未打开，则打开窗体
                editUoBodyconvMobtypesForm = new EditUoBodyconvMobtypes();
                editUoBodyconvMobtypesForm.Show();
            }
            else
            {
                // 如果已打开，则将其置于前台
                editUoBodyconvMobtypesForm.BringToFront();
            }

            // 在 EditUoBodyconvMobtypes 窗体中设置 textBoxID 的值
            editUoBodyconvMobtypesForm.TextBoxID = _currentBody.ToString(); // ID
            editUoBodyconvMobtypesForm.TextBoxBody = BodyConverter.GetTrueBody(_fileType, _currentBody).ToString(); //身体 ID
        }
        #endregion

        #region [ 将图像复制到剪贴板 ]
        private void copyFrameToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FramesListView.SelectedItems.Count == 0)
                return;

            int frameIndex = (int)FramesListView.SelectedItems[0].Tag;

            AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            if (edit == null)
                return;

            Bitmap[] frames = edit.GetFrames();
            if (frames == null || frameIndex >= frames.Length)
                return;

            Bitmap frame = frames[frameIndex];

            // 将帧复制到剪贴板
            Clipboard.SetImage(frame);

            MessageBox.Show("所选帧已成功复制到剪贴板。", "复制帧", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region [ 从剪贴板导入图像 ]
        private void importImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage() && FramesListView.SelectedItems.Count > 0)
            {
                int frameIndex = (int)FramesListView.SelectedItems[0].Tag;

                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                if (edit == null)
                    return;

                Bitmap newFrame = (Bitmap)Clipboard.GetImage();

                edit.ReplaceFrame(newFrame, frameIndex);

                FramesListView.Items[frameIndex].ImageIndex = frameIndex;
                FramesListView.Invalidate();

                Options.ChangedUltimaClass["Animations"] = true;

                MessageBox.Show("帧图像已从剪贴板导入");
            }
        }

        private void FramesImportandCopyListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                importImageToolStripMenuItem_Click(null, null);
            }

            if (e.Control && e.KeyCode == Keys.X)
            {
                copyFrameToClipboardToolStripMenuItem_Click(null, null);
            }
        }
        #endregion

        #region [ 镜像图像 ]
        private void mirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FramesListView.SelectedItems.Count > 0)
            {
                int frameIndex = (int)FramesListView.SelectedItems[0].Tag;

                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                if (edit == null) return;

                // 加载位图
                Bitmap frame = edit.GetFrames()[frameIndex];

                // 镜像图像
                frame.RotateFlip(RotateFlipType.RotateNoneFlipX);

                // 替换帧
                edit.ReplaceFrame(frame, frameIndex);

                // 重建列表视图
                FramesListView.BeginUpdate();
                FramesListView.Items.Clear();
                // 重新加载所有帧
                Bitmap[] frames = edit.GetFrames();
                for (int i = 0; i < frames.Length; i++)
                {
                    ListViewItem item = new ListViewItem(i.ToString()) { Tag = i };
                    if (frames[i] != null)
                    {
                        item.ImageIndex = i;
                    }
                    FramesListView.Items.Add(item);
                }
                FramesListView.EndUpdate();

                // 更新显示
                FramesListView.Items[frameIndex].Selected = true;
                FramesListView.Select();

                Options.ChangedUltimaClass["Animations"] = true;

                MessageBox.Show("帧已镜像");
            }
        }
        #endregion

        #region [ 向左旋转90度 ]
        private void rotateLeft90DegreesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FramesListView.SelectedItems.Count > 0)
            {
                int index = (int)FramesListView.SelectedItems[0].Tag;

                AnimIdx edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);

                if (edit != null)
                {
                    Bitmap frame = edit.GetFrames()[index];

                    if (frame != null)
                    {
                        frame.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        edit.ReplaceFrame(frame, index);
                    }
                }

                FramesListView.Invalidate();
            }
            // 标记更改
            Options.ChangedUltimaClass["Animations"] = true;
        }
        #endregion

        #region [ 搜索动画 ] toolStripTextBoxSearch_TextChanged
        private void toolStripTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = toolStripTextBoxSearch.Text;
            foreach (TreeNode node in AnimationListTreeView.Nodes)
            {
                if (node.Tag.ToString() == searchText)
                {
                    AnimationListTreeView.SelectedNode = node;
                    break;
                }
            }
        }
        #endregion

        #region [ 查找 ID ]
        private async void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 显示进度条
            ProgressBar.Visible = true;
            ProgressBar.Maximum = AnimationListTreeView.Nodes.Count;
            ProgressBar.Value = 0;

            await Task.Run(() =>
            {
                // 遍历 TreeView 节点
                foreach (TreeNode node in AnimationListTreeView.Nodes)
                {
                    int bodyIndex = (int)node.Tag;

                    // 检查动作 0
                    if (!AnimationEdit.IsActionDefined(_fileType, bodyIndex, 0))
                    {
                        // 如果未定义，则槽位空闲
                        this.Invoke(new Action(() =>
                        {
                            node.ForeColor = Color.Blue;
                            node.Text += " - 空闲";
                        }));
                    }

                    // 更新进度条
                    this.Invoke(new Action(() =>
                    {
                        ProgressBar.Value++;
                    }));
                }
            });

            // 隐藏进度条
            ProgressBar.Visible = false;

            AnimationListTreeView.Invalidate();
        }
        #endregion

        #region [ 列出所有 ID ]
        private void listsAllIDsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string defaultFileName = "AminID.txt"; // 设置默认文件名

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = defaultFileName,
                Filter = "文本文件 (*.txt)|*.txt",
                Title = "保存 ID 概述"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("您是否还想列出空闲的 ID？",
                                                      "ID 选择", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                bool includeFreeIDs = result == DialogResult.Yes; // 如果“是”，则同时列出空闲 ID

                // 已用和空闲 ID 的计数器
                int occupiedCount = 0;
                int freeCount = 0;

                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.WriteLine("ID 概述：");
                        writer.WriteLine("--------------------");

                        // 遍历 AnimationListTreeView 中的所有节点
                        foreach (TreeNode node in AnimationListTreeView.Nodes)
                        {
                            if (node.Tag != null)
                            {
                                int id = (int)node.Tag;
                                bool valid = false;

                                // 检查此 ID 的所有动作
                                foreach (TreeNode actionNode in node.Nodes)
                                {
                                    int actionId = (int)actionNode.Tag;
                                    if (AnimationEdit.IsActionDefined(_fileType, id, actionId))
                                    {
                                        valid = true; // 至少定义了一个动作
                                        break;
                                    }
                                }

                                if (valid)
                                {
                                    writer.WriteLine($"ID: {id} - 状态: 已占用");
                                    occupiedCount++;
                                }
                                else if (includeFreeIDs)
                                {
                                    writer.WriteLine($"ID: {id} - 状态: 未占用");
                                    freeCount++;
                                }
                            }
                        }

                        writer.WriteLine("\n--------------------");
                        writer.WriteLine("摘要：");
                        writer.WriteLine($"已占用 ID: {occupiedCount}");
                        if (includeFreeIDs)
                        {
                            writer.WriteLine($"空闲 ID: {freeCount}");
                        }
                    }

                    MessageBox.Show("ID 概述已成功保存。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存文件时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region [ 显示 ]
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showOnlyValid = !_showOnlyValid;

            if (_showOnlyValid)
            {
                AnimationListTreeView.BeginUpdate();
                try
                {
                    for (int i = AnimationListTreeView.Nodes.Count - 1; i >= 0; --i)
                    {
                        if (AnimationListTreeView.Nodes[i].ForeColor == Color.Red)
                        {
                            AnimationListTreeView.Nodes[i].Remove();
                        }
                    }
                }
                finally
                {
                    AnimationListTreeView.EndUpdate();
                }
            }
            else
            {
                OnLoad(null);
            }
        }
        #endregion

        #region [ 保存当前文件 ]
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                return;
            }

            AnimationEdit.Save(_fileType, Options.OutputPath);
            Options.ChangedUltimaClass["Animations"] = false;

            MessageBox.Show($"动画文件已保存到 {Options.OutputPath}", "已保存", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        #endregion

        #region [ OnCheckBoxIDBlueChanged ] - 重新检查可加载的动画并相应更新 TreeView
        private void OnCheckBoxIDBlueChanged(object sender, EventArgs e)
        {
            if (_fileType == UOP_FILE_TYPE)
            {
                LoadUopAnimations();
            }
            else if (_fileType != 0)
            {
                LoadMulAnimations();
            }
        }
        #endregion

        #region [ checkBoxMount_CheckedChanged ]
        private void checkBoxMount_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAnimations();
        }
        #endregion

        #region [ UpdateAnimation ]
        private void UpdateAnimations()
        {
            if (isAnimationVisible)
            {
                // 隐藏动画
                additionalAnimation = null;
                buttonShow.Text = "显示";
            }
            else
            {
                // 根据所选性别显示动画
                string selectedGender = comboBoxMenWoman.SelectedItem.ToString();
                int animId = selectedGender == "men" ? 400 : 401;

                if (checkBoxMount.Checked)
                {
                    _currentAction = 24; // 骑马动画 ID
                }

                additionalAnimation = AnimationEdit.GetAnimation(_fileType, animId, _currentAction, _currentDir);
                buttonShow.Text = "隐藏";
            }

            isAnimationVisible = !isAnimationVisible;
            AnimationPictureBox.Invalidate();
        }
        #endregion

        #region [ buttonShow ] 动画 男人/女人
        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (isAnimationVisible)
            {
                // 隐藏动画
                additionalAnimation = null;
                buttonShow.Text = "显示";
            }
            else
            {
                // 根据所选性别显示动画
                string selectedGender = comboBoxMenWoman.SelectedItem.ToString();
                int animId = selectedGender == "men" ? 400 : 401;

                if (checkBoxMount.Checked)
                {
                    _currentAction = 24; // 选择骑马动画时将动作设置为 24
                }

                additionalAnimation = AnimationEdit.GetAnimation(_fileType, animId, _currentAction, _currentDir);
                buttonShow.Text = "隐藏";
            }

            isAnimationVisible = !isAnimationVisible;
            AnimationPictureBox.Invalidate();
        }
        #endregion

        #region [ comboBoxMenWoman_SelectedIndexChanged ] 动画 男人/女人
        private void comboBoxMenWoman_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isAnimationVisible)
            {
                string selectedGender = comboBoxMenWoman.SelectedItem.ToString();
                int animId = selectedGender == "men" ? 400 : 401;

                if (checkBoxMount.Checked)
                {
                    _currentAction = 24; // 选择骑马动画时将动作设置为 24
                }

                additionalAnimation = AnimationEdit.GetAnimation(_fileType, animId, _currentAction, _currentDir);
                AnimationPictureBox.Invalidate();
            }
        }
        #endregion

        #region [ btnUp ]
        private void btnUp_Click(object sender, EventArgs e)
        {
            int direction = DirectionTrackBar.Value;
            int frame = FramesTrackBar.Value;

            if (checkBoxMount.Checked)
            {
                HorseRunOffsets[direction][frame][1]--;
            }
            else
            {
                Offsets[direction][frame][1]--;
            }
            AnimationPictureBox.Invalidate(); // 重绘图像
        }
        #endregion

        #region [ btnDown ]
        private void btnDown_Click(object sender, EventArgs e)
        {
            int direction = DirectionTrackBar.Value;
            int frame = FramesTrackBar.Value;

            if (checkBoxMount.Checked)
            {
                HorseRunOffsets[direction][frame][1]++;
            }
            else
            {
                Offsets[direction][frame][1]++;
            }
            AnimationPictureBox.Invalidate(); // 重绘图像
        }
        #endregion

        #region [ btnLeft ]
        private void btnLeft_Click(object sender, EventArgs e)
        {
            int direction = DirectionTrackBar.Value;
            int frame = FramesTrackBar.Value;

            if (checkBoxMount.Checked)
            {
                HorseRunOffsets[direction][frame][0]--;
            }
            else
            {
                Offsets[direction][frame][0]--;
            }
            AnimationPictureBox.Invalidate(); // 重绘图像
        }
        #endregion

        #region [ btnRight ]
        private void btnRight_Click(object sender, EventArgs e)
        {
            int direction = DirectionTrackBar.Value;
            int frame = FramesTrackBar.Value;

            if (checkBoxMount.Checked)
            {
                HorseRunOffsets[direction][frame][0]++;
            }
            else
            {
                Offsets[direction][frame][0]++;
            }
            AnimationPictureBox.Invalidate(); // 重绘图像
        }
        #endregion

        #region [ btn_ScreenShot ]
        private void btn_ScreenShot_Click(object sender, EventArgs e)
        {
            // 定义保存截图的路径
            string path = Options.OutputPath;

            // 使用当前日期和时间创建唯一文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = Path.Combine(path, $"AnimationScreenshot_{timestamp}.png");

            // 创建与 AnimationPictureBox 大小相同的位图
            using (Bitmap bmp = new Bitmap(AnimationPictureBox.Width, AnimationPictureBox.Height))
            {
                // 将 AnimationPictureBox 绘制到位图上
                AnimationPictureBox.DrawToBitmap(bmp, new Rectangle(0, 0, AnimationPictureBox.Width, AnimationPictureBox.Height));

                // 将位图保存为 .png 文件
                bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            }

            MessageBox.Show($"截图已保存到 {fileName}");
        }
        #endregion

        #region [ MapUopToolStripButton ] 切换 UOP 映射并重新加载动画
        private void MapUopToolStripButton_Click(object sender, EventArgs e)
        {
            _useUopMapping = MapUopToolStripButton.Checked;
            if (_uopManager != null)
            {
                _uopManager.IgnoreAnimationSequence = !_useUopMapping;
                LoadUopAnimations();
            }
        }
        #endregion


        #region [ ShowCrosshairtoolStripButton ] 在动画预览上切换十字线显示
        private void ShowCrosshairtoolStripButton_Click(object sender, EventArgs e)
        {
            _drawCrosshair = !_drawCrosshair;
            AnimationPictureBox.Invalidate();
        }
        #endregion


        #region 序列 UOP 选项卡

        private void InitializeSequenceTab()
        {
            _sequenceGrid.Columns.Clear();
            // 标准 UOP 列
            AddUopColumns();
            _sequenceGrid.DataSource = _sequenceBindingSource;
            _sequenceTimer.Interval = 150;
            _btnSaveMainMisc.Enabled = false;
            _gridIsInMulMode = false;
        }

        // ── 网格模式 ────────────────────────────────────────────────────────

        private bool _gridIsInMulMode = false;

        /// <summary>
        /// 将网格切换到 MUL 列。
        /// 如果 _fileType 是 MUL，总是执行此操作——没有提前返回。
        /// </summary>
        private void SetupSequenceGridForMul()
        {
            _sequenceGrid.DataSource = null;
            _sequenceGrid.Columns.Clear();
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ActionIndex", HeaderText = "动作", Width = 35, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ActionName", HeaderText = "名称", Width = 130, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FramesDir0", HeaderText = "D0", Width = 35, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FramesDir1", HeaderText = "D1", Width = 35, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FramesDir2", HeaderText = "D2", Width = 35, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FramesDir3", HeaderText = "D3", Width = 35, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FramesDir4", HeaderText = "D4", Width = 35, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CenterX", HeaderText = "CX", Width = 40, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CenterY", HeaderText = "CY", Width = 40, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdxOffset", HeaderText = "IDX 偏移", Width = 80, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdxLength", HeaderText = "IDX 长度", Width = 70, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "StatusText", HeaderText = "状态", Width = 90, ReadOnly = true });
            _gridIsInMulMode = true;
        }

        /// <summary>
        /// 将网格切换到 UOP 列。
        /// 如果 _fileType 是 UOP，总是执行此操作——没有提前返回。
        /// </summary>
        private void SetupSequenceGridForUop()
        {
            _sequenceGrid.DataSource = null;
            _sequenceGrid.Columns.Clear();
            AddUopColumns();
            _sequenceGrid.DataSource = _sequenceBindingSource;
            _gridIsInMulMode = false;
        }

        private void AddUopColumns()
        {
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UopGroupIndex", HeaderText = "UOP 组", Width = 60 });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FrameCount", HeaderText = "帧数", Width = 50 });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MulGroupIndex", HeaderText = "MUL", Width = 60 });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Speed", HeaderText = "速度", Width = 50 });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "BaseGroup", HeaderText = "基础", Width = 50, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "WarGroupId", HeaderText = "战斗", Width = 50, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "WarModifier", HeaderText = "修正", Width = 40, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PeaceGroupId", HeaderText = "和平", Width = 50, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PeaceModifier", HeaderText = "修正", Width = 40, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MountPeaceGroupId", HeaderText = "骑和平", Width = 50, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MountPeaceModifier", HeaderText = "修正", Width = 40, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MountWarGroupId", HeaderText = "骑战斗", Width = 50, ReadOnly = true });
            _sequenceGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MountWarModifier", HeaderText = "修正", Width = 40, ReadOnly = true });
        }

        // ── PopulateSequenceGrid — 调度器 ────────────────────────────────

        private void PopulateSequenceGrid(int bodyId)
        {
            if (_fileType == UOP_FILE_TYPE)
            {
                SetupSequenceGridForUop();
                PopulateSequenceGridUop(bodyId);
            }
            else if (_fileType >= 1 && _fileType <= 5)
            {
                SetupSequenceGridForMul();
                PopulateSequenceGridMul(bodyId);
            }
            else
            {
                _sequenceGrid.DataSource = null;
                _sequenceBindingSource.DataSource = null;
            }
        }

        // ── PopulateSequenceGridUop ───────────────────────────────────────────

        private void PopulateSequenceGridUop(int bodyId)
        {
            if (_uopManager == null || _uopManager.SequenceEntries == null)
            { _sequenceBindingSource.DataSource = null; return; }

            uint uid = (uint)bodyId;
            if (!_uopManager.SequenceEntries.TryGetValue(uid, out var list))
            {
                _sequenceBindingSource.DataSource = null;
                _seqCurrentAction = -1;
                _currentSeqEntry = null;
                _sequencePreviewBox.Image = null;
                return;
            }

            _sequenceViewModelList = new System.ComponentModel.BindingList<SequenceViewModelItem>();

            var warOverrides = _uopManager.GetStateOverrides((int)uid, CreatureState.War);
            var peaceOverrides = _uopManager.GetStateOverrides((int)uid, CreatureState.Peace);
            var mountPeaceOverrides = _uopManager.GetStateOverrides((int)uid, CreatureState.MountPeace);
            var mountWarOverrides = _uopManager.GetStateOverrides((int)uid, CreatureState.MountWar);

            foreach (var entry in list)
            {
                var vm = new SequenceViewModelItem(entry);
                int action = (int)entry.UopGroupIndex;

                void FillState(Dictionary<int, StateOverride> overrides,
                               Action<int?> setGroupId,
                               Action<ushort?> setMod)
                {
                    if (overrides != null && overrides.TryGetValue(action, out var ov))
                    { setGroupId(ov.GroupId); setMod(ov.Modifier); }
                    else
                    { setGroupId(vm.BaseGroup); setMod(null); }
                }

                FillState(warOverrides, s => vm.WarGroupId = s, s => vm.WarModifier = s);
                FillState(peaceOverrides, s => vm.PeaceGroupId = s, s => vm.PeaceModifier = s);
                FillState(mountPeaceOverrides, s => vm.MountPeaceGroupId = s, s => vm.MountPeaceModifier = s);
                FillState(mountWarOverrides, s => vm.MountWarGroupId = s, s => vm.MountWarModifier = s);

                _sequenceViewModelList.Add(vm);
            }

            _sequenceBindingSource.DataSource = _sequenceViewModelList;

            if (_sequenceViewModelList.Count > 0)
            {
                _seqCurrentAction = (int)_sequenceViewModelList[0].UopGroupIndex;
                _currentSeqEntry = _sequenceViewModelList[0].Entry;
                _seqPreviewFrameIndex = 0;
                if (_sequenceGrid.Rows.Count > 0)
                    _sequenceGrid.Rows[0].Selected = true;
            }

            UpdateSequencePreviewUop();
        }

        // ── PopulateSequenceGridMul ───────────────────────────────────────────

        private void PopulateSequenceGridMul(int bodyId)
        {
            var items = new System.ComponentModel.BindingList<MulSequenceViewItem>();

            int animLength = Animations.GetAnimLength(bodyId, _fileType);
            if (animLength <= 0) animLength = 22;

            // 重映射
            int resolvedBody = bodyId;
            int resolvedFileType = _fileType;

            int btBody = bodyId;
            if (Ultima.BodyTable.Entries != null
                && Ultima.BodyTable.Entries.TryGetValue(bodyId, out Ultima.BodyTableEntry btEntry)
                && !Ultima.BodyConverter.Contains(bodyId))
                btBody = btEntry.OldId;

            int bcBody = btBody;
            int bcFt = Ultima.BodyConverter.Convert(ref bcBody);
            if (bcFt >= 1 && bcFt <= 5)
            { resolvedBody = bcBody; resolvedFileType = bcFt; }

            string[] nameTable = animLength == 13
                ? new[] { "行走","奔跑","站立","进食","警觉","攻击1","攻击2",
                  "受击","死亡1","烦躁1","烦躁2","躺下","死亡2" }
                : animLength == 22
                ? new[] { "行走","站立","死亡1","死亡2","攻击1","攻击2","攻击3",
                  "弓攻击","弩攻击","投掷攻击","受击","掠夺",
                  "踩踏","施法2","施法3","右格挡","左格挡","空闲",
                  "烦躁","飞行","起飞","空中受击" }
                : new[] { "行走","持杖行走","奔跑","持杖奔跑","空闲","空闲2","烦躁",
                  "单手战斗空闲","单手战斗空闲2","单手挥砍攻击","单手穿刺攻击",
                  "单手钝击攻击","双手钝击攻击","双手挥砍攻击","双手穿刺攻击",
                  "单手战斗前进","法术1","法术2","弓攻击","弩攻击",
                  "受击","前倒死亡","后倒死亡","骑马行走","骑马奔跑","骑马空闲",
                  "骑马单手攻击","骑马弓攻击","骑马弩攻击","骑马双手攻击",
                  "盾牌格挡","拳击刺拳","小鞠躬","武装敬礼","进食" };

            string ixName = resolvedFileType == 1 ? "anim.idx" : $"anim{resolvedFileType}.idx";
            string ixPath = Ultima.Files.GetFilePath(ixName);

            for (int action = 0; action < animLength; action++)
            {
                var item = new MulSequenceViewItem
                {
                    ActionIndex = action,
                    ActionName = action < nameTable.Length ? nameTable[action] : $"动作{action}",
                };

                int totalFrames = 0;
                bool hasAnyData = false;

                for (int dir = 0; dir < 5; dir++)
                {
                    AnimIdx edit = AnimationEdit.GetAnimation(_fileType, bodyId, action, dir);
                    if (edit?.Frames == null || edit.Frames.Count == 0) continue;

                    hasAnyData = true;
                    totalFrames += edit.Frames.Count;

                    if (dir == 0) { item.CenterX = edit.Frames[0].Center.X; item.CenterY = edit.Frames[0].Center.Y; item.FramesDir0 = edit.Frames.Count; }
                    if (dir == 1) item.FramesDir1 = edit.Frames.Count;
                    if (dir == 2) item.FramesDir2 = edit.Frames.Count;
                    if (dir == 3) item.FramesDir3 = edit.Frames.Count;
                    if (dir == 4) item.FramesDir4 = edit.Frames.Count;
                }

                item.TotalFrames = totalFrames;
                item.HasData = hasAnyData;

                // 读取 IDX 条目（所有 3 个字段：偏移量、长度、额外）
                if (!string.IsNullOrEmpty(ixPath) && File.Exists(ixPath))
                {
                    long pos = (long)(resolvedBody * 110 * 5 + action * 5) * 12;
                    try
                    {
                        using var fs = new FileStream(ixPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        if (pos + 12 <= fs.Length)
                        {
                            fs.Seek(pos, SeekOrigin.Begin);
                            using var br = new BinaryReader(fs);
                            int off = br.ReadInt32();
                            int len = br.ReadInt32();
                            int ext = br.ReadInt32();
                            int act = len > 0 ? len : ext > 0 ? ext : 0;
                            item.IdxOffset = off;
                            item.IdxLength = act;
                            item.InFile = (off >= 0 && act > 0);
                            item.OnlyInCache = hasAnyData && !item.InFile;
                        }
                    }
                    catch { }
                }

                items.Add(item);
            }

            // 在设置 DataSource 时分离事件——
            // 防止在绑定期间网格自动选择行时发生 NullReferenceException
            _sequenceGrid.SelectionChanged -= OnSequenceGridSelectionChanged;
            try
            {
                _sequenceBindingSource.DataSource = items;
                _sequenceGrid.DataSource = _sequenceBindingSource;
            }
            finally
            {
                _sequenceGrid.SelectionChanged += OnSequenceGridSelectionChanged;
            }

            // 选择包含数据的第一行
            _seqCurrentAction = -1;
            for (int i = 0; i < _sequenceGrid.Rows.Count; i++)
            {
                var boundItem = _sequenceGrid.Rows[i].DataBoundItem as MulSequenceViewItem;
                if (boundItem != null && boundItem.HasData)
                {
                    _seqCurrentAction = boundItem.ActionIndex;
                    _sequenceGrid.Rows[i].Selected = true;
                    break;
                }
            }

            UpdateSequencePreviewMul();
        }

        // ── MulSequenceViewItem ───────────────────────────────────────────────

        public sealed class MulSequenceViewItem
        {
            public int ActionIndex { get; set; }
            public string ActionName { get; set; }
            public int FramesDir0 { get; set; }
            public int FramesDir1 { get; set; }
            public int FramesDir2 { get; set; }
            public int FramesDir3 { get; set; }
            public int FramesDir4 { get; set; }
            public int TotalFrames { get; set; }
            public int CenterX { get; set; }
            public int CenterY { get; set; }
            public int IdxOffset { get; set; }
            public int IdxLength { get; set; }
            public bool HasData { get; set; }
            public bool InFile { get; set; }
            public bool OnlyInCache { get; set; }

            public string StatusText =>
                !HasData ? "空" :
                OnlyInCache ? "仅缓存" :
                InFile ? "在文件中" : "?";
        }

        // ── TryFindAnimationForPreview ────────────────────────────────────────

        private UopAnimIdx TryFindAnimationForPreview(int body, int action,
            int preferredDir, out int foundDir)
        {
            foundDir = -1;
            if (_uopManager == null || body < 0 || action < 0) return null;

            int[] dirs = new int[5];
            dirs[0] = preferredDir;
            int idx = 1;
            for (int d = 0; d < 5; d++)
                if (d != preferredDir) dirs[idx++] = d;

            foreach (int dir in dirs)
            {
                var cached = _uopManager.GetUopAnimation(body, action, dir);
                if (cached != null && cached.Frames.Count > 0)
                { foundDir = dir; return cached; }

                var fileInfo = _uopManager.GetAnimationData(body, action, dir);
                if (fileInfo == null) continue;
                byte[] raw = fileInfo.GetData();
                if (raw == null || raw.Length == 0) continue;

                var afterLoad = _uopManager.GetUopAnimation(body, action, dir);
                if (afterLoad != null && afterLoad.Frames.Count > 0)
                { foundDir = dir; return afterLoad; }

                if (raw.Length >= 12)
                {
                    try
                    {
                        using var ms = new System.IO.MemoryStream(raw);
                        using var br = new System.IO.BinaryReader(ms);
                        var header = UopAnimationDataManager.ReadUopBinHeader(br);
                        if (header != null && header.FrameCount > 0)
                            System.Diagnostics.Debug.WriteLine(
                                $"[序列预览] Body={body} Act={action} Dir={dir}: " +
                                $"FrameCount={header.FrameCount} 但 GetUopAnimation=null。");
                    }
                    catch { }
                }
            }
            return null;
        }

        // ── UpdateSequencePreview ─────────────────────────────────────────────

        private void UpdateSequencePreview()
        {
            if (_fileType == UOP_FILE_TYPE) UpdateSequencePreviewUop();
            else UpdateSequencePreviewMul();
        }

        private void UpdateSequencePreviewUop()
        {
            if (_uopManager == null || _seqCurrentAction < 0)
            { _sequencePreviewBox.Image = null; return; }

            var anim = TryFindAnimationForPreview(
                _currentBody, _seqCurrentAction, _seqPreviewDirection, out _);

            if (anim == null || anim.Frames.Count == 0)
            { _sequencePreviewBox.Image = null; return; }

            if (_seqPreviewFrameIndex >= anim.Frames.Count) _seqPreviewFrameIndex = 0;
            _sequencePreviewBox.Image = anim.Frames[_seqPreviewFrameIndex].Image;
        }

        private void UpdateSequencePreviewMul()
        {
            // 防护：拦截无效状态
            if (_seqCurrentAction < 0 || _fileType == 0 || _currentBody < 0)
            {
                _sequencePreviewBox.Image = null;
                return;
            }

            // 如果当前选中的行不包含数据 → 立即取消
            if (_gridIsInMulMode && _sequenceGrid.SelectedRows.Count > 0)
            {
                var boundItem = _sequenceGrid.SelectedRows[0].DataBoundItem as MulSequenceViewItem;
                if (boundItem != null && !boundItem.HasData)
                {
                    _sequencePreviewBox.Image = null;
                    return;
                }
            }

            AnimIdx edit = AnimationEdit.GetAnimation(
                _fileType, _currentBody, _seqCurrentAction, _seqPreviewDirection);

            if (edit == null || edit.Frames?.Count == 0)
            {
                for (int dir = 0; dir < 5; dir++)
                {
                    if (dir == _seqPreviewDirection) continue;
                    edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _seqCurrentAction, dir);
                    if (edit?.Frames?.Count > 0) break;
                }
            }

            if (edit == null || edit.Frames?.Count == 0)
            {
                _sequencePreviewBox.Image = null;
                return;
            }

            if (_seqPreviewFrameIndex >= edit.Frames.Count)
                _seqPreviewFrameIndex = 0;

            var bitmaps = edit.GetFrames();
            if (bitmaps == null || bitmaps.Length == 0)
            {
                _sequencePreviewBox.Image = null;
                return;
            }

            if (_seqPreviewFrameIndex < bitmaps.Length)
                _sequencePreviewBox.Image = bitmaps[_seqPreviewFrameIndex];
            else
                _sequencePreviewBox.Image = null;
        }

        // ── OnSequenceGridSelectionChanged ───────────────────────────────────

        private void OnSequenceGridSelectionChanged(object sender, EventArgs e)
        {
            if (_sequenceGrid.SelectedRows.Count == 0) return;
            if (_fileType == 0) return;

            if (_fileType == UOP_FILE_TYPE)
            {
                var vm = _sequenceGrid.SelectedRows[0].DataBoundItem as SequenceViewModelItem;
                if (vm == null) return;  // ← 点击了空行
                _currentSeqEntry = vm.Entry;
                _seqCurrentAction = (int)vm.UopGroupIndex;
                _seqPreviewFrameIndex = 0;
                UpdateSequencePreviewUop();
            }
            else
            {
                var item = _sequenceGrid.SelectedRows[0].DataBoundItem as MulSequenceViewItem;
                if (item == null) return;  // ← 点击了空行
                _seqCurrentAction = item.ActionIndex;
                _seqPreviewFrameIndex = 0;
                UpdateSequencePreviewMul();
            }
        }

        // ── OnSequenceTimerTick ───────────────────────────────────────────────

        private void OnSequenceTimerTick(object sender, EventArgs e)
        {
            if (_fileType == UOP_FILE_TYPE)
            {
                if (_uopManager == null || _seqCurrentAction < 0) return;
                var anim = TryFindAnimationForPreview(
                    _currentBody, _seqCurrentAction, _seqPreviewDirection, out _);
                if (anim == null || anim.Frames.Count == 0) { _sequencePreviewBox.Image = null; return; }
                _seqPreviewFrameIndex++;
                if (_seqPreviewFrameIndex >= anim.Frames.Count) _seqPreviewFrameIndex = 0;
                _sequencePreviewBox.Image = anim.Frames[_seqPreviewFrameIndex].Image;
            }
            else
            {
                if (_seqCurrentAction < 0) return;
                AnimIdx edit = AnimationEdit.GetAnimation(
                    _fileType, _currentBody, _seqCurrentAction, _seqPreviewDirection);
                if (edit == null || edit.Frames?.Count == 0) return;
                var bitmaps = edit.GetFrames();
                if (bitmaps == null) return;
                _seqPreviewFrameIndex++;
                if (_seqPreviewFrameIndex >= bitmaps.Length) _seqPreviewFrameIndex = 0;
                _sequencePreviewBox.Image = bitmaps[_seqPreviewFrameIndex];
            }
        }

        // ── OnSequenceGridCellValueChanged ───────────────────────────────────

        private void OnSequenceGridCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_fileType != UOP_FILE_TYPE) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || _uopManager == null || _currentBody == -1) return;

            var grid = (DataGridView)sender;
            var vm = grid.Rows[e.RowIndex].DataBoundItem as SequenceViewModelItem;
            if (vm == null) return;

            if (grid.IsCurrentCellDirty)
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);

            bool isFrameCountChange = grid.Columns[e.ColumnIndex].DataPropertyName == "FrameCount";
            try
            {
                _uopManager.UpdateSequenceEntry(
                    (uint)_currentBody, vm.UopGroupIndex,
                    vm.FrameCount, vm.MulGroupIndex, vm.Speed,
                    vm.Entry.ExtraData, autoPopulate: isFrameCountChange);
                if (isFrameCountChange) grid.InvalidateRow(e.RowIndex);
                UpdateSequencePreviewUop();
            }
            catch (Exception ex)
            { MessageBox.Show($"更新序列条目时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── 保存 / 克隆 / MainMisc ───────────────────────────────────────────

        private void OnSaveSequenceClick(object sender, EventArgs e)
        {
            if (_uopManager == null || _sequenceViewModelList == null) return;
            using var dialog = new SaveFileDialog
            { InitialDirectory = Options.OutputPath, FileName = "AnimationSequence.uop", Filter = "UOP 文件 (*.uop)|*.uop", Title = "保存动画序列" };
            if (dialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                _uopManager.SequenceEntries[(uint)_currentBody] = _sequenceViewModelList.Select(vm => vm.Entry).ToList();
                _uopManager.SaveAnimationSequence(dialog.FileName);
                MessageBox.Show($"成功保存到：\n{dialog.FileName}");
            }
            catch (Exception ex) { MessageBox.Show($"保存时出错：{ex.Message}"); }
        }

        private void OnSaveBinClick(object sender, EventArgs e)
        {
            if (_uopManager == null || _sequenceViewModelList == null) return;
            using var dialog = new SaveFileDialog
            { InitialDirectory = Options.OutputPath, FileName = $"Sequence_{_currentBody}.bin", Filter = "二进制文件 (*.bin)|*.bin", Title = "保存序列二进制数据" };
            if (dialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                File.WriteAllBytes(dialog.FileName, _uopManager.GetBinaryDataForAnimationId((uint)_currentBody));
                MessageBox.Show($"成功保存 .bin 到：\n{dialog.FileName}");
            }
            catch (Exception ex) { MessageBox.Show($"保存 .bin 时出错：{ex.Message}"); }
        }

        private void OnCloneSequenceIdClick(object sender, EventArgs e)
        {
            if (_uopManager == null) return;
            string srcInput = ShowInputDialog("输入源动画 ID：", "克隆序列 ID", "");
            if (!uint.TryParse(srcInput, out uint srcId)) { MessageBox.Show("无效的源 ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (!_uopManager.SequenceEntries.ContainsKey(srcId)) { MessageBox.Show($"未找到源 ID {srcId}。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            string newInput = ShowInputDialog("输入新动画 ID：", "克隆序列 ID", "");
            if (!uint.TryParse(newInput, out uint newId)) { MessageBox.Show("无效的新 ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (_uopManager.SequenceEntries.ContainsKey(newId))
                if (MessageBox.Show($"ID {newId} 已存在。是否覆盖？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                _uopManager.CloneAnimationSequenceEntry(srcId, newId);
                _uopManager.EnsureIdInMainMisc(newId, forceCreatureFlag: true);
                RefreshMainMiscButtonState();
                LoadUopAnimations();
                MessageBox.Show($"已克隆 {srcId} → {newId}。\n记得保存 MainMisc 表。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show($"克隆时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void OnAddCurrentToMainMiscClick(object sender, EventArgs e)
        {
            if (_uopManager == null || _currentBody == -1) return;
            uint uid = (uint)_currentBody;
            const uint CREATURE_FLAG = 0x0C000000;
            if (_uopManager.IsIdInMainMisc(uid) && _uopManager.GetMainMiscFlag(uid) == CREATURE_FLAG)
            { MessageBox.Show($"ID {uid} 已存在。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            _uopManager.EnsureIdInMainMisc(uid, forceCreatureFlag: true);
            RefreshMainMiscButtonState();
            MessageBox.Show($"ID {uid} 已添加到 MainMisc。\n别忘了保存。", "ID 已添加", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnSyncMainMiscClick(object sender, EventArgs e)
        {
            if (_uopManager == null) return;
            Cursor = Cursors.WaitCursor;
            try
            {
                int missing = _uopManager.CheckMissingMainMiscEntries(dryRun: true);
                if (missing > 0)
                {
                    if (MessageBox.Show($"找到 {missing} 个缺失条目。\n立即添加？", "同步", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int added = _uopManager.CheckMissingMainMiscEntries(dryRun: false);
                        RefreshMainMiscButtonState();
                        MessageBox.Show($"已添加 {added} 个 ID。", "同步完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    MessageBox.Show("MainMisc 是最新的。", "同步完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally { Cursor = Cursors.Default; }
        }

        private void RefreshMainMiscButtonState()
        {
            if (_btnSaveMainMisc != null && _uopManager != null)
            {
                _btnSaveMainMisc.Enabled = _uopManager.MainMiscModified;
                System.Diagnostics.Debug.WriteLine($"[MainMisc] 已启用={_btnSaveMainMisc.Enabled}");
            }
        }

        private void OnSaveMainMiscClick(object sender, EventArgs e)
        {
            if (_uopManager == null) return;
            using var dialog = new SaveFileDialog
            { InitialDirectory = Options.OutputPath, FileName = "MainMisc.uop", Filter = "UOP 文件 (*.uop)|*.uop", Title = "保存 MainMisc 表" };
            if (dialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                _uopManager.SaveMainMisc(dialog.FileName);
                MessageBox.Show($"MainMisc.uop 已保存到：\n{dialog.FileName}\n\n条目数：{_uopManager.GetMainMiscEntryCount()}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show($"保存 MainMisc 时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── ShowInputDialog ───────────────────────────────────────────────────

        private string ShowInputDialog(string text, string caption, string defaultValue)
        {
            using var prompt = new Form { Width = 500, Height = 150, FormBorderStyle = FormBorderStyle.FixedDialog, Text = caption, StartPosition = FormStartPosition.CenterScreen };
            var lbl = new Label { Left = 50, Top = 20, Width = 400, Text = text };
            var tb = new TextBox { Left = 50, Top = 50, Width = 400, Text = defaultValue };
            var btn = new Button { Text = "确定", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            btn.Click += (s, a) => prompt.Close();
            prompt.Controls.AddRange(new Control[] { lbl, tb, btn });
            prompt.AcceptButton = btn;
            return prompt.ShowDialog() == DialogResult.OK ? tb.Text : "";
        }

        // ── 按钮 ───────────────────────────────────────────────────────────

        private void BtnSeqPlay_Click(object sender, EventArgs e) => _sequenceTimer.Start();
        private void BtnSeqStop_Click(object sender, EventArgs e) => _sequenceTimer.Stop();

        private void SeqDirTrackBar_ValueChanged(object sender, EventArgs e)
        {
            var tb = (TrackBar)sender;
            _seqPreviewDirection = tb.Value;
            _seqDirLabel.Text = $"方向：{_seqPreviewDirection}";
            _seqPreviewFrameIndex = 0;
            UpdateSequencePreview();
        }

        #endregion

        #region [ 十六进制编辑器 ] - 按钮处理程序和打开带有相应数据的十六进制编辑器的逻辑

        private AnimationHexEditorForm _hexEditor;

        private void btnOpenHexEditor_Click(object sender, EventArgs e)
        {
            if (_fileType == 0)
            {
                MessageBox.Show("请先选择一个动画文件。",
                    "十六进制编辑器", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (AnimationListTreeView.SelectedNode == null)
            {
                MessageBox.Show("请先选择一个动画。",
                    "十六进制编辑器", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_hexEditor == null || _hexEditor.IsDisposed)
            {
                _hexEditor = new AnimationHexEditorForm();

                _hexEditor.OnByteWritten += (absoluteOffset, newByte) =>
                {
                    AnimationPictureBox.Invalidate();
                };

                // ── 从 HexEditor 连接“固定为 A / 固定为 B”到 HexCompareForm
                _hexEditor.OnPinAsA += buf =>
                {
                    EnsureHexCompare();
                    _hexCompare.LoadBufferA(buf);
                    _hexCompare.Show();
                    _hexCompare.BringToFront();
                };

                _hexEditor.OnPinAsB += buf =>
                {
                    EnsureHexCompare();
                    _hexCompare.LoadBufferB(buf);
                    _hexCompare.Show();
                    _hexCompare.BringToFront();
                };
            }

            if (_fileType == 6)
                OpenHexEditorUop();
            else
                OpenHexEditorMul();

            _hexEditor.Show();
            _hexEditor.BringToFront();
        }

        private void EnsureHexCompare()
        {
            if (_hexCompare == null || _hexCompare.IsDisposed)
                _hexCompare = new HexCompareForm();
        }

        // ──────────────────────────────────────────────────────────────────────
        //  UOP – 从 UopManager 检索原始数据和区域
        // ──────────────────────────────────────────────────────────────────────

        private void OpenHexEditorUop()
        {
            if (_uopManager == null) return;

            // 获取原始数据 - 首先尝试当前方向，然后尝试所有其他方向
            var fileInfo = _uopManager.GetAnimationData(_currentBody, _currentAction, _currentDir);
            byte[] rawData = null;
            long fileOffset = 0;          // ← 使用 0，因为 DataOffset 不存在
            string filePath = "(UOP – 内存)";

            if (fileInfo != null)
            {
                rawData = fileInfo.GetData();
                // fileInfo.DataOffset 不存在 → 使用 0 作为占位符
                fileOffset = 0;
                filePath = fileInfo.File?.FilePath ?? filePath;
            }

            // 后备：尝试所有方向
            if (rawData == null || rawData.Length == 0)
            {
                for (int dir = 0; dir < 5; dir++)
                {
                    var fi = _uopManager.GetAnimationData(_currentBody, _currentAction, dir);
                    if (fi == null) continue;
                    byte[] d = fi.GetData();
                    if (d != null && d.Length > 0)
                    {
                        rawData = d;
                        fileOffset = 0;   // ← 同样使用 0
                        filePath = fi.File?.FilePath ?? filePath;
                        break;
                    }
                }
            }

            if (rawData == null || rawData.Length == 0)
            {
                MessageBox.Show("未找到此动画的原始数据。",
                    "十六进制编辑器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var regions = BuildUopRegions(rawData);
            var preview = GetCurrentFrameBitmap();

            _hexEditor.LoadUopAnimation(rawData, fileOffset, filePath,
                _currentBody, _currentAction, _currentDir, _currentAction, regions);

            if (preview != null)
                _hexEditor.SetPreviewImage(preview, regions.Count > 0 ? regions[0] : null);
        }

        // ──────────────────────────────────────────────────────────────────────
        //  MUL – 直接从 idx/mul 文件读取原始数据
        // ──────────────────────────────────────────────────────────────────────       

        private void OpenHexEditorMul()
        {
            //DebugBodyIdxEntry(_currentBody); // ← 临时，调试后移除

            // 1. 通过 SDK 获取 AnimIdx — 与显示帧时完全相同
            AnimIdx edit = AnimationEdit.GetAnimation(
                _fileType, _currentBody, _currentAction, _currentDir);

            if (edit == null)
            {
                MessageBox.Show(
                    $"未找到身体 {_currentBody}、动作 {_currentAction}、方向 {_currentDir} 的 AnimIdx 数据。",
                    "十六进制编辑器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bitmap[] frames = edit.GetFrames();
            if (frames == null || frames.Length == 0)
            {
                MessageBox.Show(
                    $"身体 {_currentBody} 的 AnimIdx 没有帧。",
                    "十六进制编辑器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] rawData = null;
            long dataOffset = 0;
            string mulPath = null;

            rawData = TryFindRawDataForAnimIdx(edit, out dataOffset, out mulPath, out int mappedBody);

            // 3. 如果找到原始数据：用实际的 MUL 文件填充十六进制编辑器。
            if (rawData != null && rawData.Length > 0)
            {
                var regions = BuildMulRegions(rawData, edit);
                var preview = GetCurrentFrameBitmap();

                _hexEditor.LoadMulAnimation(rawData, dataOffset, mulPath,
                    mappedBody, _currentAction, _currentDir,
                    FramesTrackBar.Value, regions);

                if (preview != null)
                    _hexEditor.SetPreviewImage(preview,
                        regions.Count > 0 ? regions[0] : null);
                return;
            }

            // 4. 后备：从 AnimIdx 帧重建数据
            // （仅存在于 RAM 缓存中的身体，例如导入的身体）
            rawData = SerializeAnimIdxToMulFormat(edit);
            if (rawData == null)
            {
                MessageBox.Show("序列化 AnimIdx 数据时出错。",
                    "十六进制编辑器", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string pseudoPath = $"(RAM-缓存：身体 {_currentBody} / " +
                                $"anim{_fileType}.mul 未找到)";
            var regions2 = BuildMulRegions(rawData, edit);
            var preview2 = GetCurrentFrameBitmap();

            _hexEditor.LoadMulAnimation(rawData, 0, pseudoPath,
                mappedBody, _currentAction, _currentDir,
                FramesTrackBar.Value, regions2);

            if (preview2 != null)
                _hexEditor.SetPreviewImage(preview2,
                    regions2.Count > 0 ? regions2[0] : null);
        }

        /*
         * 此方法仅用于调试目的。它为给定的身体读取相应的 IDX 条目，并记录有关它的所有相关信息，包括：
         * - 计算的 IDX 条目位置
         * - 条目是否在 IDX 文件的边界内
         * - IDX 条目的原始偏移量、长度和额外字段
         * - 偏移量是否有效（不是负数或 0xFFFFFFFF）
         * - 与此身体相关的 body.def 和 bodyconv.def 信息
         * - SDK 返回的 AnimIdx 实例的反射数据
         */
        private void DebugBodyIdxEntry(int body)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"=== 调试身体 {body} 文件类型 {_fileType} 动作 {_currentAction} 方向 {_currentDir} ===");

            string mulName = _fileType == 1 ? "anim.mul" : $"anim{_fileType}.mul";
            string idxName = _fileType == 1 ? "anim.idx" : $"anim{_fileType}.idx";
            string mulPath = Ultima.Files.GetFilePath(mulName);
            string idxPath = Ultima.Files.GetFilePath(idxName);

            long idxEntry = (long)(body * 110 * 5 + _currentAction * 5 + _currentDir) * 12;
            sb.AppendLine($"IDX 条目位置：{idxEntry} (0x{idxEntry:X})");

            try
            {
                using var idxFs = new System.IO.FileStream(idxPath,
                    System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                sb.AppendLine($"IDX 文件大小：{idxFs.Length} 字节");
                sb.AppendLine($"IDX 条目在边界内：{idxEntry + 12 <= idxFs.Length}");
                if (idxEntry + 12 <= idxFs.Length)
                {
                    idxFs.Seek(idxEntry, System.IO.SeekOrigin.Begin);
                    using var br = new System.IO.BinaryReader(idxFs);
                    int rawOff = br.ReadInt32();
                    int rawLen = br.ReadInt32();
                    int rawExt = br.ReadInt32();
                    sb.AppendLine($"IDX 偏移量    : 0x{rawOff:X8} ({rawOff})");
                    sb.AppendLine($"IDX 长度    : {rawLen} (0x{rawLen:X})");
                    sb.AppendLine($"IDX 额外     : {rawExt} (0x{rawExt:X})");
                    sb.AppendLine($"偏移量有效  : {rawOff >= 0 && rawOff != unchecked((int)0xFFFFFFFF)}");
                }
            }
            catch (Exception ex) { sb.AppendLine($"IDX 读取错误：{ex.Message}"); }

            sb.AppendLine();
            sb.AppendLine("--- SDK AnimIdx ---");
            AnimIdx editForReflection = null;
            try
            {
                editForReflection = AnimationEdit.GetAnimation(_fileType, body, _currentAction, _currentDir);
                if (editForReflection == null)
                {
                    sb.AppendLine("SDK：edit == null");
                }
                else
                {
                    var frames = editForReflection.GetFrames();
                    sb.AppendLine($"SDK：edit != null");
                    sb.AppendLine($"SDK：Frames.Count = {editForReflection.Frames?.Count}");
                    sb.AppendLine($"SDK：GetFrames() = {frames?.Length} 位图");
                    if (frames != null && frames.Length > 0 && frames[0] != null)
                        sb.AppendLine($"SDK：Frame[0] 大小 = {frames[0].Width}x{frames[0].Height}");
                }
            }
            catch (Exception ex) { sb.AppendLine($"SDK 错误：{ex.Message}"); }



            // ── body.def 检查 ───────────────────────────────────────────────────
            sb.AppendLine();
            sb.AppendLine("--- body.def (BodyTable) ---");
            if (Ultima.BodyTable.Entries != null
                && Ultima.BodyTable.Entries.TryGetValue(body, out var btEntry))
                sb.AppendLine($"body.def：身体 {body} → OldId {btEntry.OldId}");
            else
                sb.AppendLine($"body.def：身体 {body} 未列出");

            // ── BodyConverter 检查 ──────────────────────────────────────────────
            sb.AppendLine();
            sb.AppendLine("--- BodyConverter (bodyconv.def) ---");
            int bcBody = body;
            int bcFileType = Ultima.BodyConverter.Convert(ref bcBody);
            sb.AppendLine($"BodyConverter.Convert：身体 {body} → 解析后身体 {bcBody}  文件类型 {bcFileType}");
            sb.AppendLine($"BodyConverter.Contains({body})：{Ultima.BodyConverter.Contains(body)}");

            // ── AnimIdx 反射 ───────────────────────────────────────────────
            sb.AppendLine();
            sb.AppendLine("--- AnimIdx 反射 ---");
            if (editForReflection != null)
            {
                try
                {
                    var type = editForReflection.GetType();
                    sb.AppendLine($"类型：{type.FullName}");
                    sb.AppendLine("字段：");
                    foreach (var f in type.GetFields(
                        System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Public))
                    {
                        try { sb.AppendLine($"  [{f.FieldType.Name}] {f.Name} = {f.GetValue(editForReflection)}"); }
                        catch { sb.AppendLine($"  {f.Name} = [错误]"); }
                    }
                    sb.AppendLine("属性：");
                    foreach (var p in type.GetProperties(
                        System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Public))
                    {
                        try { sb.AppendLine($"  [{p.PropertyType.Name}] {p.Name} = {p.GetValue(editForReflection)}"); }
                        catch { sb.AppendLine($"  {p.Name} = [错误]"); }
                    }
                    // 在属性块之后但在写入文件之前：
                    sb.AppendLine();
                    sb.AppendLine("--- FrameEdit 反射 (Frame[0]) ---");
                    try
                    {
                        var edit2 = AnimationEdit.GetAnimation(_fileType, body, _currentAction, _currentDir);
                        if (edit2?.Frames != null && edit2.Frames.Count > 0)
                        {
                            var frame0 = edit2.Frames[0];
                            var ftype = frame0.GetType();
                            sb.AppendLine($"FrameEdit 类型：{ftype.FullName}");
                            sb.AppendLine("字段：");
                            foreach (var f in ftype.GetFields(
                                System.Reflection.BindingFlags.Instance |
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Public))
                            {
                                try { sb.AppendLine($"  [{f.FieldType.Name}] {f.Name} = {f.GetValue(frame0)}"); }
                                catch { sb.AppendLine($"  {f.Name} = [错误]"); }
                            }
                            sb.AppendLine("属性：");
                            foreach (var p in ftype.GetProperties(
                                System.Reflection.BindingFlags.Instance |
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Public))
                            {
                                try { sb.AppendLine($"  [{p.PropertyType.Name}] {p.Name} = {p.GetValue(frame0)}"); }
                                catch { sb.AppendLine($"  {p.Name} = [错误]"); }
                            }
                        }
                    }
                    catch (Exception ex) { sb.AppendLine($"FrameEdit 反射错误：{ex.Message}"); }
                }
                catch (Exception ex) { sb.AppendLine($"反射错误：{ex.Message}"); }
            }
            else
            {
                sb.AppendLine("(edit == null，无法反射)");
            }

            // ── 写入文件 + MessageBox ──────────────────────────────────
            string dumpPath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                $"AnimIdxDump_Body{body}_FT{_fileType}_A{_currentAction}_D{_currentDir}.txt");
            System.IO.File.WriteAllText(dumpPath, sb.ToString());

            MessageBox.Show(
                $"转储已保存：\n{dumpPath}\n\n" +
                sb.ToString().Substring(0, Math.Min(1000, sb.Length)),
                $"调试身体 {body}",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // ──────────────────────────────────────────────────────────────────────
        //  为 AnimIdx 在所有动画文件中搜索原始数据。
        //  将 IDX 中的帧数与 edit.Frames.Count 进行比较。
        // ──────────────────────────────────────────────────────────────────────

        private byte[] TryFindRawDataForAnimIdx(AnimIdx edit, out long foundOffset, out string foundMulPath, out int foundMappedBody)
        {
            foundOffset = 0;
            foundMulPath = null;
            foundMappedBody = _currentBody;

            string mulName = _fileType == 1 ? "anim.mul" : $"anim{_fileType}.mul";
            string idxName = _fileType == 1 ? "anim.idx" : $"anim{_fileType}.idx";
            string mulPathFb = Ultima.Files.GetFilePath(mulName);
            string idxPathFb = Ultima.Files.GetFilePath(idxName);

            bool mulExists = !string.IsNullOrEmpty(mulPathFb) && System.IO.File.Exists(mulPathFb);
            bool idxExists = !string.IsNullOrEmpty(idxPathFb) && System.IO.File.Exists(idxPathFb);
            if (!mulExists || !idxExists) return null;

            // ── 从 SDK-FrameEdit 检索指纹 ─────────────────────────────────────────────
            // 仅使用 W + H + frameCount — 不使用 CX/CY，因为 SDK 中心 != MUL 标头 CX/CY
            int refW = -1, refH = -1;
            int refCount = edit?.Frames?.Count ?? -1;
            if (edit?.Frames != null && edit.Frames.Count > 0)
            {
                var f0 = edit.Frames[0];
                var ftype = f0.GetType();
                var fW = ftype.GetField("Width",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Public);
                var fH = ftype.GetField("Height",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Public);
                if (fW != null) refW = (int)fW.GetValue(f0);
                if (fH != null) refH = (int)fH.GetValue(f0);
            }

            // ── 步骤 1：直接使用 _currentBody 并验证 ────────────────────
            var candidate = ReadRawFromIdx(_currentBody, _fileType, idxPathFb, mulPathFb,
                out long off1, out string path1);
            if (candidate != null && VerifyBlockEx(candidate, refW, refH, refCount))
            {
                foundOffset = off1; foundMulPath = path1;
                foundMappedBody = _currentBody;
                return candidate;
            }

            // ── 步骤 2：bodyconv.def ───────────────────────────────────────────
            string bodyconvPath = Ultima.Files.GetFilePath("bodyconv.def");
            if (!string.IsNullOrEmpty(bodyconvPath) && System.IO.File.Exists(bodyconvPath))
            {
                int targetCol = _fileType - 1;
                if (targetCol >= 1 && targetCol <= 4)
                {
                    try
                    {
                        foreach (string rawLine in System.IO.File.ReadLines(bodyconvPath))
                        {
                            int ci = rawLine.IndexOf('#');
                            string clean = (ci >= 0 ? rawLine.Substring(0, ci) : rawLine).Trim();
                            if (clean.Length == 0) continue;
                            string[] parts = clean.Split(
                                new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length < 2) continue;
                            if (!int.TryParse(parts[0], out int lineBody)) continue;
                            if (lineBody != _currentBody) continue;
                            if (targetCol >= parts.Length) break;
                            if (!int.TryParse(parts[targetCol], out int mappedBody)) break;
                            if (mappedBody < 0) break;
                            var r = ReadRawFromIdx(mappedBody, _fileType, idxPathFb, mulPathFb,
                                out long off2, out string path2);
                            if (r != null && VerifyBlockEx(r, refW, refH, refCount))
                            {
                                foundOffset = off2; foundMulPath = path2;
                                foundMappedBody = mappedBody;
                                return r;
                            }
                            break;
                        }
                    }
                    catch { }
                }
            }

            // ── 步骤 3：body.def ───────────────────────────────────────────────
            if (Ultima.BodyTable.Entries != null
                && Ultima.BodyTable.Entries.TryGetValue(_currentBody, out Ultima.BodyTableEntry btEntry))
            {
                var r = ReadRawFromIdx(btEntry.OldId, _fileType, idxPathFb, mulPathFb,
                    out long off3, out string path3);
                if (r != null && VerifyBlockEx(r, refW, refH, refCount))
                {
                    foundOffset = off3; foundMulPath = path3;
                    foundMappedBody = btEntry.OldId;
                    return r;
                }
            }

            // ── 步骤 4：扫描整个 IDX ──────────────────────────────────
            // 仅当所有其他步骤都失败时。
            // 每个条目读取 600 字节 — Frame[0] 可以在 >530 的偏移量处。
            if (refW > 0 && refH > 0)
            {
                try
                {
                    using var idxFs = new System.IO.FileStream(idxPathFb,
                        System.IO.FileMode.Open, System.IO.FileAccess.Read,
                        System.IO.FileShare.Read);
                    using var mulFs = new System.IO.FileStream(mulPathFb,
                        System.IO.FileMode.Open, System.IO.FileAccess.Read,
                        System.IO.FileShare.Read);
                    using var idxBr = new System.IO.BinaryReader(idxFs);

                    long entryCount = idxFs.Length / 12;
                    for (long e = 0; e < entryCount; e++)
                    {
                        idxFs.Seek(e * 12, System.IO.SeekOrigin.Begin);
                        int rawOff = idxBr.ReadInt32();
                        int rawLen = idxBr.ReadInt32();
                        idxBr.ReadInt32();

                        if (rawOff < 0 || rawOff == unchecked((int)0xFFFFFFFF)) continue;
                        if (rawLen < 520) continue;
                        if ((long)rawOff + rawLen > mulFs.Length) continue;

                        mulFs.Seek(rawOff, System.IO.SeekOrigin.Begin);
                        var hdr = new byte[600];
                        int hdrRead = mulFs.Read(hdr, 0, 600);
                        if (hdrRead < 520) continue;

                        ushort fc = (ushort)(hdr[512] | (hdr[513] << 8));
                        if (refCount > 0 && fc != refCount) continue;
                        if (fc == 0 || fc > 256) continue;

                        int relOff = hdr[514 + 2] | (hdr[514 + 3] << 8);
                        int absOff = 512 + relOff;
                        if (absOff + 8 > hdrRead) continue;

                        ushort w = (ushort)(hdr[absOff + 4] | (hdr[absOff + 5] << 8));
                        ushort h = (ushort)(hdr[absOff + 6] | (hdr[absOff + 7] << 8));
                        if (w != refW || h != refH) continue;

                        // 匹配 — 加载整个块
                        mulFs.Seek(rawOff, System.IO.SeekOrigin.Begin);
                        var buf = new byte[rawLen];
                        int read = mulFs.Read(buf, 0, buf.Length);
                        if (read < 514) continue;
                        if (read < buf.Length) System.Array.Resize(ref buf, read);

                        foundOffset = rawOff;
                        foundMulPath = mulPathFb;
                        foundMappedBody = (int)(e / (110 * 5));
                        return buf;
                    }
                }
                catch { }
            }
            return null;
        }

        private bool VerifyBlockEx(byte[] data, int refW, int refH, int refCount)
        {
            if (data == null || data.Length < 520) return false;
            if (refW <= 0 || refH <= 0) return true;

            ushort fc = (ushort)(data[512] | (data[513] << 8));
            if (refCount > 0 && fc != refCount) return false;
            if (fc == 0 || fc > 256) return false;

            int relOff = data[514 + 2] | (data[514 + 3] << 8);
            int absOff = 512 + relOff;
            if (absOff + 8 > data.Length) return false;

            ushort w = (ushort)(data[absOff + 4] | (data[absOff + 5] << 8));
            ushort h = (ushort)(data[absOff + 6] | (data[absOff + 7] << 8));
            return w == refW && h == refH;
        }

        private byte[] ReadRawFromIdx(int body, int ft, string idxPath, string mulPath,
            out long foundOffset, out string foundMulPath)
        {
            foundOffset = 0;
            foundMulPath = null;

            long idxEntry = (long)(body * 110 * 5 + _currentAction * 5 + _currentDir) * 12;
            try
            {
                using var idxFs = new System.IO.FileStream(idxPath,
                    System.IO.FileMode.Open, System.IO.FileAccess.Read,
                    System.IO.FileShare.Read);
                if (idxEntry + 12 > idxFs.Length) return null;

                idxFs.Seek(idxEntry, System.IO.SeekOrigin.Begin);
                using var br = new System.IO.BinaryReader(idxFs);
                int rawOff = br.ReadInt32();
                int rawLen = br.ReadInt32();
                int rawExt = br.ReadInt32();

                if (rawOff < 0 || rawOff == unchecked((int)0xFFFFFFFF)) return null;

                int actualLen = rawLen > 0 ? rawLen : rawExt > 0 ? rawExt : 0;
                if (actualLen <= 0) return null;

                using var mulFs = new System.IO.FileStream(mulPath,
                    System.IO.FileMode.Open, System.IO.FileAccess.Read,
                    System.IO.FileShare.Read);
                if (rawOff + actualLen > mulFs.Length) return null;

                mulFs.Seek(rawOff, System.IO.SeekOrigin.Begin);
                var buf = new byte[actualLen];
                int read = mulFs.Read(buf, 0, buf.Length);
                if (read < 514) return null;

                if (read < buf.Length) System.Array.Resize(ref buf, read);
                foundOffset = rawOff;
                foundMulPath = mulPath;
                return buf;
            }
            catch { return null; }
        }


        // ──────────────────────────────────────────────────────────────────────
        //  将 AnimIdx 帧序列化为 MUL 格式（RAM 缓存的后备）
        //  格式：调色板(512) + 帧计数(2) + 查找表(N*4) + 帧数据
        // ──────────────────────────────────────────────────────────────────────

        private byte[] SerializeAnimIdxToMulFormat(AnimIdx edit)
        {
            try
            {
                if (edit?.Frames == null) return null;

                using var ms = new System.IO.MemoryStream();
                using var bw = new System.IO.BinaryWriter(ms);

                // 调色板 (256 × uint16 = 512 字节)
                for (int i = 0; i < 256; i++)
                    bw.Write(i < edit.Palette.Length ? edit.Palette[i] : (ushort)0x8000);

                // 帧计数
                ushort fc = (ushort)edit.Frames.Count;
                bw.Write(fc);

                // 查找表：占位符，稍后填充
                long lookupStart = ms.Position;
                for (int i = 0; i < fc; i++) bw.Write((int)0);

                // 写入帧数据并存储偏移量
                var offsets = new int[fc];
                var bitmaps = edit.GetFrames();

                for (int i = 0; i < fc; i++)
                {
                    offsets[i] = (int)ms.Position;

                    var frame = edit.Frames[i];
                    var bmp = (bitmaps != null && i < bitmaps.Length) ? bitmaps[i] : null;

                    // 帧标头：CX (int16), CY (int16), 宽度 (uint16), 高度 (uint16)
                    bw.Write((short)(frame?.Center.X ?? 0));
                    bw.Write((short)(frame?.Center.Y ?? 0));

                    if (bmp != null)
                    {
                        bw.Write((ushort)bmp.Width);
                        bw.Write((ushort)bmp.Height);

                        // 像素数据（简化：RLE 太复杂）
                        // 我们只需将像素值作为 uint16 写入
                        for (int y = 0; y < bmp.Height; y++)
                            for (int x = 0; x < bmp.Width; x++)
                            {
                                var c = bmp.GetPixel(x, y);
                                ushort v = c.A == 0 ? (ushort)0
                                    : (ushort)(0x8000
                                        | ((c.R >> 3) << 10)
                                        | ((c.G >> 3) << 5)
                                        | (c.B >> 3));
                                bw.Write(v);
                            }
                    }
                    else
                    {
                        bw.Write((ushort)0);
                        bw.Write((ushort)0);
                    }
                }

                // 用实际偏移量填充查找表
                long endPos = ms.Position;
                ms.Seek(lookupStart, System.IO.SeekOrigin.Begin);
                for (int i = 0; i < fc; i++) bw.Write(offsets[i]);
                ms.Seek(endPos, System.IO.SeekOrigin.Begin);

                return ms.ToArray();
            }
            catch { return null; }
        }

        // ──────────────────────────────────────────────────────────────────────
        //  为 UOP 构建区域
        //  分析原始数据块并标记标头和帧起始位置。
        // ──────────────────────────────────────────────────────────────────────

        private List<HexRegion> BuildUopRegions(byte[] data)
        {
            var regions = new List<HexRegion>();
            if (data == null || data.Length < 8) return regions;

            // 28 个序列的颜色（循环）
            System.Drawing.Color[] palette =
            {
        System.Drawing.Color.FromArgb(60, 100, 200, 255),
        System.Drawing.Color.FromArgb(60, 255, 140,  0),
        System.Drawing.Color.FromArgb(60,   0, 200, 80),
        System.Drawing.Color.FromArgb(60, 200,  50, 50),
        System.Drawing.Color.FromArgb(60, 180,  60, 220),
        System.Drawing.Color.FromArgb(60,  60, 200, 200),
        System.Drawing.Color.FromArgb(60, 220, 220,  50),
    };

            try
            {
                using var ms = new System.IO.MemoryStream(data);
                using var br = new System.IO.BinaryReader(ms);

                // UOP 标头（根据 UopAnimationDataManager 固定为 24 字节）
                int headerLen = 24;
                if (data.Length >= headerLen)
                {
                    regions.Add(new HexRegion
                    {
                        Offset = 0,
                        Length = headerLen,
                        Label = "UOP 标头",
                        Tooltip = $"身体:{_currentBody}  动作:{_currentAction}",
                        HighlightColor = System.Drawing.Color.FromArgb(80, 255, 215, 0),
                        IsSequenceStart = true,
                        SequenceIndex = 0,
                        DirectionIndex = _currentDir
                    });
                }

                // 帧表：从偏移量 24 开始，每帧 12 字节（偏移量 + 长度 + 额外）
                // 帧计数存储在字节 8-11（int32）
                if (data.Length >= 12)
                {
                    br.BaseStream.Seek(8, System.IO.SeekOrigin.Begin);
                    int frameCount = br.ReadInt32();

                    if (frameCount > 0 && frameCount < 10000)
                    {
                        long tableOffset = headerLen;
                        long tableLen = frameCount * 12;

                        if (tableOffset + tableLen <= data.Length)
                        {
                            regions.Add(new HexRegion
                            {
                                Offset = tableOffset,
                                Length = tableLen,
                                Label = $"帧表 ({frameCount} 帧)",
                                Tooltip = "每帧的偏移量/长度（每条目 12 B）",
                                HighlightColor = System.Drawing.Color.FromArgb(50, 100, 255, 100),
                                IsSequenceStart = false
                            });

                            // 标记各个帧数据
                            br.BaseStream.Seek(tableOffset, System.IO.SeekOrigin.Begin);
                            for (int i = 0; i < frameCount && i < 28; i++)
                            {
                                int fOff = br.ReadInt32();
                                int fLen = br.ReadInt32();
                                br.ReadInt32(); // 额外

                                if (fOff < 0 || fLen <= 0) continue;
                                if (fOff + fLen > data.Length) continue;

                                var color = palette[i % palette.Length];
                                var preview = GetFramePreviewBitmap(i);

                                regions.Add(new HexRegion
                                {
                                    Offset = fOff,
                                    Length = fLen,
                                    Label = $"帧 {i}  方向 {_currentDir}",
                                    Tooltip = $"长度：{fLen} 字节  偏移量：0x{fOff:X}",
                                    HighlightColor = color,
                                    IsSequenceStart = (i == 0),
                                    SequenceIndex = _currentAction,
                                    DirectionIndex = _currentDir,
                                    FrameIndex = i,
                                    PreviewImage = preview
                                });
                            }
                        }
                    }
                }
            }
            catch { /* 尽力而为 – 部分区域也没问题 */ }

            return regions;
        }

        // ──────────────────────────────────────────────────────────────────────
        //  为 MUL 建立区域
        //  MUL 格式：调色板 (512 B) + 帧计数 (2 B) + 帧
        // ──────────────────────────────────────────────────────────────────────

        private List<HexRegion> BuildMulRegions(byte[] data, Ultima.AnimIdx edit)
        {
            var regions = new List<HexRegion>();
            if (data == null || data.Length < 514) return regions;

            System.Drawing.Color[] palette =
            {
        System.Drawing.Color.FromArgb(60, 100, 200, 255),
        System.Drawing.Color.FromArgb(60, 255, 140,  0),
        System.Drawing.Color.FromArgb(60,   0, 200, 80),
        System.Drawing.Color.FromArgb(60, 200,  50, 50),
        System.Drawing.Color.FromArgb(60, 180,  60, 220),
        System.Drawing.Color.FromArgb(60,  60, 200, 200),
        System.Drawing.Color.FromArgb(60, 220, 220,  50),
    };

            // 调色板：256 × 2 字节 = 512 字节
            regions.Add(new HexRegion
            {
                Offset = 0,
                Length = 512,
                Label = "调色板 (256 × uint16)",
                Tooltip = "16bpp ARGB1555 颜色调色板",
                HighlightColor = System.Drawing.Color.FromArgb(80, 255, 215, 0),
                IsSequenceStart = true,
                SequenceIndex = 0,
                DirectionIndex = _currentDir
            });

            // 帧计数：偏移量 512 处的 2 字节
            if (data.Length >= 514)
            {
                ushort frameCount = (ushort)(data[512] | (data[513] << 8));
                regions.Add(new HexRegion
                {
                    Offset = 512,
                    Length = 2,
                    Label = $"FrameCount = {frameCount}",
                    Tooltip = "此方向上的帧数",
                    HighlightColor = System.Drawing.Color.FromArgb(80, 200, 100, 255)
                });

                // 帧查找表：frameCount × 4 字节（每帧偏移量）
                long lookupLen = frameCount * 4L;
                if (514 + lookupLen <= data.Length)
                {
                    regions.Add(new HexRegion
                    {
                        Offset = 514,
                        Length = lookupLen,
                        Label = $"帧查找表 ({frameCount} × 4 B)",
                        Tooltip = "各帧的字节偏移量",
                        HighlightColor = System.Drawing.Color.FromArgb(50, 100, 255, 100)
                    });

                    // 各个帧数据区域
                    if (edit?.Frames != null)
                    {
                        for (int i = 0; i < edit.Frames.Count && i < 28; i++)
                        {
                            //从查找表读取帧偏移量
                            int lookupPos = 514 + i * 4;
                            if (lookupPos + 4 > data.Length) break;

                            int fOff = System.BitConverter.ToInt32(data, lookupPos);
                            if (fOff <= 0 || fOff >= data.Length) continue;

                            // 帧长度 = 下一偏移量 - 当前偏移量（或文件结尾）
                            int nextOff = data.Length;
                            if (i + 1 < edit.Frames.Count)
                            {
                                int nextLookup = 514 + (i + 1) * 4;
                                if (nextLookup + 4 <= data.Length)
                                {
                                    int n = System.BitConverter.ToInt32(data, nextLookup);
                                    if (n > fOff && n <= data.Length) nextOff = n;
                                }
                            }

                            long fLen = nextOff - fOff;
                            if (fLen <= 0) continue;

                            var color = palette[i % palette.Length];
                            var preview = GetFramePreviewBitmap(i);

                            regions.Add(new HexRegion
                            {
                                Offset = fOff,
                                Length = fLen,
                                Label = $"帧 {i}  方向 {_currentDir}",
                                Tooltip = $"CenterX:{edit.Frames[i].Center.X}  CenterY:{edit.Frames[i].Center.Y}  长度:{fLen} B",
                                HighlightColor = color,
                                IsSequenceStart = (i == 0),
                                SequenceIndex = _currentAction,
                                DirectionIndex = _currentDir,
                                FrameIndex = i,
                                PreviewImage = preview
                            });
                        }
                    }
                }
            }

            return regions;
        }

        // ──────────────────────────────────────────────────────────────────────
        //  辅助函数：获取当前帧预览图像
        // ──────────────────────────────────────────────────────────────────────

        private System.Drawing.Bitmap GetCurrentFrameBitmap()
        {
            return GetFramePreviewBitmap(FramesTrackBar.Value);
        }

        private System.Drawing.Bitmap GetFramePreviewBitmap(int frameIndex)
        {
            try
            {
                if (_fileType == 6 && _uopManager != null)
                {
                    var uopAnim = _uopManager.GetUopAnimation(_currentBody, _currentAction, _currentDir);
                    if (uopAnim != null && frameIndex >= 0 && frameIndex < uopAnim.Frames.Count)
                        return new System.Drawing.Bitmap(uopAnim.Frames[frameIndex].Image);
                }
                else if (_fileType != 0)
                {
                    var edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
                    var bits = edit?.GetFrames();
                    if (bits != null && frameIndex >= 0 && frameIndex < bits.Length && bits[frameIndex] != null)
                        return new System.Drawing.Bitmap(bits[frameIndex]);
                }
            }
            catch { /* 预览可选 */ }
            return null;
        }

        // ──────────────────────────────────────────────────────────────────────
        //  当选择更改时调用（方向 / 帧 / 动作）
        //  在 OnDirectionChanged、AfterSelectTreeView 和
        //  OnFrameCountBarChanged 末尾添加此行：
        //
        //  NotifyHexEditor();
        // ──────────────────────────────────────────────────────────────────────

        private void NotifyHexEditor()
        {
            if (_hexEditor == null || _hexEditor.IsDisposed || !_hexEditor.Visible)
                return;

            // 总是完全重新加载，使 _data、_bodyId 和 _dataOffset 与当前选中的身体匹配。
            // UpdateSelection() 永远不会交换 _data → 导致 RLE 解码器使用错误的身体。
            if (_fileType == 6)
                OpenHexEditorUop();
            else
                OpenHexEditorMul();
        }

        private byte[] GetCurrentUopRawData()
        {
            if (_uopManager == null) return null;
            var fi = _uopManager.GetAnimationData(_currentBody, _currentAction, _currentDir);
            return fi?.GetData();
        }

        private List<HexRegion> BuildMulRegionsForCurrentSelection()
        {
            var edit = AnimationEdit.GetAnimation(_fileType, _currentBody, _currentAction, _currentDir);
            if (edit == null) return new List<HexRegion>();

            // 重新读取原始数据（短暂，因为文件被缓存）
            string mulFileName = _fileType == 1 ? "anim.mul" : $"anim{_fileType}.mul";
            string idxFileName = _fileType == 1 ? "anim.idx" : $"anim{_fileType}.idx";
            string mulPath = Ultima.Files.GetFilePath(mulFileName);
            string idxPath = Ultima.Files.GetFilePath(idxFileName);

            if (string.IsNullOrEmpty(mulPath)) return new List<HexRegion>();

            try
            {
                int entryIndex = _currentBody * 110 * 5 + _currentAction * 5 + _currentDir;
                using var idxFs = new System.IO.FileStream(idxPath,
                    System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                using var idxBr = new System.IO.BinaryReader(idxFs);
                idxFs.Seek((long)entryIndex * 12, System.IO.SeekOrigin.Begin);
                int offset = idxBr.ReadInt32();
                int length = idxBr.ReadInt32();
                if (offset < 0 || length <= 0) return new List<HexRegion>();

                byte[] raw = new byte[length];
                using var mulFs = new System.IO.FileStream(mulPath,
                    System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                mulFs.Seek(offset, System.IO.SeekOrigin.Begin);
                mulFs.Read(raw, 0, raw.Length);

                return BuildMulRegions(raw, edit);


            }
            catch { return new List<HexRegion>(); }
        }
        #endregion

        #region [ 诊断 ] - 调用诊断对话框的按钮处理程序
        /// <summary>
        /// 显示当前选中身体的完整 IDX 诊断。
        /// 有助于了解为什么某些动作显示为红色。
        /// </summary>
        private void btnDiagSingle_Click(object sender, EventArgs e)
        {
            if (_fileType == 0 || _fileType == 6) return;
            MulAnimDiagnostics.ShowSingleDiagDialog(_currentBody, _fileType, this);
        }

        /// <summary>
        /// 将功能正常的身体与有问题的身体进行比较。
        /// 准确显示差异所在（BodyConverter、IDX 等）。
        /// </summary>
        private void btnDiagCompare_Click(object sender, EventArgs e)
        {
            if (_fileType == 0 || _fileType == 6) return;

            // 将当前身体视为“有问题的”
            // 询问功能正常的身体
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"比较：身体 {_currentBody}（当前）与哪个功能正常的身体？\n\n" +
                "输入功能正常身体的身体 ID：",
                "比较诊断", "0");

            if (string.IsNullOrEmpty(input)) return;
            if (!int.TryParse(input, out int workingBody)) return;

            MulAnimDiagnostics.ShowComparisonDialog(workingBody, _currentBody, _fileType, this);
        }

        /// <summary>
        /// 扫描加载文件中的所有身体，并列出所有有问题的身体。
        /// 请注意：对于大文件，这可能需要一些时间（约 1-2 秒）。
        /// </summary>
        private void btnDiagMassScan_Click(object sender, EventArgs e)
        {
            if (_fileType == 0 || _fileType == 6) return;

            var result = MessageBox.Show(
                $"扫描 anim{_fileType}.mul 中的所有身体？\n" +
                "仅输出有问题的身体（部分/损坏）。",
                "批量扫描", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            Cursor = Cursors.WaitCursor;
            try { MulAnimDiagnostics.ShowMassScanDialog(_fileType, this); }
            finally { Cursor = Cursors.Default; }
        }
        #endregion

        #region [ 动作映射诊断 Mul ]

        #region [ 诊断 Mul ]

        private void BtnAnimMap_Click(object sender, EventArgs e)
        {
            if (_fileType == 0 || _fileType == 6) return;
            ShowBodyAnimationMap(_currentBody, _fileType);
        }

        /// <summary>
        /// 身体的完整动画映射：
        /// 身体 → 动作 → 方向 → 块信息 → 帧数 → 帧尺寸
        /// 显示哪些有效 / 空 / 损坏以及属于哪个动作名称。
        /// 对于 FileType 2-5，通过 bodyconv.def 解析物理身体索引。
        /// 当检测到 OOB 时，包含完整的诊断信息。
        /// </summary>

        #region [ ShowBodyAnimationMap ]

        /// <summary>
        /// 身体的完整动画映射：
        /// 身体 → 动作 → 方向 → 块信息 → 帧数 → 帧尺寸
        /// 显示哪些有效 / 空 / 损坏以及属于哪个动作名称。
        /// 对于 FileType 2-5，通过 bodyconv.def 解析物理身体索引。
        /// 当 OOB 时，回退到 Translate()、body.def 和完整的 anim*.idx 扫描。
        /// 当 checkBoxDiagInfo 被选中时，包含每个条目的扩展调试输出。
        /// </summary>
        private void ShowBodyAnimationMap(int body, int fileType)
        {
            string mulName = fileType == 1 ? "anim.mul" : $"anim{fileType}.mul";
            string idxName = fileType == 1 ? "anim.idx" : $"anim{fileType}.idx";
            string mulPath = Ultima.Files.GetFilePath(mulName);
            string idxPath = Ultima.Files.GetFilePath(idxName);
            if (string.IsNullOrEmpty(idxPath) || !System.IO.File.Exists(idxPath))
            {
                MessageBox.Show($"{idxName} 未找到。", "动作映射");
                return;
            }

            // ── Diag-Info 标志 ───────────────────────────────────────────────────
            bool diagInfoEnabled = checkBoxDiagInfo != null && checkBoxDiagInfo.Checked;

            // ── 按类型的动作名称 ─────────────────────────────────────────────
            int animLength = Animations.GetAnimLength(body, fileType);
            string typLabel;
            string[] actionNames;

            if (animLength == 22)
            {
                typLabel = "H (怪物)";
                actionNames = new[] {
                    "行走","站立","死亡1","死亡2","攻击1","攻击2","攻击3",
                    "弓攻击","弩攻击","投掷攻击","受击","掠夺",
                    "踩踏","施法2","施法3","右格挡","左格挡","空闲",
                    "烦躁","飞行","起飞","空中受击"
                };
            }
            else if (animLength == 13)
            {
                typLabel = "L (动物)";
                actionNames = new[] {
                    "行走","奔跑","空闲","进食","警觉","攻击1","攻击2",
                    "受击","死亡1","烦躁1","烦躁2","躺下","死亡2"
                };
            }
            else
            {
                typLabel = "P (人类/装备)";
                actionNames = new[] {
                    "行走_01","持杖行走_01","奔跑_01","持杖奔跑_01","空闲_01",
                    "空闲_01b","烦躁_打哈欠","单手战斗空闲","单手战斗空闲b",
                    "单手挥砍攻击","单手穿刺攻击","单手钝击攻击","双手钝击攻击",
                    "双手挥砍攻击","双手穿刺攻击","单手战斗前进","法术1",
                    "法术2","弓攻击","弩攻击","受击_前/高",
                    "死亡_硬直前倒","死亡_硬直后倒","骑马行走","骑马奔跑",
                    "骑马空闲","骑马单手攻击","骑马弓攻击",
                    "骑马弩攻击","骑马双手攻击","盾牌格挡",
                    "拳击_刺拳","鞠躬_小","武装敬礼","进食"
                };
            }

            // ── 通过 bodyconv.def 解析物理身体索引 ─────────────────────
            int physicalBody = body;
            int physicalFileType = fileType;
            string bodyconvPath = Ultima.Files.GetFilePath("bodyconv.def");
            if (!string.IsNullOrEmpty(bodyconvPath) && File.Exists(bodyconvPath))
            {
                try
                {
                    foreach (string rawLine in File.ReadLines(bodyconvPath))
                    {
                        int ci = rawLine.IndexOf('#');
                        string clean = (ci >= 0 ? rawLine.Substring(0, ci) : rawLine).Trim();
                        if (clean.Length == 0) continue;
                        string[] parts = clean.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length < 2) continue;
                        if (!int.TryParse(parts[0], out int lineBody)) continue;
                        if (lineBody != body) continue;

                        for (int targetCol = 1; targetCol <= 4; targetCol++)
                        {
                            if (parts.Length <= targetCol) continue;
                            if (int.TryParse(parts[targetCol], out int mapped) && mapped >= 0)
                            {
                                physicalBody = mapped;
                                physicalFileType = targetCol + 1;
                                break;
                            }
                        }
                        break;
                    }
                }
                catch { }
            }

            // ── IDX 文件大小检查 ──────────────────────────────────────────────
            long idxFileLen = 0;
            try { idxFileLen = new System.IO.FileInfo(idxPath).Length; } catch { }
            (long idxRangeStart, long idxRangeEnd) = GetIdxBodyRange(physicalBody, animLength, physicalFileType);
            bool willBeOob = idxRangeEnd > idxFileLen;

            // ── 对于 OOB 情况，尝试 Animations.Translate ───────────────────────────
            int translatedBody = body;
            int translatedFileType = fileType;
            bool translateAvailable = false;
            bool translateHelps = false;
            try
            {
                Ultima.Animations.Translate(ref translatedBody, ref translatedFileType);
                translateAvailable = true;
                if (translatedBody != body || translatedFileType != fileType)
                {
                    long idxStartTr = (long)(translatedBody * 110 * 5) * 12;
                    long idxEndTr = idxStartTr + (long)(animLength * 5 * 12);
                    string trIdxPath = translatedFileType == 1
                        ? Ultima.Files.GetFilePath("anim.idx")
                        : Ultima.Files.GetFilePath($"anim{translatedFileType}.idx");
                    long trIdxLen = 0;
                    if (!string.IsNullOrEmpty(trIdxPath) && System.IO.File.Exists(trIdxPath))
                        try { trIdxLen = new System.IO.FileInfo(trIdxPath).Length; } catch { }
                    else
                        trIdxLen = idxFileLen;
                    translateHelps = idxEndTr <= trIdxLen;
                }
            }
            catch { }

            // ── 对于 OOB 情况，尝试 body.def (BodyTable) ───────────────────────────
            int bodyDefOldId = -1;
            bool bodyDefHelps = false;
            Ultima.BodyTableEntry btEntry = null;
            if (willBeOob && Ultima.BodyTable.Entries != null &&
                Ultima.BodyTable.Entries.TryGetValue(body, out btEntry))
            {
                bodyDefOldId = btEntry.OldId;
                long idxStartBd = (long)(bodyDefOldId * 110 * 5) * 12;
                long idxEndBd = idxStartBd + (long)(animLength * 5 * 12);
                bodyDefHelps = idxEndBd <= idxFileLen;
                if (bodyDefHelps) physicalBody = bodyDefOldId;
            }

            // ── 如果 Translate 给出不同的文件，则切换到该文件 ──────────────
            if (willBeOob && translateAvailable && translateHelps
                && translatedFileType != fileType)
            {
                physicalBody = translatedBody;
                physicalFileType = translatedFileType;
                mulName = physicalFileType == 1 ? "anim.mul" : $"anim{physicalFileType}.mul";
                idxName = physicalFileType == 1 ? "anim.idx" : $"anim{physicalFileType}.idx";
                mulPath = Ultima.Files.GetFilePath(mulName);
                idxPath = Ultima.Files.GetFilePath(idxName);
                idxFileLen = 0;
                try { idxFileLen = new System.IO.FileInfo(idxPath).Length; } catch { }
                idxRangeEnd = (long)(physicalBody * 110 * 5) * 12 + (long)(animLength * 5 * 12);
                willBeOob = idxRangeEnd > idxFileLen;
            }
            else if (willBeOob && translateAvailable && translateHelps
                     && translatedFileType == fileType)
            {
                physicalBody = translatedBody;
                idxRangeEnd = (long)(physicalBody * 110 * 5) * 12 + (long)(animLength * 5 * 12);
                willBeOob = idxRangeEnd > idxFileLen;
            }

            // ── 后备：扫描所有 anim*.idx 文件 + 通过帧大小 + 可见大小验证 MUL ─────
            // 像 290 或 631 这样的身体存在于不同的 anim 文件中，但
            // bodyconv.def / body.def 中没有条目，并且 Translate() 原样返回。

            string animScanNote = null;
            bool needsRedirect = willBeOob;

            if (!needsRedirect)
            {
                try
                {
                    using var testFs = new System.IO.FileStream(idxPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                    using var testBr = new System.IO.BinaryReader(testFs);
                    long testPos = GetIdxOffset(physicalBody, 0, 0, physicalFileType);
                    if (testPos + 12 <= testFs.Length)
                    {
                        testFs.Seek(testPos, System.IO.SeekOrigin.Begin);
                        int testOff = testBr.ReadInt32();
                        int testLen = testBr.ReadInt32();
                        if (testOff <= 0 || testOff == unchecked((int)0xFFFFFFFF) || testLen <= 0)
                            needsRedirect = true;
                    }
                }
                catch { }
            }

            if (needsRedirect)
            {
                for (int ft = 1; ft <= 5; ft++)
                {
                    if (ft == physicalFileType) continue;
                    string scanIdxName = ft == 1 ? "anim.idx" : $"anim{ft}.idx";
                    string scanIdxPath = Ultima.Files.GetFilePath(scanIdxName);
                    if (string.IsNullOrEmpty(scanIdxPath) || !System.IO.File.Exists(scanIdxPath)) continue;

                    try
                    {
                        long scanIdxLen = new System.IO.FileInfo(scanIdxPath).Length;
                        long scanRangeStart = GetIdxOffset(body, 0, 0, ft);
                        long scanRangeEnd = scanRangeStart + (long)(animLength * 5 * 12);
                        if (scanRangeEnd > scanIdxLen) continue;

                        using var scanFs = new System.IO.FileStream(scanIdxPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                        using var scanBr = new System.IO.BinaryReader(scanFs);
                        scanFs.Seek(scanRangeStart, System.IO.SeekOrigin.Begin);
                        int chkOff = scanBr.ReadInt32();
                        int chkLen = scanBr.ReadInt32();
                        if (chkOff <= 0 || chkOff == unchecked((int)0xFFFFFFFF) || chkLen <= 0) continue;

                        // === MUL 验证 + 可见大小 ===
                        string scanMulName = ft == 1 ? "anim.mul" : $"anim{ft}.mul";
                        string scanMulPath = Ultima.Files.GetFilePath(scanMulName);
                        bool verified = false;
                        int verifiedFc = 0;
                        int headerW = 0, headerH = 0;
                        int visibleW = 0, visibleH = 0;
                        uint miniHash = 0;

                        if (!string.IsNullOrEmpty(scanMulPath) && System.IO.File.Exists(scanMulPath))
                        {
                            using var mulVerify = new System.IO.FileStream(scanMulPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                            if (chkOff + 518 <= mulVerify.Length)
                            {
                                mulVerify.Seek(chkOff + 512, System.IO.SeekOrigin.Begin);
                                int fc = mulVerify.ReadByte() | (mulVerify.ReadByte() << 8);
                                if (fc >= 1 && fc <= 256)
                                {
                                    mulVerify.Seek(chkOff + 514, System.IO.SeekOrigin.Begin);
                                    mulVerify.ReadByte(); mulVerify.ReadByte();
                                    int relOff = mulVerify.ReadByte() | (mulVerify.ReadByte() << 8);
                                    int absOff = 512 + relOff;

                                    if (chkOff + absOff + 8 <= mulVerify.Length)
                                    {
                                        mulVerify.Seek(chkOff + absOff, System.IO.SeekOrigin.Begin);
                                        short cx = (short)(mulVerify.ReadByte() | (mulVerify.ReadByte() << 8));
                                        short cy = (short)(mulVerify.ReadByte() | (mulVerify.ReadByte() << 8));
                                        int w = mulVerify.ReadByte() | (mulVerify.ReadByte() << 8);
                                        int h = mulVerify.ReadByte() | (mulVerify.ReadByte() << 8);

                                        headerW = w;
                                        headerH = h;

                                        // 近似可见大小（移除填充 – 完美匹配）
                                        visibleW = Math.Max(1, w - 4);
                                        visibleH = Math.Max(1, h - 10);

                                        // 前 64 字节的迷你哈希
                                        if (chkOff + absOff + 64 <= mulVerify.Length)
                                        {
                                            mulVerify.Seek(chkOff + absOff, System.IO.SeekOrigin.Begin);
                                            byte[] buf = new byte[64];
                                            mulVerify.Read(buf, 0, 64);
                                            for (int i = 0; i < 64; i++)
                                                miniHash = (miniHash * 31) + buf[i];
                                        }

                                        verified = true;
                                        verifiedFc = fc;
                                    }
                                }
                            }
                        }

                        if (!verified) continue;

                        physicalBody = body;
                        physicalFileType = ft;
                        mulName = scanMulName;
                        idxName = scanIdxName;
                        mulPath = scanMulPath;
                        idxPath = scanIdxPath;
                        idxFileLen = scanIdxLen;
                        idxRangeEnd = scanRangeEnd;
                        willBeOob = false;

                        animScanNote = $"anim-scan：身体 {body} 在 {idxName} (FT{ft}) 中找到 " +
                                       $"→ 已验证 fc={verifiedFc} ({headerW}×{headerH} 可见 ~{visibleW}×{visibleH}) " +
                                       $"哈希=0x{miniHash:X8} 位于 {mulName}";
                        break;
                    }
                    catch { }
                }

                if (willBeOob)
                    animScanNote = $"anim-scan：未为身体 {body} 找到有效文件";
            }

            // ── 写入诊断文件 ───────────────────────────────────────────
            string diagPath = null;
            if (diagInfoEnabled)
            {
                var diagSb = new System.Text.StringBuilder();
                diagSb.AppendLine($"=== 诊断：身体 {body} 文件类型 {fileType} ===");
                diagSb.AppendLine();
                diagSb.AppendLine($"IDX 文件：{idxName}");
                diagSb.AppendLine($"IDX 文件大小：{idxFileLen} 字节 ({idxFileLen / 12} 条目)");
                diagSb.AppendLine($"物理身体：{physicalBody}");
                diagSb.AppendLine($"IDX 范围所需：[{idxRangeStart}..{idxRangeEnd}] " +
                                  $"({idxRangeEnd - idxRangeStart} 字节)");
                diagSb.AppendLine($"IDX 范围适合：{(!willBeOob ? "是 ✓" : "否 — 越界 ✗")}");
                if (animScanNote != null)
                    diagSb.AppendLine($"Anim-scan 结果：{animScanNote}");
                diagSb.AppendLine();
                diagSb.AppendLine($"身体 {body} 的 bodyconv.def 查找：");
                string bcDiagPath = Ultima.Files.GetFilePath("bodyconv.def");
                if (!string.IsNullOrEmpty(bcDiagPath) && System.IO.File.Exists(bcDiagPath))
                {
                    bool found = false;
                    try
                    {
                        foreach (string rawLine in System.IO.File.ReadLines(bcDiagPath))
                        {
                            int ci = rawLine.IndexOf('#');
                            string clean = (ci >= 0 ? rawLine.Substring(0, ci) : rawLine).Trim();
                            if (clean.Length == 0) continue;
                            string[] parts = clean.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length < 2) continue;
                            if (!int.TryParse(parts[0], out int lb) || lb != body) continue;
                            diagSb.AppendLine($" 行：{string.Join(" ", parts)}");
                            diagSb.AppendLine($" 列：[0]=身体 [1]=anim2 [2]=anim3 [3]=anim4 [4]=anim5");
                            found = true;
                            break;
                        }
                    }
                    catch { }
                    if (!found) diagSb.AppendLine(" 在 bodyconv.def 中未找到");
                }
                else diagSb.AppendLine(" bodyconv.def 未找到");
                diagSb.AppendLine();
                diagSb.AppendLine($"身体 {body} 的 body.def (BodyTable) 查找：");
                if (bodyDefOldId >= 0 && btEntry != null)
                    diagSb.AppendLine($" 找到：{body} → OldId={bodyDefOldId} 适合={bodyDefHelps}");
                else
                    diagSb.AppendLine(" 在 body.def 中未找到");
                diagSb.AppendLine();
                diagSb.AppendLine($"Animations.Translate({body}, {fileType})：");
                if (translateAvailable)
                    diagSb.AppendLine($" 结果：身体={translatedBody} 文件类型={translatedFileType}" +
                                      $" 有帮助={translateHelps}");
                else
                    diagSb.AppendLine(" 不可用");

                diagPath = System.IO.Path.Combine(
                    System.IO.Path.GetTempPath(),
                    $"ActionMap_Diag_Body{body}_FT{fileType}.txt");
                System.IO.File.WriteAllText(diagPath, diagSb.ToString());
            }

            // ── 构建报告 ─────────────────────────────────────────────────────
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"=== 动作映射：身体 {body} [{typLabel}] 文件类型 {fileType} ===");
            sb.AppendLine($"文件：{idxName} / {mulName}");
            sb.AppendLine($"IDX 中的物理身体索引：{physicalBody}" +
                          (physicalBody != body
                              ? $" (从 {body} 通过 " +
                                (bodyDefHelps ? "body.def" : "bodyconv.def") + " 映射)"
                              : ""));
            if (physicalFileType != fileType)
                sb.AppendLine($"重定向到文件类型 {physicalFileType} ({idxName} / {mulName})");
            if (animScanNote != null)
                sb.AppendLine($"ℹ {animScanNote}");
            sb.AppendLine($"动画长度：{animLength} → {animLength * 5} 个 IDX 条目" +
                          $" ({animLength * 5 * 12} 字节在 IDX 中)");
            if (diagInfoEnabled)
                sb.AppendLine("[DiagInfo：已启用 — 每个条目的扩展调试输出处于活动状态]");
            if (willBeOob)
            {
                sb.AppendLine();
                sb.AppendLine($"⚠ 警告：IDX 范围 [{idxRangeStart}..{idxRangeEnd}] 超出" +
                              $" IDX 文件大小 {idxFileLen} — 所有条目都将越界");
                sb.AppendLine($" bodyconv.def：{(physicalBody != body ? $"{body}→{physicalBody}" : "无映射")}");
                sb.AppendLine($" body.def：{(bodyDefOldId >= 0 ? $"{body}→{bodyDefOldId} (适合={bodyDefHelps})" : "无条目")}");
                sb.AppendLine($" Translate()：{(translateAvailable ? $"{body}→{translatedBody} ft={translatedFileType} (有帮助={translateHelps})" : "不可用")}");
                sb.AppendLine($" anim-scan：{animScanNote ?? "未运行"}");
                sb.AppendLine(diagInfoEnabled && diagPath != null
                    ? $" 诊断：{diagPath}"
                    : $" 诊断：（启用 checkBoxDiagInfo 以保存诊断文件）");
            }

            sb.AppendLine();
            sb.AppendLine($"{"动作",-4} {"名称",-24} {"方向0",10} {"方向1",10} {"方向2",10}" +
                          $" {"方向3",10} {"方向4",10} {"状态",-8}");
            sb.AppendLine(new string('─', 90));
            int totalOk = 0, totalEmpty = 0, totalCorrupt = 0;

            // ── 扩展调试 StringBuilder（仅在 diagInfoEnabled 时） ──────────────
            var extDbgSb = diagInfoEnabled ? new System.Text.StringBuilder() : null;
            if (diagInfoEnabled)
            {
                extDbgSb.AppendLine($"=== 扩展调试：身体 {body} 文件类型 {fileType} ===");
                extDbgSb.AppendLine($"物理身体：{physicalBody} | 文件：{idxName}");
                if (animScanNote != null)
                    extDbgSb.AppendLine($"Anim-scan：{animScanNote}");
                extDbgSb.AppendLine();
            }

            try
            {
                using var idxFs = new System.IO.FileStream(idxPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                using var mulFs = (!string.IsNullOrEmpty(mulPath) && System.IO.File.Exists(mulPath))
                    ? new System.IO.FileStream(mulPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)
                    : null;
                using var br = new System.IO.BinaryReader(idxFs);

                for (int action = 0; action < animLength; action++)
                {
                    string actName = action < actionNames.Length ? actionNames[action] : $"动作{action:D2}";
                    var dirInfos = new string[5];
                    var dirDetails = new System.Text.StringBuilder();
                    bool anyValid = false, anyCorrupt = false, anyEmpty = false;

                    for (int dir = 0; dir < 5; dir++)
                    {
                        //long idxPos = (long)(physicalBody * 110 * 5 + action * 5 + dir) * 12;
                        long idxPos = GetIdxOffset(physicalBody, action, dir, physicalFileType);

                        if (idxPos + 12 > idxFs.Length)
                        {
                            dirInfos[dir] = "越界";
                            anyCorrupt = true;
                            if (diagInfoEnabled)
                            {
                                extDbgSb.AppendLine($"--- 动作={action:D2} ({actName}) 方向={dir} ---");
                                extDbgSb.AppendLine($" IDX 位置：字节 {idxPos} → 越界（文件大小={idxFs.Length}）");
                                extDbgSb.AppendLine();
                            }
                            continue;
                        }

                        idxFs.Seek(idxPos, System.IO.SeekOrigin.Begin);
                        int rawOff = br.ReadInt32();
                        int rawLen = br.ReadInt32();
                        int rawExt = br.ReadInt32();

                        // ── 扩展调试 ────────────────────────────────────────
                        if (diagInfoEnabled)
                        {
                            // ── 扩展调试（您完整的原始块） ─────
                            extDbgSb.AppendLine($"--- 动作={action:D2} ({actName}) 方向={dir} ---");
                            extDbgSb.AppendLine($" IDX 位置：字节 {idxPos} (= {physicalBody}*110*5*12 + {action}*5*12 + {dir}*12)");
                            extDbgSb.AppendLine($" IDX 原始：off=0x{rawOff:X8} ({rawOff}) len=0x{rawLen:X8} ({rawLen}) ext=0x{rawExt:X8}");
                            extDbgSb.AppendLine($" off==-1：{rawOff == -1} off==0xFFFFFFFF：{rawOff == unchecked((int)0xFFFFFFFF)} len<=0：{rawLen <= 0}");

                            // 周围的身体 — 每动作一次，your=0
                            if (dir == 0)
                            {
                                extDbgSb.AppendLine($" 周围 IDX 条目（physBody±2，动作={action} 方向=0）：");
                                for (int scanBody = Math.Max(0, physicalBody - 2); scanBody <= physicalBody + 2; scanBody++)
                                {
                                    long scanPos = GetIdxOffset(scanBody, action, 0, physicalFileType);
                                    if (scanPos + 12 > idxFs.Length)
                                    {
                                        extDbgSb.AppendLine($" 身体 {scanBody,4}：越界");
                                        continue;
                                    }
                                    idxFs.Seek(scanPos, System.IO.SeekOrigin.Begin);
                                    int sOff = br.ReadInt32();
                                    int sLen = br.ReadInt32();
                                    br.ReadInt32();
                                    string valid = (sOff > 0 && sOff != unchecked((int)0xFFFFFFFF) && sLen > 0) ? "有效" : "空/无效";
                                    extDbgSb.AppendLine($" 身体 {scanBody,4}：off=0x{sOff:X8} len={sLen,8} [{valid}]");
                                }
                            }

                            // 所有 anim*.idx 交叉扫描 — 在动作=0 方向=0 时执行一次
                            if (action == 0 && dir == 0)
                            {
                                extDbgSb.AppendLine($" 扫描身体 {body} 的所有 anim*.idx 动作=0 方向=0：");
                                for (int ft = 1; ft <= 5; ft++)
                                {
                                    string scanIdxName2 = ft == 1 ? "anim.idx" : $"anim{ft}.idx";
                                    string scanIdxPath2 = Ultima.Files.GetFilePath(scanIdxName2);
                                    if (string.IsNullOrEmpty(scanIdxPath2) || !System.IO.File.Exists(scanIdxPath2))
                                    {
                                        extDbgSb.AppendLine($" FT{ft}：{scanIdxName2} 未找到");
                                        continue;
                                    }
                                    try
                                    {
                                        using var scanFs2 = new System.IO.FileStream(scanIdxPath2, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                                        using var scanBr2 = new System.IO.BinaryReader(scanFs2);
                                        long scanFileLen2 = scanFs2.Length;
                                        long scanPos2 = GetIdxOffset(body, 0, 0, ft);
                                        if (scanPos2 + 12 <= scanFileLen2)
                                        {
                                            scanFs2.Seek(scanPos2, System.IO.SeekOrigin.Begin);
                                            int sOff = scanBr2.ReadInt32();
                                            int sLen = scanBr2.ReadInt32();
                                            string valid = (sOff > 0 && sOff != unchecked((int)0xFFFFFFFF) && sLen > 0) ? "有效 ✓" : "空";
                                            extDbgSb.AppendLine($" FT{ft}：{scanIdxName2,-16} 身体={body} off=0x{sOff:X8} len={sLen,8} [{valid}]");
                                        }
                                        else
                                        {
                                            extDbgSb.AppendLine($" FT{ft}：{scanIdxName2,-16} 身体={body} → 越界（文件={scanFileLen2}）");
                                        }
                                    }
                                    catch (Exception ex2)
                                    {
                                        extDbgSb.AppendLine($" FT{ft}：错误 — {ex2.Message}");
                                    }
                                }

                                extDbgSb.AppendLine($" 身体 {body} 的 UOP 检查：");
                                if (_uopManager != null)
                                {
                                    int uopFoundAct = -1, uopFoundDir = -1;
                                    for (int a = 0; a < animLength && uopFoundAct < 0; a++)
                                        for (int d = 0; d < 5 && uopFoundAct < 0; d++)
                                            if (_uopManager.GetAnimationData(body, a, d) != null)
                                            { uopFoundAct = a; uopFoundDir = d; }
                                    extDbgSb.AppendLine(uopFoundAct >= 0 ? $" 在 UOP 中找到，位于 动作={uopFoundAct} 方向={uopFoundDir} ✓" : " 在 UOP 中未找到");
                                }
                                else
                                {
                                    extDbgSb.AppendLine(" UOP 管理器不可用");
                                }
                            }
                            extDbgSb.AppendLine();
                        }
                        // ── 扩展调试结束 ────────────────────────────────────

                        if (rawOff < 0 || rawOff == unchecked((int)0xFFFFFFFF) || rawLen <= 0)
                        {
                            dirInfos[dir] = "空";
                            anyEmpty = true;
                            continue;
                        }

                        if (mulFs == null || rawOff + 514 > mulFs.Length)
                        {
                            dirInfos[dir] = $"@{rawOff:X}";
                            anyValid = true;
                            continue;
                        }

                        mulFs.Seek(rawOff + 512, System.IO.SeekOrigin.Begin);
                        int fc = mulFs.ReadByte() | (mulFs.ReadByte() << 8);
                        if (fc == 0 || fc > 256)
                        {
                            dirInfos[dir] = $"!fc={fc}";
                            anyCorrupt = true;
                            continue;
                        }

                        if (rawOff + 518 <= mulFs.Length)
                        {
                            mulFs.Seek(rawOff + 514, System.IO.SeekOrigin.Begin);
                            mulFs.ReadByte(); mulFs.ReadByte();
                            int relOff = mulFs.ReadByte() | (mulFs.ReadByte() << 8);
                            int absOff = 512 + relOff;
                            if (rawOff + absOff + 8 <= mulFs.Length)
                            {
                                mulFs.Seek(rawOff + absOff, System.IO.SeekOrigin.Begin);
                                short cx = (short)(mulFs.ReadByte() | (mulFs.ReadByte() << 8));
                                short cy = (short)(mulFs.ReadByte() | (mulFs.ReadByte() << 8));
                                int w = mulFs.ReadByte() | (mulFs.ReadByte() << 8);
                                int h = mulFs.ReadByte() | (mulFs.ReadByte() << 8);
                                dirInfos[dir] = $"fc={fc}";
                                dirDetails.AppendLine($" 方向{dir}：偏移量=0x{rawOff:X8} 长度={rawLen,7} 帧数={fc,3} F0：{w,4}×{h,-4} CX={cx,4} CY={cy,4}");
                                anyValid = true;
                                continue;
                            }
                        }

                        dirInfos[dir] = $"fc={fc}";
                        anyValid = true;
                    }

                    if (anyValid) totalOk++;
                    if (anyEmpty && !anyValid) totalEmpty++;
                    if (anyCorrupt) totalCorrupt++;

                    string status = anyCorrupt ? "损坏" : !anyValid ? "缺失" : anyEmpty ? "部分" : "正常";
                    sb.Append($"{action:D2} {actName,-24}");
                    foreach (var d in dirInfos) sb.Append($" {d,10}");
                    sb.AppendLine($" {status,-8}");
                    if (dirDetails.Length > 0) sb.Append(dirDetails);
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"\n读取文件时出错：{ex.Message}");
            }

            sb.AppendLine();
            sb.AppendLine(new string('─', 90));
            sb.AppendLine($"摘要：正常={totalOk} 部分/缺失={totalEmpty} 损坏={totalCorrupt}");
            string fullText = sb.ToString();

            // ── 保存主报告 ─────────────────────────────────────────────────
            string dumpPath = null;
            if (diagInfoEnabled)
            {
                dumpPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"ActionMap_Body{body}_FT{fileType}.txt");
                System.IO.File.WriteAllText(dumpPath, fullText);
            }

            // ── 保存扩展调试文件（仅在 diagInfoEnabled 时） ─────────────
            string extDbgPath = null;
            if (diagInfoEnabled && extDbgSb != null)
            {
                extDbgPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"ActionMap_ExtDebug_Body{body}_FT{fileType}.txt");
                System.IO.File.WriteAllText(extDbgPath, extDbgSb.ToString());
            }

            // ── 构建对话框 ─────────────────────────────────────────────────────
            var dlg = new Form
            {
                Text = $"动作映射 — 身体 {body} [{typLabel}] 文件类型 {fileType}" + (diagInfoEnabled ? " [DiagInfo 开启]" : ""),
                Size = new System.Drawing.Size(960, 660),
                MinimumSize = new System.Drawing.Size(700, 400),
                StartPosition = FormStartPosition.CenterParent,
                Icon = this.Icon
            };

            var toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 34,
                BackColor = System.Drawing.Color.FromArgb(38, 38, 50)
            };

            var btnCopyText = new Button
            {
                Text = "复制文本",
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(0, 90, 150),
                ForeColor = System.Drawing.Color.White,
                Width = 90,
                Height = 26,
                Left = 6,
                Top = 4,
                Font = new System.Drawing.Font("Segoe UI", 8.5f)
            };
            btnCopyText.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 130, 200);

            var btnCopyScreen = new Button
            {
                Text = "复制截图",
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(60, 60, 80),
                ForeColor = System.Drawing.Color.White,
                Width = 120,
                Height = 26,
                Left = 102,
                Top = 4,
                Font = new System.Drawing.Font("Segoe UI", 8.5f)
            };
            btnCopyScreen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(90, 90, 110);

            var btnDiag = new Button
            {
                Text = "打开诊断",
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(80, 50, 20),
                ForeColor = System.Drawing.Color.White,
                Width = 120,
                Height = 26,
                Left = 228,
                Top = 4,
                Font = new System.Drawing.Font("Segoe UI", 8.5f)
            };
            btnDiag.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(180, 120, 40);
            btnDiag.Click += (s, ev) =>
            {
                try { System.Diagnostics.Process.Start("notepad.exe", diagPath); }
                catch { }
            };

            // ── “打开扩展调试”按钮 — 仅在 diagInfoEnabled 时可见 ────────
            var btnExtDbg = new Button
            {
                Text = "打开扩展调试",
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(30, 70, 30),
                ForeColor = System.Drawing.Color.White,
                Width = 120,
                Height = 26,
                Left = 354,
                Top = 4,
                Font = new System.Drawing.Font("Segoe UI", 8.5f),
                Visible = diagInfoEnabled && extDbgPath != null
            };
            btnExtDbg.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(60, 160, 60);
            btnExtDbg.Click += (s, ev) =>
            {
                if (extDbgPath != null)
                    try { System.Diagnostics.Process.Start("notepad.exe", extDbgPath); }
                    catch { }
            };

            var lblInfo = new Label
            {
                Text = $"身体 {body}  ·  {typLabel}  ·  文件类型 {fileType}" +
                       $"  ·  {animLength} 个动作 × 5 个方向" +
                       (physicalBody != body ? $"  ·  物理={physicalBody}" : "") +
                       (physicalFileType != fileType ? $"  ·  FT{physicalFileType}" : "") +
                       (animScanNote != null && !willBeOob ? $"  ·  扫描→FT{physicalFileType}" : "") +
                       (diagInfoEnabled ? "  ·  DiagInfo 开启" : ""),
                ForeColor = diagInfoEnabled
                    ? System.Drawing.Color.FromArgb(100, 220, 130)
                    : System.Drawing.Color.FromArgb(140, 140, 160),
                Font = new System.Drawing.Font("Segoe UI", 8.5f),
                AutoSize = true,
                Left = diagInfoEnabled ? 480 : 358,
                Top = 9
            };

            toolbar.Controls.Add(btnCopyText);
            toolbar.Controls.Add(btnCopyScreen);
            toolbar.Controls.Add(btnDiag);
            toolbar.Controls.Add(btnExtDbg);
            toolbar.Controls.Add(lblInfo);

            var txt = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                WordWrap = false,
                Font = new System.Drawing.Font("Consolas", 9f),
                BackColor = System.Drawing.Color.FromArgb(28, 28, 32),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 220)
            };

            foreach (var line in fullText.Split('\n'))
            {
                int start = txt.TextLength;
                txt.AppendText(line + "\n");

                System.Drawing.Color col;
                if (line.Contains(" 正常"))
                    col = System.Drawing.Color.FromArgb(100, 220, 130);
                else if (line.Contains("损坏"))
                    col = System.Drawing.Color.FromArgb(255, 100, 100);
                else if (line.Contains("缺失") || line.Contains("部分"))
                    col = System.Drawing.Color.FromArgb(255, 200, 80);
                else if (line.TrimStart().StartsWith("方向"))
                    col = System.Drawing.Color.FromArgb(140, 180, 255);
                else if (line.StartsWith("==="))
                    col = System.Drawing.Color.FromArgb(200, 180, 255);
                else if (line.StartsWith("摘要"))
                    col = System.Drawing.Color.FromArgb(200, 200, 100);
                else if (line.Contains("⚠") || line.Contains("警告"))
                    col = System.Drawing.Color.FromArgb(255, 140, 40);
                else if (line.Contains("ℹ") || line.Contains("anim-scan") || line.Contains("重定向"))
                    col = System.Drawing.Color.FromArgb(100, 210, 255);
                else if (line.Contains("从映射"))
                    col = System.Drawing.Color.FromArgb(255, 180, 80);
                else if (line.Contains("DiagInfo"))
                    col = System.Drawing.Color.FromArgb(100, 220, 130);
                else if (line.TrimStart().StartsWith("身体") ||
                         line.TrimStart().StartsWith("Translate") ||
                         line.TrimStart().StartsWith("诊断"))
                    col = System.Drawing.Color.FromArgb(160, 200, 255);
                else
                    col = System.Drawing.Color.FromArgb(220, 220, 220);

                txt.Select(start, line.Length);
                txt.SelectionColor = col;
            }
            txt.SelectionStart = 0;
            txt.SelectionLength = 0;

            var statusBar = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 22,
                Text = $"  保存：{dumpPath}" +
                       (diagInfoEnabled && extDbgPath != null
                           ? $"  |  扩展调试：{extDbgPath}" : "") +
                       $"  |  正常={totalOk}" +
                       $"  缺失/部分={totalEmpty}  损坏={totalCorrupt}" +
                       (physicalBody != body ? $"  |  映射：{body}→{physicalBody}" : "") +
                       (physicalFileType != fileType ? $"  |  FT{fileType}→FT{physicalFileType}" : "") +
                       (willBeOob ? "  |  ⚠ 越界 — 检查诊断" : ""),
                Font = new System.Drawing.Font("Segoe UI", 8f),
                ForeColor = System.Drawing.Color.FromArgb(140, 140, 160),
                BackColor = System.Drawing.Color.FromArgb(38, 38, 50)
            };

            btnCopyText.Click += (s, ev) =>
            {
                Clipboard.SetText(fullText);
                btnCopyText.Text = "已复制！";
                var t = new System.Windows.Forms.Timer { Interval = 1500 };
                t.Tick += (_, __) => { btnCopyText.Text = "复制文本"; t.Stop(); t.Dispose(); };
                t.Start();
            };

            btnCopyScreen.Click += (s, ev) =>
            {
                var bmp = new System.Drawing.Bitmap(dlg.Width, dlg.Height);
                dlg.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));
                Clipboard.SetImage(bmp);
                btnCopyScreen.Text = "已复制！";
                var t = new System.Windows.Forms.Timer { Interval = 1500 };
                t.Tick += (_, __) => { btnCopyScreen.Text = "复制截图"; t.Stop(); t.Dispose(); };
                t.Start();
            };

            dlg.Controls.Add(txt);
            dlg.Controls.Add(toolbar);
            dlg.Controls.Add(statusBar);
            dlg.ShowDialog(this);
        }

        /// <summary>
        /// 为给定的物理身体 + 动作 + 方向组合返回 anim.idx / anim2.idx 等中的正确字节偏移量。
        /// UO anim.idx 布局（文件类型 1）：
        ///   身体   0–199 → 人类/装备（35 个动作 × 5 个方向）
        ///   身体 200–399 → 动物（13 个动作 × 5 个方向）
        ///   身体 400+    → 怪物（22 个动作 × 5 个方向）
        /// 对于 anim2–anim5，步长始终为 22（仅限怪物文件）。
        /// </summary>
        private static long GetIdxOffset(int body, int action, int dir, int fileType)
        {
            long baseOffset;
            if (fileType == 1)
            {
                if (body < 200)
                    baseOffset = (long)body * 35 * 5 * 12;
                else if (body < 400)
                    baseOffset = (long)(200 * 35 * 5 + (body - 200) * 13 * 5) * 12;
                else
                    baseOffset = (long)(200 * 35 * 5 + 200 * 13 * 5 + (body - 400) * 22 * 5) * 12;
            }
            else // anim2–anim5：仅怪物
            {
                baseOffset = (long)body * 22 * 5 * 12;
            }
            return baseOffset + ((long)action * 5 + dir) * 12;
        }

        /// <summary>
        /// 返回一个身体的所有动作在 anim.idx 中的字节范围 [起始, 结束)。
        /// </summary>
        private static (long start, long end) GetIdxBodyRange(int body, int animLength, int fileType)
        {
            long start = GetIdxOffset(body, 0, 0, fileType);
            long end = start + (long)animLength * 5 * 12;
            return (start, end);
        }
        #endregion
        #endregion



        #endregion

        #region [ 诊断 UOP ]
        private void BtnAnimMapUop_Click(object sender, EventArgs e)
        {
            if (_fileType != 6 || _uopManager == null)
            {
                MessageBox.Show("请先加载 UOP 动画文件。", "动作映射 UOP");
                return;
            }
            ShowBodyAnimationMapUop(_currentBody);
        }

        /// <summary>
        /// UOP 身体的完整动画映射：
        /// 身体 → 动作 → 方向 → UOP 块信息 → 帧数 → 帧尺寸
        /// 显示哪些有效 / 空 / 缺失以及属于哪个动作名称。
        /// </summary>

        private void ShowBodyAnimationMapUop(int body)
        {
            if (_uopManager == null)
            {
                MessageBox.Show("UOP 管理器未初始化。", "动作映射 UOP");
                return;
            }

            // ── 动作名称 ─────────────────────────────────────────────────────────
            int animLength = 26;
            string typLabel = "UOP 生物";
            string[] actionNames = new[] {
        "行走","奔跑","空闲","进食","警觉","攻击1","攻击2","受击",
        "死亡1","空闲2","烦躁","躺下","死亡2","攻击3","弓攻击",
        "弩攻击","投掷攻击","掠夺","踩踏","施法2","施法3",
        "右格挡","左格挡","飞行","起飞","空中受击"
    };

            // ── 检测实际动作计数 ───────────────────────────────────────────
            int detectedLength = 0;
            for (int a = 0; a < 35; a++)
            {
                bool hasAny = false;
                for (int d = 0; d < 5; d++)
                {
                    if (_uopManager.GetAnimationData(body, a, d) != null)
                    { hasAny = true; break; }
                }
                if (hasAny) detectedLength = a + 1;
            }
            if (detectedLength > 0) animLength = detectedLength;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"=== 动作映射 UOP：身体 {body}  [{typLabel}] ===");
            sb.AppendLine($"UOP 管理器：{_uopManager.GetType().Name}");
            sb.AppendLine($"检测到的动作：{animLength}  （扫描 0–{animLength - 1}）");
            sb.AppendLine();
            sb.AppendLine($"{"动作",-4} {"名称",-24} {"方向0",12} {"方向1",12} {"方向2",12} {"方向3",12} {"方向4",12}  {"状态",-8}");
            sb.AppendLine(new string('─', 100));

            int totalOk = 0, totalEmpty = 0, totalCorrupt = 0;

            for (int action = 0; action < animLength; action++)
            {
                string actName = action < actionNames.Length
                    ? actionNames[action] : $"动作{action:D2}";

                var dirInfos = new string[5];
                var dirDetails = new System.Text.StringBuilder();
                bool anyValid = false, anyEmpty = false, anyCorrupt = false;

                for (int dir = 0; dir < 5; dir++)
                {
                    // ── 步骤 1：获取 UOP 条目 ────────────────────────────────────────
                    var fileInfo = _uopManager.GetAnimationData(body, action, dir);
                    if (fileInfo == null)
                    {
                        dirInfos[dir] = "缺失";
                        anyEmpty = true;
                        continue;
                    }

                    // ── 步骤 2：加载原始数据 ────────────────────────────────────────
                    byte[] raw = null;
                    try { raw = fileInfo.GetData(); }
                    catch { }

                    if (raw == null || raw.Length < 30)
                    {
                        dirInfos[dir] = raw == null ? "无数据" : "太短";
                        anyCorrupt = true;
                        continue;
                    }

                    // ── 步骤 3：验证 Magic "AMOU" ──────────────────────────────────
                    if (raw[0] != 0x41 || raw[1] != 0x4D || raw[2] != 0x4F || raw[3] != 0x55)
                    {
                        dirInfos[dir] = "!魔术";
                        anyCorrupt = true;
                        continue;
                    }

                    // ── 步骤 4：在字节 28 处读取 frameCount（uint16 LE）───────────────
                    int fc = raw[28] | (raw[29] << 8);

                    if (fc <= 0 || fc > 256)
                    {
                        dirInfos[dir] = $"!fc={fc}";
                        anyCorrupt = true;
                        continue;
                    }

                    // ── 步骤 5：从 UOP 标头读取 Frame[0] 尺寸 ─────────────
                    // [16..17] CX  [18..19] CY  [20..21] W  [22..23] H
                    int cx = (short)(raw[16] | (raw[17] << 8));
                    int cy = (short)(raw[18] | (raw[19] << 8));
                    int w = raw[20] | (raw[21] << 8);
                    int h = raw[22] | (raw[23] << 8);
                    bool gotFrame = (w > 0 && w < 2048 && h > 0 && h < 2048);

                    string filePath = fileInfo.File?.FilePath ?? "(内存)";
                    string fileShort = System.IO.Path.GetFileName(filePath) ?? "(内存)";

                    dirInfos[dir] = $"fc={fc}";
                    dirDetails.AppendLine(
                        $"         方向{dir}：文件={fileShort,-28}  大小={raw.Length,7}B" +
                        $"  帧数={fc,3}" +
                        (gotFrame ? $"  F0：{w,4}×{h,-4}  CX={cx,4}  CY={cy,4}" : "  F0：不可用"));
                    anyValid = true;
                }

                if (anyValid) totalOk++;
                if (anyEmpty && !anyValid) totalEmpty++;
                if (anyCorrupt) totalCorrupt++;

                string status = anyCorrupt ? "损坏"
                              : !anyValid ? "缺失"
                              : anyEmpty ? "部分"
                              : "正常";

                sb.Append($"{action:D2}   {actName,-24}");
                foreach (var d in dirInfos) sb.Append($" {d,12}");
                sb.AppendLine($"  {status,-8}");

                if (dirDetails.Length > 0)
                    sb.Append(dirDetails);
            }

            sb.AppendLine();
            sb.AppendLine(new string('─', 100));
            sb.AppendLine($"摘要：  正常={totalOk}  部分/缺失={totalEmpty}  损坏={totalCorrupt}");

            string fullText = sb.ToString();

            // ── 保存到临时文件 ────────────────────────────────────────────────────
            string dumpPath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                $"ActionMapUOP_Body{body}.txt");
            System.IO.File.WriteAllText(dumpPath, fullText);

            // ── 构建对话框 ─────────────────────────────────────────────────────────
            var dlg = new Form
            {
                Text = $"动作映射 UOP — 身体 {body}  [{typLabel}]",
                Size = new System.Drawing.Size(1060, 660),
                MinimumSize = new System.Drawing.Size(700, 400),
                StartPosition = FormStartPosition.CenterParent,
                Icon = this.Icon
            };

            var toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 34,
                BackColor = System.Drawing.Color.FromArgb(38, 38, 50)
            };

            var btnCopyText = new Button
            {
                Text = "复制文本",
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(0, 90, 150),
                ForeColor = System.Drawing.Color.White,
                Width = 90,
                Height = 26,
                Left = 6,
                Top = 4,
                Font = new System.Drawing.Font("Segoe UI", 8.5f)
            };
            btnCopyText.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 130, 200);

            var btnCopyScreen = new Button
            {
                Text = "复制截图",
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(60, 60, 80),
                ForeColor = System.Drawing.Color.White,
                Width = 120,
                Height = 26,
                Left = 102,
                Top = 4,
                Font = new System.Drawing.Font("Segoe UI", 8.5f)
            };
            btnCopyScreen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(90, 90, 110);

            var lblInfo = new Label
            {
                Text = $"身体 {body}  ·  {typLabel}  ·  {animLength} 个动作 × 5 个方向  ·  UOP",
                ForeColor = System.Drawing.Color.FromArgb(140, 140, 160),
                Font = new System.Drawing.Font("Segoe UI", 8.5f),
                AutoSize = true,
                Left = 230,
                Top = 9
            };

            toolbar.Controls.Add(btnCopyText);
            toolbar.Controls.Add(btnCopyScreen);
            toolbar.Controls.Add(lblInfo);

            var txt = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                WordWrap = false,
                Font = new System.Drawing.Font("Consolas", 9f),
                BackColor = System.Drawing.Color.FromArgb(28, 28, 32),
                ForeColor = System.Drawing.Color.FromArgb(220, 220, 220)
            };

            foreach (var line in fullText.Split('\n'))
            {
                int start = txt.TextLength;
                txt.AppendText(line + "\n");

                System.Drawing.Color col;
                if (line.Contains(" 正常")) col = System.Drawing.Color.FromArgb(100, 220, 130);
                else if (line.Contains("损坏")) col = System.Drawing.Color.FromArgb(255, 100, 100);
                else if (line.Contains("缺失") || line.Contains("部分")) col = System.Drawing.Color.FromArgb(255, 200, 80);
                else if (line.TrimStart().StartsWith("方向")) col = System.Drawing.Color.FromArgb(140, 180, 255);
                else if (line.StartsWith("===")) col = System.Drawing.Color.FromArgb(200, 180, 255);
                else if (line.StartsWith("摘要")) col = System.Drawing.Color.FromArgb(200, 200, 100);
                else col = System.Drawing.Color.FromArgb(220, 220, 220);

                txt.Select(start, line.Length);
                txt.SelectionColor = col;
            }
            txt.SelectionStart = 0;
            txt.SelectionLength = 0;

            var statusBar = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 22,
                Text = $"  保存：{dumpPath}  |  正常={totalOk}  缺失/部分={totalEmpty}  损坏={totalCorrupt}",
                Font = new System.Drawing.Font("Segoe UI", 8f),
                ForeColor = System.Drawing.Color.FromArgb(140, 140, 160),
                BackColor = System.Drawing.Color.FromArgb(38, 38, 50)
            };

            btnCopyText.Click += (s, ev) =>
            {
                Clipboard.SetText(fullText);
                btnCopyText.Text = "已复制！";
                var t = new System.Windows.Forms.Timer { Interval = 1500 };
                t.Tick += (_, __) => { btnCopyText.Text = "复制文本"; t.Stop(); t.Dispose(); };
                t.Start();
            };

            btnCopyScreen.Click += (s, ev) =>
            {
                var bmp = new System.Drawing.Bitmap(dlg.Width, dlg.Height);
                dlg.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));
                Clipboard.SetImage(bmp);
                btnCopyScreen.Text = "已复制！";
                var t = new System.Windows.Forms.Timer { Interval = 1500 };
                t.Tick += (_, __) => { btnCopyScreen.Text = "复制截图"; t.Stop(); t.Dispose(); };
                t.Start();
            };

            dlg.Controls.Add(txt);
            dlg.Controls.Add(toolbar);
            dlg.Controls.Add(statusBar);
            dlg.ShowDialog(this);
        }

        #endregion

        private void comboBoxBackground_SelectedIndexChanged(object sender, EventArgs e)
        {
            _backgroundImage?.Dispose();
            _backgroundImage = null;

            _backgroundMode = comboBoxBackground.SelectedItem?.ToString() ?? "无";

            switch (_backgroundMode)
            {
                case "草地":
                    _backgroundImage = new Bitmap(
                        UoFiddler.Controls.Properties.Resources.Grass);
                    break;
                case "水":
                    _backgroundImage = new Bitmap(
                        UoFiddler.Controls.Properties.Resources.Water);
                    break;
                    // “无” → _backgroundImage 保持 null
            }

            AnimationPictureBox.Invalidate();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _backgroundImage?.Dispose();
            base.OnFormClosed(e);
        }


    }
}
