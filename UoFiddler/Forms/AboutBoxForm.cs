/***************************************************************************
 *
 * $Author: Turley
 * Advanced Nikodemus
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
using System.Windows.Forms;
using UoFiddler.Classes;
using UoFiddler.Controls.Classes;
using System.Drawing;
using System.Net.Http;

namespace UoFiddler.Forms
{
    public partial class AboutBoxForm : Form
    {        
        private Timer animationTimer;
        private Random random;
        private List<Label> labels;
        private List<string> specialWords = new List<string>
        {
            "Code", "Nikodemus", "Matrix", "Ultima", "Turley", "Ares", "AsYlum", "MuadDib", "Nibbio", "Soulblighter", "Andreew", "Online"
        };

        #region AboutBoxForm
        public AboutBoxForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // 启用双缓冲
            Icon = Options.GetFiddlerIcon();
            checkBoxCheckOnStart.Checked = FiddlerOptions.UpdateCheckOnStart;
            checkBoxFormState.Checked = FiddlerOptions.StoreFormState;
            labels = new List<Label>();
            this.Load += AboutBoxForm_Load;
        }
        #endregion

        #region AboutBoxForm_Load
        private void AboutBoxForm_Load(object sender, EventArgs e)
        {
            InitializeAnimation();
        }
        #endregion

        #region InitializeAnimation()
        private void InitializeAnimation()
        {
            animationTimer = new Timer
            {
                Interval = 50
            };
            
            random = new Random();
            
            for (int i = 0; i < 100; i++)
            {
                var label = new Label
                {
                    ForeColor = Color.Green,
                    Font = new Font("Courier New", 14, FontStyle.Bold),
                    Text = GetRandomCharacter(),
                    Location = new Point(random.Next(animationPanel.Width), random.Next(animationPanel.Height))
                };
                labels.Add(label);
                animationPanel.Controls.Add(label);
            }
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }
        #endregion

        #region GetRandomCharacter()
        private string GetRandomCharacter()
        {
            // 偶尔返回一个特殊单词
            if (random.Next(200) == 0) // 增加此数字可降低特殊单词的出现频率
            {
                return specialWords[random.Next(specialWords.Count)];
            }

            // 否则返回一个随机字符
            return ((char)random.Next(33, 127)).ToString();
        }
        #endregion

        #region AnimationTimer_Tick
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            foreach (var label in labels)
            {
                label.Top += 10;
                
                if (label.Top > animationPanel.Height)
                {
                    label.Top = 0;
                    label.Left = random.Next(animationPanel.Width);
                    label.Text = GetRandomCharacter();
                }
            }
        }
        #endregion

        #region OnChangeCheck
        private void OnChangeCheck(object sender, EventArgs e)
        {
            FiddlerOptions.UpdateCheckOnStart = checkBoxCheckOnStart.Checked;
        }
        #endregion

        #region OnClickUpdate
        /*private async void OnClickUpdate(object sender, EventArgs e)
        {
            await UpdateRunner.RunAsync(FiddlerOptions.RepositoryOwner, FiddlerOptions.RepositoryName, FiddlerOptions.AppVersion).ConfigureAwait(false);
        }*/

        private async void OnClickUpdate(object sender, EventArgs e)
        {
            try
            {
                await UpdateRunner.RunAsync(FiddlerOptions.RepositoryOwner, FiddlerOptions.RepositoryName, FiddlerOptions.AppVersion).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                // 记录详细的错误信息
                Console.WriteLine($"请求错误: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"内部异常: {ex.InnerException.Message}");
                }
                MessageBox.Show("检查更新时发生错误。请检查您的网络连接后重试。", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // 记录其他可能的错误
                Console.WriteLine($"意外错误: {ex.Message}");
                MessageBox.Show("发生意外错误。请稍后重试。", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region OnClickLink Old version 4.8
        private void OnClickLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "http://uofiddler.polserver.com/",
                UseShellExecute = true
            });
        }
        #endregion

        #region OnChangeFormState
        private void OnChangeFormState(object sender, EventArgs e)
        {
            FiddlerOptions.StoreFormState = checkBoxFormState.Checked;
        }
        #endregion

        #region linkLabel2
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/jedi661/UoFiddlerPixel/",
                UseShellExecute = true
            });
        }
        #endregion

        #region ShowRepoInfoButton
        private void ShowRepoInfoButton_Click(object sender, EventArgs e)
        {
            string repoOwner = FiddlerOptions.RepositoryOwner;
            string repoName = FiddlerOptions.RepositoryName;

            MessageBox.Show($"仓库所有者: {repoOwner}\n仓库名称: {repoName}", "仓库信息"); // 来自 FiddlerOptions
        }
        #endregion
    }
}