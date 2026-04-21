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

using System.Windows.Forms;

namespace UoFiddler.Controls.UserControls
{
    partial class ItemsControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemsControl));
            splitContainer2 = new SplitContainer();
            chkApplyColorChange = new CheckBox();
            DetailPictureBox = new PictureBox();
            DetailPictureBoxContextMenuStrip = new ContextMenuStrip(components);
            changeBackgroundColorToolStripMenuItemDetail = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            particleGraylToolStripMenuItem = new ToolStripMenuItem();
            particleGrayColorToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator10 = new ToolStripSeparator();
            grayscaleToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            drawRhombusToolStripMenuItem = new ToolStripMenuItem();
            gridPictureToolStripMenuItem = new ToolStripMenuItem();
            SelectColorToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator8 = new ToolStripSeparator();
            copyClipboardToolStripMenuItem = new ToolStripMenuItem();
            DetailTextBox = new RichTextBox();
            splitContainer1 = new SplitContainer();
            ItemsTileView = new UoFiddler.Controls.UserControls.TileView.TileViewControl();
            TileViewContextMenuStrip = new ContextMenuStrip(components);
            showFreeSlotsToolStripMenuItem = new ToolStripMenuItem();
            findNextFreeSlotToolStripMenuItem = new ToolStripMenuItem();
            countItemsToolStripMenuItem = new ToolStripMenuItem();
            ChangeBackgroundColorToolStripMenuItem = new ToolStripMenuItem();
            colorsImageToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            selectInTileDataTabToolStripMenuItem = new ToolStripMenuItem();
            selectInRadarColorTabToolStripMenuItem = new ToolStripMenuItem();
            SelectIDToHexToolStripMenuItem = new ToolStripMenuItem();
            selectInGumpsTabMaleToolStripMenuItem = new ToolStripMenuItem();
            selectInGumpsTabFemaleToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            extractToolStripMenuItem = new ToolStripMenuItem();
            bmpToolStripMenuItem = new ToolStripMenuItem();
            tiffToolStripMenuItem = new ToolStripMenuItem();
            asJpgToolStripMenuItem1 = new ToolStripMenuItem();
            asPngToolStripMenuItem1 = new ToolStripMenuItem();
            SaveImageNameAndHexToTempToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator11 = new ToolStripSeparator();
            replaceToolStripMenuItem = new ToolStripMenuItem();
            replaceStartingFromToolStripMenuItem = new ToolStripMenuItem();
            ReplaceStartingFromText = new ToolStripTextBox();
            toolStripSeparator12 = new ToolStripSeparator();
            removeToolStripMenuItem = new ToolStripMenuItem();
            removeAllToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator13 = new ToolStripSeparator();
            insertAtToolStripMenuItem = new ToolStripMenuItem();
            InsertText = new ToolStripTextBox();
            imageSwapToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            mirrorToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            copyToolStripMenuItem = new ToolStripMenuItem();
            importToolStripclipboardMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            markToolStripMenuItem = new ToolStripMenuItem();
            gotoMarkToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator9 = new ToolStripSeparator();
            saveToolStripMenuItem = new ToolStripMenuItem();
            StatusStrip = new StatusStrip();
            NameLabel = new ToolStripStatusLabel();
            GraphicLabel = new ToolStripStatusLabel();
            toolStripStatusLabelGraficDecimal = new ToolStripStatusLabel();
            toolStripStatusLabelItemHowMuch = new ToolStripStatusLabel();
            PreLoader = new System.ComponentModel.BackgroundWorker();
            ToolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            searchByIdToolStripTextBox = new ToolStripTextBox();
            toolStripLabel2 = new ToolStripLabel();
            searchByNameToolStripTextBox = new ToolStripTextBox();
            searchByNameToolStripButton = new ToolStripButton();
            SearchToolStripButton = new ToolStripButton();
            ReverseSearchToolStripButton = new ToolStripButton();
            ProgressBar = new ToolStripProgressBar();
            PreloadItemsToolStripButton = new ToolStripButton();
            MiscToolStripDropDownButton = new ToolStripDropDownButton();
            ExportAllToolStripMenuItem = new ToolStripMenuItem();
            asBmpToolStripMenuItem = new ToolStripMenuItem();
            asTiffToolStripMenuItem = new ToolStripMenuItem();
            asJpgToolStripMenuItem = new ToolStripMenuItem();
            asPngToolStripMenuItem = new ToolStripMenuItem();
            toolStripButtonColorImage = new ToolStripButton();
            toolStripButtondrawRhombus = new ToolStripButton();
            toolStripButton1 = new ToolStripButton();
            toolStripButtonArtWorkGallery = new ToolStripButton();
            colorDialog = new ColorDialog();
            collapsibleSplitter1 = new CollapsibleSplitter();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DetailPictureBox).BeginInit();
            DetailPictureBoxContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            TileViewContextMenuStrip.SuspendLayout();
            StatusStrip.SuspendLayout();
            ToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Margin = new Padding(4, 3, 4, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(chkApplyColorChange);
            splitContainer2.Panel1.Controls.Add(DetailPictureBox);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(DetailTextBox);
            splitContainer2.Size = new System.Drawing.Size(338, 342);
            splitContainer2.SplitterDistance = 192;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 0;
            // 
            // chkApplyColorChange
            // 
            chkApplyColorChange.AutoSize = true;
            chkApplyColorChange.Location = new System.Drawing.Point(3, 3);
            chkApplyColorChange.Name = "chkApplyColorChange";
            chkApplyColorChange.Size = new System.Drawing.Size(98, 19);
            chkApplyColorChange.TabIndex = 1;
            chkApplyColorChange.Text = "粒子灰度";
            chkApplyColorChange.UseVisualStyleBackColor = true;
            chkApplyColorChange.CheckedChanged += ChkApplyColorChange_CheckedChanged;
            // 
            // DetailPictureBox
            // 
            DetailPictureBox.ContextMenuStrip = DetailPictureBoxContextMenuStrip;
            DetailPictureBox.Dock = DockStyle.Fill;
            DetailPictureBox.Location = new System.Drawing.Point(0, 0);
            DetailPictureBox.Margin = new Padding(4, 3, 4, 3);
            DetailPictureBox.Name = "DetailPictureBox";
            DetailPictureBox.Size = new System.Drawing.Size(338, 192);
            DetailPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            DetailPictureBox.TabIndex = 0;
            DetailPictureBox.TabStop = false;
            DetailPictureBox.Paint += DetailPictureBox_Paint;
            DetailPictureBox.MouseDoubleClick += DetailPictureBox_MouseDoubleClick;
            // 
            // DetailPictureBoxContextMenuStrip
            // 
            DetailPictureBoxContextMenuStrip.Items.AddRange(new ToolStripItem[] { changeBackgroundColorToolStripMenuItemDetail, toolStripSeparator6, particleGraylToolStripMenuItem, particleGrayColorToolStripMenuItem, toolStripSeparator10, grayscaleToolStripMenuItem, toolStripSeparator7, drawRhombusToolStripMenuItem, gridPictureToolStripMenuItem, SelectColorToolStripMenuItem, toolStripSeparator8, copyClipboardToolStripMenuItem });
            DetailPictureBoxContextMenuStrip.Name = "contextMenuStrip2";
            DetailPictureBoxContextMenuStrip.Size = new System.Drawing.Size(213, 204);
            // 
            // changeBackgroundColorToolStripMenuItemDetail
            // 
            changeBackgroundColorToolStripMenuItemDetail.Image = Properties.Resources.colordialog_background;
            changeBackgroundColorToolStripMenuItemDetail.Name = "changeBackgroundColorToolStripMenuItemDetail";
            changeBackgroundColorToolStripMenuItemDetail.Size = new System.Drawing.Size(212, 22);
            changeBackgroundColorToolStripMenuItemDetail.Text = "更改背景颜色";
            changeBackgroundColorToolStripMenuItemDetail.ToolTipText = "背景显示的颜色对话框";
            changeBackgroundColorToolStripMenuItemDetail.Click += ChangeBackgroundColorToolStripMenuItemDetail_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(209, 6);
            // 
            // particleGraylToolStripMenuItem
            // 
            particleGraylToolStripMenuItem.Image = Properties.Resources.particle_gray_hue;
            particleGraylToolStripMenuItem.Name = "particleGraylToolStripMenuItem";
            particleGraylToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            particleGraylToolStripMenuItem.Text = "粒子灰度";
            particleGraylToolStripMenuItem.ToolTipText = "显示可用于游戏内着色的颜色。";
            particleGraylToolStripMenuItem.Click += ParticleGraylToolStripMenuItem_Click;
            // 
            // particleGrayColorToolStripMenuItem
            // 
            particleGrayColorToolStripMenuItem.Image = Properties.Resources.colordialog;
            particleGrayColorToolStripMenuItem.Name = "particleGrayColorToolStripMenuItem";
            particleGrayColorToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            particleGrayColorToolStripMenuItem.Text = "粒子灰度颜色";
            particleGrayColorToolStripMenuItem.ToolTipText = "粒子灰度的颜色对话框";
            particleGrayColorToolStripMenuItem.Click += ParticleGrayColorToolStripMenuItem_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new System.Drawing.Size(209, 6);
            // 
            // grayscaleToolStripMenuItem
            // 
            grayscaleToolStripMenuItem.Image = Properties.Resources.grayscale_image;
            grayscaleToolStripMenuItem.Name = "grayscaleToolStripMenuItem";
            grayscaleToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            grayscaleToolStripMenuItem.Text = "灰度";
            grayscaleToolStripMenuItem.ToolTipText = "灰度图像";
            grayscaleToolStripMenuItem.Click += grayscaleToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(209, 6);
            // 
            // drawRhombusToolStripMenuItem
            // 
            drawRhombusToolStripMenuItem.Image = Properties.Resources.diamand_;
            drawRhombusToolStripMenuItem.Name = "drawRhombusToolStripMenuItem";
            drawRhombusToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            drawRhombusToolStripMenuItem.Text = "绘制菱形";
            drawRhombusToolStripMenuItem.ToolTipText = "在图像上绘制菱形。";
            drawRhombusToolStripMenuItem.Click += DrawRhombusToolStripMenuItem_Click;
            // 
            // gridPictureToolStripMenuItem
            // 
            gridPictureToolStripMenuItem.Image = Properties.Resources.draw_rhombus;
            gridPictureToolStripMenuItem.Name = "gridPictureToolStripMenuItem";
            gridPictureToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            gridPictureToolStripMenuItem.Text = "网格图片";
            gridPictureToolStripMenuItem.ToolTipText = "创建网格图片。";
            gridPictureToolStripMenuItem.Click += GridPictureToolStripMenuItem_Click;
            // 
            // SelectColorToolStripMenuItem
            // 
            SelectColorToolStripMenuItem.Image = Properties.Resources.Color;
            SelectColorToolStripMenuItem.Name = "SelectColorToolStripMenuItem";
            SelectColorToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            SelectColorToolStripMenuItem.Text = "网格颜色";
            SelectColorToolStripMenuItem.ToolTipText = "更改网格的颜色。";
            SelectColorToolStripMenuItem.Click += SelectColorToolStripMenuItem_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(209, 6);
            // 
            // copyClipboardToolStripMenuItem
            // 
            copyClipboardToolStripMenuItem.Image = Properties.Resources.Copy;
            copyClipboardToolStripMenuItem.Name = "copyClipboardToolStripMenuItem";
            copyClipboardToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            copyClipboardToolStripMenuItem.Text = "复制到剪贴板";
            copyClipboardToolStripMenuItem.ToolTipText = "将图像复制到剪贴板。";
            copyClipboardToolStripMenuItem.Click += CopyClipboardToolStripMenuItem_Click;
            // 
            // DetailTextBox
            // 
            DetailTextBox.Dock = DockStyle.Fill;
            DetailTextBox.Location = new System.Drawing.Point(0, 0);
            DetailTextBox.Margin = new Padding(4, 3, 4, 3);
            DetailTextBox.Name = "DetailTextBox";
            DetailTextBox.Size = new System.Drawing.Size(338, 145);
            DetailTextBox.TabIndex = 0;
            DetailTextBox.Text = "";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 36);
            splitContainer1.Margin = new Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(ItemsTileView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new System.Drawing.Size(1303, 342);
            splitContainer1.SplitterDistance = 960;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 6;
            // 
            // ItemsTileView
            // 
            ItemsTileView.AutoScroll = true;
            ItemsTileView.AutoScrollMinSize = new System.Drawing.Size(0, 102);
            ItemsTileView.BackColor = System.Drawing.SystemColors.Window;
            ItemsTileView.ContextMenuStrip = TileViewContextMenuStrip;
            ItemsTileView.Dock = DockStyle.Fill;
            ItemsTileView.FocusIndex = -1;
            ItemsTileView.Location = new System.Drawing.Point(0, 0);
            ItemsTileView.Margin = new Padding(4, 3, 4, 3);
            ItemsTileView.MultiSelect = true;
            ItemsTileView.Name = "ItemsTileView";
            ItemsTileView.Size = new System.Drawing.Size(960, 342);
            ItemsTileView.TabIndex = 0;
            ItemsTileView.TileBackgroundColor = System.Drawing.SystemColors.Window;
            ItemsTileView.TileBorderColor = System.Drawing.Color.Gray;
            ItemsTileView.TileBorderWidth = 1F;
            ItemsTileView.TileFocusColor = System.Drawing.Color.DarkRed;
            ItemsTileView.TileHighlightColor = System.Drawing.SystemColors.Highlight;
            ItemsTileView.TileMargin = new Padding(2, 2, 0, 0);
            ItemsTileView.TilePadding = new Padding(1);
            ItemsTileView.TileSize = new System.Drawing.Size(96, 96);
            ItemsTileView.VirtualListSize = 1;
            ItemsTileView.ItemSelectionChanged += ItemsTileView_ItemSelectionChanged;
            ItemsTileView.FocusSelectionChanged += ItemsTileView_FocusSelectionChanged;
            ItemsTileView.DrawItem += ItemsTileView_DrawItem;
            ItemsTileView.KeyDown += ItemsTileView_KeyDown;
            ItemsTileView.KeyUp += ItemsTileView_KeyUp;
            ItemsTileView.MouseDoubleClick += ItemsTileView_MouseDoubleClick;
            // 
            // TileViewContextMenuStrip
            // 
            TileViewContextMenuStrip.Items.AddRange(new ToolStripItem[] { showFreeSlotsToolStripMenuItem, findNextFreeSlotToolStripMenuItem, countItemsToolStripMenuItem, ChangeBackgroundColorToolStripMenuItem, colorsImageToolStripMenuItem, toolStripSeparator3, selectInTileDataTabToolStripMenuItem, selectInRadarColorTabToolStripMenuItem, SelectIDToHexToolStripMenuItem, selectInGumpsTabMaleToolStripMenuItem, selectInGumpsTabFemaleToolStripMenuItem, toolStripSeparator2, extractToolStripMenuItem, SaveImageNameAndHexToTempToolStripMenuItem, toolStripSeparator11, replaceToolStripMenuItem, replaceStartingFromToolStripMenuItem, toolStripSeparator12, removeToolStripMenuItem, removeAllToolStripMenuItem, toolStripSeparator13, insertAtToolStripMenuItem, imageSwapToolStripMenuItem, toolStripSeparator5, mirrorToolStripMenuItem, toolStripSeparator1, copyToolStripMenuItem, importToolStripclipboardMenuItem, toolStripSeparator4, markToolStripMenuItem, gotoMarkToolStripMenuItem, toolStripSeparator9, saveToolStripMenuItem });
            TileViewContextMenuStrip.Name = "contextMenuStrip1";
            TileViewContextMenuStrip.Size = new System.Drawing.Size(230, 586);
            TileViewContextMenuStrip.Closing += TileViewContextMenuStrip_Closing;
            TileViewContextMenuStrip.Opening += TileViewContextMenuStrip_Opening;
            // 
            // showFreeSlotsToolStripMenuItem
            // 
            showFreeSlotsToolStripMenuItem.CheckOnClick = true;
            showFreeSlotsToolStripMenuItem.Image = Properties.Resources.show;
            showFreeSlotsToolStripMenuItem.Name = "showFreeSlotsToolStripMenuItem";
            showFreeSlotsToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            showFreeSlotsToolStripMenuItem.Text = "显示空闲槽位";
            showFreeSlotsToolStripMenuItem.ToolTipText = "显示所有可用的 ID 槽位。";
            showFreeSlotsToolStripMenuItem.Click += OnClickShowFreeSlots;
            // 
            // findNextFreeSlotToolStripMenuItem
            // 
            findNextFreeSlotToolStripMenuItem.Image = Properties.Resources.Search;
            findNextFreeSlotToolStripMenuItem.Name = "findNextFreeSlotToolStripMenuItem";
            findNextFreeSlotToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            findNextFreeSlotToolStripMenuItem.Text = "查找下一个空闲槽位";
            findNextFreeSlotToolStripMenuItem.ToolTipText = "查找下一个 ID 槽位。";
            findNextFreeSlotToolStripMenuItem.Click += OnClickFindFree;
            // 
            // countItemsToolStripMenuItem
            // 
            countItemsToolStripMenuItem.Image = Properties.Resources.reset__2_;
            countItemsToolStripMenuItem.Name = "countItemsToolStripMenuItem";
            countItemsToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            countItemsToolStripMenuItem.Text = "物品计数器";
            countItemsToolStripMenuItem.Click += countItemsToolStripMenuItem_Click;
            // 
            // ChangeBackgroundColorToolStripMenuItem
            // 
            ChangeBackgroundColorToolStripMenuItem.Image = Properties.Resources.Color;
            ChangeBackgroundColorToolStripMenuItem.Name = "ChangeBackgroundColorToolStripMenuItem";
            ChangeBackgroundColorToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            ChangeBackgroundColorToolStripMenuItem.Text = "更改背景颜色";
            ChangeBackgroundColorToolStripMenuItem.ToolTipText = "更改背景颜色。";
            ChangeBackgroundColorToolStripMenuItem.Click += ChangeBackgroundColorToolStripMenuItem_Click;
            // 
            // colorsImageToolStripMenuItem
            // 
            colorsImageToolStripMenuItem.Image = Properties.Resources.colordialog_background;
            colorsImageToolStripMenuItem.Name = "colorsImageToolStripMenuItem";
            colorsImageToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            colorsImageToolStripMenuItem.Text = "图像颜色";
            colorsImageToolStripMenuItem.ToolTipText = "显示图像中的颜色";
            colorsImageToolStripMenuItem.Click += ColorsImageToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(226, 6);
            // 
            // selectInTileDataTabToolStripMenuItem
            // 
            selectInTileDataTabToolStripMenuItem.Image = Properties.Resources.Select;
            selectInTileDataTabToolStripMenuItem.Name = "selectInTileDataTabToolStripMenuItem";
            selectInTileDataTabToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            selectInTileDataTabToolStripMenuItem.Text = "在 TileData 选项卡中选择";
            selectInTileDataTabToolStripMenuItem.ToolTipText = "在 Tiledata 选项卡中高亮显示该 ID。";
            selectInTileDataTabToolStripMenuItem.Click += OnClickSelectTiledata;
            // 
            // selectInRadarColorTabToolStripMenuItem
            // 
            selectInRadarColorTabToolStripMenuItem.Image = Properties.Resources.Select;
            selectInRadarColorTabToolStripMenuItem.Name = "selectInRadarColorTabToolStripMenuItem";
            selectInRadarColorTabToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            selectInRadarColorTabToolStripMenuItem.Text = "在 RadarColor 选项卡中选择";
            selectInRadarColorTabToolStripMenuItem.ToolTipText = "在 RadarColor 选项卡中高亮显示该 ID。";
            selectInRadarColorTabToolStripMenuItem.Click += OnClickSelectRadarCol;
            // 
            // SelectIDToHexToolStripMenuItem
            // 
            SelectIDToHexToolStripMenuItem.Image = Properties.Resources.hexdecimal_adresse_to_clipbord;
            SelectIDToHexToolStripMenuItem.Name = "SelectIDToHexToolStripMenuItem";
            SelectIDToHexToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            SelectIDToHexToolStripMenuItem.Text = "复制十六进制 ID 到剪贴板";
            SelectIDToHexToolStripMenuItem.ToolTipText = "将十六进制地址保存到剪贴板。";
            SelectIDToHexToolStripMenuItem.Click += SelectIDToHexToolStripMenuItem_Click;
            // 
            // selectInGumpsTabMaleToolStripMenuItem
            // 
            selectInGumpsTabMaleToolStripMenuItem.Enabled = false;
            selectInGumpsTabMaleToolStripMenuItem.Image = Properties.Resources.gumps_men_fantasy;
            selectInGumpsTabMaleToolStripMenuItem.Name = "selectInGumpsTabMaleToolStripMenuItem";
            selectInGumpsTabMaleToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            selectInGumpsTabMaleToolStripMenuItem.Text = "在 Gumps (男) 选项卡中选择";
            selectInGumpsTabMaleToolStripMenuItem.ToolTipText = "在 Gumps 选项卡中高亮显示该 ID。";
            selectInGumpsTabMaleToolStripMenuItem.Click += SelectInGumpsTabMaleToolStripMenuItem_Click;
            // 
            // selectInGumpsTabFemaleToolStripMenuItem
            // 
            selectInGumpsTabFemaleToolStripMenuItem.Enabled = false;
            selectInGumpsTabFemaleToolStripMenuItem.Image = Properties.Resources.gumps_woman_fantasy;
            selectInGumpsTabFemaleToolStripMenuItem.Name = "selectInGumpsTabFemaleToolStripMenuItem";
            selectInGumpsTabFemaleToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            selectInGumpsTabFemaleToolStripMenuItem.Text = "在 Gumps (女) 选项卡中选择";
            selectInGumpsTabFemaleToolStripMenuItem.ToolTipText = "在 Gumps 选项卡中高亮显示该 ID。";
            selectInGumpsTabFemaleToolStripMenuItem.Click += SelectInGumpsTabFemaleToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(226, 6);
            // 
            // extractToolStripMenuItem
            // 
            extractToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { bmpToolStripMenuItem, tiffToolStripMenuItem, asJpgToolStripMenuItem1, asPngToolStripMenuItem1 });
            extractToolStripMenuItem.Image = Properties.Resources.Export;
            extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            extractToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            extractToolStripMenuItem.Text = "导出图像...";
            extractToolStripMenuItem.ToolTipText = "导出图像为...";
            // 
            // bmpToolStripMenuItem
            // 
            bmpToolStripMenuItem.Name = "bmpToolStripMenuItem";
            bmpToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            bmpToolStripMenuItem.Text = "Bmp 格式";
            bmpToolStripMenuItem.Click += Extract_Image_ClickBmp;
            // 
            // tiffToolStripMenuItem
            // 
            tiffToolStripMenuItem.Name = "tiffToolStripMenuItem";
            tiffToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            tiffToolStripMenuItem.Text = "Tiff 格式";
            tiffToolStripMenuItem.Click += Extract_Image_ClickTiff;
            // 
            // asJpgToolStripMenuItem1
            // 
            asJpgToolStripMenuItem1.Name = "asJpgToolStripMenuItem1";
            asJpgToolStripMenuItem1.Size = new System.Drawing.Size(115, 22);
            asJpgToolStripMenuItem1.Text = "Jpg 格式";
            asJpgToolStripMenuItem1.Click += Extract_Image_ClickJpg;
            // 
            // asPngToolStripMenuItem1
            // 
            asPngToolStripMenuItem1.Name = "asPngToolStripMenuItem1";
            asPngToolStripMenuItem1.Size = new System.Drawing.Size(115, 22);
            asPngToolStripMenuItem1.Text = "Png 格式";
            asPngToolStripMenuItem1.Click += Extract_Image_ClickPng;
            // 
            // SaveImageNameAndHexToTempToolStripMenuItem
            // 
            SaveImageNameAndHexToTempToolStripMenuItem.Image = Properties.Resources.Image;
            SaveImageNameAndHexToTempToolStripMenuItem.Name = "SaveImageNameAndHexToTempToolStripMenuItem";
            SaveImageNameAndHexToTempToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            SaveImageNameAndHexToTempToolStripMenuItem.Text = "保存图像 十六进制_名称 到临时目录";
            SaveImageNameAndHexToTempToolStripMenuItem.ToolTipText = "将图形及其十六进制地址和名称保存到临时目录";
            SaveImageNameAndHexToTempToolStripMenuItem.Click += SaveImageNameAndHexToTempToolStripMenuItem_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new System.Drawing.Size(226, 6);
            // 
            // replaceToolStripMenuItem
            // 
            replaceToolStripMenuItem.Image = Properties.Resources.replace;
            replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            replaceToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            replaceToolStripMenuItem.Text = "替换...";
            replaceToolStripMenuItem.ToolTipText = "替换图像。";
            replaceToolStripMenuItem.Click += OnClickReplace;
            // 
            // replaceStartingFromToolStripMenuItem
            // 
            replaceStartingFromToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ReplaceStartingFromText });
            replaceStartingFromToolStripMenuItem.Image = Properties.Resources.replace2;
            replaceStartingFromToolStripMenuItem.Name = "replaceStartingFromToolStripMenuItem";
            replaceStartingFromToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            replaceStartingFromToolStripMenuItem.Text = "从...开始替换";
            replaceStartingFromToolStripMenuItem.ToolTipText = "从指定 ID 位置开始替换图像。";
            // 
            // ReplaceStartingFromText
            // 
            ReplaceStartingFromText.Name = "ReplaceStartingFromText";
            ReplaceStartingFromText.Size = new System.Drawing.Size(100, 23);
            ReplaceStartingFromText.KeyDown += ReplaceStartingFromText_KeyDown;
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new System.Drawing.Size(226, 6);
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Image = Properties.Resources.Remove;
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            removeToolStripMenuItem.Text = "移除";
            removeToolStripMenuItem.ToolTipText = "移除图像。单个或多个（Ctrl 键）。";
            removeToolStripMenuItem.Click += OnClickRemove;
            // 
            // removeAllToolStripMenuItem
            // 
            removeAllToolStripMenuItem.Image = Properties.Resources.Remove;
            removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            removeAllToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            removeAllToolStripMenuItem.Text = "全部移除";
            removeAllToolStripMenuItem.Click += RemoveAllToolStripMenuItem_Click;
            // 
            // toolStripSeparator13
            // 
            toolStripSeparator13.Name = "toolStripSeparator13";
            toolStripSeparator13.Size = new System.Drawing.Size(226, 6);
            // 
            // insertAtToolStripMenuItem
            // 
            insertAtToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { InsertText });
            insertAtToolStripMenuItem.Image = Properties.Resources.import;
            insertAtToolStripMenuItem.Name = "insertAtToolStripMenuItem";
            insertAtToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            insertAtToolStripMenuItem.Text = "在...处插入";
            insertAtToolStripMenuItem.ToolTipText = "在指定 ID 位置插入。";
            // 
            // InsertText
            // 
            InsertText.Name = "InsertText";
            InsertText.Size = new System.Drawing.Size(100, 23);
            InsertText.KeyDown += OnKeyDownInsertText;
            InsertText.TextChanged += OnTextChangedInsert;
            // 
            // imageSwapToolStripMenuItem
            // 
            imageSwapToolStripMenuItem.Image = Properties.Resources.two_image_swap;
            imageSwapToolStripMenuItem.Name = "imageSwapToolStripMenuItem";
            imageSwapToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            imageSwapToolStripMenuItem.Text = "图像交换";
            imageSwapToolStripMenuItem.ToolTipText = "相互交换图形";
            imageSwapToolStripMenuItem.Click += ImageSwapToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(226, 6);
            // 
            // mirrorToolStripMenuItem
            // 
            mirrorToolStripMenuItem.Image = Properties.Resources.Mirror;
            mirrorToolStripMenuItem.Name = "mirrorToolStripMenuItem";
            mirrorToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            mirrorToolStripMenuItem.Text = "镜像";
            mirrorToolStripMenuItem.ToolTipText = "镜像图像。";
            mirrorToolStripMenuItem.Click += MirrorToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(226, 6);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Image = Properties.Resources.Copy;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            copyToolStripMenuItem.Text = "复制";
            copyToolStripMenuItem.ToolTipText = "将图形复制到剪贴板。";
            copyToolStripMenuItem.Click += CopyToolStripMenuItem_Click;
            // 
            // importToolStripclipboardMenuItem
            // 
            importToolStripclipboardMenuItem.Image = Properties.Resources.import;
            importToolStripclipboardMenuItem.Name = "importToolStripclipboardMenuItem";
            importToolStripclipboardMenuItem.Size = new System.Drawing.Size(229, 22);
            importToolStripclipboardMenuItem.Text = "导入";
            importToolStripclipboardMenuItem.ToolTipText = "从剪贴板导入图像";
            importToolStripclipboardMenuItem.Click += ImportToolStripclipboardMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(226, 6);
            // 
            // markToolStripMenuItem
            // 
            markToolStripMenuItem.Image = Properties.Resources.Mark;
            markToolStripMenuItem.Name = "markToolStripMenuItem";
            markToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            markToolStripMenuItem.Text = "标记位置";
            markToolStripMenuItem.ToolTipText = "标记位置";
            markToolStripMenuItem.Click += MarkToolStripMenuItem_Click;
            // 
            // gotoMarkToolStripMenuItem
            // 
            gotoMarkToolStripMenuItem.Image = Properties.Resources.zoom_image_into_tile;
            gotoMarkToolStripMenuItem.Name = "gotoMarkToolStripMenuItem";
            gotoMarkToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            gotoMarkToolStripMenuItem.Text = "跳转到位置";
            gotoMarkToolStripMenuItem.ToolTipText = "Ctrl+J 跳转到上次标记的位置";
            gotoMarkToolStripMenuItem.Click += GoToMarkedPositionToolStripMenuItem_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new System.Drawing.Size(226, 6);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = Properties.Resources.Save2;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            saveToolStripMenuItem.Text = "保存";
            saveToolStripMenuItem.ToolTipText = "保存 .mul 文件。";
            saveToolStripMenuItem.Click += OnClickSave;
            // 
            // StatusStrip
            // 
            StatusStrip.Items.AddRange(new ToolStripItem[] { NameLabel, GraphicLabel, toolStripStatusLabelGraficDecimal, toolStripStatusLabelItemHowMuch });
            StatusStrip.Location = new System.Drawing.Point(0, 378);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Padding = new Padding(1, 0, 16, 0);
            StatusStrip.Size = new System.Drawing.Size(1303, 22);
            StatusStrip.TabIndex = 5;
            StatusStrip.Text = "statusStrip1";
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = false;
            NameLabel.BorderStyle = Border3DStyle.SunkenInner;
            NameLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new System.Drawing.Size(200, 17);
            NameLabel.Text = "名称:";
            NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GraphicLabel
            // 
            GraphicLabel.AutoSize = false;
            GraphicLabel.Name = "GraphicLabel";
            GraphicLabel.Size = new System.Drawing.Size(150, 17);
            GraphicLabel.Text = "图形:";
            GraphicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelGraficDecimal
            // 
            toolStripStatusLabelGraficDecimal.Name = "toolStripStatusLabelGraficDecimal";
            toolStripStatusLabelGraficDecimal.Size = new System.Drawing.Size(51, 17);
            toolStripStatusLabelGraficDecimal.Text = "图形:";
            // 
            // toolStripStatusLabelItemHowMuch
            // 
            toolStripStatusLabelItemHowMuch.Name = "toolStripStatusLabelItemHowMuch";
            toolStripStatusLabelItemHowMuch.Size = new System.Drawing.Size(34, 17);
            toolStripStatusLabelItemHowMuch.Text = "物品:";
            // 
            // PreLoader
            // 
            PreLoader.WorkerReportsProgress = true;
            PreLoader.DoWork += PreLoaderDoWork;
            PreLoader.ProgressChanged += PreLoaderProgressChanged;
            PreLoader.RunWorkerCompleted += PreLoaderCompleted;
            // 
            // ToolStrip
            // 
            ToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            ToolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, searchByIdToolStripTextBox, toolStripLabel2, searchByNameToolStripTextBox, searchByNameToolStripButton, SearchToolStripButton, ReverseSearchToolStripButton, ProgressBar, PreloadItemsToolStripButton, MiscToolStripDropDownButton, toolStripButtonColorImage, toolStripButtondrawRhombus, toolStripButton1, toolStripButtonArtWorkGallery });
            ToolStrip.Location = new System.Drawing.Point(0, 0);
            ToolStrip.Name = "ToolStrip";
            ToolStrip.RenderMode = ToolStripRenderMode.System;
            ToolStrip.Size = new System.Drawing.Size(1303, 28);
            ToolStrip.TabIndex = 7;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(39, 25);
            toolStripLabel1.Text = "索引:";
            // 
            // searchByIdToolStripTextBox
            // 
            searchByIdToolStripTextBox.Name = "searchByIdToolStripTextBox";
            searchByIdToolStripTextBox.Size = new System.Drawing.Size(100, 28);
            searchByIdToolStripTextBox.ToolTipText = "按 ID 搜索";
            searchByIdToolStripTextBox.KeyUp += SearchByIdToolStripTextBox_KeyUp;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new System.Drawing.Size(42, 25);
            toolStripLabel2.Text = "名称:";
            // 
            // searchByNameToolStripTextBox
            // 
            searchByNameToolStripTextBox.Name = "searchByNameToolStripTextBox";
            searchByNameToolStripTextBox.Size = new System.Drawing.Size(100, 28);
            searchByNameToolStripTextBox.ToolTipText = "按名称搜索";
            searchByNameToolStripTextBox.KeyUp += SearchByNameToolStripTextBox_KeyUp;
            // 
            // searchByNameToolStripButton
            // 
            searchByNameToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            searchByNameToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            searchByNameToolStripButton.Name = "searchByNameToolStripButton";
            searchByNameToolStripButton.Size = new System.Drawing.Size(60, 25);
            searchByNameToolStripButton.Text = "查找下一个";
            searchByNameToolStripButton.Click += SearchByNameToolStripButton_Click;
            // 
            // SearchToolStripButton
            // 
            SearchToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            SearchToolStripButton.Image = Properties.Resources.Search;
            SearchToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            SearchToolStripButton.Name = "SearchToolStripButton";
            SearchToolStripButton.Size = new System.Drawing.Size(23, 25);
            SearchToolStripButton.Text = "搜索";
            SearchToolStripButton.ToolTipText = "旧版搜索";
            SearchToolStripButton.Click += OnSearchClick;
            // 
            // ReverseSearchToolStripButton
            // 
            ReverseSearchToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ReverseSearchToolStripButton.Image = (System.Drawing.Image)resources.GetObject("ReverseSearchToolStripButton.Image");
            ReverseSearchToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            ReverseSearchToolStripButton.Name = "ReverseSearchToolStripButton";
            ReverseSearchToolStripButton.Size = new System.Drawing.Size(94, 25);
            ReverseSearchToolStripButton.Text = "上一个搜索";
            ReverseSearchToolStripButton.ToolTipText = "上一个搜索";
            ReverseSearchToolStripButton.Click += ReverseSearchToolStripButton_Click;
            // 
            // ProgressBar
            // 
            ProgressBar.Alignment = ToolStripItemAlignment.Right;
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new System.Drawing.Size(117, 25);
            // 
            // PreloadItemsToolStripButton
            // 
            PreloadItemsToolStripButton.Alignment = ToolStripItemAlignment.Right;
            PreloadItemsToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            PreloadItemsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            PreloadItemsToolStripButton.Name = "PreloadItemsToolStripButton";
            PreloadItemsToolStripButton.Size = new System.Drawing.Size(83, 25);
            PreloadItemsToolStripButton.Text = "预加载物品";
            PreloadItemsToolStripButton.Click += OnClickPreLoad;
            // 
            // MiscToolStripDropDownButton
            // 
            MiscToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            MiscToolStripDropDownButton.DropDownItems.AddRange(new ToolStripItem[] { ExportAllToolStripMenuItem });
            MiscToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            MiscToolStripDropDownButton.Name = "MiscToolStripDropDownButton";
            MiscToolStripDropDownButton.Size = new System.Drawing.Size(45, 25);
            MiscToolStripDropDownButton.Text = "杂项";
            // 
            // ExportAllToolStripMenuItem
            // 
            ExportAllToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { asBmpToolStripMenuItem, asTiffToolStripMenuItem, asJpgToolStripMenuItem, asPngToolStripMenuItem });
            ExportAllToolStripMenuItem.Name = "ExportAllToolStripMenuItem";
            ExportAllToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            ExportAllToolStripMenuItem.Text = "全部导出...";
            // 
            // asBmpToolStripMenuItem
            // 
            asBmpToolStripMenuItem.Name = "asBmpToolStripMenuItem";
            asBmpToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            asBmpToolStripMenuItem.Text = "Bmp 格式";
            asBmpToolStripMenuItem.Click += OnClick_SaveAllBmp;
            // 
            // asTiffToolStripMenuItem
            // 
            asTiffToolStripMenuItem.Name = "asTiffToolStripMenuItem";
            asTiffToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            asTiffToolStripMenuItem.Text = "Tiff 格式";
            asTiffToolStripMenuItem.Click += OnClick_SaveAllTiff;
            // 
            // asJpgToolStripMenuItem
            // 
            asJpgToolStripMenuItem.Name = "asJpgToolStripMenuItem";
            asJpgToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            asJpgToolStripMenuItem.Text = "Jpg 格式";
            asJpgToolStripMenuItem.Click += OnClick_SaveAllJpg;
            // 
            // asPngToolStripMenuItem
            // 
            asPngToolStripMenuItem.Name = "asPngToolStripMenuItem";
            asPngToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            asPngToolStripMenuItem.Text = "Png 格式";
            asPngToolStripMenuItem.Click += OnClick_SaveAllPng;
            // 
            // toolStripButtonColorImage
            // 
            toolStripButtonColorImage.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonColorImage.Image = Properties.Resources.colordialog;
            toolStripButtonColorImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonColorImage.Name = "toolStripButtonColorImage";
            toolStripButtonColorImage.Size = new System.Drawing.Size(23, 25);
            toolStripButtonColorImage.Text = "toolStripButton2";
            toolStripButtonColorImage.ToolTipText = "从物品图像读取颜色，在 PictureBox 和 RichTextBox 中显示。";
            toolStripButtonColorImage.Click += toolStripButtonColorImage_Click;
            // 
            // toolStripButtondrawRhombus
            // 
            toolStripButtondrawRhombus.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtondrawRhombus.Image = Properties.Resources.diamand_;
            toolStripButtondrawRhombus.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtondrawRhombus.Name = "toolStripButtondrawRhombus";
            toolStripButtondrawRhombus.Size = new System.Drawing.Size(23, 25);
            toolStripButtondrawRhombus.Text = "在图像上绘制菱形。";
            toolStripButtondrawRhombus.Click += toolStripButtondrawRhombus_Click;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = Properties.Resources.Save2;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(23, 25);
            toolStripButton1.Text = "toolStripButtonSave";
            toolStripButton1.ToolTipText = "保存 Items.mul 文件";
            toolStripButton1.Click += OnClickSave;
            // 
            // toolStripButtonArtWorkGallery
            // 
            toolStripButtonArtWorkGallery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonArtWorkGallery.Image = Properties.Resources.artworkgallery;
            toolStripButtonArtWorkGallery.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonArtWorkGallery.Name = "toolStripButtonArtWorkGallery";
            toolStripButtonArtWorkGallery.Size = new System.Drawing.Size(23, 25);
            toolStripButtonArtWorkGallery.Text = "美术画廊";
            toolStripButtonArtWorkGallery.Click += toolStripButtonArtWorkGallery_Click;
            // 
            // collapsibleSplitter1
            // 
            collapsibleSplitter1.AnimationDelay = 20;
            collapsibleSplitter1.AnimationStep = 20;
            collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
            collapsibleSplitter1.ControlToHide = ToolStrip;
            collapsibleSplitter1.Dock = DockStyle.Top;
            collapsibleSplitter1.ExpandParentForm = false;
            collapsibleSplitter1.Location = new System.Drawing.Point(0, 28);
            collapsibleSplitter1.Margin = new Padding(4, 3, 4, 3);
            collapsibleSplitter1.Name = "collapsibleSplitter1";
            collapsibleSplitter1.Size = new System.Drawing.Size(1303, 8);
            collapsibleSplitter1.TabIndex = 8;
            collapsibleSplitter1.TabStop = false;
            collapsibleSplitter1.UseAnimations = false;
            collapsibleSplitter1.VisualStyle = VisualStyles.DoubleDots;
            // 
            // ItemsControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Controls.Add(StatusStrip);
            Controls.Add(collapsibleSplitter1);
            Controls.Add(ToolStrip);
            DoubleBuffered = true;
            Margin = new Padding(4, 3, 4, 3);
            Name = "ItemsControl";
            Size = new System.Drawing.Size(1303, 400);
            Load += OnLoad;
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DetailPictureBox).EndInit();
            DetailPictureBoxContextMenuStrip.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            TileViewContextMenuStrip.ResumeLayout(false);
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ToolStrip.ResumeLayout(false);
            ToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStripMenuItem asBmpToolStripMenuItem;
        private ToolStripMenuItem asJpgToolStripMenuItem;
        private ToolStripMenuItem asJpgToolStripMenuItem1;
        private ToolStripMenuItem asPngToolStripMenuItem;
        private ToolStripMenuItem asPngToolStripMenuItem1;
        private ToolStripMenuItem asTiffToolStripMenuItem;
        private ToolStripMenuItem bmpToolStripMenuItem;
        private UoFiddler.Controls.UserControls.CollapsibleSplitter collapsibleSplitter1;
        private ContextMenuStrip TileViewContextMenuStrip;
        private PictureBox DetailPictureBox;
        private ToolStripMenuItem ExportAllToolStripMenuItem;
        private ToolStripMenuItem extractToolStripMenuItem;
        private ToolStripMenuItem findNextFreeSlotToolStripMenuItem;
        private ToolStripStatusLabel GraphicLabel;
        private ToolStripMenuItem insertAtToolStripMenuItem;
        private ToolStripTextBox InsertText;
        private ToolStripStatusLabel NameLabel;
        private System.ComponentModel.BackgroundWorker PreLoader;
        private ToolStripProgressBar ProgressBar;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem selectInRadarColorTabToolStripMenuItem;
        private ToolStripMenuItem selectInTileDataTabToolStripMenuItem;
        private ToolStripMenuItem showFreeSlotsToolStripMenuItem;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private StatusStrip StatusStrip;
        private ToolStripMenuItem tiffToolStripMenuItem;
        private ToolStrip ToolStrip;
        private ToolStripButton SearchToolStripButton;
        private ToolStripButton PreloadItemsToolStripButton;
        private ToolStripDropDownButton MiscToolStripDropDownButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ContextMenuStrip DetailPictureBoxContextMenuStrip;
        private ToolStripMenuItem changeBackgroundColorToolStripMenuItemDetail;
        private ColorDialog colorDialog;
        private ToolStripMenuItem ChangeBackgroundColorToolStripMenuItem;
        private TileView.TileViewControl ItemsTileView;
        private RichTextBox DetailTextBox;
        private ToolStripMenuItem selectInGumpsTabMaleToolStripMenuItem;
        private ToolStripMenuItem selectInGumpsTabFemaleToolStripMenuItem;
        private ToolStripMenuItem replaceStartingFromToolStripMenuItem;
        private ToolStripTextBox ReplaceStartingFromText;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem importToolStripclipboardMenuItem;
        private ToolStripMenuItem mirrorToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox searchByIdToolStripTextBox;
        private ToolStripLabel toolStripLabel2;
        private ToolStripTextBox searchByNameToolStripTextBox;
        private ToolStripButton searchByNameToolStripButton;
        private ToolStripButton toolStripButton1;
        private ToolStripMenuItem SelectIDToHexToolStripMenuItem;
        private ToolStripMenuItem imageSwapToolStripMenuItem;
        private ToolStripButton ReverseSearchToolStripButton;
        private ToolStripMenuItem particleGraylToolStripMenuItem;
        private CheckBox chkApplyColorChange;
        private ToolStripMenuItem particleGrayColorToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem drawRhombusToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem gridPictureToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem copyClipboardToolStripMenuItem;
        private ToolStripMenuItem SelectColorToolStripMenuItem;
        private ToolStripMenuItem colorsImageToolStripMenuItem;
        private ToolStripMenuItem markToolStripMenuItem;
        private ToolStripMenuItem gotoMarkToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem grayscaleToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem SaveImageNameAndHexToTempToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripStatusLabel toolStripStatusLabelItemHowMuch;
        private ToolStripMenuItem countItemsToolStripMenuItem;
        private ToolStripButton toolStripButtonColorImage;
        private ToolStripStatusLabel toolStripStatusLabelGraficDecimal;
        private ToolStripButton toolStripButtondrawRhombus;
        private ToolStripMenuItem removeAllToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripButton toolStripButtonArtWorkGallery;
    }
}