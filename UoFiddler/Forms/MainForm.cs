// /***************************************************************************
//  *
//  * $Author: Turley
//  * Advanced Nikodemus
//  * 
//  * \"啤酒-葡萄酒许可证\"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯啤酒和葡萄酒作为回报。
//  *
//  ***************************************************************************/

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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ultima;
using Ultima.Helpers;
using UoFiddler.Classes;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Plugin;
using UoFiddler.Plugin.ConverterMultiTextPlugin.Forms;
using WMPLib;

namespace UoFiddler.Forms
{
    public partial class MainForm : Form
    {
        //Bin_Dec_Hex_ConverterForm
        private Bin_Dec_Hex_ConverterForm _binDecHexConverterForm;

        private WindowsMediaPlayer _player = new WindowsMediaPlayer(); // 从 UO 目录加载 Mp3
        private Timer timer; // Mp3 计时器
        private bool isStoppedByUser = false; // 停止 Mp3 计时器



        //AlarmClock
        private AlarmClockForm _alarmClockForm;
        public MainForm()
        {
            InitializeComponent();

            // 原始
            if (FiddlerOptions.StoreFormState)
            {
                if (FiddlerOptions.MaximisedForm)
                {
                    StartPosition = FormStartPosition.Manual;
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    if (IsOkFormStateLocation(FiddlerOptions.FormPosition, FiddlerOptions.FormSize))
                    {
                        StartPosition = FormStartPosition.Manual;
                        WindowState = FormWindowState.Normal;
                        Location = FiddlerOptions.FormPosition;
                        Size = FiddlerOptions.FormSize;
                    }
                }
            }

            // 请定义所需的选项卡顺序。
            string[] tabOrder = new string[] { "StartTab", "ItemsTab", "GumpsTab", "DressTab", "TileDataTab", "LandTilesTab", "TextureTab", "MapTab", "MultiMapTab", "MultisTab", "RadarColTab", "HuesTab", "AnimationTab", "AnimDataTab", "LightTab", "SoundsTab", "SkillsTab", "SkillGrpTab", "SpeechTab", "ClilocTab", "FontsTab", };

            // 创建一个新的 TabPage 列表。
            List<TabPage> orderedPages = new List<TabPage>();

            // 按所需顺序将 TabPage 添加到列表。
            foreach (string tabName in tabOrder)
            {
                TabPage tabPage = TabPanel.TabPages[tabName];
                orderedPages.Add(tabPage);
            }

            // 从 TabPanel 中删除所有 TabPage。
            TabPanel.TabPages.Clear();

            // 将重新排序的 TabPage 添加到 TabPanel。
            foreach (TabPage tabPage in orderedPages)
            {
                TabPanel.TabPages.Add(tabPage);
            }

            // 设置 TabPanel 控件的 DrawMode 属性
            TabPanel.DrawMode = TabDrawMode.OwnerDrawFixed;
            // 将 TabPanel_DrawItem 方法注册为 TabPanel 控件 DrawItem 事件的事件处理程序
            TabPanel.DrawItem += TabPanel_DrawItem;

            // 图标
            Icon = Options.GetFiddlerIcon();
            // 版本
            Versionlabel.Text = $"版本 {FiddlerOptions.AppVersion.Major}.{FiddlerOptions.AppVersion.Minor}.{FiddlerOptions.AppVersion.Build}";
            Versionlabel.Left = StartTab.Size.Width - Versionlabel.Width - 5;
            // 加载插件
            LoadExternToolStripMenu();
            GlobalPlugins.Plugins.FindPlugins($@"{Application.StartupPath}\plugins");

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            foreach (AvailablePlugin plug in GlobalPlugins.Plugins.AvailablePlugins)
            {
                if (!plug.Loaded)
                {
                    continue;
                }

                plug.Instance.ModifyPluginToolStrip(toolStripDropDownButtonPlugins);
                plug.Instance.ModifyTabPages(TabPanel);
            }

            foreach (TabPage tab in TabPanel.TabPages)
            {
                if ((int)tab.Tag >= 0 && (int)tab.Tag < Options.ChangedViewState.Count &&
                    !Options.ChangedViewState[(int)tab.Tag])
                {
                    ToggleView(tab);
                }
            }

            // 将可用图像添加到 toolStripComboBoxImage。
            toolStripComboBoxImage.Items.AddRange(new[] { "UOFiddler", "UOFiddler1", "UOFiddler2", "UOFiddler3", "UOFiddler4", "UOFiddler5", "UOFiddler6", "UOFiddler7", "UOFiddler8", "UOFiddler9", "UOFiddler10", "UOFiddler11", "UOFiddler12" });

            // 为 toolStripComboBoxImage 的 SelectedIndexChanged 事件注册事件处理程序。
            toolStripComboBoxImage.SelectedIndexChanged += ImageSwitcher_SelectedIndexChanged;

            // 加载保存的用户选择并相应地设置背景图像。
            var selectedImage = Properties.Settings.Default.SelectedImage;
            switch (selectedImage)
            {
                case "UOFiddler":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler; // 将 StartTab 的背景图像设置为 UOFiddler。
                    break;
                case "UOFiddler1":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler1; // 将 StartTab 的背景图像设置为 UOFiddler1
                    break;
                case "UOFiddler2":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler2; // 将 StartTab 的背景图像设置为 UOFiddler2
                    break;
                case "UOFiddler3":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler3; // 将 StartTab 的背景图像设置为 UOFiddler3
                    break;
                case "UOFiddler4":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler4; // 将 StartTab 的背景图像设置为 UOFiddler4
                    break;
                case "UOFiddler5":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler5; // 将 StartTab 的背景图像设置为 UOFiddler5
                    break;
                case "UOFiddler6":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler6; // 将 StartTab 的背景图像设置为 UOFiddler6
                    break;
                case "UOFiddler7":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler7; // 将 StartTab 的背景图像设置为 UOFiddler7
                    break;
                case "UOFiddler8":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler8; // 将 StartTab 的背景图像设置为 UOFiddler8
                    break;
                case "UOFiddler9":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler9; // 将 StartTab 的背景图像设置为 UOFiddler9
                    break;
                case "UOFiddler10":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler10; // 将 StartTab 的背景图像设置为 UOFiddler10
                    break;
                case "UOFiddler11":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler11; // 将 StartTab 的背景图像设置为 UOFiddler11
                    break;
                case "UOFiddler12":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler12; // 将 StartTab 的背景图像设置为 UOFiddler12
                    break;
            }


            // 闹钟设置位置 User.config
            if (Properties.Settings.Default.FormLocationAlarm != Point.Empty)
            {
                this.Location = Properties.Settings.Default.FormLocationAlarm;
            }


            _player.PlayStateChange += Player_PlayStateChange; // 注册事件

            // 初始化计时器
            timer = new Timer();
            timer.Interval = 1000; // 1 秒
            timer.Tick += Timer_Tick;

            // 绑定 Load 事件处理程序
            this.Load += new EventHandler(MainForm_Load);
        }

        #region [ TabPanel_DrawItem => 选项卡设计 ]
        // TabPanel_DrawItem 方法是 TabPanel 控件 DrawItem 事件的事件处理程序
        private void TabPanel_DrawItem(object sender, DrawItemEventArgs e)
        {
            // 获取 Graphics 对象以绘制选项卡
            Graphics g = e.Graphics;
            // 创建画笔以绘制选项卡文本
            Brush textBrush = new SolidBrush(TabPanel.ForeColor);
            // 获取当前 TabPage
            TabPage tabPage = TabPanel.TabPages[e.Index];
            // 获取当前 TabPage 的边界
            Rectangle tabBounds = TabPanel.GetTabRect(e.Index);
            // 根据选项卡名称选择背景颜色

            // 检查当前选项卡是否被选中。
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // 根据选择状态设置背景颜色。
            Color backColor = isSelected ? Color.LightBlue : TabPanel.BackColor;

            // 选中选项卡的高亮颜色。
            Color highlightColor = Color.Yellow;

            // 如果选项卡被选中则应用高亮。
            if (isSelected)
            {
                // 在选项卡区域周围绘制高亮边框。
                using (Pen highlightPen = new Pen(highlightColor, 2))
                {
                    g.DrawRectangle(highlightPen, tabBounds);
                }
            }


            //Color backColor;
            switch (tabPage.Name)
            {
                case "MapTab":
                case "TextureTab":
                case "LandTilesTab":
                case "MultiMapTab":
                    backColor = Color.LightGreen;
                    break;
                case "ItemsTab":
                case "GumpsTab":
                case "DressTab":
                case "TileDataTab":
                    backColor = Color.LightBlue;
                    break;
                case "MultisTab":
                case "RadarColTab":
                case "HuesTab":
                    backColor = Color.Orange;
                    break;
                case "AnimationTab":
                case "AnimDataTab":
                    backColor = Color.LightCoral;
                    break;
                case "SoundsTab":
                case "LightTab":
                case "SkillsTab":
                case "SkillGrpTab":
                case "ClilocTab":
                case "FontsTab":
                case "SpeechTab":
                    backColor = Color.White;
                    break;
                    /*default:
                        backColor = TabPanel.BackColor;
                        //backColor = Color.Red;
                        break;*/
            }
            // 用所选颜色填充当前 TabPage 的背景
            g.FillRectangle(new SolidBrush(backColor), e.Bounds);
            // 创建 StringFormat 对象以将文本居中于 TabPage
            StringFormat stringFlags = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            // 绘制当前 TabPage 的文本
            g.DrawString(tabPage.Text, TabPanel.Font, textBrush, tabBounds, stringFlags);
            // 释放画笔
            textBrush.Dispose();
        }
        #endregion

        #region [ PathSettingsForm ]
        private PathSettingsForm _pathSettingsForm = new PathSettingsForm();

        private void Click_path(object sender, EventArgs e)
        {
            if (_pathSettingsForm.IsDisposed)
            {
                _pathSettingsForm = new PathSettingsForm();
            }
            else
            {
                _pathSettingsForm.Focus();
            }

            _pathSettingsForm.TopMost = true;
            _pathSettingsForm.Show();
        }
        #endregion

        #region [ OnClickAlwaysTop ]
        private void OnClickAlwaysTop(object sender, EventArgs e)
        {
            TopMost = AlwaysOnTopMenuitem.Checked;
            ControlEvents.FireAlwaysOnTopChangeEvent(TopMost);
        }
        #endregion

        #region [ 重新加载文件 ]
        private void ReloadFiles(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Verdata.Initialize();

            if (Options.LoadedUltimaClass["Art"] || Options.LoadedUltimaClass["TileData"])
            {
                // 看起来我们必须先重新加载 art 才能正确加载 tiledata
                // 这里的顺序很重要
                Art.Reload();
                TileData.Initialize();
            }

            if (Options.LoadedUltimaClass["Hues"])
            {
                Hues.Initialize();
            }

            if (Options.LoadedUltimaClass["ASCIIFont"])
            {
                AsciiText.Initialize();
            }

            if (Options.LoadedUltimaClass["UnicodeFont"])
            {
                UnicodeFonts.Initialize();
            }

            if (Options.LoadedUltimaClass["Animdata"])
            {
                Animdata.Initialize();
            }

            if (Options.LoadedUltimaClass["Light"])
            {
                Light.Reload();
            }

            if (Options.LoadedUltimaClass["Skills"])
            {
                Skills.Reload();
            }

            if (Options.LoadedUltimaClass["Sound"])
            {
                Sounds.Initialize();
            }

            if (Options.LoadedUltimaClass["Texture"])
            {
                Textures.Reload();
            }

            if (Options.LoadedUltimaClass["Gumps"])
            {
                Gumps.Reload();
            }

            if (Options.LoadedUltimaClass["Animations"])
            {
                Animations.Reload();
            }

            if (Options.LoadedUltimaClass["RadarColor"])
            {
                RadarCol.Initialize();
            }

            if (Options.LoadedUltimaClass["Map"])
            {
                MapHelper.CheckForNewMapSize();
                Map.Reload();
            }

            if (Options.LoadedUltimaClass["Multis"])
            {
                Multis.Reload();
            }

            if (Options.LoadedUltimaClass["Speech"])
            {
                SpeechList.Initialize();
            }

            if (Options.LoadedUltimaClass["AnimationEdit"])
            {
                AnimationEdit.Reload();
            }

            ControlEvents.FireFilePathChangeEvent();

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region [ LoadExternToolsStripMenu ]

        /// <summary>
        /// 重新加载外部工具下拉菜单 <see cref="FiddlerOptions.ExternTools"/>
        /// </summary>
        ///
        public void LoadExternToolStripMenu()
        {
            ExternToolsDropDown.DropDownItems.Clear();

            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = "管理.."
            };
            item.Click += OnClickToolManage;

            ExternToolsDropDown.DropDownItems.Add(item);
            ExternToolsDropDown.DropDownItems.Add(new ToolStripSeparator());

            if (FiddlerOptions.ExternTools is null)
            {
                return;
            }

            for (int i = 0; i < FiddlerOptions.ExternTools.Count; i++)
            {
                ExternTool tool = FiddlerOptions.ExternTools[i];
                item = new ToolStripMenuItem
                {
                    Text = tool.Name,
                    Tag = i
                };

                item.DropDownItemClicked += ExternTool_ItemClicked;

                ToolStripMenuItem sub = new ToolStripMenuItem
                {
                    Text = "启动",
                    Tag = -1
                };

                item.DropDownItems.Add(sub);
                item.DropDownItems.Add(new ToolStripSeparator());

                for (int j = 0; j < tool.Args.Count; j++)
                {
                    ToolStripMenuItem arg = new ToolStripMenuItem
                    {
                        Text = tool.ArgsName[j],
                        Tag = j
                    };
                    item.DropDownItems.Add(arg);
                }

                ExternToolsDropDown.DropDownItems.Add(item);
            }
        }
        #endregion

        #region [ ExternTool ]
        private static void ExternTool_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int argInfo = (int)e.ClickedItem.Tag;
            int toolInfo = (int)e.ClickedItem.OwnerItem.Tag;

            if (toolInfo < 0 || argInfo < -1)
            {
                return;
            }

            using (Process process = new Process())
            {
                ExternTool tool = FiddlerOptions.ExternTools[toolInfo];
                process.StartInfo.FileName = tool.FileName;
                if (argInfo >= 0)
                {
                    process.StartInfo.Arguments = tool.Args[argInfo];
                }

                try
                {
                    process.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "启动工具时出错",
                        MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                }
            }
        }
        #endregion

        #region [ ManageToolsForm ]
        private ManageToolsForm _manageForm;

        private void OnClickToolManage(object sender, EventArgs e)
        {
            if (_manageForm?.IsDisposed == false)
            {
                return;
            }

            _manageForm = new ManageToolsForm(LoadExternToolStripMenu)
            {
                TopMost = true
            };
            _manageForm.Show();
        }
        #endregion

        #region [ OptionsForm ]
        private OptionsForm _optionsForm;

        private void OnClickOptions(object sender, EventArgs e)
        {
            if (_optionsForm?.IsDisposed == false)
            {
                return;
            }

            _optionsForm = new OptionsForm(
                UpdateAllTileViews,
                UpdateItemsTab,
                UpdateSoundTab,
                UpdateMapTab)
            {
                TopMost = true
            };
            _optionsForm.Show();
        }
        #endregion

        #region [ UpdateAllTilesViews ]
        /// <summary>
        /// 更新所有图块视图选项卡
        /// </summary>
        /// 
        private void UpdateAllTileViews()
        {
            UpdateItemsTab();
            UpdateLandTilesTab();
            UpdateTexturesTab();
            UpdateFontsTab();
        }
        #endregion

        #region [ UpdateItemsTab ]
        /// <summary>
        /// 更新项目选项卡
        /// </summary>
        /// 
        private void UpdateItemsTab()
        {
            itemShowControl.UpdateTileView();
        }
        #endregion

        #region [ UpdateTileTab ]
        /// <summary>
        /// 更新地形图块选项卡
        /// </summary>
        /// 
        private void UpdateLandTilesTab()
        {
            landTilesControl.UpdateTileView();
        }
        #endregion

        #region [ UpdateTextureTab ]

        /// <summary>
        /// 更新纹理选项卡
        /// </summary>
        /// 
        private void UpdateTexturesTab()
        {
            textureControl.UpdateTileView();
        }
        #endregion

        #region [ UpdateFontsTab ]

        /// <summary>
        /// 更新字体选项卡
        /// </summary>
        /// 
        private void UpdateFontsTab()
        {
            fontsControl.UpdateTileView();
        }
        #endregion

        #region [ UpdateMapTab ]

        /// <summary>
        /// 更新地图选项卡
        /// </summary>
        /// 
        private void UpdateMapTab()
        {
            if (Options.LoadedUltimaClass["Map"])
            {
                Map.Reload();
            }

            ControlEvents.FireMapSizeChangeEvent();
        }
        #endregion

        #region [ UpdateSoundTab ]

        /// <summary>
        /// 更新声音选项卡
        /// </summary>
        /// 
        private void UpdateSoundTab()
        {
            soundControl.Reload();
        }
        #endregion

        #region [ 停靠和取消停靠 ]
        private void OnClickUnDock(object sender, EventArgs e)
        {
            int tag = (int)TabPanel.SelectedTab.Tag;
            if (tag <= 0)
            {
                return;
            }

            new UnDockedForm(TabPanel.SelectedTab, ReDock).Show();
            TabPanel.TabPages.Remove(TabPanel.SelectedTab);
        }

        /// <summary>
        /// 重新停靠已关闭的窗体
        /// </summary>
        /// <param name="oldTab"></param>
        public void ReDock(TabPage oldTab)
        {
            bool done = false;
            foreach (TabPage page in TabPanel.TabPages)
            {
                if ((int)page.Tag <= (int)oldTab.Tag)
                {
                    continue;
                }

                TabPanel.TabPages.Insert(TabPanel.TabPages.IndexOf(page), oldTab);
                done = true;
                break;
            }

            if (!done)
            {
                TabPanel.TabPages.Add(oldTab);
            }

            TabPanel.SelectedTab = oldTab;
        }
        #endregion

        #region [ ManagePlugins ]
        private ManagePluginsForm _pluginsFormForm;

        private void OnClickManagePlugins(object sender, EventArgs e)
        {
            if (_pluginsFormForm?.IsDisposed == false)
            {
                return;
            }

            _pluginsFormForm = new ManagePluginsForm
            {
                TopMost = true
            };
            _pluginsFormForm.Show();
        }
        #endregion

        #region [ OnClosing ]
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            FiddlerOptions.Logger.Information("MainForm - OnClosing - 开始");
            string files = Options.ChangedUltimaClass
                                    .Where(key => key.Value)
                                    .Aggregate(string.Empty, (current, key) => current + $"- {key.Key} \r\n");

            if (files.Length > 0)
            {
                DialogResult result =
                    MessageBox.Show($"确定要退出吗？\r\n\r\n存在未保存的文件：\r\n{files}",
                        "未保存的更改", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            FiddlerOptions.MaximisedForm = WindowState == FormWindowState.Maximized;
            FiddlerOptions.FormPosition = this.Location;
            FiddlerOptions.FormSize = this.Size;

            FiddlerOptions.Logger.Information("MainForm - OnClosing - 卸载插件");
            GlobalPlugins.Plugins.ClosePlugins();

            FiddlerOptions.Logger.Information("MainForm - OnClosing - 完成");
        }
        #endregion

        #region [ IsOkFormStateLocation ]
        private bool IsOkFormStateLocation(Point loc, Size size)
        {
            int maxX = Screen.PrimaryScreen.WorkingArea.Width;
            int maxY = Screen.PrimaryScreen.WorkingArea.Height;

            // 调整 X 和 Y 坐标            
            loc = AdjustCoordinates(loc, size, maxX, maxY);

            // 调整大小            
            size = AdjustSize(size);

            // 检查位置和大小是否有效并返回结果            
            bool isValid = (loc.X >= 0 &&
                            loc.Y >= 0 &&
                            loc.X + size.Width <= maxX &&
                            loc.Y + size.Height <= maxY);

            if (!isValid)
            {
                throw new Exception("窗口的位置或大小无效。");
                // 如果窗口的位置或大小无效，则抛出异常。
            }

            return isValid;
        }

        private Point AdjustCoordinates(Point loc, Size size, int maxX, int maxY)
        {
            // 检查 X 坐标            
            if (loc.X < 0 || loc.X + size.Width > maxX)
            {
                loc.X = Math.Max(0, maxX - size.Width);
            }

            // 检查 Y 坐标            
            if (loc.Y < 0 || loc.Y + size.Height > maxY)
            {
                loc.Y = Math.Max(0, maxY - size.Height);
            }

            return loc;
        }

        private Size AdjustSize(Size size)
        {
            // 也检查大小            
            if (size.Width > Screen.PrimaryScreen.WorkingArea.Width ||
                size.Height > Screen.PrimaryScreen.WorkingArea.Height)
            {
                // 大小太大，调整它                
                size = new Size(
                    Math.Min(size.Width, Screen.PrimaryScreen.WorkingArea.Width),
                    Math.Min(size.Height, Screen.PrimaryScreen.WorkingArea.Height)
                );
            }

            return size;
        }

        #endregion

        #region [ 查看选项卡列表 ]
        private void ToggleView(object sender, EventArgs e)
        {
            ToolStripMenuItem theMenuItem = (ToolStripMenuItem)sender;
            TabPage thePage = TabFromTag((int)theMenuItem.Tag);

            int tag = (int)thePage.Tag;

            if (theMenuItem.Checked)
            {
                if (!TabPanel.TabPages.Contains(thePage))
                {
                    return;
                }

                theMenuItem.Checked = false;
                TabPanel.TabPages.Remove(thePage);
                Options.ChangedViewState[tag] = false;
            }
            else
            {
                theMenuItem.Checked = true;
                bool done = false;
                foreach (TabPage page in TabPanel.TabPages)
                {
                    if ((int)page.Tag <= tag)
                    {
                        continue;
                    }

                    TabPanel.TabPages.Insert(TabPanel.TabPages.IndexOf(page), thePage);
                    done = true;
                    break;
                }

                if (!done)
                {
                    TabPanel.TabPages.Add(thePage);
                }

                Options.ChangedViewState[tag] = true;
            }
        }

        private void ToggleView(TabPage thePage)
        {
            int tag = (int)thePage.Tag;
            ToolStripMenuItem theMenuItem = MenuFromTag(tag);

            if (theMenuItem.Checked)
            {
                if (!TabPanel.TabPages.Contains(thePage))
                {
                    return;
                }

                theMenuItem.Checked = false;
                TabPanel.TabPages.Remove(thePage);
                Options.ChangedViewState[tag] = false;
            }
            else
            {
                theMenuItem.Checked = true;
                bool done = false;
                foreach (TabPage page in TabPanel.TabPages)
                {
                    if ((int)page.Tag <= tag)
                    {
                        continue;
                    }

                    TabPanel.TabPages.Insert(TabPanel.TabPages.IndexOf(page), thePage);
                    done = true;
                    break;
                }

                if (!done)
                {
                    TabPanel.TabPages.Add(thePage);
                }

                Options.ChangedViewState[tag] = true;
            }
        }


        private TabPage TabFromTag(int tag)
        {
            switch (tag)
            {
                case 0: return StartTab;
                case 1: return MultisTab;
                case 2: return AnimationTab;
                case 3: return ItemsTab;
                case 4: return LandTilesTab;
                case 5: return TextureTab;
                case 6: return GumpsTab;
                case 7: return SoundsTab;
                case 8: return HuesTab;
                case 9: return FontsTab;
                case 10: return ClilocTab;
                case 11: return MapTab;
                case 12: return LightTab;
                case 13: return SpeechTab;
                case 14: return SkillsTab;
                case 15: return AnimDataTab;
                case 16: return MultiMapTab;
                case 17: return DressTab;
                case 18: return TileDataTab;
                case 19: return RadarColTab;
                case 20: return SkillGrpTab;
                default: return StartTab;
            }
        }


        private ToolStripMenuItem MenuFromTag(int tag)
        {
            switch (tag)
            {
                case 0: return ToggleViewStart;
                case 1: return ToggleViewMulti;
                case 2: return ToggleViewAnimations;
                case 3: return ToggleViewItems;
                case 4: return ToggleViewLandTiles;
                case 5: return ToggleViewTexture;
                case 6: return ToggleViewGumps;
                case 7: return ToggleViewSounds;
                case 8: return ToggleViewHue;
                case 9: return ToggleViewFonts;
                case 10: return ToggleViewCliloc;
                case 11: return ToggleViewMap;
                case 12: return ToggleViewLight;
                case 13: return ToggleViewSpeech;
                case 14: return ToggleViewSkills;
                case 15: return ToggleViewAnimData;
                case 16: return ToggleViewMultiMap;
                case 17: return ToggleViewDress;
                case 18: return ToggleViewTileData;
                case 19: return ToggleViewRadarColor;
                case 20: return ToggleViewSkillGrp;
                default: return ToggleViewStart;
            }
        }
        #endregion

        #region [ Polserver 链接 ]
        private void ToolStripMenuItemHelp_Click(object sender, EventArgs e)
        {
            using (HelpDokuForm helpDokuForm = new HelpDokuForm())
            {
                helpDokuForm.FileName = "UOFiddler.htm";
                helpDokuForm.ShowDialog();
            }
        }
        #endregion

        #region [ FileFormatGerman Html ]
        private void ToolStripMenuItemFileFormatsGerman_Click(object sender, EventArgs e)
        {
            using (HelpDokuForm helpDokuForm = new HelpDokuForm())
            {
                helpDokuForm.FileName = "FileFormatsGerman.html";
                helpDokuForm.ShowDialog();
            }
        }
        #endregion

        #region [ FileFormatEnglisch Html ]
        private void ToolStripMenuItemFileFormatsEnglisch_Click(object sender, EventArgs e)
        {
            using (HelpDokuForm helpDokuForm = new HelpDokuForm())
            {
                helpDokuForm.FileName = "FileFormatsEnglisch.html";
                helpDokuForm.ShowDialog();
            }
        }
        #endregion

        #region [ Animation Html ]
        private void AnimationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (HelpDokuForm helpDokuForm = new HelpDokuForm())
            {
                helpDokuForm.FileName = "Animations.html";
                helpDokuForm.ShowDialog();
            }
        }
        #endregion

        #region [ Animation Install German ]
        private void AnimationInstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (HelpDokuForm helpDokuForm = new HelpDokuForm())
            {
                helpDokuForm.FileName = "AnimationInstallation.html";
                helpDokuForm.ShowDialog();
            }
        }
        #endregion

        #region [ Animation Install Englisch ]
        private void AnimationInstallEnglischToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (HelpDokuForm helpDokuForm = new HelpDokuForm())
            {
                helpDokuForm.FileName = "AnimationInstallationEng.html";
                helpDokuForm.ShowDialog();
            }
        }
        #endregion

        #region [ 关于 ]
        private void ToolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            using (AboutBoxForm aboutBoxForm = new AboutBoxForm())
            {
                aboutBoxForm.ShowDialog(this);
            }
        }
        #endregion

        #region [ 更新日志 ]
        private void ChangelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ChangeLogForm changelogForm = new ChangeLogForm())
            {
                changelogForm.ShowDialog(this);
            }
        }
        #endregion

        #region [ 删除 WebView 缓存 ]
        private void HelpDokuForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 获取 %LOCALAPPDATA% 目录的路径
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // 在 %LOCALAPPDATA% 目录中创建 UoFiddler.exe.WebView2 文件夹的路径
            string userDataFolder = Path.Combine(localAppData, "UoFiddler.exe.WebView2");
            // 检查 UoFiddler.exe.WebView2 文件夹是否存在
            if (Directory.Exists(userDataFolder))
            {
                // 删除 UoFiddler.exe.WebView2 文件夹
                Directory.Delete(userDataFolder, true);
            }
        }
        #endregion

        #region [ 图像切换 ]
        private void ToolStripComboBoxImage_Click(object sender, EventArgs e)
        {
            // 将可用图像添加到 toolStripComboBoxImage。
            toolStripComboBoxImage.Items.AddRange(new[] { "UOFiddler", "UOFiddler1", "UOFiddler2", "UOFiddler3", "UOFiddler4", "UOFiddler5", "UOFiddler6", "UOFiddler7", "UOFiddler8", "UOFiddler9", "UOFiddler10", "UOFiddler11", "UOFiddler12" });
        }

        // SelectedIndexChanged 事件的事件处理程序。
        private void ImageSwitcher_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedImage = ((ToolStripComboBox)sender).SelectedItem.ToString();

            // 将用户的选择保存在用户设置中。
            Properties.Settings.Default.SelectedImage = selectedImage;
            Properties.Settings.Default.Save();

            switch (selectedImage)
            {
                case "UOFiddler":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler; // 将 StartTab 的背景图像设置为 UOFiddler
                    break;
                case "UOFiddler1":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler1; // 将 StartTab 的背景图像设置为 UOFiddler1
                    break;
                case "UOFiddler2":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler2; // 将 StartTab 的背景图像设置为 UOFiddler2
                    break;
                case "UOFiddler3":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler3; // 将 StartTab 的背景图像设置为 UOFiddler3
                    break;
                case "UOFiddler4":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler4; // 将 StartTab 的背景图像设置为 UOFiddler4
                    break;
                case "UOFiddler5":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler5; // 将 StartTab 的背景图像设置为 UOFiddler5。
                    break;
                case "UOFiddler6":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler6; // 将 StartTab 的背景图像设置为 UOFiddler6。
                    break;
                case "UOFiddler7":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler7; // 将 StartTab 的背景图像设置为 UOFiddler7。
                    break;
                case "UOFiddler8":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler8; // 将 StartTab 的背景图像设置为 UOFiddler8。
                    break;
                case "UOFiddler9":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler9; // 将 StartTab 的背景图像设置为 UOFiddler9。
                    break;
                case "UOFiddler10":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler10; // 将 StartTab 的背景图像设置为 UOFiddler10。
                    break;
                case "UOFiddler11":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler11; // 将 StartTab 的背景图像设置为 UOFiddler11。
                    break;
                case "UOFiddler12":
                    StartTab.BackgroundImage = Properties.Resources.UOFiddler12; // 将 StartTab 的背景图像设置为 UOFiddler12。
                    break;
            }
        }
        #endregion

        #region [ 链接 ]
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://uo-freeshards.de",
                UseShellExecute = true
            });
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "http://www.uo-pixel.de",
                UseShellExecute = true
            });
        }

        private void UodevuofreeshardsdeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://uodev.uo-freeshards.de/",
                UseShellExecute = true
            });
        }

        #endregion

        #region [ 目录 ]
        private void DirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 从 Options 类获取输出路径
            string path = Options.OutputPath;
            // 启动 Windows 资源管理器进程并将路径作为参数传递
            // 将路径括在引号中以处理包含空格的路径
            System.Diagnostics.Process.Start("explorer.exe", $"\"{path}\"");
        }
        #endregion

        #region [ DecimalHexConverter ]
        private void BinaryDecimalHexadecimalConverterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (_binDecHexConverterForm == null || _binDecHexConverterForm.IsDisposed)
            {
                _binDecHexConverterForm = new Bin_Dec_Hex_ConverterForm()
                {
                    TopMost = true
                };
                _binDecHexConverterForm.Show();
            }
            else
            {
                _binDecHexConverterForm.Focus();
            }
        }
        #endregion

        #region [ 链接 Servuo.com 和 Discord ]
        private void ToolStripMenuItemLink3_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "http://www.servuo.com",
                UseShellExecute = true
            });
        }
        private void ToolStripMenuItemDiscordUoFreeshardsDe_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://discord.com/invite/9zpXy43WWT",
                UseShellExecute = true
            });
        }
        #endregion

        #region [ F1-F12 ]
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 检查是否按下了 F1-F12 键
            //if (keyData >= Keys.F1 && keyData <= Keys.F12)
            if ((keyData >= Keys.F1 && keyData <= Keys.F12) || keyData == Keys.PageUp || keyData == Keys.PageDown)
            {
                // 根据按下的键选择选项卡
                switch (keyData)
                {
                    case Keys.F1:
                        TabPanel.SelectedIndex = 1; // 物品
                        break;
                    case Keys.F2:
                        TabPanel.SelectedIndex = 4; // Tiledata
                        break;
                    case Keys.F3:
                        TabPanel.SelectedIndex = 5; // 地形图块
                        break;
                    case Keys.F4:
                        TabPanel.SelectedIndex = 6; // 纹理
                        break;
                    case Keys.F5:
                        TabPanel.SelectedIndex = 7; // 地图
                        break;
                    case Keys.F6:
                        TabPanel.SelectedIndex = 2; // Gumps
                        break;
                    case Keys.F7:
                        TabPanel.SelectedIndex = 9; // Multis
                        break;
                    case Keys.F8:
                        TabPanel.SelectedIndex = 10; // 雷达颜色
                        break;
                    case Keys.F9:
                        TabPanel.SelectedIndex = 11; // 色调
                        break;
                    case Keys.F10:
                        TabPanel.SelectedIndex = 12; // 动画
                        break;
                    case Keys.F11:
                        TabPanel.SelectedIndex = 13; // 动画数据
                        break;
                    case Keys.F12:
                        TabPanel.SelectedIndex = 14; // 光照
                        break;
                    case Keys.PageUp:
                        // 滚动到上一个选项卡
                        if (TabPanel.SelectedIndex > 0)
                        {
                            TabPanel.SelectedIndex--;
                        }
                        break;
                    case Keys.PageDown:
                        // 滚动到下一个选项卡
                        if (TabPanel.SelectedIndex < TabPanel.TabCount - 1)
                        {
                            TabPanel.SelectedIndex++;
                        }
                        break;
                }

                // 阻止进一步处理该键
                return true;
            }

            // 调用基类继续标准处理
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region [ 打开临时目录 ]
        private void TempDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Application.StartupPath, "tempGrafic");
            if (Directory.Exists(path))
            {
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
            else
            {
                MessageBox.Show("'tempGrafic' 文件夹不存在。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region [ 闹钟 ]
        private void AlarmClockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_alarmClockForm == null || _alarmClockForm.IsDisposed)
            {
                _alarmClockForm = new AlarmClockForm()
                {
                    TopMost = true
                };
                _alarmClockForm.Show();
            }
            else
            {
                _alarmClockForm.Focus();
            }
        }
        #endregion

        #region [ 记事本编辑器 ]
        private NotepadForm _notepadForm; // 声明 NotepadForm 的实例
        private void NotPadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_notepadForm == null || _notepadForm.IsDisposed)
            {
                _notepadForm = new NotepadForm()
                {
                    TopMost = true
                };
                _notepadForm.Show();
            }
            else
            {
                _notepadForm.Focus();
            }
        }
        #endregion

        #region [ 笔记 ]
        private void NotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建一个新窗体
            Form notesForm = new Form()
            {
                Text = "笔记",
                Size = new System.Drawing.Size(400, 300),
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle,
                MaximizeBox = false
            };

            // 创建一个 RichTextBox
            RichTextBox rtxtNotes = new RichTextBox()
            {
                ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical,
                Dock = System.Windows.Forms.DockStyle.Fill,
                ReadOnly = true
            };

            // currentNoteIndex 变量
            int currentNoteIndex = 0;

            // 从 XML 文件加载笔记的方法
            List<string> LoadNotesFromXml()
            {
                // XML 文件的路径
                string xmlFilePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "NotepadMessage.xml");

                // 创建一个列表来存储笔记
                List<string> notes = new List<string>();

                try
                {
                    // 加载 XML 文档
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.Load(xmlFilePath);

                    // 循环遍历 XML 文档中的每个 "note"
                    foreach (System.Xml.XmlNode noteNode in xmlDoc.SelectNodes("/Notes/Note"))
                    {
                        // 从 "Note" 中提取 RTF 文本
                        string rtfText = noteNode.Attributes["rtfText"].Value;

                        // 将 RTF 文本添加到列表
                        notes.Add(rtfText);
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    rtxtNotes.Text = "XML 文件尚未创建。";
                }

                // 返回笔记列表
                return notes;
            }

            // 加载笔记并将第一个添加到 RichTextBox
            List<string> notes = LoadNotesFromXml();
            if (notes.Count > 0)
            {
                rtxtNotes.Rtf = notes[currentNoteIndex];
            }

            // 创建滚动按钮
            Button btnScrollUp = new Button() { Text = "向上滚动", Dock = System.Windows.Forms.DockStyle.Top };
            Button btnScrollDown = new Button() { Text = "向下滚动", Dock = System.Windows.Forms.DockStyle.Bottom };

            // 为按钮添加事件处理程序
            btnScrollUp.Click += (s, ev) =>
            {
                if (currentNoteIndex > 0)
                {
                    currentNoteIndex--;
                    rtxtNotes.Rtf = notes[currentNoteIndex];
                }
            };

            btnScrollDown.Click += (s, ev) =>
            {
                if (currentNoteIndex < notes.Count - 1)
                {
                    currentNoteIndex++;
                    rtxtNotes.Rtf = notes[currentNoteIndex];
                }
            };

            // 将控件添加到窗体
            notesForm.Controls.Add(rtxtNotes);
            notesForm.Controls.Add(btnScrollUp);
            notesForm.Controls.Add(btnScrollDown);

            // 显示窗体
            notesForm.Show();
        }
        #endregion

        #region [ 屏幕截图 ]
        private void ScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建一个与窗体尺寸相同的新 Bitmap 对象
            using (Bitmap bmp = new Bitmap(this.Width, this.Height))
            {
                // 从位图创建一个新的 Graphics 对象
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // 对窗体进行屏幕截图并将其保存到位图
                    g.CopyFromScreen(this.Location, Point.Empty, this.Size);
                }

                // 将位图复制到剪贴板
                Clipboard.SetImage(bmp);
            }
        }
        #endregion

        #region [ 日历窗体 ]

        private CalendarForm _calendarForm; // 声明 NotepadForm 的实例
        private void CalendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_calendarForm == null || _calendarForm.IsDisposed)
            {
                _calendarForm = new CalendarForm()
                {
                    TopMost = true
                };
                _calendarForm.Show();
            }
            else
            {
                _calendarForm.Focus();
            }
        }
        #endregion

        #region [ colorBackgroundToolStripMenuItem ]
        private void ColorBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    // 设置窗体和其它控件的背景颜色
                    this.BackColor = colorDialog.Color;
                    tsMainMenu.BackColor = colorDialog.Color;
                    contextMenuStripMainForm.BackColor = colorDialog.Color;
                    toolTip.BackColor = colorDialog.Color;

                    // 将所选颜色保存在用户设置中
                    Properties.Settings.Default.BackgroundColor = colorDialog.Color.ToArgb();
                    Properties.Settings.Default.Save();
                }
            }
        }
        #endregion

        #region [ MainForm_Load ]
        private void MainForm_Load(object sender, EventArgs e)
        {
            // 加载保存的背景颜色
            int savedColorArgb = Properties.Settings.Default.BackgroundColor;

            // 检查颜色是否已被玩家设置
            if (savedColorArgb == 0) // 假设 0 表示颜色未设置
            {
                // 如果未设置，使用默认颜色
                this.BackColor = SystemColors.Control; // 或任何其他默认颜色
                tsMainMenu.BackColor = SystemColors.Control;
                contextMenuStripMainForm.BackColor = SystemColors.Control;
                toolTip.BackColor = SystemColors.Control;
            }
            else
            {
                // 如果已设置，使用保存的颜色
                Color savedColor = Color.FromArgb(savedColorArgb);
                this.BackColor = savedColor;
                tsMainMenu.BackColor = savedColor;
                contextMenuStripMainForm.BackColor = savedColor;
                toolTip.BackColor = savedColor;
            }
        }
        #endregion

        #region [ ResetColorToolStripMenuItem ]
        private void ResetColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 将背景颜色重置为默认颜色
            Color defaultColor = SystemColors.Control;

            this.BackColor = defaultColor;
            tsMainMenu.BackColor = defaultColor;
            contextMenuStripMainForm.BackColor = defaultColor;
            toolTip.BackColor = defaultColor;

            // 将默认颜色保存在用户设置中
            Properties.Settings.Default.BackgroundColor = defaultColor.ToArgb();
            Properties.Settings.Default.Save();
        }
        #endregion

        #region [ ConvertLineToolStripMenuItem ]
        private LineConverterForm _lineConverterForm;

        private void ConvertLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_lineConverterForm == null || _lineConverterForm.IsDisposed)
            {
                _lineConverterForm = new LineConverterForm()
                {
                    TopMost = true
                };
                _lineConverterForm.Show();
            }
            else
            {
                _lineConverterForm.Focus();
            }
        }
        #endregion

        #region [ ultimaOnlineDirToolStripMenuItem ]
        private PathSettingsForm pathSettingsForm = new PathSettingsForm(); // 创建一个 PathSettingsForm 的实例供此类使用。
        private void ultimaOnlineDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 从 pathSettingsForm 访问 tsTbRootPath
            string path = pathSettingsForm.tsTbRootPath.Text;

            // 检查路径是否存在
            if (Directory.Exists(path))
            {
                // 使用该路径启动 Windows 资源管理器
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
            else
            {
                // 如果路径不存在，显示错误消息
                MessageBox.Show("路径不存在。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region [ soundPlayToolStripMenuItem from Ultima Dir ] 
        private void soundPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // 从保存的设置中加载目录
            string savedPath = Properties.Settings.Default.MusicDirectory;

            // 检查是否存在有效目录
            if (string.IsNullOrEmpty(savedPath) || !Directory.Exists(savedPath))
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "选择包含 MP3 文件的目录";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        savedPath = dialog.SelectedPath;
                        Properties.Settings.Default.MusicDirectory = savedPath;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        MessageBox.Show("未选择目录。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }

            // 从目录中加载 MP3 文件
            string[] mp3Files = Directory.GetFiles(savedPath, "*.mp3");

            // 检查 MP3 文件是否存在
            if (mp3Files.Length == 0)
            {
                MessageBox.Show("所选目录中未找到 MP3 文件。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 选择并播放随机 MP3 文件
            PlayRandomMp3(mp3Files);
        }
        #endregion

        #region [ PlayRandomMp3 ]
        private void PlayRandomMp3(string[] mp3Files)
        {
            Random random = new Random();
            string randomMp3 = mp3Files[random.Next(mp3Files.Length)];

            _player.URL = randomMp3;
            _player.controls.play();
        }
        #endregion

        #region [  Player_PlayStateChange ]
        private void Player_PlayStateChange(int newState)
        {
            if ((WMPPlayState)newState == WMPPlayState.wmppsMediaEnded ||
                ((WMPPlayState)newState == WMPPlayState.wmppsStopped && !isStoppedByUser))
            {
                // 仅在声音正常结束时启动计时器
                isStoppedByUser = false; // 为下一首歌重置变量
                timer.Start();
            }
        }
        #endregion

        #region [ Timer_Tick ]
        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop(); // 停止计时器以防止其再次触发

            string savedPath = Properties.Settings.Default.MusicDirectory;
            if (!string.IsNullOrEmpty(savedPath) && Directory.Exists(savedPath))
            {
                string[] mp3Files = Directory.GetFiles(savedPath, "*.mp3");
                if (mp3Files.Length > 0)
                {
                    PlayRandomMp3(mp3Files);
                }
            }
        }
        #endregion

        #region [ stopSoundToolStripMenuItem ]
        private void stopSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isStoppedByUser = true;
            _player.controls.stop();
            timer.Stop(); // 此处也停止计时器            
        }
        #endregion

        #region [ newDirLoadToolStripMenuItem ]
        private void newDirLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "为 MP3 文件选择新目录";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 将新目录保存在设置中
                    Properties.Settings.Default.MusicDirectory = dialog.SelectedPath;
                    Properties.Settings.Default.Save();

                    MessageBox.Show("已指定新目录。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("未选择目录。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion

        #region [ Versionlabel_Click CPU Memory Show]
        private Form cpuMemoryForm;

        private void Versionlabel_Click(object sender, EventArgs e)
        {
            if (cpuMemoryForm == null || cpuMemoryForm.IsDisposed)
            {
                cpuMemoryForm = new Form();
                cpuMemoryForm.Text = "CPU 和内存使用情况";
                cpuMemoryForm.FormBorderStyle = FormBorderStyle.FixedSingle;
                cpuMemoryForm.Size = new Size(300, 150);
                cpuMemoryForm.MaximizeBox = false;
                cpuMemoryForm.ShowIcon = false;

                PerformanceCounter cpuCounter = new PerformanceCounter("处理器", "% 处理器时间", "_Total");
                PerformanceCounter memoryCounter = new PerformanceCounter("内存", "可用 MB 数");

                Label cpuLabel = new Label();
                cpuLabel.Location = new Point(10, 10);
                cpuLabel.Size = new Size(250, 25);

                Label memoryLabel = new Label();
                memoryLabel.Location = new Point(10, 40);
                memoryLabel.Size = new Size(250, 25);

                Timer timer = new Timer();
                timer.Interval = 1000; // 每秒更新一次
                timer.Tick += (s, args) =>
                {
                    cpuLabel.Text = $"CPU 使用率: {cpuCounter.NextValue()}%";
                    memoryLabel.Text = $"可用内存: {memoryCounter.NextValue()} MB";
                };
                timer.Start();

                cpuMemoryForm.Controls.Add(cpuLabel);
                cpuMemoryForm.Controls.Add(memoryLabel);
                cpuMemoryForm.Show();
            }
        }
        #endregion
    }
}