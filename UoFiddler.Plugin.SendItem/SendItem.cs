/***************************************************************************
 *
 * @Author: Turley
 * 
 * "啤酒软件许可协议"
 * 只要你保留此声明，你可以随意使用此代码。
 * 如果我们某天相遇，你觉得此代码值得，
 * 可以请我喝一杯啤酒作为回报。
 *
 ***************************************************************************/

using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Plugin;
using UoFiddler.Controls.Plugin.Interfaces;
using UoFiddler.Controls.UserControls;
using UoFiddler.Controls.UserControls.TileView;
using UoFiddler.Plugin.SendItem.Forms;
using Events = UoFiddler.Controls.Plugin.PluginEvents;

namespace UoFiddler.Plugin.SendItem
{
    public class SendItemPluginBase : PluginBase
    {
        public SendItemPluginBase()
        {
            _refMarker = this;
            Events.ModifyItemsControlContextMenuEvent += EventsModifyItemsControlContextMenuEvent;
        }

        private static SendItemPluginBase _refMarker;
        private static bool _overrideClick;

        /// <summary>
        /// 发送命令
        /// </summary>
        public static string Cmd { get; set; } = ".create";

        /// <summary>
        /// 命令参数格式
        /// </summary>
        public static string CmdArg { get; set; } = "0x{1:X4}";

        /// <summary>
        /// 覆盖双击事件
        /// </summary>
        public static bool OverrideClick
        {
            get => _overrideClick;
            set
            {
                if (value != _overrideClick)
                    _refMarker.ChangeOverrideClick(value, false);
                _overrideClick = value;
            }
        }

        /// <summary>
        /// 插件名称
        /// </summary>
        public override string Name { get; } = "发送物品插件";

        /// <summary>
        /// 插件功能描述
        /// </summary>
        public override string Description { get; } = "在物品标签页中，将自定义命令发送到客户端以生成选中的物品";

        /// <summary>
        /// 插件作者
        /// </summary>
        public override string Author { get; } = "Turley";

        /// <summary>
        /// 插件版本
        /// </summary>
        public override string Version { get; } = "1.0.1";

        /// <summary>
        /// 插件宿主
        /// </summary>
        public override IPluginHost Host { get; set; } = null;

        /// <summary>
        /// 初始化插件
        /// </summary>
        public override void Initialize()
        {
            _ = Files.RootDir;

            LoadXml();
            ChangeOverrideClick(OverrideClick, true);
        }

        private void PlugOnDoubleClick(object sender, MouseEventArgs e)
        {
            ItemShowContextClicked(this, EventArgs.Empty);
        }

        /// <summary>
        /// 卸载插件
        /// </summary>
        public override void Unload()
        {
            SaveXml();
        }

        /// <summary>
        /// 修改标签页（本插件无）
        /// </summary>
        public override void ModifyTabPages(TabControl tabControl)
        {
        }

        /// <summary>
        /// 修改插件工具条菜单
        /// </summary>
        public override void ModifyPluginToolStrip(ToolStripDropDownButton toolStrip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = "发送物品"
            };
            item.Click += ToolStripClick;
            toolStrip.DropDownItems.Add(item);
        }

        /// <summary>
        /// 设置是否覆盖双击行为
        /// </summary>
        private void ChangeOverrideClick(bool value, bool init)
        {
            ItemsControl itemsControl = Host.GetItemsControl();
            TileViewControl itemsControlTileView = Host.GetItemsControlTileView();
            if (value)
            {
                itemsControlTileView.MouseDoubleClick -= itemsControl.ItemsTileView_MouseDoubleClick;

                // 确保只添加一次事件
                itemsControlTileView.MouseDoubleClick -= PlugOnDoubleClick;
                itemsControlTileView.MouseDoubleClick += PlugOnDoubleClick;
            }
            else if (!init)
            {
                itemsControlTileView.MouseDoubleClick -= PlugOnDoubleClick;

                // 确保只添加一次事件
                itemsControlTileView.MouseDoubleClick -= itemsControl.ItemsTileView_MouseDoubleClick;
                itemsControlTileView.MouseDoubleClick += itemsControl.ItemsTileView_MouseDoubleClick;
            }
        }

        /// <summary>
        /// 打开插件设置窗口
        /// </summary>
        private static void ToolStripClick(object sender, EventArgs e)
        {
            new SendItemOptionsForm().Show();
        }

        /// <summary>
        /// 给物品控件添加右键菜单
        /// </summary>
        private void EventsModifyItemsControlContextMenuEvent(ContextMenuStrip strip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem { Text = "发送物品到客户端" };
            item.Click += ItemShowContextClicked;
            strip.Items.Add(item);
        }

        /// <summary>
        /// 执行发送物品命令
        /// </summary>
        private void ItemShowContextClicked(object sender, EventArgs e)
        {
            int currSelected = Host.GetSelectedIdFromItemsControl();
            if (currSelected <= -1)
            {
                return;
            }

            if (Client.Running)
            {
                string format = "{0} " + CmdArg;
                Client.SendText(string.Format(format, Cmd, currSelected));
            }
            else
            {
                MessageBox.Show(
                    "未运行客户端或无法识别",
                    "发送物品",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
        }

        /// <summary>
        /// 从XML加载配置
        /// </summary>
        private static void LoadXml()
        {
            string path = Options.AppDataPath;
            string fileName = Path.Combine(path, "plugins/SendItem.xml");
            if (!File.Exists(fileName))
            {
                return;
            }

            XmlDocument dom = new XmlDocument();
            dom.Load(fileName);

            XmlElement xOptions = dom["Options"];

            XmlElement elem = (XmlElement)xOptions?.SelectSingleNode("SendItem");
            if (elem == null)
            {
                return;
            }

            Cmd = elem.GetAttribute("cmd");
            CmdArg = elem.GetAttribute("args");
            OverrideClick = bool.Parse(elem.GetAttribute("overrideclick"));
        }

        /// <summary>
        /// 保存配置到XML
        /// </summary>
        private static void SaveXml()
        {
            string path = Options.AppDataPath;
            string fileName = Path.Combine(path, "plugins/senditem.xml");

            XmlDocument dom = new XmlDocument();

            XmlDeclaration decl = dom.CreateXmlDeclaration("1.0", "utf-8", null);
            dom.AppendChild(decl);

            XmlElement sr = dom.CreateElement("Options");

            XmlComment comment = dom.CreateComment("定义生成物品的命令");
            sr.AppendChild(comment);

            comment = dom.CreateComment("{1} = 物品ID");
            sr.AppendChild(comment);

            XmlElement elem = dom.CreateElement("SendItem");
            elem.SetAttribute("cmd", Cmd);
            elem.SetAttribute("args", CmdArg);
            elem.SetAttribute("overrideclick", OverrideClick.ToString());
            sr.AppendChild(elem);

            dom.AppendChild(sr);
            dom.Save(fileName);
        }
    }
}