/***************************************************************************
 *
 * $Author: Nikodemus
 * Coder: Nikodemus
 * 
 * "啤酒许可证"
 * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
 * 如果我们某天相遇，并且你认为这个东西有价值，
 * 你可以请我喝杯啤酒作为回报。
 *
 ***************************************************************************/

using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Linq;
using System.Drawing;
using System.Net.Http;

namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    public partial class AdminToolForm : Form
    {
        private static AdminToolForm _instance; // 静态变量，用于存储实例
        private AdminToolForm _adminToolForm;


        public AdminToolForm()
        {
            // 检查窗体是否已打开
            if (_instance != null && !_instance.IsDisposed)
            {
                // 已存在实例，则聚焦现有实例并关闭新实例
                _instance.Focus();
                Close();
                return;
            }

            // 未找到其他实例，则保存当前实例
            _instance = this;

            InitializeComponent();

            labelIP.Text = "";
            this.Load += AdminToolForm_Load;
        }

        #region AdminToolForm_Load
        private async void AdminToolForm_Load(object sender, EventArgs e)
        {
            await CheckInternetConnectionAsync();
        }
        #endregion

        #region 打开管理工具窗体
        public void ÖffneAdminToolForm()
        {
            // 检查 AdminToolForm 是否已释放或为空
            if (_adminToolForm == null || _adminToolForm.IsDisposed)
            {
                // 创建 AdminToolForm 的新实例
                _adminToolForm = new AdminToolForm();
                // 显示窗体
                _adminToolForm.Show();
            }
            else
            {
                // 聚焦已存在的窗体
                _adminToolForm.Focus();
            }
        }
        #endregion

        #region 管理工具窗体
        // 获取已打开实例的方法
        public static AdminToolForm GetInstance()
        {
            if (_instance == null || _instance.IsDisposed)
            {
                // 如果没有实例或实例已释放，则创建新实例
                _instance = new AdminToolForm();
            }

            return _instance;
        }
        #endregion

        #region btnPing
        private void BtnPing_Click(object sender, System.EventArgs e)
        {
            string address = textBoxAdress.Text;

            // 验证地址是否为有效的 IP 地址或域名
            if (!IsValidIPAddress(address) && !IsValidDomainName(address))
            {
                // 如果地址无效，显示消息并退出方法
                MessageBox.Show("输入的地址无效。请输入有效的 IP 地址或域名。");
                return;
            }

            // 创建新的 Ping 对象
            Ping pingSender = new();

            // 尝试向地址发送三个 ping
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    PingReply reply = pingSender.Send(address);

                    if (reply.Status == IPStatus.Success)
                    {
                        // 如果 ping 成功，显示回复信息
                        textBoxPingAusgabe.AppendText($"来自 {reply.Address} 的回复：时间={reply.RoundtripTime}ms\n");
                    }
                    else
                    {
                        // 如果 ping 失败，显示错误状态
                        textBoxPingAusgabe.AppendText($"错误：{reply.Status}\n");
                    }
                }
                catch (PingException ex)
                {
                    // 如果抛出 PingException，显示异常消息
                    textBoxPingAusgabe.AppendText($"PingException：{ex.Message}\n");
                }
            }

            // 在标签中显示地址
            labelIP.Text = address;
        }

        #endregion

        #region IsValidIPAddress
        // 检查指定字符串是否为有效的 IP 地址
        private static bool IsValidIPAddress(string address)
        {
            // 检查 IPv4 地址
            string patternIPv4 = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            if (Regex.IsMatch(address, patternIPv4))
            {
                return true;
            }

            // 检查 IPv6 地址
            string patternIPv6 = @"^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$";
            if (Regex.IsMatch(address, patternIPv6))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region IsValidDomainName
        // 检查指定字符串是否为有效域名
        private static bool IsValidDomainName(string address)
        {
            string pattern = @"^([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,}$";
            return Regex.IsMatch(address, pattern);
        }
        #endregion

        #region textBoxAdress_KeyDown
        private void TextBoxAdress_KeyDown(object sender, KeyEventArgs e)
        {
            // 检查是否按下了 Enter 键
            if (e.KeyCode == Keys.Enter)
            {
                // 开始 ping
                BtnPing_Click(this, EventArgs.Empty);
            }
        }
        #endregion

        #region btnTracert
        private async void BtnTracert_Click(object sender, EventArgs e)
        {
            string address = textBoxAdress.Text;
            // 验证地址是否为有效的 IP 地址或域名
            if (IsValidIPAddressTracert(address) || IsValidDomainNameTracert(address))
            {
                // 清空 textBoxPingOutput
                textBoxPingAusgabe.Clear();
                // 最大 TTL 值
                int maxHops = 30;
                // 当前 TTL 值
                int currentHop = 1;
                // 是否到达目标？
                bool targetReached = false;
                // 创建 ping 对象
                Ping pingSender = new();
                // 创建 ping 选项
                PingOptions pingOptions = new(currentHop, true);
                // 创建缓冲区
                byte[] buffer = new byte[32];
                // 设置超时
                int timeout = 5000;
                try
                {
                    // 为目标地址创建 IPHostEntry 对象
                    IPHostEntry hostEntry = Dns.GetHostEntry(address);
                    // 设置目标 IP 地址
                    IPAddress targetAddress = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetworkV6) ?? hostEntry.AddressList[0];
                    while (!targetReached && currentHop <= maxHops)
                    {
                        // 发送 ping
                        PingReply reply = await pingSender.SendPingAsync(targetAddress, timeout, buffer, pingOptions);
                        // 显示结果
                        if (reply.Status == IPStatus.Success)
                        {
                            textBoxPingAusgabe.AppendText(currentHop + "\t" + reply.Address.ToString() + "\r\n");
                            targetReached = true;
                        }
                        else if (reply.Status == IPStatus.TtlExpired)
                        {
                            textBoxPingAusgabe.AppendText(currentHop + "\t" + reply.Address.ToString() + "\r\n");
                        }
                        else
                        {
                            textBoxPingAusgabe.AppendText(currentHop + "\t*\r\n");
                        }
                        // 增加 TTL 值
                        currentHop++;
                        pingOptions.Ttl = currentHop;
                    }
                }
                catch (SocketException)
                {
                    // 如果发生 SocketException，显示消息
                    MessageBox.Show("输入的地址无效。请输入有效的 IP 地址或域名。");
                }
            }
            else
            {
                // 如果地址无效，显示消息
                MessageBox.Show("输入的地址无效。请输入有效的 IP 地址或域名。");
            }
        }
        #endregion

        #region IsValidIPAddressTracert
        // 检查指定字符串是否为有效的 IP 地址
        private static bool IsValidIPAddressTracert(string address)
        {
            return IPAddress.TryParse(address, out _);
        }
        #endregion

        #region IsValidDomainNameTracert
        // 检查指定字符串是否为有效域名
        private static bool IsValidDomainNameTracert(string address)
        {
            return Uri.CheckHostName(address) != UriHostNameType.Unknown;
        }
        #endregion

        #region btnClose
        // 关闭窗体的方法
        private void BtnClose_Click(object sender, EventArgs e)
        {
            // 将实例变量设置为 null，以便允许重新打开窗体
            _instance = null;
            Close();
        }
        #endregion

        #region BtnCopyIP
        private void BtnCopyIP_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(labelIP.Text);
        }
        #endregion

        #region CheckInternetConnectionAsync
        private async Task CheckInternetConnectionAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://google.com/generate_204");
                    if (response.IsSuccessStatusCode)
                    {
                        LabelInternetStatus.Text = "已连接互联网";
                        LabelInternetStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        LabelInternetStatus.Text = "未连接互联网";
                        LabelInternetStatus.ForeColor = Color.Red;
                    }
                }
            }
            catch
            {
                LabelInternetStatus.Text = "未连接互联网";
                LabelInternetStatus.ForeColor = Color.Red;
            }
        }
        #endregion
    }
}