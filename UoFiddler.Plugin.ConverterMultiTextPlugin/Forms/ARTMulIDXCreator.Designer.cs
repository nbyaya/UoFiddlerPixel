namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    partial class ARTMulIDXCreator
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();

            // 现有选项卡
            this.tabPageCreateMuls = new System.Windows.Forms.TabPage();
            this.tabPageReadMuls = new System.Windows.Forms.TabPage();
            this.tabPageTileData = new System.Windows.Forms.TabPage();
            this.tabPageReadOut = new System.Windows.Forms.TabPage();
            this.tabPageTexturen = new System.Windows.Forms.TabPage();
            this.tabPageRadarColor = new System.Windows.Forms.TabPage();
            this.tabPagePalette = new System.Windows.Forms.TabPage();
            this.tabPageAnimation = new System.Windows.Forms.TabPage();
            this.tabPageArtmul = new System.Windows.Forms.TabPage();
            this.tabPageSound = new System.Windows.Forms.TabPage();
            this.tabPageGump = new System.Windows.Forms.TabPage();

            // 新增选项卡
            this.tabPageHues = new System.Windows.Forms.TabPage();
            this.tabPageMap = new System.Windows.Forms.TabPage();
            this.tabPageMulti = new System.Windows.Forms.TabPage();
            this.tabPageSkills = new System.Windows.Forms.TabPage();
            this.tabPageValidator = new System.Windows.Forms.TabPage();
            this.tabPageIdxPatcher = new System.Windows.Forms.TabPage();
            this.tabPageBatch = new System.Windows.Forms.TabPage();
            this.tabPageHexViewer = new System.Windows.Forms.TabPage();

            // ── 选项卡1: 创建 Muls 控件 ──────────────────────────────────
            this.grpCreateMulsDir = new System.Windows.Forms.GroupBox();
            this.BtFileOrder = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblDirInfo = new System.Windows.Forms.Label();
            this.grpCreateMulsCount = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblCountHint = new System.Windows.Forms.Label();
            this.grpCreateMulsButtons = new System.Windows.Forms.GroupBox();
            this.BtCreateARTIDXMul = new System.Windows.Forms.Button();
            this.BtCreateARTIDXMul_Ulong = new System.Windows.Forms.Button();
            this.BtCreateARTIDXMul_uint = new System.Windows.Forms.Button();
            this.BtCreateARTIDXMul_Int = new System.Windows.Forms.Button();
            this.BtCreateARTIDXMul_Ushort = new System.Windows.Forms.Button();
            this.BtCreateARTIDXMul_Short = new System.Windows.Forms.Button();
            this.BtCreateARTIDXMul_Byte = new System.Windows.Forms.Button();
            this.BtCreateARTIDXMul_Sbyte = new System.Windows.Forms.Button();
            this.lblButtonsNote = new System.Windows.Forms.Label();
            this.grpRename = new System.Windows.Forms.GroupBox();
            this.ComboBoxMuls = new System.Windows.Forms.ComboBox();
            this.lblRenameHint = new System.Windows.Forms.Label();
            this.grpCreateOutput = new System.Windows.Forms.GroupBox();
            this.lbCreatedMul = new System.Windows.Forms.Label();

            // ── 选项卡2: 读取 Muls 控件 ────────────────────────────────────
            this.grpReadMulsActions = new System.Windows.Forms.GroupBox();
            this.BtnCountEntries = new System.Windows.Forms.Button();
            this.lblEntryCount = new System.Windows.Forms.Label();
            this.grpReadMulsResult = new System.Windows.Forms.GroupBox();
            this.BtnShowInfo = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.grpReadSingle = new System.Windows.Forms.GroupBox();
            this.lblIndexHint = new System.Windows.Forms.Label();
            this.textBoxIndex = new System.Windows.Forms.TextBox();
            this.BtnReadArtIdx = new System.Windows.Forms.Button();

            // ── 选项卡3: TileData 控件 ─────────────────────────────────────
            this.grpTileDataDir = new System.Windows.Forms.GroupBox();
            this.btnTileDataBrowse = new System.Windows.Forms.Button();
            this.tbDirTileData = new System.Windows.Forms.TextBox();
            this.grpTileDataConfig = new System.Windows.Forms.GroupBox();
            this.lblLandGroupsLbl = new System.Windows.Forms.Label();
            this.tblandTileGroups = new System.Windows.Forms.TextBox();
            this.lblLandGroupsHint = new System.Windows.Forms.Label();
            this.lblStaticGroupsLbl = new System.Windows.Forms.Label();
            this.tbstaticTileGroups = new System.Windows.Forms.TextBox();
            this.lblStaticGroupsHint = new System.Windows.Forms.Label();
            this.BtCreateTiledata = new System.Windows.Forms.Button();
            this.grpTileDataQuick = new System.Windows.Forms.GroupBox();
            this.BtCreateTiledataEmpty = new System.Windows.Forms.Button();
            this.BtCreateTiledataEmpty2 = new System.Windows.Forms.Button();
            this.BtCreateSimpleTiledata = new System.Windows.Forms.Button();
            this.grpTileDataRead = new System.Windows.Forms.GroupBox();
            this.BtTiledatainfo = new System.Windows.Forms.Button();
            this.BtnCountTileDataEntries = new System.Windows.Forms.Button();
            this.lblTileDataEntryCount = new System.Windows.Forms.Label();
            this.BtReadTileFlags = new System.Windows.Forms.Button();
            this.grpTileDataIndex = new System.Windows.Forms.GroupBox();
            this.lblTiledataIndexHint = new System.Windows.Forms.Label();
            this.textBoxTiledataIndex = new System.Windows.Forms.TextBox();
            this.BtReadIndexTiledata = new System.Windows.Forms.Button();
            this.BtReadLandTile = new System.Windows.Forms.Button();
            this.BtReadStaticTile = new System.Windows.Forms.Button();
            this.BtSelectDirectory = new System.Windows.Forms.Button();
            this.grpTileDataOutput = new System.Windows.Forms.GroupBox();
            this.lbTileDataCreate = new System.Windows.Forms.Label();
            this.checkBoxTileData = new System.Windows.Forms.CheckBox();

            // ── 选项卡4: 读出控件 ──────────────────────────────────────
            this.grpReadOutActions = new System.Windows.Forms.GroupBox();
            this.ButtonReadTileData = new System.Windows.Forms.Button();
            this.ButtonReadLandTileData = new System.Windows.Forms.Button();
            this.ButtonReadStaticTileData = new System.Windows.Forms.Button();
            this.listViewTileData = new System.Windows.Forms.ListView();
            this.lblSelectedEntry = new System.Windows.Forms.Label();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.grpReadOutInfo = new System.Windows.Forms.GroupBox();
            this.lblReadOutIdxLbl = new System.Windows.Forms.Label();
            this.textBoxTileDataInfo = new System.Windows.Forms.TextBox();

            // ── 选项卡5: 纹理控件 ─────────────────────────────────────
            this.grpTexConfig = new System.Windows.Forms.GroupBox();
            this.lblTexCountLbl = new System.Windows.Forms.Label();
            this.tbIndexCountTexture = new System.Windows.Forms.TextBox();
            this.lblTexCountHint = new System.Windows.Forms.Label();
            this.checkBoxTexture = new System.Windows.Forms.CheckBox();
            this.grpTexActions = new System.Windows.Forms.GroupBox();
            this.BtCreateTextur = new System.Windows.Forms.Button();
            this.BtCreateIndexes = new System.Windows.Forms.Button();
            this.grpTexOutput = new System.Windows.Forms.GroupBox();
            this.lbTextureCount = new System.Windows.Forms.Label();
            this.tbIndexCount = new System.Windows.Forms.Label();

            // ── 选项卡6: RadarColor 控件 ───────────────────────────────────
            this.grpRadarConfig = new System.Windows.Forms.GroupBox();
            this.lblRadarCountLbl = new System.Windows.Forms.Label();
            this.indexCountTextBox = new System.Windows.Forms.TextBox();
            this.lblRadarCountHint = new System.Windows.Forms.Label();
            this.grpRadarActions = new System.Windows.Forms.GroupBox();
            this.CreateFileButtonRadarColor = new System.Windows.Forms.Button();
            this.grpRadarOutput = new System.Windows.Forms.GroupBox();
            this.lbRadarColor = new System.Windows.Forms.Label();

            // ── 选项卡7: 调色板控件 ──────────────────────────────────────
            this.grpPaletteCreate = new System.Windows.Forms.GroupBox();
            this.BtCreatePalette = new System.Windows.Forms.Button();
            this.lbCreatePalette = new System.Windows.Forms.Label();
            this.BtCreatePaletteFull = new System.Windows.Forms.Button();
            this.lbCreateColorPalette = new System.Windows.Forms.Label();
            this.grpPaletteLoad = new System.Windows.Forms.GroupBox();
            this.LoadPaletteButton = new System.Windows.Forms.Button();
            this.lblPalettePreview = new System.Windows.Forms.Label();
            this.pictureBoxPalette = new System.Windows.Forms.PictureBox();
            this.grpPaletteValues = new System.Windows.Forms.GroupBox();
            this.textBoxRgbValues = new System.Windows.Forms.TextBox();

            // ── 选项卡8: 动画控件 ────────────────────────────────────
            this.grpAnimSource = new System.Windows.Forms.GroupBox();
            this.tbfilename = new System.Windows.Forms.TextBox();
            this.BtnBrowse = new System.Windows.Forms.Button();
            this.grpAnimOutput = new System.Windows.Forms.GroupBox();
            this.txtOutputDirectory = new System.Windows.Forms.TextBox();
            this.BtnSetOutputDirectory = new System.Windows.Forms.Button();
            this.lblAnimSuffixLbl = new System.Windows.Forms.Label();
            this.txtOutputFilename = new System.Windows.Forms.TextBox();
            this.grpAnimCreature = new System.Windows.Forms.GroupBox();
            this.lblOrigIDHint = new System.Windows.Forms.Label();
            this.txtOrigCreatureID = new System.Windows.Forms.TextBox();
            this.lblHexWarning = new System.Windows.Forms.Label();
            this.lblCopyCountHint = new System.Windows.Forms.Label();
            this.txtNewCreatureID = new System.Windows.Forms.TextBox();
            this.panelCheckbox = new System.Windows.Forms.Panel();
            this.lbCopys = new System.Windows.Forms.Label();
            this.chkLowDetail = new System.Windows.Forms.CheckBox();
            this.chkHighDetail = new System.Windows.Forms.CheckBox();
            this.chkHuman = new System.Windows.Forms.CheckBox();
            this.grpAnimActions = new System.Windows.Forms.GroupBox();
            this.BtnNewAnimIDXFiles = new System.Windows.Forms.Button();
            this.BtnProcessClickOld = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.grpAnimInfo = new System.Windows.Forms.GroupBox();
            this.ReadAnimIdx = new System.Windows.Forms.Button();
            this.btnCountIndices = new System.Windows.Forms.Button();
            this.BtnLoadAnimationMulData = new System.Windows.Forms.Button();
            this.txtData = new System.Windows.Forms.TextBox();
            this.grpAnimLog = new System.Windows.Forms.GroupBox();
            this.tbProcessAminidx = new System.Windows.Forms.TextBox();
            this.lblNewIdCount = new System.Windows.Forms.Label();

            // ── 选项卡9: Artmul 控件 ───────────────────────────────────────
            this.grpArtCreate = new System.Windows.Forms.GroupBox();
            this.lblArtCreateHint = new System.Windows.Forms.Label();
            this.BtnCreateArtIdx100K = new System.Windows.Forms.Button();
            this.BtnCreateArtIdx150K = new System.Windows.Forms.Button();
            this.BtnCreateArtIdx200K = new System.Windows.Forms.Button();
            this.BtnCreateArtIdx250K = new System.Windows.Forms.Button();
            this.BtnCreateArtIdx500K = new System.Windows.Forms.Button();
            this.BtnCreateArtIdx = new System.Windows.Forms.Button();
            this.grpArtCustom = new System.Windows.Forms.GroupBox();
            this.lblArtCustomHint = new System.Windows.Forms.Label();
            this.tbxNewIndex = new System.Windows.Forms.TextBox();
            this.Button2 = new System.Windows.Forms.Button();
            this.lblOldVersionHint = new System.Windows.Forms.Label();
            this.BtCreateOldVersionArtidx = new System.Windows.Forms.Button();
            this.grpArtSplit = new System.Windows.Forms.GroupBox();
            this.lblArtSplitHint = new System.Windows.Forms.Label();
            this.lblArtSplitTotalLbl = new System.Windows.Forms.Label();
            this.tbxNewIndex2 = new System.Windows.Forms.TextBox();
            this.lbArtsCount = new System.Windows.Forms.Label();
            this.tbxArtsCount = new System.Windows.Forms.TextBox();
            this.lbLandTilesCount = new System.Windows.Forms.Label();
            this.tbxLandTilesCount = new System.Windows.Forms.TextBox();
            this.Button3 = new System.Windows.Forms.Button();
            this.grpArtRead = new System.Windows.Forms.GroupBox();
            this.lblArtReadHint = new System.Windows.Forms.Label();
            this.ReadArtmul = new System.Windows.Forms.Button();
            this.ReadArtmul2 = new System.Windows.Forms.Button();
            this.lblIndexCount = new System.Windows.Forms.Label();
            this.grpArtLog = new System.Windows.Forms.GroupBox();
            this.infoARTIDXMULID = new System.Windows.Forms.TextBox();

            // ── 选项卡10: 声音控件 ───────────────────────────────────────
            this.grpSoundConfig = new System.Windows.Forms.GroupBox();
            this.lblSoundCountLbl = new System.Windows.Forms.Label();
            this.SoundIDXMul = new System.Windows.Forms.TextBox();
            this.lblSoundCountHint = new System.Windows.Forms.Label();
            this.grpSoundActions = new System.Windows.Forms.GroupBox();
            this.CreateOrgSoundMul = new System.Windows.Forms.Button();
            this.ReadIndexSize = new System.Windows.Forms.Button();
            this.grpSoundOutput = new System.Windows.Forms.GroupBox();
            this.IndexSizeLabel = new System.Windows.Forms.Label();

            // ── 选项卡11: Gump 控件 ────────────────────────────────────────
            this.grpGumpConfig = new System.Windows.Forms.GroupBox();
            this.lblGumpCountLbl = new System.Windows.Forms.Label();
            this.IndexSizeTextBox = new System.Windows.Forms.TextBox();
            this.lblGumpCountHint = new System.Windows.Forms.Label();
            this.grpGumpActions = new System.Windows.Forms.GroupBox();
            this.CreateGumpButton = new System.Windows.Forms.Button();
            this.ReadGumpButton = new System.Windows.Forms.Button();
            this.grpGumpOutput = new System.Windows.Forms.GroupBox();
            this.gumpLabel = new System.Windows.Forms.Label();

            // ── 选项卡12: Hues 控件 ────────────────────────────────────────
            this.grpHuesActions = new System.Windows.Forms.GroupBox();
            this.BtnCreateHues = new System.Windows.Forms.Button();
            this.BtnReadHues = new System.Windows.Forms.Button();
            this.grpHuesOutput = new System.Windows.Forms.GroupBox();
            this.lblHuesOutput = new System.Windows.Forms.Label();

            // ── 选项卡13: Map/Statics 控件 ─────────────────────────────────
            this.grpMapConfig = new System.Windows.Forms.GroupBox();
            this.lblMapSizeComboLbl = new System.Windows.Forms.Label();
            this.comboMapSize = new System.Windows.Forms.ComboBox();
            this.lblMapWidthLbl = new System.Windows.Forms.Label();
            this.tbMapWidth = new System.Windows.Forms.TextBox();
            this.lblMapHeightLbl = new System.Windows.Forms.Label();
            this.tbMapHeight = new System.Windows.Forms.TextBox();
            this.lblMapIndexLbl = new System.Windows.Forms.Label();
            this.tbMapIndex = new System.Windows.Forms.TextBox();
            this.lblMapSizeInfo = new System.Windows.Forms.Label();
            this.grpMapActions = new System.Windows.Forms.GroupBox();
            this.BtnCreateMap = new System.Windows.Forms.Button();
            this.BtnCreateStatics = new System.Windows.Forms.Button();
            this.BtnCreateMapAndStatics = new System.Windows.Forms.Button();
            this.grpMapOutput = new System.Windows.Forms.GroupBox();
            this.lblMapOutput = new System.Windows.Forms.Label();

            // ── 选项卡14: Multi 控件 ───────────────────────────────────────
            this.grpMultiConfig = new System.Windows.Forms.GroupBox();
            this.lblMultiCountLbl = new System.Windows.Forms.Label();
            this.tbMultiCount = new System.Windows.Forms.TextBox();
            this.lblMultiIndexLbl = new System.Windows.Forms.Label();
            this.tbMultiIndex = new System.Windows.Forms.TextBox();
            this.checkBoxMultiHS = new System.Windows.Forms.CheckBox();
            this.grpMultiActions = new System.Windows.Forms.GroupBox();
            this.BtnCreateMulti = new System.Windows.Forms.Button();
            this.BtnReadMulti = new System.Windows.Forms.Button();
            this.grpMultiOutput = new System.Windows.Forms.GroupBox();
            this.lblMultiOutput = new System.Windows.Forms.Label();

            // ── 选项卡15: Skills 控件 ──────────────────────────────────────
            this.grpSkillsConfig = new System.Windows.Forms.GroupBox();
            this.lblSkillCountLbl = new System.Windows.Forms.Label();
            this.tbSkillCount = new System.Windows.Forms.TextBox();
            this.grpSkillsActions = new System.Windows.Forms.GroupBox();
            this.BtnCreateDefaultSkills = new System.Windows.Forms.Button();
            this.BtnCreateEmptySkills = new System.Windows.Forms.Button();
            this.BtnReadSkills = new System.Windows.Forms.Button();
            this.grpSkillsOutput = new System.Windows.Forms.GroupBox();
            this.lblSkillsOutput = new System.Windows.Forms.Label();
            this.textBoxSkillsInfo = new System.Windows.Forms.TextBox();

            // ── 选项卡16: Validator 控件 ───────────────────────────────────
            this.grpValidatorActions = new System.Windows.Forms.GroupBox();
            this.BtnValidate = new System.Windows.Forms.Button();
            this.BtnCompareDirectories = new System.Windows.Forms.Button();
            this.lblValidatorStatus = new System.Windows.Forms.Label();
            this.grpValidatorOutput = new System.Windows.Forms.GroupBox();
            this.textBoxValidatorOutput = new System.Windows.Forms.TextBox();

            // ── 选项卡17: IDX Patcher 控件 ─────────────────────────────────
            this.grpPatcherFile = new System.Windows.Forms.GroupBox();
            this.lblPatchIdxLbl = new System.Windows.Forms.Label();
            this.tbPatchIdxPath = new System.Windows.Forms.TextBox();
            this.BtnPatchBrowseIdx = new System.Windows.Forms.Button();
            this.grpPatcherEdit = new System.Windows.Forms.GroupBox();
            this.lblPatchIndexLbl = new System.Windows.Forms.Label();
            this.tbPatchIndex = new System.Windows.Forms.TextBox();
            this.lblPatchLookupLbl = new System.Windows.Forms.Label();
            this.tbPatchLookup = new System.Windows.Forms.TextBox();
            this.lblPatchSizeLbl = new System.Windows.Forms.Label();
            this.tbPatchSize = new System.Windows.Forms.TextBox();
            this.lblPatchUnknownLbl = new System.Windows.Forms.Label();
            this.tbPatchUnknown = new System.Windows.Forms.TextBox();
            this.BtnPatchEntry = new System.Windows.Forms.Button();
            this.BtnClearEntry = new System.Windows.Forms.Button();
            this.grpPatcherRange = new System.Windows.Forms.GroupBox();
            this.lblPatchRangeFromLbl = new System.Windows.Forms.Label();
            this.tbPatchRangeFrom = new System.Windows.Forms.TextBox();
            this.lblPatchRangeCountLbl = new System.Windows.Forms.Label();
            this.tbPatchRangeCount = new System.Windows.Forms.TextBox();
            this.BtnReadRange = new System.Windows.Forms.Button();
            this.grpPatcherOutput = new System.Windows.Forms.GroupBox();
            this.textBoxPatcherOutput = new System.Windows.Forms.TextBox();

            // ── 选项卡18: Batch Setup 控件 ─────────────────────────────────
            this.grpBatchConfig = new System.Windows.Forms.GroupBox();
            this.lblBatchMapWLbl = new System.Windows.Forms.Label();
            this.tbBatchMapW = new System.Windows.Forms.TextBox();
            this.lblBatchMapHLbl = new System.Windows.Forms.Label();
            this.tbBatchMapH = new System.Windows.Forms.TextBox();
            this.lblBatchMapIdxLbl = new System.Windows.Forms.Label();
            this.tbBatchMapIdx = new System.Windows.Forms.TextBox();
            this.lblBatchArtLbl = new System.Windows.Forms.Label();
            this.tbBatchArtCount = new System.Windows.Forms.TextBox();
            this.lblBatchSoundLbl = new System.Windows.Forms.Label();
            this.tbBatchSoundCount = new System.Windows.Forms.TextBox();
            this.lblBatchGumpLbl = new System.Windows.Forms.Label();
            this.tbBatchGumpCount = new System.Windows.Forms.TextBox();
            this.lblBatchMultiLbl = new System.Windows.Forms.Label();
            this.tbBatchMultiCount = new System.Windows.Forms.TextBox();
            this.lblBatchTileLandLbl = new System.Windows.Forms.Label();
            this.tbBatchTileLand = new System.Windows.Forms.TextBox();
            this.lblBatchTileStaticLbl = new System.Windows.Forms.Label();
            this.tbBatchTileStatic = new System.Windows.Forms.TextBox();
            this.lblBatchSkillCountLbl = new System.Windows.Forms.Label();
            this.tbBatchSkillCount = new System.Windows.Forms.TextBox();
            this.checkBoxBatchSkills = new System.Windows.Forms.CheckBox();
            this.checkBoxBatchDefaultSkills = new System.Windows.Forms.CheckBox();
            this.grpBatchActions = new System.Windows.Forms.GroupBox();
            this.btnBatchCreate = new System.Windows.Forms.Button();
            this.lblBatchStatus = new System.Windows.Forms.Label();
            this.grpBatchLog = new System.Windows.Forms.GroupBox();
            this.textBoxBatchLog = new System.Windows.Forms.TextBox();

            // ── 选项卡19: Hex Viewer 控件 ──────────────────────────────────
            this.grpHexFile = new System.Windows.Forms.GroupBox();
            this.lblHexFilePathLbl = new System.Windows.Forms.Label();
            this.tbHexFilePath = new System.Windows.Forms.TextBox();
            this.BtnHexBrowse = new System.Windows.Forms.Button();
            this.lblHexFileInfo = new System.Windows.Forms.Label();
            this.grpHexRead = new System.Windows.Forms.GroupBox();
            this.lblHexOffsetLbl = new System.Windows.Forms.Label();
            this.tbHexOffset = new System.Windows.Forms.TextBox();
            this.lblHexLengthLbl = new System.Windows.Forms.Label();
            this.tbHexLength = new System.Windows.Forms.TextBox();
            this.BtnHexRead = new System.Windows.Forms.Button();
            this.BtnHexFileInfo = new System.Windows.Forms.Button();
            this.grpHexSearch = new System.Windows.Forms.GroupBox();
            this.lblHexPatternLbl = new System.Windows.Forms.Label();
            this.tbHexPattern = new System.Windows.Forms.TextBox();
            this.BtnHexSearch = new System.Windows.Forms.Button();
            this.grpHexOutput = new System.Windows.Forms.GroupBox();
            this.textBoxHexOutput = new System.Windows.Forms.TextBox();

            // 对话框
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPalette)).BeginInit();
            this.SuspendLayout();

            // ════════════════════════════════════════════════════════════════
            // 窗体
            // ════════════════════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 700);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ARTMulIDXCreator";
            this.Text = "ARTMulIDXCreator  -  UO MUL/IDX 工具  v2.0";
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);

            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.tsStatusLabel });
            this.statusStrip1.Location = new System.Drawing.Point(0, 674);
            this.statusStrip1.Size = new System.Drawing.Size(960, 22);
            this.tsStatusLabel.Text = "就绪。";

            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Size = new System.Drawing.Size(954, 668);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Controls.Add(this.tabPageCreateMuls);
            this.tabControl1.Controls.Add(this.tabPageReadMuls);
            this.tabControl1.Controls.Add(this.tabPageTileData);
            this.tabControl1.Controls.Add(this.tabPageReadOut);
            this.tabControl1.Controls.Add(this.tabPageTexturen);
            this.tabControl1.Controls.Add(this.tabPageRadarColor);
            this.tabControl1.Controls.Add(this.tabPagePalette);
            this.tabControl1.Controls.Add(this.tabPageAnimation);
            this.tabControl1.Controls.Add(this.tabPageArtmul);
            this.tabControl1.Controls.Add(this.tabPageSound);
            this.tabControl1.Controls.Add(this.tabPageGump);
            this.tabControl1.Controls.Add(this.tabPageHues);
            this.tabControl1.Controls.Add(this.tabPageMap);
            this.tabControl1.Controls.Add(this.tabPageMulti);
            this.tabControl1.Controls.Add(this.tabPageSkills);
            this.tabControl1.Controls.Add(this.tabPageValidator);
            this.tabControl1.Controls.Add(this.tabPageIdxPatcher);
            this.tabControl1.Controls.Add(this.tabPageBatch);
            this.tabControl1.Controls.Add(this.tabPageHexViewer);

            // ════════════════════════════════════════════════════════════════
            // 选项卡1 – 创建 Muls
            // ════════════════════════════════════════════════════════════════
            this.tabPageCreateMuls.Text = "创建 Muls";
            this.tabPageCreateMuls.Size = new System.Drawing.Size(946, 638);
            this.tabPageCreateMuls.TabIndex = 0;
            this.tabPageCreateMuls.UseVisualStyleBackColor = true;

            this.grpCreateMulsDir.Text = "1. 选择目标目录";
            this.grpCreateMulsDir.Location = new System.Drawing.Point(10, 10);
            this.grpCreateMulsDir.Size = new System.Drawing.Size(920, 65);
            this.grpCreateMulsDir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCreateMulsDir.TabIndex = 0;
            this.tabPageCreateMuls.Controls.Add(this.grpCreateMulsDir);

            this.BtFileOrder.Text = "选择目录...";
            this.BtFileOrder.Location = new System.Drawing.Point(12, 25);
            this.BtFileOrder.Size = new System.Drawing.Size(185, 26);
            this.BtFileOrder.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtFileOrder.TabIndex = 0;
            this.BtFileOrder.Click += new System.EventHandler(this.BtFileOrder_Click);
            this.grpCreateMulsDir.Controls.Add(this.BtFileOrder);

            this.textBox1.Location = new System.Drawing.Point(205, 27);
            this.textBox1.Size = new System.Drawing.Size(560, 23);
            this.textBox1.ReadOnly = true;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 9F);
            this.textBox1.TabIndex = 1;
            this.grpCreateMulsDir.Controls.Add(this.textBox1);

            this.lblDirInfo.Text = "artidx.MUL + art.MUL 的目标文件夹";
            this.lblDirInfo.Location = new System.Drawing.Point(775, 30);
            this.lblDirInfo.AutoSize = true;
            this.lblDirInfo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblDirInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblDirInfo.TabIndex = 2;
            this.grpCreateMulsDir.Controls.Add(this.lblDirInfo);

            this.grpCreateMulsCount.Text = "2. 设置条目数";
            this.grpCreateMulsCount.Location = new System.Drawing.Point(10, 83);
            this.grpCreateMulsCount.Size = new System.Drawing.Size(920, 65);
            this.grpCreateMulsCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCreateMulsCount.TabIndex = 1;
            this.tabPageCreateMuls.Controls.Add(this.grpCreateMulsCount);

            this.textBox2.Text = "81884";
            this.textBox2.Location = new System.Drawing.Point(12, 28);
            this.textBox2.Size = new System.Drawing.Size(130, 23);
            this.textBox2.Font = new System.Drawing.Font("Consolas", 10F);
            this.textBox2.TabIndex = 0;
            this.grpCreateMulsCount.Controls.Add(this.textBox2);

            this.lblCountHint.Text = "允许十六进制输入（如 0x14F9C） | 标准原版 UO = 81884";
            this.lblCountHint.Location = new System.Drawing.Point(155, 31);
            this.lblCountHint.AutoSize = true;
            this.lblCountHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblCountHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblCountHint.TabIndex = 1;
            this.grpCreateMulsCount.Controls.Add(this.lblCountHint);

            this.grpCreateMulsButtons.Text = "3. 创建（所有按钮均生成相同的传统格式）";
            this.grpCreateMulsButtons.Location = new System.Drawing.Point(10, 156);
            this.grpCreateMulsButtons.Size = new System.Drawing.Size(560, 195);
            this.grpCreateMulsButtons.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCreateMulsButtons.TabIndex = 2;
            this.tabPageCreateMuls.Controls.Add(this.grpCreateMulsButtons);

            this.BtCreateARTIDXMul.Text = "创建 (long)";
            this.BtCreateARTIDXMul.Location = new System.Drawing.Point(12, 25);
            this.BtCreateARTIDXMul.Size = new System.Drawing.Size(150, 28);
            this.BtCreateARTIDXMul.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateARTIDXMul.TabIndex = 0;
            this.BtCreateARTIDXMul.Click += new System.EventHandler(this.BtCreateARTIDXMul_Click);
            this.grpCreateMulsButtons.Controls.Add(this.BtCreateARTIDXMul);

            this.BtCreateARTIDXMul_Ulong.Text = "创建 (ulong)";
            this.BtCreateARTIDXMul_Ulong.Location = new System.Drawing.Point(170, 25);
            this.BtCreateARTIDXMul_Ulong.Size = new System.Drawing.Size(150, 28);
            this.BtCreateARTIDXMul_Ulong.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateARTIDXMul_Ulong.TabIndex = 1;
            this.BtCreateARTIDXMul_Ulong.Click += new System.EventHandler(this.BtCreateARTIDXMul_Ulong_Click);
            this.grpCreateMulsButtons.Controls.Add(this.BtCreateARTIDXMul_Ulong);

            this.BtCreateARTIDXMul_uint.Text = "创建 (uint)";
            this.BtCreateARTIDXMul_uint.Location = new System.Drawing.Point(328, 25);
            this.BtCreateARTIDXMul_uint.Size = new System.Drawing.Size(150, 28);
            this.BtCreateARTIDXMul_uint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateARTIDXMul_uint.TabIndex = 2;
            this.BtCreateARTIDXMul_uint.Click += new System.EventHandler(this.BtCreateARTIDXMul_uint_Click);
            this.grpCreateMulsButtons.Controls.Add(this.BtCreateARTIDXMul_uint);

            this.BtCreateARTIDXMul_Int.Text = "创建 (int)";
            this.BtCreateARTIDXMul_Int.Location = new System.Drawing.Point(12, 61);
            this.BtCreateARTIDXMul_Int.Size = new System.Drawing.Size(150, 28);
            this.BtCreateARTIDXMul_Int.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateARTIDXMul_Int.TabIndex = 3;
            this.BtCreateARTIDXMul_Int.Click += new System.EventHandler(this.BtCreateARTIDXMul_Int_Click);
            this.grpCreateMulsButtons.Controls.Add(this.BtCreateARTIDXMul_Int);

            this.BtCreateARTIDXMul_Ushort.Text = "创建 (ushort)";
            this.BtCreateARTIDXMul_Ushort.Location = new System.Drawing.Point(170, 61);
            this.BtCreateARTIDXMul_Ushort.Size = new System.Drawing.Size(150, 28);
            this.BtCreateARTIDXMul_Ushort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateARTIDXMul_Ushort.TabIndex = 4;
            this.BtCreateARTIDXMul_Ushort.Click += new System.EventHandler(this.BtCreateARTIDXMul_Ushort_Click);
            this.grpCreateMulsButtons.Controls.Add(this.BtCreateARTIDXMul_Ushort);

            this.BtCreateARTIDXMul_Short.Text = "创建 (short)";
            this.BtCreateARTIDXMul_Short.Location = new System.Drawing.Point(328, 61);
            this.BtCreateARTIDXMul_Short.Size = new System.Drawing.Size(150, 28);
            this.BtCreateARTIDXMul_Short.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateARTIDXMul_Short.TabIndex = 5;
            this.BtCreateARTIDXMul_Short.Click += new System.EventHandler(this.BtCreateARTIDXMul_Short_Click);
            this.grpCreateMulsButtons.Controls.Add(this.BtCreateARTIDXMul_Short);

            this.BtCreateARTIDXMul_Byte.Text = "创建 (byte)";
            this.BtCreateARTIDXMul_Byte.Location = new System.Drawing.Point(12, 97);
            this.BtCreateARTIDXMul_Byte.Size = new System.Drawing.Size(150, 28);
            this.BtCreateARTIDXMul_Byte.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateARTIDXMul_Byte.TabIndex = 6;
            this.BtCreateARTIDXMul_Byte.Click += new System.EventHandler(this.BtCreateARTIDXMul_Byte_Click);
            this.grpCreateMulsButtons.Controls.Add(this.BtCreateARTIDXMul_Byte);

            this.BtCreateARTIDXMul_Sbyte.Text = "创建 (sbyte)";
            this.BtCreateARTIDXMul_Sbyte.Location = new System.Drawing.Point(170, 97);
            this.BtCreateARTIDXMul_Sbyte.Size = new System.Drawing.Size(150, 28);
            this.BtCreateARTIDXMul_Sbyte.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateARTIDXMul_Sbyte.TabIndex = 7;
            this.grpCreateMulsButtons.Controls.Add(this.BtCreateARTIDXMul_Sbyte);

            this.lblButtonsNote.Text = "所有 8 个按钮都生成相同的传统 IDX 格式文件。类型名称是历史遗留。";
            this.lblButtonsNote.Location = new System.Drawing.Point(12, 140);
            this.lblButtonsNote.Size = new System.Drawing.Size(530, 42);
            this.lblButtonsNote.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblButtonsNote.ForeColor = System.Drawing.Color.DimGray;
            this.lblButtonsNote.TabIndex = 8;
            this.grpCreateMulsButtons.Controls.Add(this.lblButtonsNote);

            this.grpRename.Text = "4. 可选：重命名";
            this.grpRename.Location = new System.Drawing.Point(580, 156);
            this.grpRename.Size = new System.Drawing.Size(350, 120);
            this.grpRename.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpRename.TabIndex = 3;
            this.tabPageCreateMuls.Controls.Add(this.grpRename);

            this.ComboBoxMuls.Items.AddRange(new object[] { "纹理" });
            this.ComboBoxMuls.Location = new System.Drawing.Point(12, 28);
            this.ComboBoxMuls.Size = new System.Drawing.Size(200, 23);
            this.ComboBoxMuls.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ComboBoxMuls.TabIndex = 0;
            this.ComboBoxMuls.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMuls_SelectedIndexChanged);
            this.grpRename.Controls.Add(this.ComboBoxMuls);

            this.lblRenameHint.Text = "纹理：art.mul/artidx.mul 将重命名为 texmaps.mul/texidx.mul";
            this.lblRenameHint.Location = new System.Drawing.Point(12, 56);
            this.lblRenameHint.AutoSize = true;
            this.lblRenameHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblRenameHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblRenameHint.TabIndex = 1;
            this.grpRename.Controls.Add(this.lblRenameHint);

            this.grpCreateOutput.Text = "输出";
            this.grpCreateOutput.Location = new System.Drawing.Point(10, 360);
            this.grpCreateOutput.Size = new System.Drawing.Size(920, 65);
            this.grpCreateOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCreateOutput.TabIndex = 4;
            this.tabPageCreateMuls.Controls.Add(this.grpCreateOutput);

            this.lbCreatedMul.Text = "-";
            this.lbCreatedMul.Location = new System.Drawing.Point(12, 22);
            this.lbCreatedMul.Size = new System.Drawing.Size(890, 36);
            this.lbCreatedMul.Font = new System.Drawing.Font("Consolas", 9F);
            this.lbCreatedMul.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbCreatedMul.TabIndex = 0;
            this.grpCreateOutput.Controls.Add(this.lbCreatedMul);

            // ════════════════════════════════════════════════════════════════
            // 选项卡2 – 读取 Muls
            // ════════════════════════════════════════════════════════════════
            this.tabPageReadMuls.Text = "读取 Muls";
            this.tabPageReadMuls.Size = new System.Drawing.Size(946, 638);
            this.tabPageReadMuls.TabIndex = 1;
            this.tabPageReadMuls.UseVisualStyleBackColor = true;

            this.grpReadMulsActions.Text = "操作";
            this.grpReadMulsActions.Location = new System.Drawing.Point(10, 10);
            this.grpReadMulsActions.Size = new System.Drawing.Size(920, 70);
            this.grpReadMulsActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpReadMulsActions.TabIndex = 0;
            this.tabPageReadMuls.Controls.Add(this.grpReadMulsActions);

            this.BtnCountEntries.Text = "统计条目数";
            this.BtnCountEntries.Location = new System.Drawing.Point(12, 28);
            this.BtnCountEntries.Size = new System.Drawing.Size(155, 28);
            this.BtnCountEntries.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCountEntries.TabIndex = 0;
            this.BtnCountEntries.Click += new System.EventHandler(this.BtnCountEntries_Click);
            this.grpReadMulsActions.Controls.Add(this.BtnCountEntries);

            this.lblEntryCount.Text = "-";
            this.lblEntryCount.Location = new System.Drawing.Point(180, 33);
            this.lblEntryCount.AutoSize = true;
            this.lblEntryCount.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblEntryCount.ForeColor = System.Drawing.Color.Navy;
            this.lblEntryCount.TabIndex = 1;
            this.grpReadMulsActions.Controls.Add(this.lblEntryCount);

            this.grpReadMulsResult.Text = "读取所有条目（最多 2000 行）";
            this.grpReadMulsResult.Location = new System.Drawing.Point(10, 88);
            this.grpReadMulsResult.Size = new System.Drawing.Size(920, 360);
            this.grpReadMulsResult.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpReadMulsResult.TabIndex = 1;
            this.tabPageReadMuls.Controls.Add(this.grpReadMulsResult);

            this.BtnShowInfo.Text = "读取整个索引";
            this.BtnShowInfo.Location = new System.Drawing.Point(12, 26);
            this.BtnShowInfo.Size = new System.Drawing.Size(175, 28);
            this.BtnShowInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnShowInfo.TabIndex = 0;
            this.BtnShowInfo.Click += new System.EventHandler(this.BtnShowInfo_Click);
            this.grpReadMulsResult.Controls.Add(this.BtnShowInfo);

            this.textBoxInfo.Location = new System.Drawing.Point(12, 62);
            this.textBoxInfo.Size = new System.Drawing.Size(890, 285);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxInfo.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.TabIndex = 1;
            this.grpReadMulsResult.Controls.Add(this.textBoxInfo);

            this.grpReadSingle.Text = "读取单个条目";
            this.grpReadSingle.Location = new System.Drawing.Point(10, 456);
            this.grpReadSingle.Size = new System.Drawing.Size(920, 65);
            this.grpReadSingle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpReadSingle.TabIndex = 2;
            this.tabPageReadMuls.Controls.Add(this.grpReadSingle);

            this.lblIndexHint.Text = "索引号：";
            this.lblIndexHint.Location = new System.Drawing.Point(12, 32);
            this.lblIndexHint.AutoSize = true;
            this.lblIndexHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblIndexHint.TabIndex = 0;
            this.grpReadSingle.Controls.Add(this.lblIndexHint);

            this.textBoxIndex.Text = "1";
            this.textBoxIndex.Location = new System.Drawing.Point(75, 29);
            this.textBoxIndex.Size = new System.Drawing.Size(70, 23);
            this.textBoxIndex.Font = new System.Drawing.Font("Consolas", 10F);
            this.textBoxIndex.TabIndex = 1;
            this.grpReadSingle.Controls.Add(this.textBoxIndex);

            this.BtnReadArtIdx.Text = "显示条目";
            this.BtnReadArtIdx.Location = new System.Drawing.Point(160, 27);
            this.BtnReadArtIdx.Size = new System.Drawing.Size(160, 28);
            this.BtnReadArtIdx.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnReadArtIdx.TabIndex = 2;
            this.BtnReadArtIdx.Click += new System.EventHandler(this.BtnReadArtIdx2_Click);
            this.grpReadSingle.Controls.Add(this.BtnReadArtIdx);

            // ════════════════════════════════════════════════════════════════
            // 选项卡3 – TileData
            // ════════════════════════════════════════════════════════════════
            this.tabPageTileData.Text = "TileData";
            this.tabPageTileData.Size = new System.Drawing.Size(946, 638);
            this.tabPageTileData.TabIndex = 2;
            this.tabPageTileData.UseVisualStyleBackColor = true;

            this.grpTileDataDir.Text = "目录";
            this.grpTileDataDir.Location = new System.Drawing.Point(10, 10);
            this.grpTileDataDir.Size = new System.Drawing.Size(920, 58);
            this.grpTileDataDir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTileDataDir.TabIndex = 0;
            this.tabPageTileData.Controls.Add(this.grpTileDataDir);

            this.btnTileDataBrowse.Text = "选择...";
            this.btnTileDataBrowse.Location = new System.Drawing.Point(12, 22);
            this.btnTileDataBrowse.Size = new System.Drawing.Size(120, 26);
            this.btnTileDataBrowse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTileDataBrowse.TabIndex = 0;
            this.btnTileDataBrowse.Click += new System.EventHandler(this.BtCreateTiledata_Click);
            this.grpTileDataDir.Controls.Add(this.btnTileDataBrowse);

            this.tbDirTileData.Location = new System.Drawing.Point(142, 24);
            this.tbDirTileData.Size = new System.Drawing.Size(560, 23);
            this.tbDirTileData.ReadOnly = true;
            this.tbDirTileData.Font = new System.Drawing.Font("Consolas", 9F);
            this.tbDirTileData.TabIndex = 1;
            this.grpTileDataDir.Controls.Add(this.tbDirTileData);

            this.grpTileDataConfig.Text = "配置 - 组数 x 32 = 图块条目数";
            this.grpTileDataConfig.Location = new System.Drawing.Point(10, 76);
            this.grpTileDataConfig.Size = new System.Drawing.Size(560, 125);
            this.grpTileDataConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTileDataConfig.TabIndex = 1;
            this.tabPageTileData.Controls.Add(this.grpTileDataConfig);

            this.lblLandGroupsLbl.Text = "地形组数：";
            this.lblLandGroupsLbl.Location = new System.Drawing.Point(12, 28);
            this.lblLandGroupsLbl.AutoSize = true;
            this.lblLandGroupsLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLandGroupsLbl.TabIndex = 0;
            this.grpTileDataConfig.Controls.Add(this.lblLandGroupsLbl);

            this.tblandTileGroups.Text = "512";
            this.tblandTileGroups.Location = new System.Drawing.Point(140, 25);
            this.tblandTileGroups.Size = new System.Drawing.Size(90, 23);
            this.tblandTileGroups.Font = new System.Drawing.Font("Consolas", 10F);
            this.tblandTileGroups.TabIndex = 1;
            this.grpTileDataConfig.Controls.Add(this.tblandTileGroups);

            this.lblLandGroupsHint.Text = "标准：512 x 32 = 16,384 个地形图块";
            this.lblLandGroupsHint.Location = new System.Drawing.Point(245, 28);
            this.lblLandGroupsHint.AutoSize = true;
            this.lblLandGroupsHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblLandGroupsHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblLandGroupsHint.TabIndex = 2;
            this.grpTileDataConfig.Controls.Add(this.lblLandGroupsHint);

            this.lblStaticGroupsLbl.Text = "静态组数：";
            this.lblStaticGroupsLbl.Location = new System.Drawing.Point(12, 62);
            this.lblStaticGroupsLbl.AutoSize = true;
            this.lblStaticGroupsLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStaticGroupsLbl.TabIndex = 3;
            this.grpTileDataConfig.Controls.Add(this.lblStaticGroupsLbl);

            this.tbstaticTileGroups.Text = "2048";
            this.tbstaticTileGroups.Location = new System.Drawing.Point(140, 59);
            this.tbstaticTileGroups.Size = new System.Drawing.Size(90, 23);
            this.tbstaticTileGroups.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbstaticTileGroups.TabIndex = 4;
            this.grpTileDataConfig.Controls.Add(this.tbstaticTileGroups);

            this.lblStaticGroupsHint.Text = "标准：2048 x 32 = 65,536 个静态图块";
            this.lblStaticGroupsHint.Location = new System.Drawing.Point(245, 62);
            this.lblStaticGroupsHint.AutoSize = true;
            this.lblStaticGroupsHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblStaticGroupsHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblStaticGroupsHint.TabIndex = 5;
            this.grpTileDataConfig.Controls.Add(this.lblStaticGroupsHint);

            this.BtCreateTiledata.Text = "创建 Tiledata.mul";
            this.BtCreateTiledata.Location = new System.Drawing.Point(12, 90);
            this.BtCreateTiledata.Size = new System.Drawing.Size(200, 28);
            this.BtCreateTiledata.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtCreateTiledata.TabIndex = 6;
            this.BtCreateTiledata.Click += new System.EventHandler(this.BtCreateTiledata_Click);
            this.grpTileDataConfig.Controls.Add(this.BtCreateTiledata);

            this.grpTileDataQuick.Text = "快速创建（标准值）";
            this.grpTileDataQuick.Location = new System.Drawing.Point(580, 76);
            this.grpTileDataQuick.Size = new System.Drawing.Size(350, 125);
            this.grpTileDataQuick.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTileDataQuick.TabIndex = 2;
            this.tabPageTileData.Controls.Add(this.grpTileDataQuick);

            this.BtCreateTiledataEmpty.Text = "标准空文件 (512 / 2048)";
            this.BtCreateTiledataEmpty.Location = new System.Drawing.Point(10, 22);
            this.BtCreateTiledataEmpty.Size = new System.Drawing.Size(325, 26);
            this.BtCreateTiledataEmpty.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateTiledataEmpty.TabIndex = 0;
            this.BtCreateTiledataEmpty.Click += new System.EventHandler(this.BtCreateTiledataEmpty_Click);
            this.grpTileDataQuick.Controls.Add(this.BtCreateTiledataEmpty);

            this.BtCreateTiledataEmpty2.Text = "完全空文件（最小）";
            this.BtCreateTiledataEmpty2.Location = new System.Drawing.Point(10, 55);
            this.BtCreateTiledataEmpty2.Size = new System.Drawing.Size(325, 26);
            this.BtCreateTiledataEmpty2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateTiledataEmpty2.TabIndex = 1;
            this.BtCreateTiledataEmpty2.Click += new System.EventHandler(this.BtCreateTiledataEmpty2_Click);
            this.grpTileDataQuick.Controls.Add(this.BtCreateTiledataEmpty2);

            this.BtCreateSimpleTiledata.Text = "简单 Tiledata";
            this.BtCreateSimpleTiledata.Location = new System.Drawing.Point(10, 88);
            this.BtCreateSimpleTiledata.Size = new System.Drawing.Size(325, 26);
            this.BtCreateSimpleTiledata.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateSimpleTiledata.TabIndex = 2;
            this.BtCreateSimpleTiledata.Click += new System.EventHandler(this.BtCreateSimpleTiledata_Click);
            this.grpTileDataQuick.Controls.Add(this.BtCreateSimpleTiledata);

            this.grpTileDataRead.Text = "读取 Tiledata.mul";
            this.grpTileDataRead.Location = new System.Drawing.Point(10, 209);
            this.grpTileDataRead.Size = new System.Drawing.Size(920, 80);
            this.grpTileDataRead.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTileDataRead.TabIndex = 3;
            this.tabPageTileData.Controls.Add(this.grpTileDataRead);

            this.BtTiledatainfo.Text = "摘要";
            this.BtTiledatainfo.Location = new System.Drawing.Point(12, 28);
            this.BtTiledatainfo.Size = new System.Drawing.Size(155, 28);
            this.BtTiledatainfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtTiledatainfo.TabIndex = 0;
            this.BtTiledatainfo.Click += new System.EventHandler(this.BtTiledatainfo_Click);
            this.grpTileDataRead.Controls.Add(this.BtTiledatainfo);

            this.BtnCountTileDataEntries.Text = "统计条目数";
            this.BtnCountTileDataEntries.Location = new System.Drawing.Point(180, 28);
            this.BtnCountTileDataEntries.Size = new System.Drawing.Size(150, 28);
            this.BtnCountTileDataEntries.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCountTileDataEntries.TabIndex = 1;
            this.BtnCountTileDataEntries.Click += new System.EventHandler(this.BtnCountTileDataEntries_Click);
            this.grpTileDataRead.Controls.Add(this.BtnCountTileDataEntries);

            this.lblTileDataEntryCount.Text = "-";
            this.lblTileDataEntryCount.Location = new System.Drawing.Point(345, 33);
            this.lblTileDataEntryCount.AutoSize = true;
            this.lblTileDataEntryCount.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblTileDataEntryCount.ForeColor = System.Drawing.Color.Navy;
            this.lblTileDataEntryCount.TabIndex = 2;
            this.grpTileDataRead.Controls.Add(this.lblTileDataEntryCount);

            this.BtReadTileFlags.Text = "显示标志名称";
            this.BtReadTileFlags.Location = new System.Drawing.Point(700, 28);
            this.BtReadTileFlags.Size = new System.Drawing.Size(175, 28);
            this.BtReadTileFlags.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtReadTileFlags.TabIndex = 3;
            this.BtReadTileFlags.Click += new System.EventHandler(this.BtReadTileFlags_Click);
            this.grpTileDataRead.Controls.Add(this.BtReadTileFlags);

            this.grpTileDataIndex.Text = "按索引读取单个条目";
            this.grpTileDataIndex.Location = new System.Drawing.Point(10, 297);
            this.grpTileDataIndex.Size = new System.Drawing.Size(920, 90);
            this.grpTileDataIndex.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTileDataIndex.TabIndex = 4;
            this.tabPageTileData.Controls.Add(this.grpTileDataIndex);

            this.lblTiledataIndexHint.Text = "索引号：";
            this.lblTiledataIndexHint.Location = new System.Drawing.Point(12, 36);
            this.lblTiledataIndexHint.AutoSize = true;
            this.lblTiledataIndexHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTiledataIndexHint.TabIndex = 0;
            this.grpTileDataIndex.Controls.Add(this.lblTiledataIndexHint);

            this.textBoxTiledataIndex.Location = new System.Drawing.Point(78, 33);
            this.textBoxTiledataIndex.Size = new System.Drawing.Size(110, 23);
            this.textBoxTiledataIndex.Font = new System.Drawing.Font("Consolas", 10F);
            this.textBoxTiledataIndex.TabIndex = 1;
            this.grpTileDataIndex.Controls.Add(this.textBoxTiledataIndex);

            this.BtReadIndexTiledata.Text = "读取索引";
            this.BtReadIndexTiledata.Location = new System.Drawing.Point(205, 30);
            this.BtReadIndexTiledata.Size = new System.Drawing.Size(120, 28);
            this.BtReadIndexTiledata.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtReadIndexTiledata.TabIndex = 2;
            this.BtReadIndexTiledata.Click += new System.EventHandler(this.BtReadIndexTiledata_Click);
            this.grpTileDataIndex.Controls.Add(this.BtReadIndexTiledata);

            this.BtReadLandTile.Text = "地形图块";
            this.BtReadLandTile.Location = new System.Drawing.Point(335, 30);
            this.BtReadLandTile.Size = new System.Drawing.Size(120, 28);
            this.BtReadLandTile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtReadLandTile.TabIndex = 3;
            this.BtReadLandTile.Click += new System.EventHandler(this.BtReadLandTile_Click);
            this.grpTileDataIndex.Controls.Add(this.BtReadLandTile);

            this.BtReadStaticTile.Text = "静态图块";
            this.BtReadStaticTile.Location = new System.Drawing.Point(465, 30);
            this.BtReadStaticTile.Size = new System.Drawing.Size(120, 28);
            this.BtReadStaticTile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtReadStaticTile.TabIndex = 4;
            this.BtReadStaticTile.Click += new System.EventHandler(this.BtReadStaticTile_Click);
            this.grpTileDataIndex.Controls.Add(this.BtReadStaticTile);

            this.BtSelectDirectory.Text = "十六进制原始数据";
            this.BtSelectDirectory.Location = new System.Drawing.Point(595, 30);
            this.BtSelectDirectory.Size = new System.Drawing.Size(120, 28);
            this.BtSelectDirectory.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtSelectDirectory.TabIndex = 5;
            this.BtSelectDirectory.Click += new System.EventHandler(this.BtTReadHexAndSelectDirectory_Click);
            this.grpTileDataIndex.Controls.Add(this.BtSelectDirectory);

            this.grpTileDataOutput.Text = "输出";
            this.grpTileDataOutput.Location = new System.Drawing.Point(10, 395);
            this.grpTileDataOutput.Size = new System.Drawing.Size(920, 80);
            this.grpTileDataOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTileDataOutput.TabIndex = 5;
            this.tabPageTileData.Controls.Add(this.grpTileDataOutput);

            this.lbTileDataCreate.Text = "-";
            this.lbTileDataCreate.Location = new System.Drawing.Point(12, 22);
            this.lbTileDataCreate.Size = new System.Drawing.Size(890, 50);
            this.lbTileDataCreate.Font = new System.Drawing.Font("Consolas", 9F);
            this.lbTileDataCreate.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbTileDataCreate.TabIndex = 0;
            this.grpTileDataOutput.Controls.Add(this.lbTileDataCreate);

            this.checkBoxTileData.Text = "原始";
            this.checkBoxTileData.Location = new System.Drawing.Point(10, 482);
            this.checkBoxTileData.AutoSize = true;
            this.checkBoxTileData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxTileData.TabIndex = 6;
            this.tabPageTileData.Controls.Add(this.checkBoxTileData);

            // ════════════════════════════════════════════════════════════════
            // 选项卡4 – 读出
            // ════════════════════════════════════════════════════════════════
            this.tabPageReadOut.Text = "读出";
            this.tabPageReadOut.Size = new System.Drawing.Size(946, 638);
            this.tabPageReadOut.TabIndex = 3;
            this.tabPageReadOut.UseVisualStyleBackColor = true;

            this.grpReadOutActions.Text = "加载 Tiledata.mul（每次点击 50 个条目 | 空格键 = 再加载 50）";
            this.grpReadOutActions.Location = new System.Drawing.Point(10, 10);
            this.grpReadOutActions.Size = new System.Drawing.Size(560, 62);
            this.grpReadOutActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpReadOutActions.TabIndex = 0;
            this.tabPageReadOut.Controls.Add(this.grpReadOutActions);

            this.ButtonReadTileData.Text = "十六进制位置";
            this.ButtonReadTileData.Location = new System.Drawing.Point(12, 26);
            this.ButtonReadTileData.Size = new System.Drawing.Size(145, 26);
            this.ButtonReadTileData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonReadTileData.TabIndex = 0;
            this.ButtonReadTileData.Click += new System.EventHandler(this.ButtonReadTileData_Click);
            this.grpReadOutActions.Controls.Add(this.ButtonReadTileData);

            this.ButtonReadLandTileData.Text = "地形图块";
            this.ButtonReadLandTileData.Location = new System.Drawing.Point(165, 26);
            this.ButtonReadLandTileData.Size = new System.Drawing.Size(145, 26);
            this.ButtonReadLandTileData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonReadLandTileData.TabIndex = 1;
            this.ButtonReadLandTileData.Click += new System.EventHandler(this.ButtonReadLandTileData_Click);
            this.grpReadOutActions.Controls.Add(this.ButtonReadLandTileData);

            this.ButtonReadStaticTileData.Text = "静态图块";
            this.ButtonReadStaticTileData.Location = new System.Drawing.Point(318, 26);
            this.ButtonReadStaticTileData.Size = new System.Drawing.Size(145, 26);
            this.ButtonReadStaticTileData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonReadStaticTileData.TabIndex = 2;
            this.ButtonReadStaticTileData.Click += new System.EventHandler(this.ButtonReadStaticTileData_Click);
            this.grpReadOutActions.Controls.Add(this.ButtonReadStaticTileData);

            this.listViewTileData.Location = new System.Drawing.Point(10, 80);
            this.listViewTileData.Size = new System.Drawing.Size(560, 355);
            this.listViewTileData.View = System.Windows.Forms.View.Details;
            this.listViewTileData.FullRowSelect = true;
            this.listViewTileData.GridLines = true;
            this.listViewTileData.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.listViewTileData.TabIndex = 1;
            this.listViewTileData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TiledataHex_KeyDown);
            this.listViewTileData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListViewTileData_MouseClick);
            this.tabPageReadOut.Controls.Add(this.listViewTileData);

            this.lblSelectedEntry.Text = "选中的条目：";
            this.lblSelectedEntry.Location = new System.Drawing.Point(10, 440);
            this.lblSelectedEntry.AutoSize = true;
            this.lblSelectedEntry.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSelectedEntry.TabIndex = 2;
            this.tabPageReadOut.Controls.Add(this.lblSelectedEntry);

            this.textBoxOutput.Location = new System.Drawing.Point(10, 460);
            this.textBoxOutput.Size = new System.Drawing.Size(560, 80);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Font = new System.Drawing.Font("Consolas", 9F);
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.TabIndex = 3;
            this.tabPageReadOut.Controls.Add(this.textBoxOutput);

            this.grpReadOutInfo.Text = "详细信息";
            this.grpReadOutInfo.Location = new System.Drawing.Point(580, 10);
            this.grpReadOutInfo.Size = new System.Drawing.Size(356, 530);
            this.grpReadOutInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpReadOutInfo.TabIndex = 4;
            this.tabPageReadOut.Controls.Add(this.grpReadOutInfo);

            this.lblReadOutIdxLbl.Text = "索引：";
            this.lblReadOutIdxLbl.Location = new System.Drawing.Point(10, 26);
            this.lblReadOutIdxLbl.AutoSize = true;
            this.lblReadOutIdxLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblReadOutIdxLbl.TabIndex = 0;
            this.grpReadOutInfo.Controls.Add(this.lblReadOutIdxLbl);

            this.textBoxTileDataInfo.Location = new System.Drawing.Point(10, 290);
            this.textBoxTileDataInfo.Size = new System.Drawing.Size(330, 225);
            this.textBoxTileDataInfo.Multiline = true;
            this.textBoxTileDataInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTileDataInfo.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.textBoxTileDataInfo.ReadOnly = true;
            this.textBoxTileDataInfo.TabIndex = 8;
            this.grpReadOutInfo.Controls.Add(this.textBoxTileDataInfo);

            // ════════════════════════════════════════════════════════════════
            // 选项卡5 – 纹理
            // ════════════════════════════════════════════════════════════════
            this.tabPageTexturen.Text = "纹理";
            this.tabPageTexturen.Size = new System.Drawing.Size(946, 638);
            this.tabPageTexturen.TabIndex = 4;
            this.tabPageTexturen.UseVisualStyleBackColor = true;

            this.grpTexConfig.Text = "配置";
            this.grpTexConfig.Location = new System.Drawing.Point(10, 10);
            this.grpTexConfig.Size = new System.Drawing.Size(920, 95);
            this.grpTexConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTexConfig.TabIndex = 0;
            this.tabPageTexturen.Controls.Add(this.grpTexConfig);

            this.lblTexCountLbl.Text = "索引条目数：";
            this.lblTexCountLbl.Location = new System.Drawing.Point(12, 30);
            this.lblTexCountLbl.AutoSize = true;
            this.lblTexCountLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTexCountLbl.TabIndex = 0;
            this.grpTexConfig.Controls.Add(this.lblTexCountLbl);

            this.tbIndexCountTexture.Text = "16383";
            this.tbIndexCountTexture.Location = new System.Drawing.Point(185, 27);
            this.tbIndexCountTexture.Size = new System.Drawing.Size(110, 23);
            this.tbIndexCountTexture.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbIndexCountTexture.TabIndex = 1;
            this.grpTexConfig.Controls.Add(this.tbIndexCountTexture);

            this.lblTexCountHint.Text = "标准：16383 个条目。";
            this.lblTexCountHint.Location = new System.Drawing.Point(310, 30);
            this.lblTexCountHint.AutoSize = true;
            this.lblTexCountHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblTexCountHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblTexCountHint.TabIndex = 2;
            this.grpTexConfig.Controls.Add(this.lblTexCountHint);

            this.checkBoxTexture.Text = "仅前 2 个图像有效（其余为黑色占位符）";
            this.checkBoxTexture.Checked = true;
            this.checkBoxTexture.Location = new System.Drawing.Point(12, 60);
            this.checkBoxTexture.AutoSize = true;
            this.checkBoxTexture.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxTexture.TabIndex = 3;
            this.grpTexConfig.Controls.Add(this.checkBoxTexture);

            this.grpTexActions.Text = "操作";
            this.grpTexActions.Location = new System.Drawing.Point(10, 113);
            this.grpTexActions.Size = new System.Drawing.Size(920, 75);
            this.grpTexActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTexActions.TabIndex = 1;
            this.tabPageTexturen.Controls.Add(this.grpTexActions);

            this.BtCreateTextur.Text = "创建 TexMaps.mul + TexIdx.mul";
            this.BtCreateTextur.Location = new System.Drawing.Point(12, 28);
            this.BtCreateTextur.Size = new System.Drawing.Size(280, 30);
            this.BtCreateTextur.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtCreateTextur.TabIndex = 0;
            this.BtCreateTextur.Click += new System.EventHandler(this.BtCreateTextur_Click);
            this.grpTexActions.Controls.Add(this.BtCreateTextur);

            this.BtCreateIndexes.Text = "仅创建空的 TexIdx.mul";
            this.BtCreateIndexes.Location = new System.Drawing.Point(310, 28);
            this.BtCreateIndexes.Size = new System.Drawing.Size(255, 30);
            this.BtCreateIndexes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreateIndexes.TabIndex = 1;
            this.BtCreateIndexes.Click += new System.EventHandler(this.BtCreateIndexes_Click);
            this.grpTexActions.Controls.Add(this.BtCreateIndexes);

            this.grpTexOutput.Text = "输出";
            this.grpTexOutput.Location = new System.Drawing.Point(10, 196);
            this.grpTexOutput.Size = new System.Drawing.Size(920, 65);
            this.grpTexOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpTexOutput.TabIndex = 2;
            this.tabPageTexturen.Controls.Add(this.grpTexOutput);

            this.lbTextureCount.Text = "-";
            this.lbTextureCount.Location = new System.Drawing.Point(12, 22);
            this.lbTextureCount.AutoSize = true;
            this.lbTextureCount.Font = new System.Drawing.Font("Consolas", 9F);
            this.lbTextureCount.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbTextureCount.TabIndex = 0;
            this.grpTexOutput.Controls.Add(this.lbTextureCount);

            this.tbIndexCount.Text = "";
            this.tbIndexCount.Location = new System.Drawing.Point(12, 40);
            this.tbIndexCount.AutoSize = true;
            this.tbIndexCount.Font = new System.Drawing.Font("Consolas", 9F);
            this.tbIndexCount.ForeColor = System.Drawing.Color.Navy;
            this.tbIndexCount.TabIndex = 1;
            this.grpTexOutput.Controls.Add(this.tbIndexCount);

            // ════════════════════════════════════════════════════════════════
            // 选项卡6 – RadarColor
            // ════════════════════════════════════════════════════════════════
            this.tabPageRadarColor.Text = "雷达颜色";
            this.tabPageRadarColor.Size = new System.Drawing.Size(946, 638);
            this.tabPageRadarColor.TabIndex = 5;
            this.tabPageRadarColor.UseVisualStyleBackColor = true;

            this.grpRadarConfig.Text = "配置 - radarcol.mul";
            this.grpRadarConfig.Location = new System.Drawing.Point(10, 10);
            this.grpRadarConfig.Size = new System.Drawing.Size(920, 105);
            this.grpRadarConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpRadarConfig.TabIndex = 0;
            this.tabPageRadarColor.Controls.Add(this.grpRadarConfig);

            this.lblRadarCountLbl.Text = "颜色条目数：";
            this.lblRadarCountLbl.Location = new System.Drawing.Point(12, 32);
            this.lblRadarCountLbl.AutoSize = true;
            this.lblRadarCountLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRadarCountLbl.TabIndex = 0;
            this.grpRadarConfig.Controls.Add(this.lblRadarCountLbl);

            this.indexCountTextBox.Text = "82374";
            this.indexCountTextBox.Location = new System.Drawing.Point(180, 29);
            this.indexCountTextBox.Size = new System.Drawing.Size(120, 23);
            this.indexCountTextBox.Font = new System.Drawing.Font("Consolas", 10F);
            this.indexCountTextBox.TabIndex = 1;
            this.grpRadarConfig.Controls.Add(this.indexCountTextBox);

            this.lblRadarCountHint.Text = "每个条目 = 2 字节（16 位 RGB555）。标准 UO：82374 个条目";
            this.lblRadarCountHint.Location = new System.Drawing.Point(315, 32);
            this.lblRadarCountHint.AutoSize = true;
            this.lblRadarCountHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblRadarCountHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblRadarCountHint.TabIndex = 2;
            this.grpRadarConfig.Controls.Add(this.lblRadarCountHint);

            this.grpRadarActions.Text = "操作";
            this.grpRadarActions.Location = new System.Drawing.Point(10, 123);
            this.grpRadarActions.Size = new System.Drawing.Size(920, 65);
            this.grpRadarActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpRadarActions.TabIndex = 1;
            this.tabPageRadarColor.Controls.Add(this.grpRadarActions);

            this.CreateFileButtonRadarColor.Text = "创建 radarcol.mul";
            this.CreateFileButtonRadarColor.Location = new System.Drawing.Point(12, 22);
            this.CreateFileButtonRadarColor.Size = new System.Drawing.Size(220, 30);
            this.CreateFileButtonRadarColor.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.CreateFileButtonRadarColor.TabIndex = 0;
            this.CreateFileButtonRadarColor.Click += new System.EventHandler(this.CreateFileButtonRadarColor_Click);
            this.grpRadarActions.Controls.Add(this.CreateFileButtonRadarColor);

            this.grpRadarOutput.Text = "输出";
            this.grpRadarOutput.Location = new System.Drawing.Point(10, 196);
            this.grpRadarOutput.Size = new System.Drawing.Size(920, 65);
            this.grpRadarOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpRadarOutput.TabIndex = 2;
            this.tabPageRadarColor.Controls.Add(this.grpRadarOutput);

            this.lbRadarColor.Text = "-";
            this.lbRadarColor.Location = new System.Drawing.Point(12, 22);
            this.lbRadarColor.AutoSize = true;
            this.lbRadarColor.Font = new System.Drawing.Font("Consolas", 9F);
            this.lbRadarColor.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbRadarColor.TabIndex = 0;
            this.grpRadarOutput.Controls.Add(this.lbRadarColor);

            // ════════════════════════════════════════════════════════════════
            // 选项卡7 – 调色板
            // ════════════════════════════════════════════════════════════════
            this.tabPagePalette.Text = "调色板";
            this.tabPagePalette.Size = new System.Drawing.Size(946, 638);
            this.tabPagePalette.TabIndex = 6;
            this.tabPagePalette.UseVisualStyleBackColor = true;

            this.grpPaletteCreate.Text = "创建调色板 - 256 色";
            this.grpPaletteCreate.Location = new System.Drawing.Point(10, 10);
            this.grpPaletteCreate.Size = new System.Drawing.Size(920, 95);
            this.grpPaletteCreate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPaletteCreate.TabIndex = 0;
            this.tabPagePalette.Controls.Add(this.grpPaletteCreate);

            this.BtCreatePalette.Text = "灰度调色板";
            this.BtCreatePalette.Location = new System.Drawing.Point(12, 25);
            this.BtCreatePalette.Size = new System.Drawing.Size(180, 30);
            this.BtCreatePalette.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreatePalette.TabIndex = 0;
            this.BtCreatePalette.Click += new System.EventHandler(this.BtCreatePalette_Click);
            this.grpPaletteCreate.Controls.Add(this.BtCreatePalette);

            this.lbCreatePalette.Text = "256 级灰度（#000 至 #FFF）";
            this.lbCreatePalette.Location = new System.Drawing.Point(200, 31);
            this.lbCreatePalette.AutoSize = true;
            this.lbCreatePalette.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lbCreatePalette.ForeColor = System.Drawing.Color.DimGray;
            this.lbCreatePalette.TabIndex = 1;
            this.grpPaletteCreate.Controls.Add(this.lbCreatePalette);

            this.BtCreatePaletteFull.Text = "UO 标准调色板";
            this.BtCreatePaletteFull.Location = new System.Drawing.Point(12, 60);
            this.BtCreatePaletteFull.Size = new System.Drawing.Size(180, 30);
            this.BtCreatePaletteFull.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtCreatePaletteFull.TabIndex = 2;
            this.BtCreatePaletteFull.Click += new System.EventHandler(this.BtCreatePaletteFull_Click);
            this.grpPaletteCreate.Controls.Add(this.BtCreatePaletteFull);

            this.lbCreateColorPalette.Text = "UO 定义的颜色 + 灰度填充至 256 色";
            this.lbCreateColorPalette.Location = new System.Drawing.Point(200, 66);
            this.lbCreateColorPalette.AutoSize = true;
            this.lbCreateColorPalette.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lbCreateColorPalette.ForeColor = System.Drawing.Color.DimGray;
            this.lbCreateColorPalette.TabIndex = 3;
            this.grpPaletteCreate.Controls.Add(this.lbCreateColorPalette);

            this.grpPaletteLoad.Text = "加载并预览调色板";
            this.grpPaletteLoad.Location = new System.Drawing.Point(10, 113);
            this.grpPaletteLoad.Size = new System.Drawing.Size(920, 195);
            this.grpPaletteLoad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPaletteLoad.TabIndex = 1;
            this.tabPagePalette.Controls.Add(this.grpPaletteLoad);

            this.LoadPaletteButton.Text = "加载 Palette.mul ...";
            this.LoadPaletteButton.Location = new System.Drawing.Point(12, 26);
            this.LoadPaletteButton.Size = new System.Drawing.Size(200, 30);
            this.LoadPaletteButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LoadPaletteButton.TabIndex = 0;
            this.LoadPaletteButton.Click += new System.EventHandler(this.LoadPaletteButton_Click);
            this.grpPaletteLoad.Controls.Add(this.LoadPaletteButton);

            this.lblPalettePreview.Text = "预览（256 色）：";
            this.lblPalettePreview.Location = new System.Drawing.Point(12, 65);
            this.lblPalettePreview.AutoSize = true;
            this.lblPalettePreview.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPalettePreview.TabIndex = 1;
            this.grpPaletteLoad.Controls.Add(this.lblPalettePreview);

            this.pictureBoxPalette.Location = new System.Drawing.Point(12, 83);
            this.pictureBoxPalette.Size = new System.Drawing.Size(890, 80);
            this.pictureBoxPalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPalette.TabIndex = 2;
            this.pictureBoxPalette.TabStop = false;
            this.grpPaletteLoad.Controls.Add(this.pictureBoxPalette);

            this.grpPaletteValues.Text = "全部 256 色的 RGB 值";
            this.grpPaletteValues.Location = new System.Drawing.Point(10, 316);
            this.grpPaletteValues.Size = new System.Drawing.Size(920, 230);
            this.grpPaletteValues.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPaletteValues.TabIndex = 2;
            this.tabPagePalette.Controls.Add(this.grpPaletteValues);

            this.textBoxRgbValues.Location = new System.Drawing.Point(12, 24);
            this.textBoxRgbValues.Size = new System.Drawing.Size(890, 195);
            this.textBoxRgbValues.Multiline = true;
            this.textBoxRgbValues.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxRgbValues.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.textBoxRgbValues.ReadOnly = true;
            this.textBoxRgbValues.TabIndex = 0;
            this.grpPaletteValues.Controls.Add(this.textBoxRgbValues);

            // ════════════════════════════════════════════════════════════════
            // 选项卡8 – 动画
            // ════════════════════════════════════════════════════════════════
            this.tabPageAnimation.Text = "动画";
            this.tabPageAnimation.Size = new System.Drawing.Size(946, 638);
            this.tabPageAnimation.TabIndex = 7;
            this.tabPageAnimation.UseVisualStyleBackColor = true;

            this.grpAnimSource.Text = "1. 源文件 (Anim.idx)";
            this.grpAnimSource.Location = new System.Drawing.Point(10, 10);
            this.grpAnimSource.Size = new System.Drawing.Size(460, 62);
            this.grpAnimSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpAnimSource.TabIndex = 0;
            this.tabPageAnimation.Controls.Add(this.grpAnimSource);

            this.tbfilename.Location = new System.Drawing.Point(12, 28);
            this.tbfilename.Size = new System.Drawing.Size(315, 23);
            this.tbfilename.Font = new System.Drawing.Font("Consolas", 9F);
            this.tbfilename.TabIndex = 0;
            this.grpAnimSource.Controls.Add(this.tbfilename);

            this.BtnBrowse.Text = "加载 ...";
            this.BtnBrowse.Location = new System.Drawing.Point(340, 25);
            this.BtnBrowse.Size = new System.Drawing.Size(108, 26);
            this.BtnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnBrowse.TabIndex = 1;
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowseClick);
            this.grpAnimSource.Controls.Add(this.BtnBrowse);

            this.grpAnimOutput.Text = "2. 目标目录和文件名";
            this.grpAnimOutput.Location = new System.Drawing.Point(480, 10);
            this.grpAnimOutput.Size = new System.Drawing.Size(456, 62);
            this.grpAnimOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpAnimOutput.TabIndex = 1;
            this.tabPageAnimation.Controls.Add(this.grpAnimOutput);

            this.txtOutputDirectory.Location = new System.Drawing.Point(12, 28);
            this.txtOutputDirectory.Size = new System.Drawing.Size(220, 23);
            this.txtOutputDirectory.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtOutputDirectory.TabIndex = 0;
            this.grpAnimOutput.Controls.Add(this.txtOutputDirectory);

            this.BtnSetOutputDirectory.Text = "选择";
            this.BtnSetOutputDirectory.Location = new System.Drawing.Point(240, 25);
            this.BtnSetOutputDirectory.Size = new System.Drawing.Size(90, 26);
            this.BtnSetOutputDirectory.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnSetOutputDirectory.TabIndex = 1;
            this.BtnSetOutputDirectory.Click += new System.EventHandler(this.BtnSetOutputDirectoryClick);
            this.grpAnimOutput.Controls.Add(this.BtnSetOutputDirectory);

            this.lblAnimSuffixLbl.Text = "后缀：";
            this.lblAnimSuffixLbl.Location = new System.Drawing.Point(340, 31);
            this.lblAnimSuffixLbl.AutoSize = true;
            this.lblAnimSuffixLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAnimSuffixLbl.TabIndex = 2;
            this.grpAnimOutput.Controls.Add(this.lblAnimSuffixLbl);

            this.txtOutputFilename.Location = new System.Drawing.Point(385, 28);
            this.txtOutputFilename.Size = new System.Drawing.Size(60, 23);
            this.txtOutputFilename.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtOutputFilename.TabIndex = 3;
            this.grpAnimOutput.Controls.Add(this.txtOutputFilename);

            this.grpAnimCreature.Text = "3. 生物设置";
            this.grpAnimCreature.Location = new System.Drawing.Point(10, 80);
            this.grpAnimCreature.Size = new System.Drawing.Size(460, 145);
            this.grpAnimCreature.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpAnimCreature.TabIndex = 2;
            this.tabPageAnimation.Controls.Add(this.grpAnimCreature);

            this.lblOrigIDHint.Text = "源生物 ID（十六进制）：";
            this.lblOrigIDHint.Location = new System.Drawing.Point(12, 30);
            this.lblOrigIDHint.AutoSize = true;
            this.lblOrigIDHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOrigIDHint.TabIndex = 0;
            this.grpAnimCreature.Controls.Add(this.lblOrigIDHint);

            this.txtOrigCreatureID.Location = new System.Drawing.Point(200, 27);
            this.txtOrigCreatureID.Size = new System.Drawing.Size(100, 23);
            this.txtOrigCreatureID.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtOrigCreatureID.TabIndex = 1;
            this.grpAnimCreature.Controls.Add(this.txtOrigCreatureID);

            this.lblHexWarning.Text = "注意：十六进制输入！例如 0A = ID 10";
            this.lblHexWarning.Location = new System.Drawing.Point(12, 55);
            this.lblHexWarning.AutoSize = true;
            this.lblHexWarning.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblHexWarning.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblHexWarning.TabIndex = 2;
            this.grpAnimCreature.Controls.Add(this.lblHexWarning);

            this.lblCopyCountHint.Text = "副本数量（或复选框）：";
            this.lblCopyCountHint.Location = new System.Drawing.Point(12, 80);
            this.lblCopyCountHint.AutoSize = true;
            this.lblCopyCountHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCopyCountHint.TabIndex = 3;
            this.grpAnimCreature.Controls.Add(this.lblCopyCountHint);

            this.txtNewCreatureID.Location = new System.Drawing.Point(220, 77);
            this.txtNewCreatureID.Size = new System.Drawing.Size(100, 23);
            this.txtNewCreatureID.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtNewCreatureID.TabIndex = 4;
            this.grpAnimCreature.Controls.Add(this.txtNewCreatureID);

            this.panelCheckbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCheckbox.Location = new System.Drawing.Point(12, 108);
            this.panelCheckbox.Size = new System.Drawing.Size(435, 28);
            this.panelCheckbox.BackColor = System.Drawing.Color.AliceBlue;
            this.panelCheckbox.TabIndex = 5;
            this.grpAnimCreature.Controls.Add(this.panelCheckbox);

            this.lbCopys.Text = "快速选择：";
            this.lbCopys.Location = new System.Drawing.Point(5, 6);
            this.lbCopys.AutoSize = true;
            this.lbCopys.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lbCopys.TabIndex = 0;
            this.panelCheckbox.Controls.Add(this.lbCopys);

            this.chkLowDetail.Text = "低细节 x65";
            this.chkLowDetail.Location = new System.Drawing.Point(90, 5);
            this.chkLowDetail.AutoSize = true;
            this.chkLowDetail.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkLowDetail.TabIndex = 1;
            this.panelCheckbox.Controls.Add(this.chkLowDetail);

            this.chkHighDetail.Text = "高细节 x110";
            this.chkHighDetail.Location = new System.Drawing.Point(200, 5);
            this.chkHighDetail.AutoSize = true;
            this.chkHighDetail.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkHighDetail.TabIndex = 2;
            this.panelCheckbox.Controls.Add(this.chkHighDetail);

            this.chkHuman.Text = "人类 x175";
            this.chkHuman.Location = new System.Drawing.Point(320, 5);
            this.chkHuman.AutoSize = true;
            this.chkHuman.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkHuman.TabIndex = 3;
            this.panelCheckbox.Controls.Add(this.chkHuman);

            this.grpAnimActions.Text = "4. 操作";
            this.grpAnimActions.Location = new System.Drawing.Point(480, 80);
            this.grpAnimActions.Size = new System.Drawing.Size(456, 145);
            this.grpAnimActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpAnimActions.TabIndex = 3;
            this.tabPageAnimation.Controls.Add(this.grpAnimActions);

            this.BtnNewAnimIDXFiles.Text = "创建动画 IDX（新版）";
            this.BtnNewAnimIDXFiles.Location = new System.Drawing.Point(10, 25);
            this.BtnNewAnimIDXFiles.Size = new System.Drawing.Size(260, 28);
            this.BtnNewAnimIDXFiles.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtnNewAnimIDXFiles.TabIndex = 0;
            this.BtnNewAnimIDXFiles.Click += new System.EventHandler(this.BtnProcessClick);
            this.grpAnimActions.Controls.Add(this.BtnNewAnimIDXFiles);

            this.BtnProcessClickOld.Text = "创建动画 IDX（旧版）";
            this.BtnProcessClickOld.Location = new System.Drawing.Point(10, 61);
            this.BtnProcessClickOld.Size = new System.Drawing.Size(260, 28);
            this.BtnProcessClickOld.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnProcessClickOld.TabIndex = 1;
            this.BtnProcessClickOld.Click += new System.EventHandler(this.BtnProcessClickOldVersion);
            this.grpAnimActions.Controls.Add(this.BtnProcessClickOld);

            this.Button1.Text = "创建空的 anim.mul";
            this.Button1.Location = new System.Drawing.Point(10, 97);
            this.Button1.Size = new System.Drawing.Size(260, 28);
            this.Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Button1.TabIndex = 2;
            this.Button1.Click += new System.EventHandler(this.BtnSingleEmptyAnimMul_Click);
            this.grpAnimActions.Controls.Add(this.Button1);

            this.grpAnimInfo.Text = "读取并分析 Anim.idx";
            this.grpAnimInfo.Location = new System.Drawing.Point(10, 235);
            this.grpAnimInfo.Size = new System.Drawing.Size(460, 100);
            this.grpAnimInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpAnimInfo.TabIndex = 4;
            this.tabPageAnimation.Controls.Add(this.grpAnimInfo);

            this.ReadAnimIdx.Text = "显示条目";
            this.ReadAnimIdx.Location = new System.Drawing.Point(12, 28);
            this.ReadAnimIdx.Size = new System.Drawing.Size(175, 28);
            this.ReadAnimIdx.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ReadAnimIdx.TabIndex = 0;
            this.ReadAnimIdx.Click += new System.EventHandler(this.ReadAnimIdx_Click);
            this.grpAnimInfo.Controls.Add(this.ReadAnimIdx);

            this.btnCountIndices.Text = "统计条目数";
            this.btnCountIndices.Location = new System.Drawing.Point(200, 28);
            this.btnCountIndices.Size = new System.Drawing.Size(150, 28);
            this.btnCountIndices.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCountIndices.TabIndex = 1;
            this.btnCountIndices.Click += new System.EventHandler(this.BtnCountIndices_Click);
            this.grpAnimInfo.Controls.Add(this.btnCountIndices);

            this.BtnLoadAnimationMulData.Text = "加载 Anim.mul + .idx";
            this.BtnLoadAnimationMulData.Location = new System.Drawing.Point(12, 62);
            this.BtnLoadAnimationMulData.Size = new System.Drawing.Size(200, 28);
            this.BtnLoadAnimationMulData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnLoadAnimationMulData.TabIndex = 2;
            this.BtnLoadAnimationMulData.Click += new System.EventHandler(this.BtnLoadAnimationMulData_Click);
            this.grpAnimInfo.Controls.Add(this.BtnLoadAnimationMulData);

            this.txtData.Location = new System.Drawing.Point(222, 62);
            this.txtData.Size = new System.Drawing.Size(225, 28);
            this.txtData.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtData.ReadOnly = true;
            this.txtData.TabIndex = 3;
            this.grpAnimInfo.Controls.Add(this.txtData);

            this.grpAnimLog.Text = "日志 / 输出";
            this.grpAnimLog.Location = new System.Drawing.Point(10, 343);
            this.grpAnimLog.Size = new System.Drawing.Size(926, 250);
            this.grpAnimLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpAnimLog.TabIndex = 5;
            this.tabPageAnimation.Controls.Add(this.grpAnimLog);

            this.tbProcessAminidx.Location = new System.Drawing.Point(12, 24);
            this.tbProcessAminidx.Size = new System.Drawing.Size(800, 215);
            this.tbProcessAminidx.Multiline = true;
            this.tbProcessAminidx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbProcessAminidx.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.tbProcessAminidx.ReadOnly = true;
            this.tbProcessAminidx.TabIndex = 0;
            this.grpAnimLog.Controls.Add(this.tbProcessAminidx);

            this.lblNewIdCount.Text = "已创建 ID：0";
            this.lblNewIdCount.Location = new System.Drawing.Point(824, 24);
            this.lblNewIdCount.AutoSize = true;
            this.lblNewIdCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNewIdCount.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblNewIdCount.TabIndex = 1;
            this.grpAnimLog.Controls.Add(this.lblNewIdCount);

            // ════════════════════════════════════════════════════════════════
            // 选项卡9 – Artmul
            // ════════════════════════════════════════════════════════════════
            this.tabPageArtmul.Text = "Artmul";
            this.tabPageArtmul.Size = new System.Drawing.Size(946, 638);
            this.tabPageArtmul.TabIndex = 8;
            this.tabPageArtmul.UseVisualStyleBackColor = true;

            this.grpArtCreate.Text = "快速创建 - 预定义大小";
            this.grpArtCreate.Location = new System.Drawing.Point(10, 10);
            this.grpArtCreate.Size = new System.Drawing.Size(295, 260);
            this.grpArtCreate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpArtCreate.TabIndex = 0;
            this.tabPageArtmul.Controls.Add(this.grpArtCreate);

            this.lblArtCreateHint.Text = "保存对话框将自动打开。\r\n将创建 artidx.mul + art.mul。";
            this.lblArtCreateHint.Location = new System.Drawing.Point(12, 22);
            this.lblArtCreateHint.AutoSize = true;
            this.lblArtCreateHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblArtCreateHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblArtCreateHint.TabIndex = 0;
            this.grpArtCreate.Controls.Add(this.lblArtCreateHint);

            this.BtnCreateArtIdx100K.Text = "100,000 个条目";
            this.BtnCreateArtIdx100K.Location = new System.Drawing.Point(12, 55);
            this.BtnCreateArtIdx100K.Size = new System.Drawing.Size(265, 28);
            this.BtnCreateArtIdx100K.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCreateArtIdx100K.TabIndex = 1;
            this.BtnCreateArtIdx100K.Click += new System.EventHandler(this.BtnCreateArtIdx100K_Click);
            this.grpArtCreate.Controls.Add(this.BtnCreateArtIdx100K);

            this.BtnCreateArtIdx150K.Text = "150,000 个条目";
            this.BtnCreateArtIdx150K.Location = new System.Drawing.Point(12, 89);
            this.BtnCreateArtIdx150K.Size = new System.Drawing.Size(265, 28);
            this.BtnCreateArtIdx150K.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCreateArtIdx150K.TabIndex = 2;
            this.BtnCreateArtIdx150K.Click += new System.EventHandler(this.BtnCreateArtIdx150K_Click);
            this.grpArtCreate.Controls.Add(this.BtnCreateArtIdx150K);

            this.BtnCreateArtIdx200K.Text = "200,000 个条目";
            this.BtnCreateArtIdx200K.Location = new System.Drawing.Point(12, 123);
            this.BtnCreateArtIdx200K.Size = new System.Drawing.Size(265, 28);
            this.BtnCreateArtIdx200K.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCreateArtIdx200K.TabIndex = 3;
            this.BtnCreateArtIdx200K.Click += new System.EventHandler(this.BtnCreateArtIdx200K_Click);
            this.grpArtCreate.Controls.Add(this.BtnCreateArtIdx200K);

            this.BtnCreateArtIdx250K.Text = "250,000 个条目";
            this.BtnCreateArtIdx250K.Location = new System.Drawing.Point(12, 157);
            this.BtnCreateArtIdx250K.Size = new System.Drawing.Size(265, 28);
            this.BtnCreateArtIdx250K.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCreateArtIdx250K.TabIndex = 4;
            this.BtnCreateArtIdx250K.Click += new System.EventHandler(this.BtnCreateArtIdx250K_Click);
            this.grpArtCreate.Controls.Add(this.BtnCreateArtIdx250K);

            this.BtnCreateArtIdx500K.Text = "500,000 个条目";
            this.BtnCreateArtIdx500K.Location = new System.Drawing.Point(12, 191);
            this.BtnCreateArtIdx500K.Size = new System.Drawing.Size(265, 28);
            this.BtnCreateArtIdx500K.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCreateArtIdx500K.TabIndex = 5;
            this.BtnCreateArtIdx500K.Click += new System.EventHandler(this.BtnCreateArtIdx500K_Click);
            this.grpArtCreate.Controls.Add(this.BtnCreateArtIdx500K);

            this.BtnCreateArtIdx.Text = "1,000,000 个条目";
            this.BtnCreateArtIdx.Location = new System.Drawing.Point(12, 225);
            this.BtnCreateArtIdx.Size = new System.Drawing.Size(265, 28);
            this.BtnCreateArtIdx.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCreateArtIdx.TabIndex = 6;
            this.BtnCreateArtIdx.Click += new System.EventHandler(this.BtnCreateArtIdx_Click);
            this.grpArtCreate.Controls.Add(this.BtnCreateArtIdx);

            this.grpArtCustom.Text = "输入自定义大小";
            this.grpArtCustom.Location = new System.Drawing.Point(315, 10);
            this.grpArtCustom.Size = new System.Drawing.Size(275, 155);
            this.grpArtCustom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpArtCustom.TabIndex = 1;
            this.tabPageArtmul.Controls.Add(this.grpArtCustom);

            this.lblArtCustomHint.Text = "条目数（十进制或十六进制 0x...）：";
            this.lblArtCustomHint.Location = new System.Drawing.Point(12, 25);
            this.lblArtCustomHint.AutoSize = true;
            this.lblArtCustomHint.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblArtCustomHint.TabIndex = 0;
            this.grpArtCustom.Controls.Add(this.lblArtCustomHint);

            this.tbxNewIndex.Text = "65500";
            this.tbxNewIndex.Location = new System.Drawing.Point(12, 46);
            this.tbxNewIndex.Size = new System.Drawing.Size(150, 23);
            this.tbxNewIndex.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbxNewIndex.TabIndex = 1;
            this.grpArtCustom.Controls.Add(this.tbxNewIndex);

            this.Button2.Text = "创建";
            this.Button2.Location = new System.Drawing.Point(12, 76);
            this.Button2.Size = new System.Drawing.Size(150, 28);
            this.Button2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Button2.TabIndex = 2;
            this.Button2.Click += new System.EventHandler(this.BtnCreateNewArtidx);
            this.grpArtCustom.Controls.Add(this.Button2);

            this.lblOldVersionHint.Text = "旧版格式（2003 风格）：";
            this.lblOldVersionHint.Location = new System.Drawing.Point(12, 112);
            this.lblOldVersionHint.AutoSize = true;
            this.lblOldVersionHint.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblOldVersionHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblOldVersionHint.TabIndex = 3;
            this.grpArtCustom.Controls.Add(this.lblOldVersionHint);

            this.BtCreateOldVersionArtidx.Text = "旧版 2003 变体";
            this.BtCreateOldVersionArtidx.Location = new System.Drawing.Point(12, 128);
            this.BtCreateOldVersionArtidx.Size = new System.Drawing.Size(150, 22);
            this.BtCreateOldVersionArtidx.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.BtCreateOldVersionArtidx.TabIndex = 4;
            this.BtCreateOldVersionArtidx.Click += new System.EventHandler(this.BtnCreateOldVersionArtidx);
            this.grpArtCustom.Controls.Add(this.BtCreateOldVersionArtidx);

            this.grpArtSplit.Text = "定义物品部分 + 地形部分的数量";
            this.grpArtSplit.Location = new System.Drawing.Point(315, 173);
            this.grpArtSplit.Size = new System.Drawing.Size(390, 140);
            this.grpArtSplit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpArtSplit.TabIndex = 2;
            this.tabPageArtmul.Controls.Add(this.grpArtSplit);

            this.lblArtSplitHint.Text = "条件：物品数 + 地形数 = 索引总数";
            this.lblArtSplitHint.Location = new System.Drawing.Point(12, 22);
            this.lblArtSplitHint.AutoSize = true;
            this.lblArtSplitHint.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblArtSplitHint.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblArtSplitHint.TabIndex = 0;
            this.grpArtSplit.Controls.Add(this.lblArtSplitHint);

            this.lblArtSplitTotalLbl.Text = "索引总数：";
            this.lblArtSplitTotalLbl.Location = new System.Drawing.Point(12, 48);
            this.lblArtSplitTotalLbl.AutoSize = true;
            this.lblArtSplitTotalLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblArtSplitTotalLbl.TabIndex = 1;
            this.grpArtSplit.Controls.Add(this.lblArtSplitTotalLbl);

            this.tbxNewIndex2.Text = "100000";
            this.tbxNewIndex2.Location = new System.Drawing.Point(12, 66);
            this.tbxNewIndex2.Size = new System.Drawing.Size(105, 23);
            this.tbxNewIndex2.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbxNewIndex2.TabIndex = 2;
            this.grpArtSplit.Controls.Add(this.tbxNewIndex2);

            this.lbArtsCount.Text = "物品数：";
            this.lbArtsCount.Location = new System.Drawing.Point(125, 48);
            this.lbArtsCount.AutoSize = true;
            this.lbArtsCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbArtsCount.TabIndex = 3;
            this.grpArtSplit.Controls.Add(this.lbArtsCount);

            this.tbxArtsCount.Text = "70000";
            this.tbxArtsCount.Location = new System.Drawing.Point(125, 66);
            this.tbxArtsCount.Size = new System.Drawing.Size(105, 23);
            this.tbxArtsCount.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbxArtsCount.TabIndex = 4;
            this.grpArtSplit.Controls.Add(this.tbxArtsCount);

            this.lbLandTilesCount.Text = "地形数：";
            this.lbLandTilesCount.Location = new System.Drawing.Point(240, 48);
            this.lbLandTilesCount.AutoSize = true;
            this.lbLandTilesCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbLandTilesCount.TabIndex = 5;
            this.grpArtSplit.Controls.Add(this.lbLandTilesCount);

            this.tbxLandTilesCount.Text = "30000";
            this.tbxLandTilesCount.Location = new System.Drawing.Point(240, 66);
            this.tbxLandTilesCount.Size = new System.Drawing.Size(105, 23);
            this.tbxLandTilesCount.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbxLandTilesCount.TabIndex = 6;
            this.grpArtSplit.Controls.Add(this.tbxLandTilesCount);

            this.Button3.Text = "创建";
            this.Button3.Location = new System.Drawing.Point(12, 100);
            this.Button3.Size = new System.Drawing.Size(150, 28);
            this.Button3.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Button3.TabIndex = 7;
            this.Button3.Click += new System.EventHandler(this.BtnCreateNewArtidx2);
            this.grpArtSplit.Controls.Add(this.Button3);

            this.grpArtRead.Text = "读取 Artidx.mul";
            this.grpArtRead.Location = new System.Drawing.Point(615, 10);
            this.grpArtRead.Size = new System.Drawing.Size(320, 130);
            this.grpArtRead.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpArtRead.TabIndex = 3;
            this.tabPageArtmul.Controls.Add(this.grpArtRead);

            this.lblArtReadHint.Text = "选择文件，信息将显示在日志中：";
            this.lblArtReadHint.Location = new System.Drawing.Point(12, 22);
            this.lblArtReadHint.AutoSize = true;
            this.lblArtReadHint.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblArtReadHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblArtReadHint.TabIndex = 0;
            this.grpArtRead.Controls.Add(this.lblArtReadHint);

            this.ReadArtmul.Text = "摘要";
            this.ReadArtmul.Location = new System.Drawing.Point(12, 42);
            this.ReadArtmul.Size = new System.Drawing.Size(290, 28);
            this.ReadArtmul.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ReadArtmul.TabIndex = 1;
            this.ReadArtmul.Click += new System.EventHandler(this.BtnReadArtIdx_Click);
            this.grpArtRead.Controls.Add(this.ReadArtmul);

            this.ReadArtmul2.Text = "详细列表（300 行）";
            this.ReadArtmul2.Location = new System.Drawing.Point(12, 76);
            this.ReadArtmul2.Size = new System.Drawing.Size(290, 28);
            this.ReadArtmul2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ReadArtmul2.TabIndex = 2;
            this.ReadArtmul2.Click += new System.EventHandler(this.ReadArtmul2_Click);
            this.grpArtRead.Controls.Add(this.ReadArtmul2);

            this.lblIndexCount.Text = "-";
            this.lblIndexCount.Location = new System.Drawing.Point(615, 148);
            this.lblIndexCount.AutoSize = true;
            this.lblIndexCount.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblIndexCount.ForeColor = System.Drawing.Color.Navy;
            this.lblIndexCount.TabIndex = 4;
            this.tabPageArtmul.Controls.Add(this.lblIndexCount);

            this.grpArtLog.Text = "日志 / 输出";
            this.grpArtLog.Location = new System.Drawing.Point(10, 322);
            this.grpArtLog.Size = new System.Drawing.Size(926, 280);
            this.grpArtLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpArtLog.TabIndex = 5;
            this.tabPageArtmul.Controls.Add(this.grpArtLog);

            this.infoARTIDXMULID.Location = new System.Drawing.Point(12, 24);
            this.infoARTIDXMULID.Size = new System.Drawing.Size(898, 245);
            this.infoARTIDXMULID.Multiline = true;
            this.infoARTIDXMULID.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.infoARTIDXMULID.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.infoARTIDXMULID.ReadOnly = true;
            this.infoARTIDXMULID.TabIndex = 0;
            this.grpArtLog.Controls.Add(this.infoARTIDXMULID);

            // ════════════════════════════════════════════════════════════════
            // 选项卡10 – 声音
            // ════════════════════════════════════════════════════════════════
            this.tabPageSound.Text = "声音";
            this.tabPageSound.Size = new System.Drawing.Size(946, 638);
            this.tabPageSound.TabIndex = 9;
            this.tabPageSound.UseVisualStyleBackColor = true;

            this.grpSoundConfig.Text = "配置 - SoundIdx.mul + Sound.mul";
            this.grpSoundConfig.Location = new System.Drawing.Point(10, 10);
            this.grpSoundConfig.Size = new System.Drawing.Size(920, 115);
            this.grpSoundConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSoundConfig.TabIndex = 0;
            this.tabPageSound.Controls.Add(this.grpSoundConfig);

            this.lblSoundCountLbl.Text = "声音槽位数：";
            this.lblSoundCountLbl.Location = new System.Drawing.Point(12, 35);
            this.lblSoundCountLbl.AutoSize = true;
            this.lblSoundCountLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSoundCountLbl.TabIndex = 0;
            this.grpSoundConfig.Controls.Add(this.lblSoundCountLbl);

            this.SoundIDXMul.Text = "4095";
            this.SoundIDXMul.Location = new System.Drawing.Point(165, 32);
            this.SoundIDXMul.Size = new System.Drawing.Size(110, 23);
            this.SoundIDXMul.Font = new System.Drawing.Font("Consolas", 10F);
            this.SoundIDXMul.TabIndex = 1;
            this.grpSoundConfig.Controls.Add(this.SoundIDXMul);

            this.lblSoundCountHint.Text = "标准 UO：4095 个槽位\r\n每个槽位 = 12 字节索引 + 1024 字节占位音效";
            this.lblSoundCountHint.Location = new System.Drawing.Point(290, 28);
            this.lblSoundCountHint.AutoSize = true;
            this.lblSoundCountHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblSoundCountHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblSoundCountHint.TabIndex = 2;
            this.grpSoundConfig.Controls.Add(this.lblSoundCountHint);

            this.grpSoundActions.Text = "操作";
            this.grpSoundActions.Location = new System.Drawing.Point(10, 133);
            this.grpSoundActions.Size = new System.Drawing.Size(920, 70);
            this.grpSoundActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSoundActions.TabIndex = 1;
            this.tabPageSound.Controls.Add(this.grpSoundActions);

            this.CreateOrgSoundMul.Text = "创建 SoundIdx.mul + Sound.mul";
            this.CreateOrgSoundMul.Location = new System.Drawing.Point(12, 22);
            this.CreateOrgSoundMul.Size = new System.Drawing.Size(300, 30);
            this.CreateOrgSoundMul.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.CreateOrgSoundMul.TabIndex = 0;
            this.CreateOrgSoundMul.Click += new System.EventHandler(this.CreateOrgSoundMul_Click);
            this.grpSoundActions.Controls.Add(this.CreateOrgSoundMul);

            this.ReadIndexSize.Text = "统计条目数";
            this.ReadIndexSize.Location = new System.Drawing.Point(325, 22);
            this.ReadIndexSize.Size = new System.Drawing.Size(160, 30);
            this.ReadIndexSize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ReadIndexSize.TabIndex = 1;
            this.ReadIndexSize.Click += new System.EventHandler(this.ReadIndexSize_Click);
            this.grpSoundActions.Controls.Add(this.ReadIndexSize);

            this.grpSoundOutput.Text = "输出";
            this.grpSoundOutput.Location = new System.Drawing.Point(10, 211);
            this.grpSoundOutput.Size = new System.Drawing.Size(920, 60);
            this.grpSoundOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSoundOutput.TabIndex = 2;
            this.tabPageSound.Controls.Add(this.grpSoundOutput);

            this.IndexSizeLabel.Text = "-";
            this.IndexSizeLabel.Location = new System.Drawing.Point(12, 22);
            this.IndexSizeLabel.AutoSize = true;
            this.IndexSizeLabel.Font = new System.Drawing.Font("Consolas", 9F);
            this.IndexSizeLabel.ForeColor = System.Drawing.Color.DarkGreen;
            this.IndexSizeLabel.TabIndex = 0;
            this.grpSoundOutput.Controls.Add(this.IndexSizeLabel);

            // ════════════════════════════════════════════════════════════════
            // 选项卡11 – Gump
            // ════════════════════════════════════════════════════════════════
            this.tabPageGump.Text = "Gump";
            this.tabPageGump.Size = new System.Drawing.Size(946, 638);
            this.tabPageGump.TabIndex = 10;
            this.tabPageGump.UseVisualStyleBackColor = true;

            this.grpGumpConfig.Text = "配置 - GUMPIDX.MUL + GUMPART.MUL";
            this.grpGumpConfig.Location = new System.Drawing.Point(10, 10);
            this.grpGumpConfig.Size = new System.Drawing.Size(920, 115);
            this.grpGumpConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpGumpConfig.TabIndex = 0;
            this.tabPageGump.Controls.Add(this.grpGumpConfig);

            this.lblGumpCountLbl.Text = "Gump 条目数：";
            this.lblGumpCountLbl.Location = new System.Drawing.Point(12, 35);
            this.lblGumpCountLbl.AutoSize = true;
            this.lblGumpCountLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGumpCountLbl.TabIndex = 0;
            this.grpGumpConfig.Controls.Add(this.lblGumpCountLbl);

            this.IndexSizeTextBox.Text = "65535";
            this.IndexSizeTextBox.Location = new System.Drawing.Point(185, 32);
            this.IndexSizeTextBox.Size = new System.Drawing.Size(110, 23);
            this.IndexSizeTextBox.Font = new System.Drawing.Font("Consolas", 10F);
            this.IndexSizeTextBox.TabIndex = 1;
            this.grpGumpConfig.Controls.Add(this.IndexSizeTextBox);

            this.lblGumpCountHint.Text = "标准 UO：65535 个条目\r\n每个条目在 GUMPIDX.MUL 中占用 12 字节。";
            this.lblGumpCountHint.Location = new System.Drawing.Point(310, 28);
            this.lblGumpCountHint.AutoSize = true;
            this.lblGumpCountHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblGumpCountHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblGumpCountHint.TabIndex = 2;
            this.grpGumpConfig.Controls.Add(this.lblGumpCountHint);

            this.grpGumpActions.Text = "操作";
            this.grpGumpActions.Location = new System.Drawing.Point(10, 133);
            this.grpGumpActions.Size = new System.Drawing.Size(920, 70);
            this.grpGumpActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpGumpActions.TabIndex = 1;
            this.tabPageGump.Controls.Add(this.grpGumpActions);

            this.CreateGumpButton.Text = "创建 GUMPIDX.MUL + GUMPART.MUL";
            this.CreateGumpButton.Location = new System.Drawing.Point(12, 22);
            this.CreateGumpButton.Size = new System.Drawing.Size(330, 30);
            this.CreateGumpButton.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.CreateGumpButton.TabIndex = 0;
            this.CreateGumpButton.Click += new System.EventHandler(this.CreateGumpButton_Click);
            this.grpGumpActions.Controls.Add(this.CreateGumpButton);

            this.ReadGumpButton.Text = "统计条目数";
            this.ReadGumpButton.Location = new System.Drawing.Point(355, 22);
            this.ReadGumpButton.Size = new System.Drawing.Size(160, 30);
            this.ReadGumpButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ReadGumpButton.TabIndex = 1;
            this.ReadGumpButton.Click += new System.EventHandler(this.ReadGumpButton_Click);
            this.grpGumpActions.Controls.Add(this.ReadGumpButton);

            this.grpGumpOutput.Text = "输出";
            this.grpGumpOutput.Location = new System.Drawing.Point(10, 211);
            this.grpGumpOutput.Size = new System.Drawing.Size(920, 60);
            this.grpGumpOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpGumpOutput.TabIndex = 2;
            this.tabPageGump.Controls.Add(this.grpGumpOutput);

            this.gumpLabel.Text = "-";
            this.gumpLabel.Location = new System.Drawing.Point(12, 22);
            this.gumpLabel.AutoSize = true;
            this.gumpLabel.Font = new System.Drawing.Font("Consolas", 9F);
            this.gumpLabel.ForeColor = System.Drawing.Color.DarkGreen;
            this.gumpLabel.TabIndex = 0;
            this.grpGumpOutput.Controls.Add(this.gumpLabel);

            // ════════════════════════════════════════════════════════════════
            // 选项卡12 – Hues
            // ════════════════════════════════════════════════════════════════
            this.tabPageHues.Text = "色调";
            this.tabPageHues.Size = new System.Drawing.Size(946, 638);
            this.tabPageHues.TabIndex = 11;
            this.tabPageHues.UseVisualStyleBackColor = true;

            this.grpHuesActions.Text = "操作 - hues.mul（3000 个 x 88 字节的色板）";
            this.grpHuesActions.Location = new System.Drawing.Point(10, 10);
            this.grpHuesActions.Size = new System.Drawing.Size(920, 75);
            this.grpHuesActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpHuesActions.TabIndex = 0;
            this.tabPageHues.Controls.Add(this.grpHuesActions);

            this.BtnCreateHues.Text = "创建空的 hues.mul（3000 个条目）";
            this.BtnCreateHues.Location = new System.Drawing.Point(12, 28);
            this.BtnCreateHues.Size = new System.Drawing.Size(320, 30);
            this.BtnCreateHues.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtnCreateHues.TabIndex = 0;
            this.BtnCreateHues.Click += new System.EventHandler(this.BtnCreateHues_Click);
            this.grpHuesActions.Controls.Add(this.BtnCreateHues);

            this.BtnReadHues.Text = "读取 hues.mul";
            this.BtnReadHues.Location = new System.Drawing.Point(345, 28);
            this.BtnReadHues.Size = new System.Drawing.Size(180, 30);
            this.BtnReadHues.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnReadHues.TabIndex = 1;
            this.BtnReadHues.Click += new System.EventHandler(this.BtnReadHues_Click);
            this.grpHuesActions.Controls.Add(this.BtnReadHues);

            this.grpHuesOutput.Text = "输出";
            this.grpHuesOutput.Location = new System.Drawing.Point(10, 93);
            this.grpHuesOutput.Size = new System.Drawing.Size(920, 510);
            this.grpHuesOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpHuesOutput.TabIndex = 1;
            this.tabPageHues.Controls.Add(this.grpHuesOutput);

            this.lblHuesOutput.Text = "-";
            this.lblHuesOutput.Location = new System.Drawing.Point(12, 22);
            this.lblHuesOutput.Size = new System.Drawing.Size(890, 475);
            this.lblHuesOutput.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblHuesOutput.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblHuesOutput.TabIndex = 0;
            this.grpHuesOutput.Controls.Add(this.lblHuesOutput);

            // ════════════════════════════════════════════════════════════════
            // 选项卡13 – 地图 / 静态
            // ════════════════════════════════════════════════════════════════
            this.tabPageMap.Text = "地图/静态";
            this.tabPageMap.Size = new System.Drawing.Size(946, 638);
            this.tabPageMap.TabIndex = 12;
            this.tabPageMap.UseVisualStyleBackColor = true;

            this.grpMapConfig.Text = "地图配置";
            this.grpMapConfig.Location = new System.Drawing.Point(10, 10);
            this.grpMapConfig.Size = new System.Drawing.Size(920, 175);
            this.grpMapConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpMapConfig.TabIndex = 0;
            this.tabPageMap.Controls.Add(this.grpMapConfig);

            this.lblMapSizeComboLbl.Text = "预设：";
            this.lblMapSizeComboLbl.Location = new System.Drawing.Point(12, 30);
            this.lblMapSizeComboLbl.AutoSize = true;
            this.lblMapSizeComboLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMapSizeComboLbl.TabIndex = 0;
            this.grpMapConfig.Controls.Add(this.lblMapSizeComboLbl);

            this.comboMapSize.Location = new System.Drawing.Point(110, 27);
            this.comboMapSize.Size = new System.Drawing.Size(340, 23);
            this.comboMapSize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboMapSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMapSize.TabIndex = 1;
            this.comboMapSize.SelectedIndexChanged += new System.EventHandler(this.ComboMapSize_SelectedIndexChanged);
            this.grpMapConfig.Controls.Add(this.comboMapSize);

            this.lblMapWidthLbl.Text = "宽度（图块）：";
            this.lblMapWidthLbl.Location = new System.Drawing.Point(12, 65);
            this.lblMapWidthLbl.AutoSize = true;
            this.lblMapWidthLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMapWidthLbl.TabIndex = 2;
            this.grpMapConfig.Controls.Add(this.lblMapWidthLbl);

            this.tbMapWidth.Text = "7168";
            this.tbMapWidth.Location = new System.Drawing.Point(110, 62);
            this.tbMapWidth.Size = new System.Drawing.Size(110, 23);
            this.tbMapWidth.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbMapWidth.TabIndex = 3;
            this.tbMapWidth.TextChanged += new System.EventHandler(this.TbMapWidth_TextChanged);
            this.grpMapConfig.Controls.Add(this.tbMapWidth);

            this.lblMapHeightLbl.Text = "高度（图块）：";
            this.lblMapHeightLbl.Location = new System.Drawing.Point(240, 65);
            this.lblMapHeightLbl.AutoSize = true;
            this.lblMapHeightLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMapHeightLbl.TabIndex = 4;
            this.grpMapConfig.Controls.Add(this.lblMapHeightLbl);

            this.tbMapHeight.Text = "4096";
            this.tbMapHeight.Location = new System.Drawing.Point(345, 62);
            this.tbMapHeight.Size = new System.Drawing.Size(110, 23);
            this.tbMapHeight.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbMapHeight.TabIndex = 5;
            this.tbMapHeight.TextChanged += new System.EventHandler(this.TbMapWidth_TextChanged);
            this.grpMapConfig.Controls.Add(this.tbMapHeight);

            this.lblMapIndexLbl.Text = "地图索引：";
            this.lblMapIndexLbl.Location = new System.Drawing.Point(480, 65);
            this.lblMapIndexLbl.AutoSize = true;
            this.lblMapIndexLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMapIndexLbl.TabIndex = 6;
            this.grpMapConfig.Controls.Add(this.lblMapIndexLbl);

            this.tbMapIndex.Text = "0";
            this.tbMapIndex.Location = new System.Drawing.Point(580, 62);
            this.tbMapIndex.Size = new System.Drawing.Size(60, 23);
            this.tbMapIndex.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbMapIndex.TabIndex = 7;
            this.grpMapConfig.Controls.Add(this.tbMapIndex);

            this.lblMapSizeInfo.Text = "-";
            this.lblMapSizeInfo.Location = new System.Drawing.Point(12, 100);
            this.lblMapSizeInfo.Size = new System.Drawing.Size(890, 60);
            this.lblMapSizeInfo.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblMapSizeInfo.ForeColor = System.Drawing.Color.Navy;
            this.lblMapSizeInfo.TabIndex = 8;
            this.grpMapConfig.Controls.Add(this.lblMapSizeInfo);

            this.grpMapActions.Text = "操作";
            this.grpMapActions.Location = new System.Drawing.Point(10, 193);
            this.grpMapActions.Size = new System.Drawing.Size(920, 75);
            this.grpMapActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpMapActions.TabIndex = 1;
            this.tabPageMap.Controls.Add(this.grpMapActions);

            this.BtnCreateMap.Text = "仅创建 map*.mul";
            this.BtnCreateMap.Location = new System.Drawing.Point(12, 28);
            this.BtnCreateMap.Size = new System.Drawing.Size(220, 30);
            this.BtnCreateMap.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtnCreateMap.TabIndex = 0;
            this.BtnCreateMap.Click += new System.EventHandler(this.BtnCreateMap_Click);
            this.grpMapActions.Controls.Add(this.BtnCreateMap);

            this.BtnCreateStatics.Text = "仅创建 statics*.mul + staidx*.mul";
            this.BtnCreateStatics.Location = new System.Drawing.Point(245, 28);
            this.BtnCreateStatics.Size = new System.Drawing.Size(255, 30);
            this.BtnCreateStatics.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCreateStatics.TabIndex = 1;
            this.BtnCreateStatics.Click += new System.EventHandler(this.BtnCreateStatics_Click);
            this.grpMapActions.Controls.Add(this.BtnCreateStatics);

            this.BtnCreateMapAndStatics.Text = "同时创建地图 + 静态文件";
            this.BtnCreateMapAndStatics.Location = new System.Drawing.Point(515, 28);
            this.BtnCreateMapAndStatics.Size = new System.Drawing.Size(295, 30);
            this.BtnCreateMapAndStatics.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCreateMapAndStatics.TabIndex = 2;
            this.BtnCreateMapAndStatics.Click += new System.EventHandler(this.BtnCreateMapAndStatics_Click);
            this.grpMapActions.Controls.Add(this.BtnCreateMapAndStatics);

            this.grpMapOutput.Text = "输出";
            this.grpMapOutput.Location = new System.Drawing.Point(10, 276);
            this.grpMapOutput.Size = new System.Drawing.Size(920, 330);
            this.grpMapOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpMapOutput.TabIndex = 2;
            this.tabPageMap.Controls.Add(this.grpMapOutput);

            this.lblMapOutput.Text = "-";
            this.lblMapOutput.Location = new System.Drawing.Point(12, 22);
            this.lblMapOutput.Size = new System.Drawing.Size(890, 295);
            this.lblMapOutput.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblMapOutput.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblMapOutput.TabIndex = 0;
            this.grpMapOutput.Controls.Add(this.lblMapOutput);

            // ════════════════════════════════════════════════════════════════
            // 选项卡14 – Multi
            // ════════════════════════════════════════════════════════════════
            this.tabPageMulti.Text = "多重";
            this.tabPageMulti.Size = new System.Drawing.Size(946, 638);
            this.tabPageMulti.TabIndex = 13;
            this.tabPageMulti.UseVisualStyleBackColor = true;

            this.grpMultiConfig.Text = "配置 - multi.mul + multi.idx";
            this.grpMultiConfig.Location = new System.Drawing.Point(10, 10);
            this.grpMultiConfig.Size = new System.Drawing.Size(920, 110);
            this.grpMultiConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpMultiConfig.TabIndex = 0;
            this.tabPageMulti.Controls.Add(this.grpMultiConfig);

            this.lblMultiCountLbl.Text = "Multi 条目数：";
            this.lblMultiCountLbl.Location = new System.Drawing.Point(12, 30);
            this.lblMultiCountLbl.AutoSize = true;
            this.lblMultiCountLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMultiCountLbl.TabIndex = 0;
            this.grpMultiConfig.Controls.Add(this.lblMultiCountLbl);

            this.tbMultiCount.Text = "5000";
            this.tbMultiCount.Location = new System.Drawing.Point(190, 27);
            this.tbMultiCount.Size = new System.Drawing.Size(110, 23);
            this.tbMultiCount.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbMultiCount.TabIndex = 1;
            this.grpMultiConfig.Controls.Add(this.tbMultiCount);

            this.lblMultiIndexLbl.Text = "读取索引：";
            this.lblMultiIndexLbl.Location = new System.Drawing.Point(12, 65);
            this.lblMultiIndexLbl.AutoSize = true;
            this.lblMultiIndexLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMultiIndexLbl.TabIndex = 2;
            this.grpMultiConfig.Controls.Add(this.lblMultiIndexLbl);

            this.tbMultiIndex.Text = "0";
            this.tbMultiIndex.Location = new System.Drawing.Point(100, 62);
            this.tbMultiIndex.Size = new System.Drawing.Size(90, 23);
            this.tbMultiIndex.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbMultiIndex.TabIndex = 3;
            this.grpMultiConfig.Controls.Add(this.tbMultiIndex);

            this.checkBoxMultiHS.Text = "HighSeas 格式（每个图块 16 字节而非 12）";
            this.checkBoxMultiHS.Location = new System.Drawing.Point(210, 63);
            this.checkBoxMultiHS.AutoSize = true;
            this.checkBoxMultiHS.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxMultiHS.TabIndex = 4;
            this.grpMultiConfig.Controls.Add(this.checkBoxMultiHS);

            this.grpMultiActions.Text = "操作";
            this.grpMultiActions.Location = new System.Drawing.Point(10, 128);
            this.grpMultiActions.Size = new System.Drawing.Size(920, 75);
            this.grpMultiActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpMultiActions.TabIndex = 1;
            this.tabPageMulti.Controls.Add(this.grpMultiActions);

            this.BtnCreateMulti.Text = "创建 multi.mul + multi.idx";
            this.BtnCreateMulti.Location = new System.Drawing.Point(12, 28);
            this.BtnCreateMulti.Size = new System.Drawing.Size(280, 30);
            this.BtnCreateMulti.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtnCreateMulti.TabIndex = 0;
            this.BtnCreateMulti.Click += new System.EventHandler(this.BtnCreateMulti_Click);
            this.grpMultiActions.Controls.Add(this.BtnCreateMulti);

            this.BtnReadMulti.Text = "读取 Multi 条目";
            this.BtnReadMulti.Location = new System.Drawing.Point(305, 28);
            this.BtnReadMulti.Size = new System.Drawing.Size(200, 30);
            this.BtnReadMulti.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnReadMulti.TabIndex = 1;
            this.BtnReadMulti.Click += new System.EventHandler(this.BtnReadMulti_Click);
            this.grpMultiActions.Controls.Add(this.BtnReadMulti);

            this.grpMultiOutput.Text = "输出";
            this.grpMultiOutput.Location = new System.Drawing.Point(10, 211);
            this.grpMultiOutput.Size = new System.Drawing.Size(920, 400);
            this.grpMultiOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpMultiOutput.TabIndex = 2;
            this.tabPageMulti.Controls.Add(this.grpMultiOutput);

            this.lblMultiOutput.Text = "-";
            this.lblMultiOutput.Location = new System.Drawing.Point(12, 22);
            this.lblMultiOutput.Size = new System.Drawing.Size(890, 365);
            this.lblMultiOutput.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblMultiOutput.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblMultiOutput.TabIndex = 0;
            this.grpMultiOutput.Controls.Add(this.lblMultiOutput);

            // ════════════════════════════════════════════════════════════════
            // 选项卡15 – 技能
            // ════════════════════════════════════════════════════════════════
            this.tabPageSkills.Text = "技能";
            this.tabPageSkills.Size = new System.Drawing.Size(946, 638);
            this.tabPageSkills.TabIndex = 14;
            this.tabPageSkills.UseVisualStyleBackColor = true;

            this.grpSkillsConfig.Text = "配置 - skills.mul + skills.idx";
            this.grpSkillsConfig.Location = new System.Drawing.Point(10, 10);
            this.grpSkillsConfig.Size = new System.Drawing.Size(920, 75);
            this.grpSkillsConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSkillsConfig.TabIndex = 0;
            this.tabPageSkills.Controls.Add(this.grpSkillsConfig);

            this.lblSkillCountLbl.Text = "技能数量（空版本）：";
            this.lblSkillCountLbl.Location = new System.Drawing.Point(12, 32);
            this.lblSkillCountLbl.AutoSize = true;
            this.lblSkillCountLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSkillCountLbl.TabIndex = 0;
            this.grpSkillsConfig.Controls.Add(this.lblSkillCountLbl);

            this.tbSkillCount.Text = "58";
            this.tbSkillCount.Location = new System.Drawing.Point(245, 29);
            this.tbSkillCount.Size = new System.Drawing.Size(90, 23);
            this.tbSkillCount.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbSkillCount.TabIndex = 1;
            this.grpSkillsConfig.Controls.Add(this.tbSkillCount);

            this.grpSkillsActions.Text = "操作";
            this.grpSkillsActions.Location = new System.Drawing.Point(10, 93);
            this.grpSkillsActions.Size = new System.Drawing.Size(920, 75);
            this.grpSkillsActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSkillsActions.TabIndex = 1;
            this.tabPageSkills.Controls.Add(this.grpSkillsActions);

            this.BtnCreateDefaultSkills.Text = "创建标准技能（58 个 UO 技能）";
            this.BtnCreateDefaultSkills.Location = new System.Drawing.Point(12, 28);
            this.BtnCreateDefaultSkills.Size = new System.Drawing.Size(310, 30);
            this.BtnCreateDefaultSkills.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtnCreateDefaultSkills.TabIndex = 0;
            this.BtnCreateDefaultSkills.Click += new System.EventHandler(this.BtnCreateDefaultSkills_Click);
            this.grpSkillsActions.Controls.Add(this.BtnCreateDefaultSkills);

            this.BtnCreateEmptySkills.Text = "创建空技能文件";
            this.BtnCreateEmptySkills.Location = new System.Drawing.Point(335, 28);
            this.BtnCreateEmptySkills.Size = new System.Drawing.Size(200, 30);
            this.BtnCreateEmptySkills.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCreateEmptySkills.TabIndex = 1;
            this.BtnCreateEmptySkills.Click += new System.EventHandler(this.BtnCreateEmptySkills_Click);
            this.grpSkillsActions.Controls.Add(this.BtnCreateEmptySkills);

            this.BtnReadSkills.Text = "读取技能文件";
            this.BtnReadSkills.Location = new System.Drawing.Point(548, 28);
            this.BtnReadSkills.Size = new System.Drawing.Size(160, 30);
            this.BtnReadSkills.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnReadSkills.TabIndex = 2;
            this.BtnReadSkills.Click += new System.EventHandler(this.BtnReadSkills_Click);
            this.grpSkillsActions.Controls.Add(this.BtnReadSkills);

            this.grpSkillsOutput.Text = "输出";
            this.grpSkillsOutput.Location = new System.Drawing.Point(10, 176);
            this.grpSkillsOutput.Size = new System.Drawing.Size(920, 435);
            this.grpSkillsOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSkillsOutput.TabIndex = 2;
            this.tabPageSkills.Controls.Add(this.grpSkillsOutput);

            this.lblSkillsOutput.Text = "-";
            this.lblSkillsOutput.Location = new System.Drawing.Point(12, 22);
            this.lblSkillsOutput.AutoSize = true;
            this.lblSkillsOutput.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblSkillsOutput.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblSkillsOutput.TabIndex = 0;
            this.grpSkillsOutput.Controls.Add(this.lblSkillsOutput);

            this.textBoxSkillsInfo.Location = new System.Drawing.Point(12, 50);
            this.textBoxSkillsInfo.Size = new System.Drawing.Size(890, 370);
            this.textBoxSkillsInfo.Multiline = true;
            this.textBoxSkillsInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSkillsInfo.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.textBoxSkillsInfo.ReadOnly = true;
            this.textBoxSkillsInfo.TabIndex = 1;
            this.grpSkillsOutput.Controls.Add(this.textBoxSkillsInfo);

            // ════════════════════════════════════════════════════════════════
            // 选项卡16 – 验证器
            // ════════════════════════════════════════════════════════════════
            this.tabPageValidator.Text = "验证器";
            this.tabPageValidator.Size = new System.Drawing.Size(946, 638);
            this.tabPageValidator.TabIndex = 15;
            this.tabPageValidator.UseVisualStyleBackColor = true;

            this.grpValidatorActions.Text = "IDX <-> MUL 一致性检查";
            this.grpValidatorActions.Location = new System.Drawing.Point(10, 10);
            this.grpValidatorActions.Size = new System.Drawing.Size(920, 80);
            this.grpValidatorActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpValidatorActions.TabIndex = 0;
            this.tabPageValidator.Controls.Add(this.grpValidatorActions);

            this.BtnValidate.Text = "验证 IDX + MUL";
            this.BtnValidate.Location = new System.Drawing.Point(12, 28);
            this.BtnValidate.Size = new System.Drawing.Size(220, 30);
            this.BtnValidate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtnValidate.TabIndex = 0;
            this.BtnValidate.Click += new System.EventHandler(this.BtnValidate_Click);
            this.grpValidatorActions.Controls.Add(this.BtnValidate);

            this.BtnCompareDirectories.Text = "比较两个目录";
            this.BtnCompareDirectories.Location = new System.Drawing.Point(245, 28);
            this.BtnCompareDirectories.Size = new System.Drawing.Size(270, 30);
            this.BtnCompareDirectories.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnCompareDirectories.TabIndex = 1;
            this.BtnCompareDirectories.Click += new System.EventHandler(this.BtnCompareDirectories_Click);
            this.grpValidatorActions.Controls.Add(this.BtnCompareDirectories);

            this.lblValidatorStatus.Text = "-";
            this.lblValidatorStatus.Location = new System.Drawing.Point(530, 33);
            this.lblValidatorStatus.AutoSize = true;
            this.lblValidatorStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblValidatorStatus.ForeColor = System.Drawing.Color.Navy;
            this.lblValidatorStatus.TabIndex = 2;
            this.grpValidatorActions.Controls.Add(this.lblValidatorStatus);

            this.grpValidatorOutput.Text = "结果";
            this.grpValidatorOutput.Location = new System.Drawing.Point(10, 98);
            this.grpValidatorOutput.Size = new System.Drawing.Size(920, 512);
            this.grpValidatorOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpValidatorOutput.TabIndex = 1;
            this.tabPageValidator.Controls.Add(this.grpValidatorOutput);

            this.textBoxValidatorOutput.Location = new System.Drawing.Point(12, 24);
            this.textBoxValidatorOutput.Size = new System.Drawing.Size(890, 478);
            this.textBoxValidatorOutput.Multiline = true;
            this.textBoxValidatorOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxValidatorOutput.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.textBoxValidatorOutput.ReadOnly = true;
            this.textBoxValidatorOutput.TabIndex = 0;
            this.grpValidatorOutput.Controls.Add(this.textBoxValidatorOutput);

            // ════════════════════════════════════════════════════════════════
            // 选项卡17 – IDX 修补器
            // ════════════════════════════════════════════════════════════════
            this.tabPageIdxPatcher.Text = "IDX 修补器";
            this.tabPageIdxPatcher.Size = new System.Drawing.Size(946, 638);
            this.tabPageIdxPatcher.TabIndex = 16;
            this.tabPageIdxPatcher.UseVisualStyleBackColor = true;

            this.grpPatcherFile.Text = "IDX 文件";
            this.grpPatcherFile.Location = new System.Drawing.Point(10, 10);
            this.grpPatcherFile.Size = new System.Drawing.Size(920, 65);
            this.grpPatcherFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPatcherFile.TabIndex = 0;
            this.tabPageIdxPatcher.Controls.Add(this.grpPatcherFile);

            this.lblPatchIdxLbl.Text = "IDX 文件：";
            this.lblPatchIdxLbl.Location = new System.Drawing.Point(12, 30);
            this.lblPatchIdxLbl.AutoSize = true;
            this.lblPatchIdxLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPatchIdxLbl.TabIndex = 0;
            this.grpPatcherFile.Controls.Add(this.lblPatchIdxLbl);

            this.tbPatchIdxPath.Location = new System.Drawing.Point(85, 27);
            this.tbPatchIdxPath.Size = new System.Drawing.Size(680, 23);
            this.tbPatchIdxPath.Font = new System.Drawing.Font("Consolas", 9F);
            this.tbPatchIdxPath.TabIndex = 1;
            this.grpPatcherFile.Controls.Add(this.tbPatchIdxPath);

            this.BtnPatchBrowseIdx.Text = "...";
            this.BtnPatchBrowseIdx.Location = new System.Drawing.Point(775, 25);
            this.BtnPatchBrowseIdx.Size = new System.Drawing.Size(130, 26);
            this.BtnPatchBrowseIdx.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnPatchBrowseIdx.TabIndex = 2;
            this.BtnPatchBrowseIdx.Click += new System.EventHandler(this.BtnPatchBrowseIdx_Click);
            this.grpPatcherFile.Controls.Add(this.BtnPatchBrowseIdx);

            this.grpPatcherEdit.Text = "编辑条目";
            this.grpPatcherEdit.Location = new System.Drawing.Point(10, 83);
            this.grpPatcherEdit.Size = new System.Drawing.Size(920, 100);
            this.grpPatcherEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPatcherEdit.TabIndex = 1;
            this.tabPageIdxPatcher.Controls.Add(this.grpPatcherEdit);

            this.lblPatchIndexLbl.Text = "索引：";
            this.lblPatchIndexLbl.Location = new System.Drawing.Point(12, 30);
            this.lblPatchIndexLbl.AutoSize = true;
            this.lblPatchIndexLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPatchIndexLbl.TabIndex = 0;
            this.grpPatcherEdit.Controls.Add(this.lblPatchIndexLbl);

            this.tbPatchIndex.Text = "0";
            this.tbPatchIndex.Location = new System.Drawing.Point(60, 27);
            this.tbPatchIndex.Size = new System.Drawing.Size(90, 23);
            this.tbPatchIndex.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbPatchIndex.TabIndex = 1;
            this.grpPatcherEdit.Controls.Add(this.tbPatchIndex);

            this.lblPatchLookupLbl.Text = "查找：";
            this.lblPatchLookupLbl.Location = new System.Drawing.Point(170, 30);
            this.lblPatchLookupLbl.AutoSize = true;
            this.lblPatchLookupLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPatchLookupLbl.TabIndex = 2;
            this.grpPatcherEdit.Controls.Add(this.lblPatchLookupLbl);

            this.tbPatchLookup.Text = "0x0";
            this.tbPatchLookup.Location = new System.Drawing.Point(230, 27);
            this.tbPatchLookup.Size = new System.Drawing.Size(130, 23);
            this.tbPatchLookup.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbPatchLookup.TabIndex = 3;
            this.grpPatcherEdit.Controls.Add(this.tbPatchLookup);

            this.lblPatchSizeLbl.Text = "大小：";
            this.lblPatchSizeLbl.Location = new System.Drawing.Point(380, 30);
            this.lblPatchSizeLbl.AutoSize = true;
            this.lblPatchSizeLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPatchSizeLbl.TabIndex = 4;
            this.grpPatcherEdit.Controls.Add(this.lblPatchSizeLbl);

            this.tbPatchSize.Text = "0";
            this.tbPatchSize.Location = new System.Drawing.Point(420, 27);
            this.tbPatchSize.Size = new System.Drawing.Size(110, 23);
            this.tbPatchSize.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbPatchSize.TabIndex = 5;
            this.grpPatcherEdit.Controls.Add(this.tbPatchSize);

            this.lblPatchUnknownLbl.Text = "未知：";
            this.lblPatchUnknownLbl.Location = new System.Drawing.Point(550, 30);
            this.lblPatchUnknownLbl.AutoSize = true;
            this.lblPatchUnknownLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPatchUnknownLbl.TabIndex = 6;
            this.grpPatcherEdit.Controls.Add(this.lblPatchUnknownLbl);

            this.tbPatchUnknown.Text = "0";
            this.tbPatchUnknown.Location = new System.Drawing.Point(625, 27);
            this.tbPatchUnknown.Size = new System.Drawing.Size(100, 23);
            this.tbPatchUnknown.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbPatchUnknown.TabIndex = 7;
            this.grpPatcherEdit.Controls.Add(this.tbPatchUnknown);

            this.BtnPatchEntry.Text = "修补条目";
            this.BtnPatchEntry.Location = new System.Drawing.Point(12, 58);
            this.BtnPatchEntry.Size = new System.Drawing.Size(180, 28);
            this.BtnPatchEntry.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtnPatchEntry.TabIndex = 8;
            this.BtnPatchEntry.Click += new System.EventHandler(this.BtnPatchEntry_Click);
            this.grpPatcherEdit.Controls.Add(this.BtnPatchEntry);

            this.BtnClearEntry.Text = "清空条目";
            this.BtnClearEntry.Location = new System.Drawing.Point(205, 58);
            this.BtnClearEntry.Size = new System.Drawing.Size(160, 28);
            this.BtnClearEntry.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnClearEntry.TabIndex = 9;
            this.BtnClearEntry.Click += new System.EventHandler(this.BtnClearEntry_Click);
            this.grpPatcherEdit.Controls.Add(this.BtnClearEntry);

            this.grpPatcherRange.Text = "读取条目范围";
            this.grpPatcherRange.Location = new System.Drawing.Point(10, 191);
            this.grpPatcherRange.Size = new System.Drawing.Size(920, 75);
            this.grpPatcherRange.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPatcherRange.TabIndex = 2;
            this.tabPageIdxPatcher.Controls.Add(this.grpPatcherRange);

            this.lblPatchRangeFromLbl.Text = "起始索引：";
            this.lblPatchRangeFromLbl.Location = new System.Drawing.Point(12, 30);
            this.lblPatchRangeFromLbl.AutoSize = true;
            this.lblPatchRangeFromLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPatchRangeFromLbl.TabIndex = 0;
            this.grpPatcherRange.Controls.Add(this.lblPatchRangeFromLbl);

            this.tbPatchRangeFrom.Text = "0";
            this.tbPatchRangeFrom.Location = new System.Drawing.Point(90, 27);
            this.tbPatchRangeFrom.Size = new System.Drawing.Size(90, 23);
            this.tbPatchRangeFrom.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbPatchRangeFrom.TabIndex = 1;
            this.grpPatcherRange.Controls.Add(this.tbPatchRangeFrom);

            this.lblPatchRangeCountLbl.Text = "数量：";
            this.lblPatchRangeCountLbl.Location = new System.Drawing.Point(200, 30);
            this.lblPatchRangeCountLbl.AutoSize = true;
            this.lblPatchRangeCountLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPatchRangeCountLbl.TabIndex = 2;
            this.grpPatcherRange.Controls.Add(this.lblPatchRangeCountLbl);

            this.tbPatchRangeCount.Text = "20";
            this.tbPatchRangeCount.Location = new System.Drawing.Point(255, 27);
            this.tbPatchRangeCount.Size = new System.Drawing.Size(90, 23);
            this.tbPatchRangeCount.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbPatchRangeCount.TabIndex = 3;
            this.grpPatcherRange.Controls.Add(this.tbPatchRangeCount);

            this.BtnReadRange.Text = "显示范围";
            this.BtnReadRange.Location = new System.Drawing.Point(360, 25);
            this.BtnReadRange.Size = new System.Drawing.Size(180, 28);
            this.BtnReadRange.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnReadRange.TabIndex = 4;
            this.BtnReadRange.Click += new System.EventHandler(this.BtnReadRange_Click);
            this.grpPatcherRange.Controls.Add(this.BtnReadRange);

            this.grpPatcherOutput.Text = "输出";
            this.grpPatcherOutput.Location = new System.Drawing.Point(10, 274);
            this.grpPatcherOutput.Size = new System.Drawing.Size(920, 337);
            this.grpPatcherOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpPatcherOutput.TabIndex = 3;
            this.tabPageIdxPatcher.Controls.Add(this.grpPatcherOutput);

            this.textBoxPatcherOutput.Location = new System.Drawing.Point(12, 24);
            this.textBoxPatcherOutput.Size = new System.Drawing.Size(890, 300);
            this.textBoxPatcherOutput.Multiline = true;
            this.textBoxPatcherOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPatcherOutput.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.textBoxPatcherOutput.ReadOnly = true;
            this.textBoxPatcherOutput.TabIndex = 0;
            this.grpPatcherOutput.Controls.Add(this.textBoxPatcherOutput);

            // ════════════════════════════════════════════════════════════════
            // 选项卡18 – 批量设置
            // ════════════════════════════════════════════════════════════════
            this.tabPageBatch.Text = "批量设置";
            this.tabPageBatch.Size = new System.Drawing.Size(946, 638);
            this.tabPageBatch.TabIndex = 17;
            this.tabPageBatch.UseVisualStyleBackColor = true;

            this.grpBatchConfig.Text = "配置 - 一次性创建所有分片文件";
            this.grpBatchConfig.Location = new System.Drawing.Point(10, 10);
            this.grpBatchConfig.Size = new System.Drawing.Size(920, 280);
            this.grpBatchConfig.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpBatchConfig.TabIndex = 0;
            this.tabPageBatch.Controls.Add(this.grpBatchConfig);

            // 第1行
            this.lblBatchMapWLbl.Text = "地图宽度：";
            this.lblBatchMapWLbl.Location = new System.Drawing.Point(12, 28);
            this.lblBatchMapWLbl.AutoSize = true;
            this.lblBatchMapWLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchMapWLbl.TabIndex = 0;
            this.grpBatchConfig.Controls.Add(this.lblBatchMapWLbl);

            this.tbBatchMapW.Text = "7168";
            this.tbBatchMapW.Location = new System.Drawing.Point(100, 25);
            this.tbBatchMapW.Size = new System.Drawing.Size(90, 23);
            this.tbBatchMapW.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchMapW.TabIndex = 1;
            this.grpBatchConfig.Controls.Add(this.tbBatchMapW);

            this.lblBatchMapHLbl.Text = "高度：";
            this.lblBatchMapHLbl.Location = new System.Drawing.Point(200, 28);
            this.lblBatchMapHLbl.AutoSize = true;
            this.lblBatchMapHLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchMapHLbl.TabIndex = 2;
            this.grpBatchConfig.Controls.Add(this.lblBatchMapHLbl);

            this.tbBatchMapH.Text = "4096";
            this.tbBatchMapH.Location = new System.Drawing.Point(255, 25);
            this.tbBatchMapH.Size = new System.Drawing.Size(90, 23);
            this.tbBatchMapH.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchMapH.TabIndex = 3;
            this.grpBatchConfig.Controls.Add(this.tbBatchMapH);

            this.lblBatchMapIdxLbl.Text = "地图索引：";
            this.lblBatchMapIdxLbl.Location = new System.Drawing.Point(360, 28);
            this.lblBatchMapIdxLbl.AutoSize = true;
            this.lblBatchMapIdxLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchMapIdxLbl.TabIndex = 4;
            this.grpBatchConfig.Controls.Add(this.lblBatchMapIdxLbl);

            this.tbBatchMapIdx.Text = "0";
            this.tbBatchMapIdx.Location = new System.Drawing.Point(450, 25);
            this.tbBatchMapIdx.Size = new System.Drawing.Size(60, 23);
            this.tbBatchMapIdx.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchMapIdx.TabIndex = 5;
            this.grpBatchConfig.Controls.Add(this.tbBatchMapIdx);

            // 第2行
            this.lblBatchArtLbl.Text = "物品条目数：";
            this.lblBatchArtLbl.Location = new System.Drawing.Point(12, 62);
            this.lblBatchArtLbl.AutoSize = true;
            this.lblBatchArtLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchArtLbl.TabIndex = 6;
            this.grpBatchConfig.Controls.Add(this.lblBatchArtLbl);

            this.tbBatchArtCount.Text = "81884";
            this.tbBatchArtCount.Location = new System.Drawing.Point(110, 59);
            this.tbBatchArtCount.Size = new System.Drawing.Size(90, 23);
            this.tbBatchArtCount.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchArtCount.TabIndex = 7;
            this.grpBatchConfig.Controls.Add(this.tbBatchArtCount);

            this.lblBatchSoundLbl.Text = "声音槽位数：";
            this.lblBatchSoundLbl.Location = new System.Drawing.Point(215, 62);
            this.lblBatchSoundLbl.AutoSize = true;
            this.lblBatchSoundLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchSoundLbl.TabIndex = 8;
            this.grpBatchConfig.Controls.Add(this.lblBatchSoundLbl);

            this.tbBatchSoundCount.Text = "4095";
            this.tbBatchSoundCount.Location = new System.Drawing.Point(315, 59);
            this.tbBatchSoundCount.Size = new System.Drawing.Size(90, 23);
            this.tbBatchSoundCount.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchSoundCount.TabIndex = 9;
            this.grpBatchConfig.Controls.Add(this.tbBatchSoundCount);

            this.lblBatchGumpLbl.Text = "Gump 条目数：";
            this.lblBatchGumpLbl.Location = new System.Drawing.Point(420, 62);
            this.lblBatchGumpLbl.AutoSize = true;
            this.lblBatchGumpLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchGumpLbl.TabIndex = 10;
            this.grpBatchConfig.Controls.Add(this.lblBatchGumpLbl);

            this.tbBatchGumpCount.Text = "65535";
            this.tbBatchGumpCount.Location = new System.Drawing.Point(535, 59);
            this.tbBatchGumpCount.Size = new System.Drawing.Size(90, 23);
            this.tbBatchGumpCount.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchGumpCount.TabIndex = 11;
            this.grpBatchConfig.Controls.Add(this.tbBatchGumpCount);

            this.lblBatchMultiLbl.Text = "Multi 条目数：";
            this.lblBatchMultiLbl.Location = new System.Drawing.Point(640, 62);
            this.lblBatchMultiLbl.AutoSize = true;
            this.lblBatchMultiLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchMultiLbl.TabIndex = 12;
            this.grpBatchConfig.Controls.Add(this.lblBatchMultiLbl);

            this.tbBatchMultiCount.Text = "5000";
            this.tbBatchMultiCount.Location = new System.Drawing.Point(750, 59);
            this.tbBatchMultiCount.Size = new System.Drawing.Size(80, 23);
            this.tbBatchMultiCount.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchMultiCount.TabIndex = 13;
            this.grpBatchConfig.Controls.Add(this.tbBatchMultiCount);

            // 第3行
            this.lblBatchTileLandLbl.Text = "TileData 地形组数：";
            this.lblBatchTileLandLbl.Location = new System.Drawing.Point(12, 97);
            this.lblBatchTileLandLbl.AutoSize = true;
            this.lblBatchTileLandLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchTileLandLbl.TabIndex = 14;
            this.grpBatchConfig.Controls.Add(this.lblBatchTileLandLbl);

            this.tbBatchTileLand.Text = "512";
            this.tbBatchTileLand.Location = new System.Drawing.Point(195, 94);
            this.tbBatchTileLand.Size = new System.Drawing.Size(90, 23);
            this.tbBatchTileLand.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchTileLand.TabIndex = 15;
            this.grpBatchConfig.Controls.Add(this.tbBatchTileLand);

            this.lblBatchTileStaticLbl.Text = "静态组数：";
            this.lblBatchTileStaticLbl.Location = new System.Drawing.Point(300, 97);
            this.lblBatchTileStaticLbl.AutoSize = true;
            this.lblBatchTileStaticLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchTileStaticLbl.TabIndex = 16;
            this.grpBatchConfig.Controls.Add(this.lblBatchTileStaticLbl);

            this.tbBatchTileStatic.Text = "2048";
            this.tbBatchTileStatic.Location = new System.Drawing.Point(415, 94);
            this.tbBatchTileStatic.Size = new System.Drawing.Size(90, 23);
            this.tbBatchTileStatic.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchTileStatic.TabIndex = 17;
            this.grpBatchConfig.Controls.Add(this.tbBatchTileStatic);

            // 第4行 – 技能
            this.checkBoxBatchSkills.Text = "创建技能";
            this.checkBoxBatchSkills.Checked = true;
            this.checkBoxBatchSkills.Location = new System.Drawing.Point(12, 130);
            this.checkBoxBatchSkills.AutoSize = true;
            this.checkBoxBatchSkills.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxBatchSkills.TabIndex = 18;
            this.grpBatchConfig.Controls.Add(this.checkBoxBatchSkills);

            this.checkBoxBatchDefaultSkills.Text = "标准技能（58 个 UO 技能）";
            this.checkBoxBatchDefaultSkills.Checked = true;
            this.checkBoxBatchDefaultSkills.Location = new System.Drawing.Point(155, 130);
            this.checkBoxBatchDefaultSkills.AutoSize = true;
            this.checkBoxBatchDefaultSkills.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxBatchDefaultSkills.TabIndex = 19;
            this.grpBatchConfig.Controls.Add(this.checkBoxBatchDefaultSkills);

            this.lblBatchSkillCountLbl.Text = "技能数量（空）：";
            this.lblBatchSkillCountLbl.Location = new System.Drawing.Point(400, 131);
            this.lblBatchSkillCountLbl.AutoSize = true;
            this.lblBatchSkillCountLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBatchSkillCountLbl.TabIndex = 20;
            this.grpBatchConfig.Controls.Add(this.lblBatchSkillCountLbl);

            this.tbBatchSkillCount.Text = "58";
            this.tbBatchSkillCount.Location = new System.Drawing.Point(520, 128);
            this.tbBatchSkillCount.Size = new System.Drawing.Size(70, 23);
            this.tbBatchSkillCount.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.tbBatchSkillCount.TabIndex = 21;
            this.grpBatchConfig.Controls.Add(this.tbBatchSkillCount);

            this.grpBatchActions.Text = "执行";
            this.grpBatchActions.Location = new System.Drawing.Point(10, 298);
            this.grpBatchActions.Size = new System.Drawing.Size(920, 70);
            this.grpBatchActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpBatchActions.TabIndex = 1;
            this.tabPageBatch.Controls.Add(this.grpBatchActions);

            this.btnBatchCreate.Text = "创建所有文件";
            this.btnBatchCreate.Location = new System.Drawing.Point(12, 22);
            this.btnBatchCreate.Size = new System.Drawing.Size(300, 35);
            this.btnBatchCreate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBatchCreate.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnBatchCreate.ForeColor = System.Drawing.Color.White;
            this.btnBatchCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchCreate.TabIndex = 0;
            this.btnBatchCreate.Click += new System.EventHandler(this.BtnBatchCreate_Click);
            this.grpBatchActions.Controls.Add(this.btnBatchCreate);

            this.lblBatchStatus.Text = "-";
            this.lblBatchStatus.Location = new System.Drawing.Point(325, 30);
            this.lblBatchStatus.AutoSize = true;
            this.lblBatchStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBatchStatus.ForeColor = System.Drawing.Color.Navy;
            this.lblBatchStatus.TabIndex = 1;
            this.grpBatchActions.Controls.Add(this.lblBatchStatus);

            this.grpBatchLog.Text = "进度日志";
            this.grpBatchLog.Location = new System.Drawing.Point(10, 376);
            this.grpBatchLog.Size = new System.Drawing.Size(920, 235);
            this.grpBatchLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpBatchLog.TabIndex = 2;
            this.tabPageBatch.Controls.Add(this.grpBatchLog);

            this.textBoxBatchLog.Location = new System.Drawing.Point(12, 24);
            this.textBoxBatchLog.Size = new System.Drawing.Size(890, 200);
            this.textBoxBatchLog.Multiline = true;
            this.textBoxBatchLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxBatchLog.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.textBoxBatchLog.ReadOnly = true;
            this.textBoxBatchLog.TabIndex = 0;
            this.grpBatchLog.Controls.Add(this.textBoxBatchLog);

            // ════════════════════════════════════════════════════════════════
            // 选项卡19 – 十六进制查看器
            // ════════════════════════════════════════════════════════════════
            this.tabPageHexViewer.Text = "十六进制查看器";
            this.tabPageHexViewer.Size = new System.Drawing.Size(946, 638);
            this.tabPageHexViewer.TabIndex = 18;
            this.tabPageHexViewer.UseVisualStyleBackColor = true;

            this.grpHexFile.Text = "选择文件";
            this.grpHexFile.Location = new System.Drawing.Point(10, 10);
            this.grpHexFile.Size = new System.Drawing.Size(920, 90);
            this.grpHexFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpHexFile.TabIndex = 0;
            this.tabPageHexViewer.Controls.Add(this.grpHexFile);

            this.lblHexFilePathLbl.Text = "文件：";
            this.lblHexFilePathLbl.Location = new System.Drawing.Point(12, 28);
            this.lblHexFilePathLbl.AutoSize = true;
            this.lblHexFilePathLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHexFilePathLbl.TabIndex = 0;
            this.grpHexFile.Controls.Add(this.lblHexFilePathLbl);

            this.tbHexFilePath.Location = new System.Drawing.Point(55, 25);
            this.tbHexFilePath.Size = new System.Drawing.Size(720, 23);
            this.tbHexFilePath.Font = new System.Drawing.Font("Consolas", 9F);
            this.tbHexFilePath.TabIndex = 1;
            this.grpHexFile.Controls.Add(this.tbHexFilePath);

            this.BtnHexBrowse.Text = "打开 ...";
            this.BtnHexBrowse.Location = new System.Drawing.Point(785, 23);
            this.BtnHexBrowse.Size = new System.Drawing.Size(120, 26);
            this.BtnHexBrowse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnHexBrowse.TabIndex = 2;
            this.BtnHexBrowse.Click += new System.EventHandler(this.BtnHexBrowse_Click);
            this.grpHexFile.Controls.Add(this.BtnHexBrowse);

            this.lblHexFileInfo.Text = "-";
            this.lblHexFileInfo.Location = new System.Drawing.Point(12, 58);
            this.lblHexFileInfo.Size = new System.Drawing.Size(890, 22);
            this.lblHexFileInfo.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.lblHexFileInfo.ForeColor = System.Drawing.Color.Navy;
            this.lblHexFileInfo.TabIndex = 3;
            this.grpHexFile.Controls.Add(this.lblHexFileInfo);

            this.grpHexRead.Text = "读取十六进制";
            this.grpHexRead.Location = new System.Drawing.Point(10, 108);
            this.grpHexRead.Size = new System.Drawing.Size(920, 75);
            this.grpHexRead.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpHexRead.TabIndex = 1;
            this.tabPageHexViewer.Controls.Add(this.grpHexRead);

            this.lblHexOffsetLbl.Text = "偏移量 (0x...)：";
            this.lblHexOffsetLbl.Location = new System.Drawing.Point(12, 30);
            this.lblHexOffsetLbl.AutoSize = true;
            this.lblHexOffsetLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHexOffsetLbl.TabIndex = 0;
            this.grpHexRead.Controls.Add(this.lblHexOffsetLbl);

            this.tbHexOffset.Text = "0x0";
            this.tbHexOffset.Location = new System.Drawing.Point(105, 27);
            this.tbHexOffset.Size = new System.Drawing.Size(130, 23);
            this.tbHexOffset.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbHexOffset.TabIndex = 1;
            this.grpHexRead.Controls.Add(this.tbHexOffset);

            this.lblHexLengthLbl.Text = "长度（字节）：";
            this.lblHexLengthLbl.Location = new System.Drawing.Point(255, 30);
            this.lblHexLengthLbl.AutoSize = true;
            this.lblHexLengthLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHexLengthLbl.TabIndex = 2;
            this.grpHexRead.Controls.Add(this.lblHexLengthLbl);

            this.tbHexLength.Text = "256";
            this.tbHexLength.Location = new System.Drawing.Point(360, 27);
            this.tbHexLength.Size = new System.Drawing.Size(100, 23);
            this.tbHexLength.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbHexLength.TabIndex = 3;
            this.grpHexRead.Controls.Add(this.tbHexLength);

            this.BtnHexRead.Text = "读取十六进制";
            this.BtnHexRead.Location = new System.Drawing.Point(475, 25);
            this.BtnHexRead.Size = new System.Drawing.Size(140, 28);
            this.BtnHexRead.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtnHexRead.TabIndex = 4;
            this.BtnHexRead.Click += new System.EventHandler(this.BtnHexRead_Click);
            this.grpHexRead.Controls.Add(this.BtnHexRead);

            this.BtnHexFileInfo.Text = "文件信息";
            this.BtnHexFileInfo.Location = new System.Drawing.Point(625, 25);
            this.BtnHexFileInfo.Size = new System.Drawing.Size(140, 28);
            this.BtnHexFileInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnHexFileInfo.TabIndex = 5;
            this.BtnHexFileInfo.Click += new System.EventHandler(this.BtnHexFileInfo_Click);
            this.grpHexRead.Controls.Add(this.BtnHexFileInfo);

            this.grpHexSearch.Text = "搜索字节模式";
            this.grpHexSearch.Location = new System.Drawing.Point(10, 191);
            this.grpHexSearch.Size = new System.Drawing.Size(920, 70);
            this.grpHexSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpHexSearch.TabIndex = 2;
            this.tabPageHexViewer.Controls.Add(this.grpHexSearch);

            this.lblHexPatternLbl.Text = "十六进制模式（例如 FF00AB）：";
            this.lblHexPatternLbl.Location = new System.Drawing.Point(12, 28);
            this.lblHexPatternLbl.AutoSize = true;
            this.lblHexPatternLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHexPatternLbl.TabIndex = 0;
            this.grpHexSearch.Controls.Add(this.lblHexPatternLbl);

            this.tbHexPattern.Location = new System.Drawing.Point(215, 25);
            this.tbHexPattern.Size = new System.Drawing.Size(250, 23);
            this.tbHexPattern.Font = new System.Drawing.Font("Consolas", 10F);
            this.tbHexPattern.TabIndex = 1;
            this.grpHexSearch.Controls.Add(this.tbHexPattern);

            this.BtnHexSearch.Text = "搜索";
            this.BtnHexSearch.Location = new System.Drawing.Point(480, 23);
            this.BtnHexSearch.Size = new System.Drawing.Size(140, 28);
            this.BtnHexSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.BtnHexSearch.TabIndex = 2;
            this.BtnHexSearch.Click += new System.EventHandler(this.BtnHexSearch_Click);
            this.grpHexSearch.Controls.Add(this.BtnHexSearch);

            this.grpHexOutput.Text = "输出";
            this.grpHexOutput.Location = new System.Drawing.Point(10, 269);
            this.grpHexOutput.Size = new System.Drawing.Size(920, 342);
            this.grpHexOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpHexOutput.TabIndex = 3;
            this.tabPageHexViewer.Controls.Add(this.grpHexOutput);

            this.textBoxHexOutput.Location = new System.Drawing.Point(12, 24);
            this.textBoxHexOutput.Size = new System.Drawing.Size(890, 306);
            this.textBoxHexOutput.Multiline = true;
            this.textBoxHexOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxHexOutput.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.textBoxHexOutput.ReadOnly = true;
            this.textBoxHexOutput.TabIndex = 0;
            this.grpHexOutput.Controls.Add(this.textBoxHexOutput);

            // ════════════════════════════════════════════════════════════════
            // 恢复布局
            // ════════════════════════════════════════════════════════════════
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPalette)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // ── 字段声明 ───────────────────────────────────────────────
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatusLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;

        // 现有选项卡
        private System.Windows.Forms.TabPage tabPageCreateMuls;
        private System.Windows.Forms.TabPage tabPageReadMuls;
        private System.Windows.Forms.TabPage tabPageTileData;
        private System.Windows.Forms.TabPage tabPageReadOut;
        private System.Windows.Forms.TabPage tabPageTexturen;
        private System.Windows.Forms.TabPage tabPageRadarColor;
        private System.Windows.Forms.TabPage tabPagePalette;
        private System.Windows.Forms.TabPage tabPageAnimation;
        private System.Windows.Forms.TabPage tabPageArtmul;
        private System.Windows.Forms.TabPage tabPageSound;
        private System.Windows.Forms.TabPage tabPageGump;

        // 新增选项卡
        private System.Windows.Forms.TabPage tabPageHues;
        private System.Windows.Forms.TabPage tabPageMap;
        private System.Windows.Forms.TabPage tabPageMulti;
        private System.Windows.Forms.TabPage tabPageSkills;
        private System.Windows.Forms.TabPage tabPageValidator;
        private System.Windows.Forms.TabPage tabPageIdxPatcher;
        private System.Windows.Forms.TabPage tabPageBatch;
        private System.Windows.Forms.TabPage tabPageHexViewer;

        // 选项卡1
        private System.Windows.Forms.GroupBox grpCreateMulsDir, grpCreateMulsCount, grpCreateMulsButtons, grpRename, grpCreateOutput;
        private System.Windows.Forms.TextBox textBox1, textBox2;
        private System.Windows.Forms.Button BtFileOrder;
        private System.Windows.Forms.Label lblDirInfo, lblCountHint, lblButtonsNote, lblRenameHint;
        private System.Windows.Forms.Button BtCreateARTIDXMul, BtCreateARTIDXMul_uint, BtCreateARTIDXMul_Int;
        private System.Windows.Forms.Button BtCreateARTIDXMul_Ushort, BtCreateARTIDXMul_Short, BtCreateARTIDXMul_Byte;
        private System.Windows.Forms.Button BtCreateARTIDXMul_Ulong, BtCreateARTIDXMul_Sbyte;
        private System.Windows.Forms.ComboBox ComboBoxMuls;
        private System.Windows.Forms.Label lbCreatedMul;

        // 选项卡2
        private System.Windows.Forms.GroupBox grpReadMulsActions, grpReadMulsResult, grpReadSingle;
        private System.Windows.Forms.Button BtnCountEntries, BtnShowInfo, BtnReadArtIdx;
        private System.Windows.Forms.Label lblEntryCount, lblIndexHint;
        private System.Windows.Forms.TextBox textBoxInfo, textBoxIndex;

        // 选项卡3
        private System.Windows.Forms.GroupBox grpTileDataDir, grpTileDataConfig, grpTileDataQuick;
        private System.Windows.Forms.GroupBox grpTileDataRead, grpTileDataIndex, grpTileDataOutput;
        private System.Windows.Forms.TextBox tbDirTileData, tblandTileGroups, tbstaticTileGroups;
        private System.Windows.Forms.Button btnTileDataBrowse, BtCreateTiledata;
        private System.Windows.Forms.Label lblLandGroupsLbl, lblLandGroupsHint, lblStaticGroupsLbl, lblStaticGroupsHint;
        private System.Windows.Forms.Button BtCreateTiledataEmpty, BtCreateTiledataEmpty2, BtCreateSimpleTiledata;
        private System.Windows.Forms.Button BtTiledatainfo, BtnCountTileDataEntries, BtReadIndexTiledata;
        private System.Windows.Forms.Button BtReadLandTile, BtReadStaticTile, BtReadTileFlags, BtSelectDirectory;
        private System.Windows.Forms.Label lblTileDataEntryCount, lblTiledataIndexHint;
        private System.Windows.Forms.TextBox textBoxTiledataIndex;
        private System.Windows.Forms.Label lbTileDataCreate;
        private System.Windows.Forms.CheckBox checkBoxTileData;

        // 选项卡4
        private System.Windows.Forms.GroupBox grpReadOutActions, grpReadOutInfo;
        private System.Windows.Forms.Button ButtonReadTileData, ButtonReadLandTileData, ButtonReadStaticTileData;
        private System.Windows.Forms.Label lblSelectedEntry, lblReadOutIdxLbl;
        private System.Windows.Forms.ListView listViewTileData;
        private System.Windows.Forms.TextBox textBoxOutput, textBoxTileDataInfo;

        // 选项卡5
        private System.Windows.Forms.GroupBox grpTexConfig, grpTexActions, grpTexOutput;
        private System.Windows.Forms.Label lblTexCountLbl;
        private System.Windows.Forms.TextBox tbIndexCountTexture;
        private System.Windows.Forms.Label lblTexCountHint, lbTextureCount, tbIndexCount;
        private System.Windows.Forms.CheckBox checkBoxTexture;
        private System.Windows.Forms.Button BtCreateTextur, BtCreateIndexes;

        // 选项卡6
        private System.Windows.Forms.GroupBox grpRadarConfig, grpRadarActions, grpRadarOutput;
        private System.Windows.Forms.Label lblRadarCountLbl;
        private System.Windows.Forms.TextBox indexCountTextBox;
        private System.Windows.Forms.Label lblRadarCountHint, lbRadarColor;
        private System.Windows.Forms.Button CreateFileButtonRadarColor;

        // 选项卡7
        private System.Windows.Forms.GroupBox grpPaletteCreate, grpPaletteLoad, grpPaletteValues;
        private System.Windows.Forms.Button BtCreatePalette, BtCreatePaletteFull, LoadPaletteButton;
        private System.Windows.Forms.Label lbCreatePalette, lbCreateColorPalette, lblPalettePreview;
        private System.Windows.Forms.PictureBox pictureBoxPalette;
        private System.Windows.Forms.TextBox textBoxRgbValues;

        // 选项卡8
        private System.Windows.Forms.GroupBox grpAnimSource, grpAnimOutput, grpAnimCreature;
        private System.Windows.Forms.GroupBox grpAnimActions, grpAnimInfo, grpAnimLog;
        private System.Windows.Forms.TextBox tbfilename, txtOutputDirectory, txtOutputFilename;
        private System.Windows.Forms.Button BtnBrowse, BtnSetOutputDirectory;
        private System.Windows.Forms.Label lblAnimSuffixLbl, lblOrigIDHint, lblHexWarning, lblCopyCountHint;
        private System.Windows.Forms.TextBox txtOrigCreatureID, txtNewCreatureID;
        private System.Windows.Forms.Panel panelCheckbox;
        private System.Windows.Forms.Label lbCopys;
        private System.Windows.Forms.CheckBox chkHighDetail, chkLowDetail, chkHuman;
        private System.Windows.Forms.Button BtnNewAnimIDXFiles, BtnProcessClickOld, Button1;
        private System.Windows.Forms.Button ReadAnimIdx, btnCountIndices, BtnLoadAnimationMulData;
        private System.Windows.Forms.TextBox txtData, tbProcessAminidx;
        private System.Windows.Forms.Label lblNewIdCount;

        // 选项卡9
        private System.Windows.Forms.GroupBox grpArtCreate, grpArtCustom, grpArtSplit, grpArtRead, grpArtLog;
        private System.Windows.Forms.Button BtnCreateArtIdx, BtnCreateArtIdx100K, BtnCreateArtIdx150K;
        private System.Windows.Forms.Button BtnCreateArtIdx200K, BtnCreateArtIdx250K, BtnCreateArtIdx500K;
        private System.Windows.Forms.TextBox tbxNewIndex, tbxNewIndex2, tbxArtsCount, tbxLandTilesCount;
        private System.Windows.Forms.Button Button2, Button3, BtCreateOldVersionArtidx;
        private System.Windows.Forms.Label lblArtCreateHint, lblArtCustomHint, lblOldVersionHint;
        private System.Windows.Forms.Label lblArtSplitHint, lblArtSplitTotalLbl, lbArtsCount, lbLandTilesCount, lblArtReadHint;
        private System.Windows.Forms.Button ReadArtmul, ReadArtmul2;
        private System.Windows.Forms.Label lblIndexCount;
        private System.Windows.Forms.TextBox infoARTIDXMULID;

        // 选项卡10
        private System.Windows.Forms.GroupBox grpSoundConfig, grpSoundActions, grpSoundOutput;
        private System.Windows.Forms.Label lblSoundCountLbl;
        private System.Windows.Forms.TextBox SoundIDXMul;
        private System.Windows.Forms.Label lblSoundCountHint, IndexSizeLabel;
        private System.Windows.Forms.Button CreateOrgSoundMul, ReadIndexSize;

        // 选项卡11
        private System.Windows.Forms.GroupBox grpGumpConfig, grpGumpActions, grpGumpOutput;
        private System.Windows.Forms.Label lblGumpCountLbl;
        private System.Windows.Forms.TextBox IndexSizeTextBox;
        private System.Windows.Forms.Label lblGumpCountHint, gumpLabel;
        private System.Windows.Forms.Button CreateGumpButton, ReadGumpButton;

        // 选项卡12 – Hues
        private System.Windows.Forms.GroupBox grpHuesActions, grpHuesOutput;
        private System.Windows.Forms.Button BtnCreateHues, BtnReadHues;
        private System.Windows.Forms.Label lblHuesOutput;

        // 选项卡13 – 地图/静态
        private System.Windows.Forms.GroupBox grpMapConfig, grpMapActions, grpMapOutput;
        private System.Windows.Forms.Label lblMapSizeComboLbl;
        private System.Windows.Forms.ComboBox comboMapSize;
        private System.Windows.Forms.Label lblMapWidthLbl;
        private System.Windows.Forms.TextBox tbMapWidth;
        private System.Windows.Forms.Label lblMapHeightLbl;
        private System.Windows.Forms.TextBox tbMapHeight;
        private System.Windows.Forms.Label lblMapIndexLbl;
        private System.Windows.Forms.TextBox tbMapIndex;
        private System.Windows.Forms.Label lblMapSizeInfo, lblMapOutput;
        private System.Windows.Forms.Button BtnCreateMap, BtnCreateStatics, BtnCreateMapAndStatics;

        // 选项卡14 – Multi
        private System.Windows.Forms.GroupBox grpMultiConfig, grpMultiActions, grpMultiOutput;
        private System.Windows.Forms.Label lblMultiCountLbl;
        private System.Windows.Forms.TextBox tbMultiCount;
        private System.Windows.Forms.Label lblMultiIndexLbl;
        private System.Windows.Forms.TextBox tbMultiIndex;
        private System.Windows.Forms.CheckBox checkBoxMultiHS;
        private System.Windows.Forms.Button BtnCreateMulti, BtnReadMulti;
        private System.Windows.Forms.Label lblMultiOutput;

        // 选项卡15 – 技能
        private System.Windows.Forms.GroupBox grpSkillsConfig, grpSkillsActions, grpSkillsOutput;
        private System.Windows.Forms.Label lblSkillCountLbl;
        private System.Windows.Forms.TextBox tbSkillCount;
        private System.Windows.Forms.Button BtnCreateDefaultSkills, BtnCreateEmptySkills, BtnReadSkills;
        private System.Windows.Forms.Label lblSkillsOutput;
        private System.Windows.Forms.TextBox textBoxSkillsInfo;

        // 选项卡16 – 验证器
        private System.Windows.Forms.GroupBox grpValidatorActions, grpValidatorOutput;
        private System.Windows.Forms.Button BtnValidate, BtnCompareDirectories;
        private System.Windows.Forms.Label lblValidatorStatus;
        private System.Windows.Forms.TextBox textBoxValidatorOutput;

        // 选项卡17 – IDX 修补器
        private System.Windows.Forms.GroupBox grpPatcherFile, grpPatcherEdit, grpPatcherRange, grpPatcherOutput;
        private System.Windows.Forms.Label lblPatchIdxLbl;
        private System.Windows.Forms.TextBox tbPatchIdxPath;
        private System.Windows.Forms.Button BtnPatchBrowseIdx;
        private System.Windows.Forms.Label lblPatchIndexLbl;
        private System.Windows.Forms.TextBox tbPatchIndex;
        private System.Windows.Forms.Label lblPatchLookupLbl;
        private System.Windows.Forms.TextBox tbPatchLookup;
        private System.Windows.Forms.Label lblPatchSizeLbl;
        private System.Windows.Forms.TextBox tbPatchSize;
        private System.Windows.Forms.Label lblPatchUnknownLbl;
        private System.Windows.Forms.TextBox tbPatchUnknown;
        private System.Windows.Forms.Button BtnPatchEntry, BtnClearEntry;
        private System.Windows.Forms.Label lblPatchRangeFromLbl;
        private System.Windows.Forms.TextBox tbPatchRangeFrom;
        private System.Windows.Forms.Label lblPatchRangeCountLbl;
        private System.Windows.Forms.TextBox tbPatchRangeCount;
        private System.Windows.Forms.Button BtnReadRange;
        private System.Windows.Forms.TextBox textBoxPatcherOutput;

        // 选项卡18 – 批量设置
        private System.Windows.Forms.GroupBox grpBatchConfig, grpBatchActions, grpBatchLog;
        private System.Windows.Forms.Label lblBatchMapWLbl;
        private System.Windows.Forms.TextBox tbBatchMapW;
        private System.Windows.Forms.Label lblBatchMapHLbl;
        private System.Windows.Forms.TextBox tbBatchMapH;
        private System.Windows.Forms.Label lblBatchMapIdxLbl;
        private System.Windows.Forms.TextBox tbBatchMapIdx;
        private System.Windows.Forms.Label lblBatchArtLbl;
        private System.Windows.Forms.TextBox tbBatchArtCount;
        private System.Windows.Forms.Label lblBatchSoundLbl;
        private System.Windows.Forms.TextBox tbBatchSoundCount;
        private System.Windows.Forms.Label lblBatchGumpLbl;
        private System.Windows.Forms.TextBox tbBatchGumpCount;
        private System.Windows.Forms.Label lblBatchMultiLbl;
        private System.Windows.Forms.TextBox tbBatchMultiCount;
        private System.Windows.Forms.Label lblBatchTileLandLbl;
        private System.Windows.Forms.TextBox tbBatchTileLand;
        private System.Windows.Forms.Label lblBatchTileStaticLbl;
        private System.Windows.Forms.TextBox tbBatchTileStatic;
        private System.Windows.Forms.Label lblBatchSkillCountLbl;
        private System.Windows.Forms.TextBox tbBatchSkillCount;
        private System.Windows.Forms.CheckBox checkBoxBatchSkills, checkBoxBatchDefaultSkills;
        private System.Windows.Forms.Button btnBatchCreate;
        private System.Windows.Forms.Label lblBatchStatus;
        private System.Windows.Forms.TextBox textBoxBatchLog;

        // 选项卡19 – 十六进制查看器
        private System.Windows.Forms.GroupBox grpHexFile, grpHexRead, grpHexSearch, grpHexOutput;
        private System.Windows.Forms.Label lblHexFilePathLbl;
        private System.Windows.Forms.TextBox tbHexFilePath;
        private System.Windows.Forms.Button BtnHexBrowse;
        private System.Windows.Forms.Label lblHexFileInfo;
        private System.Windows.Forms.Label lblHexOffsetLbl;
        private System.Windows.Forms.TextBox tbHexOffset;
        private System.Windows.Forms.Label lblHexLengthLbl;
        private System.Windows.Forms.TextBox tbHexLength;
        private System.Windows.Forms.Button BtnHexRead, BtnHexFileInfo;
        private System.Windows.Forms.Label lblHexPatternLbl;
        private System.Windows.Forms.TextBox tbHexPattern;
        private System.Windows.Forms.Button BtnHexSearch;
        private System.Windows.Forms.TextBox textBoxHexOutput;
    }
}