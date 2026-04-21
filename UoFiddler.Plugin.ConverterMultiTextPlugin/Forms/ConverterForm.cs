// /***************************************************************************
//  *
//  * $Author: Nikodemus
//  * 
//  * 
//  * \"啤酒-葡萄酒许可证\"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯啤酒和葡萄酒作为回报。
//  *
//  ***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;


namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    public partial class ConverterForm : Form
    {
        public ConverterForm()
        {
            InitializeComponent();
        }

        #region 将黑色转换为自定义颜色
        private void BtConverterBlack_Click(object sender, EventArgs e)
        {
            ConvertColor(Color.White, Color.Black, "black");
        }
        #endregion

        #region 将白色转换为自定义颜色
        private void BtConverterWhite_Click(object sender, EventArgs e)
        {
            ConvertColor(Color.Black, Color.White, "white");
        }
        #endregion

        #region 自定义颜色转换
        private void BtConverterCustom_Click(object sender, EventArgs e)
        {
            using var colorDialog = new ColorDialog();
            // 从应用程序设置中加载自定义颜色
            string customColorsSetting = Properties.Settings.Default.CustomColors;
            if (!string.IsNullOrEmpty(customColorsSetting))
            {
                int[] customColors = customColorsSetting.Split(',').Select(int.Parse).ToArray();
                colorDialog.CustomColors = customColors;
            }

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color newColor = colorDialog.Color;
                string folderName = $"custom_{newColor.R}_{newColor.G}_{newColor.B}";

                ConvertColor(Color.Black, newColor, folderName);
                ConvertColor(Color.White, newColor, folderName);

                // 将自定义颜色保存到应用程序设置中
                customColorsSetting = string.Join(",", colorDialog.CustomColors);
                Properties.Settings.Default.CustomColors = customColorsSetting;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region 颜色转换方法
        private static void ConvertColor(Color fromColor, Color toColor, string folderName)
        {
            using var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string directoryPath = fbd.SelectedPath;
                string newDirectoryPath = Path.Combine(directoryPath, folderName);
                // 如果新目录不存在则创建
                if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }

                int count = 0; // 已处理图像计数器

                foreach (var filePath in Directory.GetFiles(directoryPath))
                {
                    string extension = Path.GetExtension(filePath).ToLower();
                    if (extension == ".bmp" || extension == ".png" || extension == ".jpg" || extension == ".tiff")
                    {
                        using var img = Image.FromFile(filePath);
                        for (int y = 0; y < img.Height; y++)
                        {
                            for (int x = 0; x < img.Width; x++)
                            {
                                Color pixelColor = ((Bitmap)img).GetPixel(x, y);
                                if (pixelColor.R == fromColor.R && pixelColor.G == fromColor.G && pixelColor.B == fromColor.B)
                                {
                                    ((Bitmap)img).SetPixel(x, y, toColor);
                                }
                            }
                        }
                        // 将图像保存到新目录
                        string newFilePath = Path.Combine(newDirectoryPath, Path.GetFileName(filePath));
                        img.Save(newFilePath);
                        count++; // 增加计数器
                    }
                }
                MessageBox.Show($"{count} 张图像已成功处理！");
            }
        }

        #endregion

        #region 打开颜色对话框
        private void BtnOpenColorDialog_Click(object sender, EventArgs e)
        {
            using var colorDialog = new ColorDialog();
            // 从应用程序设置中加载自定义颜色
            string customColorsSetting = Properties.Settings.Default.CustomColors;
            if (!string.IsNullOrEmpty(customColorsSetting))
            {
                int[] customColors = customColorsSetting.Split(',').Select(int.Parse).ToArray();
                colorDialog.CustomColors = customColors;
            }

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // 将自定义颜色保存到应用程序设置中
                customColorsSetting = string.Join(",", colorDialog.CustomColors);
                Properties.Settings.Default.CustomColors = customColorsSetting;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region 镜像图像
        private void BtMirrorImages_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string directoryPath = fbd.SelectedPath;
                string newDirectoryPath = Path.Combine(directoryPath, "mirror");

                // 如果新目录不存在则创建
                if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }

                int count = 0; // 已处理图像计数器

                foreach (var filePath in Directory.GetFiles(directoryPath))
                {
                    string extension = Path.GetExtension(filePath).ToLower();

                    if (extension == ".bmp" || extension == ".png" || extension == ".jpg" || extension == ".tiff")
                    {
                        using var img = (Bitmap)Image.FromFile(filePath);
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);

                        // 将镜像后的图像保存到新目录
                        string newFilePath = Path.Combine(newDirectoryPath, Path.GetFileName(filePath));
                        img.Save(newFilePath);

                        count++; // 增加计数器
                    }
                }

                MessageBox.Show($"{count} 张图像已成功镜像！");
            }
        }
        #endregion

        #region 将黑色/白色转换为透明
        private void BtConverterTransparent_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string directoryPath = fbd.SelectedPath;
                string newDirectoryPath = Path.Combine(directoryPath, "transparent");

                // 如果新目录不存在则创建
                if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }

                ConvertColorToTransparent(directoryPath, newDirectoryPath, Color.Black);
                ConvertColorToTransparent(directoryPath, newDirectoryPath, Color.White);
            }
        }
        #endregion

        #region 将指定颜色转换为透明
        private static void ConvertColorToTransparent(string directoryPath, string newDirectoryPath, Color fromColor)
        {
            int count = 0; // 已处理图像计数器

            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                string extension = Path.GetExtension(filePath).ToLower();

                if (extension == ".bmp" || extension == ".png" || extension == ".jpg" || extension == ".tiff")
                {
                    using var img = Image.FromFile(filePath);
                    Bitmap bitmap = new(img);

                    bitmap.MakeTransparent(fromColor);

                    // 将图像保存到新目录
                    string newFilePath = Path.Combine(newDirectoryPath, Path.GetFileName(filePath));
                    bitmap.Save(newFilePath);

                    count++; // 增加计数器
                }
            }

            MessageBox.Show($"{count} 张图像已成功处理！");
        }

        #endregion

        #region 旋转图像
        private void BtRotateImages_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string directoryPath = fbd.SelectedPath;
                RotateImages(directoryPath);
            }
        }
        #endregion

        #region 旋转图像方法
        private static void RotateImages(string directoryPath)
        {
            int count = 0; // 已处理图像计数器

            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                string extension = Path.GetExtension(filePath).ToLower();

                if (extension == ".bmp" || extension == ".png" || extension == ".jpg" || extension == ".tiff")
                {
                    using var img = Image.FromFile(filePath);
                    // 将图像向左旋转 90 度
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);

                    // 保存旋转后的图像
                    img.Save(filePath);

                    count++; // 增加计数器
                }
            }

            MessageBox.Show($"{count} 张图像已成功旋转！");
        }
        #endregion

        #region 转换图像格式
        private void BtConvert_Click(object sender, EventArgs e)
        {
            if (comboBoxFileType.SelectedItem == null)
            {
                MessageBox.Show("请从下拉列表中选择文件类型。");
                return;
            }

            string selectedFileType = comboBoxFileType.SelectedItem.ToString();

            using var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string directoryPath = fbd.SelectedPath;
                string newDirectoryPath = Path.Combine(directoryPath, selectedFileType);

                // 如果新目录不存在则创建
                if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }

                int count = 0; // 已处理图像计数器

                foreach (var filePath in Directory.GetFiles(directoryPath))
                {
                    string extension = Path.GetExtension(filePath).ToLower();

                    if (extension == ".bmp" || extension == ".png" || extension == ".jpg" || extension == ".tiff")
                    {
                        using var img = Image.FromFile(filePath);
                        // 将图像以所选格式保存到新目录
                        string newFilePath = Path.Combine(newDirectoryPath, Path.GetFileNameWithoutExtension(filePath) + $".{selectedFileType}");
                        img.Save(newFilePath, GetImageFormat(selectedFileType));

                        count++; // 增加计数器
                    }
                }

                MessageBox.Show($"{count} 张图像已成功转换为 .{selectedFileType} 格式！");
            }
        }
        #endregion

        #region 获取图像格式
        private static ImageFormat GetImageFormat(string fileType)
        {
            return fileType.ToLower() switch
            {
                "bmp" => ImageFormat.Bmp,
                "png" => ImageFormat.Png,
                "jpg" => ImageFormat.Jpeg,
                "tiff" => ImageFormat.Tiff,
                _ => ImageFormat.Png,
            };
        }
        #endregion
    }
}