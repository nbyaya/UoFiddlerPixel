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

namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    partial class AdminToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminToolForm));
            BtnPing = new System.Windows.Forms.Button();
            labelIP = new System.Windows.Forms.Label();
            textBoxAdress = new System.Windows.Forms.TextBox();
            textBoxPingAusgabe = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            BtnTracert = new System.Windows.Forms.Button();
            BtnCopyIP = new System.Windows.Forms.Button();
            LabelInternetStatus = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // BtnPing
            // 
            BtnPing.Location = new System.Drawing.Point(19, 17);
            BtnPing.Name = "BtnPing";
            BtnPing.Size = new System.Drawing.Size(75, 23);
            BtnPing.TabIndex = 0;
            BtnPing.Text = "Ping";
            BtnPing.UseVisualStyleBackColor = true;
            BtnPing.Click += BtnPing_Click;
            // 
            // labelIP
            // 
            labelIP.AutoSize = true;
            labelIP.Location = new System.Drawing.Point(188, 261);
            labelIP.Name = "labelIP";
            labelIP.Size = new System.Drawing.Size(17, 15);
            labelIP.TabIndex = 1;
            labelIP.Text = "IP";
            // 
            // textBoxAdress
            // 
            textBoxAdress.Location = new System.Drawing.Point(110, 17);
            textBoxAdress.Name = "textBoxAdress";
            textBoxAdress.Size = new System.Drawing.Size(100, 23);
            textBoxAdress.TabIndex = 2;
            textBoxAdress.KeyDown += TextBoxAdress_KeyDown;
            // 
            // textBoxPingAusgabe
            // 
            textBoxPingAusgabe.Location = new System.Drawing.Point(110, 52);
            textBoxPingAusgabe.Multiline = true;
            textBoxPingAusgabe.Name = "textBoxPingAusgabe";
            textBoxPingAusgabe.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxPingAusgabe.Size = new System.Drawing.Size(262, 200);
            textBoxPingAusgabe.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(110, 261);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 15);
            label2.TabIndex = 4;
            label2.Text = "最后地址：";
            // 
            // BtnTracert
            // 
            BtnTracert.Location = new System.Drawing.Point(19, 52);
            BtnTracert.Name = "BtnTracert";
            BtnTracert.Size = new System.Drawing.Size(75, 23);
            BtnTracert.TabIndex = 5;
            BtnTracert.Text = "Tracert";
            BtnTracert.UseVisualStyleBackColor = true;
            BtnTracert.Click += BtnTracert_Click;
            // 
            // BtnCopyIP
            // 
            BtnCopyIP.Location = new System.Drawing.Point(12, 257);
            BtnCopyIP.Name = "BtnCopyIP";
            BtnCopyIP.Size = new System.Drawing.Size(75, 23);
            BtnCopyIP.TabIndex = 6;
            BtnCopyIP.Text = "剪贴板";
            BtnCopyIP.UseVisualStyleBackColor = true;
            BtnCopyIP.Click += BtnCopyIP_Click;
            // 
            // LabelInternetStatus
            // 
            LabelInternetStatus.AutoSize = true;
            LabelInternetStatus.Location = new System.Drawing.Point(216, 21);
            LabelInternetStatus.Name = "LabelInternetStatus";
            LabelInternetStatus.Size = new System.Drawing.Size(54, 15);
            LabelInternetStatus.TabIndex = 7;
            LabelInternetStatus.Text = "互联网：";
            // 
            // AdminToolForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(384, 285);
            Controls.Add(LabelInternetStatus);
            Controls.Add(BtnCopyIP);
            Controls.Add(BtnTracert);
            Controls.Add(label2);
            Controls.Add(textBoxPingAusgabe);
            Controls.Add(textBoxAdress);
            Controls.Add(labelIP);
            Controls.Add(BtnPing);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "AdminToolForm";
            Text = "管理工具";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnPing;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox textBoxAdress;
        private System.Windows.Forms.TextBox textBoxPingAusgabe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnTracert;
        private System.Windows.Forms.Button BtnCopyIP;
        private System.Windows.Forms.Label LabelInternetStatus;
    }
}