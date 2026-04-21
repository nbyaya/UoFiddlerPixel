// /***************************************************************************
//  *
//  * $Author: Nikodemus
//  * 
//  * "葡萄酒许可证"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯葡萄酒作为回报。
//  *
//  ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using AnimatedGif;
using System.Linq;
using System.Diagnostics;

namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    public partial class AnimationVDForm : Form
    {
        private List<Image> images = new List<Image>(10);
        private int animationSpeed = 500; // 默认速度（毫秒）
        private CancellationTokenSource cancellationTokenSource;
        private bool isPlaying = false; // 标志，指示动画是否正在播放

        public AnimationVDForm()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                images.Add(null); // 使用 null 值初始化列表
            }

            checkedListBoxAminID.SetItemChecked(0, true); // 启动时激活第一个复选框

            // 将 TrackBar 设置为默认值（中间值）
            trackBarSpeedAmin.Value = 3;

            // 以默认速度初始化标签
            UpdateLabelSpeed();

            // 订阅事件处理程序
            trackBarSpeedAmin.Scroll += TrackBarSpeedAmin_Scroll;
            checkedListBoxAminID.ItemCheck += CheckedListBoxAminID_ItemCheck; // CheckedListBox 的事件处理程序
        }

        #region [ CheckedListBoxAminID_ItemCheck ]
        private void CheckedListBoxAminID_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // 仅在动画未运行时显示图像
            if (!isPlaying && e.NewValue == CheckState.Checked)
            {
                // 取消选中所有其他复选框
                for (int i = 0; i < checkedListBoxAminID.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        checkedListBoxAminID.SetItemChecked(i, false);
                    }
                }

                var selectedIndex = e.Index;

                if (selectedIndex >= 0 && selectedIndex < images.Count && images[selectedIndex] != null)
                {
                    pictureBoxAminImage.Image = images[selectedIndex];
                    pictureBoxAminImage.Refresh();
                }
                else
                {
                    pictureBoxAminImage.Image = null;
                }
            }
            else if (isPlaying)
            {
                e.NewValue = e.CurrentValue; // 动画运行时阻止更改
            }
        }
        #endregion

        #region [ btLoadAminID ]
        private void btLoadAminID_Click(object sender, EventArgs e)
        {
            if (checkedListBoxAminID.SelectedIndex == -1)
            {
                checkedListBoxAminID.SelectedIndex = 0; // 如果未选中任何项，则将 SelectedIndex 设置为 0
            }

            if (checkedListBoxAminID.SelectedIndex >= 0 && checkedListBoxAminID.SelectedIndex < 10)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "图像文件|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(openFileDialog.FileName);
                    images[checkedListBoxAminID.SelectedIndex] = img;

                    // 如果动画未激活，立即显示加载的图像
                    if (!isPlaying)
                    {
                        pictureBoxAminImage.Image = img;
                        pictureBoxAminImage.Refresh();
                    }
                }
            }
        }
        #endregion

        #region [ btPlayAminID ]
        private async void btPlayAminID_Click(object sender, EventArgs e)
        {
            isPlaying = true; // 设置动画正在运行的标志
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            try
            {
                await Task.Run(async () =>
                {
                    do
                    {
                        foreach (var img in images)
                        {
                            if (img != null)
                            {
                                // 在尝试更新 PictureBox 之前，验证它仍然有效
                                if (pictureBoxAminImage != null && !pictureBoxAminImage.IsDisposed && pictureBoxAminImage.IsHandleCreated)
                                {
                                    pictureBoxAminImage.Invoke(new Action(() =>
                                    {
                                        if (!pictureBoxAminImage.IsDisposed) // 再次检查
                                        {
                                            pictureBoxAminImage.Image = img;
                                            pictureBoxAminImage.Refresh();
                                        }
                                    }));
                                }
                                await Task.Delay(animationSpeed, token); // 图像之间的等待时间
                            }
                        }
                    } while (checkBoxLoop.Checked && !token.IsCancellationRequested);
                }, token);
            }
            catch (TaskCanceledException)
            {
                // 任务已取消，无需操作
            }
            finally
            {
                isPlaying = false; // 动画结束
            }
        }
        #endregion

        #region [ TrackBarSpeedAmin_Scroll ]
        private void TrackBarSpeedAmin_Scroll(object sender, EventArgs e)
        {
            // 根据 TrackBar 的位置计算速度
            switch (trackBarSpeedAmin.Value)
            {
                case 1:
                    animationSpeed = 2000; // 最慢速度
                    break;
                case 2:
                    animationSpeed = 1000; // 慢速
                    break;
                case 3:
                    animationSpeed = 500; // 标准速度
                    break;
                case 4:
                    animationSpeed = 100; // 较快
                    break;
                case 5:
                    animationSpeed = 25; // 最快速度
                    break;
                default:
                    animationSpeed = 500; // 标准速度
                    break;
            }

            // 使用当前速度更新标签
            UpdateLabelSpeed();
        }
        #endregion

        #region [ UpdateLabelSpeed ]
        private void UpdateLabelSpeed()
        {
            labelSpeed.Text = $"速度：{animationSpeed} 毫秒";
        }
        #endregion

        #region [ btStopAminID ]
        private void btStopAminID_Click(object sender, EventArgs e)
        {
            StopAnimation(); // 安全停止动画
        }
        #endregion

        #region [ AnimationVDForm_FormClosing ]
        private async void AnimationVDForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果动画正在运行，先停止它
            if (isPlaying)
            {
                e.Cancel = true; // 取消关闭
                await Task.Run(() => StopAnimation()); // 停止动画
                this.Close(); // 再次关闭窗体
            }
        }
        #endregion

        #region [ StopAnimation ]
        private void StopAnimation()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                isPlaying = false;
            }
        }
        #endregion

        #region [ importImageToolStripMenuItem ]
        private void importImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                Image img = Clipboard.GetImage();

                if (checkedListBoxAminID.SelectedIndex >= 0 && checkedListBoxAminID.SelectedIndex < images.Count)
                {
                    images[checkedListBoxAminID.SelectedIndex] = img;

                    // 如果动画未激活，立即显示加载的图像
                    if (!isPlaying)
                    {
                        pictureBoxAminImage.Image = img;
                        pictureBoxAminImage.Refresh();
                    }
                }
                else
                {
                    MessageBox.Show("请选择一个有效的复选框。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("剪贴板上未找到图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region [ createGifToolStripMenuItem ]
        private void createGifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 检查是否加载了图像
            if (images.Count == 0 || images.All(img => img == null))
            {
                MessageBox.Show("未加载图像。请在导出 GIF 前加载图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 获取程序目录
            string programDirectory = Application.StartupPath;

            // 定义临时目录的路径
            string tempGraficDirectory = Path.Combine(programDirectory, "tempGrafic");

            // 如果目录不存在则创建
            if (!Directory.Exists(tempGraficDirectory))
            {
                Directory.CreateDirectory(tempGraficDirectory);
            }

            // 将默认输出路径设置为上次使用的目录
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "GIF 图像|*.gif",
                Title = "另存为 GIF",
                InitialDirectory = tempGraficDirectory,  // 使用 tempGrafic 目录作为默认目录
                FileName = "Animation.gif"  // 默认文件名
            };

            // 显示对话框，让用户设置位置和文件名
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string gifPath = saveFileDialog.FileName;

                // 根据 animationSpeed 定义延迟（毫秒）
                var delay = animationSpeed > 0 ? animationSpeed : 500;

                // 创建 GIF
                using (var gif = AnimatedGif.AnimatedGif.Create(gifPath, delay))
                {
                    foreach (var img in images)
                    {
                        if (img != null)
                        {
                            gif.AddFrame(img, delay: -1, quality: GifQuality.Bit8);
                        }
                    }
                }

                MessageBox.Show($"GIF 已成功保存至 {gifPath}。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region [ ButtonOpenTempGrafic ]
        private void ButtonOpenTempGrafic_Click_1(object sender, EventArgs e)
        {
            string programDirectory = Application.StartupPath;
            string directory = Path.Combine(programDirectory, "tempGrafic");

            // 检查目录是否存在
            if (Directory.Exists(directory))
            {
                Process.Start("explorer.exe", directory);
            }
            else
            {
                MessageBox.Show("tempGrafic 目录不存在。");
            }
        }
        #endregion

        #region [ btEmptyImages ]
        private void btEmptyImages_Click(object sender, EventArgs e)
        {
            // 从列表中删除所有图像并将其设置为 null
            for (int i = 0; i < images.Count; i++)
            {
                images[i]?.Dispose();  // 可选：释放图像以释放内存
                images[i] = null;  // 将列表中的图像位置设置为 null
            }

            // 清空 PictureBox
            pictureBoxAminImage.Image = null;
            pictureBoxAminImage.Refresh();  // 刷新 PictureBox 显示

            MessageBox.Show("所有图像已被移除。", "图片已清空", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}