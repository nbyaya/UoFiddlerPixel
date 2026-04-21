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

namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    partial class ConverterForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，则为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConverterForm));
            BtConverterWhite = new System.Windows.Forms.Button();
            BtConverterBlack = new System.Windows.Forms.Button();
            panelColor = new System.Windows.Forms.Panel();
            BtnOpenColorDialog = new System.Windows.Forms.Button();
            BtConverterCustom = new System.Windows.Forms.Button();
            lbBlackWhite = new System.Windows.Forms.Label();
            BtMirrorImages = new System.Windows.Forms.Button();
            panelFunctions = new System.Windows.Forms.Panel();
            BtConvert = new System.Windows.Forms.Button();
            comboBoxFileType = new System.Windows.Forms.ComboBox();
            BtRotateImages = new System.Windows.Forms.Button();
            BtConverterTransparent = new System.Windows.Forms.Button();
            lbFunction = new System.Windows.Forms.Label();
            panelColor.SuspendLayout();
            panelFunctions.SuspendLayout();
            SuspendLayout();
            // 
            // BtConverterWhite
            // 
            BtConverterWhite.Location = new System.Drawing.Point(14, 29);
            BtConverterWhite.Name = "BtConverterWhite";
            BtConverterWhite.Size = new System.Drawing.Size(54, 23);
            BtConverterWhite.TabIndex = 0;
            BtConverterWhite.Text = "白色";
            BtConverterWhite.UseVisualStyleBackColor = true;
            BtConverterWhite.Click += BtConverterWhite_Click;
            // 
            // BtConverterBlack
            // 
            BtConverterBlack.Location = new System.Drawing.Point(14, 58);
            BtConverterBlack.Name = "BtConverterBlack";
            BtConverterBlack.Size = new System.Drawing.Size(54, 23);
            BtConverterBlack.TabIndex = 1;
            BtConverterBlack.Text = "黑色";
            BtConverterBlack.UseVisualStyleBackColor = true;
            BtConverterBlack.Click += BtConverterBlack_Click;
            // 
            // panelColor
            // 
            panelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelColor.Controls.Add(BtnOpenColorDialog);
            panelColor.Controls.Add(BtConverterCustom);
            panelColor.Controls.Add(lbBlackWhite);
            panelColor.Controls.Add(BtConverterBlack);
            panelColor.Controls.Add(BtConverterWhite);
            panelColor.Location = new System.Drawing.Point(12, 12);
            panelColor.Name = "panelColor";
            panelColor.Size = new System.Drawing.Size(200, 100);
            panelColor.TabIndex = 2;
            // 
            // BtnOpenColorDialog
            // 
            BtnOpenColorDialog.Location = new System.Drawing.Point(89, 58);
            BtnOpenColorDialog.Name = "BtnOpenColorDialog";
            BtnOpenColorDialog.Size = new System.Drawing.Size(100, 23);
            BtnOpenColorDialog.TabIndex = 3;
            BtnOpenColorDialog.Text = "保存颜色";
            BtnOpenColorDialog.UseVisualStyleBackColor = true;
            BtnOpenColorDialog.Click += BtnOpenColorDialog_Click;
            // 
            // BtConverterCustom
            // 
            BtConverterCustom.Location = new System.Drawing.Point(89, 29);
            BtConverterCustom.Name = "BtConverterCustom";
            BtConverterCustom.Size = new System.Drawing.Size(100, 23);
            BtConverterCustom.TabIndex = 4;
            BtConverterCustom.Text = "自定义颜色";
            BtConverterCustom.UseVisualStyleBackColor = true;
            BtConverterCustom.Click += BtConverterCustom_Click;
            // 
            // lbBlackWhite
            // 
            lbBlackWhite.AutoSize = true;
            lbBlackWhite.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lbBlackWhite.Location = new System.Drawing.Point(14, 8);
            lbBlackWhite.Name = "lbBlackWhite";
            lbBlackWhite.Size = new System.Drawing.Size(181, 15);
            lbBlackWhite.TabIndex = 3;
            lbBlackWhite.Text = "转换图像为黑色或白色：";
            // 
            // BtMirrorImages
            // 
            BtMirrorImages.Location = new System.Drawing.Point(13, 30);
            BtMirrorImages.Name = "BtMirrorImages";
            BtMirrorImages.Size = new System.Drawing.Size(51, 23);
            BtMirrorImages.TabIndex = 3;
            BtMirrorImages.Text = "镜像";
            BtMirrorImages.UseVisualStyleBackColor = true;
            BtMirrorImages.Click += BtMirrorImages_Click;
            // 
            // panelFunctions
            // 
            panelFunctions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelFunctions.Controls.Add(BtConvert);
            panelFunctions.Controls.Add(comboBoxFileType);
            panelFunctions.Controls.Add(BtRotateImages);
            panelFunctions.Controls.Add(BtConverterTransparent);
            panelFunctions.Controls.Add(lbFunction);
            panelFunctions.Controls.Add(BtMirrorImages);
            panelFunctions.Location = new System.Drawing.Point(218, 12);
            panelFunctions.Name = "panelFunctions";
            panelFunctions.Size = new System.Drawing.Size(200, 100);
            panelFunctions.TabIndex = 4;
            // 
            // BtConvert
            // 
            BtConvert.Location = new System.Drawing.Point(126, 59);
            BtConvert.Name = "BtConvert";
            BtConvert.Size = new System.Drawing.Size(53, 22);
            BtConvert.TabIndex = 8;
            BtConvert.Text = "文件类型";
            BtConvert.UseVisualStyleBackColor = true;
            BtConvert.Click += BtConvert_Click;
            // 
            // comboBoxFileType
            // 
            comboBoxFileType.FormattingEnabled = true;
            comboBoxFileType.Items.AddRange(new object[] { "bmp", "png", "jpg", "tiff" });
            comboBoxFileType.Location = new System.Drawing.Point(70, 59);
            comboBoxFileType.Name = "comboBoxFileType";
            comboBoxFileType.Size = new System.Drawing.Size(55, 23);
            comboBoxFileType.TabIndex = 7;
            // 
            // BtRotateImages
            // 
            BtRotateImages.Location = new System.Drawing.Point(13, 59);
            BtRotateImages.Name = "BtRotateImages";
            BtRotateImages.Size = new System.Drawing.Size(51, 23);
            BtRotateImages.TabIndex = 6;
            BtRotateImages.Text = "旋转";
            BtRotateImages.UseVisualStyleBackColor = true;
            BtRotateImages.Click += BtRotateImages_Click;
            // 
            // BtConverterTransparent
            // 
            BtConverterTransparent.Location = new System.Drawing.Point(70, 30);
            BtConverterTransparent.Name = "BtConverterTransparent";
            BtConverterTransparent.Size = new System.Drawing.Size(79, 23);
            BtConverterTransparent.TabIndex = 5;
            BtConverterTransparent.Text = "透明";
            BtConverterTransparent.UseVisualStyleBackColor = true;
            BtConverterTransparent.Click += BtConverterTransparent_Click;
            // 
            // lbFunction
            // 
            lbFunction.AutoSize = true;
            lbFunction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lbFunction.Location = new System.Drawing.Point(10, 9);
            lbFunction.Name = "lbFunction";
            lbFunction.Size = new System.Drawing.Size(66, 15);
            lbFunction.TabIndex = 4;
            lbFunction.Text = "功能：";
            // 
            // ConverterForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(425, 116);
            Controls.Add(panelFunctions);
            Controls.Add(panelColor);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "ConverterForm";
            Text = "图像转换器";
            panelColor.ResumeLayout(false);
            panelColor.PerformLayout();
            panelFunctions.ResumeLayout(false);
            panelFunctions.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button BtConverterWhite;
        private System.Windows.Forms.Button BtConverterBlack;
        private System.Windows.Forms.Panel panelColor;
        private System.Windows.Forms.Label lbBlackWhite;
        private System.Windows.Forms.Button BtConverterCustom;
        private System.Windows.Forms.Button BtnOpenColorDialog;
        private System.Windows.Forms.Button BtMirrorImages;
        private System.Windows.Forms.Panel panelFunctions;
        private System.Windows.Forms.Label lbFunction;
        private System.Windows.Forms.Button BtConverterTransparent;
        private System.Windows.Forms.Button BtRotateImages;
        private System.Windows.Forms.ComboBox comboBoxFileType;
        private System.Windows.Forms.Button BtConvert;
    }
}