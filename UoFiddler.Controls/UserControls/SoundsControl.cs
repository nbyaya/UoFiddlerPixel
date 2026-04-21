/***************************************************************************
 *
 * $Author: Turley
 * Advanced: Nikodemus
 * 
 * "啤酒-葡萄酒许可证"
 * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
 * 如果我们某天相遇，并且你认为这个东西有价值，
 * 你可以请我喝杯啤酒作为回报。
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Helpers;

namespace UoFiddler.Controls.UserControls
{
    public partial class SoundsControl : UserControl
    {
        private const int _soundsLength = 0xFFF;

        private System.Media.SoundPlayer _sp;
        private readonly Timer _spTimer;
        private int _spTimerMax;
        private DateTime _spTimerStart;

        private bool _playing;

        private bool _loaded;

        private int _soundIdOffset;

        #region SoundsControl
        public SoundsControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            _spTimer = new Timer();
            _spTimer.Tick += OnSpTimerTick;

            treeView.LabelEdit = true;
            treeView.BeforeLabelEdit += TreeView_BeforeLabelEdit;
            treeView.AfterLabelEdit += TreeViewOnAfterLabelEdit;

            _soundIdOffset = GetSoundIdOffset();
        }
        #endregion

        #region Reload
        /// <summary>
        /// 如果已加载，则重新加载
        /// </summary>
        public void Reload()
        {
            if (!_loaded)
            {
                return;
            }

            nameSortToolStripMenuItem.Checked = false;

            OnLoad(this, EventArgs.Empty);            
        }
        #endregion

        #region OnLoad
        private void OnLoad(object sender, EventArgs e)
        {
            if (IsAncestorSiteInDesignMode || FormsDesignerHelper.IsInDesignMode())
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            Options.LoadedUltimaClass["Sound"] = true;

            int? oldItem = null;

            if (treeView.SelectedNode != null)
            {
                oldItem = (int)treeView.SelectedNode.Tag;
            }

            treeView.BeginUpdate();
            try
            {
                treeView.Nodes.Clear();

                _soundIdOffset = GetSoundIdOffset();

                var cache = new List<TreeNode>();
                for (int i = 0; i < _soundsLength; ++i)
                {
                    if (Sounds.IsValidSound(i, out string name, out bool translated))
                    {
                        TreeNode node = new TreeNode($"0x{i + _soundIdOffset:X3} {name}")
                        {
                            Tag = i
                        };

                        if (translated)
                        {
                            node.ForeColor = Color.Blue;
                            node.NodeFont = new Font(Font, FontStyle.Underline);
                        }

                        cache.Add(node);
                    }
                    else if (showFreeSlotsToolStripMenuItem.Checked)
                    {
                        TreeNode node = new TreeNode($"0x{i:X3} ")
                        {
                            Tag = i,
                            ForeColor = Color.Red
                        };

                        cache.Add(node);
                    }
                }

                treeView.Nodes.AddRange(cache.ToArray());
            }
            finally
            {
                treeView.EndUpdate();
            }

            if (treeView.Nodes.Count > 0)
            {
                treeView.SelectedNode = treeView.Nodes[0];
            }

            _sp = new System.Media.SoundPlayer();
            if (!_loaded)
            {
                ControlEvents.FilePathChangeEvent += OnFilePathChangeEvent;
            }

            _loaded = true;
            _playing = false;

            Cursor.Current = Cursors.Default;

            if (oldItem != null)
            {
                SearchId(oldItem.Value);
            }

            // 在方法结束时更新ID计数
            UpdateIDCountLabel();
        }
        #endregion

        #region GetSoundIdOffset
        private static int GetSoundIdOffset()
        {
            return Options.PolSoundIdOffset ? 1 : 0;
        }
        #endregion

        #region OnSpTimerTick
        private void OnSpTimerTick(object sender, EventArgs eventArgs)
        {
            BeginInvoke((Action)(() =>
                {
                    TimeSpan diff = DateTime.Now - _spTimerStart;
                    playing.Value = Math.Min(100, (int)(diff.TotalMilliseconds * 100d / _spTimerMax));
                    SoundPlaytimeBar.Value = playing.Value;

                    if (diff.TotalMilliseconds < _spTimerMax)
                    {
                        return;
                    }

                    playing.Visible = false;
                    SoundPlaytimeBar.Value = 0;

                    stopButton.Visible = false;
                    StopSoundButton.Enabled = false;
                    _spTimer.Stop();
                }));
        }
        #endregion

        #region OnFilePathChangeEvent
        private void OnFilePathChangeEvent()
        {
            Reload();
        }
        #endregion

        #region OnClickPlay
        private void OnClickPlay(object sender, EventArgs e)
        {
            PlaySound((int)treeView.SelectedNode.Tag);
        }
        #endregion

        #region OnDoubleClick
        private void OnDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            PlaySound((int)e.Node.Tag);
        }
        #endregion

        #region  OnClickStop
        private void OnClickStop(object sender, EventArgs e)
        {
            // 如果正在播放声音，停止它
            StopSound();

            // 如果循环正在运行，停止它
            lock (_loopLock)
            {
                if (_looping)
                {
                    _looping = false;
                    if (_loopThread != null)
                    {
                        if (!_loopThread.Join(1000)) // 等待最多1秒让线程退出
                        {
                            // 线程没有及时结束，您可以在此处采取额外操作
                        }
                        _loopThread = null;
                    }
                }
            }
        }
        #endregion

        #region StopSound
        private void StopSound()
        {
            _sp.Stop();
            _spTimer.Stop();
            _playing = false;
            playing.Visible = false;
            SoundPlaytimeBar.Value = 0;
            stopButton.Visible = false;
            StopSoundButton.Enabled = false;
        }
        #endregion

        #region PlaySound
        private void PlaySound(int id)
        {
            _sp.Stop();
            _spTimer.Stop();
            _playing = false;
            playing.Visible = false;
            SoundPlaytimeBar.Value = 0;
            stopButton.Visible = false;
            StopSoundButton.Enabled = false;

            if (treeView.SelectedNode == null)
            {
                return;
            }

            UoSound sound = Sounds.GetSound(id);
            if (sound == null)
            {
                return;
            }

            using (MemoryStream mStream = new MemoryStream(sound.Buffer))
            {
                _sp.Stream = mStream;
                _sp.Play();

                playing.Value = 0;
                playing.Visible = true;
                SoundPlaytimeBar.Value = 0;
                stopButton.Visible = true;
                StopSoundButton.Enabled = true;
                _spTimerStart = DateTime.Now;
                _spTimerMax = (int)(Sounds.GetSoundLength(id) * 1000);
                _spTimer.Interval = 50;
                _spTimer.Start();

                _playing = true;
            }
        }
        #endregion

        #region BeforeSelect
        private void BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_playing)
            {
                StopSound();
            }
        }
        #endregion

        #region AfterSelect
        private void AfterSelect(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null)
            {
                playSoundToolStripMenuItem.Enabled = false;
                extractSoundToolStripMenuItem.Enabled = false;
                removeSoundToolStripMenuItem.Enabled = false;
                replaceToolStripMenuItem.Enabled = false;
                replaceToolStripMenuItem.Text = "插入/替换";
            }

            if (treeView.SelectedNode != null)
            {
                double length = Sounds.GetSoundLength((int)treeView.SelectedNode.Tag);
                seconds.Text = length > 0 ? $"{length:f}s" : "空槽位";
            }

            bool isValidSound = treeView.SelectedNode != null && Sounds.IsValidSound((int)treeView.SelectedNode.Tag, out _, out _);

            playSoundToolStripMenuItem.Enabled = isValidSound;
            extractSoundToolStripMenuItem.Enabled = isValidSound;
            removeSoundToolStripMenuItem.Enabled = isValidSound;

            replaceToolStripMenuItem.Enabled = true;
            replaceToolStripMenuItem.Text = isValidSound ? "替换" : "插入";

            SelectedSoundGroup.Visible = treeView.SelectedNode != null;

            if (treeView.SelectedNode != null)
            {
                SelectedSoundGroup.Text = $"当前声音: {treeView.SelectedNode.Text} - 时长: {seconds.Text}";
                IdInsertTextbox.Text = $"0x{(int)treeView.SelectedNode.Tag + _soundIdOffset:X}";
            }
        }
        #endregion

        #region OnChangeSort
        private void OnChangeSort(object sender, EventArgs e)
        {
            if (showFreeSlotsToolStripMenuItem.Checked)
            {
                showFreeSlotsToolStripMenuItem.Checked = false;
                nextFreeSlotToolStripMenuItem.Enabled = false;
                Reload();
                nameSortToolStripMenuItem.Checked = true;
            }

            int? oldItem = null;
            if (treeView.SelectedNode != null)
            {
                oldItem = (int)treeView.SelectedNode.Tag;
            }

            const string delimiter = " ";

            treeView.BeginUpdate();

            for (int i = 0; i < treeView.Nodes.Count; ++i)
            {
                string name = treeView.Nodes[i].Text;

                int splitIndex = nameSortToolStripMenuItem.Checked
                    ? name.IndexOf(delimiter, StringComparison.Ordinal)
                    : name.LastIndexOf(delimiter, StringComparison.Ordinal);

                treeView.Nodes[i].Text = $"{name.Substring(splitIndex).Trim()} {name.Substring(0, splitIndex).Trim()}";
            }

            treeView.Sort();
            treeView.EndUpdate();

            if (oldItem != null)
            {
                SearchId(oldItem.Value);
            }
        }
        #endregion

        #region DoSearchName
        private void DoSearchName(string name, bool next, bool prev)
        {
            int index = 0;

            if (prev)
            {
                if (treeView.SelectedNode.Index >= 0)
                {
                    index = treeView.SelectedNode.Index - _soundIdOffset;
                }

                if (index <= 0)
                {
                    index = 0;
                }

                for (int i = index - 1; i >= 0; --i)
                {
                    TreeNode node = treeView.Nodes[i];
                    if (!node.Text.ContainsCaseInsensitive(name))
                    {
                        continue;
                    }

                    treeView.SelectedNode = node;

                    node.EnsureVisible();

                    return;
                }
            }
            else
            {
                if (next)
                {
                    if (treeView.SelectedNode.Index >= 0)
                    {
                        index = treeView.SelectedNode.Index + 1;
                    }

                    if (index >= treeView.Nodes.Count)
                    {
                        index = 0;
                    }
                }

                for (int i = index; i < treeView.Nodes.Count; ++i)
                {
                    TreeNode node = treeView.Nodes[i];
                    if (!node.Text.ContainsCaseInsensitive(name))
                    {
                        continue;
                    }

                    treeView.SelectedNode = node;

                    node.EnsureVisible();

                    return;
                }
            }
        }
        #endregion

        #region OnClickExtract
        private void OnClickExtract(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null)
            {
                return;
            }

            int id = (int)treeView.SelectedNode.Tag;

            Sounds.IsValidSound(id, out string name, out _);

            string fileName = Path.Combine(Options.OutputPath, $"{name}");

            if (!fileName.EndsWith(".wav"))
            {
                fileName += ".wav";
            }

            using (MemoryStream stream = new MemoryStream(Sounds.GetSound(id).Buffer))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    stream.WriteTo(fs);
                }
            }

            MessageBox.Show($"声音已保存至 {fileName}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
        #endregion

        #region OnClickSave(
        private void OnClickSave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string path = Options.OutputPath;
            Sounds.Save(path);
            Cursor.Current = Cursors.Default;
            MessageBox.Show($"已保存至 {path}", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            Options.ChangedUltimaClass["Sound"] = false;
        }
        #endregion

        #region OnClickRemove
        private void OnClickRemove(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null)
            {
                return;
            }

            int id = (int)treeView.SelectedNode.Tag;

            DialogResult result = MessageBox.Show($"确定要移除 {treeView.SelectedNode.Text} 吗？", "移除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
            {
                return;
            }

            Sounds.Remove(id);

            if (!showFreeSlotsToolStripMenuItem.Checked)
            {
                treeView.SelectedNode.Remove();
            }
            else
            {
                treeView.SelectedNode.Text = $"0x{(int)treeView.SelectedNode.Tag + _soundIdOffset:X3}";
                treeView.SelectedNode.ForeColor = Color.Red;
            }

            AfterSelect(this, e);
            Options.ChangedUltimaClass["Sound"] = true;

            // 在方法结束时更新ID计数
            UpdateIDCountLabel();
        }
        #endregion

        #region OnClickExportSoundListCsv
        private void OnClickExportSoundListCsv(object sender, EventArgs e)
        {
            string fileName = Path.Combine(Options.OutputPath, "SoundList.csv");

            Sounds.SaveSoundListToCsv(fileName, _soundIdOffset);

            MessageBox.Show($"声音列表已保存至 {fileName}", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
        #endregion

        #region SearchId
        public bool SearchId(int id)
        {
            for (int i = 0; i < treeView.Nodes.Count; ++i)
            {
                TreeNode node = treeView.Nodes[i];

                if ((int)node.Tag != id)
                {
                    continue;
                }

                treeView.SelectedNode = node;
                node.EnsureVisible();

                return true;
            }

            return false;
        }
        #endregion

        #region ShowFreeSlotsClick
        private void ShowFreeSlotsClick(object sender, EventArgs e)
        {
            Reload();

            nextFreeSlotToolStripMenuItem.Enabled = showFreeSlotsToolStripMenuItem.Checked;
        }
        #endregion

        #region OnClickReplace
        private void OnClickReplace(object sender, EventArgs e)
        {
            string file;
            if (sender != null)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "选择波形文件";
                    dialog.CheckFileExists = true;
                    dialog.Filter = "wav 文件 (*.wav)|*.wav";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        file = dialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                file = _wavChosen;
            }

            // 在访问前检查“file”是否为空
            if (file == null)
            {
                MessageBox.Show("未选择文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = (int)treeView.SelectedNode.Tag;
            string name = Path.GetFileName(file);

            if (!File.Exists(file))
            {
                MessageBox.Show("无效的文件名", "添加/替换", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            if (name.Length > 32)
            {
                name = name.Substring(0, 32);
            }

            if (Sounds.IsValidSound(id, out _, out _))
            {
                DialogResult result = MessageBox.Show($"确定要替换 {treeView.SelectedNode.Text} 吗？",
                    "替换", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            try
            {
                Sounds.Add(id, name, file);
            }
            catch (WaveFormatException waveFormatException)
            {
                MessageBox.Show("不支持的 WAV 格式：\n" + waveFormatException.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TreeNode node = new TreeNode($"0x{id + _soundIdOffset:X3} {name}");

            if (nameSortToolStripMenuItem.Checked)
            {
                node.Text = $"{name} 0x{id + _soundIdOffset:X3}";
            }

            node.Tag = id;

            bool done = false;

            for (int i = 0; i < treeView.Nodes.Count; ++i)
            {
                if ((int)treeView.Nodes[i].Tag != id)
                {
                    continue;
                }

                done = true;

                treeView.Nodes.RemoveAt(i);
                treeView.Nodes.Insert(i, node);

                break;
            }

            if (!done)
            {
                treeView.Nodes.Add(node);
                treeView.Sort();
            }

            node.EnsureVisible();

            treeView.SelectedNode = node;
            treeView.Invalidate();

            Options.ChangedUltimaClass["Sound"] = true;

            // 在方法结束时更新ID计数
            UpdateIDCountLabel();
        }
        #endregion

        #region NextFreeSlotToolStripMenuItem
        private void NextFreeSlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = treeView.Nodes.IndexOf(treeView.SelectedNode) + 1; i < treeView.Nodes.Count; ++i)
            {
                TreeNode node = treeView.Nodes[i];

                if (Sounds.IsValidSound((int)node.Tag, out _, out _))
                {
                    continue;
                }

                treeView.SelectedNode = node;

                node.EnsureVisible();

                return;
            }
        }
        #endregion

        #region TreeView_KeyDown 
        private void TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            // 如果按下了 Ctrl+E 组合键并且 TreeView 中选中了一个节点，
            // 则将选中的节点置于编辑模式
            if (e.Control && e.KeyCode == Keys.E)
            {
                if (treeView.SelectedNode == null)
                {
                    return;
                }

                treeView.SelectedNode.BeginEdit();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // 如果按下了 Enter 键并且当前没有节点正在编辑，
                // 则调用 OnClickPlay 事件处理程序，播放选中的声音
                if (treeView.Nodes.OfType<TreeNode>().Any(n => n.IsEditing))
                {
                    return;
                }

                OnClickPlay(this, e);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F && e.Control)
            {
                // 如果按下了 Ctrl+F 组合键，则将焦点设置到 SearchNameTextbox，
                // 允许用户立即开始输入搜索查询
                SearchNameTextbox.Focus();

                e.SuppressKeyPress = true;
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Space)
            {
                // 如果按下了空格键，则检查是否有声音正在播放
                if (_playing)
                {
                    // 如果有声音正在播放，则停止声音
                    _sp.Stop();
                    _playing = false;
                }
                else
                {
                    // 如果没有声音正在播放，则播放选中的声音
                    if (treeView.SelectedNode != null)
                    {
                        int id = (int)treeView.SelectedNode.Tag;
                        PlaySound(id);
                    }
                }
            }
        }
        #endregion

        #region TreeViewOnAfterLabelEdit
        private void TreeViewOnAfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            int id = (int)e.Node.Tag;

            UoSound sound = Sounds.GetSound(id);

            if (sound != null && e.Label != null)
            {
                string newName = e.Label;
                if (newName.Length > 32)
                {
                    newName = newName.Substring(0, 32);
                }

                string oldName = sound.Name;
                sound.Name = newName;
                if (oldName != newName)
                {
                    Options.ChangedUltimaClass["Sound"] = true;
                }
            }

            Sounds.IsValidSound(id, out string name, out _);

            e.Node.Text = $"0x{id + _soundIdOffset:X3} {name}";

            if (nameSortToolStripMenuItem.Checked)
            {
                e.Node.Text = $"{name} 0x{id + _soundIdOffset:X3}";
            }

            e.CancelEdit = true;
        }
        #endregion

        #region TreeView_BeforeLabelEdit
        private void TreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            int id = (int)e.Node.Tag;

            if (Sounds.IsValidSound(id, out string name, out bool translated) && !translated)
            {
                treeView.SetEditText(name);
            }
            else
            {
                e.CancelEdit = true;
            }
        }
        #endregion

        #region WavChooseInsertButton
        private string _wavChosen;

        private void WavChooseInsertButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = "选择波形文件";
                dialog.CheckFileExists = true;
                dialog.Filter = "wav 文件 (*.wav)|*.wav";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _wavChosen = dialog.FileName;
                    WavFileInsertTextbox.Text = _wavChosen;
                }
            }
        }
        #endregion

        #region AddInsertReplaceButton
        private void AddInsertReplaceButton_Click(object sender, EventArgs e)
        {
            OnClickReplace(null, e);
        }
        #endregion

        #region SearchByIdButton
        private void SearchByIdButton_Click(object sender, EventArgs e)
        {
            if (!Utils.ConvertStringToInt(SearchNameTextbox.Text, out int id))
            {
                return;
            }

            if (!SearchId(id))
            {
                MessageBox.Show($"找不到 ID 为 {SearchNameTextbox.Text} 的声音？");
            }
        }
        #endregion

        #region SearchByNameButton
        private void SearchByNameButton_Click(object sender, EventArgs e)
        {
            DoSearchName(SearchNameTextbox.Text, false, false);
        }
        #endregion

        #region GoNextResultButton
        private void GoNextResultButton_Click(object sender, EventArgs e)
        {
            DoSearchName(SearchNameTextbox.Text, true, false);
        }
        #endregion

        #region GoPrevResultButton
        private void GoPrevResultButton_Click(object sender, EventArgs e)
        {
            DoSearchName(SearchNameTextbox.Text, false, true);
        }
        #endregion

        #region ExportAllSoundsToolStripMenuItem
        private void ExportAllSoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportAllSounds();
        }
        #endregion

        #region ExportAllSoundsButton
        private void ExportAllSoundsButton_Click(object sender, EventArgs e)
        {
            ExportAllSounds();
        }
        #endregion

        #region ExportAllSounds
        private void ExportAllSounds()
        {
            for (int i = 0; i < _soundsLength; ++i)
            {
                if (!Sounds.IsValidSound(i, out string name, out _))
                {
                    continue;
                }

                string fileName = includeSoundIdCheckBox.Checked
                    ? $"0x{i:X4} {name}"
                    : $"{name}";

                string path = Path.Combine(Options.OutputPath, fileName);

                if (!path.EndsWith(".wav"))
                {
                    path += ".wav";
                }

                using (MemoryStream stream = new MemoryStream(Sounds.GetSound(i).Buffer))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        stream.WriteTo(fs);
                    }
                }
            }

            MessageBox.Show("所有声音提取完成。", "已保存", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
        #endregion

        #region loopToolStripMenuItem
        private System.Threading.Thread _loopThread;
        private bool _looping;
        private readonly object _loopLock = new object();

        private void loopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 确保选中了一个声音
            if (treeView.SelectedNode == null)
            {
                return;
            }

            int id = (int)treeView.SelectedNode.Tag;
            UoSound sound = Sounds.GetSound(id);
            if (sound == null)
            {
                return;
            }

            // 如果已经有循环正在运行，则停止它
            lock (_loopLock)
            {
                if (_looping)
                {
                    _looping = false;
                    _spTimer.Stop();
                    if (_loopThread != null)
                    {
                        if (!_loopThread.Join(1000)) // 等待最多1秒让线程退出
                        {
                            // 线程没有及时结束，您可以在此处采取额外操作
                        }
                        _loopThread = null;
                    }
                    return;
                }
            }

            // 如果当前有声音正在播放，则停止它
            if (_playing)
            {
                StopSound();
            }

            // 启动一个新线程来循环播放声音
            _loopThread = new System.Threading.Thread(() =>
            {
                try
                {
                    using (MemoryStream mStream = new MemoryStream(sound.Buffer))
                    {
                        _sp.Stream = mStream;
                        lock (_loopLock)
                        {
                            _looping = true;
                        }
                        while (_looping)
                        {
                            _sp.PlaySync();
                            // 当声音重新开始时重置计时器并启动
                            this.Invoke((MethodInvoker)delegate
                            {
                                _spTimerStart = DateTime.Now;
                                _spTimer.Start();
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 适当处理错误
                    Console.WriteLine(ex);
                }
            });
            _loopThread.Start();

            _playing = true;
        }
        #endregion

        #region UpdateIDCountLabel
        private void UpdateIDCountLabel()
        {
            // 可能的音效 ID 总数为 4095，目前是手动值，看看是否可以增加 mul
            int totalIDs = 0xFFE; // 替换为实际总数

            // 正在使用的音效 ID 数量
            int occupiedIDs = 0;
            foreach (TreeNode node in treeView.Nodes)
            {
                if (Sounds.IsValidSound((int)node.Tag, out _, out _))
                {
                    occupiedIDs++;
                }
            }

            // 空闲的音效 ID 数量
            int freeIDs = totalIDs - occupiedIDs;

            // 更新标签
            IDCount.Text = $"已占用音效 ID: {occupiedIDs}, 空闲音效 ID: {freeIDs}";
        }
        #endregion        
    }
}