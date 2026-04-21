// =============================================================================
//  ARTMulIDXCreator.cs – 窗体代码隐藏  版本 2.0
//  所有新增选项卡：Hues、Map/Statics、Multi、Skills、Validator、IdxPatcher、
//  BatchSetup、HexViewer、DirCompare
// =============================================================================

using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using UoFiddler.Plugin.ConverterMultiTextPlugin.MulCore;

namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    public partial class ARTMulIDXCreator : Form
    {
        // =====================================================================
        //  字段
        // =====================================================================

        private BinaryReader _reader;
        private int _landItemsLoaded = 0;
        private int _staticItemsLoaded = 0;
        private int _itemsLoaded = 0;
        private int _newIdCount = 0;

        private readonly PaletteFile _palette = new PaletteFile();

        private const int HighDetail = (int)CreatureType.HighDetail;
        private const int LowDetail = (int)CreatureType.LowDetail;
        private const int Human = (int)CreatureType.Human;

        // =====================================================================
        //  构造函数
        // =====================================================================

        public ARTMulIDXCreator() { InitializeComponent(); PopulateMapSizeCombo(); }

        // =====================================================================
        //  选项卡：创建 Muls
        // =====================================================================

        #region 创建 ArtIDX

        private void CreateArtFiles(IndexFormat format = IndexFormat.Legacy)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text)) { ShowWarn("请先选择目录。"); return; }
                long count = MulFileHelper.ParseEntryCount(textBox2.Text, 65500, 1);
                lbCreatedMul.Text = MulFileHelper.CreateIndexAndMul(textBox1.Text, "artidx.MUL", "art.MUL", count, format);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtCreateARTIDXMul_Click(object s, EventArgs e) => CreateArtFiles();
        private void BtCreateARTIDXMul_uint_Click(object s, EventArgs e) => CreateArtFiles();
        private void BtCreateARTIDXMul_Int_Click(object s, EventArgs e) => CreateArtFiles();
        private void BtCreateARTIDXMul_Ushort_Click(object s, EventArgs e) => CreateArtFiles();
        private void BtCreateARTIDXMul_Short_Click(object s, EventArgs e) => CreateArtFiles();
        private void BtCreateARTIDXMul_Byte_Click(object s, EventArgs e) => CreateArtFiles();
        private void BtCreateARTIDXMul_Ulong_Click(object s, EventArgs e) => CreateArtFiles();

        private void BtFileOrder_Click(object s, EventArgs e)
        { using var d = new FolderBrowserDialog(); if (d.ShowDialog() == DialogResult.OK) textBox1.Text = d.SelectedPath; }

        private void ComboBoxMuls_SelectedIndexChanged(object s, EventArgs e)
        {
            try
            {
                if (ComboBoxMuls.SelectedItem?.ToString() != "Texture") return;
                string dir = textBox1.Text;
                string oa = Path.Combine(dir, "Art.mul"), oi = Path.Combine(dir, "Artidx.mul");
                string na = Path.Combine(dir, "texmaps.mul"), ni = Path.Combine(dir, "texidx.mul");
                if (!File.Exists(oa) || !File.Exists(oi)) { lbCreatedMul.Text = "未找到 Art.mul / Artidx.mul。"; return; }
                File.Move(oa, na); File.Move(oi, ni);
                lbCreatedMul.Text = $"已重命名：\n  {na}\n  {ni}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  选项卡：读取 Muls
        // =====================================================================

        #region 读取 Muls

        private void BtnCountEntries_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                var (idx, _) = MulFileHelper.LoadAndSummarize(Path.Combine(d.SelectedPath, "artidx.MUL"));
                lblEntryCount.Text = $"条目数：{idx.Count:N0}  [{idx.Format}]";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnShowInfo_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                textBox1.Text = d.SelectedPath;
                var (idx, _) = MulFileHelper.LoadAndSummarize(Path.Combine(d.SelectedPath, "artidx.MUL"));
                textBoxInfo.Text = idx.GetDetailedInfo(2000);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnReadArtIdx2_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                textBox1.Text = d.SelectedPath;
                int i = int.TryParse(textBoxIndex.Text, out int v) ? v : 0;
                textBoxInfo.Text = MulFileHelper.ReadSingleEntry(Path.Combine(d.SelectedPath, "artidx.MUL"), i);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  选项卡：TileData
        // =====================================================================

        #region 创建 TileData

        private void BtCreateTiledata_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                tbDirTileData.Text = d.SelectedPath;
                int land = ParseInt(tblandTileGroups.Text, 512), stat = ParseInt(tbstaticTileGroups.Text, 2048);
                string path = Path.Combine(d.SelectedPath, "Tiledata.mul");
                TileDataFile.CreateEmpty(land, stat).SaveToFile(path);
                lbTileDataCreate.Text = $"已创建：{path}\n地形：{land * 32:N0}  静态：{stat * 32:N0}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtCreateTiledataEmpty_Click(object s, EventArgs e) => BtCreateTiledata_Click(s, e);
        private void BtCreateTiledataEmpty2_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                string path = Path.Combine(d.SelectedPath, "Tiledata.mul");
                TileDataFile.CreateEmpty().SaveToFile(path);
                MessageBox.Show($"已创建空的默认 Tiledata.mul：\n{path}");
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtCreateSimpleTiledata_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int land = ParseInt(tblandTileGroups.Text, 512), stat = ParseInt(tbstaticTileGroups.Text, 2048);
                string path = Path.Combine(d.SelectedPath, "Tiledata.mul");
                if (File.Exists(path)) File.Delete(path);
                TileDataFile.CreateEmpty(land, stat).SaveToFile(path);
                MessageBox.Show($"已创建简单 Tiledata：\n{path}");
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        #region 读取 TileData

        private void BtTiledatainfo_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                tbDirTileData.Text = d.SelectedPath;
                string path = Path.Combine(d.SelectedPath, "Tiledata.mul");
                if (!File.Exists(path)) { textBoxTileDataInfo.Text = "未找到 Tiledata.mul。"; return; }
                var td = new TileDataFile(); td.LoadFromFile(path); textBoxTileDataInfo.Text = td.GetSummary();
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtReadIndexTiledata_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                string path = Path.Combine(d.SelectedPath, "Tiledata.mul");
                if (!File.Exists(path)) { textBoxTileDataInfo.Text = "未找到 Tiledata.mul。"; return; }
                int idx = ParseInt(textBoxTiledataIndex.Text, 0);
                var td = new TileDataFile(); td.LoadFromFile(path);
                var sb = new StringBuilder($"=== 索引 {idx} ===\n");
                sb.AppendLine(idx < td.LandTiles.Count ? $"[地形   ] {td.LandTiles[idx]}" : $"[地形   ] 不存在");
                sb.AppendLine(idx < td.StaticTiles.Count ? $"[静态 ] {td.StaticTiles[idx]}" : $"[静态 ] 不存在");
                textBoxTileDataInfo.Text = sb.ToString();
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        public void BtReadLandTile_Click(object s, EventArgs e) => ReadTileData("地形");
        public void BtReadStaticTile_Click(object s, EventArgs e) => ReadTileData("静态");

        public void ReadTileData(string tt)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                string path = Path.Combine(d.SelectedPath, "Tiledata.mul");
                if (!File.Exists(path)) { textBoxTileDataInfo.Text = "未找到 Tiledata.mul。"; return; }
                int idx = ParseInt(textBoxTiledataIndex.Text, 0);
                var td = new TileDataFile(); td.LoadFromFile(path);
                if (tt == "地形" && idx < td.LandTiles.Count) textBoxTileDataInfo.Text = $"[地形 {idx}]\n{td.LandTiles[idx]}";
                else if (tt == "静态" && idx < td.StaticTiles.Count) textBoxTileDataInfo.Text = $"[静态 {idx}]\n{td.StaticTiles[idx]}";
                else textBoxTileDataInfo.Text = $"索引 {idx} 在 '{tt}' 中不存在。";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtReadTileFlags_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                var td = new TileDataFile(); td.LoadFromFile(Path.Combine(d.SelectedPath, "Tiledata.mul"));
                textBoxTileDataInfo.Text = td.GetSummary() + "\n\n已知标志：\n" + string.Join(", ", TileDataFlags.KnownFlags.Keys);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        public void BtTReadHexAndSelectDirectory_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                string path = Path.Combine(d.SelectedPath, "Tiledata.mul");
                if (!File.Exists(path)) { textBoxTileDataInfo.Text = "未找到 Tiledata.mul。"; return; }
                string raw = textBoxTiledataIndex.Text.Trim();
                int idx = raw.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(raw.Substring(2), 16) : ParseInt(raw, 0);
                textBoxTileDataInfo.Text = HexViewHelper.ReadHex(path, (long)idx * 836, 836);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnCountTileDataEntries_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                var td = new TileDataFile(); td.LoadFromFile(Path.Combine(d.SelectedPath, "Tiledata.mul"));
                lblTileDataEntryCount.Text = $"地形：{td.LandTiles.Count:N0}  |  静态：{td.StaticTiles.Count:N0}  [{td.Version}]";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void CreateTiledataMul_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int land = ParseInt(tblandTileGroups.Text, 512), stat = ParseInt(tbstaticTileGroups.Text, 2048);
                string path = Path.Combine(d.SelectedPath, "Tiledata.mul");
                TileDataFile.CreateEmpty(land, stat).SaveToFile(path);
                lblTileDataEntryCount.Text = $"地形：{land * 32:N0}  静态：{stat * 32:N0}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void CountIndices_Click(object s, EventArgs e)
        {
            try
            {
                using var ofd = new OpenFileDialog { Filter = "MUL|*.mul" }; if (ofd.ShowDialog() != DialogResult.OK) return;
                var td = new TileDataFile(); td.LoadFromFile(ofd.FileName);
                lblTileDataEntryCount.Text = $"地形：{td.LandTiles.Count:N0}  静态：{td.StaticTiles.Count:N0}  [{td.Version}]";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        #region TileData 读出

        private void ButtonReadTileData_Click(object s, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "MUL|*.mul" }; if (ofd.ShowDialog() != DialogResult.OK) return;
            DisposeReader(); _reader = new BinaryReader(File.Open(ofd.FileName, FileMode.Open)); _itemsLoaded = 0;
            listViewTileData.Items.Clear(); LoadItems();
        }

        private void ButtonReadLandTileData_Click(object s, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "MUL|*.mul" }; if (ofd.ShowDialog() != DialogResult.OK) return;
            DisposeReader(); _reader = new BinaryReader(File.Open(ofd.FileName, FileMode.Open)); _landItemsLoaded = 0; LoadLandTiles();
        }

        private void ButtonReadStaticTileData_Click(object s, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "MUL|*.mul" }; if (ofd.ShowDialog() != DialogResult.OK) return;
            DisposeReader(); _reader = new BinaryReader(File.Open(ofd.FileName, FileMode.Open)); _staticItemsLoaded = 0; LoadStaticTiles();
        }

        private void TiledataHex_KeyDown(object s, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Space) return;
            if (_landItemsLoaded < 16384) LoadLandTiles();
            else if (_staticItemsLoaded < 65536) LoadStaticTiles();
        }

        private void LoadItems()
        {
            if (_reader == null) return;
            for (int i = 0; i < 50 && _reader.BaseStream.Position + 30 <= _reader.BaseStream.Length; i++)
            {
                ulong flags = _reader.ReadUInt64(); ushort texId = _reader.ReadUInt16();
                string name = Encoding.Default.GetString(_reader.ReadBytes(20)).TrimEnd('\0');
                listViewTileData.Items.Add(new ListViewItem(new[] { _reader.BaseStream.Position.ToString("X"), name, texId.ToString(), Convert.ToString((long)flags, 2).PadLeft(64, '0') }));
                _itemsLoaded++;
            }
        }

        private void LoadLandTiles()
        {
            if (_reader == null) return;
            for (int i = 0; i < 50 && _reader.BaseStream.Position + 26 <= _reader.BaseStream.Length; i++)
            {
                uint flags = _reader.ReadUInt32(); ushort texId = _reader.ReadUInt16();
                string name = Encoding.Default.GetString(_reader.ReadBytes(20)).TrimEnd('\0');
                listViewTileData.Items.Add(new ListViewItem(new[] { _reader.BaseStream.Position.ToString("X"), flags.ToString(), texId.ToString(), name }));
                _landItemsLoaded++;
            }
        }

        private void LoadStaticTiles()
        {
            if (_reader == null) return;
            for (int i = 0; i < 50 && _reader.BaseStream.Position + 41 <= _reader.BaseStream.Length; i++)
            {
                uint unk1 = _reader.ReadUInt32(), flags = _reader.ReadUInt32();
                byte wt = _reader.ReadByte(), qu = _reader.ReadByte(); ushort u1 = _reader.ReadUInt16();
                byte u2 = _reader.ReadByte(), qty = _reader.ReadByte(); ushort an = _reader.ReadUInt16();
                byte u3 = _reader.ReadByte(), hu = _reader.ReadByte(), so = _reader.ReadByte(), va = _reader.ReadByte(), ht = _reader.ReadByte();
                string name = Encoding.Default.GetString(_reader.ReadBytes(20)).TrimEnd('\0');
                listViewTileData.Items.Add(new ListViewItem(new[] { _reader.BaseStream.Position.ToString("X"), unk1.ToString(), flags.ToString(), wt.ToString(), qu.ToString(), u1.ToString(), u2.ToString(), qty.ToString(), an.ToString(), u3.ToString(), hu.ToString(), so.ToString(), va.ToString(), ht.ToString(), name }));
                _staticItemsLoaded++;
            }
        }

        private void ListViewTileData_MouseClick(object s, MouseEventArgs e)
        {
            if (listViewTileData.SelectedItems.Count == 0) return;
            var sb = new StringBuilder();
            foreach (ListViewItem.ListViewSubItem sub in listViewTileData.SelectedItems[0].SubItems) sb.AppendLine(sub.Text);
            textBoxOutput.Text = sb.ToString();
        }

        #endregion

        // =====================================================================
        //  选项卡：纹理
        // =====================================================================

        #region 纹理

        private void BtCreateTextur_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int count = int.TryParse(tbIndexCountTexture.Text, out int c) ? c : 16383;
                bool only2 = checkBoxTexture.Checked;
                using var mw = new BinaryWriter(File.Open(Path.Combine(d.SelectedPath, "TexMaps.mul"), FileMode.Create));
                using var iw = new BinaryWriter(File.Open(Path.Combine(d.SelectedPath, "TexIdx.mul"), FileMode.Create));
                for (int i = 0; i < count; i++) { bool big = !only2 && (i % 2 != 0); int dim = big ? 128 : 64; for (int p = 0; p < dim * dim; p++) mw.Write((short)0); iw.Write(dim); iw.Write(dim); iw.Write(big ? 1 : 0); }
                lbTextureCount.Text = $"已创建：{count:N0} 个条目"; tbIndexCount.Text = count.ToString();
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtCreateIndexes_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int count = int.TryParse(tbIndexCountTexture.Text, out int c) ? c : 16383;
                using var iw = new BinaryWriter(File.Open(Path.Combine(d.SelectedPath, "TexIdx.mul"), FileMode.Create));
                for (int i = 0; i < count; i++) { iw.Write(0); iw.Write(0); iw.Write(0); }
                lbTextureCount.Text = $"空的 TexIdx.mul：{count:N0}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  选项卡：RadarColor
        // =====================================================================

        #region RadarColor

        private void CreateFileButtonRadarColor_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int count = int.TryParse(indexCountTextBox.Text, out int c) ? c : 0x8000;
                string path = Path.Combine(d.SelectedPath, "radarcol.mul");
                using var w = new BinaryWriter(File.Open(path, FileMode.Create));
                for (int i = 0; i < count; i++) w.Write((short)0);
                lbRadarColor.Text = $"radarcol.mul：{count:N0} 种颜色\n{path}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  选项卡：调色板
        // =====================================================================

        #region 调色板

        private void BtCreatePalette_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                _palette.Colors.Clear(); _palette.CreateGrayscale();
                string path = Path.Combine(d.SelectedPath, "Palette.mul"); _palette.Save(path);
                lbCreatePalette.Text = $"灰度调色板：\n{path}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtCreatePaletteFull_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                _palette.Colors.Clear(); InitUoPalette();
                string path = Path.Combine(d.SelectedPath, "Palette.mul"); _palette.Save(path);
                lbCreateColorPalette.Text = $"已创建 UO 调色板：\n{path}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void LoadPaletteButton_Click(object s, EventArgs e)
        {
            try
            {
                using var ofd = new OpenFileDialog { Filter = "MUL|*.mul" }; if (ofd.ShowDialog() != DialogResult.OK) return;
                _palette.Colors.Clear(); _palette.Load(ofd.FileName);
                var bmp = new Bitmap(pictureBoxPalette.Width, pictureBoxPalette.Height);
                using (var g = Graphics.FromImage(bmp)) _palette.Draw(g, bmp.Width, bmp.Height);
                pictureBoxPalette.Image = bmp;
                var sb = new StringBuilder();
                for (int i = 0; i < _palette.Colors.Count; i++) { var (r, gr, b) = _palette.Colors[i]; sb.AppendLine($"[{i,3}]  R={r,3}  G={gr,3}  B={b,3}"); }
                textBoxRgbValues.Text = sb.ToString();
                try { Clipboard.SetText(sb.ToString()); } catch { }
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  选项卡：动画
        // =====================================================================

        #region 动画

        private void BtnLoadAnimationMulData_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                var anim = AnimationFile.Load(Path.Combine(d.SelectedPath, "Anim.mul"), Path.Combine(d.SelectedPath, "Anim.idx"));
                txtData.Text = anim.ToString();
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private async void ReadAnimIdx_Click(object s, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "IDX|ANIM.IDX;*.idx" }; if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                tbProcessAminidx.Clear();
                var sb = new StringBuilder();
                await Task.Run(() => { using var r = new BinaryReader(File.OpenRead(ofd.FileName)); int n = 0; while (r.BaseStream.Position + 12 <= r.BaseStream.Length) { int lk = r.ReadInt32(), sz = r.ReadInt32(), uk = r.ReadInt32(); sb.AppendLine($"[{n++,6}]  Lookup={lk,12}  Size={sz,10}  Unknown={uk}"); } });
                tbProcessAminidx.Text = sb.ToString();
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private async void BtnCountIndices_Click(object s, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "IDX|ANIM.IDX;*.idx" }; if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                int count = 0;
                await Task.Run(() => { using var r = new BinaryReader(File.OpenRead(ofd.FileName)); while (r.BaseStream.Position + 12 <= r.BaseStream.Length) { r.ReadInt32(); r.ReadInt32(); r.ReadInt32(); count++; } });
                lblNewIdCount.Text = $"条目数：{count:N0}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnBrowseClick(object s, EventArgs e) { using var ofd = new OpenFileDialog(); if (ofd.ShowDialog() == DialogResult.OK) tbfilename.Text = ofd.FileName; }
        private void BtnSetOutputDirectoryClick(object s, EventArgs e) { using var d = new FolderBrowserDialog(); if (d.ShowDialog() == DialogResult.OK) txtOutputDirectory.Text = d.SelectedPath; }

        private void BtnProcessClick(object s, EventArgs e)
        {
            tbProcessAminidx.Clear();
            try
            {
                if (!File.Exists(tbfilename.Text)) { tbProcessAminidx.AppendText("未找到源文件！\n"); return; }
                if (!int.TryParse(txtOrigCreatureID.Text, System.Globalization.NumberStyles.HexNumber, null, out int cid)) { tbProcessAminidx.AppendText("无效的动画 ID（十六进制）。\n"); return; }
                int copyCount;
                if (chkHighDetail.Checked) copyCount = HighDetail;
                else if (chkLowDetail.Checked) copyCount = LowDetail;
                else if (chkHuman.Checked) copyCount = Human;
                else if (!int.TryParse(txtNewCreatureID.Text, out copyCount)) { tbProcessAminidx.AppendText("无效的副本数量。\n"); return; }
                string outFile = string.IsNullOrWhiteSpace(txtOutputFilename.Text)
                    ? Path.Combine(txtOutputDirectory.Text, "anim.idx")
                    : Path.Combine(txtOutputDirectory.Text, "anim" + txtOutputFilename.Text + ".idx");
                File.Copy(tbfilename.Text, outFile, true);
                string result = AnimationFile.CopyCreatureIdx(outFile, cid, copyCount, msg => tbProcessAminidx.AppendText(msg + "\n"));
                tbProcessAminidx.AppendText(result + "\n");
                _newIdCount += copyCount; lblNewIdCount.Text = $"已创建的 ID：{_newIdCount:N0}";
            }
            catch (Exception ex) { tbProcessAminidx.AppendText($"错误：{ex.Message}\n"); }
        }

        private void BtnProcessClickOldVersion(object s, EventArgs e)
        {
            tbProcessAminidx.Clear();
            try
            {
                if (!File.Exists(tbfilename.Text)) { tbProcessAminidx.AppendText("未找到源文件！\n"); return; }
                if (!int.TryParse(txtOrigCreatureID.Text, System.Globalization.NumberStyles.HexNumber, null, out int cid)) { tbProcessAminidx.AppendText("无效的 ID。\n"); return; }
                if (!int.TryParse(txtNewCreatureID.Text, out int cc)) { tbProcessAminidx.AppendText("无效的副本数量。\n"); return; }
                string result = AnimationFile.CopyCreatureIdx(tbfilename.Text, cid, cc, msg => tbProcessAminidx.AppendText(msg + "\n"));
                tbProcessAminidx.AppendText(result + "\n");
                _newIdCount += cc; lblNewIdCount.Text = $"已创建的 ID：{_newIdCount:N0}";
            }
            catch (Exception ex) { tbProcessAminidx.AppendText($"错误：{ex.Message}\n"); }
        }

        private void BtnSingleEmptyAnimMul_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                using var _ = File.Create(Path.Combine(d.SelectedPath, "anim.mul"));
                tbProcessAminidx.AppendText($"空的 anim.mul：{d.SelectedPath}\n");
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  选项卡：Artmul
        // =====================================================================

        #region Artmul

        private void BtnCreateArtIdx_Click(object s, EventArgs e) { using var sfd = new SaveFileDialog { Filter = "MUL|*.mul" }; if (sfd.ShowDialog() != DialogResult.OK) return; try { infoARTIDXMULID.AppendText(MulFileHelper.ExtendIndex(sfd.FileName, 1_000_000) + "\n"); } catch (Exception ex) { infoARTIDXMULID.AppendText($"错误：{ex.Message}\n"); } }
        private void BtnCreateArtIdx100K_Click(object s, EventArgs e) => CreateArtIdxFixed(100_000);
        private void BtnCreateArtIdx150K_Click(object s, EventArgs e) => CreateArtIdxFixed(150_000);
        private void BtnCreateArtIdx200K_Click(object s, EventArgs e) => CreateArtIdxFixed(200_000);
        private void BtnCreateArtIdx250K_Click(object s, EventArgs e) => CreateArtIdxFixed(250_000);
        private void BtnCreateArtIdx500K_Click(object s, EventArgs e) => CreateArtIdxFixed(500_000);

        private void CreateArtIdxFixed(long entries)
        {
            using var sfd = new SaveFileDialog { Filter = "MUL|*.mul", Title = $"保存 artidx（{entries:N0} 个条目）" };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            try { infoARTIDXMULID.AppendText(MulFileHelper.ExtendIndex(sfd.FileName, entries) + "\n"); }
            catch (Exception ex) { infoARTIDXMULID.AppendText($"错误：{ex.Message}\n"); }
        }

        private void BtnReadArtIdx_Click(object s, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "MUL|*.mul" }; if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                var idx = new MulIndexFile(); idx.LoadFromFile(ofd.FileName);
                infoARTIDXMULID.AppendText(idx.GetSummary() + "\n" + ofd.FileName + "\n");
                lblIndexCount.Text = $"总计：{idx.Count:N0}";
            }
            catch (Exception ex) { infoARTIDXMULID.AppendText($"错误：{ex.Message}\n"); }
        }

        private void ReadArtmul2_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                string path = Path.Combine(d.SelectedPath, "artidx.mul");
                if (!File.Exists(path)) { MessageBox.Show("未找到 artidx.mul。"); return; }
                lblIndexCount.Text = $"artidx.mul：{new FileInfo(path).Length / 12:N0} 个条目";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void ReadArtmul_Click(object s, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "MUL|*.mul" }; if (ofd.ShowDialog() != DialogResult.OK) return;
            try { var idx = new MulIndexFile(); idx.LoadFromFile(ofd.FileName); infoARTIDXMULID.AppendText(idx.GetDetailedInfo(300)); }
            catch (Exception ex) { infoARTIDXMULID.AppendText($"错误：{ex.Message}\n"); }
        }

        private void BtnCreateNewArtidx(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                long count = MulFileHelper.ParseEntryCount(tbxNewIndex.Text, 65500, 1);
                MessageBox.Show(MulFileHelper.CreateIndexAndMul(d.SelectedPath, "artidx.mul", "art.mul", count));
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnCreateOldVersionArtidx(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                if (!int.TryParse(tbxNewIndex.Text, out int count) || count <= 0) { MessageBox.Show("请输入有效的数字。"); return; }
                byte[] bytes = new byte[count * 12];
                for (int i = 0; i < count; i++) { int o = i * 12; bytes[o] = bytes[o + 1] = bytes[o + 2] = bytes[o + 3] = 0xFF; }
                string path = Path.Combine(d.SelectedPath, "artidx.mul"); File.WriteAllBytes(path, bytes);
                MessageBox.Show($"旧版 artidx.mul：{count:N0} 个条目\n{path}");
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnCreateNewArtidx2(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                if (!int.TryParse(tbxNewIndex2.Text, out int total) || total <= 0 ||
                   !int.TryParse(tbxArtsCount.Text, out int arts) || arts < 0 ||
                   !int.TryParse(tbxLandTilesCount.Text, out int land) || land < 0 || arts + land != total)
                { MessageBox.Show("错误：物品数 + 地形数 必须等于总数。"); return; }
                MessageBox.Show(MulFileHelper.CreateIndexAndMul(d.SelectedPath, "artidx.mul", "art.mul", total) + $"\n\n物品：{arts:N0}  地形：{land:N0}");
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  选项卡：声音
        // =====================================================================

        #region 声音

        private void CreateOrgSoundMul_Click(object s, EventArgs e)
        {
            try
            {
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
                int count = int.TryParse(SoundIDXMul.Text, out int c) ? c : 4095;
                IndexSizeLabel.Text = SoundFile.CreateEmpty(folderBrowserDialog.SelectedPath, count);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void ReadIndexSize_Click(object s, EventArgs e)
        {
            try
            {
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
                var sf = new SoundFile(); int cnt = sf.LoadIndex(Path.Combine(folderBrowserDialog.SelectedPath, "SoundIdx.mul"));
                IndexSizeLabel.Text = $"SoundIdx.mul：{cnt:N0} 个条目";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  选项卡：Gump
        // =====================================================================

        #region Gump

        private void CreateGumpButton_Click(object s, EventArgs e)
        {
            try
            {
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
                if (!int.TryParse(IndexSizeTextBox.Text, out int size) || size <= 0) { MessageBox.Show("无效的大小。"); return; }
                GumpFile.CreateEmpty(folderBrowserDialog.SelectedPath, size);
                gumpLabel.Text = $"已创建 Gump：{size:N0}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void ReadGumpButton_Click(object s, EventArgs e)
        {
            try
            {
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
                gumpLabel.Text = $"Gump 条目数：{GumpFile.CountEntries(folderBrowserDialog.SelectedPath):N0}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  新增选项卡：Hues
        // =====================================================================

        #region Hues

        private void BtnCreateHues_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                string path = Path.Combine(d.SelectedPath, "hues.mul");
                HuesFile.CreateEmpty().SaveToFile(path);
                lblHuesOutput.Text = $"已创建 hues.mul：\n  {path}\n  {HuesFile.TotalEntries:N0} 个色调条目（{new FileInfo(path).Length:N0} 字节）";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnReadHues_Click(object s, EventArgs e)
        {
            try
            {
                using var ofd = new OpenFileDialog { Filter = "MUL|*.mul", Title = "打开 hues.mul" }; if (ofd.ShowDialog() != DialogResult.OK) return;
                var hf = new HuesFile(); hf.LoadFromFile(ofd.FileName);
                var sb = new StringBuilder(hf.GetSummary() + "\n\n前 20 个条目：\n");
                for (int i = 0; i < Math.Min(20, hf.Count); i++) sb.AppendLine($"  [{i,4}] {hf.Entries[i]}");
                lblHuesOutput.Text = sb.ToString();
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  新增选项卡：Map/Statics
        // =====================================================================

        #region Map / Statics

        private void PopulateMapSizeCombo()
        {
            if (comboMapSize == null) return;
            comboMapSize.Items.Clear();
            foreach (var (name, w, h) in MapFile.KnownSizes)
                comboMapSize.Items.Add($"{name}  ({w}×{h})");
            if (comboMapSize.Items.Count > 0) comboMapSize.SelectedIndex = 0;
        }

        private void ComboMapSize_SelectedIndexChanged(object s, EventArgs e)
        {
            int i = comboMapSize.SelectedIndex;
            if (i < 0 || i >= MapFile.KnownSizes.Length) return;
            var (_, w, h) = MapFile.KnownSizes[i];
            tbMapWidth.Text = w.ToString(); tbMapHeight.Text = h.ToString();
            lblMapSizeInfo.Text = MapFile.GetSizeInfo(w, h);
        }

        private void BtnCreateMap_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int w = ParseInt(tbMapWidth.Text, 7168), h = ParseInt(tbMapHeight.Text, 4096);
                int idx = ParseInt(tbMapIndex.Text, 0);
                string fn = $"map{idx}.mul";
                lblMapOutput.Text = MapFile.CreateEmpty(d.SelectedPath, fn, w, h);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnCreateStatics_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int w = ParseInt(tbMapWidth.Text, 7168), h = ParseInt(tbMapHeight.Text, 4096);
                int idx = ParseInt(tbMapIndex.Text, 0);
                lblMapOutput.Text = StaticsFile.CreateEmpty(d.SelectedPath, $"statics{idx}.mul", $"staidx{idx}.mul", w, h);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnCreateMapAndStatics_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int w = ParseInt(tbMapWidth.Text, 7168), h = ParseInt(tbMapHeight.Text, 4096);
                int idx = ParseInt(tbMapIndex.Text, 0);
                string r1 = MapFile.CreateEmpty(d.SelectedPath, $"map{idx}.mul", w, h);
                string r2 = StaticsFile.CreateEmpty(d.SelectedPath, $"statics{idx}.mul", $"staidx{idx}.mul", w, h);
                lblMapOutput.Text = r1 + "\n" + r2;
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void TbMapWidth_TextChanged(object s, EventArgs e)
        {
            if (int.TryParse(tbMapWidth.Text, out int w) && int.TryParse(tbMapHeight.Text, out int h) && w > 0 && h > 0)
                try { lblMapSizeInfo.Text = MapFile.GetSizeInfo(w, h); } catch { }
        }

        #endregion

        // =====================================================================
        //  新增选项卡：Multi
        // =====================================================================

        #region Multi

        private void BtnCreateMulti_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int count = ParseInt(tbMultiCount.Text, 5000);
                lblMultiOutput.Text = MultiFile.CreateEmpty(d.SelectedPath, count);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnReadMulti_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int total = MultiFile.CountEntries(d.SelectedPath);
                int idx = ParseInt(tbMultiIndex.Text, 0);
                bool hs = checkBoxMultiHS.Checked;
                var tiles = MultiFile.ReadMulti(d.SelectedPath, idx, hs);
                var sb = new StringBuilder($"multi.idx：{total:N0} 个条目\n\nMulti [{idx}] – {tiles.Count} 个图块：\n");
                foreach (var t in tiles) sb.AppendLine($"  {t}");
                lblMultiOutput.Text = sb.ToString();
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  新增选项卡：Skills
        // =====================================================================

        #region Skills

        private void BtnCreateDefaultSkills_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                SkillsFile.CreateDefault().SaveToFile(d.SelectedPath);
                lblSkillsOutput.Text = $"已创建标准技能（{SkillsFile.DefaultSkillNames.Length} 个技能）：\n  {d.SelectedPath}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnCreateEmptySkills_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                int count = ParseInt(tbSkillCount.Text, 58);
                SkillsFile.CreateEmpty(count).SaveToFile(d.SelectedPath);
                lblSkillsOutput.Text = $"已创建空的技能文件（{count} 个技能）：\n  {d.SelectedPath}";
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnReadSkills_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog(); if (d.ShowDialog() != DialogResult.OK) return;
                var sf = new SkillsFile(); sf.LoadFromFile(d.SelectedPath);
                textBoxSkillsInfo.Text = sf.GetSummary();
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  新增选项卡：Validator
        // =====================================================================

        #region Validator

        private void BtnValidate_Click(object s, EventArgs e)
        {
            try
            {
                using var ofd = new OpenFileDialog { Filter = "IDX|*.idx;*.mul", Title = "选择 IDX 文件" }; if (ofd.ShowDialog() != DialogResult.OK) return;
                string idxPath = ofd.FileName;
                string mulPath = Path.ChangeExtension(idxPath, ".mul");
                if (!File.Exists(mulPath))
                {
                    using var ofd2 = new OpenFileDialog { Filter = "MUL|*.mul", Title = "选择对应的 MUL 文件" };
                    if (ofd2.ShowDialog() == DialogResult.OK) mulPath = ofd2.FileName; else mulPath = null;
                }
                var result = MulValidator.Validate(idxPath, mulPath ?? string.Empty);
                textBoxValidatorOutput.Text = result.ToString();
                lblValidatorStatus.Text = result.IsHealthy ? "✓ 无错误" : "✗ " + result.BrokenEntries + " 个错误";
                lblValidatorStatus.ForeColor = result.IsHealthy ? Color.DarkGreen : Color.Red;
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnCompareDirectories_Click(object s, EventArgs e)
        {
            try
            {
                using var da = new FolderBrowserDialog { Description = "选择目录 A" }; if (da.ShowDialog() != DialogResult.OK) return;
                using var db = new FolderBrowserDialog { Description = "选择目录 B" }; if (db.ShowDialog() != DialogResult.OK) return;
                textBoxValidatorOutput.Text = MulValidator.CompareDirectories(da.SelectedPath, db.SelectedPath);
                lblValidatorStatus.Text = "比较完成。"; lblValidatorStatus.ForeColor = Color.Navy;
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  新增选项卡：IDX Patcher
        // =====================================================================

        #region IDX Patcher

        private void BtnPatchBrowseIdx_Click(object s, EventArgs e)
        { using var ofd = new OpenFileDialog { Filter = "IDX|*.idx;*.mul" }; if (ofd.ShowDialog() == DialogResult.OK) tbPatchIdxPath.Text = ofd.FileName; }

        private void BtnPatchEntry_Click(object s, EventArgs e)
        {
            try
            {
                if (!File.Exists(tbPatchIdxPath.Text)) { ShowWarn("未找到 IDX 文件。"); return; }
                if (!int.TryParse(tbPatchIndex.Text, out int idx)) { ShowWarn("无效的索引。"); return; }
                long lookup = tbPatchLookup.Text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                    ? Convert.ToInt64(tbPatchLookup.Text.Substring(2), 16)
                    : long.Parse(tbPatchLookup.Text);
                long size = long.Parse(tbPatchSize.Text);
                uint unk = uint.TryParse(tbPatchUnknown.Text, out uint u) ? u : 0;
                textBoxPatcherOutput.Text = IdxPatcher.PatchEntry(tbPatchIdxPath.Text, idx, lookup, size, unk);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnClearEntry_Click(object s, EventArgs e)
        {
            try
            {
                if (!File.Exists(tbPatchIdxPath.Text)) { ShowWarn("未找到 IDX 文件。"); return; }
                if (!int.TryParse(tbPatchIndex.Text, out int idx)) { ShowWarn("无效的索引。"); return; }
                textBoxPatcherOutput.Text = IdxPatcher.ClearEntry(tbPatchIdxPath.Text, idx);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnReadRange_Click(object s, EventArgs e)
        {
            try
            {
                if (!File.Exists(tbPatchIdxPath.Text)) { ShowWarn("未找到 IDX 文件。"); return; }
                int from = ParseInt(tbPatchRangeFrom.Text, 0);
                int count = ParseInt(tbPatchRangeCount.Text, 20);
                textBoxPatcherOutput.Text = IdxPatcher.ReadRange(tbPatchIdxPath.Text, from, count);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        #endregion

        // =====================================================================
        //  新增选项卡：Batch Setup
        // =====================================================================

        #region Batch Setup

        private async void BtnBatchCreate_Click(object s, EventArgs e)
        {
            try
            {
                using var d = new FolderBrowserDialog { Description = "所有分片文件的目标目录" }; if (d.ShowDialog() != DialogResult.OK) return;
                btnBatchCreate.Enabled = false;
                textBoxBatchLog.Clear();
                var opt = new BatchSetupOptions
                {
                    MapWidth = ParseInt(tbBatchMapW.Text, 7168),
                    MapHeight = ParseInt(tbBatchMapH.Text, 4096),
                    MapIndex = ParseInt(tbBatchMapIdx.Text, 0),
                    ArtEntries = ParseInt(tbBatchArtCount.Text, 81884),
                    SoundCount = ParseInt(tbBatchSoundCount.Text, 4095),
                    GumpCount = ParseInt(tbBatchGumpCount.Text, 65535),
                    MultiCount = ParseInt(tbBatchMultiCount.Text, 5000),
                    TileDataLandGroups = ParseInt(tbBatchTileLand.Text, 512),
                    TileDataStaticGroups = ParseInt(tbBatchTileStatic.Text, 2048),
                    CreateSkills = checkBoxBatchSkills.Checked,
                    CreateDefaultSkills = checkBoxBatchDefaultSkills.Checked,
                    SkillCount = ParseInt(tbBatchSkillCount.Text, 58),
                };
                string log = await Task.Run(() => BatchSetup.CreateAll(d.SelectedPath, opt,
                    msg => Invoke((Action)(() => textBoxBatchLog.AppendText(msg + "\r\n")))));
                textBoxBatchLog.Text = log;
                lblBatchStatus.Text = "完成！"; lblBatchStatus.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex) { textBoxBatchLog.AppendText($"错误：{ex.Message}\r\n"); lblBatchStatus.Text = "错误！"; lblBatchStatus.ForeColor = Color.Red; }
            finally { btnBatchCreate.Enabled = true; }
        }

        #endregion

        // =====================================================================
        //  新增选项卡：Hex Viewer
        // =====================================================================

        #region Hex Viewer

        private void BtnHexBrowse_Click(object s, EventArgs e)
        { using var ofd = new OpenFileDialog { Filter = "MUL|*.mul|所有文件|*.*" }; if (ofd.ShowDialog() == DialogResult.OK) { tbHexFilePath.Text = ofd.FileName; lblHexFileInfo.Text = HexViewHelper.GetFileInfo(ofd.FileName); } }

        private void BtnHexRead_Click(object s, EventArgs e)
        {
            try
            {
                if (!File.Exists(tbHexFilePath.Text)) { ShowWarn("文件未找到。"); return; }
                long offset = tbHexOffset.Text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                    ? Convert.ToInt64(tbHexOffset.Text.Substring(2), 16)
                    : long.Parse(tbHexOffset.Text);
                int length = ParseInt(tbHexLength.Text, 256);
                textBoxHexOutput.Text = HexViewHelper.ReadHex(tbHexFilePath.Text, offset, length);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnHexSearch_Click(object s, EventArgs e)
        {
            try
            {
                if (!File.Exists(tbHexFilePath.Text)) { ShowWarn("文件未找到。"); return; }
                string patternStr = tbHexPattern.Text.Replace(" ", "").Replace("-", "");
                if (patternStr.Length % 2 != 0 || patternStr.Length == 0) { ShowWarn("无效的十六进制模式（例如 FF00AB）。"); return; }
                byte[] pat = new byte[patternStr.Length / 2];
                for (int i = 0; i < pat.Length; i++) pat[i] = Convert.ToByte(patternStr.Substring(i * 2, 2), 16);
                textBoxHexOutput.Text = HexViewHelper.SearchPattern(tbHexFilePath.Text, pat);
            }
            catch (Exception ex) { ShowErr(ex); }
        }

        private void BtnHexFileInfo_Click(object s, EventArgs e)
        {
            if (!File.Exists(tbHexFilePath.Text)) { ShowWarn("文件未找到。"); return; }
            textBoxHexOutput.Text = HexViewHelper.GetFileInfo(tbHexFilePath.Text);
        }

        #endregion

        // =====================================================================
        //  辅助方法
        // =====================================================================

        #region 辅助方法

        private static int ParseInt(string s, int def) => (string.IsNullOrWhiteSpace(s) || !int.TryParse(s.Trim(), out int v)) ? def : v;

        private void DisposeReader() { try { _reader?.Dispose(); } catch { } _reader = null; }

        private static void ShowErr(Exception ex) => MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        private static void ShowWarn(string msg) => MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void InitUoPalette()
        {
            Color[] colors = {
                Color.FromArgb(0,0,0),       Color.FromArgb(0,0,128),     Color.FromArgb(0,128,0),
                Color.FromArgb(0,128,128),   Color.FromArgb(128,0,0),     Color.FromArgb(128,0,128),
                Color.FromArgb(128,128,0),   Color.FromArgb(192,192,192), Color.FromArgb(128,128,128),
                Color.FromArgb(0,0,255),     Color.FromArgb(0,255,0),     Color.FromArgb(0,255,255),
                Color.FromArgb(255,0,0),     Color.FromArgb(255,0,255),   Color.FromArgb(255,255,0),
                Color.FromArgb(255,255,255), Color.FromArgb(0,0,0),       Color.FromArgb(0,0,95),
                Color.FromArgb(0,0,127),     Color.FromArgb(0,0,159),     Color.FromArgb(0,0,191),
                Color.FromArgb(0,0,223),     Color.FromArgb(0,0,255),     Color.FromArgb(0,35,0),
                Color.FromArgb(0,71,0),      Color.FromArgb(0,107,0),     Color.FromArgb(0,143,0),
                Color.FromArgb(0,179,0),     Color.FromArgb(0,215,0),     Color.FromArgb(0,255,0),
                Color.FromArgb(0,63,63),     Color.FromArgb(0,127,127),   Color.FromArgb(23,23,23),
                Color.FromArgb(39,39,39),    Color.FromArgb(55,55,55),    Color.FromArgb(71,71,71),
                Color.FromArgb(87,87,87),    Color.FromArgb(103,103,103), Color.FromArgb(119,119,119),
                Color.FromArgb(135,135,135), Color.FromArgb(151,151,151), Color.FromArgb(167,167,167),
                Color.FromArgb(183,183,183), Color.FromArgb(199,199,199), Color.FromArgb(215,215,215),
                Color.FromArgb(231,231,231), Color.FromArgb(247,247,247), Color.FromArgb(255,255,255),
            };
            foreach (var c in colors) _palette.Add(c);
            while (_palette.Colors.Count < 256) { byte v = (byte)_palette.Colors.Count; _palette.Add(Color.FromArgb(v, v, v)); }
        }

        #endregion
    }
}