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
using System.IO;
using System.Windows.Forms;
using UoFiddler.Controls.Classes;
using UoFiddler.Classes;
using System.Runtime.InteropServices;
using System.Media;

namespace UoFiddler.Forms
{
    public partial class LoadProfileForm : Form
    {
        private readonly string[] _profiles;

        private const int HOTKEY_ID = 1;

        [DllImport("user32.dll")] // 注册热键 (Ctrl + Alt + P)
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")] // 注册热键 (Ctrl + Alt + P)
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        public LoadProfileForm()
        {
            InitializeComponent();
            Icon = Options.GetFiddlerIcon();
            _profiles = GetProfiles();
            FiddlerOptions.Logger.Information("找到配置文件: {Profiles}", _profiles);
            foreach (string profile in _profiles)
            {
                string name = profile.Substring(8);
                comboBoxLoad.Items.Add(name);
                comboBoxBasedOn.Items.Add(name);
            }
            string lastSelectedProfile = Properties.Settings.Default.LastSelectedProfile; // 配置文件加载
            int index = Array.IndexOf(_profiles, lastSelectedProfile);
            comboBoxLoad.SelectedIndex = index != -1 ? index : 0;
            comboBoxBasedOn.SelectedIndex = 0;
            
            RegisterHotKey(this.Handle, HOTKEY_ID, 0x0003, (uint)Keys.P); // 注册热键 (Ctrl + Alt + P)
        }

        #region [ WndProc ]
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                BringToFrontAndCenter();
            }
            base.WndProc(ref m);
        }
        #endregion

        #region [ BringToFrontAndCenter ]
        private void BringToFrontAndCenter()
        {
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BringToFront();
            this.Activate();

            // 设置插入后播放音效
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = "sound.wav";
            player.Play();
        }
        #endregion

        #region [ OnFormClosed ]
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            base.OnFormClosed(e);
        }
        #endregion

        #region [ GetProfiles ]
        private static string[] GetProfiles()
        {
            string[] files = Directory.GetFiles(Options.AppDataPath, "Options_*.xml", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < files.Length; i++)
            {
                string[] path = files[i].Split(Path.DirectorySeparatorChar);
                files[i] = path[path.Length - 1];
                files[i] = files[i].Substring(0, files[i].Length - 4);
            }

            return files;
        }
        #endregion

        #region [ OnClickLoad ]
        private void OnClickLoad(object sender, EventArgs e)
        {
            LoadSelectedProfile();
            Properties.Settings.Default.LastSelectedProfile = _profiles[comboBoxLoad.SelectedIndex];
            Properties.Settings.Default.Save(); // 配置文件保存属性
        }
        #endregion

        #region [ LoadSelectedProfile ]
        private void LoadSelectedProfile()
        {
            if (comboBoxLoad.SelectedIndex == -1)
            {
                return;
            }

            Options.ProfileName = $"{_profiles[comboBoxLoad.SelectedIndex]}.xml";
            FiddlerOptions.Logger.Information("加载配置文件: {ProfileName}", Options.ProfileName);
            FiddlerOptions.LoadProfile($"{_profiles[comboBoxLoad.SelectedIndex]}.xml");

            Close();
        }
        #endregion

        #region [ OnClickCreate ]
        private void OnClickCreate(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCreate.Text))
            {
                MessageBox.Show("缺少配置文件名称", "新建配置文件", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Options.ProfileName = $"Options_{textBoxCreate.Text}.xml";
            FiddlerOptions.Logger.Information("创建配置文件: {ProfileName}", Options.ProfileName);
            FiddlerOptions.LoadProfile($"{_profiles[comboBoxBasedOn.SelectedIndex]}.xml");

            Close();
        }
        #endregion

        #region [ ComboBoxLoad_KeyDown ]
        private void ComboBoxLoad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadSelectedProfile();
            }
        }
        #endregion

        #region [ ComboBoxLoad_SelectedIndexChanged ]
        private void ComboBoxLoad_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = comboBoxLoad.SelectedIndex != -1;
        }
        #endregion

        #region [ ComboBoxLoad_KeyUp ]
        private void ComboBoxLoad_KeyUp(object sender, KeyEventArgs e)
        {
            button1.Enabled = comboBoxLoad.SelectedIndex != -1;
        }
        #endregion

        #region [ LoadProfile_FormClosed ]
        private void LoadProfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
            {
                Application.Exit();
            }
        }
        #endregion

        #region [ 删除条目 ]
        private void bt_Delete_List_Click(object sender, EventArgs e)
        {
            if (comboBoxBasedOn.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要删除的条目。", "删除条目", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string profileName = comboBoxBasedOn.SelectedItem.ToString();
                string profilePath = Path.Combine(Options.AppDataPath, $"Options_{profileName}.xml");
                if (File.Exists(profilePath))
                {
                    File.Delete(profilePath);
                }
                comboBoxBasedOn.Items.RemoveAt(comboBoxBasedOn.SelectedIndex);
            }
        }
        #endregion

        #region [ class ProfileManager ]
        public class ProfileManager
        {
            private const string LastSelectedProfileKey = "LastSelectedProfile";
            
            public string LastSelectedProfile
            {
                get
                {
                    if (Properties.Settings.Default[LastSelectedProfileKey] is string profile)
                    {
                        return profile;
                    }
                    return null;
                }
                set
                {
                    Properties.Settings.Default[LastSelectedProfileKey] = value;
                    Properties.Settings.Default.Save();
                }
            }
        }
        #endregion
    }
}